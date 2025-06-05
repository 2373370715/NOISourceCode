using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000AFD RID: 2813
[AddComponentMenu("KMonoBehaviour/scripts/Prioritizable")]
public class Prioritizable : KMonoBehaviour
{
	// Token: 0x06003417 RID: 13335 RVA: 0x000C66D0 File Offset: 0x000C48D0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Prioritizable>(-905833192, Prioritizable.OnCopySettingsDelegate);
	}

	// Token: 0x06003418 RID: 13336 RVA: 0x002160B0 File Offset: 0x002142B0
	private void OnCopySettings(object data)
	{
		Prioritizable component = ((GameObject)data).GetComponent<Prioritizable>();
		if (component != null)
		{
			this.SetMasterPriority(component.GetMasterPriority());
		}
	}

	// Token: 0x06003419 RID: 13337 RVA: 0x002160E0 File Offset: 0x002142E0
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.masterPriority != -2147483648)
		{
			this.masterPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
			this.masterPriority = int.MinValue;
		}
		PrioritySetting prioritySetting;
		if (SaveLoader.Instance.GameInfo.IsVersionExactly(7, 2) && Prioritizable.conversions.TryGetValue(this.masterPrioritySetting, out prioritySetting))
		{
			this.masterPrioritySetting = prioritySetting;
		}
	}

	// Token: 0x0600341A RID: 13338 RVA: 0x00216144 File Offset: 0x00214344
	protected override void OnSpawn()
	{
		if (this.onPriorityChanged != null)
		{
			this.onPriorityChanged(this.masterPrioritySetting);
		}
		this.RefreshHighPriorityNotification();
		this.RefreshTopPriorityOnWorld();
		Vector3 position = base.transform.GetPosition();
		Extents extents = new Extents((int)position.x, (int)position.y, 1, 1);
		this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(base.name, this, extents, GameScenePartitioner.Instance.prioritizableObjects, null);
		Components.Prioritizables.Add(this);
	}

	// Token: 0x0600341B RID: 13339 RVA: 0x000C66E9 File Offset: 0x000C48E9
	public PrioritySetting GetMasterPriority()
	{
		return this.masterPrioritySetting;
	}

	// Token: 0x0600341C RID: 13340 RVA: 0x002161C8 File Offset: 0x002143C8
	public void SetMasterPriority(PrioritySetting priority)
	{
		if (!priority.Equals(this.masterPrioritySetting))
		{
			this.masterPrioritySetting = priority;
			if (this.onPriorityChanged != null)
			{
				this.onPriorityChanged(this.masterPrioritySetting);
			}
			this.RefreshTopPriorityOnWorld();
			this.RefreshHighPriorityNotification();
		}
	}

	// Token: 0x0600341D RID: 13341 RVA: 0x000C66F1 File Offset: 0x000C48F1
	private void RefreshTopPriorityOnWorld()
	{
		this.SetTopPriorityOnWorld(this.IsTopPriority());
	}

	// Token: 0x0600341E RID: 13342 RVA: 0x0021621C File Offset: 0x0021441C
	private void SetTopPriorityOnWorld(bool state)
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (Game.Instance == null || myWorld == null)
		{
			return;
		}
		if (state)
		{
			myWorld.AddTopPriorityPrioritizable(this);
			return;
		}
		myWorld.RemoveTopPriorityPrioritizable(this);
	}

	// Token: 0x0600341F RID: 13343 RVA: 0x000C66FF File Offset: 0x000C48FF
	public void AddRef()
	{
		this.refCount++;
		this.RefreshTopPriorityOnWorld();
		this.RefreshHighPriorityNotification();
	}

	// Token: 0x06003420 RID: 13344 RVA: 0x000C671B File Offset: 0x000C491B
	public void RemoveRef()
	{
		this.refCount--;
		if (this.IsTopPriority() || this.refCount == 0)
		{
			this.SetTopPriorityOnWorld(false);
		}
		this.RefreshHighPriorityNotification();
	}

	// Token: 0x06003421 RID: 13345 RVA: 0x000C6748 File Offset: 0x000C4948
	public bool IsPrioritizable()
	{
		return this.refCount > 0;
	}

	// Token: 0x06003422 RID: 13346 RVA: 0x000C6753 File Offset: 0x000C4953
	public bool IsTopPriority()
	{
		return this.masterPrioritySetting.priority_class == PriorityScreen.PriorityClass.topPriority && this.IsPrioritizable();
	}

	// Token: 0x06003423 RID: 13347 RVA: 0x00216260 File Offset: 0x00214460
	protected override void OnCleanUp()
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (myWorld != null)
		{
			myWorld.RemoveTopPriorityPrioritizable(this);
		}
		else
		{
			global::Debug.LogWarning("World has been destroyed before prioritizable " + base.name);
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				worldContainer.RemoveTopPriorityPrioritizable(this);
			}
		}
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
		Components.Prioritizables.Remove(this);
	}

	// Token: 0x06003424 RID: 13348 RVA: 0x0021630C File Offset: 0x0021450C
	public static void AddRef(GameObject go)
	{
		Prioritizable component = go.GetComponent<Prioritizable>();
		if (component != null)
		{
			component.AddRef();
		}
	}

	// Token: 0x06003425 RID: 13349 RVA: 0x00216330 File Offset: 0x00214530
	public static void RemoveRef(GameObject go)
	{
		Prioritizable component = go.GetComponent<Prioritizable>();
		if (component != null)
		{
			component.RemoveRef();
		}
	}

	// Token: 0x06003426 RID: 13350 RVA: 0x00216354 File Offset: 0x00214554
	private void RefreshHighPriorityNotification()
	{
		bool flag = this.masterPrioritySetting.priority_class == PriorityScreen.PriorityClass.topPriority && this.IsPrioritizable();
		if (flag && this.highPriorityStatusItem == Guid.Empty)
		{
			this.highPriorityStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.EmergencyPriority, null);
			return;
		}
		if (!flag && this.highPriorityStatusItem != Guid.Empty)
		{
			this.highPriorityStatusItem = base.GetComponent<KSelectable>().RemoveStatusItem(this.highPriorityStatusItem, false);
		}
	}

	// Token: 0x040023A7 RID: 9127
	[SerializeField]
	[Serialize]
	private int masterPriority = int.MinValue;

	// Token: 0x040023A8 RID: 9128
	[SerializeField]
	[Serialize]
	private PrioritySetting masterPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);

	// Token: 0x040023A9 RID: 9129
	public Action<PrioritySetting> onPriorityChanged;

	// Token: 0x040023AA RID: 9130
	public bool showIcon = true;

	// Token: 0x040023AB RID: 9131
	public Vector2 iconOffset;

	// Token: 0x040023AC RID: 9132
	public float iconScale = 1f;

	// Token: 0x040023AD RID: 9133
	[SerializeField]
	private int refCount;

	// Token: 0x040023AE RID: 9134
	private static readonly EventSystem.IntraObjectHandler<Prioritizable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Prioritizable>(delegate(Prioritizable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040023AF RID: 9135
	private static Dictionary<PrioritySetting, PrioritySetting> conversions = new Dictionary<PrioritySetting, PrioritySetting>
	{
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 1),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 4)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 2),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 5)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 3),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 6)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 4),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 7)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 5),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 8)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 1),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 6)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 2),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 7)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 3),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 8)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 4),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 9)
		},
		{
			new PrioritySetting(PriorityScreen.PriorityClass.high, 5),
			new PrioritySetting(PriorityScreen.PriorityClass.basic, 9)
		}
	};

	// Token: 0x040023B0 RID: 9136
	private HandleVector<int>.Handle scenePartitionerEntry;

	// Token: 0x040023B1 RID: 9137
	private Guid highPriorityStatusItem;
}
