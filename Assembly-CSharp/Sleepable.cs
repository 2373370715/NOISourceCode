using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x020018D4 RID: 6356
[AddComponentMenu("KMonoBehaviour/Workable/Sleepable")]
public class Sleepable : Workable
{
	// Token: 0x0600836B RID: 33643 RVA: 0x000FAE34 File Offset: 0x000F9034
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
		this.workerStatusItem = null;
		this.synchronizeAnims = false;
		this.triggerWorkReactions = false;
		this.lightEfficiencyBonus = false;
		this.approachable = base.GetComponent<IApproachable>();
	}

	// Token: 0x0600836C RID: 33644 RVA: 0x000FAE73 File Offset: 0x000F9073
	protected override void OnSpawn()
	{
		if (this.isNormalBed)
		{
			Components.NormalBeds.Add(base.gameObject.GetMyWorldId(), this);
		}
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x0600836D RID: 33645 RVA: 0x0034F024 File Offset: 0x0034D224
	public override HashedString[] GetWorkAnims(WorkerBase worker)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return Sleepable.hatWorkAnims;
		}
		return Sleepable.normalWorkAnims;
	}

	// Token: 0x0600836E RID: 33646 RVA: 0x0034F064 File Offset: 0x0034D264
	public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return Sleepable.hatWorkPstAnim;
		}
		return Sleepable.normalWorkPstAnim;
	}

	// Token: 0x0600836F RID: 33647 RVA: 0x0034F0A4 File Offset: 0x0034D2A4
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		KAnimControllerBase animController = this.GetAnimController();
		if (animController != null)
		{
			animController.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
			animController.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
		}
		base.Subscribe(worker.gameObject, -1142962013, new Action<object>(this.PlayPstAnim));
		if (this.operational != null)
		{
			this.operational.SetActive(true, false);
		}
		worker.Trigger(-1283701846, this);
		worker.GetComponent<Effects>().Add(this.effectName, false);
		this.isDoneSleeping = false;
	}

	// Token: 0x06008370 RID: 33648 RVA: 0x0034F160 File Offset: 0x0034D360
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.isDoneSleeping)
		{
			return Time.time > this.wakeTime;
		}
		if (this.Dreamable != null && !this.Dreamable.DreamIsDisturbed)
		{
			this.Dreamable.WorkTick(worker, dt);
		}
		if (worker.GetSMI<StaminaMonitor.Instance>().ShouldExitSleep())
		{
			this.isDoneSleeping = true;
			this.wakeTime = Time.time + UnityEngine.Random.value * 3f;
		}
		return false;
	}

	// Token: 0x06008371 RID: 33649 RVA: 0x0034F1D8 File Offset: 0x0034D3D8
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.operational != null)
		{
			this.operational.SetActive(false, false);
		}
		base.Unsubscribe(worker.gameObject, -1142962013, new Action<object>(this.PlayPstAnim));
		if (worker != null)
		{
			Effects component = worker.GetComponent<Effects>();
			component.Remove(this.effectName);
			if (this.wakeEffects != null)
			{
				foreach (string effect_id in this.wakeEffects)
				{
					component.Add(effect_id, true);
				}
			}
			if (this.stretchOnWake && UnityEngine.Random.value < 0.33f)
			{
				new EmoteChore(worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, Db.Get().Emotes.Minion.MorningStretch, 1, null);
			}
			if (worker.GetAmounts().Get(Db.Get().Amounts.Stamina).value < worker.GetAmounts().Get(Db.Get().Amounts.Stamina).GetMax())
			{
				worker.Trigger(1338475637, this);
			}
		}
	}

	// Token: 0x06008372 RID: 33650 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x06008373 RID: 33651 RVA: 0x000FAE9E File Offset: 0x000F909E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.isNormalBed)
		{
			Components.NormalBeds.Remove(base.gameObject.GetMyWorldId(), this);
		}
	}

	// Token: 0x06008374 RID: 33652 RVA: 0x0034F324 File Offset: 0x0034D524
	private void PlayPstAnim(object data)
	{
		WorkerBase workerBase = (WorkerBase)data;
		if (workerBase != null && workerBase.GetWorkable() != null)
		{
			KAnimControllerBase component = workerBase.GetWorkable().gameObject.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				component.Play("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x04006418 RID: 25624
	private const float STRECH_CHANCE = 0.33f;

	// Token: 0x04006419 RID: 25625
	[MyCmpGet]
	public Assignable assignable;

	// Token: 0x0400641A RID: 25626
	public IApproachable approachable;

	// Token: 0x0400641B RID: 25627
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400641C RID: 25628
	public string effectName = "Sleep";

	// Token: 0x0400641D RID: 25629
	public List<string> wakeEffects;

	// Token: 0x0400641E RID: 25630
	public bool stretchOnWake = true;

	// Token: 0x0400641F RID: 25631
	private float wakeTime;

	// Token: 0x04006420 RID: 25632
	private bool isDoneSleeping;

	// Token: 0x04006421 RID: 25633
	public bool isNormalBed = true;

	// Token: 0x04006422 RID: 25634
	public ClinicDreamable Dreamable;

	// Token: 0x04006423 RID: 25635
	private static readonly HashedString[] normalWorkAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04006424 RID: 25636
	private static readonly HashedString[] hatWorkAnims = new HashedString[]
	{
		"hat_pre",
		"working_loop"
	};

	// Token: 0x04006425 RID: 25637
	private static readonly HashedString[] normalWorkPstAnim = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x04006426 RID: 25638
	private static readonly HashedString[] hatWorkPstAnim = new HashedString[]
	{
		"hat_pst"
	};
}
