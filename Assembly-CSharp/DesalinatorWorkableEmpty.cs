using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000D58 RID: 3416
[AddComponentMenu("KMonoBehaviour/Workable/DesalinatorWorkableEmpty")]
public class DesalinatorWorkableEmpty : Workable
{
	// Token: 0x0600424E RID: 16974 RVA: 0x0024ECA0 File Offset: 0x0024CEA0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_desalinator_kanim")
		};
		this.workAnims = DesalinatorWorkableEmpty.WORK_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			DesalinatorWorkableEmpty.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			DesalinatorWorkableEmpty.PST_ANIM
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x0600424F RID: 16975 RVA: 0x000CF577 File Offset: 0x000CD777
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.timesCleaned++;
		base.OnCompleteWork(worker);
	}

	// Token: 0x04002DC2 RID: 11714
	[Serialize]
	public int timesCleaned;

	// Token: 0x04002DC3 RID: 11715
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"salt_pre",
		"salt_loop"
	};

	// Token: 0x04002DC4 RID: 11716
	private static readonly HashedString PST_ANIM = new HashedString("salt_pst");
}
