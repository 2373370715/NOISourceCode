using System;
using UnityEngine;

// Token: 0x02000679 RID: 1657
public class BeIncapacitatedChore : Chore<BeIncapacitatedChore.StatesInstance>
{
	// Token: 0x06001D88 RID: 7560 RVA: 0x001BB910 File Offset: 0x001B9B10
	public void FindAvailableMedicalBed(Navigator navigator)
	{
		Clinic clinic = null;
		AssignableSlot clinic2 = Db.Get().AssignableSlots.Clinic;
		Ownables soleOwner = this.gameObject.GetComponent<MinionIdentity>().GetSoleOwner();
		AssignableSlotInstance slot = soleOwner.GetSlot(clinic2);
		if (slot.assignable == null)
		{
			Assignable assignable = soleOwner.AutoAssignSlot(clinic2);
			if (assignable != null)
			{
				clinic = assignable.GetComponent<Clinic>();
			}
		}
		else
		{
			clinic = slot.assignable.GetComponent<Clinic>();
		}
		if (clinic != null && navigator.CanReach(clinic))
		{
			base.smi.sm.clinic.Set(clinic.gameObject, base.smi, false);
			base.smi.GoTo(base.smi.sm.incapacitation_root.rescue.waitingForPickup);
		}
	}

	// Token: 0x06001D89 RID: 7561 RVA: 0x000B7F16 File Offset: 0x000B6116
	public GameObject GetChosenClinic()
	{
		return base.smi.sm.clinic.Get(base.smi);
	}

	// Token: 0x06001D8A RID: 7562 RVA: 0x001BB9DC File Offset: 0x001B9BDC
	public BeIncapacitatedChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.BeIncapacitated, master, master.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BeIncapacitatedChore.StatesInstance(this);
	}

	// Token: 0x040012C5 RID: 4805
	private static string IncapacitatedDuplicantAnim_pre = "incapacitate_pre";

	// Token: 0x040012C6 RID: 4806
	private static string IncapacitatedDuplicantAnim_loop = "incapacitate_loop";

	// Token: 0x040012C7 RID: 4807
	private static string IncapacitatedDuplicantAnim_death = "incapacitate_death";

	// Token: 0x040012C8 RID: 4808
	private static string IncapacitatedDuplicantAnim_carry = "carry_loop";

	// Token: 0x040012C9 RID: 4809
	private static string IncapacitatedDuplicantAnim_place = "place";

	// Token: 0x0200067A RID: 1658
	public class StatesInstance : GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.GameInstance
	{
		// Token: 0x06001D8C RID: 7564 RVA: 0x000B7F67 File Offset: 0x000B6167
		public StatesInstance(BeIncapacitatedChore master) : base(master)
		{
		}
	}

	// Token: 0x0200067B RID: 1659
	public class States : GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore>
	{
		// Token: 0x06001D8D RID: 7565 RVA: 0x001BBA20 File Offset: 0x001B9C20
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAnims("anim_incapacitated_kanim", 0f).ToggleStatusItem(Db.Get().DuplicantStatusItems.Incapacitated, (BeIncapacitatedChore.StatesInstance smi) => smi.master.gameObject.GetSMI<IncapacitationMonitor.Instance>()).Enter(delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.GoTo(this.incapacitation_root.lookingForBed);
			});
			this.incapacitation_root.EventHandler(GameHashes.Died, delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.StopSM("died");
			});
			this.incapacitation_root.lookingForBed.Update("LookForAvailableClinic", delegate(BeIncapacitatedChore.StatesInstance smi, float dt)
			{
				smi.master.FindAvailableMedicalBed(smi.master.GetComponent<Navigator>());
			}, UpdateRate.SIM_1000ms, false).Enter("PlayAnim", delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.sm.clinic.Set(null, smi);
				smi.Play(BeIncapacitatedChore.IncapacitatedDuplicantAnim_pre, KAnim.PlayMode.Once);
				smi.Queue(BeIncapacitatedChore.IncapacitatedDuplicantAnim_loop, KAnim.PlayMode.Loop);
			});
			this.incapacitation_root.rescue.ToggleChore((BeIncapacitatedChore.StatesInstance smi) => new RescueIncapacitatedChore(smi.master, this.masterTarget.Get(smi)), this.incapacitation_root.recovering, this.incapacitation_root.lookingForBed);
			this.incapacitation_root.rescue.waitingForPickup.EventTransition(GameHashes.OnStore, this.incapacitation_root.rescue.carried, null).Update("LookForAvailableClinic", delegate(BeIncapacitatedChore.StatesInstance smi, float dt)
			{
				bool flag = false;
				if (smi.sm.clinic.Get(smi) == null)
				{
					flag = true;
				}
				else if (!smi.master.gameObject.GetComponent<Navigator>().CanReach(this.clinic.Get(smi).GetComponent<Clinic>()))
				{
					flag = true;
				}
				else if (!this.clinic.Get(smi).GetComponent<Assignable>().IsAssignedTo(smi.master.GetComponent<IAssignableIdentity>()))
				{
					flag = true;
				}
				if (flag)
				{
					smi.GoTo(this.incapacitation_root.lookingForBed);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.incapacitation_root.rescue.carried.Update("LookForAvailableClinic", delegate(BeIncapacitatedChore.StatesInstance smi, float dt)
			{
				bool flag = false;
				if (smi.sm.clinic.Get(smi) == null)
				{
					flag = true;
				}
				else if (!this.clinic.Get(smi).GetComponent<Assignable>().IsAssignedTo(smi.master.GetComponent<IAssignableIdentity>()))
				{
					flag = true;
				}
				if (flag)
				{
					smi.GoTo(this.incapacitation_root.lookingForBed);
				}
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.Queue(BeIncapacitatedChore.IncapacitatedDuplicantAnim_carry, KAnim.PlayMode.Loop);
			}).Exit(delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.Play(BeIncapacitatedChore.IncapacitatedDuplicantAnim_place, KAnim.PlayMode.Once);
			});
			this.incapacitation_root.death.PlayAnim(BeIncapacitatedChore.IncapacitatedDuplicantAnim_death).Enter(delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.StopSM("died");
			});
			this.incapacitation_root.recovering.ToggleUrge(Db.Get().Urges.HealCritical).Enter(delegate(BeIncapacitatedChore.StatesInstance smi)
			{
				smi.Trigger(-1256572400, null);
				smi.SetStatus(StateMachine.Status.Success);
				smi.StopSM("recovering");
			});
		}

		// Token: 0x040012CA RID: 4810
		public BeIncapacitatedChore.States.IncapacitatedStates incapacitation_root;

		// Token: 0x040012CB RID: 4811
		public StateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.TargetParameter clinic;

		// Token: 0x0200067C RID: 1660
		public class IncapacitatedStates : GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State
		{
			// Token: 0x040012CC RID: 4812
			public GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State lookingForBed;

			// Token: 0x040012CD RID: 4813
			public BeIncapacitatedChore.States.BeingRescued rescue;

			// Token: 0x040012CE RID: 4814
			public GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State death;

			// Token: 0x040012CF RID: 4815
			public GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State recovering;
		}

		// Token: 0x0200067D RID: 1661
		public class BeingRescued : GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State
		{
			// Token: 0x040012D0 RID: 4816
			public GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State waitingForPickup;

			// Token: 0x040012D1 RID: 4817
			public GameStateMachine<BeIncapacitatedChore.States, BeIncapacitatedChore.StatesInstance, BeIncapacitatedChore, object>.State carried;
		}
	}
}
