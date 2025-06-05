using System;
using UnityEngine;

// Token: 0x020016D2 RID: 5842
[AddComponentMenu("KMonoBehaviour/scripts/OneshotReactableHost")]
public class OneshotReactableHost : KMonoBehaviour
{
	// Token: 0x0600788B RID: 30859 RVA: 0x000F3C21 File Offset: 0x000F1E21
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("CleanupOneshotReactable", this.lifetime, new Action<object>(this.OnExpire), null, null);
	}

	// Token: 0x0600788C RID: 30860 RVA: 0x000F3C4D File Offset: 0x000F1E4D
	public void SetReactable(Reactable reactable)
	{
		this.reactable = reactable;
	}

	// Token: 0x0600788D RID: 30861 RVA: 0x0031FD60 File Offset: 0x0031DF60
	private void OnExpire(object obj)
	{
		if (!this.reactable.IsReacting)
		{
			this.reactable.Cleanup();
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GameScheduler.Instance.Schedule("CleanupOneshotReactable", 0.5f, new Action<object>(this.OnExpire), null, null);
	}

	// Token: 0x04005A8E RID: 23182
	private Reactable reactable;

	// Token: 0x04005A8F RID: 23183
	public float lifetime = 1f;
}
