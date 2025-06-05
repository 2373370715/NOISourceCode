using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x0200215B RID: 8539
	public abstract class InlineDrawer : MemberDrawer
	{
		// Token: 0x0600B61E RID: 46622 RVA: 0x000B1628 File Offset: 0x000AF828
		public sealed override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
		{
			return MemberDrawType.Inline;
		}

		// Token: 0x0600B61F RID: 46623 RVA: 0x0011AB49 File Offset: 0x00118D49
		protected sealed override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			this.DrawInline(context, member);
		}
	}
}
