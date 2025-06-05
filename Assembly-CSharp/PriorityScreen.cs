using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x02001F0D RID: 7949
public class PriorityScreen : KScreen
{
	// Token: 0x0600A716 RID: 42774 RVA: 0x00403008 File Offset: 0x00401208
	public void InstantiateButtons(Action<PrioritySetting> on_click, bool playSelectionSound = true)
	{
		this.onClick = on_click;
		for (int i = 1; i <= 9; i++)
		{
			int num = i;
			PriorityButton priorityButton = global::Util.KInstantiateUI<PriorityButton>(this.buttonPrefab_basic.gameObject, this.buttonPrefab_basic.transform.parent.gameObject, false);
			this.buttons_basic.Add(priorityButton);
			priorityButton.playSelectionSound = playSelectionSound;
			priorityButton.onClick = this.onClick;
			priorityButton.text.text = num.ToString();
			priorityButton.priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, num);
			priorityButton.tooltip.SetSimpleTooltip(string.Format(UI.PRIORITYSCREEN.BASIC, num));
		}
		this.buttonPrefab_basic.gameObject.SetActive(false);
		this.button_emergency.playSelectionSound = playSelectionSound;
		this.button_emergency.onClick = this.onClick;
		this.button_emergency.priority = new PrioritySetting(PriorityScreen.PriorityClass.topPriority, 1);
		this.button_emergency.tooltip.SetSimpleTooltip(UI.PRIORITYSCREEN.TOP_PRIORITY);
		this.button_toggleHigh.gameObject.SetActive(false);
		this.PriorityMenuContainer.SetActive(true);
		this.button_priorityMenu.gameObject.SetActive(true);
		this.button_priorityMenu.onClick += this.PriorityButtonClicked;
		this.button_priorityMenu.GetComponent<ToolTip>().SetSimpleTooltip(UI.PRIORITYSCREEN.OPEN_JOBS_SCREEN);
		this.diagram.SetActive(false);
		this.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5), false);
	}

	// Token: 0x0600A717 RID: 42775 RVA: 0x00110DA0 File Offset: 0x0010EFA0
	private void OnClick(PrioritySetting priority)
	{
		if (this.onClick != null)
		{
			this.onClick(priority);
		}
	}

	// Token: 0x0600A718 RID: 42776 RVA: 0x00110DB6 File Offset: 0x0010EFB6
	public void ShowDiagram(bool show)
	{
		this.diagram.SetActive(show);
	}

	// Token: 0x0600A719 RID: 42777 RVA: 0x00110DC4 File Offset: 0x0010EFC4
	public void ResetPriority()
	{
		this.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5), false);
	}

	// Token: 0x0600A71A RID: 42778 RVA: 0x00110DD4 File Offset: 0x0010EFD4
	public void PriorityButtonClicked()
	{
		ManagementMenu.Instance.TogglePriorities();
	}

	// Token: 0x0600A71B RID: 42779 RVA: 0x0040318C File Offset: 0x0040138C
	private void RefreshButton(PriorityButton b, PrioritySetting priority, bool play_sound)
	{
		if (b.priority == priority)
		{
			b.toggle.Select();
			b.toggle.isOn = true;
			if (play_sound)
			{
				b.toggle.soundPlayer.Play(0);
				return;
			}
		}
		else
		{
			b.toggle.isOn = false;
		}
	}

	// Token: 0x0600A71C RID: 42780 RVA: 0x004031E0 File Offset: 0x004013E0
	public void SetScreenPriority(PrioritySetting priority, bool play_sound = false)
	{
		if (this.lastSelectedPriority == priority)
		{
			return;
		}
		this.lastSelectedPriority = priority;
		if (priority.priority_class == PriorityScreen.PriorityClass.high)
		{
			this.button_toggleHigh.isOn = true;
		}
		else if (priority.priority_class == PriorityScreen.PriorityClass.basic)
		{
			this.button_toggleHigh.isOn = false;
		}
		for (int i = 0; i < this.buttons_basic.Count; i++)
		{
			this.buttons_basic[i].priority = new PrioritySetting(this.button_toggleHigh.isOn ? PriorityScreen.PriorityClass.high : PriorityScreen.PriorityClass.basic, i + 1);
			this.buttons_basic[i].tooltip.SetSimpleTooltip(string.Format(this.button_toggleHigh.isOn ? UI.PRIORITYSCREEN.HIGH : UI.PRIORITYSCREEN.BASIC, i + 1));
			this.RefreshButton(this.buttons_basic[i], this.lastSelectedPriority, play_sound);
		}
		this.RefreshButton(this.button_emergency, this.lastSelectedPriority, play_sound);
	}

	// Token: 0x0600A71D RID: 42781 RVA: 0x00110DE0 File Offset: 0x0010EFE0
	public PrioritySetting GetLastSelectedPriority()
	{
		return this.lastSelectedPriority;
	}

	// Token: 0x0600A71E RID: 42782 RVA: 0x004032E4 File Offset: 0x004014E4
	public static void PlayPriorityConfirmSound(PrioritySetting priority)
	{
		EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("Priority_Tool_Confirm", false), Vector3.zero, 1f);
		if (instance.isValid())
		{
			float num = 0f;
			if (priority.priority_class >= PriorityScreen.PriorityClass.high)
			{
				num += 10f;
			}
			if (priority.priority_class >= PriorityScreen.PriorityClass.topPriority)
			{
				num += 0f;
			}
			num += (float)priority.priority_value;
			instance.setParameterByName("priority", num, false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x04008308 RID: 33544
	[SerializeField]
	protected PriorityButton buttonPrefab_basic;

	// Token: 0x04008309 RID: 33545
	[SerializeField]
	protected GameObject EmergencyContainer;

	// Token: 0x0400830A RID: 33546
	[SerializeField]
	protected PriorityButton button_emergency;

	// Token: 0x0400830B RID: 33547
	[SerializeField]
	protected GameObject PriorityMenuContainer;

	// Token: 0x0400830C RID: 33548
	[SerializeField]
	protected KButton button_priorityMenu;

	// Token: 0x0400830D RID: 33549
	[SerializeField]
	protected KToggle button_toggleHigh;

	// Token: 0x0400830E RID: 33550
	[SerializeField]
	protected GameObject diagram;

	// Token: 0x0400830F RID: 33551
	protected List<PriorityButton> buttons_basic = new List<PriorityButton>();

	// Token: 0x04008310 RID: 33552
	protected List<PriorityButton> buttons_emergency = new List<PriorityButton>();

	// Token: 0x04008311 RID: 33553
	private PrioritySetting priority;

	// Token: 0x04008312 RID: 33554
	private PrioritySetting lastSelectedPriority = new PrioritySetting(PriorityScreen.PriorityClass.basic, -1);

	// Token: 0x04008313 RID: 33555
	private Action<PrioritySetting> onClick;

	// Token: 0x02001F0E RID: 7950
	public enum PriorityClass
	{
		// Token: 0x04008315 RID: 33557
		idle = -1,
		// Token: 0x04008316 RID: 33558
		basic,
		// Token: 0x04008317 RID: 33559
		high,
		// Token: 0x04008318 RID: 33560
		personalNeeds,
		// Token: 0x04008319 RID: 33561
		topPriority,
		// Token: 0x0400831A RID: 33562
		compulsory
	}
}
