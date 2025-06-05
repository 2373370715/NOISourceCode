using System;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FD3 RID: 8147
public class GeneticAnalysisStationSideScreen : SideScreenContent
{
	// Token: 0x0600AC1F RID: 44063 RVA: 0x0011451E File Offset: 0x0011271E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Refresh();
	}

	// Token: 0x0600AC20 RID: 44064 RVA: 0x0011452C File Offset: 0x0011272C
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<GeneticAnalysisStation.StatesInstance>() != null;
	}

	// Token: 0x0600AC21 RID: 44065 RVA: 0x00114537 File Offset: 0x00112737
	public override void SetTarget(GameObject target)
	{
		this.target = target.GetSMI<GeneticAnalysisStation.StatesInstance>();
		target.GetComponent<GeneticAnalysisStationWorkable>();
		this.Refresh();
	}

	// Token: 0x0600AC22 RID: 44066 RVA: 0x00114552 File Offset: 0x00112752
	private void Refresh()
	{
		if (this.target == null)
		{
			return;
		}
		this.DrawPickerMenu();
	}

	// Token: 0x0600AC23 RID: 44067 RVA: 0x0041C580 File Offset: 0x0041A780
	private void DrawPickerMenu()
	{
		Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> dictionary = new Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>>();
		foreach (Tag tag in PlantSubSpeciesCatalog.Instance.GetAllDiscoveredSpecies())
		{
			dictionary[tag] = new List<PlantSubSpeciesCatalog.SubSpeciesInfo>(PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(tag));
		}
		int num = 0;
		foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> keyValuePair in dictionary)
		{
			if (PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(keyValuePair.Key).Count > 1)
			{
				GameObject prefab = Assets.GetPrefab(keyValuePair.Key);
				if (!(prefab == null))
				{
					SeedProducer component = prefab.GetComponent<SeedProducer>();
					if (!(component == null))
					{
						Tag tag2 = component.seedInfo.seedId.ToTag();
						if (DiscoveredResources.Instance.IsDiscovered(tag2))
						{
							HierarchyReferences hierarchyReferences;
							if (num < this.rows.Count)
							{
								hierarchyReferences = this.rows[num];
							}
							else
							{
								hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.rowPrefab.gameObject, this.rowContainer, false);
								this.rows.Add(hierarchyReferences);
							}
							this.ConfigureButton(hierarchyReferences, keyValuePair.Key);
							this.rows[num].gameObject.SetActive(true);
							num++;
						}
					}
				}
			}
		}
		for (int i = num; i < this.rows.Count; i++)
		{
			this.rows[i].gameObject.SetActive(false);
		}
		if (num == 0)
		{
			this.message.text = UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.NONE_DISCOVERED;
			this.contents.gameObject.SetActive(false);
			return;
		}
		this.message.text = UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SELECT_SEEDS;
		this.contents.gameObject.SetActive(true);
	}

	// Token: 0x0600AC24 RID: 44068 RVA: 0x0041C78C File Offset: 0x0041A98C
	private void ConfigureButton(HierarchyReferences button, Tag speciesID)
	{
		TMP_Text reference = button.GetReference<LocText>("Label");
		Image reference2 = button.GetReference<Image>("Icon");
		LocText reference3 = button.GetReference<LocText>("ProgressLabel");
		button.GetReference<ToolTip>("ToolTip");
		Tag seedID = this.GetSeedIDFromPlantID(speciesID);
		bool isForbidden = this.target.GetSeedForbidden(seedID);
		reference.text = seedID.ProperName();
		reference2.sprite = Def.GetUISprite(seedID, "ui", false).first;
		if (PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(speciesID).Count > 0)
		{
			reference3.text = (isForbidden ? UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SEED_FORBIDDEN : UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SEED_ALLOWED);
		}
		else
		{
			reference3.text = UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SEED_NO_MUTANTS;
		}
		KToggle component = button.GetComponent<KToggle>();
		component.isOn = !isForbidden;
		component.ClearOnClick();
		component.onClick += delegate()
		{
			this.target.SetSeedForbidden(seedID, !isForbidden);
			this.Refresh();
		};
	}

	// Token: 0x0600AC25 RID: 44069 RVA: 0x00114563 File Offset: 0x00112763
	private Tag GetSeedIDFromPlantID(Tag speciesID)
	{
		return Assets.GetPrefab(speciesID).GetComponent<SeedProducer>().seedInfo.seedId;
	}

	// Token: 0x0400878B RID: 34699
	[SerializeField]
	private LocText message;

	// Token: 0x0400878C RID: 34700
	[SerializeField]
	private GameObject contents;

	// Token: 0x0400878D RID: 34701
	[SerializeField]
	private GameObject rowContainer;

	// Token: 0x0400878E RID: 34702
	[SerializeField]
	private HierarchyReferences rowPrefab;

	// Token: 0x0400878F RID: 34703
	private List<HierarchyReferences> rows = new List<HierarchyReferences>();

	// Token: 0x04008790 RID: 34704
	private GeneticAnalysisStation.StatesInstance target;

	// Token: 0x04008791 RID: 34705
	private Dictionary<Tag, bool> expandedSeeds = new Dictionary<Tag, bool>();
}
