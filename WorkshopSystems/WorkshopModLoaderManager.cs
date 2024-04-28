using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportCEOModLoader.Core;
using AirportCEOModLoader.SaveLoadUtils;
using AirportCEOModLoader.WorkshopUtils;

namespace AirportCEOStickerFix;

internal class WorkshopModLoaderManager
{
    internal static void SetUpModLoaderInteractions()
    {
        WorkshopUtils.Register("Stickers", ProccessWorkshopMod);
        EventDispatcher.EndOfLoad += StickerManager.ProccessStickers;
    }

    private static void ProccessWorkshopMod(string extendedPath)
    {
        if (!Directory.Exists(extendedPath))
        {
            AirportCEOStickerFix.LogError("Sticker path provided does not exist!");
            return;
        }
        if (!AirportCEOStickerFixConfig.LoadFromWorkshop.Value)
        {
            return;
        }

        try
        {
            string[] files = Directory.GetFiles(extendedPath, "*.png");
            if (files.Length <= 0)
            {
                return;
            }

            StickerManager.AddStickers(files);
        }
        catch (Exception ex)
        {
            AirportCEOStickerFix.LogError($"Error while proccessing sticker mod. {ExceptionUtils.ProccessException(ex)}");
        }
    }
}
