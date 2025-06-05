using System;

namespace Klei.Actions
{
	// Token: 0x02003D0E RID: 15630
	[AttributeUsage(AttributeTargets.Class)]
	public class ActionAttribute : Attribute
	{
		// Token: 0x0600F011 RID: 61457 RVA: 0x00145917 File Offset: 0x00143B17
		public ActionAttribute(string actionName)
		{
			this.ActionName = actionName;
		}

		// Token: 0x0400EB89 RID: 60297
		public readonly string ActionName;
	}
}
