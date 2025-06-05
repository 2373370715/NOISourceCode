using System;
using UnityEngine;

// Token: 0x02001D4D RID: 7501
public class HealthBar : ProgressBar
{
	// Token: 0x17000A52 RID: 2642
	// (get) Token: 0x06009CA3 RID: 40099 RVA: 0x0010A558 File Offset: 0x00108758
	private bool ShouldShow
	{
		get
		{
			return this.showTimer > 0f || base.PercentFull < this.alwaysShowThreshold;
		}
	}

	// Token: 0x06009CA4 RID: 40100 RVA: 0x0010A577 File Offset: 0x00108777
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.barColor = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
		base.gameObject.SetActive(this.ShouldShow);
	}

	// Token: 0x06009CA5 RID: 40101 RVA: 0x0010A5A5 File Offset: 0x001087A5
	public void OnChange()
	{
		base.enabled = true;
		this.showTimer = this.maxShowTime;
	}

	// Token: 0x06009CA6 RID: 40102 RVA: 0x003D30D8 File Offset: 0x003D12D8
	public override void Update()
	{
		base.Update();
		if (Time.timeScale > 0f)
		{
			this.showTimer = Mathf.Max(0f, this.showTimer - Time.unscaledDeltaTime);
		}
		if (!this.ShouldShow)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06009CA7 RID: 40103 RVA: 0x0010A5BA File Offset: 0x001087BA
	private void OnBecameInvisible()
	{
		base.enabled = false;
	}

	// Token: 0x06009CA8 RID: 40104 RVA: 0x0010A5C3 File Offset: 0x001087C3
	private void OnBecameVisible()
	{
		base.enabled = true;
	}

	// Token: 0x06009CA9 RID: 40105 RVA: 0x003D3128 File Offset: 0x003D1328
	public override void OnOverlayChanged(object data = null)
	{
		if (!this.autoHide)
		{
			return;
		}
		if ((HashedString)data == OverlayModes.None.ID)
		{
			if (!base.gameObject.activeSelf && this.ShouldShow)
			{
				base.enabled = true;
				base.gameObject.SetActive(true);
				return;
			}
		}
		else if (base.gameObject.activeSelf)
		{
			base.enabled = false;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04007ABD RID: 31421
	private float showTimer;

	// Token: 0x04007ABE RID: 31422
	private float maxShowTime = 10f;

	// Token: 0x04007ABF RID: 31423
	private float alwaysShowThreshold = 0.8f;
}
