using System;

// Token: 0x020009D7 RID: 2519
[SkipSaveFileSerialization]
public class CancellableDig : Cancellable
{
	// Token: 0x06002D95 RID: 11669 RVA: 0x001FE930 File Offset: 0x001FCB30
	protected override void OnCancel(object data)
	{
		if (data != null && (bool)data)
		{
			this.OnAnimationDone("ScaleDown");
			return;
		}
		EasingAnimations componentInChildren = base.GetComponentInChildren<EasingAnimations>();
		int num = Grid.PosToCell(this);
		if (componentInChildren.IsPlaying && Grid.Element[num].hardness == 255)
		{
			EasingAnimations easingAnimations = componentInChildren;
			easingAnimations.OnAnimationDone = (Action<string>)Delegate.Combine(easingAnimations.OnAnimationDone, new Action<string>(this.DoCancelAnim));
			return;
		}
		EasingAnimations easingAnimations2 = componentInChildren;
		easingAnimations2.OnAnimationDone = (Action<string>)Delegate.Combine(easingAnimations2.OnAnimationDone, new Action<string>(this.OnAnimationDone));
		componentInChildren.PlayAnimation("ScaleDown", 0.1f);
	}

	// Token: 0x06002D96 RID: 11670 RVA: 0x001FE9D8 File Offset: 0x001FCBD8
	private void DoCancelAnim(string animName)
	{
		EasingAnimations componentInChildren = base.GetComponentInChildren<EasingAnimations>();
		componentInChildren.OnAnimationDone = (Action<string>)Delegate.Remove(componentInChildren.OnAnimationDone, new Action<string>(this.DoCancelAnim));
		componentInChildren.OnAnimationDone = (Action<string>)Delegate.Combine(componentInChildren.OnAnimationDone, new Action<string>(this.OnAnimationDone));
		componentInChildren.PlayAnimation("ScaleDown", 0.1f);
	}

	// Token: 0x06002D97 RID: 11671 RVA: 0x000C1FE7 File Offset: 0x000C01E7
	private void OnAnimationDone(string animationName)
	{
		if (animationName != "ScaleDown")
		{
			return;
		}
		EasingAnimations componentInChildren = base.GetComponentInChildren<EasingAnimations>();
		componentInChildren.OnAnimationDone = (Action<string>)Delegate.Remove(componentInChildren.OnAnimationDone, new Action<string>(this.OnAnimationDone));
		this.DeleteObject();
	}
}
