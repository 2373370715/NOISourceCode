using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001D56 RID: 7510
public class IncrementorToggle : MultiToggle
{
	// Token: 0x06009CC2 RID: 40130 RVA: 0x003D34C8 File Offset: 0x003D16C8
	protected override void Update()
	{
		if (this.clickHeldDown)
		{
			this.totalHeldTime += Time.unscaledDeltaTime;
			if (this.timeToNextIncrement <= 0f)
			{
				this.PlayClickSound();
				this.onClick();
				this.timeToNextIncrement = Mathf.Lerp(this.timeBetweenIncrementsMax, this.timeBetweenIncrementsMin, this.totalHeldTime / 2.5f);
				return;
			}
			this.timeToNextIncrement -= Time.unscaledDeltaTime;
		}
	}

	// Token: 0x06009CC3 RID: 40131 RVA: 0x003D3544 File Offset: 0x003D1744
	private void PlayClickSound()
	{
		if (this.play_sound_on_click)
		{
			if (this.states[this.state].on_click_override_sound_path == "")
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
				return;
			}
			KFMOD.PlayUISound(GlobalAssets.GetSound(this.states[this.state].on_click_override_sound_path, false));
		}
	}

	// Token: 0x06009CC4 RID: 40132 RVA: 0x0010A6E5 File Offset: 0x001088E5
	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		this.timeToNextIncrement = this.timeBetweenIncrementsMax;
	}

	// Token: 0x06009CC5 RID: 40133 RVA: 0x003D35B0 File Offset: 0x003D17B0
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (!this.clickHeldDown)
		{
			this.clickHeldDown = true;
			this.PlayClickSound();
			if (this.onClick != null)
			{
				this.onClick();
			}
		}
		if (this.states.Length - 1 < this.state)
		{
			global::Debug.LogWarning("Multi toggle has too few / no states");
		}
		base.RefreshHoverColor();
	}

	// Token: 0x06009CC6 RID: 40134 RVA: 0x0010A6FA File Offset: 0x001088FA
	public override void OnPointerClick(PointerEventData eventData)
	{
		base.RefreshHoverColor();
	}

	// Token: 0x04007ACB RID: 31435
	private float timeBetweenIncrementsMin = 0.033f;

	// Token: 0x04007ACC RID: 31436
	private float timeBetweenIncrementsMax = 0.25f;

	// Token: 0x04007ACD RID: 31437
	private const float incrementAccelerationScale = 2.5f;

	// Token: 0x04007ACE RID: 31438
	private float timeToNextIncrement;
}
