using System;
using STRINGS;
using UnityEngine;

// Token: 0x020006DD RID: 1757
public class FoodFightChore : Chore<FoodFightChore.StatesInstance>
{
	// Token: 0x06001F3E RID: 7998 RVA: 0x001C3F6C File Offset: 0x001C216C
	public FoodFightChore(IStateMachineTarget master, GameObject locator) : base(Db.Get().ChoreTypes.Party, master, master.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		base.smi = new FoodFightChore.StatesInstance(this, locator);
		this.showAvailabilityInHoverText = false;
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(FoodFightChore.EdibleIsNotNull, null);
	}

	// Token: 0x06001F3F RID: 7999 RVA: 0x001C3FD4 File Offset: 0x001C21D4
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("FOODFIGHTCHORE null context.consumer");
			return;
		}
		RationMonitor.Instance smi = context.consumerState.consumer.GetSMI<RationMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("FOODFIGHTCHORE null RationMonitor.Instance");
			return;
		}
		Edible edible = smi.GetEdible();
		if (edible.gameObject == null)
		{
			global::Debug.LogError("FOODFIGHTCHORE null edible.gameObject");
			return;
		}
		if (base.smi == null)
		{
			global::Debug.LogError("FOODFIGHTCHORE null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			global::Debug.LogError("FOODFIGHTCHORE null smi.sm");
			return;
		}
		if (base.smi.sm.ediblesource == null)
		{
			global::Debug.LogError("FOODFIGHTCHORE null smi.sm.ediblesource");
			return;
		}
		base.smi.sm.ediblesource.Set(edible.gameObject, base.smi, false);
		KCrashReporter.Assert(edible.FoodInfo.CaloriesPerUnit > 0f, edible.GetProperName() + " has invalid calories per unit. Will result in NaNs", null);
		float num = 0.5f;
		KCrashReporter.Assert(num > 0f, "FoodFightChore is requesting an invalid amount of food", null);
		base.smi.sm.requestedfoodunits.Set(num, base.smi, false);
		base.smi.sm.eater.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x04001489 RID: 5257
	public static readonly Chore.Precondition EdibleIsNotNull = new Chore.Precondition
	{
		id = "EdibleIsNotNull",
		description = DUPLICANTS.CHORES.PRECONDITIONS.EDIBLE_IS_NOT_NULL,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return null != context.consumerState.consumer.GetSMI<RationMonitor.Instance>().GetEdible();
		}
	};

	// Token: 0x020006DE RID: 1758
	public class StatesInstance : GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.GameInstance
	{
		// Token: 0x06001F41 RID: 8001 RVA: 0x000B9093 File Offset: 0x000B7293
		public StatesInstance(FoodFightChore master, GameObject locator) : base(master)
		{
			base.sm.locator.Set(locator, base.smi, false);
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x001C4188 File Offset: 0x001C2388
		public void UpdateAttackTarget()
		{
			int num = 0;
			MinionIdentity minionIdentity;
			for (;;)
			{
				num++;
				minionIdentity = Components.LiveMinionIdentities[UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Count)];
				if (num > 30)
				{
					break;
				}
				if (Components.LiveMinionIdentities.Count <= 1 || ((!(base.sm.attackableTarget.Get(base.smi) != null) || !(minionIdentity.gameObject == base.sm.attackableTarget.Get(base.smi).gameObject)) && Game.Instance.roomProber.GetRoomOfGameObject(minionIdentity.gameObject) != null && (Game.Instance.roomProber.GetRoomOfGameObject(minionIdentity.gameObject).roomType == Db.Get().RoomTypes.MessHall || Game.Instance.roomProber.GetRoomOfGameObject(minionIdentity.gameObject).roomType == Db.Get().RoomTypes.GreatHall) && !(minionIdentity.gameObject == base.smi.master.gameObject) && Game.Instance.roomProber.GetRoomOfGameObject(minionIdentity.gameObject) == Game.Instance.roomProber.GetRoomOfGameObject(base.smi.master.gameObject)))
				{
					goto IL_152;
				}
			}
			minionIdentity = null;
			IL_152:
			if (minionIdentity == null)
			{
				base.smi.GoTo(base.sm.end);
				return;
			}
			base.smi.sm.attackableTarget.Set(minionIdentity.GetComponent<AttackableBase>(), base.smi);
		}

		// Token: 0x0400148A RID: 5258
		private int locatorCell;
	}

	// Token: 0x020006DF RID: 1759
	public class States : GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore>
	{
		// Token: 0x06001F43 RID: 8003 RVA: 0x001C4328 File Offset: 0x001C2528
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.eater);
			this.root.ToggleAnims("anim_loco_run_angry_kanim", 0f);
			this.fetch.InitializeStates(this.eater, this.ediblesource, this.ediblechunk, this.requestedfoodunits, this.actualfoodunits, this.moveToArena, null).ToggleAnims("anim_loco_run_angry_kanim", 0f);
			this.moveToArena.InitializeStates(this.eater, this.locator, this.waitForParticipants, null, null, null);
			this.waitForParticipants.Enter(delegate(FoodFightChore.StatesInstance smi)
			{
				smi.master.GetComponent<Facing>().SetFacing(Game.Instance.roomProber.GetRoomOfGameObject(smi.master.gameObject).cavity.GetCenter().x <= smi.master.transform.position.x);
			}).ToggleAnims("anim_rage_kanim", 0f).PlayAnim("idle_pre").QueueAnim("idle_default", true, null).ScheduleGoTo(30f, this.emoteRoar).EventTransition(GameHashes.GameplayEventCommence, this.emoteRoar, null);
			this.emoteRoar.Enter("ChooseTarget", delegate(FoodFightChore.StatesInstance smi)
			{
				smi.UpdateAttackTarget();
			}).ToggleAnims("anim_rage_kanim", 0f).PlayAnim("rage_pre").QueueAnim("rage_loop", false, null).QueueAnim("rage_pst", false, null).OnAnimQueueComplete(this.fight);
			this.fight.DefaultState(this.fight.moveto);
			this.fight.moveto.InitializeStates(this.eater, this.attackableTarget, this.fight.throwFood, null, null, NavigationTactics.Range_3_ProhibitOverlap);
			this.fight.throwFood.Enter(delegate(FoodFightChore.StatesInstance smi)
			{
				smi.master.GetComponent<Facing>().Face(this.attackableTarget.Get(smi).transform.position.x);
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(FoodCometConfig.ID), smi.master.transform.position + Vector3.up);
				gameObject.SetActive(true);
				Comet comet = gameObject.GetComponent<Comet>();
				Vector3 a = this.attackableTarget.Get(smi).transform.position - smi.master.transform.position;
				a.Normalize();
				comet.Velocity = a * 5f;
				Comet comet2 = comet;
				comet2.OnImpact = (System.Action)Delegate.Combine(comet2.OnImpact, new System.Action(delegate()
				{
					GameObject gameObject3 = Grid.Objects[Grid.PosToCell(comet), 0];
					if (gameObject3 != null)
					{
						if (UnityEngine.Random.Range(0, 100) > 75)
						{
							new FleeChore(gameObject3.GetComponent<IStateMachineTarget>(), smi.master.gameObject);
							return;
						}
						gameObject3.Trigger(508119890, null);
					}
				}));
				GameObject gameObject2 = smi.master.GetComponent<Storage>().FindFirst(GameTags.Edible);
				if (!(gameObject2 != null))
				{
					smi.GoTo(this.end);
					return;
				}
				Edible component = gameObject2.GetComponent<Edible>();
				float num = Math.Min(200000f, component.Calories);
				component.Calories -= num;
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -num, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.FOODFIGHT_CONTEXT, "{0}", component.GetProperName()), null);
				if (component.Calories <= 0f)
				{
					Util.KDestroyGameObject(gameObject2);
					smi.GoTo(this.end);
					return;
				}
				smi.GoTo(this.emoteRoar);
			});
			this.end.Enter(delegate(FoodFightChore.StatesInstance smi)
			{
				Util.KDestroyGameObject(this.ediblechunk.Get(smi));
				smi.StopSM("complete");
			});
		}

		// Token: 0x0400148B RID: 5259
		public StateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.TargetParameter eater;

		// Token: 0x0400148C RID: 5260
		public StateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.TargetParameter ediblesource;

		// Token: 0x0400148D RID: 5261
		public StateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.TargetParameter ediblechunk;

		// Token: 0x0400148E RID: 5262
		public StateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.TargetParameter attackableTarget;

		// Token: 0x0400148F RID: 5263
		public StateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.FloatParameter requestedfoodunits;

		// Token: 0x04001490 RID: 5264
		public StateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.FloatParameter actualfoodunits;

		// Token: 0x04001491 RID: 5265
		public StateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.TargetParameter locator;

		// Token: 0x04001492 RID: 5266
		public GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.State waitForParticipants;

		// Token: 0x04001493 RID: 5267
		public GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.State emoteRoar;

		// Token: 0x04001494 RID: 5268
		public GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.ApproachSubState<IApproachable> moveToArena;

		// Token: 0x04001495 RID: 5269
		public GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.FetchSubState fetch;

		// Token: 0x04001496 RID: 5270
		public FoodFightChore.States.AttackStates fight;

		// Token: 0x04001497 RID: 5271
		public GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.State end;

		// Token: 0x020006E0 RID: 1760
		public class AttackStates : GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.State
		{
			// Token: 0x04001498 RID: 5272
			public GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.ApproachSubState<AttackableBase> moveto;

			// Token: 0x04001499 RID: 5273
			public GameStateMachine<FoodFightChore.States, FoodFightChore.StatesInstance, FoodFightChore, object>.State throwFood;
		}
	}
}
