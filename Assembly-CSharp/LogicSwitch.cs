using System;
using System.Collections;
using KSerialization;
using UnityEngine;

// Token: 0x02000EA0 RID: 3744
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicSwitch : Switch, IPlayerControlledToggle, ISim33ms
{
	// Token: 0x06004A7C RID: 19068 RVA: 0x000D4A91 File Offset: 0x000D2C91
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicSwitch>(-905833192, LogicSwitch.OnCopySettingsDelegate);
	}

	// Token: 0x06004A7D RID: 19069 RVA: 0x00269EE4 File Offset: 0x002680E4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.wasOn = this.switchedOn;
		this.UpdateLogicCircuit();
		base.GetComponent<KBatchedAnimController>().Play(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06004A7E RID: 19070 RVA: 0x000C4795 File Offset: 0x000C2995
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06004A7F RID: 19071 RVA: 0x00269F38 File Offset: 0x00268138
	private void OnCopySettings(object data)
	{
		LogicSwitch component = ((GameObject)data).GetComponent<LogicSwitch>();
		if (component != null && this.switchedOn != component.switchedOn)
		{
			this.switchedOn = component.switchedOn;
			this.UpdateVisualization();
			this.UpdateLogicCircuit();
		}
	}

	// Token: 0x06004A80 RID: 19072 RVA: 0x000D4AAA File Offset: 0x000D2CAA
	protected override void Toggle()
	{
		base.Toggle();
		this.UpdateVisualization();
		this.UpdateLogicCircuit();
	}

	// Token: 0x06004A81 RID: 19073 RVA: 0x00269F80 File Offset: 0x00268180
	private void UpdateVisualization()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.wasOn != this.switchedOn)
		{
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06004A82 RID: 19074 RVA: 0x000CEE6A File Offset: 0x000CD06A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06004A83 RID: 19075 RVA: 0x0026A004 File Offset: 0x00268204
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSwitchStatusActive : Db.Get().BuildingStatusItems.LogicSwitchStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x06004A84 RID: 19076 RVA: 0x000D4ABE File Offset: 0x000D2CBE
	public void Sim33ms(float dt)
	{
		if (this.ToggleRequested)
		{
			this.Toggle();
			this.ToggleRequested = false;
			this.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
	}

	// Token: 0x06004A85 RID: 19077 RVA: 0x000D4AF2 File Offset: 0x000D2CF2
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06004A86 RID: 19078 RVA: 0x000D4B08 File Offset: 0x000D2D08
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06004A87 RID: 19079 RVA: 0x000CE030 File Offset: 0x000CC230
	public void ToggledByPlayer()
	{
		this.Toggle();
	}

	// Token: 0x06004A88 RID: 19080 RVA: 0x000CE038 File Offset: 0x000CC238
	public bool ToggledOn()
	{
		return this.switchedOn;
	}

	// Token: 0x06004A89 RID: 19081 RVA: 0x000CE040 File Offset: 0x000CC240
	public KSelectable GetSelectable()
	{
		return base.GetComponent<KSelectable>();
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x06004A8A RID: 19082 RVA: 0x000D4B17 File Offset: 0x000D2D17
	public string SideScreenTitleKey
	{
		get
		{
			return "STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.SIDESCREEN_TITLE";
		}
	}

	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x06004A8B RID: 19083 RVA: 0x000D4B1E File Offset: 0x000D2D1E
	// (set) Token: 0x06004A8C RID: 19084 RVA: 0x000D4B26 File Offset: 0x000D2D26
	public bool ToggleRequested { get; set; }

	// Token: 0x0400345A RID: 13402
	public static readonly HashedString PORT_ID = "LogicSwitch";

	// Token: 0x0400345B RID: 13403
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400345C RID: 13404
	private static readonly EventSystem.IntraObjectHandler<LogicSwitch> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicSwitch>(delegate(LogicSwitch component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400345D RID: 13405
	private bool wasOn;

	// Token: 0x0400345E RID: 13406
	private System.Action firstFrameCallback;
}
