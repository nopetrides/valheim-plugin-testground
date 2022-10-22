using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;

namespace MyFirstPlugin
{
    
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess("valheim.exe")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;

        private ConfigEntry<bool> ShowLoadDoneMessage;
        private ConfigEntry<string> LoadDoneLogMessage;
        private void Awake()
        {
            Log = BepInEx.Logging.Logger.CreateLogSource("MyPluginLog");
            // Plugin startup logic
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Log.LogInfo("This C# script was coded by Noah Petrides and is built with BepInEx");

            BindConfigs();

            if (ShowLoadDoneMessage.Value)
            {
                Log.LogInfo(LoadDoneLogMessage.Value);
            }
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
}
