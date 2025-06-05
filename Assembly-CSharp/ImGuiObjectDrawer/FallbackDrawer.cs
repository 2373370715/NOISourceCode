using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x0200215E RID: 8542
	public sealed class FallbackDrawer : SimpleDrawer
	{
		// Token: 0x0600B629 RID: 46633 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return true;
		}

		// Token: 0x0600B62A RID: 46634 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public override bool CanDrawAtDepth(int depth)
		{
			return true;
		}
	}
}
