using System;
using UnityEngine;

// Token: 0x02001062 RID: 4194
public class UraniumCentrifuge : ComplexFabricator
{
	// Token: 0x06005538 RID: 21816 RVA: 0x000DBE99 File Offset: 0x000DA099
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<UraniumCentrifuge>(-1697596308, UraniumCentrifuge.DropEnrichedProductDelegate);
		base.Subscribe<UraniumCentrifuge>(-2094018600, UraniumCentrifuge.CheckPipesDelegate);
	}

	// Token: 0x06005539 RID: 21817 RVA: 0x0028C07C File Offset: 0x0028A27C
	private void DropEnrichedProducts(object data)
	{
		Storage[] components = base.GetComponents<Storage>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].Drop(ElementLoader.FindElementByHash(SimHashes.EnrichedUranium).tag);
		}
	}

	// Token: 0x0600553A RID: 21818 RVA: 0x0028C0B8 File Offset: 0x0028A2B8
	private void CheckPipes(object data)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		int cell = Grid.OffsetCell(Grid.PosToCell(this), UraniumCentrifugeConfig.outPipeOffset);
		GameObject gameObject = Grid.Objects[cell, 16];
		if (!(gameObject != null))
		{
			component.RemoveStatusItem(this.statusHandle, false);
			return;
		}
		if (gameObject.GetComponent<PrimaryElement>().Element.highTemp > ElementLoader.FindElementByHash(SimHashes.MoltenUranium).lowTemp)
		{
			component.RemoveStatusItem(this.statusHandle, false);
			return;
		}
		this.statusHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.PipeMayMelt, null);
	}

	// Token: 0x04003C3E RID: 15422
	private Guid statusHandle;

	// Token: 0x04003C3F RID: 15423
	private static readonly EventSystem.IntraObjectHandler<UraniumCentrifuge> CheckPipesDelegate = new EventSystem.IntraObjectHandler<UraniumCentrifuge>(delegate(UraniumCentrifuge component, object data)
	{
		component.CheckPipes(data);
	});

	// Token: 0x04003C40 RID: 15424
	private static readonly EventSystem.IntraObjectHandler<UraniumCentrifuge> DropEnrichedProductDelegate = new EventSystem.IntraObjectHandler<UraniumCentrifuge>(delegate(UraniumCentrifuge component, object data)
	{
		component.DropEnrichedProducts(data);
	});
}
