using System;
using KSerialization;

// Token: 0x02000E6F RID: 3695
public class LogicBroadcaster : KMonoBehaviour, ISimEveryTick
{
	// Token: 0x1700037E RID: 894
	// (get) Token: 0x06004867 RID: 18535 RVA: 0x000D367B File Offset: 0x000D187B
	// (set) Token: 0x06004868 RID: 18536 RVA: 0x000D3683 File Offset: 0x000D1883
	public int BroadCastChannelID
	{
		get
		{
			return this.broadcastChannelID;
		}
		private set
		{
			this.broadcastChannelID = value;
		}
	}

	// Token: 0x06004869 RID: 18537 RVA: 0x000D368C File Offset: 0x000D188C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.LogicBroadcasters.Add(this);
	}

	// Token: 0x0600486A RID: 18538 RVA: 0x000D369F File Offset: 0x000D189F
	protected override void OnCleanUp()
	{
		Components.LogicBroadcasters.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600486B RID: 18539 RVA: 0x002642F8 File Offset: 0x002624F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicBroadcaster>(-801688580, LogicBroadcaster.OnLogicValueChangedDelegate);
		base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
		this.operational.SetFlag(LogicBroadcaster.spaceVisible, this.IsSpaceVisible());
		this.wasOperational = !this.operational.IsOperational;
		this.OnOperationalChanged(null);
	}

	// Token: 0x0600486C RID: 18540 RVA: 0x000D3565 File Offset: 0x000D1765
	public bool IsSpaceVisible()
	{
		return base.gameObject.GetMyWorld().IsModuleInterior || Grid.ExposedToSunlight[Grid.PosToCell(base.gameObject)] > 0;
	}

	// Token: 0x0600486D RID: 18541 RVA: 0x000D36B2 File Offset: 0x000D18B2
	public int GetCurrentValue()
	{
		return base.GetComponent<LogicPorts>().GetInputValue(this.PORT_ID);
	}

	// Token: 0x0600486E RID: 18542 RVA: 0x000AA038 File Offset: 0x000A8238
	private void OnLogicValueChanged(object data)
	{
	}

	// Token: 0x0600486F RID: 18543 RVA: 0x00264368 File Offset: 0x00262568
	public void SimEveryTick(float dt)
	{
		bool flag = this.IsSpaceVisible();
		this.operational.SetFlag(LogicBroadcaster.spaceVisible, flag);
		if (!flag)
		{
			if (this.spaceNotVisibleStatusItem == Guid.Empty)
			{
				this.spaceNotVisibleStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSurfaceSight, null);
				return;
			}
		}
		else if (this.spaceNotVisibleStatusItem != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.spaceNotVisibleStatusItem, false);
			this.spaceNotVisibleStatusItem = Guid.Empty;
		}
	}

	// Token: 0x06004870 RID: 18544 RVA: 0x002643F4 File Offset: 0x002625F4
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

	// Token: 0x040032DB RID: 13019
	public static int RANGE = 5;

	// Token: 0x040032DC RID: 13020
	private static int INVALID_CHANNEL_ID = -1;

	// Token: 0x040032DD RID: 13021
	public string PORT_ID = "";

	// Token: 0x040032DE RID: 13022
	private bool wasOperational;

	// Token: 0x040032DF RID: 13023
	[Serialize]
	private int broadcastChannelID = LogicBroadcaster.INVALID_CHANNEL_ID;

	// Token: 0x040032E0 RID: 13024
	private static readonly EventSystem.IntraObjectHandler<LogicBroadcaster> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicBroadcaster>(delegate(LogicBroadcaster component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x040032E1 RID: 13025
	public static readonly Operational.Flag spaceVisible = new Operational.Flag("spaceVisible", Operational.Flag.Type.Requirement);

	// Token: 0x040032E2 RID: 13026
	private Guid spaceNotVisibleStatusItem = Guid.Empty;

	// Token: 0x040032E3 RID: 13027
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040032E4 RID: 13028
	[MyCmpGet]
	private KBatchedAnimController animController;
}
