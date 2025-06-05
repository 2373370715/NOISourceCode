using System;
using UnityEngine;

// Token: 0x020004D4 RID: 1236
public class MorbRoverMaker_Capsule : KMonoBehaviour
{
	// Token: 0x0600153D RID: 5437 RVA: 0x0019DF20 File Offset: 0x0019C120
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.MorbDevelopment_Meter = new MeterController(this.buildingAnimCtr, "meter_morb_target", "meter_morb", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
		this.GermMeter = new MeterController(this.buildingAnimCtr, "meter_germs_target", "meter_germs", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
		this.MorbDevelopment_Capsule_Meter = new MeterController(this.buildingAnimCtr, "meter_capsule_target", "meter_capsule", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingBack, Array.Empty<string>());
		this.MorbDevelopment_Capsule_Meter.meterController.onAnimComplete += this.OnGermAddedAnimationComplete;
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x000B3E77 File Offset: 0x000B2077
	private void OnGermAddedAnimationComplete(HashedString animName)
	{
		if (animName == MorbRoverMaker_Capsule.MORB_CAPSULE_METER_PUMP_ANIM_NAME)
		{
			this.MorbDevelopment_Capsule_Meter.meterController.Play("meter_capsule", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x0600153F RID: 5439 RVA: 0x000B3EAB File Offset: 0x000B20AB
	public void PlayPumpGermsAnimation()
	{
		if (this.MorbDevelopment_Capsule_Meter.meterController.currentAnim != MorbRoverMaker_Capsule.MORB_CAPSULE_METER_PUMP_ANIM_NAME)
		{
			this.MorbDevelopment_Capsule_Meter.meterController.Play(MorbRoverMaker_Capsule.MORB_CAPSULE_METER_PUMP_ANIM_NAME, KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06001540 RID: 5440 RVA: 0x0019DFB8 File Offset: 0x0019C1B8
	public void SetMorbDevelopmentProgress(float morbDevelopmentProgress)
	{
		global::Debug.Assert(true, "MORB PHASES COUNT needs to be larger than 0");
		string s = "meter_morb_" + (1 + Mathf.FloorToInt(morbDevelopmentProgress * 4f)).ToString();
		if (this.MorbDevelopment_Meter.meterController.currentAnim != s)
		{
			this.MorbDevelopment_Meter.meterController.Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x06001541 RID: 5441 RVA: 0x000B3EE9 File Offset: 0x000B20E9
	public void SetGermMeterProgress(float progress)
	{
		this.GermMeter.SetPositionPercent(progress);
	}

	// Token: 0x04000E97 RID: 3735
	public const byte MORB_PHASES_COUNT = 5;

	// Token: 0x04000E98 RID: 3736
	public const byte MORB_FIRST_PHASE_INDEX = 1;

	// Token: 0x04000E99 RID: 3737
	private const string GERM_METER_TARGET_NAME = "meter_germs_target";

	// Token: 0x04000E9A RID: 3738
	private const string GERM_METER_ANIMATION_NAME = "meter_germs";

	// Token: 0x04000E9B RID: 3739
	private const string MORB_METER_TARGET_NAME = "meter_morb_target";

	// Token: 0x04000E9C RID: 3740
	private const string MORB_METER_ANIMATION_NAME = "meter_morb";

	// Token: 0x04000E9D RID: 3741
	private const string MORB_CAPSULE_METER_TARGET_NAME = "meter_capsule_target";

	// Token: 0x04000E9E RID: 3742
	private const string MORB_CAPSULE_METER_ANIMATION_NAME = "meter_capsule";

	// Token: 0x04000E9F RID: 3743
	private static HashedString MORB_CAPSULE_METER_PUMP_ANIM_NAME = new HashedString("germ_pump");

	// Token: 0x04000EA0 RID: 3744
	[MyCmpGet]
	private KBatchedAnimController buildingAnimCtr;

	// Token: 0x04000EA1 RID: 3745
	private MeterController MorbDevelopment_Meter;

	// Token: 0x04000EA2 RID: 3746
	private MeterController MorbDevelopment_Capsule_Meter;

	// Token: 0x04000EA3 RID: 3747
	private MeterController GermMeter;
}
