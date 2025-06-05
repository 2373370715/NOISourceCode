using System;
using System.Collections.Generic;
using Klei.AI;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02000659 RID: 1625
public class MinionBrain : Brain
{
	// Token: 0x06001CF7 RID: 7415 RVA: 0x001B9724 File Offset: 0x001B7924
	public bool IsCellClear(int cell)
	{
		if (Grid.Reserved[cell])
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[cell, 0];
		return !(gameObject != null) || !(base.gameObject != gameObject) || gameObject.GetComponent<Navigator>().IsMoving();
	}

	// Token: 0x06001CF8 RID: 7416 RVA: 0x000B786F File Offset: 0x000B5A6F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Navigator.SetAbilities(new MinionPathFinderAbilities(this.Navigator));
		base.Subscribe<MinionBrain>(-1697596308, MinionBrain.AnimTrackStoredItemDelegate);
		base.Subscribe<MinionBrain>(-975551167, MinionBrain.OnUnstableGroundImpactDelegate);
	}

	// Token: 0x06001CF9 RID: 7417 RVA: 0x001B977C File Offset: 0x001B797C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (GameObject go in base.GetComponent<Storage>().items)
		{
			this.AddAnimTracker(go);
		}
		Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
	}

	// Token: 0x06001CFA RID: 7418 RVA: 0x001B97F8 File Offset: 0x001B79F8
	private void AnimTrackStoredItem(object data)
	{
		Storage component = base.GetComponent<Storage>();
		GameObject gameObject = (GameObject)data;
		this.RemoveTracker(gameObject);
		if (component.items.Contains(gameObject))
		{
			this.AddAnimTracker(gameObject);
		}
	}

	// Token: 0x06001CFB RID: 7419 RVA: 0x001B9830 File Offset: 0x001B7A30
	private void AddAnimTracker(GameObject go)
	{
		KAnimControllerBase component = go.GetComponent<KAnimControllerBase>();
		if (component == null)
		{
			return;
		}
		if (component.AnimFiles != null && component.AnimFiles.Length != 0 && component.AnimFiles[0] != null && component.GetComponent<Pickupable>().trackOnPickup)
		{
			KBatchedAnimTracker kbatchedAnimTracker = go.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.useTargetPoint = false;
			kbatchedAnimTracker.fadeOut = false;
			kbatchedAnimTracker.symbol = new HashedString("snapTo_chest");
			kbatchedAnimTracker.forceAlwaysVisible = true;
		}
	}

	// Token: 0x06001CFC RID: 7420 RVA: 0x001B98A8 File Offset: 0x001B7AA8
	private void RemoveTracker(GameObject go)
	{
		KBatchedAnimTracker component = go.GetComponent<KBatchedAnimTracker>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x001B98CC File Offset: 0x001B7ACC
	public override void UpdateBrain()
	{
		base.UpdateBrain();
		if (Game.Instance == null)
		{
			return;
		}
		if (!Game.Instance.savedInfo.discoveredSurface)
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space)
			{
				Game.Instance.savedInfo.discoveredSurface = true;
				DiscoveredSpaceMessage message = new DiscoveredSpaceMessage(base.gameObject.transform.GetPosition());
				Messenger.Instance.QueueMessage(message);
				Game.Instance.Trigger(-818188514, base.gameObject);
			}
		}
		if (!Game.Instance.savedInfo.discoveredOilField)
		{
			int cell2 = Grid.PosToCell(base.gameObject);
			if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell2) == SubWorld.ZoneType.OilField)
			{
				Game.Instance.savedInfo.discoveredOilField = true;
			}
		}
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x001B99A4 File Offset: 0x001B7BA4
	private void RegisterReactEmotePair(string reactable_id, Emote emote, float max_trigger_time)
	{
		if (base.gameObject == null)
		{
			return;
		}
		ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
		if (smi != null)
		{
			EmoteChore emoteChore = new EmoteChore(base.gameObject.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteIdle, emote, 1, null);
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.gameObject, reactable_id, Db.Get().ChoreTypes.Cough, max_trigger_time, 20f, float.PositiveInfinity, 0f);
			emoteChore.PairReactable(selfEmoteReactable);
			selfEmoteReactable.SetEmote(emote);
			selfEmoteReactable.PairEmote(emoteChore);
			smi.AddOneshotReactable(selfEmoteReactable);
		}
	}

	// Token: 0x06001CFF RID: 7423 RVA: 0x001B9A40 File Offset: 0x001B7C40
	private void OnResearchComplete(object data)
	{
		if (Time.time - this.lastResearchCompleteEmoteTime > 1f)
		{
			this.RegisterReactEmotePair("ResearchComplete", Db.Get().Emotes.Minion.ResearchComplete, 3f);
			this.lastResearchCompleteEmoteTime = Time.time;
		}
	}

	// Token: 0x06001D00 RID: 7424 RVA: 0x001B9A90 File Offset: 0x001B7C90
	public Notification CreateCollapseNotification()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		return new Notification(MISC.NOTIFICATIONS.TILECOLLAPSE.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.TILECOLLAPSE.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + component.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06001D01 RID: 7425 RVA: 0x001B9AF0 File Offset: 0x001B7CF0
	public void RemoveCollapseNotification(Notification notification)
	{
		Vector3 position = notification.clickFocus.GetPosition();
		position.z = -40f;
		WorldContainer myWorld = notification.clickFocus.gameObject.GetMyWorld();
		if (myWorld != null && myWorld.IsDiscovered)
		{
			GameUtil.FocusCameraOnWorld(myWorld.id, position, 10f, null, true);
		}
		base.gameObject.AddOrGet<Notifier>().Remove(notification);
	}

	// Token: 0x06001D02 RID: 7426 RVA: 0x001B9B5C File Offset: 0x001B7D5C
	private void OnUnstableGroundImpact(object data)
	{
		GameObject telepad = GameUtil.GetTelepad(base.gameObject.GetMyWorld().id);
		Navigator component = base.GetComponent<Navigator>();
		Assignable assignable = base.GetComponent<MinionIdentity>().GetSoleOwner().GetAssignable(Db.Get().AssignableSlots.Bed);
		bool flag = assignable != null && component.CanReach(Grid.PosToCell(assignable.transform.GetPosition()));
		bool flag2 = telepad != null && component.CanReach(Grid.PosToCell(telepad.transform.GetPosition()));
		if (!flag && !flag2)
		{
			this.RegisterReactEmotePair("UnstableGroundShock", Db.Get().Emotes.Minion.Shock, 1f);
			Notification notification = this.CreateCollapseNotification();
			notification.customClickCallback = delegate(object o)
			{
				this.RemoveCollapseNotification(notification);
			};
			base.gameObject.AddOrGet<Notifier>().Add(notification, "");
		}
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x000B78AF File Offset: 0x000B5AAF
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(-107300940, new Action<object>(this.OnResearchComplete));
	}

	// Token: 0x04001254 RID: 4692
	[MyCmpReq]
	public Navigator Navigator;

	// Token: 0x04001255 RID: 4693
	[MyCmpGet]
	public OxygenBreather OxygenBreather;

	// Token: 0x04001256 RID: 4694
	private float lastResearchCompleteEmoteTime;

	// Token: 0x04001257 RID: 4695
	private static readonly EventSystem.IntraObjectHandler<MinionBrain> AnimTrackStoredItemDelegate = new EventSystem.IntraObjectHandler<MinionBrain>(delegate(MinionBrain component, object data)
	{
		component.AnimTrackStoredItem(data);
	});

	// Token: 0x04001258 RID: 4696
	private static readonly EventSystem.IntraObjectHandler<MinionBrain> OnUnstableGroundImpactDelegate = new EventSystem.IntraObjectHandler<MinionBrain>(delegate(MinionBrain component, object data)
	{
		component.OnUnstableGroundImpact(data);
	});
}
