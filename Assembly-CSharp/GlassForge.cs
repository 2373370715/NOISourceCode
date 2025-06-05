using System;
using UnityEngine;

// Token: 0x02000E02 RID: 3586
public class GlassForge : ComplexFabricator
{
	// Token: 0x0600460E RID: 17934 RVA: 0x000D1C2B File Offset: 0x000CFE2B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<GlassForge>(-2094018600, GlassForge.CheckPipesDelegate);
	}

	// Token: 0x0600460F RID: 17935 RVA: 0x0025C084 File Offset: 0x0025A284
	private void CheckPipes(object data)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		int cell = Grid.OffsetCell(Grid.PosToCell(this), GlassForgeConfig.outPipeOffset);
		GameObject gameObject = Grid.Objects[cell, 16];
		if (!(gameObject != null))
		{
			component.RemoveStatusItem(this.statusHandle, false);
			return;
		}
		if (gameObject.GetComponent<PrimaryElement>().Element.highTemp > ElementLoader.FindElementByHash(SimHashes.MoltenGlass).lowTemp)
		{
			component.RemoveStatusItem(this.statusHandle, false);
			return;
		}
		this.statusHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.PipeMayMelt, null);
	}

	// Token: 0x040030DF RID: 12511
	private Guid statusHandle;

	// Token: 0x040030E0 RID: 12512
	private static readonly EventSystem.IntraObjectHandler<GlassForge> CheckPipesDelegate = new EventSystem.IntraObjectHandler<GlassForge>(delegate(GlassForge component, object data)
	{
		component.CheckPipes(data);
	});
}
