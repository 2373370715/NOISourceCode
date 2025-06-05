using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002013 RID: 8211
public class ReceptacleSideScreen : SideScreenContent, IRender1000ms
{
	// Token: 0x0600ADC0 RID: 44480 RVA: 0x00423B40 File Offset: 0x00421D40
	public override string GetTitle()
	{
		if (this.targetReceptacle == null)
		{
			return Strings.Get(this.titleKey).ToString().Replace("{0}", "");
		}
		return string.Format(Strings.Get(this.titleKey), this.targetReceptacle.GetProperName());
	}

	// Token: 0x0600ADC1 RID: 44481 RVA: 0x00423B9C File Offset: 0x00421D9C
	public void Initialize(SingleEntityReceptacle target)
	{
		if (target == null)
		{
			global::Debug.LogError("SingleObjectReceptacle provided was null.");
			return;
		}
		this.targetReceptacle = target;
		base.gameObject.SetActive(true);
		this.depositObjectMap = new Dictionary<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity>();
		this.entityToggles.ForEach(delegate(ReceptacleToggle rbi)
		{
			UnityEngine.Object.Destroy(rbi.gameObject);
		});
		this.entityToggles.Clear();
		foreach (Tag tag in this.targetReceptacle.possibleDepositObjectTags)
		{
			List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(tag);
			int num = prefabsWithTag.Count;
			List<IHasSortOrder> list = new List<IHasSortOrder>();
			foreach (GameObject gameObject in prefabsWithTag)
			{
				if (!this.targetReceptacle.IsValidEntity(gameObject))
				{
					num--;
				}
				else
				{
					IHasSortOrder component = gameObject.GetComponent<IHasSortOrder>();
					if (component != null)
					{
						list.Add(component);
					}
				}
			}
			global::Debug.Assert(list.Count == num, "Not all entities in this receptacle implement IHasSortOrder!");
			list.Sort((IHasSortOrder a, IHasSortOrder b) => a.sortOrder - b.sortOrder);
			foreach (IHasSortOrder hasSortOrder in list)
			{
				GameObject gameObject2 = (hasSortOrder as MonoBehaviour).gameObject;
				GameObject gameObject3 = Util.KInstantiateUI(this.entityToggle, this.requestObjectList, false);
				gameObject3.SetActive(true);
				ReceptacleToggle newToggle = gameObject3.GetComponent<ReceptacleToggle>();
				IReceptacleDirection component2 = gameObject2.GetComponent<IReceptacleDirection>();
				string entityName = this.GetEntityName(gameObject2.PrefabID());
				newToggle.title.text = entityName;
				Sprite entityIcon = this.GetEntityIcon(gameObject2.PrefabID());
				if (entityIcon == null)
				{
					entityIcon = this.elementPlaceholderSpr;
				}
				newToggle.image.sprite = entityIcon;
				newToggle.toggle.onClick += delegate()
				{
					this.ToggleClicked(newToggle);
				};
				newToggle.toggle.onPointerEnter += delegate()
				{
					this.CheckAmountsAndUpdate(null);
				};
				ToolTip component3 = newToggle.GetComponent<ToolTip>();
				if (component3 != null)
				{
					component3.SetSimpleTooltip(this.GetEntityTooltip(gameObject2.PrefabID()));
				}
				this.depositObjectMap.Add(newToggle, new ReceptacleSideScreen.SelectableEntity
				{
					tag = gameObject2.PrefabID(),
					direction = ((component2 != null) ? component2.Direction : SingleEntityReceptacle.ReceptacleDirection.Top),
					asset = gameObject2
				});
				this.entityToggles.Add(newToggle);
			}
		}
		this.RestoreSelectionFromOccupant();
		this.selectedEntityToggle = null;
		if (this.entityToggles.Count > 0)
		{
			if (this.entityPreviousSelectionMap.ContainsKey(this.targetReceptacle))
			{
				int index = this.entityPreviousSelectionMap[this.targetReceptacle];
				this.ToggleClicked(this.entityToggles[index]);
			}
			else
			{
				this.subtitleLabel.SetText(Strings.Get(this.subtitleStringSelect).ToString());
				this.requestSelectedEntityBtn.isInteractable = false;
				this.descriptionLabel.SetText(Strings.Get(this.subtitleStringSelectDescription).ToString());
				this.HideAllDescriptorPanels();
			}
		}
		this.onStorageChangedHandle = this.targetReceptacle.gameObject.Subscribe(-1697596308, new Action<object>(this.CheckAmountsAndUpdate));
		this.onOccupantValidChangedHandle = this.targetReceptacle.gameObject.Subscribe(-1820564715, new Action<object>(this.OnOccupantValidChanged));
		this.UpdateState(null);
		SimAndRenderScheduler.instance.Add(this, false);
	}

	// Token: 0x0600ADC2 RID: 44482 RVA: 0x00423FBC File Offset: 0x004221BC
	protected virtual void UpdateState(object data)
	{
		this.requestSelectedEntityBtn.ClearOnClick();
		if (this.targetReceptacle == null)
		{
			return;
		}
		if (this.CheckReceptacleOccupied())
		{
			Uprootable uprootable = this.targetReceptacle.Occupant.GetComponent<Uprootable>();
			if (uprootable != null && uprootable.IsMarkedForUproot)
			{
				this.requestSelectedEntityBtn.onClick += delegate()
				{
					uprootable.ForceCancelUproot(null);
					this.UpdateState(null);
				};
				this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringCancelRemove).ToString();
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingRemoval).ToString(), this.targetReceptacle.Occupant.GetProperName()));
			}
			else
			{
				this.requestSelectedEntityBtn.onClick += delegate()
				{
					this.targetReceptacle.OrderRemoveOccupant();
					this.UpdateState(null);
				};
				this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringRemove).ToString();
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringEntityDeposited).ToString(), this.targetReceptacle.Occupant.GetProperName()));
			}
			this.requestSelectedEntityBtn.isInteractable = true;
			this.ToggleObjectPicker(false);
			Tag tag = this.targetReceptacle.Occupant.GetComponent<KSelectable>().PrefabID();
			this.ConfigureActiveEntity(tag);
			this.SetResultDescriptions(this.targetReceptacle.Occupant);
		}
		else if (this.targetReceptacle.GetActiveRequest != null)
		{
			this.requestSelectedEntityBtn.onClick += delegate()
			{
				this.targetReceptacle.CancelActiveRequest();
				this.ClearSelection();
				this.UpdateAvailableAmounts(null);
				this.UpdateState(null);
			};
			this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringCancelDeposit).ToString();
			this.requestSelectedEntityBtn.isInteractable = true;
			this.ToggleObjectPicker(false);
			this.ConfigureActiveEntity(this.targetReceptacle.GetActiveRequest.tagsFirst);
			GameObject prefab = Assets.GetPrefab(this.targetReceptacle.GetActiveRequest.tagsFirst);
			if (prefab != null)
			{
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingDelivery).ToString(), prefab.GetProperName()));
				this.SetResultDescriptions(prefab);
			}
		}
		else if (this.selectedEntityToggle != null)
		{
			this.requestSelectedEntityBtn.onClick += delegate()
			{
				this.targetReceptacle.CreateOrder(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag);
				this.UpdateAvailableAmounts(null);
				this.UpdateState(null);
			};
			this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringDeposit).ToString();
			this.targetReceptacle.SetPreview(this.depositObjectMap[this.selectedEntityToggle].tag, false);
			bool flag = this.CanDepositEntity(this.depositObjectMap[this.selectedEntityToggle]);
			this.requestSelectedEntityBtn.isInteractable = flag;
			this.SetImageToggleState(this.selectedEntityToggle.toggle, flag ? ImageToggleState.State.Active : ImageToggleState.State.DisabledActive);
			this.ToggleObjectPicker(true);
			GameObject prefab2 = Assets.GetPrefab(this.selectedDepositObjectTag);
			if (prefab2 != null)
			{
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingSelection).ToString(), prefab2.GetProperName()));
				this.SetResultDescriptions(prefab2);
			}
		}
		else
		{
			this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringDeposit).ToString();
			this.requestSelectedEntityBtn.isInteractable = false;
			this.ToggleObjectPicker(true);
		}
		this.UpdateAvailableAmounts(null);
		this.UpdateListeners();
	}

	// Token: 0x0600ADC3 RID: 44483 RVA: 0x0042433C File Offset: 0x0042253C
	private void UpdateListeners()
	{
		if (this.CheckReceptacleOccupied())
		{
			if (this.onObjectDestroyedHandle == -1)
			{
				this.onObjectDestroyedHandle = this.targetReceptacle.Occupant.gameObject.Subscribe(1969584890, delegate(object d)
				{
					this.UpdateState(null);
				});
				return;
			}
		}
		else if (this.onObjectDestroyedHandle != -1)
		{
			this.onObjectDestroyedHandle = -1;
		}
	}

	// Token: 0x0600ADC4 RID: 44484 RVA: 0x00424398 File Offset: 0x00422598
	private void OnOccupantValidChanged(object obj)
	{
		if (this.targetReceptacle == null)
		{
			return;
		}
		if (!this.CheckReceptacleOccupied() && this.targetReceptacle.GetActiveRequest != null)
		{
			bool flag = false;
			ReceptacleSideScreen.SelectableEntity entity;
			if (this.depositObjectMap.TryGetValue(this.selectedEntityToggle, out entity))
			{
				flag = this.CanDepositEntity(entity);
			}
			if (!flag)
			{
				this.targetReceptacle.CancelActiveRequest();
				this.ClearSelection();
				this.UpdateState(null);
				this.UpdateAvailableAmounts(null);
			}
		}
	}

	// Token: 0x0600ADC5 RID: 44485 RVA: 0x00115597 File Offset: 0x00113797
	private bool CanDepositEntity(ReceptacleSideScreen.SelectableEntity entity)
	{
		return this.ValidRotationForDeposit(entity.direction) && (!this.RequiresAvailableAmountToDeposit() || this.GetAvailableAmount(entity.tag) > 0f) && this.AdditionalCanDepositTest();
	}

	// Token: 0x0600ADC6 RID: 44486 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	protected virtual bool AdditionalCanDepositTest()
	{
		return true;
	}

	// Token: 0x0600ADC7 RID: 44487 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	protected virtual bool RequiresAvailableAmountToDeposit()
	{
		return true;
	}

	// Token: 0x0600ADC8 RID: 44488 RVA: 0x0042440C File Offset: 0x0042260C
	private void ClearSelection()
	{
		foreach (KeyValuePair<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> keyValuePair in this.depositObjectMap)
		{
			keyValuePair.Key.toggle.Deselect();
		}
	}

	// Token: 0x0600ADC9 RID: 44489 RVA: 0x0042446C File Offset: 0x0042266C
	private void ToggleObjectPicker(bool Show)
	{
		this.requestObjectListContainer.SetActive(Show);
		if (this.scrollBarContainer != null)
		{
			this.scrollBarContainer.SetActive(Show);
		}
		this.requestObjectList.SetActive(Show);
		this.activeEntityContainer.SetActive(!Show);
	}

	// Token: 0x0600ADCA RID: 44490 RVA: 0x004244BC File Offset: 0x004226BC
	private void ConfigureActiveEntity(Tag tag)
	{
		string properName = Assets.GetPrefab(tag).GetProperName();
		this.activeEntityContainer.GetComponentInChildrenOnly<LocText>().text = properName;
		this.activeEntityContainer.transform.GetChild(0).gameObject.GetComponentInChildrenOnly<Image>().sprite = this.GetEntityIcon(tag);
	}

	// Token: 0x0600ADCB RID: 44491 RVA: 0x001155CA File Offset: 0x001137CA
	protected virtual string GetEntityName(Tag prefabTag)
	{
		return Assets.GetPrefab(prefabTag).GetProperName();
	}

	// Token: 0x0600ADCC RID: 44492 RVA: 0x00424510 File Offset: 0x00422710
	protected virtual string GetEntityTooltip(Tag prefabTag)
	{
		InfoDescription component = Assets.GetPrefab(prefabTag).GetComponent<InfoDescription>();
		string text = this.GetEntityName(prefabTag);
		if (component != null)
		{
			text = text + "\n\n" + component.description;
		}
		return text;
	}

	// Token: 0x0600ADCD RID: 44493 RVA: 0x001147CE File Offset: 0x001129CE
	protected virtual Sprite GetEntityIcon(Tag prefabTag)
	{
		return Def.GetUISprite(Assets.GetPrefab(prefabTag), "ui", false).first;
	}

	// Token: 0x0600ADCE RID: 44494 RVA: 0x00424550 File Offset: 0x00422750
	public override bool IsValidForTarget(GameObject target)
	{
		SingleEntityReceptacle component = target.GetComponent<SingleEntityReceptacle>();
		return component != null && component.enabled && target.GetComponent<PlantablePlot>() == null && target.GetComponent<EggIncubator>() == null && target.GetComponent<SpecialCargoBayClusterReceptacle>() == null;
	}

	// Token: 0x0600ADCF RID: 44495 RVA: 0x004245A0 File Offset: 0x004227A0
	public override void SetTarget(GameObject target)
	{
		SingleEntityReceptacle component = target.GetComponent<SingleEntityReceptacle>();
		if (component == null)
		{
			global::Debug.LogError("The object selected doesn't have a SingleObjectReceptacle!");
			return;
		}
		this.Initialize(component);
		this.UpdateState(null);
	}

	// Token: 0x0600ADD0 RID: 44496 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void RestoreSelectionFromOccupant()
	{
	}

	// Token: 0x0600ADD1 RID: 44497 RVA: 0x004245D8 File Offset: 0x004227D8
	public override void ClearTarget()
	{
		if (this.targetReceptacle != null)
		{
			if (this.CheckReceptacleOccupied())
			{
				this.targetReceptacle.Occupant.gameObject.Unsubscribe(this.onObjectDestroyedHandle);
				this.onObjectDestroyedHandle = -1;
			}
			this.targetReceptacle.Unsubscribe(this.onStorageChangedHandle);
			this.onStorageChangedHandle = -1;
			this.targetReceptacle.Unsubscribe(this.onOccupantValidChangedHandle);
			this.onOccupantValidChangedHandle = -1;
			if (this.targetReceptacle.GetActiveRequest == null)
			{
				this.targetReceptacle.SetPreview(Tag.Invalid, false);
			}
			SimAndRenderScheduler.instance.Remove(this);
			this.targetReceptacle = null;
		}
	}

	// Token: 0x0600ADD2 RID: 44498 RVA: 0x00424680 File Offset: 0x00422880
	protected void SetImageToggleState(KToggle toggle, ImageToggleState.State state)
	{
		switch (state)
		{
		case ImageToggleState.State.Disabled:
			toggle.GetComponent<ImageToggleState>().SetDisabled();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.desaturatedMaterial;
			return;
		case ImageToggleState.State.Inactive:
			toggle.GetComponent<ImageToggleState>().SetInactive();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.defaultMaterial;
			return;
		case ImageToggleState.State.Active:
			toggle.GetComponent<ImageToggleState>().SetActive();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.defaultMaterial;
			return;
		case ImageToggleState.State.DisabledActive:
			toggle.GetComponent<ImageToggleState>().SetDisabledActive();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.desaturatedMaterial;
			return;
		default:
			return;
		}
	}

	// Token: 0x0600ADD3 RID: 44499 RVA: 0x001155D7 File Offset: 0x001137D7
	public void Render1000ms(float dt)
	{
		this.CheckAmountsAndUpdate(null);
	}

	// Token: 0x0600ADD4 RID: 44500 RVA: 0x001155E0 File Offset: 0x001137E0
	private void CheckAmountsAndUpdate(object data)
	{
		if (this.targetReceptacle == null)
		{
			return;
		}
		if (this.UpdateAvailableAmounts(null))
		{
			this.UpdateState(null);
		}
	}

	// Token: 0x0600ADD5 RID: 44501 RVA: 0x0042472C File Offset: 0x0042292C
	private bool UpdateAvailableAmounts(object data)
	{
		bool result = false;
		foreach (KeyValuePair<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> keyValuePair in this.depositObjectMap)
		{
			if (!DebugHandler.InstantBuildMode && this.hideUndiscoveredEntities && !DiscoveredResources.Instance.IsDiscovered(keyValuePair.Value.tag))
			{
				keyValuePair.Key.gameObject.SetActive(false);
			}
			else if (!keyValuePair.Key.gameObject.activeSelf)
			{
				keyValuePair.Key.gameObject.SetActive(true);
			}
			float availableAmount = this.GetAvailableAmount(keyValuePair.Value.tag);
			if (keyValuePair.Value.lastAmount != availableAmount)
			{
				result = true;
				keyValuePair.Value.lastAmount = availableAmount;
				keyValuePair.Key.amount.text = availableAmount.ToString();
			}
			if (!this.ValidRotationForDeposit(keyValuePair.Value.direction) || availableAmount <= 0f)
			{
				if (this.selectedEntityToggle != keyValuePair.Key)
				{
					this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.Disabled);
				}
				else
				{
					this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.DisabledActive);
				}
			}
			else if (this.selectedEntityToggle != keyValuePair.Key)
			{
				this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.Inactive);
			}
			else
			{
				this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.Active);
			}
		}
		return result;
	}

	// Token: 0x0600ADD6 RID: 44502 RVA: 0x004248CC File Offset: 0x00422ACC
	protected float GetAvailableAmount(Tag tag)
	{
		if (this.ALLOW_ORDER_IGNORING_WOLRD_NEED)
		{
			IEnumerable<Pickupable> pickupables = this.targetReceptacle.GetMyWorld().worldInventory.GetPickupables(tag, true);
			float num = 0f;
			foreach (Pickupable pickupable in pickupables)
			{
				num += (float)Mathf.CeilToInt(pickupable.TotalAmount);
			}
			return num;
		}
		return this.targetReceptacle.GetMyWorld().worldInventory.GetAmount(tag, true);
	}

	// Token: 0x0600ADD7 RID: 44503 RVA: 0x00115601 File Offset: 0x00113801
	private bool ValidRotationForDeposit(SingleEntityReceptacle.ReceptacleDirection depositDir)
	{
		return this.targetReceptacle.rotatable == null || depositDir == this.targetReceptacle.Direction;
	}

	// Token: 0x0600ADD8 RID: 44504 RVA: 0x0042495C File Offset: 0x00422B5C
	protected virtual void ToggleClicked(ReceptacleToggle toggle)
	{
		if (!this.depositObjectMap.ContainsKey(toggle))
		{
			global::Debug.LogError("Recipe not found on recipe list.");
			return;
		}
		if (this.selectedEntityToggle != null)
		{
			bool flag = this.CanDepositEntity(this.depositObjectMap[this.selectedEntityToggle]);
			this.requestSelectedEntityBtn.isInteractable = flag;
			this.SetImageToggleState(this.selectedEntityToggle.toggle, flag ? ImageToggleState.State.Inactive : ImageToggleState.State.Disabled);
		}
		this.selectedEntityToggle = toggle;
		this.entityPreviousSelectionMap[this.targetReceptacle] = this.entityToggles.IndexOf(toggle);
		this.selectedDepositObjectTag = this.depositObjectMap[toggle].tag;
		MutantPlant component = this.depositObjectMap[toggle].asset.GetComponent<MutantPlant>();
		this.selectedDepositObjectAdditionalTag = (component ? component.SubSpeciesID : Tag.Invalid);
		this.UpdateAvailableAmounts(null);
		this.UpdateState(null);
	}

	// Token: 0x0600ADD9 RID: 44505 RVA: 0x00115626 File Offset: 0x00113826
	private void CreateOrder(bool isInfinite)
	{
		this.targetReceptacle.CreateOrder(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag);
	}

	// Token: 0x0600ADDA RID: 44506 RVA: 0x0011563F File Offset: 0x0011383F
	protected bool CheckReceptacleOccupied()
	{
		return this.targetReceptacle != null && this.targetReceptacle.Occupant != null;
	}

	// Token: 0x0600ADDB RID: 44507 RVA: 0x00424A48 File Offset: 0x00422C48
	protected virtual void SetResultDescriptions(GameObject go)
	{
		string text = "";
		InfoDescription component = go.GetComponent<InfoDescription>();
		if (component)
		{
			text = component.description;
		}
		else
		{
			KPrefabID component2 = go.GetComponent<KPrefabID>();
			if (component2 != null)
			{
				Element element = ElementLoader.GetElement(component2.PrefabID());
				if (element != null)
				{
					text = element.Description();
				}
			}
			else
			{
				text = go.GetProperName();
			}
		}
		this.descriptionLabel.SetText(text);
	}

	// Token: 0x0600ADDC RID: 44508 RVA: 0x00424AB0 File Offset: 0x00422CB0
	protected virtual void HideAllDescriptorPanels()
	{
		for (int i = 0; i < this.descriptorPanels.Count; i++)
		{
			this.descriptorPanels[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x040088B9 RID: 35001
	protected bool ALLOW_ORDER_IGNORING_WOLRD_NEED = true;

	// Token: 0x040088BA RID: 35002
	[SerializeField]
	protected KButton requestSelectedEntityBtn;

	// Token: 0x040088BB RID: 35003
	[SerializeField]
	private string requestStringDeposit;

	// Token: 0x040088BC RID: 35004
	[SerializeField]
	private string requestStringCancelDeposit;

	// Token: 0x040088BD RID: 35005
	[SerializeField]
	private string requestStringRemove;

	// Token: 0x040088BE RID: 35006
	[SerializeField]
	private string requestStringCancelRemove;

	// Token: 0x040088BF RID: 35007
	public GameObject activeEntityContainer;

	// Token: 0x040088C0 RID: 35008
	public GameObject nothingDiscoveredContainer;

	// Token: 0x040088C1 RID: 35009
	[SerializeField]
	protected LocText descriptionLabel;

	// Token: 0x040088C2 RID: 35010
	protected Dictionary<SingleEntityReceptacle, int> entityPreviousSelectionMap = new Dictionary<SingleEntityReceptacle, int>();

	// Token: 0x040088C3 RID: 35011
	[SerializeField]
	private string subtitleStringSelect;

	// Token: 0x040088C4 RID: 35012
	[SerializeField]
	private string subtitleStringSelectDescription;

	// Token: 0x040088C5 RID: 35013
	[SerializeField]
	private string subtitleStringAwaitingSelection;

	// Token: 0x040088C6 RID: 35014
	[SerializeField]
	private string subtitleStringAwaitingDelivery;

	// Token: 0x040088C7 RID: 35015
	[SerializeField]
	private string subtitleStringEntityDeposited;

	// Token: 0x040088C8 RID: 35016
	[SerializeField]
	private string subtitleStringAwaitingRemoval;

	// Token: 0x040088C9 RID: 35017
	[SerializeField]
	private LocText subtitleLabel;

	// Token: 0x040088CA RID: 35018
	[SerializeField]
	private List<DescriptorPanel> descriptorPanels;

	// Token: 0x040088CB RID: 35019
	public Material defaultMaterial;

	// Token: 0x040088CC RID: 35020
	public Material desaturatedMaterial;

	// Token: 0x040088CD RID: 35021
	[SerializeField]
	private GameObject requestObjectList;

	// Token: 0x040088CE RID: 35022
	[SerializeField]
	private GameObject requestObjectListContainer;

	// Token: 0x040088CF RID: 35023
	[SerializeField]
	private GameObject scrollBarContainer;

	// Token: 0x040088D0 RID: 35024
	[SerializeField]
	private GameObject entityToggle;

	// Token: 0x040088D1 RID: 35025
	[SerializeField]
	private Sprite buttonSelectedBG;

	// Token: 0x040088D2 RID: 35026
	[SerializeField]
	private Sprite buttonNormalBG;

	// Token: 0x040088D3 RID: 35027
	[SerializeField]
	private Sprite elementPlaceholderSpr;

	// Token: 0x040088D4 RID: 35028
	[SerializeField]
	private bool hideUndiscoveredEntities;

	// Token: 0x040088D5 RID: 35029
	protected ReceptacleToggle selectedEntityToggle;

	// Token: 0x040088D6 RID: 35030
	protected SingleEntityReceptacle targetReceptacle;

	// Token: 0x040088D7 RID: 35031
	protected Tag selectedDepositObjectTag;

	// Token: 0x040088D8 RID: 35032
	protected Tag selectedDepositObjectAdditionalTag;

	// Token: 0x040088D9 RID: 35033
	protected Dictionary<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> depositObjectMap;

	// Token: 0x040088DA RID: 35034
	protected List<ReceptacleToggle> entityToggles = new List<ReceptacleToggle>();

	// Token: 0x040088DB RID: 35035
	private int onObjectDestroyedHandle = -1;

	// Token: 0x040088DC RID: 35036
	private int onOccupantValidChangedHandle = -1;

	// Token: 0x040088DD RID: 35037
	private int onStorageChangedHandle = -1;

	// Token: 0x02002014 RID: 8212
	protected class SelectableEntity
	{
		// Token: 0x040088DE RID: 35038
		public Tag tag;

		// Token: 0x040088DF RID: 35039
		public SingleEntityReceptacle.ReceptacleDirection direction;

		// Token: 0x040088E0 RID: 35040
		public GameObject asset;

		// Token: 0x040088E1 RID: 35041
		public float lastAmount = -1f;
	}
}
