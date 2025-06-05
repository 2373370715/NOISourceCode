using System;
using System.Collections.Generic;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	// Token: 0x02003D07 RID: 15623
	[CreateAssetMenu(fileName = "InterfaceToolConfig", menuName = "Klei/Interface Tools/Config")]
	public class InterfaceToolConfig : ScriptableObject
	{
		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x0600EFF8 RID: 61432 RVA: 0x00145814 File Offset: 0x00143A14
		public DigAction DigAction
		{
			get
			{
				return ActionFactory<DigToolActionFactory, DigAction, DigToolActionFactory.Actions>.GetOrCreateAction(this.digAction);
			}
		}

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x0600EFF9 RID: 61433 RVA: 0x00145821 File Offset: 0x00143A21
		public int Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x0600EFFA RID: 61434 RVA: 0x00145829 File Offset: 0x00143A29
		public global::Action InputAction
		{
			get
			{
				return (global::Action)Enum.Parse(typeof(global::Action), this.inputAction);
			}
		}

		// Token: 0x0400EB82 RID: 60290
		[SerializeField]
		private DigToolActionFactory.Actions digAction;

		// Token: 0x0400EB83 RID: 60291
		public static InterfaceToolConfig.Comparer ConfigComparer = new InterfaceToolConfig.Comparer();

		// Token: 0x0400EB84 RID: 60292
		[SerializeField]
		[Tooltip("Defines which config will take priority should multiple configs be activated\n0 is the lower bound for this value.")]
		private int priority;

		// Token: 0x0400EB85 RID: 60293
		[SerializeField]
		[Tooltip("This will serve as a key for activating different configs. Currently, these Actionsare how we indicate that different input modes are desired.\nAssigning Action.Invalid to this field will indicate that this is the \"default\" config")]
		private string inputAction = global::Action.Invalid.ToString();

		// Token: 0x02003D08 RID: 15624
		public class Comparer : IComparer<InterfaceToolConfig>
		{
			// Token: 0x0600EFFD RID: 61437 RVA: 0x00145851 File Offset: 0x00143A51
			public int Compare(InterfaceToolConfig lhs, InterfaceToolConfig rhs)
			{
				if (lhs.Priority == rhs.Priority)
				{
					return 0;
				}
				if (lhs.Priority <= rhs.Priority)
				{
					return -1;
				}
				return 1;
			}
		}
	}
}
