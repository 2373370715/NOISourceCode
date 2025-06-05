using System;
using System.Collections.Generic;

namespace Klei.AI
{
	// Token: 0x02003CDD RID: 15581
	public class Modifier : Resource
	{
		// Token: 0x0600EF1F RID: 61215 RVA: 0x00144EB1 File Offset: 0x001430B1
		public Modifier(string id, string name, string description) : base(id, name)
		{
			this.description = description;
		}

		// Token: 0x0600EF20 RID: 61216 RVA: 0x00144ECD File Offset: 0x001430CD
		public void Add(AttributeModifier modifier)
		{
			if (modifier.AttributeId != "")
			{
				this.SelfModifiers.Add(modifier);
			}
		}

		// Token: 0x0600EF21 RID: 61217 RVA: 0x004E8F50 File Offset: 0x004E7150
		public virtual void AddTo(Attributes attributes)
		{
			foreach (AttributeModifier modifier in this.SelfModifiers)
			{
				attributes.Add(modifier);
			}
		}

		// Token: 0x0600EF22 RID: 61218 RVA: 0x004E8FA4 File Offset: 0x004E71A4
		public virtual void RemoveFrom(Attributes attributes)
		{
			foreach (AttributeModifier modifier in this.SelfModifiers)
			{
				attributes.Remove(modifier);
			}
		}

		// Token: 0x0400EACE RID: 60110
		public string description;

		// Token: 0x0400EACF RID: 60111
		public List<AttributeModifier> SelfModifiers = new List<AttributeModifier>();
	}
}
