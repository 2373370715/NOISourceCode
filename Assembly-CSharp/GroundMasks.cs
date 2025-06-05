using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001AC9 RID: 6857
public class GroundMasks : ScriptableObject
{
	// Token: 0x06008F6A RID: 36714 RVA: 0x00381D58 File Offset: 0x0037FF58
	public void Initialize()
	{
		if (this.maskAtlas == null || this.maskAtlas.items == null)
		{
			return;
		}
		this.biomeMasks = new Dictionary<string, GroundMasks.BiomeMaskData>();
		foreach (TextureAtlas.Item item in this.maskAtlas.items)
		{
			string name = item.name;
			int num = name.IndexOf('/');
			string text = name.Substring(0, num);
			string value = name.Substring(num + 1, 4);
			text = text.ToLower();
			for (int num2 = text.IndexOf('_'); num2 != -1; num2 = text.IndexOf('_'))
			{
				text = text.Remove(num2, 1);
			}
			GroundMasks.BiomeMaskData biomeMaskData = null;
			if (!this.biomeMasks.TryGetValue(text, out biomeMaskData))
			{
				biomeMaskData = new GroundMasks.BiomeMaskData(text);
				this.biomeMasks[text] = biomeMaskData;
			}
			int num3 = Convert.ToInt32(value, 2);
			GroundMasks.Tile tile = biomeMaskData.tiles[num3];
			if (tile.variationUVs == null)
			{
				tile.isSource = true;
				tile.variationUVs = new GroundMasks.UVData[1];
			}
			else
			{
				GroundMasks.UVData[] array = new GroundMasks.UVData[tile.variationUVs.Length + 1];
				Array.Copy(tile.variationUVs, array, tile.variationUVs.Length);
				tile.variationUVs = array;
			}
			Vector4 vector = new Vector4(item.uvBox.x, item.uvBox.w, item.uvBox.z, item.uvBox.y);
			Vector2 bl = new Vector2(vector.x, vector.y);
			Vector2 br = new Vector2(vector.z, vector.y);
			Vector2 tl = new Vector2(vector.x, vector.w);
			Vector2 tr = new Vector2(vector.z, vector.w);
			GroundMasks.UVData uvdata = new GroundMasks.UVData(bl, br, tl, tr);
			tile.variationUVs[tile.variationUVs.Length - 1] = uvdata;
			biomeMaskData.tiles[num3] = tile;
		}
		foreach (KeyValuePair<string, GroundMasks.BiomeMaskData> keyValuePair in this.biomeMasks)
		{
			keyValuePair.Value.GenerateRotations();
			keyValuePair.Value.Validate();
		}
	}

	// Token: 0x06008F6B RID: 36715 RVA: 0x00381FBC File Offset: 0x003801BC
	[ContextMenu("Print Variations")]
	private void Regenerate()
	{
		this.Initialize();
		string text = "Listing all variations:\n";
		foreach (KeyValuePair<string, GroundMasks.BiomeMaskData> keyValuePair in this.biomeMasks)
		{
			GroundMasks.BiomeMaskData value = keyValuePair.Value;
			text = text + "Biome: " + value.name + "\n";
			for (int i = 1; i < value.tiles.Length; i++)
			{
				GroundMasks.Tile tile = value.tiles[i];
				text += string.Format("  tile {0}: {1} variations\n", Convert.ToString(i, 2).PadLeft(4, '0'), tile.variationUVs.Length);
			}
		}
		global::Debug.Log(text);
	}

	// Token: 0x04006C0B RID: 27659
	public TextureAtlas maskAtlas;

	// Token: 0x04006C0C RID: 27660
	[NonSerialized]
	public Dictionary<string, GroundMasks.BiomeMaskData> biomeMasks;

	// Token: 0x02001ACA RID: 6858
	public struct UVData
	{
		// Token: 0x06008F6D RID: 36717 RVA: 0x001021A3 File Offset: 0x001003A3
		public UVData(Vector2 bl, Vector2 br, Vector2 tl, Vector2 tr)
		{
			this.bl = bl;
			this.br = br;
			this.tl = tl;
			this.tr = tr;
		}

		// Token: 0x04006C0D RID: 27661
		public Vector2 bl;

		// Token: 0x04006C0E RID: 27662
		public Vector2 br;

		// Token: 0x04006C0F RID: 27663
		public Vector2 tl;

		// Token: 0x04006C10 RID: 27664
		public Vector2 tr;
	}

	// Token: 0x02001ACB RID: 6859
	public struct Tile
	{
		// Token: 0x04006C11 RID: 27665
		public bool isSource;

		// Token: 0x04006C12 RID: 27666
		public GroundMasks.UVData[] variationUVs;
	}

	// Token: 0x02001ACC RID: 6860
	public class BiomeMaskData
	{
		// Token: 0x06008F6E RID: 36718 RVA: 0x001021C2 File Offset: 0x001003C2
		public BiomeMaskData(string name)
		{
			this.name = name;
			this.tiles = new GroundMasks.Tile[16];
		}

		// Token: 0x06008F6F RID: 36719 RVA: 0x00382094 File Offset: 0x00380294
		public void GenerateRotations()
		{
			for (int i = 1; i < 15; i++)
			{
				if (!this.tiles[i].isSource)
				{
					GroundMasks.Tile tile = this.tiles[i];
					tile.variationUVs = this.GetNonNullRotationUVs(i);
					this.tiles[i] = tile;
				}
			}
		}

		// Token: 0x06008F70 RID: 36720 RVA: 0x003820EC File Offset: 0x003802EC
		public GroundMasks.UVData[] GetNonNullRotationUVs(int dest_mask)
		{
			GroundMasks.UVData[] array = null;
			int num = dest_mask;
			for (int i = 0; i < 3; i++)
			{
				int num2 = num & 1;
				int num3 = (num & 2) >> 1;
				int num4 = (num & 4) >> 2;
				int num5 = (num & 8) >> 3 << 2 | num4 | num3 << 3 | num2 << 1;
				if (this.tiles[num5].isSource)
				{
					array = new GroundMasks.UVData[this.tiles[num5].variationUVs.Length];
					for (int j = 0; j < this.tiles[num5].variationUVs.Length; j++)
					{
						GroundMasks.UVData uvdata = this.tiles[num5].variationUVs[j];
						GroundMasks.UVData uvdata2 = uvdata;
						switch (i)
						{
						case 0:
							uvdata2 = new GroundMasks.UVData(uvdata.tl, uvdata.bl, uvdata.tr, uvdata.br);
							break;
						case 1:
							uvdata2 = new GroundMasks.UVData(uvdata.tr, uvdata.tl, uvdata.br, uvdata.bl);
							break;
						case 2:
							uvdata2 = new GroundMasks.UVData(uvdata.br, uvdata.tr, uvdata.bl, uvdata.tl);
							break;
						default:
							global::Debug.LogError("Unhandled rotation case");
							break;
						}
						array[j] = uvdata2;
					}
					break;
				}
				num = num5;
			}
			return array;
		}

		// Token: 0x06008F71 RID: 36721 RVA: 0x0038224C File Offset: 0x0038044C
		public void Validate()
		{
			for (int i = 1; i < this.tiles.Length; i++)
			{
				if (this.tiles[i].variationUVs == null)
				{
					DebugUtil.LogErrorArgs(new object[]
					{
						this.name,
						"has invalid tile at index",
						i
					});
				}
			}
		}

		// Token: 0x04006C13 RID: 27667
		public string name;

		// Token: 0x04006C14 RID: 27668
		public GroundMasks.Tile[] tiles;
	}
}
