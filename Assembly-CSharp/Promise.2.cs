using System;
using System.Collections;

// Token: 0x0200062C RID: 1580
public class Promise<T> : IEnumerator
{
	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06001C21 RID: 7201 RVA: 0x000B6E69 File Offset: 0x000B5069
	public bool IsResolved
	{
		get
		{
			return this.promise.IsResolved;
		}
	}

	// Token: 0x06001C22 RID: 7202 RVA: 0x000B6E76 File Offset: 0x000B5076
	public Promise(Action<Action<T>> fn)
	{
		fn(delegate(T value)
		{
			this.Resolve(value);
		});
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x000B6E9B File Offset: 0x000B509B
	public Promise()
	{
	}

	// Token: 0x06001C24 RID: 7204 RVA: 0x000B6EAE File Offset: 0x000B50AE
	public void EnsureResolved(T value)
	{
		this.result = value;
		this.promise.EnsureResolved();
	}

	// Token: 0x06001C25 RID: 7205 RVA: 0x000B6EC2 File Offset: 0x000B50C2
	public void Resolve(T value)
	{
		this.result = value;
		this.promise.Resolve();
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x001B84F8 File Offset: 0x001B66F8
	public Promise<T> Then(Action<T> fn)
	{
		this.promise.Then(delegate
		{
			fn(this.result);
		});
		return this;
	}

	// Token: 0x06001C27 RID: 7207 RVA: 0x000B6ED6 File Offset: 0x000B50D6
	public Promise ThenWait(Func<Promise> fn)
	{
		return this.promise.ThenWait(fn);
	}

	// Token: 0x06001C28 RID: 7208 RVA: 0x000B6EE4 File Offset: 0x000B50E4
	public Promise<T> ThenWait(Func<Promise<T>> fn)
	{
		return this.promise.ThenWait<T>(fn);
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06001C29 RID: 7209 RVA: 0x000AA765 File Offset: 0x000A8965
	object IEnumerator.Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001C2A RID: 7210 RVA: 0x000B6EF2 File Offset: 0x000B50F2
	bool IEnumerator.MoveNext()
	{
		return !this.promise.IsResolved;
	}

	// Token: 0x06001C2B RID: 7211 RVA: 0x000AA038 File Offset: 0x000A8238
	void IEnumerator.Reset()
	{
	}

	// Token: 0x040011E5 RID: 4581
	private Promise promise = new Promise();

	// Token: 0x040011E6 RID: 4582
	private T result;
}
