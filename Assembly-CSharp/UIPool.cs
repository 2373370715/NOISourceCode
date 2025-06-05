using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001AF0 RID: 6896
public class UIPool<T> where T : MonoBehaviour
{
	// Token: 0x17000994 RID: 2452
	// (get) Token: 0x06009038 RID: 36920 RVA: 0x00102B8D File Offset: 0x00100D8D
	public int ActiveElementsCount
	{
		get
		{
			return this.activeElements.Count;
		}
	}

	// Token: 0x17000995 RID: 2453
	// (get) Token: 0x06009039 RID: 36921 RVA: 0x00102B9A File Offset: 0x00100D9A
	public int FreeElementsCount
	{
		get
		{
			return this.freeElements.Count;
		}
	}

	// Token: 0x17000996 RID: 2454
	// (get) Token: 0x0600903A RID: 36922 RVA: 0x00102BA7 File Offset: 0x00100DA7
	public int TotalElementsCount
	{
		get
		{
			return this.ActiveElementsCount + this.FreeElementsCount;
		}
	}

	// Token: 0x0600903B RID: 36923 RVA: 0x00102BB6 File Offset: 0x00100DB6
	public UIPool(T prefab)
	{
		this.prefab = prefab;
		this.freeElements = new List<T>();
		this.activeElements = new List<T>();
	}

	// Token: 0x0600903C RID: 36924 RVA: 0x00386104 File Offset: 0x00384304
	public T GetFreeElement(GameObject instantiateParent = null, bool forceActive = false)
	{
		if (this.freeElements.Count == 0)
		{
			this.activeElements.Add(Util.KInstantiateUI<T>(this.prefab.gameObject, instantiateParent, false));
		}
		else
		{
			T t = this.freeElements[0];
			this.activeElements.Add(t);
			if (t.transform.parent != instantiateParent)
			{
				t.transform.SetParent(instantiateParent.transform);
			}
			this.freeElements.RemoveAt(0);
		}
		T t2 = this.activeElements[this.activeElements.Count - 1];
		if (t2.gameObject.activeInHierarchy != forceActive)
		{
			t2.gameObject.SetActive(forceActive);
		}
		return t2;
	}

	// Token: 0x0600903D RID: 36925 RVA: 0x003861D4 File Offset: 0x003843D4
	public void ClearElement(T element)
	{
		if (!this.activeElements.Contains(element))
		{
			global::Debug.LogError(this.freeElements.Contains(element) ? "The element provided is already inactive" : "The element provided does not belong to this pool");
			return;
		}
		if (this.disabledElementParent != null)
		{
			element.gameObject.transform.SetParent(this.disabledElementParent);
		}
		element.gameObject.SetActive(false);
		this.freeElements.Add(element);
		this.activeElements.Remove(element);
	}

	// Token: 0x0600903E RID: 36926 RVA: 0x00386264 File Offset: 0x00384464
	public void ClearAll()
	{
		while (this.activeElements.Count > 0)
		{
			if (this.disabledElementParent != null)
			{
				this.activeElements[0].gameObject.transform.SetParent(this.disabledElementParent);
			}
			this.activeElements[0].gameObject.SetActive(false);
			this.freeElements.Add(this.activeElements[0]);
			this.activeElements.RemoveAt(0);
		}
	}

	// Token: 0x0600903F RID: 36927 RVA: 0x00102BF1 File Offset: 0x00100DF1
	public void DestroyAll()
	{
		this.DestroyAllActive();
		this.DestroyAllFree();
	}

	// Token: 0x06009040 RID: 36928 RVA: 0x00102BFF File Offset: 0x00100DFF
	public void DestroyAllActive()
	{
		this.activeElements.ForEach(delegate(T ae)
		{
			UnityEngine.Object.Destroy(ae.gameObject);
		});
		this.activeElements.Clear();
	}

	// Token: 0x06009041 RID: 36929 RVA: 0x00102C36 File Offset: 0x00100E36
	public void DestroyAllFree()
	{
		this.freeElements.ForEach(delegate(T ae)
		{
			UnityEngine.Object.Destroy(ae.gameObject);
		});
		this.freeElements.Clear();
	}

	// Token: 0x06009042 RID: 36930 RVA: 0x00102C6D File Offset: 0x00100E6D
	public void ForEachActiveElement(Action<T> predicate)
	{
		this.activeElements.ForEach(predicate);
	}

	// Token: 0x06009043 RID: 36931 RVA: 0x00102C7B File Offset: 0x00100E7B
	public void ForEachFreeElement(Action<T> predicate)
	{
		this.freeElements.ForEach(predicate);
	}

	// Token: 0x04006D06 RID: 27910
	private T prefab;

	// Token: 0x04006D07 RID: 27911
	private List<T> freeElements = new List<T>();

	// Token: 0x04006D08 RID: 27912
	private List<T> activeElements = new List<T>();

	// Token: 0x04006D09 RID: 27913
	public Transform disabledElementParent;
}
