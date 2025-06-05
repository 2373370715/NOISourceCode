using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001B03 RID: 6915
public class CharacterContainer : KScreen, ITelepadDeliverableContainer
{
	// Token: 0x060090B7 RID: 37047 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x1700099B RID: 2459
	// (get) Token: 0x060090B8 RID: 37048 RVA: 0x001030C6 File Offset: 0x001012C6
	public MinionStartingStats Stats
	{
		get
		{
			return this.stats;
		}
	}

	// Token: 0x060090B9 RID: 37049 RVA: 0x003891E4 File Offset: 0x003873E4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Initialize();
		this.characterNameTitle.OnStartedEditing += this.OnStartedEditing;
		this.characterNameTitle.OnNameChanged += this.OnNameChanged;
		this.reshuffleButton.onClick += delegate()
		{
			this.Reshuffle(true);
		};
		List<IListableOption> list = new List<IListableOption>();
		foreach (SkillGroup item in new List<SkillGroup>(Db.Get().SkillGroups.resources))
		{
			list.Add(item);
		}
		list.Remove(Db.Get().SkillGroups.BionicSkills);
		this.archetypeDropDown.Initialize(list, new Action<IListableOption, object>(this.OnArchetypeEntryClick), new Func<IListableOption, IListableOption, object, int>(this.archetypeDropDownSort), new Action<DropDownEntry, object>(this.archetypeDropEntryRefreshAction), false, null);
		this.archetypeDropDown.CustomizeEmptyRow(Strings.Get("STRINGS.UI.CHARACTERCONTAINER_NOARCHETYPESELECTED"), this.noArchetypeIcon);
		List<IListableOption> contentKeys = new List<IListableOption>
		{
			new CharacterContainer.MinionModelOption(DUPLICANTS.MODEL.STANDARD.NAME, new List<Tag>
			{
				GameTags.Minions.Models.Standard
			}, Assets.GetSprite("ui_duplicant_minion_selection")),
			new CharacterContainer.MinionModelOption(DUPLICANTS.MODEL.BIONIC.NAME, new List<Tag>
			{
				GameTags.Minions.Models.Bionic
			}, Assets.GetSprite("ui_duplicant_bionicminion_selection"))
		};
		this.modelDropDown.Initialize(contentKeys, new Action<IListableOption, object>(this.OnModelEntryClick), new Func<IListableOption, IListableOption, object, int>(this.modelDropDownSort), new Action<DropDownEntry, object>(this.modelDropEntryRefreshAction), true, null);
		this.modelDropDown.CustomizeEmptyRow(UI.CHARACTERCONTAINER_ALL_MODELS, Assets.GetSprite(this.allModelSprite));
		base.StartCoroutine(this.DelayedGeneration());
	}

	// Token: 0x060090BA RID: 37050 RVA: 0x001030CE File Offset: 0x001012CE
	public void ForceStopEditingTitle()
	{
		this.characterNameTitle.ForceStopEditing();
	}

	// Token: 0x060090BB RID: 37051 RVA: 0x00102E82 File Offset: 0x00101082
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x060090BC RID: 37052 RVA: 0x001030DB File Offset: 0x001012DB
	private IEnumerator DelayedGeneration()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		this.GenerateCharacter(this.controller.IsStarterMinion, null);
		yield break;
	}

	// Token: 0x060090BD RID: 37053 RVA: 0x001030EA File Offset: 0x001012EA
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.animController != null)
		{
			this.animController.gameObject.DeleteObject();
			this.animController = null;
		}
	}

	// Token: 0x060090BE RID: 37054 RVA: 0x00103117 File Offset: 0x00101317
	protected override void OnForcedCleanUp()
	{
		CharacterContainer.containers.Remove(this);
		base.OnForcedCleanUp();
	}

	// Token: 0x060090BF RID: 37055 RVA: 0x003893D8 File Offset: 0x003875D8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.controller != null)
		{
			CharacterSelectionController characterSelectionController = this.controller;
			characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Remove(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
			CharacterSelectionController characterSelectionController2 = this.controller;
			characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Remove(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
			CharacterSelectionController characterSelectionController3 = this.controller;
			characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Remove(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		}
	}

	// Token: 0x060090C0 RID: 37056 RVA: 0x00389470 File Offset: 0x00387670
	private void Initialize()
	{
		this.iconGroups = new List<GameObject>();
		this.traitEntries = new List<GameObject>();
		this.expectationLabels = new List<LocText>();
		this.aptitudeEntries = new List<GameObject>();
		if (CharacterContainer.containers == null)
		{
			CharacterContainer.containers = new List<CharacterContainer>();
		}
		CharacterContainer.containers.Add(this);
	}

	// Token: 0x060090C1 RID: 37057 RVA: 0x0010312B File Offset: 0x0010132B
	private void OnNameChanged(string newName)
	{
		this.stats.Name = newName;
		this.stats.personality.Name = newName;
		this.description.text = this.stats.personality.description;
	}

	// Token: 0x060090C2 RID: 37058 RVA: 0x00103165 File Offset: 0x00101365
	private void OnStartedEditing()
	{
		KScreenManager.Instance.RefreshStack();
	}

	// Token: 0x060090C3 RID: 37059 RVA: 0x003894C8 File Offset: 0x003876C8
	public void SetMinion(MinionStartingStats statsProposed)
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			this.DeselectDeliverable();
		}
		this.stats = statsProposed;
		if (this.animController != null)
		{
			UnityEngine.Object.Destroy(this.animController.gameObject);
			this.animController = null;
		}
		this.SetAnimator();
		this.SetInfoText();
		base.StartCoroutine(this.SetAttributes());
		this.selectButton.ClearOnClick();
		if (!this.controller.IsStarterMinion)
		{
			this.selectButton.enabled = true;
			this.selectButton.onClick += delegate()
			{
				this.SelectDeliverable();
			};
		}
	}

	// Token: 0x060090C4 RID: 37060 RVA: 0x0038957C File Offset: 0x0038777C
	public void GenerateCharacter(bool is_starter, string guaranteedAptitudeID = null)
	{
		int num = 0;
		do
		{
			this.stats = new MinionStartingStats(this.permittedModels, is_starter, guaranteedAptitudeID, null, false);
			num++;
		}
		while (this.IsCharacterInvalid() && num < 20);
		if (this.animController != null)
		{
			UnityEngine.Object.Destroy(this.animController.gameObject);
			this.animController = null;
		}
		this.SetAnimator();
		this.SetInfoText();
		base.StartCoroutine(this.SetAttributes());
		this.selectButton.ClearOnClick();
		if (!this.controller.IsStarterMinion)
		{
			this.selectButton.enabled = true;
			this.selectButton.onClick += delegate()
			{
				this.SelectDeliverable();
			};
		}
	}

	// Token: 0x060090C5 RID: 37061 RVA: 0x0038962C File Offset: 0x0038782C
	private void SetAnimator()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(GameTags.MinionSelectPreview), this.contentBody.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = this.baseCharacterScale;
		}
		BaseMinionConfig.ConfigureSymbols(this.animController.gameObject, true);
		this.stats.ApplyTraits(this.animController.gameObject);
		this.stats.ApplyRace(this.animController.gameObject);
		this.stats.ApplyAccessories(this.animController.gameObject);
		this.stats.ApplyOutfit(this.stats.personality, this.animController.gameObject);
		this.stats.ApplyJoyResponseOutfit(this.stats.personality, this.animController.gameObject);
		this.stats.ApplyExperience(this.animController.gameObject);
		HashedString idleAnim = this.GetIdleAnim(this.stats);
		this.idle_anim = Assets.GetAnim(idleAnim);
		if (this.idle_anim != null)
		{
			this.animController.AddAnimOverrides(this.idle_anim, 0f);
		}
		KAnimFile anim = Assets.GetAnim(new HashedString("crewSelect_fx_kanim"));
		this.bgAnimController.SwapAnims(new KAnimFile[]
		{
			Assets.GetAnim(CharacterContainer.portraitBGAnims[this.stats.personality.model])
		});
		this.bgAnimController.Play("crewSelect_bg", KAnim.PlayMode.Loop, 1f, 0f);
		if (anim != null)
		{
			this.animController.AddAnimOverrides(anim, 0f);
		}
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x060090C6 RID: 37062 RVA: 0x0038981C File Offset: 0x00387A1C
	private HashedString GetIdleAnim(MinionStartingStats minionStartingStats)
	{
		List<HashedString> list = new List<HashedString>();
		foreach (KeyValuePair<HashedString, string[]> keyValuePair in CharacterContainer.traitIdleAnims)
		{
			foreach (Trait trait in minionStartingStats.Traits)
			{
				if (keyValuePair.Value.Contains(trait.Id))
				{
					list.Add(keyValuePair.Key);
				}
			}
			if (keyValuePair.Value.Contains(minionStartingStats.joyTrait.Id) || keyValuePair.Value.Contains(minionStartingStats.stressTrait.Id))
			{
				list.Add(keyValuePair.Key);
			}
		}
		if (list.Count > 0)
		{
			return list.ToArray()[UnityEngine.Random.Range(0, list.Count)];
		}
		return CharacterContainer.idleAnims[UnityEngine.Random.Range(0, CharacterContainer.idleAnims.Length)];
	}

	// Token: 0x060090C7 RID: 37063 RVA: 0x00389948 File Offset: 0x00387B48
	private void SetInfoText()
	{
		this.traitEntries.ForEach(delegate(GameObject tl)
		{
			UnityEngine.Object.Destroy(tl.gameObject);
		});
		this.traitEntries.Clear();
		this.characterNameTitle.SetTitle(this.stats.Name);
		this.traitHeaderLabel.SetText((this.stats.personality.model == GameTags.Minions.Models.Bionic) ? UI.CHARACTERCONTAINER_TRAITS_TITLE_BIONIC : UI.CHARACTERCONTAINER_TRAITS_TITLE);
		for (int i = 1; i < this.stats.Traits.Count; i++)
		{
			Trait trait = this.stats.Traits[i];
			LocText locText = trait.PositiveTrait ? this.goodTrait : this.badTrait;
			LocText locText2 = Util.KInstantiateUI<LocText>(locText.gameObject, locText.transform.parent.gameObject, false);
			locText2.gameObject.SetActive(true);
			locText2.text = this.stats.Traits[i].GetName();
			locText2.color = (trait.PositiveTrait ? Constants.POSITIVE_COLOR : Constants.NEGATIVE_COLOR);
			locText2.GetComponent<ToolTip>().SetSimpleTooltip(trait.GetTooltip());
			for (int j = 0; j < trait.SelfModifiers.Count; j++)
			{
				GameObject gameObject = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject.SetActive(true);
				LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
				string format = (trait.SelfModifiers[j].Value > 0f) ? UI.CHARACTERCONTAINER_ATTRIBUTEMODIFIER_INCREASED : UI.CHARACTERCONTAINER_ATTRIBUTEMODIFIER_DECREASED;
				componentInChildren.text = string.Format(format, Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + trait.SelfModifiers[j].AttributeId.ToUpper() + ".NAME"));
				trait.SelfModifiers[j].AttributeId == "GermResistance";
				Klei.AI.Attribute attribute = Db.Get().Attributes.Get(trait.SelfModifiers[j].AttributeId);
				string text = attribute.Description;
				text = string.Concat(new string[]
				{
					text,
					"\n\n",
					Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + trait.SelfModifiers[j].AttributeId.ToUpper() + ".NAME"),
					": ",
					trait.SelfModifiers[j].GetFormattedString()
				});
				List<AttributeConverter> convertersForAttribute = Db.Get().AttributeConverters.GetConvertersForAttribute(attribute);
				for (int k = 0; k < convertersForAttribute.Count; k++)
				{
					string text2 = convertersForAttribute[k].DescriptionFromAttribute(convertersForAttribute[k].multiplier * trait.SelfModifiers[j].Value, null);
					if (text2 != "")
					{
						text = text + "\n    • " + text2;
					}
				}
				componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(text);
				this.traitEntries.Add(gameObject);
			}
			if (trait.disabledChoreGroups != null)
			{
				GameObject gameObject2 = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject2.SetActive(true);
				LocText componentInChildren2 = gameObject2.GetComponentInChildren<LocText>();
				componentInChildren2.text = trait.GetDisabledChoresString(false);
				string text3 = "";
				string text4 = "";
				for (int l = 0; l < trait.disabledChoreGroups.Length; l++)
				{
					if (l > 0)
					{
						text3 += ", ";
						text4 += "\n";
					}
					text3 += trait.disabledChoreGroups[l].Name;
					text4 += trait.disabledChoreGroups[l].description;
				}
				componentInChildren2.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(DUPLICANTS.TRAITS.CANNOT_DO_TASK_TOOLTIP, text3, text4));
				this.traitEntries.Add(gameObject2);
			}
			if (trait.ignoredEffects != null && trait.ignoredEffects.Length != 0)
			{
				GameObject gameObject3 = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject3.SetActive(true);
				LocText componentInChildren3 = gameObject3.GetComponentInChildren<LocText>();
				componentInChildren3.text = trait.GetIgnoredEffectsString(false);
				string text5 = "";
				for (int m = 0; m < trait.ignoredEffects.Length; m++)
				{
					if (m > 0)
					{
						text5 += "\n";
					}
					text5 += string.Format(DUPLICANTS.TRAITS.IGNORED_EFFECTS_TOOLTIP, Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + trait.ignoredEffects[m].ToUpper() + ".NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + trait.ignoredEffects[m].ToUpper() + ".CAUSE"));
					if (m < trait.ignoredEffects.Length - 1)
					{
						text5 += ",";
					}
				}
				componentInChildren3.GetComponent<ToolTip>().SetSimpleTooltip(text5);
				this.traitEntries.Add(gameObject3);
			}
			StringEntry stringEntry = null;
			if (trait.ShortDescCB != null || Strings.TryGet("STRINGS.DUPLICANTS.TRAITS." + trait.Id.ToUpper() + ".SHORT_DESC", out stringEntry))
			{
				string text6 = (trait.ShortDescCB != null) ? trait.ShortDescCB() : stringEntry.String;
				string simpleTooltip = (trait.ShortDescTooltipCB != null) ? trait.ShortDescTooltipCB() : Strings.Get("STRINGS.DUPLICANTS.TRAITS." + trait.Id.ToUpper() + ".SHORT_DESC_TOOLTIP");
				GameObject gameObject4 = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject4.SetActive(true);
				LocText componentInChildren4 = gameObject4.GetComponentInChildren<LocText>();
				componentInChildren4.text = text6;
				componentInChildren4.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
				this.traitEntries.Add(gameObject4);
			}
			this.traitEntries.Add(locText2.gameObject);
		}
		this.aptitudeEntries.ForEach(delegate(GameObject al)
		{
			UnityEngine.Object.Destroy(al.gameObject);
		});
		this.aptitudeEntries.Clear();
		this.expectationLabels.ForEach(delegate(LocText el)
		{
			UnityEngine.Object.Destroy(el.gameObject);
		});
		this.expectationLabels.Clear();
		if (this.stats.personality.model == GameTags.Minions.Models.Bionic)
		{
			this.aptitudeContainer.SetActive(false);
		}
		else
		{
			this.aptitudeContainer.SetActive(true);
			List<string> list = new List<string>();
			foreach (KeyValuePair<SkillGroup, float> keyValuePair in this.stats.skillAptitudes)
			{
				if (keyValuePair.Value != 0f)
				{
					SkillGroup skillGroup = Db.Get().SkillGroups.Get(keyValuePair.Key.IdHash);
					if (skillGroup == null)
					{
						global::Debug.LogWarningFormat("Role group not found for aptitude: {0}", new object[]
						{
							keyValuePair.Key
						});
					}
					else
					{
						GameObject gameObject5 = Util.KInstantiateUI(this.aptitudeEntry.gameObject, this.aptitudeContainer, false);
						LocText locText3 = Util.KInstantiateUI<LocText>(this.aptitudeLabel.gameObject, gameObject5, false);
						locText3.gameObject.SetActive(true);
						locText3.text = skillGroup.Name;
						string simpleTooltip2;
						if (skillGroup.choreGroupID != "")
						{
							ChoreGroup choreGroup = Db.Get().ChoreGroups.Get(skillGroup.choreGroupID);
							simpleTooltip2 = string.Format(DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION_CHOREGROUP, skillGroup.Name, DUPLICANTSTATS.APTITUDE_BONUS, choreGroup.description);
						}
						else
						{
							simpleTooltip2 = string.Format(DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION, skillGroup.Name, DUPLICANTSTATS.APTITUDE_BONUS);
						}
						locText3.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip2);
						string id = keyValuePair.Key.relevantAttributes[0].Id;
						float num = (float)this.stats.StartingLevels[id];
						LocText locText4 = Util.KInstantiateUI<LocText>(this.attributeLabelAptitude.gameObject, gameObject5, false);
						locText4.gameObject.SetActive(!list.Contains(id));
						locText4.text = "+" + num.ToString() + " " + keyValuePair.Key.relevantAttributes[0].Name;
						string text7 = keyValuePair.Key.relevantAttributes[0].Description;
						text7 = string.Concat(new string[]
						{
							text7,
							"\n\n",
							keyValuePair.Key.relevantAttributes[0].Name,
							": +",
							num.ToString()
						});
						List<AttributeConverter> convertersForAttribute2 = Db.Get().AttributeConverters.GetConvertersForAttribute(keyValuePair.Key.relevantAttributes[0]);
						for (int n = 0; n < convertersForAttribute2.Count; n++)
						{
							text7 = text7 + "\n    • " + convertersForAttribute2[n].DescriptionFromAttribute(convertersForAttribute2[n].multiplier * num, null);
						}
						list.Add(id);
						locText4.GetComponent<ToolTip>().SetSimpleTooltip(text7);
						gameObject5.gameObject.SetActive(true);
						this.aptitudeEntries.Add(gameObject5);
					}
				}
			}
		}
		if (this.stats.stressTrait != null)
		{
			LocText locText5 = Util.KInstantiateUI<LocText>(this.expectationRight.gameObject, this.expectationRight.transform.parent.gameObject, false);
			locText5.gameObject.SetActive(true);
			locText5.text = string.Format(UI.CHARACTERCONTAINER_STRESSTRAIT, this.stats.stressTrait.GetName());
			locText5.GetComponent<ToolTip>().SetSimpleTooltip(this.stats.stressTrait.GetTooltip());
			this.expectationLabels.Add(locText5);
		}
		if (this.stats.joyTrait != null)
		{
			LocText locText6 = Util.KInstantiateUI<LocText>(this.expectationRight.gameObject, this.expectationRight.transform.parent.gameObject, false);
			locText6.gameObject.SetActive(true);
			locText6.text = string.Format(UI.CHARACTERCONTAINER_JOYTRAIT, this.stats.joyTrait.GetName());
			locText6.GetComponent<ToolTip>().SetSimpleTooltip(this.stats.joyTrait.GetTooltip());
			this.expectationLabels.Add(locText6);
		}
		this.description.text = this.stats.personality.description;
	}

	// Token: 0x060090C8 RID: 37064 RVA: 0x00103171 File Offset: 0x00101371
	private IEnumerator SetAttributes()
	{
		yield return null;
		this.iconGroups.ForEach(delegate(GameObject icg)
		{
			UnityEngine.Object.Destroy(icg);
		});
		this.iconGroups.Clear();
		List<AttributeInstance> list = new List<AttributeInstance>(this.animController.gameObject.GetAttributes().AttributeTable);
		list.RemoveAll((AttributeInstance at) => at.Attribute.ShowInUI != Klei.AI.Attribute.Display.Skill);
		list = (from at in list
		orderby at.Name
		select at).ToList<AttributeInstance>();
		for (int i = 0; i < list.Count; i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.iconGroup.gameObject, this.iconGroup.transform.parent.gameObject, false);
			LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
			gameObject.SetActive(true);
			float totalValue = list[i].GetTotalValue();
			if (totalValue > 0f)
			{
				componentInChildren.color = Constants.POSITIVE_COLOR;
			}
			else if (totalValue == 0f)
			{
				componentInChildren.color = Constants.NEUTRAL_COLOR;
			}
			else
			{
				componentInChildren.color = Constants.NEGATIVE_COLOR;
			}
			componentInChildren.text = string.Format(UI.CHARACTERCONTAINER_SKILL_VALUE, GameUtil.AddPositiveSign(totalValue.ToString(), totalValue > 0f), list[i].Name);
			AttributeInstance attributeInstance = list[i];
			string text = attributeInstance.Description;
			if (attributeInstance.Attribute.converters.Count > 0)
			{
				text += "\n";
				foreach (AttributeConverter attributeConverter in attributeInstance.Attribute.converters)
				{
					AttributeConverterInstance converter = this.animController.gameObject.GetComponent<Klei.AI.AttributeConverters>().GetConverter(attributeConverter.Id);
					string text2 = converter.DescriptionFromAttribute(converter.Evaluate(), converter.gameObject);
					if (text2 != null)
					{
						text = text + "\n" + text2;
					}
				}
			}
			gameObject.GetComponent<ToolTip>().SetSimpleTooltip(text);
			this.iconGroups.Add(gameObject);
		}
		yield break;
	}

	// Token: 0x060090C9 RID: 37065 RVA: 0x0038A47C File Offset: 0x0038867C
	public void SelectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.AddDeliverable(this.stats);
		}
		if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
		{
			MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 1f, true);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetActive();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.DeselectDeliverable();
			if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
			{
				MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 0f, true);
			}
		};
		this.selectedBorder.SetActive(true);
		this.titleBar.color = this.selectedTitleColor;
		this.animController.Play("cheer_pre", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Play("cheer_loop", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x060090CA RID: 37066 RVA: 0x0038A564 File Offset: 0x00388764
	public void DeselectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.RemoveDeliverable(this.stats);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetInactive();
		this.selectButton.Deselect();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
		this.selectedBorder.SetActive(false);
		this.titleBar.color = this.deselectedTitleColor;
		this.animController.Queue("cheer_pst", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x060090CB RID: 37067 RVA: 0x00103180 File Offset: 0x00101380
	private void OnReplacedEvent(ITelepadDeliverable deliverable)
	{
		if (deliverable == this.stats)
		{
			this.DeselectDeliverable();
		}
	}

	// Token: 0x060090CC RID: 37068 RVA: 0x0038A62C File Offset: 0x0038882C
	private void OnCharacterSelectionLimitReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		if (this.controller.AllowsReplacing)
		{
			this.selectButton.onClick += this.ReplaceCharacterSelection;
			return;
		}
		this.selectButton.onClick += this.CantSelectCharacter;
	}

	// Token: 0x060090CD RID: 37069 RVA: 0x00102F16 File Offset: 0x00101116
	private void CantSelectCharacter()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x060090CE RID: 37070 RVA: 0x00103191 File Offset: 0x00101391
	private void ReplaceCharacterSelection()
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.RemoveLast();
		this.SelectDeliverable();
	}

	// Token: 0x060090CF RID: 37071 RVA: 0x0038A6A4 File Offset: 0x003888A4
	private void OnCharacterSelectionLimitUnReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
	}

	// Token: 0x060090D0 RID: 37072 RVA: 0x0038A6F8 File Offset: 0x003888F8
	public void SetReshufflingState(bool enable)
	{
		this.reshuffleButton.gameObject.SetActive(enable);
		this.archetypeDropDown.gameObject.SetActive(enable);
		this.modelDropDown.transform.parent.gameObject.SetActive(enable && Game.IsDlcActiveForCurrentSave("DLC3_ID"));
	}

	// Token: 0x060090D1 RID: 37073 RVA: 0x0038A754 File Offset: 0x00388954
	public void Reshuffle(bool is_starter)
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			this.DeselectDeliverable();
		}
		if (this.fxAnim != null)
		{
			this.fxAnim.Play("loop", KAnim.PlayMode.Once, 1f, 0f);
		}
		this.GenerateCharacter(is_starter, this.guaranteedAptitudeID);
	}

	// Token: 0x060090D2 RID: 37074 RVA: 0x0038A7C4 File Offset: 0x003889C4
	public void SetController(CharacterSelectionController csc)
	{
		if (csc == this.controller)
		{
			return;
		}
		this.controller = csc;
		CharacterSelectionController characterSelectionController = this.controller;
		characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Combine(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
		CharacterSelectionController characterSelectionController2 = this.controller;
		characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Combine(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
		CharacterSelectionController characterSelectionController3 = this.controller;
		characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Combine(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		CharacterSelectionController characterSelectionController4 = this.controller;
		characterSelectionController4.OnReplacedEvent = (Action<ITelepadDeliverable>)Delegate.Combine(characterSelectionController4.OnReplacedEvent, new Action<ITelepadDeliverable>(this.OnReplacedEvent));
	}

	// Token: 0x060090D3 RID: 37075 RVA: 0x0038A884 File Offset: 0x00388A84
	public void DisableSelectButton()
	{
		this.selectButton.soundPlayer.AcceptClickCondition = (() => false);
		this.selectButton.GetComponent<ImageToggleState>().SetDisabled();
		this.selectButton.soundPlayer.Enabled = false;
	}

	// Token: 0x060090D4 RID: 37076 RVA: 0x0038A8E4 File Offset: 0x00388AE4
	private bool IsCharacterInvalid()
	{
		return CharacterContainer.containers.Find((CharacterContainer container) => container != null && container.stats != null && container != this && container.stats.personality.Id == this.stats.personality.Id && container.stats.IsValid) != null || (Game.Instance != null && !Game.IsDlcActiveForCurrentSave(this.stats.personality.requiredDlcId)) || (this.stats.personality.model != GameTags.Minions.Models.Bionic && Components.LiveMinionIdentities.Items.Any((MinionIdentity id) => id.personalityResourceId == this.stats.personality.Id));
	}

	// Token: 0x060090D5 RID: 37077 RVA: 0x00102F93 File Offset: 0x00101193
	public string GetValueColor(bool isPositive)
	{
		if (!isPositive)
		{
			return "<color=#ff2222ff>";
		}
		return "<color=green>";
	}

	// Token: 0x060090D6 RID: 37078 RVA: 0x001031B3 File Offset: 0x001013B3
	public override void OnPointerEnter(PointerEventData eventData)
	{
		this.scroll_rect.mouseIsOver = true;
		base.OnPointerEnter(eventData);
	}

	// Token: 0x060090D7 RID: 37079 RVA: 0x001031C8 File Offset: 0x001013C8
	public override void OnPointerExit(PointerEventData eventData)
	{
		this.scroll_rect.mouseIsOver = false;
		base.OnPointerExit(eventData);
	}

	// Token: 0x060090D8 RID: 37080 RVA: 0x0038A974 File Offset: 0x00388B74
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.IsAction(global::Action.Escape) || e.IsAction(global::Action.MouseRight))
		{
			this.characterNameTitle.ForceStopEditing();
			this.controller.OnPressBack();
			this.archetypeDropDown.scrollRect.gameObject.SetActive(false);
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
			return;
		}
		if (this.archetypeDropDown.scrollRect.activeInHierarchy)
		{
			KScrollRect component = this.archetypeDropDown.scrollRect.GetComponent<KScrollRect>();
			Vector2 point = component.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
			if (component.rectTransform().rect.Contains(point))
			{
				component.mouseIsOver = true;
			}
			else
			{
				component.mouseIsOver = false;
			}
			component.OnKeyDown(e);
			return;
		}
		this.scroll_rect.OnKeyDown(e);
	}

	// Token: 0x060090D9 RID: 37081 RVA: 0x0038AA44 File Offset: 0x00388C44
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
			return;
		}
		if (this.archetypeDropDown.scrollRect.activeInHierarchy)
		{
			KScrollRect component = this.archetypeDropDown.scrollRect.GetComponent<KScrollRect>();
			Vector2 point = component.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
			if (component.rectTransform().rect.Contains(point))
			{
				component.mouseIsOver = true;
			}
			else
			{
				component.mouseIsOver = false;
			}
			component.OnKeyUp(e);
			return;
		}
		this.scroll_rect.OnKeyUp(e);
	}

	// Token: 0x060090DA RID: 37082 RVA: 0x001031DD File Offset: 0x001013DD
	protected override void OnCmpEnable()
	{
		base.OnActivate();
		if (this.stats == null)
		{
			return;
		}
		this.SetAnimator();
	}

	// Token: 0x060090DB RID: 37083 RVA: 0x001031F4 File Offset: 0x001013F4
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.characterNameTitle.ForceStopEditing();
	}

	// Token: 0x060090DC RID: 37084 RVA: 0x0038AAD4 File Offset: 0x00388CD4
	private void OnArchetypeEntryClick(IListableOption skill, object data)
	{
		if (skill != null)
		{
			SkillGroup skillGroup = skill as SkillGroup;
			this.guaranteedAptitudeID = skillGroup.Id;
			this.selectedArchetypeIcon.sprite = Assets.GetSprite(skillGroup.archetypeIcon);
			this.Reshuffle(true);
			return;
		}
		this.guaranteedAptitudeID = null;
		this.selectedArchetypeIcon.sprite = this.dropdownArrowIcon;
		this.Reshuffle(true);
	}

	// Token: 0x060090DD RID: 37085 RVA: 0x00103208 File Offset: 0x00101408
	private int archetypeDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		if (b.Equals("Random"))
		{
			return -1;
		}
		return b.GetProperName().CompareTo(a.GetProperName());
	}

	// Token: 0x060090DE RID: 37086 RVA: 0x0038AB3C File Offset: 0x00388D3C
	private void archetypeDropEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		if (entry.entryData != null)
		{
			SkillGroup skillGroup = entry.entryData as SkillGroup;
			entry.image.sprite = Assets.GetSprite(skillGroup.archetypeIcon);
		}
	}

	// Token: 0x060090DF RID: 37087 RVA: 0x0038AB78 File Offset: 0x00388D78
	private void OnModelEntryClick(IListableOption listItem, object data)
	{
		bool flag = false;
		if (listItem == null)
		{
			this.permittedModels = this.allMinionModels;
			this.selectedModelIcon.sprite = Assets.GetSprite(this.allModelSprite);
			this.Reshuffle(true);
		}
		else
		{
			CharacterContainer.MinionModelOption minionModelOption = listItem as CharacterContainer.MinionModelOption;
			if (minionModelOption != null)
			{
				flag = (minionModelOption.permittedModels.Count == 1 && minionModelOption.permittedModels[0] == GameTags.Minions.Models.Bionic);
				this.permittedModels = minionModelOption.permittedModels;
				this.selectedModelIcon.sprite = minionModelOption.sprite;
				this.Reshuffle(true);
			}
		}
		this.reshuffleButton.soundPlayer.widget_sound_events()[0].OverrideAssetName = (flag ? "DupeShuffle_bionic" : "DupeShuffle");
	}

	// Token: 0x060090E0 RID: 37088 RVA: 0x0010322A File Offset: 0x0010142A
	private int modelDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		return a.GetProperName().CompareTo(b.GetProperName());
	}

	// Token: 0x060090E1 RID: 37089 RVA: 0x0038AC3C File Offset: 0x00388E3C
	private void modelDropEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		if (entry.entryData != null)
		{
			CharacterContainer.MinionModelOption minionModelOption = entry.entryData as CharacterContainer.MinionModelOption;
			entry.image.sprite = minionModelOption.sprite;
		}
	}

	// Token: 0x04006D5B RID: 27995
	public const string SHUFFLE_BUTTON_DEFAULT_SOUND_NAME_ON_USE = "DupeShuffle";

	// Token: 0x04006D5C RID: 27996
	public const string SHUFFLE_BUTTON_BIONIC_SOUND_NAME_ON_USE = "DupeShuffle_bionic";

	// Token: 0x04006D5D RID: 27997
	[SerializeField]
	private GameObject contentBody;

	// Token: 0x04006D5E RID: 27998
	[SerializeField]
	private LocText characterName;

	// Token: 0x04006D5F RID: 27999
	[SerializeField]
	private EditableTitleBar characterNameTitle;

	// Token: 0x04006D60 RID: 28000
	[SerializeField]
	private LocText characterJob;

	// Token: 0x04006D61 RID: 28001
	[SerializeField]
	private LocText traitHeaderLabel;

	// Token: 0x04006D62 RID: 28002
	public GameObject selectedBorder;

	// Token: 0x04006D63 RID: 28003
	[SerializeField]
	private Image titleBar;

	// Token: 0x04006D64 RID: 28004
	[SerializeField]
	private Color selectedTitleColor;

	// Token: 0x04006D65 RID: 28005
	[SerializeField]
	private Color deselectedTitleColor;

	// Token: 0x04006D66 RID: 28006
	[SerializeField]
	private KButton reshuffleButton;

	// Token: 0x04006D67 RID: 28007
	private KBatchedAnimController animController;

	// Token: 0x04006D68 RID: 28008
	[SerializeField]
	private KBatchedAnimController bgAnimController;

	// Token: 0x04006D69 RID: 28009
	[SerializeField]
	private GameObject iconGroup;

	// Token: 0x04006D6A RID: 28010
	private List<GameObject> iconGroups;

	// Token: 0x04006D6B RID: 28011
	[SerializeField]
	private LocText goodTrait;

	// Token: 0x04006D6C RID: 28012
	[SerializeField]
	private LocText badTrait;

	// Token: 0x04006D6D RID: 28013
	[SerializeField]
	private GameObject aptitudeContainer;

	// Token: 0x04006D6E RID: 28014
	[SerializeField]
	private GameObject aptitudeEntry;

	// Token: 0x04006D6F RID: 28015
	[SerializeField]
	private Transform aptitudeLabel;

	// Token: 0x04006D70 RID: 28016
	[SerializeField]
	private Transform attributeLabelAptitude;

	// Token: 0x04006D71 RID: 28017
	[SerializeField]
	private Transform attributeLabelTrait;

	// Token: 0x04006D72 RID: 28018
	[SerializeField]
	private LocText expectationRight;

	// Token: 0x04006D73 RID: 28019
	private List<LocText> expectationLabels;

	// Token: 0x04006D74 RID: 28020
	[SerializeField]
	private DropDown archetypeDropDown;

	// Token: 0x04006D75 RID: 28021
	[SerializeField]
	private Image selectedArchetypeIcon;

	// Token: 0x04006D76 RID: 28022
	[SerializeField]
	private Sprite noArchetypeIcon;

	// Token: 0x04006D77 RID: 28023
	[SerializeField]
	private Sprite dropdownArrowIcon;

	// Token: 0x04006D78 RID: 28024
	private string guaranteedAptitudeID;

	// Token: 0x04006D79 RID: 28025
	private List<GameObject> aptitudeEntries;

	// Token: 0x04006D7A RID: 28026
	private List<GameObject> traitEntries;

	// Token: 0x04006D7B RID: 28027
	[SerializeField]
	private LocText description;

	// Token: 0x04006D7C RID: 28028
	[SerializeField]
	private Image selectedModelIcon;

	// Token: 0x04006D7D RID: 28029
	[SerializeField]
	private DropDown modelDropDown;

	// Token: 0x04006D7E RID: 28030
	private List<Tag> permittedModels = new List<Tag>
	{
		GameTags.Minions.Models.Standard,
		GameTags.Minions.Models.Bionic
	};

	// Token: 0x04006D7F RID: 28031
	[SerializeField]
	private KToggle selectButton;

	// Token: 0x04006D80 RID: 28032
	[SerializeField]
	private KBatchedAnimController fxAnim;

	// Token: 0x04006D81 RID: 28033
	private string allModelSprite = "ui_duplicant_any_selection";

	// Token: 0x04006D82 RID: 28034
	private static Dictionary<Tag, string> portraitBGAnims = new Dictionary<Tag, string>
	{
		{
			GameTags.Minions.Models.Standard,
			"crewselect_backdrop_kanim"
		},
		{
			GameTags.Minions.Models.Bionic,
			"updated_crewSelect_bionic_backdrop_kanim"
		}
	};

	// Token: 0x04006D83 RID: 28035
	private MinionStartingStats stats;

	// Token: 0x04006D84 RID: 28036
	private CharacterSelectionController controller;

	// Token: 0x04006D85 RID: 28037
	private static List<CharacterContainer> containers;

	// Token: 0x04006D86 RID: 28038
	private KAnimFile idle_anim;

	// Token: 0x04006D87 RID: 28039
	[HideInInspector]
	public bool addMinionToIdentityList = true;

	// Token: 0x04006D88 RID: 28040
	[SerializeField]
	private Sprite enabledSpr;

	// Token: 0x04006D89 RID: 28041
	[SerializeField]
	private KScrollRect scroll_rect;

	// Token: 0x04006D8A RID: 28042
	private static readonly Dictionary<HashedString, string[]> traitIdleAnims = new Dictionary<HashedString, string[]>
	{
		{
			"anim_idle_food_kanim",
			new string[]
			{
				"Foodie"
			}
		},
		{
			"anim_idle_animal_lover_kanim",
			new string[]
			{
				"RanchingUp"
			}
		},
		{
			"anim_idle_loner_kanim",
			new string[]
			{
				"Loner"
			}
		},
		{
			"anim_idle_mole_hands_kanim",
			new string[]
			{
				"MoleHands"
			}
		},
		{
			"anim_idle_buff_kanim",
			new string[]
			{
				"StrongArm"
			}
		},
		{
			"anim_idle_distracted_kanim",
			new string[]
			{
				"CantResearch",
				"CantBuild",
				"CantCook",
				"CantDig"
			}
		},
		{
			"anim_idle_coaster_kanim",
			new string[]
			{
				"HappySinger"
			}
		}
	};

	// Token: 0x04006D8B RID: 28043
	private List<Tag> allMinionModels = new List<Tag>
	{
		GameTags.Minions.Models.Standard,
		GameTags.Minions.Models.Bionic
	};

	// Token: 0x04006D8C RID: 28044
	private static readonly HashedString[] idleAnims = new HashedString[]
	{
		"anim_idle_healthy_kanim",
		"anim_idle_susceptible_kanim",
		"anim_idle_keener_kanim",
		"anim_idle_fastfeet_kanim",
		"anim_idle_breatherdeep_kanim",
		"anim_idle_breathershallow_kanim"
	};

	// Token: 0x04006D8D RID: 28045
	public float baseCharacterScale = 0.38f;

	// Token: 0x02001B04 RID: 6916
	[Serializable]
	public struct ProfessionIcon
	{
		// Token: 0x04006D8E RID: 28046
		public string professionName;

		// Token: 0x04006D8F RID: 28047
		public Sprite iconImg;
	}

	// Token: 0x02001B05 RID: 6917
	private class MinionModelOption : IListableOption
	{
		// Token: 0x060090EC RID: 37100 RVA: 0x001032A3 File Offset: 0x001014A3
		public MinionModelOption(string name, List<Tag> permittedModels, Sprite sprite)
		{
			this.properName = name;
			this.permittedModels = permittedModels;
			this.sprite = sprite;
		}

		// Token: 0x060090ED RID: 37101 RVA: 0x001032C0 File Offset: 0x001014C0
		public string GetProperName()
		{
			return this.properName;
		}

		// Token: 0x04006D90 RID: 28048
		private string properName;

		// Token: 0x04006D91 RID: 28049
		public List<Tag> permittedModels;

		// Token: 0x04006D92 RID: 28050
		public Sprite sprite;
	}
}
