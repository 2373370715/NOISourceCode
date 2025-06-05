using System;
using UnityEngine;

// Token: 0x02001C4F RID: 7247
public class SizePulse : MonoBehaviour
{
	// Token: 0x0600969A RID: 38554 RVA: 0x003ACFD0 File Offset: 0x003AB1D0
	private void Start()
	{
		if (base.GetComponents<SizePulse>().Length > 1)
		{
			UnityEngine.Object.Destroy(this);
		}
		RectTransform rectTransform = (RectTransform)base.transform;
		this.from = rectTransform.localScale;
		this.cur = this.from;
		this.to = this.from * this.multiplier;
	}

	// Token: 0x0600969B RID: 38555 RVA: 0x003AD030 File Offset: 0x003AB230
	private void Update()
	{
		float num = this.updateWhenPaused ? Time.unscaledDeltaTime : Time.deltaTime;
		num *= this.speed;
		SizePulse.State state = this.state;
		if (state != SizePulse.State.Up)
		{
			if (state == SizePulse.State.Down)
			{
				this.cur = Vector2.Lerp(this.cur, this.from, num);
				if ((this.from - this.cur).sqrMagnitude < 0.0001f)
				{
					this.cur = this.from;
					this.state = SizePulse.State.Finished;
					if (this.onComplete != null)
					{
						this.onComplete();
					}
				}
			}
		}
		else
		{
			this.cur = Vector2.Lerp(this.cur, this.to, num);
			if ((this.to - this.cur).sqrMagnitude < 0.0001f)
			{
				this.cur = this.to;
				this.state = SizePulse.State.Down;
			}
		}
		((RectTransform)base.transform).localScale = new Vector3(this.cur.x, this.cur.y, 1f);
	}

	// Token: 0x040074FA RID: 29946
	public System.Action onComplete;

	// Token: 0x040074FB RID: 29947
	public Vector2 from = Vector2.one;

	// Token: 0x040074FC RID: 29948
	public Vector2 to = Vector2.one;

	// Token: 0x040074FD RID: 29949
	public float multiplier = 1.25f;

	// Token: 0x040074FE RID: 29950
	public float speed = 1f;

	// Token: 0x040074FF RID: 29951
	public bool updateWhenPaused;

	// Token: 0x04007500 RID: 29952
	private Vector2 cur;

	// Token: 0x04007501 RID: 29953
	private SizePulse.State state;

	// Token: 0x02001C50 RID: 7248
	private enum State
	{
		// Token: 0x04007503 RID: 29955
		Up,
		// Token: 0x04007504 RID: 29956
		Down,
		// Token: 0x04007505 RID: 29957
		Finished
	}
}
