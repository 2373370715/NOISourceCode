using System;

// Token: 0x02000B8C RID: 2956
public class FullPuftTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x06003774 RID: 14196 RVA: 0x000C8704 File Offset: 0x000C6904
	public FullPuftTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x06003775 RID: 14197 RVA: 0x002245FC File Offset: 0x002227FC
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		CreatureCalorieMonitor.Instance smi = navigator.GetSMI<CreatureCalorieMonitor.Instance>();
		if (smi != null && smi.stomach.IsReadyToPoop())
		{
			string s = HashCache.Get().Get(transition.anim.HashValue) + "_full";
			if (navigator.animController.HasAnimation(s))
			{
				transition.anim = s;
			}
		}
	}
}
