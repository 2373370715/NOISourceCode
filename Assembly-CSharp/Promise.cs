using System;
using System.Collections;

// Token: 0x02000625 RID: 1573
public class Promise : IEnumerator
{
	// Token: 0x170000AE RID: 174
	// (get) Token: 0x06001C03 RID: 7171 RVA: 0x000B6D10 File Offset: 0x000B4F10
	public bool IsResolved
	{
		get
		{
			return this.m_is_resolved;
		}
	}

	// Token: 0x06001C04 RID: 7172 RVA: 0x000B6D18 File Offset: 0x000B4F18
	public Promise(Action<System.Action> fn)
	{
		fn(delegate
		{
			this.Resolve();
		});
	}

	// Token: 0x06001C05 RID: 7173 RVA: 0x000AA024 File Offset: 0x000A8224
	public Promise()
	{
	}

	// Token: 0x06001C06 RID: 7174 RVA: 0x000B6D32 File Offset: 0x000B4F32
	public void EnsureResolved()
	{
		if (this.IsResolved)
		{
			return;
		}
		this.Resolve();
	}

	// Token: 0x06001C07 RID: 7175 RVA: 0x000B6D43 File Offset: 0x000B4F43
	public void Resolve()
	{
		DebugUtil.Assert(!this.m_is_resolved, "Can only resolve a promise once");
		this.m_is_resolved = true;
		if (this.on_complete != null)
		{
			this.on_complete();
			this.on_complete = null;
		}
	}

	// Token: 0x06001C08 RID: 7176 RVA: 0x000B6D79 File Offset: 0x000B4F79
	public Promise Then(System.Action callback)
	{
		if (this.m_is_resolved)
		{
			callback();
		}
		else
		{
			this.on_complete = (System.Action)Delegate.Combine(this.on_complete, callback);
		}
		return this;
	}

	// Token: 0x06001C09 RID: 7177 RVA: 0x001B82BC File Offset: 0x001B64BC
	public Promise ThenWait(Func<Promise> callback)
	{
		if (this.m_is_resolved)
		{
			return callback();
		}
		return new Promise(delegate(System.Action resolve)
		{
			this.on_complete = (System.Action)Delegate.Combine(this.on_complete, new System.Action(delegate()
			{
				callback().Then(resolve);
			}));
		});
	}

	// Token: 0x06001C0A RID: 7178 RVA: 0x001B8304 File Offset: 0x001B6504
	public Promise<T> ThenWait<T>(Func<Promise<T>> callback)
	{
		if (this.m_is_resolved)
		{
			return callback();
		}
		return new Promise<T>(delegate(Action<T> resolve)
		{
			this.on_complete = (System.Action)Delegate.Combine(this.on_complete, new System.Action(delegate()
			{
				callback().Then(resolve);
			}));
		});
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x06001C0B RID: 7179 RVA: 0x000AA765 File Offset: 0x000A8965
	object IEnumerator.Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001C0C RID: 7180 RVA: 0x000B6DA3 File Offset: 0x000B4FA3
	bool IEnumerator.MoveNext()
	{
		return !this.IsResolved;
	}

	// Token: 0x06001C0D RID: 7181 RVA: 0x000AA038 File Offset: 0x000A8238
	void IEnumerator.Reset()
	{
	}

	// Token: 0x06001C0E RID: 7182 RVA: 0x000B6DAE File Offset: 0x000B4FAE
	static Promise()
	{
		Promise.m_instant.Resolve();
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06001C0F RID: 7183 RVA: 0x000B6DC4 File Offset: 0x000B4FC4
	public static Promise Instant
	{
		get
		{
			return Promise.m_instant;
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06001C10 RID: 7184 RVA: 0x000B6DCB File Offset: 0x000B4FCB
	public static Promise Fail
	{
		get
		{
			return new Promise();
		}
	}

	// Token: 0x06001C11 RID: 7185 RVA: 0x001B834C File Offset: 0x001B654C
	public static Promise All(params Promise[] promises)
	{
		Promise.<>c__DisplayClass21_0 CS$<>8__locals1 = new Promise.<>c__DisplayClass21_0();
		CS$<>8__locals1.promises = promises;
		if (CS$<>8__locals1.promises == null || CS$<>8__locals1.promises.Length == 0)
		{
			return Promise.Instant;
		}
		CS$<>8__locals1.all_resolved_promise = new Promise();
		Promise[] promises2 = CS$<>8__locals1.promises;
		for (int i = 0; i < promises2.Length; i++)
		{
			promises2[i].Then(new System.Action(CS$<>8__locals1.<All>g__TryResolve|0));
		}
		return CS$<>8__locals1.all_resolved_promise;
	}

	// Token: 0x06001C12 RID: 7186 RVA: 0x000B6DD2 File Offset: 0x000B4FD2
	public static Promise Chain(params Func<Promise>[] make_promise_fns)
	{
		Promise.<>c__DisplayClass22_0 CS$<>8__locals1 = new Promise.<>c__DisplayClass22_0();
		CS$<>8__locals1.make_promise_fns = make_promise_fns;
		CS$<>8__locals1.all_resolve_promise = new Promise();
		CS$<>8__locals1.current_promise_fn_index = 0;
		CS$<>8__locals1.<Chain>g__TryNext|0();
		return CS$<>8__locals1.all_resolve_promise;
	}

	// Token: 0x040011D5 RID: 4565
	private System.Action on_complete;

	// Token: 0x040011D6 RID: 4566
	private bool m_is_resolved;

	// Token: 0x040011D7 RID: 4567
	private static Promise m_instant = new Promise();
}
