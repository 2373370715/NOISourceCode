using System;
using UnityEngine;

// Token: 0x0200094E RID: 2382
public class KBatchedAnimHeatPostProcessingEffect : KMonoBehaviour
{
	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06002A81 RID: 10881 RVA: 0x000C0207 File Offset: 0x000BE407
	public float HeatProduction
	{
		get
		{
			return this.heatProduction;
		}
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06002A82 RID: 10882 RVA: 0x000C020F File Offset: 0x000BE40F
	public bool IsHeatProductionEnoughToShowEffect
	{
		get
		{
			return this.HeatProduction >= 1f;
		}
	}

	// Token: 0x06002A83 RID: 10883 RVA: 0x000C0221 File Offset: 0x000BE421
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.animController.postProcessingEffectsAllowed |= KAnimConverter.PostProcessingEffects.TemperatureOverlay;
	}

	// Token: 0x06002A84 RID: 10884 RVA: 0x000C023C File Offset: 0x000BE43C
	public void SetHeatBeingProducedValue(float heat)
	{
		this.heatProduction = heat;
		this.RefreshEffectVisualState();
	}

	// Token: 0x06002A85 RID: 10885 RVA: 0x000C024B File Offset: 0x000BE44B
	public void RefreshEffectVisualState()
	{
		if (base.enabled && this.IsHeatProductionEnoughToShowEffect)
		{
			this.SetParameterValue(1f);
			return;
		}
		this.SetParameterValue(0f);
	}

	// Token: 0x06002A86 RID: 10886 RVA: 0x000C0274 File Offset: 0x000BE474
	private void SetParameterValue(float value)
	{
		if (this.animController != null)
		{
			this.animController.postProcessingParameters = value;
		}
	}

	// Token: 0x06002A87 RID: 10887 RVA: 0x000C0290 File Offset: 0x000BE490
	protected override void OnCmpEnable()
	{
		this.RefreshEffectVisualState();
	}

	// Token: 0x06002A88 RID: 10888 RVA: 0x000C0290 File Offset: 0x000BE490
	protected override void OnCmpDisable()
	{
		this.RefreshEffectVisualState();
	}

	// Token: 0x06002A89 RID: 10889 RVA: 0x001E6E4C File Offset: 0x001E504C
	private void Update()
	{
		int num = Mathf.FloorToInt(Time.timeSinceLevelLoad / 1f);
		if (num != this.loopsPlayed)
		{
			this.loopsPlayed = num;
			this.OnNewLoopReached();
		}
	}

	// Token: 0x06002A8A RID: 10890 RVA: 0x001E6E80 File Offset: 0x001E5080
	private void OnNewLoopReached()
	{
		if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode == OverlayModes.Temperature.ID && this.IsHeatProductionEnoughToShowEffect)
		{
			Vector3 position = base.transform.GetPosition();
			string sound = GlobalAssets.GetSound("Temperature_Heat_Emission", false);
			position.z = 0f;
			SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, position, 1f, false));
		}
	}

	// Token: 0x04001CC3 RID: 7363
	public const float SHOW_EFFECT_HEAT_TRESHOLD = 1f;

	// Token: 0x04001CC4 RID: 7364
	private const float DISABLING_VALUE = 0f;

	// Token: 0x04001CC5 RID: 7365
	private const float ENABLING_VALUE = 1f;

	// Token: 0x04001CC6 RID: 7366
	private float heatProduction;

	// Token: 0x04001CC7 RID: 7367
	public const float ANIM_DURATION = 1f;

	// Token: 0x04001CC8 RID: 7368
	private int loopsPlayed;

	// Token: 0x04001CC9 RID: 7369
	[MyCmpGet]
	private KBatchedAnimController animController;
}
