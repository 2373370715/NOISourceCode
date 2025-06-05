using System;
using UnityEngine;

// Token: 0x02001680 RID: 5760
public class EntityElementExchanger : StateMachineComponent<EntityElementExchanger.StatesInstance>
{
	// Token: 0x0600770C RID: 30476 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600770D RID: 30477 RVA: 0x000F2C1F File Offset: 0x000F0E1F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0600770E RID: 30478 RVA: 0x000F2C32 File Offset: 0x000F0E32
	public void SetConsumptionRate(float consumptionRate)
	{
		this.consumeRate = consumptionRate;
	}

	// Token: 0x0600770F RID: 30479 RVA: 0x00319FE8 File Offset: 0x003181E8
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		EntityElementExchanger entityElementExchanger = (EntityElementExchanger)data;
		if (entityElementExchanger != null)
		{
			entityElementExchanger.OnSimConsume(mass_cb_info);
		}
	}

	// Token: 0x06007710 RID: 30480 RVA: 0x0031A00C File Offset: 0x0031820C
	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		float num = mass_cb_info.mass * base.smi.master.exchangeRatio;
		if (this.reportExchange && base.smi.master.emittedElement == SimHashes.Oxygen)
		{
			string text = base.gameObject.GetProperName();
			ReceptacleMonitor component = base.GetComponent<ReceptacleMonitor>();
			if (component != null && component.GetReceptacle() != null)
			{
				text = text + " (" + component.GetReceptacle().gameObject.GetProperName() + ")";
			}
			ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, num, text, null);
		}
		SimMessages.EmitMass(Grid.PosToCell(base.smi.master.transform.GetPosition() + this.outputOffset), ElementLoader.FindElementByHash(base.smi.master.emittedElement).idx, num, ElementLoader.FindElementByHash(base.smi.master.emittedElement).defaultValues.temperature, byte.MaxValue, 0, -1);
	}

	// Token: 0x0400599D RID: 22941
	public Vector3 outputOffset = Vector3.zero;

	// Token: 0x0400599E RID: 22942
	public bool reportExchange;

	// Token: 0x0400599F RID: 22943
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040059A0 RID: 22944
	public SimHashes consumedElement;

	// Token: 0x040059A1 RID: 22945
	public SimHashes emittedElement;

	// Token: 0x040059A2 RID: 22946
	public float consumeRate;

	// Token: 0x040059A3 RID: 22947
	public float exchangeRatio;

	// Token: 0x02001681 RID: 5761
	public class StatesInstance : GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.GameInstance
	{
		// Token: 0x06007712 RID: 30482 RVA: 0x000F2C4E File Offset: 0x000F0E4E
		public StatesInstance(EntityElementExchanger master) : base(master)
		{
		}
	}

	// Token: 0x02001682 RID: 5762
	public class States : GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger>
	{
		// Token: 0x06007713 RID: 30483 RVA: 0x0031A118 File Offset: 0x00318318
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.exchanging;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.exchanging.Enter(delegate(EntityElementExchanger.StatesInstance smi)
			{
				WiltCondition component = smi.master.gameObject.GetComponent<WiltCondition>();
				if (component != null && component.IsWilting())
				{
					smi.GoTo(smi.sm.paused);
				}
			}).EventTransition(GameHashes.Wilt, this.paused, null).ToggleStatusItem(Db.Get().CreatureStatusItems.ExchangingElementConsume, null).ToggleStatusItem(Db.Get().CreatureStatusItems.ExchangingElementOutput, null).Update("EntityElementExchanger", delegate(EntityElementExchanger.StatesInstance smi, float dt)
			{
				HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(EntityElementExchanger.OnSimConsumeCallback), smi.master, "EntityElementExchanger");
				SimMessages.ConsumeMass(Grid.PosToCell(smi.master.gameObject), smi.master.consumedElement, smi.master.consumeRate * dt, 3, handle.index);
			}, UpdateRate.SIM_1000ms, false);
			this.paused.EventTransition(GameHashes.WiltRecover, this.exchanging, null);
		}

		// Token: 0x040059A4 RID: 22948
		public GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.State exchanging;

		// Token: 0x040059A5 RID: 22949
		public GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.State paused;
	}
}
