using Discord;
using MongoDB.Bson.Serialization.Attributes;
using Snowflake;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DoomBot.Server.Modules
{
    public class Inventory
    {
        //This is a convenient way of ensuring that there aren't Items with same ID AND Name

        private static InvItemComparer Comparer = new InvItemComparer();

        [BsonId]
        public long ID { get; set; }

        public int Cash { get; set; }

        public int Boosts { get; set; }

        public HashSet<(string ID, string Name, string Desc, int Quantity)> Items { get; set; }

        public Inventory(long _ID)
        {
            ID = _ID;

            Cash = 0;

            Boosts = 0;

            Items = new HashSet<(string ID, string Name, string Desc, int Quantity)>(Comparer);
        }

        public bool TryAddItem(string ID, string ItemName, string Desc, int Count = -1)
        {
            if (Items.TryGetValue((ID, ItemName, null, default), out var Data))
            {
                if (Count == -1)
                {
                    return false;
                }

                Data.Quantity += Count;

                return true;
            }

            Items.Add((ID, ItemName, Desc, Count));

            return true;
        }
    }

    public class InvItemComparer : IEqualityComparer<(string ID, string Name, string Desc, int Quantity)>
    {
        public bool Equals((string ID, string Name, string Desc, int Quantity) X, (string ID, string Name, string Desc, int Quantity) Y)
        {
            if (X.ID == Y.ID && X.Name == Y.Name)
            {
                return true;
            }

            return false;
        }

        public int GetHashCode((string ID, string Name, string Desc, int Quantity) Obj)
        {
            return Obj.ID.GetHashCode();
        }
    }
}