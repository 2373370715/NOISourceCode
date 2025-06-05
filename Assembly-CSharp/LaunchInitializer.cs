using System;
using System.IO;
using System.Threading;
using UnityEngine;

// Token: 0x020014C5 RID: 5317
public class LaunchInitializer : MonoBehaviour
{
	// Token: 0x06006E15 RID: 28181 RVA: 0x000ECB20 File Offset: 0x000EAD20
	public static string BuildPrefix()
	{
		return LaunchInitializer.BUILD_PREFIX;
	}

	// Token: 0x06006E16 RID: 28182 RVA: 0x000ECB27 File Offset: 0x000EAD27
	public static int UpdateNumber()
	{
		return 55;
	}

	// Token: 0x06006E17 RID: 28183 RVA: 0x002FC1DC File Offset: 0x002FA3DC
	private void Update()
	{
		if (this.numWaitFrames > Time.renderedFrameCount)
		{
			return;
		}
		if (!DistributionPlatform.Initialized)
		{
			if (!SystemInfo.SupportsTextureFormat(TextureFormat.RGBAFloat))
			{
				global::Debug.LogError("Machine does not support RGBAFloat32");
			}
			GraphicsOptionsScreen.SetSettingsFromPrefs();
			Util.ApplyInvariantCultureToThread(Thread.CurrentThread);
			global::Debug.Log("Date: " + System.DateTime.Now.ToString());
			global::Debug.Log("Build: " + BuildWatermark.GetBuildText() + " (release)");
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			KPlayerPrefs.instance.Load();
			DistributionPlatform.Initialize();
		}
		if (!DistributionPlatform.Inst.IsDLCStatusReady())
		{
			return;
		}
		global::Debug.Log("DistributionPlatform initialized.");
		DebugUtil.LogArgs(new object[]
		{
			DebugUtil.LINE
		});
		global::Debug.Log("Build: " + BuildWatermark.GetBuildText() + " (release)");
		DebugUtil.LogArgs(new object[]
		{
			DebugUtil.LINE
		});
		DebugUtil.LogArgs(new object[]
		{
			"DLC Information"
		});
		foreach (string text in DlcManager.GetOwnedDLCIds())
		{
			global::Debug.Log(string.Format("- {0} loaded: {1}", text, DlcManager.IsContentSubscribed(text)));
		}
		DebugUtil.LogArgs(new object[]
		{
			DebugUtil.LINE
		});
		KFMOD.Initialize();
		for (int i = 0; i < this.SpawnPrefabs.Length; i++)
		{
			GameObject gameObject = this.SpawnPrefabs[i];
			if (gameObject != null)
			{
				Util.KInstantiate(gameObject, base.gameObject, null);
			}
		}
		LaunchInitializer.DeleteLingeringFiles();
		base.enabled = false;
	}

	// Token: 0x06006E18 RID: 28184 RVA: 0x002FC38C File Offset: 0x002FA58C
	private static void DeleteLingeringFiles()
	{
		string[] array = new string[]
		{
			"fmod.log",
			"load_stats_0.json",
			"OxygenNotIncluded_Data/output_log.txt"
		};
		string directoryName = Path.GetDirectoryName(Application.dataPath);
		foreach (string path in array)
		{
			string path2 = Path.Combine(directoryName, path);
			try
			{
				if (File.Exists(path2))
				{
					File.Delete(path2);
				}
			}
			catch (Exception obj)
			{
				global::Debug.LogWarning(obj);
			}
		}
	}

	// Token: 0x04005303 RID: 21251
	private const string PREFIX = "U";

	// Token: 0x04005304 RID: 21252
	private const int UPDATE_NUMBER = 55;

	// Token: 0x04005305 RID: 21253
	private static readonly string BUILD_PREFIX = "U" + 55.ToString();

	// Token: 0x04005306 RID: 21254
	public GameObject[] SpawnPrefabs;

	// Token: 0x04005307 RID: 21255
	[SerializeField]
	private int numWaitFrames = 1;
}
