using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020015F9 RID: 5625
[SerializationConfig(MemberSerialization.OptIn)]
public class MutantPlant : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x17000775 RID: 1909
	// (get) Token: 0x06007489 RID: 29833 RVA: 0x000F0F83 File Offset: 0x000EF183
	public List<string> MutationIDs
	{
		get
		{
			return this.mutationIDs;
		}
	}

	// Token: 0x17000776 RID: 1910
	// (get) Token: 0x0600748A RID: 29834 RVA: 0x000F0F8B File Offset: 0x000EF18B
	public bool IsOriginal
	{
		get
		{
			return this.mutationIDs == null || this.mutationIDs.Count == 0;
		}
	}

	// Token: 0x17000777 RID: 1911
	// (get) Token: 0x0600748B RID: 29835 RVA: 0x000F0FA5 File Offset: 0x000EF1A5
	public bool IsIdentified
	{
		get
		{
			return this.analyzed && PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(this.SubSpeciesID);
		}
	}

	// Token: 0x17000778 RID: 1912
	// (get) Token: 0x0600748C RID: 29836 RVA: 0x000F0FC1 File Offset: 0x000EF1C1
	// (set) Token: 0x0600748D RID: 29837 RVA: 0x000F0FE4 File Offset: 0x000EF1E4
	public Tag SpeciesID
	{
		get
		{
			global::Debug.Assert(this.speciesID != null, "Ack, forgot to configure the species ID for this mutantPlant!");
			return this.speciesID;
		}
		set
		{
			this.speciesID = value;
		}
	}

	// Token: 0x17000779 RID: 1913
	// (get) Token: 0x0600748E RID: 29838 RVA: 0x000F0FED File Offset: 0x000EF1ED
	public Tag SubSpeciesID
	{
		get
		{
			if (this.cachedSubspeciesID == null)
			{
				this.cachedSubspeciesID = this.GetSubSpeciesInfo().ID;
			}
			return this.cachedSubspeciesID;
		}
	}

	// Token: 0x0600748F RID: 29839 RVA: 0x000F1019 File Offset: 0x000EF219
	protected override void OnPrefabInit()
	{
		base.Subscribe<MutantPlant>(-2064133523, MutantPlant.OnAbsorbDelegate);
		base.Subscribe<MutantPlant>(1335436905, MutantPlant.OnSplitFromChunkDelegate);
	}

	// Token: 0x06007490 RID: 29840 RVA: 0x0031296C File Offset: 0x00310B6C
	protected override void OnSpawn()
	{
		if (this.IsOriginal || this.HasTag(GameTags.Plant))
		{
			this.analyzed = true;
		}
		if (!this.IsOriginal)
		{
			this.AddTag(GameTags.MutatedSeed);
		}
		this.AddTag(this.SubSpeciesID);
		Components.MutantPlants.Add(this);
		base.OnSpawn();
		this.ApplyMutations();
		this.UpdateNameAndTags();
		PlantSubSpeciesCatalog.Instance.DiscoverSubSpecies(this.GetSubSpeciesInfo(), this);
	}

	// Token: 0x06007491 RID: 29841 RVA: 0x000F103D File Offset: 0x000EF23D
	protected override void OnCleanUp()
	{
		Components.MutantPlants.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06007492 RID: 29842 RVA: 0x003129E4 File Offset: 0x00310BE4
	private void OnAbsorb(object data)
	{
		MutantPlant component = (data as Pickupable).GetComponent<MutantPlant>();
		global::Debug.Assert(component != null && this.SubSpeciesID == component.SubSpeciesID, "Two seeds of different subspecies just absorbed!");
	}

	// Token: 0x06007493 RID: 29843 RVA: 0x00312A24 File Offset: 0x00310C24
	private void OnSplitFromChunk(object data)
	{
		MutantPlant component = (data as Pickupable).GetComponent<MutantPlant>();
		if (component != null)
		{
			component.CopyMutationsTo(this);
		}
	}

	// Token: 0x06007494 RID: 29844 RVA: 0x00312A50 File Offset: 0x00310C50
	public void Mutate()
	{
		List<string> list = (this.mutationIDs != null) ? new List<string>(this.mutationIDs) : new List<string>();
		while (list.Count >= 1 && list.Count > 0)
		{
			list.RemoveAt(UnityEngine.Random.Range(0, list.Count));
		}
		list.Add(Db.Get().PlantMutations.GetRandomMutation(this.PrefabID().Name).Id);
		this.SetSubSpecies(list);
	}

	// Token: 0x06007495 RID: 29845 RVA: 0x000F1050 File Offset: 0x000EF250
	public void Analyze()
	{
		this.analyzed = true;
		this.UpdateNameAndTags();
	}

	// Token: 0x06007496 RID: 29846 RVA: 0x00312AD0 File Offset: 0x00310CD0
	public void ApplyMutations()
	{
		if (this.IsOriginal)
		{
			return;
		}
		foreach (string id in this.mutationIDs)
		{
			Db.Get().PlantMutations.Get(id).ApplyTo(this);
		}
	}

	// Token: 0x06007497 RID: 29847 RVA: 0x000F105F File Offset: 0x000EF25F
	public void DummySetSubspecies(List<string> mutations)
	{
		this.mutationIDs = mutations;
	}

	// Token: 0x06007498 RID: 29848 RVA: 0x000F1068 File Offset: 0x000EF268
	public void SetSubSpecies(List<string> mutations)
	{
		if (base.gameObject.HasTag(this.SubSpeciesID))
		{
			base.gameObject.RemoveTag(this.SubSpeciesID);
		}
		this.cachedSubspeciesID = Tag.Invalid;
		this.mutationIDs = mutations;
		this.UpdateNameAndTags();
	}

	// Token: 0x06007499 RID: 29849 RVA: 0x000F10A6 File Offset: 0x000EF2A6
	public PlantSubSpeciesCatalog.SubSpeciesInfo GetSubSpeciesInfo()
	{
		return new PlantSubSpeciesCatalog.SubSpeciesInfo(this.SpeciesID, this.mutationIDs);
	}

	// Token: 0x0600749A RID: 29850 RVA: 0x000F10B9 File Offset: 0x000EF2B9
	public void CopyMutationsTo(MutantPlant target)
	{
		target.SetSubSpecies(this.mutationIDs);
		target.analyzed = this.analyzed;
	}

	// Token: 0x0600749B RID: 29851 RVA: 0x00312B3C File Offset: 0x00310D3C
	public void UpdateNameAndTags()
	{
		bool flag = !base.IsInitialized() || this.IsIdentified;
		bool flag2 = PlantSubSpeciesCatalog.Instance == null || PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(this.SpeciesID).Count == 1;
		KPrefabID component = base.GetComponent<KPrefabID>();
		component.AddTag(this.SubSpeciesID, false);
		component.SetTag(GameTags.UnidentifiedSeed, !flag);
		base.gameObject.name = component.PrefabTag.ToString() + " (" + this.SubSpeciesID.ToString() + ")";
		base.GetComponent<KSelectable>().SetName(this.GetSubSpeciesInfo().GetNameWithMutations(component.PrefabTag.ProperName(), flag, flag2));
		KSelectable component2 = base.GetComponent<KSelectable>();
		foreach (Guid guid in this.statusItemHandles)
		{
			component2.RemoveStatusItem(guid, false);
		}
		this.statusItemHandles.Clear();
		if (!flag2)
		{
			if (this.IsOriginal)
			{
				this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.OriginalPlantMutation, null));
				return;
			}
			if (!flag)
			{
				this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.UnknownMutation, null));
				return;
			}
			foreach (string id in this.mutationIDs)
			{
				this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.SpecificPlantMutation, Db.Get().PlantMutations.Get(id)));
			}
		}
	}

	// Token: 0x0600749C RID: 29852 RVA: 0x00312D30 File Offset: 0x00310F30
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.IsOriginal)
		{
			return null;
		}
		List<Descriptor> result = new List<Descriptor>();
		foreach (string id in this.mutationIDs)
		{
			Db.Get().PlantMutations.Get(id).GetDescriptors(ref result, go);
		}
		return result;
	}

	// Token: 0x0600749D RID: 29853 RVA: 0x00312DA8 File Offset: 0x00310FA8
	public List<string> GetSoundEvents()
	{
		List<string> list = new List<string>();
		if (this.mutationIDs != null)
		{
			foreach (string id in this.mutationIDs)
			{
				PlantMutation plantMutation = Db.Get().PlantMutations.Get(id);
				list.AddRange(plantMutation.AdditionalSoundEvents);
			}
		}
		return list;
	}

	// Token: 0x04005785 RID: 22405
	[Serialize]
	private bool analyzed;

	// Token: 0x04005786 RID: 22406
	[Serialize]
	private List<string> mutationIDs;

	// Token: 0x04005787 RID: 22407
	private List<Guid> statusItemHandles = new List<Guid>();

	// Token: 0x04005788 RID: 22408
	private const int MAX_MUTATIONS = 1;

	// Token: 0x04005789 RID: 22409
	[SerializeField]
	private Tag speciesID;

	// Token: 0x0400578A RID: 22410
	private Tag cachedSubspeciesID;

	// Token: 0x0400578B RID: 22411
	private static readonly EventSystem.IntraObjectHandler<MutantPlant> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<MutantPlant>(delegate(MutantPlant component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x0400578C RID: 22412
	private static readonly EventSystem.IntraObjectHandler<MutantPlant> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<MutantPlant>(delegate(MutantPlant component, object data)
	{
		component.OnSplitFromChunk(data);
	});
}
