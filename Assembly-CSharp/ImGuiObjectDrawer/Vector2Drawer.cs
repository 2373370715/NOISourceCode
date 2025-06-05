using System;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02002163 RID: 8547
	public sealed class Vector2Drawer : InlineDrawer
	{
		// Token: 0x0600B638 RID: 46648 RVA: 0x0011AC19 File Offset: 0x00118E19
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is Vector2;
		}

		// Token: 0x0600B639 RID: 46649 RVA: 0x00455D50 File Offset: 0x00453F50
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Vector2 vector = (Vector2)member.value;
			ImGuiEx.SimpleField(member.name, string.Format("( {0}, {1} )", vector.x, vector.y));
		}
	}
}
