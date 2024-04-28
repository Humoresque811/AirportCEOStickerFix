using BepInEx.Configuration;

namespace AirportCEOStickerFix;

internal class AirportCEOStickerFixConfig
{
    internal static ConfigEntry<bool> UseScaleModule { get; set; }
    internal static ConfigEntry<bool> UseInfiniteSizeModule { get; set; }
    internal static ConfigEntry<bool> UseStickerLayerModule { get; set; }
    internal static ConfigEntry<bool> LoadFromWorkshop { get; set; }
    internal static ConfigEntry<bool> DebugLogs { get; set; }

    internal static void SetUpConfig()
    {
        UseScaleModule = AirportCEOStickerFix.ConfigReference.Bind("General", "Use Re-Scale Module", true, "Use the suggested sticker scale which optimizes sticker looks and removes bugs");
        UseInfiniteSizeModule = AirportCEOStickerFix.ConfigReference.Bind("General", "Use Infinite Sticker Size Module", true, "Allows you to place stickers as large as you want - may be dependency for sticker packs");
        UseStickerLayerModule = AirportCEOStickerFix.ConfigReference.Bind("General", "Use Sticker Layers Module", true, "Allows you to place stickers on top of each other");
        LoadFromWorkshop = AirportCEOStickerFix.ConfigReference.Bind("Workshop", "Load Stickers From Worshop", true, "Removes need to move stickers to the new location");
        DebugLogs = AirportCEOStickerFix.ConfigReference.Bind("Debug", "Log Debug Data", false, "Log debug/info data from this mod");
    }
}