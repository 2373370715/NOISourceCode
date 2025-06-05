using System;
using ImGuiNET;

namespace ImGuiObjectDrawer
{
	// Token: 0x02002166 RID: 8550
	public abstract class CollectionDrawer : MemberDrawer
	{
		// Token: 0x0600B641 RID: 46657
		public abstract bool IsEmpty(in MemberDrawContext context, in MemberDetails member);

		// Token: 0x0600B642 RID: 46658 RVA: 0x0011AC49 File Offset: 0x00118E49
		public override MemberDrawType GetDrawType(in MemberDrawContext context, in MemberDetails member)
		{
			if (this.IsEmpty(context, member))
			{
				return MemberDrawType.Inline;
			}
			return MemberDrawType.Custom;
		}

		// Token: 0x0600B643 RID: 46659 RVA: 0x0011AC58 File Offset: 0x00118E58
		protected sealed override void DrawInline(in MemberDrawContext context, in MemberDetails member)
		{
			Debug.Assert(this.IsEmpty(context, member));
			this.DrawEmpty(context, member);
		}

		// Token: 0x0600B644 RID: 46660 RVA: 0x0011AC6F File Offset: 0x00118E6F
		protected sealed override void DrawCustom(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			Debug.Assert(!this.IsEmpty(context, member));
			this.DrawWithContents(context, member, depth);
		}

		// Token: 0x0600B645 RID: 46661 RVA: 0x0011AC8A File Offset: 0x00118E8A
		private void DrawEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			ImGui.Text(member.name + "(empty)");
		}

		// Token: 0x0600B646 RID: 46662 RVA: 0x00455E50 File Offset: 0x00454050
		private void DrawWithContents(in MemberDrawContext context, in MemberDetails member, int depth)
		{
			CollectionDrawer.<>c__DisplayClass5_0 CS$<>8__locals1 = new CollectionDrawer.<>c__DisplayClass5_0();
			CS$<>8__locals1.depth = depth;
			ImGuiTreeNodeFlags imGuiTreeNodeFlags = ImGuiTreeNodeFlags.None;
			if (context.default_open && CS$<>8__locals1.depth <= 0)
			{
				imGuiTreeNodeFlags |= ImGuiTreeNodeFlags.DefaultOpen;
			}
			bool flag = ImGui.TreeNodeEx(member.name, imGuiTreeNodeFlags);
			DrawerUtil.Tooltip(member.type);
			if (flag)
			{
				this.VisitElements(new CollectionDrawer.ElementVisitor(CS$<>8__locals1.<DrawWithContents>g__Visitor|0), context, member);
				ImGui.TreePop();
			}
		}

		// Token: 0x0600B647 RID: 46663
		protected abstract void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member);

		// Token: 0x02002167 RID: 8551
		// (Invoke) Token: 0x0600B64A RID: 46666
		protected delegate void ElementVisitor(in MemberDrawContext context, CollectionDrawer.Element element);

		// Token: 0x02002168 RID: 8552
		protected struct Element
		{
			// Token: 0x0600B64D RID: 46669 RVA: 0x0011ACA1 File Offset: 0x00118EA1
			public Element(string node_name, System.Action draw_tooltip, Func<object> get_object_to_inspect)
			{
				this.node_name = node_name;
				this.draw_tooltip = draw_tooltip;
				this.get_object_to_inspect = get_object_to_inspect;
			}

			// Token: 0x0600B64E RID: 46670 RVA: 0x0011ACB8 File Offset: 0x00118EB8
			public Element(int index, System.Action draw_tooltip, Func<object> get_object_to_inspect)
			{
				this = new CollectionDrawer.Element(string.Format("[{0}]", index), draw_tooltip, get_object_to_inspect);
			}

			// Token: 0x04009018 RID: 36888
			public readonly string node_name;

			// Token: 0x04009019 RID: 36889
			public readonly System.Action draw_tooltip;

			// Token: 0x0400901A RID: 36890
			public readonly Func<object> get_object_to_inspect;
		}
	}
}
