using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001905 RID: 6405
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactPOIConfigurator")]
public class ArtifactPOIConfigurator : KMonoBehaviour
{
	// Token: 0x06008492 RID: 33938 RVA: 0x00352F60 File Offset: 0x00351160
	public static ArtifactPOIConfigurator.ArtifactPOIType FindType(HashedString typeId)
	{
		ArtifactPOIConfigurator.ArtifactPOIType artifactPOIType = null;
		if (typeId != HashedString.Invalid)
		{
			artifactPOIType = ArtifactPOIConfigurator._poiTypes.Find((ArtifactPOIConfigurator.ArtifactPOIType t) => t.id == typeId);
		}
		if (artifactPOIType == null)
		{
			global::Debug.LogError(string.Format("Tried finding a harvestable poi with id {0} but it doesn't exist!", typeId.ToString()));
		}
		return artifactPOIType;
	}

	// Token: 0x06008493 RID: 33939 RVA: 0x000FB96F File Offset: 0x000F9B6F
	public ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration MakeConfiguration()
	{
		return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
	}

	// Token: 0x06008494 RID: 33940 RVA: 0x00352FCC File Offset: 0x003511CC
	private ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		int globalWorldSeed = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
		ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
		Vector3 position = ClusterGrid.Instance.GetPosition(component);
		KRandom randomSource = new KRandom(globalWorldSeed + (int)position.x + (int)position.y);
		return new ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration
		{
			typeId = typeId,
			rechargeRoll = this.Roll(randomSource, min, max)
		};
	}

	// Token: 0x06008495 RID: 33941 RVA: 0x000E82B4 File Offset: 0x000E64B4
	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}

	// Token: 0x040064EF RID: 25839
	private static List<ArtifactPOIConfigurator.ArtifactPOIType> _poiTypes;

	// Token: 0x040064F0 RID: 25840
	public static ArtifactPOIConfigurator.ArtifactPOIType defaultArtifactPoiType = new ArtifactPOIConfigurator.ArtifactPOIType("HarvestablePOIArtifacts", null, false, 30000f, 60000f, DlcManager.EXPANSION1, null);

	// Token: 0x040064F1 RID: 25841
	public HashedString presetType;

	// Token: 0x040064F2 RID: 25842
	public float presetMin;

	// Token: 0x040064F3 RID: 25843
	public float presetMax = 1f;

	// Token: 0x02001906 RID: 6406
	public class ArtifactPOIType : IHasDlcRestrictions
	{
		// Token: 0x06008498 RID: 33944 RVA: 0x000FB9BF File Offset: 0x000F9BBF
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x06008499 RID: 33945 RVA: 0x000FB9C7 File Offset: 0x000F9BC7
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x0600849A RID: 33946 RVA: 0x0035302C File Offset: 0x0035122C
		public ArtifactPOIType(string id, string harvestableArtifactID = null, bool destroyOnHarvest = false, float poiRechargeTimeMin = 30000f, float poiRechargeTimeMax = 60000f, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			this.id = id;
			this.idHash = id;
			this.harvestableArtifactID = harvestableArtifactID;
			this.destroyOnHarvest = destroyOnHarvest;
			this.poiRechargeTimeMin = poiRechargeTimeMin;
			this.poiRechargeTimeMax = poiRechargeTimeMax;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
			if (ArtifactPOIConfigurator._poiTypes == null)
			{
				ArtifactPOIConfigurator._poiTypes = new List<ArtifactPOIConfigurator.ArtifactPOIType>();
			}
			ArtifactPOIConfigurator._poiTypes.Add(this);
		}

		// Token: 0x0600849B RID: 33947 RVA: 0x003530C4 File Offset: 0x003512C4
		[Obsolete]
		public ArtifactPOIType(string id, string harvestableArtifactID = null, bool destroyOnHarvest = false, float poiRechargeTimeMin = 30000f, float poiRechargeTimeMax = 60000f, string dlcID = "EXPANSION1_ID")
		{
			this.id = id;
			this.idHash = id;
			this.harvestableArtifactID = harvestableArtifactID;
			this.destroyOnHarvest = destroyOnHarvest;
			this.poiRechargeTimeMin = poiRechargeTimeMin;
			this.poiRechargeTimeMax = poiRechargeTimeMax;
			this.dlcID = dlcID;
			if (ArtifactPOIConfigurator._poiTypes == null)
			{
				ArtifactPOIConfigurator._poiTypes = new List<ArtifactPOIConfigurator.ArtifactPOIType>();
			}
			ArtifactPOIConfigurator._poiTypes.Add(this);
		}

		// Token: 0x040064F4 RID: 25844
		public string id;

		// Token: 0x040064F5 RID: 25845
		public HashedString idHash;

		// Token: 0x040064F6 RID: 25846
		public string harvestableArtifactID;

		// Token: 0x040064F7 RID: 25847
		public bool destroyOnHarvest;

		// Token: 0x040064F8 RID: 25848
		public float poiRechargeTimeMin;

		// Token: 0x040064F9 RID: 25849
		public float poiRechargeTimeMax;

		// Token: 0x040064FA RID: 25850
		[Obsolete]
		public string dlcID;

		// Token: 0x040064FB RID: 25851
		public string[] requiredDlcIds;

		// Token: 0x040064FC RID: 25852
		public string[] forbiddenDlcIds;

		// Token: 0x040064FD RID: 25853
		public List<string> orbitalObject = new List<string>
		{
			Db.Get().OrbitalTypeCategories.gravitas.Id
		};
	}

	// Token: 0x02001907 RID: 6407
	[Serializable]
	public class ArtifactPOIInstanceConfiguration
	{
		// Token: 0x0600849C RID: 33948 RVA: 0x00353154 File Offset: 0x00351354
		private void Init()
		{
			if (this.didInit)
			{
				return;
			}
			this.didInit = true;
			this.poiRechargeTime = MathUtil.ReRange(this.rechargeRoll, 0f, 1f, this.poiType.poiRechargeTimeMin, this.poiType.poiRechargeTimeMax);
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x0600849D RID: 33949 RVA: 0x000FB9CF File Offset: 0x000F9BCF
		public ArtifactPOIConfigurator.ArtifactPOIType poiType
		{
			get
			{
				return ArtifactPOIConfigurator.FindType(this.typeId);
			}
		}

		// Token: 0x0600849E RID: 33950 RVA: 0x000FB9DC File Offset: 0x000F9BDC
		public bool DestroyOnHarvest()
		{
			this.Init();
			return this.poiType.destroyOnHarvest;
		}

		// Token: 0x0600849F RID: 33951 RVA: 0x000FB9EF File Offset: 0x000F9BEF
		public string GetArtifactID()
		{
			this.Init();
			return this.poiType.harvestableArtifactID;
		}

		// Token: 0x060084A0 RID: 33952 RVA: 0x000FBA02 File Offset: 0x000F9C02
		public float GetRechargeTime()
		{
			this.Init();
			return this.poiRechargeTime;
		}

		// Token: 0x040064FE RID: 25854
		public HashedString typeId;

		// Token: 0x040064FF RID: 25855
		private bool didInit;

		// Token: 0x04006500 RID: 25856
		public float rechargeRoll;

		// Token: 0x04006501 RID: 25857
		private float poiRechargeTime;
	}
}
