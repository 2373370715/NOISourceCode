using System;
using Klei.AI;

// Token: 0x020010A7 RID: 4263
public class ChilledBones : GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>
{
	// Token: 0x06005693 RID: 22163 RVA: 0x00290B1C File Offset: 0x0028ED1C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.normal;
		this.normal.UpdateTransition(this.chilled, new Func<ChilledBones.Instance, float, bool>(this.IsChilling), UpdateRate.SIM_200ms, false);
		this.chilled.ToggleEffect("ChilledBones").UpdateTransition(this.normal, new Func<ChilledBones.Instance, float, bool>(this.IsNotChilling), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x06005694 RID: 22164 RVA: 0x000DCDDD File Offset: 0x000DAFDD
	public bool IsNotChilling(ChilledBones.Instance smi, float dt)
	{
		return !this.IsChilling(smi, dt);
	}

	// Token: 0x06005695 RID: 22165 RVA: 0x000DCDEA File Offset: 0x000DAFEA
	public bool IsChilling(ChilledBones.Instance smi, float dt)
	{
		return smi.IsChilled;
	}

	// Token: 0x04003D56 RID: 15702
	public const string EFFECT_NAME = "ChilledBones";

	// Token: 0x04003D57 RID: 15703
	public GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>.State normal;

	// Token: 0x04003D58 RID: 15704
	public GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>.State chilled;

	// Token: 0x020010A8 RID: 4264
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04003D59 RID: 15705
		public float THRESHOLD = -1f;
	}

	// Token: 0x020010A9 RID: 4265
	public new class Instance : GameStateMachine<ChilledBones, ChilledBones.Instance, IStateMachineTarget, ChilledBones.Def>.GameInstance
	{
		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06005698 RID: 22168 RVA: 0x000DCE0D File Offset: 0x000DB00D
		public float TemperatureTransferAttribute
		{
			get
			{
				return this.minionModifiers.GetAttributes().GetValue(this.bodyTemperatureTransferAttribute.Id) * 600f;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06005699 RID: 22169 RVA: 0x000DCE30 File Offset: 0x000DB030
		public bool IsChilled
		{
			get
			{
				return this.TemperatureTransferAttribute < base.def.THRESHOLD;
			}
		}

		// Token: 0x0600569A RID: 22170 RVA: 0x000DCE45 File Offset: 0x000DB045
		public Instance(IStateMachineTarget master, ChilledBones.Def def) : base(master, def)
		{
			this.bodyTemperatureTransferAttribute = Db.Get().Attributes.TryGet("TemperatureDelta");
		}

		// Token: 0x04003D5A RID: 15706
		[MyCmpGet]
		public MinionModifiers minionModifiers;

		// Token: 0x04003D5B RID: 15707
		public Klei.AI.Attribute bodyTemperatureTransferAttribute;
	}
}
