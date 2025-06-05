using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000435 RID: 1077
public class ArtifactPOIConfig : IMultiEntityConfig
{
	// Token: 0x06001201 RID: 4609 RVA: 0x00191CF0 File Offset: 0x0018FEF0
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (ArtifactPOIConfig.ArtifactPOIParams artifactPOIParams in this.GenerateConfigs())
		{
			list.Add(ArtifactPOIConfig.CreateArtifactPOI(artifactPOIParams.id, artifactPOIParams.anim, Strings.Get(artifactPOIParams.nameStringKey), Strings.Get(artifactPOIParams.descStringKey), artifactPOIParams.poiType.idHash));
		}
		return list;
	}

	// Token: 0x06001202 RID: 4610 RVA: 0x00191D88 File Offset: 0x0018FF88
	public static GameObject CreateArtifactPOI(string id, string anim, string name, string desc, HashedString poiType)
	{
		GameObject gameObject = EntityTemplates.CreateEntity(id, id, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<ArtifactPOIConfigurator>().presetType = poiType;
		ArtifactPOIClusterGridEntity artifactPOIClusterGridEntity = gameObject.AddOrGet<ArtifactPOIClusterGridEntity>();
		artifactPOIClusterGridEntity.m_name = name;
		artifactPOIClusterGridEntity.m_Anim = anim;
		gameObject.AddOrGetDef<ArtifactPOIStates.Def>();
		LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.UnlockNextSpaceEntry));
		return gameObject;
	}

	// Token: 0x06001203 RID: 4611 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001204 RID: 4612 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06001205 RID: 4613 RVA: 0x00191DE0 File Offset: 0x0018FFE0
	private List<ArtifactPOIConfig.ArtifactPOIParams> GenerateConfigs()
	{
		List<ArtifactPOIConfig.ArtifactPOIParams> list = new List<ArtifactPOIConfig.ArtifactPOIParams>();
		if (!DlcManager.IsExpansion1Active())
		{
			return list;
		}
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_1", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation1", null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_2", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation2", null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_3", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation3", null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_4", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation4", null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_5", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation5", null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_6", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation6", null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_7", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation7", null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("station_8", new ArtifactPOIConfigurator.ArtifactPOIType("GravitasSpaceStation8", null, false, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.Add(new ArtifactPOIConfig.ArtifactPOIParams("russels_teapot", new ArtifactPOIConfigurator.ArtifactPOIType("RussellsTeapot", "artifact_TeaPot", true, 30000f, 60000f, DlcManager.EXPANSION1, null)));
		list.RemoveAll((ArtifactPOIConfig.ArtifactPOIParams poi) => !DlcManager.IsCorrectDlcSubscribed(poi.poiType));
		return list;
	}

	// Token: 0x04000C8B RID: 3211
	public const string GravitasSpaceStation1 = "GravitasSpaceStation1";

	// Token: 0x04000C8C RID: 3212
	public const string GravitasSpaceStation2 = "GravitasSpaceStation2";

	// Token: 0x04000C8D RID: 3213
	public const string GravitasSpaceStation3 = "GravitasSpaceStation3";

	// Token: 0x04000C8E RID: 3214
	public const string GravitasSpaceStation4 = "GravitasSpaceStation4";

	// Token: 0x04000C8F RID: 3215
	public const string GravitasSpaceStation5 = "GravitasSpaceStation5";

	// Token: 0x04000C90 RID: 3216
	public const string GravitasSpaceStation6 = "GravitasSpaceStation6";

	// Token: 0x04000C91 RID: 3217
	public const string GravitasSpaceStation7 = "GravitasSpaceStation7";

	// Token: 0x04000C92 RID: 3218
	public const string GravitasSpaceStation8 = "GravitasSpaceStation8";

	// Token: 0x04000C93 RID: 3219
	public const string RussellsTeapot = "RussellsTeapot";

	// Token: 0x02000436 RID: 1078
	public struct ArtifactPOIParams
	{
		// Token: 0x06001207 RID: 4615 RVA: 0x00191FB4 File Offset: 0x001901B4
		public ArtifactPOIParams(string anim, ArtifactPOIConfigurator.ArtifactPOIType poiType)
		{
			this.id = "ArtifactSpacePOI_" + poiType.id;
			this.anim = anim;
			this.nameStringKey = new StringKey("STRINGS.UI.SPACEDESTINATIONS.ARTIFACT_POI." + poiType.id.ToUpper() + ".NAME");
			this.descStringKey = new StringKey("STRINGS.UI.SPACEDESTINATIONS.ARTIFACT_POI." + poiType.id.ToUpper() + ".DESC");
			this.poiType = poiType;
		}

		// Token: 0x04000C94 RID: 3220
		public string id;

		// Token: 0x04000C95 RID: 3221
		public string anim;

		// Token: 0x04000C96 RID: 3222
		public StringKey nameStringKey;

		// Token: 0x04000C97 RID: 3223
		public StringKey descStringKey;

		// Token: 0x04000C98 RID: 3224
		public ArtifactPOIConfigurator.ArtifactPOIType poiType;
	}
}
