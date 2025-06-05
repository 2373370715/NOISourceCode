using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FA4 RID: 8100
public class CheckboxListGroupSideScreen : SideScreenContent
{
	// Token: 0x0600AB2D RID: 43821 RVA: 0x00113AB4 File Offset: 0x00111CB4
	private CheckboxListGroupSideScreen.CheckboxContainer InstantiateCheckboxContainer()
	{
		return new CheckboxListGroupSideScreen.CheckboxContainer(Util.KInstantiateUI(this.checkboxGroupPrefab, this.groupParent.gameObject, true).GetComponent<HierarchyReferences>());
	}

	// Token: 0x0600AB2E RID: 43822 RVA: 0x00113AD7 File Offset: 0x00111CD7
	private GameObject InstantiateCheckbox()
	{
		return Util.KInstantiateUI(this.checkboxPrefab, this.checkboxParent.gameObject, false);
	}

	// Token: 0x0600AB2F RID: 43823 RVA: 0x00113AF0 File Offset: 0x00111CF0
	protected override void OnSpawn()
	{
		this.checkboxPrefab.SetActive(false);
		this.checkboxGroupPrefab.SetActive(false);
		base.OnSpawn();
	}

	// Token: 0x0600AB30 RID: 43824 RVA: 0x00417F94 File Offset: 0x00416194
	public override bool IsValidForTarget(GameObject target)
	{
		ICheckboxListGroupControl[] components = target.GetComponents<ICheckboxListGroupControl>();
		if (components != null)
		{
			ICheckboxListGroupControl[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].SidescreenEnabled())
				{
					return true;
				}
			}
		}
		using (List<ICheckboxListGroupControl>.Enumerator enumerator = target.GetAllSMI<ICheckboxListGroupControl>().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.SidescreenEnabled())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600AB31 RID: 43825 RVA: 0x00113B10 File Offset: 0x00111D10
	public override int GetSideScreenSortOrder()
	{
		if (this.targets == null)
		{
			return 20;
		}
		return this.targets[0].CheckboxSideScreenSortOrder();
	}

	// Token: 0x0600AB32 RID: 43826 RVA: 0x00418018 File Offset: 0x00416218
	public override void SetTarget(GameObject target)
	{
		if (target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.targets = target.GetAllSMI<ICheckboxListGroupControl>();
		this.targets.AddRange(target.GetComponents<ICheckboxListGroupControl>());
		this.Rebuild(target);
		this.uiRefreshSubHandle = this.currentBuildTarget.Subscribe(1980521255, new Action<object>(this.Refresh));
	}

	// Token: 0x0600AB33 RID: 43827 RVA: 0x00418080 File Offset: 0x00416280
	public override void ClearTarget()
	{
		if (this.uiRefreshSubHandle != -1 && this.currentBuildTarget != null)
		{
			this.currentBuildTarget.Unsubscribe(this.uiRefreshSubHandle);
			this.uiRefreshSubHandle = -1;
		}
		this.ReleaseContainers(this.activeChecklistGroups.Count);
	}

	// Token: 0x0600AB34 RID: 43828 RVA: 0x00113B2E File Offset: 0x00111D2E
	public override string GetTitle()
	{
		if (this.targets != null && this.targets.Count > 0 && this.targets[0] != null)
		{
			return this.targets[0].Title;
		}
		return base.GetTitle();
	}

	// Token: 0x0600AB35 RID: 43829 RVA: 0x004180D0 File Offset: 0x004162D0
	private void Rebuild(GameObject buildTarget)
	{
		if (this.checkboxContainerPool == null)
		{
			this.checkboxContainerPool = new ObjectPool<CheckboxListGroupSideScreen.CheckboxContainer>(new Func<CheckboxListGroupSideScreen.CheckboxContainer>(this.InstantiateCheckboxContainer), 0);
			this.checkboxPool = new GameObjectPool(new Func<GameObject>(this.InstantiateCheckbox), 0);
		}
		this.descriptionLabel.enabled = !this.targets[0].Description.IsNullOrWhiteSpace();
		if (!this.targets[0].Description.IsNullOrWhiteSpace())
		{
			this.descriptionLabel.SetText(this.targets[0].Description);
		}
		if (buildTarget == this.currentBuildTarget)
		{
			this.Refresh(null);
			return;
		}
		this.currentBuildTarget = buildTarget;
		foreach (ICheckboxListGroupControl checkboxListGroupControl in this.targets)
		{
			foreach (ICheckboxListGroupControl.ListGroup group in checkboxListGroupControl.GetData())
			{
				CheckboxListGroupSideScreen.CheckboxContainer instance = this.checkboxContainerPool.GetInstance();
				this.InitContainer(checkboxListGroupControl, group, instance);
			}
		}
	}

	// Token: 0x0600AB36 RID: 43830 RVA: 0x00113B6C File Offset: 0x00111D6C
	[ContextMenu("Force refresh")]
	private void Test()
	{
		this.Refresh(null);
	}

	// Token: 0x0600AB37 RID: 43831 RVA: 0x00418200 File Offset: 0x00416400
	private void Refresh(object data = null)
	{
		int num = 0;
		foreach (ICheckboxListGroupControl checkboxListGroupControl in this.targets)
		{
			foreach (ICheckboxListGroupControl.ListGroup listGroup in checkboxListGroupControl.GetData())
			{
				if (++num > this.activeChecklistGroups.Count)
				{
					this.InitContainer(checkboxListGroupControl, listGroup, this.checkboxContainerPool.GetInstance());
				}
				CheckboxListGroupSideScreen.CheckboxContainer checkboxContainer = this.activeChecklistGroups[num - 1];
				if (listGroup.resolveTitleCallback != null)
				{
					checkboxContainer.container.GetReference<LocText>("Text").SetText(listGroup.resolveTitleCallback(listGroup.title));
				}
				for (int j = 0; j < listGroup.checkboxItems.Length; j++)
				{
					ICheckboxListGroupControl.CheckboxItem data3 = listGroup.checkboxItems[j];
					if (checkboxContainer.checkboxUIItems.Count <= j)
					{
						this.CreateSingleCheckBoxForGroupUI(checkboxContainer);
					}
					HierarchyReferences checkboxUI = checkboxContainer.checkboxUIItems[j];
					this.SetCheckboxData(checkboxUI, data3, checkboxListGroupControl);
				}
				while (checkboxContainer.checkboxUIItems.Count > listGroup.checkboxItems.Length)
				{
					HierarchyReferences checkbox = checkboxContainer.checkboxUIItems[checkboxContainer.checkboxUIItems.Count - 1];
					this.RemoveSingleCheckboxFromContainer(checkbox, checkboxContainer);
				}
			}
		}
		this.ReleaseContainers(this.activeChecklistGroups.Count - num);
	}

	// Token: 0x0600AB38 RID: 43832 RVA: 0x004183A0 File Offset: 0x004165A0
	private void ReleaseContainers(int count)
	{
		int count2 = this.activeChecklistGroups.Count;
		for (int i = 1; i <= count; i++)
		{
			int index = count2 - i;
			CheckboxListGroupSideScreen.CheckboxContainer checkboxContainer = this.activeChecklistGroups[index];
			this.activeChecklistGroups.RemoveAt(index);
			for (int j = checkboxContainer.checkboxUIItems.Count - 1; j >= 0; j--)
			{
				HierarchyReferences checkbox = checkboxContainer.checkboxUIItems[j];
				this.RemoveSingleCheckboxFromContainer(checkbox, checkboxContainer);
			}
			checkboxContainer.container.gameObject.SetActive(false);
			this.checkboxContainerPool.ReleaseInstance(checkboxContainer);
		}
	}

	// Token: 0x0600AB39 RID: 43833 RVA: 0x00418434 File Offset: 0x00416634
	private void InitContainer(ICheckboxListGroupControl target, ICheckboxListGroupControl.ListGroup group, CheckboxListGroupSideScreen.CheckboxContainer groupUI)
	{
		this.activeChecklistGroups.Add(groupUI);
		groupUI.container.gameObject.SetActive(true);
		string text = group.title;
		if (group.resolveTitleCallback != null)
		{
			text = group.resolveTitleCallback(text);
		}
		groupUI.container.GetReference<LocText>("Text").SetText(text);
		foreach (ICheckboxListGroupControl.CheckboxItem data in group.checkboxItems)
		{
			this.CreateSingleCheckBoxForGroupUI(data, target, groupUI);
		}
	}

	// Token: 0x0600AB3A RID: 43834 RVA: 0x00113B75 File Offset: 0x00111D75
	public void RemoveSingleCheckboxFromContainer(HierarchyReferences checkbox, CheckboxListGroupSideScreen.CheckboxContainer container)
	{
		container.checkboxUIItems.Remove(checkbox);
		checkbox.gameObject.SetActive(false);
		checkbox.transform.SetParent(this.checkboxParent);
		this.checkboxPool.ReleaseInstance(checkbox.gameObject);
	}

	// Token: 0x0600AB3B RID: 43835 RVA: 0x004184B8 File Offset: 0x004166B8
	public HierarchyReferences CreateSingleCheckBoxForGroupUI(CheckboxListGroupSideScreen.CheckboxContainer container)
	{
		HierarchyReferences component = this.checkboxPool.GetInstance().GetComponent<HierarchyReferences>();
		component.gameObject.SetActive(true);
		container.checkboxUIItems.Add(component);
		component.transform.SetParent(container.container.transform);
		return component;
	}

	// Token: 0x0600AB3C RID: 43836 RVA: 0x00418508 File Offset: 0x00416708
	public HierarchyReferences CreateSingleCheckBoxForGroupUI(ICheckboxListGroupControl.CheckboxItem data, ICheckboxListGroupControl target, CheckboxListGroupSideScreen.CheckboxContainer container)
	{
		HierarchyReferences hierarchyReferences = this.CreateSingleCheckBoxForGroupUI(container);
		this.SetCheckboxData(hierarchyReferences, data, target);
		return hierarchyReferences;
	}

	// Token: 0x0600AB3D RID: 43837 RVA: 0x00418528 File Offset: 0x00416728
	public void SetCheckboxData(HierarchyReferences checkboxUI, ICheckboxListGroupControl.CheckboxItem data, ICheckboxListGroupControl target)
	{
		LocText reference = checkboxUI.GetReference<LocText>("Text");
		reference.SetText(data.text);
		reference.SetLinkOverrideAction(data.overrideLinkActions);
		checkboxUI.GetReference<Image>("Check").enabled = data.isOn;
		ToolTip reference2 = checkboxUI.GetReference<ToolTip>("Tooltip");
		reference2.SetSimpleTooltip(data.tooltip);
		reference2.refreshWhileHovering = (data.resolveTooltipCallback != null);
		reference2.OnToolTip = delegate()
		{
			if (data.resolveTooltipCallback == null)
			{
				return data.tooltip;
			}
			return data.resolveTooltipCallback(data.tooltip, target);
		};
	}

	// Token: 0x040086BF RID: 34495
	public const int DefaultCheckboxListSideScreenSortOrder = 20;

	// Token: 0x040086C0 RID: 34496
	private ObjectPool<CheckboxListGroupSideScreen.CheckboxContainer> checkboxContainerPool;

	// Token: 0x040086C1 RID: 34497
	private GameObjectPool checkboxPool;

	// Token: 0x040086C2 RID: 34498
	[SerializeField]
	private GameObject checkboxGroupPrefab;

	// Token: 0x040086C3 RID: 34499
	[SerializeField]
	private GameObject checkboxPrefab;

	// Token: 0x040086C4 RID: 34500
	[SerializeField]
	private RectTransform groupParent;

	// Token: 0x040086C5 RID: 34501
	[SerializeField]
	private RectTransform checkboxParent;

	// Token: 0x040086C6 RID: 34502
	[SerializeField]
	private LocText descriptionLabel;

	// Token: 0x040086C7 RID: 34503
	private List<ICheckboxListGroupControl> targets;

	// Token: 0x040086C8 RID: 34504
	private GameObject currentBuildTarget;

	// Token: 0x040086C9 RID: 34505
	private int uiRefreshSubHandle = -1;

	// Token: 0x040086CA RID: 34506
	private List<CheckboxListGroupSideScreen.CheckboxContainer> activeChecklistGroups = new List<CheckboxListGroupSideScreen.CheckboxContainer>();

	// Token: 0x02001FA5 RID: 8101
	public class CheckboxContainer
	{
		// Token: 0x0600AB3F RID: 43839 RVA: 0x00113BCC File Offset: 0x00111DCC
		public CheckboxContainer(HierarchyReferences container)
		{
			this.container = container;
		}

		// Token: 0x040086CB RID: 34507
		public HierarchyReferences container;

		// Token: 0x040086CC RID: 34508
		public List<HierarchyReferences> checkboxUIItems = new List<HierarchyReferences>();
	}
}
