using System;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C5A RID: 7258
public class BarterConfirmationScreen : KModalScreen
{
	// Token: 0x060096D3 RID: 38611 RVA: 0x00106B13 File Offset: 0x00104D13
	protected override void OnActivate()
	{
		base.OnActivate();
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.cancelButton.onClick += delegate()
		{
			this.Show(false);
		};
	}

	// Token: 0x060096D4 RID: 38612 RVA: 0x003AE690 File Offset: 0x003AC890
	public void Present(PermitResource permit, bool isPurchase)
	{
		this.Show(true);
		this.ShowContentContainer(true);
		this.ShowLoadingPanel(false);
		this.HideResultPanel();
		if (isPurchase)
		{
			this.itemIcon.transform.SetAsLastSibling();
			this.filamentIcon.transform.SetAsFirstSibling();
		}
		else
		{
			this.itemIcon.transform.SetAsFirstSibling();
			this.filamentIcon.transform.SetAsLastSibling();
		}
		KleiItems.ResponseCallback <>9__1;
		KleiItems.ResponseCallback <>9__2;
		this.confirmButton.onClick += delegate()
		{
			string serverTypeFromPermit = PermitItems.GetServerTypeFromPermit(permit);
			if (serverTypeFromPermit == null)
			{
				return;
			}
			this.ShowContentContainer(false);
			this.HideResultPanel();
			this.ShowLoadingPanel(true);
			if (isPurchase)
			{
				string itemType = serverTypeFromPermit;
				KleiItems.ResponseCallback cb;
				if ((cb = <>9__1) == null)
				{
					cb = (<>9__1 = delegate(KleiItems.Result result)
					{
						if (this.IsNullOrDestroyed())
						{
							return;
						}
						this.ShowContentContainer(false);
						this.ShowLoadingPanel(false);
						if (!result.Success)
						{
							this.ShowResultPanel(permit, true, false);
							return;
						}
						this.ShowResultPanel(permit, true, true);
					});
				}
				KleiItems.AddRequestBarterGainItem(itemType, cb);
				return;
			}
			ulong itemInstanceID = KleiItems.GetItemInstanceID(serverTypeFromPermit);
			KleiItems.ResponseCallback cb2;
			if ((cb2 = <>9__2) == null)
			{
				cb2 = (<>9__2 = delegate(KleiItems.Result result)
				{
					if (this.IsNullOrDestroyed())
					{
						return;
					}
					this.ShowContentContainer(false);
					this.ShowLoadingPanel(false);
					if (!result.Success)
					{
						this.ShowResultPanel(permit, false, false);
						return;
					}
					this.ShowResultPanel(permit, false, true);
				});
			}
			KleiItems.AddRequestBarterLoseItem(itemInstanceID, cb2);
		};
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(permit.Id, out num, out num2);
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		this.itemIcon.GetComponent<Image>().sprite = permitPresentationInfo.sprite;
		this.itemLabel.SetText(permit.Name);
		this.transactionDescriptionLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_PRINT : UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_RECYCLE);
		this.panelHeaderLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_PRINT_HEADER : UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_RECYCLE_HEADER);
		this.confirmButtonActionLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.BUY : UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL);
		this.confirmButtonFilamentLabel.SetText(isPurchase ? num.ToString() : (UIConstants.ColorPrefixGreen + "+" + num2.ToString() + UIConstants.ColorSuffix));
		this.largeCostLabel.SetText(isPurchase ? ("x" + num.ToString()) : ("x" + num2.ToString()));
	}

	// Token: 0x060096D5 RID: 38613 RVA: 0x00106B49 File Offset: 0x00104D49
	private void Update()
	{
		if (this.shouldCloseScreen)
		{
			this.ShowContentContainer(false);
			this.ShowLoadingPanel(false);
			this.HideResultPanel();
			this.Show(false);
		}
	}

	// Token: 0x060096D6 RID: 38614 RVA: 0x00106B6E File Offset: 0x00104D6E
	private void ShowContentContainer(bool show)
	{
		this.contentContainer.SetActive(show);
	}

	// Token: 0x060096D7 RID: 38615 RVA: 0x003AE85C File Offset: 0x003ACA5C
	private void ShowLoadingPanel(bool show)
	{
		this.loadingContainer.SetActive(show);
		this.resultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.LOADING);
		if (show)
		{
			this.loadingAnimation.Play("loading_rocket", KAnim.PlayMode.Loop, 1f, 0f);
		}
		else
		{
			this.loadingAnimation.Stop();
		}
		if (!show)
		{
			this.shouldCloseScreen = false;
		}
	}

	// Token: 0x060096D8 RID: 38616 RVA: 0x00106B7C File Offset: 0x00104D7C
	private void HideResultPanel()
	{
		this.resultContainer.SetActive(false);
	}

	// Token: 0x060096D9 RID: 38617 RVA: 0x003AE8C4 File Offset: 0x003ACAC4
	private void ShowResultPanel(PermitResource permit, bool isPurchase, bool transationResult)
	{
		this.resultContainer.SetActive(true);
		if (!transationResult)
		{
			this.resultIcon.sprite = Assets.GetSprite("error_message");
			this.mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_ERROR);
			this.panelHeaderLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_INCOMPLETE_HEADER);
			this.resultFilamentLabel.SetText("");
			KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Failed", false));
			return;
		}
		this.panelHeaderLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_COMPLETE_HEADER);
		if (isPurchase)
		{
			PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
			this.resultIcon.sprite = permitPresentationInfo.sprite;
			this.resultFilamentLabel.SetText("");
			this.mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.PURCHASE_SUCCESS);
			KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Print_Succeed", false));
			return;
		}
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(permit.Id, out num, out num2);
		this.resultIcon.sprite = Assets.GetSprite("filament");
		this.resultFilamentLabel.GetComponent<LocText>().SetText("x" + num2.ToString());
		this.mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL_SUCCESS);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Succeed", false));
	}

	// Token: 0x04007543 RID: 30019
	[SerializeField]
	private GameObject itemIcon;

	// Token: 0x04007544 RID: 30020
	[SerializeField]
	private GameObject filamentIcon;

	// Token: 0x04007545 RID: 30021
	[SerializeField]
	private LocText largeCostLabel;

	// Token: 0x04007546 RID: 30022
	[SerializeField]
	private LocText largeQuantityLabel;

	// Token: 0x04007547 RID: 30023
	[SerializeField]
	private LocText itemLabel;

	// Token: 0x04007548 RID: 30024
	[SerializeField]
	private LocText transactionDescriptionLabel;

	// Token: 0x04007549 RID: 30025
	[SerializeField]
	private KButton confirmButton;

	// Token: 0x0400754A RID: 30026
	[SerializeField]
	private KButton cancelButton;

	// Token: 0x0400754B RID: 30027
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400754C RID: 30028
	[SerializeField]
	private LocText panelHeaderLabel;

	// Token: 0x0400754D RID: 30029
	[SerializeField]
	private LocText confirmButtonActionLabel;

	// Token: 0x0400754E RID: 30030
	[SerializeField]
	private LocText confirmButtonFilamentLabel;

	// Token: 0x0400754F RID: 30031
	[SerializeField]
	private LocText resultLabel;

	// Token: 0x04007550 RID: 30032
	[SerializeField]
	private KBatchedAnimController loadingAnimation;

	// Token: 0x04007551 RID: 30033
	[SerializeField]
	private GameObject contentContainer;

	// Token: 0x04007552 RID: 30034
	[SerializeField]
	private GameObject loadingContainer;

	// Token: 0x04007553 RID: 30035
	[SerializeField]
	private GameObject resultContainer;

	// Token: 0x04007554 RID: 30036
	[SerializeField]
	private Image resultIcon;

	// Token: 0x04007555 RID: 30037
	[SerializeField]
	private LocText mainResultLabel;

	// Token: 0x04007556 RID: 30038
	[SerializeField]
	private LocText resultFilamentLabel;

	// Token: 0x04007557 RID: 30039
	private bool shouldCloseScreen;
}
