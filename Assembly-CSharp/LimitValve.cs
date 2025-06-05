using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000E5B RID: 3675
[SerializationConfig(MemberSerialization.OptIn)]
public class LimitValve : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x1700037B RID: 891
	// (get) Token: 0x060047CC RID: 18380 RVA: 0x000D2F87 File Offset: 0x000D1187
	public float RemainingCapacity
	{
		get
		{
			return Mathf.Max(0f, this.m_limit - this.m_amount);
		}
	}

	// Token: 0x060047CD RID: 18381 RVA: 0x000D2FA0 File Offset: 0x000D11A0
	public NonLinearSlider.Range[] GetRanges()
	{
		if (this.sliderRanges != null && this.sliderRanges.Length != 0)
		{
			return this.sliderRanges;
		}
		return NonLinearSlider.GetDefaultRange(this.maxLimitKg);
	}

	// Token: 0x1700037C RID: 892
	// (get) Token: 0x060047CE RID: 18382 RVA: 0x000D2FC5 File Offset: 0x000D11C5
	// (set) Token: 0x060047CF RID: 18383 RVA: 0x000D2FCD File Offset: 0x000D11CD
	public float Limit
	{
		get
		{
			return this.m_limit;
		}
		set
		{
			this.m_limit = value;
			this.Refresh();
		}
	}

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x060047D0 RID: 18384 RVA: 0x000D2FDC File Offset: 0x000D11DC
	// (set) Token: 0x060047D1 RID: 18385 RVA: 0x000D2FE4 File Offset: 0x000D11E4
	public float Amount
	{
		get
		{
			return this.m_amount;
		}
		set
		{
			this.m_amount = value;
			base.Trigger(-1722241721, this.Amount);
			this.Refresh();
		}
	}

	// Token: 0x060047D2 RID: 18386 RVA: 0x000D3009 File Offset: 0x000D1209
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LimitValve>(-905833192, LimitValve.OnCopySettingsDelegate);
	}

	// Token: 0x060047D3 RID: 18387 RVA: 0x002619E4 File Offset: 0x0025FBE4
	protected override void OnSpawn()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Combine(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		base.Subscribe<LimitValve>(-801688580, LimitValve.OnLogicValueChangedDelegate);
		if (this.conduitType == ConduitType.Gas || this.conduitType == ConduitType.Liquid)
		{
			ConduitBridge conduitBridge = this.conduitBridge;
			conduitBridge.desiredMassTransfer = (ConduitBridgeBase.DesiredMassTransfer)Delegate.Combine(conduitBridge.desiredMassTransfer, new ConduitBridgeBase.DesiredMassTransfer(this.DesiredMassTransfer));
			ConduitBridge conduitBridge2 = this.conduitBridge;
			conduitBridge2.OnMassTransfer = (ConduitBridgeBase.ConduitBridgeEvent)Delegate.Combine(conduitBridge2.OnMassTransfer, new ConduitBridgeBase.ConduitBridgeEvent(this.OnMassTransfer));
		}
		else if (this.conduitType == ConduitType.Solid)
		{
			SolidConduitBridge solidConduitBridge = this.solidConduitBridge;
			solidConduitBridge.desiredMassTransfer = (ConduitBridgeBase.DesiredMassTransfer)Delegate.Combine(solidConduitBridge.desiredMassTransfer, new ConduitBridgeBase.DesiredMassTransfer(this.DesiredMassTransfer));
			SolidConduitBridge solidConduitBridge2 = this.solidConduitBridge;
			solidConduitBridge2.OnMassTransfer = (ConduitBridgeBase.ConduitBridgeEvent)Delegate.Combine(solidConduitBridge2.OnMassTransfer, new ConduitBridgeBase.ConduitBridgeEvent(this.OnMassTransfer));
		}
		if (this.limitMeter == null)
		{
			this.limitMeter = new MeterController(this.controller, "meter_target_counter", "meter_counter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target_counter"
			});
		}
		this.Refresh();
		base.OnSpawn();
	}

	// Token: 0x060047D4 RID: 18388 RVA: 0x000D3022 File Offset: 0x000D1222
	protected override void OnCleanUp()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Remove(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		base.OnCleanUp();
	}

	// Token: 0x060047D5 RID: 18389 RVA: 0x000D3055 File Offset: 0x000D1255
	private void LogicTick()
	{
		if (this.m_resetRequested)
		{
			this.ResetAmount();
		}
	}

	// Token: 0x060047D6 RID: 18390 RVA: 0x000D3065 File Offset: 0x000D1265
	public void ResetAmount()
	{
		this.m_resetRequested = false;
		this.Amount = 0f;
	}

	// Token: 0x060047D7 RID: 18391 RVA: 0x00261B28 File Offset: 0x0025FD28
	private float DesiredMassTransfer(float dt, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable)
	{
		if (!this.operational.IsOperational)
		{
			return 0f;
		}
		if (this.conduitType == ConduitType.Solid && pickupable != null && GameTags.DisplayAsUnits.Contains(pickupable.KPrefabID.PrefabID()))
		{
			float num = pickupable.PrimaryElement.Units;
			if (this.RemainingCapacity < num)
			{
				num = (float)Mathf.FloorToInt(this.RemainingCapacity);
			}
			return num * pickupable.PrimaryElement.MassPerUnit;
		}
		return Mathf.Min(mass, this.RemainingCapacity);
	}

	// Token: 0x060047D8 RID: 18392 RVA: 0x00261BB4 File Offset: 0x0025FDB4
	private void OnMassTransfer(SimHashes element, float transferredMass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable)
	{
		if (!LogicCircuitNetwork.IsBitActive(0, this.ports.GetInputValue(LimitValve.RESET_PORT_ID)))
		{
			if (this.conduitType == ConduitType.Gas || this.conduitType == ConduitType.Liquid)
			{
				this.Amount += transferredMass;
			}
			else if (this.conduitType == ConduitType.Solid && pickupable != null)
			{
				this.Amount += transferredMass / pickupable.PrimaryElement.MassPerUnit;
			}
		}
		this.operational.SetActive(this.operational.IsOperational && transferredMass > 0f, false);
		this.Refresh();
	}

	// Token: 0x060047D9 RID: 18393 RVA: 0x00261C54 File Offset: 0x0025FE54
	private void Refresh()
	{
		if (this.operational == null)
		{
			return;
		}
		this.ports.SendSignal(LimitValve.OUTPUT_PORT_ID, (this.RemainingCapacity <= 0f) ? 1 : 0);
		this.operational.SetFlag(LimitValve.limitNotReached, this.RemainingCapacity > 0f);
		if (this.RemainingCapacity > 0f)
		{
			this.limitMeter.meterController.Play("meter_counter", KAnim.PlayMode.Paused, 1f, 0f);
			this.limitMeter.SetPositionPercent(this.Amount / this.Limit);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.LimitValveLimitNotReached, this);
			return;
		}
		this.limitMeter.meterController.Play("meter_on", KAnim.PlayMode.Paused, 1f, 0f);
		this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.LimitValveLimitReached, this);
	}

	// Token: 0x060047DA RID: 18394 RVA: 0x00261D74 File Offset: 0x0025FF74
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == LimitValve.RESET_PORT_ID && LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue))
		{
			this.ResetAmount();
		}
	}

	// Token: 0x060047DB RID: 18395 RVA: 0x00261DB0 File Offset: 0x0025FFB0
	private void OnCopySettings(object data)
	{
		LimitValve component = ((GameObject)data).GetComponent<LimitValve>();
		if (component != null)
		{
			this.Limit = component.Limit;
		}
	}

	// Token: 0x04003255 RID: 12885
	public static readonly HashedString RESET_PORT_ID = new HashedString("LimitValveReset");

	// Token: 0x04003256 RID: 12886
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LimitValveOutput");

	// Token: 0x04003257 RID: 12887
	public static readonly Operational.Flag limitNotReached = new Operational.Flag("limitNotReached", Operational.Flag.Type.Requirement);

	// Token: 0x04003258 RID: 12888
	public ConduitType conduitType;

	// Token: 0x04003259 RID: 12889
	public float maxLimitKg = 100f;

	// Token: 0x0400325A RID: 12890
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400325B RID: 12891
	[MyCmpReq]
	private LogicPorts ports;

	// Token: 0x0400325C RID: 12892
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x0400325D RID: 12893
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x0400325E RID: 12894
	[MyCmpGet]
	private ConduitBridge conduitBridge;

	// Token: 0x0400325F RID: 12895
	[MyCmpGet]
	private SolidConduitBridge solidConduitBridge;

	// Token: 0x04003260 RID: 12896
	[Serialize]
	[SerializeField]
	private float m_limit;

	// Token: 0x04003261 RID: 12897
	[Serialize]
	private float m_amount;

	// Token: 0x04003262 RID: 12898
	[Serialize]
	private bool m_resetRequested;

	// Token: 0x04003263 RID: 12899
	private MeterController limitMeter;

	// Token: 0x04003264 RID: 12900
	public bool displayUnitsInsteadOfMass;

	// Token: 0x04003265 RID: 12901
	public NonLinearSlider.Range[] sliderRanges;

	// Token: 0x04003266 RID: 12902
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003267 RID: 12903
	private static readonly EventSystem.IntraObjectHandler<LimitValve> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LimitValve>(delegate(LimitValve component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04003268 RID: 12904
	private static readonly EventSystem.IntraObjectHandler<LimitValve> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LimitValve>(delegate(LimitValve component, object data)
	{
		component.OnCopySettings(data);
	});
}
