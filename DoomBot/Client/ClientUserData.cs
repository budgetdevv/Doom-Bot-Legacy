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

        //Perks

        public RoleData RoleData;

        public ClientUserData(ILocalStorageService LS, ISyncLocalStorageService SLS, HttpClient HC)
        {
            this.LS = LS;

            this.SLS = SLS;

            this.HC = HC;

            if (SLS.ContainKey("AuthToken"))
            {
                _ = Task.Run(async () =>
                {
                    await LoginCallback(SLS.GetItemAsString("AuthToken"));

                    Ready = true;
                });
            }

            else
            {
                Ready = true;
            }
        }

        public async Task LoginCallback(string AccessToken)
        {
            try
            {
                await LS.SetItemAsync("AuthToken", AccessToken);

                //HC.DefaultRequestHeaders.Clear();

                HC.DefaultRequestHeaders.Add("AuthToken", AccessToken);

                await GetClientData();
            }

            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        public async Task GetClientData()
        {
            BaseData = await HC.GetAs<(string Username, string AvatarURL, Perk[] Perks)>("Users/Data");

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
        }

        public async Task UpdateRoleData()
        {
            RoleData = await HC.PostAs<RoleData, RoleData>("Perks/Roles", RoleData);
        }

        public async Task RevertRoleData()
        {
            RoleData = await HC.GetAs<RoleData>("Perks/Roles");
        }
    }
}
