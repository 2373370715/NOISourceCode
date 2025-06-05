using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C8D RID: 15501
	public class CustomSickEffectSickness : Sickness.SicknessComponent
	{
		// Token: 0x0600EDD1 RID: 60881 RVA: 0x001440A8 File Offset: 0x001422A8
		public CustomSickEffectSickness(string effect_kanim, string effect_anim_name)
		{
			this.kanim = effect_kanim;
			this.animName = effect_anim_name;
		}

		// Token: 0x0600EDD2 RID: 60882 RVA: 0x004E4290 File Offset: 0x004E2490
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(this.kanim, go.transform.GetPosition() + new Vector3(0f, 0f, -0.1f), go.transform, true, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play(this.animName, KAnim.PlayMode.Loop, 1f, 0f);
			return kbatchedAnimController;
		}

		// Token: 0x0600EDD3 RID: 60883 RVA: 0x00144096 File Offset: 0x00142296
		public override void OnCure(GameObject go, object instance_data)
		{
			((KAnimControllerBase)instance_data).gameObject.DeleteObject();
		}

		// Token: 0x0400E9C7 RID: 59847
		private string kanim;

		// Token: 0x0400E9C8 RID: 59848
		private string animName;
	}
}
