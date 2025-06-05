using System;
using Klei.Actions;

namespace Klei.Input
{
	// Token: 0x02003D0B RID: 15627
	[Action("Immediate")]
	public class ImmediateDigAction : DigAction
	{
		// Token: 0x0600F006 RID: 61446 RVA: 0x00145889 File Offset: 0x00143A89
		public override void Dig(int cell, int distFromOrigin)
		{
			if (Grid.Solid[cell] && !Grid.Foundation[cell])
			{
				SimMessages.Dig(cell, -1, false);
			}
		}

		// Token: 0x0600F007 RID: 61447 RVA: 0x001458AD File Offset: 0x00143AAD
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
