using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DoomBot.Server.Modules;

namespace DoomBot.Server.EM
{
    public class EmbedMenu
    {
        //Instance
        
        protected readonly List<(string Emoji, string Name, string Desc, Func<EmbedMenu, Task<bool>> Act)> ElementsMetaData;

        protected int Page;

        public string Title, Desc;

        public int PageSize, ExpireIn;

        protected EmbedMenu()
        {
            ElementsMetaData = new List<(string Emoji, string Name, string Desc, Func<EmbedMenu, Task<bool>> Act)>();
        }
        
        public void AddElement(string Emoji, string Name, string Desc, Func<EmbedMenu, Task<bool>> Act = default)
        {
            ElementsMetaData.Add((Emoji, Name, Desc, Act));
        }
    }
}