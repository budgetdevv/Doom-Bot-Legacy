using Newtonsoft.Json;
using Recycler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DoomBot.Shared.Perks.Role
{
    public class RoleData: IDisposable
    {
        public string Name { get; set; }

        public bool Enabled { get; set; }

        public bool Hoisted { get; set; }

        public Color Color { get; set; }

        [JsonIgnore]
        public string ColorString
        {
            get
            {
                var SB = new StringBuilder("#");

                SB.Append(Color.R.ToString("X2"));

                SB.Append(Color.G.ToString("X2"));

                SB.Append(Color.B.ToString("X2"));

                return SB.ToString();
            }

            set
            {
                try
                {
                    Color = ColorTranslator.FromHtml($"{value}");
                }

                catch
                {

                }
            }
        }

        public void Dispose()
        {
            Recycler<RoleData>.Recycle(this);
        }
    }
}
