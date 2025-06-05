using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001F6C RID: 8044
[AddComponentMenu("KMonoBehaviour/scripts/ScheduledUIInstantiation")]
public class ScheduledUIInstantiation : KMonoBehaviour
{
	// Token: 0x0600A9E1 RID: 43489 RVA: 0x00112B81 File Offset: 0x00110D81
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.InstantiateOnAwake)
		{
			this.InstantiateElements(null);
			return;
		}
		Game.Instance.Subscribe((int)this.InstantiationEvent, new Action<object>(this.InstantiateElements));
	}

	// Token: 0x0600A9E2 RID: 43490 RVA: 0x00413234 File Offset: 0x00411434
	public void InstantiateElements(object data)
	{
		if (this.completed)
		{
			return;
		}
		this.completed = true;
		foreach (ScheduledUIInstantiation.Instantiation instantiation in this.UIElements)
		{
			if (instantiation.RequiredDlcId.IsNullOrWhiteSpace() || Game.IsDlcActiveForCurrentSave(instantiation.RequiredDlcId))
			{
				foreach (GameObject gameObject in instantiation.prefabs)
				{
					Vector3 v = gameObject.rectTransform().anchoredPosition;
					GameObject gameObject2 = Util.KInstantiateUI(gameObject, instantiation.parent.gameObject, false);
					gameObject2.rectTransform().anchoredPosition = v;
					gameObject2.rectTransform().localScale = Vector3.one;
					this.instantiatedObjects.Add(gameObject2);
				}
			}
		}
		if (!this.InstantiateOnAwake)
		{
			base.Unsubscribe((int)this.InstantiationEvent, new Action<object>(this.InstantiateElements));
		}
	}

	// Token: 0x0600A9E3 RID: 43491 RVA: 0x00413324 File Offset: 0x00411524
	public T GetInstantiatedObject<T>() where T : Component
	{
		for (int i = 0; i < this.instantiatedObjects.Count; i++)
		{
			if (this.instantiatedObjects[i].GetComponent(typeof(T)) != null)
			{
				return this.instantiatedObjects[i].GetComponent(typeof(T)) as T;
			}
		}
		return default(T);
	}

	// Token: 0x040085C6 RID: 34246
	public ScheduledUIInstantiation.Instantiation[] UIElements;

	// Token: 0x040085C7 RID: 34247
	public bool InstantiateOnAwake;

	// Token: 0x040085C8 RID: 34248
	public GameHashes InstantiationEvent = GameHashes.StartGameUser;

	// Token: 0x040085C9 RID: 34249
	private bool completed;

	// Token: 0x040085CA RID: 34250
	private List<GameObject> instantiatedObjects = new List<GameObject>();

	// Token: 0x02001F6D RID: 8045
	[Serializable]
	public struct Instantiation
	{
		// Token: 0x040085CB RID: 34251
		public string Name;

		// Token: 0x040085CC RID: 34252
		public string Comment;

		// Token: 0x040085CD RID: 34253
		public GameObject[] prefabs;

		// Token: 0x040085CE RID: 34254
		public Transform parent;

		// Token: 0x040085CF RID: 34255
		public string RequiredDlcId;
	}
}
