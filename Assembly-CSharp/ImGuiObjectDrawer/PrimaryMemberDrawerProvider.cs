using System;
using System.Collections.Generic;

namespace ImGuiObjectDrawer
{
	// Token: 0x0200215A RID: 8538
	public class PrimaryMemberDrawerProvider : IMemberDrawerProvider
	{
		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x0600B61B RID: 46619 RVA: 0x000D3AD3 File Offset: 0x000D1CD3
		public int Priority
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x0600B61C RID: 46620 RVA: 0x00455BF0 File Offset: 0x00453DF0
		public void AppendDrawersTo(List<MemberDrawer> drawers)
		{
			drawers.AddRange(new MemberDrawer[]
			{
				new NullDrawer(),
				new SimpleDrawer(),
				new LocStringDrawer(),
				new EnumDrawer(),
				new HashedStringDrawer(),
				new KAnimHashedStringDrawer(),
				new Vector2Drawer(),
				new Vector3Drawer(),
				new Vector4Drawer(),
				new UnityObjectDrawer(),
				new ArrayDrawer(),
				new IDictionaryDrawer(),
				new IEnumerableDrawer(),
				new PlainCSharpObjectDrawer(),
				new FallbackDrawer()
			});
		}
	}
}
