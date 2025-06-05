using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CC6 RID: 15558
	public class PlantBlightEvent : GameplayEvent<PlantBlightEvent.StatesInstance>
	{
		// Token: 0x0600EED4 RID: 61140 RVA: 0x004E8310 File Offset: 0x004E6510
		public PlantBlightEvent(string id, string targetPlantPrefab, float infectionDuration, float incubationDuration) : base(id, 0, 0)
		{
			this.targetPlantPrefab = targetPlantPrefab;
			this.infectionDuration = infectionDuration;
			this.incubationDuration = incubationDuration;
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.DESCRIPTION;
		}

		// Token: 0x0600EED5 RID: 61141 RVA: 0x00144B11 File Offset: 0x00142D11
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new PlantBlightEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0400EAA1 RID: 60065
		private const float BLIGHT_DISTANCE = 6f;

		// Token: 0x0400EAA2 RID: 60066
		public string targetPlantPrefab;

		// Token: 0x0400EAA3 RID: 60067
		public float infectionDuration;

		// Token: 0x0400EAA4 RID: 60068
		public float incubationDuration;

		// Token: 0x02003CC7 RID: 15559
		public class States : GameplayEventStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, PlantBlightEvent>
		{
			// Token: 0x0600EED6 RID: 61142 RVA: 0x004E835C File Offset: 0x004E655C
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				base.InitializeStates(out default_state);
				default_state = this.planning;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.planning.Enter(delegate(PlantBlightEvent.StatesInstance smi)
				{
					smi.InfectAPlant(true);
				}).GoTo(this.running);
				this.running.ToggleNotification((PlantBlightEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null)).EventHandlerTransition(GameHashes.Uprooted, this.finished, new Func<PlantBlightEvent.StatesInstance, object, bool>(this.NoBlightedPlants)).DefaultState(this.running.waiting).OnSignal(this.doFinish, this.finished);
				this.running.waiting.ParamTransition<float>(this.nextInfection, this.running.infect, (PlantBlightEvent.StatesInstance smi, float p) => p <= 0f).Update(delegate(PlantBlightEvent.StatesInstance smi, float dt)
				{
					this.nextInfection.Delta(-dt, smi);
				}, UpdateRate.SIM_4000ms, false);
				this.running.infect.Enter(delegate(PlantBlightEvent.StatesInstance smi)
				{
					smi.InfectAPlant(false);
				}).GoTo(this.running.waiting);
				this.finished.DoNotification((PlantBlightEvent.StatesInstance smi) => this.CreateSuccessNotification(smi, this.GenerateEventPopupData(smi))).ReturnSuccess();
			}

			// Token: 0x0600EED7 RID: 61143 RVA: 0x004E84BC File Offset: 0x004E66BC
			public override EventInfoData GenerateEventPopupData(PlantBlightEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				string value = smi.gameplayEvent.targetPlantPrefab.ToTag().ProperName();
				eventInfoData.location = GAMEPLAY_EVENTS.LOCATIONS.COLONY_WIDE;
				eventInfoData.whenDescription = GAMEPLAY_EVENTS.TIMES.NOW;
				eventInfoData.SetTextParameter("plant", value);
				return eventInfoData;
			}

			// Token: 0x0600EED8 RID: 61144 RVA: 0x004E8534 File Offset: 0x004E6734
			private Notification CreateSuccessNotification(PlantBlightEvent.StatesInstance smi, EventInfoData eventInfoData)
			{
				string plantName = smi.gameplayEvent.targetPlantPrefab.ToTag().ProperName();
				return new Notification(GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.SUCCESS.Replace("{plant}", plantName), NotificationType.Neutral, (List<Notification> list, object data) => GAMEPLAY_EVENTS.EVENT_TYPES.PLANT_BLIGHT.SUCCESS_TOOLTIP.Replace("{plant}", plantName), null, true, 0f, null, null, null, true, false, false);
			}

			// Token: 0x0600EED9 RID: 61145 RVA: 0x004E8598 File Offset: 0x004E6798
			private bool NoBlightedPlants(PlantBlightEvent.StatesInstance smi, object obj)
			{
				GameObject gameObject = (GameObject)obj;
				if (!gameObject.HasTag(GameTags.Blighted))
				{
					return true;
				}
				foreach (Crop crop in Components.Crops.Items.FindAll((Crop p) => p.name == smi.gameplayEvent.targetPlantPrefab))
				{
					if (!(gameObject.gameObject == crop.gameObject) && crop.HasTag(GameTags.Blighted))
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x0400EAA5 RID: 60069
			public GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State planning;

			// Token: 0x0400EAA6 RID: 60070
			public PlantBlightEvent.States.RunningStates running;

			// Token: 0x0400EAA7 RID: 60071
			public GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State finished;

			// Token: 0x0400EAA8 RID: 60072
			public StateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.Signal doFinish;

			// Token: 0x0400EAA9 RID: 60073
			public StateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.FloatParameter nextInfection;

			// Token: 0x02003CC8 RID: 15560
			public class RunningStates : GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400EAAA RID: 60074
				public GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State waiting;

				// Token: 0x0400EAAB RID: 60075
				public GameStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, object>.State infect;
			}
		}

		// Token: 0x02003CCC RID: 15564
		public class StatesInstance : GameplayEventStateMachine<PlantBlightEvent.States, PlantBlightEvent.StatesInstance, GameplayEventManager, PlantBlightEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600EEE8 RID: 61160 RVA: 0x00144BAD File Offset: 0x00142DAD
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, PlantBlightEvent blightEvent) : base(master, eventInstance, blightEvent)
			{
				this.startTime = Time.time;
			}

			// Token: 0x0600EEE9 RID: 61161 RVA: 0x004E8648 File Offset: 0x004E6848
			public void InfectAPlant(bool initialInfection)
			{
				if (Time.time - this.startTime > base.smi.gameplayEvent.infectionDuration)
				{
					base.sm.doFinish.Trigger(base.smi);
					return;
				}
				List<Crop> list = Components.Crops.Items.FindAll((Crop p) => p.name == base.smi.gameplayEvent.targetPlantPrefab);
				if (list.Count == 0)
				{
					base.sm.doFinish.Trigger(base.smi);
					return;
				}
				if (list.Count > 0)
				{
					List<Crop> list2 = new List<Crop>();
					List<Crop> list3 = new List<Crop>();
					foreach (Crop crop in list)
					{
						if (crop.HasTag(GameTags.Blighted))
						{
							list2.Add(crop);
						}
						else
						{
							list3.Add(crop);
						}
					}
					if (list2.Count == 0)
					{
						if (initialInfection)
						{
							Crop crop2 = list3[UnityEngine.Random.Range(0, list3.Count)];
							global::Debug.Log("Blighting a random plant", crop2);
							crop2.GetComponent<BlightVulnerable>().MakeBlighted();
						}
						else
						{
							base.sm.doFinish.Trigger(base.smi);
						}
					}
					else if (list3.Count > 0)
					{
						Crop crop3 = list2[UnityEngine.Random.Range(0, list2.Count)];
						global::Debug.Log("Spreading blight from a plant", crop3);
						list3.Shuffle<Crop>();
						foreach (Crop crop4 in list3)
						{
							if ((crop4.transform.GetPosition() - crop3.transform.GetPosition()).magnitude < 6f)
							{
								crop4.GetComponent<BlightVulnerable>().MakeBlighted();
								break;
							}
						}
					}
				}
				base.sm.nextInfection.Set(base.smi.gameplayEvent.incubationDuration, this, false);
			}

			// Token: 0x0400EAB2 RID: 60082
			[Serialize]
			private float startTime;
		}
	}
}
