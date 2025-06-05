using System;

namespace EventSystem2Syntax
{
	// Token: 0x0200214D RID: 8525
	internal class KMonoBehaviour2
	{
		// Token: 0x0600B5A0 RID: 46496 RVA: 0x000AA038 File Offset: 0x000A8238
		protected virtual void OnPrefabInit()
		{
		}

		// Token: 0x0600B5A1 RID: 46497 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Subscribe(int evt, Action<object> cb)
		{
		}

		// Token: 0x0600B5A2 RID: 46498 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Trigger(int evt, object data)
		{
		}

		// Token: 0x0600B5A3 RID: 46499 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Subscribe<ListenerType, EventType>(Action<ListenerType, EventType> cb) where EventType : IEventData
		{
		}

		// Token: 0x0600B5A4 RID: 46500 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Trigger<EventType>(EventType evt) where EventType : IEventData
		{
		}
	}
}
