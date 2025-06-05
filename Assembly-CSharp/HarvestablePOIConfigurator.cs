using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200193B RID: 6459
[AddComponentMenu("KMonoBehaviour/scripts/HarvestablePOIConfigurator")]
public class HarvestablePOIConfigurator : KMonoBehaviour
{
	// Token: 0x0600866E RID: 34414 RVA: 0x00359F9C File Offset: 0x0035819C
	public static HarvestablePOIConfigurator.HarvestablePOIType FindType(HashedString typeId)
	{
		HarvestablePOIConfigurator.HarvestablePOIType harvestablePOIType = null;
		if (typeId != HashedString.Invalid)
		{
			harvestablePOIType = HarvestablePOIConfigurator._poiTypes.Find((HarvestablePOIConfigurator.HarvestablePOIType t) => t.id == typeId);
		}
		if (harvestablePOIType == null)
		{
			global::Debug.LogError(string.Format("Tried finding a harvestable poi with id {0} but it doesn't exist!", typeId.ToString()));
		}
		return harvestablePOIType;
	}

	// Token: 0x0600866F RID: 34415 RVA: 0x000FCBE1 File Offset: 0x000FADE1
	public HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration MakeConfiguration()
	{
		return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
	}

	// Token: 0x06008670 RID: 34416 RVA: 0x0035A008 File Offset: 0x00358208
	private HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		int globalWorldSeed = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
		ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
		Vector3 position = ClusterGrid.Instance.GetPosition(component);
		KRandom randomSource = new KRandom(globalWorldSeed + (int)position.x + (int)position.y);
		return new HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration
		{
			typeId = typeId,
			capacityRoll = this.Roll(randomSource, min, max),
			rechargeRoll = this.Roll(randomSource, min, max)
		};
	}

	// Token: 0x06008671 RID: 34417 RVA: 0x000E82B4 File Offset: 0x000E64B4
	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}

	// Token: 0x040065E2 RID: 26082
	private static List<HarvestablePOIConfigurator.HarvestablePOIType> _poiTypes;

	// Token: 0x040065E3 RID: 26083
	public HashedString presetType;

	// Token: 0x040065E4 RID: 26084
	public float presetMin;

	// Token: 0x040065E5 RID: 26085
	public float presetMax = 1f;

	// Token: 0x0200193C RID: 6460
	public class HarvestablePOIType : IHasDlcRestrictions
	{
		// Token: 0x06008673 RID: 34419 RVA: 0x000FCC0E File Offset: 0x000FAE0E
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x06008674 RID: 34420 RVA: 0x000FCC16 File Offset: 0x000FAE16
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x06008675 RID: 34421 RVA: 0x0035A078 File Offset: 0x00358278
		public HarvestablePOIType(string id, Dictionary<SimHashes, float> harvestableElements, float poiCapacityMin = 54000f, float poiCapacityMax = 81000f, float poiRechargeMin = 30000f, float poiRechargeMax = 60000f, bool canProvideArtifacts = true, List<string> orbitalObject = null, int maxNumOrbitingObjects = 20, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			this.id = id;
			this.idHash = id;
			this.harvestableElements = harvestableElements;
			this.poiCapacityMin = poiCapacityMin;
			this.poiCapacityMax = poiCapacityMax;
			this.poiRechargeMin = poiRechargeMin;
			this.poiRechargeMax = poiRechargeMax;
			this.canProvideArtifacts = canProvideArtifacts;
			this.orbitalObject = orbitalObject;
			this.maxNumOrbitingObjects = maxNumOrbitingObjects;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
			if (HarvestablePOIConfigurator._poiTypes == null)
			{
				HarvestablePOIConfigurator._poiTypes = new List<HarvestablePOIConfigurator.HarvestablePOIType>();
			}
			HarvestablePOIConfigurator._poiTypes.Add(this);
		}

		// Token: 0x06008676 RID: 34422 RVA: 0x0035A108 File Offset: 0x00358308
		[Obsolete]
		public HarvestablePOIType(string id, Dictionary<SimHashes, float> harvestableElements, float poiCapacityMin = 54000f, float poiCapacityMax = 81000f, float poiRechargeMin = 30000f, float poiRechargeMax = 60000f, bool canProvideArtifacts = true, List<string> orbitalObject = null, int maxNumOrbitingObjects = 20, string dlcID = "EXPANSION1_ID") : this(id, harvestableElements, poiCapacityMin, poiCapacityMax, poiRechargeMin, poiRechargeMax, canProvideArtifacts, orbitalObject, maxNumOrbitingObjects, null, null)
		{
			this.requiredDlcIds = DlcManager.EXPANSION1;
		}

		// Token: 0x040065E6 RID: 26086
		public string id;

		// Token: 0x040065E7 RID: 26087
		public HashedString idHash;

		// Token: 0x040065E8 RID: 26088
		public Dictionary<SimHashes, float> harvestableElements;

		// Token: 0x040065E9 RID: 26089
		public float poiCapacityMin;

		// Token: 0x040065EA RID: 26090
		public float poiCapacityMax;

		// Token: 0x040065EB RID: 26091
		public float poiRechargeMin;

		// Token: 0x040065EC RID: 26092
		public float poiRechargeMax;

		// Token: 0x040065ED RID: 26093
		public bool canProvideArtifacts;

		// Token: 0x040065EE RID: 26094
		[Obsolete]
		public string dlcID;

		// Token: 0x040065EF RID: 26095
		public string[] requiredDlcIds;

		// Token: 0x040065F0 RID: 26096
		public string[] forbiddenDlcIds;

		// Token: 0x040065F1 RID: 26097
		public List<string> orbitalObject;

		// Token: 0x040065F2 RID: 26098
		public int maxNumOrbitingObjects;
	}

	// Token: 0x0200193D RID: 6461
	[Serializable]
	public class HarvestablePOIInstanceConfiguration
	{
		// Token: 0x06008677 RID: 34423 RVA: 0x0035A138 File Offset: 0x00358338
		private void Init()
		{
			if (this.didInit)
			{
				return;
			}
			this.didInit = true;
			this.poiTotalCapacity = MathUtil.ReRange(this.capacityRoll, 0f, 1f, this.poiType.poiCapacityMin, this.poiType.poiCapacityMax);
			this.poiRecharge = MathUtil.ReRange(this.rechargeRoll, 0f, 1f, this.poiType.poiRechargeMin, this.poiType.poiRechargeMax);
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06008678 RID: 34424 RVA: 0x000FCC1E File Offset: 0x000FAE1E
		public HarvestablePOIConfigurator.HarvestablePOIType poiType
		{
			get
			{
				return HarvestablePOIConfigurator.FindType(this.typeId);
			}
		}

		// Token: 0x06008679 RID: 34425 RVA: 0x000FCC2B File Offset: 0x000FAE2B
		public Dictionary<SimHashes, float> GetElementsWithWeights()
		{
			this.Init();
			return this.poiType.harvestableElements;
		}

		// Token: 0x0600867A RID: 34426 RVA: 0x000FCC3E File Offset: 0x000FAE3E
		public bool CanProvideArtifacts()
		{
			this.Init();
			return this.poiType.canProvideArtifacts;
		}

		// Token: 0x0600867B RID: 34427 RVA: 0x000FCC51 File Offset: 0x000FAE51
		public float GetMaxCapacity()
		{
			this.Init();
			return this.poiTotalCapacity;
		}

		// Token: 0x0600867C RID: 34428 RVA: 0x000FCC5F File Offset: 0x000FAE5F
		public float GetRechargeTime()
		{
			this.Init();
			return this.poiRecharge;
		}

		// Token: 0x040065F3 RID: 26099
		public HashedString typeId;

		// Token: 0x040065F4 RID: 26100
		private bool didInit;

		// Token: 0x040065F5 RID: 26101
		public float capacityRoll;

		// Token: 0x040065F6 RID: 26102
		public float rechargeRoll;

		// Token: 0x040065F7 RID: 26103
		private float poiTotalCapacity;

		// Token: 0x040065F8 RID: 26104
		private float poiRecharge;
	}
}
