using System;

namespace Klei.AI
{
	// Token: 0x02003CF8 RID: 15608
	public class TraitGroup : ModifierGroup<Trait>
	{
		// Token: 0x0600EFA6 RID: 61350 RVA: 0x0014548C File Offset: 0x0014368C
		public TraitGroup(string id, string name, bool is_spawn_trait) : base(id, name)
		{
			this.IsSpawnTrait = is_spawn_trait;
		}

		// Token: 0x0400EB40 RID: 60224
		public bool IsSpawnTrait;
	}
}
