using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001328 RID: 4904
public class FetchList2 : IFetchList
{
	// Token: 0x1700063F RID: 1599
	// (get) Token: 0x06006457 RID: 25687 RVA: 0x000E5F57 File Offset: 0x000E4157
	// (set) Token: 0x06006458 RID: 25688 RVA: 0x000E5F5F File Offset: 0x000E415F
	public bool ShowStatusItem
	{
		get
		{
			return this.bShowStatusItem;
		}
		set
		{
			this.bShowStatusItem = value;
		}
	}

	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x06006459 RID: 25689 RVA: 0x000E5F68 File Offset: 0x000E4168
	public bool IsComplete
	{
		get
		{
			return this.FetchOrders.Count == 0;
		}
	}

	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x0600645A RID: 25690 RVA: 0x002CC5D0 File Offset: 0x002CA7D0
	public bool InProgress
	{
		get
		{
			if (this.FetchOrders.Count < 0)
			{
				return false;
			}
			bool result = false;
			using (List<FetchOrder2>.Enumerator enumerator = this.FetchOrders.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.InProgress)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
	}

	// Token: 0x17000642 RID: 1602
	// (get) Token: 0x0600645B RID: 25691 RVA: 0x000E5F78 File Offset: 0x000E4178
	// (set) Token: 0x0600645C RID: 25692 RVA: 0x000E5F80 File Offset: 0x000E4180
	public Storage Destination { get; private set; }

	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x0600645D RID: 25693 RVA: 0x000E5F89 File Offset: 0x000E4189
	// (set) Token: 0x0600645E RID: 25694 RVA: 0x000E5F91 File Offset: 0x000E4191
	public int PriorityMod { get; private set; }

	// Token: 0x0600645F RID: 25695 RVA: 0x002CC63C File Offset: 0x002CA83C
	public FetchList2(Storage destination, ChoreType chore_type)
	{
		this.Destination = destination;
		this.choreType = chore_type;
	}

	// Token: 0x06006460 RID: 25696 RVA: 0x002CC6A8 File Offset: 0x002CA8A8
	public void SetPriorityMod(int priorityMod)
	{
		this.PriorityMod = priorityMod;
		for (int i = 0; i < this.FetchOrders.Count; i++)
		{
			this.FetchOrders[i].SetPriorityMod(this.PriorityMod);
		}
	}

	// Token: 0x06006461 RID: 25697 RVA: 0x002CC6EC File Offset: 0x002CA8EC
	public void Add(HashSet<Tag> tags, Tag requiredTag, Tag[] forbidden_tags = null, float amount = 1f, Operational.State operationalRequirementDEPRECATED = Operational.State.None)
	{
		foreach (Tag key in tags)
		{
			if (!this.MinimumAmount.ContainsKey(key))
			{
				this.MinimumAmount[key] = amount;
			}
		}
		FetchOrder2 item = new FetchOrder2(this.choreType, tags, FetchChore.MatchCriteria.MatchID, requiredTag, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod);
		this.FetchOrders.Add(item);
	}

	// Token: 0x06006462 RID: 25698 RVA: 0x002CC77C File Offset: 0x002CA97C
	public void Add(HashSet<Tag> tags, Tag[] forbidden_tags = null, float amount = 1f, Operational.State operationalRequirementDEPRECATED = Operational.State.None)
	{
		foreach (Tag key in tags)
		{
			if (!this.MinimumAmount.ContainsKey(key))
			{
				this.MinimumAmount[key] = amount;
			}
		}
		FetchOrder2 item = new FetchOrder2(this.choreType, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod);
		this.FetchOrders.Add(item);
	}

	// Token: 0x06006463 RID: 25699 RVA: 0x002CC810 File Offset: 0x002CAA10
	public void Add(Tag tag, Tag[] forbidden_tags = null, float amount = 1f, Operational.State operationalRequirementDEPRECATED = Operational.State.None)
	{
		if (!this.MinimumAmount.ContainsKey(tag))
		{
			this.MinimumAmount[tag] = amount;
		}
		FetchOrder2 item = new FetchOrder2(this.choreType, new HashSet<Tag>
		{
			tag
		}, FetchChore.MatchCriteria.MatchTags, Tag.Invalid, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod);
		this.FetchOrders.Add(item);
	}

	// Token: 0x06006464 RID: 25700 RVA: 0x002CC874 File Offset: 0x002CAA74
	public float GetMinimumAmount(Tag tag)
	{
		float result = 0f;
		this.MinimumAmount.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x06006465 RID: 25701 RVA: 0x000E5F9A File Offset: 0x000E419A
	private void OnFetchOrderComplete(FetchOrder2 fetch_order, Pickupable fetched_item)
	{
		this.FetchOrders.Remove(fetch_order);
		if (this.FetchOrders.Count == 0)
		{
			if (this.OnComplete != null)
			{
				this.OnComplete();
			}
			FetchListStatusItemUpdater.instance.RemoveFetchList(this);
			this.ClearStatus();
		}
	}

	// Token: 0x06006466 RID: 25702 RVA: 0x002CC898 File Offset: 0x002CAA98
	public void Cancel(string reason)
	{
		FetchListStatusItemUpdater.instance.RemoveFetchList(this);
		this.ClearStatus();
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			fetchOrder.Cancel(reason);
		}
	}

	// Token: 0x06006467 RID: 25703 RVA: 0x002CC8FC File Offset: 0x002CAAFC
	public void UpdateRemaining()
	{
		this.Remaining.Clear();
		for (int i = 0; i < this.FetchOrders.Count; i++)
		{
			FetchOrder2 fetchOrder = this.FetchOrders[i];
			foreach (Tag key in fetchOrder.Tags)
			{
				float num = 0f;
				this.Remaining.TryGetValue(key, out num);
				this.Remaining[key] = num + fetchOrder.AmountWaitingToFetch();
			}
		}
	}

	// Token: 0x06006468 RID: 25704 RVA: 0x000E5FDA File Offset: 0x000E41DA
	public Dictionary<Tag, float> GetRemaining()
	{
		return this.Remaining;
	}

	// Token: 0x06006469 RID: 25705 RVA: 0x002CC9A4 File Offset: 0x002CABA4
	public Dictionary<Tag, float> GetRemainingMinimum()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			foreach (Tag key in fetchOrder.Tags)
			{
				dictionary[key] = this.MinimumAmount[key];
			}
		}
		foreach (GameObject gameObject in this.Destination.items)
		{
			if (gameObject != null)
			{
				Pickupable component = gameObject.GetComponent<Pickupable>();
				if (component != null)
				{
					KPrefabID kprefabID = component.KPrefabID;
					if (dictionary.ContainsKey(kprefabID.PrefabTag))
					{
						dictionary[kprefabID.PrefabTag] = Math.Max(dictionary[kprefabID.PrefabTag] - component.TotalAmount, 0f);
					}
					foreach (Tag key2 in kprefabID.Tags)
					{
						if (dictionary.ContainsKey(key2))
						{
							dictionary[key2] = Math.Max(dictionary[key2] - component.TotalAmount, 0f);
						}
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x0600646A RID: 25706 RVA: 0x002CCB5C File Offset: 0x002CAD5C
	public void Suspend(string reason)
	{
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			fetchOrder.Suspend(reason);
		}
	}

	// Token: 0x0600646B RID: 25707 RVA: 0x002CCBB0 File Offset: 0x002CADB0
	public void Resume(string reason)
	{
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			fetchOrder.Resume(reason);
		}
	}

	// Token: 0x0600646C RID: 25708 RVA: 0x002CCC04 File Offset: 0x002CAE04
	public void Submit(System.Action on_complete, bool check_storage_contents)
	{
		this.OnComplete = on_complete;
		foreach (FetchOrder2 fetchOrder in this.FetchOrders.GetRange(0, this.FetchOrders.Count))
		{
			fetchOrder.Submit(new Action<FetchOrder2, Pickupable>(this.OnFetchOrderComplete), check_storage_contents, null);
		}
		if (!this.IsComplete && this.ShowStatusItem)
		{
			FetchListStatusItemUpdater.instance.AddFetchList(this);
		}
	}

	// Token: 0x0600646D RID: 25709 RVA: 0x002CCC98 File Offset: 0x002CAE98
	private void ClearStatus()
	{
		if (this.Destination != null)
		{
			KSelectable component = this.Destination.GetComponent<KSelectable>();
			if (component != null)
			{
				this.waitingForMaterialsHandle = component.RemoveStatusItem(this.waitingForMaterialsHandle, false);
				this.materialsUnavailableHandle = component.RemoveStatusItem(this.materialsUnavailableHandle, false);
				this.materialsUnavailableForRefillHandle = component.RemoveStatusItem(this.materialsUnavailableForRefillHandle, false);
			}
		}
	}

	// Token: 0x0600646E RID: 25710 RVA: 0x002CCD04 File Offset: 0x002CAF04
	public void UpdateStatusItem(MaterialsStatusItem status_item, ref Guid handle, bool should_add)
	{
		bool flag = handle != Guid.Empty;
		if (should_add != flag)
		{
			if (should_add)
			{
				KSelectable component = this.Destination.GetComponent<KSelectable>();
				if (component != null)
				{
					handle = component.AddStatusItem(status_item, this);
					GameScheduler.Instance.Schedule("Digging Tutorial", 2f, delegate(object obj)
					{
						Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Digging, true);
					}, null, null);
					return;
				}
			}
			else
			{
				KSelectable component2 = this.Destination.GetComponent<KSelectable>();
				if (component2 != null)
				{
					handle = component2.RemoveStatusItem(handle, false);
				}
			}
		}
	}

	// Token: 0x04004840 RID: 18496
	private System.Action OnComplete;

	// Token: 0x04004843 RID: 18499
	private ChoreType choreType;

	// Token: 0x04004844 RID: 18500
	public Guid waitingForMaterialsHandle = Guid.Empty;

	// Token: 0x04004845 RID: 18501
	public Guid materialsUnavailableForRefillHandle = Guid.Empty;

	// Token: 0x04004846 RID: 18502
	public Guid materialsUnavailableHandle = Guid.Empty;

	// Token: 0x04004847 RID: 18503
	public Dictionary<Tag, float> MinimumAmount = new Dictionary<Tag, float>();

	// Token: 0x04004848 RID: 18504
	public List<FetchOrder2> FetchOrders = new List<FetchOrder2>();

	// Token: 0x04004849 RID: 18505
	private Dictionary<Tag, float> Remaining = new Dictionary<Tag, float>();

	// Token: 0x0400484A RID: 18506
	private bool bShowStatusItem = true;
}
