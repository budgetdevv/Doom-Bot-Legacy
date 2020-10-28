using Blazored.LocalStorage;
using DoomBot.Shared;
using DoomBot.Shared.Perks;
using DoomBot.Shared.Perks.Role;
using System;
using System.Net.Http;
using System.Threading.Tasks; 

namespace DoomBot.Client
{
    public class ClientUserData
    {
        private ILocalStorageService LS;
            
        private ISyncLocalStorageService SLS;

        private HttpClient HC;

        public (string Username, string AvatarURL, Perk[] Perks) BaseData;

        public bool Ready = false;

        public delegate void OnUpdate();

        public event OnUpdate OnDownloadComplete;

        //Perks

        public RoleData RoleData;

        public ClientUserData(ILocalStorageService LS, ISyncLocalStorageService SLS, HttpClient HC)
        {
            this.LS = LS;

            this.SLS = SLS;

            this.HC = HC;

            Task.Run(() =>
            {
                _ = Boot();
            });
        }

        private void SetUserID(string UserID)
        {
            SLS.SetItem("UserID", UserID);

            SetUserIDHeader(UserID);
        }

        private void SetUserIDHeader(string UserID)
        {
            HC.DefaultRequestHeaders.Remove("UserID");

            HC.DefaultRequestHeaders.Add("UserID", UserID);
        }

        private void SetAuthToken(string AuthToken)
        {
            SLS.SetItem("AuthToken", AuthToken);

            SetAuthTokenHeader(AuthToken);
        }

        private void SetAuthTokenHeader(string AuthToken)
        {
            HC.DefaultRequestHeaders.Remove("AuthToken");

            HC.DefaultRequestHeaders.Add("AuthToken", AuthToken);
        }

        private async Task Boot()
        {
            SetUserIDHeader(SLS.GetItemAsString("UserID"));

            SetAuthTokenHeader(SLS.GetItemAsString("AuthToken"));

            await GetClientData();

            Ready = true;
        }

        private async Task LoginCallback(string AuthToken)
        {
            SetAuthToken(AuthToken);

            await GetClientData();
        }

        public async Task GetClientData()
        {
            BaseData = await HC.GetAs<(string Username, string AvatarURL, Perk[] Perks)>("Users/Data");

            if (BaseData == default)
            {
                return;
            }

            var Perks = BaseData.Perks;

            //Download Perk-related data

            foreach (var P in Perks)
            {
                switch (P)
                {
                    default: continue;

                    case Perk.RolePerk:
                        {
                            await RevertRoleData();

                            break;
                        }
                }
            }

            OnDownloadComplete.Invoke();
        }

        public async Task UpdateRoleData()
        {
            RoleData = await HC.PostAs<RoleData, RoleData>("Perks/Roles", RoleData);
        }

        public async Task RevertRoleData()
        {
            RoleData = await HC.GetAs<RoleData>("Perks/Roles");
        }

        public async Task TryLogin(string UserID, string Bearer)
        {
            SetUserID(UserID);

            long Token = await HC.GetAs<long>($"Auth/{Bearer}");

            if (Token != default)
            {
                await LoginCallback(Token.ToString());
            }
        }

        public void Logout()
        {
            _ = HC.GetAsync("Auth");

            //Just have to remove one to break the system

            SLS.RemoveItem("AuthToken");

            BaseData = default;

            OnDownloadComplete.Invoke();
        }
    }
}
