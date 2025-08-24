using AirportCEOModLoader.Core;
using AirportCEOModLoader.SaveLoadUtils;
using AirportCEOModLoader.WorkshopUtils;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;
using System.Collections.Concurrent;

namespace AirportCEOStickerFix;

internal class StickerManager
{
    internal static int StickerLayer = -50;
	internal static List<Sticker> cachedStickers = new();

    internal static IEnumerator ProcessWorkshopMod(string extendedPath)
    {
        if (!AirportCEOStickerFixConfig.LoadFromWorkshop.Value)
        {
            yield break;
        }

        CoroutineEventDispatcher.GetTextUpdater()($"{AirportCEOStickerFix.MODNAME}: Reading Textures", 0);
        yield return null;

		List<string> filePaths = new List<string>();
        string[] files = Directory.GetFiles(extendedPath, "*.png");
        if (files.Length <= 0)
        {
            yield break;
        }

        filePaths.AddRange(files);

		int fileNumber = filePaths.Count;
		float percentagePerFile = 100f / (float)fileNumber;
		float currentCounter = 0;

        foreach (string file in filePaths)
        {
			CoroutineEventDispatcher.GetTextUpdater()($"{AirportCEOStickerFix.MODNAME}: Reading Textures", currentCounter.Clamp(0f, 100f).RoundToIntLikeANormalPerson());
			yield return null;
			currentCounter += percentagePerFile;

			Sprite sprite = LoadImagePerformant(file);
			string stickerName = new FileInfo(file)?.Name?.Replace(".png", "");
			bool addSticker = true;

			if (!addSticker)
			{
				continue;
			} 

			cachedStickers.Add(new Sticker
			{
				name = stickerName,
				sprite = sprite
			});
        }
		yield return null;
    }

	internal static IEnumerator AddStickersInGame()
	{
		foreach (Sticker newSticker in cachedStickers)
		{
			bool addSticker = true;
			foreach (Sticker existingSticker in DataPlaceholderItems.Instance.stickers)
			{
				if (existingSticker.name != null && existingSticker.name.Equals(newSticker.name))
				{
					addSticker = false;
				}
			}

			if (addSticker)
			{
				DataPlaceholderItems.Instance.stickers.Add(newSticker);
			}
		}
		yield break;
	}

	// This is written to replace the games LoadImage function to add the apply set.
    private static Sprite LoadImagePerformant(string filePath)
	{
		Sprite result = null;
		try
		{
			if (File.Exists(filePath))
			{
				byte[] data = File.ReadAllBytes(filePath);
				Texture2D texture2D = new Texture2D(2, 2, TextureFormat.ARGB32, true)
				{
					filterMode = FilterMode.Bilinear
				};
				texture2D.LoadImage(data);
				if (GameSettingManager.CompressImages)
				{
					texture2D.Compress(highQuality: true);
				}
				texture2D.Apply(true, true); // NEW
				result = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), Vector2.one / 2f, 100, 0u, SpriteMeshType.FullRect);
			}
		}
		catch (Exception ex)
		{
			AirportCEOStickerFix.LogError($"Failed to create a sticker sprite! {ExceptionUtils.ProccessException(ex)}");
		}
		return result;
	}

	internal static void UpdateUnlimitedSizeValue(object _, System.EventArgs __)
	{
		
        if (AirportCEOStickerFixConfig.UseInfiniteSizeModule.Value)
        {
            GameSettingManager.GameSettings.unlimitedLogoSize = true;
        }
        else
        {
            GameSettingManager.GameSettings.unlimitedLogoSize = false;
        }
	}

}
