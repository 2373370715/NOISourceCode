using System;

// Token: 0x02000E51 RID: 3665
public class LandingBeacon : GameStateMachine<LandingBeacon, LandingBeacon.Instance>
{
	// Token: 0x060047A8 RID: 18344 RVA: 0x002614B0 File Offset: 0x0025F6B0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.Update(new Action<LandingBeacon.Instance, float>(LandingBeacon.UpdateLineOfSight), UpdateRate.SIM_200ms, false);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.working, (LandingBeacon.Instance smi) => smi.operational.IsOperational);
		this.working.DefaultState(this.working.pre).EventTransition(GameHashes.OperationalChanged, this.off, (LandingBeacon.Instance smi) => !smi.operational.IsOperational);
		this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
		this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Enter("SetActive", delegate(LandingBeacon.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive", delegate(LandingBeacon.Instance smi)
		{
			smi.operational.SetActive(false, false);
		});
	}

	// Token: 0x060047A9 RID: 18345 RVA: 0x002615F4 File Offset: 0x0025F7F4
	public static void UpdateLineOfSight(LandingBeacon.Instance smi, float dt)
	{
		WorldContainer myWorld = smi.GetMyWorld();
		bool flag = true;
		int num = Grid.PosToCell(smi);
		int num2 = (int)myWorld.maximumBounds.y;
		while (Grid.CellRow(num) <= num2)
		{
			if (!Grid.IsValidCell(num) || Grid.Solid[num])
			{
				flag = false;
				break;
			}
			num = Grid.CellAbove(num);
		}
		if (smi.skyLastVisible != flag)
		{
			smi.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NoSurfaceSight, !flag, null);
			smi.operational.SetFlag(LandingBeacon.noSurfaceSight, flag);
			smi.skyLastVisible = flag;
		}
	}

	// Token: 0x04003234 RID: 12852
	public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04003235 RID: 12853
	public LandingBeacon.WorkingStates working;

	// Token: 0x04003236 RID: 12854
	public static readonly Operational.Flag noSurfaceSight = new Operational.Flag("noSurfaceSight", Operational.Flag.Type.Requirement);

	// Token: 0x02000E52 RID: 3666
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000E53 RID: 3667
	public class WorkingStates : GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04003237 RID: 12855
		public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State pre;

		// Token: 0x04003238 RID: 12856
		public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State loop;

		// Token: 0x04003239 RID: 12857
		public GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.State pst;
	}

	// Token: 0x02000E54 RID: 3668
	public new class Instance : GameStateMachine<LandingBeacon, LandingBeacon.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060047AE RID: 18350 RVA: 0x000D2E33 File Offset: 0x000D1033
		public Instance(IStateMachineTarget master, LandingBeacon.Def def) : base(master, def)
		{
			Components.LandingBeacons.Add(this);
			this.operational = base.GetComponent<Operational>();
			this.selectable = base.GetComponent<KSelectable>();
		}

		// Token: 0x060047AF RID: 18351 RVA: 0x000D2E67 File Offset: 0x000D1067
		public override void StartSM()
		{
			base.StartSM();
			LandingBeacon.UpdateLineOfSight(this, 0f);
		}

		// Token: 0x060047B0 RID: 18352 RVA: 0x000D2E7A File Offset: 0x000D107A
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Components.LandingBeacons.Remove(this);
		}

		// Token: 0x060047B1 RID: 18353 RVA: 0x000D2E8D File Offset: 0x000D108D
		public bool CanBeTargeted()
		{
			return base.IsInsideState(base.sm.working);
		}

		// Token: 0x0400323A RID: 12858
		public Operational operational;

		// Token: 0x0400323B RID: 12859
		public KSelectable selectable;

		// Token: 0x0400323C RID: 12860
		public bool skyLastVisible = true;
	}
}
