using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02001640 RID: 5696
public class SteppedInMonitor : GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance>
{
	// Token: 0x060075D6 RID: 30166 RVA: 0x0031692C File Offset: 0x00314B2C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.carpetedFloor, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsOnCarpet), UpdateRate.SIM_200ms).Transition(this.wetFloor, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet), UpdateRate.SIM_200ms).Transition(this.wetBody, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged), UpdateRate.SIM_200ms);
		this.carpetedFloor.Enter(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetCarpetFeet)).ToggleExpression(Db.Get().Expressions.Tickled, null).Update(new Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetCarpetFeet), UpdateRate.SIM_1000ms, false).Transition(this.satisfied, GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsOnCarpet)), UpdateRate.SIM_200ms).Transition(this.wetFloor, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet), UpdateRate.SIM_200ms).Transition(this.wetBody, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged), UpdateRate.SIM_200ms);
		this.wetFloor.Enter(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetWetFeet)).Update(new Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetWetFeet), UpdateRate.SIM_1000ms, false).Transition(this.satisfied, GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsFloorWet)), UpdateRate.SIM_200ms).Transition(this.wetBody, new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged), UpdateRate.SIM_200ms);
		this.wetBody.Enter(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State.Callback(SteppedInMonitor.GetSoaked)).Update(new Action<SteppedInMonitor.Instance, float>(SteppedInMonitor.GetSoaked), UpdateRate.SIM_1000ms, false).Transition(this.wetFloor, GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(SteppedInMonitor.IsSubmerged)), UpdateRate.SIM_200ms);
	}

	// Token: 0x060075D7 RID: 30167 RVA: 0x000F1EF6 File Offset: 0x000F00F6
	private static void GetCarpetFeet(SteppedInMonitor.Instance smi, float dt)
	{
		SteppedInMonitor.GetCarpetFeet(smi);
	}

	// Token: 0x060075D8 RID: 30168 RVA: 0x00316AC8 File Offset: 0x00314CC8
	private static void GetCarpetFeet(SteppedInMonitor.Instance smi)
	{
		if (!smi.effects.HasEffect("SoakingWet") && !smi.effects.HasEffect("WetFeet") && smi.IsEffectAllowed("CarpetFeet"))
		{
			smi.effects.Add("CarpetFeet", true);
		}
	}

	// Token: 0x060075D9 RID: 30169 RVA: 0x000F1EFE File Offset: 0x000F00FE
	private static void GetWetFeet(SteppedInMonitor.Instance smi, float dt)
	{
		SteppedInMonitor.GetWetFeet(smi);
	}

	// Token: 0x060075DA RID: 30170 RVA: 0x000F1F06 File Offset: 0x000F0106
	private static void GetWetFeet(SteppedInMonitor.Instance smi)
	{
		if (!smi.effects.HasEffect("SoakingWet") && smi.IsEffectAllowed("WetFeet"))
		{
			smi.effects.Add("WetFeet", true);
		}
	}

	// Token: 0x060075DB RID: 30171 RVA: 0x000F1F39 File Offset: 0x000F0139
	private static void GetSoaked(SteppedInMonitor.Instance smi, float dt)
	{
		SteppedInMonitor.GetSoaked(smi);
	}

	// Token: 0x060075DC RID: 30172 RVA: 0x00316B18 File Offset: 0x00314D18
	private static void GetSoaked(SteppedInMonitor.Instance smi)
	{
		if (smi.effects.HasEffect("WetFeet"))
		{
			smi.effects.Remove("WetFeet");
		}
		if (smi.IsEffectAllowed("SoakingWet"))
		{
			smi.effects.Add("SoakingWet", true);
		}
	}

	// Token: 0x060075DD RID: 30173 RVA: 0x00316B68 File Offset: 0x00314D68
	private static bool IsOnCarpet(SteppedInMonitor.Instance smi)
	{
		int cell = Grid.CellBelow(Grid.PosToCell(smi));
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[cell, 9];
		return Grid.IsValidCell(cell) && gameObject != null && gameObject.HasTag(GameTags.Carpeted);
	}

	// Token: 0x060075DE RID: 30174 RVA: 0x00316BB8 File Offset: 0x00314DB8
	private static bool IsFloorWet(SteppedInMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi);
		return Grid.IsValidCell(num) && Grid.Element[num].IsLiquid;
	}

	// Token: 0x060075DF RID: 30175 RVA: 0x00316BE4 File Offset: 0x00314DE4
	private static bool IsSubmerged(SteppedInMonitor.Instance smi)
	{
		int num = Grid.CellAbove(Grid.PosToCell(smi));
		return Grid.IsValidCell(num) && Grid.Element[num].IsLiquid;
	}

	// Token: 0x04005890 RID: 22672
	public const string CARPET_EFFECT_NAME = "CarpetFeet";

	// Token: 0x04005891 RID: 22673
	public const string WET_FEET_EFFECT_NAME = "WetFeet";

	// Token: 0x04005892 RID: 22674
	public const string SOAK_EFFECT_NAME = "SoakingWet";

	// Token: 0x04005893 RID: 22675
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04005894 RID: 22676
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State carpetedFloor;

	// Token: 0x04005895 RID: 22677
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetFloor;

	// Token: 0x04005896 RID: 22678
	public GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.State wetBody;

	// Token: 0x02001641 RID: 5697
	public new class Instance : GameStateMachine<SteppedInMonitor, SteppedInMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x060075E2 RID: 30178 RVA: 0x000F1F52 File Offset: 0x000F0152
		// (set) Token: 0x060075E1 RID: 30177 RVA: 0x000F1F49 File Offset: 0x000F0149
		public string[] effectsAllowed { get; private set; }

		// Token: 0x060075E3 RID: 30179 RVA: 0x000F1F5A File Offset: 0x000F015A
		public Instance(IStateMachineTarget master) : this(master, new string[]
		{
			"CarpetFeet",
			"WetFeet",
			"SoakingWet"
		})
		{
		}

		// Token: 0x060075E4 RID: 30180 RVA: 0x000F1F81 File Offset: 0x000F0181
		public Instance(IStateMachineTarget master, string[] effectsAllowed) : base(master)
		{
			this.effects = base.GetComponent<Effects>();
			this.effectsAllowed = effectsAllowed;
		}

		// Token: 0x060075E5 RID: 30181 RVA: 0x00316C14 File Offset: 0x00314E14
		public bool IsEffectAllowed(string effectName)
		{
			if (this.effectsAllowed == null || this.effectsAllowed.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.effectsAllowed.Length; i++)
			{
				if (this.effectsAllowed[i] == effectName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005897 RID: 22679
		public Effects effects;
	}
}
