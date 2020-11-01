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
    /*public class ElementActionComparer : IEqualityComparer<(string Emoji, Func<EmbedMenu, Task<bool>> Act)>
    {
        public bool Equals((string Emoji, Func<EmbedMenu, Task<bool>> Act) X, (string Emoji, Func<EmbedMenu, Task<bool>> Act) Y)
        {
            return X.Emoji == Y.Emoji;
        }

        public int GetHashCode((string Emoji, Func<EmbedMenu, Task<bool>> Act) Obj)
        {
            return Obj.Emoji.GetHashCode();
        }
    }*/
    
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

        /*

private Span<(string Emoji, string Name, string Desc)> GetCurrentPageElementsUnsafe()
{
//Collection of Elements should NOT be modified after compilation

int StartIndex = (Page - 1) * PageSize;

int RemainingElements = ElementsMetaData.Count - 1 - StartIndex;

if (RemainingElements < PageSize)
{
return CollectionsMarshal.AsSpan(ElementsMetaData).Slice(StartIndex);
} 

return CollectionsMarshal.AsSpan(ElementsMetaData).Slice(StartIndex, PageSize);
}

*/
    }
}