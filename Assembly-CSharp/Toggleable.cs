using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B69 RID: 2921
[AddComponentMenu("KMonoBehaviour/Workable/Toggleable")]
public class Toggleable : Workable
{
	// Token: 0x060036F2 RID: 14066 RVA: 0x000C8314 File Offset: 0x000C6514
	protected Toggleable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x060036F3 RID: 14067 RVA: 0x00222740 File Offset: 0x00220940
	protected override void OnPrefabInit()
	{
		this.faceTargetWhenWorking = true;
		base.OnPrefabInit();
		this.targets = new List<KeyValuePair<IToggleHandler, Chore>>();
		base.SetWorkTime(3f);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Toggling;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x060036F4 RID: 14068 RVA: 0x000C8327 File Offset: 0x000C6527
	public int SetTarget(IToggleHandler handler)
	{
		this.targets.Add(new KeyValuePair<IToggleHandler, Chore>(handler, null));
		return this.targets.Count - 1;
	}

	// Token: 0x060036F5 RID: 14069 RVA: 0x002227AC File Offset: 0x002209AC
	public IToggleHandler GetToggleHandlerForWorker(WorkerBase worker)
	{
		int targetForWorker = this.GetTargetForWorker(worker);
		if (targetForWorker != -1)
		{
			return this.targets[targetForWorker].Key;
		}
		return null;
	}

	// Token: 0x060036F6 RID: 14070 RVA: 0x002227DC File Offset: 0x002209DC
	private int GetTargetForWorker(WorkerBase worker)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			if (this.targets[i].Value != null && this.targets[i].Value.driver != null && this.targets[i].Value.driver.gameObject == worker.gameObject)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060036F7 RID: 14071 RVA: 0x00222864 File Offset: 0x00220A64
	protected override void OnCompleteWork(WorkerBase worker)
	{
		int targetForWorker = this.GetTargetForWorker(worker);
		if (targetForWorker != -1 && this.targets[targetForWorker].Key != null)
		{
			this.targets[targetForWorker] = new KeyValuePair<IToggleHandler, Chore>(this.targets[targetForWorker].Key, null);
			this.targets[targetForWorker].Key.HandleToggle();
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, false);
	}

	// Token: 0x060036F8 RID: 14072 RVA: 0x002228F0 File Offset: 0x00220AF0
	private void QueueToggle(int targetIdx)
	{
		if (this.targets[targetIdx].Value == null)
		{
			if (DebugHandler.InstantBuildMode)
			{
				this.targets[targetIdx].Key.HandleToggle();
				return;
			}
			this.targets[targetIdx] = new KeyValuePair<IToggleHandler, Chore>(this.targets[targetIdx].Key, new WorkChore<Toggleable>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true));
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, null);
		}
	}

	// Token: 0x060036F9 RID: 14073 RVA: 0x002229A0 File Offset: 0x00220BA0
	public void Toggle(int targetIdx)
	{
		if (targetIdx >= this.targets.Count)
		{
			return;
		}
		if (this.targets[targetIdx].Value == null)
		{
			this.QueueToggle(targetIdx);
			return;
		}
		this.CancelToggle(targetIdx);
	}

	// Token: 0x060036FA RID: 14074 RVA: 0x002229E4 File Offset: 0x00220BE4
	private void CancelToggle(int targetIdx)
	{
		if (this.targets[targetIdx].Value != null)
		{
			this.targets[targetIdx].Value.Cancel("Toggle cancelled");
			this.targets[targetIdx] = new KeyValuePair<IToggleHandler, Chore>(this.targets[targetIdx].Key, null);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, false);
		}
	}

	// Token: 0x060036FB RID: 14075 RVA: 0x00222A68 File Offset: 0x00220C68
	public bool IsToggleQueued(int targetIdx)
	{
		return this.targets[targetIdx].Value != null;
	}

	// Token: 0x040025FB RID: 9723
	private List<KeyValuePair<IToggleHandler, Chore>> targets;
}
