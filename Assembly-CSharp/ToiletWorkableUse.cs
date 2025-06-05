using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001041 RID: 4161
[AddComponentMenu("KMonoBehaviour/Workable/ToiletWorkableUse")]
public class ToiletWorkableUse : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06005469 RID: 21609 RVA: 0x000DB60B File Offset: 0x000D980B
	private ToiletWorkableUse()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x0600546A RID: 21610 RVA: 0x000DB63C File Offset: 0x000D983C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
		base.SetWorkTime(8.5f);
	}

	// Token: 0x0600546B RID: 21611 RVA: 0x0028941C File Offset: 0x0028761C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (Sim.IsRadiationEnabled() && worker.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
		{
			worker.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			roomOfGameObject.roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), worker.GetComponent<Effects>());
		}
		if (worker != null)
		{
			this.last_user_id = worker.gameObject.PrefabID();
		}
	}

	// Token: 0x0600546C RID: 21612 RVA: 0x002894C8 File Offset: 0x002876C8
	public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		HashedString[] array = null;
		if (this.workerTypePstAnims.TryGetValue(worker.PrefabID(), out array))
		{
			this.workingPstComplete = array;
			this.workingPstFailed = array;
		}
		return base.GetWorkPstAnims(worker, successfully_completed);
	}

	// Token: 0x0600546D RID: 21613 RVA: 0x00289504 File Offset: 0x00287704
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		KAnimFile[] overrideAnims = null;
		if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out overrideAnims))
		{
			this.overrideAnims = overrideAnims;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x0600546E RID: 21614 RVA: 0x000DB672 File Offset: 0x000D9872
	protected override void OnStopWork(WorkerBase worker)
	{
		if (Sim.IsRadiationEnabled())
		{
			worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
		}
		base.OnStopWork(worker);
	}

	// Token: 0x0600546F RID: 21615 RVA: 0x000DB6A3 File Offset: 0x000D98A3
	protected override void OnAbortWork(WorkerBase worker)
	{
		if (Sim.IsRadiationEnabled())
		{
			worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
		}
		base.OnAbortWork(worker);
	}

	// Token: 0x06005470 RID: 21616 RVA: 0x00289538 File Offset: 0x00287738
	protected override void OnCompleteWork(WorkerBase worker)
	{
		AmountInstance amountInstance = Db.Get().Amounts.Bladder.Lookup(worker);
		if (amountInstance != null)
		{
			this.lastAmountOfWasteMassRemovedFromDupe = DUPLICANTSTATS.STANDARD.Secretions.PEE_PER_TOILET_PEE;
			this.lastElementRemovedFromDupe = SimHashes.DirtyWater;
			amountInstance.SetValue(0f);
		}
		else
		{
			GunkMonitor.Instance smi = worker.GetSMI<GunkMonitor.Instance>();
			if (smi != null)
			{
				this.lastAmountOfWasteMassRemovedFromDupe = smi.CurrentGunkMass;
				this.lastElementRemovedFromDupe = GunkMonitor.GunkElement;
				smi.SetGunkMassValue(0f);
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_GunkedToilet, true);
			}
		}
		if (Sim.IsRadiationEnabled())
		{
			worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
			AmountInstance amountInstance2 = Db.Get().Amounts.RadiationBalance.Lookup(worker);
			RadiationMonitor.Instance smi2 = worker.GetSMI<RadiationMonitor.Instance>();
			float num = Math.Min(amountInstance2.value, 100f * smi2.difficultySettingMod);
			if (num >= 1f)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Math.Floor((double)num).ToString() + UI.UNITSUFFIXES.RADIATION.RADS, worker.transform, Vector3.up * 2f, 1.5f, false, false);
			}
			amountInstance2.ApplyDelta(-num);
		}
		this.timesUsed++;
		if (amountInstance != null)
		{
			base.Trigger(-350347868, worker);
		}
		else
		{
			base.Trigger(1234642927, worker);
		}
		base.OnCompleteWork(worker);
	}

	// Token: 0x06005471 RID: 21617 RVA: 0x000DB6D4 File Offset: 0x000D98D4
	public override StatusItem GetWorkerStatusItem()
	{
		if (base.worker != null && base.worker.gameObject.HasTag(GameTags.Minions.Models.Bionic))
		{
			return Db.Get().DuplicantStatusItems.CloggingToilet;
		}
		return base.GetWorkerStatusItem();
	}

	// Token: 0x04003B87 RID: 15239
	public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();

	// Token: 0x04003B88 RID: 15240
	public Dictionary<Tag, HashedString[]> workerTypePstAnims = new Dictionary<Tag, HashedString[]>();

	// Token: 0x04003B89 RID: 15241
	[Serialize]
	public int timesUsed;

	// Token: 0x04003B8A RID: 15242
	[Serialize]
	public Tag last_user_id;

	// Token: 0x04003B8B RID: 15243
	[Serialize]
	public SimHashes lastElementRemovedFromDupe = SimHashes.DirtyWater;

	// Token: 0x04003B8C RID: 15244
	[Serialize]
	public float lastAmountOfWasteMassRemovedFromDupe;
}
