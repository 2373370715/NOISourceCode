using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001628 RID: 5672
public class SicknessMonitor : GameStateMachine<SicknessMonitor, SicknessMonitor.Instance>
{
	// Token: 0x06007561 RID: 30049 RVA: 0x00315040 File Offset: 0x00313240
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		default_state = this.healthy;
		this.healthy.EventTransition(GameHashes.SicknessAdded, this.sick, (SicknessMonitor.Instance smi) => smi.IsSick());
		this.sick.DefaultState(this.sick.minor).EventTransition(GameHashes.SicknessCured, this.post_nocheer, (SicknessMonitor.Instance smi) => !smi.IsSick()).ToggleThought(Db.Get().Thoughts.GotInfected, null);
		this.sick.minor.EventTransition(GameHashes.SicknessAdded, this.sick.major, (SicknessMonitor.Instance smi) => smi.HasMajorDisease());
		this.sick.major.EventTransition(GameHashes.SicknessCured, this.sick.minor, (SicknessMonitor.Instance smi) => !smi.HasMajorDisease()).ToggleUrge(Db.Get().Urges.RestDueToDisease).Update("AutoAssignClinic", delegate(SicknessMonitor.Instance smi, float dt)
		{
			smi.AutoAssignClinic();
		}, UpdateRate.SIM_4000ms, false).Exit(delegate(SicknessMonitor.Instance smi)
		{
			smi.UnassignClinic();
		});
		this.post_nocheer.Enter(delegate(SicknessMonitor.Instance smi)
		{
			new SicknessCuredFX.Instance(smi.master, new Vector3(0f, 0f, -0.1f)).StartSM();
			if (smi.IsSleepingOrSleepSchedule())
			{
				smi.GoTo(this.healthy);
				return;
			}
			smi.GoTo(this.post);
		});
		this.post.ToggleChore((SicknessMonitor.Instance smi) => new EmoteChore(smi.master, Db.Get().ChoreTypes.EmoteHighPriority, SicknessMonitor.SickPostKAnim, SicknessMonitor.SickPostAnims, KAnim.PlayMode.Once, false), this.healthy);
	}

	// Token: 0x0400582F RID: 22575
	public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State healthy;

	// Token: 0x04005830 RID: 22576
	public SicknessMonitor.SickStates sick;

	// Token: 0x04005831 RID: 22577
	public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State post;

	// Token: 0x04005832 RID: 22578
	public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State post_nocheer;

	// Token: 0x04005833 RID: 22579
	private static readonly HashedString SickPostKAnim = "anim_cheer_kanim";

	// Token: 0x04005834 RID: 22580
	private static readonly HashedString[] SickPostAnims = new HashedString[]
	{
		"cheer_pre",
		"cheer_loop",
		"cheer_pst"
	};

	// Token: 0x02001629 RID: 5673
	public class SickStates : GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04005835 RID: 22581
		public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State minor;

		// Token: 0x04005836 RID: 22582
		public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State major;
	}

	// Token: 0x0200162A RID: 5674
	public new class Instance : GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007566 RID: 30054 RVA: 0x000F19EE File Offset: 0x000EFBEE
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.sicknesses = master.GetComponent<MinionModifiers>().sicknesses;
		}

		// Token: 0x06007567 RID: 30055 RVA: 0x000F1A08 File Offset: 0x000EFC08
		private string OnGetToolTip(List<Notification> notifications, object data)
		{
			return DUPLICANTS.STATUSITEMS.HASDISEASE.TOOLTIP;
		}

		// Token: 0x06007568 RID: 30056 RVA: 0x000F1A14 File Offset: 0x000EFC14
		public bool IsSick()
		{
			return this.sicknesses.Count > 0;
		}

		// Token: 0x06007569 RID: 30057 RVA: 0x003152CC File Offset: 0x003134CC
		public bool HasMajorDisease()
		{
			using (IEnumerator<SicknessInstance> enumerator = this.sicknesses.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.modifier.severity >= Sickness.Severity.Major)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600756A RID: 30058 RVA: 0x00315328 File Offset: 0x00313528
		public void AutoAssignClinic()
		{
			Ownables soleOwner = base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().GetSoleOwner();
			AssignableSlot clinic = Db.Get().AssignableSlots.Clinic;
			AssignableSlotInstance slot = soleOwner.GetSlot(clinic);
			if (slot == null)
			{
				return;
			}
			if (slot.assignable != null)
			{
				return;
			}
			soleOwner.AutoAssignSlot(clinic);
		}

		// Token: 0x0600756B RID: 30059 RVA: 0x0031538C File Offset: 0x0031358C
		public void UnassignClinic()
		{
			Assignables soleOwner = base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().GetSoleOwner();
			AssignableSlot clinic = Db.Get().AssignableSlots.Clinic;
			AssignableSlotInstance slot = soleOwner.GetSlot(clinic);
			if (slot != null)
			{
				slot.Unassign(true);
			}
		}

		// Token: 0x0600756C RID: 30060 RVA: 0x003153DC File Offset: 0x003135DC
		public bool IsSleepingOrSleepSchedule()
		{
			Schedulable component = base.GetComponent<Schedulable>();
			if (component != null && component.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep))
			{
				return true;
			}
			KPrefabID component2 = base.GetComponent<KPrefabID>();
			return component2 != null && component2.HasTag(GameTags.Asleep);
		}

		// Token: 0x04005837 RID: 22583
		private Sicknesses sicknesses;
	}
}
