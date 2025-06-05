using System;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02002165 RID: 8549
	public sealed class Vector4Drawer : InlineDrawer
	{
		// Token: 0x0600B63E RID: 46654 RVA: 0x0011AC39 File Offset: 0x00118E39
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is Vector4;
		}

		// Token: 0x0600B63F RID: 46655 RVA: 0x00455DE4 File Offset: 0x00453FE4
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Vector4 vector = (Vector4)member.value;
			ImGuiEx.SimpleField(member.name, string.Format("( {0}, {1}, {2}, {3} )", new object[]
			{
				vector.x,
				vector.y,
				vector.z,
				vector.w
			}));
		}
	}
}
