using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02001E9B RID: 7835
public class MotdDataFetchRequest : IDisposable
{
	// Token: 0x0600A437 RID: 42039 RVA: 0x0010EFFD File Offset: 0x0010D1FD
	public void Dispose()
	{
		this.onCompleteFn = null;
	}

	// Token: 0x0600A438 RID: 42040 RVA: 0x0010F006 File Offset: 0x0010D206
	public void Fetch(string url)
	{
		MotdDataFetchRequest.FetchWebMotdJson(url, delegate(MotdData webMotd)
		{
			this.data = webMotd;
			if (webMotd == null)
			{
				global::Debug.LogWarning("MOTD Error: failed to get web motd json");
				this.<Fetch>g__CompleteWith|4_1(null);
				return;
			}
			MotdDataFetchRequest.FetchWebMotdImagesFor(webMotd, delegate(bool isOk)
			{
				using (List<MotdData_Box>.Enumerator enumerator = webMotd.boxesLive.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.resolvedImage.IsNullOrDestroyed())
						{
							isOk = false;
						}
					}
				}
				if (!isOk)
				{
					global::Debug.LogWarning("MOTD Error: couldn't fetch all web motd images");
					this.<Fetch>g__CompleteWith|4_1(null);
					return;
				}
				MotdDataFetchRequest.WriteCachedMotdImages(webMotd);
				this.<Fetch>g__CompleteWith|4_1(webMotd);
			});
		});
	}

	// Token: 0x0600A439 RID: 42041 RVA: 0x0010F01A File Offset: 0x0010D21A
	public void OnComplete(Action<MotdData> callbackFn)
	{
		if (this.isComplete)
		{
			callbackFn(this.data);
			return;
		}
		this.onCompleteFn = (Action<MotdData>)Delegate.Combine(this.onCompleteFn, callbackFn);
	}

	// Token: 0x0600A43A RID: 42042 RVA: 0x003F48D4 File Offset: 0x003F2AD4
	public static void FetchWebMotdJson(string url, Action<MotdData> onCompleteFn)
	{
		UnityWebRequest webRequest = UnityWebRequest.Get(url);
		webRequest.timeout = 3;
		webRequest.SetRequestHeader("Content-Type", "application/json");
		webRequest.SendWebRequest().completed += delegate(AsyncOperation operation)
		{
			if (string.IsNullOrEmpty(webRequest.error))
			{
				onCompleteFn(MotdData.Parse(webRequest.downloadHandler.text));
			}
			else
			{
				global::Debug.LogWarning("MOTD Error: failed to fetch web motd. " + webRequest.error);
				onCompleteFn(null);
			}
			webRequest.Dispose();
		};
	}

	// Token: 0x0600A43B RID: 42043 RVA: 0x003F4938 File Offset: 0x003F2B38
	public static void FetchWebMotdImagesFor(MotdData motdData, Action<bool> onCompleteFn)
	{
		using (List<MotdData_Box>.Enumerator enumerator = motdData.boxesLive.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.image == null)
				{
					onCompleteFn(false);
					return;
				}
			}
		}
		int imagesToFetchCount = motdData.boxesLive.Count;
		if (imagesToFetchCount == 0)
		{
			onCompleteFn(false);
			return;
		}
		int imagesValidCount = 0;
		using (List<MotdData_Box>.Enumerator enumerator = motdData.boxesLive.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MotdData_Box box = enumerator.Current;
				MotdDataFetchRequest.FetchWebMotdImage(box.image, delegate(Texture2D resolvedImage, bool isFromDisk)
				{
					imagesToFetchCount--;
					box.resolvedImage = resolvedImage;
					box.resolvedImageIsFromDisk = isFromDisk;
					if (box.resolvedImage != null)
					{
						imagesValidCount++;
					}
					if (imagesToFetchCount == 0)
					{
						onCompleteFn(imagesValidCount == motdData.boxesLive.Count);
					}
				});
			}
		}
	}

	// Token: 0x0600A43C RID: 42044 RVA: 0x003F4A54 File Offset: 0x003F2C54
	public static void FetchWebMotdImage(string url, Action<Texture2D, bool> onCompleteFn)
	{
		Texture2D texture2D = MotdDataFetchRequest.ReadCachedMotdImage(url);
		if (texture2D != null)
		{
			onCompleteFn(texture2D, true);
			return;
		}
		UnityWebRequest webRequest = UnityWebRequest.Get(url);
		webRequest.timeout = 3;
		webRequest.SendWebRequest().completed += delegate(AsyncOperation operation)
		{
			if (string.IsNullOrEmpty(webRequest.error))
			{
				onCompleteFn(MotdDataFetchRequest.ParseImage(webRequest.downloadHandler.data), false);
			}
			else
			{
				global::Debug.LogWarning("MOTD Error: failed to fetch web image at " + url + ". " + webRequest.error);
				onCompleteFn(null, false);
			}
			webRequest.Dispose();
		};
	}

	// Token: 0x0600A43D RID: 42045 RVA: 0x0010F048 File Offset: 0x0010D248
	public static string GetCachePath()
	{
		return Path.Combine(Util.CacheFolder(), "motd");
	}

	// Token: 0x0600A43E RID: 42046 RVA: 0x0010F059 File Offset: 0x0010D259
	public static string GetCachedFilePath(string filePath)
	{
		return Path.Combine(Util.CacheFolder(), "motd", Path.GetFileName(filePath));
	}

	// Token: 0x0600A43F RID: 42047 RVA: 0x003F4AD4 File Offset: 0x003F2CD4
	public static void WriteCachedMotdImages(MotdData data)
	{
		if (data == null)
		{
			return;
		}
		try
		{
			if (!Directory.Exists(MotdDataFetchRequest.GetCachePath()))
			{
				Directory.CreateDirectory(MotdDataFetchRequest.GetCachePath());
			}
		}
		catch (Exception arg)
		{
			global::Debug.LogWarning(string.Format("MOTD Error: Failed to create image cache directory --- {0}", arg));
		}
		try
		{
			if (Directory.Exists(MotdDataFetchRequest.GetCachePath()))
			{
				using (List<MotdData_Box>.Enumerator enumerator = data.boxesLive.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MotdData_Box motdData_Box = enumerator.Current;
						if (motdData_Box.image != null && motdData_Box.resolvedImage != null && !motdData_Box.resolvedImageIsFromDisk)
						{
							File.WriteAllBytes(MotdDataFetchRequest.GetCachedFilePath(motdData_Box.image), motdData_Box.resolvedImage.EncodeToPNG());
						}
					}
					goto IL_B0;
				}
			}
			global::Debug.LogWarning("MOTD Error: Failed to write cached motd images, couldn't find a valid cache directory");
			IL_B0:;
		}
		catch (Exception arg2)
		{
			global::Debug.LogWarning(string.Format("MOTD Error: Failed to write cached motd images --- {0}", arg2));
		}
		try
		{
			if (Directory.Exists(MotdDataFetchRequest.GetCachePath()))
			{
				List<string> list = new List<string>(16);
				foreach (MotdData_Box motdData_Box2 in data.boxesLive)
				{
					if (motdData_Box2.image != null)
					{
						list.Add(MotdDataFetchRequest.GetCachedFilePath(motdData_Box2.image));
					}
				}
				foreach (string text in Directory.GetFiles(MotdDataFetchRequest.GetCachePath()))
				{
					if (!list.Contains(MotdDataFetchRequest.GetCachedFilePath(text)))
					{
						File.Delete(text);
					}
				}
			}
			else
			{
				global::Debug.LogWarning("MOTD Error: Failed to clean cached motd images, couldn't find a valid cache directory");
			}
		}
		catch (Exception arg3)
		{
			global::Debug.LogWarning(string.Format("MOTD Error: Failed to clean cached motd images --- {0}", arg3));
		}
	}

	// Token: 0x0600A440 RID: 42048 RVA: 0x003F4CAC File Offset: 0x003F2EAC
	public static Texture2D ReadCachedMotdImage(string url)
	{
		string fileName = Path.GetFileName(url);
		string cachedFilePath = MotdDataFetchRequest.GetCachedFilePath(fileName);
		if (!File.Exists(cachedFilePath))
		{
			return null;
		}
		Texture2D result;
		try
		{
			result = MotdDataFetchRequest.ParseImage(File.ReadAllBytes(cachedFilePath));
		}
		catch (Exception arg)
		{
			global::Debug.LogWarning(string.Format("MOTD Error: Can't load cached motd image \"{0}\" --- {1}", fileName, arg));
			result = null;
		}
		return result;
	}

	// Token: 0x0600A441 RID: 42049 RVA: 0x003F4D08 File Offset: 0x003F2F08
	public static string GetLocaleCode()
	{
		Localization.Locale locale = Localization.GetLocale();
		if (locale != null)
		{
			Localization.Language lang = locale.Lang;
			if (lang == Localization.Language.Chinese || lang - Localization.Language.Korean <= 1)
			{
				return locale.Code;
			}
		}
		return null;
	}

	// Token: 0x0600A442 RID: 42050 RVA: 0x003F4D38 File Offset: 0x003F2F38
	public static Texture2D ParseImage(byte[] buffer)
	{
		if (MotdDataFetchRequest.<ParseImage>g__IsPng|14_0(buffer) || MotdDataFetchRequest.<ParseImage>g__IsJpg|14_1(buffer))
		{
			Texture2D texture2D = new Texture2D(0, 0);
			texture2D.LoadImage(buffer);
			return texture2D;
		}
		if (MotdDataFetchRequest.<ParseImage>g__IsKleiTex|14_2(buffer))
		{
			global::Debug.LogWarning("MOTD Error: Couldn't load image - KTEX isn't supported yet.");
			return null;
		}
		global::Debug.LogWarning("MOTD Error: Couldn't load image - Unsupported image file format.");
		return null;
	}

	// Token: 0x0600A443 RID: 42051 RVA: 0x003F4D84 File Offset: 0x003F2F84
	public static void GetUrlParams(out string platformCode, out string languageCode)
	{
		platformCode = "default";
		if ((((Localization.GetLocale() == null) ? null : new Localization.Language?(Localization.GetLocale().Lang)) ?? Localization.Language.Japanese) == Localization.Language.Chinese)
		{
			languageCode = "schinese";
			return;
		}
		languageCode = "en";
	}

	// Token: 0x0600A444 RID: 42052 RVA: 0x003F4DDC File Offset: 0x003F2FDC
	public static string BuildUrl()
	{
		string str;
		string str2;
		MotdDataFetchRequest.GetUrlParams(out str, out str2);
		return "https://motd.klei.com/motd.json/?game=oni&platform=" + str + "&lang=" + str2;
	}

	// Token: 0x0600A447 RID: 42055 RVA: 0x0010F070 File Offset: 0x0010D270
	[CompilerGenerated]
	private void <Fetch>g__CompleteWith|4_1(MotdData data)
	{
		if (this.isComplete)
		{
			return;
		}
		this.isComplete = true;
		this.data = data;
		if (this.onCompleteFn != null)
		{
			this.onCompleteFn(data);
		}
	}

	// Token: 0x0600A448 RID: 42056 RVA: 0x0010F09D File Offset: 0x0010D29D
	[CompilerGenerated]
	internal static bool <ParseImage>g__IsPng|14_0(byte[] buffer)
	{
		return buffer[0] == 137 && buffer[1] == 80 && buffer[2] == 78 && buffer[3] == 71 && buffer[4] == 13 && buffer[5] == 10 && buffer[6] == 26 && buffer[7] == 10;
	}

	// Token: 0x0600A449 RID: 42057 RVA: 0x0010F0DC File Offset: 0x0010D2DC
	[CompilerGenerated]
	internal static bool <ParseImage>g__IsJpg|14_1(byte[] buffer)
	{
		return buffer[0] == byte.MaxValue && buffer[1] == 216 && buffer[6] == 74 && buffer[7] == 70 && buffer[8] == 73 && buffer[9] == 70;
	}

	// Token: 0x0600A44A RID: 42058 RVA: 0x0010F111 File Offset: 0x0010D311
	[CompilerGenerated]
	internal static bool <ParseImage>g__IsKleiTex|14_2(byte[] buffer)
	{
		return buffer[0] == 75 && buffer[1] == 84 && buffer[2] == 69 && buffer[3] == 88;
	}

	// Token: 0x0400806A RID: 32874
	private MotdData data;

	// Token: 0x0400806B RID: 32875
	private bool isComplete;

	// Token: 0x0400806C RID: 32876
	private Action<MotdData> onCompleteFn;
}
