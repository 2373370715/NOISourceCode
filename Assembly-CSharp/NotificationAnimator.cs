using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E57 RID: 7767
public class NotificationAnimator : MonoBehaviour
{
	// Token: 0x0600A2A5 RID: 41637 RVA: 0x0010E24D File Offset: 0x0010C44D
	public void Begin(bool startOffset = true)
	{
		this.Reset();
		this.animating = true;
		if (startOffset)
		{
			this.layoutElement.minWidth = 100f;
			return;
		}
		this.layoutElement.minWidth = 1f;
		this.speed = -10f;
	}

	// Token: 0x0600A2A6 RID: 41638 RVA: 0x0010E28B File Offset: 0x0010C48B
	private void Reset()
	{
		this.bounceCount = 2;
		this.layoutElement = base.GetComponent<LayoutElement>();
		this.layoutElement.minWidth = 0f;
		this.speed = 1f;
	}

	// Token: 0x0600A2A7 RID: 41639 RVA: 0x0010E2BB File Offset: 0x0010C4BB
	public void Stop()
	{
		this.Reset();
		this.animating = false;
	}

	// Token: 0x0600A2A8 RID: 41640 RVA: 0x003EDFCC File Offset: 0x003EC1CC
	private void LateUpdate()
	{
		if (!this.animating)
		{
			return;
		}
		this.layoutElement.minWidth -= this.speed;
		this.speed += 0.5f;
		if (this.layoutElement.minWidth <= 0f)
		{
			if (this.bounceCount > 0)
			{
				this.bounceCount--;
				this.speed = -this.speed / Mathf.Pow(2f, (float)(2 - this.bounceCount));
				this.layoutElement.minWidth = -this.speed;
				return;
			}
			this.layoutElement.minWidth = 0f;
			this.Stop();
		}
	}

	// Token: 0x04007F50 RID: 32592
	private const float START_SPEED = 1f;

	// Token: 0x04007F51 RID: 32593
	private const float ACCELERATION = 0.5f;

	// Token: 0x04007F52 RID: 32594
	private const float BOUNCE_DAMPEN = 2f;

	// Token: 0x04007F53 RID: 32595
	private const int BOUNCE_COUNT = 2;

	// Token: 0x04007F54 RID: 32596
	private const float OFFSETX = 100f;

	// Token: 0x04007F55 RID: 32597
	private float speed = 1f;

	// Token: 0x04007F56 RID: 32598
	private int bounceCount = 2;

	// Token: 0x04007F57 RID: 32599
	private LayoutElement layoutElement;

	// Token: 0x04007F58 RID: 32600
	[SerializeField]
	private bool animating = true;
}
