using System;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CCD RID: 15565
	public class SatelliteCrashEvent : GameplayEvent<SatelliteCrashEvent.StatesInstance>
	{
		// Token: 0x0600EEEB RID: 61163 RVA: 0x00144BE0 File Offset: 0x00142DE0
		public SatelliteCrashEvent() : base("SatelliteCrash", 0, 0)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.SATELLITE_CRASH.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.SATELLITE_CRASH.DESCRIPTION;
		}

		// Token: 0x0600EEEC RID: 61164 RVA: 0x00144C0F File Offset: 0x00142E0F
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new SatelliteCrashEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x02003CCE RID: 15566
		public class StatesInstance : GameplayEventStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, SatelliteCrashEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600EEED RID: 61165 RVA: 0x00144C19 File Offset: 0x00142E19
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, SatelliteCrashEvent crashEvent) : base(master, eventInstance, crashEvent)
			{
			}

			// Token: 0x0600EEEE RID: 61166 RVA: 0x004E8858 File Offset: 0x004E6A58
			public Notification Plan()
			{
				Vector3 position = new Vector3((float)(Grid.WidthInCells / 2 + UnityEngine.Random.Range(-Grid.WidthInCells / 3, Grid.WidthInCells / 3)), (float)(Grid.HeightInCells - 1), Grid.GetLayerZ(Grid.SceneLayer.FXFront));
				GameObject spawn = Util.KInstantiate(Assets.GetPrefab(SatelliteCometConfig.ID), position);
				spawn.SetActive(true);
				Notification notification = EventInfoScreen.CreateNotification(base.smi.sm.GenerateEventPopupData(base.smi), null);
				notification.clickFocus = spawn.transform;
				Comet component = spawn.GetComponent<Comet>();
				component.OnImpact = (System.Action)Delegate.Combine(component.OnImpact, new System.Action(delegate()
				{
					GameObject gameObject = new GameObject();
					gameObject.transform.position = spawn.transform.position;
					notification.clickFocus = gameObject.transform;
					GridVisibility.Reveal(Grid.PosToXY(gameObject.transform.position).x, Grid.PosToXY(gameObject.transform.position).y, 6, 4f);
				}));
				return notification;
			}
		}

		// Token: 0x02003CD0 RID: 15568
		public class States : GameplayEventStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, SatelliteCrashEvent>
		{
			// Token: 0x0600EEF1 RID: 61169 RVA: 0x004E89A4 File Offset: 0x004E6BA4
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.notify;
				this.notify.ToggleNotification((SatelliteCrashEvent.StatesInstance smi) => smi.Plan());
				this.ending.ReturnSuccess();
			}

			// Token: 0x0600EEF2 RID: 61170 RVA: 0x004E89F0 File Offset: 0x004E6BF0
			public override EventInfoData GenerateEventPopupData(SatelliteCrashEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				eventInfoData.location = GAMEPLAY_EVENTS.LOCATIONS.SURFACE;
				eventInfoData.whenDescription = GAMEPLAY_EVENTS.TIMES.NOW;
				eventInfoData.AddDefaultOption(delegate
				{
					smi.GoTo(smi.sm.ending);
				});
				return eventInfoData;
			}

			// Token: 0x0400EAB5 RID: 60085
			public GameStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, object>.State notify;

			// Token: 0x0400EAB6 RID: 60086
			public GameStateMachine<SatelliteCrashEvent.States, SatelliteCrashEvent.StatesInstance, GameplayEventManager, object>.State ending;
		}
	}
}
