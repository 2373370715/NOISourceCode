using System;
using STRINGS;
using UnityEngine;

// Token: 0x02002042 RID: 8258
public class SuitLockerSideScreen : SideScreenContent
{
	// Token: 0x0600AF2D RID: 44845 RVA: 0x00107377 File Offset: 0x00105577
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600AF2E RID: 44846 RVA: 0x00116693 File Offset: 0x00114893
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<SuitLocker>() != null;
	}

	// Token: 0x0600AF2F RID: 44847 RVA: 0x0042941C File Offset: 0x0042761C
	public override void SetTarget(GameObject target)
	{
		this.suitLocker = target.GetComponent<SuitLocker>();
		this.initialConfigRequestSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_REQUEST_SUIT_TOOLTIP);
		this.initialConfigNoSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_NO_SUIT_TOOLTIP);
		this.initialConfigRequestSuitButton.ClearOnClick();
		this.initialConfigRequestSuitButton.onClick += delegate()
		{
			this.suitLocker.ConfigRequestSuit();
		};
		this.initialConfigNoSuitButton.ClearOnClick();
		this.initialConfigNoSuitButton.onClick += delegate()
		{
			this.suitLocker.ConfigNoSuit();
		};
		this.regularConfigRequestSuitButton.ClearOnClick();
		this.regularConfigRequestSuitButton.onClick += delegate()
		{
			if (this.suitLocker.smi.sm.isWaitingForSuit.Get(this.suitLocker.smi))
			{
				this.suitLocker.ConfigNoSuit();
				return;
			}
			this.suitLocker.ConfigRequestSuit();
		};
		this.regularConfigDropSuitButton.ClearOnClick();
		this.regularConfigDropSuitButton.onClick += delegate()
		{
			this.suitLocker.DropSuit();
		};
	}

	// Token: 0x0600AF30 RID: 44848 RVA: 0x004294F4 File Offset: 0x004276F4
	private void Update()
	{
		bool flag = this.suitLocker.smi.sm.isConfigured.Get(this.suitLocker.smi);
		this.initialConfigScreen.gameObject.SetActive(!flag);
		this.regularConfigScreen.gameObject.SetActive(flag);
		bool flag2 = this.suitLocker.GetStoredOutfit() != null;
		bool flag3 = this.suitLocker.smi.sm.isWaitingForSuit.Get(this.suitLocker.smi);
		this.regularConfigRequestSuitButton.isInteractable = !flag2;
		if (!flag3)
		{
			this.regularConfigRequestSuitButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_REQUEST_SUIT;
			this.regularConfigRequestSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_REQUEST_SUIT_TOOLTIP);
		}
		else
		{
			this.regularConfigRequestSuitButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_CANCEL_REQUEST;
			this.regularConfigRequestSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_CANCEL_REQUEST_TOOLTIP);
		}
		if (flag2)
		{
			this.regularConfigDropSuitButton.isInteractable = true;
			this.regularConfigDropSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_DROP_SUIT_TOOLTIP);
		}
		else
		{
			this.regularConfigDropSuitButton.isInteractable = false;
			this.regularConfigDropSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_DROP_SUIT_NO_SUIT_TOOLTIP);
		}
		KSelectable component = this.suitLocker.GetComponent<KSelectable>();
		if (component != null)
		{
			StatusItemGroup.Entry statusItem = component.GetStatusItem(Db.Get().StatusItemCategories.Main);
			if (statusItem.item != null)
			{
				this.regularConfigLabel.text = statusItem.item.GetName(statusItem.data);
				this.regularConfigLabel.GetComponentInChildren<ToolTip>().SetSimpleTooltip(statusItem.item.GetTooltip(statusItem.data));
			}
		}
	}

	// Token: 0x040089AA RID: 35242
	[SerializeField]
	private GameObject initialConfigScreen;

	// Token: 0x040089AB RID: 35243
	[SerializeField]
	private GameObject regularConfigScreen;

	// Token: 0x040089AC RID: 35244
	[SerializeField]
	private LocText initialConfigLabel;

	// Token: 0x040089AD RID: 35245
	[SerializeField]
	private KButton initialConfigRequestSuitButton;

	// Token: 0x040089AE RID: 35246
	[SerializeField]
	private KButton initialConfigNoSuitButton;

	// Token: 0x040089AF RID: 35247
	[SerializeField]
	private LocText regularConfigLabel;

	// Token: 0x040089B0 RID: 35248
	[SerializeField]
	private KButton regularConfigRequestSuitButton;

	// Token: 0x040089B1 RID: 35249
	[SerializeField]
	private KButton regularConfigDropSuitButton;

	// Token: 0x040089B2 RID: 35250
	private SuitLocker suitLocker;
}
