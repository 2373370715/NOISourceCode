using System;
using UnityEngine;

// Token: 0x0200128A RID: 4746
public class DreamBubble : KMonoBehaviour
{
	// Token: 0x170005CF RID: 1487
	// (get) Token: 0x060060E6 RID: 24806 RVA: 0x000E3862 File Offset: 0x000E1A62
	// (set) Token: 0x060060E5 RID: 24805 RVA: 0x000E3859 File Offset: 0x000E1A59
	public bool IsVisible { get; private set; }

	// Token: 0x060060E7 RID: 24807 RVA: 0x000E386A File Offset: 0x000E1A6A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.dreamBackgroundComponent.SetSymbolVisiblity(this.snapToPivotSymbol, false);
		this.SetVisibility(false);
	}

	// Token: 0x060060E8 RID: 24808 RVA: 0x002BDB10 File Offset: 0x002BBD10
	public void Tick(float dt)
	{
		if (this._currentDream != null && this._currentDream.Icons.Length != 0)
		{
			float num = this._timePassedSinceDreamStarted / this._currentDream.secondPerImage;
			int num2 = Mathf.FloorToInt(num);
			float num3 = num - (float)num2;
			int num4 = (int)Mathf.Repeat((float)Mathf.FloorToInt(num), (float)this._currentDream.Icons.Length);
			if (this.dreamContentComponent.sprite != this._currentDream.Icons[num4])
			{
				this.dreamContentComponent.sprite = this._currentDream.Icons[num4];
			}
			this.dreamContentComponent.rectTransform.localScale = Vector3.one * num3;
			this._color.a = (Mathf.Sin(num3 * 6.2831855f - 1.5707964f) + 1f) * 0.5f;
			this.dreamContentComponent.color = this._color;
			this._timePassedSinceDreamStarted += dt;
		}
	}

	// Token: 0x060060E9 RID: 24809 RVA: 0x002BDC0C File Offset: 0x002BBE0C
	public void SetDream(Dream dream)
	{
		this._currentDream = dream;
		this.dreamBackgroundComponent.Stop();
		this.dreamBackgroundComponent.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(dream.BackgroundAnim)
		};
		this.dreamContentComponent.color = this._color;
		this.dreamContentComponent.enabled = (dream != null && dream.Icons != null && dream.Icons.Length != 0);
		this._timePassedSinceDreamStarted = 0f;
		this._color.a = 0f;
	}

	// Token: 0x060060EA RID: 24810 RVA: 0x002BDCA0 File Offset: 0x002BBEA0
	public void SetVisibility(bool visible)
	{
		this.IsVisible = visible;
		this.dreamBackgroundComponent.SetVisiblity(visible);
		this.dreamContentComponent.gameObject.SetActive(visible);
		if (visible)
		{
			if (this._currentDream != null)
			{
				this.dreamBackgroundComponent.Play("dream_loop", KAnim.PlayMode.Loop, 1f, 0f);
			}
			this.dreamBubbleBorderKanim.Play("dream_bubble_loop", KAnim.PlayMode.Loop, 1f, 0f);
			this.maskKanim.Play("dream_bubble_mask", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		this.dreamBackgroundComponent.Stop();
		this.maskKanim.Stop();
		this.dreamBubbleBorderKanim.Stop();
	}

	// Token: 0x060060EB RID: 24811 RVA: 0x000E3890 File Offset: 0x000E1A90
	public void StopDreaming()
	{
		this._currentDream = null;
		this.SetVisibility(false);
	}

	// Token: 0x0400453C RID: 17724
	public KBatchedAnimController dreamBackgroundComponent;

	// Token: 0x0400453D RID: 17725
	public KBatchedAnimController maskKanim;

	// Token: 0x0400453E RID: 17726
	public KBatchedAnimController dreamBubbleBorderKanim;

	// Token: 0x0400453F RID: 17727
	public KImage dreamContentComponent;

	// Token: 0x04004540 RID: 17728
	private const string dreamBackgroundAnimationName = "dream_loop";

	// Token: 0x04004541 RID: 17729
	private const string dreamMaskAnimationName = "dream_bubble_mask";

	// Token: 0x04004542 RID: 17730
	private const string dreamBubbleBorderAnimationName = "dream_bubble_loop";

	// Token: 0x04004543 RID: 17731
	private HashedString snapToPivotSymbol = new HashedString("snapto_pivot");

	// Token: 0x04004545 RID: 17733
	private Dream _currentDream;

	// Token: 0x04004546 RID: 17734
	private float _timePassedSinceDreamStarted;

	// Token: 0x04004547 RID: 17735
	private Color _color = Color.white;

	// Token: 0x04004548 RID: 17736
	private const float PI_2 = 6.2831855f;

	// Token: 0x04004549 RID: 17737
	private const float HALF_PI = 1.5707964f;
}
