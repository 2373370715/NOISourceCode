using System;

namespace EventSystem2Syntax
{
	// Token: 0x0200214B RID: 8523
	internal class NewExample : KMonoBehaviour2
	{
		// Token: 0x0600B59D RID: 46493 RVA: 0x00455494 File Offset: 0x00453694
		protected override void OnPrefabInit()
		{
			base.Subscribe<NewExample, NewExample.ObjectDestroyedEvent>(new Action<NewExample, NewExample.ObjectDestroyedEvent>(NewExample.OnObjectDestroyed));
			base.Trigger<NewExample.ObjectDestroyedEvent>(new NewExample.ObjectDestroyedEvent
			{
				parameter = false
			});
		}

		// Token: 0x0600B59E RID: 46494 RVA: 0x000AA038 File Offset: 0x000A8238
		private static void OnObjectDestroyed(NewExample example, NewExample.ObjectDestroyedEvent evt)
		{
		}

		// Token: 0x0200214C RID: 8524
		private struct ObjectDestroyedEvent : IEventData
		{
			// Token: 0x04008FDD RID: 36829
			public bool parameter;
		}
	}
}
