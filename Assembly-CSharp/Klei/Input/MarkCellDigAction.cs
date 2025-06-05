using System;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	// Token: 0x02003D0A RID: 15626
	[Action("Mark Cell")]
	public class MarkCellDigAction : DigAction
	{
		// Token: 0x0600F003 RID: 61443 RVA: 0x004EBABC File Offset: 0x004E9CBC
		public override void Dig(int cell, int distFromOrigin)
		{
			GameObject gameObject = DigTool.PlaceDig(cell, distFromOrigin);
			if (gameObject != null)
			{
				Prioritizable component = gameObject.GetComponent<Prioritizable>();
				if (component != null)
				{
					component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
				}
			}
		}

		// Token: 0x0600F004 RID: 61444 RVA: 0x00145874 File Offset: 0x00143A74
		protected override void EntityDig(IDigActionEntity digActionEntity)
		{
			if (digActionEntity == null)
			{
				return;
			}
			digActionEntity.MarkForDig(true);
		}
	}
}
