using Discord;
using Discord.WebSocket;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

public class RQComparer : IEqualityComparer<(ulong ChannelID, Queue<(IUserMessage Msg, string Reaction)> Queue)>
{
    public bool Equals((ulong ChannelID, Queue<(IUserMessage Msg, string Reaction)> Queue) X, (ulong ChannelID, Queue<(IUserMessage Msg, string Reaction)> Queue) Y)
    {
        return X.ChannelID == Y.ChannelID;
    }

    public int GetHashCode((ulong ChannelID, Queue<(IUserMessage Msg, string Reaction)> Queue) Obj)
    {
        return Obj.ChannelID.GetHashCode();
    }
}

public class ReactionQueue
{
    private static object Lock = new object();

    private static int I;

    public static ReactionQueue Instance
    {
        get
        {
            if (_Instance == default)
            {
                lock (Lock)
                {
                    if (_Instance == default)
                    {
                        _Instance = new ReactionQueue();
                    }
                }
            }

            return _Instance;
        }
    }

    private static ReactionQueue _Instance;

    private HashSet<(ulong ChannelID, Queue<(IUserMessage Msg, string Reaction)> Queue)> RQ;

    private RequestOptions BypassRL;

    public ReactionQueue()
    {
        RQ = new HashSet<(ulong ChannelID, Queue<(IUserMessage Msg, string Reaction)> Queue)>(new RQComparer());

        BypassRL = new RequestOptions { BypassBuckets = true };
    }

    public void Queue(IUserMessage Msg, string React)
    {
        var MsgChID = Msg.Channel.Id;

        lock (Lock)
        {
            if (!RQ.TryGetValue((MsgChID, null), out var Data))
            {
                Console.WriteLine($"Queue init - {I}");

                Data = (MsgChID, new Queue<(IUserMessage Msg, string Reaction)>());

                RQ.Add(Data);
            }

            var TheQueue = Data.Queue;

            TheQueue.Enqueue((Msg, React));

            if (TheQueue.Count == 1)
            {
                Task.Run(async () => await ProcQueue(TheQueue));
            }
        }
    }

    private async Task ProcQueue(Queue<(IUserMessage Msg, string Reaction)> Queue)
    {
        while (Queue.Count != 0)
        {
            var ReactMsg = Queue.Peek();

            var Msg = ReactMsg.Msg;

            if (Msg != default)
            {
                var DT = DateTime.UtcNow;

                await Msg.AddReactionAsync(new Emoji(ReactMsg.Reaction), BypassRL);
                
                Console.WriteLine((DateTime.UtcNow - DT).TotalMilliseconds);
            }

            Queue.Dequeue();
        }
    }
}