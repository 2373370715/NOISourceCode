using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B0F RID: 2831
[AddComponentMenu("KMonoBehaviour/scripts/ReportManager")]
public class ReportManager : KMonoBehaviour
{
	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06003482 RID: 13442 RVA: 0x000C6BAE File Offset: 0x000C4DAE
	public List<ReportManager.DailyReport> reports
	{
		get
		{
			return this.dailyReports;
		}
	}

	// Token: 0x06003483 RID: 13443 RVA: 0x000C6BB6 File Offset: 0x000C4DB6
	public static void DestroyInstance()
	{
		ReportManager.Instance = null;
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06003484 RID: 13444 RVA: 0x000C6BBE File Offset: 0x000C4DBE
	// (set) Token: 0x06003485 RID: 13445 RVA: 0x000C6BC5 File Offset: 0x000C4DC5
	public static ReportManager Instance { get; private set; }

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06003486 RID: 13446 RVA: 0x000C6BCD File Offset: 0x000C4DCD
	public ReportManager.DailyReport TodaysReport
	{
		get
		{
			return this.todaysReport;
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06003487 RID: 13447 RVA: 0x000C6BD5 File Offset: 0x000C4DD5
	public ReportManager.DailyReport YesterdaysReport
	{
		get
		{
			if (this.dailyReports.Count <= 1)
			{
				return null;
			}
			return this.dailyReports[this.dailyReports.Count - 1];
		}
	}

	// Token: 0x06003488 RID: 13448 RVA: 0x000C6BFF File Offset: 0x000C4DFF
	protected override void OnPrefabInit()
	{
		ReportManager.Instance = this;
		base.Subscribe(Game.Instance.gameObject, -1917495436, new Action<object>(this.OnSaveGameReady));
		this.noteStorage = new ReportManager.NoteStorage();
	}

	// Token: 0x06003489 RID: 13449 RVA: 0x000C6BB6 File Offset: 0x000C4DB6
	protected override void OnCleanUp()
	{
		ReportManager.Instance = null;
	}

	// Token: 0x0600348A RID: 13450 RVA: 0x000C6C34 File Offset: 0x000C4E34
	[CustomSerialize]
	private void CustomSerialize(BinaryWriter writer)
	{
		writer.Write(0);
		this.noteStorage.Serialize(writer);
	}

	// Token: 0x0600348B RID: 13451 RVA: 0x00217AF4 File Offset: 0x00215CF4
	[CustomDeserialize]
	private void CustomDeserialize(IReader reader)
	{
		if (this.noteStorageBytes == null)
		{
			global::Debug.Assert(reader.ReadInt32() == 0);
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(reader.RawBytes()));
			binaryReader.BaseStream.Position = (long)reader.Position;
			this.noteStorage.Deserialize(binaryReader);
			reader.SkipBytes((int)binaryReader.BaseStream.Position - reader.Position);
		}
	}

	// Token: 0x0600348C RID: 13452 RVA: 0x000C6C49 File Offset: 0x000C4E49
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.noteStorageBytes != null)
		{
			this.noteStorage.Deserialize(new BinaryReader(new MemoryStream(this.noteStorageBytes)));
			this.noteStorageBytes = null;
		}
	}

	// Token: 0x0600348D RID: 13453 RVA: 0x00217B60 File Offset: 0x00215D60
	private void OnSaveGameReady(object data)
	{
		base.Subscribe(GameClock.Instance.gameObject, -722330267, new Action<object>(this.OnNightTime));
		if (this.todaysReport == null)
		{
			this.todaysReport = new ReportManager.DailyReport(this);
			this.todaysReport.day = GameUtil.GetCurrentCycle();
		}
	}

	// Token: 0x0600348E RID: 13454 RVA: 0x000C6C75 File Offset: 0x000C4E75
	public void ReportValue(ReportManager.ReportType reportType, float value, string note = null, string context = null)
	{
		this.TodaysReport.AddData(reportType, value, note, context);
	}

	// Token: 0x0600348F RID: 13455 RVA: 0x00217BB4 File Offset: 0x00215DB4
	private void OnNightTime(object data)
	{
		this.dailyReports.Add(this.todaysReport);
		int day = this.todaysReport.day;
		ManagementMenuNotification notification = new ManagementMenuNotification(global::Action.ManageReport, NotificationValence.Good, null, string.Format(UI.ENDOFDAYREPORT.NOTIFICATION_TITLE, day), NotificationType.Good, (List<Notification> n, object d) => string.Format(UI.ENDOFDAYREPORT.NOTIFICATION_TOOLTIP, day), null, true, 0f, delegate(object d)
		{
			ManagementMenu.Instance.OpenReports(day);
		}, null, null, true);
		if (this.notifier == null)
		{
			global::Debug.LogError("Cant notify, null notifier");
		}
		else
		{
			this.notifier.Add(notification, "");
		}
		this.todaysReport = new ReportManager.DailyReport(this);
		this.todaysReport.day = GameUtil.GetCurrentCycle() + 1;
	}

	// Token: 0x06003490 RID: 13456 RVA: 0x00217C7C File Offset: 0x00215E7C
	public ReportManager.DailyReport FindReport(int day)
	{
		foreach (ReportManager.DailyReport dailyReport in this.dailyReports)
		{
			if (dailyReport.day == day)
			{
				return dailyReport;
			}
		}
		if (this.todaysReport.day == day)
		{
			return this.todaysReport;
		}
		return null;
	}

	// Token: 0x06003491 RID: 13457 RVA: 0x00217CF0 File Offset: 0x00215EF0
	public ReportManager()
	{
		Dictionary<ReportManager.ReportType, ReportManager.ReportGroup> dictionary = new Dictionary<ReportManager.ReportType, ReportManager.ReportGroup>();
		dictionary.Add(ReportManager.ReportType.DuplicantHeader, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.DUPLICANT_DETAILS_HEADER, "", "", ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, null));
		dictionary.Add(ReportManager.ReportType.CaloriesCreated, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedCalories(v, GameUtil.TimeSlice.None, true), true, 1, UI.ENDOFDAYREPORT.CALORIES_CREATED.NAME, UI.ENDOFDAYREPORT.CALORIES_CREATED.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CALORIES_CREATED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.StressDelta, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v, GameUtil.TimeSlice.None), true, 1, UI.ENDOFDAYREPORT.STRESS_DELTA.NAME, UI.ENDOFDAYREPORT.STRESS_DELTA.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.STRESS_DELTA.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.DiseaseAdded, new ReportManager.ReportGroup(null, false, 1, UI.ENDOFDAYREPORT.DISEASE_ADDED.NAME, UI.ENDOFDAYREPORT.DISEASE_ADDED.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.DISEASE_ADDED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.DiseaseStatus, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedDiseaseAmount((int)v, GameUtil.TimeSlice.None), true, 1, UI.ENDOFDAYREPORT.DISEASE_STATUS.NAME, UI.ENDOFDAYREPORT.DISEASE_STATUS.TOOLTIP, UI.ENDOFDAYREPORT.DISEASE_STATUS.TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.LevelUp, new ReportManager.ReportGroup(null, false, 1, UI.ENDOFDAYREPORT.LEVEL_UP.NAME, UI.ENDOFDAYREPORT.LEVEL_UP.TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ToiletIncident, new ReportManager.ReportGroup(null, false, 1, UI.ENDOFDAYREPORT.TOILET_INCIDENT.NAME, UI.ENDOFDAYREPORT.TOILET_INCIDENT.TOOLTIP, UI.ENDOFDAYREPORT.TOILET_INCIDENT.TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ChoreStatus, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.CHORE_STATUS.NAME, UI.ENDOFDAYREPORT.CHORE_STATUS.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CHORE_STATUS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.DomesticatedCritters, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.NAME, UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.WildCritters, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.NAME, UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.RocketsInFlight, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.NAME, UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.TimeSpentHeader, new ReportManager.ReportGroup(null, true, 2, UI.ENDOFDAYREPORT.TIME_DETAILS_HEADER, "", "", ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, null));
		dictionary.Add(ReportManager.ReportType.WorkTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.WORK_TIME.NAME, UI.ENDOFDAYREPORT.WORK_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.TravelTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.TRAVEL_TIME.NAME, UI.ENDOFDAYREPORT.TRAVEL_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.PersonalTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.PERSONAL_TIME.NAME, UI.ENDOFDAYREPORT.PERSONAL_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.IdleTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.IDLE_TIME.NAME, UI.ENDOFDAYREPORT.IDLE_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.BaseHeader, new ReportManager.ReportGroup(null, true, 3, UI.ENDOFDAYREPORT.BASE_DETAILS_HEADER, "", "", ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, null));
		dictionary.Add(ReportManager.ReportType.OxygenCreated, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), true, 3, UI.ENDOFDAYREPORT.OXYGEN_CREATED.NAME, UI.ENDOFDAYREPORT.OXYGEN_CREATED.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.OXYGEN_CREATED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.EnergyCreated, new ReportManager.ReportGroup(new ReportManager.FormattingFn(GameUtil.GetFormattedRoundedJoules), true, 3, UI.ENDOFDAYREPORT.ENERGY_USAGE.NAME, UI.ENDOFDAYREPORT.ENERGY_USAGE.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.ENERGY_USAGE.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.EnergyWasted, new ReportManager.ReportGroup(new ReportManager.FormattingFn(GameUtil.GetFormattedRoundedJoules), true, 3, UI.ENDOFDAYREPORT.ENERGY_WASTED.NAME, UI.ENDOFDAYREPORT.NONE, UI.ENDOFDAYREPORT.ENERGY_WASTED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ContaminatedOxygenToilet, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), false, 3, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.NAME, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ContaminatedOxygenSublimation, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), false, 3, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.NAME, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		this.ReportGroups = dictionary;
		this.dailyReports = new List<ReportManager.DailyReport>();
		base..ctor();
	}

	// Token: 0x040023F5 RID: 9205
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x040023F6 RID: 9206
	private ReportManager.NoteStorage noteStorage;

	// Token: 0x040023F7 RID: 9207
	public Dictionary<ReportManager.ReportType, ReportManager.ReportGroup> ReportGroups;

	// Token: 0x040023F8 RID: 9208
	[Serialize]
	private List<ReportManager.DailyReport> dailyReports;

	// Token: 0x040023F9 RID: 9209
	[Serialize]
	private ReportManager.DailyReport todaysReport;

	// Token: 0x040023FA RID: 9210
	[Serialize]
	private byte[] noteStorageBytes;

	// Token: 0x02000B10 RID: 2832
	// (Invoke) Token: 0x06003493 RID: 13459
	public delegate string FormattingFn(float v);

	// Token: 0x02000B11 RID: 2833
	// (Invoke) Token: 0x06003497 RID: 13463
	public delegate string GroupFormattingFn(float v, float numEntries);

	// Token: 0x02000B12 RID: 2834
	public enum ReportType
	{
		// Token: 0x040023FD RID: 9213
		DuplicantHeader,
		// Token: 0x040023FE RID: 9214
		CaloriesCreated,
		// Token: 0x040023FF RID: 9215
		StressDelta,
		// Token: 0x04002400 RID: 9216
		LevelUp,
		// Token: 0x04002401 RID: 9217
		DiseaseStatus,
		// Token: 0x04002402 RID: 9218
		DiseaseAdded,
		// Token: 0x04002403 RID: 9219
		ToiletIncident,
		// Token: 0x04002404 RID: 9220
		ChoreStatus,
		// Token: 0x04002405 RID: 9221
		TimeSpentHeader,
		// Token: 0x04002406 RID: 9222
		TimeSpent,
		// Token: 0x04002407 RID: 9223
		WorkTime,
		// Token: 0x04002408 RID: 9224
		TravelTime,
		// Token: 0x04002409 RID: 9225
		PersonalTime,
		// Token: 0x0400240A RID: 9226
		IdleTime,
		// Token: 0x0400240B RID: 9227
		BaseHeader,
		// Token: 0x0400240C RID: 9228
		ContaminatedOxygenFlatulence,
		// Token: 0x0400240D RID: 9229
		ContaminatedOxygenToilet,
		// Token: 0x0400240E RID: 9230
		ContaminatedOxygenSublimation,
		// Token: 0x0400240F RID: 9231
		OxygenCreated,
		// Token: 0x04002410 RID: 9232
		EnergyCreated,
		// Token: 0x04002411 RID: 9233
		EnergyWasted,
		// Token: 0x04002412 RID: 9234
		DomesticatedCritters,
		// Token: 0x04002413 RID: 9235
		WildCritters,
		// Token: 0x04002414 RID: 9236
		RocketsInFlight
	}

	// Token: 0x02000B13 RID: 2835
	public struct ReportGroup
	{
		// Token: 0x0600349A RID: 13466 RVA: 0x002182F8 File Offset: 0x002164F8
		public ReportGroup(ReportManager.FormattingFn formatfn, bool reportIfZero, int group, string stringKey, string positiveTooltip, string negativeTooltip, ReportManager.ReportEntry.Order pos_note_order = ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order neg_note_order = ReportManager.ReportEntry.Order.Unordered, bool is_header = false, ReportManager.GroupFormattingFn group_format_fn = null)
		{
			ReportManager.FormattingFn formattingFn;
			if (formatfn == null)
			{
				formattingFn = ((float v) => v.ToString());
			}
			else
			{
				formattingFn = formatfn;
			}
			this.formatfn = formattingFn;
			this.groupFormatfn = group_format_fn;
			this.stringKey = stringKey;
			this.positiveTooltip = positiveTooltip;
			this.negativeTooltip = negativeTooltip;
			this.reportIfZero = reportIfZero;
			this.group = group;
			this.posNoteOrder = pos_note_order;
			this.negNoteOrder = neg_note_order;
			this.isHeader = is_header;
		}

		// Token: 0x04002415 RID: 9237
		public ReportManager.FormattingFn formatfn;

		// Token: 0x04002416 RID: 9238
		public ReportManager.GroupFormattingFn groupFormatfn;

		// Token: 0x04002417 RID: 9239
		public string stringKey;

		// Token: 0x04002418 RID: 9240
		public string positiveTooltip;

		// Token: 0x04002419 RID: 9241
		public string negativeTooltip;

		// Token: 0x0400241A RID: 9242
		public bool reportIfZero;

		// Token: 0x0400241B RID: 9243
		public int group;

		// Token: 0x0400241C RID: 9244
		public bool isHeader;

		// Token: 0x0400241D RID: 9245
		public ReportManager.ReportEntry.Order posNoteOrder;

		// Token: 0x0400241E RID: 9246
		public ReportManager.ReportEntry.Order negNoteOrder;
	}

	// Token: 0x02000B15 RID: 2837
	[SerializationConfig(MemberSerialization.OptIn)]
	public class ReportEntry
	{
		// Token: 0x0600349E RID: 13470 RVA: 0x00218378 File Offset: 0x00216578
		public ReportEntry(ReportManager.ReportType reportType, int note_storage_id, string context, bool is_child = false)
		{
			this.reportType = reportType;
			this.context = context;
			this.isChild = is_child;
			this.accumulate = 0f;
			this.accPositive = 0f;
			this.accNegative = 0f;
			this.noteStorageId = note_storage_id;
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x0600349F RID: 13471 RVA: 0x000C6C9C File Offset: 0x000C4E9C
		public float Positive
		{
			get
			{
				return this.accPositive;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060034A0 RID: 13472 RVA: 0x000C6CA4 File Offset: 0x000C4EA4
		public float Negative
		{
			get
			{
				return this.accNegative;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060034A1 RID: 13473 RVA: 0x000C6CAC File Offset: 0x000C4EAC
		public float Net
		{
			get
			{
				return this.accPositive + this.accNegative;
			}
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x000C6CBB File Offset: 0x000C4EBB
		[OnDeserializing]
		private void OnDeserialize()
		{
			this.contextEntries.Clear();
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x000C6CC8 File Offset: 0x000C4EC8
		public void IterateNotes(Action<ReportManager.ReportEntry.Note> callback)
		{
			ReportManager.Instance.noteStorage.IterateNotes(this.noteStorageId, callback);
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x000C6CE0 File Offset: 0x000C4EE0
		[OnDeserialized]
		private void OnDeserialized()
		{
			if (this.gameHash != -1)
			{
				this.reportType = (ReportManager.ReportType)this.gameHash;
				this.gameHash = -1;
			}
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x002183D0 File Offset: 0x002165D0
		public void AddData(ReportManager.NoteStorage note_storage, float value, string note = null, string dataContext = null)
		{
			this.AddActualData(note_storage, value, note);
			if (dataContext != null)
			{
				ReportManager.ReportEntry reportEntry = null;
				for (int i = 0; i < this.contextEntries.Count; i++)
				{
					if (this.contextEntries[i].context == dataContext)
					{
						reportEntry = this.contextEntries[i];
						break;
					}
				}
				if (reportEntry == null)
				{
					reportEntry = new ReportManager.ReportEntry(this.reportType, note_storage.GetNewNoteId(), dataContext, true);
					this.contextEntries.Add(reportEntry);
				}
				reportEntry.AddActualData(note_storage, value, note);
			}
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x0021845C File Offset: 0x0021665C
		private void AddActualData(ReportManager.NoteStorage note_storage, float value, string note = null)
		{
			this.accumulate += value;
			if (value > 0f)
			{
				this.accPositive += value;
			}
			else
			{
				this.accNegative += value;
			}
			if (note != null)
			{
				note_storage.Add(this.noteStorageId, value, note);
			}
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x000C6CFE File Offset: 0x000C4EFE
		public bool HasContextEntries()
		{
			return this.contextEntries.Count > 0;
		}

		// Token: 0x04002421 RID: 9249
		[Serialize]
		public int noteStorageId;

		// Token: 0x04002422 RID: 9250
		[Serialize]
		public int gameHash = -1;

		// Token: 0x04002423 RID: 9251
		[Serialize]
		public ReportManager.ReportType reportType;

		// Token: 0x04002424 RID: 9252
		[Serialize]
		public string context;

		// Token: 0x04002425 RID: 9253
		[Serialize]
		public float accumulate;

		// Token: 0x04002426 RID: 9254
		[Serialize]
		public float accPositive;

		// Token: 0x04002427 RID: 9255
		[Serialize]
		public float accNegative;

		// Token: 0x04002428 RID: 9256
		[Serialize]
		public ArrayRef<ReportManager.ReportEntry> contextEntries;

		// Token: 0x04002429 RID: 9257
		public bool isChild;

		// Token: 0x02000B16 RID: 2838
		public struct Note
		{
			// Token: 0x060034A8 RID: 13480 RVA: 0x000C6D0E File Offset: 0x000C4F0E
			public Note(float value, string note)
			{
				this.value = value;
				this.note = note;
			}

			// Token: 0x0400242A RID: 9258
			public float value;

			// Token: 0x0400242B RID: 9259
			public string note;
		}

		// Token: 0x02000B17 RID: 2839
		public enum Order
		{
			// Token: 0x0400242D RID: 9261
			Unordered,
			// Token: 0x0400242E RID: 9262
			Ascending,
			// Token: 0x0400242F RID: 9263
			Descending
		}
	}

	// Token: 0x02000B18 RID: 2840
	public class DailyReport
	{
		// Token: 0x060034A9 RID: 13481 RVA: 0x002184B0 File Offset: 0x002166B0
		public DailyReport(ReportManager manager)
		{
			foreach (KeyValuePair<ReportManager.ReportType, ReportManager.ReportGroup> keyValuePair in manager.ReportGroups)
			{
				this.reportEntries.Add(new ReportManager.ReportEntry(keyValuePair.Key, this.noteStorage.GetNewNoteId(), null, false));
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060034AA RID: 13482 RVA: 0x000C6D1E File Offset: 0x000C4F1E
		private ReportManager.NoteStorage noteStorage
		{
			get
			{
				return ReportManager.Instance.noteStorage;
			}
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x00218534 File Offset: 0x00216734
		public ReportManager.ReportEntry GetEntry(ReportManager.ReportType reportType)
		{
			for (int i = 0; i < this.reportEntries.Count; i++)
			{
				ReportManager.ReportEntry reportEntry = this.reportEntries[i];
				if (reportEntry.reportType == reportType)
				{
					return reportEntry;
				}
			}
			ReportManager.ReportEntry reportEntry2 = new ReportManager.ReportEntry(reportType, this.noteStorage.GetNewNoteId(), null, false);
			this.reportEntries.Add(reportEntry2);
			return reportEntry2;
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x000C6D2A File Offset: 0x000C4F2A
		public void AddData(ReportManager.ReportType reportType, float value, string note = null, string context = null)
		{
			this.GetEntry(reportType).AddData(this.noteStorage, value, note, context);
		}

		// Token: 0x04002430 RID: 9264
		[Serialize]
		public int day;

		// Token: 0x04002431 RID: 9265
		[Serialize]
		public List<ReportManager.ReportEntry> reportEntries = new List<ReportManager.ReportEntry>();
	}

	// Token: 0x02000B19 RID: 2841
	public class NoteStorage
	{
		// Token: 0x060034AD RID: 13485 RVA: 0x000C6D42 File Offset: 0x000C4F42
		public NoteStorage()
		{
			this.noteEntries = new ReportManager.NoteStorage.NoteEntries();
			this.stringTable = new ReportManager.NoteStorage.StringTable();
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x00218590 File Offset: 0x00216790
		public void Add(int report_entry_id, float value, string note)
		{
			int note_id = this.stringTable.AddString(note, 6);
			this.noteEntries.Add(report_entry_id, value, note_id);
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x002185BC File Offset: 0x002167BC
		public int GetNewNoteId()
		{
			int result = this.nextNoteId + 1;
			this.nextNoteId = result;
			return result;
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000C6D60 File Offset: 0x000C4F60
		public void IterateNotes(int report_entry_id, Action<ReportManager.ReportEntry.Note> callback)
		{
			this.noteEntries.IterateNotes(this.stringTable, report_entry_id, callback);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x000C6D75 File Offset: 0x000C4F75
		public void Serialize(BinaryWriter writer)
		{
			writer.Write(6);
			writer.Write(this.nextNoteId);
			this.stringTable.Serialize(writer);
			this.noteEntries.Serialize(writer);
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x002185DC File Offset: 0x002167DC
		public void Deserialize(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			if (num < 5)
			{
				return;
			}
			this.nextNoteId = reader.ReadInt32();
			this.stringTable.Deserialize(reader, num);
			this.noteEntries.Deserialize(reader, num);
		}

		// Token: 0x04002432 RID: 9266
		public const int SERIALIZATION_VERSION = 6;

		// Token: 0x04002433 RID: 9267
		private int nextNoteId;

		// Token: 0x04002434 RID: 9268
		private ReportManager.NoteStorage.NoteEntries noteEntries;

		// Token: 0x04002435 RID: 9269
		private ReportManager.NoteStorage.StringTable stringTable;

		// Token: 0x02000B1A RID: 2842
		private class StringTable
		{
			// Token: 0x060034B3 RID: 13491 RVA: 0x0021861C File Offset: 0x0021681C
			public int AddString(string str, int version = 6)
			{
				int num = Hash.SDBMLower(str);
				this.strings[num] = str;
				return num;
			}

			// Token: 0x060034B4 RID: 13492 RVA: 0x00218640 File Offset: 0x00216840
			public string GetStringByHash(int hash)
			{
				string result = "";
				this.strings.TryGetValue(hash, out result);
				return result;
			}

			// Token: 0x060034B5 RID: 13493 RVA: 0x00218664 File Offset: 0x00216864
			public void Serialize(BinaryWriter writer)
			{
				writer.Write(this.strings.Count);
				foreach (KeyValuePair<int, string> keyValuePair in this.strings)
				{
					writer.Write(keyValuePair.Value);
				}
			}

			// Token: 0x060034B6 RID: 13494 RVA: 0x002186D0 File Offset: 0x002168D0
			public void Deserialize(BinaryReader reader, int version)
			{
				int num = reader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					string str = reader.ReadString();
					this.AddString(str, version);
				}
			}

			// Token: 0x04002436 RID: 9270
			private Dictionary<int, string> strings = new Dictionary<int, string>();
		}

		// Token: 0x02000B1B RID: 2843
		private class NoteEntries
		{
			// Token: 0x060034B8 RID: 13496 RVA: 0x00218700 File Offset: 0x00216900
			public void Add(int report_entry_id, float value, int note_id)
			{
				Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary;
				if (!this.entries.TryGetValue(report_entry_id, out dictionary))
				{
					dictionary = new Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>(ReportManager.NoteStorage.NoteEntries.sKeyComparer);
					this.entries[report_entry_id] = dictionary;
				}
				ReportManager.NoteStorage.NoteEntries.NoteEntryKey noteEntryKey = new ReportManager.NoteStorage.NoteEntries.NoteEntryKey
				{
					noteHash = note_id,
					isPositive = (value > 0f)
				};
				if (dictionary.ContainsKey(noteEntryKey))
				{
					Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary2 = dictionary;
					ReportManager.NoteStorage.NoteEntries.NoteEntryKey key = noteEntryKey;
					dictionary2[key] += value;
					return;
				}
				dictionary[noteEntryKey] = value;
			}

			// Token: 0x060034B9 RID: 13497 RVA: 0x0021877C File Offset: 0x0021697C
			public void Serialize(BinaryWriter writer)
			{
				writer.Write(this.entries.Count);
				foreach (KeyValuePair<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>> keyValuePair in this.entries)
				{
					writer.Write(keyValuePair.Key);
					writer.Write(keyValuePair.Value.Count);
					foreach (KeyValuePair<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> keyValuePair2 in keyValuePair.Value)
					{
						writer.Write(keyValuePair2.Key.noteHash);
						writer.Write(keyValuePair2.Key.isPositive);
						writer.WriteSingleFast(keyValuePair2.Value);
					}
				}
			}

			// Token: 0x060034BA RID: 13498 RVA: 0x0021886C File Offset: 0x00216A6C
			public void Deserialize(BinaryReader reader, int version)
			{
				if (version < 6)
				{
					OldNoteEntriesV5 oldNoteEntriesV = new OldNoteEntriesV5();
					oldNoteEntriesV.Deserialize(reader);
					foreach (OldNoteEntriesV5.NoteStorageBlock noteStorageBlock in oldNoteEntriesV.storageBlocks)
					{
						for (int i = 0; i < noteStorageBlock.entryCount; i++)
						{
							OldNoteEntriesV5.NoteEntry noteEntry = noteStorageBlock.entries.structs[i];
							this.Add(noteEntry.reportEntryId, noteEntry.value, noteEntry.noteHash);
						}
					}
					return;
				}
				int num = reader.ReadInt32();
				this.entries = new Dictionary<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>>(num);
				for (int j = 0; j < num; j++)
				{
					int key = reader.ReadInt32();
					int num2 = reader.ReadInt32();
					Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary = new Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>(num2, ReportManager.NoteStorage.NoteEntries.sKeyComparer);
					this.entries[key] = dictionary;
					for (int k = 0; k < num2; k++)
					{
						ReportManager.NoteStorage.NoteEntries.NoteEntryKey key2 = new ReportManager.NoteStorage.NoteEntries.NoteEntryKey
						{
							noteHash = reader.ReadInt32(),
							isPositive = reader.ReadBoolean()
						};
						dictionary[key2] = reader.ReadSingle();
					}
				}
			}

			// Token: 0x060034BB RID: 13499 RVA: 0x002189A0 File Offset: 0x00216BA0
			public void IterateNotes(ReportManager.NoteStorage.StringTable string_table, int report_entry_id, Action<ReportManager.ReportEntry.Note> callback)
			{
				Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary;
				if (this.entries.TryGetValue(report_entry_id, out dictionary))
				{
					foreach (KeyValuePair<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> keyValuePair in dictionary)
					{
						string stringByHash = string_table.GetStringByHash(keyValuePair.Key.noteHash);
						ReportManager.ReportEntry.Note obj = new ReportManager.ReportEntry.Note(keyValuePair.Value, stringByHash);
						callback(obj);
					}
				}
			}

			// Token: 0x04002437 RID: 9271
			private static ReportManager.NoteStorage.NoteEntries.NoteEntryKeyComparer sKeyComparer = new ReportManager.NoteStorage.NoteEntries.NoteEntryKeyComparer();

			// Token: 0x04002438 RID: 9272
			private Dictionary<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>> entries = new Dictionary<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>>();

			// Token: 0x02000B1C RID: 2844
			public struct NoteEntryKey
			{
				// Token: 0x04002439 RID: 9273
				public int noteHash;

				// Token: 0x0400243A RID: 9274
				public bool isPositive;
			}

			// Token: 0x02000B1D RID: 2845
			public class NoteEntryKeyComparer : IEqualityComparer<ReportManager.NoteStorage.NoteEntries.NoteEntryKey>
			{
				// Token: 0x060034BE RID: 13502 RVA: 0x000C6DD4 File Offset: 0x000C4FD4
				public bool Equals(ReportManager.NoteStorage.NoteEntries.NoteEntryKey a, ReportManager.NoteStorage.NoteEntries.NoteEntryKey b)
				{
					return a.noteHash == b.noteHash && a.isPositive == b.isPositive;
				}

				// Token: 0x060034BF RID: 13503 RVA: 0x000C6DF4 File Offset: 0x000C4FF4
				public int GetHashCode(ReportManager.NoteStorage.NoteEntries.NoteEntryKey a)
				{
					return a.noteHash * (a.isPositive ? 1 : -1);
				}
			}
		}
	}
}
