using System;
using UnityEngine;

// Token: 0x02000946 RID: 2374
public class KAnimLink
{
	// Token: 0x06002A11 RID: 10769 RVA: 0x000BFD04 File Offset: 0x000BDF04
	public KAnimLink(KAnimControllerBase master, KAnimControllerBase slave)
	{
		this.slave = slave;
		this.master = master;
		this.Register();
	}

	// Token: 0x06002A12 RID: 10770 RVA: 0x001E4C84 File Offset: 0x001E2E84
	private void Register()
	{
		this.master.OnOverlayColourChanged += this.OnOverlayColourChanged;
		KAnimControllerBase kanimControllerBase = this.master;
		kanimControllerBase.OnTintChanged = (Action<Color>)Delegate.Combine(kanimControllerBase.OnTintChanged, new Action<Color>(this.OnTintColourChanged));
		KAnimControllerBase kanimControllerBase2 = this.master;
		kanimControllerBase2.OnHighlightChanged = (Action<Color>)Delegate.Combine(kanimControllerBase2.OnHighlightChanged, new Action<Color>(this.OnHighlightColourChanged));
		this.master.onLayerChanged += this.slave.SetLayer;
	}

	// Token: 0x06002A13 RID: 10771 RVA: 0x001E4D14 File Offset: 0x001E2F14
	public void Unregister()
	{
		if (this.master != null)
		{
			this.master.OnOverlayColourChanged -= this.OnOverlayColourChanged;
			KAnimControllerBase kanimControllerBase = this.master;
			kanimControllerBase.OnTintChanged = (Action<Color>)Delegate.Remove(kanimControllerBase.OnTintChanged, new Action<Color>(this.OnTintColourChanged));
			KAnimControllerBase kanimControllerBase2 = this.master;
			kanimControllerBase2.OnHighlightChanged = (Action<Color>)Delegate.Remove(kanimControllerBase2.OnHighlightChanged, new Action<Color>(this.OnHighlightColourChanged));
			if (this.slave != null)
			{
				this.master.onLayerChanged -= this.slave.SetLayer;
			}
		}
	}

	// Token: 0x06002A14 RID: 10772 RVA: 0x000BFD27 File Offset: 0x000BDF27
	private void OnOverlayColourChanged(Color32 c)
	{
		if (this.slave != null)
		{
			this.slave.OverlayColour = c;
		}
	}

	// Token: 0x06002A15 RID: 10773 RVA: 0x000BFD48 File Offset: 0x000BDF48
	private void OnTintColourChanged(Color c)
	{
		if (this.syncTint && this.slave != null)
		{
			this.slave.TintColour = c;
		}
	}

	// Token: 0x06002A16 RID: 10774 RVA: 0x000BFD71 File Offset: 0x000BDF71
	private void OnHighlightColourChanged(Color c)
	{
		if (this.slave != null)
		{
			this.slave.HighlightColour = c;
		}
	}

	// Token: 0x04001C8F RID: 7311
	public bool syncTint = true;

	// Token: 0x04001C90 RID: 7312
	private KAnimControllerBase master;

	// Token: 0x04001C91 RID: 7313
	private KAnimControllerBase slave;
}
