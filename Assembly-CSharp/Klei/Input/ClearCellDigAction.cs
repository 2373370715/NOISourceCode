using System;
using Klei.Actions;

namespace Klei.Input
{
	// Token: 0x02003D0C RID: 15628
	[Action("Clear Cell")]
	public class ClearCellDigAction : DigAction
	{
		// Token: 0x0600F009 RID: 61449 RVA: 0x001458B9 File Offset: 0x00143AB9
		public override void Dig(int cell, int distFromOrigin)
		{
			if (Grid.Solid[cell] && !Grid.Foundation[cell])
			{
				SimMessages.Dig(cell, -1, true);
			}
		}

		// Token: 0x0600F00A RID: 61450 RVA: 0x001458AD File Offset: 0x00143AAD
		protected override void EntityDig(IDigActionEntity digActionEntity)
		{
			if (digActionEntity == null)
			{
				return;
			}
			digActionEntity.Dig();
		}
	}
}
