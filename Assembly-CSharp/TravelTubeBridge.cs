using System;
using UnityEngine;

// Token: 0x02001046 RID: 4166
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/TravelTubeBridge")]
public class TravelTubeBridge : KMonoBehaviour, ITravelTubePiece
{
	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06005489 RID: 21641 RVA: 0x000C656E File Offset: 0x000C476E
	public Vector3 Position
	{
		get
		{
			return base.transform.GetPosition();
		}
	}

	// Token: 0x0600548A RID: 21642 RVA: 0x00289A34 File Offset: 0x00287C34
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Grid.HasTube[Grid.PosToCell(this)] = true;
		Components.ITravelTubePieces.Add(this);
		base.Subscribe<TravelTubeBridge>(774203113, TravelTubeBridge.OnBuildingBrokenDelegate);
		base.Subscribe<TravelTubeBridge>(-1735440190, TravelTubeBridge.OnBuildingFullyRepairedDelegate);
	}

	// Token: 0x0600548B RID: 21643 RVA: 0x00289A88 File Offset: 0x00287C88
	protected override void OnCleanUp()
	{
		base.Unsubscribe<TravelTubeBridge>(774203113, TravelTubeBridge.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<TravelTubeBridge>(-1735440190, TravelTubeBridge.OnBuildingFullyRepairedDelegate, false);
		Grid.HasTube[Grid.PosToCell(this)] = false;
		Components.ITravelTubePieces.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600548C RID: 21644 RVA: 0x000AA038 File Offset: 0x000A8238
	private void OnBuildingBroken(object data)
	{
	}

	// Token: 0x0600548D RID: 21645 RVA: 0x000AA038 File Offset: 0x000A8238
	private void OnBuildingFullyRepaired(object data)
	{
	}

	// Token: 0x04003B99 RID: 15257
	private static readonly EventSystem.IntraObjectHandler<TravelTubeBridge> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<TravelTubeBridge>(delegate(TravelTubeBridge component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04003B9A RID: 15258
	private static readonly EventSystem.IntraObjectHandler<TravelTubeBridge> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<TravelTubeBridge>(delegate(TravelTubeBridge component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});
}
