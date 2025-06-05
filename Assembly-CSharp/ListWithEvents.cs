using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000618 RID: 1560
public class ListWithEvents<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x06001BAA RID: 7082 RVA: 0x000B67BF File Offset: 0x000B49BF
	public int Count
	{
		get
		{
			return this.internalList.Count;
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x06001BAB RID: 7083 RVA: 0x000B67CC File Offset: 0x000B49CC
	public bool IsReadOnly
	{
		get
		{
			return ((ICollection<T>)this.internalList).IsReadOnly;
		}
	}

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06001BAC RID: 7084 RVA: 0x001B7D84 File Offset: 0x001B5F84
	// (remove) Token: 0x06001BAD RID: 7085 RVA: 0x001B7DBC File Offset: 0x001B5FBC
	public event Action<T> onAdd;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06001BAE RID: 7086 RVA: 0x001B7DF4 File Offset: 0x001B5FF4
	// (remove) Token: 0x06001BAF RID: 7087 RVA: 0x001B7E2C File Offset: 0x001B602C
	public event Action<T> onRemove;

	// Token: 0x170000A6 RID: 166
	public T this[int index]
	{
		get
		{
			return this.internalList[index];
		}
		set
		{
			this.internalList[index] = value;
		}
	}

	// Token: 0x06001BB2 RID: 7090 RVA: 0x000B67F6 File Offset: 0x000B49F6
	public void Add(T item)
	{
		this.internalList.Add(item);
		if (this.onAdd != null)
		{
			this.onAdd(item);
		}
	}

	// Token: 0x06001BB3 RID: 7091 RVA: 0x000B6818 File Offset: 0x000B4A18
	public void Insert(int index, T item)
	{
		this.internalList.Insert(index, item);
		if (this.onAdd != null)
		{
			this.onAdd(item);
		}
	}

	// Token: 0x06001BB4 RID: 7092 RVA: 0x001B7E64 File Offset: 0x001B6064
	public void RemoveAt(int index)
	{
		T obj = this.internalList[index];
		this.internalList.RemoveAt(index);
		if (this.onRemove != null)
		{
			this.onRemove(obj);
		}
	}

	// Token: 0x06001BB5 RID: 7093 RVA: 0x000B683B File Offset: 0x000B4A3B
	public bool Remove(T item)
	{
		bool flag = this.internalList.Remove(item);
		if (flag && this.onRemove != null)
		{
			this.onRemove(item);
		}
		return flag;
	}

	// Token: 0x06001BB6 RID: 7094 RVA: 0x000B6860 File Offset: 0x000B4A60
	public void Clear()
	{
		while (this.Count > 0)
		{
			this.RemoveAt(0);
		}
	}

	// Token: 0x06001BB7 RID: 7095 RVA: 0x000B6874 File Offset: 0x000B4A74
	public int IndexOf(T item)
	{
		return this.internalList.IndexOf(item);
	}

	// Token: 0x06001BB8 RID: 7096 RVA: 0x000B6882 File Offset: 0x000B4A82
	public void CopyTo(T[] array, int arrayIndex)
	{
		this.internalList.CopyTo(array, arrayIndex);
	}

	// Token: 0x06001BB9 RID: 7097 RVA: 0x000B6891 File Offset: 0x000B4A91
	public bool Contains(T item)
	{
		return this.internalList.Contains(item);
	}

	// Token: 0x06001BBA RID: 7098 RVA: 0x000B689F File Offset: 0x000B4A9F
	public IEnumerator<T> GetEnumerator()
	{
		return this.internalList.GetEnumerator();
	}

	// Token: 0x06001BBB RID: 7099 RVA: 0x000B689F File Offset: 0x000B4A9F
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.internalList.GetEnumerator();
	}

	// Token: 0x040011C3 RID: 4547
	private List<T> internalList = new List<T>();
}
