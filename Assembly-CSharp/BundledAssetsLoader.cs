using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200108D RID: 4237
public class BundledAssetsLoader : KMonoBehaviour
{
	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x0600560F RID: 22031 RVA: 0x000DC8A8 File Offset: 0x000DAAA8
	// (set) Token: 0x06005610 RID: 22032 RVA: 0x000DC8B0 File Offset: 0x000DAAB0
	public BundledAssets Expansion1Assets { get; private set; }

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x06005611 RID: 22033 RVA: 0x000DC8B9 File Offset: 0x000DAAB9
	// (set) Token: 0x06005612 RID: 22034 RVA: 0x000DC8C1 File Offset: 0x000DAAC1
	public List<BundledAssets> DlcAssetsList { get; private set; }

	// Token: 0x06005613 RID: 22035 RVA: 0x0028E954 File Offset: 0x0028CB54
	protected override void OnPrefabInit()
	{
		BundledAssetsLoader.instance = this;
		if (DlcManager.IsExpansion1Active())
		{
			global::Debug.Log("Loading Expansion1 assets from bundle");
			AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, DlcManager.GetContentBundleName("EXPANSION1_ID")));
			global::Debug.Assert(assetBundle != null, "Expansion1 is Active but its asset bundle failed to load");
			GameObject gameObject = assetBundle.LoadAsset<GameObject>("Expansion1Assets");
			global::Debug.Assert(gameObject != null, "Could not load the Expansion1Assets prefab");
			this.Expansion1Assets = Util.KInstantiate(gameObject, base.gameObject, null).GetComponent<BundledAssets>();
		}
		this.DlcAssetsList = new List<BundledAssets>(DlcManager.DLC_PACKS.Count);
		foreach (KeyValuePair<string, DlcManager.DlcInfo> keyValuePair in DlcManager.DLC_PACKS)
		{
			if (DlcManager.IsContentSubscribed(keyValuePair.Key))
			{
				global::Debug.Log("Loading DLC " + keyValuePair.Key + " assets from bundle");
				AssetBundle assetBundle2 = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, DlcManager.GetContentBundleName(keyValuePair.Key)));
				global::Debug.Assert(assetBundle2 != null, "DLC " + keyValuePair.Key + " is Active but its asset bundle failed to load");
				GameObject gameObject2 = assetBundle2.LoadAsset<GameObject>(keyValuePair.Value.directory + "Assets");
				global::Debug.Assert(gameObject2 != null, "Could not load the " + keyValuePair.Key + " prefab");
				this.DlcAssetsList.Add(Util.KInstantiate(gameObject2, base.gameObject, null).GetComponent<BundledAssets>());
			}
		}
	}

	// Token: 0x04003CE7 RID: 15591
	public static BundledAssetsLoader instance;
}
