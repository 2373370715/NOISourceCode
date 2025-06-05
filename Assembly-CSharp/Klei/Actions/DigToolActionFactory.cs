using System;
using Klei.Input;

namespace Klei.Actions
{
	// Token: 0x02003D10 RID: 15632
	public class DigToolActionFactory : ActionFactory<DigToolActionFactory, DigAction, DigToolActionFactory.Actions>
	{
		// Token: 0x0600F017 RID: 61463 RVA: 0x00145976 File Offset: 0x00143B76
		protected override DigAction CreateAction(DigToolActionFactory.Actions action)
		{
			if (action == DigToolActionFactory.Actions.Immediate)
			{
				return new ImmediateDigAction();
			}
			if (action == DigToolActionFactory.Actions.ClearCell)
			{
				return new ClearCellDigAction();
			}
			if (action == DigToolActionFactory.Actions.MarkCell)
			{
				return new MarkCellDigAction();
			}
			throw new InvalidOperationException("Can not create DigAction 'Count'. Please provide a valid action.");
		}

		// Token: 0x02003D11 RID: 15633
		public enum Actions
		{
			// Token: 0x0400EB8D RID: 60301
			MarkCell = 145163119,
			// Token: 0x0400EB8E RID: 60302
			Immediate = -1044758767,
			// Token: 0x0400EB8F RID: 60303
			ClearCell = -1011242513,
			// Token: 0x0400EB90 RID: 60304
			Count = -1427607121
		}
	}
}
