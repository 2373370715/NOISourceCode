using System;
using UnityEngine;

// Token: 0x02001F0B RID: 7947
public class PopIn : MonoBehaviour
{
	// Token: 0x0600A70C RID: 42764 RVA: 0x00110CFD File Offset: 0x0010EEFD
	private void OnEnable()
	{
		this.StartPopIn(true);
	}

	// Token: 0x0600A70D RID: 42765 RVA: 0x00402F50 File Offset: 0x00401150
	private void Update()
	{
		float num = Mathf.Lerp(base.transform.localScale.x, this.targetScale, Time.unscaledDeltaTime * this.speed);
		base.transform.localScale = new Vector3(num, num, 1f);
	}

	// Token: 0x0600A70E RID: 42766 RVA: 0x00110D06 File Offset: 0x0010EF06
	public void StartPopIn(bool force_reset = false)
	{
		if (force_reset)
		{
			base.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
		}
		this.targetScale = 1f;
	}

	// Token: 0x0600A70F RID: 42767 RVA: 0x00110D35 File Offset: 0x0010EF35
	public void StartPopOut()
	{
		this.targetScale = 0f;
	}

	// Token: 0x040082FD RID: 33533
	private float targetScale;

	// Token: 0x040082FE RID: 33534
	public float speed;
}
