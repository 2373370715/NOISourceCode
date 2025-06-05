using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200189B RID: 6299
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleManager")]
public class ScheduleManager : KMonoBehaviour, ISim33ms
{
	// Token: 0x14000023 RID: 35
	// (add) Token: 0x06008234 RID: 33332 RVA: 0x00349868 File Offset: 0x00347A68
	// (remove) Token: 0x06008235 RID: 33333 RVA: 0x003498A0 File Offset: 0x00347AA0
	public event Action<List<Schedule>> onSchedulesChanged;

	// Token: 0x06008236 RID: 33334 RVA: 0x000FA32A File Offset: 0x000F852A
	public static void DestroyInstance()
	{
		ScheduleManager.Instance = null;
	}

	// Token: 0x06008237 RID: 33335 RVA: 0x000FA332 File Offset: 0x000F8532
	public Schedule GetDefaultBionicSchedule()
	{
		return this.schedules.Find((Schedule match) => match.isDefaultForBionics);
	}

	// Token: 0x06008238 RID: 33336 RVA: 0x000FA35E File Offset: 0x000F855E
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.schedules.Count == 0)
		{
			this.AddDefaultSchedule(true, true);
		}
	}

	// Token: 0x06008239 RID: 33337 RVA: 0x000FA375 File Offset: 0x000F8575
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.schedules = new List<Schedule>();
		ScheduleManager.Instance = this;
	}

	// Token: 0x0600823A RID: 33338 RVA: 0x003498D8 File Offset: 0x00347AD8
	protected override void OnSpawn()
	{
		if (this.schedules.Count == 0)
		{
			this.AddDefaultSchedule(true, true);
		}
		foreach (Schedule schedule in this.schedules)
		{
			schedule.ClearNullReferences();
		}
		List<ScheduleBlock> scheduleBlocksFromGroupDefaults = Schedule.GetScheduleBlocksFromGroupDefaults(Db.Get().ScheduleGroups.allGroups);
		foreach (Schedule schedule2 in this.schedules)
		{
			List<ScheduleBlock> blocks = schedule2.GetBlocks();
			for (int i = 0; i < blocks.Count; i++)
			{
				ScheduleBlock scheduleBlock = blocks[i];
				if (Db.Get().ScheduleGroups.FindGroupForScheduleTypes(scheduleBlock.allowed_types) == null)
				{
					ScheduleGroup group = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(scheduleBlocksFromGroupDefaults[i].allowed_types);
					schedule2.SetBlockGroup(i, group);
				}
			}
		}
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			Schedulable component = minionIdentity.GetComponent<Schedulable>();
			if (this.GetSchedule(component) == null)
			{
				this.schedules[0].Assign(component);
			}
		}
		Components.LiveMinionIdentities.OnAdd += this.OnAddDupe;
		Components.LiveMinionIdentities.OnRemove += this.OnRemoveDupe;
	}

	// Token: 0x0600823B RID: 33339 RVA: 0x00349A84 File Offset: 0x00347C84
	private void OnAddDupe(MinionIdentity minion)
	{
		Schedulable component = minion.GetComponent<Schedulable>();
		if (component.GetSchedule() != null)
		{
			return;
		}
		Schedule schedule = this.schedules[0];
		if (minion.model == GameTags.Minions.Models.Bionic)
		{
			if (this.GetDefaultBionicSchedule() == null)
			{
				if (!this.hasDeletedDefaultBionicSchedule)
				{
					Schedule schedule2 = this.AddSchedule(Db.Get().ScheduleGroups.allGroups, UI.SCHEDULESCREEN.SCHEDULE_NAME_DEFAULT_BIONIC, true);
					schedule2.AddTimetable(Schedule.GetScheduleBlocksFromGroupDefaults(Db.Get().ScheduleGroups.allGroups));
					schedule2.AddTimetable(Schedule.GetScheduleBlocksFromGroupDefaults(Db.Get().ScheduleGroups.allGroups));
					for (int i = 0; i < schedule2.GetBlocks().Count; i++)
					{
						schedule2.SetBlockGroup(i, Db.Get().ScheduleGroups.Worktime);
					}
					for (int j = 1; j <= 6; j++)
					{
						schedule2.SetBlockGroup(schedule2.GetBlocks().Count - j, Db.Get().ScheduleGroups.Sleep);
					}
					for (int k = 7; k <= 12; k++)
					{
						schedule2.SetBlockGroup(schedule2.GetBlocks().Count - k, Db.Get().ScheduleGroups.Recreation);
					}
					schedule = schedule2;
					schedule2.isDefaultForBionics = true;
					if (this.onSchedulesChanged != null)
					{
						this.onSchedulesChanged(this.schedules);
					}
				}
			}
			else
			{
				schedule = this.GetDefaultBionicSchedule();
			}
		}
		else if (this.GetSchedule(component) != null)
		{
			schedule = this.GetSchedule(component);
		}
		schedule.Assign(component);
	}

	// Token: 0x0600823C RID: 33340 RVA: 0x00349C08 File Offset: 0x00347E08
	private void OnRemoveDupe(MinionIdentity minion)
	{
		Schedulable component = minion.GetComponent<Schedulable>();
		Schedule schedule = this.GetSchedule(component);
		if (schedule != null)
		{
			schedule.Unassign(component);
		}
	}

	// Token: 0x0600823D RID: 33341 RVA: 0x00349C30 File Offset: 0x00347E30
	public void OnStoredDupeDestroyed(StoredMinionIdentity dupe)
	{
		foreach (Schedule schedule in this.schedules)
		{
			schedule.Unassign(dupe.gameObject.GetComponent<Schedulable>());
		}
	}

	// Token: 0x0600823E RID: 33342 RVA: 0x00349C8C File Offset: 0x00347E8C
	public void AddDefaultSchedule(bool alarmOn, bool useDefaultName = true)
	{
		Schedule schedule = this.AddSchedule(Db.Get().ScheduleGroups.allGroups, useDefaultName ? UI.SCHEDULESCREEN.SCHEDULE_NAME_DEFAULT : UI.SCHEDULESCREEN.SCHEDULE_NAME_NEW, alarmOn);
		if (Game.Instance.FastWorkersModeActive)
		{
			for (int i = 0; i < 21; i++)
			{
				schedule.SetBlockGroup(i, Db.Get().ScheduleGroups.Worktime);
			}
			schedule.SetBlockGroup(21, Db.Get().ScheduleGroups.Recreation);
			schedule.SetBlockGroup(22, Db.Get().ScheduleGroups.Recreation);
			schedule.SetBlockGroup(23, Db.Get().ScheduleGroups.Sleep);
		}
	}

	// Token: 0x0600823F RID: 33343 RVA: 0x00349D38 File Offset: 0x00347F38
	public Schedule AddSchedule(List<ScheduleGroup> groups, string name = null, bool alarmOn = false)
	{
		if (name == null)
		{
			this.scheduleNameIncrementor++;
			name = string.Format(UI.SCHEDULESCREEN.SCHEDULE_NAME_FORMAT, this.scheduleNameIncrementor.ToString());
		}
		Schedule schedule = new Schedule(name, groups, alarmOn);
		this.schedules.Add(schedule);
		if (this.onSchedulesChanged != null)
		{
			this.onSchedulesChanged(this.schedules);
		}
		return schedule;
	}

	// Token: 0x06008240 RID: 33344 RVA: 0x00349DA4 File Offset: 0x00347FA4
	public Schedule DuplicateSchedule(Schedule source)
	{
		if (base.name == null)
		{
			this.scheduleNameIncrementor++;
			base.name = string.Format(UI.SCHEDULESCREEN.SCHEDULE_NAME_FORMAT, this.scheduleNameIncrementor.ToString());
		}
		Schedule schedule = new Schedule("copy of " + source.name, source.GetBlocks(), source.alarmActivated);
		schedule.ProgressTimetableIdx = source.ProgressTimetableIdx;
		this.schedules.Add(schedule);
		if (this.onSchedulesChanged != null)
		{
			this.onSchedulesChanged(this.schedules);
		}
		return schedule;
	}

	// Token: 0x06008241 RID: 33345 RVA: 0x00349E3C File Offset: 0x0034803C
	public void DeleteSchedule(Schedule schedule)
	{
		if (this.schedules.Count == 1)
		{
			return;
		}
		List<Ref<Schedulable>> assigned = schedule.GetAssigned();
		if (schedule.isDefaultForBionics)
		{
			this.hasDeletedDefaultBionicSchedule = true;
		}
		this.schedules.Remove(schedule);
		foreach (Ref<Schedulable> @ref in assigned)
		{
			this.schedules[0].Assign(@ref.Get());
		}
		if (this.onSchedulesChanged != null)
		{
			this.onSchedulesChanged(this.schedules);
		}
	}

	// Token: 0x06008242 RID: 33346 RVA: 0x00349EE4 File Offset: 0x003480E4
	public Schedule GetSchedule(Schedulable schedulable)
	{
		foreach (Schedule schedule in this.schedules)
		{
			if (schedule.IsAssigned(schedulable))
			{
				return schedule;
			}
		}
		return null;
	}

	// Token: 0x06008243 RID: 33347 RVA: 0x000FA38E File Offset: 0x000F858E
	public List<Schedule> GetSchedules()
	{
		return this.schedules;
	}

	// Token: 0x06008244 RID: 33348 RVA: 0x00349F40 File Offset: 0x00348140
	public bool IsAllowed(Schedulable schedulable, ScheduleBlockType schedule_block_type)
	{
		Schedule schedule = this.GetSchedule(schedulable);
		return schedule != null && schedule.GetCurrentScheduleBlock().IsAllowed(schedule_block_type);
	}

	// Token: 0x06008245 RID: 33349 RVA: 0x000FA396 File Offset: 0x000F8596
	public static int GetCurrentHour()
	{
		return Math.Min((int)(GameClock.Instance.GetCurrentCycleAsPercentage() * 24f), 23);
	}

	// Token: 0x06008246 RID: 33350 RVA: 0x00349F68 File Offset: 0x00348168
	public void Sim33ms(float dt)
	{
		int currentHour = ScheduleManager.GetCurrentHour();
		if (ScheduleManager.GetCurrentHour() != this.lastHour)
		{
			foreach (Schedule schedule in this.schedules)
			{
				schedule.Tick();
			}
			this.lastHour = currentHour;
		}
	}

	// Token: 0x06008247 RID: 33351 RVA: 0x00349FD4 File Offset: 0x003481D4
	public void PlayScheduleAlarm(Schedule schedule, ScheduleBlock block, bool forwards)
	{
		Notification notification = new Notification(string.Format(MISC.NOTIFICATIONS.SCHEDULE_CHANGED.NAME, schedule.name, block.name), NotificationType.Good, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.SCHEDULE_CHANGED.TOOLTIP.Replace("{0}", schedule.name).Replace("{1}", block.name).Replace("{2}", Db.Get().ScheduleGroups.Get(block.GroupId).notificationTooltip), null, true, 0f, null, null, null, true, false, false);
		base.GetComponent<Notifier>().Add(notification, "");
		base.StartCoroutine(this.PlayScheduleTone(schedule, forwards));
	}

	// Token: 0x06008248 RID: 33352 RVA: 0x000FA3B0 File Offset: 0x000F85B0
	private IEnumerator PlayScheduleTone(Schedule schedule, bool forwards)
	{
		int[] tones = schedule.GetTones();
		int num2;
		for (int i = 0; i < tones.Length; i = num2 + 1)
		{
			int num = forwards ? i : (tones.Length - 1 - i);
			this.PlayTone(tones[num], forwards);
			yield return SequenceUtil.WaitForSeconds(TuningData<ScheduleManager.Tuning>.Get().toneSpacingSeconds);
			num2 = i;
		}
		yield break;
	}

	// Token: 0x06008249 RID: 33353 RVA: 0x0034A060 File Offset: 0x00348260
	private void PlayTone(int pitch, bool forwards)
	{
		EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("WorkChime_tone", false), Vector3.zero, 1f);
		instance.setParameterByName("WorkChime_pitch", (float)pitch, false);
		instance.setParameterByName("WorkChime_start", (float)(forwards ? 1 : 0), false);
		KFMOD.EndOneShot(instance);
	}

	// Token: 0x04006304 RID: 25348
	[Serialize]
	private List<Schedule> schedules;

	// Token: 0x04006305 RID: 25349
	[Serialize]
	private int lastHour;

	// Token: 0x04006306 RID: 25350
	[Serialize]
	private int scheduleNameIncrementor;

	// Token: 0x04006308 RID: 25352
	public static ScheduleManager Instance;

	// Token: 0x04006309 RID: 25353
	[Serialize]
	private bool hasDeletedDefaultBionicSchedule;

	// Token: 0x0200189C RID: 6300
	public class Tuning : TuningData<ScheduleManager.Tuning>
	{
		// Token: 0x0400630A RID: 25354
		public float toneSpacingSeconds;

		// Token: 0x0400630B RID: 25355
		public int minToneIndex;

		// Token: 0x0400630C RID: 25356
		public int maxToneIndex;

		// Token: 0x0400630D RID: 25357
		public int firstLastToneSpacing;
	}
}
