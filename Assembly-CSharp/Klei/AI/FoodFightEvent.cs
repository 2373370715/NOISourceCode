using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CA9 RID: 15529
	public class FoodFightEvent : GameplayEvent<FoodFightEvent.StatesInstance>
	{
		// Token: 0x0600EE59 RID: 61017 RVA: 0x00144601 File Offset: 0x00142801
		public FoodFightEvent() : base("FoodFight", 0, 0)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.DESCRIPTION;
		}

		// Token: 0x0600EE5A RID: 61018 RVA: 0x00144630 File Offset: 0x00142830
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new FoodFightEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0400EA31 RID: 59953
		public const float FUTURE_TIME = 60f;

		// Token: 0x0400EA32 RID: 59954
		public const float DURATION = 60f;

		// Token: 0x02003CAA RID: 15530
		public class StatesInstance : GameplayEventStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, FoodFightEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600EE5B RID: 61019 RVA: 0x0014463A File Offset: 0x0014283A
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, FoodFightEvent foodEvent) : base(master, eventInstance, foodEvent)
			{
			}

			// Token: 0x0600EE5C RID: 61020 RVA: 0x004E66D4 File Offset: 0x004E48D4
			public void CreateChores(FoodFightEvent.StatesInstance smi)
			{
				this.chores = new List<FoodFightChore>();
				List<Room> list = Game.Instance.roomProber.rooms.FindAll((Room match) => match.roomType == Db.Get().RoomTypes.MessHall || match.roomType == Db.Get().RoomTypes.GreatHall);
				if (list == null || list.Count == 0)
				{
					return;
				}
				List<GameObject> buildingsOnFloor = list[UnityEngine.Random.Range(0, list.Count)].GetBuildingsOnFloor();
				for (int i = 0; i < Math.Min(Components.LiveMinionIdentities.Count, buildingsOnFloor.Count); i++)
				{
					IStateMachineTarget master = Components.LiveMinionIdentities[i];
					GameObject gameObject = buildingsOnFloor[UnityEngine.Random.Range(0, buildingsOnFloor.Count)];
					GameObject locator = ChoreHelpers.CreateLocator("FoodFightLocator", gameObject.transform.position);
					FoodFightChore foodFightChore = new FoodFightChore(master, locator);
					buildingsOnFloor.Remove(gameObject);
					FoodFightChore foodFightChore2 = foodFightChore;
					foodFightChore2.onExit = (Action<Chore>)Delegate.Combine(foodFightChore2.onExit, new Action<Chore>(delegate(Chore data)
					{
						Util.KDestroyGameObject(locator);
					}));
					this.chores.Add(foodFightChore);
				}
			}

			// Token: 0x0600EE5D RID: 61021 RVA: 0x004E67F4 File Offset: 0x004E49F4
			public void ClearChores()
			{
				if (this.chores != null)
				{
					for (int i = this.chores.Count - 1; i >= 0; i--)
					{
						if (this.chores[i] != null)
						{
							this.chores[i].Cancel("end");
						}
					}
				}
				this.chores = null;
			}

			// Token: 0x0400EA33 RID: 59955
			public List<FoodFightChore> chores;
		}

		// Token: 0x02003CAD RID: 15533
		public class States : GameplayEventStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, FoodFightEvent>
		{
			// Token: 0x0600EE63 RID: 61027 RVA: 0x004E684C File Offset: 0x004E4A4C
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				base.InitializeStates(out default_state);
				default_state = this.planning;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.root.Exit(delegate(FoodFightEvent.StatesInstance smi)
				{
					smi.ClearChores();
				});
				this.planning.ToggleNotification((FoodFightEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null));
				this.warmup.ToggleNotification((FoodFightEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null));
				this.warmup.wait.ScheduleGoTo(60f, this.warmup.start);
				this.warmup.start.Enter(delegate(FoodFightEvent.StatesInstance smi)
				{
					smi.CreateChores(smi);
				}).Update(delegate(FoodFightEvent.StatesInstance smi, float data)
				{
					int num = 0;
					foreach (FoodFightChore foodFightChore in smi.chores)
					{
						if (foodFightChore.smi.IsInsideState(foodFightChore.smi.sm.waitForParticipants))
						{
							num++;
						}
					}
					if (num >= smi.chores.Count || smi.timeinstate > 30f)
					{
						foreach (FoodFightChore foodFightChore2 in smi.chores)
						{
							foodFightChore2.gameObject.Trigger(-2043101269, null);
						}
						smi.GoTo(this.partying);
					}
				}, UpdateRate.RENDER_1000ms, false);
				this.partying.ToggleNotification((FoodFightEvent.StatesInstance smi) => new Notification(GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.UNDERWAY, NotificationType.Good, (List<Notification> a, object b) => GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.UNDERWAY_TOOLTIP, null, true, 0f, null, null, null, true, false, false)).ScheduleGoTo(60f, this.ending);
				this.ending.ReturnSuccess();
				this.canceled.DoNotification((FoodFightEvent.StatesInstance smi) => GameplayEventManager.CreateStandardCancelledNotification(this.GenerateEventPopupData(smi))).Enter(delegate(FoodFightEvent.StatesInstance smi)
				{
					foreach (object obj in Components.LiveMinionIdentities)
					{
						((MinionIdentity)obj).GetComponent<Effects>().Add("NoFunAllowed", true);
					}
				}).ReturnFailure();
			}

			// Token: 0x0600EE64 RID: 61028 RVA: 0x004E69B8 File Offset: 0x004E4BB8
			public override EventInfoData GenerateEventPopupData(FoodFightEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				eventInfoData.location = GAMEPLAY_EVENTS.LOCATIONS.PRINTING_POD;
				eventInfoData.whenDescription = string.Format(GAMEPLAY_EVENTS.TIMES.IN_CYCLES, 0.1f);
				eventInfoData.AddOption(GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.ACCEPT_OPTION_NAME, null).callback = delegate()
				{
					smi.GoTo(smi.sm.warmup.wait);
				};
				eventInfoData.AddOption(GAMEPLAY_EVENTS.EVENT_TYPES.FOOD_FIGHT.REJECT_OPTION_NAME, null).callback = delegate()
				{
					smi.GoTo(smi.sm.canceled);
				};
				return eventInfoData;
			}

			// Token: 0x0400EA37 RID: 59959
			public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State planning;

			// Token: 0x0400EA38 RID: 59960
			public FoodFightEvent.States.WarmupStates warmup;

			// Token: 0x0400EA39 RID: 59961
			public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State partying;

			// Token: 0x0400EA3A RID: 59962
			public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State ending;

			// Token: 0x0400EA3B RID: 59963
			public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State canceled;

			// Token: 0x02003CAE RID: 15534
			public class WarmupStates : GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400EA3C RID: 59964
				public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State wait;

				// Token: 0x0400EA3D RID: 59965
				public GameStateMachine<FoodFightEvent.States, FoodFightEvent.StatesInstance, GameplayEventManager, object>.State start;
			}
		}
	}
}
