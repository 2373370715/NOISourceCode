using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x02001E62 RID: 7778
public class MeterScreen : KScreen, IRender1000ms
{
	// Token: 0x17000A8C RID: 2700
	// (get) Token: 0x0600A2EC RID: 41708 RVA: 0x0010E5C7 File Offset: 0x0010C7C7
	// (set) Token: 0x0600A2ED RID: 41709 RVA: 0x0010E5CE File Offset: 0x0010C7CE
	public static MeterScreen Instance { get; private set; }

	// Token: 0x0600A2EE RID: 41710 RVA: 0x0010E5D6 File Offset: 0x0010C7D6
	public static void DestroyInstance()
	{
		MeterScreen.Instance = null;
	}

	// Token: 0x17000A8D RID: 2701
	// (get) Token: 0x0600A2EF RID: 41711 RVA: 0x0010E5DE File Offset: 0x0010C7DE
	public bool StartValuesSet
	{
		get
		{
			return this.startValuesSet;
		}
	}

	// Token: 0x0600A2F0 RID: 41712 RVA: 0x0010E5E6 File Offset: 0x0010C7E6
	protected override void OnPrefabInit()
	{
		MeterScreen.Instance = this;
	}

	// Token: 0x0600A2F1 RID: 41713 RVA: 0x003EE5DC File Offset: 0x003EC7DC
	protected override void OnSpawn()
	{
		this.RedAlertTooltip.OnToolTip = new Func<string>(this.OnRedAlertTooltip);
		MultiToggle redAlertButton = this.RedAlertButton;
		redAlertButton.onClick = (System.Action)Delegate.Combine(redAlertButton.onClick, new System.Action(delegate()
		{
			this.OnRedAlertClick();
		}));
		Game.Instance.Subscribe(1983128072, delegate(object data)
		{
			this.Refresh();
		});
		Game.Instance.Subscribe(1585324898, delegate(object data)
		{
			this.RefreshRedAlertButtonState();
		});
		Game.Instance.Subscribe(-1393151672, delegate(object data)
		{
			this.RefreshRedAlertButtonState();
		});
	}

	// Token: 0x0600A2F2 RID: 41714 RVA: 0x003EE67C File Offset: 0x003EC87C
	private void OnRedAlertClick()
	{
		bool flag = !ClusterManager.Instance.activeWorld.AlertManager.IsRedAlertToggledOn();
		ClusterManager.Instance.activeWorld.AlertManager.ToggleRedAlert(flag);
		if (flag)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
			return;
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
	}

	// Token: 0x0600A2F3 RID: 41715 RVA: 0x0010E5EE File Offset: 0x0010C7EE
	private void RefreshRedAlertButtonState()
	{
		this.RedAlertButton.ChangeState(ClusterManager.Instance.activeWorld.IsRedAlert() ? 1 : 0);
	}

	// Token: 0x0600A2F4 RID: 41716 RVA: 0x0010E610 File Offset: 0x0010C810
	public void Render1000ms(float dt)
	{
		this.Refresh();
	}

	// Token: 0x0600A2F5 RID: 41717 RVA: 0x0010E618 File Offset: 0x0010C818
	public void InitializeValues()
	{
		if (this.startValuesSet)
		{
			return;
		}
		this.startValuesSet = true;
		this.Refresh();
	}

	// Token: 0x0600A2F6 RID: 41718 RVA: 0x003EE6DC File Offset: 0x003EC8DC
	private void Refresh()
	{
		this.RefreshWorldMinionIdentities();
		this.RefreshMinions();
		for (int i = 0; i < this.valueDisplayers.Length; i++)
		{
			this.valueDisplayers[i].Refresh();
		}
		this.RefreshRedAlertButtonState();
	}

	// Token: 0x0600A2F7 RID: 41719 RVA: 0x003EE71C File Offset: 0x003EC91C
	private void RefreshWorldMinionIdentities()
	{
		this.worldLiveMinionIdentities = new List<MinionIdentity>(from x in Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId, false)
		where !x.IsNullOrDestroyed()
		select x);
	}

	// Token: 0x0600A2F8 RID: 41720 RVA: 0x0010E630 File Offset: 0x0010C830
	private List<MinionIdentity> GetWorldMinionIdentities()
	{
		if (this.worldLiveMinionIdentities == null)
		{
			this.RefreshWorldMinionIdentities();
		}
		return this.worldLiveMinionIdentities;
	}

	// Token: 0x0600A2F9 RID: 41721 RVA: 0x003EE770 File Offset: 0x003EC970
	private void RefreshMinions()
	{
		int count = Components.LiveMinionIdentities.Count;
		int count2 = this.GetWorldMinionIdentities().Count;
		if (count2 == this.cachedMinionCount)
		{
			return;
		}
		this.cachedMinionCount = count2;
		string newString;
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			ClusterGridEntity component = ClusterManager.Instance.activeWorld.GetComponent<ClusterGridEntity>();
			newString = string.Format(UI.TOOLTIPS.METERSCREEN_POPULATION_CLUSTER, component.Name, count2, count);
			this.currentMinions.text = string.Format("{0}/{1}", count2, count);
		}
		else
		{
			this.currentMinions.text = string.Format("{0}", count);
			newString = string.Format(UI.TOOLTIPS.METERSCREEN_POPULATION, count.ToString("0"));
		}
		this.MinionsTooltip.ClearMultiStringTooltip();
		this.MinionsTooltip.AddMultiStringTooltip(newString, this.ToolTipStyle_Header);
	}

	// Token: 0x0600A2FA RID: 41722 RVA: 0x003EE85C File Offset: 0x003ECA5C
	private string OnRedAlertTooltip()
	{
		this.RedAlertTooltip.ClearMultiStringTooltip();
		this.RedAlertTooltip.AddMultiStringTooltip(UI.TOOLTIPS.RED_ALERT_TITLE, this.ToolTipStyle_Header);
		this.RedAlertTooltip.AddMultiStringTooltip(UI.TOOLTIPS.RED_ALERT_CONTENT, this.ToolTipStyle_Property);
		return "";
	}

	// Token: 0x04007F71 RID: 32625
	[SerializeField]
	private LocText currentMinions;

	// Token: 0x04007F73 RID: 32627
	public ToolTip MinionsTooltip;

	// Token: 0x04007F74 RID: 32628
	public MeterScreen_ValueTrackerDisplayer[] valueDisplayers;

	// Token: 0x04007F75 RID: 32629
	public TextStyleSetting ToolTipStyle_Header;

	// Token: 0x04007F76 RID: 32630
	public TextStyleSetting ToolTipStyle_Property;

	// Token: 0x04007F77 RID: 32631
	private bool startValuesSet;

	// Token: 0x04007F78 RID: 32632
	public MultiToggle RedAlertButton;

	// Token: 0x04007F79 RID: 32633
	public ToolTip RedAlertTooltip;

	// Token: 0x04007F7A RID: 32634
	private MeterScreen.DisplayInfo immunityDisplayInfo = new MeterScreen.DisplayInfo
	{
		selectedIndex = -1
	};

	// Token: 0x04007F7B RID: 32635
	private List<MinionIdentity> worldLiveMinionIdentities;

	// Token: 0x04007F7C RID: 32636
	private int cachedMinionCount = -1;

	// Token: 0x02001E63 RID: 7779
	private struct DisplayInfo
	{
		// Token: 0x04007F7D RID: 32637
		public int selectedIndex;
	}
}
