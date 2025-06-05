using System;

namespace Klei.Actions
{
	// Token: 0x02003D0D RID: 15629
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class ActionTypeAttribute : Attribute
	{
		// Token: 0x0600F00C RID: 61452 RVA: 0x001458DD File Offset: 0x00143ADD
		public ActionTypeAttribute(string groupName, string typeName, bool generateConfig = true)
		{
			this.TypeName = typeName;
			this.GroupName = groupName;
			this.GenerateConfig = generateConfig;
		}

		// Token: 0x0600F00D RID: 61453 RVA: 0x004EBB00 File Offset: 0x004E9D00
		public static bool operator ==(ActionTypeAttribute lhs, ActionTypeAttribute rhs)
		{
			bool flag = object.Equals(lhs, null);
			bool flag2 = object.Equals(rhs, null);
			if (flag || flag2)
			{
				return flag == flag2;
			}
			return lhs.TypeName == rhs.TypeName && lhs.GroupName == rhs.GroupName;
		}

		// Token: 0x0600F00E RID: 61454 RVA: 0x001458FA File Offset: 0x00143AFA
		public static bool operator !=(ActionTypeAttribute lhs, ActionTypeAttribute rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600F00F RID: 61455 RVA: 0x00145906 File Offset: 0x00143B06
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x0600F010 RID: 61456 RVA: 0x0014590F File Offset: 0x00143B0F
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0400EB86 RID: 60294
		public readonly string TypeName;

		// Token: 0x0400EB87 RID: 60295
		public readonly string GroupName;

		// Token: 0x0400EB88 RID: 60296
		public readonly bool GenerateConfig;
	}
}
