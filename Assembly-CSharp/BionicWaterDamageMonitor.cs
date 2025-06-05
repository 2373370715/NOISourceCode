using System;
using Klei.AI;

// Token: 0x02001568 RID: 5480
public class BionicWaterDamageMonitor : GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>
{
	// Token: 0x0600723F RID: 29247 RVA: 0x0030C5A8 File Offset: 0x0030A7A8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.safe;
		this.safe.Transition(this.suffering, new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.Transition.ConditionCallback(BionicWaterDamageMonitor.IsSuffering), UpdateRate.SIM_200ms);
		this.suffering.Transition(this.safe, GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.Not(new StateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.Transition.ConditionCallback(BionicWaterDamageMonitor.IsSuffering)), UpdateRate.SIM_200ms).ToggleEffect("BionicWaterStress").ToggleReactable(new Func<BionicWaterDamageMonitor.Instance, Reactable>(BionicWaterDamageMonitor.ZapReactable));
	}

	// Token: 0x06007240 RID: 29248 RVA: 0x000EF481 File Offset: 0x000ED681
	private static Reactable ZapReactable(BionicWaterDamageMonitor.Instance smi)
	{
		return smi.GetZapReactable();
	}

	// Token: 0x06007241 RID: 29249 RVA: 0x000EF489 File Offset: 0x000ED689
	private static bool IsSuffering(BionicWaterDamageMonitor.Instance smi)
	{
		return BionicWaterDamageMonitor.IsFloorWetWithIntolerantSubstance(smi);
	}

	// Token: 0x06007242 RID: 29250 RVA: 0x0030C624 File Offset: 0x0030A824
	private static bool IsFloorWetWithIntolerantSubstance(BionicWaterDamageMonitor.Instance smi)
	{
		if (smi.master.gameObject.HasTag(GameTags.InTransitTube))
		{
			return false;
		}
		int num = Grid.PosToCell(smi);
		return Grid.IsValidCell(num) && Grid.Element[num].IsLiquid && !smi.kpid.HasTag(GameTags.HasAirtightSuit) && smi.def.IsElementIntolerable(Grid.Element[num].id);
	}

	// Token: 0x040055B1 RID: 21937
	public const string EFFECT_NAME = "BionicWaterStress";

	// Token: 0x040055B2 RID: 21938
	public GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State safe;

	// Token: 0x040055B3 RID: 21939
	public GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.State suffering;

	// Token: 0x02001569 RID: 5481
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06007244 RID: 29252 RVA: 0x0030C694 File Offset: 0x0030A894
		public bool IsElementIntolerable(SimHashes element)
		{
			for (int i = 0; i < this.IntolerantToElements.Length; i++)
			{
				if (this.IntolerantToElements[i] == element)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040055B4 RID: 21940
		public readonly SimHashes[] IntolerantToElements = new SimHashes[]
		{
			SimHashes.Water,
			SimHashes.DirtyWater,
			SimHashes.SaltWater,
			SimHashes.Brine
		};

		// Token: 0x040055B5 RID: 21941
		public static float ZapInterval = 10f;
	}

	// Token: 0x0200156A RID: 5482
	public new class Instance : GameStateMachine<BionicWaterDamageMonitor, BionicWaterDamageMonitor.Instance, IStateMachineTarget, BionicWaterDamageMonitor.Def>.GameInstance
	{
		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06007247 RID: 29255 RVA: 0x000EF4C4 File Offset: 0x000ED6C4
		public bool IsAffectedByWaterDamage
		{
			get
			{
				return this.effects.HasEffect("BionicWaterStress");
			}
		}

		// Token: 0x06007248 RID: 29256 RVA: 0x000EF4D6 File Offset: 0x000ED6D6
		public Instance(IStateMachineTarget master, BionicWaterDamageMonitor.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
		}

		// Token: 0x06007249 RID: 29257 RVA: 0x0030C6C4 File Offset: 0x0030A8C4
		public Reactable GetZapReactable()
		{
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, Db.Get().Emotes.Minion.WaterDamage.Id, Db.Get().ChoreTypes.WaterDamageZap, 0f, BionicWaterDamageMonitor.Def.ZapInterval, float.PositiveInfinity, 0f);
			Emote waterDamage = Db.Get().Emotes.Minion.WaterDamage;
			selfEmoteReactable.SetEmote(waterDamage);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable;
		}

		// Token: 0x040055B6 RID: 21942
		public Effects effects;

		// Token: 0x040055B7 RID: 21943
		[MyCmpGet]
		public KPrefabID kpid;
	}
}
