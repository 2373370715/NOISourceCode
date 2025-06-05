using System;
using UnityEngine;

// Token: 0x02001800 RID: 6144
public class TechTreeTitle : Resource
{
	// Token: 0x17000801 RID: 2049
	// (get) Token: 0x06007E6B RID: 32363 RVA: 0x000F7B4F File Offset: 0x000F5D4F
	public Vector2 center
	{
		get
		{
			return this.node.center;
		}
	}

	// Token: 0x17000802 RID: 2050
	// (get) Token: 0x06007E6C RID: 32364 RVA: 0x000F7B5C File Offset: 0x000F5D5C
	public float width
	{
		get
		{
			return this.node.width;
		}
	}

	// Token: 0x17000803 RID: 2051
	// (get) Token: 0x06007E6D RID: 32365 RVA: 0x000F7B69 File Offset: 0x000F5D69
	public float height
	{
		get
		{
			return this.node.height;
		}
	}

	// Token: 0x06007E6E RID: 32366 RVA: 0x000F7B76 File Offset: 0x000F5D76
	public TechTreeTitle(string id, ResourceSet parent, string name, ResourceTreeNode node) : base(id, parent, name)
	{
		this.node = node;
	}

	// Token: 0x04006015 RID: 24597
	public string desc;

	// Token: 0x04006016 RID: 24598
	private ResourceTreeNode node;
}
