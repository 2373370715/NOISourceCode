using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C8C RID: 15500
	public class CommonSickEffectSickness : Sickness.SicknessComponent
	{
		// Token: 0x0600EDCE RID: 60878 RVA: 0x004E4230 File Offset: 0x004E2430
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("contaminated_crew_fx_kanim", go.transform.GetPosition() + new Vector3(0f, 0f, -0.1f), go.transform, true, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play("fx_loop", KAnim.PlayMode.Loop, 1f, 0f);
			return kbatchedAnimController;
		}

		// Token: 0x0600EDCF RID: 60879 RVA: 0x00144096 File Offset: 0x00142296
		public override void OnCure(GameObject go, object instance_data)
		{
			((KAnimControllerBase)instance_data).gameObject.DeleteObject();
		}
	}
}
