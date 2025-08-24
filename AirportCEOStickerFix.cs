using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace AirportCEOStickerFix;

[BepInPlugin("org.airportCEOStickerFix.humoresque", MODNAME, PluginInfo.PLUGIN_VERSION)]
public class AirportCEOStickerFix : BaseUnityPlugin
{
    public static AirportCEOStickerFix Instance { get; private set; }
    internal static Harmony Harmony { get; private set; }
    internal static ManualLogSource SFLogger { get; private set; }
    internal static ConfigFile ConfigReference {  get; private set; }

    internal const string MODNAME = "AirportCEO Sticker Fix";

    private void Awake()
    {
        Harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        Harmony.PatchAll(); 

        // Plugin startup logic
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        Instance = this;
        SFLogger = Logger;
        ConfigReference = Config;

        Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} is setting up config.");
        AirportCEOStickerFixConfig.SetUpConfig();
        Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} finished setting up config.");

        Logger.LogInfo("Finished Awake");
    }

    private void Start()
    {
        Logger.LogInfo("Started Start");

        AirportCEOModLoader.WatermarkUtils.WatermarkUtils.Register(new AirportCEOModLoader.WatermarkUtils.WatermarkInfo("SF", "1.4.3", true));
        AirportCEOModLoader.WorkshopUtils.WorkshopUtils.Register("Stickers", StickerManager.ProcessWorkshopMod);
        AirportCEOModLoader.SaveLoadUtils.CoroutineEventDispatcher.RegisterToLaunchGamePhase(StickerManager.AddStickersInGame, AirportCEOModLoader.SaveLoadUtils.CoroutineEventDispatcher.CoroutineAttachmentType.After);

        AirportCEOStickerFixConfig.UseInfiniteSizeModule.SettingChanged += StickerManager.UpdateUnlimitedSizeValue;

        Logger.LogInfo("Finished Start");
    }

    internal static void LogInfo(string message) => SFLogger.LogInfo(message);
    internal static void LogError(string message) => SFLogger.LogError(message);
}
