using System;

namespace Database
{
	// Token: 0x020021D7 RID: 8663
	public class StateMachineCategories : ResourceSet<StateMachine.Category>
	{
		// Token: 0x0600B8B3 RID: 47283 RVA: 0x004721D4 File Offset: 0x004703D4
		public StateMachineCategories()
		{
			this.Ai = base.Add(new StateMachine.Category("Ai"));
			this.Monitor = base.Add(new StateMachine.Category("Monitor"));
			this.Chore = base.Add(new StateMachine.Category("Chore"));
			this.Misc = base.Add(new StateMachine.Category("Misc"));
		}

		// Token: 0x040096D1 RID: 38609
		public StateMachine.Category Ai;

		// Token: 0x040096D2 RID: 38610
		public StateMachine.Category Monitor;

		// Token: 0x040096D3 RID: 38611
		public StateMachine.Category Chore;

		// Token: 0x040096D4 RID: 38612
		public StateMachine.Category Misc;
	}
}
