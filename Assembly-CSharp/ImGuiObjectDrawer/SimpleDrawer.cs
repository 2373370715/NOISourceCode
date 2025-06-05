using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x0200215D RID: 8541
	public class SimpleDrawer : InlineDrawer
	{
		// Token: 0x0600B625 RID: 46629 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public override bool CanDrawAtDepth(int depth)
		{
			return true;
		}

		// Token: 0x0600B626 RID: 46630 RVA: 0x0011AB80 File Offset: 0x00118D80
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.type.IsPrimitive || member.CanAssignToType<string>();
		}

		// Token: 0x0600B627 RID: 46631 RVA: 0x0011AB97 File Offset: 0x00118D97
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, member.value.ToString());
		}
	}
}
