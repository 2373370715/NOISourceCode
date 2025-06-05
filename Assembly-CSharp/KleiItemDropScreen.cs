using System;
using System.Collections;
using System.Collections.Generic;
using Database;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D97 RID: 7575
public class KleiItemDropScreen : KModalScreen
{
	// Token: 0x06009E35 RID: 40501 RVA: 0x0010B6F3 File Offset: 0x001098F3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		KleiItemDropScreen.Instance = this;
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		if (string.IsNullOrEmpty(KleiAccount.KleiToken))
		{
			base.Show(false);
		}
	}

	// Token: 0x06009E36 RID: 40502 RVA: 0x0010B72B File Offset: 0x0010992B
	protected override void OnActivate()
	{
		KleiItemDropScreen.Instance = this;
		this.Show(false);
	}

	// Token: 0x06009E37 RID: 40503 RVA: 0x003DB3A0 File Offset: 0x003D95A0
	public override void Show(bool show = true)
	{
		this.serverRequestState.Reset();
		if (!show)
		{
			this.animatedLoadingIcon.gameObject.SetActive(false);
			if (this.activePresentationRoutine != null)
			{
				base.StopCoroutine(this.activePresentationRoutine);
			}
			if (this.shouldDoCloseRoutine)
			{
				this.closeButton.gameObject.SetActive(false);
				Updater.RunRoutine(this, this.AnimateScreenOutRoutine()).Then(delegate
				{
					base.Show(false);
				});
				this.shouldDoCloseRoutine = false;
			}
			else
			{
				base.Show(false);
			}
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndItemDropScreenSnapshot, STOP_MODE.ALLOWFADEOUT);
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndItemDropScreenSnapshot);
		base.Show(true);
	}

	// Token: 0x06009E38 RID: 40504 RVA: 0x0010B39B File Offset: 0x0010959B
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009E39 RID: 40505 RVA: 0x003DB460 File Offset: 0x003D9660
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			return;
		}
		if (PermitItems.HasUnopenedItem())
		{
			this.PresentNextUnopenedItem(true);
			this.shouldDoCloseRoutine = true;
			return;
		}
		this.userMessageLabel.SetText(UI.ITEM_DROP_SCREEN.NOTHING_AVAILABLE);
		this.PresentNoItemAvailablePrompt(true);
		this.shouldDoCloseRoutine = true;
	}

	// Token: 0x06009E3A RID: 40506 RVA: 0x003DB4B4 File Offset: 0x003D96B4
	public void PresentNextUnopenedItem(bool firstItemPresentation = true)
	{
		int num = 0;
		using (IEnumerator<KleiItems.ItemData> enumerator = PermitItems.IterateInventory().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsOpened)
				{
					num++;
				}
			}
		}
		this.RefreshUnopenedItemsLabel();
		foreach (KleiItems.ItemData itemData in PermitItems.IterateInventory())
		{
			if (!itemData.IsOpened)
			{
				this.PresentItem(itemData, firstItemPresentation, num == 1);
				return;
			}
		}
		this.PresentNoItemAvailablePrompt(false);
	}

	// Token: 0x06009E3B RID: 40507 RVA: 0x003DB560 File Offset: 0x003D9760
	private void RefreshUnopenedItemsLabel()
	{
		int num = 0;
		using (IEnumerator<KleiItems.ItemData> enumerator = PermitItems.IterateInventory().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsOpened)
				{
					num++;
				}
			}
		}
		if (num > 1)
		{
			this.unopenedItemCountLabel.gameObject.SetActive(true);
			this.unopenedItemCountLabel.SetText(UI.ITEM_DROP_SCREEN.UNOPENED_ITEM_COUNT, (float)num);
			return;
		}
		if (num == 1)
		{
			this.unopenedItemCountLabel.gameObject.SetActive(true);
			this.unopenedItemCountLabel.SetText(UI.ITEM_DROP_SCREEN.UNOPENED_ITEM, (float)num);
			return;
		}
		this.unopenedItemCountLabel.gameObject.SetActive(false);
	}

	// Token: 0x06009E3C RID: 40508 RVA: 0x003DB61C File Offset: 0x003D981C
	public void PresentItem(KleiItems.ItemData item, bool firstItemPresentation, bool lastItemPresentation)
	{
		this.userMessageLabel.SetText(UI.ITEM_DROP_SCREEN.THANKS_FOR_PLAYING);
		this.giftAcknowledged = false;
		this.serverRequestState.revealConfirmedByServer = false;
		this.serverRequestState.revealRejectedByServer = false;
		if (this.activePresentationRoutine != null)
		{
			base.StopCoroutine(this.activePresentationRoutine);
		}
		this.activePresentationRoutine = base.StartCoroutine(this.PresentItemRoutine(item, firstItemPresentation, lastItemPresentation));
		this.acceptButton.ClearOnClick();
		this.acknowledgeButton.ClearOnClick();
		this.acceptButton.GetComponentInChildren<LocText>().SetText(UI.ITEM_DROP_SCREEN.PRINT_ITEM_BUTTON);
		this.acceptButton.onClick += delegate()
		{
			this.RequestReveal(item);
		};
		this.acknowledgeButton.onClick += delegate()
		{
			if (this.serverRequestState.revealConfirmedByServer)
			{
				this.giftAcknowledged = true;
			}
		};
	}

	// Token: 0x06009E3D RID: 40509 RVA: 0x0010B73A File Offset: 0x0010993A
	private void RequestReveal(KleiItems.ItemData item)
	{
		this.serverRequestState.revealRequested = true;
		PermitItems.QueueRequestOpenOrUnboxItem(item, new KleiItems.ResponseCallback(this.OnOpenItemRequestResponse));
	}

	// Token: 0x06009E3E RID: 40510 RVA: 0x003DB700 File Offset: 0x003D9900
	public void OnOpenItemRequestResponse(KleiItems.Result result)
	{
		if (!this.serverRequestState.revealRequested)
		{
			return;
		}
		this.serverRequestState.revealRequested = false;
		if (result.Success)
		{
			this.serverRequestState.revealRejectedByServer = false;
			this.serverRequestState.revealConfirmedByServer = true;
			return;
		}
		this.serverRequestState.revealRejectedByServer = true;
		this.serverRequestState.revealConfirmedByServer = false;
	}

	// Token: 0x06009E3F RID: 40511 RVA: 0x003DB760 File Offset: 0x003D9960
	public void PresentNoItemAvailablePrompt(bool firstItemPresentation)
	{
		this.userMessageLabel.SetText(UI.ITEM_DROP_SCREEN.NOTHING_AVAILABLE);
		this.noItemAvailableAcknowledged = false;
		this.acknowledgeButton.ClearOnClick();
		this.acceptButton.ClearOnClick();
		this.acceptButton.GetComponentInChildren<LocText>().SetText(UI.ITEM_DROP_SCREEN.DISMISS_BUTTON);
		this.acceptButton.onClick += delegate()
		{
			this.noItemAvailableAcknowledged = true;
		};
		if (this.activePresentationRoutine != null)
		{
			base.StopCoroutine(this.activePresentationRoutine);
		}
		this.activePresentationRoutine = base.StartCoroutine(this.PresentNoItemAvailableRoutine(firstItemPresentation));
	}

	// Token: 0x06009E40 RID: 40512 RVA: 0x0010B75A File Offset: 0x0010995A
	private IEnumerator AnimateScreenInRoutine()
	{
		float scaleFactor = base.transform.parent.GetComponent<CanvasScaler>().scaleFactor;
		float OPEN_WIDTH = (float)Screen.width / scaleFactor;
		float y = Mathf.Clamp((float)Screen.height / scaleFactor, 720f, 900f);
		KFMOD.PlayUISound(GlobalAssets.GetSound("GiftItemDrop_Screen_Open", false));
		this.userMessageLabel.gameObject.SetActive(false);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(this.shieldMaskRect.sizeDelta.x, y), 0.5f, Easing.CircInOut, -1f);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(OPEN_WIDTH, this.shieldMaskRect.sizeDelta.y), 0.25f, Easing.CircInOut, -1f);
		this.userMessageLabel.gameObject.SetActive(true);
		yield break;
	}

	// Token: 0x06009E41 RID: 40513 RVA: 0x0010B769 File Offset: 0x00109969
	private IEnumerator AnimateScreenOutRoutine()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("GiftItemDrop_Screen_Close", false));
		this.userMessageLabel.gameObject.SetActive(false);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(8f, this.shieldMaskRect.sizeDelta.y), 0.25f, Easing.CircInOut, -1f);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			this.shieldMaskRect.sizeDelta = v2;
		}, this.shieldMaskRect.sizeDelta, new Vector2(this.shieldMaskRect.sizeDelta.x, 0f), 0.25f, Easing.CircInOut, -1f);
		yield break;
	}

	// Token: 0x06009E42 RID: 40514 RVA: 0x0010B778 File Offset: 0x00109978
	private IEnumerator PresentNoItemAvailableRoutine(bool firstItem)
	{
		yield return null;
		this.itemNameLabel.SetText("");
		this.itemDescriptionLabel.SetText("");
		this.itemRarityLabel.SetText("");
		this.itemCategoryLabel.SetText("");
		if (firstItem)
		{
			this.animatedPod.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.acceptButtonRect.gameObject.SetActive(false);
			this.shieldMaskRect.sizeDelta = new Vector2(8f, 0f);
			this.shieldMaskRect.gameObject.SetActive(true);
		}
		if (firstItem)
		{
			this.closeButton.gameObject.SetActive(false);
			yield return Updater.WaitForSeconds(0.5f);
			yield return this.AnimateScreenInRoutine();
			yield return Updater.WaitForSeconds(0.125f);
			this.closeButton.gameObject.SetActive(true);
		}
		else
		{
			yield return Updater.WaitForSeconds(0.25f);
		}
		Vector2 animate_offset = new Vector2(0f, -30f);
		this.acceptButtonRect.FindOrAddComponent<CanvasGroup>().alpha = 0f;
		this.acceptButtonRect.gameObject.SetActive(true);
		this.acceptButtonPosition.SetOn(this.acceptButtonRect);
		yield return Updater.WaitForSeconds(0.75f);
		yield return PresUtil.OffsetToAndFade(this.acceptButton.rectTransform(), animate_offset, 1f, 0.125f, Easing.ExpoOut);
		yield return Updater.Until(() => this.noItemAvailableAcknowledged);
		yield return PresUtil.OffsetFromAndFade(this.acceptButton.rectTransform(), animate_offset, 0f, 0.125f, Easing.SmoothStep);
		this.Show(false);
		yield break;
	}

	// Token: 0x06009E43 RID: 40515 RVA: 0x0010B78E File Offset: 0x0010998E
	private IEnumerator PresentItemRoutine(KleiItems.ItemData item, bool firstItem, bool lastItem)
	{
		yield return null;
		if (item.ItemId == 0UL)
		{
			global::Debug.LogError("Could not find dropped item inventory.");
			yield break;
		}
		this.itemNameLabel.SetText("");
		this.itemDescriptionLabel.SetText("");
		this.itemRarityLabel.SetText("");
		this.itemCategoryLabel.SetText("");
		this.permitVisualizer.ResetState();
		if (firstItem)
		{
			this.animatedPod.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.acceptButtonRect.gameObject.SetActive(false);
			this.shieldMaskRect.sizeDelta = new Vector2(8f, 0f);
			this.shieldMaskRect.gameObject.SetActive(true);
		}
		if (firstItem)
		{
			this.closeButton.gameObject.SetActive(false);
			yield return Updater.WaitForSeconds(0.5f);
			yield return this.AnimateScreenInRoutine();
			yield return Updater.WaitForSeconds(0.125f);
			this.closeButton.gameObject.SetActive(true);
		}
		Vector2 animate_offset = new Vector2(0f, -30f);
		if (firstItem)
		{
			this.acceptButtonRect.FindOrAddComponent<CanvasGroup>().alpha = 0f;
			this.acceptButtonRect.gameObject.SetActive(true);
			this.acceptButtonPosition.SetOn(this.acceptButtonRect);
			this.animatedPod.Play("powerup", KAnim.PlayMode.Once, 1f, 0f);
			this.animatedPod.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
			yield return Updater.WaitForSeconds(1.25f);
			yield return PresUtil.OffsetToAndFade(this.acceptButton.rectTransform(), animate_offset, 1f, 0.125f, Easing.ExpoOut);
			yield return Updater.Until(() => this.serverRequestState.revealRequested);
			yield return PresUtil.OffsetFromAndFade(this.acceptButton.rectTransform(), animate_offset, 0f, 0.125f, Easing.SmoothStep);
		}
		else
		{
			this.RequestReveal(item);
		}
		this.animatedLoadingIcon.gameObject.rectTransform().anchoredPosition = new Vector2(0f, -352f);
		if (this.animatedLoadingIcon.GetComponent<CanvasGroup>() != null)
		{
			this.animatedLoadingIcon.GetComponent<CanvasGroup>().alpha = 1f;
		}
		yield return new WaitForSecondsRealtime(0.3f);
		if (!this.serverRequestState.revealConfirmedByServer && !this.serverRequestState.revealRejectedByServer)
		{
			this.animatedLoadingIcon.gameObject.SetActive(true);
			this.animatedLoadingIcon.Play("loading_rocket", KAnim.PlayMode.Loop, 1f, 0f);
			yield return Updater.Until(() => this.serverRequestState.revealConfirmedByServer || this.serverRequestState.revealRejectedByServer);
			yield return new WaitForSecondsRealtime(2f);
			yield return PresUtil.OffsetFromAndFade(this.animatedLoadingIcon.gameObject.rectTransform(), new Vector2(0f, -512f), 0f, 0.25f, Easing.SmoothStep);
			this.animatedLoadingIcon.gameObject.SetActive(false);
		}
		if (this.serverRequestState.revealRejectedByServer)
		{
			this.animatedPod.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
			this.errorMessage.gameObject.SetActive(true);
			yield return Updater.WaitForSeconds(3f);
			this.errorMessage.gameObject.SetActive(false);
		}
		else if (this.serverRequestState.revealConfirmedByServer)
		{
			float num = 1f;
			this.animatedPod.PlaySpeedMultiplier = (firstItem ? 1f : (1f * num));
			this.animatedPod.Play("additional_pre", KAnim.PlayMode.Once, 1f, 0f);
			this.animatedPod.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
			yield return Updater.WaitForSeconds(firstItem ? 1f : (1f / num));
			this.animatedPod.PlaySpeedMultiplier = 1f;
			this.RefreshUnopenedItemsLabel();
			DropScreenPresentationInfo info;
			info.UseEquipmentVis = false;
			info.BuildOverride = null;
			info.Sprite = null;
			string name = "";
			string desc = "";
			PermitRarity rarity = PermitRarity.Unknown;
			string categoryString = "";
			string s;
			if (PermitItems.TryGetBoxInfo(item, out name, out desc, out s))
			{
				info.UseEquipmentVis = false;
				info.BuildOverride = null;
				info.Sprite = Assets.GetSprite(s);
				rarity = PermitRarity.Loyalty;
			}
			else
			{
				PermitResource permitResource = Db.Get().Permits.Get(item.Id);
				info.Sprite = permitResource.GetPermitPresentationInfo().sprite;
				info.UseEquipmentVis = (permitResource.Category == PermitCategory.Equipment);
				if (permitResource is EquippableFacadeResource)
				{
					info.BuildOverride = (permitResource as EquippableFacadeResource).BuildOverride;
				}
				name = permitResource.Name;
				desc = permitResource.Description;
				rarity = permitResource.Rarity;
				PermitCategory category = permitResource.Category;
				if (category != PermitCategory.Building)
				{
					if (category != PermitCategory.Artwork)
					{
						if (category != PermitCategory.JoyResponse)
						{
							categoryString = PermitCategories.GetDisplayName(permitResource.Category);
						}
						else
						{
							categoryString = PermitCategories.GetDisplayName(permitResource.Category);
							if (permitResource is BalloonArtistFacadeResource)
							{
								categoryString = PermitCategories.GetDisplayName(permitResource.Category) + ": " + UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSES.BALLOON_ARTIST;
							}
						}
					}
					else
					{
						categoryString = PermitCategories.GetDisplayName(permitResource.Category);
						if (permitResource is ArtableStage)
						{
							categoryString = Assets.GetPrefab((permitResource as ArtableStage).prefabId).GetProperName();
						}
					}
				}
				else
				{
					categoryString = Assets.GetPrefab((permitResource as BuildingFacadeResource).PrefabID).GetProperName();
				}
			}
			this.permitVisualizer.ConfigureWith(info);
			yield return this.permitVisualizer.AnimateIn();
			KFMOD.PlayUISoundWithLabeledParameter(GlobalAssets.GetSound("GiftItemDrop_Rarity", false), "GiftItemRarity", string.Format("{0}", rarity));
			this.itemNameLabel.SetText(name);
			this.itemDescriptionLabel.SetText(desc);
			this.itemRarityLabel.SetText(rarity.GetLocStringName());
			this.itemCategoryLabel.SetText(categoryString);
			this.itemTextContainerPosition.SetOn(this.itemTextContainer);
			yield return Updater.Parallel(new Updater[]
			{
				PresUtil.OffsetToAndFade(this.itemTextContainer.rectTransform(), animate_offset, 1f, 0.125f, Easing.CircInOut)
			});
			yield return Updater.Until(() => this.giftAcknowledged);
			if (lastItem)
			{
				this.animatedPod.Play("working_pst", KAnim.PlayMode.Once, 1f, 0f);
				this.animatedPod.Queue("idle", KAnim.PlayMode.Loop, 1f, 0f);
				yield return Updater.Parallel(new Updater[]
				{
					PresUtil.OffsetFromAndFade(this.itemTextContainer.rectTransform(), animate_offset, 0f, 0.125f, Easing.CircInOut)
				});
				this.itemNameLabel.SetText("");
				this.itemDescriptionLabel.SetText("");
				this.itemRarityLabel.SetText("");
				this.itemCategoryLabel.SetText("");
				yield return this.permitVisualizer.AnimateOut();
			}
			else
			{
				this.itemNameLabel.SetText("");
				this.itemDescriptionLabel.SetText("");
				this.itemRarityLabel.SetText("");
				this.itemCategoryLabel.SetText("");
			}
			name = null;
			desc = null;
			categoryString = null;
		}
		this.PresentNextUnopenedItem(false);
		yield break;
	}

	// Token: 0x06009E44 RID: 40516 RVA: 0x0010B7B2 File Offset: 0x001099B2
	public static bool HasItemsToShow()
	{
		return PermitItems.HasUnopenedItem();
	}

	// Token: 0x04007C4E RID: 31822
	[SerializeField]
	private RectTransform shieldMaskRect;

	// Token: 0x04007C4F RID: 31823
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007C50 RID: 31824
	[Header("Animated Item")]
	[SerializeField]
	private KleiItemDropScreen_PermitVis permitVisualizer;

	// Token: 0x04007C51 RID: 31825
	[SerializeField]
	private KBatchedAnimController animatedPod;

	// Token: 0x04007C52 RID: 31826
	[SerializeField]
	private LocText userMessageLabel;

	// Token: 0x04007C53 RID: 31827
	[SerializeField]
	private LocText unopenedItemCountLabel;

	// Token: 0x04007C54 RID: 31828
	[Header("Item Info")]
	[SerializeField]
	private RectTransform itemTextContainer;

	// Token: 0x04007C55 RID: 31829
	[SerializeField]
	private LocText itemNameLabel;

	// Token: 0x04007C56 RID: 31830
	[SerializeField]
	private LocText itemDescriptionLabel;

	// Token: 0x04007C57 RID: 31831
	[SerializeField]
	private LocText itemRarityLabel;

	// Token: 0x04007C58 RID: 31832
	[SerializeField]
	private LocText itemCategoryLabel;

	// Token: 0x04007C59 RID: 31833
	[Header("Accept Button")]
	[SerializeField]
	private RectTransform acceptButtonRect;

	// Token: 0x04007C5A RID: 31834
	[SerializeField]
	private KButton acceptButton;

	// Token: 0x04007C5B RID: 31835
	[SerializeField]
	private KBatchedAnimController animatedLoadingIcon;

	// Token: 0x04007C5C RID: 31836
	[SerializeField]
	private KButton acknowledgeButton;

	// Token: 0x04007C5D RID: 31837
	[SerializeField]
	private LocText errorMessage;

	// Token: 0x04007C5E RID: 31838
	private Coroutine activePresentationRoutine;

	// Token: 0x04007C5F RID: 31839
	private KleiItemDropScreen.ServerRequestState serverRequestState;

	// Token: 0x04007C60 RID: 31840
	private bool giftAcknowledged;

	// Token: 0x04007C61 RID: 31841
	private bool noItemAvailableAcknowledged;

	// Token: 0x04007C62 RID: 31842
	public static KleiItemDropScreen Instance;

	// Token: 0x04007C63 RID: 31843
	private bool shouldDoCloseRoutine;

	// Token: 0x04007C64 RID: 31844
	private const float TEXT_AND_BUTTON_ANIMATE_OFFSET_Y = -30f;

	// Token: 0x04007C65 RID: 31845
	private PrefabDefinedUIPosition acceptButtonPosition = new PrefabDefinedUIPosition();

	// Token: 0x04007C66 RID: 31846
	private PrefabDefinedUIPosition itemTextContainerPosition = new PrefabDefinedUIPosition();

	// Token: 0x02001D98 RID: 7576
	private struct ServerRequestState
	{
		// Token: 0x06009E51 RID: 40529 RVA: 0x0010B835 File Offset: 0x00109A35
		public void Reset()
		{
			this.revealRequested = false;
			this.revealConfirmedByServer = false;
			this.revealRejectedByServer = false;
		}

		// Token: 0x04007C67 RID: 31847
		public bool revealRequested;

		// Token: 0x04007C68 RID: 31848
		public bool revealConfirmedByServer;

		// Token: 0x04007C69 RID: 31849
		public bool revealRejectedByServer;
	}
}
