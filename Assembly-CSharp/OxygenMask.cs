using System;
using UnityEngine;

// Token: 0x020016E1 RID: 5857
public class OxygenMask : KMonoBehaviour, ISim200ms
{
	// Token: 0x060078D8 RID: 30936 RVA: 0x000F3E91 File Offset: 0x000F2091
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OxygenMask>(608245985, OxygenMask.OnSuitTankDeltaDelegate);
	}

	// Token: 0x060078D9 RID: 30937 RVA: 0x00321328 File Offset: 0x0031F528
	private void CheckOxygenLevels(object data)
	{
		if (this.suitTank.IsEmpty())
		{
			Equippable component = base.GetComponent<Equippable>();
			if (component.assignee != null)
			{
				Ownables soleOwner = component.assignee.GetSoleOwner();
				if (soleOwner != null)
				{
					soleOwner.GetComponent<Equipment>().Unequip(component);
				}
			}
		}
	}

	// Token: 0x060078DA RID: 30938 RVA: 0x00321374 File Offset: 0x0031F574
	public void Sim200ms(float dt)
	{
		if (base.GetComponent<Equippable>().assignee == null)
		{
			float num = this.leakRate * dt;
			float massAvailable = this.storage.GetMassAvailable(this.suitTank.elementTag);
			num = Mathf.Min(num, massAvailable);
			this.storage.DropSome(this.suitTank.elementTag, num, true, true, default(Vector3), true, false);
		}
		if (this.suitTank.IsEmpty())
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x04005ABD RID: 23229
	private static readonly EventSystem.IntraObjectHandler<OxygenMask> OnSuitTankDeltaDelegate = new EventSystem.IntraObjectHandler<OxygenMask>(delegate(OxygenMask component, object data)
	{
		component.CheckOxygenLevels(data);
	});

	// Token: 0x04005ABE RID: 23230
	[MyCmpGet]
	private SuitTank suitTank;

	// Token: 0x04005ABF RID: 23231
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04005AC0 RID: 23232
	private float leakRate = 0.1f;
}
