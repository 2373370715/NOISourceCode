using System;
using ImGuiNET;

namespace ImGuiObjectDrawer
{
	// Token: 0x02002172 RID: 8562
	public class PlainCSharpObjectDrawer : MemberDrawer
	{
		// Token: 0x0600B66A RID: 46698 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return true;
		}

		// Token: 0x0600B66B RID: 46699 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
		{
			return MemberDrawType.Custom;
		}

		// Token: 0x0600B66C RID: 46700 RVA: 0x0011AE2C File Offset: 0x0011902C
		protected override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600B66D RID: 46701 RVA: 0x004560D0 File Offset: 0x004542D0
		protected override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			ImGuiTreeNodeFlags imGuiTreeNodeFlags = ImGuiTreeNodeFlags.None;
			if (context.default_open && depth <= 0)
			{
				imGuiTreeNodeFlags |= ImGuiTreeNodeFlags.DefaultOpen;
			}
			bool flag = ImGui.TreeNodeEx(member.name, imGuiTreeNodeFlags);
			DrawerUtil.Tooltip(member.type);
			if (flag)
			{
				this.DrawContents(context, member, depth);
				ImGui.TreePop();
			}
		}

		// Token: 0x0600B66E RID: 46702 RVA: 0x0011AE33 File Offset: 0x00119033
		protected virtual void DrawContents(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			DrawerUtil.DrawObjectContents(member.value, context, depth + 1);
		}
	}
}
