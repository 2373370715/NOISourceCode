using System;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02003CD7 RID: 15575
	public class SolarFlareEvent : GameplayEvent<SolarFlareEvent.StatesInstance>
	{
		// Token: 0x0600EF03 RID: 61187 RVA: 0x00144D3A File Offset: 0x00142F3A
		public SolarFlareEvent() : base("SolarFlareEvent", 0, 0)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.SOLAR_FLARE.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.SOLAR_FLARE.DESCRIPTION;
		}

		// Token: 0x0600EF04 RID: 61188 RVA: 0x00144D69 File Offset: 0x00142F69
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new SolarFlareEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0400EAC2 RID: 60098
		public const string ID = "SolarFlareEvent";

		// Token: 0x0400EAC3 RID: 60099
		public const float DURATION = 7f;

		// Token: 0x02003CD8 RID: 15576
		public class StatesInstance : GameplayEventStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, SolarFlareEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600EF05 RID: 61189 RVA: 0x00144D73 File Offset: 0x00142F73
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, SolarFlareEvent solarFlareEvent) : base(master, eventInstance, solarFlareEvent)
			{
			}
		}

		// Token: 0x02003CD9 RID: 15577
		public class States : GameplayEventStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, SolarFlareEvent>
		{
			// Token: 0x0600EF06 RID: 61190 RVA: 0x00144D7E File Offset: 0x00142F7E
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.idle;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.idle.DoNothing();
				this.start.ScheduleGoTo(7f, this.finished);
				this.finished.ReturnSuccess();
			}

			// Token: 0x0600EF07 RID: 61191 RVA: 0x004E8B88 File Offset: 0x004E6D88
			public override EventInfoData GenerateEventPopupData(SolarFlareEvent.StatesInstance smi)
			{
				return new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName)
				{
					location = GAMEPLAY_EVENTS.LOCATIONS.SUN,
					whenDescription = GAMEPLAY_EVENTS.TIMES.NOW
				};
			}

			// Token: 0x0400EAC4 RID: 60100
			public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State idle;

			// Token: 0x0400EAC5 RID: 60101
			public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State start;

			// Token: 0x0400EAC6 RID: 60102
			public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State finished;
		}
	}
}
