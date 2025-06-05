using System;
using System.Collections.Generic;
using TemplateClasses;
using UnityEngine;

// Token: 0x0200149C RID: 5276
public class StampToolPreview_Placers : IStampToolPreviewPlugin
{
	// Token: 0x06006D45 RID: 27973 RVA: 0x002F7C50 File Offset: 0x002F5E50
	public StampToolPreview_Placers(GameObject placerPrefab)
	{
		StampToolPreview_Placers <>4__this = this;
		this.pool = new GameObjectPool(delegate()
		{
			if (<>4__this.poolParent == null)
			{
				<>4__this.poolParent = new GameObject("StampToolPreview::PlacerPool").transform;
			}
			GameObject gameObject = Util.KInstantiate(placerPrefab, <>4__this.poolParent.gameObject, null);
			gameObject.SetActive(false);
			return gameObject;
		}, 0);
	}

	// Token: 0x06006D46 RID: 27974 RVA: 0x002F7C9C File Offset: 0x002F5E9C
	public void Setup(StampToolPreviewContext context)
	{
		for (int i = 0; i < context.stampTemplate.cells.Count; i++)
		{
			Cell cell = context.stampTemplate.cells[i];
			GameObject instance = this.pool.GetInstance();
			instance.transform.SetParent(context.previewParent.transform, false);
			instance.transform.localPosition = new Vector3((float)cell.location_x, (float)cell.location_y);
			instance.SetActive(true);
			this.inUse.Add(instance);
		}
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, new Action<string>(delegate(string error)
		{
			foreach (GameObject gameObject in this.inUse)
			{
				if (!gameObject.IsNullOrDestroyed())
				{
					gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.color = ((error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK);
				}
			}
		}));
		context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, new System.Action(delegate()
		{
			foreach (GameObject gameObject in this.inUse)
			{
				if (!gameObject.IsNullOrDestroyed())
				{
					gameObject.SetActive(false);
					gameObject.transform.SetParent(this.poolParent);
					this.pool.ReleaseInstance(gameObject);
				}
			}
			this.inUse.Clear();
		}));
	}

	// Token: 0x0400526D RID: 21101
	private List<GameObject> inUse = new List<GameObject>();

	// Token: 0x0400526E RID: 21102
	private GameObjectPool pool;

	// Token: 0x0400526F RID: 21103
	private Transform poolParent;
}
