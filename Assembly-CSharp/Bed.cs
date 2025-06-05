using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000CE0 RID: 3296
[AddComponentMenu("KMonoBehaviour/Workable/Bed")]
public class Bed : Workable, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x06003F22 RID: 16162 RVA: 0x000CD82E File Offset: 0x000CBA2E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
	}

	// Token: 0x06003F23 RID: 16163 RVA: 0x00244800 File Offset: 0x00242A00
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.BasicBuildings.Add(this);
		this.sleepable = base.GetComponent<Sleepable>();
		Sleepable sleepable = this.sleepable;
		sleepable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(sleepable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
	}

	// Token: 0x06003F24 RID: 16164 RVA: 0x000CD83D File Offset: 0x000CBA3D
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent workable_event)
	{
		if (workable_event == Workable.WorkableEvent.WorkStarted)
		{
			this.AddEffects();
			return;
		}
		if (workable_event == Workable.WorkableEvent.WorkStopped)
		{
			this.RemoveEffects();
		}
	}

	// Token: 0x06003F25 RID: 16165 RVA: 0x00244854 File Offset: 0x00242A54
	private void AddEffects()
	{
		this.targetWorker = this.sleepable.worker;
		if (this.effects != null)
		{
			foreach (string effect_id in this.effects)
			{
				this.targetWorker.GetComponent<Effects>().Add(effect_id, false);
			}
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject == null)
		{
			return;
		}
		RoomType roomType = roomOfGameObject.roomType;
		foreach (KeyValuePair<string, string> keyValuePair in Bed.roomSleepingEffects)
		{
			if (keyValuePair.Key == roomType.Id)
			{
				this.targetWorker.GetComponent<Effects>().Add(keyValuePair.Value, false);
			}
		}
		roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), this.targetWorker.GetComponent<Effects>());
	}

	// Token: 0x06003F26 RID: 16166 RVA: 0x00244950 File Offset: 0x00242B50
	private void RemoveEffects()
	{
		if (this.targetWorker == null)
		{
			return;
		}
		if (this.effects != null)
		{
			foreach (string effect_id in this.effects)
			{
				this.targetWorker.GetComponent<Effects>().Remove(effect_id);
			}
		}
		foreach (KeyValuePair<string, string> keyValuePair in Bed.roomSleepingEffects)
		{
			this.targetWorker.GetComponent<Effects>().Remove(keyValuePair.Value);
		}
		this.targetWorker = null;
	}

	// Token: 0x06003F27 RID: 16167 RVA: 0x002449FC File Offset: 0x00242BFC
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.effects != null)
		{
			foreach (string text in this.effects)
			{
				if (text != null && text != "")
				{
					Effect.AddModifierDescriptions(base.gameObject, list, text, false);
				}
			}
		}
		return list;
	}

	// Token: 0x06003F28 RID: 16168 RVA: 0x00244A50 File Offset: 0x00242C50
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.BasicBuildings.Remove(this);
		if (this.sleepable != null)
		{
			Sleepable sleepable = this.sleepable;
			sleepable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(sleepable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
		}
	}

	// Token: 0x04002BAB RID: 11179
	[MyCmpReq]
	private Sleepable sleepable;

	// Token: 0x04002BAC RID: 11180
	private WorkerBase targetWorker;

	// Token: 0x04002BAD RID: 11181
	public string[] effects;

	// Token: 0x04002BAE RID: 11182
	public static readonly Dictionary<string, string> roomSleepingEffects = new Dictionary<string, string>
	{
		{
			"Barracks",
			"BarracksStamina"
		},
		{
			"Luxury Barracks",
			"BarracksStamina"
		},
		{
			"Private Bedroom",
			"BedroomStamina"
		}
	};
}
