#pragma checksum "C:\Users\Administrator\Desktop\Code\DoomBot\Client\Shared\ClientDataWrapper.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2139072ededcb829764b20aecd78795274aadd26"
// <auto-generated/>
#pragma warning disable 1591
namespace DoomBot.Client.Shared
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
    public partial class ClientDataWrapper : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
#nullable restore
#line 26 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\Shared\ClientDataWrapper.razor"
 if (Data.Ready)
{
    

#line default
#line hidden
#nullable disable
            __builder.AddContent(0, 
#nullable restore
#line 28 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\Shared\ClientDataWrapper.razor"
     ChildContent(Data)

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 28 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\Shared\ClientDataWrapper.razor"
                       
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 3 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\Shared\ClientDataWrapper.razor"
       
    [Parameter]

    public RenderFragment<ClientUserData> ChildContent { get; set; }

    [Parameter]

    public Action<ClientUserData> OnDataReady { get; set; }

    protected override async Task OnInitializedAsync()
    {
        while (!Data.Ready)
        {
            await Task.Delay(1);
        }

        if (OnDataReady != default)
        {
            OnDataReady.Invoke(Data);
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ClientUserData Data { get; set; }
    }
}
#pragma warning restore 1591
