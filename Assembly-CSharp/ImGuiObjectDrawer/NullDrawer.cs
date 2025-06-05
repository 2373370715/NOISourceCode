using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x0200215C RID: 8540
	public class NullDrawer : InlineDrawer
	{
		// Token: 0x0600B621 RID: 46625 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public override bool CanDrawAtDepth(int depth)
		{
			return true;
		}

		// Token: 0x0600B622 RID: 46626 RVA: 0x0011AB5B File Offset: 0x00118D5B
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value == null;
		}

		// Token: 0x0600B623 RID: 46627 RVA: 0x0011AB66 File Offset: 0x00118D66
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, "null");
		}
	}
}
