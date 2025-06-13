﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class StressShockChore : Chore<StressShockChore.StatesInstance>
{
	private static bool CheckBlocked(int sourceCell, int destinationCell)
	{
		HashSet<int> hashSet = new HashSet<int>();
		Grid.CollectCellsInLine(sourceCell, destinationCell, hashSet);
		bool result = false;
		foreach (int i in hashSet)
		{
			if (Grid.Solid[i])
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public static void AddBatteryDrainModifier(StressShockChore.StatesInstance smi)
	{
		smi.SetDrainModifierActiveState(true);
	}

	public static void RemoveBatteryDrainModifier(StressShockChore.StatesInstance smi)
	{
		smi.SetDrainModifierActiveState(false);
	}

	public static void ForceStressMonitorToTimeOut(StressShockChore.StatesInstance smi)
	{
		StressBehaviourMonitor.Instance smi2 = smi.GetSMI<StressBehaviourMonitor.Instance>();
		if (smi2 != null)
		{
			smi2.ManualSetStressTier2TimeCounter(150f);
		}
	}

	public StressShockChore(ChoreType chore_type, IStateMachineTarget target, Notification notification, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.StressShock, target, target.GetComponent<ChoreProvider>(), false, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new StressShockChore.StatesInstance(this, target.gameObject, notification);
	}

	public const float FaceBeamZOffset = 0.01f;

	public class StatesInstance : GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.GameInstance
	{
		public StatesInstance(StressShockChore master, GameObject shocker, Notification notification) : base(master)
		{
			base.sm.shocker.Set(shocker, base.smi, false);
			this.notification = notification;
		}

		public void SetDrainModifierActiveState(bool draining)
		{
			if (draining)
			{
				this.batteryMonitor.AddOrUpdateModifier(this.powerDrainModifier, true);
				return;
			}
			this.batteryMonitor.RemoveModifier(this.powerDrainModifier.id, true);
		}

		public void FindDestination()
		{
			int num = this.FindIdleCell();
			if (num != -1 && num != Grid.PosToCell(base.gameObject))
			{
				base.sm.targetMoveLocation.Set(num, base.smi, false);
				this.GoTo(base.sm.shocking.runAroundShockingStuff);
				return;
			}
			num = this.FindMinionTarget();
			if (num != -1 && num != Grid.PosToCell(base.gameObject))
			{
				base.sm.targetMoveLocation.Set(num, base.smi, false);
				this.GoTo(base.sm.shocking.runAroundShockingStuff);
				return;
			}
			base.sm.targetMoveLocation.Set(Grid.PosToCell(base.gameObject), base.smi, false);
			this.GoTo(base.sm.shocking.standStillShockingStuff);
		}

		private int FindMinionTarget()
		{
			Navigator component = base.smi.gameObject.GetComponent<Navigator>();
			if (component == null)
			{
				return Grid.InvalidCell;
			}
			int num = int.MaxValue;
			int result = Grid.InvalidCell;
			List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.smi.gameObject.GetMyWorldId(), false);
			for (int i = 0; i < worldItems.Count; i++)
			{
				if (!worldItems[i].IsNullOrDestroyed() && !(worldItems[i].gameObject == base.gameObject))
				{
					int num2 = Grid.PosToCell(worldItems[i]);
					if (component.CanReach(num2))
					{
						int navigationCost = component.GetNavigationCost(num2);
						if (navigationCost < num)
						{
							num = navigationCost;
							result = num2;
						}
					}
				}
			}
			return result;
		}

		private int FindIdleCell()
		{
			Navigator component = base.smi.master.GetComponent<Navigator>();
			MinionPathFinderAbilities minionPathFinderAbilities = (MinionPathFinderAbilities)component.GetCurrentAbilities();
			minionPathFinderAbilities.SetIdleNavMaskEnabled(true);
			IdleCellQuery idleCellQuery = PathFinderQueries.idleCellQuery.Reset(base.GetComponent<MinionBrain>(), UnityEngine.Random.Range(90, 180));
			component.RunQuery(idleCellQuery);
			if (idleCellQuery.GetResultCell() == Grid.PosToCell(base.gameObject))
			{
				idleCellQuery = PathFinderQueries.idleCellQuery.Reset(base.GetComponent<MinionBrain>(), UnityEngine.Random.Range(0, 90));
				component.RunQuery(idleCellQuery);
			}
			minionPathFinderAbilities.SetIdleNavMaskEnabled(false);
			return idleCellQuery.GetResultCell();
		}

		public void ShockUpdateRender(StressShockChore.StatesInstance smi, float dt)
		{
			if (smi.sm.faceLightningFX.Get(smi) != null)
			{
				smi.sm.faceLightningFX.Get(smi).transform.SetPosition(smi.FaceOriginLocation());
			}
			if (smi.sm.beamTarget.Get(smi) != null)
			{
				Vector3 vector = smi.sm.beamTarget.Get(smi).transform.position + Vector3.up / 2f;
				if (smi.sm.beamFX.Get(smi) == null)
				{
					smi.MakeBeam();
				}
				if (!StressShockChore.CheckBlocked(Grid.PosToCell(smi.sm.beamFX.Get(smi).transform.position), Grid.PosToCell(vector)))
				{
					smi.AimBeam(vector, 0);
				}
			}
		}

		public void ShockUpdate200(StressShockChore.StatesInstance smi, float dt)
		{
			float num = dt * STRESS.SHOCKER.POWER_CONSUMPTION_RATE;
			smi.sm.powerConsumed.Delta(num, smi);
			smi.batteryMonitor.ConsumePower(num);
			if (smi.sm.beamTarget.Get(smi) != null)
			{
				Health component = smi.sm.beamTarget.Get(smi).GetComponent<Health>();
				if (component != null)
				{
					component.Damage(dt * STRESS.SHOCKER.DAMAGE_RATE);
					return;
				}
				Electrobank component2 = smi.sm.beamTarget.Get(smi).GetComponent<Electrobank>();
				if (component2 != null)
				{
					component2.Damage(dt * STRESS.SHOCKER.DAMAGE_RATE);
					return;
				}
				if (smi.sm.beamTarget.Get(smi).HasTag(GameTags.Wires))
				{
					BuildingHP component3 = smi.sm.beamTarget.Get(smi).GetComponent<BuildingHP>();
					if (component3 != null)
					{
						component3.DoDamage(Mathf.RoundToInt(dt * STRESS.SHOCKER.DAMAGE_RATE));
					}
				}
			}
		}

		public void PickShockTarget(StressShockChore.StatesInstance smi)
		{
			int num = Grid.PosToCell(smi.master.gameObject);
			int worldId = (int)Grid.WorldIdx[num];
			List<GameObject> list = new List<GameObject>();
			float num2 = UnityEngine.Random.Range(0f, 2f);
			foreach (Health health in Components.Health.GetWorldItems(worldId, false))
			{
				if (!health.IsNullOrDestroyed() && !(health.gameObject == smi.master.gameObject))
				{
					int num3 = Grid.PosToCell(health);
					float num4 = Vector2.Distance(Grid.CellToPos2D(num), Grid.CellToPos2D(num3));
					if (num4 <= (float)STRESS.SHOCKER.SHOCK_RADIUS && num4 > num2 && !StressShockChore.CheckBlocked(num, num3))
					{
						list.Add(health.gameObject);
					}
				}
			}
			if (list.Count == 0)
			{
				Vector2I vector2I = Grid.CellToXY(num);
				List<ScenePartitionerEntry> list2 = new List<ScenePartitionerEntry>();
				GameScenePartitioner.Instance.GatherEntries(vector2I.x - STRESS.SHOCKER.SHOCK_RADIUS, vector2I.y - STRESS.SHOCKER.SHOCK_RADIUS, STRESS.SHOCKER.SHOCK_RADIUS * 2, STRESS.SHOCKER.SHOCK_RADIUS * 2, GameScenePartitioner.Instance.completeBuildings, list2);
				foreach (ScenePartitionerEntry scenePartitionerEntry in list2)
				{
					if (!StressShockChore.CheckBlocked(num, Grid.PosToCell(new Vector2((float)scenePartitionerEntry.x, (float)scenePartitionerEntry.y))))
					{
						BuildingComplete buildingComplete = scenePartitionerEntry.obj as BuildingComplete;
						if (buildingComplete != null)
						{
							list.Add(buildingComplete.gameObject);
						}
					}
				}
			}
			if (list.Count == 0)
			{
				this.ClearBeam(false);
				return;
			}
			GameObject random = list.GetRandom<GameObject>();
			GameObject gameObject = random;
			float num5 = float.MaxValue;
			foreach (GameObject gameObject2 in list)
			{
				if (list.Count <= 1 || !(gameObject2 == base.sm.previousTarget.Get(smi)))
				{
					float num6 = Vector2.Distance(base.transform.position, gameObject2.transform.position);
					if (num6 < num5)
					{
						num5 = num6;
						gameObject = gameObject2;
					}
				}
			}
			if (random != null && gameObject != null && UnityEngine.Random.Range(0, 100) > 50)
			{
				base.sm.beamTarget.Set(gameObject, smi, false);
				return;
			}
			base.sm.beamTarget.Set(gameObject, smi, false);
		}

		public void MakeBeam()
		{
			GameObject gameObject = new GameObject("shockFX");
			gameObject.SetActive(false);
			KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
			base.sm.beamFX.Set(kbatchedAnimController, base.smi, false);
			kbatchedAnimController.SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("bionic_dupe_stress_beam_fx_kanim")
			});
			gameObject.SetActive(true);
			bool flag;
			Vector3 vector = base.GetComponent<KBatchedAnimController>().GetSymbolTransform("snapTo_hat", out flag).GetColumn(3);
			vector -= Vector3.up / 4f;
			vector.z = base.transform.position.z + 0.01f;
			gameObject.transform.position = vector;
			kbatchedAnimController.Play("beam1", KAnim.PlayMode.Loop, 1f, 0f);
			if (base.sm.faceLightningFX.Get(base.smi) != null)
			{
				Util.KDestroyGameObject(base.sm.faceLightningFX.Get(base.smi).gameObject);
				base.sm.faceLightningFX.Set(null, base.smi, false);
			}
			GameObject gameObject2 = new GameObject("faceLightningFX");
			gameObject2.SetActive(false);
			KBatchedAnimController kbatchedAnimController2 = gameObject2.AddComponent<KBatchedAnimController>();
			base.sm.faceLightningFX.Set(kbatchedAnimController2, base.smi, false);
			kbatchedAnimController2.SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("bionic_dupe_stress_lightning_fx_kanim")
			});
			gameObject2.SetActive(true);
			gameObject2.transform.position = this.FaceOriginLocation();
			kbatchedAnimController2.Play("lightning", KAnim.PlayMode.Loop, 1f, 0f);
			GameObject gameObject3 = new GameObject("impactFX");
			gameObject3.SetActive(false);
			KBatchedAnimController kbatchedAnimController3 = gameObject3.AddComponent<KBatchedAnimController>();
			base.sm.impactFX.Set(kbatchedAnimController3, base.smi, false);
			kbatchedAnimController3.SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("bionic_dupe_stress_beam_impact_fx_kanim")
			});
			gameObject3.SetActive(true);
			kbatchedAnimController3.Play("stress_beam_impact_fx", KAnim.PlayMode.Loop, 1f, 0f);
		}

		public Vector3 FaceOriginLocation()
		{
			bool flag;
			Vector3 vector = base.GetComponent<KBatchedAnimController>().GetSymbolTransform("snapTo_hat", out flag).GetColumn(3);
			vector -= Vector3.up / 4f;
			vector.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
			return vector;
		}

		public void ClearBeam(bool clearFaceFX = true)
		{
			base.sm.previousTarget.Set(base.sm.beamTarget.Get(base.smi), base.smi, false);
			base.sm.beamTarget.Set(null, base.smi, false);
			if (base.sm.beamFX.Get(base.smi) != null)
			{
				Util.KDestroyGameObject(base.sm.beamFX.Get(base.smi).gameObject);
				base.sm.beamFX.Set(null, base.smi, false);
			}
			if (base.sm.impactFX.Get(base.smi) != null)
			{
				Util.KDestroyGameObject(base.sm.impactFX.Get(base.smi).gameObject);
				base.sm.impactFX.Set(null, base.smi, false);
			}
			if (clearFaceFX && base.sm.faceLightningFX.Get(base.smi) != null)
			{
				Util.KDestroyGameObject(base.sm.faceLightningFX.Get(base.smi).gameObject);
				base.sm.faceLightningFX.Set(null, base.smi, false);
			}
		}

		public void AimBeam(Vector3 targetPosition, int beamIdx)
		{
			Vector3 position = this.FaceOriginLocation();
			position.z = base.transform.position.z + 0.01f;
			base.smi.sm.beamFX.Get(base.smi).transform.SetPosition(position);
			Vector3 v = Vector3.Normalize(targetPosition - base.smi.sm.beamFX.Get(base.smi).transform.position);
			float rotation = MathUtil.AngleSigned(Vector3.up, v, Vector3.forward) + 90f;
			base.smi.sm.beamFX.Get(base.smi).Rotation = rotation;
			base.smi.sm.impactFX.Get(base.smi).transform.position = targetPosition;
			base.smi.sm.faceLightningFX.Get(base.smi).FlipX = (targetPosition.x < base.smi.sm.faceLightningFX.Get(base.smi).transform.position.x);
			Vector3 position2 = base.smi.sm.beamFX.Get(base.smi).transform.position;
			position2.z = 0f;
			Vector3 b = targetPosition;
			b.z = 0f;
			float num = Vector3.Distance(position2, b);
			if (num > 3f)
			{
				if (base.smi.sm.beamFX.Get(base.smi).CurrentAnim == null || base.smi.sm.beamFX.Get(base.smi).CurrentAnim.name != "beam3")
				{
					base.smi.sm.beamFX.Get(base.smi).Play("beam3", KAnim.PlayMode.Loop, 1f, 0f);
				}
				base.smi.sm.beamFX.Get(base.smi).animWidth = num / 3f;
				return;
			}
			if (num > 2f)
			{
				if (base.smi.sm.beamFX.Get(base.smi).CurrentAnim == null || base.smi.sm.beamFX.Get(base.smi).CurrentAnim.name != "beam2")
				{
					base.smi.sm.beamFX.Get(base.smi).Play("beam2", KAnim.PlayMode.Loop, 1f, 0f);
				}
				base.smi.sm.beamFX.Get(base.smi).animWidth = num / 2f;
				return;
			}
			if (base.smi.sm.beamFX.Get(base.smi).CurrentAnim == null || base.smi.sm.beamFX.Get(base.smi).CurrentAnim.name != "beam1")
			{
				base.smi.sm.beamFX.Get(base.smi).Play("beam1", KAnim.PlayMode.Loop, 1f, 0f);
			}
			base.smi.sm.beamFX.Get(base.smi).animWidth = num;
		}

		public void ShowBeam(bool show)
		{
			if (base.smi.sm.impactFX.Get(base.smi) != null)
			{
				base.smi.sm.impactFX.Get(base.smi).enabled = show;
			}
			if (base.smi.sm.beamFX.Get(base.smi) != null)
			{
				base.smi.sm.beamFX.Get(base.smi).enabled = show;
			}
		}

		public Notification notification;

		[MySmiReq]
		public BionicBatteryMonitor.Instance batteryMonitor;

		public BionicBatteryMonitor.WattageModifier powerDrainModifier = new BionicBatteryMonitor.WattageModifier("StressShockChore", string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_ACTIVE_TEMPLATE, DUPLICANTS.TRAITS.STRESSSHOCKER.DRAIN_ATTRIBUTE, "<b>+</b>" + GameUtil.GetFormattedWattage(STRESS.SHOCKER.POWER_CONSUMPTION_RATE, GameUtil.WattageFormatterUnit.Automatic, true)), STRESS.SHOCKER.POWER_CONSUMPTION_RATE, STRESS.SHOCKER.POWER_CONSUMPTION_RATE);
	}

	public class States : GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore>
	{
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.shocking.findDestination;
			base.serializable = StateMachine.SerializeType.Never;
			base.Target(this.shocker);
			this.shocking.EventTransition(GameHashes.BionicOffline, this.offline, null).DefaultState(this.shocking.findDestination).ToggleAnims("anim_loco_stressshocker_kanim", 0f).ParamTransition<float>(this.powerConsumed, this.complete, (StressShockChore.StatesInstance smi, float p) => p >= STRESS.SHOCKER.MAX_POWER_USE).Enter(delegate(StressShockChore.StatesInstance smi)
			{
				smi.MakeBeam();
			}).Exit(delegate(StressShockChore.StatesInstance smi)
			{
				smi.ClearBeam(true);
			});
			this.shocking.findDestination.Enter("FindDestination", delegate(StressShockChore.StatesInstance smi)
			{
				smi.ShowBeam(false);
				smi.FindDestination();
			}).Update(delegate(StressShockChore.StatesInstance smi, float dt)
			{
				float delta_value = dt * STRESS.SHOCKER.FAKE_POWER_CONSUMPTION_RATE;
				smi.sm.powerConsumed.Delta(delta_value, smi);
				smi.FindDestination();
			}, UpdateRate.SIM_1000ms, false);
			this.shocking.runAroundShockingStuff.MoveTo((StressShockChore.StatesInstance smi) => smi.sm.targetMoveLocation.Get(smi), this.shocking.findDestination, this.delay, false).Toggle("BatteryDrain", new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.AddBatteryDrainModifier), new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.RemoveBatteryDrainModifier)).Enter(delegate(StressShockChore.StatesInstance smi)
			{
				smi.ShowBeam(true);
			}).Update(delegate(StressShockChore.StatesInstance smi, float dt)
			{
				smi.PickShockTarget(smi);
				smi.ShockUpdate200(smi, dt);
			}, UpdateRate.SIM_200ms, false).Update(delegate(StressShockChore.StatesInstance smi, float dt)
			{
				smi.ShockUpdateRender(smi, dt);
			}, UpdateRate.RENDER_EVERY_TICK, false);
			this.shocking.standStillShockingStuff.Toggle("BatteryDrain", new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.AddBatteryDrainModifier), new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.RemoveBatteryDrainModifier)).Enter(delegate(StressShockChore.StatesInstance smi)
			{
				smi.ShowBeam(true);
			}).PlayAnim("interrupt_shocker", KAnim.PlayMode.Loop).ScheduleGoTo(2f, this.delay).Update(delegate(StressShockChore.StatesInstance smi, float dt)
			{
				smi.PickShockTarget(smi);
				smi.ShockUpdate200(smi, dt);
			}, UpdateRate.SIM_200ms, false).Update(delegate(StressShockChore.StatesInstance smi, float dt)
			{
				smi.ShockUpdateRender(smi, dt);
			}, UpdateRate.RENDER_EVERY_TICK, false);
			this.delay.ScheduleGoTo(0.5f, this.shocking);
			this.complete.Enter(delegate(StressShockChore.StatesInstance smi)
			{
				smi.StopSM("complete");
			});
			this.offline.Enter(new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.ForceStressMonitorToTimeOut)).ReturnSuccess();
		}

		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.TargetParameter shocker;

		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController[]> cosmeticBeamFXs;

		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController> beamFX;

		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController> impactFX;

		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController> faceLightningFX;

		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<GameObject> beamTarget;

		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<GameObject> previousTarget;

		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.IntParameter targetMoveLocation;

		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.FloatParameter powerConsumed;

		public StressShockChore.States.ShockStates shocking;

		public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State delay;

		public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State complete;

		public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State offline;

		public class ShockStates : GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State
		{
			public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State findDestination;

			public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State runAroundShockingStuff;

			public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State standStillShockingStuff;
		}
	}
}
