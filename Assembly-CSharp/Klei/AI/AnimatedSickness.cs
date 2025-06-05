using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C8A RID: 15498
	public class AnimatedSickness : Sickness.SicknessComponent
	{
		// Token: 0x0600EDC4 RID: 60868 RVA: 0x004E3D94 File Offset: 0x004E1F94
		public AnimatedSickness(HashedString[] kanim_filenames, Expression expression)
		{
			this.kanims = new KAnimFile[kanim_filenames.Length];
			for (int i = 0; i < kanim_filenames.Length; i++)
			{
				this.kanims[i] = Assets.GetAnim(kanim_filenames[i]);
			}
			this.expression = expression;
		}

		// Token: 0x0600EDC5 RID: 60869 RVA: 0x004E3DE0 File Offset: 0x004E1FE0
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			for (int i = 0; i < this.kanims.Length; i++)
			{
				go.GetComponent<KAnimControllerBase>().AddAnimOverrides(this.kanims[i], 10f);
			}
			if (this.expression != null)
			{
				go.GetComponent<FaceGraph>().AddExpression(this.expression);
			}
			return null;
		}

		// Token: 0x0600EDC6 RID: 60870 RVA: 0x004E3E34 File Offset: 0x004E2034
		public override void OnCure(GameObject go, object instace_data)
		{
			if (this.expression != null)
			{
				go.GetComponent<FaceGraph>().RemoveExpression(this.expression);
			}
			for (int i = 0; i < this.kanims.Length; i++)
			{
				go.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(this.kanims[i]);
			}
		}

		// Token: 0x0400E9C3 RID: 59843
		private KAnimFile[] kanims;

		// Token: 0x0400E9C4 RID: 59844
		private Expression expression;
	}
}
