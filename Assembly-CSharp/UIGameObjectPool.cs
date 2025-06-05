using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001AEE RID: 6894
public class UIGameObjectPool
{
	// Token: 0x17000991 RID: 2449
	// (get) Token: 0x06009028 RID: 36904 RVA: 0x00102A7D File Offset: 0x00100C7D
	public int ActiveElementsCount
	{
		get
		{
			return this.activeElements.Count;
		}
	}

	// Token: 0x17000992 RID: 2450
	// (get) Token: 0x06009029 RID: 36905 RVA: 0x00102A8A File Offset: 0x00100C8A
	public int FreeElementsCount
	{
		get
		{
			return this.freeElements.Count;
		}
	}

	// Token: 0x17000993 RID: 2451
	// (get) Token: 0x0600902A RID: 36906 RVA: 0x00102A97 File Offset: 0x00100C97
	public int TotalElementsCount
	{
		get
		{
			return this.ActiveElementsCount + this.FreeElementsCount;
		}
	}

	// Token: 0x0600902B RID: 36907 RVA: 0x00102AA6 File Offset: 0x00100CA6
	public UIGameObjectPool(GameObject prefab)
	{
		this.prefab = prefab;
		this.freeElements = new List<GameObject>();
		this.activeElements = new List<GameObject>();
	}

	// Token: 0x0600902C RID: 36908 RVA: 0x00385F18 File Offset: 0x00384118
	public GameObject GetFreeElement(GameObject instantiateParent = null, bool forceActive = false)
	{
		if (this.freeElements.Count == 0)
		{
			this.activeElements.Add(Util.KInstantiateUI(this.prefab.gameObject, instantiateParent, false));
		}
		else
		{
			GameObject gameObject = this.freeElements[0];
			this.activeElements.Add(gameObject);
			if (gameObject.transform.parent != instantiateParent)
			{
				gameObject.transform.SetParent(instantiateParent.transform);
			}
			this.freeElements.RemoveAt(0);
		}
		GameObject gameObject2 = this.activeElements[this.activeElements.Count - 1];
		if (gameObject2.gameObject.activeInHierarchy != forceActive)
		{
			gameObject2.gameObject.SetActive(forceActive);
		}
		return gameObject2;
	}

	// Token: 0x0600902D RID: 36909 RVA: 0x00385FD0 File Offset: 0x003841D0
	public void ClearElement(GameObject element)
	{
		if (!this.activeElements.Contains(element))
		{
			object obj = this.freeElements.Contains(element) ? (element.name + ": The element provided is already inactive") : (element.name + ": The element provided does not belong to this pool");
			element.SetActive(false);
			if (this.disabledElementParent != null)
			{
				element.transform.SetParent(this.disabledElementParent);
			}
			global::Debug.LogError(obj);
			return;
		}
		if (this.disabledElementParent != null)
		{
			element.transform.SetParent(this.disabledElementParent);
		}
		element.SetActive(false);
		this.freeElements.Add(element);
		this.activeElements.Remove(element);
	}

	// Token: 0x0600902E RID: 36910 RVA: 0x00386088 File Offset: 0x00384288
	public void ClearAll()
	{
		while (this.activeElements.Count > 0)
		{
			if (this.disabledElementParent != null)
			{
				this.activeElements[0].transform.SetParent(this.disabledElementParent);
			}
			this.activeElements[0].SetActive(false);
			this.freeElements.Add(this.activeElements[0]);
			this.activeElements.RemoveAt(0);
		}
	}

	// Token: 0x0600902F RID: 36911 RVA: 0x00102AE1 File Offset: 0x00100CE1
	public void DestroyAll()
	{
		this.DestroyAllActive();
		this.DestroyAllFree();
	}

	// Token: 0x06009030 RID: 36912 RVA: 0x00102AEF File Offset: 0x00100CEF
	public void DestroyAllActive()
	{
		this.activeElements.ForEach(delegate(GameObject ae)
		{
			UnityEngine.Object.Destroy(ae);
		});
		this.activeElements.Clear();
	}

	// Token: 0x06009031 RID: 36913 RVA: 0x00102B26 File Offset: 0x00100D26
	public void DestroyAllFree()
	{
		this.freeElements.ForEach(delegate(GameObject ae)
		{
			UnityEngine.Object.Destroy(ae);
		});
		this.freeElements.Clear();
	}

	// Token: 0x06009032 RID: 36914 RVA: 0x00102B5D File Offset: 0x00100D5D
	public void ForEachActiveElement(Action<GameObject> predicate)
	{
		this.activeElements.ForEach(predicate);
	}

	// Token: 0x06009033 RID: 36915 RVA: 0x00102B6B File Offset: 0x00100D6B
	public void ForEachFreeElement(Action<GameObject> predicate)
	{
		this.freeElements.ForEach(predicate);
	}

	// Token: 0x04006CFF RID: 27903
	private GameObject prefab;

	// Token: 0x04006D00 RID: 27904
	private List<GameObject> freeElements = new List<GameObject>();

	// Token: 0x04006D01 RID: 27905
	private List<GameObject> activeElements = new List<GameObject>();

	// Token: 0x04006D02 RID: 27906
	public Transform disabledElementParent;
}
