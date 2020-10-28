using Discord;
using Discord.WebSocket;
using DoomBot.Server.Modules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

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

    private string Path => "EconConf.json";

    private DiscordAccessor DA;

    private InventoryModule IM;

    private RolePerkModule RPM;

    private List<(Inventory Inv, SocketRole Role)> RoleCompiler;

    private HashSet<(ulong UserID, DateTime Completion)> Cooldown;

    private Random Rand;

    private int TicketCountdown;

    private bool TokenDropping;

    public EconManager(DiscordAccessor DA, InventoryModule IM, RolePerkModule RPM)
    {
        this.DA = DA;

        this.IM = IM;

        this.RPM = RPM;

        RoleCompiler = new List<(Inventory Inv, SocketRole Role)>();

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

        _ = Task.Run(async () =>
        {
            _ = Loop();
        });
    }

    public void UpdateConf()
    {
        File.WriteAllText(Path, JsonConvert.SerializeObject(EconConf));
    }

    private async Task Loop()
    {
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

        foreach (var OU in OnlineUsers)
        {
            if (OU.Status == UserStatus.Offline)
            {
                continue;
            }

            var Inv = await IM.TryGetInvUnwrapped((long)OU.Id);

            if (Inv == default)
            {
                continue;
            }

            var RP = await RPM.TryGetRoleUnwrapped(OU);

            if (RP == default)
            {
                continue;
            }

            var Role = RPM.TryGetSocketRole(OU, RP.RoleID);

            if (Role == default || !Role.IsHoisted)
            {
                continue;
            }

            RoleCompiler.Add((Inv, Role));
        }

        var Sorted = RoleCompiler.OrderBy(x => x.Inv.Boosts).ThenBy(x => x.Inv.Cash);

        int Pos = PHRole.Position;

        foreach (var S in Sorted)
        {
            Pos++;

            await S.Role.ModifyAsync(x => x.Position = Pos);
        }
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

    private async Task CountdownTicket(SocketUserMessage Msg, DoomModuleBase<Inventory>.MongoDataWrapper Inv)
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

    public void RemoveSpawnChannel(ulong ID)
    {
        EconConf.SpawnChannels.Remove(ID);

        UpdateConf();
    }

    public void SetPlaceHolderRole(SocketRole Role = default)
    {
        EconConf.PlaceholderRole = (Role != default) ? Role.Id : default;

        UpdateConf();
    }
}