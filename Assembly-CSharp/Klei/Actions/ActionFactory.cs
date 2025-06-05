using System;
using System.Collections.Generic;

namespace Klei.Actions
{
	// Token: 0x02003D0F RID: 15631
	public class ActionFactory<ActionFactoryType, ActionType, EnumType> where ActionFactoryType : ActionFactory<ActionFactoryType, ActionType, EnumType>
	{
		// Token: 0x0600F012 RID: 61458 RVA: 0x004EBB50 File Offset: 0x004E9D50
		public static ActionType GetOrCreateAction(EnumType actionType)
		{
			ActionType result;
			if (!ActionFactory<ActionFactoryType, ActionType, EnumType>.actionInstances.TryGetValue(actionType, out result))
			{
				ActionFactory<ActionFactoryType, ActionType, EnumType>.EnsureFactoryInstance();
				result = (ActionFactory<ActionFactoryType, ActionType, EnumType>.actionInstances[actionType] = ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory.CreateAction(actionType));
			}
			return result;
		}

		// Token: 0x0600F013 RID: 61459 RVA: 0x00145926 File Offset: 0x00143B26
		private static void EnsureFactoryInstance()
		{
			if (ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory != null)
			{
				return;
			}
			ActionFactory<ActionFactoryType, ActionType, EnumType>.actionFactory = (Activator.CreateInstance(typeof(ActionFactoryType)) as ActionFactoryType);
		}

		// Token: 0x0600F014 RID: 61460 RVA: 0x00145953 File Offset: 0x00143B53
		protected virtual ActionType CreateAction(EnumType actionType)
		{
			throw new InvalidOperationException("Can not call InterfaceToolActionFactory<T1, T2>.CreateAction()! This function must be called from a deriving class!");
		}

		// Token: 0x0400EB8A RID: 60298
		private static Dictionary<EnumType, ActionType> actionInstances = new Dictionary<EnumType, ActionType>();

		// Token: 0x0400EB8B RID: 60299
		private static ActionFactoryType actionFactory = default(ActionFactoryType);
	}
}
