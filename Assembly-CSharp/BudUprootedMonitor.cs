using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001170 RID: 4464
[AddComponentMenu("KMonoBehaviour/scripts/BudUprootedMonitor")]
public class BudUprootedMonitor : KMonoBehaviour
{
	// Token: 0x1700056C RID: 1388
	// (get) Token: 0x06005AF9 RID: 23289 RVA: 0x000DFAE6 File Offset: 0x000DDCE6
	public bool IsUprooted
	{
		get
		{
			return this.uprooted || base.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted);
		}
	}

	// Token: 0x06005AFA RID: 23290 RVA: 0x000DFB02 File Offset: 0x000DDD02
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<BudUprootedMonitor>(-216549700, BudUprootedMonitor.OnUprootedDelegate);
	}

	// Token: 0x06005AFB RID: 23291 RVA: 0x000DFB1B File Offset: 0x000DDD1B
	public void SetParentObject(KPrefabID id)
	{
		this.parentObject = new Ref<KPrefabID>(id);
		base.Subscribe(id.gameObject, 1969584890, new Action<object>(this.OnLoseParent));
	}

	// Token: 0x06005AFC RID: 23292 RVA: 0x002A4D84 File Offset: 0x002A2F84
	private void OnLoseParent(object obj)
	{
		if (!this.uprooted && !base.isNull)
		{
			base.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			this.uprooted = true;
			base.Trigger(-216549700, null);
			if (this.destroyOnParentLost)
			{
				Util.KDestroyGameObject(base.gameObject);
			}
		}
	}

	// Token: 0x06005AFD RID: 23293 RVA: 0x000C4795 File Offset: 0x000C2995
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06005AFE RID: 23294 RVA: 0x002A4DD8 File Offset: 0x002A2FD8
	public static bool IsObjectUprooted(GameObject plant)
	{
		BudUprootedMonitor component = plant.GetComponent<BudUprootedMonitor>();
		return !(component == null) && component.IsUprooted;
	}

	// Token: 0x040040C2 RID: 16578
	[Serialize]
	public bool canBeUprooted = true;

	// Token: 0x040040C3 RID: 16579
	[Serialize]
	private bool uprooted;

	// Token: 0x040040C4 RID: 16580
	public bool destroyOnParentLost;

	// Token: 0x040040C5 RID: 16581
	public Ref<KPrefabID> parentObject = new Ref<KPrefabID>();

	// Token: 0x040040C6 RID: 16582
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040040C7 RID: 16583
	private static readonly EventSystem.IntraObjectHandler<BudUprootedMonitor> OnUprootedDelegate = new EventSystem.IntraObjectHandler<BudUprootedMonitor>(delegate(BudUprootedMonitor component, object data)
	{
		if (!component.uprooted)
		{
			component.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			component.uprooted = true;
			component.Trigger(-216549700, null);
		}
	});
}
