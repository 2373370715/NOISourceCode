using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001F64 RID: 8036
public class ScheduleScreen : KScreen
{
	// Token: 0x17000AD2 RID: 2770
	// (get) Token: 0x0600A9A4 RID: 43428 RVA: 0x00112906 File Offset: 0x00110B06
	// (set) Token: 0x0600A9A5 RID: 43429 RVA: 0x0011290E File Offset: 0x00110B0E
	public string SelectedPaint { get; set; }

	// Token: 0x0600A9A6 RID: 43430 RVA: 0x00102E82 File Offset: 0x00101082
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x0600A9A7 RID: 43431 RVA: 0x00112917 File Offset: 0x00110B17
	protected override void OnPrefabInit()
	{
		base.ConsumeMouseScroll = true;
		this.scheduleEntries = new List<ScheduleScreenEntry>();
		ScheduleScreen.Instance = this;
	}

	// Token: 0x0600A9A8 RID: 43432 RVA: 0x004120CC File Offset: 0x004102CC
	protected override void OnSpawn()
	{
		foreach (Schedule schedule in ScheduleManager.Instance.GetSchedules())
		{
			this.AddScheduleEntry(schedule);
		}
		this.addScheduleButton.onClick += this.OnAddScheduleClick;
		this.closeButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		ScheduleManager.Instance.onSchedulesChanged += this.OnSchedulesChanged;
		Game.Instance.Subscribe(1983128072, new Action<object>(this.RefreshWidgetWorldData));
	}

	// Token: 0x0600A9A9 RID: 43433 RVA: 0x00112931 File Offset: 0x00110B31
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		ScheduleManager.Instance.onSchedulesChanged -= this.OnSchedulesChanged;
		ScheduleScreen.Instance = null;
	}

	// Token: 0x0600A9AA RID: 43434 RVA: 0x00112955 File Offset: 0x00110B55
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			base.Activate();
			this.SetScreenHeight();
		}
	}

	// Token: 0x0600A9AB RID: 43435 RVA: 0x00412198 File Offset: 0x00410398
	private void SetScreenHeight()
	{
		bool flag = ScheduleManager.Instance.GetSchedules().Count == 1;
		base.GetComponent<LayoutElement>().preferredHeight = (float)(flag ? 410 : 604);
		this.bottomSpacer.SetActive(flag);
	}

	// Token: 0x0600A9AC RID: 43436 RVA: 0x004121E0 File Offset: 0x004103E0
	public void RefreshAllPaintButtons()
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.RefreshPaintButtons();
		}
	}

	// Token: 0x0600A9AD RID: 43437 RVA: 0x0011296D File Offset: 0x00110B6D
	private void OnAddScheduleClick()
	{
		ScheduleManager.Instance.AddDefaultSchedule(false, false);
	}

	// Token: 0x0600A9AE RID: 43438 RVA: 0x00412230 File Offset: 0x00410430
	private void AddScheduleEntry(Schedule schedule)
	{
		ScheduleScreenEntry scheduleScreenEntry = Util.KInstantiateUI<ScheduleScreenEntry>(this.scheduleEntryPrefab.gameObject, this.scheduleEntryContainer, true);
		scheduleScreenEntry.Setup(schedule);
		this.scheduleEntries.Add(scheduleScreenEntry);
		this.SetScreenHeight();
	}

	// Token: 0x0600A9AF RID: 43439 RVA: 0x00412270 File Offset: 0x00410470
	private void OnSchedulesChanged(List<Schedule> schedules)
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.Deregister();
			Util.KDestroyGameObject(scheduleScreenEntry.gameObject);
		}
		this.scheduleEntries.Clear();
		foreach (Schedule schedule in schedules)
		{
			this.AddScheduleEntry(schedule);
		}
		this.SetScreenHeight();
	}

	// Token: 0x0600A9B0 RID: 43440 RVA: 0x0041231C File Offset: 0x0041051C
	private void RefreshWidgetWorldData(object data = null)
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.RefreshWidgetWorldData();
		}
	}

	// Token: 0x0600A9B1 RID: 43441 RVA: 0x0041236C File Offset: 0x0041056C
	public void OnChangeCurrentTimetable()
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.RefreshTimeOfDayPositioner();
		}
	}

	// Token: 0x0600A9B2 RID: 43442 RVA: 0x0011297B File Offset: 0x00110B7B
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.CheckBlockedInput())
		{
			if (!e.Consumed)
			{
				e.Consumed = true;
				return;
			}
		}
		else
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x0600A9B3 RID: 43443 RVA: 0x004123BC File Offset: 0x004105BC
	private bool CheckBlockedInput()
	{
		bool result = false;
		if (UnityEngine.EventSystems.EventSystem.current != null)
		{
			GameObject currentSelectedGameObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject != null)
			{
				foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
				{
					if (currentSelectedGameObject == scheduleScreenEntry.GetNameInputField())
					{
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x04008599 RID: 34201
	public static ScheduleScreen Instance;

	// Token: 0x0400859B RID: 34203
	[SerializeField]
	private ScheduleScreenEntry scheduleEntryPrefab;

	// Token: 0x0400859C RID: 34204
	[SerializeField]
	private GameObject scheduleEntryContainer;

	// Token: 0x0400859D RID: 34205
	[SerializeField]
	private KButton addScheduleButton;

	// Token: 0x0400859E RID: 34206
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400859F RID: 34207
	[SerializeField]
	private GameObject bottomSpacer;

	// Token: 0x040085A0 RID: 34208
	private List<ScheduleScreenEntry> scheduleEntries;
}
