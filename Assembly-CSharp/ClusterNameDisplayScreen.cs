using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001C76 RID: 7286
public class ClusterNameDisplayScreen : KScreen
{
	// Token: 0x06009796 RID: 38806 RVA: 0x00107361 File Offset: 0x00105561
	public static void DestroyInstance()
	{
		ClusterNameDisplayScreen.Instance = null;
	}

	// Token: 0x06009797 RID: 38807 RVA: 0x00107369 File Offset: 0x00105569
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ClusterNameDisplayScreen.Instance = this;
	}

	// Token: 0x06009798 RID: 38808 RVA: 0x00107377 File Offset: 0x00105577
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06009799 RID: 38809 RVA: 0x003B4560 File Offset: 0x003B2760
	public void AddNewEntry(ClusterGridEntity representedObject)
	{
		if (this.GetEntry(representedObject) != null)
		{
			return;
		}
		ClusterNameDisplayScreen.Entry entry = new ClusterNameDisplayScreen.Entry();
		entry.grid_entity = representedObject;
		GameObject gameObject = Util.KInstantiateUI(this.nameAndBarsPrefab, base.gameObject, true);
		entry.display_go = gameObject;
		gameObject.name = representedObject.name + " cluster overlay";
		entry.Name = representedObject.name;
		entry.refs = gameObject.GetComponent<HierarchyReferences>();
		entry.bars_go = entry.refs.GetReference<RectTransform>("Bars").gameObject;
		this.m_entries.Add(entry);
		if (representedObject.GetComponent<KSelectable>() != null)
		{
			this.UpdateName(representedObject);
			this.UpdateBars(representedObject);
		}
	}

	// Token: 0x0600979A RID: 38810 RVA: 0x003B4610 File Offset: 0x003B2810
	private void LateUpdate()
	{
		if (App.isLoading || App.IsExiting)
		{
			return;
		}
		int num = this.m_entries.Count;
		int i = 0;
		while (i < num)
		{
			if (this.m_entries[i].grid_entity != null && ClusterMapScreen.GetRevealLevel(this.m_entries[i].grid_entity) == ClusterRevealLevel.Visible)
			{
				Transform gridEntityNameTarget = ClusterMapScreen.Instance.GetGridEntityNameTarget(this.m_entries[i].grid_entity);
				if (gridEntityNameTarget != null)
				{
					Vector3 position = gridEntityNameTarget.GetPosition();
					this.m_entries[i].display_go.GetComponent<RectTransform>().SetPositionAndRotation(position, Quaternion.identity);
					this.m_entries[i].display_go.SetActive(this.m_entries[i].grid_entity.IsVisible && this.m_entries[i].grid_entity.ShowName());
				}
				else if (this.m_entries[i].display_go.activeSelf)
				{
					this.m_entries[i].display_go.SetActive(false);
				}
				this.UpdateBars(this.m_entries[i].grid_entity);
				if (this.m_entries[i].bars_go != null)
				{
					this.m_entries[i].bars_go.GetComponentsInChildren<KCollider2D>(false, this.workingList);
					foreach (KCollider2D kcollider2D in this.workingList)
					{
						kcollider2D.MarkDirty(false);
					}
				}
				i++;
			}
			else
			{
				UnityEngine.Object.Destroy(this.m_entries[i].display_go);
				num--;
				this.m_entries[i] = this.m_entries[num];
			}
		}
		this.m_entries.RemoveRange(num, this.m_entries.Count - num);
	}

	// Token: 0x0600979B RID: 38811 RVA: 0x003B4828 File Offset: 0x003B2A28
	public void UpdateName(ClusterGridEntity representedObject)
	{
		ClusterNameDisplayScreen.Entry entry = this.GetEntry(representedObject);
		if (entry == null)
		{
			return;
		}
		KSelectable component = representedObject.GetComponent<KSelectable>();
		entry.display_go.name = component.GetProperName() + " cluster overlay";
		LocText componentInChildren = entry.display_go.GetComponentInChildren<LocText>();
		if (componentInChildren != null)
		{
			componentInChildren.text = component.GetProperName();
		}
	}

	// Token: 0x0600979C RID: 38812 RVA: 0x003B4884 File Offset: 0x003B2A84
	private void UpdateBars(ClusterGridEntity representedObject)
	{
		ClusterNameDisplayScreen.Entry entry = this.GetEntry(representedObject);
		if (entry == null)
		{
			return;
		}
		GenericUIProgressBar componentInChildren = entry.bars_go.GetComponentInChildren<GenericUIProgressBar>(true);
		if (entry.grid_entity.ShowProgressBar())
		{
			if (!componentInChildren.gameObject.activeSelf)
			{
				componentInChildren.gameObject.SetActive(true);
			}
			componentInChildren.SetFillPercentage(entry.grid_entity.GetProgress());
			return;
		}
		if (componentInChildren.gameObject.activeSelf)
		{
			componentInChildren.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600979D RID: 38813 RVA: 0x003B48FC File Offset: 0x003B2AFC
	private ClusterNameDisplayScreen.Entry GetEntry(ClusterGridEntity entity)
	{
		return this.m_entries.Find((ClusterNameDisplayScreen.Entry entry) => entry.grid_entity == entity);
	}

	// Token: 0x04007615 RID: 30229
	public static ClusterNameDisplayScreen Instance;

	// Token: 0x04007616 RID: 30230
	public GameObject nameAndBarsPrefab;

	// Token: 0x04007617 RID: 30231
	[SerializeField]
	private Color selectedColor;

	// Token: 0x04007618 RID: 30232
	[SerializeField]
	private Color defaultColor;

	// Token: 0x04007619 RID: 30233
	private List<ClusterNameDisplayScreen.Entry> m_entries = new List<ClusterNameDisplayScreen.Entry>();

	// Token: 0x0400761A RID: 30234
	private List<KCollider2D> workingList = new List<KCollider2D>();

	// Token: 0x02001C77 RID: 7287
	private class Entry
	{
		// Token: 0x0400761B RID: 30235
		public string Name;

		// Token: 0x0400761C RID: 30236
		public ClusterGridEntity grid_entity;

		// Token: 0x0400761D RID: 30237
		public GameObject display_go;

		// Token: 0x0400761E RID: 30238
		public GameObject bars_go;

		// Token: 0x0400761F RID: 30239
		public HierarchyReferences refs;
	}
}
