using System;
using System.Collections.Generic;

namespace Klei.AI
{
	// Token: 0x02003CE3 RID: 15587
	public class Emote : Resource
	{
		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x0600EF4A RID: 61258 RVA: 0x00145102 File Offset: 0x00143302
		public int StepCount
		{
			get
			{
				if (this.emoteSteps != null)
				{
					return this.emoteSteps.Count;
				}
				return 0;
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x0600EF4B RID: 61259 RVA: 0x00145119 File Offset: 0x00143319
		public KAnimFile AnimSet
		{
			get
			{
				if (this.animSetName != HashedString.Invalid && this.animSet == null)
				{
					this.animSet = Assets.GetAnim(this.animSetName);
				}
				return this.animSet;
			}
		}

		// Token: 0x0600EF4C RID: 61260 RVA: 0x00145152 File Offset: 0x00143352
		public Emote(ResourceSet parent, string emoteId, EmoteStep[] defaultSteps, string animSetName = null) : base(emoteId, parent, null)
		{
			this.emoteSteps.AddRange(defaultSteps);
			this.animSetName = animSetName;
		}

		// Token: 0x0600EF4D RID: 61261 RVA: 0x004E9410 File Offset: 0x004E7610
		public bool IsValidForController(KBatchedAnimController animController)
		{
			bool flag = true;
			int num = 0;
			while (flag && num < this.StepCount)
			{
				flag = animController.HasAnimation(this.emoteSteps[num].anim);
				num++;
			}
			KAnimFileData kanimFileData = (this.animSet == null) ? null : this.animSet.GetData();
			int num2 = 0;
			while (kanimFileData != null && flag && num2 < this.StepCount)
			{
				bool flag2 = false;
				int num3 = 0;
				while (!flag2 && num3 < kanimFileData.animCount)
				{
					flag2 = (kanimFileData.GetAnim(num2).id == this.emoteSteps[num2].anim);
					num3++;
				}
				flag = flag2;
				num2++;
			}
			return flag;
		}

		// Token: 0x0600EF4E RID: 61262 RVA: 0x004E94C8 File Offset: 0x004E76C8
		public void ApplyAnimOverrides(KBatchedAnimController animController, KAnimFile overrideSet)
		{
			KAnimFile kanimFile = (overrideSet != null) ? overrideSet : this.AnimSet;
			if (kanimFile == null || animController == null)
			{
				return;
			}
			animController.AddAnimOverrides(kanimFile, 0f);
		}

		// Token: 0x0600EF4F RID: 61263 RVA: 0x004E9508 File Offset: 0x004E7708
		public void RemoveAnimOverrides(KBatchedAnimController animController, KAnimFile overrideSet)
		{
			KAnimFile kanimFile = (overrideSet != null) ? overrideSet : this.AnimSet;
			if (kanimFile == null || animController == null)
			{
				return;
			}
			animController.RemoveAnimOverrides(kanimFile);
		}

		// Token: 0x0600EF50 RID: 61264 RVA: 0x004E9544 File Offset: 0x004E7744
		public void CollectStepAnims(out HashedString[] emoteAnims, int iterations)
		{
			emoteAnims = new HashedString[this.emoteSteps.Count * iterations];
			for (int i = 0; i < emoteAnims.Length; i++)
			{
				emoteAnims[i] = this.emoteSteps[i % this.emoteSteps.Count].anim;
			}
		}

		// Token: 0x0600EF51 RID: 61265 RVA: 0x0014518D File Offset: 0x0014338D
		public bool IsValidStep(int stepIdx)
		{
			return stepIdx >= 0 && stepIdx < this.emoteSteps.Count;
		}

		// Token: 0x17000C6C RID: 3180
		public EmoteStep this[int stepIdx]
		{
			get
			{
				if (!this.IsValidStep(stepIdx))
				{
					return null;
				}
				return this.emoteSteps[stepIdx];
			}
		}

		// Token: 0x0600EF53 RID: 61267 RVA: 0x004E959C File Offset: 0x004E779C
		public int GetStepIndex(HashedString animName)
		{
			int i = 0;
			bool condition = false;
			while (i < this.emoteSteps.Count)
			{
				if (this.emoteSteps[i].anim == animName)
				{
					condition = true;
					break;
				}
				i++;
			}
			Debug.Assert(condition, string.Format("Could not find emote step {0} for emote {1}!", animName, this.Id));
			return i;
		}

		// Token: 0x0400EADA RID: 60122
		private HashedString animSetName = null;

		// Token: 0x0400EADB RID: 60123
		private KAnimFile animSet;

		// Token: 0x0400EADC RID: 60124
		private List<EmoteStep> emoteSteps = new List<EmoteStep>();
	}
}
