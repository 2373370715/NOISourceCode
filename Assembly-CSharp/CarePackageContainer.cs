using System;
using System.Collections;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001AFE RID: 6910
public class CarePackageContainer : KScreen, ITelepadDeliverableContainer
{
	// Token: 0x06009083 RID: 36995 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x17000998 RID: 2456
	// (get) Token: 0x06009084 RID: 36996 RVA: 0x00102E5F File Offset: 0x0010105F
	public CarePackageInfo Info
	{
		get
		{
			return this.info;
		}
	}

	// Token: 0x06009085 RID: 36997 RVA: 0x00102E67 File Offset: 0x00101067
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Initialize();
		base.StartCoroutine(this.DelayedGeneration());
	}

	// Token: 0x06009086 RID: 36998 RVA: 0x00102E82 File Offset: 0x00101082
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x06009087 RID: 36999 RVA: 0x00102E89 File Offset: 0x00101089
	private IEnumerator DelayedGeneration()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		if (this.controller != null)
		{
			this.GenerateCharacter(this.controller.IsStarterMinion);
		}
		yield break;
	}

	// Token: 0x06009088 RID: 37000 RVA: 0x00102E98 File Offset: 0x00101098
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.animController != null)
		{
			this.animController.gameObject.DeleteObject();
			this.animController = null;
		}
	}

	// Token: 0x06009089 RID: 37001 RVA: 0x0038842C File Offset: 0x0038662C
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

	// Token: 0x0600908A RID: 37002 RVA: 0x00102EC5 File Offset: 0x001010C5
	private void Initialize()
	{
		this.professionIconMap = new Dictionary<string, Sprite>();
		this.professionIcons.ForEach(delegate(CarePackageContainer.ProfessionIcon ic)
		{
			this.professionIconMap.Add(ic.professionName, ic.iconImg);
		});
		if (CarePackageContainer.containers == null)
		{
			CarePackageContainer.containers = new List<ITelepadDeliverableContainer>();
		}
		CarePackageContainer.containers.Add(this);
	}

	// Token: 0x0600908B RID: 37003 RVA: 0x003884C4 File Offset: 0x003866C4
	private void GenerateCharacter(bool is_starter)
	{
		int num = 0;
		do
		{
			this.info = Immigration.Instance.RandomCarePackage();
			num++;
		}
		while (this.IsCharacterRedundant() && num < 20);
		if (this.animController != null)
		{
			UnityEngine.Object.Destroy(this.animController.gameObject);
			this.animController = null;
		}
		this.carePackageInstanceData = new CarePackageContainer.CarePackageInstanceData();
		this.carePackageInstanceData.info = this.info;
		if (this.info.facadeID == "SELECTRANDOM")
		{
			this.carePackageInstanceData.facadeID = Db.GetEquippableFacades().resources.FindAll((EquippableFacadeResource match) => match.DefID == this.info.id).GetRandom<EquippableFacadeResource>().Id;
		}
		else
		{
			this.carePackageInstanceData.facadeID = this.info.facadeID;
		}
		this.SetAnimator();
		this.SetInfoText();
		this.selectButton.ClearOnClick();
		if (!this.controller.IsStarterMinion)
		{
			this.selectButton.onClick += delegate()
			{
				this.SelectDeliverable();
			};
		}
	}

	// Token: 0x0600908C RID: 37004 RVA: 0x003885D0 File Offset: 0x003867D0
	private void SetAnimator()
	{
		GameObject prefab = Assets.GetPrefab(this.info.id.ToTag());
		EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(this.info.id);
		int num;
		if (ElementLoader.FindElementByName(this.info.id) != null)
		{
			num = 1;
		}
		else if (foodInfo != null)
		{
			num = (int)(this.info.quantity % foodInfo.CaloriesPerUnit);
		}
		else
		{
			num = (int)this.info.quantity;
		}
		if (prefab != null)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Util.KInstantiateUI(this.contentBody, this.contentBody.transform.parent.gameObject, false);
				gameObject.SetActive(true);
				Image component = gameObject.GetComponent<Image>();
				global::Tuple<Sprite, Color> uisprite;
				if (!this.carePackageInstanceData.facadeID.IsNullOrWhiteSpace())
				{
					uisprite = Def.GetUISprite(prefab.PrefabID(), this.carePackageInstanceData.facadeID);
				}
				else
				{
					uisprite = Def.GetUISprite(prefab, "ui", false);
				}
				component.sprite = uisprite.first;
				component.color = uisprite.second;
				this.entryIcons.Add(gameObject);
				if (num > 1)
				{
					int num2;
					int num3;
					int num4;
					if (num % 2 == 1)
					{
						num2 = Mathf.CeilToInt((float)(num / 2));
						num3 = num2 - i;
						num4 = ((num3 > 0) ? 1 : -1);
						num3 = Mathf.Abs(num3);
					}
					else
					{
						num2 = num / 2 - 1;
						if (i <= num2)
						{
							num3 = Mathf.Abs(num2 - i);
							num4 = -1;
						}
						else
						{
							num3 = Mathf.Abs(num2 + 1 - i);
							num4 = 1;
						}
					}
					int num5 = 0;
					if (num % 2 == 0)
					{
						num5 = ((i <= num2) ? -6 : 6);
						gameObject.transform.SetPosition(gameObject.transform.position += new Vector3((float)num5, 0f, 0f));
					}
					gameObject.transform.localScale = new Vector3(1f - (float)num3 * 0.1f, 1f - (float)num3 * 0.1f, 1f);
					gameObject.transform.Rotate(0f, 0f, 3f * (float)num3 * (float)num4);
					gameObject.transform.SetPosition(gameObject.transform.position + new Vector3(25f * (float)num3 * (float)num4, 5f * (float)num3) + new Vector3((float)num5, 0f, 0f));
					gameObject.GetComponent<Canvas>().sortingOrder = num - num3;
				}
			}
			return;
		}
		GameObject gameObject2 = Util.KInstantiateUI(this.contentBody, this.contentBody.transform.parent.gameObject, false);
		gameObject2.SetActive(true);
		Image component2 = gameObject2.GetComponent<Image>();
		component2.sprite = Def.GetUISpriteFromMultiObjectAnim(ElementLoader.GetElement(this.info.id.ToTag()).substance.anim, "ui", false, "");
		component2.color = ElementLoader.GetElement(this.info.id.ToTag()).substance.uiColour;
		this.entryIcons.Add(gameObject2);
	}

	// Token: 0x0600908D RID: 37005 RVA: 0x00388900 File Offset: 0x00386B00
	private string GetSpawnableName()
	{
		GameObject prefab = Assets.GetPrefab(this.info.id);
		if (prefab == null)
		{
			Element element = ElementLoader.FindElementByName(this.info.id);
			if (element != null)
			{
				return element.substance.name;
			}
			return "";
		}
		else
		{
			if (string.IsNullOrEmpty(this.carePackageInstanceData.facadeID))
			{
				return prefab.GetProperName();
			}
			return EquippableFacade.GetNameOverride(this.carePackageInstanceData.info.id, this.carePackageInstanceData.facadeID);
		}
	}

	// Token: 0x0600908E RID: 37006 RVA: 0x0038898C File Offset: 0x00386B8C
	private string GetSpawnableQuantityOnly()
	{
		if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, GameUtil.GetFormattedMass(this.info.quantity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		if (EdiblesManager.GetFoodInfo(this.info.id) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, GameUtil.GetFormattedCaloriesForItem(this.info.id, this.info.quantity, GameUtil.TimeSlice.None, true));
		}
		return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, this.info.quantity.ToString());
	}

	// Token: 0x0600908F RID: 37007 RVA: 0x00388A40 File Offset: 0x00386C40
	private string GetCurrentQuantity(WorldInventory inventory)
	{
		if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
		{
			float amount = inventory.GetAmount(this.info.id.ToTag(), false);
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		if (EdiblesManager.GetFoodInfo(this.info.id) != null)
		{
			float calories = WorldResourceAmountTracker<RationTracker>.Get().CountAmountForItemWithID(this.info.id, inventory, true);
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true));
		}
		float amount2 = inventory.GetAmount(this.info.id.ToTag(), false);
		return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, amount2.ToString());
	}

	// Token: 0x06009090 RID: 37008 RVA: 0x00388B0C File Offset: 0x00386D0C
	private string GetSpawnableQuantity()
	{
		if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_QUANTITY, GameUtil.GetFormattedMass(this.info.quantity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), Assets.GetPrefab(this.info.id).GetProperName());
		}
		if (EdiblesManager.GetFoodInfo(this.info.id) != null)
		{
			return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_QUANTITY, GameUtil.GetFormattedCaloriesForItem(this.info.id, this.info.quantity, GameUtil.TimeSlice.None, true), Assets.GetPrefab(this.info.id).GetProperName());
		}
		return string.Format(UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT, Assets.GetPrefab(this.info.id).GetProperName(), this.info.quantity.ToString());
	}

	// Token: 0x06009091 RID: 37009 RVA: 0x00388C0C File Offset: 0x00386E0C
	private string GetSpawnableDescription()
	{
		Element element = ElementLoader.GetElement(this.info.id.ToTag());
		if (element != null)
		{
			return element.Description();
		}
		GameObject prefab = Assets.GetPrefab(this.info.id);
		if (prefab == null)
		{
			return "";
		}
		InfoDescription component = prefab.GetComponent<InfoDescription>();
		if (component != null)
		{
			return component.description;
		}
		return prefab.GetProperName();
	}

	// Token: 0x06009092 RID: 37010 RVA: 0x00388C7C File Offset: 0x00386E7C
	private string GetSpawnableEffects()
	{
		GameObject prefab = Assets.GetPrefab(this.info.id);
		if (prefab == null)
		{
			return "";
		}
		string text = "";
		IGameObjectEffectDescriptor[] components = prefab.GetComponents<IGameObjectEffectDescriptor>();
		if (components != null)
		{
			IGameObjectEffectDescriptor[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				List<Descriptor> descriptors = array[i].GetDescriptors(prefab);
				if (descriptors != null)
				{
					foreach (Descriptor descriptor in descriptors)
					{
						text = text + descriptor.text + "\n";
					}
				}
			}
		}
		return text;
	}

	// Token: 0x06009093 RID: 37011 RVA: 0x00388D34 File Offset: 0x00386F34
	private void SetInfoText()
	{
		this.characterName.SetText(this.GetSpawnableName());
		this.effects.SetText(this.GetSpawnableEffects());
		this.description.SetText(this.GetSpawnableDescription());
		this.itemName.SetText(this.GetSpawnableName());
		this.quantity.SetText(this.GetSpawnableQuantityOnly());
		this.currentQuantity.SetText(this.GetCurrentQuantity(ClusterManager.Instance.activeWorld.worldInventory));
	}

	// Token: 0x06009094 RID: 37012 RVA: 0x00388DB8 File Offset: 0x00386FB8
	public void SelectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.AddDeliverable(this.carePackageInstanceData);
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
	}

	// Token: 0x06009095 RID: 37013 RVA: 0x00388E60 File Offset: 0x00387060
	public void DeselectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.RemoveDeliverable(this.carePackageInstanceData);
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
	}

	// Token: 0x06009096 RID: 37014 RVA: 0x00102F05 File Offset: 0x00101105
	private void OnReplacedEvent(ITelepadDeliverable stats)
	{
		if (stats == this.carePackageInstanceData)
		{
			this.DeselectDeliverable();
		}
	}

	// Token: 0x06009097 RID: 37015 RVA: 0x00388EE8 File Offset: 0x003870E8
	private void OnCharacterSelectionLimitReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.info))
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

	// Token: 0x06009098 RID: 37016 RVA: 0x00102F16 File Offset: 0x00101116
	private void CantSelectCharacter()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x06009099 RID: 37017 RVA: 0x00102F28 File Offset: 0x00101128
	private void ReplaceCharacterSelection()
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.RemoveLast();
		this.SelectDeliverable();
	}

	// Token: 0x0600909A RID: 37018 RVA: 0x00388F60 File Offset: 0x00387160
	private void OnCharacterSelectionLimitUnReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.info))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
	}

	// Token: 0x0600909B RID: 37019 RVA: 0x00102F4A File Offset: 0x0010114A
	public void SetReshufflingState(bool enable)
	{
		this.reshuffleButton.gameObject.SetActive(enable);
	}

	// Token: 0x0600909C RID: 37020 RVA: 0x00102F5D File Offset: 0x0010115D
	private void Reshuffle(bool is_starter)
	{
		if (this.controller != null && this.controller.IsSelected(this.info))
		{
			this.DeselectDeliverable();
		}
		this.ClearEntryIcons();
		this.GenerateCharacter(is_starter);
	}

	// Token: 0x0600909D RID: 37021 RVA: 0x00388FB4 File Offset: 0x003871B4
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

	// Token: 0x0600909E RID: 37022 RVA: 0x00389074 File Offset: 0x00387274
	public void DisableSelectButton()
	{
		this.selectButton.soundPlayer.AcceptClickCondition = (() => false);
		this.selectButton.GetComponent<ImageToggleState>().SetDisabled();
		this.selectButton.soundPlayer.Enabled = false;
	}

	// Token: 0x0600909F RID: 37023 RVA: 0x003890D4 File Offset: 0x003872D4
	private bool IsCharacterRedundant()
	{
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in CarePackageContainer.containers)
		{
			if (telepadDeliverableContainer != this)
			{
				CarePackageContainer carePackageContainer = telepadDeliverableContainer as CarePackageContainer;
				if (carePackageContainer != null && carePackageContainer.info == this.info)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060090A0 RID: 37024 RVA: 0x00102F93 File Offset: 0x00101193
	public string GetValueColor(bool isPositive)
	{
		if (!isPositive)
		{
			return "<color=#ff2222ff>";
		}
		return "<color=green>";
	}

	// Token: 0x060090A1 RID: 37025 RVA: 0x00102FA3 File Offset: 0x001011A3
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.IsAction(global::Action.Escape))
		{
			this.controller.OnPressBack();
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x060090A2 RID: 37026 RVA: 0x00102FC7 File Offset: 0x001011C7
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x060090A3 RID: 37027 RVA: 0x00102FD7 File Offset: 0x001011D7
	protected override void OnCmpEnable()
	{
		base.OnActivate();
		if (this.info == null)
		{
			return;
		}
		this.ClearEntryIcons();
		this.SetAnimator();
		this.SetInfoText();
	}

	// Token: 0x060090A4 RID: 37028 RVA: 0x00389148 File Offset: 0x00387348
	private void ClearEntryIcons()
	{
		for (int i = 0; i < this.entryIcons.Count; i++)
		{
			UnityEngine.Object.Destroy(this.entryIcons[i]);
		}
	}

	// Token: 0x04006D3B RID: 27963
	[Header("UI References")]
	[SerializeField]
	private GameObject contentBody;

	// Token: 0x04006D3C RID: 27964
	[SerializeField]
	private LocText characterName;

	// Token: 0x04006D3D RID: 27965
	public GameObject selectedBorder;

	// Token: 0x04006D3E RID: 27966
	[SerializeField]
	private Image titleBar;

	// Token: 0x04006D3F RID: 27967
	[SerializeField]
	private Color selectedTitleColor;

	// Token: 0x04006D40 RID: 27968
	[SerializeField]
	private Color deselectedTitleColor;

	// Token: 0x04006D41 RID: 27969
	[SerializeField]
	private KButton reshuffleButton;

	// Token: 0x04006D42 RID: 27970
	private KBatchedAnimController animController;

	// Token: 0x04006D43 RID: 27971
	[SerializeField]
	private LocText itemName;

	// Token: 0x04006D44 RID: 27972
	[SerializeField]
	private LocText quantity;

	// Token: 0x04006D45 RID: 27973
	[SerializeField]
	private LocText currentQuantity;

	// Token: 0x04006D46 RID: 27974
	[SerializeField]
	private LocText description;

	// Token: 0x04006D47 RID: 27975
	[SerializeField]
	private LocText effects;

	// Token: 0x04006D48 RID: 27976
	[SerializeField]
	private KToggle selectButton;

	// Token: 0x04006D49 RID: 27977
	private CarePackageInfo info;

	// Token: 0x04006D4A RID: 27978
	public CarePackageContainer.CarePackageInstanceData carePackageInstanceData;

	// Token: 0x04006D4B RID: 27979
	private CharacterSelectionController controller;

	// Token: 0x04006D4C RID: 27980
	private static List<ITelepadDeliverableContainer> containers;

	// Token: 0x04006D4D RID: 27981
	[SerializeField]
	private Sprite enabledSpr;

	// Token: 0x04006D4E RID: 27982
	[SerializeField]
	private List<CarePackageContainer.ProfessionIcon> professionIcons;

	// Token: 0x04006D4F RID: 27983
	private Dictionary<string, Sprite> professionIconMap;

	// Token: 0x04006D50 RID: 27984
	public float baseCharacterScale = 0.38f;

	// Token: 0x04006D51 RID: 27985
	private List<GameObject> entryIcons = new List<GameObject>();

	// Token: 0x02001AFF RID: 6911
	[Serializable]
	public struct ProfessionIcon
	{
		// Token: 0x04006D52 RID: 27986
		public string professionName;

		// Token: 0x04006D53 RID: 27987
		public Sprite iconImg;
	}

	// Token: 0x02001B00 RID: 6912
	public class CarePackageInstanceData : ITelepadDeliverable
	{
		// Token: 0x060090AC RID: 37036 RVA: 0x00103084 File Offset: 0x00101284
		public GameObject Deliver(Vector3 position)
		{
			GameObject gameObject = this.info.Deliver(position);
			gameObject.GetComponent<CarePackage>().SetFacade(this.facadeID);
			return gameObject;
		}

		// Token: 0x04006D54 RID: 27988
		public CarePackageInfo info;

		// Token: 0x04006D55 RID: 27989
		public string facadeID;
	}
}
