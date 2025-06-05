using System;
using ImGuiNET;
using UnityEngine;

namespace ImGuiObjectDrawer
{
	// Token: 0x02002171 RID: 8561
	public class UnityObjectDrawer : PlainCSharpObjectDrawer
	{
		// Token: 0x0600B667 RID: 46695 RVA: 0x0011AE14 File Offset: 0x00119014
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.value is UnityEngine.Object;
		}

		// Token: 0x0600B668 RID: 46696 RVA: 0x0045607C File Offset: 0x0045427C
		protected override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			UnityEngine.Object @object = (UnityEngine.Object)member.value;
			ImGuiTreeNodeFlags imGuiTreeNodeFlags = ImGuiTreeNodeFlags.None;
			if (context.default_open && depth <= 0)
			{
				imGuiTreeNodeFlags |= ImGuiTreeNodeFlags.DefaultOpen;
			}
			bool flag = ImGui.TreeNodeEx(member.name, imGuiTreeNodeFlags);
			DrawerUtil.Tooltip(member.type);
			if (flag)
			{
				base.DrawContents(context, member, depth);
				ImGui.TreePop();
			}
		}
	}
}
