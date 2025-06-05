using System;
using UnityEngine;

// Token: 0x02001D60 RID: 7520
[AddComponentMenu("KMonoBehaviour/scripts/InstantiateUIPrefabChild")]
public class InstantiateUIPrefabChild : KMonoBehaviour
{
	// Token: 0x06009D0C RID: 40204 RVA: 0x0010AA73 File Offset: 0x00108C73
	protected override void OnPrefabInit()
	{
		if (this.InstantiateOnAwake)
		{
			this.Instantiate();
		}
	}

	// Token: 0x06009D0D RID: 40205 RVA: 0x003D4264 File Offset: 0x003D2464
	public void Instantiate()
	{
		if (this.alreadyInstantiated)
		{
			global::Debug.LogWarning(base.gameObject.name + "trying to instantiate UI prefabs multiple times.");
			return;
		}
		this.alreadyInstantiated = true;
		foreach (GameObject gameObject in this.prefabs)
		{
			if (!(gameObject == null))
			{
				Vector3 v = gameObject.rectTransform().anchoredPosition;
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				gameObject2.transform.SetParent(base.transform);
				gameObject2.rectTransform().anchoredPosition = v;
				gameObject2.rectTransform().localScale = Vector3.one;
				if (this.setAsFirstSibling)
				{
					gameObject2.transform.SetAsFirstSibling();
				}
			}
		}
	}

	// Token: 0x04007B04 RID: 31492
	public GameObject[] prefabs;

	// Token: 0x04007B05 RID: 31493
	public bool InstantiateOnAwake = true;

	// Token: 0x04007B06 RID: 31494
	private bool alreadyInstantiated;

	// Token: 0x04007B07 RID: 31495
	public bool setAsFirstSibling;
}
