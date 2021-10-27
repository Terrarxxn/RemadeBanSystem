using System;
using TShockAPI;
using TShockAPI.DB;

namespace BanSystem
{
    public class BanInformation
    {
        public BanInformation(string victim, string administrator, string accountUuid, string accountIp, string banReason, string banExpiration)
        {
            this.accountUuid = accountUuid;
            this.accountIp = accountIp;
            this.banReason = banReason;

            this.victim = TShock.UserAccounts.GetUserAccountByName(victim);
            this.administrator = TShock.UserAccounts.GetUserAccountByName(administrator);

            DateTime banOccured;
            if (DateTime.TryParse(banExpiration, out banOccured))
            this.banExpiration = banOccured;
        }
        public readonly UserAccount victim;
        public readonly string accountUuid;
        public readonly string accountIp;
        public readonly DateTime banExpiration;
        public readonly UserAccount administrator;
        public readonly string banReason;
    }
}
