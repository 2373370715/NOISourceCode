using System;
using Klei.AI;

// Token: 0x020015D6 RID: 5590
public class HygieneMonitor : GameStateMachine<HygieneMonitor, HygieneMonitor.Instance>
{
	// Token: 0x0600740D RID: 29709 RVA: 0x00311930 File Offset: 0x0030FB30
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.needsshower;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.clean.EventTransition(GameHashes.EffectRemoved, this.needsshower, (HygieneMonitor.Instance smi) => smi.NeedsShower());
		this.needsshower.EventTransition(GameHashes.EffectAdded, this.clean, (HygieneMonitor.Instance smi) => !smi.NeedsShower()).ToggleUrge(Db.Get().Urges.Shower).Enter(delegate(HygieneMonitor.Instance smi)
		{
			smi.SetDirtiness(1f);
		});
	}

	// Token: 0x04005721 RID: 22305
	public StateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.FloatParameter dirtiness;

	// Token: 0x04005722 RID: 22306
	public GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.State clean;

	// Token: 0x04005723 RID: 22307
	public GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.State needsshower;

	// Token: 0x020015D7 RID: 5591
	public new class Instance : GameStateMachine<HygieneMonitor, HygieneMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600740F RID: 29711 RVA: 0x000F08B8 File Offset: 0x000EEAB8
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.effects = master.GetComponent<Effects>();
		}

		// Token: 0x06007410 RID: 29712 RVA: 0x000F08CD File Offset: 0x000EEACD
		public float GetDirtiness()
		{
			return base.sm.dirtiness.Get(this);
		}

		// Token: 0x06007411 RID: 29713 RVA: 0x000F08E0 File Offset: 0x000EEAE0
		public void SetDirtiness(float dirtiness)
		{
			base.sm.dirtiness.Set(dirtiness, this, false);
		}

		// Token: 0x06007412 RID: 29714 RVA: 0x000F08F6 File Offset: 0x000EEAF6
		public bool NeedsShower()
		{
			return !this.effects.HasEffect(Shower.SHOWER_EFFECT);
		}

		// Token: 0x04005724 RID: 22308
		private Effects effects;
	}
}
