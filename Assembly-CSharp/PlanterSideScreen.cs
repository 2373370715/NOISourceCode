using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002009 RID: 8201
public class PlanterSideScreen : ReceptacleSideScreen
{
	// Token: 0x17000B1E RID: 2846
	// (get) Token: 0x0600AD73 RID: 44403 RVA: 0x004226AC File Offset: 0x004208AC
	// (set) Token: 0x0600AD74 RID: 44404 RVA: 0x00422704 File Offset: 0x00420904
	private Tag selectedSubspecies
	{
		get
		{
			if (!this.entityPreviousSubSelectionMap.ContainsKey((PlantablePlot)this.targetReceptacle))
			{
				this.entityPreviousSubSelectionMap.Add((PlantablePlot)this.targetReceptacle, Tag.Invalid);
			}
			return this.entityPreviousSubSelectionMap[(PlantablePlot)this.targetReceptacle];
		}
		set
		{
			if (!this.entityPreviousSubSelectionMap.ContainsKey((PlantablePlot)this.targetReceptacle))
			{
				this.entityPreviousSubSelectionMap.Add((PlantablePlot)this.targetReceptacle, Tag.Invalid);
			}
			this.entityPreviousSubSelectionMap[(PlantablePlot)this.targetReceptacle] = value;
			this.selectedDepositObjectAdditionalTag = value;
		}
	}

	// Token: 0x0600AD75 RID: 44405 RVA: 0x00422764 File Offset: 0x00420964
	private void LoadTargetSubSpeciesRequest()
	{
		PlantablePlot plantablePlot = (PlantablePlot)this.targetReceptacle;
		Tag tag = Tag.Invalid;
		if (plantablePlot.requestedEntityTag != Tag.Invalid && plantablePlot.requestedEntityTag != GameTags.Empty)
		{
			tag = plantablePlot.requestedEntityTag;
		}
		else if (this.selectedEntityToggle != null)
		{
			tag = this.depositObjectMap[this.selectedEntityToggle].tag;
		}
		if (DlcManager.FeaturePlantMutationsEnabled() && tag.IsValid)
		{
			MutantPlant component = Assets.GetPrefab(tag).GetComponent<MutantPlant>();
			if (component == null)
			{
				this.selectedSubspecies = Tag.Invalid;
				return;
			}
			if (plantablePlot.requestedEntityAdditionalFilterTag != Tag.Invalid && plantablePlot.requestedEntityAdditionalFilterTag != GameTags.Empty)
			{
				this.selectedSubspecies = plantablePlot.requestedEntityAdditionalFilterTag;
				return;
			}
			if (this.selectedSubspecies == Tag.Invalid)
			{
				PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo;
				if (PlantSubSpeciesCatalog.Instance.GetOriginalSubSpecies(component.SpeciesID, out subSpeciesInfo))
				{
					this.selectedSubspecies = subSpeciesInfo.ID;
				}
				plantablePlot.requestedEntityAdditionalFilterTag = this.selectedSubspecies;
			}
		}
	}

	// Token: 0x0600AD76 RID: 44406 RVA: 0x00115303 File Offset: 0x00113503
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<PlantablePlot>() != null;
	}

	// Token: 0x0600AD77 RID: 44407 RVA: 0x00115311 File Offset: 0x00113511
	protected override void ToggleClicked(ReceptacleToggle toggle)
	{
		this.LoadTargetSubSpeciesRequest();
		base.ToggleClicked(toggle);
		this.UpdateState(null);
	}

	// Token: 0x0600AD78 RID: 44408 RVA: 0x00115327 File Offset: 0x00113527
	protected void MutationToggleClicked(GameObject toggle)
	{
		this.selectedSubspecies = this.subspeciesToggles[toggle];
		this.UpdateState(null);
	}

	// Token: 0x0600AD79 RID: 44409 RVA: 0x00115342 File Offset: 0x00113542
	protected override void UpdateState(object data)
	{
		base.UpdateState(data);
		this.requestSelectedEntityBtn.onClick += this.RefreshSubspeciesToggles;
		this.RefreshSubspeciesToggles();
	}

	// Token: 0x0600AD7A RID: 44410 RVA: 0x00115368 File Offset: 0x00113568
	private IEnumerator ExpandMutations()
	{
		LayoutElement le = this.mutationViewport.GetComponent<LayoutElement>();
		float num = 94f;
		float travelPerSecond = num / 0.33f;
		while (le.minHeight < 118f)
		{
			float num2 = le.minHeight;
			float num3 = Time.unscaledDeltaTime * travelPerSecond;
			num2 = Mathf.Min(num2 + num3, 118f);
			le.minHeight = num2;
			le.preferredHeight = num2;
			yield return new WaitForEndOfFrame();
		}
		this.mutationPanelCollapsed = false;
		this.activeAnimationRoutine = null;
		yield return 0;
		yield break;
	}

	// Token: 0x0600AD7B RID: 44411 RVA: 0x00115377 File Offset: 0x00113577
	private IEnumerator CollapseMutations()
	{
		LayoutElement le = this.mutationViewport.GetComponent<LayoutElement>();
		float num = -94f;
		float travelPerSecond = num / 0.33f;
		while (le.minHeight > 24f)
		{
			float num2 = le.minHeight;
			float num3 = Time.unscaledDeltaTime * travelPerSecond;
			num2 = Mathf.Max(num2 + num3, 24f);
			le.minHeight = num2;
			le.preferredHeight = num2;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.mutationPanelCollapsed = true;
		this.activeAnimationRoutine = null;
		yield return SequenceUtil.WaitForNextFrame;
		yield break;
	}

	// Token: 0x0600AD7C RID: 44412 RVA: 0x0042287C File Offset: 0x00420A7C
	private void RefreshSubspeciesToggles()
	{
		foreach (KeyValuePair<GameObject, Tag> keyValuePair in this.subspeciesToggles)
		{
			UnityEngine.Object.Destroy(keyValuePair.Key);
		}
		this.subspeciesToggles.Clear();
		if (!PlantSubSpeciesCatalog.Instance.AnyNonOriginalDiscovered)
		{
			this.mutationPanel.SetActive(false);
			return;
		}
		this.mutationPanel.SetActive(true);
		foreach (GameObject obj in this.blankMutationObjects)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.blankMutationObjects.Clear();
		this.selectSpeciesPrompt.SetActive(false);
		if (this.selectedDepositObjectTag.IsValid)
		{
			Tag plantID = Assets.GetPrefab(this.selectedDepositObjectTag).GetComponent<PlantableSeed>().PlantID;
			List<PlantSubSpeciesCatalog.SubSpeciesInfo> allSubSpeciesForSpecies = PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(plantID);
			if (allSubSpeciesForSpecies != null)
			{
				foreach (PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo in allSubSpeciesForSpecies)
				{
					GameObject option = Util.KInstantiateUI(this.mutationOption, this.mutationContainer, true);
					option.GetComponentInChildren<LocText>().text = subSpeciesInfo.GetNameWithMutations(plantID.ProperName(), PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(subSpeciesInfo.ID), false);
					MultiToggle component = option.GetComponent<MultiToggle>();
					component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
					{
						this.MutationToggleClicked(option);
					}));
					option.GetComponent<ToolTip>().SetSimpleTooltip(subSpeciesInfo.GetMutationsTooltip());
					this.subspeciesToggles.Add(option, subSpeciesInfo.ID);
				}
				for (int i = allSubSpeciesForSpecies.Count; i < 5; i++)
				{
					this.blankMutationObjects.Add(Util.KInstantiateUI(this.blankMutationOption, this.mutationContainer, true));
				}
				if (!this.selectedSubspecies.IsValid || !this.subspeciesToggles.ContainsValue(this.selectedSubspecies))
				{
					this.selectedSubspecies = allSubSpeciesForSpecies[0].ID;
				}
			}
		}
		else
		{
			this.selectSpeciesPrompt.SetActive(true);
		}
		ICollection<Pickupable> collection = new List<Pickupable>();
		bool flag = base.CheckReceptacleOccupied();
		bool flag2 = this.targetReceptacle.GetActiveRequest != null;
		WorldContainer myWorld = this.targetReceptacle.GetMyWorld();
		collection = myWorld.worldInventory.GetPickupables(this.selectedDepositObjectTag, myWorld.IsModuleInterior);
		foreach (KeyValuePair<GameObject, Tag> keyValuePair2 in this.subspeciesToggles)
		{
			float num = 0f;
			bool flag3 = PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(keyValuePair2.Value);
			if (collection != null)
			{
				foreach (Pickupable pickupable in collection)
				{
					if (pickupable.HasTag(keyValuePair2.Value))
					{
						num += pickupable.GetComponent<PrimaryElement>().Units;
					}
				}
			}
			if (num > 0f && flag3)
			{
				keyValuePair2.Key.GetComponent<MultiToggle>().ChangeState((keyValuePair2.Value == this.selectedSubspecies) ? 1 : 0);
			}
			else
			{
				keyValuePair2.Key.GetComponent<MultiToggle>().ChangeState((keyValuePair2.Value == this.selectedSubspecies) ? 3 : 2);
			}
			LocText componentInChildren = keyValuePair2.Key.GetComponentInChildren<LocText>();
			componentInChildren.text += string.Format(" ({0})", num);
			if (flag || flag2)
			{
				if (keyValuePair2.Value == this.selectedSubspecies)
				{
					keyValuePair2.Key.SetActive(true);
					keyValuePair2.Key.GetComponent<MultiToggle>().ChangeState(1);
				}
				else
				{
					keyValuePair2.Key.SetActive(false);
				}
			}
			else
			{
				keyValuePair2.Key.SetActive(this.selectedEntityToggle != null);
			}
		}
		bool flag4 = !flag && !flag2 && this.selectedEntityToggle != null && this.subspeciesToggles.Count >= 1;
		if (flag4 && this.mutationPanelCollapsed)
		{
			if (this.activeAnimationRoutine != null)
			{
				base.StopCoroutine(this.activeAnimationRoutine);
			}
			this.activeAnimationRoutine = base.StartCoroutine(this.ExpandMutations());
			return;
		}
		if (!flag4 && !this.mutationPanelCollapsed)
		{
			if (this.activeAnimationRoutine != null)
			{
				base.StopCoroutine(this.activeAnimationRoutine);
			}
			this.activeAnimationRoutine = base.StartCoroutine(this.CollapseMutations());
		}
	}

	// Token: 0x0600AD7D RID: 44413 RVA: 0x00422DBC File Offset: 0x00420FBC
	protected override Sprite GetEntityIcon(Tag prefabTag)
	{
		PlantableSeed component = Assets.GetPrefab(prefabTag).GetComponent<PlantableSeed>();
		if (component != null)
		{
			return base.GetEntityIcon(new Tag(component.PlantID));
		}
		return base.GetEntityIcon(prefabTag);
	}

	// Token: 0x0600AD7E RID: 44414 RVA: 0x00422DF8 File Offset: 0x00420FF8
	protected override string GetEntityName(Tag prefabTag)
	{
		PlantableSeed component = Assets.GetPrefab(prefabTag).GetComponent<PlantableSeed>();
		if (component != null)
		{
			return Assets.GetPrefab(component.PlantID).GetProperName();
		}
		return base.GetEntityName(prefabTag);
	}

	// Token: 0x0600AD7F RID: 44415 RVA: 0x00422E34 File Offset: 0x00421034
	protected override string GetEntityTooltip(Tag prefabTag)
	{
		PlantableSeed component = Assets.GetPrefab(prefabTag).GetComponent<PlantableSeed>();
		return string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANT_TOGGLE_TOOLTIP, this.GetEntityName(prefabTag), component.domesticatedDescription, base.GetAvailableAmount(prefabTag));
	}

	// Token: 0x0600AD80 RID: 44416 RVA: 0x00422E78 File Offset: 0x00421078
	protected override void SetResultDescriptions(GameObject seed_or_plant)
	{
		string text = "";
		GameObject gameObject = seed_or_plant;
		PlantableSeed component = seed_or_plant.GetComponent<PlantableSeed>();
		List<Descriptor> list = new List<Descriptor>();
		bool flag = true;
		if (seed_or_plant.GetComponent<MutantPlant>() != null && this.selectedDepositObjectAdditionalTag != Tag.Invalid)
		{
			flag = PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(this.selectedDepositObjectAdditionalTag);
		}
		if (!flag)
		{
			text = CREATURES.PLANT_MUTATIONS.UNIDENTIFIED + "\n\n" + CREATURES.PLANT_MUTATIONS.UNIDENTIFIED_DESC;
		}
		else if (component != null)
		{
			list = component.GetDescriptors(component.gameObject);
			if (this.targetReceptacle.rotatable != null && this.targetReceptacle.Direction != component.direction)
			{
				if (component.direction == SingleEntityReceptacle.ReceptacleDirection.Top)
				{
					text += UI.UISIDESCREENS.PLANTERSIDESCREEN.ROTATION_NEED_FLOOR;
				}
				else if (component.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
				{
					text += UI.UISIDESCREENS.PLANTERSIDESCREEN.ROTATION_NEED_WALL;
				}
				else if (component.direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
				{
					text += UI.UISIDESCREENS.PLANTERSIDESCREEN.ROTATION_NEED_CEILING;
				}
				text += "\n\n";
			}
			gameObject = Assets.GetPrefab(component.PlantID);
			MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
			if (component2 != null && this.selectedDepositObjectAdditionalTag.IsValid)
			{
				PlantSubSpeciesCatalog.SubSpeciesInfo subSpecies = PlantSubSpeciesCatalog.Instance.GetSubSpecies(component.PlantID, this.selectedDepositObjectAdditionalTag);
				component2.DummySetSubspecies(subSpecies.mutationIDs);
			}
			if (!string.IsNullOrEmpty(component.domesticatedDescription))
			{
				text += component.domesticatedDescription;
			}
		}
		else
		{
			InfoDescription component3 = gameObject.GetComponent<InfoDescription>();
			if (component3)
			{
				text += component3.description;
			}
		}
		this.descriptionLabel.SetText(text);
		List<Descriptor> plantLifeCycleDescriptors = GameUtil.GetPlantLifeCycleDescriptors(gameObject);
		if (plantLifeCycleDescriptors.Count > 0 && flag)
		{
			this.HarvestDescriptorPanel.SetDescriptors(plantLifeCycleDescriptors);
			this.HarvestDescriptorPanel.gameObject.SetActive(true);
		}
		else
		{
			this.HarvestDescriptorPanel.gameObject.SetActive(false);
		}
		List<Descriptor> plantRequirementDescriptors = GameUtil.GetPlantRequirementDescriptors(gameObject);
		if (list.Count > 0)
		{
			GameUtil.IndentListOfDescriptors(list, 1);
			plantRequirementDescriptors.InsertRange(plantRequirementDescriptors.Count, list);
		}
		if (plantRequirementDescriptors.Count > 0 && flag)
		{
			this.RequirementsDescriptorPanel.SetDescriptors(plantRequirementDescriptors);
			this.RequirementsDescriptorPanel.gameObject.SetActive(true);
		}
		else
		{
			this.RequirementsDescriptorPanel.gameObject.SetActive(false);
		}
		List<Descriptor> plantEffectDescriptors = GameUtil.GetPlantEffectDescriptors(gameObject);
		if (plantEffectDescriptors.Count > 0 && flag)
		{
			this.EffectsDescriptorPanel.SetDescriptors(plantEffectDescriptors);
			this.EffectsDescriptorPanel.gameObject.SetActive(true);
			return;
		}
		this.EffectsDescriptorPanel.gameObject.SetActive(false);
	}

	// Token: 0x0600AD81 RID: 44417 RVA: 0x00423120 File Offset: 0x00421320
	protected override bool AdditionalCanDepositTest()
	{
		bool flag = false;
		if (this.selectedDepositObjectTag.IsValid)
		{
			if (DlcManager.FeaturePlantMutationsEnabled())
			{
				flag = PlantSubSpeciesCatalog.Instance.IsValidPlantableSeed(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag);
			}
			else
			{
				flag = this.selectedDepositObjectTag.IsValid;
			}
		}
		PlantablePlot plantablePlot = this.targetReceptacle as PlantablePlot;
		WorldContainer myWorld = this.targetReceptacle.GetMyWorld();
		return flag && plantablePlot.ValidPlant && myWorld.worldInventory.GetCountWithAdditionalTag(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag, myWorld.IsModuleInterior) > 0;
	}

	// Token: 0x0600AD82 RID: 44418 RVA: 0x00115386 File Offset: 0x00113586
	public override void SetTarget(GameObject target)
	{
		this.selectedDepositObjectTag = Tag.Invalid;
		this.selectedDepositObjectAdditionalTag = Tag.Invalid;
		base.SetTarget(target);
		this.LoadTargetSubSpeciesRequest();
		this.RefreshSubspeciesToggles();
	}

	// Token: 0x0600AD83 RID: 44419 RVA: 0x004231B0 File Offset: 0x004213B0
	protected override void RestoreSelectionFromOccupant()
	{
		base.RestoreSelectionFromOccupant();
		PlantablePlot plantablePlot = (PlantablePlot)this.targetReceptacle;
		Tag tag = Tag.Invalid;
		Tag tag2 = Tag.Invalid;
		bool flag = false;
		if (plantablePlot.Occupant != null)
		{
			tag = plantablePlot.Occupant.GetComponent<SeedProducer>().seedInfo.seedId;
			MutantPlant component = plantablePlot.Occupant.GetComponent<MutantPlant>();
			if (component != null)
			{
				tag2 = component.SubSpeciesID;
			}
		}
		else if (plantablePlot.GetActiveRequest != null)
		{
			tag = plantablePlot.requestedEntityTag;
			tag2 = plantablePlot.requestedEntityAdditionalFilterTag;
			this.selectedDepositObjectTag = tag;
			this.selectedDepositObjectAdditionalTag = tag2;
			flag = true;
		}
		if (tag != Tag.Invalid)
		{
			if (!this.entityPreviousSelectionMap.ContainsKey(plantablePlot) || flag)
			{
				int value = 0;
				foreach (KeyValuePair<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> keyValuePair in this.depositObjectMap)
				{
					if (keyValuePair.Value.tag == tag)
					{
						value = this.entityToggles.IndexOf(keyValuePair.Key);
					}
				}
				if (!this.entityPreviousSelectionMap.ContainsKey(plantablePlot))
				{
					this.entityPreviousSelectionMap.Add(plantablePlot, -1);
				}
				this.entityPreviousSelectionMap[plantablePlot] = value;
			}
			if (!this.entityPreviousSubSelectionMap.ContainsKey(plantablePlot))
			{
				this.entityPreviousSubSelectionMap.Add(plantablePlot, Tag.Invalid);
			}
			if (this.entityPreviousSubSelectionMap[plantablePlot] == Tag.Invalid || flag)
			{
				this.entityPreviousSubSelectionMap[plantablePlot] = tag2;
			}
		}
	}

	// Token: 0x04008886 RID: 34950
	public DescriptorPanel RequirementsDescriptorPanel;

	// Token: 0x04008887 RID: 34951
	public DescriptorPanel HarvestDescriptorPanel;

	// Token: 0x04008888 RID: 34952
	public DescriptorPanel EffectsDescriptorPanel;

	// Token: 0x04008889 RID: 34953
	public GameObject mutationPanel;

	// Token: 0x0400888A RID: 34954
	public GameObject mutationViewport;

	// Token: 0x0400888B RID: 34955
	public GameObject mutationContainer;

	// Token: 0x0400888C RID: 34956
	public GameObject mutationOption;

	// Token: 0x0400888D RID: 34957
	public GameObject blankMutationOption;

	// Token: 0x0400888E RID: 34958
	public GameObject selectSpeciesPrompt;

	// Token: 0x0400888F RID: 34959
	private bool mutationPanelCollapsed = true;

	// Token: 0x04008890 RID: 34960
	public Dictionary<GameObject, Tag> subspeciesToggles = new Dictionary<GameObject, Tag>();

	// Token: 0x04008891 RID: 34961
	private List<GameObject> blankMutationObjects = new List<GameObject>();

	// Token: 0x04008892 RID: 34962
	private Dictionary<PlantablePlot, Tag> entityPreviousSubSelectionMap = new Dictionary<PlantablePlot, Tag>();

	// Token: 0x04008893 RID: 34963
	private Coroutine activeAnimationRoutine;

	// Token: 0x04008894 RID: 34964
	private const float EXPAND_DURATION = 0.33f;

	// Token: 0x04008895 RID: 34965
	private const float EXPAND_MIN = 24f;

	// Token: 0x04008896 RID: 34966
	private const float EXPAND_MAX = 118f;
}
