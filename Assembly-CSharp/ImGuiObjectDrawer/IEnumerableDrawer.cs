using System;
using System.Collections;

namespace ImGuiObjectDrawer
{
	// Token: 0x0200216F RID: 8559
	public sealed class IEnumerableDrawer : CollectionDrawer
	{
		// Token: 0x0600B660 RID: 46688 RVA: 0x0011ADD3 File Offset: 0x00118FD3
		public override bool CanDraw(in MemberDrawContext context, in MemberDetails member)
		{
			return member.CanAssignToType<IEnumerable>();
		}

		// Token: 0x0600B661 RID: 46689 RVA: 0x0011ADDB File Offset: 0x00118FDB
		public override bool IsEmpty(in MemberDrawContext context, in MemberDetails member)
		{
			return !((IEnumerable)member.value).GetEnumerator().MoveNext();
		}

		// Token: 0x0600B662 RID: 46690 RVA: 0x00455FF4 File Offset: 0x004541F4
		protected override void VisitElements(CollectionDrawer.ElementVisitor visit, in MemberDrawContext context, in MemberDetails member)
		{
			IEnumerable enumerable = (IEnumerable)member.value;
			int num = 0;
			using (IEnumerator enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object el = enumerator.Current;
					visit(context, new CollectionDrawer.Element(num, delegate()
					{
						DrawerUtil.Tooltip(el.GetType());
					}, () => new
					{
						value = el
					}));
					num++;
				}
			}
		}
	}
}
