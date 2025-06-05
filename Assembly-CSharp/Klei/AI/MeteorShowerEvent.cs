using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CB5 RID: 15541
	public class MeteorShowerEvent : GameplayEvent<MeteorShowerEvent.StatesInstance>
	{
		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x0600EE84 RID: 61060 RVA: 0x0014479C File Offset: 0x0014299C
		public bool canStarTravel
		{
			get
			{
				return this.clusterMapMeteorShowerID != null && DlcManager.FeatureClusterSpaceEnabled();
			}
		}

		// Token: 0x0600EE85 RID: 61061 RVA: 0x001447AD File Offset: 0x001429AD
		public string GetClusterMapMeteorShowerID()
		{
			return this.clusterMapMeteorShowerID;
		}

		// Token: 0x0600EE86 RID: 61062 RVA: 0x001447B5 File Offset: 0x001429B5
		public List<MeteorShowerEvent.BombardmentInfo> GetMeteorsInfo()
		{
			return new List<MeteorShowerEvent.BombardmentInfo>(this.bombardmentInfo);
		}

		// Token: 0x0600EE87 RID: 61063 RVA: 0x004E6FBC File Offset: 0x004E51BC
		public MeteorShowerEvent(string id, float duration, float secondsPerMeteor, MathUtil.MinMax secondsBombardmentOff = default(MathUtil.MinMax), MathUtil.MinMax secondsBombardmentOn = default(MathUtil.MinMax), string clusterMapMeteorShowerID = null, bool affectedByDifficulty = true) : base(id, 0, 0)
		{
			this.allowMultipleEventInstances = true;
			this.clusterMapMeteorShowerID = clusterMapMeteorShowerID;
			this.duration = duration;
			this.secondsPerMeteor = secondsPerMeteor;
			this.secondsBombardmentOff = secondsBombardmentOff;
			this.secondsBombardmentOn = secondsBombardmentOn;
			this.affectedByDifficulty = affectedByDifficulty;
			this.bombardmentInfo = new List<MeteorShowerEvent.BombardmentInfo>();
			this.tags.Add(GameTags.SpaceDanger);
		}

		// Token: 0x0600EE88 RID: 61064 RVA: 0x004E7034 File Offset: 0x004E5234
		public MeteorShowerEvent AddMeteor(string prefab, float weight)
		{
			this.bombardmentInfo.Add(new MeteorShowerEvent.BombardmentInfo
			{
				prefab = prefab,
				weight = weight
			});
			return this;
		}

		// Token: 0x0600EE89 RID: 61065 RVA: 0x001447C2 File Offset: 0x001429C2
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new MeteorShowerEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0600EE8A RID: 61066 RVA: 0x001447CC File Offset: 0x001429CC
		public override bool IsAllowed()
		{
			return base.IsAllowed() && (!this.affectedByDifficulty || CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers).id != "ClearSkies");
		}

		// Token: 0x0400EA62 RID: 60002
		private List<MeteorShowerEvent.BombardmentInfo> bombardmentInfo;

		// Token: 0x0400EA63 RID: 60003
		private MathUtil.MinMax secondsBombardmentOff;

		// Token: 0x0400EA64 RID: 60004
		private MathUtil.MinMax secondsBombardmentOn;

		// Token: 0x0400EA65 RID: 60005
		private float secondsPerMeteor = 0.33f;

		// Token: 0x0400EA66 RID: 60006
		private float duration;

		// Token: 0x0400EA67 RID: 60007
		private string clusterMapMeteorShowerID;

		// Token: 0x0400EA68 RID: 60008
		private bool affectedByDifficulty = true;

		// Token: 0x02003CB6 RID: 15542
		public struct BombardmentInfo
		{
			// Token: 0x0400EA69 RID: 60009
			public string prefab;

			// Token: 0x0400EA6A RID: 60010
			public float weight;
		}

		// Token: 0x02003CB7 RID: 15543
		public class States : GameplayEventStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, MeteorShowerEvent>
		{
			// Token: 0x0600EE8B RID: 61067 RVA: 0x004E7068 File Offset: 0x004E5268
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				base.InitializeStates(out default_state);
				default_state = this.planning;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.planning.Enter(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					this.runTimeRemaining.Set(smi.gameplayEvent.duration, smi, false);
					this.bombardTimeRemaining.Set(smi.GetBombardOnTime(), smi, false);
					this.snoozeTimeRemaining.Set(smi.GetBombardOffTime(), smi, false);
					if (smi.gameplayEvent.canStarTravel && smi.clusterTravelDuration > 0f)
					{
						smi.GoTo(smi.sm.starMap);
						return;
					}
					smi.GoTo(smi.sm.running);
				});
				this.starMap.Enter(new StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State.Callback(MeteorShowerEvent.States.CreateClusterMapMeteorShower)).DefaultState(this.starMap.travelling);
				this.starMap.travelling.OnSignal(this.OnClusterMapDestinationReached, this.starMap.arrive);
				this.starMap.arrive.GoTo(this.running.bombarding);
				this.running.DefaultState(this.running.snoozing).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					this.runTimeRemaining.Delta(-dt, smi);
				}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.runTimeRemaining, this.finished, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero);
				this.running.bombarding.Enter(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					MeteorShowerEvent.States.TriggerMeteorGlobalEvent(smi, GameHashes.MeteorShowerBombardStateBegins);
				}).Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					MeteorShowerEvent.States.TriggerMeteorGlobalEvent(smi, GameHashes.MeteorShowerBombardStateEnds);
				}).Enter(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					smi.StartBackgroundEffects();
				}).Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					smi.StopBackgroundEffects();
				}).Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					this.bombardTimeRemaining.Set(smi.GetBombardOnTime(), smi, false);
				}).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					this.bombardTimeRemaining.Delta(-dt, smi);
				}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.bombardTimeRemaining, this.running.snoozing, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					smi.Bombarding(dt);
				}, UpdateRate.SIM_200ms, false);
				this.running.snoozing.Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					this.snoozeTimeRemaining.Set(smi.GetBombardOffTime(), smi, false);
				}).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					this.snoozeTimeRemaining.Delta(-dt, smi);
				}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.snoozeTimeRemaining, this.running.bombarding, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero);
				this.finished.ReturnSuccess();
			}

			// Token: 0x0600EE8C RID: 61068 RVA: 0x00144800 File Offset: 0x00142A00
			public static void TriggerMeteorGlobalEvent(MeteorShowerEvent.StatesInstance smi, GameHashes hash)
			{
				Game.Instance.Trigger((int)hash, smi.eventInstance.worldId);
			}

			// Token: 0x0600EE8D RID: 61069 RVA: 0x004E72A4 File Offset: 0x004E54A4
			public static void CreateClusterMapMeteorShower(MeteorShowerEvent.StatesInstance smi)
			{
				if (smi.sm.clusterMapMeteorShower.Get(smi) == null)
				{
					GameObject prefab = Assets.GetPrefab(smi.gameplayEvent.clusterMapMeteorShowerID.ToTag());
					float arrivalTime = smi.eventInstance.eventStartTime * 600f + smi.clusterTravelDuration;
					AxialI randomCellAtEdgeOfUniverse = ClusterGrid.Instance.GetRandomCellAtEdgeOfUniverse();
					GameObject gameObject = Util.KInstantiate(prefab, null, null);
					gameObject.GetComponent<ClusterMapMeteorShowerVisualizer>().SetInitialLocation(randomCellAtEdgeOfUniverse);
					ClusterMapMeteorShower.Def def = gameObject.AddOrGetDef<ClusterMapMeteorShower.Def>();
					def.destinationWorldID = smi.eventInstance.worldId;
					def.arrivalTime = arrivalTime;
					gameObject.SetActive(true);
					smi.sm.clusterMapMeteorShower.Set(gameObject, smi, false);
				}
				GameObject go = smi.sm.clusterMapMeteorShower.Get(smi);
				go.GetDef<ClusterMapMeteorShower.Def>();
				go.Subscribe(1796608350, new Action<object>(smi.OnClusterMapDestinationReached));
			}

			// Token: 0x0400EA6B RID: 60011
			public MeteorShowerEvent.States.ClusterMapStates starMap;

			// Token: 0x0400EA6C RID: 60012
			public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State planning;

			// Token: 0x0400EA6D RID: 60013
			public MeteorShowerEvent.States.RunningStates running;

			// Token: 0x0400EA6E RID: 60014
			public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State finished;

			// Token: 0x0400EA6F RID: 60015
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.TargetParameter clusterMapMeteorShower;

			// Token: 0x0400EA70 RID: 60016
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter runTimeRemaining;

			// Token: 0x0400EA71 RID: 60017
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter bombardTimeRemaining;

			// Token: 0x0400EA72 RID: 60018
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter snoozeTimeRemaining;

			// Token: 0x0400EA73 RID: 60019
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.Signal OnClusterMapDestinationReached;

			// Token: 0x02003CB8 RID: 15544
			public class ClusterMapStates : GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400EA74 RID: 60020
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State travelling;

				// Token: 0x0400EA75 RID: 60021
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State arrive;
			}

			// Token: 0x02003CB9 RID: 15545
			public class RunningStates : GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400EA76 RID: 60022
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State bombarding;

				// Token: 0x0400EA77 RID: 60023
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State snoozing;
			}
		}

		// Token: 0x02003CBB RID: 15547
		public class StatesInstance : GameplayEventStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, MeteorShowerEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600EE9E RID: 61086 RVA: 0x001448CB File Offset: 0x00142ACB
			public float GetSleepTimerValue()
			{
				return Mathf.Clamp(GameplayEventManager.Instance.GetSleepTimer(this.gameplayEvent) - GameUtil.GetCurrentTimeInCycles(), 0f, float.MaxValue);
			}

			// Token: 0x0600EE9F RID: 61087 RVA: 0x004E7410 File Offset: 0x004E5610
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, MeteorShowerEvent meteorShowerEvent) : base(master, eventInstance, meteorShowerEvent)
			{
				this.world = ClusterManager.Instance.GetWorld(this.m_worldId);
				this.difficultyLevel = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
				this.m_worldId = eventInstance.worldId;
				Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
			}

			// Token: 0x0600EEA0 RID: 61088 RVA: 0x001448F2 File Offset: 0x00142AF2
			public void OnClusterMapDestinationReached(object obj)
			{
				base.smi.sm.OnClusterMapDestinationReached.Trigger(this);
			}

			// Token: 0x0600EEA1 RID: 61089 RVA: 0x004E7484 File Offset: 0x004E5684
			private void OnActiveWorldChanged(object data)
			{
				int first = ((global::Tuple<int, int>)data).first;
				if (this.activeMeteorBackground != null)
				{
					this.activeMeteorBackground.GetComponent<ParticleSystemRenderer>().enabled = (first == this.m_worldId);
				}
			}

			// Token: 0x0600EEA2 RID: 61090 RVA: 0x0014490A File Offset: 0x00142B0A
			public override void StopSM(string reason)
			{
				this.StopBackgroundEffects();
				base.StopSM(reason);
			}

			// Token: 0x0600EEA3 RID: 61091 RVA: 0x00144919 File Offset: 0x00142B19
			protected override void OnCleanUp()
			{
				Game.Instance.Unsubscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
				this.DestroyClusterMapMeteorShowerObject();
				base.OnCleanUp();
			}

			// Token: 0x0600EEA4 RID: 61092 RVA: 0x004E74C4 File Offset: 0x004E56C4
			private void DestroyClusterMapMeteorShowerObject()
			{
				if (base.sm.clusterMapMeteorShower.Get(this) != null)
				{
					ClusterMapMeteorShower.Instance smi = base.sm.clusterMapMeteorShower.Get(this).GetSMI<ClusterMapMeteorShower.Instance>();
					if (smi != null)
					{
						smi.StopSM("Event is being aborted");
						Util.KDestroyGameObject(smi.gameObject);
					}
				}
			}

			// Token: 0x0600EEA5 RID: 61093 RVA: 0x004E751C File Offset: 0x004E571C
			public void StartBackgroundEffects()
			{
				if (this.activeMeteorBackground == null)
				{
					this.activeMeteorBackground = Util.KInstantiate(EffectPrefabs.Instance.MeteorBackground, null, null);
					float x = (this.world.maximumBounds.x + this.world.minimumBounds.x) / 2f;
					float y = this.world.maximumBounds.y;
					float z = 25f;
					this.activeMeteorBackground.transform.SetPosition(new Vector3(x, y, z));
					this.activeMeteorBackground.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
				}
			}

			// Token: 0x0600EEA6 RID: 61094 RVA: 0x004E75D0 File Offset: 0x004E57D0
			public void StopBackgroundEffects()
			{
				if (this.activeMeteorBackground != null)
				{
					ParticleSystem component = this.activeMeteorBackground.GetComponent<ParticleSystem>();
					component.main.stopAction = ParticleSystemStopAction.Destroy;
					component.Stop();
					if (!component.IsAlive())
					{
						UnityEngine.Object.Destroy(this.activeMeteorBackground);
					}
					this.activeMeteorBackground = null;
				}
			}

			// Token: 0x0600EEA7 RID: 61095 RVA: 0x004E7624 File Offset: 0x004E5824
			public float TimeUntilNextShower()
			{
				if (base.IsInsideState(base.sm.running.bombarding))
				{
					return 0f;
				}
				if (!base.IsInsideState(base.sm.starMap))
				{
					return base.sm.snoozeTimeRemaining.Get(this);
				}
				float num = base.smi.eventInstance.eventStartTime * 600f + base.smi.clusterTravelDuration - GameUtil.GetCurrentTimeInCycles() * 600f;
				if (num >= 0f)
				{
					return num;
				}
				return 0f;
			}

			// Token: 0x0600EEA8 RID: 61096 RVA: 0x004E76B4 File Offset: 0x004E58B4
			public void Bombarding(float dt)
			{
				this.nextMeteorTime -= dt;
				while (this.nextMeteorTime < 0f)
				{
					if (this.GetSleepTimerValue() <= 0f)
					{
						this.DoBombardment(this.gameplayEvent.bombardmentInfo);
					}
					this.nextMeteorTime += this.GetNextMeteorTime();
				}
			}

			// Token: 0x0600EEA9 RID: 61097 RVA: 0x004E7714 File Offset: 0x004E5914
			private void DoBombardment(List<MeteorShowerEvent.BombardmentInfo> bombardment_info)
			{
				float num = 0f;
				foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo in bombardment_info)
				{
					num += bombardmentInfo.weight;
				}
				num = UnityEngine.Random.Range(0f, num);
				MeteorShowerEvent.BombardmentInfo bombardmentInfo2 = bombardment_info[0];
				int num2 = 0;
				while (num - bombardmentInfo2.weight > 0f)
				{
					num -= bombardmentInfo2.weight;
					bombardmentInfo2 = bombardment_info[++num2];
				}
				Game.Instance.Trigger(-84771526, null);
				this.SpawnBombard(bombardmentInfo2.prefab);
			}

			// Token: 0x0600EEAA RID: 61098 RVA: 0x004E77C8 File Offset: 0x004E59C8
			private GameObject SpawnBombard(string prefab)
			{
				WorldContainer worldContainer = ClusterManager.Instance.GetWorld(this.m_worldId);
				float x = (float)(worldContainer.Width - 1) * UnityEngine.Random.value + (float)worldContainer.WorldOffset.x;
				float y = (float)(worldContainer.Height + worldContainer.WorldOffset.y - 1);
				float layerZ = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
				Vector3 position = new Vector3(x, y, layerZ);
				GameObject prefab2 = Assets.GetPrefab(prefab);
				if (prefab2 == null)
				{
					return null;
				}
				GameObject gameObject = Util.KInstantiate(prefab2, position, Quaternion.identity, null, null, true, 0);
				Comet component = gameObject.GetComponent<Comet>();
				if (component != null)
				{
					component.spawnWithOffset = true;
				}
				gameObject.SetActive(true);
				return gameObject;
			}

			// Token: 0x0600EEAB RID: 61099 RVA: 0x00144942 File Offset: 0x00142B42
			public float BombardTimeRemaining()
			{
				return Mathf.Min(base.sm.bombardTimeRemaining.Get(this), base.sm.runTimeRemaining.Get(this));
			}

			// Token: 0x0600EEAC RID: 61100 RVA: 0x004E7878 File Offset: 0x004E5A78
			public float GetBombardOffTime()
			{
				float num = this.gameplayEvent.secondsBombardmentOff.Get();
				if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
				{
					string id = this.difficultyLevel.id;
					if (!(id == "Infrequent"))
					{
						if (!(id == "Intense"))
						{
							if (id == "Doomed")
							{
								num *= 0.5f;
							}
						}
						else
						{
							num *= 1f;
						}
					}
					else
					{
						num *= 1f;
					}
				}
				return num;
			}

			// Token: 0x0600EEAD RID: 61101 RVA: 0x004E7900 File Offset: 0x004E5B00
			public float GetBombardOnTime()
			{
				float num = this.gameplayEvent.secondsBombardmentOn.Get();
				if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
				{
					string id = this.difficultyLevel.id;
					if (!(id == "Infrequent"))
					{
						if (!(id == "Intense"))
						{
							if (id == "Doomed")
							{
								num *= 1f;
							}
						}
						else
						{
							num *= 1f;
						}
					}
					else
					{
						num *= 1f;
					}
				}
				return num;
			}

			// Token: 0x0600EEAE RID: 61102 RVA: 0x004E7988 File Offset: 0x004E5B88
			private float GetNextMeteorTime()
			{
				float num = this.gameplayEvent.secondsPerMeteor;
				num *= 256f / (float)this.world.Width;
				if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
				{
					string id = this.difficultyLevel.id;
					if (!(id == "Infrequent"))
					{
						if (!(id == "Intense"))
						{
							if (id == "Doomed")
							{
								num *= 0.5f;
							}
						}
						else
						{
							num *= 0.8f;
						}
					}
					else
					{
						num *= 1.5f;
					}
				}
				return num;
			}

			// Token: 0x0400EA7E RID: 60030
			public GameObject activeMeteorBackground;

			// Token: 0x0400EA7F RID: 60031
			[Serialize]
			public float clusterTravelDuration = -1f;

			// Token: 0x0400EA80 RID: 60032
			[Serialize]
			private float nextMeteorTime;

			// Token: 0x0400EA81 RID: 60033
			[Serialize]
			private int m_worldId;

			// Token: 0x0400EA82 RID: 60034
			private WorldContainer world;

			// Token: 0x0400EA83 RID: 60035
			private SettingLevel difficultyLevel;
		}
	}
}
