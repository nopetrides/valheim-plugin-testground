
using System;
using System.Text.RegularExpressions;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;

namespace MyFirstPlugin
{
    
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess("valheim.exe")]
    public class Plugin : BaseUnityPlugin
    {
        internal const string LoggerName = "MyPluginLog";
        internal static ManualLogSource Log;
        internal static MyLogListener LogEcho;
        private ConfigEntry<bool> ShowLoadDoneMessage;
        private ConfigEntry<string> LoadDoneLogMessage;
        private void Awake()
        {
            Log = new MyManualLogger(LoggerName);
            BepInEx.Logging.Logger.Sources.Add(Log);
            LogEcho = new MyLogListener();
            BepInEx.Logging.Logger.Listeners.Add(LogEcho);
            // Plugin startup logic
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Log.LogInfo("This C# script was coded by Noah Petrides and is built with BepInEx");

            BindConfigs();

            if (ShowLoadDoneMessage.Value)
            {
                Log.LogInfo(LoadDoneLogMessage.Value);
            }
        }

        private void Update()
        {

        }

        private void BindConfigs()
        {
            ShowLoadDoneMessage = Config.Bind("Logging",             // The section under which the option is shown
                                              "ShouldShowLoadDoneMessage", // The key of the configuration option in the configuration file
                                              true,                  // The default value
                                              "Should the plugin show the load complete message"); // Description of the option to show in the config file);
            Log.LogInfo("ShouldShowLoadDoneMessage configured");
            LoadDoneLogMessage = Config.Bind("Logging",              // The section under which the option is shown
                                              "LoadDoneMessage", // The key of the configuration option in the configuration file
                                              "Huh, it worked",   // The default value
                                              "A fun message to show when the plugin is loaded"); // Description of the option to show in the config file);
            Log.LogInfo("LoadDoneMessage configured");
        }

    }

    internal class MyManualLogger : ManualLogSource
    {

        public MyManualLogger(string sourceName) : base(sourceName)
        {
            // no custom constructor logic yet
        }
    }

    internal class MyLogListener : ILogListener, IDisposable
    {
        internal bool WriteUnityLogs { get; set; } = true;

        public void LogEvent(object sender, LogEventArgs eventArgs)
        {
            if ((sender is MyManualLogger))
            {
                return;
            }

            if (ContainsSoughtInfo(eventArgs.ToString(), out string message))
            {
                Plugin.Log.LogInfo(message);
            }
        }

        public void Dispose()
        {
        }

        // Add to config
        private bool EchoLogs = true;
        private const string LogQueryDungeon = "Dungeon loaded *";
        private const string LogQuerySpawned = "Spawned ";
        private const string DungeonMessaage = "A Dungeon is nearby";
        private const string SpawnMessage = "{0} {1} appeared nearby";
        /// <summary>
        /// Check a log, and see if we want that info
        /// </summary>
        private bool ContainsSoughtInfo(string log, out string message)
        {
            message = "";
            if (!EchoLogs)
            {
                return false;
            }
            if (Regex.IsMatch(log, LogQueryDungeon))
            {
                message = DungeonMessaage;
                return true;
            }
            if (Regex.IsMatch(log, LogQuerySpawned))
            { 
                int mobNameStart = log.IndexOf(LogQuerySpawned)+LogQuerySpawned.Length;
                int mobNameEnd = log.IndexOf(" x ", mobNameStart);
                string mobName = log.Substring(mobNameStart, mobNameEnd - mobNameStart);
                int numberStart = log.LastIndexOf(" ")+1;
                string mobCount = log.Substring(numberStart).Trim('\r','\n');
                message = string.Format(SpawnMessage, mobCount, mobName);
                return true;
            }
            return false;
        }
    }
}
