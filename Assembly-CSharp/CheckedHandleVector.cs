using System;
using System.Collections.Generic;

// Token: 0x020010A6 RID: 4262
public class CheckedHandleVector<T> where T : new()
{
	// Token: 0x0600568F RID: 22159 RVA: 0x002909BC File Offset: 0x0028EBBC
	public CheckedHandleVector(int initial_size)
	{
		this.handleVector = new HandleVector<T>(initial_size);
		this.isFree = new List<bool>(initial_size);
		for (int i = 0; i < initial_size; i++)
		{
			this.isFree.Add(true);
		}
	}

	// Token: 0x06005690 RID: 22160 RVA: 0x00290A0C File Offset: 0x0028EC0C
	public HandleVector<T>.Handle Add(T item, string debug_info)
	{
		HandleVector<T>.Handle result = this.handleVector.Add(item);
		if (result.index >= this.isFree.Count)
		{
			this.isFree.Add(false);
		}
		else
		{
			this.isFree[result.index] = false;
		}
		int i = this.handleVector.Items.Count;
		while (i > this.debugInfo.Count)
		{
			this.debugInfo.Add(null);
		}
		this.debugInfo[result.index] = debug_info;
		return result;
	}

	// Token: 0x06005691 RID: 22161 RVA: 0x00290A9C File Offset: 0x0028EC9C
	public T Release(HandleVector<T>.Handle handle)
	{
		if (this.isFree[handle.index])
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Tried to double free checked handle ",
				handle.index,
				"- Debug info:",
				this.debugInfo[handle.index]
			});
		}
		this.isFree[handle.index] = true;
		return this.handleVector.Release(handle);
	}

	// Token: 0x06005692 RID: 22162 RVA: 0x000DCDCF File Offset: 0x000DAFCF
	public T Get(HandleVector<T>.Handle handle)
	{
		return this.handleVector.GetItem(handle);
	}

	// Token: 0x04003D53 RID: 15699
	private HandleVector<T> handleVector;

	// Token: 0x04003D54 RID: 15700
	private List<string> debugInfo = new List<string>();

	// Token: 0x04003D55 RID: 15701
	private List<bool> isFree;
}
