using System;
using UnityEngine;

// Token: 0x02000F61 RID: 3937
[AddComponentMenu("KMonoBehaviour/scripts/Pump")]
public class Pump : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06004EF5 RID: 20213 RVA: 0x000D7C1C File Offset: 0x000D5E1C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.consumer.EnableConsumption(false);
	}

	// Token: 0x06004EF6 RID: 20214 RVA: 0x000D7C30 File Offset: 0x000D5E30
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.elapsedTime = 0f;
		this.pumpable = this.UpdateOperational();
		this.dispenser.GetConduitManager().AddConduitUpdater(new Action<float>(this.OnConduitUpdate), ConduitFlowPriority.LastPostUpdate);
	}

	// Token: 0x06004EF7 RID: 20215 RVA: 0x000D7C6D File Offset: 0x000D5E6D
	protected override void OnCleanUp()
	{
		this.dispenser.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.OnConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x06004EF8 RID: 20216 RVA: 0x00277C04 File Offset: 0x00275E04
	public void Sim1000ms(float dt)
	{
		this.elapsedTime += dt;
		if (this.elapsedTime >= 1f)
		{
			this.pumpable = this.UpdateOperational();
			this.elapsedTime = 0f;
		}
		if (this.operational.IsOperational && this.pumpable)
		{
			this.operational.SetActive(true, false);
			return;
		}
		this.operational.SetActive(false, false);
	}

	// Token: 0x06004EF9 RID: 20217 RVA: 0x00277C74 File Offset: 0x00275E74
	private bool UpdateOperational()
	{
		Element.State state = Element.State.Vacuum;
		ConduitType conduitType = this.dispenser.conduitType;
		if (conduitType != ConduitType.Gas)
		{
			if (conduitType == ConduitType.Liquid)
			{
				state = Element.State.Liquid;
			}
		}
		else
		{
			state = Element.State.Gas;
		}
		bool flag = this.IsPumpable(state, (int)this.consumer.consumptionRadius);
		StatusItem status_item = (state == Element.State.Gas) ? Db.Get().BuildingStatusItems.NoGasElementToPump : Db.Get().BuildingStatusItems.NoLiquidElementToPump;
		this.noElementStatusGuid = this.selectable.ToggleStatusItem(status_item, this.noElementStatusGuid, !flag, null);
		this.operational.SetFlag(Pump.PumpableFlag, !this.storage.IsFull() && flag);
		return flag;
	}

	// Token: 0x06004EFA RID: 20218 RVA: 0x00277D18 File Offset: 0x00275F18
	private bool IsPumpable(Element.State expected_state, int radius)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		for (int i = 0; i < (int)this.consumer.consumptionRadius; i++)
		{
			for (int j = 0; j < (int)this.consumer.consumptionRadius; j++)
			{
				int num2 = num + j + Grid.WidthInCells * i;
				if (Grid.Element[num2].IsState(expected_state))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004EFB RID: 20219 RVA: 0x000D7C91 File Offset: 0x000D5E91
	private void OnConduitUpdate(float dt)
	{
		this.conduitBlockedStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.ConduitBlocked, this.conduitBlockedStatusGuid, this.dispenser.blocked, null);
	}

	// Token: 0x17000461 RID: 1121
	// (get) Token: 0x06004EFC RID: 20220 RVA: 0x000D7CC5 File Offset: 0x000D5EC5
	public ConduitType conduitType
	{
		get
		{
			return this.dispenser.conduitType;
		}
	}

	// Token: 0x04003763 RID: 14179
	public static readonly Operational.Flag PumpableFlag = new Operational.Flag("vent", Operational.Flag.Type.Requirement);

	// Token: 0x04003764 RID: 14180
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003765 RID: 14181
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04003766 RID: 14182
	[MyCmpGet]
	private ElementConsumer consumer;

	// Token: 0x04003767 RID: 14183
	[MyCmpGet]
	private ConduitDispenser dispenser;

	// Token: 0x04003768 RID: 14184
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04003769 RID: 14185
	private const float OperationalUpdateInterval = 1f;

	// Token: 0x0400376A RID: 14186
	private float elapsedTime;

	// Token: 0x0400376B RID: 14187
	private bool pumpable;

	// Token: 0x0400376C RID: 14188
	private Guid conduitBlockedStatusGuid;

	// Token: 0x0400376D RID: 14189
	private Guid noElementStatusGuid;
}
