using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000D0A RID: 3338
public class ClusterCometDetector : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>
{
	// Token: 0x0600400D RID: 16397 RVA: 0x002479BC File Offset: 0x00245BBC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(delegate(ClusterCometDetector.Instance smi)
		{
			smi.UpdateDetectionState(this.lastIsTargetDetected.Get(smi), true);
			smi.remainingSecondsToFreezeLogicSignal = 3f;
		}).Update(delegate(ClusterCometDetector.Instance smi, float deltaSeconds)
		{
			smi.remainingSecondsToFreezeLogicSignal -= deltaSeconds;
			if (smi.remainingSecondsToFreezeLogicSignal < 0f)
			{
				smi.remainingSecondsToFreezeLogicSignal = 0f;
				return;
			}
			smi.SetLogicSignal(this.lastIsTargetDetected.Get(smi));
		}, UpdateRate.SIM_200ms, false);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (ClusterCometDetector.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.DetectorScanning, null).Enter("ToggleActive", delegate(ClusterCometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(true, false);
		}).Exit("ToggleActive", delegate(ClusterCometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(false, false);
		});
		this.on.pre.PlayAnim("on_pre").OnAnimQueueComplete(this.on.loop);
		this.on.loop.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.pst, (ClusterCometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).TagTransition(GameTags.Detecting, this.on.working, false).Enter("UpdateLogic", delegate(ClusterCometDetector.Instance smi)
		{
			smi.UpdateDetectionState(smi.HasTag(GameTags.Detecting), false);
		}).Update("Scan Sky", delegate(ClusterCometDetector.Instance smi, float dt)
		{
			smi.ScanSky(false);
		}, UpdateRate.SIM_200ms, false);
		this.on.pst.PlayAnim("on_pst").OnAnimQueueComplete(this.off);
		this.on.working.DefaultState(this.on.working.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.IncomingMeteors, null).Enter("UpdateLogic", delegate(ClusterCometDetector.Instance smi)
		{
			smi.SetLogicSignal(true);
		}).Exit("UpdateLogic", delegate(ClusterCometDetector.Instance smi)
		{
			smi.SetLogicSignal(false);
		}).Update("Scan Sky", delegate(ClusterCometDetector.Instance smi, float dt)
		{
			smi.ScanSky(true);
		}, UpdateRate.SIM_200ms, false);
		this.on.working.pre.PlayAnim("detect_pre").OnAnimQueueComplete(this.on.working.loop);
		this.on.working.loop.PlayAnim("detect_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.working.pst, (ClusterCometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.on.working.pst, (ClusterCometDetector.Instance smi) => !smi.GetComponent<Operational>().IsActive).TagTransition(GameTags.Detecting, this.on.working.pst, true);
		this.on.working.pst.PlayAnim("detect_pst").OnAnimQueueComplete(this.on.loop);
	}

	// Token: 0x04002C44 RID: 11332
	public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State off;

	// Token: 0x04002C45 RID: 11333
	public ClusterCometDetector.OnStates on;

	// Token: 0x04002C46 RID: 11334
	public StateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.BoolParameter lastIsTargetDetected;

	// Token: 0x02000D0B RID: 3339
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000D0C RID: 3340
	public class OnStates : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State
	{
		// Token: 0x04002C47 RID: 11335
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pre;

		// Token: 0x04002C48 RID: 11336
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State loop;

		// Token: 0x04002C49 RID: 11337
		public ClusterCometDetector.WorkingStates working;

		// Token: 0x04002C4A RID: 11338
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pst;
	}

	// Token: 0x02000D0D RID: 3341
	public class WorkingStates : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State
	{
		// Token: 0x04002C4B RID: 11339
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pre;

		// Token: 0x04002C4C RID: 11340
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State loop;

		// Token: 0x04002C4D RID: 11341
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pst;
	}

	// Token: 0x02000D0E RID: 3342
	public new class Instance : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.GameInstance
	{
		// Token: 0x06004014 RID: 16404 RVA: 0x000CE104 File Offset: 0x000CC304
		public Instance(IStateMachineTarget master, ClusterCometDetector.Def def) : base(master, def)
		{
			this.detectorNetworkDef = new DetectorNetwork.Def();
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x000CE124 File Offset: 0x000CC324
		public override void StartSM()
		{
			if (this.detectorNetwork == null)
			{
				this.detectorNetwork = (DetectorNetwork.Instance)this.detectorNetworkDef.CreateSMI(base.master);
			}
			this.detectorNetwork.StartSM();
			base.StartSM();
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x000CE15B File Offset: 0x000CC35B
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			this.detectorNetwork.StopSM(reason);
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x00247D74 File Offset: 0x00245F74
		public void UpdateDetectionState(bool currentDetection, bool expectedDetectionForState)
		{
			KPrefabID component = base.GetComponent<KPrefabID>();
			if (currentDetection)
			{
				component.AddTag(GameTags.Detecting, false);
			}
			else
			{
				component.RemoveTag(GameTags.Detecting);
			}
			if (currentDetection == expectedDetectionForState)
			{
				this.SetLogicSignal(currentDetection);
			}
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x00247DB0 File Offset: 0x00245FB0
		public void ScanSky(bool expectedDetectionForState)
		{
			Option<SpaceScannerTarget> option;
			switch (this.GetDetectorState())
			{
			case ClusterCometDetector.Instance.ClusterCometDetectorState.MeteorShower:
				option = SpaceScannerTarget.MeteorShower();
				break;
			case ClusterCometDetector.Instance.ClusterCometDetectorState.BallisticObject:
				option = SpaceScannerTarget.BallisticObject();
				break;
			case ClusterCometDetector.Instance.ClusterCometDetectorState.Rocket:
				if (this.targetCraft != null && this.targetCraft.Get() != null)
				{
					option = SpaceScannerTarget.RocketDlc1(this.targetCraft.Get());
				}
				else
				{
					option = Option.None;
				}
				break;
			default:
				throw new NotImplementedException();
			}
			bool flag = option.IsSome() && Game.Instance.spaceScannerNetworkManager.IsTargetDetectedOnWorld(this.GetMyWorldId(), option.Unwrap());
			base.smi.sm.lastIsTargetDetected.Set(flag, this, false);
			this.UpdateDetectionState(flag, expectedDetectionForState);
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x000CE170 File Offset: 0x000CC370
		public void SetLogicSignal(bool on)
		{
			base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, on ? 1 : 0);
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x000CE189 File Offset: 0x000CC389
		public void SetDetectorState(ClusterCometDetector.Instance.ClusterCometDetectorState newState)
		{
			this.detectorState = newState;
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x000CE192 File Offset: 0x000CC392
		public ClusterCometDetector.Instance.ClusterCometDetectorState GetDetectorState()
		{
			return this.detectorState;
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x000CE19A File Offset: 0x000CC39A
		public void SetClustercraftTarget(Clustercraft target)
		{
			if (target)
			{
				this.targetCraft = new Ref<Clustercraft>(target);
				return;
			}
			this.targetCraft = null;
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x000CE1B8 File Offset: 0x000CC3B8
		public Clustercraft GetClustercraftTarget()
		{
			if (this.targetCraft == null)
			{
				return null;
			}
			return this.targetCraft.Get();
		}

		// Token: 0x04002C4E RID: 11342
		public bool ShowWorkingStatus;

		// Token: 0x04002C4F RID: 11343
		[Serialize]
		private ClusterCometDetector.Instance.ClusterCometDetectorState detectorState;

		// Token: 0x04002C50 RID: 11344
		[Serialize]
		private Ref<Clustercraft> targetCraft;

		// Token: 0x04002C51 RID: 11345
		[NonSerialized]
		public float remainingSecondsToFreezeLogicSignal;

		// Token: 0x04002C52 RID: 11346
		private DetectorNetwork.Def detectorNetworkDef;

		// Token: 0x04002C53 RID: 11347
		private DetectorNetwork.Instance detectorNetwork;

		// Token: 0x04002C54 RID: 11348
		private List<GameplayEventInstance> meteorShowers = new List<GameplayEventInstance>();

		// Token: 0x02000D0F RID: 3343
		public enum ClusterCometDetectorState
		{
			// Token: 0x04002C56 RID: 11350
			MeteorShower,
			// Token: 0x04002C57 RID: 11351
			BallisticObject,
			// Token: 0x04002C58 RID: 11352
			Rocket
		}
	}
}
