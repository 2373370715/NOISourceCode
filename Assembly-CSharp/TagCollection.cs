using System;
using System.Collections.Generic;

// Token: 0x02001A2D RID: 6701
public class TagCollection : IReadonlyTags
{
	// Token: 0x06008B96 RID: 35734 RVA: 0x000FFE28 File Offset: 0x000FE028
	public TagCollection()
	{
	}

	// Token: 0x06008B97 RID: 35735 RVA: 0x0036E050 File Offset: 0x0036C250
	public TagCollection(int[] initialTags)
	{
		for (int i = 0; i < initialTags.Length; i++)
		{
			this.tags.Add(initialTags[i]);
		}
	}

	// Token: 0x06008B98 RID: 35736 RVA: 0x0036E08C File Offset: 0x0036C28C
	public TagCollection(string[] initialTags)
	{
		for (int i = 0; i < initialTags.Length; i++)
		{
			this.tags.Add(Hash.SDBMLower(initialTags[i]));
		}
	}

	// Token: 0x06008B99 RID: 35737 RVA: 0x000FFE3B File Offset: 0x000FE03B
	public TagCollection(TagCollection initialTags)
	{
		if (initialTags != null && initialTags.tags != null)
		{
			this.tags.UnionWith(initialTags.tags);
		}
	}

	// Token: 0x06008B9A RID: 35738 RVA: 0x0036E0CC File Offset: 0x0036C2CC
	public TagCollection Append(TagCollection others)
	{
		foreach (int item in others.tags)
		{
			this.tags.Add(item);
		}
		return this;
	}

	// Token: 0x06008B9B RID: 35739 RVA: 0x000FFE6A File Offset: 0x000FE06A
	public void AddTag(string tag)
	{
		this.tags.Add(Hash.SDBMLower(tag));
	}

	// Token: 0x06008B9C RID: 35740 RVA: 0x000FFE7E File Offset: 0x000FE07E
	public void AddTag(int tag)
	{
		this.tags.Add(tag);
	}

	// Token: 0x06008B9D RID: 35741 RVA: 0x000FFE8D File Offset: 0x000FE08D
	public void RemoveTag(string tag)
	{
		this.tags.Remove(Hash.SDBMLower(tag));
	}

	// Token: 0x06008B9E RID: 35742 RVA: 0x000FFEA1 File Offset: 0x000FE0A1
	public void RemoveTag(int tag)
	{
		this.tags.Remove(tag);
	}

	// Token: 0x06008B9F RID: 35743 RVA: 0x000FFEB0 File Offset: 0x000FE0B0
	public bool HasTag(string tag)
	{
		return this.tags.Contains(Hash.SDBMLower(tag));
	}

	// Token: 0x06008BA0 RID: 35744 RVA: 0x000FFEC3 File Offset: 0x000FE0C3
	public bool HasTag(int tag)
	{
		return this.tags.Contains(tag);
	}

	// Token: 0x06008BA1 RID: 35745 RVA: 0x0036E128 File Offset: 0x0036C328
	public bool HasTags(int[] searchTags)
	{
		for (int i = 0; i < searchTags.Length; i++)
		{
			if (!this.tags.Contains(searchTags[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04006955 RID: 26965
	private HashSet<int> tags = new HashSet<int>();
}
