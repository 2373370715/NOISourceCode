using System;
using System.Collections;

namespace ImGuiObjectDrawer
{
	// Token: 0x0200216D RID: 8557
	public sealed class IDictionaryDrawer : CollectionDrawer
	{
		// Token: 0x0600B659 RID: 46681 RVA: 0x0011AD68 File Offset: 0x00118F68
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.CanAssignToType<IDictionary>();
		}

		// Token: 0x0600B65A RID: 46682 RVA: 0x0011AD70 File Offset: 0x00118F70
		public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			return ((IDictionary)member.value).Count == 0;
		}

		// Token: 0x0600B65B RID: 46683 RVA: 0x00455F64 File Offset: 0x00454164
		protected override void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
		{
			IDictionary dictionary = (IDictionary)member.value;
			int num = 0;
			using (IDictionaryEnumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DictionaryEntry kvp = (DictionaryEntry)enumerator.Current;
					visit(context, new CollectionDrawer.Element(num, delegate()
					{
						DrawerUtil.Tooltip(string.Format("{0} -> {1}", kvp.Key.GetType(), kvp.Value.GetType()));
					}, () => new
					{
						key = kvp.Key,
						value = kvp.Value
					}));
					num++;
				}
			}
		}
	}
}
