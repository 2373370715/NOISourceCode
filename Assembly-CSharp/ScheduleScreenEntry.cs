using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F68 RID: 8040
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleScreenEntry")]
public class ScheduleScreenEntry : KMonoBehaviour
{
	// Token: 0x17000AD3 RID: 2771
	// (get) Token: 0x0600A9BD RID: 43453 RVA: 0x001129CD File Offset: 0x00110BCD
	// (set) Token: 0x0600A9BE RID: 43454 RVA: 0x001129D5 File Offset: 0x00110BD5
	public Schedule schedule { get; private set; }

	// Token: 0x0600A9BF RID: 43455 RVA: 0x00412440 File Offset: 0x00410640
	public void Setup(Schedule schedule)
	{
		this.schedule = schedule;
		base.gameObject.name = "Schedule_" + schedule.name;
		this.title.SetTitle(schedule.name);
		this.title.OnNameChanged += this.OnNameChanged;
		this.duplicateScheduleButton.onClick += this.DuplicateSchedule;
		this.deleteScheduleButton.onClick += this.DeleteSchedule;
		this.timetableRows = new List<GameObject>();
		this.blockButtonsByTimetableRow = new Dictionary<GameObject, List<ScheduleBlockButton>>();
		int num = Mathf.CeilToInt((float)(schedule.GetBlocks().Count / 24));
		for (int i = 0; i < num; i++)
		{
			this.AddTimetableRow(i * 24);
		}
		this.minionWidgets = new List<ScheduleMinionWidget>();
		this.blankMinionWidget = Util.KInstantiateUI<ScheduleMinionWidget>(this.minionWidgetPrefab.gameObject, this.minionWidgetContainer, false);
		this.blankMinionWidget.SetupBlank(schedule);
		this.RebuildMinionWidgets();
		this.RefreshStatus();
		this.RefreshAlarmButton();
		MultiToggle multiToggle = this.alarmButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnAlarmClicked));
		schedule.onChanged = (Action<Schedule>)Delegate.Combine(schedule.onChanged, new Action<Schedule>(this.OnScheduleChanged));
		this.ConfigPaintButton(this.PaintButtonBathtime, Db.Get().ScheduleGroups.Hygene, Def.GetUISprite(Assets.GetPrefab(ShowerConfig.ID), "ui", false).first);
		this.ConfigPaintButton(this.PaintButtonWorktime, Db.Get().ScheduleGroups.Worktime, Def.GetUISprite(Assets.GetPrefab("ManualGenerator"), "ui", false).first);
		this.ConfigPaintButton(this.PaintButtonRecreation, Db.Get().ScheduleGroups.Recreation, Def.GetUISprite(Assets.GetPrefab("WaterCooler"), "ui", false).first);
		this.ConfigPaintButton(this.PaintButtonSleep, Db.Get().ScheduleGroups.Sleep, Def.GetUISprite(Assets.GetPrefab("Bed"), "ui", false).first);
		this.RefreshPaintButtons();
		this.RefreshTimeOfDayPositioner();
	}

	// Token: 0x0600A9C0 RID: 43456 RVA: 0x001129DE File Offset: 0x00110BDE
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.Deregister();
	}

	// Token: 0x0600A9C1 RID: 43457 RVA: 0x001129EC File Offset: 0x00110BEC
	public void Deregister()
	{
		if (this.schedule != null)
		{
			Schedule schedule = this.schedule;
			schedule.onChanged = (Action<Schedule>)Delegate.Remove(schedule.onChanged, new Action<Schedule>(this.OnScheduleChanged));
		}
	}

	// Token: 0x0600A9C2 RID: 43458 RVA: 0x00112A1D File Offset: 0x00110C1D
	private void DuplicateSchedule()
	{
		ScheduleManager.Instance.DuplicateSchedule(this.schedule);
	}

	// Token: 0x0600A9C3 RID: 43459 RVA: 0x00112A30 File Offset: 0x00110C30
	private void DeleteSchedule()
	{
		ScheduleManager.Instance.DeleteSchedule(this.schedule);
	}

	// Token: 0x0600A9C4 RID: 43460 RVA: 0x0041268C File Offset: 0x0041088C
	public void RefreshTimeOfDayPositioner()
	{
		if (this.schedule.ProgressTimetableIdx >= this.timetableRows.Count || this.schedule.ProgressTimetableIdx < 0)
		{
			KCrashReporter.ReportDevNotification("RefreshTimeOfDayPositionerError", Environment.StackTrace, string.Format("DevError: schedule.ProgressTimetableIdx is out of bounds. schedule.name:{0}, schedule.ProgressTimetableIdx:{1}, : timetableRows.Count:{2}", this.schedule.name, this.schedule.ProgressTimetableIdx, this.timetableRows.Count), true, null);
			this.timeOfDayPositioner.SetTargetTimetable(null);
			return;
		}
		GameObject targetTimetable = this.timetableRows[this.schedule.ProgressTimetableIdx];
		this.timeOfDayPositioner.SetTargetTimetable(targetTimetable);
	}

	// Token: 0x0600A9C5 RID: 43461 RVA: 0x00412738 File Offset: 0x00410938
	private void DuplicateTimetableRow(int sourceTimetableIdx)
	{
		List<ScheduleBlock> range = this.schedule.GetBlocks().GetRange(sourceTimetableIdx * 24, 24);
		List<ScheduleBlock> list = new List<ScheduleBlock>();
		for (int i = 0; i < range.Count; i++)
		{
			list.Add(new ScheduleBlock(range[i].name, range[i].GroupId));
		}
		int num = sourceTimetableIdx + 1;
		this.schedule.InsertTimetable(num, list);
		this.AddTimetableRow(num * 24);
	}

	// Token: 0x0600A9C6 RID: 43462 RVA: 0x004127B4 File Offset: 0x004109B4
	private void AddTimetableRow(int startingBlockIdx)
	{
		GameObject row = Util.KInstantiateUI(this.timetableRowPrefab, this.timetableRowContainer, true);
		int num = startingBlockIdx / 24;
		this.timetableRows.Insert(num, row);
		row.transform.SetSiblingIndex(num);
		HierarchyReferences component = row.GetComponent<HierarchyReferences>();
		List<ScheduleBlockButton> list = new List<ScheduleBlockButton>();
		for (int i = startingBlockIdx; i < startingBlockIdx + 24; i++)
		{
			GameObject gameObject = component.GetReference<RectTransform>("BlockContainer").gameObject;
			ScheduleBlockButton scheduleBlockButton = Util.KInstantiateUI<ScheduleBlockButton>(this.blockButtonPrefab.gameObject, gameObject, true);
			scheduleBlockButton.Setup(i - startingBlockIdx);
			scheduleBlockButton.SetBlockTypes(this.schedule.GetBlock(i).allowed_types);
			list.Add(scheduleBlockButton);
		}
		this.blockButtonsByTimetableRow.Add(row, list);
		component.GetReference<ScheduleBlockPainter>("BlockPainter").SetEntry(this);
		component.GetReference<KButton>("DuplicateButton").onClick += delegate()
		{
			this.DuplicateTimetableRow(this.timetableRows.IndexOf(row));
		};
		component.GetReference<KButton>("DeleteButton").onClick += delegate()
		{
			this.RemoveTimetableRow(row);
		};
		component.GetReference<KButton>("RotateLeftButton").onClick += delegate()
		{
			this.schedule.RotateBlocks(true, this.timetableRows.IndexOf(row));
		};
		component.GetReference<KButton>("RotateRightButton").onClick += delegate()
		{
			this.schedule.RotateBlocks(false, this.timetableRows.IndexOf(row));
		};
		KButton rotateUpButton = component.GetReference<KButton>("ShiftUpButton");
		rotateUpButton.onClick += delegate()
		{
			int timetableToShiftIdx = this.timetableRows.IndexOf(row);
			this.schedule.ShiftTimetable(true, timetableToShiftIdx);
			if (rotateUpButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName == "ScheduleMenu_Shift_up")
			{
				rotateUpButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_up_reset";
				return;
			}
			rotateUpButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_up";
		};
		KButton rotateDownButton = component.GetReference<KButton>("ShiftDownButton");
		rotateDownButton.onClick += delegate()
		{
			int timetableToShiftIdx = this.timetableRows.IndexOf(row);
			this.schedule.ShiftTimetable(false, timetableToShiftIdx);
			if (rotateDownButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName == "ScheduleMenu_Shift_down")
			{
				rotateDownButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_down_reset";
				return;
			}
			rotateDownButton.soundPlayer.button_widget_sound_events[0].OverrideAssetName = "ScheduleMenu_Shift_down";
		};
	}

	// Token: 0x0600A9C7 RID: 43463 RVA: 0x0041296C File Offset: 0x00410B6C
	private void RemoveTimetableRow(GameObject row)
	{
		if (this.timetableRows.Count == 1)
		{
			return;
		}
		this.timeOfDayPositioner.SetTargetTimetable(null);
		int timetableToRemoveIdx = this.timetableRows.IndexOf(row);
		this.timetableRows.Remove(row);
		this.blockButtonsByTimetableRow.Remove(row);
		UnityEngine.Object.Destroy(row);
		this.schedule.RemoveTimetable(timetableToRemoveIdx);
	}

	// Token: 0x0600A9C8 RID: 43464 RVA: 0x00112A42 File Offset: 0x00110C42
	public GameObject GetNameInputField()
	{
		return this.title.inputField.gameObject;
	}

	// Token: 0x0600A9C9 RID: 43465 RVA: 0x004129D0 File Offset: 0x00410BD0
	private void RebuildMinionWidgets()
	{
		if (this.IsNullOrDestroyed())
		{
			return;
		}
		if (!this.MinionWidgetsNeedRebuild())
		{
			return;
		}
		foreach (ScheduleMinionWidget original in this.minionWidgets)
		{
			Util.KDestroyGameObject(original);
		}
		this.minionWidgets.Clear();
		foreach (Ref<Schedulable> @ref in this.schedule.GetAssigned())
		{
			ScheduleMinionWidget scheduleMinionWidget = Util.KInstantiateUI<ScheduleMinionWidget>(this.minionWidgetPrefab.gameObject, this.minionWidgetContainer, true);
			scheduleMinionWidget.Setup(@ref.Get());
			this.minionWidgets.Add(scheduleMinionWidget);
		}
		if (Components.LiveMinionIdentities.Count > this.schedule.GetAssigned().Count)
		{
			this.blankMinionWidget.transform.SetAsLastSibling();
			this.blankMinionWidget.gameObject.SetActive(true);
			return;
		}
		this.blankMinionWidget.gameObject.SetActive(false);
	}

	// Token: 0x0600A9CA RID: 43466 RVA: 0x00412AFC File Offset: 0x00410CFC
	private bool MinionWidgetsNeedRebuild()
	{
		List<Ref<Schedulable>> assigned = this.schedule.GetAssigned();
		if (assigned.Count != this.minionWidgets.Count)
		{
			return true;
		}
		if (assigned.Count != Components.LiveMinionIdentities.Count != this.blankMinionWidget.gameObject.activeSelf)
		{
			return true;
		}
		for (int i = 0; i < assigned.Count; i++)
		{
			if (assigned[i].Get() != this.minionWidgets[i].schedulable)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600A9CB RID: 43467 RVA: 0x00412B8C File Offset: 0x00410D8C
	public void RefreshWidgetWorldData()
	{
		foreach (ScheduleMinionWidget scheduleMinionWidget in this.minionWidgets)
		{
			if (!scheduleMinionWidget.IsNullOrDestroyed())
			{
				scheduleMinionWidget.RefreshWidgetWorldData();
			}
		}
	}

	// Token: 0x0600A9CC RID: 43468 RVA: 0x00112A54 File Offset: 0x00110C54
	private void OnNameChanged(string newName)
	{
		this.schedule.name = newName;
		base.gameObject.name = "Schedule_" + this.schedule.name;
	}

	// Token: 0x0600A9CD RID: 43469 RVA: 0x00112A82 File Offset: 0x00110C82
	private void OnAlarmClicked()
	{
		this.schedule.alarmActivated = !this.schedule.alarmActivated;
		this.RefreshAlarmButton();
	}

	// Token: 0x0600A9CE RID: 43470 RVA: 0x00412BE8 File Offset: 0x00410DE8
	private void RefreshAlarmButton()
	{
		this.alarmButton.ChangeState(this.schedule.alarmActivated ? 1 : 0);
		ToolTip component = this.alarmButton.GetComponent<ToolTip>();
		component.SetSimpleTooltip(this.schedule.alarmActivated ? UI.SCHEDULESCREEN.ALARM_BUTTON_ON_TOOLTIP : UI.SCHEDULESCREEN.ALARM_BUTTON_OFF_TOOLTIP);
		ToolTipScreen.Instance.MarkTooltipDirty(component);
		this.alarmField.text = (this.schedule.alarmActivated ? UI.SCHEDULESCREEN.ALARM_TITLE_ENABLED : UI.SCHEDULESCREEN.ALARM_TITLE_DISABLED);
	}

	// Token: 0x0600A9CF RID: 43471 RVA: 0x00112AA3 File Offset: 0x00110CA3
	private void OnResetClicked()
	{
		this.schedule.SetBlocksToGroupDefaults(Db.Get().ScheduleGroups.allGroups);
	}

	// Token: 0x0600A9D0 RID: 43472 RVA: 0x00112A30 File Offset: 0x00110C30
	private void OnDeleteClicked()
	{
		ScheduleManager.Instance.DeleteSchedule(this.schedule);
	}

	// Token: 0x0600A9D1 RID: 43473 RVA: 0x00412C78 File Offset: 0x00410E78
	private void OnScheduleChanged(Schedule changedSchedule)
	{
		foreach (KeyValuePair<GameObject, List<ScheduleBlockButton>> keyValuePair in this.blockButtonsByTimetableRow)
		{
			GameObject key = keyValuePair.Key;
			int num = this.timetableRows.IndexOf(key);
			List<ScheduleBlockButton> value = keyValuePair.Value;
			for (int i = 0; i < value.Count; i++)
			{
				int idx = num * 24 + i;
				value[i].SetBlockTypes(changedSchedule.GetBlock(idx).allowed_types);
			}
		}
		this.RefreshStatus();
		this.RebuildMinionWidgets();
	}

	// Token: 0x0600A9D2 RID: 43474 RVA: 0x00412D28 File Offset: 0x00410F28
	private void RefreshStatus()
	{
		this.blockTypeCounts.Clear();
		foreach (ScheduleBlockType scheduleBlockType in Db.Get().ScheduleBlockTypes.resources)
		{
			this.blockTypeCounts[scheduleBlockType.Id] = 0;
		}
		foreach (ScheduleBlock scheduleBlock in this.schedule.GetBlocks())
		{
			foreach (ScheduleBlockType scheduleBlockType2 in scheduleBlock.allowed_types)
			{
				Dictionary<string, int> dictionary = this.blockTypeCounts;
				string id = scheduleBlockType2.Id;
				int num = dictionary[id];
				dictionary[id] = num + 1;
			}
		}
		if (this.noteEntryRight == null)
		{
			return;
		}
		int num2 = 0;
		ToolTip component = this.noteEntryRight.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		foreach (KeyValuePair<string, int> keyValuePair in this.blockTypeCounts)
		{
			if (keyValuePair.Value == 0)
			{
				num2++;
				component.AddMultiStringTooltip(string.Format(UI.SCHEDULEGROUPS.NOTIME, Db.Get().ScheduleBlockTypes.Get(keyValuePair.Key).Name), null);
			}
		}
		if (num2 > 0)
		{
			this.noteEntryRight.text = string.Format(UI.SCHEDULEGROUPS.MISSINGBLOCKS, num2);
			return;
		}
		this.noteEntryRight.text = "";
	}

	// Token: 0x0600A9D3 RID: 43475 RVA: 0x00412F0C File Offset: 0x0041110C
	private void ConfigPaintButton(GameObject button, ScheduleGroup group, Sprite iconSprite)
	{
		string groupID = group.Id;
		button.GetComponent<MultiToggle>().onClick = delegate()
		{
			ScheduleScreen.Instance.SelectedPaint = groupID;
			ScheduleScreen.Instance.RefreshAllPaintButtons();
		};
		this.paintButtons.Add(group.Id, button);
		HierarchyReferences component = button.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").sprite = iconSprite;
		component.GetReference<LocText>("Label").text = group.Name;
	}

	// Token: 0x0600A9D4 RID: 43476 RVA: 0x00412F80 File Offset: 0x00411180
	public void RefreshPaintButtons()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.paintButtons)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == ScheduleScreen.Instance.SelectedPaint) ? 1 : 0);
		}
	}

	// Token: 0x0600A9D5 RID: 43477 RVA: 0x00412FFC File Offset: 0x004111FC
	public bool PaintBlock(ScheduleBlockButton blockButton)
	{
		foreach (KeyValuePair<GameObject, List<ScheduleBlockButton>> keyValuePair in this.blockButtonsByTimetableRow)
		{
			GameObject key = keyValuePair.Key;
			int i = 0;
			while (i < keyValuePair.Value.Count)
			{
				if (keyValuePair.Value[i] == blockButton)
				{
					int idx = this.timetableRows.IndexOf(key) * 24 + i;
					ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.Get(ScheduleScreen.Instance.SelectedPaint);
					if (this.schedule.GetBlock(idx).GroupId != scheduleGroup.Id)
					{
						this.schedule.SetBlockGroup(idx, scheduleGroup);
						return true;
					}
					return false;
				}
				else
				{
					i++;
				}
			}
		}
		return false;
	}

	// Token: 0x040085A6 RID: 34214
	[SerializeField]
	private ScheduleBlockButton blockButtonPrefab;

	// Token: 0x040085A7 RID: 34215
	[SerializeField]
	private ScheduleMinionWidget minionWidgetPrefab;

	// Token: 0x040085A8 RID: 34216
	[SerializeField]
	private GameObject minionWidgetContainer;

	// Token: 0x040085A9 RID: 34217
	private ScheduleMinionWidget blankMinionWidget;

	// Token: 0x040085AA RID: 34218
	[SerializeField]
	private KButton duplicateScheduleButton;

	// Token: 0x040085AB RID: 34219
	[SerializeField]
	private KButton deleteScheduleButton;

	// Token: 0x040085AC RID: 34220
	[SerializeField]
	private EditableTitleBar title;

	// Token: 0x040085AD RID: 34221
	[SerializeField]
	private LocText alarmField;

	// Token: 0x040085AE RID: 34222
	[SerializeField]
	private KButton optionsButton;

	// Token: 0x040085AF RID: 34223
	[SerializeField]
	private LocText noteEntryLeft;

	// Token: 0x040085B0 RID: 34224
	[SerializeField]
	private LocText noteEntryRight;

	// Token: 0x040085B1 RID: 34225
	[SerializeField]
	private MultiToggle alarmButton;

	// Token: 0x040085B2 RID: 34226
	private List<GameObject> timetableRows;

	// Token: 0x040085B3 RID: 34227
	private Dictionary<GameObject, List<ScheduleBlockButton>> blockButtonsByTimetableRow;

	// Token: 0x040085B4 RID: 34228
	private List<ScheduleMinionWidget> minionWidgets;

	// Token: 0x040085B5 RID: 34229
	[SerializeField]
	private GameObject timetableRowPrefab;

	// Token: 0x040085B6 RID: 34230
	[SerializeField]
	private GameObject timetableRowContainer;

	// Token: 0x040085B7 RID: 34231
	private Dictionary<string, GameObject> paintButtons = new Dictionary<string, GameObject>();

	// Token: 0x040085B8 RID: 34232
	[SerializeField]
	private GameObject PaintButtonBathtime;

	// Token: 0x040085B9 RID: 34233
	[SerializeField]
	private GameObject PaintButtonWorktime;

	// Token: 0x040085BA RID: 34234
	[SerializeField]
	private GameObject PaintButtonRecreation;

	// Token: 0x040085BB RID: 34235
	[SerializeField]
	private GameObject PaintButtonSleep;

	// Token: 0x040085BC RID: 34236
	[SerializeField]
	private TimeOfDayPositioner timeOfDayPositioner;

	// Token: 0x040085BE RID: 34238
	private Dictionary<string, int> blockTypeCounts = new Dictionary<string, int>();
}
