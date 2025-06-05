using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x0200215F RID: 8543
	public sealed class LocStringDrawer : InlineDrawer
	{
		// Token: 0x0600B62C RID: 46636 RVA: 0x0011ABB7 File Offset: 0x00118DB7
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.CanAssignToType<LocString>();
		}

		// Token: 0x0600B62D RID: 46637 RVA: 0x0011ABBF File Offset: 0x00118DBF
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, string.Format("{0}({1})", member.value, ((LocString)member.value).text));
		}
	}
}
