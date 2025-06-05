using System;
using STRINGS;

// Token: 0x02000ED2 RID: 3794
public class MassiveHeatSink : StateMachineComponent<MassiveHeatSink.StatesInstance>
{
	// Token: 0x06004BE8 RID: 19432 RVA: 0x000D575B File Offset: 0x000D395B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0400351D RID: 13597
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400351E RID: 13598
	[MyCmpReq]
	private ElementConverter elementConverter;

	// Token: 0x02000ED3 RID: 3795
	public class States : GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink>
	{
		// Token: 0x06004BEA RID: 19434 RVA: 0x0026E240 File Offset: 0x0026C440
		private string AwaitingFuelResolveString(string str, object obj)
		{
			ElementConverter elementConverter = ((MassiveHeatSink.StatesInstance)obj).master.elementConverter;
			string arg = elementConverter.consumedElements[0].Tag.ProperName();
			string formattedMass = GameUtil.GetFormattedMass(elementConverter.consumedElements[0].MassConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
			str = string.Format(str, arg, formattedMass);
			return str;
		}

		// Token: 0x06004BEB RID: 19435 RVA: 0x0026E2A0 File Offset: 0x0026C4A0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.idle, (MassiveHeatSink.StatesInstance smi) => smi.master.operational.IsOperational);
			GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State state = this.idle.EventTransition(GameHashes.OperationalChanged, this.disabled, (MassiveHeatSink.StatesInstance smi) => !smi.master.operational.IsOperational);
			string name = BUILDING.STATUSITEMS.AWAITINGFUEL.NAME;
			string tooltip = BUILDING.STATUSITEMS.AWAITINGFUEL.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Exclamation;
			NotificationType notification_type = NotificationType.BadMinor;
			bool allow_multiples = false;
			Func<string, MassiveHeatSink.StatesInstance, string> resolve_string_callback = new Func<string, MassiveHeatSink.StatesInstance, string>(this.AwaitingFuelResolveString);
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, resolve_string_callback, null, null).EventTransition(GameHashes.OnStorageChange, this.active, (MassiveHeatSink.StatesInstance smi) => smi.master.elementConverter.HasEnoughMassToStartConverting(false));
			this.active.EventTransition(GameHashes.OperationalChanged, this.disabled, (MassiveHeatSink.StatesInstance smi) => !smi.master.operational.IsOperational).EventTransition(GameHashes.OnStorageChange, this.idle, (MassiveHeatSink.StatesInstance smi) => !smi.master.elementConverter.HasEnoughMassToStartConverting(false)).Enter(delegate(MassiveHeatSink.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(MassiveHeatSink.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
		}

		// Token: 0x0400351F RID: 13599
		public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State disabled;

		// Token: 0x04003520 RID: 13600
		public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State idle;

		// Token: 0x04003521 RID: 13601
		public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State active;
	}

	// Token: 0x02000ED5 RID: 3797
	public class StatesInstance : GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.GameInstance
	{
		// Token: 0x06004BF6 RID: 19446 RVA: 0x000D5802 File Offset: 0x000D3A02
		public StatesInstance(MassiveHeatSink master) : base(master)
		{
		}
	}
}
