using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000E6E RID: 3694
public class LogicBroadcastReceiver : KMonoBehaviour, ISimEveryTick
{
	// Token: 0x06004858 RID: 18520 RVA: 0x00263EEC File Offset: 0x002620EC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
		this.SetChannel(this.channel.Get());
		this.operational.SetFlag(LogicBroadcastReceiver.spaceVisible, this.IsSpaceVisible());
		this.operational.SetFlag(LogicBroadcastReceiver.validChannelInRange, this.CheckChannelValid() && LogicBroadcastReceiver.CheckRange(this.channel.Get().gameObject, base.gameObject));
		this.wasOperational = !this.operational.IsOperational;
		this.OnOperationalChanged(null);
	}

	// Token: 0x06004859 RID: 18521 RVA: 0x00263F90 File Offset: 0x00262190
	public void SimEveryTick(float dt)
	{
		bool flag = this.IsSpaceVisible();
		this.operational.SetFlag(LogicBroadcastReceiver.spaceVisible, flag);
		if (!flag)
		{
			if (this.spaceNotVisibleStatusItem == Guid.Empty)
			{
				this.spaceNotVisibleStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSurfaceSight, null);
			}
		}
		else if (this.spaceNotVisibleStatusItem != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.spaceNotVisibleStatusItem, false);
			this.spaceNotVisibleStatusItem = Guid.Empty;
		}
		bool flag2 = this.CheckChannelValid() && LogicBroadcastReceiver.CheckRange(this.channel.Get().gameObject, base.gameObject);
		this.operational.SetFlag(LogicBroadcastReceiver.validChannelInRange, flag2);
		if (flag2 && !this.syncToChannelComplete)
		{
			this.SyncWithBroadcast();
		}
	}

	// Token: 0x0600485A RID: 18522 RVA: 0x000D3565 File Offset: 0x000D1765
	public bool IsSpaceVisible()
	{
		return base.gameObject.GetMyWorld().IsModuleInterior || Grid.ExposedToSunlight[Grid.PosToCell(base.gameObject)] > 0;
	}

	// Token: 0x0600485B RID: 18523 RVA: 0x000D3593 File Offset: 0x000D1793
	private bool CheckChannelValid()
	{
		return this.channel.Get() != null && this.channel.Get().GetComponent<LogicPorts>().inputPorts != null;
	}

	// Token: 0x0600485C RID: 18524 RVA: 0x000D35C2 File Offset: 0x000D17C2
	public LogicBroadcaster GetChannel()
	{
		return this.channel.Get();
	}

	// Token: 0x0600485D RID: 18525 RVA: 0x00264068 File Offset: 0x00262268
	public void SetChannel(LogicBroadcaster broadcaster)
	{
		this.ClearChannel();
		if (broadcaster == null)
		{
			return;
		}
		this.channel.Set(broadcaster);
		this.syncToChannelComplete = false;
		this.channelEventListeners.Add(this.channel.Get().gameObject.Subscribe(-801688580, new Action<object>(this.OnChannelLogicEvent)));
		this.channelEventListeners.Add(this.channel.Get().gameObject.Subscribe(-592767678, new Action<object>(this.OnChannelLogicEvent)));
		this.SyncWithBroadcast();
	}

	// Token: 0x0600485E RID: 18526 RVA: 0x00264100 File Offset: 0x00262300
	private void ClearChannel()
	{
		if (this.CheckChannelValid())
		{
			for (int i = 0; i < this.channelEventListeners.Count; i++)
			{
				this.channel.Get().gameObject.Unsubscribe(this.channelEventListeners[i]);
			}
		}
		this.channelEventListeners.Clear();
	}

	// Token: 0x0600485F RID: 18527 RVA: 0x000D35CF File Offset: 0x000D17CF
	private void OnChannelLogicEvent(object data)
	{
		if (!this.channel.Get().GetComponent<Operational>().IsOperational)
		{
			return;
		}
		this.SyncWithBroadcast();
	}

	// Token: 0x06004860 RID: 18528 RVA: 0x00264158 File Offset: 0x00262358
	private void SyncWithBroadcast()
	{
		if (!this.CheckChannelValid())
		{
			return;
		}
		bool flag = LogicBroadcastReceiver.CheckRange(this.channel.Get().gameObject, base.gameObject);
		this.UpdateRangeStatus(flag);
		if (!flag)
		{
			return;
		}
		base.GetComponent<LogicPorts>().SendSignal(this.PORT_ID, this.channel.Get().GetCurrentValue());
		this.syncToChannelComplete = true;
	}

	// Token: 0x06004861 RID: 18529 RVA: 0x000D35EF File Offset: 0x000D17EF
	public static bool CheckRange(GameObject broadcaster, GameObject receiver)
	{
		return AxialUtil.GetDistance(broadcaster.GetMyWorldLocation(), receiver.GetMyWorldLocation()) <= LogicBroadcaster.RANGE;
	}

	// Token: 0x06004862 RID: 18530 RVA: 0x002641C4 File Offset: 0x002623C4
	private void UpdateRangeStatus(bool inRange)
	{
		if (!inRange && this.rangeStatusItem == Guid.Empty)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.rangeStatusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.BroadcasterOutOfRange, null);
			return;
		}
		if (this.rangeStatusItem != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.rangeStatusItem, false);
			this.rangeStatusItem = Guid.Empty;
		}
	}

	// Token: 0x06004863 RID: 18531 RVA: 0x0026423C File Offset: 0x0026243C
	private void OnOperationalChanged(object data)
	{
		if (this.operational.IsOperational)
		{
			if (!this.wasOperational)
			{
				this.wasOperational = true;
				this.animController.Queue("on_pre", KAnim.PlayMode.Once, 1f, 0f);
				this.animController.Queue("on", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		else if (this.wasOperational)
		{
			this.wasOperational = false;
			this.animController.Queue("on_pst", KAnim.PlayMode.Once, 1f, 0f);
			this.animController.Queue("off", KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x06004864 RID: 18532 RVA: 0x000D360C File Offset: 0x000D180C
	protected override void OnCleanUp()
	{
		this.ClearChannel();
		base.OnCleanUp();
	}

	// Token: 0x040032D0 RID: 13008
	[Serialize]
	private Ref<LogicBroadcaster> channel = new Ref<LogicBroadcaster>();

	// Token: 0x040032D1 RID: 13009
	public string PORT_ID = "";

	// Token: 0x040032D2 RID: 13010
	private List<int> channelEventListeners = new List<int>();

	// Token: 0x040032D3 RID: 13011
	private bool syncToChannelComplete;

	// Token: 0x040032D4 RID: 13012
	public static readonly Operational.Flag spaceVisible = new Operational.Flag("spaceVisible", Operational.Flag.Type.Requirement);

	// Token: 0x040032D5 RID: 13013
	public static readonly Operational.Flag validChannelInRange = new Operational.Flag("validChannelInRange", Operational.Flag.Type.Requirement);

	// Token: 0x040032D6 RID: 13014
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040032D7 RID: 13015
	private bool wasOperational;

	// Token: 0x040032D8 RID: 13016
	[MyCmpGet]
	private KBatchedAnimController animController;

	// Token: 0x040032D9 RID: 13017
	private Guid rangeStatusItem = Guid.Empty;

	// Token: 0x040032DA RID: 13018
	private Guid spaceNotVisibleStatusItem = Guid.Empty;
}
