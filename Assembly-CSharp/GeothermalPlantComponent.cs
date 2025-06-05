using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x020013C2 RID: 5058
public class GeothermalPlantComponent : KMonoBehaviour, ICheckboxListGroupControl, IRelatedEntities
{
	// Token: 0x17000672 RID: 1650
	// (get) Token: 0x060067C6 RID: 26566 RVA: 0x000E84E3 File Offset: 0x000E66E3
	string ICheckboxListGroupControl.Title
	{
		get
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.SIDESCREENS.BRING_ONLINE_TITLE;
		}
	}

	// Token: 0x17000673 RID: 1651
	// (get) Token: 0x060067C7 RID: 26567 RVA: 0x000E84EF File Offset: 0x000E66EF
	string ICheckboxListGroupControl.Description
	{
		get
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.SIDESCREENS.BRING_ONLINE_DESC;
		}
	}

	// Token: 0x060067C8 RID: 26568 RVA: 0x002E2830 File Offset: 0x002E0A30
	public ICheckboxListGroupControl.ListGroup[] GetData()
	{
		ColonyAchievement activateGeothermalPlant = Db.Get().ColonyAchievements.ActivateGeothermalPlant;
		ICheckboxListGroupControl.CheckboxItem[] array = new ICheckboxListGroupControl.CheckboxItem[activateGeothermalPlant.requirementChecklist.Count];
		for (int i = 0; i < array.Length; i++)
		{
			ICheckboxListGroupControl.CheckboxItem checkboxItem = default(ICheckboxListGroupControl.CheckboxItem);
			bool flag = activateGeothermalPlant.requirementChecklist[i].Success();
			checkboxItem.isOn = flag;
			checkboxItem.text = (activateGeothermalPlant.requirementChecklist[i] as VictoryColonyAchievementRequirement).Name();
			checkboxItem.tooltip = activateGeothermalPlant.requirementChecklist[i].GetProgress(flag);
			array[i] = checkboxItem;
		}
		return new ICheckboxListGroupControl.ListGroup[]
		{
			new ICheckboxListGroupControl.ListGroup(activateGeothermalPlant.Name, array, null, null)
		};
	}

	// Token: 0x060067C9 RID: 26569 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x060067CA RID: 26570 RVA: 0x000D3AD3 File Offset: 0x000D1CD3
	public int CheckboxSideScreenSortOrder()
	{
		return 100;
	}

	// Token: 0x060067CB RID: 26571 RVA: 0x000E84FB File Offset: 0x000E66FB
	public static bool GeothermalControllerRepaired()
	{
		return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired;
	}

	// Token: 0x060067CC RID: 26572 RVA: 0x000E850C File Offset: 0x000E670C
	public static bool GeothermalFacilityDiscovered()
	{
		return SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered;
	}

	// Token: 0x060067CD RID: 26573 RVA: 0x000E851D File Offset: 0x000E671D
	protected override void OnSpawn()
	{
		base.Subscribe(-1503271301, new Action<object>(this.OnObjectSelect));
	}

	// Token: 0x060067CE RID: 26574 RVA: 0x000C4795 File Offset: 0x000C2995
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060067CF RID: 26575 RVA: 0x002E28EC File Offset: 0x002E0AEC
	public static void DisplayPopup(string title, string desc, HashedString anim, System.Action onDismissCallback, Transform clickFocus = null)
	{
		EventInfoData eventInfoData = new EventInfoData(title, desc, anim);
		if (Components.LiveMinionIdentities.Count >= 2)
		{
			int num = UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Count);
			int num2 = UnityEngine.Random.Range(1, Components.LiveMinionIdentities.Count);
			eventInfoData.minions = new GameObject[]
			{
				Components.LiveMinionIdentities[num].gameObject,
				Components.LiveMinionIdentities[(num + num2) % Components.LiveMinionIdentities.Count].gameObject
			};
		}
		else if (Components.LiveMinionIdentities.Count == 1)
		{
			eventInfoData.minions = new GameObject[]
			{
				Components.LiveMinionIdentities[0].gameObject
			};
		}
		eventInfoData.AddDefaultOption(onDismissCallback);
		eventInfoData.clickFocus = clickFocus;
		EventInfoScreen.ShowPopup(eventInfoData);
	}

	// Token: 0x060067D0 RID: 26576 RVA: 0x002E29B8 File Offset: 0x002E0BB8
	protected void RevealAllVentsAndController()
	{
		foreach (WorldGenSpawner.Spawnable spawnable in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(true, new Tag[]
		{
			"GeothermalVentEntity"
		}))
		{
			int baseX;
			int num;
			Grid.CellToXY(spawnable.cell, out baseX, out num);
			GridVisibility.Reveal(baseX, num + 2, 5, 5f);
		}
		foreach (WorldGenSpawner.Spawnable spawnable2 in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(true, new Tag[]
		{
			"GeothermalControllerEntity"
		}))
		{
			int baseX2;
			int num2;
			Grid.CellToXY(spawnable2.cell, out baseX2, out num2);
			GridVisibility.Reveal(baseX2, num2 + 3, 7, 7f);
		}
		SelectTool.Instance.Select(null, true);
	}

	// Token: 0x060067D1 RID: 26577 RVA: 0x002E2AC8 File Offset: 0x002E0CC8
	protected void OnObjectSelect(object clicked)
	{
		base.Unsubscribe(-1503271301, new Action<object>(this.OnObjectSelect));
		if (SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered)
		{
			return;
		}
		SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered = true;
		GeothermalPlantComponent.DisplayPopup(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_DISCOVERED_TITLE, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_DISOCVERED_DESC, "geothermalplantintro_kanim", new System.Action(this.RevealAllVentsAndController), null);
	}

	// Token: 0x060067D2 RID: 26578 RVA: 0x002E2B40 File Offset: 0x002E0D40
	public static void OnVentingHotMaterial(int worldid)
	{
		foreach (GeothermalVent geothermalVent in Components.GeothermalVents.GetItems(worldid))
		{
			if (geothermalVent.IsQuestEntombed())
			{
				geothermalVent.SetQuestComplete();
				if (!SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent)
				{
					GeothermalVictorySequence.VictoryVent = geothermalVent;
					GeothermalPlantComponent.DisplayPopup(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOPLANT_ERRUPTED_TITLE, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOPLANT_ERRUPTED_DESC, "geothermalplantachievement_kanim", delegate
					{
						SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent = true;
					}, null);
					break;
				}
			}
		}
	}

	// Token: 0x060067D3 RID: 26579 RVA: 0x002E2BFC File Offset: 0x002E0DFC
	public List<KSelectable> GetRelatedEntities()
	{
		List<KSelectable> list = new List<KSelectable>();
		int myWorldId = this.GetMyWorldId();
		foreach (GeothermalController geothermalController in Components.GeothermalControllers.GetItems(myWorldId))
		{
			list.Add(geothermalController.GetComponent<KSelectable>());
		}
		foreach (GeothermalVent geothermalVent in Components.GeothermalVents.GetItems(myWorldId))
		{
			list.Add(geothermalVent.GetComponent<KSelectable>());
		}
		return list;
	}

	// Token: 0x04004E72 RID: 20082
	public const string POPUP_DISCOVERED_KANIM = "geothermalplantintro_kanim";

	// Token: 0x04004E73 RID: 20083
	public const string POPUP_PROGRESS_KANIM = "geothermalplantonline_kanim";

	// Token: 0x04004E74 RID: 20084
	public const string POPUP_COMPLETE_KANIM = "geothermalplantachievement_kanim";
}
