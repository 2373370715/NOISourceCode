using System;

// Token: 0x02001EB8 RID: 7864
public class NotificationHighlightTarget : KMonoBehaviour
{
	// Token: 0x0600A4FF RID: 42239 RVA: 0x0010F7E5 File Offset: 0x0010D9E5
	protected void OnEnable()
	{
		this.controller = base.GetComponentInParent<NotificationHighlightController>();
		if (this.controller != null)
		{
			this.controller.AddTarget(this);
		}
	}

	// Token: 0x0600A500 RID: 42240 RVA: 0x0010F80D File Offset: 0x0010DA0D
	protected override void OnDisable()
	{
		if (this.controller != null)
		{
			this.controller.RemoveTarget(this);
		}
	}

	// Token: 0x0600A501 RID: 42241 RVA: 0x0010F829 File Offset: 0x0010DA29
	public void View()
	{
		base.GetComponentInParent<NotificationHighlightController>().TargetViewed(this);
	}

	// Token: 0x04008108 RID: 33032
	public string targetKey;

	// Token: 0x04008109 RID: 33033
	private NotificationHighlightController controller;
}
