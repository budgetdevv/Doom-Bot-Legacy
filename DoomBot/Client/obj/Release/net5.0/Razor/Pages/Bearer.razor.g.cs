#pragma checksum "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\Bearer.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3b854066a189fb9bee670f732430728e83b766c2"
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
#line 1 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\_Imports.razor"
using DoomBot.Client;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\_Imports.razor"
using DoomBot.Client.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\Bearer.razor"
using DoomBot.Shared;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/Bearer")]
    [Microsoft.AspNetCore.Components.RouteAttribute("/Bearer/{BToken:long}")]
    public partial class Bearer : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<DoomBot.Client.Shared.ClientDataWrapper>(0);
            __builder.AddAttribute(1, "OnDataReady", new System.Action<DoomBot.Client.ClientUserData>(
#nullable restore
#line 20 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\Bearer.razor"
                                   async (x) =>
                                   {

                                       if (BToken == default)
                                       {
                                           _ = HC.GetAsync("Auth");

                                           Data.BaseData = default;
                                       }

                                       else if (Data.BaseData.Username == default)
                                       {
                                           BToken = await HC.GetAs<long>($"Auth/{BToken}");

                                           if (BToken != default)
                                           {
                                               await Data.LoginCallback(BToken.ToString());
                                           }
                                       }

                                       NM.NavigateTo($"", true);
                                   }

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line 12 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\Bearer.razor"
      

    [Parameter]
    public long BToken { get; set; }



#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ClientUserData Data { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private HttpClient HC { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager NM { get; set; }
    }
}
#pragma warning restore 1591
