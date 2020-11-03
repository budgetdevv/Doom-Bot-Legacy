using MongoDB.Bson.Serialization.Attributes;

namespace DoomBot.Server.Modules
{
    public class PerkRole
    {
        [BsonId]
        public long UserID { get; set; }

        public ulong RoleID { get; set; }

        public PerkRole(long UserID, ulong RoleID)
        {
            this.UserID = UserID;

            this.RoleID = RoleID;
        }
    }
}