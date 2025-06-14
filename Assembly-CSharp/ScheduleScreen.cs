﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScheduleScreen : KScreen
{
	public string SelectedPaint { get; set; }

	public override float GetSortKey()
	{
		return 50f;
	}

	protected override void OnPrefabInit()
	{
		base.ConsumeMouseScroll = true;
		this.scheduleEntries = new List<ScheduleScreenEntry>();
		ScheduleScreen.Instance = this;
	}

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

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		ScheduleManager.Instance.onSchedulesChanged -= this.OnSchedulesChanged;
		ScheduleScreen.Instance = null;
	}

	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			base.Activate();
			this.SetScreenHeight();
		}
	}

	private void SetScreenHeight()
	{
		bool flag = ScheduleManager.Instance.GetSchedules().Count == 1;
		base.GetComponent<LayoutElement>().preferredHeight = (float)(flag ? 410 : 604);
		this.bottomSpacer.SetActive(flag);
	}

	public void RefreshAllPaintButtons()
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.RefreshPaintButtons();
		}
	}

	private void OnAddScheduleClick()
	{
		ScheduleManager.Instance.AddDefaultSchedule(false, false);
	}

	private void AddScheduleEntry(Schedule schedule)
	{
		ScheduleScreenEntry scheduleScreenEntry = Util.KInstantiateUI<ScheduleScreenEntry>(this.scheduleEntryPrefab.gameObject, this.scheduleEntryContainer, true);
		scheduleScreenEntry.Setup(schedule);
		this.scheduleEntries.Add(scheduleScreenEntry);
		this.SetScreenHeight();
	}

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

	private void RefreshWidgetWorldData(object data = null)
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.RefreshWidgetWorldData();
		}
	}

	public void OnChangeCurrentTimetable()
	{
		foreach (ScheduleScreenEntry scheduleScreenEntry in this.scheduleEntries)
		{
			scheduleScreenEntry.RefreshTimeOfDayPositioner();
		}
	}

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

	public static ScheduleScreen Instance;

	[SerializeField]
	private ScheduleScreenEntry scheduleEntryPrefab;

	[SerializeField]
	private GameObject scheduleEntryContainer;

	[SerializeField]
	private KButton addScheduleButton;

	[SerializeField]
	private KButton closeButton;

	[SerializeField]
	private GameObject bottomSpacer;

	private List<ScheduleScreenEntry> scheduleEntries;
}
