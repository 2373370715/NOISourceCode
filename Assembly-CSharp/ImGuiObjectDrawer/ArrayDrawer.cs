using System;

namespace ImGuiObjectDrawer
{
	// Token: 0x0200216A RID: 8554
	public sealed class ArrayDrawer : CollectionDrawer
	{
		// Token: 0x0600B651 RID: 46673 RVA: 0x0011AD0A File Offset: 0x00118F0A
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.type.IsArray;
		}

		// Token: 0x0600B652 RID: 46674 RVA: 0x0011AD17 File Offset: 0x00118F17
		public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			return ((Array)member.value).Length == 0;
		}

		// Token: 0x0600B653 RID: 46675 RVA: 0x00455EB4 File Offset: 0x004540B4
		protected override void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
		{
			ArrayDrawer.<>c__DisplayClass2_0 CS$<>8__locals1 = new ArrayDrawer.<>c__DisplayClass2_0();
			CS$<>8__locals1.array = (Array)member.value;
			int i;
			int i2;
			for (i = 0; i < CS$<>8__locals1.array.Length; i = i2)
			{
				int j = i;
				System.Action draw_tooltip;
				if ((draw_tooltip = CS$<>8__locals1.<>9__0) == null)
				{
					draw_tooltip = (CS$<>8__locals1.<>9__0 = delegate()
					{
						DrawerUtil.Tooltip(CS$<>8__locals1.array.GetType().GetElementType());
					});
				}
				visit(context, new CollectionDrawer.Element(j, draw_tooltip, () => new
				{
					value = CS$<>8__locals1.array.GetValue(i)
				}));
				i2 = i + 1;
			}
		}
	}
}
