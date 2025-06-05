using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A2E RID: 2606
public class DrinkMilkMonitor : GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>
{
	// Token: 0x06002F41 RID: 12097 RVA: 0x002054E0 File Offset: 0x002036E0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.lookingToDrinkMilk;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.OnSignal(this.didFinishDrinkingMilk, this.applyEffect).Enter(delegate(DrinkMilkMonitor.Instance smi)
		{
			this.remainingSecondsForEffect.Set(Mathf.Clamp(this.remainingSecondsForEffect.Get(smi), 0f, 600f), smi, false);
		}).ParamTransition<float>(this.remainingSecondsForEffect, this.satisfied, (DrinkMilkMonitor.Instance smi, float val) => val > 0f);
		this.lookingToDrinkMilk.PreBrainUpdate(new Action<DrinkMilkMonitor.Instance>(DrinkMilkMonitor.FindMilkFeederTarget)).ToggleBehaviour(GameTags.Creatures.Behaviour_TryToDrinkMilkFromFeeder, (DrinkMilkMonitor.Instance smi) => !smi.targetMilkFeeder.IsNullOrStopped() && !smi.targetMilkFeeder.IsReserved(), null).Exit(delegate(DrinkMilkMonitor.Instance smi)
		{
			smi.targetMilkFeeder = null;
		});
		this.applyEffect.Enter(delegate(DrinkMilkMonitor.Instance smi)
		{
			this.remainingSecondsForEffect.Set(600f, smi, false);
		}).EnterTransition(this.satisfied, (DrinkMilkMonitor.Instance smi) => true);
		this.satisfied.Enter(delegate(DrinkMilkMonitor.Instance smi)
		{
			if (smi.def.consumesMilk)
			{
				smi.GetComponent<Effects>().Add("HadMilk", false).timeRemaining = this.remainingSecondsForEffect.Get(smi);
			}
		}).Exit(delegate(DrinkMilkMonitor.Instance smi)
		{
			if (smi.def.consumesMilk)
			{
				smi.GetComponent<Effects>().Remove("HadMilk");
			}
			this.remainingSecondsForEffect.Set(-1f, smi, false);
		}).ScheduleGoTo((DrinkMilkMonitor.Instance smi) => this.remainingSecondsForEffect.Get(smi), this.lookingToDrinkMilk).Update(delegate(DrinkMilkMonitor.Instance smi, float deltaSeconds)
		{
			this.remainingSecondsForEffect.Delta(-deltaSeconds, smi);
			if (this.remainingSecondsForEffect.Get(smi) < 0f)
			{
				smi.GoTo(this.lookingToDrinkMilk);
			}
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x06002F42 RID: 12098 RVA: 0x00205650 File Offset: 0x00203850
	private static void FindMilkFeederTarget(DrinkMilkMonitor.Instance smi)
	{
		DrinkMilkMonitor.<>c__DisplayClass8_0 CS$<>8__locals1;
		CS$<>8__locals1.smi = smi;
		int num = Grid.PosToCell(CS$<>8__locals1.smi.gameObject);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		List<MilkFeeder.Instance> items = Components.MilkFeeders.GetItems((int)Grid.WorldIdx[num]);
		if (items == null || items.Count == 0)
		{
			return;
		}
		using (ListPool<MilkFeeder.Instance, DrinkMilkMonitor>.PooledList pooledList = PoolsFor<DrinkMilkMonitor>.AllocateList<MilkFeeder.Instance>())
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(num);
			if (cavityForCell != null && cavityForCell.room != null && cavityForCell.room.roomType == Db.Get().RoomTypes.CreaturePen)
			{
				foreach (MilkFeeder.Instance instance in items)
				{
					if (!instance.IsNullOrDestroyed())
					{
						int cell = Grid.PosToCell(instance);
						if (Game.Instance.roomProber.GetCavityForCell(cell) == cavityForCell && instance.IsReadyToStartFeeding())
						{
							pooledList.Add(instance);
						}
					}
				}
			}
			DrinkMilkMonitor.<>c__DisplayClass8_1 CS$<>8__locals2;
			CS$<>8__locals2.canDrown = (CS$<>8__locals1.smi.drowningMonitor != null && CS$<>8__locals1.smi.drowningMonitor.canDrownToDeath && !CS$<>8__locals1.smi.drowningMonitor.livesUnderWater);
			CS$<>8__locals1.smi.targetMilkFeeder = null;
			CS$<>8__locals1.smi.doesTargetMilkFeederHaveSpaceForCritter = false;
			CS$<>8__locals2.resultCost = -1;
			foreach (MilkFeeder.Instance milkFeeder in pooledList)
			{
				DrinkMilkMonitor.<>c__DisplayClass8_2 CS$<>8__locals3;
				CS$<>8__locals3.milkFeeder = milkFeeder;
				if (DrinkMilkMonitor.<FindMilkFeederTarget>g__ConsiderCell|8_0(CS$<>8__locals1.smi.GetDrinkCellOf(CS$<>8__locals3.milkFeeder, false), ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3))
				{
					CS$<>8__locals1.smi.doesTargetMilkFeederHaveSpaceForCritter = false;
				}
				else if (DrinkMilkMonitor.<FindMilkFeederTarget>g__ConsiderCell|8_0(CS$<>8__locals1.smi.GetDrinkCellOf(CS$<>8__locals3.milkFeeder, true), ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3))
				{
					CS$<>8__locals1.smi.doesTargetMilkFeederHaveSpaceForCritter = true;
				}
			}
		}
	}

	// Token: 0x06002F4A RID: 12106 RVA: 0x0020588C File Offset: 0x00203A8C
	[CompilerGenerated]
	internal static bool <FindMilkFeederTarget>g__ConsiderCell|8_0(int cell, ref DrinkMilkMonitor.<>c__DisplayClass8_0 A_1, ref DrinkMilkMonitor.<>c__DisplayClass8_1 A_2, ref DrinkMilkMonitor.<>c__DisplayClass8_2 A_3)
	{
		if (A_2.canDrown && !A_1.smi.drowningMonitor.IsCellSafe(cell))
		{
			return false;
		}
		int navigationCost = A_1.smi.navigator.GetNavigationCost(cell);
		if (navigationCost == -1)
		{
			return false;
		}
		if (navigationCost < A_2.resultCost || A_2.resultCost == -1)
		{
			A_2.resultCost = navigationCost;
			A_1.smi.targetMilkFeeder = A_3.milkFeeder;
			return true;
		}
		return false;
	}

	// Token: 0x0400206A RID: 8298
	public GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.State lookingToDrinkMilk;

	// Token: 0x0400206B RID: 8299
	public GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.State applyEffect;

	// Token: 0x0400206C RID: 8300
	public GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.State satisfied;

	// Token: 0x0400206D RID: 8301
	private StateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.Signal didFinishDrinkingMilk;

	// Token: 0x0400206E RID: 8302
	private StateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.FloatParameter remainingSecondsForEffect;

	// Token: 0x02000A2F RID: 2607
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400206F RID: 8303
		public bool consumesMilk = true;

		// Token: 0x04002070 RID: 8304
		public DrinkMilkStates.Def.DrinkCellOffsetGetFn drinkCellOffsetGetFn;
	}

	// Token: 0x02000A30 RID: 2608
	public new class Instance : GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>.GameInstance
	{
		// Token: 0x06002F4C RID: 12108 RVA: 0x000C324E File Offset: 0x000C144E
		public Instance(IStateMachineTarget master, DrinkMilkMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x000C3258 File Offset: 0x000C1458
		public void NotifyFinishedDrinkingMilkFrom(MilkFeeder.Instance milkFeeder)
		{
			if (milkFeeder != null && base.def.consumesMilk)
			{
				milkFeeder.ConsumeMilkForOneFeeding();
			}
			base.sm.didFinishDrinkingMilk.Trigger(base.smi);
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x000C3286 File Offset: 0x000C1486
		public int GetDrinkCellOf(MilkFeeder.Instance milkFeeder, bool isTwoByTwoCritterCramped)
		{
			return Grid.OffsetCell(Grid.PosToCell(milkFeeder), base.def.drinkCellOffsetGetFn(milkFeeder, this, isTwoByTwoCritterCramped));
		}

		// Token: 0x04002071 RID: 8305
		public MilkFeeder.Instance targetMilkFeeder;

		// Token: 0x04002072 RID: 8306
		public bool doesTargetMilkFeederHaveSpaceForCritter;

		// Token: 0x04002073 RID: 8307
		[MyCmpReq]
		public Navigator navigator;

		// Token: 0x04002074 RID: 8308
		[MyCmpGet]
		public DrowningMonitor drowningMonitor;
	}
}
