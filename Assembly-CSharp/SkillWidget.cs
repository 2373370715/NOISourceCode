using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02002065 RID: 8293
[AddComponentMenu("KMonoBehaviour/scripts/SkillWidget")]
public class SkillWidget : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
	// Token: 0x17000B4E RID: 2894
	// (get) Token: 0x0600B05B RID: 45147 RVA: 0x001174BA File Offset: 0x001156BA
	// (set) Token: 0x0600B05C RID: 45148 RVA: 0x001174C2 File Offset: 0x001156C2
	public string skillID { get; private set; }

	// Token: 0x0600B05D RID: 45149 RVA: 0x0042FB48 File Offset: 0x0042DD48
	public void Refresh(string skillID)
	{
		Skill skill = Db.Get().Skills.Get(skillID);
		if (skill == null)
		{
			global::Debug.LogWarning("DbSkills is missing skillId " + skillID);
			return;
		}
		this.Name.text = skill.Name;
		SkillGroup skillGroup = Db.Get().SkillGroups.Get(skill.skillGroup);
		if (!string.IsNullOrEmpty(skillGroup.choreGroupID))
		{
			LocText name = this.Name;
			name.text = name.text + "\n(" + skillGroup.Name + ")";
		}
		this.skillID = skillID;
		this.tooltip.SetSimpleTooltip(this.SkillTooltip(skill));
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		MinionResume minionResume = null;
		if (minionIdentity != null)
		{
			minionResume = minionIdentity.GetComponent<MinionResume>();
			MinionResume.SkillMasteryConditions[] skillMasteryConditions = minionResume.GetSkillMasteryConditions(skillID);
			bool flag = minionResume.CanMasterSkill(skillMasteryConditions);
			if (!(minionResume == null) && (minionResume.HasMasteredSkill(skillID) || flag))
			{
				this.TitleBarBG.color = (minionResume.HasMasteredSkill(skillID) ? this.header_color_has_skill : this.header_color_can_assign);
				this.hatImage.material = this.defaultMaterial;
			}
			else
			{
				this.TitleBarBG.color = this.header_color_disabled;
				this.hatImage.material = this.desaturatedMaterial;
			}
		}
		else if (storedMinionIdentity != null)
		{
			if (storedMinionIdentity.HasMasteredSkill(skillID))
			{
				this.TitleBarBG.color = this.header_color_has_skill;
				this.hatImage.material = this.defaultMaterial;
			}
			else
			{
				this.TitleBarBG.color = this.header_color_disabled;
				this.hatImage.material = this.desaturatedMaterial;
			}
		}
		this.hatImage.sprite = Assets.GetSprite(skill.badge);
		bool active = false;
		bool flag2 = false;
		if (minionResume != null)
		{
			flag2 = minionResume.HasBeenGrantedSkill(skill);
			float num;
			minionResume.AptitudeBySkillGroup.TryGetValue(skill.skillGroup, out num);
			active = (num > 0f && !flag2);
		}
		this.aptitudeBox.SetActive(active);
		this.grantedBox.SetActive(flag2);
		if (flag2)
		{
			Sprite skillGrantSourceIcon = minionResume.GetSkillGrantSourceIcon(skill.Id);
			if (skillGrantSourceIcon != null)
			{
				this.grantedIcon.sprite = skillGrantSourceIcon;
			}
		}
		this.traitDisabledIcon.SetActive(minionResume != null && !minionResume.IsAbleToLearnSkill(skill.Id));
		string text = "";
		List<string> list = new List<string>();
		foreach (MinionIdentity minionIdentity2 in Components.LiveMinionIdentities.Items)
		{
			MinionResume component = minionIdentity2.GetComponent<MinionResume>();
			if (component != null && component.HasMasteredSkill(skillID))
			{
				list.Add(component.GetProperName());
			}
		}
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			foreach (MinionStorage.Info info in minionStorage.GetStoredMinionInfo())
			{
				if (info.serializedMinion != null)
				{
					StoredMinionIdentity storedMinionIdentity2 = info.serializedMinion.Get<StoredMinionIdentity>();
					if (storedMinionIdentity2 != null && storedMinionIdentity2.HasMasteredSkill(skillID))
					{
						list.Add(storedMinionIdentity2.GetProperName());
					}
				}
			}
		}
		this.masteryCount.gameObject.SetActive(list.Count > 0);
		foreach (string str in list)
		{
			text = text + "\n    • " + str;
		}
		this.masteryCount.SetSimpleTooltip((list.Count > 0) ? string.Format(UI.ROLES_SCREEN.WIDGET.NUMBER_OF_MASTERS_TOOLTIP, text) : UI.ROLES_SCREEN.WIDGET.NO_MASTERS_TOOLTIP.text);
		this.masteryCount.GetComponentInChildren<LocText>().text = list.Count.ToString();
	}

	// Token: 0x0600B05E RID: 45150 RVA: 0x0042FFB8 File Offset: 0x0042E1B8
	public void RefreshLines()
	{
		this.prerequisiteSkillWidgets.Clear();
		List<Vector2> list = new List<Vector2>();
		foreach (string text in Db.Get().Skills.Get(this.skillID).priorSkills)
		{
			list.Add(this.skillsScreen.GetSkillWidgetLineTargetPosition(text));
			this.prerequisiteSkillWidgets.Add(this.skillsScreen.GetSkillWidget(text));
		}
		if (this.lines != null)
		{
			for (int i = this.lines.Length - 1; i >= 0; i--)
			{
				UnityEngine.Object.Destroy(this.lines[i].gameObject);
			}
		}
		this.linePoints.Clear();
		for (int j = 0; j < list.Count; j++)
		{
			float num = this.lines_left.GetPosition().x - list[j].x - 12f;
			float y = 0f;
			this.linePoints.Add(new Vector2(0f, y));
			this.linePoints.Add(new Vector2(-num, y));
			this.linePoints.Add(new Vector2(-num, y));
			this.linePoints.Add(new Vector2(-num, -(this.lines_left.GetPosition().y - list[j].y)));
			this.linePoints.Add(new Vector2(-num, -(this.lines_left.GetPosition().y - list[j].y)));
			this.linePoints.Add(new Vector2(-(this.lines_left.GetPosition().x - list[j].x), -(this.lines_left.GetPosition().y - list[j].y)));
		}
		this.lines = new UILineRenderer[this.linePoints.Count / 2];
		int num2 = 0;
		for (int k = 0; k < this.linePoints.Count; k += 2)
		{
			GameObject gameObject = new GameObject("Line");
			gameObject.AddComponent<RectTransform>();
			gameObject.transform.SetParent(this.lines_left.transform);
			gameObject.transform.SetLocalPosition(Vector3.zero);
			gameObject.rectTransform().sizeDelta = Vector2.zero;
			this.lines[num2] = gameObject.AddComponent<UILineRenderer>();
			this.lines[num2].color = new Color(0.6509804f, 0.6509804f, 0.6509804f, 1f);
			this.lines[num2].Points = new Vector2[]
			{
				this.linePoints[k],
				this.linePoints[k + 1]
			};
			num2++;
		}
	}

	// Token: 0x0600B05F RID: 45151 RVA: 0x004302CC File Offset: 0x0042E4CC
	public void ToggleBorderHighlight(bool on)
	{
		this.borderHighlight.SetActive(on);
		if (this.lines != null)
		{
			foreach (UILineRenderer uilineRenderer in this.lines)
			{
				uilineRenderer.color = (on ? this.line_color_active : this.line_color_default);
				uilineRenderer.LineThickness = (float)(on ? 4 : 2);
				uilineRenderer.SetAllDirty();
			}
		}
		for (int j = 0; j < this.prerequisiteSkillWidgets.Count; j++)
		{
			this.prerequisiteSkillWidgets[j].ToggleBorderHighlight(on);
		}
	}

	// Token: 0x0600B060 RID: 45152 RVA: 0x001174CB File Offset: 0x001156CB
	public string SkillTooltip(Skill skill)
	{
		return "" + SkillWidget.SkillPerksString(skill) + "\n" + this.DuplicantSkillString(skill);
	}

	// Token: 0x0600B061 RID: 45153 RVA: 0x00430358 File Offset: 0x0042E558
	public static string SkillPerksString(Skill skill)
	{
		string text = "";
		foreach (SkillPerk skillPerk in skill.perks)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(skillPerk))
			{
				string text2 = GameUtil.NamesOfBuildingsRequiringSkillPerk(skillPerk.Id);
				if (!string.IsNullOrEmpty(text))
				{
					text += "\n";
				}
				text += ((text2 != null) ? text2 : ("• " + skillPerk.Name));
			}
		}
		return text;
	}

	// Token: 0x0600B062 RID: 45154 RVA: 0x004303F0 File Offset: 0x0042E5F0
	public string CriteriaString(Skill skill)
	{
		bool flag = false;
		string text = "";
		text = text + "<b>" + UI.ROLES_SCREEN.ASSIGNMENT_REQUIREMENTS.TITLE + "</b>\n";
		SkillGroup skillGroup = Db.Get().SkillGroups.Get(skill.skillGroup);
		if (skillGroup != null && skillGroup.relevantAttributes != null)
		{
			foreach (Klei.AI.Attribute attribute in skillGroup.relevantAttributes)
			{
				if (attribute != null)
				{
					text = text + "    • " + string.Format(UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.SKILLGROUP_ENABLED.DESCRIPTION, attribute.Name) + "\n";
					flag = true;
				}
			}
		}
		if (skill.priorSkills.Count > 0)
		{
			flag = true;
			for (int i = 0; i < skill.priorSkills.Count; i++)
			{
				text = text + "    • " + string.Format("{0}", Db.Get().Skills.Get(skill.priorSkills[i]).Name);
				text += "</color>";
				if (i != skill.priorSkills.Count - 1)
				{
					text += "\n";
				}
			}
		}
		if (!flag)
		{
			text = text + "    • " + string.Format(UI.ROLES_SCREEN.ASSIGNMENT_REQUIREMENTS.NONE, skill.Name);
		}
		return text;
	}

	// Token: 0x0600B063 RID: 45155 RVA: 0x00430560 File Offset: 0x0042E760
	public string DuplicantSkillString(Skill skill)
	{
		string text = "";
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity != null)
		{
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			if (component == null)
			{
				return "";
			}
			LocString loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.CAN_MASTER;
			if (component.HasMasteredSkill(skill.Id))
			{
				if (component.HasBeenGrantedSkill(skill))
				{
					text += "\n";
					loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.SKILL_GRANTED;
					text += string.Format(loc_string, minionIdentity.GetProperName(), skill.Name);
				}
			}
			else
			{
				MinionResume.SkillMasteryConditions[] skillMasteryConditions = component.GetSkillMasteryConditions(skill.Id);
				if (!component.CanMasterSkill(skillMasteryConditions))
				{
					bool flag = false;
					text += "\n";
					loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.CANNOT_MASTER;
					text += string.Format(loc_string, minionIdentity.GetProperName(), skill.Name);
					if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.UnableToLearn))
					{
						flag = true;
						string choreGroupID = Db.Get().SkillGroups.Get(skill.skillGroup).choreGroupID;
						Trait trait;
						minionIdentity.GetComponent<Traits>().IsChoreGroupDisabled(choreGroupID, out trait);
						text += "\n";
						loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.PREVENTED_BY_TRAIT;
						text += string.Format(loc_string, trait.Name);
					}
					if (!flag)
					{
						if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.MissingPreviousSkill))
						{
							text += "\n";
							loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.REQUIRES_PREVIOUS_SKILLS;
							text += string.Format(loc_string, Array.Empty<object>());
						}
					}
					if (!flag)
					{
						if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.NeedsSkillPoints))
						{
							text += "\n";
							loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.REQUIRES_MORE_SKILL_POINTS;
							text += string.Format(loc_string, Array.Empty<object>());
						}
					}
				}
				else
				{
					if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.StressWarning))
					{
						text += "\n";
						loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.STRESS_WARNING_MESSAGE;
						text += string.Format(loc_string, skill.Name, minionIdentity.GetProperName());
					}
					if (Array.Exists<MinionResume.SkillMasteryConditions>(skillMasteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.SkillAptitude))
					{
						text += "\n";
						loc_string = UI.SKILLS_SCREEN.ASSIGNMENT_REQUIREMENTS.MASTERY.SKILL_APTITUDE;
						text += string.Format(loc_string, minionIdentity.GetProperName(), skill.Name);
					}
				}
			}
		}
		return text;
	}

	// Token: 0x0600B064 RID: 45156 RVA: 0x001174EE File Offset: 0x001156EE
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.ToggleBorderHighlight(true);
		this.skillsScreen.HoverSkill(this.skillID);
		this.soundPlayer.Play(1);
	}

	// Token: 0x0600B065 RID: 45157 RVA: 0x00117514 File Offset: 0x00115714
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ToggleBorderHighlight(false);
		this.skillsScreen.HoverSkill(null);
	}

	// Token: 0x0600B066 RID: 45158 RVA: 0x00430850 File Offset: 0x0042EA50
	public void OnPointerClick(PointerEventData eventData)
	{
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		if (minionIdentity != null)
		{
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			if (DebugHandler.InstantBuildMode && component.AvailableSkillpoints < 1)
			{
				component.ForceAddSkillPoint();
			}
			MinionResume.SkillMasteryConditions[] skillMasteryConditions = component.GetSkillMasteryConditions(this.skillID);
			bool flag = component.CanMasterSkill(skillMasteryConditions);
			if (component != null && !component.HasMasteredSkill(this.skillID) && flag)
			{
				component.MasterSkill(this.skillID);
				this.skillsScreen.RefreshAll();
			}
		}
	}

	// Token: 0x0600B067 RID: 45159 RVA: 0x004308EC File Offset: 0x0042EAEC
	public void OnPointerDown(PointerEventData eventData)
	{
		MinionIdentity minionIdentity;
		StoredMinionIdentity storedMinionIdentity;
		this.skillsScreen.GetMinionIdentity(this.skillsScreen.CurrentlySelectedMinion, out minionIdentity, out storedMinionIdentity);
		MinionResume minionResume = null;
		bool flag = false;
		if (minionIdentity != null)
		{
			minionResume = minionIdentity.GetComponent<MinionResume>();
			MinionResume.SkillMasteryConditions[] skillMasteryConditions = minionResume.GetSkillMasteryConditions(this.skillID);
			flag = minionResume.CanMasterSkill(skillMasteryConditions);
		}
		if (minionResume != null && !minionResume.HasMasteredSkill(this.skillID) && flag)
		{
			KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
			return;
		}
		KFMOD.PlayUISound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x04008AB5 RID: 35509
	[SerializeField]
	private LocText Name;

	// Token: 0x04008AB6 RID: 35510
	[SerializeField]
	private LocText Description;

	// Token: 0x04008AB7 RID: 35511
	[SerializeField]
	private Image TitleBarBG;

	// Token: 0x04008AB8 RID: 35512
	[SerializeField]
	private SkillsScreen skillsScreen;

	// Token: 0x04008AB9 RID: 35513
	[SerializeField]
	private ToolTip tooltip;

	// Token: 0x04008ABA RID: 35514
	[SerializeField]
	private RectTransform lines_left;

	// Token: 0x04008ABB RID: 35515
	[SerializeField]
	public RectTransform lines_right;

	// Token: 0x04008ABC RID: 35516
	[SerializeField]
	private Color header_color_has_skill;

	// Token: 0x04008ABD RID: 35517
	[SerializeField]
	private Color header_color_can_assign;

	// Token: 0x04008ABE RID: 35518
	[SerializeField]
	private Color header_color_disabled;

	// Token: 0x04008ABF RID: 35519
	[SerializeField]
	private Color line_color_default;

	// Token: 0x04008AC0 RID: 35520
	[SerializeField]
	private Color line_color_active;

	// Token: 0x04008AC1 RID: 35521
	[SerializeField]
	private Image hatImage;

	// Token: 0x04008AC2 RID: 35522
	[SerializeField]
	private GameObject borderHighlight;

	// Token: 0x04008AC3 RID: 35523
	[SerializeField]
	private ToolTip masteryCount;

	// Token: 0x04008AC4 RID: 35524
	[SerializeField]
	private GameObject aptitudeBox;

	// Token: 0x04008AC5 RID: 35525
	[SerializeField]
	private GameObject grantedBox;

	// Token: 0x04008AC6 RID: 35526
	[SerializeField]
	private Image grantedIcon;

	// Token: 0x04008AC7 RID: 35527
	[SerializeField]
	private GameObject traitDisabledIcon;

	// Token: 0x04008AC8 RID: 35528
	public TextStyleSetting TooltipTextStyle_Header;

	// Token: 0x04008AC9 RID: 35529
	public TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;

	// Token: 0x04008ACA RID: 35530
	private List<SkillWidget> prerequisiteSkillWidgets = new List<SkillWidget>();

	// Token: 0x04008ACB RID: 35531
	private UILineRenderer[] lines;

	// Token: 0x04008ACC RID: 35532
	private List<Vector2> linePoints = new List<Vector2>();

	// Token: 0x04008ACD RID: 35533
	public Material defaultMaterial;

	// Token: 0x04008ACE RID: 35534
	public Material desaturatedMaterial;

	// Token: 0x04008ACF RID: 35535
	public ButtonSoundPlayer soundPlayer;
}
