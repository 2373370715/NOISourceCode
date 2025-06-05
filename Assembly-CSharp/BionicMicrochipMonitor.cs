using System;
using UnityEngine;

// Token: 0x02001547 RID: 5447
public class BionicMicrochipMonitor : GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>
{
	// Token: 0x0600715D RID: 29021 RVA: 0x00309FE4 File Offset: 0x003081E4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.TagTransition(GameTags.BionicBedTime, this.production, false);
		this.production.TagTransition(GameTags.BionicBedTime, this.idle, true).Enter(new StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State.Callback(BionicMicrochipMonitor.CreateProgresesBar)).Exit(new StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State.Callback(BionicMicrochipMonitor.ClearProgressBar)).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicMicrochipGeneration, null).DefaultState(this.production.charging);
		this.production.charging.ParamTransition<float>(this.Progress, this.production.produceOne, GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.IsGTEOne).Update(new Action<BionicMicrochipMonitor.Instance, float>(BionicMicrochipMonitor.ProgressUpdate), UpdateRate.SIM_200ms, false);
		this.production.produceOne.Enter(new StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State.Callback(BionicMicrochipMonitor.CreateMicrochip)).Enter(new StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State.Callback(BionicMicrochipMonitor.ResetProgress)).GoTo(this.production.charging);
	}

	// Token: 0x0600715E RID: 29022 RVA: 0x000EE9EA File Offset: 0x000ECBEA
	public static void ClearProgressBar(BionicMicrochipMonitor.Instance smi)
	{
		smi.ClearProgressBar();
	}

	// Token: 0x0600715F RID: 29023 RVA: 0x000EE9F2 File Offset: 0x000ECBF2
	public static void CreateProgresesBar(BionicMicrochipMonitor.Instance smi)
	{
		smi.CreateProgressBar();
	}

	// Token: 0x06007160 RID: 29024 RVA: 0x000EE9FA File Offset: 0x000ECBFA
	public static void ResetProgress(BionicMicrochipMonitor.Instance smi)
	{
		smi.sm.Progress.Set(0f, smi, false);
	}

	// Token: 0x06007161 RID: 29025 RVA: 0x000EEA14 File Offset: 0x000ECC14
	public static void CreateMicrochip(BionicMicrochipMonitor.Instance smi)
	{
		smi.CreateMicrochip();
	}

	// Token: 0x06007162 RID: 29026 RVA: 0x0030A0F0 File Offset: 0x003082F0
	public static void ProgressUpdate(BionicMicrochipMonitor.Instance smi, float dt)
	{
		float num = dt / 150f;
		float progress = smi.Progress;
		smi.sm.Progress.Set(progress + num, smi, false);
	}

	// Token: 0x04005525 RID: 21797
	public const float MICROCHIP_PRODUCTION_TIME = 150f;

	// Token: 0x04005526 RID: 21798
	public GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State idle;

	// Token: 0x04005527 RID: 21799
	public BionicMicrochipMonitor.ProductionStates production;

	// Token: 0x04005528 RID: 21800
	public StateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.FloatParameter Progress;

	// Token: 0x02001548 RID: 5448
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001549 RID: 5449
	public class ProductionStates : GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State
	{
		// Token: 0x04005529 RID: 21801
		public GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State charging;

		// Token: 0x0400552A RID: 21802
		public GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.State produceOne;
	}

	// Token: 0x0200154A RID: 5450
	public new class Instance : GameStateMachine<BionicMicrochipMonitor, BionicMicrochipMonitor.Instance, IStateMachineTarget, BionicMicrochipMonitor.Def>.GameInstance
	{
		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06007166 RID: 29030 RVA: 0x000EEA2C File Offset: 0x000ECC2C
		public float Progress
		{
			get
			{
				return base.sm.Progress.Get(this);
			}
		}

		// Token: 0x06007167 RID: 29031 RVA: 0x000EEA3F File Offset: 0x000ECC3F
		public Instance(IStateMachineTarget master, BionicMicrochipMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06007168 RID: 29032 RVA: 0x000EEA49 File Offset: 0x000ECC49
		public void CreateMicrochip()
		{
			Util.KInstantiate(Assets.GetPrefab(PowerStationToolsConfig.tag), Grid.CellToPos(Grid.PosToCell(base.smi.gameObject), CellAlignment.Top, Grid.SceneLayer.Ore)).SetActive(true);
		}

		// Token: 0x06007169 RID: 29033 RVA: 0x0030A124 File Offset: 0x00308324
		public void CreateProgressBar()
		{
			this.progressBar = ProgressBar.CreateProgressBar(base.gameObject, () => this.Progress);
			base.smi.progressBar.SetVisibility(true);
			base.smi.progressBar.barColor = Color.green;
		}

		// Token: 0x0600716A RID: 29034 RVA: 0x000EEA78 File Offset: 0x000ECC78
		public void ClearProgressBar()
		{
			if (this.progressBar != null)
			{
				Util.KDestroyGameObject(base.smi.progressBar.gameObject);
				this.progressBar = null;
			}
		}

		// Token: 0x0400552B RID: 21803
		public ProgressBar progressBar;
	}
}
