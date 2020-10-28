#pragma checksum "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "20a4d4edb663edc4003b85a59a24be4c205ff9e0"
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
    [Microsoft.AspNetCore.Components.RouteAttribute("/Perks/Role")]
    public partial class RolePerk : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<DoomBot.Client.Shared.ClientDataWrapper>(0);
            __builder.AddAttribute(1, "OnDataReady", new System.Action<DoomBot.Client.ClientUserData>(
#nullable restore
#line 22 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                                 (x) =>
                                                                    {

                                                     bool Yes = x.BaseData.Perks.Contains(DoomBot.Shared.Perks.Perk.RoleCustomizerPerk);

                                                     Stuff = new (string Name, Func<bool> Enabled, Action Callback)[]
                                                     {
                           ("Overview", () => true, () => { Sel = "Overview"; StateHasChanged(); } ),

                           ("Name", () => Yes, () => { Sel = "Name"; StateHasChanged(); } ),

                           ("Color", () => Yes, () => { Sel = "Color"; StateHasChanged(); } ),

                           ("Visibility", () => Yes, () => { Sel = "Visibility"; StateHasChanged(); } )

                                                     };
                                                 }

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment<DoomBot.Client.ClientUserData>)((Data) => (__builder2) => {
                __builder2.OpenComponent<DoomBot.Client.Shared.HeroMed>(3);
                __builder2.AddAttribute(4, "Stuff", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<(System.String Name, System.Func<System.Boolean> Enabled, System.Action Callback)[]>(
#nullable restore
#line 41 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                    Stuff

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.AddMarkupContent(6, "<h1 class=\"title is-1 has-text-dark\">Role Perk</h1>\r\n\r\n        ");
                    __builder3.AddMarkupContent(7, "<h2 class=\"subtitle has-text-dark\">\r\n            Manage your Custom Role here ( If you have one haha )\r\n        </h2>");
                }
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(8, "\r\n\r\n    ");
                __builder2.OpenElement(9, "section");
                __builder2.AddAttribute(10, "class", "section");
#nullable restore
#line 50 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
         if (Data.BaseData.Username == default)
        {
            NM.NavigateTo("");
        }


        else if (Data.BaseData.Perks.Contains(DoomBot.Shared.Perks.Perk.RolePerk))
        {
            bool UpdateButtons = false;


#line default
#line hidden
#nullable disable
                __builder2.OpenElement(11, "div");
                __builder2.AddAttribute(12, "class", "columns is-centered");
                __builder2.OpenElement(13, "div");
                __builder2.AddAttribute(14, "class", "column is-one-fifth");
                __builder2.OpenElement(15, "div");
                __builder2.AddAttribute(16, "class", "card");
                __builder2.OpenElement(17, "div");
                __builder2.AddAttribute(18, "class", "card-image");
                __builder2.OpenElement(19, "figure");
                __builder2.AddAttribute(20, "class", "image is-square");
                __builder2.OpenElement(21, "img");
                __builder2.AddAttribute(22, "src", 
#nullable restore
#line 66 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                           Data.BaseData.AvatarURL

#line default
#line hidden
#nullable disable
                );
                __builder2.AddAttribute(23, "alt", "Avatar");
                __builder2.CloseElement();
                __builder2.CloseElement();
                __builder2.CloseElement();
                __builder2.AddMarkupContent(24, "\r\n                        ");
                __builder2.OpenElement(25, "div");
                __builder2.AddAttribute(26, "class", "card-content has-text-centered");
                __builder2.OpenElement(27, "p");
                __builder2.AddAttribute(28, "class", "title is-4");
                __builder2.AddContent(29, 
#nullable restore
#line 70 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                                   Data.BaseData.Username

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(30, "\r\n\r\n                            ");
                __builder2.OpenElement(31, "div");
                __builder2.AddAttribute(32, "class", "content");
#nullable restore
#line 74 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                 if (Data.RoleData == default)
                                {

#line default
#line hidden
#nullable disable
                __builder2.OpenElement(33, "button");
                __builder2.AddAttribute(34, "class", "button is-dark");
                __builder2.AddAttribute(35, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 76 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                                                                () => { CreateRole = true; } 

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddContent(36, "Create Role!");
                __builder2.CloseElement();
#nullable restore
#line 77 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                }

                                else
                                {
                                    UpdateButtons = true;

                                    switch (Sel)
                                    {
                                        case "Overview":
                                            {

#line default
#line hidden
#nullable disable
                __builder2.OpenElement(37, "span");
                __builder2.AddAttribute(38, "class", "tag is-medium");
                __builder2.AddAttribute(39, "style", "background-color:" + " " + (
#nullable restore
#line 87 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                                                                                      Data.RoleData.ColorString

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddContent(40, 
#nullable restore
#line 88 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                                     Data.RoleData.Name

#line default
#line hidden
#nullable disable
                );
                __builder2.CloseElement();
                __builder2.AddMarkupContent(41, "<br><br><br>");
                __builder2.OpenElement(42, "label");
                __builder2.AddAttribute(43, "class", "checkbox");
                __builder2.OpenElement(44, "input");
                __builder2.AddAttribute(45, "type", "checkbox");
                __builder2.AddAttribute(46, "checked", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 94 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                                                                  Data.RoleData.Enabled

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(47, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => Data.RoleData.Enabled = __value, Data.RoleData.Enabled));
                __builder2.SetUpdatesAttributeName("checked");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(48, "\r\n                                                    Toggled ( Assigned to you )\r\n                                                ");
                __builder2.CloseElement();
#nullable restore
#line 97 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"

                                                break;
                                            }

                                        case "Name":
                                            {
                                                {

#line default
#line hidden
#nullable disable
                __builder2.OpenElement(49, "input");
                __builder2.AddAttribute(50, "class", "input");
                __builder2.AddAttribute(51, "type", "text");
                __builder2.AddAttribute(52, "placeholder", "Role Name");
                __builder2.AddAttribute(53, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 104 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                                                                                                    Data.RoleData.Name

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(54, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => Data.RoleData.Name = __value, Data.RoleData.Name));
                __builder2.SetUpdatesAttributeName("value");
                __builder2.CloseElement();
#nullable restore
#line 105 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"

                                                    break;
                                                }
                                            }

                                        case "Color":
                                            {
                                                {

#line default
#line hidden
#nullable disable
                __builder2.OpenElement(55, "input");
                __builder2.AddAttribute(56, "type", "color");
                __builder2.AddAttribute(57, "id", "colorpicker");
                __builder2.AddAttribute(58, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 113 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                                                                                Data.RoleData.ColorString

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(59, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => Data.RoleData.ColorString = __value, Data.RoleData.ColorString));
                __builder2.SetUpdatesAttributeName("value");
                __builder2.CloseElement();
#nullable restore
#line 114 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"

                                                    break;
                                                }
                                            }

                                        case "Visibility":
                                            {

#line default
#line hidden
#nullable disable
                __builder2.OpenElement(60, "label");
                __builder2.AddAttribute(61, "class", "checkbox");
                __builder2.OpenElement(62, "input");
                __builder2.AddAttribute(63, "type", "checkbox");
                __builder2.AddAttribute(64, "checked", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 122 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                                                                  Data.RoleData.Hoisted

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(65, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => Data.RoleData.Hoisted = __value, Data.RoleData.Hoisted));
                __builder2.SetUpdatesAttributeName("checked");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(66, "\r\n                                                    Visible ( Online List )\r\n                                                ");
                __builder2.CloseElement();
#nullable restore
#line 125 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"

                                                break;
                                            }
                                    }
                                }

#line default
#line hidden
#nullable disable
                __builder2.CloseElement();
                __builder2.CloseElement();
                __builder2.CloseElement();
                __builder2.CloseElement();
                __builder2.CloseElement();
#nullable restore
#line 137 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"

            if (UpdateButtons)
            {

#line default
#line hidden
#nullable disable
                __builder2.AddMarkupContent(67, "<br>");
                __builder2.OpenElement(68, "div");
                __builder2.AddAttribute(69, "class", "columns is-centered");
                __builder2.OpenElement(70, "div");
                __builder2.AddAttribute(71, "class", "columns is-centered");
                __builder2.OpenElement(72, "div");
                __builder2.AddAttribute(73, "class", "column");
                __builder2.OpenElement(74, "button");
                __builder2.AddAttribute(75, "class", "button is-dark");
                __builder2.AddAttribute(76, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 146 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                                                        async () => await Data.UpdateRoleData() 

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddContent(77, "Save Changes");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(78, "\r\n\r\n                            ");
                __builder2.OpenElement(79, "button");
                __builder2.AddAttribute(80, "class", "button is-light");
                __builder2.AddAttribute(81, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 148 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                                                         async () => await Data.RevertRoleData() 

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddContent(82, "Discard Changes");
                __builder2.CloseElement();
                __builder2.CloseElement();
                __builder2.CloseElement();
                __builder2.CloseElement();
#nullable restore
#line 153 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
            }
        }

        else
        {

#line default
#line hidden
#nullable disable
                __builder2.AddMarkupContent(83, "<h1 class=\"title is-1 has-text-dark\">You do NOT have the Perk! Consider purchasing it in our Shop!</h1>");
#nullable restore
#line 159 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
        }

#line default
#line hidden
#nullable disable
                __builder2.CloseElement();
#nullable restore
#line 162 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
     if (true)
    {

#line default
#line hidden
#nullable disable
                __builder2.OpenComponent<DoomBot.Client.Shared.Modal>(84);
                __builder2.AddAttribute(85, "Title", "Create New Role");
                __builder2.AddAttribute(86, "Enabled", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 164 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                                CreateRole

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(87, "OKAction", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create(this, 
#nullable restore
#line 165 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                            async () => { CreateRole = false; Data.RoleData = new DoomBot.Shared.Perks.Role.RoleData() { Name = NewRoleName };  await Data.UpdateRoleData(); } 

#line default
#line hidden
#nullable disable
                )));
                __builder2.AddAttribute(88, "CancelAction", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create(this, 
#nullable restore
#line 166 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                               () => { CreateRole = false; Data.RoleData = default; } 

#line default
#line hidden
#nullable disable
                )));
                __builder2.AddAttribute(89, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.OpenElement(90, "div");
                    __builder3.AddAttribute(91, "class", "container");
                    __builder3.AddMarkupContent(92, "<h2>Input a Role Name and click on \"Ok\" to create a Custom Role!</h2>\r\n\r\n                <br>\r\n\r\n                ");
                    __builder3.OpenElement(93, "input");
                    __builder3.AddAttribute(94, "class", "input");
                    __builder3.AddAttribute(95, "type", "text");
                    __builder3.AddAttribute(96, "placeholder", "Role Name");
                    __builder3.AddAttribute(97, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 173 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
                                                                                NewRoleName

#line default
#line hidden
#nullable disable
                    ));
                    __builder3.AddAttribute(98, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => NewRoleName = __value, NewRoleName));
                    __builder3.SetUpdatesAttributeName("value");
                    __builder3.CloseElement();
                    __builder3.CloseElement();
                }
                ));
                __builder2.CloseComponent();
#nullable restore
#line 177 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
    }

#line default
#line hidden
#nullable disable
            }
            ));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line 7 "C:\Users\Administrator\Desktop\Code\DoomBot\DoomBot\Client\Pages\RolePerk.razor"
      
    private (string Name, Func<bool> Enabled, Action Callback)[] Stuff;

    private bool CreateRole;

    private string Sel;

    private string NewRoleName;

    protected override void OnInitialized()
    {
        CreateRole = false;
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private HttpClient HC { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager NM { get; set; }
    }
}
#pragma warning restore 1591