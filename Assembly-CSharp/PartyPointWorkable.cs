using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020016EA RID: 5866
public class PartyPointWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06007900 RID: 30976 RVA: 0x000B09A4 File Offset: 0x000AEBA4
	private PartyPointWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06007901 RID: 30977 RVA: 0x00321B30 File Offset: 0x0031FD30
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_generic_convo_kanim")
		};
		this.workAnimPlayMode = KAnim.PlayMode.Loop;
		this.faceTargetWhenWorking = true;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Socializing;
		this.synchronizeAnims = false;
		this.showProgressBar = false;
		this.resetProgressOnStop = true;
		this.lightEfficiencyBonus = false;
		if (UnityEngine.Random.Range(0f, 100f) > 80f)
		{
			this.activity = PartyPointWorkable.ActivityType.Dance;
		}
		else
		{
			this.activity = PartyPointWorkable.ActivityType.Talk;
		}
		PartyPointWorkable.ActivityType activityType = this.activity;
		if (activityType == PartyPointWorkable.ActivityType.Talk)
		{
			this.workAnims = new HashedString[]
			{
				"idle"
			};
			this.workerOverrideAnims = new KAnimFile[][]
			{
				new KAnimFile[]
				{
					Assets.GetAnim("anim_generic_convo_kanim")
				}
			};
			return;
		}
		if (activityType != PartyPointWorkable.ActivityType.Dance)
		{
			return;
		}
		this.workAnims = new HashedString[]
		{
			"working_loop"
		};
		this.workerOverrideAnims = new KAnimFile[][]
		{
			new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_phonobox_danceone_kanim")
			},
			new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_phonobox_dancetwo_kanim")
			},
			new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_phonobox_dancethree_kanim")
			}
		};
	}

	// Token: 0x06007902 RID: 30978 RVA: 0x00321C94 File Offset: 0x0031FE94
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		int num = UnityEngine.Random.Range(0, this.workerOverrideAnims.Length);
		this.overrideAnims = this.workerOverrideAnims[num];
		return base.GetAnim(worker);
	}

	// Token: 0x06007903 RID: 30979 RVA: 0x000F3FC5 File Offset: 0x000F21C5
	public override Vector3 GetFacingTarget()
	{
		if (this.lastTalker != null)
		{
			return this.lastTalker.transform.GetPosition();
		}
		return base.GetFacingTarget();
	}

	// Token: 0x06007904 RID: 30980 RVA: 0x000B1628 File Offset: 0x000AF828
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		return false;
	}

	// Token: 0x06007905 RID: 30981 RVA: 0x00321CC8 File Offset: 0x0031FEC8
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		worker.Subscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Subscribe(25860745, new Action<object>(this.OnStoppedTalking));
	}

	// Token: 0x06007906 RID: 30982 RVA: 0x00321D20 File Offset: 0x0031FF20
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
		worker.Unsubscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Unsubscribe(25860745, new Action<object>(this.OnStoppedTalking));
	}

	// Token: 0x06007907 RID: 30983 RVA: 0x00321D74 File Offset: 0x0031FF74
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.specificEffect))
		{
			component.Add(this.specificEffect, true);
		}
	}

	// Token: 0x06007908 RID: 30984 RVA: 0x00321DA4 File Offset: 0x0031FFA4
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
			if (this.activity == PartyPointWorkable.ActivityType.Talk)
			{
				KBatchedAnimController component = base.worker.GetComponent<KBatchedAnimController>();
				string text = startedTalkingEvent.anim;
				text += UnityEngine.Random.Range(1, 9).ToString();
				component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
				component.Queue("idle", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		else
		{
			if (this.activity == PartyPointWorkable.ActivityType.Talk)
			{
				base.worker.GetComponent<Facing>().Face(talker.transform.GetPosition());
			}
			this.lastTalker = talker;
		}
	}

	// Token: 0x06007909 RID: 30985 RVA: 0x000AA038 File Offset: 0x000A8238
	private void OnStoppedTalking(object data)
	{
	}

	// Token: 0x0600790A RID: 30986 RVA: 0x000F3FEC File Offset: 0x000F21EC
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		if (!string.IsNullOrEmpty(this.specificEffect) && worker.GetComponent<Effects>().HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04005AD3 RID: 23251
	private GameObject lastTalker;

	// Token: 0x04005AD4 RID: 23252
	public int basePriority;

	// Token: 0x04005AD5 RID: 23253
	public string specificEffect;

	// Token: 0x04005AD6 RID: 23254
	public KAnimFile[][] workerOverrideAnims;

	// Token: 0x04005AD7 RID: 23255
	private PartyPointWorkable.ActivityType activity;

	// Token: 0x020016EB RID: 5867
	private enum ActivityType
	{
		// Token: 0x04005AD9 RID: 23257
		Talk,
		// Token: 0x04005ADA RID: 23258
		Dance,
		// Token: 0x04005ADB RID: 23259
		LENGTH
	}
}
