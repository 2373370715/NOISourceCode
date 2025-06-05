using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000931 RID: 2353
public class AnimEventManager : Singleton<AnimEventManager>
{
	// Token: 0x0600294E RID: 10574 RVA: 0x000AA038 File Offset: 0x000A8238
	public void FreeResources()
	{
	}

	// Token: 0x0600294F RID: 10575 RVA: 0x001E2158 File Offset: 0x001E0358
	public HandleVector<int>.Handle PlayAnim(KAnimControllerBase controller, KAnim.Anim anim, KAnim.PlayMode mode, float time, bool use_unscaled_time)
	{
		AnimEventManager.AnimData animData = default(AnimEventManager.AnimData);
		animData.frameRate = anim.frameRate;
		animData.totalTime = anim.totalTime;
		animData.numFrames = anim.numFrames;
		animData.useUnscaledTime = use_unscaled_time;
		AnimEventManager.EventPlayerData eventPlayerData = default(AnimEventManager.EventPlayerData);
		eventPlayerData.elapsedTime = time;
		eventPlayerData.mode = mode;
		eventPlayerData.controller = (controller as KBatchedAnimController);
		eventPlayerData.currentFrame = eventPlayerData.controller.GetFrameIdx(eventPlayerData.elapsedTime, false);
		eventPlayerData.previousFrame = -1;
		eventPlayerData.events = null;
		eventPlayerData.updatingEvents = null;
		eventPlayerData.events = GameAudioSheets.Get().GetEvents(anim.id);
		if (eventPlayerData.events == null)
		{
			eventPlayerData.events = AnimEventManager.emptyEventList;
		}
		HandleVector<int>.Handle result;
		if (animData.useUnscaledTime)
		{
			HandleVector<int>.Handle anim_data_handle = this.uiAnimData.Allocate(animData);
			HandleVector<int>.Handle event_data_handle = this.uiEventData.Allocate(eventPlayerData);
			result = this.indirectionData.Allocate(new AnimEventManager.IndirectionData(anim_data_handle, event_data_handle, true));
		}
		else
		{
			HandleVector<int>.Handle anim_data_handle2 = this.animData.Allocate(animData);
			HandleVector<int>.Handle event_data_handle2 = this.eventData.Allocate(eventPlayerData);
			result = this.indirectionData.Allocate(new AnimEventManager.IndirectionData(anim_data_handle2, event_data_handle2, false));
		}
		return result;
	}

	// Token: 0x06002950 RID: 10576 RVA: 0x001E228C File Offset: 0x001E048C
	public void SetMode(HandleVector<int>.Handle handle, KAnim.PlayMode mode)
	{
		if (!handle.IsValid())
		{
			return;
		}
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data2 = kcompactedVector.GetData(data.eventDataHandle);
		data2.mode = mode;
		kcompactedVector.SetData(data.eventDataHandle, data2);
	}

	// Token: 0x06002951 RID: 10577 RVA: 0x001E22E8 File Offset: 0x001E04E8
	public void StopAnim(HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.AnimData> kcompactedVector = data.isUIData ? this.uiAnimData : this.animData;
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector2 = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data2 = kcompactedVector2.GetData(data.eventDataHandle);
		this.StopEvents(data2);
		kcompactedVector.Free(data.animDataHandle);
		kcompactedVector2.Free(data.eventDataHandle);
		this.indirectionData.Free(handle);
	}

	// Token: 0x06002952 RID: 10578 RVA: 0x001E2374 File Offset: 0x001E0574
	public float GetElapsedTime(HandleVector<int>.Handle handle)
	{
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		return (data.isUIData ? this.uiEventData : this.eventData).GetData(data.eventDataHandle).elapsedTime;
	}

	// Token: 0x06002953 RID: 10579 RVA: 0x001E23B4 File Offset: 0x001E05B4
	public void SetElapsedTime(HandleVector<int>.Handle handle, float elapsed_time)
	{
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data2 = kcompactedVector.GetData(data.eventDataHandle);
		data2.elapsedTime = elapsed_time;
		kcompactedVector.SetData(data.eventDataHandle, data2);
	}

	// Token: 0x06002954 RID: 10580 RVA: 0x001E2408 File Offset: 0x001E0608
	public void Update()
	{
		float deltaTime = Time.deltaTime;
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		this.Update(deltaTime, this.animData.GetDataList(), this.eventData.GetDataList());
		this.Update(unscaledDeltaTime, this.uiAnimData.GetDataList(), this.uiEventData.GetDataList());
		for (int i = 0; i < this.finishedCalls.Count; i++)
		{
			this.finishedCalls[i].TriggerStop();
		}
		this.finishedCalls.Clear();
	}

	// Token: 0x06002955 RID: 10581 RVA: 0x001E2490 File Offset: 0x001E0690
	private void Update(float dt, List<AnimEventManager.AnimData> anim_data, List<AnimEventManager.EventPlayerData> event_data)
	{
		if (dt <= 0f)
		{
			return;
		}
		for (int i = 0; i < event_data.Count; i++)
		{
			AnimEventManager.EventPlayerData eventPlayerData = event_data[i];
			if (!(eventPlayerData.controller == null) && eventPlayerData.mode != KAnim.PlayMode.Paused)
			{
				eventPlayerData.currentFrame = eventPlayerData.controller.GetFrameIdx(eventPlayerData.elapsedTime, false);
				event_data[i] = eventPlayerData;
				this.PlayEvents(eventPlayerData);
				eventPlayerData.previousFrame = eventPlayerData.currentFrame;
				eventPlayerData.elapsedTime += dt * eventPlayerData.controller.GetPlaySpeed();
				event_data[i] = eventPlayerData;
				if (eventPlayerData.updatingEvents != null)
				{
					for (int j = 0; j < eventPlayerData.updatingEvents.Count; j++)
					{
						eventPlayerData.updatingEvents[j].OnUpdate(eventPlayerData);
					}
				}
				event_data[i] = eventPlayerData;
				if (eventPlayerData.mode != KAnim.PlayMode.Loop && eventPlayerData.currentFrame >= anim_data[i].numFrames - 1)
				{
					this.StopEvents(eventPlayerData);
					this.finishedCalls.Add(eventPlayerData.controller);
				}
			}
		}
	}

	// Token: 0x06002956 RID: 10582 RVA: 0x001E25A8 File Offset: 0x001E07A8
	private void PlayEvents(AnimEventManager.EventPlayerData data)
	{
		for (int i = 0; i < data.events.Count; i++)
		{
			data.events[i].Play(data);
		}
	}

	// Token: 0x06002957 RID: 10583 RVA: 0x001E25E0 File Offset: 0x001E07E0
	private void StopEvents(AnimEventManager.EventPlayerData data)
	{
		for (int i = 0; i < data.events.Count; i++)
		{
			data.events[i].Stop(data);
		}
		if (data.updatingEvents != null)
		{
			data.updatingEvents.Clear();
		}
	}

	// Token: 0x06002958 RID: 10584 RVA: 0x000BF4F2 File Offset: 0x000BD6F2
	public AnimEventManager.DevTools_DebugInfo DevTools_GetDebugInfo()
	{
		return new AnimEventManager.DevTools_DebugInfo(this, this.animData, this.eventData, this.uiAnimData, this.uiEventData);
	}

	// Token: 0x04001C13 RID: 7187
	private static readonly List<AnimEvent> emptyEventList = new List<AnimEvent>();

	// Token: 0x04001C14 RID: 7188
	private const int INITIAL_VECTOR_SIZE = 256;

	// Token: 0x04001C15 RID: 7189
	private KCompactedVector<AnimEventManager.AnimData> animData = new KCompactedVector<AnimEventManager.AnimData>(256);

	// Token: 0x04001C16 RID: 7190
	private KCompactedVector<AnimEventManager.EventPlayerData> eventData = new KCompactedVector<AnimEventManager.EventPlayerData>(256);

	// Token: 0x04001C17 RID: 7191
	private KCompactedVector<AnimEventManager.AnimData> uiAnimData = new KCompactedVector<AnimEventManager.AnimData>(256);

	// Token: 0x04001C18 RID: 7192
	private KCompactedVector<AnimEventManager.EventPlayerData> uiEventData = new KCompactedVector<AnimEventManager.EventPlayerData>(256);

	// Token: 0x04001C19 RID: 7193
	private KCompactedVector<AnimEventManager.IndirectionData> indirectionData = new KCompactedVector<AnimEventManager.IndirectionData>(0);

	// Token: 0x04001C1A RID: 7194
	private List<KBatchedAnimController> finishedCalls = new List<KBatchedAnimController>();

	// Token: 0x02000932 RID: 2354
	public struct AnimData
	{
		// Token: 0x04001C1B RID: 7195
		public float frameRate;

		// Token: 0x04001C1C RID: 7196
		public float totalTime;

		// Token: 0x04001C1D RID: 7197
		public int numFrames;

		// Token: 0x04001C1E RID: 7198
		public bool useUnscaledTime;
	}

	// Token: 0x02000933 RID: 2355
	[DebuggerDisplay("{controller.name}, Anim={currentAnim}, Frame={currentFrame}, Mode={mode}")]
	public struct EventPlayerData
	{
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600295B RID: 10587 RVA: 0x000BF51E File Offset: 0x000BD71E
		// (set) Token: 0x0600295C RID: 10588 RVA: 0x000BF526 File Offset: 0x000BD726
		public int currentFrame { readonly get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600295D RID: 10589 RVA: 0x000BF52F File Offset: 0x000BD72F
		// (set) Token: 0x0600295E RID: 10590 RVA: 0x000BF537 File Offset: 0x000BD737
		public int previousFrame { readonly get; set; }

		// Token: 0x0600295F RID: 10591 RVA: 0x000BF540 File Offset: 0x000BD740
		public ComponentType GetComponent<ComponentType>()
		{
			return this.controller.GetComponent<ComponentType>();
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06002960 RID: 10592 RVA: 0x000BF54D File Offset: 0x000BD74D
		public string name
		{
			get
			{
				return this.controller.name;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06002961 RID: 10593 RVA: 0x000BF55A File Offset: 0x000BD75A
		public float normalizedTime
		{
			get
			{
				return this.elapsedTime / this.controller.CurrentAnim.totalTime;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06002962 RID: 10594 RVA: 0x000BF573 File Offset: 0x000BD773
		public Vector3 position
		{
			get
			{
				return this.controller.transform.GetPosition();
			}
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x000BF585 File Offset: 0x000BD785
		public void AddUpdatingEvent(AnimEvent ev)
		{
			if (this.updatingEvents == null)
			{
				this.updatingEvents = new List<AnimEvent>();
			}
			this.updatingEvents.Add(ev);
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x000BF5A6 File Offset: 0x000BD7A6
		public void SetElapsedTime(float elapsedTime)
		{
			this.elapsedTime = elapsedTime;
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x000BF5AF File Offset: 0x000BD7AF
		public void FreeResources()
		{
			this.elapsedTime = 0f;
			this.mode = KAnim.PlayMode.Once;
			this.currentFrame = 0;
			this.previousFrame = 0;
			this.events = null;
			this.updatingEvents = null;
			this.controller = null;
		}

		// Token: 0x04001C1F RID: 7199
		public float elapsedTime;

		// Token: 0x04001C20 RID: 7200
		public KAnim.PlayMode mode;

		// Token: 0x04001C23 RID: 7203
		public List<AnimEvent> events;

		// Token: 0x04001C24 RID: 7204
		public List<AnimEvent> updatingEvents;

		// Token: 0x04001C25 RID: 7205
		public KBatchedAnimController controller;
	}

	// Token: 0x02000934 RID: 2356
	private struct IndirectionData
	{
		// Token: 0x06002966 RID: 10598 RVA: 0x000BF5E6 File Offset: 0x000BD7E6
		public IndirectionData(HandleVector<int>.Handle anim_data_handle, HandleVector<int>.Handle event_data_handle, bool is_ui_data)
		{
			this.isUIData = is_ui_data;
			this.animDataHandle = anim_data_handle;
			this.eventDataHandle = event_data_handle;
		}

		// Token: 0x04001C26 RID: 7206
		public bool isUIData;

		// Token: 0x04001C27 RID: 7207
		public HandleVector<int>.Handle animDataHandle;

		// Token: 0x04001C28 RID: 7208
		public HandleVector<int>.Handle eventDataHandle;
	}

	// Token: 0x02000935 RID: 2357
	public readonly struct DevTools_DebugInfo
	{
		// Token: 0x06002967 RID: 10599 RVA: 0x000BF5FD File Offset: 0x000BD7FD
		public DevTools_DebugInfo(AnimEventManager eventManager, KCompactedVector<AnimEventManager.AnimData> animData, KCompactedVector<AnimEventManager.EventPlayerData> eventData, KCompactedVector<AnimEventManager.AnimData> uiAnimData, KCompactedVector<AnimEventManager.EventPlayerData> uiEventData)
		{
			this.eventManager = eventManager;
			this.animData = animData;
			this.eventData = eventData;
			this.uiAnimData = uiAnimData;
			this.uiEventData = uiEventData;
		}

		// Token: 0x04001C29 RID: 7209
		public readonly AnimEventManager eventManager;

		// Token: 0x04001C2A RID: 7210
		public readonly KCompactedVector<AnimEventManager.AnimData> animData;

		// Token: 0x04001C2B RID: 7211
		public readonly KCompactedVector<AnimEventManager.EventPlayerData> eventData;

		// Token: 0x04001C2C RID: 7212
		public readonly KCompactedVector<AnimEventManager.AnimData> uiAnimData;

		// Token: 0x04001C2D RID: 7213
		public readonly KCompactedVector<AnimEventManager.EventPlayerData> uiEventData;
	}
}
