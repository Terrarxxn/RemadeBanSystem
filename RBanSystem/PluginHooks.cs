using TShockAPI.Hooks;
using TerrariaApi.Server;
using TShockAPI;
using System.Text;
using System;

namespace BanSystem
{
    public class PluginHooks
    {
        private BansPlugin _bansPlugin;
        public PluginHooks(BansPlugin plugin)
        {
            _bansPlugin = plugin;
        }
        public void InitializeHooks()
        {
            ServerApi.Hooks.ServerJoin.Register(_bansPlugin, Join, -int.MaxValue);
            PlayerHooks.PlayerPostLogin += PlayerLogin;
        }
        public void DeinitializeHooks()
        {
            ServerApi.Hooks.ServerJoin.Deregister(_bansPlugin, Join);
            PlayerHooks.PlayerPostLogin -= PlayerLogin;
        }

        private void Join(JoinEventArgs args)
        {
            try
            {
                TSPlayer player = TShock.Players[args.Who];

                ExpiredData data2 = BansPlugin.Database.CheckExpiredByUUID(player.UUID);
                if (!data2.isExpired)
                {
                    StringBuilder banBuilder = new StringBuilder();
                    banBuilder.AppendLine("----- You are banned on the server! -----");

                    banBuilder.AppendLine("");

                    banBuilder.Append("Administrator: ");
                    banBuilder.AppendLine(data2.ban.administrator.Name);

                    banBuilder.Append("Reason: ");
                    banBuilder.AppendLine(data2.ban.banReason);

                    banBuilder.AppendLine("");

                    banBuilder.AppendLine("Unban occurs through: ");
                    TimeSpan span = data2.ban.banExpiration - System.DateTime.UtcNow;
                    banBuilder.Append(span.Days);
                    banBuilder.Append("d. ");
                    banBuilder.Append(span.Hours);
                    banBuilder.Append("h. ");
                    banBuilder.Append(span.Minutes);
                    banBuilder.Append("m. ");


                    player.SilentKickInProgress = true;
                    player.SendData(PacketTypes.Status, banBuilder.ToString());
                    player.SendData(PacketTypes.Disconnect, banBuilder.ToString());
                    return;
                }

                ExpiredData data3 = BansPlugin.Database.CheckExpiredByIP(player.IP);
                if (!data3.isExpired)
                {
                    StringBuilder banBuilder = new StringBuilder();
                    banBuilder.AppendLine("----- You are banned on the server! -----");

                    banBuilder.AppendLine("");

                    banBuilder.Append("Administrator: ");
                    banBuilder.AppendLine(data3.ban.administrator.Name);

                    banBuilder.Append("Reason: ");
                    banBuilder.AppendLine(data3.ban.banReason);

                    banBuilder.AppendLine("");

                    banBuilder.AppendLine("Unban occurs through: ");
                    TimeSpan span = data3.ban.banExpiration - System.DateTime.UtcNow;
                    banBuilder.Append(span.Days);
                    banBuilder.Append("d. ");
                    banBuilder.Append(span.Hours);
                    banBuilder.Append("h. ");
                    banBuilder.Append(span.Minutes);
                    banBuilder.Append("m. ");


                    player.SilentKickInProgress = true;
                    player.SendData(PacketTypes.Status, banBuilder.ToString());
                    player.SendData(PacketTypes.Disconnect, banBuilder.ToString());
                    return;
                }

                if (player.Account != null)
                {
                    ExpiredData data = BansPlugin.Database.CheckExpired(player.Account.Name);
                    if (!data.isExpired)
                    {
                        StringBuilder banBuilder = new StringBuilder();
                        banBuilder.AppendLine("----- You are banned on the server! -----");

                        banBuilder.AppendLine("");

                        banBuilder.Append("Administrator: ");
                        banBuilder.AppendLine(data.ban.administrator.Name);

                        banBuilder.Append("Reason: ");
                        banBuilder.AppendLine(data.ban.banReason);

                        banBuilder.AppendLine("");

                        banBuilder.AppendLine("Unban occurs through: ");
                        TimeSpan span = data.ban.banExpiration - System.DateTime.UtcNow;
                        banBuilder.Append(span.Days);
                        banBuilder.Append("д. ");
                        banBuilder.Append(span.Hours);
                        banBuilder.Append("ч. ");
                        banBuilder.Append(span.Minutes);
                        banBuilder.Append("мин. ");


                        player.SilentKickInProgress = true;
                        player.SendData(PacketTypes.Status, banBuilder.ToString());
                        player.SendData(PacketTypes.Disconnect, banBuilder.ToString());
                    }
                }
            } catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
            }
        }
        private void PlayerLogin(PlayerPostLoginEventArgs args)
        {
            try
            {
                TSPlayer player = args.Player;

                ExpiredData data2 = BansPlugin.Database.CheckExpiredByUUID(player.UUID);
                if (!data2.isExpired)
                {
                    StringBuilder banBuilder = new StringBuilder();
                    banBuilder.AppendLine("----- You are banned on the server! -----");

                    banBuilder.AppendLine("");

                    banBuilder.Append("Administrator: ");
                    banBuilder.AppendLine(data2.ban.administrator.Name);

                    banBuilder.Append("Reason: ");
                    banBuilder.AppendLine(data2.ban.banReason);

                    banBuilder.AppendLine("");

                    banBuilder.AppendLine("Unban occurs through: ");
                    TimeSpan span = data2.ban.banExpiration - System.DateTime.UtcNow;
                    banBuilder.Append(span.Days);
                    banBuilder.Append("d. ");
                    banBuilder.Append(span.Hours);
                    banBuilder.Append("h. ");
                    banBuilder.Append(span.Minutes);
                    banBuilder.Append("m. ");


                    player.SilentKickInProgress = true;
                    player.SendData(PacketTypes.Status, banBuilder.ToString());
                    player.SendData(PacketTypes.Disconnect, banBuilder.ToString());
                    return;
                }

                ExpiredData data3 = BansPlugin.Database.CheckExpiredByIP(player.IP);
                if (!data3.isExpired)
                {
                    StringBuilder banBuilder = new StringBuilder();
                    banBuilder.AppendLine("----- You are banned on the server! -----");

                    banBuilder.AppendLine("");

                    banBuilder.Append("Administrator: ");
                    banBuilder.AppendLine(data3.ban.administrator.Name);

                    banBuilder.Append("Reason: ");
                    banBuilder.AppendLine(data3.ban.banReason);

                    banBuilder.AppendLine("");

                    banBuilder.AppendLine("Unban occurs through: ");
                    TimeSpan span = data3.ban.banExpiration - System.DateTime.UtcNow;
                    banBuilder.Append(span.Days);
                    banBuilder.Append("д. ");
                    banBuilder.Append(span.Hours);
                    banBuilder.Append("ч. ");
                    banBuilder.Append(span.Minutes);
                    banBuilder.Append("мин. ");


                    player.SilentKickInProgress = true;
                    player.SendData(PacketTypes.Status, banBuilder.ToString());
                    player.SendData(PacketTypes.Disconnect, banBuilder.ToString());
                    return;
                }
                ExpiredData data = BansPlugin.Database.CheckExpired(player.Account.Name);
                if (!data.isExpired)
                {
                    StringBuilder banBuilder = new StringBuilder();
                    banBuilder.AppendLine("----- You are banned on the server! -----");

                    banBuilder.AppendLine("");

                    banBuilder.Append("Administrator: ");
                    banBuilder.AppendLine(data.ban.administrator.Name);

                    banBuilder.Append("Reason: ");
                    banBuilder.AppendLine(data.ban.banReason);

                    banBuilder.AppendLine("");

                    banBuilder.AppendLine("Unban occurs through: ");
                    TimeSpan span = data.ban.banExpiration - System.DateTime.UtcNow;
                    banBuilder.Append(span.Days);
                    banBuilder.Append("d. ");
                    banBuilder.Append(span.Hours);
                    banBuilder.Append("h. ");
                    banBuilder.Append(span.Minutes);
                    banBuilder.Append("m. ");


                    player.SilentKickInProgress = true;
                    player.SendData(PacketTypes.Status, banBuilder.ToString());
                    player.SendData(PacketTypes.Disconnect, banBuilder.ToString());
                }
            } catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
            }
}
    }
}
