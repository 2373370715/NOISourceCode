using System;
using Rendering;
using UnityEngine;

// Token: 0x02001AD9 RID: 6873
public class BlockTileDecorInfo : ScriptableObject
{
	// Token: 0x06008FBA RID: 36794 RVA: 0x00385060 File Offset: 0x00383260
	public void PostProcess()
	{
		if (this.decor != null && this.atlas != null && this.atlas.items != null)
		{
			for (int i = 0; i < this.decor.Length; i++)
			{
				if (this.decor[i].variants != null && this.decor[i].variants.Length != 0)
				{
					for (int j = 0; j < this.decor[i].variants.Length; j++)
					{
						bool flag = false;
						foreach (TextureAtlas.Item item in this.atlas.items)
						{
							string text = item.name;
							int num = text.IndexOf("/");
							if (num != -1)
							{
								text = text.Substring(num + 1);
							}
							if (this.decor[i].variants[j].name == text)
							{
								this.decor[i].variants[j].atlasItem = item;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							DebugUtil.LogErrorArgs(new object[]
							{
								base.name,
								"/",
								this.decor[i].name,
								"could not find ",
								this.decor[i].variants[j].name,
								"in",
								this.atlas.name
							});
						}
					}
				}
			}
		}
	}

	// Token: 0x04006C49 RID: 27721
	public TextureAtlas atlas;

	// Token: 0x04006C4A RID: 27722
	public TextureAtlas atlasSpec;

	// Token: 0x04006C4B RID: 27723
	public int sortOrder;

	// Token: 0x04006C4C RID: 27724
	public BlockTileDecorInfo.Decor[] decor;

	// Token: 0x02001ADA RID: 6874
	[Serializable]
	public struct ImageInfo
	{
		// Token: 0x04006C4D RID: 27725
		public string name;

		// Token: 0x04006C4E RID: 27726
		public Vector3 offset;

		// Token: 0x04006C4F RID: 27727
		[NonSerialized]
		public TextureAtlas.Item atlasItem;
	}

	// Token: 0x02001ADB RID: 6875
	[Serializable]
	public struct Decor
	{
		// Token: 0x04006C50 RID: 27728
		public string name;

		// Token: 0x04006C51 RID: 27729
		[EnumFlags]
		public BlockTileRenderer.Bits requiredConnections;

		// Token: 0x04006C52 RID: 27730
		[EnumFlags]
		public BlockTileRenderer.Bits forbiddenConnections;

		// Token: 0x04006C53 RID: 27731
		public float probabilityCutoff;

		// Token: 0x04006C54 RID: 27732
		public BlockTileDecorInfo.ImageInfo[] variants;

		// Token: 0x04006C55 RID: 27733
		public int sortOrder;
	}
}
