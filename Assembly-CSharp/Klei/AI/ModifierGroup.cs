using System;
using System.Collections.Generic;

namespace Klei.AI
{
	// Token: 0x02003CDE RID: 15582
	public class ModifierGroup<T> : Resource
	{
		// Token: 0x0600EF23 RID: 61219 RVA: 0x00144EED File Offset: 0x001430ED
		public IEnumerator<T> GetEnumerator()
		{
			return this.modifiers.GetEnumerator();
		}

		// Token: 0x17000C64 RID: 3172
		public T this[int idx]
		{
			get
			{
				return this.modifiers[idx];
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x0600EF25 RID: 61221 RVA: 0x00144F0D File Offset: 0x0014310D
		public int Count
		{
			get
			{
				return this.modifiers.Count;
			}
		}

		// Token: 0x0600EF26 RID: 61222 RVA: 0x00144F1A File Offset: 0x0014311A
		public ModifierGroup(string id, string name) : base(id, name)
		{
		}

		// Token: 0x0600EF27 RID: 61223 RVA: 0x00144F2F File Offset: 0x0014312F
		public void Add(T modifier)
		{
			this.modifiers.Add(modifier);
		}

		// Token: 0x0400EAD0 RID: 60112
		public List<T> modifiers = new List<T>();
	}
}
