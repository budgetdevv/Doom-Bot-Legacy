using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DoomBot.Server.Modules;
using Newtonsoft.Json;

namespace DoomBot.Server.Managers
{
    public class CooldownComparer : IEqualityComparer<(ulong UserID, DateTime Completion)>
    {
        public bool Equals((ulong UserID, DateTime Completion) X, (ulong UserID, DateTime Completion) Y)
        {
            return (X.UserID == Y.UserID);
        }

        public int GetHashCode((ulong UserID, DateTime Completion) Obj)
        {
            return Obj.UserID.GetHashCode();
        }
    }

    public class EconManager
    {
        private struct EconConfig
        {
            public ulong PlaceholderRole;

            public HashSet<ulong> SpawnChannels;
        }

        private EconConfig EconConf;

        private static string Path => "EconConf.json";

        private readonly DiscordAccessor DA;

        private readonly InventoryModule IM;

        private readonly RolePerkModule RPM;

        private List<ReorderRoleProperties> RoleCompiler;

        private readonly HashSet<(ulong UserID, DateTime Completion)> Cooldown;

        private Random Rand;

        private int TicketCountdown;

        private bool TokenDropping;

        public EconManager(DiscordAccessor DA, InventoryModule IM, RolePerkModule RPM)
        {
            this.DA = DA;

            this.IM = IM;

            this.RPM = RPM;

            RoleCompiler = new List<ReorderRoleProperties>();

            Cooldown = new HashSet<(ulong UserID, DateTime Completion)>(new CooldownComparer());

            Rand = new Random();

            TicketCountdown = 3;

            if (!File.Exists(Path))
            {
                EconConf = new EconConfig();

                EconConf.SpawnChannels = new HashSet<ulong>();

                UpdateConf();
            }

            else
            {
                EconConf = JsonConvert.DeserializeObject<EconConfig>(File.ReadAllText(Path));
            }

            Task.Run(Loop);
        }

        private void UpdateConf()
        {
            File.WriteAllText(Path, JsonConvert.SerializeObject(EconConf));
        }

        private async Task Loop()
        {
            //Give time for bot to startup

            await Task.Delay(1000);

            while (true)
            {
                await SortRole();

                await Task.Delay(60000); //1 Min
            }
        }

        private async Task SortRole()
        {
            var Guild = DA.CurrentGuild;

            var PHRole = Guild?.GetRole(EconConf.PlaceholderRole);

            if (PHRole == default)
            {
                return;
            }

            RoleCompiler.Clear();

            var OnlineUsers = Guild.Users;

            int Pos = PHRole.Position;

            async IAsyncEnumerable<(Inventory Inv, PerkRole Role)> OnlineInvRole()
            {
                foreach (var OU in OnlineUsers)
                {
                    if (OU.Status == UserStatus.Offline)
                    {
                        continue;
                    }

                    var Inv = (await IM.TryGetInv((long) OU.Id))?.Data;

                    if (Inv == default)
                    {
                        continue;
                    }

                    var RP = await RPM.TryGetRole(OU);

                    if (RP == default)
                    {
                        continue;
                    }

                    yield return (Inv, RP.Data);
                }
            }

            async IAsyncEnumerable<ReorderRoleProperties> RoleOrderData()
            {
                yield return new ReorderRoleProperties(PHRole.Id, Pos);

                //Inv is guaranteed to NOT be null
                
                // ReSharper disable PossibleNullReferenceException
                await foreach (var X in OnlineInvRole().OrderByDescending(x => x.Inv.Boosts).ThenByDescending(x => x.Inv.Cash)) // ReSharper restore PossibleNullReferenceException
                {
                    Pos--;

                    yield return new ReorderRoleProperties(X.Role.RoleID, Pos);
                }
            }

            await Guild.ReorderRolesAsync(RoleOrderData().ToEnumerable());

            Console.WriteLine("Role Re-order complete!");
        }

        public async Task OnText(SocketUserMessage Msg)
        {
            ulong UserID = Msg.Author.Id;

            if (Cooldown.TryGetValue((Msg.Author.Id, default), out var Data))
            {
                if (Data.Completion > DateTime.UtcNow)
                {
                    return;
                }
            }

            using var Inv = await IM.GetOrCreateInv((long)UserID);

            await CountdownTicket(Msg, Inv);

            Inv.Data.Cash += Rand.Next(0, 11);

            var NewTime = DateTime.UtcNow + TimeSpan.FromSeconds(Rand.Next(5, 16));

            if (Data != default)
            {
                Data.Completion = NewTime;
            }

            else
            {
                Cooldown.Add((UserID, NewTime));
            }
        }

        private async Task CountdownTicket(SocketUserMessage Msg, ModuleDataWrapper<Inventory> Inv)
        {
            if (TokenDropping || !EconConf.SpawnChannels.Contains(Msg.Channel.Id))
            {
                return;
            }

            TicketCountdown--;

            if (TicketCountdown == 0)
            {
                TicketCountdown = Rand.Next(10, 20);

                var Ticket = await Msg.Channel.SendMessageAsync(":ticket: | A ticket has dropped! Hurry, grab it! ( By reacting )");

                _ = Ticket.AddReactionAsync(new Emoji("🎫"));

                TokenDropping = true;
            
                async Task OnTokenGrab(Cacheable<IUserMessage, ulong> Cache, ISocketMessageChannel Channel, SocketReaction React)
                {
                    var RU = await Channel.GetUserAsync(React.UserId);

                    if (!TokenDropping || Ticket.Id != React.MessageId || RU.IsBot)
                    {
                        return;
                    }

                    TokenDropping = false;

                    Inv.Data.TryAddItem("Ticket", "Ticket", "Just a Ticket", 1);

                    _ = Channel.SendMessageAsync($":ticket: | {RU.Mention} has claimed the Ticket!");
                }

                DA.Client.ReactionAdded += OnTokenGrab;

                _ = Task.Run(async () =>
                {
                    await Task.Delay(5000);

                    DA.Client.ReactionAdded -= OnTokenGrab;

                    TokenDropping = false;
                });
            }
        }

        public void AddSpawnChannel(SocketTextChannel Channel)
        {
            EconConf.SpawnChannels.Add(Channel.Id);

            UpdateConf();
        }

        public void RemoveSpawnChannel(SocketTextChannel Channel)
        {
            EconConf.SpawnChannels.Remove(Channel.Id);

            UpdateConf();
        }

        public void SetPlaceHolderRole(SocketRole Role = default)
        {
            EconConf.PlaceholderRole = Role?.Id ?? default;

            UpdateConf();
        }
    }
}