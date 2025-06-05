using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CA1 RID: 15521
	public class CreatureSpawnEvent : GameplayEvent<CreatureSpawnEvent.StatesInstance>
	{
		// Token: 0x0600EE3D RID: 60989 RVA: 0x001444C5 File Offset: 0x001426C5
		public CreatureSpawnEvent() : base("HatchSpawnEvent", 0, 0)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.CREATURE_SPAWN.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.CREATURE_SPAWN.DESCRIPTION;
		}

		// Token: 0x0600EE3E RID: 60990 RVA: 0x001444F4 File Offset: 0x001426F4
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new CreatureSpawnEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0400EA1C RID: 59932
		public const string ID = "HatchSpawnEvent";

		// Token: 0x0400EA1D RID: 59933
		public const float UPDATE_TIME = 4f;

		// Token: 0x0400EA1E RID: 59934
		public const float NUM_TO_SPAWN = 10f;

		// Token: 0x0400EA1F RID: 59935
		public const float duration = 40f;

		// Token: 0x0400EA20 RID: 59936
		public static List<string> CreatureSpawnEventIDs = new List<string>
		{
			"Hatch",
			"Squirrel",
			"Puft",
			"Crab",
			"Drecko",
			"Mole",
			"LightBug",
			"Pacu"
		};

		// Token: 0x02003CA2 RID: 15522
		public class StatesInstance : GameplayEventStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, CreatureSpawnEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600EE40 RID: 60992 RVA: 0x001444FE File Offset: 0x001426FE
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, CreatureSpawnEvent creatureEvent) : base(master, eventInstance, creatureEvent)
			{
			}

			// Token: 0x0600EE41 RID: 60993 RVA: 0x00144514 File Offset: 0x00142714
			private void PickCreatureToSpawn()
			{
				this.creatureID = CreatureSpawnEvent.CreatureSpawnEventIDs.GetRandom<string>();
			}

			// Token: 0x0600EE42 RID: 60994 RVA: 0x004E63D4 File Offset: 0x004E45D4
			private void PickSpawnLocations()
			{
				Vector3 position = Components.Telepads.Items.GetRandom<Telepad>().transform.GetPosition();
				int num = 100;
				ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
				GameScenePartitioner.Instance.GatherEntries((int)position.x - num / 2, (int)position.y - num / 2, num, num, GameScenePartitioner.Instance.plants, pooledList);
				foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
				{
					KPrefabID kprefabID = (KPrefabID)scenePartitionerEntry.obj;
					if (!kprefabID.GetComponent<TreeBud>())
					{
						base.smi.spawnPositions.Add(kprefabID.transform.GetPosition());
					}
				}
				pooledList.Recycle();
			}

			// Token: 0x0600EE43 RID: 60995 RVA: 0x00144526 File Offset: 0x00142726
			public void InitializeEvent()
			{
				this.PickCreatureToSpawn();
				this.PickSpawnLocations();
			}

			// Token: 0x0600EE44 RID: 60996 RVA: 0x00144534 File Offset: 0x00142734
			public void EndEvent()
			{
				this.creatureID = null;
				this.spawnPositions.Clear();
			}

			// Token: 0x0600EE45 RID: 60997 RVA: 0x004E64A8 File Offset: 0x004E46A8
			public void SpawnCreature()
			{
				if (this.spawnPositions.Count > 0)
				{
					Vector3 random = this.spawnPositions.GetRandom<Vector3>();
					Util.KInstantiate(Assets.GetPrefab(this.creatureID), random).SetActive(true);
				}
			}

			// Token: 0x0400EA21 RID: 59937
			[Serialize]
			private List<Vector3> spawnPositions = new List<Vector3>();

			// Token: 0x0400EA22 RID: 59938
			[Serialize]
			private string creatureID;
		}

		// Token: 0x02003CA3 RID: 15523
		public class States : GameplayEventStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, CreatureSpawnEvent>
		{
			// Token: 0x0600EE46 RID: 60998 RVA: 0x004E64EC File Offset: 0x004E46EC
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.initialize_event;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.initialize_event.Enter(delegate(CreatureSpawnEvent.StatesInstance smi)
				{
					smi.InitializeEvent();
					smi.GoTo(this.spawn_season);
				});
				this.start.DoNothing();
				this.spawn_season.Update(delegate(CreatureSpawnEvent.StatesInstance smi, float dt)
				{
					smi.SpawnCreature();
				}, UpdateRate.SIM_4000ms, false).Exit(delegate(CreatureSpawnEvent.StatesInstance smi)
				{
					smi.EndEvent();
				});
			}

			// Token: 0x0600EE47 RID: 60999 RVA: 0x004E6580 File Offset: 0x004E4780
			public override EventInfoData GenerateEventPopupData(CreatureSpawnEvent.StatesInstance smi)
			{
				return new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName)
				{
					location = GAMEPLAY_EVENTS.LOCATIONS.PRINTING_POD,
					whenDescription = GAMEPLAY_EVENTS.TIMES.NOW
				};
			}

			// Token: 0x0400EA23 RID: 59939
			public GameStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State initialize_event;

			// Token: 0x0400EA24 RID: 59940
			public GameStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State spawn_season;

			// Token: 0x0400EA25 RID: 59941
			public GameStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State start;
		}
	}
}
