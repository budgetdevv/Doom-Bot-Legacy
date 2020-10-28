using DoomBot.Server.Managers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http;
using DoomBot.Server.Modules;
using Discord;
using System.Threading.Tasks;

namespace DoomBot.Server.Controllers.Attributes
{
    public class RequireUserAccessAttribute : ActionFilterAttribute
    {
        /*

        public override async Task OnActionExecutionAsync(ActionExecutingContext Context, ActionExecutionDelegate Act)
        {
            void Auth()
            {
                var Services = Context.HttpContext.RequestServices;

                var UserAccessor = (UserAccessor)Services.GetService(typeof(UserAccessor));

                if (UserAccessor == default)
                {
                    return;
                }

                var AM = (AuthManager)Services.GetService(typeof(AuthManager));

                var TokenData = AM.TryAuth(Context.HttpContext);

                if (TokenData == default)
                {
                    return;
                }

                UserAccessor.ReadUserAs(TokenData.GUser, TokenData.Token);
            }

            Auth();

            //We don't care about what happens after

            _ =  Act();
        }

        */

        public override async Task OnActionExecutionAsync(ActionExecutingContext Context, ActionExecutionDelegate Next)
        {
            var Services = Context.HttpContext.RequestServices;

            var UserAccessor = (UserAccessor)Services.GetService(typeof(UserAccessor));

            if (UserAccessor == default)
            {
                return;
            }

            var AM = (AuthManager)Services.GetService(typeof(AuthManager));

            var TokenData = AM.TryAuth(Context.HttpContext);

            if (TokenData == default)
            {
                return;
            }

            UserAccessor.ReadUserAs(TokenData.GUser, TokenData.Token);

            await Next();
        }
    }
}
