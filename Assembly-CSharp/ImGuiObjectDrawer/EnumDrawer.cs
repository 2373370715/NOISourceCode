using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02002160 RID: 8544
	public sealed class EnumDrawer : InlineDrawer
	{
		// Token: 0x0600B62F RID: 46639 RVA: 0x0011ABEC File Offset: 0x00118DEC
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.type.IsEnum;
		}

		// Token: 0x0600B630 RID: 46640 RVA: 0x0011AB97 File Offset: 0x00118D97
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			ImGuiEx.SimpleField(member.name, member.value.ToString());
		}
	}
}
