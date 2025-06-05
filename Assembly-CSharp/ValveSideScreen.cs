using System;
using System.Collections;
using STRINGS;
using UnityEngine;

// Token: 0x02002054 RID: 8276
public class ValveSideScreen : SideScreenContent
{
	// Token: 0x0600AFEE RID: 45038 RVA: 0x0042C49C File Offset: 0x0042A69C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitsLabel.text = GameUtil.AddTimeSliceText(UI.UNITSUFFIXES.MASS.GRAM, GameUtil.TimeSlice.PerSecond);
		this.flowSlider.onReleaseHandle += this.OnReleaseHandle;
		this.flowSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.flowSlider.value);
		};
		this.flowSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.flowSlider.value);
		};
		this.flowSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.flowSlider.value);
			this.OnReleaseHandle();
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 1;
	}

	// Token: 0x0600AFEF RID: 45039 RVA: 0x0011707F File Offset: 0x0011527F
	public void OnReleaseHandle()
	{
		this.targetValve.ChangeFlow(this.targetFlow);
	}

	// Token: 0x0600AFF0 RID: 45040 RVA: 0x00117092 File Offset: 0x00115292
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Valve>() != null;
	}

	// Token: 0x0600AFF1 RID: 45041 RVA: 0x0042C54C File Offset: 0x0042A74C
	public override void SetTarget(GameObject target)
	{
		this.targetValve = target.GetComponent<Valve>();
		if (this.targetValve == null)
		{
			global::Debug.LogError("The target object does not have a Valve component.");
			return;
		}
		this.flowSlider.minValue = 0f;
		this.flowSlider.maxValue = this.targetValve.MaxFlow;
		this.flowSlider.value = this.targetValve.DesiredFlow;
		this.minFlowLabel.text = GameUtil.GetFormattedMass(0f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
		this.maxFlowLabel.text = GameUtil.GetFormattedMass(this.targetValve.MaxFlow, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}");
		this.numberInput.minValue = 0f;
		this.numberInput.maxValue = this.targetValve.MaxFlow * 1000f;
		this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(Mathf.Max(0f, this.targetValve.DesiredFlow), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
		this.numberInput.Activate();
	}

	// Token: 0x0600AFF2 RID: 45042 RVA: 0x001170A0 File Offset: 0x001152A0
	private void ReceiveValueFromSlider(float newValue)
	{
		newValue = Mathf.Round(newValue * 1000f) / 1000f;
		this.UpdateFlowValue(newValue);
	}

	// Token: 0x0600AFF3 RID: 45043 RVA: 0x0042C660 File Offset: 0x0042A860
	private void ReceiveValueFromInput(float input)
	{
		float newValue = input / 1000f;
		this.UpdateFlowValue(newValue);
		this.targetValve.ChangeFlow(this.targetFlow);
	}

	// Token: 0x0600AFF4 RID: 45044 RVA: 0x001170BD File Offset: 0x001152BD
	private void UpdateFlowValue(float newValue)
	{
		this.targetFlow = newValue;
		this.flowSlider.value = newValue;
		this.numberInput.SetDisplayValue(GameUtil.GetFormattedMass(newValue, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.Gram, false, "{0:0.#####}"));
	}

	// Token: 0x0600AFF5 RID: 45045 RVA: 0x001170EB File Offset: 0x001152EB
	private IEnumerator SettingDelay(float delay)
	{
		float startTime = Time.realtimeSinceStartup;
		float currentTime = startTime;
		while (currentTime < startTime + delay)
		{
			currentTime += Time.unscaledDeltaTime;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.OnReleaseHandle();
		yield break;
	}

	// Token: 0x04008A3B RID: 35387
	private Valve targetValve;

	// Token: 0x04008A3C RID: 35388
	[Header("Slider")]
	[SerializeField]
	private KSlider flowSlider;

	// Token: 0x04008A3D RID: 35389
	[SerializeField]
	private LocText minFlowLabel;

	// Token: 0x04008A3E RID: 35390
	[SerializeField]
	private LocText maxFlowLabel;

	// Token: 0x04008A3F RID: 35391
	[Header("Input Field")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x04008A40 RID: 35392
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x04008A41 RID: 35393
	private float targetFlow;
}
