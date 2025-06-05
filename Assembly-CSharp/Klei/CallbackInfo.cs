using System;

namespace Klei
{
	// Token: 0x02003C41 RID: 15425
	public struct CallbackInfo
	{
		// Token: 0x0600EC57 RID: 60503 RVA: 0x00143226 File Offset: 0x00141426
		public CallbackInfo(HandleVector<Game.CallbackInfo>.Handle h)
		{
			this.handle = h;
		}

		// Token: 0x0600EC58 RID: 60504 RVA: 0x004DD8A8 File Offset: 0x004DBAA8
		public void Release()
		{
			if (this.handle.IsValid())
			{
				Game.CallbackInfo item = Game.Instance.callbackManager.GetItem(this.handle);
				System.Action cb = item.cb;
				if (!item.manuallyRelease)
				{
					Game.Instance.callbackManager.Release(this.handle);
				}
				cb();
			}
		}

		// Token: 0x0400E88B RID: 59531
		private HandleVector<Game.CallbackInfo>.Handle handle;
	}
}
