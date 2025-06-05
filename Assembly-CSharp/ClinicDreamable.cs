using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020009DD RID: 2525
[AddComponentMenu("KMonoBehaviour/Workable/Clinic Dreamable")]
public class ClinicDreamable : Workable
{
	// Token: 0x170001BA RID: 442
	// (get) Token: 0x06002DC3 RID: 11715 RVA: 0x000C2144 File Offset: 0x000C0344
	// (set) Token: 0x06002DC4 RID: 11716 RVA: 0x000C214C File Offset: 0x000C034C
	public bool DreamIsDisturbed { get; private set; }

	// Token: 0x06002DC5 RID: 11717 RVA: 0x000C2155 File Offset: 0x000C0355
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.resetProgressOnStop = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Dreaming;
		this.workingStatusItem = null;
	}

	// Token: 0x06002DC6 RID: 11718 RVA: 0x001FF290 File Offset: 0x001FD490
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (ClinicDreamable.dreamJournalPrefab == null)
		{
			ClinicDreamable.dreamJournalPrefab = Assets.GetPrefab(DreamJournalConfig.ID);
			ClinicDreamable.sleepClinic = Db.Get().effects.Get("SleepClinic");
		}
		this.equippable = base.GetComponent<Equippable>();
		global::Debug.Assert(this.equippable != null);
		EquipmentDef def = this.equippable.def;
		def.OnEquipCallBack = (Action<Equippable>)Delegate.Combine(def.OnEquipCallBack, new Action<Equippable>(this.OnEquipPajamas));
		EquipmentDef def2 = this.equippable.def;
		def2.OnUnequipCallBack = (Action<Equippable>)Delegate.Combine(def2.OnUnequipCallBack, new Action<Equippable>(this.OnUnequipPajamas));
		this.OnEquipPajamas(this.equippable);
	}

	// Token: 0x06002DC7 RID: 11719 RVA: 0x001FF35C File Offset: 0x001FD55C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.equippable == null)
		{
			return;
		}
		EquipmentDef def = this.equippable.def;
		def.OnEquipCallBack = (Action<Equippable>)Delegate.Remove(def.OnEquipCallBack, new Action<Equippable>(this.OnEquipPajamas));
		EquipmentDef def2 = this.equippable.def;
		def2.OnUnequipCallBack = (Action<Equippable>)Delegate.Remove(def2.OnUnequipCallBack, new Action<Equippable>(this.OnUnequipPajamas));
	}

	// Token: 0x06002DC8 RID: 11720 RVA: 0x001FF3D8 File Offset: 0x001FD5D8
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.GetPercentComplete() >= 1f)
		{
			Vector3 position = this.dreamer.transform.position;
			position.y += 1f;
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			Util.KInstantiate(ClinicDreamable.dreamJournalPrefab, position, Quaternion.identity, null, null, true, 0).SetActive(true);
			this.workTimeRemaining = this.GetWorkTime();
		}
		return false;
	}

	// Token: 0x06002DC9 RID: 11721 RVA: 0x001FF450 File Offset: 0x001FD650
	public void OnEquipPajamas(Equippable eq)
	{
		if (this.equippable == null || this.equippable != eq)
		{
			return;
		}
		MinionAssignablesProxy minionAssignablesProxy = this.equippable.assignee as MinionAssignablesProxy;
		if (minionAssignablesProxy == null)
		{
			return;
		}
		if (minionAssignablesProxy.target is StoredMinionIdentity)
		{
			return;
		}
		GameObject targetGameObject = minionAssignablesProxy.GetTargetGameObject();
		this.effects = targetGameObject.GetComponent<Effects>();
		this.dreamer = targetGameObject.GetComponent<ChoreDriver>();
		this.selectable = targetGameObject.GetComponent<KSelectable>();
		this.dreamer.Subscribe(-1283701846, new Action<object>(this.WorkerStartedSleeping));
		this.dreamer.Subscribe(-2090444759, new Action<object>(this.WorkerStoppedSleeping));
		this.effects.Add(ClinicDreamable.sleepClinic, true);
		this.selectable.AddStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Wearing, null);
	}

	// Token: 0x06002DCA RID: 11722 RVA: 0x001FF538 File Offset: 0x001FD738
	public void OnUnequipPajamas(Equippable eq)
	{
		if (this.dreamer == null)
		{
			return;
		}
		if (this.equippable == null || this.equippable != eq)
		{
			return;
		}
		this.dreamer.Unsubscribe(-1283701846, new Action<object>(this.WorkerStartedSleeping));
		this.dreamer.Unsubscribe(-2090444759, new Action<object>(this.WorkerStoppedSleeping));
		this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Wearing, false);
		this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Sleeping, false);
		this.effects.Remove(ClinicDreamable.sleepClinic.Id);
		this.StopDreamingThought();
		this.dreamer = null;
		this.selectable = null;
		this.effects = null;
	}

	// Token: 0x06002DCB RID: 11723 RVA: 0x001FF614 File Offset: 0x001FD814
	public void WorkerStartedSleeping(object data)
	{
		SleepChore sleepChore = this.dreamer.GetCurrentChore() as SleepChore;
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context = sleepChore.smi.sm.isDisturbedByLight.GetContext(sleepChore.smi);
		context.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context2 = sleepChore.smi.sm.isDisturbedByMovement.GetContext(sleepChore.smi);
		context2.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context2.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context3 = sleepChore.smi.sm.isDisturbedByNoise.GetContext(sleepChore.smi);
		context3.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context3.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
		StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context4 = sleepChore.smi.sm.isScaredOfDark.GetContext(sleepChore.smi);
		context4.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Combine(context4.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
		this.sleepable = (data as Sleepable);
		this.sleepable.Dreamable = this;
		base.StartWork(this.sleepable.worker);
		this.progressBar.Retarget(this.sleepable.gameObject);
		this.selectable.AddStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Sleeping, this);
		this.StartDreamingThought();
	}

	// Token: 0x06002DCC RID: 11724 RVA: 0x001FF784 File Offset: 0x001FD984
	public void WorkerStoppedSleeping(object data)
	{
		this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.MegaBrainTank_Pajamas_Sleeping, false);
		SleepChore sleepChore = this.dreamer.GetCurrentChore() as SleepChore;
		if (!sleepChore.IsNullOrDestroyed() && !sleepChore.smi.IsNullOrDestroyed() && !sleepChore.smi.sm.IsNullOrDestroyed())
		{
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context = sleepChore.smi.sm.isDisturbedByLight.GetContext(sleepChore.smi);
			context.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context2 = sleepChore.smi.sm.isDisturbedByMovement.GetContext(sleepChore.smi);
			context2.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context2.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context3 = sleepChore.smi.sm.isDisturbedByNoise.GetContext(sleepChore.smi);
			context3.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context3.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
			StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.Parameter<bool>.Context context4 = sleepChore.smi.sm.isScaredOfDark.GetContext(sleepChore.smi);
			context4.onDirty = (Action<SleepChore.StatesInstance>)Delegate.Remove(context4.onDirty, new Action<SleepChore.StatesInstance>(this.OnSleepDisturbed));
		}
		this.StopDreamingThought();
		this.DreamIsDisturbed = false;
		if (base.worker != null)
		{
			base.StopWork(base.worker, false);
		}
		if (this.sleepable != null)
		{
			this.sleepable.Dreamable = null;
			this.sleepable = null;
		}
	}

	// Token: 0x06002DCD RID: 11725 RVA: 0x001FF928 File Offset: 0x001FDB28
	private void OnSleepDisturbed(SleepChore.StatesInstance smi)
	{
		SleepChore sleepChore = this.dreamer.GetCurrentChore() as SleepChore;
		bool flag = sleepChore.smi.sm.isDisturbedByLight.Get(sleepChore.smi);
		flag |= sleepChore.smi.sm.isDisturbedByMovement.Get(sleepChore.smi);
		flag |= sleepChore.smi.sm.isDisturbedByNoise.Get(sleepChore.smi);
		flag |= sleepChore.smi.sm.isScaredOfDark.Get(sleepChore.smi);
		this.DreamIsDisturbed = flag;
		if (flag)
		{
			this.StopDreamingThought();
		}
	}

	// Token: 0x06002DCE RID: 11726 RVA: 0x001FF9CC File Offset: 0x001FDBCC
	private void StartDreamingThought()
	{
		if (this.dreamer != null && !this.HasStartedThoughts_Dreaming)
		{
			this.dreamer.GetSMI<Dreamer.Instance>().SetDream(Db.Get().Dreams.CommonDream);
			this.dreamer.GetSMI<Dreamer.Instance>().StartDreaming();
			this.HasStartedThoughts_Dreaming = true;
		}
	}

	// Token: 0x06002DCF RID: 11727 RVA: 0x000C2180 File Offset: 0x000C0380
	private void StopDreamingThought()
	{
		if (this.dreamer != null && this.HasStartedThoughts_Dreaming)
		{
			this.dreamer.GetSMI<Dreamer.Instance>().StopDreaming();
			this.HasStartedThoughts_Dreaming = false;
		}
	}

	// Token: 0x04001F59 RID: 8025
	private static GameObject dreamJournalPrefab;

	// Token: 0x04001F5A RID: 8026
	private static Effect sleepClinic;

	// Token: 0x04001F5B RID: 8027
	public bool HasStartedThoughts_Dreaming;

	// Token: 0x04001F5D RID: 8029
	private ChoreDriver dreamer;

	// Token: 0x04001F5E RID: 8030
	private Equippable equippable;

	// Token: 0x04001F5F RID: 8031
	private Effects effects;

	// Token: 0x04001F60 RID: 8032
	private Sleepable sleepable;

	// Token: 0x04001F61 RID: 8033
	private KSelectable selectable;

	// Token: 0x04001F62 RID: 8034
	private HashedString dreamAnimName = "portal rocket comp";
}
