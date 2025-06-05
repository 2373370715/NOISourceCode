using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001333 RID: 4915
public class FetchOrder2
{
	// Token: 0x17000645 RID: 1605
	// (get) Token: 0x0600649F RID: 25759 RVA: 0x000E6168 File Offset: 0x000E4368
	// (set) Token: 0x060064A0 RID: 25760 RVA: 0x000E6170 File Offset: 0x000E4370
	public float TotalAmount { get; set; }

	// Token: 0x17000646 RID: 1606
	// (get) Token: 0x060064A1 RID: 25761 RVA: 0x000E6179 File Offset: 0x000E4379
	// (set) Token: 0x060064A2 RID: 25762 RVA: 0x000E6181 File Offset: 0x000E4381
	public int PriorityMod { get; set; }

	// Token: 0x17000647 RID: 1607
	// (get) Token: 0x060064A3 RID: 25763 RVA: 0x000E618A File Offset: 0x000E438A
	// (set) Token: 0x060064A4 RID: 25764 RVA: 0x000E6192 File Offset: 0x000E4392
	public HashSet<Tag> Tags { get; protected set; }

	// Token: 0x17000648 RID: 1608
	// (get) Token: 0x060064A5 RID: 25765 RVA: 0x000E619B File Offset: 0x000E439B
	// (set) Token: 0x060064A6 RID: 25766 RVA: 0x000E61A3 File Offset: 0x000E43A3
	public FetchChore.MatchCriteria Criteria { get; protected set; }

	// Token: 0x17000649 RID: 1609
	// (get) Token: 0x060064A7 RID: 25767 RVA: 0x000E61AC File Offset: 0x000E43AC
	// (set) Token: 0x060064A8 RID: 25768 RVA: 0x000E61B4 File Offset: 0x000E43B4
	public Tag RequiredTag { get; protected set; }

	// Token: 0x1700064A RID: 1610
	// (get) Token: 0x060064A9 RID: 25769 RVA: 0x000E61BD File Offset: 0x000E43BD
	// (set) Token: 0x060064AA RID: 25770 RVA: 0x000E61C5 File Offset: 0x000E43C5
	public Tag[] ForbiddenTags { get; protected set; }

	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x060064AB RID: 25771 RVA: 0x000E61CE File Offset: 0x000E43CE
	// (set) Token: 0x060064AC RID: 25772 RVA: 0x000E61D6 File Offset: 0x000E43D6
	public Storage Destination { get; set; }

	// Token: 0x1700064C RID: 1612
	// (get) Token: 0x060064AD RID: 25773 RVA: 0x000E61DF File Offset: 0x000E43DF
	// (set) Token: 0x060064AE RID: 25774 RVA: 0x000E61E7 File Offset: 0x000E43E7
	private float UnfetchedAmount
	{
		get
		{
			return this._UnfetchedAmount;
		}
		set
		{
			this._UnfetchedAmount = value;
			this.Assert(this._UnfetchedAmount <= this.TotalAmount, "_UnfetchedAmount <= TotalAmount");
			this.Assert(this._UnfetchedAmount >= 0f, "_UnfetchedAmount >= 0");
		}
	}

	// Token: 0x060064AF RID: 25775 RVA: 0x002CE290 File Offset: 0x002CC490
	public FetchOrder2(ChoreType chore_type, HashSet<Tag> tags, FetchChore.MatchCriteria criteria, Tag required_tag, Tag[] forbidden_tags, Storage destination, float amount, Operational.State operationalRequirementDEPRECATED = Operational.State.None, int priorityMod = 0)
	{
		if (amount <= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("FetchOrder2 {0} is requesting {1} {2} to {3}", new object[]
				{
					chore_type.Id,
					tags,
					amount,
					(destination != null) ? destination.name : "to nowhere"
				})
			});
		}
		this.choreType = chore_type;
		this.Tags = tags;
		this.Criteria = criteria;
		this.RequiredTag = required_tag;
		this.ForbiddenTags = forbidden_tags;
		this.Destination = destination;
		this.TotalAmount = amount;
		this.UnfetchedAmount = amount;
		this.PriorityMod = priorityMod;
		this.operationalRequirement = operationalRequirementDEPRECATED;
	}

	// Token: 0x1700064D RID: 1613
	// (get) Token: 0x060064B0 RID: 25776 RVA: 0x002CE35C File Offset: 0x002CC55C
	public bool InProgress
	{
		get
		{
			bool result = false;
			using (List<FetchChore>.Enumerator enumerator = this.Chores.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.InProgress())
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
	}

	// Token: 0x060064B1 RID: 25777 RVA: 0x000E6227 File Offset: 0x000E4427
	private void IssueTask()
	{
		if (this.UnfetchedAmount > 0f)
		{
			this.SetFetchTask(this.UnfetchedAmount);
			this.UnfetchedAmount = 0f;
		}
	}

	// Token: 0x060064B2 RID: 25778 RVA: 0x002CE3B8 File Offset: 0x002CC5B8
	public void SetPriorityMod(int priorityMod)
	{
		this.PriorityMod = priorityMod;
		for (int i = 0; i < this.Chores.Count; i++)
		{
			this.Chores[i].SetPriorityMod(this.PriorityMod);
		}
	}

	// Token: 0x060064B3 RID: 25779 RVA: 0x002CE3FC File Offset: 0x002CC5FC
	private void SetFetchTask(float amount)
	{
		FetchChore fetchChore = new FetchChore(this.choreType, this.Destination, amount, this.Tags, this.Criteria, this.RequiredTag, this.ForbiddenTags, null, true, new Action<Chore>(this.OnFetchChoreComplete), new Action<Chore>(this.OnFetchChoreBegin), new Action<Chore>(this.OnFetchChoreEnd), this.operationalRequirement, this.PriorityMod);
		fetchChore.validateRequiredTagOnTagChange = this.validateRequiredTagOnTagChange;
		this.Chores.Add(fetchChore);
	}

	// Token: 0x060064B4 RID: 25780 RVA: 0x002CE480 File Offset: 0x002CC680
	private void OnFetchChoreEnd(Chore chore)
	{
		FetchChore fetchChore = (FetchChore)chore;
		if (this.Chores.Contains(fetchChore))
		{
			this.UnfetchedAmount += fetchChore.amount;
			fetchChore.Cancel("FetchChore Redistribution");
			this.Chores.Remove(fetchChore);
			this.IssueTask();
		}
	}

	// Token: 0x060064B5 RID: 25781 RVA: 0x002CE4D4 File Offset: 0x002CC6D4
	private void OnFetchChoreComplete(Chore chore)
	{
		FetchChore fetchChore = (FetchChore)chore;
		this.Chores.Remove(fetchChore);
		if (this.Chores.Count == 0 && this.OnComplete != null)
		{
			this.OnComplete(this, fetchChore.fetchTarget);
		}
	}

	// Token: 0x060064B6 RID: 25782 RVA: 0x002CE51C File Offset: 0x002CC71C
	private void OnFetchChoreBegin(Chore chore)
	{
		FetchChore fetchChore = (FetchChore)chore;
		this.UnfetchedAmount += fetchChore.originalAmount - fetchChore.amount;
		this.IssueTask();
		if (this.OnBegin != null)
		{
			this.OnBegin(this, fetchChore.fetchTarget);
		}
	}

	// Token: 0x060064B7 RID: 25783 RVA: 0x002CE56C File Offset: 0x002CC76C
	public void Cancel(string reason)
	{
		while (this.Chores.Count > 0)
		{
			FetchChore fetchChore = this.Chores[0];
			fetchChore.Cancel(reason);
			this.Chores.Remove(fetchChore);
		}
	}

	// Token: 0x060064B8 RID: 25784 RVA: 0x000E624D File Offset: 0x000E444D
	public void Suspend(string reason)
	{
		global::Debug.LogError("UNIMPLEMENTED!");
	}

	// Token: 0x060064B9 RID: 25785 RVA: 0x000E624D File Offset: 0x000E444D
	public void Resume(string reason)
	{
		global::Debug.LogError("UNIMPLEMENTED!");
	}

	// Token: 0x060064BA RID: 25786 RVA: 0x002CE5AC File Offset: 0x002CC7AC
	public void Submit(Action<FetchOrder2, Pickupable> on_complete, bool check_storage_contents, Action<FetchOrder2, Pickupable> on_begin = null)
	{
		this.OnComplete = on_complete;
		this.OnBegin = on_begin;
		this.checkStorageContents = check_storage_contents;
		if (check_storage_contents)
		{
			Pickupable arg = null;
			this.UnfetchedAmount = this.GetRemaining(out arg);
			if (this.UnfetchedAmount > this.Destination.storageFullMargin)
			{
				this.IssueTask();
				return;
			}
			if (this.OnComplete != null)
			{
				this.OnComplete(this, arg);
				return;
			}
		}
		else
		{
			this.IssueTask();
		}
	}

	// Token: 0x060064BB RID: 25787 RVA: 0x002CE618 File Offset: 0x002CC818
	public bool IsMaterialOnStorage(Storage storage, ref float amount, ref Pickupable out_item)
	{
		foreach (GameObject gameObject in this.Destination.items)
		{
			if (gameObject != null)
			{
				Pickupable component = gameObject.GetComponent<Pickupable>();
				if (component != null)
				{
					KPrefabID kprefabID = component.KPrefabID;
					foreach (Tag tag in this.Tags)
					{
						if (kprefabID.HasTag(tag))
						{
							amount = component.TotalAmount;
							out_item = component;
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x060064BC RID: 25788 RVA: 0x002CE6E4 File Offset: 0x002CC8E4
	public float AmountWaitingToFetch()
	{
		if (!this.checkStorageContents)
		{
			float num = this.UnfetchedAmount;
			for (int i = 0; i < this.Chores.Count; i++)
			{
				num += this.Chores[i].AmountWaitingToFetch();
			}
			return num;
		}
		Pickupable pickupable;
		return this.GetRemaining(out pickupable);
	}

	// Token: 0x060064BD RID: 25789 RVA: 0x002CE734 File Offset: 0x002CC934
	public float GetRemaining(out Pickupable out_item)
	{
		float num = this.TotalAmount;
		float num2 = 0f;
		out_item = null;
		if (this.IsMaterialOnStorage(this.Destination, ref num2, ref out_item))
		{
			num = Math.Max(num - num2, 0f);
		}
		return num;
	}

	// Token: 0x060064BE RID: 25790 RVA: 0x002CE774 File Offset: 0x002CC974
	public bool IsComplete()
	{
		for (int i = 0; i < this.Chores.Count; i++)
		{
			if (!this.Chores[i].isComplete)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060064BF RID: 25791 RVA: 0x002CE7B0 File Offset: 0x002CC9B0
	private void Assert(bool condition, string message)
	{
		if (condition)
		{
			return;
		}
		string text = "FetchOrder error: " + message;
		if (this.Destination == null)
		{
			text += "\nDestination: None";
		}
		else
		{
			text = text + "\nDestination: " + this.Destination.name;
		}
		text = text + "\nTotal Amount: " + this.TotalAmount.ToString();
		text = text + "\nUnfetched Amount: " + this._UnfetchedAmount.ToString();
		global::Debug.LogError(text);
	}

	// Token: 0x0400486E RID: 18542
	public Action<FetchOrder2, Pickupable> OnComplete;

	// Token: 0x0400486F RID: 18543
	public Action<FetchOrder2, Pickupable> OnBegin;

	// Token: 0x04004874 RID: 18548
	public bool validateRequiredTagOnTagChange;

	// Token: 0x04004878 RID: 18552
	public List<FetchChore> Chores = new List<FetchChore>();

	// Token: 0x04004879 RID: 18553
	private ChoreType choreType;

	// Token: 0x0400487A RID: 18554
	private float _UnfetchedAmount;

	// Token: 0x0400487B RID: 18555
	private bool checkStorageContents;

	// Token: 0x0400487C RID: 18556
	private Operational.State operationalRequirement = Operational.State.None;
}
