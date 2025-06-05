using System;

// Token: 0x02000B90 RID: 2960
public class ReactableTransitionLayer : TransitionDriver.InterruptOverrideLayer
{
	// Token: 0x06003783 RID: 14211 RVA: 0x000C8772 File Offset: 0x000C6972
	public ReactableTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x06003784 RID: 14212 RVA: 0x000C877B File Offset: 0x000C697B
	protected override bool IsOverrideComplete()
	{
		return !this.reactionMonitor.IsReacting() && base.IsOverrideComplete();
	}

	// Token: 0x06003785 RID: 14213 RVA: 0x00224B20 File Offset: 0x00222D20
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (this.reactionMonitor == null)
		{
			this.reactionMonitor = navigator.GetSMI<ReactionMonitor.Instance>();
		}
		this.reactionMonitor.PollForReactables(transition);
		if (this.reactionMonitor.IsReacting())
		{
			base.BeginTransition(navigator, transition);
			transition.start = this.originalTransition.start;
			transition.end = this.originalTransition.end;
		}
	}

	// Token: 0x0400262E RID: 9774
	private ReactionMonitor.Instance reactionMonitor;
}
