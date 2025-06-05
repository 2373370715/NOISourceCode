using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000D19 RID: 3353
public class CometDetector : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>
{
	// Token: 0x0600407F RID: 16511 RVA: 0x002490D8 File Offset: 0x002472D8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(delegate(CometDetector.Instance smi)
		{
			smi.UpdateDetectionState(this.lastIsTargetDetected.Get(smi), true);
			smi.remainingSecondsToFreezeLogicSignal = 3f;
		}).Update(delegate(CometDetector.Instance smi, float deltaSeconds)
		{
			smi.remainingSecondsToFreezeLogicSignal -= deltaSeconds;
			if (smi.remainingSecondsToFreezeLogicSignal < 0f)
			{
				smi.remainingSecondsToFreezeLogicSignal = 0f;
				return;
			}
			smi.SetLogicSignal(this.lastIsTargetDetected.Get(smi));
		}, UpdateRate.SIM_200ms, false);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (CometDetector.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.DetectorScanning, null).Enter("ToggleActive", delegate(CometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(true, false);
		}).Exit("ToggleActive", delegate(CometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(false, false);
		});
		this.on.pre.PlayAnim("on_pre").OnAnimQueueComplete(this.on.loop);
		this.on.loop.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.pst, (CometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).TagTransition(GameTags.Detecting, this.on.working, false).Enter("UpdateLogic", delegate(CometDetector.Instance smi)
		{
			smi.UpdateDetectionState(smi.HasTag(GameTags.Detecting), false);
		}).Update("Scan Sky", delegate(CometDetector.Instance smi, float dt)
		{
			smi.ScanSky(false);
		}, UpdateRate.SIM_200ms, false);
		this.on.pst.PlayAnim("on_pst").OnAnimQueueComplete(this.off);
		this.on.working.DefaultState(this.on.working.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.IncomingMeteors, null).Enter("UpdateLogic", delegate(CometDetector.Instance smi)
		{
			smi.SetLogicSignal(true);
		}).Exit("UpdateLogic", delegate(CometDetector.Instance smi)
		{
			smi.SetLogicSignal(false);
		}).Update("Scan Sky", delegate(CometDetector.Instance smi, float dt)
		{
			smi.ScanSky(true);
		}, UpdateRate.SIM_200ms, false);
		this.on.working.pre.PlayAnim("detect_pre").OnAnimQueueComplete(this.on.working.loop);
		this.on.working.loop.PlayAnim("detect_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.working.pst, (CometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.on.working.pst, (CometDetector.Instance smi) => !smi.GetComponent<Operational>().IsActive).TagTransition(GameTags.Detecting, this.on.working.pst, true);
		this.on.working.pst.PlayAnim("detect_pst").OnAnimQueueComplete(this.on.loop);
	}

	// Token: 0x04002CA3 RID: 11427
	public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State off;

	// Token: 0x04002CA4 RID: 11428
	public CometDetector.OnStates on;

	// Token: 0x04002CA5 RID: 11429
	public StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.BoolParameter lastIsTargetDetected;

	// Token: 0x02000D1A RID: 3354
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000D1B RID: 3355
	public class OnStates : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State
	{
		// Token: 0x04002CA6 RID: 11430
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pre;

		// Token: 0x04002CA7 RID: 11431
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State loop;

		// Token: 0x04002CA8 RID: 11432
		public CometDetector.WorkingStates working;

		// Token: 0x04002CA9 RID: 11433
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pst;
	}

	// Token: 0x02000D1C RID: 3356
	public class WorkingStates : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State
	{
		// Token: 0x04002CAA RID: 11434
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pre;

		// Token: 0x04002CAB RID: 11435
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State loop;

		// Token: 0x04002CAC RID: 11436
		public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pst;
	}

	// Token: 0x02000D1D RID: 3357
	public new class Instance : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.GameInstance
	{
		// Token: 0x06004086 RID: 16518 RVA: 0x000CE4CA File Offset: 0x000CC6CA
		public Instance(IStateMachineTarget master, CometDetector.Def def) : base(master, def)
		{
			this.detectorNetworkDef = new DetectorNetwork.Def();
			this.targetCraft = new Ref<LaunchConditionManager>();
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x000CE4F5 File Offset: 0x000CC6F5
		public override void StartSM()
		{
			if (this.detectorNetwork == null)
			{
				this.detectorNetwork = (DetectorNetwork.Instance)this.detectorNetworkDef.CreateSMI(base.master);
			}
			this.detectorNetwork.StartSM();
			base.StartSM();
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x000CE52C File Offset: 0x000CC72C
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			this.detectorNetwork.StopSM(reason);
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x00249490 File Offset: 0x00247690
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

		// Token: 0x0600408A RID: 16522 RVA: 0x002494CC File Offset: 0x002476CC
		public void ScanSky(bool expectedDetectionForState)
		{
			LaunchConditionManager launchConditionManager = this.targetCraft.Get();
			Option<SpaceScannerTarget> option;
			if (launchConditionManager == null)
			{
				option = SpaceScannerTarget.MeteorShower();
			}
			else if (SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.targetCraft.Get()).state == Spacecraft.MissionState.Destroyed)
			{
				option = Option.None;
			}
			else
			{
				option = SpaceScannerTarget.RocketBaseGame(launchConditionManager);
			}
			bool flag = option.IsSome() && Game.Instance.spaceScannerNetworkManager.IsTargetDetectedOnWorld(this.GetMyWorldId(), option.Unwrap());
			base.smi.sm.lastIsTargetDetected.Set(flag, this, false);
			this.UpdateDetectionState(flag, expectedDetectionForState);
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x000CE170 File Offset: 0x000CC370
		public void SetLogicSignal(bool on)
		{
			base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, on ? 1 : 0);
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x000CE541 File Offset: 0x000CC741
		public void SetTargetCraft(LaunchConditionManager target)
		{
			this.targetCraft.Set(target);
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x000CE54F File Offset: 0x000CC74F
		public LaunchConditionManager GetTargetCraft()
		{
			return this.targetCraft.Get();
		}

		// Token: 0x04002CAD RID: 11437
		public bool ShowWorkingStatus;

		// Token: 0x04002CAE RID: 11438
		[Serialize]
		private Ref<LaunchConditionManager> targetCraft;

		// Token: 0x04002CAF RID: 11439
		[NonSerialized]
		public float remainingSecondsToFreezeLogicSignal;

		// Token: 0x04002CB0 RID: 11440
		private DetectorNetwork.Def detectorNetworkDef;

		// Token: 0x04002CB1 RID: 11441
		private DetectorNetwork.Instance detectorNetwork;

		// Token: 0x04002CB2 RID: 11442
		private List<GameplayEventInstance> meteorShowers = new List<GameplayEventInstance>();
	}
}
