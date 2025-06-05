using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000B23 RID: 2851
[AddComponentMenu("KMonoBehaviour/scripts/RoomTracker")]
public class RoomTracker : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x17000242 RID: 578
	// (get) Token: 0x060034DD RID: 13533 RVA: 0x000C6EC9 File Offset: 0x000C50C9
	// (set) Token: 0x060034DE RID: 13534 RVA: 0x000C6ED1 File Offset: 0x000C50D1
	public Room room { get; private set; }

	// Token: 0x060034DF RID: 13535 RVA: 0x00218C6C File Offset: 0x00216E6C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::Debug.Assert(!string.IsNullOrEmpty(this.requiredRoomType) && this.requiredRoomType != Db.Get().RoomTypes.Neutral.Id, "RoomTracker must have a requiredRoomType!");
		base.Subscribe<RoomTracker>(144050788, RoomTracker.OnUpdateRoomDelegate);
		this.FindAndSetRoom();
	}

	// Token: 0x060034E0 RID: 13536 RVA: 0x00218CD0 File Offset: 0x00216ED0
	public void FindAndSetRoom()
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(base.gameObject));
		if (cavityForCell != null && cavityForCell.room != null)
		{
			this.OnUpdateRoom(cavityForCell.room);
			return;
		}
		this.OnUpdateRoom(null);
	}

	// Token: 0x060034E1 RID: 13537 RVA: 0x000C6EDA File Offset: 0x000C50DA
	public bool IsInCorrectRoom()
	{
		return this.room != null && this.room.roomType.Id == this.requiredRoomType;
	}

	// Token: 0x060034E2 RID: 13538 RVA: 0x00218D18 File Offset: 0x00216F18
	public bool SufficientBuildLocation(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		if (this.requirement == RoomTracker.Requirement.Required || this.requirement == RoomTracker.Requirement.CustomRequired)
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			if (((cavityForCell != null) ? cavityForCell.room : null) == null)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060034E3 RID: 13539 RVA: 0x00218D64 File Offset: 0x00216F64
	private void OnUpdateRoom(object data)
	{
		this.room = (Room)data;
		if (this.room != null && !(this.room.roomType.Id != this.requiredRoomType))
		{
			this.statusItemGuid = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusItemGuid, false);
			return;
		}
		switch (this.requirement)
		{
		case RoomTracker.Requirement.TrackingOnly:
			this.statusItemGuid = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusItemGuid, false);
			return;
		case RoomTracker.Requirement.Recommended:
			this.statusItemGuid = base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.RequiredRoom, Db.Get().BuildingStatusItems.NotInRecommendedRoom, this.requiredRoomType);
			return;
		case RoomTracker.Requirement.Required:
			this.statusItemGuid = base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.RequiredRoom, Db.Get().BuildingStatusItems.NotInRequiredRoom, this.requiredRoomType);
			return;
		case RoomTracker.Requirement.CustomRecommended:
		case RoomTracker.Requirement.CustomRequired:
			this.statusItemGuid = base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.RequiredRoom, Db.Get().BuildingStatusItems.Get(this.customStatusItemID), this.requiredRoomType);
			return;
		default:
			return;
		}
	}

	// Token: 0x060034E4 RID: 13540 RVA: 0x00218EA0 File Offset: 0x002170A0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (!string.IsNullOrEmpty(this.requiredRoomType))
		{
			string name = Db.Get().RoomTypes.Get(this.requiredRoomType).Name;
			switch (this.requirement)
			{
			case RoomTracker.Requirement.Recommended:
			case RoomTracker.Requirement.CustomRecommended:
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.PREFERS_ROOM, name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.PREFERS_ROOM, name), Descriptor.DescriptorType.Requirement, false));
				break;
			case RoomTracker.Requirement.Required:
			case RoomTracker.Requirement.CustomRequired:
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.REQUIRESROOM, name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESROOM, name), Descriptor.DescriptorType.Requirement, false));
				break;
			}
		}
		return list;
	}

	// Token: 0x04002455 RID: 9301
	public RoomTracker.Requirement requirement;

	// Token: 0x04002456 RID: 9302
	public string requiredRoomType;

	// Token: 0x04002457 RID: 9303
	public string customStatusItemID;

	// Token: 0x04002458 RID: 9304
	private Guid statusItemGuid;

	// Token: 0x0400245A RID: 9306
	private static readonly EventSystem.IntraObjectHandler<RoomTracker> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<RoomTracker>(delegate(RoomTracker component, object data)
	{
		component.OnUpdateRoom(data);
	});

	// Token: 0x02000B24 RID: 2852
	public enum Requirement
	{
		// Token: 0x0400245C RID: 9308
		TrackingOnly,
		// Token: 0x0400245D RID: 9309
		Recommended,
		// Token: 0x0400245E RID: 9310
		Required,
		// Token: 0x0400245F RID: 9311
		CustomRecommended,
		// Token: 0x04002460 RID: 9312
		CustomRequired
	}
}
