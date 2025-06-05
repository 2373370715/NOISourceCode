using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001C48 RID: 7240
public class EasingAnimations : MonoBehaviour
{
	// Token: 0x170009C5 RID: 2501
	// (get) Token: 0x06009676 RID: 38518 RVA: 0x0010671D File Offset: 0x0010491D
	public bool IsPlaying
	{
		get
		{
			return this.animationCoroutine != null;
		}
	}

	// Token: 0x06009677 RID: 38519 RVA: 0x00106728 File Offset: 0x00104928
	private void Start()
	{
		if (this.animationMap == null || this.animationMap.Count == 0)
		{
			this.Initialize();
		}
	}

	// Token: 0x06009678 RID: 38520 RVA: 0x003AC65C File Offset: 0x003AA85C
	private void Initialize()
	{
		this.animationMap = new Dictionary<string, EasingAnimations.AnimationScales>();
		foreach (EasingAnimations.AnimationScales animationScales in this.scales)
		{
			this.animationMap.Add(animationScales.name, animationScales);
		}
	}

	// Token: 0x06009679 RID: 38521 RVA: 0x003AC6A4 File Offset: 0x003AA8A4
	public void PlayAnimation(string animationName, float delay = 0f)
	{
		if (this.animationMap == null || this.animationMap.Count == 0)
		{
			this.Initialize();
		}
		if (!this.animationMap.ContainsKey(animationName))
		{
			return;
		}
		if (this.animationCoroutine != null)
		{
			base.StopCoroutine(this.animationCoroutine);
		}
		this.currentAnimation = this.animationMap[animationName];
		this.currentAnimation.currentScale = this.currentAnimation.startScale;
		base.transform.localScale = Vector3.one * this.currentAnimation.currentScale;
		this.animationCoroutine = base.StartCoroutine(this.ExecuteAnimation(delay));
	}

	// Token: 0x0600967A RID: 38522 RVA: 0x00106745 File Offset: 0x00104945
	private IEnumerator ExecuteAnimation(float delay)
	{
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < startTime + delay)
		{
			yield return SequenceUtil.WaitForNextFrame;
		}
		startTime = Time.realtimeSinceStartup;
		bool keepAnimating = true;
		while (keepAnimating)
		{
			float num = Time.realtimeSinceStartup - startTime;
			this.currentAnimation.currentScale = this.GetEasing(num * this.currentAnimation.easingMultiplier);
			if (this.currentAnimation.endScale > this.currentAnimation.startScale)
			{
				keepAnimating = (this.currentAnimation.currentScale < this.currentAnimation.endScale - 0.025f);
			}
			else
			{
				keepAnimating = (this.currentAnimation.currentScale > this.currentAnimation.endScale + 0.025f);
			}
			if (!keepAnimating)
			{
				this.currentAnimation.currentScale = this.currentAnimation.endScale;
			}
			base.transform.localScale = Vector3.one * this.currentAnimation.currentScale;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.animationCoroutine = null;
		if (this.OnAnimationDone != null)
		{
			this.OnAnimationDone(this.currentAnimation.name);
		}
		yield break;
	}

	// Token: 0x0600967B RID: 38523 RVA: 0x003AC74C File Offset: 0x003AA94C
	private float GetEasing(float t)
	{
		EasingAnimations.AnimationScales.AnimationType type = this.currentAnimation.type;
		if (type == EasingAnimations.AnimationScales.AnimationType.EaseOutBack)
		{
			return this.EaseOutBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
		}
		if (type == EasingAnimations.AnimationScales.AnimationType.EaseInBack)
		{
			return this.EaseInBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
		}
		return this.EaseInOutBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
	}

	// Token: 0x0600967C RID: 38524 RVA: 0x003AC7C8 File Offset: 0x003AA9C8
	public float EaseInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end * 0.5f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end * 0.5f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x0600967D RID: 38525 RVA: 0x003AC844 File Offset: 0x003AAA44
	public float EaseInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x0600967E RID: 38526 RVA: 0x003AC878 File Offset: 0x003AAA78
	public float EaseOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value -= 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x040074CE RID: 29902
	public EasingAnimations.AnimationScales[] scales;

	// Token: 0x040074CF RID: 29903
	private EasingAnimations.AnimationScales currentAnimation;

	// Token: 0x040074D0 RID: 29904
	private Coroutine animationCoroutine;

	// Token: 0x040074D1 RID: 29905
	private Dictionary<string, EasingAnimations.AnimationScales> animationMap;

	// Token: 0x040074D2 RID: 29906
	public Action<string> OnAnimationDone;

	// Token: 0x02001C49 RID: 7241
	[Serializable]
	public struct AnimationScales
	{
		// Token: 0x040074D3 RID: 29907
		public string name;

		// Token: 0x040074D4 RID: 29908
		public float startScale;

		// Token: 0x040074D5 RID: 29909
		public float endScale;

		// Token: 0x040074D6 RID: 29910
		public EasingAnimations.AnimationScales.AnimationType type;

		// Token: 0x040074D7 RID: 29911
		public float easingMultiplier;

		// Token: 0x040074D8 RID: 29912
		[HideInInspector]
		public float currentScale;

		// Token: 0x02001C4A RID: 7242
		public enum AnimationType
		{
			// Token: 0x040074DA RID: 29914
			EaseInOutBack,
			// Token: 0x040074DB RID: 29915
			EaseOutBack,
			// Token: 0x040074DC RID: 29916
			EaseInBack
		}
	}
}
