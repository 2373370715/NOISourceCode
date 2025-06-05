using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02002064 RID: 8292
[AddComponentMenu("KMonoBehaviour/scripts/SkillMinionWidget")]
public class SkillMinionWidget : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x17000B4D RID: 2893
	// (get) Token: 0x0600B04C RID: 45132 RVA: 0x001173C2 File Offset: 0x001155C2
	// (set) Token: 0x0600B04D RID: 45133 RVA: 0x001173CA File Offset: 0x001155CA
	public IAssignableIdentity assignableIdentity { get; private set; }

	// Token: 0x0600B04E RID: 45134 RVA: 0x0042F560 File Offset: 0x0042D760
	public void SetMinon(IAssignableIdentity identity)
	{
		this.assignableIdentity = identity;
		this.portrait.SetIdentityObject(this.assignableIdentity, true);
		base.GetComponent<NotificationHighlightTarget>().targetKey = identity.GetSoleOwner().gameObject.GetInstanceID().ToString();
	}

	// Token: 0x0600B04F RID: 45135 RVA: 0x001173D3 File Offset: 0x001155D3
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.ToggleHover(true);
		this.soundPlayer.Play(1);
	}

	// Token: 0x0600B050 RID: 45136 RVA: 0x001173E8 File Offset: 0x001155E8
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ToggleHover(false);
	}

	// Token: 0x0600B051 RID: 45137 RVA: 0x001173F1 File Offset: 0x001155F1
	private void ToggleHover(bool on)
	{
		if (this.skillsScreen.CurrentlySelectedMinion != this.assignableIdentity)
		{
			this.SetColor(on ? this.hover_color : this.unselected_color);
		}
	}

	// Token: 0x0600B052 RID: 45138 RVA: 0x0011741D File Offset: 0x0011561D
	private void SetColor(Color color)
	{
		this.background.color = color;
		if (this.assignableIdentity != null && this.assignableIdentity as StoredMinionIdentity != null)
		{
			base.GetComponent<CanvasGroup>().alpha = 0.6f;
		}
	}

	// Token: 0x0600B053 RID: 45139 RVA: 0x00117456 File Offset: 0x00115656
	public void OnPointerClick(PointerEventData eventData)
	{
		this.skillsScreen.CurrentlySelectedMinion = this.assignableIdentity;
		base.GetComponent<NotificationHighlightTarget>().View();
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
	}

	// Token: 0x0600B054 RID: 45140 RVA: 0x0042F5AC File Offset: 0x0042D7AC
	public void Refresh()
	{
		if (this.assignableIdentity.IsNullOrDestroyed())
		{
			return;
		}
		this.portrait.SetIdentityObject(this.assignableIdentity, true);
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.assignableIdentity, out minionIdentity, out storedMinionIdentity);
		this.hatDropDown.gameObject.SetActive(true);
		string hat;
		if (minionIdentity != null)
		{
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			int availableSkillpoints = component.AvailableSkillpoints;
			int totalSkillPointsGained = component.TotalSkillPointsGained;
			this.masteryPoints.text = ((availableSkillpoints > 0) ? GameUtil.ApplyBoldString(GameUtil.ColourizeString(new Color(0.5f, 1f, 0.5f, 1f), availableSkillpoints.ToString())) : "0");
			AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(component);
			AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(component);
			this.morale.text = string.Format("{0}/{1}", attributeInstance.GetTotalValue(), attributeInstance2.GetTotalValue());
			this.RefreshToolTip(component);
			List<IListableOption> list = new List<IListableOption>();
			foreach (MinionResume.HatInfo hatInfo in component.GetAllHats())
			{
				list.Add(new HatListable(hatInfo.Source, hatInfo.Hat));
			}
			this.hatDropDown.Initialize(list, new Action<IListableOption, object>(this.OnHatDropEntryClick), new Func<IListableOption, IListableOption, object, int>(this.hatDropDownSort), new Action<DropDownEntry, object>(this.hatDropEntryRefreshAction), false, minionIdentity);
			hat = (string.IsNullOrEmpty(component.TargetHat) ? component.CurrentHat : component.TargetHat);
		}
		else
		{
			ToolTip component2 = base.GetComponent<ToolTip>();
			component2.ClearMultiStringTooltip();
			component2.AddMultiStringTooltip(string.Format(UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, storedMinionIdentity.GetStorageReason(), storedMinionIdentity.GetProperName()), null);
			hat = (string.IsNullOrEmpty(storedMinionIdentity.targetHat) ? storedMinionIdentity.currentHat : storedMinionIdentity.targetHat);
			this.masteryPoints.text = UI.TABLESCREENS.NA;
			this.morale.text = UI.TABLESCREENS.NA;
		}
		bool flag = this.skillsScreen.CurrentlySelectedMinion == this.assignableIdentity;
		if (this.skillsScreen.CurrentlySelectedMinion != null && this.assignableIdentity != null)
		{
			flag = (flag || this.skillsScreen.CurrentlySelectedMinion.GetSoleOwner() == this.assignableIdentity.GetSoleOwner());
		}
		this.SetColor(flag ? this.selected_color : this.unselected_color);
		HierarchyReferences component3 = base.GetComponent<HierarchyReferences>();
		this.RefreshHat(hat);
		component3.GetReference("openButton").gameObject.SetActive(minionIdentity != null);
	}

	// Token: 0x0600B055 RID: 45141 RVA: 0x0042F890 File Offset: 0x0042DA90
	private void RefreshToolTip(MinionResume resume)
	{
		if (resume != null)
		{
			AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(resume);
			AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(resume);
			ToolTip component = base.GetComponent<ToolTip>();
			component.ClearMultiStringTooltip();
			component.AddMultiStringTooltip(this.assignableIdentity.GetProperName() + "\n\n", this.TooltipTextStyle_Header);
			component.AddMultiStringTooltip(string.Format(UI.SKILLS_SCREEN.CURRENT_MORALE, attributeInstance.GetTotalValue(), attributeInstance2.GetTotalValue()), null);
			component.AddMultiStringTooltip("\n" + UI.DETAILTABS.STATS.NAME + "\n\n", this.TooltipTextStyle_Header);
			foreach (AttributeInstance attributeInstance3 in resume.GetAttributes())
			{
				if (attributeInstance3.Attribute.ShowInUI == Klei.AI.Attribute.Display.Skill)
				{
					string text = UIConstants.ColorPrefixWhite;
					if (attributeInstance3.GetTotalValue() > 0f)
					{
						text = UIConstants.ColorPrefixGreen;
					}
					else if (attributeInstance3.GetTotalValue() < 0f)
					{
						text = UIConstants.ColorPrefixRed;
					}
					component.AddMultiStringTooltip(string.Concat(new string[]
					{
						"    • ",
						attributeInstance3.Name,
						": ",
						text,
						attributeInstance3.GetTotalValue().ToString(),
						UIConstants.ColorSuffix
					}), null);
				}
			}
		}
	}

	// Token: 0x0600B056 RID: 45142 RVA: 0x00117484 File Offset: 0x00115684
	public void RefreshHat(string hat)
	{
		base.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite(string.IsNullOrEmpty(hat) ? "hat_role_none" : hat);
	}

	// Token: 0x0600B057 RID: 45143 RVA: 0x0042FA24 File Offset: 0x0042DC24
	private void OnHatDropEntryClick(IListableOption hatOption, object data)
	{
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.assignableIdentity, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity == null)
		{
			return;
		}
		MinionResume component = minionIdentity.GetComponent<MinionResume>();
		if (hatOption != null)
		{
			base.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite((hatOption as HatListable).hat);
			if (component != null)
			{
				string hat = (hatOption as HatListable).hat;
				component.SetHats(component.CurrentHat, hat);
				if (component.OwnsHat(hat))
				{
					component.CreateHatChangeChore();
				}
			}
		}
		else
		{
			base.GetComponent<HierarchyReferences>().GetReference("selectedHat").GetComponent<Image>().sprite = Assets.GetSprite("hat_role_none");
			if (component != null)
			{
				component.SetHats(component.CurrentHat, null);
				component.ApplyTargetHat();
			}
		}
		this.skillsScreen.RefreshAll();
	}

	// Token: 0x0600B058 RID: 45144 RVA: 0x0042FB0C File Offset: 0x0042DD0C
	private void hatDropEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		if (entry.entryData != null)
		{
			HatListable hatListable = entry.entryData as HatListable;
			entry.image.sprite = Assets.GetSprite(hatListable.hat);
		}
	}

	// Token: 0x0600B059 RID: 45145 RVA: 0x000B1628 File Offset: 0x000AF828
	private int hatDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		return 0;
	}

	// Token: 0x04008AA7 RID: 35495
	[SerializeField]
	private SkillsScreen skillsScreen;

	// Token: 0x04008AA8 RID: 35496
	[SerializeField]
	private CrewPortrait portrait;

	// Token: 0x04008AA9 RID: 35497
	[SerializeField]
	private LocText masteryPoints;

	// Token: 0x04008AAA RID: 35498
	[SerializeField]
	private LocText morale;

	// Token: 0x04008AAB RID: 35499
	[SerializeField]
	private Image background;

	// Token: 0x04008AAC RID: 35500
	[SerializeField]
	private Image hat_background;

	// Token: 0x04008AAD RID: 35501
	[SerializeField]
	private Color selected_color;

	// Token: 0x04008AAE RID: 35502
	[SerializeField]
	private Color unselected_color;

	// Token: 0x04008AAF RID: 35503
	[SerializeField]
	private Color hover_color;

	// Token: 0x04008AB0 RID: 35504
	[SerializeField]
	private DropDown hatDropDown;

	// Token: 0x04008AB1 RID: 35505
	[SerializeField]
	private TextStyleSetting TooltipTextStyle_Header;

	// Token: 0x04008AB2 RID: 35506
	[SerializeField]
	private TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;

	// Token: 0x04008AB3 RID: 35507
	public ButtonSoundPlayer soundPlayer;
}
