using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200183C RID: 6204
[AddComponentMenu("KMonoBehaviour/Workable/RoleStation")]
public class RoleStation : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06007F79 RID: 32633 RVA: 0x000F873C File Offset: 0x000F693C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = true;
		this.UpdateStatusItemDelegate = new Action<object>(this.UpdateSkillPointAvailableStatusItem);
	}

	// Token: 0x06007F7A RID: 32634 RVA: 0x0033C69C File Offset: 0x0033A89C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.RoleStations.Add(this);
		this.smi = new RoleStation.RoleStationSM.Instance(this);
		this.smi.StartSM();
		base.SetWorkTime(7.53f);
		this.resetProgressOnStop = true;
		this.subscriptions.Add(Game.Instance.Subscribe(-1523247426, this.UpdateStatusItemDelegate));
		this.subscriptions.Add(Game.Instance.Subscribe(1505456302, this.UpdateStatusItemDelegate));
		this.UpdateSkillPointAvailableStatusItem(null);
	}

	// Token: 0x06007F7B RID: 32635 RVA: 0x0033C72C File Offset: 0x0033A92C
	protected override void OnStopWork(WorkerBase worker)
	{
		Telepad.StatesInstance statesInstance = this.GetSMI<Telepad.StatesInstance>();
		statesInstance.sm.idlePortal.Trigger(statesInstance);
	}

	// Token: 0x06007F7C RID: 32636 RVA: 0x0033C754 File Offset: 0x0033A954
	private void UpdateSkillPointAvailableStatusItem(object data = null)
	{
		foreach (object obj in Components.MinionResumes)
		{
			MinionResume minionResume = (MinionResume)obj;
			if (!minionResume.HasTag(GameTags.Dead) && minionResume.TotalSkillPointsGained - minionResume.SkillsMastered > 0)
			{
				if (this.skillPointAvailableStatusItem == Guid.Empty)
				{
					this.skillPointAvailableStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SkillPointsAvailable, null);
				}
				return;
			}
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SkillPointsAvailable, false);
		this.skillPointAvailableStatusItem = Guid.Empty;
	}

	// Token: 0x06007F7D RID: 32637 RVA: 0x0033C820 File Offset: 0x0033AA20
	private Chore CreateWorkChore()
	{
		return new WorkChore<RoleStation>(Db.Get().ChoreTypes.LearnSkill, this, null, true, null, null, null, false, null, false, true, Assets.GetAnim("anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
	}

	// Token: 0x06007F7E RID: 32638 RVA: 0x000F875D File Offset: 0x000F695D
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		worker.GetComponent<MinionResume>().SkillLearned();
	}

	// Token: 0x06007F7F RID: 32639 RVA: 0x000F8771 File Offset: 0x000F6971
	private void OnSelectRolesClick()
	{
		DetailsScreen.Instance.Show(false);
		ManagementMenu.Instance.ToggleSkills();
	}

	// Token: 0x06007F80 RID: 32640 RVA: 0x0033C864 File Offset: 0x0033AA64
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		foreach (int id in this.subscriptions)
		{
			Game.Instance.Unsubscribe(id);
		}
		Components.RoleStations.Remove(this);
	}

	// Token: 0x06007F81 RID: 32641 RVA: 0x000F8788 File Offset: 0x000F6988
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		return base.GetDescriptors(go);
	}

	// Token: 0x040060F3 RID: 24819
	private Chore chore;

	// Token: 0x040060F4 RID: 24820
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x040060F5 RID: 24821
	[MyCmpAdd]
	private Operational operational;

	// Token: 0x040060F6 RID: 24822
	private RoleStation.RoleStationSM.Instance smi;

	// Token: 0x040060F7 RID: 24823
	private Guid skillPointAvailableStatusItem;

	// Token: 0x040060F8 RID: 24824
	private Action<object> UpdateStatusItemDelegate;

	// Token: 0x040060F9 RID: 24825
	private List<int> subscriptions = new List<int>();

	// Token: 0x0200183D RID: 6205
	public class RoleStationSM : GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation>
	{
		// Token: 0x06007F83 RID: 32643 RVA: 0x0033C8CC File Offset: 0x0033AACC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (RoleStation.RoleStationSM.Instance smi) => smi.GetComponent<Operational>().IsOperational);
			this.operational.ToggleChore((RoleStation.RoleStationSM.Instance smi) => smi.master.CreateWorkChore(), this.unoperational);
		}

		// Token: 0x040060FA RID: 24826
		public GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.State unoperational;

		// Token: 0x040060FB RID: 24827
		public GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.State operational;

		// Token: 0x0200183E RID: 6206
		public new class Instance : GameStateMachine<RoleStation.RoleStationSM, RoleStation.RoleStationSM.Instance, RoleStation, object>.GameInstance
		{
			// Token: 0x06007F85 RID: 32645 RVA: 0x000F87AC File Offset: 0x000F69AC
			public Instance(RoleStation master) : base(master)
			{
			}
		}
	}
}
