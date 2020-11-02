using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DoomBot.Server.Modules;

namespace DoomBot.Server.EM
{
    public class EMManager
    {
        //Static
        
        private static readonly EmbedBuilder EM;
        
        private static Embed EMCompile(EmbedMenu Instance, IEnumerable<(string Emoji, string Name, string Desc, Func<EmbedMenu, Task<bool>> Act)> CurrentPageElements)
        {
            EM.Title = Instance.Title;

            EM.Description = Instance.Desc;

            EM.Fields.Clear();

            foreach (var Data in CurrentPageElements)
            {
                if (Data.Emoji == default)
                {
                    EM.AddField($"{Data.Name}", Data.Desc);   
                }

                else
                {
                    EM.AddField($"{Data.Emoji} | {Data.Name}", Data.Desc);
                }
            }

            return EM.Build();
        }

        static EMManager()
        {
            EM = new EmbedBuilder();
        }
        
        private readonly Queue<EmbedMenuInternal> Pool;

        private readonly HashSet<ulong> AccessingUsers;

        private readonly DiscordAccessor DA;

        public EMManager(DiscordAccessor DA)
        {
            Pool = new Queue<EmbedMenuInternal>();
            
            AccessingUsers = new HashSet<ulong>();

            this.DA = DA;

            EmbedMenuInternal.EMM = this;
        }
        
        //Functions
        
        public bool Gen(SocketUser User, IMessageChannel MC, Func<EmbedMenu, Task<bool>> Act)
        {
            if (AccessingUsers.Contains(User.Id))
            {
                MC.SendMessageAsync("You have an Active menu! Please close it before launching another menu :>");
                
                return false;
            }

            AccessingUsers.Add(User.Id);

            if (Pool.TryDequeue(out var x))
            {
                _ = x.CompileNew(User, MC, Act);
            }

            else
            {
                x = new EmbedMenuInternal();
                
                _ = x.CompileNew(User, MC, Act);
            }

            return true;
        }

        private void Recycle(EmbedMenu EM)
        {
            Pool.Enqueue((EmbedMenuInternal)EM);
        }

        //Sealed Class

        private sealed class EmbedMenuInternal: EmbedMenu
        {
            //Static

            public static EMManager EMM;

            //Variables

            private EmbedMenuInternal Prev;

            private IUserMessage Msg;

            private SocketUser User;

            private Func<EmbedMenu, Task<bool>> Act;

            private bool OverrideAutoCancel;

            private readonly SemaphoreSlim Lock;

            private readonly HashSet<string> SButtons;

            public EmbedMenuInternal()
            {
                Lock = new SemaphoreSlim(1, 1);
                
                SButtons = new HashSet<string>();
            }
            
            //Functions
            
            private void Construct(Func<EmbedMenu, Task<bool>> Act, bool FromNext = false)
            {
                this.Act = Act;
                
                ElementsMetaData.Clear();

                if (!FromNext)
                {
                    PageSize = 5;

                    Page = 1;
                    
                    ExpireIn = 10;
                }
            }

            public async Task CompileNew(SocketUser User, IMessageChannel Channel, Func<EmbedMenu, Task<bool>> Act)
            {
                Construct(Act);

                if (!await Act.Invoke(this))
                {
                    EMM.Recycle(this);

                    return;
                }

                Prev = null;

                this.User = User;

                Msg = await Channel.SendMessageAsync(null, false, EMCompile(this, GetCurrentPageElements()));
                
                AddReactions();

                AddSpecialButtons();
                
                //Add reaction handlers
            
                EMM.DA.Client.ReactionAdded += OnReact;

                _ = AutoCancel();
            }
            
            private async Task CompileFromPrev(EmbedMenuInternal Prev, Func<EmbedMenu, Task<bool>> Act)
            {
                Construct(Act);

                if (!await Act.Invoke(this))
                {
                    _ = Prev.Compile(Prev);
                    
                    EMM.Recycle(this);

                    return;
                }

                this.Prev = Prev;

                _ = Compile(Prev);
            }
            
            private async Task CompileFromNext(EmbedMenuInternal Next) //From Next EM
            {
                Construct(Act, true);

                if (!await Act.Invoke(this))
                {
                    if (Prev != default)
                    {
                        _ = Prev.CompileFromNext(this);
                    }

                    else //If this is the First EM of a User
                    {
                        ChainCancel();
                    }
                    
                    return;
                }
                
                Page = (PageIsSync()) ? Page : 1;

                _ = Compile(Next);

                EMM.Recycle(Next);
            }

            private async Task Compile(EmbedMenuInternal RefEM)
            {
                this.User = RefEM.User;

                var Channel = RefEM.Msg.Channel;
                
                if (Channel is SocketDMChannel)
                {
                    Msg = await Channel.SendMessageAsync(null, false, EMCompile(this, GetCurrentPageElements()));

                    _ = RefEM.Msg.DeleteAsync();
                }

                else
                {
                    Msg = RefEM.Msg;

                    await Msg.RemoveAllReactionsAsync();

                    _ = Msg.ModifyAsync(x => x.Embed = EMCompile(this, GetCurrentPageElements()));
                }

                AddReactions();

                AddSpecialButtons();

                //Add reaction handlers
            
                EMM.DA.Client.ReactionAdded += OnReact;

                _ = AutoCancel();
            }

            private void AddReactions()
            {
                SButtons.Clear();
                
                foreach (var Data in GetCurrentPageElements())
                {
                    string Emoji = Data.Emoji;
                    
                    if (Emoji == default || SButtons.Contains(Emoji))
                    {
                        continue;
                    }

                    SButtons.Add(Emoji);
                    
                    ReactionQueue.Instance.Queue(Msg, Emoji);
                }
            }

            private async Task OnReact(Cacheable<IUserMessage, ulong> Cache, ISocketMessageChannel Channel, SocketReaction React)
            {
                if (React.UserId != User.Id || React.MessageId != Msg.Id)
                {
                    return;
                }

                await Lock.WaitAsync();

                try
                {
                    string RName = React.Emote.Name;

                    if (!SpecialButtons(RName))
                    {
                        var Data = GetCurrentPageElements().FirstOrDefault(x => x.Emoji == RName);

                        if (Data == default || Data.Act == null)
                        {
                            return;
                        }

                        _ = new EmbedMenuInternal().CompileFromPrev(this, Data.Act);
                    }

                    OverrideAutoCancel = true;

                    EMM.DA.Client.ReactionAdded -= OnReact;
                }

                finally
                {
                    Lock.Release();
                }
            }

            private bool SpecialButtons(string Emoji)
            {
                if (!SButtons.Contains(Emoji))
                {
                    return false;
                }
                
                switch (Emoji)
                {
                    case "❎":
                    {
                        _ = Msg.Channel.SendMessageAsync(":white_check_mark: | Menu have been closed!");
                        
                        _ = Msg.DeleteAsync();
                        
                        ChainCancel();
                        
                        break;
                    }
                    
                    case "🔙":
                    {
                        _ = Prev.CompileFromNext(this);

                        break;
                    }
                    
                    case "◀️":
                    {
                        Page--;
                        
                        _ = CompileFromNext(this);

                        break;
                    }
                    
                    case "▶️":
                    {
                        Page++;
                        
                        _ = CompileFromNext(this);
                        
                        break; 
                    }
                }

                return true;
            }

            private IEnumerable<(string Emoji, string Name, string Desc, Func<EmbedMenu, Task<bool>> Act)> GetCurrentPageElements()
            {
                int StartIndex = (Page - 1) * PageSize;

                int RemainingElements = ElementsMetaData.Count - 1 - StartIndex;

                RemainingElements = (RemainingElements < PageSize) ? RemainingElements : PageSize;

                for (int I = StartIndex; I <= StartIndex + RemainingElements; I++)
                {
                    yield return ElementsMetaData[I];
                }
            }
            

            private bool PageIsSync(int ThePage = default)
            {
                if (ThePage == default)
                {
                    ThePage = Page;
                }
                
                return ThePage * PageSize <= ElementsMetaData.Count;
            }

            private async Task AutoCancel()
            {
                for (int I = 1; I <= ExpireIn; I++)
                {
                    await Task.Delay(1000);

                    if (OverrideAutoCancel)
                    {
                        Console.WriteLine("Auto-Cancel overriden");
                        
                        OverrideAutoCancel = false;
                        
                        return;
                    }
                }

                EMM.DA.Client.ReactionAdded -= OnReact;

                _ = Msg.Channel.SendMessageAsync("Menu have closed due to inactivity!");

                _ = Msg.DeleteAsync();
                    
                ChainCancel();
            }
            
            private void ChainCancel()
            {
                Task.Run(_ChainCancel);
            }

            private void _ChainCancel()
            {
                EMM.Recycle(this);

                Prev?._ChainCancel();

                EMM.AccessingUsers.Remove(User.Id);
            }

            private void AddSpecialButtons()
            {
                SButtons.Clear();
                
                string Emoji;

                //Back
                
                if (Prev != default)
                {
                    Emoji = "🔙";
                    
                    SButtons.Add(Emoji);
                    
                    ReactionQueue.Instance.Queue(Msg, Emoji);
                }
                
                //Prev
                
                if (Page != 1)
                {
                    Emoji = "◀️";
                    
                    SButtons.Add(Emoji);
                    
                    ReactionQueue.Instance.Queue(Msg, Emoji);
                }
                
                //Next
                        
                if (PageIsSync(Page + 1))
                {
                    Emoji = "▶️";
                    
                    SButtons.Add(Emoji);
                    
                    ReactionQueue.Instance.Queue(Msg, Emoji);
                }
                
                Emoji = "❎";
                    
                SButtons.Add(Emoji);
                    
                ReactionQueue.Instance.Queue(Msg, Emoji);
            }
        }
    }
}
