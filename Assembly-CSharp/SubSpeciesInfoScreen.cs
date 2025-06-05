using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200208B RID: 8331
public class SubSpeciesInfoScreen : KModalScreen
{
	// Token: 0x0600B183 RID: 45443 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x0600B184 RID: 45444 RVA: 0x00107377 File Offset: 0x00105577
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600B185 RID: 45445 RVA: 0x00439564 File Offset: 0x00437764
	private void ClearMutations()
	{
		for (int i = this.mutationLineItems.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.mutationLineItems[i]);
		}
		this.mutationLineItems.Clear();
	}

	// Token: 0x0600B186 RID: 45446 RVA: 0x00117E2F File Offset: 0x0011602F
	public void DisplayDiscovery(Tag speciesID, Tag subSpeciesID, GeneticAnalysisStation station)
	{
		this.SetSubspecies(speciesID, subSpeciesID);
		this.targetStation = station;
	}

	// Token: 0x0600B187 RID: 45447 RVA: 0x004395A8 File Offset: 0x004377A8
	private void SetSubspecies(Tag speciesID, Tag subSpeciesID)
	{
		this.ClearMutations();
		ref PlantSubSpeciesCatalog.SubSpeciesInfo subSpecies = PlantSubSpeciesCatalog.Instance.GetSubSpecies(speciesID, subSpeciesID);
		this.plantIcon.sprite = Def.GetUISprite(Assets.GetPrefab(speciesID), "ui", false).first;
		foreach (string id in subSpecies.mutationIDs)
		{
			PlantMutation plantMutation = Db.Get().PlantMutations.Get(id);
			GameObject gameObject = Util.KInstantiateUI(this.mutationsItemPrefab, this.mutationsList.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("nameLabel").text = plantMutation.Name;
			component.GetReference<LocText>("descriptionLabel").text = plantMutation.description;
			this.mutationLineItems.Add(gameObject);
		}
	}

	// Token: 0x04008BFD RID: 35837
	[SerializeField]
	private KButton renameButton;

	// Token: 0x04008BFE RID: 35838
	[SerializeField]
	private KButton saveButton;

	// Token: 0x04008BFF RID: 35839
	[SerializeField]
	private KButton discardButton;

	// Token: 0x04008C00 RID: 35840
	[SerializeField]
	private RectTransform mutationsList;

	// Token: 0x04008C01 RID: 35841
	[SerializeField]
	private Image plantIcon;

	// Token: 0x04008C02 RID: 35842
	[SerializeField]
	private GameObject mutationsItemPrefab;

	// Token: 0x04008C03 RID: 35843
	private List<GameObject> mutationLineItems = new List<GameObject>();

	// Token: 0x04008C04 RID: 35844
	private GeneticAnalysisStation targetStation;
}
