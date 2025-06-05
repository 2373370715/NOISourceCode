using System;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000777 RID: 1911
public class VomitChore : Chore<VomitChore.StatesInstance>
{
	// Token: 0x06002163 RID: 8547 RVA: 0x001CCAA4 File Offset: 0x001CACA4
	private static KAnimFile GetAnimFileName(VomitChore.StatesInstance smi)
	{
		string s = "anim_vomit_kanim";
		GameObject gameObject = smi.sm.vomiter.Get(smi);
		if (gameObject == null)
		{
			return Assets.GetAnim(s);
		}
		MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
		if (component == null)
		{
			return Assets.GetAnim(s);
		}
		if (component.model == BionicMinionConfig.MODEL)
		{
			return Assets.GetAnim("anim_bionic_vomit_kanim");
		}
		return Assets.GetAnim(s);
	}

	// Token: 0x06002164 RID: 8548 RVA: 0x001CCB28 File Offset: 0x001CAD28
	public VomitChore(ChoreType chore_type, IStateMachineTarget target, StatusItem status_item, Notification notification, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.Vomit, target, target.GetComponent<ChoreProvider>(), true, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new VomitChore.StatesInstance(this, target.gameObject, status_item, notification);
	}

	// Token: 0x02000778 RID: 1912
	public class StatesInstance : GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.GameInstance
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x000BA48B File Offset: 0x000B868B
		// (set) Token: 0x06002165 RID: 8549 RVA: 0x000BA482 File Offset: 0x000B8682
		public SimHashes elementToVomit { get; private set; } = SimHashes.DirtyWater;

		// Token: 0x06002167 RID: 8551 RVA: 0x001CCB74 File Offset: 0x001CAD74
		public StatesInstance(VomitChore master, GameObject vomiter, StatusItem status_item, Notification notification) : base(master)
		{
			base.sm.vomiter.Set(vomiter, base.smi, false);
			this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(vomiter);
			this.statusItem = status_item;
			this.notification = notification;
			this.vomitCellQuery = new SafetyQuery(Game.Instance.safetyConditions.VomitCellChecker, base.GetComponent<KMonoBehaviour>(), 10);
			MinionIdentity component = vomiter.GetComponent<MinionIdentity>();
			if (component != null && component.model == BionicMinionConfig.MODEL)
			{
				this.elementToVomit = SimHashes.LiquidGunk;
			}
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x001CCC28 File Offset: 0x001CAE28
		private static bool CanEmitLiquid(int cell)
		{
			bool result = true;
			if (!Grid.IsValidCell(cell) || Grid.Solid[cell] || (Grid.Properties[cell] & 2) != 0)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x000BA493 File Offset: 0x000B8693
		public void SpawnDirtyWater(float dt)
		{
			this.SpawnVomitLiquid(dt, SimHashes.DirtyWater);
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x001CCC60 File Offset: 0x001CAE60
		public void SpawnVomitLiquid(float dt, SimHashes element)
		{
			if (dt > 0f)
			{
				float totalTime = base.GetComponent<KBatchedAnimController>().CurrentAnim.totalTime;
				float num = dt / totalTime;
				Sicknesses sicknesses = base.master.GetComponent<MinionModifiers>().sicknesses;
				SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
				int num2 = 0;
				while (num2 < sicknesses.Count && sicknesses[num2].modifier.sicknessType != Sickness.SicknessType.Pathogen)
				{
					num2++;
				}
				Facing component = base.sm.vomiter.Get(base.smi).GetComponent<Facing>();
				int num3 = Grid.PosToCell(component.transform.GetPosition());
				int num4 = component.GetFrontCell();
				if (!VomitChore.StatesInstance.CanEmitLiquid(num4))
				{
					num4 = num3;
				}
				Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
				if (equippable != null)
				{
					equippable.GetComponent<Storage>().AddLiquid(element, STRESS.VOMIT_AMOUNT * num, this.bodyTemperature.value, invalid.idx, invalid.count, false, true);
					return;
				}
				SimMessages.AddRemoveSubstance(num4, element, CellEventLogger.Instance.Vomit, STRESS.VOMIT_AMOUNT * num, this.bodyTemperature.value, invalid.idx, invalid.count, true, -1);
			}
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x001CCD88 File Offset: 0x001CAF88
		public int GetVomitCell()
		{
			this.vomitCellQuery.Reset();
			Navigator component = base.GetComponent<Navigator>();
			component.RunQuery(this.vomitCellQuery);
			int num = this.vomitCellQuery.GetResultCell();
			if (Grid.InvalidCell == num)
			{
				num = Grid.PosToCell(component);
			}
			return num;
		}

		// Token: 0x0400166F RID: 5743
		public StatusItem statusItem;

		// Token: 0x04001670 RID: 5744
		private AmountInstance bodyTemperature;

		// Token: 0x04001671 RID: 5745
		public Notification notification;

		// Token: 0x04001672 RID: 5746
		private SafetyQuery vomitCellQuery;
	}

	// Token: 0x02000779 RID: 1913
	public class States : GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore>
	{
		// Token: 0x0600216C RID: 8556 RVA: 0x001CCDD0 File Offset: 0x001CAFD0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.moveto;
			base.Target(this.vomiter);
			this.root.ToggleAnims("anim_emotes_default_kanim", 0f);
			this.moveto.TriggerOnEnter(GameHashes.BeginWalk, null).TriggerOnExit(GameHashes.EndWalk, null).ToggleAnims("anim_loco_vomiter_kanim", 0f).MoveTo((VomitChore.StatesInstance smi) => smi.GetVomitCell(), this.vomit, this.vomit, false);
			this.vomit.DefaultState(this.vomit.buildup).ToggleAnims(new Func<VomitChore.StatesInstance, KAnimFile>(VomitChore.GetAnimFileName)).ToggleStatusItem((VomitChore.StatesInstance smi) => smi.statusItem, null).DoNotification((VomitChore.StatesInstance smi) => smi.notification).DoTutorial(Tutorial.TutorialMessages.TM_Mopping).Enter(delegate(VomitChore.StatesInstance smi)
			{
				if (smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
				{
					smi.master.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
				}
			}).Exit(delegate(VomitChore.StatesInstance smi)
			{
				smi.master.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
				float num = Mathf.Min(smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).value, 20f);
				smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).ApplyDelta(-num);
				if (num >= 1f)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Mathf.FloorToInt(num).ToString() + UI.UNITSUFFIXES.RADIATION.RADS, smi.master.transform, 1.5f, false);
				}
			});
			this.vomit.buildup.PlayAnim("vomit_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.vomit.release);
			this.vomit.release.ToggleEffect("Vomiting").PlayAnim("vomit_loop", KAnim.PlayMode.Once).Update("SpawnVomitLiquid", delegate(VomitChore.StatesInstance smi, float dt)
			{
				smi.SpawnVomitLiquid(dt, smi.elementToVomit);
			}, UpdateRate.SIM_200ms, false).OnAnimQueueComplete(this.vomit.release_pst);
			this.vomit.release_pst.PlayAnim("vomit_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.recover);
			this.recover.PlayAnim("breathe_pre").QueueAnim("breathe_loop", true, null).ScheduleGoTo(8f, this.recover_pst);
			this.recover_pst.QueueAnim("breathe_pst", false, null).OnAnimQueueComplete(this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x04001674 RID: 5748
		public StateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.TargetParameter vomiter;

		// Token: 0x04001675 RID: 5749
		public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State moveto;

		// Token: 0x04001676 RID: 5750
		public VomitChore.States.VomitState vomit;

		// Token: 0x04001677 RID: 5751
		public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State recover;

		// Token: 0x04001678 RID: 5752
		public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State recover_pst;

		// Token: 0x04001679 RID: 5753
		public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State complete;

		// Token: 0x0200077A RID: 1914
		public class VomitState : GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State
		{
			// Token: 0x0400167A RID: 5754
			public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State buildup;

			// Token: 0x0400167B RID: 5755
			public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State release;

			// Token: 0x0400167C RID: 5756
			public GameStateMachine<VomitChore.States, VomitChore.StatesInstance, VomitChore, object>.State release_pst;
		}
	}
}
