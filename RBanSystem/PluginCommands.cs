using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TShockAPI;
using TShockAPI.DB;

namespace BanSystem
{
    public class PluginCommands
    {
        public void Initialize()
        {
            Commands.ChatCommands.RemoveAll((Command p) => p.Names.Contains("ban"));
            Commands.ChatCommands.Add(new Command("bans.addban", BanCommand, "ban"));
        }

        private void BanCommand(CommandArgs e)
        {
            Action invalidSyntax = () => e.Player.SendErrorMessage("Invalid syntax! Use: /ban help");
            Action playerNotFound = () => e.Player.SendErrorMessage("Player not found.");

            if (e.Parameters.Count == 0)
                return;

            switch (e.Parameters[0])
            {
                case "add":
                case "a":
                    if (e.Parameters.Count < 2)
                    {
                        invalidSyntax();
                        return;
                    }
                    string victim = e.Parameters[1];
                    List<TSPlayer> plr = TSPlayer.FindByNameOrID(victim);
                    if (plr.Count != 1)
                    {
                        playerNotFound();
                        return;
                    }
                    int seconds = 172800;
                    TShock.Utils.TryParseTime(e.Parameters[2], out seconds);

                    string reason = "no reason.";
                    if (e.Parameters.Count > 3)
                        reason = e.Parameters[3];

                    BansPlugin.Database.BanPlayer(plr[0], seconds, e.Player, reason);
                    
                    TShock.Utils.Broadcast(string.Format("Administrator {0} has blocked player {1} due to: {2}", e.Player.Name, plr[0].Account.Name, reason), Microsoft.Xna.Framework.Color.Red);

                    BanInformation ban = BansPlugin.Database.GetBan(plr[0].Account.Name);
                    StringBuilder banBuilder = new StringBuilder();
                    banBuilder.AppendLine("----- You are banned on the server! -----");

                    banBuilder.AppendLine("");

                    banBuilder.Append("Administrator: ");
                    banBuilder.AppendLine(ban.administrator.Name);

                    banBuilder.Append("Reason: ");
                    banBuilder.AppendLine(ban.banReason);

                    banBuilder.AppendLine("");

                    banBuilder.AppendLine("Unban occurs through: ");
                    banBuilder.Append(ban.banExpiration.Day);
                    banBuilder.Append("д. ");
                    banBuilder.Append(ban.banExpiration.Hour);
                    banBuilder.Append("ч. ");
                    banBuilder.Append(ban.banExpiration.Minute);
                    banBuilder.Append("мин. ");


                    plr[0].SilentKickInProgress = true;
                    plr[0].Disconnect(banBuilder.ToString());
                    break;
                case "addoffline":
                case "ao":
                    if (e.Parameters.Count < 2)
                    {
                        invalidSyntax();
                        return;
                    }
                    string victim2 = e.Parameters[1];
                    UserAccount account = TShock.UserAccounts.GetUserAccountByName(victim2);
                    if (account == null)
                    {
                        playerNotFound();
                        return;
                    }
                    int seconds2 = 172800;
                    TShock.Utils.TryParseTime(e.Parameters[2], out seconds2);

                    string reason2 = "no reason.";
                    if (e.Parameters.Count > 3)
                        reason2 = e.Parameters[3];

                    var ips = JsonConvert.DeserializeObject<List<string>>(account.KnownIps);
                    BansPlugin.Database.InsertPlayer(victim2, account.UUID, ips.Count == 0 ? "-" : ips[ips.Count - 1], seconds2, e.Player, reason2);
                    TShock.Utils.Broadcast(string.Format("Administrator {0} has blocked player {1} due to: {2}", e.Player.Name, victim2, reason2), Microsoft.Xna.Framework.Color.Red);
                    break;
                case "del":
                case "d":
                    if (e.Parameters.Count != 2)
                    {
                        invalidSyntax();
                        return;
                    }
                    UserAccount acc = TShock.UserAccounts.GetUserAccountByName(e.Parameters[1]);
                    if (acc == null)
                    {
                        invalidSyntax();
                        return;
                    }
                    BansPlugin.Database.UnbanPlayerByAccount(acc.Name);
                    e.Player.SendSuccessMessage("Account unbanned.");
                    break;
                case "delip":
                case "di":
                    if (e.Parameters.Count != 2)
                    {
                        invalidSyntax();
                        return;
                    }
                    BansPlugin.Database.UnbanPlayerByIP(e.Parameters[1]);
                    e.Player.SendSuccessMessage("Account unbanned.");
                    break;
                case "deluuid":
                case "du":
                    if (e.Parameters.Count != 2)
                    {
                        invalidSyntax();
                        return;
                    }
                    BansPlugin.Database.UnbanPlayerByUUID(e.Parameters[1]);
                    e.Player.SendSuccessMessage("Account unbanned.");
                    break;
                case "help":
                case "?":
                    e.Player.SendWarningMessage("— Помощь (/ban help) —");
                    e.Player.SendInfoMessage("/ban add <player> <time> [reason] - ban the account, IP, UUID of the player playing on the server.");
                    e.Player.SendInfoMessage("/ban addoffline <player> <time> [reason] - ban the account, IP, UUID of the player who is playing/not playing on the server. (silent (offline))");
                    e.Player.SendInfoMessage("/ban del <player> - unban account, reason, IP using account.");
                    e.Player.SendInfoMessage("/ban delip <player> - unban account, UUID, IP using IP.");
                    e.Player.SendInfoMessage("/ban deluuid <player> - unban account, UUID, IP using UUID.");
                    e.Player.SendInfoMessage("/ban info <player> - view ban data.");
                    e.Player.SendInfoMessage("/ban list - list bans.");
                    break;
                case "list":
                case "l":
                    e.Player.SendWarningMessage("— Banned players —");
                    IEnumerable<BanInformation> bans = BansPlugin.Database.GetBans();
                    if (bans == null)
                    {
                        e.Player.SendInfoMessage("Ban list empty.");
                    }
                    else
                    {
                        e.Player.SendInfoMessage(string.Join(", ", bans.Select((p) => p.victim)));
                    }
                    
                    break;
                case "info":
                case "i":
                    if (e.Parameters.Count != 2)
                    {
                        invalidSyntax();
                        return;
                    }
                    UserAccount account2 = TShock.UserAccounts.GetUserAccountByName(e.Parameters[1]);
                    if (account2 == null)
                    {
                        invalidSyntax();
                        return;
                    }
                    BanInformation b = BansPlugin.Database.GetBan(account2.Name);
                    e.Player.SendSuccessMessage("— Ban info —");
                    if (b != null)
                    {
                        e.Player.SendInfoMessage("Violator: " + b.victim);
                        e.Player.SendInfoMessage("Violator's IP: " + b.accountIp);
                        e.Player.SendInfoMessage("Violator's UUID (hash): " + b.accountUuid);
                        e.Player.SendInfoMessage("Administrator: " + b.administrator);
                        e.Player.SendInfoMessage(string.Format("Expires in: {0}d. {1}h. {2}m.", b.banExpiration.Day, b.banExpiration.Hour, b.banExpiration.Minute));
                    }
                    else
                    {
                        e.Player.SendErrorMessage("Ban not found.");
                    }

                    break;
                default:
                    e.Player.SendErrorMessage("Command not found, use /ban help to view available commands.");
                    break;
            }
        }
    }
}
