using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using UnityEngine;

// Token: 0x02000CF2 RID: 3314
[SerializationConfig(MemberSerialization.OptIn)]
public class BuildingFacade : KMonoBehaviour
{
	// Token: 0x17000305 RID: 773
	// (get) Token: 0x06003F96 RID: 16278 RVA: 0x000CDC3C File Offset: 0x000CBE3C
	public string CurrentFacade
	{
		get
		{
			return this.currentFacade;
		}
	}

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x06003F97 RID: 16279 RVA: 0x000CDC44 File Offset: 0x000CBE44
	public bool IsOriginal
	{
		get
		{
			return this.currentFacade.IsNullOrWhiteSpace();
		}
	}

	// Token: 0x06003F98 RID: 16280 RVA: 0x000AA038 File Offset: 0x000A8238
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x06003F99 RID: 16281 RVA: 0x000CDC51 File Offset: 0x000CBE51
	protected override void OnSpawn()
	{
		if (!this.IsOriginal)
		{
			this.ApplyBuildingFacade(Db.GetBuildingFacades().TryGet(this.currentFacade), false);
		}
	}

	// Token: 0x06003F9A RID: 16282 RVA: 0x000CDC72 File Offset: 0x000CBE72
	public void ApplyDefaultFacade(bool shouldTryAnimate = false)
	{
		this.currentFacade = "DEFAULT_FACADE";
		this.ClearFacade(shouldTryAnimate);
	}

	// Token: 0x06003F9B RID: 16283 RVA: 0x00246050 File Offset: 0x00244250
	public void ApplyBuildingFacade(BuildingFacadeResource facade, bool shouldTryAnimate = false)
	{
		if (facade == null)
		{
			this.ClearFacade(false);
			return;
		}
		this.currentFacade = facade.Id;
		KAnimFile[] array = new KAnimFile[]
		{
			Assets.GetAnim(facade.AnimFile)
		};
		this.ChangeBuilding(array, facade.Name, facade.Description, facade.InteractFile, shouldTryAnimate);
	}

	// Token: 0x06003F9C RID: 16284 RVA: 0x002460A8 File Offset: 0x002442A8
	private void ClearFacade(bool shouldTryAnimate = false)
	{
		Building component = base.GetComponent<Building>();
		this.ChangeBuilding(component.Def.AnimFiles, component.Def.Name, component.Def.Desc, null, shouldTryAnimate);
	}

	// Token: 0x06003F9D RID: 16285 RVA: 0x002460E8 File Offset: 0x002442E8
	private void ChangeBuilding(KAnimFile[] animFiles, string displayName, string desc, Dictionary<string, string> interactAnimsNames = null, bool shouldTryAnimate = false)
	{
		this.interactAnims.Clear();
		if (interactAnimsNames != null && interactAnimsNames.Count > 0)
		{
			this.interactAnims = new Dictionary<string, KAnimFile[]>();
			foreach (KeyValuePair<string, string> keyValuePair in interactAnimsNames)
			{
				this.interactAnims.Add(keyValuePair.Key, new KAnimFile[]
				{
					Assets.GetAnim(keyValuePair.Value)
				});
			}
		}
		Building[] components = base.GetComponents<Building>();
		foreach (Building building in components)
		{
			building.SetDescriptionFlavour(desc);
			KBatchedAnimController component = building.GetComponent<KBatchedAnimController>();
			HashedString batchGroupID = component.batchGroupID;
			component.SwapAnims(animFiles);
			foreach (KBatchedAnimController kbatchedAnimController in building.GetComponentsInChildren<KBatchedAnimController>(true))
			{
				if (kbatchedAnimController.batchGroupID == batchGroupID)
				{
					kbatchedAnimController.SwapAnims(animFiles);
				}
			}
			if (!this.animateIn.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(this.animateIn);
				this.animateIn = null;
			}
			if (shouldTryAnimate)
			{
				this.animateIn = BuildingFacadeAnimateIn.MakeFor(component);
				string parameter = "Unlocked";
				float parameterValue = 1f;
				KFMOD.PlayUISoundWithParameter(GlobalAssets.GetSound(KleiInventoryScreen.GetFacadeItemSoundName(Db.Get().Permits.TryGet(this.currentFacade)) + "_Click", false), parameter, parameterValue);
			}
		}
		base.GetComponent<KSelectable>().SetName(displayName);
		if (base.GetComponent<AnimTileable>() != null && components.Length != 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(components[0].GetExtents(), GameScenePartitioner.Instance.objectLayers[1], null);
		}
	}

	// Token: 0x06003F9E RID: 16286 RVA: 0x002462B0 File Offset: 0x002444B0
	public string GetNextFacade()
	{
		BuildingDef def = base.GetComponent<Building>().Def;
		int num = def.AvailableFacades.FindIndex((string s) => s == this.currentFacade) + 1;
		if (num >= def.AvailableFacades.Count)
		{
			num = 0;
		}
		return def.AvailableFacades[num];
	}

	// Token: 0x04002BED RID: 11245
	[Serialize]
	private string currentFacade;

	// Token: 0x04002BEE RID: 11246
	public KAnimFile[] animFiles;

	// Token: 0x04002BEF RID: 11247
	public Dictionary<string, KAnimFile[]> interactAnims = new Dictionary<string, KAnimFile[]>();

	// Token: 0x04002BF0 RID: 11248
	private BuildingFacadeAnimateIn animateIn;
}
