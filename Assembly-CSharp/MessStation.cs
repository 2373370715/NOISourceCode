using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000EE1 RID: 3809
[AddComponentMenu("KMonoBehaviour/Workable/MessStation")]
public class MessStation : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06004C49 RID: 19529 RVA: 0x000D5A5F File Offset: 0x000D3C5F
	protected override void OnPrefabInit()
	{
		this.ownable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.HasCaloriesOwnablePrecondition));
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_machine_kanim")
		};
	}

	// Token: 0x06004C4A RID: 19530 RVA: 0x0026FC04 File Offset: 0x0026DE04
	private bool HasCaloriesOwnablePrecondition(MinionAssignablesProxy worker)
	{
		bool result = false;
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			result = (Db.Get().Amounts.Calories.Lookup(minionIdentity) != null);
		}
		return result;
	}

	// Token: 0x06004C4B RID: 19531 RVA: 0x000D5A9C File Offset: 0x000D3C9C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		worker.GetWorkable().GetComponent<Edible>().CompleteWork(worker);
	}

	// Token: 0x06004C4C RID: 19532 RVA: 0x000D5AAF File Offset: 0x000D3CAF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new MessStation.MessStationSM.Instance(this);
		this.smi.StartSM();
	}

	// Token: 0x06004C4D RID: 19533 RVA: 0x0026FC44 File Offset: 0x0026DE44
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (go.GetComponent<Storage>().Has(TableSaltConfig.ID.ToTag()))
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.MESS_TABLE_SALT, TableSaltTuning.MORALE_MODIFIER), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.MESS_TABLE_SALT, TableSaltTuning.MORALE_MODIFIER), Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x06004C4E RID: 19534 RVA: 0x000D5ACE File Offset: 0x000D3CCE
	public bool HasSalt
	{
		get
		{
			return this.smi.HasSalt;
		}
	}

	// Token: 0x0400356E RID: 13678
	[MyCmpGet]
	private Ownable ownable;

	// Token: 0x0400356F RID: 13679
	private MessStation.MessStationSM.Instance smi;

	// Token: 0x02000EE2 RID: 3810
	public class MessStationSM : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation>
	{
		// Token: 0x06004C50 RID: 19536 RVA: 0x0026FCB0 File Offset: 0x0026DEB0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.salt.none;
			this.salt.none.Transition(this.salt.salty, (MessStation.MessStationSM.Instance smi) => smi.HasSalt, UpdateRate.SIM_200ms).PlayAnim("off");
			this.salt.salty.Transition(this.salt.none, (MessStation.MessStationSM.Instance smi) => !smi.HasSalt, UpdateRate.SIM_200ms).PlayAnim("salt").EventTransition(GameHashes.EatStart, this.eating, null);
			this.eating.Transition(this.salt.salty, (MessStation.MessStationSM.Instance smi) => smi.HasSalt && !smi.IsEating(), UpdateRate.SIM_200ms).Transition(this.salt.none, (MessStation.MessStationSM.Instance smi) => !smi.HasSalt && !smi.IsEating(), UpdateRate.SIM_200ms).PlayAnim("off");
		}

		// Token: 0x04003570 RID: 13680
		public MessStation.MessStationSM.SaltState salt;

		// Token: 0x04003571 RID: 13681
		public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State eating;

		// Token: 0x02000EE3 RID: 3811
		public class SaltState : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State
		{
			// Token: 0x04003572 RID: 13682
			public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State none;

			// Token: 0x04003573 RID: 13683
			public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State salty;
		}

		// Token: 0x02000EE4 RID: 3812
		public new class Instance : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.GameInstance
		{
			// Token: 0x06004C53 RID: 19539 RVA: 0x000D5AEB File Offset: 0x000D3CEB
			public Instance(MessStation master) : base(master)
			{
				this.saltStorage = master.GetComponent<Storage>();
				this.assigned = master.GetComponent<Assignable>();
			}

			// Token: 0x17000437 RID: 1079
			// (get) Token: 0x06004C54 RID: 19540 RVA: 0x000D5B0C File Offset: 0x000D3D0C
			public bool HasSalt
			{
				get
				{
					return this.saltStorage.Has(TableSaltConfig.ID.ToTag());
				}
			}

			// Token: 0x06004C55 RID: 19541 RVA: 0x0026FDD8 File Offset: 0x0026DFD8
			public bool IsEating()
			{
				if (this.assigned == null || this.assigned.assignee == null)
				{
					return false;
				}
				Ownables soleOwner = this.assigned.assignee.GetSoleOwner();
				if (soleOwner == null)
				{
					return false;
				}
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject == null)
				{
					return false;
				}
				ChoreDriver component = targetGameObject.GetComponent<ChoreDriver>();
				return component != null && component.HasChore() && component.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
			}

			// Token: 0x04003574 RID: 13684
			private Storage saltStorage;

			// Token: 0x04003575 RID: 13685
			private Assignable assigned;
		}
	}
}
