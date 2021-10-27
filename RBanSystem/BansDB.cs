using System.Data;
using TShockAPI.DB;
using Mono.Data.Sqlite;
using MySql.Data;
using MySql.Data.MySqlClient;
using TShockAPI;
using System;
using System.Collections.Generic;

namespace BanSystem
{
    public class BansDB
    {
        public IDbConnection dbConnection;
        public BansDB(IDbConnection connection)
        {
            dbConnection = connection;
            SqlTable table = new SqlTable("PollyxBans", new SqlColumn[]
            {
                new SqlColumn("VictimAccount", MySqlDbType.String),
                new SqlColumn("VictimUUID", MySqlDbType.String),
                new SqlColumn("VictimIP", MySqlDbType.String),
                new SqlColumn("BanTime", MySqlDbType.String),
                new SqlColumn("Administrator", MySqlDbType.String),
                new SqlColumn("Reason", MySqlDbType.String)
            });
            IQueryBuilder provider;
            if (connection.GetSqlType() != SqlType.Sqlite)
            {
                IQueryBuilder queryBuilder = new MysqlQueryCreator();
                provider = queryBuilder;
            }
            else
            {
                IQueryBuilder queryBuilder = new SqliteQueryCreator();
                provider = queryBuilder;
            }
            SqlTableCreator sqlTableCreator = new SqlTableCreator(dbConnection, provider);
            sqlTableCreator.EnsureTableStructure(table);
        }
        public ExpiredData CheckExpired(string accountName)
        {
            BanInformation ban = GetBan(accountName);
            if (ban == null)
                return new ExpiredData(true, null);
            return new ExpiredData(ban.banExpiration < DateTime.UtcNow, ban);
        }
        public ExpiredData CheckExpiredByUUID(string uuid)
        {
            BanInformation ban = GetBanByUUID(uuid);
            if (ban == null)
                return new ExpiredData(true, null);
            return new ExpiredData(ban.banExpiration < DateTime.UtcNow, ban);
        }
        public ExpiredData CheckExpiredByIP(string ip)
        {
            BanInformation ban = GetBanByIP(ip);
            if (ban == null)
                return new ExpiredData(true, null);
            return new ExpiredData(ban.banExpiration < DateTime.UtcNow, ban);
        }
        public IEnumerable<BanInformation> GetBans()
        {
            string command = "SELECT * FROM Bans;";
            QueryResult result = dbConnection.QueryReader(command);
            while (result.Read())
                yield return new BanInformation(
                    result.Get<string>("VictimAccount"),
                    result.Get<string>("Administrator"),
                    result.Get<string>("VictimUUID"),
                    result.Get<string>("VictimIP"),
                    result.Get<string>("Reason"),
                    result.Get<string>("BanTime"));
        }
        public BanInformation GetBan(string victim)
        {
            string command = "SELECT * FROM Bans WHERE VictimAccount=@0;";
            QueryResult result = dbConnection.QueryReader(command, victim);
            while (result.Read())
            {
                BanInformation v = new BanInformation(
                    result.Get<string>("VictimAccount"),
                    result.Get<string>("Administrator"),
                    result.Get<string>("VictimUUID"),
                    result.Get<string>("VictimIP"),
                    result.Get<string>("Reason"),
                    result.Get<string>("BanTime"));

                if (v.banExpiration > DateTime.UtcNow)
                    return v;
            }

            return null;
        }
        public BanInformation GetBanByUUID(string uuid)
        {
            string command = "SELECT * FROM Bans WHERE VictimUUID=@0;";
            QueryResult result = dbConnection.QueryReader(command, uuid);
            while (result.Read())
            {
                BanInformation v = new BanInformation(
                    result.Get<string>("VictimAccount"),
                    result.Get<string>("Administrator"),
                    result.Get<string>("VictimUUID"),
                    result.Get<string>("VictimIP"),
                    result.Get<string>("Reason"),
                    result.Get<string>("BanTime"));

                if (v.banExpiration > DateTime.UtcNow)
                    return v;
            }

            return null;
        }
        public BanInformation GetBanByIP(string ip)
        {
            string command = "SELECT * FROM Bans WHERE VictimIP=@0;";
            QueryResult result = dbConnection.QueryReader(command, ip);
            while (result.Read())
            {
                BanInformation v = new BanInformation(
                    result.Get<string>("VictimAccount"),
                    result.Get<string>("Administrator"),
                    result.Get<string>("VictimUUID"),
                    result.Get<string>("VictimIP"),
                    result.Get<string>("Reason"),
                    result.Get<string>("BanTime"));

                if (v.banExpiration > DateTime.UtcNow)
                    return v;
            }

            return null;
        }
        public void BanPlayer(TSPlayer victim, int seconds, TSPlayer administrator, string reason = "-")
        {
            string command = "INSERT INTO Bans (VictimAccount, VictimUUID, VictimIP, BanTime, Administrator, Reason) VALUES (@0, @1, @2, @3, @4, @5);";
            dbConnection.Query(command, victim.Account.Name, victim.UUID, victim.IP, DateTime.UtcNow.AddSeconds(seconds).ToString(), administrator.Account.Name, reason);
        }
        public void InsertPlayer(string victim, string UUID, string IP, int seconds, TSPlayer administrator, string reason = "-")
        {
            string command = "INSERT INTO Bans (VictimAccount, VictimUUID, VictimIP, BanTime, Administrator, Reason) VALUES (@0, @1, @2, @3, @4, @5);";
            dbConnection.Query(command, victim, UUID, IP, DateTime.UtcNow.AddSeconds(seconds).ToString(), administrator.Account.Name, reason);
        }
        public void UnbanPlayerByAccount(string victim)
        {
            string command = "DELETE FROM Bans WHERE VictimAccount=@0;";
            dbConnection.Query(command, victim);
        }
        public void UnbanPlayerByUUID(string uuid)
        {
            string command = "DELETE FROM Bans WHERE VictimUUID=@0;";
            dbConnection.Query(command, uuid);
        }
        public void UnbanPlayerByIP(string ip)
        {
            string command = "DELETE FROM Bans WHERE VictimIP=@0;";
            dbConnection.Query(command, ip);
        }
    }
}
