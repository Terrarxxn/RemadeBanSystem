using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace BanSystem
{
    [ApiVersion(2, 1)]
    public class BansPlugin : TerrariaPlugin
    {
        public BansPlugin(Main game) : base(game)
        {
            base.Order = -int.MaxValue;
        }

        public override string Author => "bat627";
        public override string Name => "BanSystem";

        public override void Initialize()
        {
            string databasePath = Path.Combine(TShock.SavePath, "bans-db.sqlite");
            IDbConnection db = TShock.Config.Settings.StorageType == "mysql" ? TShock.DB : new SqliteConnection(string.Format($"uri=file://{databasePath},Version=3"));
            Database = new BansDB(db);

            Commands.ChatCommands.RemoveAll((Command p) => p.Name == "ban");

            _hooks = new PluginHooks(this);
            _hooks.InitializeHooks();

            ServerApi.Hooks.GamePostInitialize.Register(this, GamePostInitialize);
        }

        private void GamePostInitialize(EventArgs e)
        {
            _cmds = new PluginCommands();
            _cmds.Initialize();

            ServerApi.Hooks.GamePostInitialize.Deregister(this, GamePostInitialize);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _hooks.DeinitializeHooks();
            }
        }

        private PluginHooks _hooks;
        private PluginCommands _cmds;
        public static BansDB Database;
    }
}
