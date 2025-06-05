using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001FC0 RID: 8128
public class CounterSideScreen : SideScreenContent, IRender200ms
{
	// Token: 0x0600ABC4 RID: 43972 RVA: 0x001131C7 File Offset: 0x001113C7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600ABC5 RID: 43973 RVA: 0x0041B100 File Offset: 0x00419300
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.resetButton.onClick += this.ResetCounter;
		this.incrementMaxButton.onClick += this.IncrementMaxCount;
		this.decrementMaxButton.onClick += this.DecrementMaxCount;
		this.incrementModeButton.onClick += this.ToggleMode;
		this.advancedModeToggle.onClick += this.ToggleAdvanced;
		this.maxCountInput.onEndEdit += delegate()
		{
			this.UpdateMaxCountFromTextInput(this.maxCountInput.currentValue);
		};
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
	}

	// Token: 0x0600ABC6 RID: 43974 RVA: 0x00114126 File Offset: 0x00112326
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicCounter>() != null;
	}

	// Token: 0x0600ABC7 RID: 43975 RVA: 0x0041B1B0 File Offset: 0x004193B0
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.maxCountInput.minValue = 1f;
		this.maxCountInput.maxValue = 10f;
		this.targetLogicCounter = target.GetComponent<LogicCounter>();
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
		this.UpdateMaxCountLabel(this.targetLogicCounter.maxCount);
		this.advancedModeCheckmark.enabled = this.targetLogicCounter.advancedMode;
	}

	// Token: 0x0600ABC8 RID: 43976 RVA: 0x00114134 File Offset: 0x00112334
	public void Render200ms(float dt)
	{
		if (this.targetLogicCounter == null)
		{
			return;
		}
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
	}

	// Token: 0x0600ABC9 RID: 43977 RVA: 0x0041B228 File Offset: 0x00419428
	private void UpdateCurrentCountLabel(int value)
	{
		string text = value.ToString();
		if (value == this.targetLogicCounter.maxCount)
		{
			text = UI.FormatAsAutomationState(text, UI.AutomationState.Active);
		}
		else
		{
			text = UI.FormatAsAutomationState(text, UI.AutomationState.Standby);
		}
		this.currentCount.text = (this.targetLogicCounter.advancedMode ? string.Format(UI.UISIDESCREENS.COUNTER_SIDE_SCREEN.CURRENT_COUNT_ADVANCED, text) : string.Format(UI.UISIDESCREENS.COUNTER_SIDE_SCREEN.CURRENT_COUNT_SIMPLE, text));
	}

	// Token: 0x0600ABCA RID: 43978 RVA: 0x00114156 File Offset: 0x00112356
	private void UpdateMaxCountLabel(int value)
	{
		this.maxCountInput.SetAmount((float)value);
	}

	// Token: 0x0600ABCB RID: 43979 RVA: 0x00114165 File Offset: 0x00112365
	private void UpdateMaxCountFromTextInput(float newValue)
	{
		this.SetMaxCount((int)newValue);
	}

	// Token: 0x0600ABCC RID: 43980 RVA: 0x0011416F File Offset: 0x0011236F
	private void IncrementMaxCount()
	{
		this.SetMaxCount(this.targetLogicCounter.maxCount + 1);
	}

	// Token: 0x0600ABCD RID: 43981 RVA: 0x00114184 File Offset: 0x00112384
	private void DecrementMaxCount()
	{
		this.SetMaxCount(this.targetLogicCounter.maxCount - 1);
	}

	// Token: 0x0600ABCE RID: 43982 RVA: 0x0041B298 File Offset: 0x00419498
	private void SetMaxCount(int newValue)
	{
		if (newValue > 10)
		{
			newValue = 1;
		}
		if (newValue < 1)
		{
			newValue = 10;
		}
		if (newValue < this.targetLogicCounter.currentCount)
		{
			this.targetLogicCounter.currentCount = newValue;
		}
		this.targetLogicCounter.maxCount = newValue;
		this.UpdateCounterStates();
		this.UpdateMaxCountLabel(newValue);
	}

	// Token: 0x0600ABCF RID: 43983 RVA: 0x00114199 File Offset: 0x00112399
	private void ResetCounter()
	{
		this.targetLogicCounter.ResetCounter();
	}

	// Token: 0x0600ABD0 RID: 43984 RVA: 0x001141A6 File Offset: 0x001123A6
	private void UpdateCounterStates()
	{
		this.targetLogicCounter.SetCounterState();
		this.targetLogicCounter.UpdateLogicCircuit();
		this.targetLogicCounter.UpdateVisualState(true);
		this.targetLogicCounter.UpdateMeter();
	}

	// Token: 0x0600ABD1 RID: 43985 RVA: 0x000AA038 File Offset: 0x000A8238
	private void ToggleMode()
	{
	}

	// Token: 0x0600ABD2 RID: 43986 RVA: 0x0041B2E8 File Offset: 0x004194E8
	private void ToggleAdvanced()
	{
		this.targetLogicCounter.advancedMode = !this.targetLogicCounter.advancedMode;
		this.advancedModeCheckmark.enabled = this.targetLogicCounter.advancedMode;
		this.UpdateCurrentCountLabel(this.targetLogicCounter.currentCount);
		this.UpdateCounterStates();
	}

	// Token: 0x04008748 RID: 34632
	public LogicCounter targetLogicCounter;

	// Token: 0x04008749 RID: 34633
	public KButton resetButton;

	// Token: 0x0400874A RID: 34634
	public KButton incrementMaxButton;

	// Token: 0x0400874B RID: 34635
	public KButton decrementMaxButton;

	// Token: 0x0400874C RID: 34636
	public KButton incrementModeButton;

	// Token: 0x0400874D RID: 34637
	public KToggle advancedModeToggle;

	// Token: 0x0400874E RID: 34638
	public KImage advancedModeCheckmark;

	// Token: 0x0400874F RID: 34639
	public LocText currentCount;

	// Token: 0x04008750 RID: 34640
	[SerializeField]
	private KNumberInputField maxCountInput;
}
