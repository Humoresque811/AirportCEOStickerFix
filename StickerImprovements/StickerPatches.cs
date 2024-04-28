using HarmonyLib;
using UnityEngine;

namespace AirportCEOStickerFix.StickerImprovements;

[HarmonyPatch]
static class StickerPatches
{
    [HarmonyPatch(typeof(PlaceableLogotype), "ChangeToPlaced")]
    [HarmonyPostfix]
    public static void LayerPatch(PlaceableLogotype __instance)
    {
        if (!AirportCEOStickerFixConfig.UseStickerLayerModule.Value)
        {
            return;
        }

        StickerManager.StickerLayer += 1;

        if (StickerManager.StickerLayer >= 30000)
        {
            StickerManager.StickerLayer = -50;
        }

        __instance.variationSpriteRenderer.sortingOrder = StickerManager.StickerLayer;

        if (AirportCEOStickerFixConfig.DebugLogs.Value)
        {
            AirportCEOStickerFix.LogInfo("Layer name is " + __instance.variationSpriteRenderer.sortingLayerName);
            AirportCEOStickerFix.LogInfo("Layer order is " + __instance.variationSpriteRenderer.sortingOrder);
        }
    }

    [HarmonyPatch(typeof(PlaceableLogotype), "SetCorrectLogoSize")]
    [HarmonyPrefix]
    public static void ScalePatch(PlaceableLogotype __instance)
    {
        if (!AirportCEOStickerFixConfig.UseScaleModule.Value)
        {
            return;
        }

        float LogoScaleX = __instance.variationSpriteRenderer.transform.localScale.x;
        float LogoScaleY = __instance.variationSpriteRenderer.transform.localScale.y;

        float ELogoScaleX = (float)(LogoScaleX / 0.4 * 0.39081);
        float ELogoScaleY = (float)(LogoScaleY / 0.4 * 0.39081);

        __instance.variationSpriteRenderer.transform.localScale = new Vector3(ELogoScaleX, ELogoScaleY, 1f);

        if (AirportCEOStickerFixConfig.DebugLogs.Value)
        {
            AirportCEOStickerFix.LogInfo("Sticker size ajusted. The postion of the sticker was " + __instance.transform.position);
        }
    }

    [HarmonyPatch(typeof(DataPlaceholderItems), nameof(DataPlaceholderItems.LoadStickers))]
    [HarmonyPostfix]
    public static void LoadStickerPatch()
    {
        StickerManager.ProccessStickers();
    }
}
