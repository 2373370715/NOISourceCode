using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02001040 RID: 4160
[AddComponentMenu("KMonoBehaviour/Workable/ToiletWorkableClean")]
public class ToiletWorkableClean : Workable
{
	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x06005463 RID: 21603 RVA: 0x000DB603 File Offset: 0x000D9803
	// (set) Token: 0x06005462 RID: 21602 RVA: 0x000DB5FA File Offset: 0x000D97FA
	public bool IsCloggedByGunk { get; private set; }

	// Token: 0x06005464 RID: 21604 RVA: 0x002891FC File Offset: 0x002873FC
	public void SetIsCloggedByGunk(bool isIt)
	{
		this.IsCloggedByGunk = isIt;
		this.workAnims = (this.IsCloggedByGunk ? ToiletWorkableClean.CLEAN_GUNK_ANIMS : ToiletWorkableClean.CLEAN_ANIMS);
		this.workingPstComplete = (this.IsCloggedByGunk ? ToiletWorkableClean.PST_GUNK_ANIM : ToiletWorkableClean.PST_ANIM);
		this.workingPstFailed = (this.IsCloggedByGunk ? ToiletWorkableClean.PST_GUNK_ANIM : ToiletWorkableClean.PST_ANIM);
	}

	// Token: 0x06005465 RID: 21605 RVA: 0x00289260 File Offset: 0x00287460
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x06005466 RID: 21606 RVA: 0x002892E4 File Offset: 0x002874E4
	protected override void OnCompleteWork(WorkerBase worker)
	{
		ToiletWorkableUse component = base.gameObject.GetComponent<ToiletWorkableUse>();
		if (component != null && this.IsCloggedByGunk && base.gameObject.GetComponent<FlushToilet>() == null)
		{
			LiquidSourceManager.Instance.CreateChunk(SimHashes.LiquidGunk, component.lastAmountOfWasteMassRemovedFromDupe, DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL, byte.MaxValue, 0, Grid.CellToPos(Grid.PosToCell(base.gameObject), CellAlignment.Top, Grid.SceneLayer.Ore));
		}
		this.timesCleaned++;
		base.OnCompleteWork(worker);
	}

	// Token: 0x04003B81 RID: 15233
	[Serialize]
	public int timesCleaned;

	// Token: 0x04003B82 RID: 15234
	private static readonly HashedString[] CLEAN_GUNK_ANIMS = new HashedString[]
	{
		"degunk_pre",
		"degunk_loop"
	};

	// Token: 0x04003B83 RID: 15235
	private static readonly HashedString[] CLEAN_ANIMS = new HashedString[]
	{
		"unclog_pre",
		"unclog_loop"
	};

	// Token: 0x04003B84 RID: 15236
	private static readonly HashedString[] PST_ANIM = new HashedString[]
	{
		new HashedString("unclog_pst")
	};

	// Token: 0x04003B85 RID: 15237
	private static readonly HashedString[] PST_GUNK_ANIM = new HashedString[]
	{
		new HashedString("degunk_pst")
	};
}
