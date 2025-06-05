using System;
using UnityEngine;

// Token: 0x0200208D RID: 8333
public abstract class TargetPanel : KMonoBehaviour
{
	// Token: 0x0600B18B RID: 45451
	public abstract bool IsValidForTarget(GameObject target);

	// Token: 0x0600B18C RID: 45452 RVA: 0x00439738 File Offset: 0x00437938
	public virtual void SetTarget(GameObject target)
	{
		if (this.selectedTarget != target)
		{
			if (this.selectedTarget != null)
			{
				this.OnDeselectTarget(this.selectedTarget);
			}
			this.selectedTarget = target;
			if (this.selectedTarget != null)
			{
				this.OnSelectTarget(this.selectedTarget);
			}
		}
	}

	// Token: 0x0600B18D RID: 45453 RVA: 0x00117E53 File Offset: 0x00116053
	protected virtual void OnSelectTarget(GameObject target)
	{
		target.Subscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x0600B18E RID: 45454 RVA: 0x00117E6D File Offset: 0x0011606D
	public virtual void OnDeselectTarget(GameObject target)
	{
		target.Unsubscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x0600B18F RID: 45455 RVA: 0x00117E86 File Offset: 0x00116086
	private void OnTargetDestroyed(object data)
	{
		DetailsScreen.Instance.Show(false);
		this.SetTarget(null);
	}

	// Token: 0x04008C07 RID: 35847
	protected GameObject selectedTarget;
}
