using AirportCEOModLoader.Core;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AirportCEOStickerFix;

internal class StickerManager
{
    public static int StickerLayer = -50;
	public static List<string> filePaths = new List<string>();

	public static void AddStickers(string[] files)
	{
		filePaths.AddRange(files);
	}

    public static void ProccessStickers(SaveLoadGameDataController _) => ProccessStickers();
    public static void ProccessStickers()
    {
        foreach (string file in filePaths)
        {
			Sprite sprite = LoadImagePerformant(file);
			string stickerName = new FileInfo(file)?.Name?.Replace(".png", "");
			bool addSticker = true;

			foreach (Sticker sticker in DataPlaceholderItems.Instance.stickers)
			{
				if (sticker.name == null || !sticker.name.Equals(stickerName))
				{
					continue;
				}

				addSticker = false;
			}

			if (!addSticker)
			{
				continue;
			} 

			DataPlaceholderItems.Instance.stickers.Add(new Sticker
			{
				name = stickerName,
				sprite = sprite
			});
			AirportCEOStickerFix.LogInfo("Loaded a workshop sticker mod!");
        }

		AirportCEOStickerFix.LogInfo($"Loaded a total of {DataPlaceholderItems.Instance.stickers.Count} sticker mods, in this load cycle.");
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
}
