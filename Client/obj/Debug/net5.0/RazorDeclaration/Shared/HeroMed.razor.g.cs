// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

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
    public partial class HeroMed : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 1 "C:\Users\Administrator\Desktop\Code\DoomBot\Client\Shared\HeroMed.razor"
      
    [Parameter]

    public (string Name, bool Enabled, Action Callback)[] Stuff { get; set; }

    [Parameter]

    public RenderFragment ChildContent { get; set; }

    private string CurrentStuff;

    protected override void OnInitialized()
    {
        if (Stuff.Length == 0)
        {
            return;
        }

        var S = Stuff[0];

        CurrentStuff = S.Name;

        S.Callback.Invoke();
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
