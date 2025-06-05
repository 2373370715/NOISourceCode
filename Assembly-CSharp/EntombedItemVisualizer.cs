using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012E5 RID: 4837
[AddComponentMenu("KMonoBehaviour/scripts/EntombedItemVisualizer")]
public class EntombedItemVisualizer : KMonoBehaviour
{
	// Token: 0x0600633A RID: 25402 RVA: 0x000E5224 File Offset: 0x000E3424
	public void Clear()
	{
		this.cellEntombedCounts.Clear();
	}

	// Token: 0x0600633B RID: 25403 RVA: 0x000E5231 File Offset: 0x000E3431
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.entombedItemPool = new GameObjectPool(new Func<GameObject>(this.InstantiateEntombedObject), 32);
	}

	// Token: 0x0600633C RID: 25404 RVA: 0x002C7B68 File Offset: 0x002C5D68
	public bool AddItem(int cell)
	{
		bool result = false;
		if (Grid.Objects[cell, 9] == null)
		{
			result = true;
			EntombedItemVisualizer.Data data;
			this.cellEntombedCounts.TryGetValue(cell, out data);
			if (data.refCount == 0)
			{
				GameObject instance = this.entombedItemPool.GetInstance();
				instance.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront));
				instance.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.value * 360f);
				KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
				int num = UnityEngine.Random.Range(0, EntombedItemVisualizer.EntombedVisualizerAnims.Length);
				string text = EntombedItemVisualizer.EntombedVisualizerAnims[num];
				component.initialAnim = text;
				instance.SetActive(true);
				component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
				data.controller = component;
			}
			data.refCount++;
			this.cellEntombedCounts[cell] = data;
		}
		return result;
	}

	// Token: 0x0600633D RID: 25405 RVA: 0x002C7C58 File Offset: 0x002C5E58
	public void RemoveItem(int cell)
	{
		EntombedItemVisualizer.Data data;
		if (this.cellEntombedCounts.TryGetValue(cell, out data))
		{
			data.refCount--;
			if (data.refCount == 0)
			{
				this.ReleaseVisualizer(cell, data);
				return;
			}
			this.cellEntombedCounts[cell] = data;
		}
	}

	// Token: 0x0600633E RID: 25406 RVA: 0x002C7CA0 File Offset: 0x002C5EA0
	public void ForceClear(int cell)
	{
		EntombedItemVisualizer.Data data;
		if (this.cellEntombedCounts.TryGetValue(cell, out data))
		{
			this.ReleaseVisualizer(cell, data);
		}
	}

	// Token: 0x0600633F RID: 25407 RVA: 0x002C7CC8 File Offset: 0x002C5EC8
	private void ReleaseVisualizer(int cell, EntombedItemVisualizer.Data data)
	{
		if (data.controller != null)
		{
			data.controller.gameObject.SetActive(false);
			this.entombedItemPool.ReleaseInstance(data.controller.gameObject);
		}
		this.cellEntombedCounts.Remove(cell);
	}

	// Token: 0x06006340 RID: 25408 RVA: 0x000E5252 File Offset: 0x000E3452
	public bool IsEntombedItem(int cell)
	{
		return this.cellEntombedCounts.ContainsKey(cell) && this.cellEntombedCounts[cell].refCount > 0;
	}

	// Token: 0x06006341 RID: 25409 RVA: 0x000E5278 File Offset: 0x000E3478
	private GameObject InstantiateEntombedObject()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.entombedItemPrefab, Grid.SceneLayer.FXFront, null, 0);
		gameObject.SetActive(false);
		return gameObject;
	}

	// Token: 0x04004725 RID: 18213
	[SerializeField]
	private GameObject entombedItemPrefab;

	// Token: 0x04004726 RID: 18214
	private static readonly string[] EntombedVisualizerAnims = new string[]
	{
		"idle1",
		"idle2",
		"idle3",
		"idle4"
	};

	// Token: 0x04004727 RID: 18215
	private GameObjectPool entombedItemPool;

	// Token: 0x04004728 RID: 18216
	private Dictionary<int, EntombedItemVisualizer.Data> cellEntombedCounts = new Dictionary<int, EntombedItemVisualizer.Data>();

	// Token: 0x020012E6 RID: 4838
	private struct Data
	{
		// Token: 0x04004729 RID: 18217
		public int refCount;

		// Token: 0x0400472A RID: 18218
		public KBatchedAnimController controller;
	}
}
