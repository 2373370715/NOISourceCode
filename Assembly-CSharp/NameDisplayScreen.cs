using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001EA1 RID: 7841
public class NameDisplayScreen : KScreen
{
	// Token: 0x0600A454 RID: 42068 RVA: 0x0010F131 File Offset: 0x0010D331
	public static void DestroyInstance()
	{
		NameDisplayScreen.Instance = null;
	}

	// Token: 0x0600A455 RID: 42069 RVA: 0x0010F139 File Offset: 0x0010D339
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NameDisplayScreen.Instance = this;
	}

	// Token: 0x0600A456 RID: 42070 RVA: 0x0010F147 File Offset: 0x0010D347
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Health.Register(new Action<Health>(this.OnHealthAdded), null);
		Components.Equipment.Register(new Action<Equipment>(this.OnEquipmentAdded), null);
		this.BindOnOverlayChange();
	}

	// Token: 0x0600A457 RID: 42071 RVA: 0x003F509C File Offset: 0x003F329C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.isOverlayChangeBound && OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			this.isOverlayChangeBound = false;
		}
	}

	// Token: 0x0600A458 RID: 42072 RVA: 0x003F50F4 File Offset: 0x003F32F4
	private void BindOnOverlayChange()
	{
		if (this.isOverlayChangeBound)
		{
			return;
		}
		if (OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			this.isOverlayChangeBound = true;
		}
	}

	// Token: 0x0600A459 RID: 42073 RVA: 0x003F5144 File Offset: 0x003F3344
	public void RemoveWorldEntries(int worldId)
	{
		this.entries.RemoveAll((NameDisplayScreen.Entry entry) => entry.world_go.IsNullOrDestroyed() || entry.world_go.GetMyWorldId() == worldId);
	}

	// Token: 0x0600A45A RID: 42074 RVA: 0x0010F183 File Offset: 0x0010D383
	private void OnOverlayChanged(HashedString new_mode)
	{
		HashedString hashedString = this.lastKnownOverlayID;
		this.lastKnownOverlayID = new_mode;
		this.nameDisplayCanvas.enabled = (this.lastKnownOverlayID == OverlayModes.None.ID);
	}

	// Token: 0x0600A45B RID: 42075 RVA: 0x0010F1AE File Offset: 0x0010D3AE
	private void OnHealthAdded(Health health)
	{
		this.RegisterComponent(health.gameObject, health, false);
	}

	// Token: 0x0600A45C RID: 42076 RVA: 0x003F5178 File Offset: 0x003F3378
	private void OnEquipmentAdded(Equipment equipment)
	{
		MinionAssignablesProxy component = equipment.GetComponent<MinionAssignablesProxy>();
		GameObject targetGameObject = component.GetTargetGameObject();
		if (targetGameObject)
		{
			this.RegisterComponent(targetGameObject, equipment, false);
			return;
		}
		global::Debug.LogWarningFormat("OnEquipmentAdded proxy target {0} was null.", new object[]
		{
			component.TargetInstanceID
		});
	}

	// Token: 0x0600A45D RID: 42077 RVA: 0x003F51C4 File Offset: 0x003F33C4
	private bool ShouldShowName(GameObject representedObject)
	{
		CharacterOverlay component = representedObject.GetComponent<CharacterOverlay>();
		return component != null && component.shouldShowName;
	}

	// Token: 0x0600A45E RID: 42078 RVA: 0x003F51EC File Offset: 0x003F33EC
	public Guid AddAreaText(string initialText, GameObject prefab)
	{
		NameDisplayScreen.TextEntry textEntry = new NameDisplayScreen.TextEntry();
		textEntry.guid = Guid.NewGuid();
		textEntry.display_go = Util.KInstantiateUI(prefab, this.areaTextDisplayCanvas.gameObject, true);
		textEntry.display_go.GetComponentInChildren<LocText>().text = initialText;
		this.textEntries.Add(textEntry);
		return textEntry.guid;
	}

	// Token: 0x0600A45F RID: 42079 RVA: 0x003F5248 File Offset: 0x003F3448
	public GameObject GetWorldText(Guid guid)
	{
		GameObject result = null;
		foreach (NameDisplayScreen.TextEntry textEntry in this.textEntries)
		{
			if (textEntry.guid == guid)
			{
				result = textEntry.display_go;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600A460 RID: 42080 RVA: 0x003F52B0 File Offset: 0x003F34B0
	public void RemoveWorldText(Guid guid)
	{
		int num = -1;
		for (int i = 0; i < this.textEntries.Count; i++)
		{
			if (this.textEntries[i].guid == guid)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			UnityEngine.Object.Destroy(this.textEntries[num].display_go);
			this.textEntries.RemoveAt(num);
		}
	}

	// Token: 0x0600A461 RID: 42081 RVA: 0x003F5318 File Offset: 0x003F3518
	public void AddNewEntry(GameObject representedObject)
	{
		NameDisplayScreen.Entry entry = new NameDisplayScreen.Entry();
		entry.world_go = representedObject;
		entry.world_go_anim_controller = representedObject.GetComponent<KAnimControllerBase>();
		GameObject original = this.ShouldShowName(representedObject) ? this.nameAndBarsPrefab : this.barsPrefab;
		entry.kprfabID = representedObject.GetComponent<KPrefabID>();
		entry.collider = representedObject.GetComponent<KBoxCollider2D>();
		GameObject gameObject = Util.KInstantiateUI(original, this.nameDisplayCanvas.gameObject, true);
		entry.display_go = gameObject;
		entry.display_go_rect = gameObject.GetComponent<RectTransform>();
		entry.nameLabel = entry.display_go.GetComponentInChildren<LocText>();
		entry.display_go.SetActive(false);
		if (this.worldSpace)
		{
			entry.display_go.transform.localScale = Vector3.one * 0.01f;
		}
		gameObject.name = representedObject.name + " character overlay";
		entry.Name = representedObject.name;
		entry.refs = gameObject.GetComponent<HierarchyReferences>();
		this.entries.Add(entry);
		UnityEngine.Object component = representedObject.GetComponent<KSelectable>();
		FactionAlignment component2 = representedObject.GetComponent<FactionAlignment>();
		if (component != null)
		{
			if (component2 != null)
			{
				if (component2.Alignment == FactionManager.FactionID.Friendly || component2.Alignment == FactionManager.FactionID.Duplicant)
				{
					this.UpdateName(representedObject);
					return;
				}
			}
			else
			{
				this.UpdateName(representedObject);
			}
		}
	}

	// Token: 0x0600A462 RID: 42082 RVA: 0x003F5450 File Offset: 0x003F3650
	public void RegisterComponent(GameObject representedObject, object component, bool force_new_entry = false)
	{
		NameDisplayScreen.Entry entry = force_new_entry ? null : this.GetEntry(representedObject);
		if (entry == null)
		{
			CharacterOverlay component2 = representedObject.GetComponent<CharacterOverlay>();
			if (component2 != null)
			{
				component2.Register();
				entry = this.GetEntry(representedObject);
			}
		}
		if (entry == null)
		{
			return;
		}
		Transform reference = entry.refs.GetReference<Transform>("Bars");
		entry.bars_go = reference.gameObject;
		if (component is Health)
		{
			if (!entry.healthBar)
			{
				Health health = (Health)component;
				GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.healthBarPrefab, reference.gameObject, false);
				gameObject.name = "Health Bar";
				health.healthBar = gameObject.GetComponent<HealthBar>();
				health.healthBar.GetComponent<KSelectable>().entityName = UI.METERS.HEALTH.TOOLTIP;
				health.healthBar.GetComponent<KSelectableHealthBar>().IsSelectable = (representedObject.GetComponent<MinionBrain>() != null);
				entry.healthBar = health.healthBar;
				entry.healthBar.autoHide = false;
				gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
				return;
			}
			global::Debug.LogWarningFormat("Health added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is OxygenBreather)
		{
			if (!entry.breathBar)
			{
				GameObject gameObject2 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.breathBar = gameObject2.GetComponent<ProgressBar>();
				entry.breathBar.autoHide = false;
				gameObject2.gameObject.GetComponent<ToolTip>().AddMultiStringTooltip("Breath", this.ToolTipStyle_Property);
				gameObject2.name = "Breath Bar";
				gameObject2.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("BreathBar");
				gameObject2.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
				return;
			}
			global::Debug.LogWarningFormat("OxygenBreather added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is BionicOxygenTankMonitor.Instance)
		{
			if (!entry.bionicOxygenTankBar)
			{
				GameObject gameObject3 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.bionicOxygenTankBar = gameObject3.GetComponent<ProgressBar>();
				entry.bionicOxygenTankBar.autoHide = false;
				gameObject3.name = "Bionic Oxygen Tank Bar";
				gameObject3.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("OxygenTankBar");
				gameObject3.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
				return;
			}
			global::Debug.LogWarningFormat("BionicOxygenTankBar added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is Equipment)
		{
			if (!entry.suitBar)
			{
				GameObject gameObject4 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.suitBar = gameObject4.GetComponent<ProgressBar>();
				entry.suitBar.autoHide = false;
				gameObject4.name = "Suit Tank Bar";
				gameObject4.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("OxygenTankBar");
				gameObject4.GetComponent<KSelectable>().entityName = UI.METERS.BREATH.TOOLTIP;
			}
			else
			{
				global::Debug.LogWarningFormat("SuitBar added twice {0}", new object[]
				{
					component
				});
			}
			if (!entry.suitFuelBar)
			{
				GameObject gameObject5 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.suitFuelBar = gameObject5.GetComponent<ProgressBar>();
				entry.suitFuelBar.autoHide = false;
				gameObject5.name = "Suit Fuel Bar";
				gameObject5.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("FuelTankBar");
				gameObject5.GetComponent<KSelectable>().entityName = UI.METERS.FUEL.TOOLTIP;
			}
			else
			{
				global::Debug.LogWarningFormat("FuelBar added twice {0}", new object[]
				{
					component
				});
			}
			if (!entry.suitBatteryBar)
			{
				GameObject gameObject6 = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
				entry.suitBatteryBar = gameObject6.GetComponent<ProgressBar>();
				entry.suitBatteryBar.autoHide = false;
				gameObject6.name = "Suit Battery Bar";
				gameObject6.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("BatteryBar");
				gameObject6.GetComponent<KSelectable>().entityName = UI.METERS.BATTERY.TOOLTIP;
				return;
			}
			global::Debug.LogWarningFormat("CoolantBar added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else if (component is ThoughtGraph.Instance || component is CreatureThoughtGraph.Instance)
		{
			if (!entry.thoughtBubble)
			{
				GameObject gameObject7 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubble, entry.display_go, false);
				entry.thoughtBubble = gameObject7.GetComponent<HierarchyReferences>();
				gameObject7.name = ((component is CreatureThoughtGraph.Instance) ? "Creature " : "") + "Thought Bubble";
				GameObject gameObject8 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubbleConvo, entry.display_go, false);
				entry.thoughtBubbleConvo = gameObject8.GetComponent<HierarchyReferences>();
				gameObject8.name = ((component is CreatureThoughtGraph.Instance) ? "Creature " : "") + "Thought Bubble Convo";
				return;
			}
			global::Debug.LogWarningFormat("ThoughtGraph added twice {0}", new object[]
			{
				component
			});
			return;
		}
		else
		{
			if (!(component is GameplayEventMonitor.Instance))
			{
				if (component is Dreamer.Instance && !entry.dreamBubble)
				{
					GameObject gameObject9 = Util.KInstantiateUI(EffectPrefabs.Instance.DreamBubble, entry.display_go, false);
					gameObject9.name = "Dream Bubble";
					entry.dreamBubble = gameObject9.GetComponent<DreamBubble>();
				}
				return;
			}
			if (!entry.gameplayEventDisplay)
			{
				GameObject gameObject10 = Util.KInstantiateUI(EffectPrefabs.Instance.GameplayEventDisplay, entry.display_go, false);
				entry.gameplayEventDisplay = gameObject10.GetComponent<HierarchyReferences>();
				gameObject10.name = "Gameplay Event Display";
				return;
			}
			global::Debug.LogWarningFormat("GameplayEventDisplay added twice {0}", new object[]
			{
				component
			});
			return;
		}
	}

	// Token: 0x0600A463 RID: 42083 RVA: 0x0010F1BE File Offset: 0x0010D3BE
	public bool IsVisibleToZoom()
	{
		return !(Game.MainCamera == null) && Game.MainCamera.orthographicSize < this.HideDistance;
	}

	// Token: 0x0600A464 RID: 42084 RVA: 0x003F5A60 File Offset: 0x003F3C60
	private void LateUpdate()
	{
		if (App.isLoading || App.IsExiting)
		{
			return;
		}
		this.BindOnOverlayChange();
		if (Game.MainCamera == null)
		{
			return;
		}
		if (this.lastKnownOverlayID != OverlayModes.None.ID)
		{
			return;
		}
		int count = this.entries.Count;
		bool flag = this.IsVisibleToZoom();
		bool flag2 = flag && this.lastKnownOverlayID == OverlayModes.None.ID;
		if (this.nameDisplayCanvas.enabled != flag2)
		{
			this.nameDisplayCanvas.enabled = flag2;
		}
		if (flag)
		{
			this.RemoveDestroyedEntries();
			this.Culling();
			this.UpdatePos();
			this.HideDeadProgressBars();
		}
	}

	// Token: 0x0600A465 RID: 42085 RVA: 0x003F5B00 File Offset: 0x003F3D00
	private void Culling()
	{
		if (this.entries.Count == 0)
		{
			return;
		}
		Vector2I vector2I;
		Vector2I vector2I2;
		Grid.GetVisibleCellRangeInActiveWorld(out vector2I, out vector2I2, 4, 1.5f);
		int num = Mathf.Min(500, this.entries.Count);
		for (int i = 0; i < num; i++)
		{
			int index = (this.currentUpdateIndex + i) % this.entries.Count;
			NameDisplayScreen.Entry entry = this.entries[index];
			Vector3 position = entry.world_go.transform.GetPosition();
			bool flag = position.x >= (float)vector2I.x && position.y >= (float)vector2I.y && position.x < (float)vector2I2.x && position.y < (float)vector2I2.y;
			if (entry.visible != flag)
			{
				entry.display_go.SetActive(flag);
			}
			entry.visible = flag;
		}
		this.currentUpdateIndex = (this.currentUpdateIndex + num) % this.entries.Count;
	}

	// Token: 0x0600A466 RID: 42086 RVA: 0x003F5C0C File Offset: 0x003F3E0C
	private void UpdatePos()
	{
		CameraController instance = CameraController.Instance;
		Transform followTarget = instance.followTarget;
		int count = this.entries.Count;
		for (int i = 0; i < count; i++)
		{
			NameDisplayScreen.Entry entry = this.entries[i];
			if (entry.visible)
			{
				GameObject world_go = entry.world_go;
				if (!(world_go == null))
				{
					Vector3 vector = world_go.transform.GetPosition();
					if (followTarget == world_go.transform)
					{
						vector = instance.followTargetPos;
					}
					else if (entry.world_go_anim_controller != null && entry.collider != null)
					{
						vector.x += entry.collider.offset.x;
						vector.y += entry.collider.offset.y - entry.collider.size.y / 2f;
					}
					entry.display_go_rect.anchoredPosition = (this.worldSpace ? vector : base.WorldToScreen(vector));
				}
			}
		}
	}

	// Token: 0x0600A467 RID: 42087 RVA: 0x003F5D30 File Offset: 0x003F3F30
	private void RemoveDestroyedEntries()
	{
		int num = this.entries.Count;
		int i = 0;
		while (i < num)
		{
			if (this.entries[i].world_go == null)
			{
				UnityEngine.Object.Destroy(this.entries[i].display_go);
				num--;
				this.entries[i] = this.entries[num];
			}
			else
			{
				i++;
			}
		}
		this.entries.RemoveRange(num, this.entries.Count - num);
	}

	// Token: 0x0600A468 RID: 42088 RVA: 0x003F5DBC File Offset: 0x003F3FBC
	private void HideDeadProgressBars()
	{
		int count = this.entries.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.entries[i].visible && !(this.entries[i].world_go == null) && this.entries[i].kprfabID.HasTag(GameTags.Dead) && this.entries[i].bars_go.activeSelf)
			{
				this.entries[i].bars_go.SetActive(false);
			}
		}
	}

	// Token: 0x0600A469 RID: 42089 RVA: 0x003F5E5C File Offset: 0x003F405C
	public void UpdateName(GameObject representedObject)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(representedObject);
		if (entry == null)
		{
			return;
		}
		KSelectable component = representedObject.GetComponent<KSelectable>();
		entry.display_go.name = component.GetProperName() + " character overlay";
		if (entry.nameLabel != null)
		{
			entry.nameLabel.text = component.GetProperName();
			if (representedObject.GetComponent<RocketModule>() != null)
			{
				entry.nameLabel.text = representedObject.GetComponent<RocketModule>().GetParentRocketName();
			}
		}
	}

	// Token: 0x0600A46A RID: 42090 RVA: 0x003F5EDC File Offset: 0x003F40DC
	public void SetDream(GameObject minion_go, Dream dream)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.dreamBubble == null)
		{
			return;
		}
		entry.dreamBubble.SetDream(dream);
		entry.dreamBubble.GetComponent<KSelectable>().entityName = "Dreaming";
		entry.dreamBubble.gameObject.SetActive(true);
		entry.dreamBubble.SetVisibility(true);
	}

	// Token: 0x0600A46B RID: 42091 RVA: 0x003F5F44 File Offset: 0x003F4144
	public void StopDreaming(GameObject minion_go)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.dreamBubble == null)
		{
			return;
		}
		entry.dreamBubble.StopDreaming();
		entry.dreamBubble.gameObject.SetActive(false);
	}

	// Token: 0x0600A46C RID: 42092 RVA: 0x003F5F88 File Offset: 0x003F4188
	public void DreamTick(GameObject minion_go, float dt)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.dreamBubble == null)
		{
			return;
		}
		entry.dreamBubble.Tick(dt);
	}

	// Token: 0x0600A46D RID: 42093 RVA: 0x003F5FBC File Offset: 0x003F41BC
	public void SetThoughtBubbleDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite bubble_sprite, Sprite topic_sprite)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.thoughtBubble == null)
		{
			return;
		}
		this.ApplyThoughtSprite(entry.thoughtBubble, bubble_sprite, "bubble_sprite");
		this.ApplyThoughtSprite(entry.thoughtBubble, topic_sprite, "icon_sprite");
		entry.thoughtBubble.GetComponent<KSelectable>().entityName = hover_text;
		entry.thoughtBubble.gameObject.SetActive(bVisible);
	}

	// Token: 0x0600A46E RID: 42094 RVA: 0x003F602C File Offset: 0x003F422C
	public void SetThoughtBubbleConvoDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite bubble_sprite, Sprite topic_sprite, Sprite mode_sprite)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.thoughtBubble == null)
		{
			return;
		}
		this.ApplyThoughtSprite(entry.thoughtBubbleConvo, bubble_sprite, "bubble_sprite");
		this.ApplyThoughtSprite(entry.thoughtBubbleConvo, topic_sprite, "icon_sprite");
		this.ApplyThoughtSprite(entry.thoughtBubbleConvo, mode_sprite, "icon_sprite_mode");
		entry.thoughtBubbleConvo.GetComponent<KSelectable>().entityName = hover_text;
		entry.thoughtBubbleConvo.gameObject.SetActive(bVisible);
	}

	// Token: 0x0600A46F RID: 42095 RVA: 0x0010F1E1 File Offset: 0x0010D3E1
	private void ApplyThoughtSprite(HierarchyReferences active_bubble, Sprite sprite, string target)
	{
		active_bubble.GetReference<Image>(target).sprite = sprite;
	}

	// Token: 0x0600A470 RID: 42096 RVA: 0x003F60B0 File Offset: 0x003F42B0
	public void SetGameplayEventDisplay(GameObject minion_go, bool bVisible, string hover_text, Sprite sprite)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.gameplayEventDisplay == null)
		{
			return;
		}
		entry.gameplayEventDisplay.GetReference<Image>("icon_sprite").sprite = sprite;
		entry.gameplayEventDisplay.GetComponent<KSelectable>().entityName = hover_text;
		entry.gameplayEventDisplay.gameObject.SetActive(bVisible);
	}

	// Token: 0x0600A471 RID: 42097 RVA: 0x003F6110 File Offset: 0x003F4310
	public void SetBreathDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.breathBar == null)
		{
			return;
		}
		entry.breathBar.SetUpdateFunc(updatePercentFull);
		entry.breathBar.SetVisibility(bVisible);
	}

	// Token: 0x0600A472 RID: 42098 RVA: 0x003F6150 File Offset: 0x003F4350
	public void SetHealthDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.healthBar == null)
		{
			return;
		}
		entry.healthBar.OnChange();
		entry.healthBar.SetUpdateFunc(updatePercentFull);
		if (entry.healthBar.gameObject.activeSelf != bVisible)
		{
			entry.healthBar.SetVisibility(bVisible);
		}
	}

	// Token: 0x0600A473 RID: 42099 RVA: 0x003F61B0 File Offset: 0x003F43B0
	public void SetSuitTankDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.suitBar == null)
		{
			return;
		}
		entry.suitBar.SetUpdateFunc(updatePercentFull);
		entry.suitBar.SetVisibility(bVisible);
	}

	// Token: 0x0600A474 RID: 42100 RVA: 0x003F61F0 File Offset: 0x003F43F0
	public void SetBionicOxygenTankDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.bionicOxygenTankBar == null)
		{
			return;
		}
		entry.bionicOxygenTankBar.SetUpdateFunc(updatePercentFull);
		entry.bionicOxygenTankBar.SetVisibility(bVisible);
	}

	// Token: 0x0600A475 RID: 42101 RVA: 0x003F6230 File Offset: 0x003F4430
	public void SetSuitFuelDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.suitFuelBar == null)
		{
			return;
		}
		entry.suitFuelBar.SetUpdateFunc(updatePercentFull);
		entry.suitFuelBar.SetVisibility(bVisible);
	}

	// Token: 0x0600A476 RID: 42102 RVA: 0x003F6270 File Offset: 0x003F4470
	public void SetSuitBatteryDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
	{
		NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
		if (entry == null || entry.suitBatteryBar == null)
		{
			return;
		}
		entry.suitBatteryBar.SetUpdateFunc(updatePercentFull);
		entry.suitBatteryBar.SetVisibility(bVisible);
	}

	// Token: 0x0600A477 RID: 42103 RVA: 0x003F62B0 File Offset: 0x003F44B0
	private NameDisplayScreen.Entry GetEntry(GameObject worldObject)
	{
		return this.entries.Find((NameDisplayScreen.Entry entry) => entry.world_go == worldObject);
	}

	// Token: 0x0400807A RID: 32890
	[SerializeField]
	private float HideDistance;

	// Token: 0x0400807B RID: 32891
	public static NameDisplayScreen Instance;

	// Token: 0x0400807C RID: 32892
	[SerializeField]
	private Canvas nameDisplayCanvas;

	// Token: 0x0400807D RID: 32893
	[SerializeField]
	private Canvas areaTextDisplayCanvas;

	// Token: 0x0400807E RID: 32894
	public GameObject nameAndBarsPrefab;

	// Token: 0x0400807F RID: 32895
	public GameObject barsPrefab;

	// Token: 0x04008080 RID: 32896
	public TextStyleSetting ToolTipStyle_Property;

	// Token: 0x04008081 RID: 32897
	[SerializeField]
	private Color selectedColor;

	// Token: 0x04008082 RID: 32898
	[SerializeField]
	private Color defaultColor;

	// Token: 0x04008083 RID: 32899
	public int fontsize_min = 14;

	// Token: 0x04008084 RID: 32900
	public int fontsize_max = 32;

	// Token: 0x04008085 RID: 32901
	public float cameraDistance_fontsize_min = 6f;

	// Token: 0x04008086 RID: 32902
	public float cameraDistance_fontsize_max = 4f;

	// Token: 0x04008087 RID: 32903
	public List<NameDisplayScreen.Entry> entries = new List<NameDisplayScreen.Entry>();

	// Token: 0x04008088 RID: 32904
	public List<NameDisplayScreen.TextEntry> textEntries = new List<NameDisplayScreen.TextEntry>();

	// Token: 0x04008089 RID: 32905
	public bool worldSpace = true;

	// Token: 0x0400808A RID: 32906
	private bool isOverlayChangeBound;

	// Token: 0x0400808B RID: 32907
	private HashedString lastKnownOverlayID = OverlayModes.None.ID;

	// Token: 0x0400808C RID: 32908
	private int currentUpdateIndex;

	// Token: 0x02001EA2 RID: 7842
	[Serializable]
	public class Entry
	{
		// Token: 0x0400808D RID: 32909
		public string Name;

		// Token: 0x0400808E RID: 32910
		public bool visible;

		// Token: 0x0400808F RID: 32911
		public GameObject world_go;

		// Token: 0x04008090 RID: 32912
		public GameObject display_go;

		// Token: 0x04008091 RID: 32913
		public GameObject bars_go;

		// Token: 0x04008092 RID: 32914
		public KPrefabID kprfabID;

		// Token: 0x04008093 RID: 32915
		public KBoxCollider2D collider;

		// Token: 0x04008094 RID: 32916
		public KAnimControllerBase world_go_anim_controller;

		// Token: 0x04008095 RID: 32917
		public RectTransform display_go_rect;

		// Token: 0x04008096 RID: 32918
		public LocText nameLabel;

		// Token: 0x04008097 RID: 32919
		public HealthBar healthBar;

		// Token: 0x04008098 RID: 32920
		public ProgressBar breathBar;

		// Token: 0x04008099 RID: 32921
		public ProgressBar suitBar;

		// Token: 0x0400809A RID: 32922
		public ProgressBar bionicOxygenTankBar;

		// Token: 0x0400809B RID: 32923
		public ProgressBar suitFuelBar;

		// Token: 0x0400809C RID: 32924
		public ProgressBar suitBatteryBar;

		// Token: 0x0400809D RID: 32925
		public DreamBubble dreamBubble;

		// Token: 0x0400809E RID: 32926
		public HierarchyReferences thoughtBubble;

		// Token: 0x0400809F RID: 32927
		public HierarchyReferences thoughtBubbleConvo;

		// Token: 0x040080A0 RID: 32928
		public HierarchyReferences gameplayEventDisplay;

		// Token: 0x040080A1 RID: 32929
		public HierarchyReferences refs;
	}

	// Token: 0x02001EA3 RID: 7843
	public class TextEntry
	{
		// Token: 0x040080A2 RID: 32930
		public Guid guid;

		// Token: 0x040080A3 RID: 32931
		public GameObject display_go;
	}
}
