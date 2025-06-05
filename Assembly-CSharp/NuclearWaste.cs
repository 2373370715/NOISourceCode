using System;
using KSerialization;

// Token: 0x020016C1 RID: 5825
public class NuclearWaste : GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>
{
	// Token: 0x06007829 RID: 30761 RVA: 0x0031D5A4 File Offset: 0x0031B7A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.PlayAnim((NuclearWaste.Instance smi) => smi.GetAnimToPlay(), KAnim.PlayMode.Once).Update(delegate(NuclearWaste.Instance smi, float dt)
		{
			smi.timeAlive += dt;
			string animToPlay = smi.GetAnimToPlay();
			if (smi.GetComponent<KBatchedAnimController>().GetCurrentAnim().name != animToPlay)
			{
				smi.Play(animToPlay, KAnim.PlayMode.Once);
			}
			if (smi.timeAlive >= 600f)
			{
				smi.GoTo(this.decayed);
			}
		}, UpdateRate.SIM_4000ms, false).EventHandler(GameHashes.Absorb, delegate(NuclearWaste.Instance smi, object otherObject)
		{
			Pickupable pickupable = (Pickupable)otherObject;
			float timeAlive = pickupable.GetSMI<NuclearWaste.Instance>().timeAlive;
			float mass = pickupable.PrimaryElement.Mass;
			float mass2 = smi.master.GetComponent<PrimaryElement>().Mass;
			float timeAlive2 = ((mass2 - mass) * smi.timeAlive + mass * timeAlive) / mass2;
			smi.timeAlive = timeAlive2;
			string animToPlay = smi.GetAnimToPlay();
			if (smi.GetComponent<KBatchedAnimController>().GetCurrentAnim().name != animToPlay)
			{
				smi.Play(animToPlay, KAnim.PlayMode.Once);
			}
			if (smi.timeAlive >= 600f)
			{
				smi.GoTo(this.decayed);
			}
		});
		this.decayed.Enter(delegate(NuclearWaste.Instance smi)
		{
			smi.GetComponent<Dumpable>().Dump();
			Util.KDestroyGameObject(smi.master.gameObject);
		});
	}

	// Token: 0x04005A53 RID: 23123
	private const float lifetime = 600f;

	// Token: 0x04005A54 RID: 23124
	public GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.State idle;

	// Token: 0x04005A55 RID: 23125
	public GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.State decayed;

	// Token: 0x020016C2 RID: 5826
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020016C3 RID: 5827
	public new class Instance : GameStateMachine<NuclearWaste, NuclearWaste.Instance, IStateMachineTarget, NuclearWaste.Def>.GameInstance
	{
		// Token: 0x0600782E RID: 30766 RVA: 0x000F38B6 File Offset: 0x000F1AB6
		public Instance(IStateMachineTarget master, NuclearWaste.Def def) : base(master, def)
		{
		}

		// Token: 0x0600782F RID: 30767 RVA: 0x0031D730 File Offset: 0x0031B930
		public string GetAnimToPlay()
		{
			this.percentageRemaining = 1f - base.smi.timeAlive / 600f;
			if (this.percentageRemaining <= 0.33f)
			{
				return "idle1";
			}
			if (this.percentageRemaining <= 0.66f)
			{
				return "idle2";
			}
			return "idle3";
		}

		// Token: 0x04005A56 RID: 23126
		[Serialize]
		public float timeAlive;

		// Token: 0x04005A57 RID: 23127
		private float percentageRemaining;
	}
}
