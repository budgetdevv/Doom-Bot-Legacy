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
        public override async Task OnActionExecutionAsync(ActionExecutingContext Context, ActionExecutionDelegate Next)
        {
            var Services = Context.HttpContext.RequestServices;

            var UserAccessor = (UserAccessor)Services.GetService(typeof(UserAccessor));

            if (UserAccessor == default)
            {
                return;
            }

            var AM = (AuthManager)Services.GetService(typeof(AuthManager));

            var UserData = AM.TryAuth(Context.HttpContext);

            if (UserData == default)
            {
                return;
            }

            UserAccessor.ReadUserAs(UserData.GUser);

            await Next();
        }
    }
}
