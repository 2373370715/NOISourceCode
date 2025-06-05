using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000F44 RID: 3908
[AddComponentMenu("KMonoBehaviour/scripts/ParkSign")]
public class ParkSign : KMonoBehaviour
{
	// Token: 0x06004E62 RID: 20066 RVA: 0x000D74DC File Offset: 0x000D56DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ParkSign>(-832141045, ParkSign.TriggerRoomEffectsDelegate);
	}

	// Token: 0x06004E63 RID: 20067 RVA: 0x00276428 File Offset: 0x00274628
	private void TriggerRoomEffects(object data)
	{
		GameObject gameObject = (GameObject)data;
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			roomOfGameObject.roomType.TriggerRoomEffects(base.gameObject.GetComponent<KPrefabID>(), gameObject.GetComponent<Effects>());
		}
	}

	// Token: 0x04003704 RID: 14084
	private static readonly EventSystem.IntraObjectHandler<ParkSign> TriggerRoomEffectsDelegate = new EventSystem.IntraObjectHandler<ParkSign>(delegate(ParkSign component, object data)
	{
		component.TriggerRoomEffects(data);
	});
}
