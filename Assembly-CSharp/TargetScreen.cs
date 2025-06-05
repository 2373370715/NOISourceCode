using System;
using UnityEngine;

// Token: 0x0200208E RID: 8334
public abstract class TargetScreen : KScreen
{
	// Token: 0x0600B191 RID: 45457
	public abstract bool IsValidForTarget(GameObject target);

	// Token: 0x0600B192 RID: 45458 RVA: 0x00439790 File Offset: 0x00437990
	public virtual void SetTarget(GameObject target)
	{
		Console.WriteLine(target);
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

	// Token: 0x0600B193 RID: 45459 RVA: 0x00117E9A File Offset: 0x0011609A
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		this.SetTarget(null);
	}

	// Token: 0x0600B194 RID: 45460 RVA: 0x00117EA9 File Offset: 0x001160A9
	public virtual void OnSelectTarget(GameObject target)
	{
		target.Subscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x0600B195 RID: 45461 RVA: 0x00117EC3 File Offset: 0x001160C3
	public virtual void OnDeselectTarget(GameObject target)
	{
		target.Unsubscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x0600B196 RID: 45462 RVA: 0x00117EDC File Offset: 0x001160DC
	private void OnTargetDestroyed(object data)
	{
		DetailsScreen.Instance.Show(false);
		this.SetTarget(null);
	}

	// Token: 0x04008C08 RID: 35848
	protected GameObject selectedTarget;
}
