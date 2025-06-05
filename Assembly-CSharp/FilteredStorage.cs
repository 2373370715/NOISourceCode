using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000DA7 RID: 3495
public class FilteredStorage
{
	// Token: 0x060043E3 RID: 17379 RVA: 0x000D04C6 File Offset: 0x000CE6C6
	public void SetHasMeter(bool has_meter)
	{
		this.hasMeter = has_meter;
	}

	// Token: 0x060043E4 RID: 17380 RVA: 0x00254720 File Offset: 0x00252920
	public FilteredStorage(KMonoBehaviour root, Tag[] forbidden_tags, IUserControlledCapacity capacity_control, bool use_logic_meter, ChoreType fetch_chore_type)
	{
		this.root = root;
		this.forbiddenTags = forbidden_tags;
		this.capacityControl = capacity_control;
		this.useLogicMeter = use_logic_meter;
		this.choreType = fetch_chore_type;
		root.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		root.Subscribe(-543130682, new Action<object>(this.OnUserSettingsChanged));
		this.filterable = root.FindOrAdd<TreeFilterable>();
		TreeFilterable treeFilterable = this.filterable;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		this.storage = root.GetComponent<Storage>();
		this.storage.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		this.storage.Subscribe(-1852328367, new Action<object>(this.OnFunctionalChanged));
	}

	// Token: 0x060043E5 RID: 17381 RVA: 0x000D04CF File Offset: 0x000CE6CF
	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		this.OnFilterChanged(this.filterable.GetTags());
	}

	// Token: 0x060043E6 RID: 17382 RVA: 0x00254814 File Offset: 0x00252A14
	private void CreateMeter()
	{
		if (!this.hasMeter)
		{
			return;
		}
		this.meter = new MeterController(this.root.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_frame",
			"meter_level"
		});
	}

	// Token: 0x060043E7 RID: 17383 RVA: 0x000D04E2 File Offset: 0x000CE6E2
	private void CreateLogicMeter()
	{
		if (!this.hasMeter)
		{
			return;
		}
		this.logicMeter = new MeterController(this.root.GetComponent<KBatchedAnimController>(), "logicmeter_target", "logicmeter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x060043E8 RID: 17384 RVA: 0x000D0515 File Offset: 0x000CE715
	public void SetMeter(MeterController meter)
	{
		this.hasMeter = true;
		this.meter = meter;
		this.UpdateMeter();
	}

	// Token: 0x060043E9 RID: 17385 RVA: 0x00254864 File Offset: 0x00252A64
	public void CleanUp()
	{
		if (this.filterable != null)
		{
			TreeFilterable treeFilterable = this.filterable;
			treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		}
		if (this.fetchList != null)
		{
			this.fetchList.Cancel("Parent destroyed");
		}
	}

	// Token: 0x060043EA RID: 17386 RVA: 0x002548C0 File Offset: 0x00252AC0
	public void FilterChanged()
	{
		if (this.hasMeter)
		{
			if (this.meter == null)
			{
				this.CreateMeter();
			}
			if (this.logicMeter == null && this.useLogicMeter)
			{
				this.CreateLogicMeter();
			}
		}
		this.OnFilterChanged(this.filterable.GetTags());
		this.UpdateMeter();
	}

	// Token: 0x060043EB RID: 17387 RVA: 0x000D052B File Offset: 0x000CE72B
	private void OnUserSettingsChanged(object data)
	{
		this.OnFilterChanged(this.filterable.GetTags());
		this.UpdateMeter();
	}

	// Token: 0x060043EC RID: 17388 RVA: 0x000D0544 File Offset: 0x000CE744
	private void OnStorageChanged(object data)
	{
		if (this.fetchList == null)
		{
			this.OnFilterChanged(this.filterable.GetTags());
		}
		this.UpdateMeter();
	}

	// Token: 0x060043ED RID: 17389 RVA: 0x000D04CF File Offset: 0x000CE6CF
	private void OnFunctionalChanged(object data)
	{
		this.OnFilterChanged(this.filterable.GetTags());
	}

	// Token: 0x060043EE RID: 17390 RVA: 0x00254910 File Offset: 0x00252B10
	private void UpdateMeter()
	{
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float positionPercent = Mathf.Clamp01(this.GetAmountStored() / maxCapacityMinusStorageMargin);
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x060043EF RID: 17391 RVA: 0x00254948 File Offset: 0x00252B48
	public bool IsFull()
	{
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float num = Mathf.Clamp01(this.GetAmountStored() / maxCapacityMinusStorageMargin);
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(num);
		}
		return num >= 1f;
	}

	// Token: 0x060043F0 RID: 17392 RVA: 0x000D04CF File Offset: 0x000CE6CF
	private void OnFetchComplete()
	{
		this.OnFilterChanged(this.filterable.GetTags());
	}

	// Token: 0x060043F1 RID: 17393 RVA: 0x0025498C File Offset: 0x00252B8C
	private float GetMaxCapacity()
	{
		float num = this.storage.capacityKg;
		if (this.capacityControl != null)
		{
			num = Mathf.Min(num, this.capacityControl.UserMaxCapacity);
		}
		return num;
	}

	// Token: 0x060043F2 RID: 17394 RVA: 0x000D0565 File Offset: 0x000CE765
	private float GetMaxCapacityMinusStorageMargin()
	{
		return this.GetMaxCapacity() - this.storage.storageFullMargin;
	}

	// Token: 0x060043F3 RID: 17395 RVA: 0x002549C0 File Offset: 0x00252BC0
	private float GetAmountStored()
	{
		float result = this.storage.MassStored();
		if (this.capacityControl != null)
		{
			result = this.capacityControl.AmountStored;
		}
		return result;
	}

	// Token: 0x060043F4 RID: 17396 RVA: 0x002549F0 File Offset: 0x00252BF0
	private bool IsFunctional()
	{
		Operational component = this.storage.GetComponent<Operational>();
		return component == null || component.IsFunctional;
	}

	// Token: 0x060043F5 RID: 17397 RVA: 0x00254A1C File Offset: 0x00252C1C
	private void OnFilterChanged(HashSet<Tag> tags)
	{
		bool flag = tags != null && tags.Count != 0;
		if (this.fetchList != null)
		{
			this.fetchList.Cancel("");
			this.fetchList = null;
		}
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float amountStored = this.GetAmountStored();
		float num = Mathf.Max(0f, maxCapacityMinusStorageMargin - amountStored);
		if (num > 0f && flag && this.IsFunctional())
		{
			num = Mathf.Max(0f, this.GetMaxCapacity() - amountStored);
			this.fetchList = new FetchList2(this.storage, this.choreType);
			this.fetchList.ShowStatusItem = false;
			this.fetchList.Add(tags, this.requiredTag, this.forbiddenTags, num, Operational.State.Functional);
			this.fetchList.Submit(new System.Action(this.OnFetchComplete), false);
		}
	}

	// Token: 0x060043F6 RID: 17398 RVA: 0x000D0579 File Offset: 0x000CE779
	public void SetLogicMeter(bool on)
	{
		if (this.logicMeter != null)
		{
			this.logicMeter.SetPositionPercent(on ? 1f : 0f);
		}
	}

	// Token: 0x060043F7 RID: 17399 RVA: 0x000D059D File Offset: 0x000CE79D
	public void SetRequiredTag(Tag tag)
	{
		if (this.requiredTag != tag)
		{
			this.requiredTag = tag;
			this.OnFilterChanged(this.filterable.GetTags());
		}
	}

	// Token: 0x060043F8 RID: 17400 RVA: 0x00254AF0 File Offset: 0x00252CF0
	public void AddForbiddenTag(Tag forbidden_tag)
	{
		if (this.forbiddenTags == null)
		{
			this.forbiddenTags = new Tag[0];
		}
		if (!this.forbiddenTags.Contains(forbidden_tag))
		{
			this.forbiddenTags = this.forbiddenTags.Append(forbidden_tag);
			this.OnFilterChanged(this.filterable.GetTags());
		}
	}

	// Token: 0x060043F9 RID: 17401 RVA: 0x00254B44 File Offset: 0x00252D44
	public void RemoveForbiddenTag(Tag forbidden_tag)
	{
		if (this.forbiddenTags != null)
		{
			List<Tag> list = new List<Tag>(this.forbiddenTags);
			list.Remove(forbidden_tag);
			this.forbiddenTags = list.ToArray();
			this.OnFilterChanged(this.filterable.GetTags());
		}
	}

	// Token: 0x04002F08 RID: 12040
	public static readonly HashedString FULL_PORT_ID = "FULL";

	// Token: 0x04002F09 RID: 12041
	private KMonoBehaviour root;

	// Token: 0x04002F0A RID: 12042
	private FetchList2 fetchList;

	// Token: 0x04002F0B RID: 12043
	private IUserControlledCapacity capacityControl;

	// Token: 0x04002F0C RID: 12044
	private TreeFilterable filterable;

	// Token: 0x04002F0D RID: 12045
	private Storage storage;

	// Token: 0x04002F0E RID: 12046
	private MeterController meter;

	// Token: 0x04002F0F RID: 12047
	private MeterController logicMeter;

	// Token: 0x04002F10 RID: 12048
	private Tag requiredTag = Tag.Invalid;

	// Token: 0x04002F11 RID: 12049
	private Tag[] forbiddenTags;

	// Token: 0x04002F12 RID: 12050
	private bool hasMeter = true;

	// Token: 0x04002F13 RID: 12051
	private bool useLogicMeter;

	// Token: 0x04002F14 RID: 12052
	private ChoreType choreType;
}
