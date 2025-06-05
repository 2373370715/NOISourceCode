using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x02002162 RID: 8546
	public sealed class KAnimHashedStringDrawer : InlineDrawer
	{
		// Token: 0x0600B635 RID: 46645 RVA: 0x0011AC09 File Offset: 0x00118E09
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is KAnimHashedString;
		}

		// Token: 0x0600B636 RID: 46646 RVA: 0x00455CEC File Offset: 0x00453EEC
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			KAnimHashedString kanimHashedString = (KAnimHashedString)member.value;
			string str = kanimHashedString.ToString();
			string str2 = "0x" + kanimHashedString.HashValue.ToString("X");
			ImGuiEx.SimpleField(member.name, str + " (" + str2 + ")");
		}
	}
}
