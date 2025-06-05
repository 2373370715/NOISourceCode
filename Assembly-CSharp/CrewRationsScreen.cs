using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DF8 RID: 7672
public class CrewRationsScreen : CrewListScreen<CrewRationsEntry>
{
	// Token: 0x0600A071 RID: 41073 RVA: 0x0010CF01 File Offset: 0x0010B101
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closebutton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
	}

	// Token: 0x0600A072 RID: 41074 RVA: 0x0010CF33 File Offset: 0x0010B133
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		base.RefreshCrewPortraitContent();
		this.SortByPreviousSelected();
	}

	// Token: 0x0600A073 RID: 41075 RVA: 0x003E3F94 File Offset: 0x003E2194
	private void SortByPreviousSelected()
	{
		if (this.sortToggleGroup == null)
		{
			return;
		}
		if (this.lastSortToggle == null)
		{
			return;
		}
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			OverviewColumnIdentity component = this.ColumnTitlesContainer.GetChild(i).GetComponent<OverviewColumnIdentity>();
			if (this.ColumnTitlesContainer.GetChild(i).GetComponent<Toggle>() == this.lastSortToggle)
			{
				if (component.columnID == "name")
				{
					base.SortByName(this.lastSortReversed);
				}
				if (component.columnID == "health")
				{
					this.SortByAmount("HitPoints", this.lastSortReversed);
				}
				if (component.columnID == "stress")
				{
					this.SortByAmount("Stress", this.lastSortReversed);
				}
				if (component.columnID == "calories")
				{
					this.SortByAmount("Calories", this.lastSortReversed);
				}
			}
		}
	}

	// Token: 0x0600A074 RID: 41076 RVA: 0x003E4098 File Offset: 0x003E2298
	protected override void PositionColumnTitles()
	{
		base.PositionColumnTitles();
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			OverviewColumnIdentity component = this.ColumnTitlesContainer.GetChild(i).GetComponent<OverviewColumnIdentity>();
			if (component.Sortable)
			{
				Toggle toggle = this.ColumnTitlesContainer.GetChild(i).GetComponent<Toggle>();
				toggle.group = this.sortToggleGroup;
				ImageToggleState toggleImage = toggle.GetComponentInChildren<ImageToggleState>(true);
				if (component.columnID == "name")
				{
					toggle.onValueChanged.AddListener(delegate(bool value)
					{
						this.SortByName(!toggle.isOn);
						this.lastSortToggle = toggle;
						this.lastSortReversed = !toggle.isOn;
						this.ResetSortToggles(toggle);
						if (toggle.isOn)
						{
							toggleImage.SetActive();
							return;
						}
						toggleImage.SetInactive();
					});
				}
				if (component.columnID == "health")
				{
					toggle.onValueChanged.AddListener(delegate(bool value)
					{
						this.SortByAmount("HitPoints", !toggle.isOn);
						this.lastSortToggle = toggle;
						this.lastSortReversed = !toggle.isOn;
						this.ResetSortToggles(toggle);
						if (toggle.isOn)
						{
							toggleImage.SetActive();
							return;
						}
						toggleImage.SetInactive();
					});
				}
				if (component.columnID == "stress")
				{
					toggle.onValueChanged.AddListener(delegate(bool value)
					{
						this.SortByAmount("Stress", !toggle.isOn);
						this.lastSortToggle = toggle;
						this.lastSortReversed = !toggle.isOn;
						this.ResetSortToggles(toggle);
						if (toggle.isOn)
						{
							toggleImage.SetActive();
							return;
						}
						toggleImage.SetInactive();
					});
				}
				if (component.columnID == "calories")
				{
					toggle.onValueChanged.AddListener(delegate(bool value)
					{
						this.SortByAmount("Calories", !toggle.isOn);
						this.lastSortToggle = toggle;
						this.lastSortReversed = !toggle.isOn;
						this.ResetSortToggles(toggle);
						if (toggle.isOn)
						{
							toggleImage.SetActive();
							return;
						}
						toggleImage.SetInactive();
					});
				}
			}
		}
	}

	// Token: 0x0600A075 RID: 41077 RVA: 0x003E41E4 File Offset: 0x003E23E4
	protected override void SpawnEntries()
	{
		base.SpawnEntries();
		foreach (MinionIdentity identity in Components.LiveMinionIdentities.Items)
		{
			CrewRationsEntry component = Util.KInstantiateUI(this.Prefab_CrewEntry, this.EntriesPanelTransform.gameObject, false).GetComponent<CrewRationsEntry>();
			component.Populate(identity);
			this.EntryObjects.Add(component);
		}
		this.SortByPreviousSelected();
	}

	// Token: 0x0600A076 RID: 41078 RVA: 0x003E4270 File Offset: 0x003E2470
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		foreach (CrewRationsEntry crewRationsEntry in this.EntryObjects)
		{
			crewRationsEntry.Refresh();
		}
	}

	// Token: 0x0600A077 RID: 41079 RVA: 0x003E42C8 File Offset: 0x003E24C8
	private void SortByAmount(string amount_id, bool reverse)
	{
		List<CrewRationsEntry> list = new List<CrewRationsEntry>(this.EntryObjects);
		list.Sort(delegate(CrewRationsEntry a, CrewRationsEntry b)
		{
			float value = a.Identity.GetAmounts().GetValue(amount_id);
			float value2 = b.Identity.GetAmounts().GetValue(amount_id);
			return value.CompareTo(value2);
		});
		base.ReorderEntries(list, reverse);
	}

	// Token: 0x0600A078 RID: 41080 RVA: 0x003E4308 File Offset: 0x003E2508
	private void ResetSortToggles(Toggle exceptToggle)
	{
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			Toggle component = this.ColumnTitlesContainer.GetChild(i).GetComponent<Toggle>();
			ImageToggleState componentInChildren = component.GetComponentInChildren<ImageToggleState>(true);
			if (component != exceptToggle)
			{
				componentInChildren.SetDisabled();
			}
		}
	}

	// Token: 0x04007E13 RID: 32275
	[SerializeField]
	private KButton closebutton;
}
