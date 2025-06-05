using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200063A RID: 1594
public class UIPrefabLocalPool
{
	// Token: 0x06001C50 RID: 7248 RVA: 0x000B709C File Offset: 0x000B529C
	public UIPrefabLocalPool(GameObject sourcePrefab, GameObject parent)
	{
		this.sourcePrefab = sourcePrefab;
		this.parent = parent;
	}

	// Token: 0x06001C51 RID: 7249 RVA: 0x001B87A4 File Offset: 0x001B69A4
	public GameObject Borrow()
	{
		GameObject gameObject;
		if (this.availableInstances.Count == 0)
		{
			gameObject = Util.KInstantiateUI(this.sourcePrefab, this.parent, true);
		}
		else
		{
			gameObject = this.availableInstances.First<KeyValuePair<int, GameObject>>().Value;
			this.availableInstances.Remove(gameObject.GetInstanceID());
		}
		this.checkedOutInstances.Add(gameObject.GetInstanceID(), gameObject);
		gameObject.SetActive(true);
		gameObject.transform.SetAsLastSibling();
		return gameObject;
	}

	// Token: 0x06001C52 RID: 7250 RVA: 0x000B70C8 File Offset: 0x000B52C8
	public void Return(GameObject instance)
	{
		this.checkedOutInstances.Remove(instance.GetInstanceID());
		this.availableInstances.Add(instance.GetInstanceID(), instance);
		instance.SetActive(false);
	}

	// Token: 0x06001C53 RID: 7251 RVA: 0x001B8820 File Offset: 0x001B6A20
	public void ReturnAll()
	{
		foreach (KeyValuePair<int, GameObject> keyValuePair in this.checkedOutInstances)
		{
			int num;
			GameObject gameObject;
			keyValuePair.Deconstruct(out num, out gameObject);
			int key = num;
			GameObject gameObject2 = gameObject;
			this.availableInstances.Add(key, gameObject2);
			gameObject2.SetActive(false);
		}
		this.checkedOutInstances.Clear();
	}

	// Token: 0x06001C54 RID: 7252 RVA: 0x000B70F5 File Offset: 0x000B52F5
	public IEnumerable<GameObject> GetBorrowedObjects()
	{
		return this.checkedOutInstances.Values;
	}

	// Token: 0x040011F5 RID: 4597
	public readonly GameObject sourcePrefab;

	// Token: 0x040011F6 RID: 4598
	public readonly GameObject parent;

	// Token: 0x040011F7 RID: 4599
	private Dictionary<int, GameObject> checkedOutInstances = new Dictionary<int, GameObject>();

	// Token: 0x040011F8 RID: 4600
	private Dictionary<int, GameObject> availableInstances = new Dictionary<int, GameObject>();
}
