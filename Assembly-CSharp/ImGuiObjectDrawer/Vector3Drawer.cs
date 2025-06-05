using System;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02002164 RID: 8548
	public sealed class Vector3Drawer : InlineDrawer
	{
		// Token: 0x0600B63B RID: 46651 RVA: 0x0011AC29 File Offset: 0x00118E29
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is Vector3;
		}

		// Token: 0x0600B63C RID: 46652 RVA: 0x00455D94 File Offset: 0x00453F94
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Vector3 vector = (Vector3)member.value;
			ImGuiEx.SimpleField(member.name, string.Format("( {0}, {1}, {2} )", vector.x, vector.y, vector.z));
		}
	}
}
