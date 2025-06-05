using System;
using UnityEngine;

// Token: 0x02000C83 RID: 3203
public class BionicUpgrade_ExplorerBooster : GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>
{
	// Token: 0x06003CD4 RID: 15572 RVA: 0x0023D5D8 File Offset: 0x0023B7D8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.not_ready;
		this.not_ready.ParamTransition<float>(this.Progress, this.ready, GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.IsGTEOne).ToggleStatusItem(Db.Get().MiscStatusItems.BionicExplorerBooster, null);
		this.ready.ParamTransition<float>(this.Progress, this.not_ready, GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.IsLTOne).ToggleStatusItem(Db.Get().MiscStatusItems.BionicExplorerBoosterReady, null);
	}

	// Token: 0x04002A25 RID: 10789
	public const float DataGatheringDuration = 600f;

	// Token: 0x04002A26 RID: 10790
	private StateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.FloatParameter Progress;

	// Token: 0x04002A27 RID: 10791
	public GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.State not_ready;

	// Token: 0x04002A28 RID: 10792
	public GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.State ready;

	// Token: 0x02000C84 RID: 3204
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000C85 RID: 3205
	public new class Instance : GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.GameInstance
	{
		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06003CD7 RID: 15575 RVA: 0x000CBC4E File Offset: 0x000C9E4E
		public bool IsBeingMonitored
		{
			get
			{
				return this.monitor != null;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06003CD8 RID: 15576 RVA: 0x000CBC59 File Offset: 0x000C9E59
		public bool IsReady
		{
			get
			{
				return this.Progress == 1f;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06003CD9 RID: 15577 RVA: 0x000CBC68 File Offset: 0x000C9E68
		public float Progress
		{
			get
			{
				return base.sm.Progress.Get(this);
			}
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x000CBC7B File Offset: 0x000C9E7B
		public Instance(IStateMachineTarget master, BionicUpgrade_ExplorerBooster.Def def) : base(master, def)
		{
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x000CBC85 File Offset: 0x000C9E85
		public void SetMonitor(BionicUpgrade_ExplorerBoosterMonitor.Instance monitor)
		{
			this.monitor = monitor;
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x0023D658 File Offset: 0x0023B858
		public void AddData(float dataProgressDelta)
		{
			float dataProgress = Mathf.Clamp(this.Progress + dataProgressDelta, 0f, 1f);
			this.SetDataProgress(dataProgress);
		}

		// Token: 0x06003CDD RID: 15581 RVA: 0x000CBC8E File Offset: 0x000C9E8E
		public void SetDataProgress(float dataProgress)
		{
			Mathf.Clamp(dataProgress, 0f, 1f);
			base.sm.Progress.Set(dataProgress, this, false);
		}

		// Token: 0x04002A29 RID: 10793
		private BionicUpgrade_ExplorerBoosterMonitor.Instance monitor;
	}
}
