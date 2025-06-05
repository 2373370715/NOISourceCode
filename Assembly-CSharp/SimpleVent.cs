using System;
using UnityEngine;

// Token: 0x02000B3B RID: 2875
[AddComponentMenu("KMonoBehaviour/scripts/SimpleVent")]
public class SimpleVent : KMonoBehaviour
{
	// Token: 0x0600355C RID: 13660 RVA: 0x000C73B0 File Offset: 0x000C55B0
	protected override void OnPrefabInit()
	{
		base.Subscribe<SimpleVent>(-592767678, SimpleVent.OnChangedDelegate);
		base.Subscribe<SimpleVent>(-111137758, SimpleVent.OnChangedDelegate);
	}

	// Token: 0x0600355D RID: 13661 RVA: 0x000C73D4 File Offset: 0x000C55D4
	protected override void OnSpawn()
	{
		this.OnChanged(null);
	}

	// Token: 0x0600355E RID: 13662 RVA: 0x0021B564 File Offset: 0x00219764
	private void OnChanged(object data)
	{
		if (this.operational.IsFunctional)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, this);
			return;
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
	}

	// Token: 0x040024D6 RID: 9430
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040024D7 RID: 9431
	private static readonly EventSystem.IntraObjectHandler<SimpleVent> OnChangedDelegate = new EventSystem.IntraObjectHandler<SimpleVent>(delegate(SimpleVent component, object data)
	{
		component.OnChanged(data);
	});
}
