#pragma checksum "C:\Users\Administrator\Desktop\Code\DoomBot\Client\Pages\Index.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a1ccf9ad7f4c007a3e0fb736d3baac943b6aae07"
// <auto-generated/>
#pragma warning disable 1591
namespace DoomBot.Client.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\_Imports.razor"
using DoomBot.Client;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\_Imports.razor"
using DoomBot.Client.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\Pages\Index.razor"
using DoomBot.Shared;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/")]
    public partial class Index : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "section");
            __builder.AddAttribute(1, "class", "hero is-fullheight");
            __builder.OpenElement(2, "div");
            __builder.AddAttribute(3, "class", "hero-body");
            __builder.AddAttribute(4, "style", @"background-image: url(https://cdn.discordapp.com/attachments/769521085653450763/769525946000080906/KingsAndQueensExtended.png);
        background-position: center left;
        background-repeat: no-repeat;
        background-size: contain;
        background-color: #F5F2B3;
    ");
            __builder.OpenElement(5, "div");
            __builder.AddAttribute(6, "class", "container has-text-centered");
            __builder.AddMarkupContent(7, "<h1 class=\"title is-1\">Kings & Queens</h1>\r\n\r\n            ");
            __builder.AddMarkupContent(8, "<h2 class=\"subtitle\">\r\n                Join today for the ULTIMATE Discord Experience!\r\n            </h2>\r\n\r\n            <br><br>\r\n\r\n            ");
            __builder.OpenElement(9, "div");
            __builder.AddAttribute(10, "class", "columns is-centered has-text-centered");
            __builder.OpenElement(11, "div");
            __builder.AddAttribute(12, "class", "column is-2 is-centered");
            __builder.OpenElement(13, "article");
            __builder.AddAttribute(14, "class", "message");
            __builder.AddMarkupContent(15, "<div class=\"message-header\"><p>Online Members</p></div>\r\n\r\n                        ");
            __builder.OpenElement(16, "div");
            __builder.AddAttribute(17, "class", "message-body");
            __builder.AddContent(18, 
#nullable restore
#line 45 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\Pages\Index.razor"
                             Membercount.Online

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(19, "\r\n                            <br>");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(20, "\r\n\r\n                ");
            __builder.OpenElement(21, "div");
            __builder.AddAttribute(22, "class", "column is-2 is-centered");
            __builder.OpenElement(23, "article");
            __builder.AddAttribute(24, "class", "message");
            __builder.AddMarkupContent(25, "<div class=\"message-header\"><p>Total Members</p></div>\r\n\r\n                        ");
            __builder.OpenElement(26, "div");
            __builder.AddAttribute(27, "class", "message-body");
            __builder.AddContent(28, 
#nullable restore
#line 60 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\Pages\Index.razor"
                             Membercount.Total

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(29, "\r\n                            <br>");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(30, "\r\n\r\n            <br><br>\r\n\r\n            ");
            __builder.AddMarkupContent(31, "<a class=\"button is-large\" style=\"border: 5px solid black; border-color: #FFB52C; background-color: #E1DC76\" href=\"https://discord.gg/kfnPZmR\">Join us Today!</a>");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 7 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\Pages\Index.razor"
          
    (int Online, int Total) Membercount;

    protected override async Task OnInitializedAsync()
    {
        Membercount = await HC.GetAs <(int, int)>("Guild/Presence");
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private HttpClient HC { get; set; }
    }
}
#pragma warning restore 1591
