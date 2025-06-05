using System;
using System.Collections.Generic;
using Database;
using KSerialization;

// Token: 0x02001A09 RID: 6665
[SerializationConfig(MemberSerialization.OptIn)]
public class StoryInstance : ISaveLoadable
{
	// Token: 0x17000913 RID: 2323
	// (get) Token: 0x06008ABC RID: 35516 RVA: 0x000FF3D0 File Offset: 0x000FD5D0
	// (set) Token: 0x06008ABD RID: 35517 RVA: 0x0036B36C File Offset: 0x0036956C
	public StoryInstance.State CurrentState
	{
		get
		{
			return this.state;
		}
		set
		{
			if (this.state == value)
			{
				return;
			}
			this.state = value;
			this.Telemetry.LogStateChange(this.state, GameClock.Instance.GetTimeInCycles());
			Action<StoryInstance.State> storyStateChanged = this.StoryStateChanged;
			if (storyStateChanged == null)
			{
				return;
			}
			storyStateChanged(this.state);
		}
	}

	// Token: 0x17000914 RID: 2324
	// (get) Token: 0x06008ABE RID: 35518 RVA: 0x000FF3D8 File Offset: 0x000FD5D8
	public StoryManager.StoryTelemetry Telemetry
	{
		get
		{
			if (this.telemetry == null)
			{
				this.telemetry = new StoryManager.StoryTelemetry();
			}
			return this.telemetry;
		}
	}

	// Token: 0x17000915 RID: 2325
	// (get) Token: 0x06008ABF RID: 35519 RVA: 0x000FF3F3 File Offset: 0x000FD5F3
	// (set) Token: 0x06008AC0 RID: 35520 RVA: 0x000FF3FB File Offset: 0x000FD5FB
	public EventInfoData EventInfo { get; private set; }

	// Token: 0x17000916 RID: 2326
	// (get) Token: 0x06008AC1 RID: 35521 RVA: 0x000FF404 File Offset: 0x000FD604
	// (set) Token: 0x06008AC2 RID: 35522 RVA: 0x000FF40C File Offset: 0x000FD60C
	public Notification Notification { get; private set; }

	// Token: 0x17000917 RID: 2327
	// (get) Token: 0x06008AC3 RID: 35523 RVA: 0x000FF415 File Offset: 0x000FD615
	// (set) Token: 0x06008AC4 RID: 35524 RVA: 0x000FF41D File Offset: 0x000FD61D
	public EventInfoDataHelper.PopupType PendingType { get; private set; } = EventInfoDataHelper.PopupType.NONE;

	// Token: 0x06008AC5 RID: 35525 RVA: 0x000FF426 File Offset: 0x000FD626
	public Story GetStory()
	{
		if (this._story == null)
		{
			this._story = Db.Get().Stories.Get(this.storyId);
		}
		return this._story;
	}

	// Token: 0x06008AC6 RID: 35526 RVA: 0x000FF451 File Offset: 0x000FD651
	public StoryInstance()
	{
	}

	// Token: 0x06008AC7 RID: 35527 RVA: 0x000FF46B File Offset: 0x000FD66B
	public StoryInstance(Story story, int worldId)
	{
		this._story = story;
		this.storyId = story.Id;
		this.worldId = worldId;
	}

	// Token: 0x06008AC8 RID: 35528 RVA: 0x000FF49F File Offset: 0x000FD69F
	public bool HasDisplayedPopup(EventInfoDataHelper.PopupType type)
	{
		return this.popupDisplayedStates != null && this.popupDisplayedStates.Contains(type);
	}

	// Token: 0x06008AC9 RID: 35529 RVA: 0x0036B3BC File Offset: 0x003695BC
	public void SetPopupData(StoryManager.PopupInfo info, EventInfoData eventInfo, Notification notification = null)
	{
		this.EventInfo = eventInfo;
		this.Notification = notification;
		this.PendingType = info.PopupType;
		eventInfo.showCallback = (System.Action)Delegate.Combine(eventInfo.showCallback, new System.Action(this.OnPopupDisplayed));
		if (info.DisplayImmediate)
		{
			EventInfoScreen.ShowPopup(eventInfo);
		}
	}

	// Token: 0x06008ACA RID: 35530 RVA: 0x000FF4B7 File Offset: 0x000FD6B7
	private void OnPopupDisplayed()
	{
		if (this.popupDisplayedStates == null)
		{
			this.popupDisplayedStates = new HashSet<EventInfoDataHelper.PopupType>();
		}
		this.popupDisplayedStates.Add(this.PendingType);
		this.EventInfo = null;
		this.Notification = null;
		this.PendingType = EventInfoDataHelper.PopupType.NONE;
	}

	// Token: 0x040068BF RID: 26815
	public Action<StoryInstance.State> StoryStateChanged;

	// Token: 0x040068C0 RID: 26816
	[Serialize]
	public readonly string storyId;

	// Token: 0x040068C1 RID: 26817
	[Serialize]
	public int worldId;

	// Token: 0x040068C2 RID: 26818
	[Serialize]
	private StoryInstance.State state;

	// Token: 0x040068C3 RID: 26819
	[Serialize]
	private StoryManager.StoryTelemetry telemetry;

	// Token: 0x040068C4 RID: 26820
	[Serialize]
	private HashSet<EventInfoDataHelper.PopupType> popupDisplayedStates = new HashSet<EventInfoDataHelper.PopupType>();

	// Token: 0x040068C8 RID: 26824
	private Story _story;

	// Token: 0x02001A0A RID: 6666
	public enum State
	{
		// Token: 0x040068CA RID: 26826
		RETROFITTED = -1,
		// Token: 0x040068CB RID: 26827
		NOT_STARTED,
		// Token: 0x040068CC RID: 26828
		DISCOVERED,
		// Token: 0x040068CD RID: 26829
		IN_PROGRESS,
		// Token: 0x040068CE RID: 26830
		COMPLETE
	}
}
