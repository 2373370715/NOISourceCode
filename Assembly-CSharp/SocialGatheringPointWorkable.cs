using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020018E3 RID: 6371
[AddComponentMenu("KMonoBehaviour/Workable/SocialGatheringPointWorkable")]
public class SocialGatheringPointWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x060083C7 RID: 33735 RVA: 0x000B09A4 File Offset: 0x000AEBA4
	private SocialGatheringPointWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x060083C8 RID: 33736 RVA: 0x0034FD70 File Offset: 0x0034DF70
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_generic_convo_kanim")
		};
		this.workAnims = new HashedString[]
		{
			"idle"
		};
		this.faceTargetWhenWorking = true;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Socializing;
		this.synchronizeAnims = false;
		this.showProgressBar = false;
		this.resetProgressOnStop = true;
		this.lightEfficiencyBonus = false;
	}

	// Token: 0x060083C9 RID: 33737 RVA: 0x000FB30A File Offset: 0x000F950A
	public override Vector3 GetFacingTarget()
	{
		if (this.lastTalker != null)
		{
			return this.lastTalker.transform.GetPosition();
		}
		return base.GetFacingTarget();
	}

	// Token: 0x060083CA RID: 33738 RVA: 0x0034FDF8 File Offset: 0x0034DFF8
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (!worker.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation))
		{
			Effects component = worker.GetComponent<Effects>();
			if (string.IsNullOrEmpty(this.specificEffect) || component.HasEffect(this.specificEffect))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060083CB RID: 33739 RVA: 0x0034FE48 File Offset: 0x0034E048
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		worker.Subscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Subscribe(25860745, new Action<object>(this.OnStoppedTalking));
		this.timesConversed = 0;
	}

	// Token: 0x060083CC RID: 33740 RVA: 0x0034FEA4 File Offset: 0x0034E0A4
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
		worker.Unsubscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Unsubscribe(25860745, new Action<object>(this.OnStoppedTalking));
	}

	// Token: 0x060083CD RID: 33741 RVA: 0x0034FEF8 File Offset: 0x0034E0F8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.timesConversed > 0)
		{
			Effects component = worker.GetComponent<Effects>();
			if (!string.IsNullOrEmpty(this.specificEffect))
			{
				component.Add(this.specificEffect, true);
			}
		}
	}

	// Token: 0x060083CE RID: 33742 RVA: 0x0034FF30 File Offset: 0x0034E130
	private void OnStartedTalking(object data)
	{
		ConversationManager.StartedTalkingEvent startedTalkingEvent = data as ConversationManager.StartedTalkingEvent;
		if (startedTalkingEvent == null)
		{
			return;
		}
		GameObject talker = startedTalkingEvent.talker;
		if (talker == base.worker.gameObject)
		{
			KBatchedAnimController component = base.worker.GetComponent<KBatchedAnimController>();
			string text = startedTalkingEvent.anim;
			text += UnityEngine.Random.Range(1, 9).ToString();
			component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("idle", KAnim.PlayMode.Loop, 1f, 0f);
		}
		else
		{
			base.worker.GetComponent<Facing>().Face(talker.transform.GetPosition());
			this.lastTalker = talker;
		}
		this.timesConversed++;
	}

	// Token: 0x060083CF RID: 33743 RVA: 0x000AA038 File Offset: 0x000A8238
	private void OnStoppedTalking(object data)
	{
	}

	// Token: 0x060083D0 RID: 33744 RVA: 0x000FB331 File Offset: 0x000F9531
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		if (!string.IsNullOrEmpty(this.specificEffect) && worker.GetComponent<Effects>().HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x0400645B RID: 25691
	private GameObject lastTalker;

	// Token: 0x0400645C RID: 25692
	public int basePriority;

	// Token: 0x0400645D RID: 25693
	public string specificEffect;

	// Token: 0x0400645E RID: 25694
	public int timesConversed;
}
