using System;

// Token: 0x02000827 RID: 2087
public class SafetyChecker
{
	// Token: 0x17000114 RID: 276
	// (get) Token: 0x060024CC RID: 9420 RVA: 0x000BC6C9 File Offset: 0x000BA8C9
	// (set) Token: 0x060024CD RID: 9421 RVA: 0x000BC6D1 File Offset: 0x000BA8D1
	public SafetyChecker.Condition[] conditions { get; private set; }

	// Token: 0x060024CE RID: 9422 RVA: 0x000BC6DA File Offset: 0x000BA8DA
	public SafetyChecker(SafetyChecker.Condition[] conditions)
	{
		this.conditions = conditions;
	}

	// Token: 0x060024CF RID: 9423 RVA: 0x001D7698 File Offset: 0x001D5898
	public int GetSafetyConditions(int cell, int cost, SafetyChecker.Context context, out bool all_conditions_met)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.conditions.Length; i++)
		{
			SafetyChecker.Condition condition = this.conditions[i];
			if (condition.callback(cell, cost, context))
			{
				num |= condition.mask;
				num2++;
			}
		}
		all_conditions_met = (num2 == this.conditions.Length);
		return num;
	}

	// Token: 0x02000828 RID: 2088
	public struct Condition
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060024D0 RID: 9424 RVA: 0x000BC6E9 File Offset: 0x000BA8E9
		// (set) Token: 0x060024D1 RID: 9425 RVA: 0x000BC6F1 File Offset: 0x000BA8F1
		public SafetyChecker.Condition.Callback callback { readonly get; private set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060024D2 RID: 9426 RVA: 0x000BC6FA File Offset: 0x000BA8FA
		// (set) Token: 0x060024D3 RID: 9427 RVA: 0x000BC702 File Offset: 0x000BA902
		public int mask { readonly get; private set; }

		// Token: 0x060024D4 RID: 9428 RVA: 0x000BC70B File Offset: 0x000BA90B
		public Condition(string id, int condition_mask, SafetyChecker.Condition.Callback condition_callback)
		{
			this = default(SafetyChecker.Condition);
			this.callback = condition_callback;
			this.mask = condition_mask;
		}

		// Token: 0x02000829 RID: 2089
		// (Invoke) Token: 0x060024D6 RID: 9430
		public delegate bool Callback(int cell, int cost, SafetyChecker.Context context);
	}

	// Token: 0x0200082A RID: 2090
	public struct Context
	{
		// Token: 0x060024D9 RID: 9433 RVA: 0x001D76F8 File Offset: 0x001D58F8
		public Context(KMonoBehaviour cmp)
		{
			this.cell = Grid.PosToCell(cmp);
			this.navigator = cmp.GetComponent<Navigator>();
			this.oxygenBreather = cmp.GetComponent<OxygenBreather>();
			this.minionBrain = cmp.GetComponent<MinionBrain>();
			this.temperatureTransferer = cmp.GetComponent<SimTemperatureTransfer>();
			this.primaryElement = cmp.GetComponent<PrimaryElement>();
			this.worldID = this.navigator.GetMyWorldId();
		}

		// Token: 0x04001944 RID: 6468
		public Navigator navigator;

		// Token: 0x04001945 RID: 6469
		public OxygenBreather oxygenBreather;

		// Token: 0x04001946 RID: 6470
		public SimTemperatureTransfer temperatureTransferer;

		// Token: 0x04001947 RID: 6471
		public PrimaryElement primaryElement;

		// Token: 0x04001948 RID: 6472
		public MinionBrain minionBrain;

		// Token: 0x04001949 RID: 6473
		public int worldID;

		// Token: 0x0400194A RID: 6474
		public int cell;
	}
}
