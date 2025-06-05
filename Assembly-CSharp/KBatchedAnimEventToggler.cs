using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200094C RID: 2380
[AddComponentMenu("KMonoBehaviour/scripts/KBatchedAnimEventToggler")]
public class KBatchedAnimEventToggler : KMonoBehaviour
{
	// Token: 0x06002A7B RID: 10875 RVA: 0x001E6C4C File Offset: 0x001E4E4C
	protected override void OnPrefabInit()
	{
		Vector3 position = this.eventSource.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
		int layer = LayerMask.NameToLayer("Default");
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			entry.controller.transform.SetPosition(position);
			entry.controller.SetLayer(layer);
			entry.controller.gameObject.SetActive(false);
		}
		int hash = Hash.SDBMLower(this.enableEvent);
		int hash2 = Hash.SDBMLower(this.disableEvent);
		base.Subscribe(this.eventSource, hash, new Action<object>(this.Enable));
		base.Subscribe(this.eventSource, hash2, new Action<object>(this.Disable));
	}

	// Token: 0x06002A7C RID: 10876 RVA: 0x000C01F1 File Offset: 0x000BE3F1
	protected override void OnSpawn()
	{
		this.animEventHandler = base.GetComponentInParent<AnimEventHandler>();
	}

	// Token: 0x06002A7D RID: 10877 RVA: 0x001E6D3C File Offset: 0x001E4F3C
	private void Enable(object data)
	{
		this.StopAll();
		HashedString context = this.animEventHandler.GetContext();
		if (!context.IsValid)
		{
			return;
		}
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			if (entry.context == context)
			{
				entry.controller.gameObject.SetActive(true);
				entry.controller.Play(entry.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}
	}

	// Token: 0x06002A7E RID: 10878 RVA: 0x000C01FF File Offset: 0x000BE3FF
	private void Disable(object data)
	{
		this.StopAll();
	}

	// Token: 0x06002A7F RID: 10879 RVA: 0x001E6DE4 File Offset: 0x001E4FE4
	private void StopAll()
	{
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			entry.controller.StopAndClear();
			entry.controller.gameObject.SetActive(false);
		}
	}

	// Token: 0x04001CBB RID: 7355
	[SerializeField]
	public GameObject eventSource;

	// Token: 0x04001CBC RID: 7356
	[SerializeField]
	public string enableEvent;

	// Token: 0x04001CBD RID: 7357
	[SerializeField]
	public string disableEvent;

	// Token: 0x04001CBE RID: 7358
	[SerializeField]
	public List<KBatchedAnimEventToggler.Entry> entries;

	// Token: 0x04001CBF RID: 7359
	private AnimEventHandler animEventHandler;

	// Token: 0x0200094D RID: 2381
	[Serializable]
	public struct Entry
	{
		// Token: 0x04001CC0 RID: 7360
		public string anim;

		// Token: 0x04001CC1 RID: 7361
		public HashedString context;

		// Token: 0x04001CC2 RID: 7362
		public KBatchedAnimController controller;
	}
}
