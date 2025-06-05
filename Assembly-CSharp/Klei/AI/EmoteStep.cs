using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CE4 RID: 15588
	public class EmoteStep
	{
		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x0600EF54 RID: 61268 RVA: 0x001451BC File Offset: 0x001433BC
		public int Id
		{
			get
			{
				return this.anim.HashValue;
			}
		}

		// Token: 0x0600EF55 RID: 61269 RVA: 0x004E95FC File Offset: 0x004E77FC
		public HandleVector<EmoteStep.Callbacks>.Handle RegisterCallbacks(Action<GameObject> startedCb, Action<GameObject> finishedCb)
		{
			if (startedCb == null && finishedCb == null)
			{
				return HandleVector<EmoteStep.Callbacks>.InvalidHandle;
			}
			EmoteStep.Callbacks item = new EmoteStep.Callbacks
			{
				StartedCb = startedCb,
				FinishedCb = finishedCb
			};
			return this.callbacks.Add(item);
		}

		// Token: 0x0600EF56 RID: 61270 RVA: 0x001451C9 File Offset: 0x001433C9
		public void UnregisterCallbacks(HandleVector<EmoteStep.Callbacks>.Handle callbackHandle)
		{
			this.callbacks.Release(callbackHandle);
		}

		// Token: 0x0600EF57 RID: 61271 RVA: 0x001451D8 File Offset: 0x001433D8
		public void UnregisterAllCallbacks()
		{
			this.callbacks = new HandleVector<EmoteStep.Callbacks>(64);
		}

		// Token: 0x0600EF58 RID: 61272 RVA: 0x004E963C File Offset: 0x004E783C
		public void OnStepStarted(HandleVector<EmoteStep.Callbacks>.Handle callbackHandle, GameObject parameter)
		{
			if (callbackHandle == HandleVector<EmoteStep.Callbacks>.Handle.InvalidHandle)
			{
				return;
			}
			EmoteStep.Callbacks item = this.callbacks.GetItem(callbackHandle);
			if (item.StartedCb != null)
			{
				item.StartedCb(parameter);
			}
		}

		// Token: 0x0600EF59 RID: 61273 RVA: 0x004E9678 File Offset: 0x004E7878
		public void OnStepFinished(HandleVector<EmoteStep.Callbacks>.Handle callbackHandle, GameObject parameter)
		{
			if (callbackHandle == HandleVector<EmoteStep.Callbacks>.Handle.InvalidHandle)
			{
				return;
			}
			EmoteStep.Callbacks item = this.callbacks.GetItem(callbackHandle);
			if (item.FinishedCb != null)
			{
				item.FinishedCb(parameter);
			}
		}

		// Token: 0x0400EADD RID: 60125
		public HashedString anim = HashedString.Invalid;

		// Token: 0x0400EADE RID: 60126
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;

		// Token: 0x0400EADF RID: 60127
		public float timeout = -1f;

		// Token: 0x0400EAE0 RID: 60128
		private HandleVector<EmoteStep.Callbacks> callbacks = new HandleVector<EmoteStep.Callbacks>(64);

		// Token: 0x02003CE5 RID: 15589
		public struct Callbacks
		{
			// Token: 0x0400EAE1 RID: 60129
			public Action<GameObject> StartedCb;

			// Token: 0x0400EAE2 RID: 60130
			public Action<GameObject> FinishedCb;
		}
	}
}
