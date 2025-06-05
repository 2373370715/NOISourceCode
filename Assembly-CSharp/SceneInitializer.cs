using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02001888 RID: 6280
public class SceneInitializer : MonoBehaviour
{
	// Token: 0x17000844 RID: 2116
	// (get) Token: 0x060081A2 RID: 33186 RVA: 0x000F9CA9 File Offset: 0x000F7EA9
	// (set) Token: 0x060081A3 RID: 33187 RVA: 0x000F9CB0 File Offset: 0x000F7EB0
	public static SceneInitializer Instance { get; private set; }

	// Token: 0x060081A4 RID: 33188 RVA: 0x00347040 File Offset: 0x00345240
	private void Awake()
	{
		Localization.SwapToLocalizedFont();
		string environmentVariable = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
		string text = Application.dataPath + Path.DirectorySeparatorChar.ToString() + "Plugins";
		if (!environmentVariable.Contains(text))
		{
			Environment.SetEnvironmentVariable("PATH", environmentVariable + Path.PathSeparator.ToString() + text, EnvironmentVariableTarget.Process);
		}
		SceneInitializer.Instance = this;
		this.PreLoadPrefabs();
	}

	// Token: 0x060081A5 RID: 33189 RVA: 0x000F9CB8 File Offset: 0x000F7EB8
	private void OnDestroy()
	{
		SceneInitializer.Instance = null;
	}

	// Token: 0x060081A6 RID: 33190 RVA: 0x003470B0 File Offset: 0x003452B0
	private void PreLoadPrefabs()
	{
		foreach (GameObject gameObject in this.preloadPrefabs)
		{
			if (gameObject != null)
			{
				Util.KInstantiate(gameObject, gameObject.transform.GetPosition(), Quaternion.identity, base.gameObject, null, true, 0);
			}
		}
	}

	// Token: 0x060081A7 RID: 33191 RVA: 0x000F9CC0 File Offset: 0x000F7EC0
	public void NewSaveGamePrefab()
	{
		if (this.prefab_NewSaveGame != null && SaveGame.Instance == null)
		{
			Util.KInstantiate(this.prefab_NewSaveGame, base.gameObject, null);
		}
	}

	// Token: 0x060081A8 RID: 33192 RVA: 0x00347128 File Offset: 0x00345328
	public void PostLoadPrefabs()
	{
		foreach (GameObject gameObject in this.prefabs)
		{
			if (gameObject != null)
			{
				Util.KInstantiate(gameObject, base.gameObject, null);
			}
		}
	}

	// Token: 0x04006299 RID: 25241
	public const int MAXDEPTH = -30000;

	// Token: 0x0400629A RID: 25242
	public const int SCREENDEPTH = -1000;

	// Token: 0x0400629C RID: 25244
	public GameObject prefab_NewSaveGame;

	// Token: 0x0400629D RID: 25245
	public List<GameObject> preloadPrefabs = new List<GameObject>();

	// Token: 0x0400629E RID: 25246
	public List<GameObject> prefabs = new List<GameObject>();
}
