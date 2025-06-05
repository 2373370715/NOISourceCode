using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000F7F RID: 3967
public class Reactor : StateMachineComponent<Reactor.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x06004FBA RID: 20410 RVA: 0x000D85F0 File Offset: 0x000D67F0
	// (set) Token: 0x06004FBB RID: 20411 RVA: 0x000D85F8 File Offset: 0x000D67F8
	private float ReactionMassTarget
	{
		get
		{
			return this.reactionMassTarget;
		}
		set
		{
			this.fuelDelivery.capacity = value * 2f;
			this.fuelDelivery.refillMass = value * 0.2f;
			this.fuelDelivery.MinimumMass = value * 0.2f;
			this.reactionMassTarget = value;
		}
	}

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x06004FBC RID: 20412 RVA: 0x000D8637 File Offset: 0x000D6837
	public float FuelTemperature
	{
		get
		{
			if (this.reactionStorage.items.Count > 0)
			{
				return this.reactionStorage.items[0].GetComponent<PrimaryElement>().Temperature;
			}
			return -1f;
		}
	}

	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x06004FBD RID: 20413 RVA: 0x0027A09C File Offset: 0x0027829C
	public float ReserveCoolantMass
	{
		get
		{
			PrimaryElement storedCoolant = this.GetStoredCoolant();
			if (!(storedCoolant == null))
			{
				return storedCoolant.Mass;
			}
			return 0f;
		}
	}

	// Token: 0x17000479 RID: 1145
	// (get) Token: 0x06004FBE RID: 20414 RVA: 0x000D866D File Offset: 0x000D686D
	public bool On
	{
		get
		{
			return base.smi.IsInsideState(base.smi.sm.on);
		}
	}

	// Token: 0x06004FBF RID: 20415 RVA: 0x0027A0C8 File Offset: 0x002782C8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.NuclearReactors.Add(this);
		Storage[] components = base.GetComponents<Storage>();
		this.supplyStorage = components[0];
		this.reactionStorage = components[1];
		this.wasteStorage = components[2];
		this.CreateMeters();
		base.smi.StartSM();
		this.fuelDelivery = base.GetComponent<ManualDeliveryKG>();
		this.CheckLogicInputValueChanged(true);
	}

	// Token: 0x06004FC0 RID: 20416 RVA: 0x000D868A File Offset: 0x000D688A
	protected override void OnCleanUp()
	{
		Components.NuclearReactors.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06004FC1 RID: 20417 RVA: 0x000D869D File Offset: 0x000D689D
	private void Update()
	{
		this.CheckLogicInputValueChanged(false);
	}

	// Token: 0x06004FC2 RID: 20418 RVA: 0x0027A12C File Offset: 0x0027832C
	public Notification CreateMeltdownNotification()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		return new Notification(MISC.NOTIFICATIONS.REACTORMELTDOWN.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.REACTORMELTDOWN.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + component.GetProperName(), false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06004FC3 RID: 20419 RVA: 0x000D86A6 File Offset: 0x000D68A6
	public void SetStorages(Storage supply, Storage reaction, Storage waste)
	{
		this.supplyStorage = supply;
		this.reactionStorage = reaction;
		this.wasteStorage = waste;
	}

	// Token: 0x06004FC4 RID: 20420 RVA: 0x0027A18C File Offset: 0x0027838C
	private void CheckLogicInputValueChanged(bool onLoad = false)
	{
		int num = 1;
		if (this.logicPorts.IsPortConnected("CONTROL_FUEL_DELIVERY"))
		{
			num = this.logicPorts.GetInputValue("CONTROL_FUEL_DELIVERY");
		}
		if (num == 0 && (this.fuelDeliveryEnabled || onLoad))
		{
			this.fuelDelivery.refillMass = -1f;
			this.fuelDeliveryEnabled = false;
			this.fuelDelivery.AbortDelivery("AutomationDisabled");
			return;
		}
		if (num == 1 && (!this.fuelDeliveryEnabled || onLoad))
		{
			this.fuelDelivery.refillMass = this.reactionMassTarget * 0.2f;
			this.fuelDeliveryEnabled = true;
		}
	}

	// Token: 0x06004FC5 RID: 20421 RVA: 0x000AA038 File Offset: 0x000A8238
	private void OnLogicConnectionChanged(int value, bool connection)
	{
	}

	// Token: 0x06004FC6 RID: 20422 RVA: 0x0027A22C File Offset: 0x0027842C
	private void CreateMeters()
	{
		this.temperatureMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "temperature_meter_target", "meter_temperature", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"temperature_meter_target"
		});
		this.waterMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "water_meter_target", "meter_water", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"water_meter_target"
		});
	}

	// Token: 0x06004FC7 RID: 20423 RVA: 0x0027A294 File Offset: 0x00278494
	private void TransferFuel()
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		PrimaryElement storedFuel = this.GetStoredFuel();
		float num = (activeFuel != null) ? activeFuel.Mass : 0f;
		float num2 = (storedFuel != null) ? storedFuel.Mass : 0f;
		float num3 = this.ReactionMassTarget - num;
		num3 = Mathf.Min(num2, num3);
		if (num3 > 0.5f || num3 == num2)
		{
			this.supplyStorage.Transfer(this.reactionStorage, this.fuelTag, num3, false, true);
		}
	}

	// Token: 0x06004FC8 RID: 20424 RVA: 0x0027A31C File Offset: 0x0027851C
	private void TransferCoolant()
	{
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		PrimaryElement storedCoolant = this.GetStoredCoolant();
		float num = (activeCoolant != null) ? activeCoolant.Mass : 0f;
		float a = (storedCoolant != null) ? storedCoolant.Mass : 0f;
		float num2 = 30f - num;
		num2 = Mathf.Min(a, num2);
		if (num2 > 0f)
		{
			this.supplyStorage.Transfer(this.reactionStorage, this.coolantTag, num2, false, true);
		}
	}

	// Token: 0x06004FC9 RID: 20425 RVA: 0x0027A398 File Offset: 0x00278598
	private PrimaryElement GetStoredFuel()
	{
		GameObject gameObject = this.supplyStorage.FindFirst(this.fuelTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06004FCA RID: 20426 RVA: 0x0027A3D4 File Offset: 0x002785D4
	private PrimaryElement GetActiveFuel()
	{
		GameObject gameObject = this.reactionStorage.FindFirst(this.fuelTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06004FCB RID: 20427 RVA: 0x0027A410 File Offset: 0x00278610
	private PrimaryElement GetStoredCoolant()
	{
		GameObject gameObject = this.supplyStorage.FindFirst(this.coolantTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06004FCC RID: 20428 RVA: 0x0027A44C File Offset: 0x0027864C
	private PrimaryElement GetActiveCoolant()
	{
		GameObject gameObject = this.reactionStorage.FindFirst(this.coolantTag);
		if (gameObject && gameObject.GetComponent<PrimaryElement>())
		{
			return gameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06004FCD RID: 20429 RVA: 0x0027A488 File Offset: 0x00278688
	private bool CanStartReaction()
	{
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		PrimaryElement activeFuel = this.GetActiveFuel();
		return activeCoolant && activeFuel && activeCoolant.Mass >= 30f && activeFuel.Mass >= 0.5f;
	}

	// Token: 0x06004FCE RID: 20430 RVA: 0x0027A4D0 File Offset: 0x002786D0
	private void Cool(float dt)
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		if (activeFuel == null)
		{
			return;
		}
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		if (activeCoolant == null)
		{
			return;
		}
		GameUtil.ForceConduction(activeFuel, activeCoolant, dt * 5f);
		if (activeCoolant.Temperature > 673.15f)
		{
			base.smi.sm.doVent.Trigger(base.smi);
		}
	}

	// Token: 0x06004FCF RID: 20431 RVA: 0x0027A538 File Offset: 0x00278738
	private void React(float dt)
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		if (activeFuel != null && activeFuel.Mass >= 0.25f)
		{
			float num = GameUtil.EnergyToTemperatureDelta(-100f * dt * activeFuel.Mass, activeFuel);
			activeFuel.Temperature += num;
			this.spentFuel += dt * 0.016666668f;
		}
	}

	// Token: 0x06004FD0 RID: 20432 RVA: 0x000D86BD File Offset: 0x000D68BD
	private void SetEmitRads(float rads)
	{
		base.smi.master.radEmitter.emitRads = rads;
		base.smi.master.radEmitter.Refresh();
	}

	// Token: 0x06004FD1 RID: 20433 RVA: 0x0027A59C File Offset: 0x0027879C
	private bool ReadyToCool()
	{
		PrimaryElement activeCoolant = this.GetActiveCoolant();
		return activeCoolant != null && activeCoolant.Mass > 0f;
	}

	// Token: 0x06004FD2 RID: 20434 RVA: 0x0027A5C8 File Offset: 0x002787C8
	private void DumpSpentFuel()
	{
		PrimaryElement activeFuel = this.GetActiveFuel();
		if (activeFuel != null)
		{
			if (this.spentFuel <= 0f)
			{
				return;
			}
			float num = this.spentFuel * 100f;
			if (num > 0f)
			{
				this.wasteStorage.AddLiquid(SimHashes.NuclearWaste, num, activeFuel.Temperature, Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id), Mathf.RoundToInt(num * 50f), false, true);
			}
			if (this.wasteStorage.MassStored() >= 100f)
			{
				this.wasteStorage.DropAll(true, true, default(Vector3), true, null);
			}
			if (this.spentFuel >= activeFuel.Mass)
			{
				Util.KDestroyGameObject(activeFuel.gameObject);
				this.spentFuel = 0f;
				return;
			}
			activeFuel.Mass -= this.spentFuel;
			this.spentFuel = 0f;
		}
	}

	// Token: 0x06004FD3 RID: 20435 RVA: 0x0027A6C4 File Offset: 0x002788C4
	private void UpdateVentStatus()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.ClearToVent())
		{
			if (component.HasStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure))
			{
				base.smi.sm.canVent.Set(true, base.smi, false);
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure, false);
				return;
			}
		}
		else if (!component.HasStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure))
		{
			base.smi.sm.canVent.Set(false, base.smi, false);
			component.AddStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure, null);
		}
	}

	// Token: 0x06004FD4 RID: 20436 RVA: 0x0027A77C File Offset: 0x0027897C
	private void UpdateCoolantStatus()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.GetStoredCoolant() != null || base.smi.GetCurrentState() == base.smi.sm.meltdown || base.smi.GetCurrentState() == base.smi.sm.dead)
		{
			if (component.HasStatusItem(Db.Get().BuildingStatusItems.NoCoolant))
			{
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.NoCoolant, false);
				return;
			}
		}
		else if (!component.HasStatusItem(Db.Get().BuildingStatusItems.NoCoolant))
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.NoCoolant, null);
		}
	}

	// Token: 0x06004FD5 RID: 20437 RVA: 0x0027A838 File Offset: 0x00278A38
	private void InitVentCells()
	{
		if (this.ventCells == null)
		{
			this.ventCells = new int[]
			{
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.zero),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.left),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.right + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.left + Vector3.left),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.left),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.right + Vector3.right),
				Grid.PosToCell(base.transform.GetPosition() + base.smi.master.dumpOffset + Vector3.down + Vector3.left + Vector3.left)
			};
		}
	}

	// Token: 0x06004FD6 RID: 20438 RVA: 0x0027AAA4 File Offset: 0x00278CA4
	public int GetVentCell()
	{
		this.InitVentCells();
		for (int i = 0; i < this.ventCells.Length; i++)
		{
			if (Grid.Mass[this.ventCells[i]] < 150f && !Grid.Solid[this.ventCells[i]])
			{
				return this.ventCells[i];
			}
		}
		return -1;
	}

	// Token: 0x06004FD7 RID: 20439 RVA: 0x0027AB04 File Offset: 0x00278D04
	private bool ClearToVent()
	{
		this.InitVentCells();
		for (int i = 0; i < this.ventCells.Length; i++)
		{
			if (Grid.Mass[this.ventCells[i]] < 150f && !Grid.Solid[this.ventCells[i]])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004FD8 RID: 20440 RVA: 0x000CE880 File Offset: 0x000CCA80
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>();
	}

	// Token: 0x04003822 RID: 14370
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04003823 RID: 14371
	[MyCmpGet]
	private RadiationEmitter radEmitter;

	// Token: 0x04003824 RID: 14372
	[MyCmpGet]
	private ManualDeliveryKG fuelDelivery;

	// Token: 0x04003825 RID: 14373
	private MeterController temperatureMeter;

	// Token: 0x04003826 RID: 14374
	private MeterController waterMeter;

	// Token: 0x04003827 RID: 14375
	private Storage supplyStorage;

	// Token: 0x04003828 RID: 14376
	private Storage reactionStorage;

	// Token: 0x04003829 RID: 14377
	private Storage wasteStorage;

	// Token: 0x0400382A RID: 14378
	private Tag fuelTag = SimHashes.EnrichedUranium.CreateTag();

	// Token: 0x0400382B RID: 14379
	private Tag coolantTag = GameTags.AnyWater;

	// Token: 0x0400382C RID: 14380
	private Vector3 dumpOffset = new Vector3(0f, 5f, 0f);

	// Token: 0x0400382D RID: 14381
	public static string MELTDOWN_STINGER = "Stinger_Loop_NuclearMeltdown";

	// Token: 0x0400382E RID: 14382
	private static float meterFrameScaleHack = 3f;

	// Token: 0x0400382F RID: 14383
	[Serialize]
	private float spentFuel;

	// Token: 0x04003830 RID: 14384
	private float timeSinceMeltdownEmit;

	// Token: 0x04003831 RID: 14385
	private const float reactorMeltDownBonusMassAmount = 10f;

	// Token: 0x04003832 RID: 14386
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x04003833 RID: 14387
	private LogicEventHandler fuelControlPort;

	// Token: 0x04003834 RID: 14388
	private bool fuelDeliveryEnabled = true;

	// Token: 0x04003835 RID: 14389
	public Guid refuelStausHandle;

	// Token: 0x04003836 RID: 14390
	[Serialize]
	public int numCyclesRunning;

	// Token: 0x04003837 RID: 14391
	private float reactionMassTarget = 60f;

	// Token: 0x04003838 RID: 14392
	private int[] ventCells;

	// Token: 0x02000F80 RID: 3968
	public class StatesInstance : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.GameInstance
	{
		// Token: 0x06004FDB RID: 20443 RVA: 0x000D8700 File Offset: 0x000D6900
		public StatesInstance(Reactor smi) : base(smi)
		{
		}
	}

	// Token: 0x02000F81 RID: 3969
	public class States : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor>
	{
		// Token: 0x06004FDC RID: 20444 RVA: 0x0027ABB8 File Offset: 0x00278DB8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			default_state = this.off;
			this.root.EventHandler(GameHashes.OnStorageChange, delegate(Reactor.StatesInstance smi)
			{
				PrimaryElement storedCoolant = smi.master.GetStoredCoolant();
				if (!storedCoolant)
				{
					smi.master.waterMeter.SetPositionPercent(0f);
					return;
				}
				smi.master.waterMeter.SetPositionPercent(storedCoolant.Mass / 90f);
			});
			this.off_pre.QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.off);
			this.off.PlayAnim("off").Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.master.radEmitter.SetEmitting(false);
				smi.master.SetEmitRads(0f);
			}).ParamTransition<bool>(this.reactionUnderway, this.on, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).ParamTransition<bool>(this.melted, this.dead, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).ParamTransition<bool>(this.meltingDown, this.meltdown, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.master.TransferFuel();
				smi.master.TransferCoolant();
				if (smi.master.CanStartReaction())
				{
					smi.GoTo(this.on);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.on.Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.sm.reactionUnderway.Set(true, smi, false);
				smi.master.operational.SetActive(true, false);
				smi.master.SetEmitRads(2400f);
				smi.master.radEmitter.SetEmitting(true);
			}).EventHandler(GameHashes.NewDay, (Reactor.StatesInstance smi) => GameClock.Instance, delegate(Reactor.StatesInstance smi)
			{
				smi.master.numCyclesRunning++;
			}).Exit(delegate(Reactor.StatesInstance smi)
			{
				smi.sm.reactionUnderway.Set(false, smi, false);
				smi.master.numCyclesRunning = 0;
			}).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.master.TransferFuel();
				smi.master.TransferCoolant();
				smi.master.React(dt);
				smi.master.UpdateCoolantStatus();
				smi.master.UpdateVentStatus();
				smi.master.DumpSpentFuel();
				if (!smi.master.fuelDeliveryEnabled)
				{
					smi.master.refuelStausHandle = smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ReactorRefuelDisabled, null);
				}
				else
				{
					smi.master.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ReactorRefuelDisabled, false);
					smi.master.refuelStausHandle = Guid.Empty;
				}
				if (smi.master.GetActiveCoolant() != null)
				{
					smi.master.Cool(dt);
				}
				PrimaryElement activeFuel = smi.master.GetActiveFuel();
				if (activeFuel != null)
				{
					smi.master.temperatureMeter.SetPositionPercent(Mathf.Clamp01(activeFuel.Temperature / 3000f) / Reactor.meterFrameScaleHack);
					if (activeFuel.Temperature >= 3000f)
					{
						smi.sm.meltdownMassRemaining.Set(10f + smi.master.supplyStorage.MassStored() + smi.master.reactionStorage.MassStored() + smi.master.wasteStorage.MassStored(), smi, false);
						smi.master.supplyStorage.ConsumeAllIgnoringDisease();
						smi.master.reactionStorage.ConsumeAllIgnoringDisease();
						smi.master.wasteStorage.ConsumeAllIgnoringDisease();
						smi.GoTo(this.meltdown.pre);
						return;
					}
					if (activeFuel.Mass <= 0.25f)
					{
						smi.GoTo(this.off_pre);
						smi.master.temperatureMeter.SetPositionPercent(0f);
						return;
					}
				}
				else
				{
					smi.GoTo(this.off_pre);
					smi.master.temperatureMeter.SetPositionPercent(0f);
				}
			}, UpdateRate.SIM_200ms, false).DefaultState(this.on.pre);
			this.on.pre.PlayAnim("working_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.on.reacting).OnSignal(this.doVent, this.on.venting);
			this.on.reacting.PlayAnim("working_loop", KAnim.PlayMode.Loop).OnSignal(this.doVent, this.on.venting);
			this.on.venting.ParamTransition<bool>(this.canVent, this.on.venting.vent, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue).ParamTransition<bool>(this.canVent, this.on.venting.ventIssue, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsFalse);
			this.on.venting.ventIssue.PlayAnim("venting_issue", KAnim.PlayMode.Loop).ParamTransition<bool>(this.canVent, this.on.venting.vent, GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.IsTrue);
			this.on.venting.vent.PlayAnim("venting").Enter(delegate(Reactor.StatesInstance smi)
			{
				PrimaryElement activeCoolant = smi.master.GetActiveCoolant();
				if (activeCoolant != null)
				{
					activeCoolant.GetComponent<Dumpable>().Dump(Grid.CellToPos(smi.master.GetVentCell()));
				}
			}).OnAnimQueueComplete(this.on.reacting);
			this.meltdown.ToggleStatusItem(Db.Get().BuildingStatusItems.ReactorMeltdown, null).ToggleNotification((Reactor.StatesInstance smi) => smi.master.CreateMeltdownNotification()).ParamTransition<float>(this.meltdownMassRemaining, this.dead, (Reactor.StatesInstance smi, float p) => p <= 0f).ToggleTag(GameTags.DeadReactor).DefaultState(this.meltdown.loop);
			this.meltdown.pre.PlayAnim("almost_meltdown_pre", KAnim.PlayMode.Once).QueueAnim("almost_meltdown_loop", false, null).QueueAnim("meltdown_pre", false, null).OnAnimQueueComplete(this.meltdown.loop);
			this.meltdown.loop.PlayAnim("meltdown_loop", KAnim.PlayMode.Loop).Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.master.radEmitter.SetEmitting(true);
				smi.master.SetEmitRads(4800f);
				smi.master.temperatureMeter.SetPositionPercent(1f / Reactor.meterFrameScaleHack);
				smi.master.UpdateCoolantStatus();
				if (this.meltingDown.Get(smi))
				{
					MusicManager.instance.PlaySong(Reactor.MELTDOWN_STINGER, false);
					MusicManager.instance.StopDynamicMusic(false);
				}
				else
				{
					MusicManager.instance.PlaySong(Reactor.MELTDOWN_STINGER, false);
					MusicManager.instance.SetSongParameter(Reactor.MELTDOWN_STINGER, "Music_PlayStinger", 1f, true);
					MusicManager.instance.StopDynamicMusic(false);
				}
				this.meltingDown.Set(true, smi, false);
			}).Exit(delegate(Reactor.StatesInstance smi)
			{
				this.meltingDown.Set(false, smi, false);
				MusicManager.instance.SetSongParameter(Reactor.MELTDOWN_STINGER, "Music_NuclearMeltdownActive", 0f, true);
			}).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.master.timeSinceMeltdownEmit += dt;
				float num = 0.5f;
				float b = 5f;
				if (smi.master.timeSinceMeltdownEmit > num && smi.sm.meltdownMassRemaining.Get(smi) > 0f)
				{
					smi.master.timeSinceMeltdownEmit -= num;
					float num2 = Mathf.Min(smi.sm.meltdownMassRemaining.Get(smi), b);
					smi.sm.meltdownMassRemaining.Delta(-num2, smi);
					for (int i = 0; i < 3; i++)
					{
						if (num2 >= NuclearWasteCometConfig.MASS)
						{
							GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(NuclearWasteCometConfig.ID), smi.master.transform.position + Vector3.up * 2f, Quaternion.identity, null, null, true, 0);
							gameObject.SetActive(true);
							Comet component = gameObject.GetComponent<Comet>();
							component.ignoreObstacleForDamage.Set(smi.master.gameObject.GetComponent<KPrefabID>());
							component.addTiles = 1;
							int num3 = 270;
							while (num3 > 225 && num3 < 335)
							{
								num3 = UnityEngine.Random.Range(0, 360);
							}
							float f = (float)num3 * 3.1415927f / 180f;
							component.Velocity = new Vector2(-Mathf.Cos(f) * 20f, Mathf.Sin(f) * 20f);
							component.GetComponent<KBatchedAnimController>().Rotation = (float)(-(float)num3) - 90f;
							num2 -= NuclearWasteCometConfig.MASS;
						}
					}
					for (int j = 0; j < 3; j++)
					{
						if (num2 >= 0.001f)
						{
							SimMessages.AddRemoveSubstance(Grid.PosToCell(smi.master.transform.position + Vector3.up * 3f + Vector3.right * (float)j * 2f), SimHashes.NuclearWaste, CellEventLogger.Instance.ElementEmitted, num2 / 3f, 3000f, Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id), Mathf.RoundToInt(50f * (num2 / 3f)), true, -1);
						}
					}
				}
			}, UpdateRate.SIM_200ms, false);
			this.dead.PlayAnim("dead").ToggleTag(GameTags.DeadReactor).Enter(delegate(Reactor.StatesInstance smi)
			{
				smi.master.temperatureMeter.SetPositionPercent(1f / Reactor.meterFrameScaleHack);
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DeadReactorCoolingOff, smi);
				this.melted.Set(true, smi, false);
			}).Exit(delegate(Reactor.StatesInstance smi)
			{
				smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DeadReactorCoolingOff, false);
			}).Update(delegate(Reactor.StatesInstance smi, float dt)
			{
				smi.sm.timeSinceMeltdown.Delta(dt, smi);
				smi.master.radEmitter.emitRads = Mathf.Lerp(4800f, 0f, smi.sm.timeSinceMeltdown.Get(smi) / 3000f);
				smi.master.radEmitter.Refresh();
			}, UpdateRate.SIM_200ms, false);
		}

		// Token: 0x04003839 RID: 14393
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.Signal doVent;

		// Token: 0x0400383A RID: 14394
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter canVent = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter(true);

		// Token: 0x0400383B RID: 14395
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter reactionUnderway = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter();

		// Token: 0x0400383C RID: 14396
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter meltdownMassRemaining = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter(0f);

		// Token: 0x0400383D RID: 14397
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter timeSinceMeltdown = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.FloatParameter(0f);

		// Token: 0x0400383E RID: 14398
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter meltingDown = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter(false);

		// Token: 0x0400383F RID: 14399
		public StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter melted = new StateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.BoolParameter(false);

		// Token: 0x04003840 RID: 14400
		public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State off;

		// Token: 0x04003841 RID: 14401
		public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State off_pre;

		// Token: 0x04003842 RID: 14402
		public Reactor.States.ReactingStates on;

		// Token: 0x04003843 RID: 14403
		public Reactor.States.MeltdownStates meltdown;

		// Token: 0x04003844 RID: 14404
		public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State dead;

		// Token: 0x02000F82 RID: 3970
		public class ReactingStates : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State
		{
			// Token: 0x04003845 RID: 14405
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State pre;

			// Token: 0x04003846 RID: 14406
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State reacting;

			// Token: 0x04003847 RID: 14407
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State pst;

			// Token: 0x04003848 RID: 14408
			public Reactor.States.ReactingStates.VentingStates venting;

			// Token: 0x02000F83 RID: 3971
			public class VentingStates : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State
			{
				// Token: 0x04003849 RID: 14409
				public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State ventIssue;

				// Token: 0x0400384A RID: 14410
				public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State vent;
			}
		}

		// Token: 0x02000F84 RID: 3972
		public class MeltdownStates : GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State
		{
			// Token: 0x0400384B RID: 14411
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State almost_pre;

			// Token: 0x0400384C RID: 14412
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State almost_loop;

			// Token: 0x0400384D RID: 14413
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State pre;

			// Token: 0x0400384E RID: 14414
			public GameStateMachine<Reactor.States, Reactor.StatesInstance, Reactor, object>.State loop;
		}
	}
}
