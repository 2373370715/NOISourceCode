using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C0C RID: 3084
public class StatusItemStackTraceWatcher : IDisposable
{
	// Token: 0x06003A7B RID: 14971 RVA: 0x000CA376 File Offset: 0x000C8576
	public bool GetShouldWatch()
	{
		return this.shouldWatch;
	}

	// Token: 0x06003A7C RID: 14972 RVA: 0x000CA37E File Offset: 0x000C857E
	public void SetShouldWatch(bool shouldWatch)
	{
		if (this.shouldWatch == shouldWatch)
		{
			return;
		}
		this.shouldWatch = shouldWatch;
		this.Refresh();
	}

	// Token: 0x06003A7D RID: 14973 RVA: 0x000CA397 File Offset: 0x000C8597
	public Option<StatusItemGroup> GetTarget()
	{
		return this.currentTarget;
	}

	// Token: 0x06003A7E RID: 14974 RVA: 0x002353A4 File Offset: 0x002335A4
	public void SetTarget(Option<StatusItemGroup> nextTarget)
	{
		if (this.currentTarget.IsNone() && nextTarget.IsNone())
		{
			return;
		}
		if (this.currentTarget.IsSome() && nextTarget.IsSome() && this.currentTarget.Unwrap() == nextTarget.Unwrap())
		{
			return;
		}
		this.currentTarget = nextTarget;
		this.Refresh();
	}

	// Token: 0x06003A7F RID: 14975 RVA: 0x00235400 File Offset: 0x00233600
	private void Refresh()
	{
		if (this.onCleanup != null)
		{
			System.Action action = this.onCleanup;
			if (action != null)
			{
				action();
			}
			this.onCleanup = null;
		}
		if (!this.shouldWatch)
		{
			return;
		}
		if (this.currentTarget.IsSome())
		{
			StatusItemGroup target = this.currentTarget.Unwrap();
			Action<StatusItemGroup.Entry, StatusItemCategory> onAddStatusItem = delegate(StatusItemGroup.Entry entry, StatusItemCategory category)
			{
				this.entryIdToStackTraceMap[entry.id] = new StackTrace(true);
			};
			StatusItemGroup target3 = target;
			target3.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Combine(target3.OnAddStatusItem, onAddStatusItem);
			this.onCleanup = (System.Action)Delegate.Combine(this.onCleanup, new System.Action(delegate()
			{
				StatusItemGroup target2 = target;
				target2.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Remove(target2.OnAddStatusItem, onAddStatusItem);
			}));
			StatusItemStackTraceWatcher.StatusItemStackTraceWatcher_OnDestroyListenerMB destroyListener = this.currentTarget.Unwrap().gameObject.AddOrGet<StatusItemStackTraceWatcher.StatusItemStackTraceWatcher_OnDestroyListenerMB>();
			destroyListener.owner = this;
			this.onCleanup = (System.Action)Delegate.Combine(this.onCleanup, new System.Action(delegate()
			{
				if (destroyListener.IsNullOrDestroyed())
				{
					return;
				}
				UnityEngine.Object.Destroy(destroyListener);
			}));
			this.onCleanup = (System.Action)Delegate.Combine(this.onCleanup, new System.Action(delegate()
			{
				this.entryIdToStackTraceMap.Clear();
			}));
		}
	}

	// Token: 0x06003A80 RID: 14976 RVA: 0x000CA39F File Offset: 0x000C859F
	public bool GetStackTraceForEntry(StatusItemGroup.Entry entry, out StackTrace stackTrace)
	{
		return this.entryIdToStackTraceMap.TryGetValue(entry.id, out stackTrace);
	}

	// Token: 0x06003A81 RID: 14977 RVA: 0x000CA3B3 File Offset: 0x000C85B3
	public void Dispose()
	{
		if (this.onCleanup != null)
		{
			System.Action action = this.onCleanup;
			if (action != null)
			{
				action();
			}
			this.onCleanup = null;
		}
	}

	// Token: 0x04002882 RID: 10370
	private Dictionary<Guid, StackTrace> entryIdToStackTraceMap = new Dictionary<Guid, StackTrace>();

	// Token: 0x04002883 RID: 10371
	private Option<StatusItemGroup> currentTarget;

	// Token: 0x04002884 RID: 10372
	private bool shouldWatch;

	// Token: 0x04002885 RID: 10373
	private System.Action onCleanup;

	// Token: 0x02000C0D RID: 3085
	public class StatusItemStackTraceWatcher_OnDestroyListenerMB : MonoBehaviour
	{
		// Token: 0x06003A85 RID: 14981 RVA: 0x00235520 File Offset: 0x00233720
		private void OnDestroy()
		{
			bool flag = this.owner != null;
			bool flag2 = this.owner.currentTarget.IsSome() && this.owner.currentTarget.Unwrap().gameObject == base.gameObject;
			if (flag && flag2)
			{
				this.owner.SetTarget(Option.None);
			}
		}

		// Token: 0x04002886 RID: 10374
		public StatusItemStackTraceWatcher owner;
	}
}
