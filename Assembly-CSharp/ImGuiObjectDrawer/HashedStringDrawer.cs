using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02002161 RID: 8545
	public sealed class HashedStringDrawer : InlineDrawer
	{
		// Token: 0x0600B632 RID: 46642 RVA: 0x0011ABF9 File Offset: 0x00118DF9
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is HashedString;
		}

		// Token: 0x0600B633 RID: 46643 RVA: 0x00455C88 File Offset: 0x00453E88
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			HashedString hashedString = (HashedString)member.value;
			string str = hashedString.ToString();
			string str2 = "0x" + hashedString.HashValue.ToString("X");
			ImGuiEx.SimpleField(member.name, str + " (" + str2 + ")");
		}
	}
}
