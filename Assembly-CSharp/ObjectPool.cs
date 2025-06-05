using System;
using System.Collections.Generic;

// Token: 0x02000619 RID: 1561
public class ObjectPool<T>
{
	// Token: 0x06001BBD RID: 7101 RVA: 0x001B7EA0 File Offset: 0x001B60A0
	public ObjectPool(Func<T> instantiator, int initial_count = 0)
	{
		this.instantiator = instantiator;
		this.unused = new Stack<T>(initial_count);
		for (int i = 0; i < initial_count; i++)
		{
			this.unused.Push(instantiator());
		}
	}

	// Token: 0x06001BBE RID: 7102 RVA: 0x001B7EE4 File Offset: 0x001B60E4
	public virtual T GetInstance()
	{
		T result = default(T);
		if (this.unused.Count > 0)
		{
			result = this.unused.Pop();
		}
		else
		{
			result = this.instantiator();
		}
		return result;
	}

	// Token: 0x06001BBF RID: 7103 RVA: 0x000B68C4 File Offset: 0x000B4AC4
	public void ReleaseInstance(T instance)
	{
		if (object.Equals(instance, null))
		{
			return;
		}
		this.unused.Push(instance);
	}

	// Token: 0x040011C6 RID: 4550
	protected Stack<T> unused;

	// Token: 0x040011C7 RID: 4551
	protected Func<T> instantiator;
}
