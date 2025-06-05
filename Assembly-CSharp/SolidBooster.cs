using System;
using UnityEngine;

// Token: 0x02001994 RID: 6548
public class SolidBooster : RocketEngine
{
	// Token: 0x06008860 RID: 34912 RVA: 0x000FDC30 File Offset: 0x000FBE30
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SolidBooster>(-887025858, SolidBooster.OnRocketLandedDelegate);
	}

	// Token: 0x06008861 RID: 34913 RVA: 0x0036297C File Offset: 0x00360B7C
	[ContextMenu("Fill Tank")]
	public void FillTank()
	{
		Element element = ElementLoader.GetElement(this.fuelTag);
		GameObject go = element.substance.SpawnResource(base.gameObject.transform.GetPosition(), this.fuelStorage.capacityKg / 2f, element.defaultValues.temperature, byte.MaxValue, 0, false, false, false);
		this.fuelStorage.Store(go, false, false, true, false);
		element = ElementLoader.GetElement(GameTags.OxyRock);
		go = element.substance.SpawnResource(base.gameObject.transform.GetPosition(), this.fuelStorage.capacityKg / 2f, element.defaultValues.temperature, byte.MaxValue, 0, false, false, false);
		this.fuelStorage.Store(go, false, false, true, false);
	}

	// Token: 0x06008862 RID: 34914 RVA: 0x00362A44 File Offset: 0x00360C44
	private void OnRocketLanded(object data)
	{
		if (this.fuelStorage != null && this.fuelStorage.items != null)
		{
			for (int i = this.fuelStorage.items.Count - 1; i >= 0; i--)
			{
				Util.KDestroyGameObject(this.fuelStorage.items[i]);
			}
			this.fuelStorage.items.Clear();
		}
	}

	// Token: 0x04006751 RID: 26449
	public Storage fuelStorage;

	// Token: 0x04006752 RID: 26450
	private static readonly EventSystem.IntraObjectHandler<SolidBooster> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<SolidBooster>(delegate(SolidBooster component, object data)
	{
		component.OnRocketLanded(data);
	});
}
