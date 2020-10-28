using Recycler;
using System;
using System.Drawing;
using System.Text;
using System.Text.Json.Serialization;

namespace DoomBot.Shared.Perks.Role
{
    public class RoleData: IDisposable
    {
        public string Name { get; set; }

        public bool Enabled { get; set; }

        public bool Hoisted { get; set; }

        public uint Color { get; set; }

        [JsonIgnore]
        public string ColorHTML
        {
            get => $"#{Color.ToString("X6")}";

            set
            {
                if (uint.TryParse(value.AsSpan(1), System.Globalization.NumberStyles.HexNumber, default, out uint Color))
                {
                    this.Color = Color;
                }
            }
        }

        public void Dispose()
        {
            Recycler<RoleData>.Recycle(this);
        }
    }
}
