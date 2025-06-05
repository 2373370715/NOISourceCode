using System;
using System.Collections.Generic;
using ProcGen;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02001ACD RID: 6861
[AddComponentMenu("KMonoBehaviour/scripts/GroundRenderer")]
public class GroundRenderer : KMonoBehaviour
{
	// Token: 0x06008F72 RID: 36722 RVA: 0x003822A4 File Offset: 0x003804A4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
		this.OnShadersReloaded();
		this.masks.Initialize();
		SubWorld.ZoneType[] array = (SubWorld.ZoneType[])Enum.GetValues(typeof(SubWorld.ZoneType));
		this.biomeMasks = new GroundMasks.BiomeMaskData[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			SubWorld.ZoneType zone_type = array[i];
			this.biomeMasks[i] = this.GetBiomeMask(zone_type);
		}
	}

	// Token: 0x06008F73 RID: 36723 RVA: 0x00382320 File Offset: 0x00380520
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.size = new Vector2I((Grid.WidthInCells + 16 - 1) / 16, (Grid.HeightInCells + 16 - 1) / 16);
		this.dirtyChunks = new bool[this.size.x, this.size.y];
		this.worldChunks = new GroundRenderer.WorldChunk[this.size.x, this.size.y];
		for (int i = 0; i < this.size.y; i++)
		{
			for (int j = 0; j < this.size.x; j++)
			{
				this.worldChunks[j, i] = new GroundRenderer.WorldChunk(j, i);
				this.dirtyChunks[j, i] = true;
			}
		}
	}

	// Token: 0x06008F74 RID: 36724 RVA: 0x003823E8 File Offset: 0x003805E8
	public void Render(Vector2I vis_min, Vector2I vis_max, bool forceVisibleRebuild = false)
	{
		if (!base.enabled)
		{
			return;
		}
		int layer = LayerMask.NameToLayer("World");
		Vector2I vector2I = new Vector2I(vis_min.x / 16, vis_min.y / 16);
		Vector2I vector2I2 = new Vector2I((vis_max.x + 16 - 1) / 16, (vis_max.y + 16 - 1) / 16);
		for (int i = vector2I.y; i < vector2I2.y; i++)
		{
			for (int j = vector2I.x; j < vector2I2.x; j++)
			{
				GroundRenderer.WorldChunk worldChunk = this.worldChunks[j, i];
				if (this.dirtyChunks[j, i] || forceVisibleRebuild)
				{
					this.dirtyChunks[j, i] = false;
					worldChunk.Rebuild(this.biomeMasks, this.elementMaterials);
				}
				worldChunk.Render(layer);
			}
		}
		this.RebuildDirtyChunks();
	}

	// Token: 0x06008F75 RID: 36725 RVA: 0x001021DE File Offset: 0x001003DE
	public void RenderAll()
	{
		this.Render(new Vector2I(0, 0), new Vector2I(this.worldChunks.GetLength(0) * 16, this.worldChunks.GetLength(1) * 16), true);
	}

	// Token: 0x06008F76 RID: 36726 RVA: 0x003824C8 File Offset: 0x003806C8
	private void RebuildDirtyChunks()
	{
		for (int i = 0; i < this.dirtyChunks.GetLength(1); i++)
		{
			for (int j = 0; j < this.dirtyChunks.GetLength(0); j++)
			{
				if (this.dirtyChunks[j, i])
				{
					this.dirtyChunks[j, i] = false;
					this.worldChunks[j, i].Rebuild(this.biomeMasks, this.elementMaterials);
				}
			}
		}
	}

	// Token: 0x06008F77 RID: 36727 RVA: 0x00382540 File Offset: 0x00380740
	public void MarkDirty(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = new Vector2I(vector2I.x / 16, vector2I.y / 16);
		this.dirtyChunks[vector2I2.x, vector2I2.y] = true;
		bool flag = vector2I.x % 16 == 0 && vector2I2.x > 0;
		bool flag2 = vector2I.x % 16 == 15 && vector2I2.x < this.size.x - 1;
		bool flag3 = vector2I.y % 16 == 0 && vector2I2.y > 0;
		bool flag4 = vector2I.y % 16 == 15 && vector2I2.y < this.size.y - 1;
		if (flag)
		{
			this.dirtyChunks[vector2I2.x - 1, vector2I2.y] = true;
			if (flag3)
			{
				this.dirtyChunks[vector2I2.x - 1, vector2I2.y - 1] = true;
			}
			if (flag4)
			{
				this.dirtyChunks[vector2I2.x - 1, vector2I2.y + 1] = true;
			}
		}
		if (flag3)
		{
			this.dirtyChunks[vector2I2.x, vector2I2.y - 1] = true;
		}
		if (flag4)
		{
			this.dirtyChunks[vector2I2.x, vector2I2.y + 1] = true;
		}
		if (flag2)
		{
			this.dirtyChunks[vector2I2.x + 1, vector2I2.y] = true;
			if (flag3)
			{
				this.dirtyChunks[vector2I2.x + 1, vector2I2.y - 1] = true;
			}
			if (flag4)
			{
				this.dirtyChunks[vector2I2.x + 1, vector2I2.y + 1] = true;
			}
		}
	}

	// Token: 0x06008F78 RID: 36728 RVA: 0x003826F4 File Offset: 0x003808F4
	private Vector2I GetChunkIdx(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		return new Vector2I(vector2I.x / 16, vector2I.y / 16);
	}

	// Token: 0x06008F79 RID: 36729 RVA: 0x00382720 File Offset: 0x00380920
	private GroundMasks.BiomeMaskData GetBiomeMask(SubWorld.ZoneType zone_type)
	{
		GroundMasks.BiomeMaskData result = null;
		string key = zone_type.ToString().ToLower();
		this.masks.biomeMasks.TryGetValue(key, out result);
		return result;
	}

	// Token: 0x06008F7A RID: 36730 RVA: 0x00382758 File Offset: 0x00380958
	private void InitOpaqueMaterial(Material material, Element element)
	{
		material.name = element.id.ToString() + "_opaque";
		material.renderQueue = RenderQueues.WorldOpaque;
		material.EnableKeyword("OPAQUE");
		material.DisableKeyword("ALPHA");
		this.ConfigureMaterialShine(material);
		material.SetInt("_SrcAlpha", 1);
		material.SetInt("_DstAlpha", 0);
		material.SetInt("_ZWrite", 1);
		material.SetTexture("_AlphaTestMap", Texture2D.whiteTexture);
	}

	// Token: 0x06008F7B RID: 36731 RVA: 0x003827E4 File Offset: 0x003809E4
	private void InitAlphaMaterial(Material material, Element element)
	{
		material.name = element.id.ToString() + "_alpha";
		material.renderQueue = RenderQueues.WorldTransparent;
		material.EnableKeyword("ALPHA");
		material.DisableKeyword("OPAQUE");
		this.ConfigureMaterialShine(material);
		material.SetTexture("_AlphaTestMap", this.masks.maskAtlas.texture);
		material.SetInt("_SrcAlpha", 5);
		material.SetInt("_DstAlpha", 10);
		material.SetInt("_ZWrite", 0);
	}

	// Token: 0x06008F7C RID: 36732 RVA: 0x0038287C File Offset: 0x00380A7C
	private void ConfigureMaterialShine(Material material)
	{
		if (material.GetTexture("_ShineMask") != null)
		{
			material.DisableKeyword("MATTE");
			material.EnableKeyword("SHINY");
			return;
		}
		material.EnableKeyword("MATTE");
		material.DisableKeyword("SHINY");
	}

	// Token: 0x06008F7D RID: 36733 RVA: 0x003828CC File Offset: 0x00380ACC
	[ContextMenu("Reload Shaders")]
	public void OnShadersReloaded()
	{
		this.FreeMaterials();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsSolid)
			{
				if (element.substance.material == null)
				{
					DebugUtil.LogErrorArgs(new object[]
					{
						element.name,
						"must have material associated with it in the substance table"
					});
				}
				Material material = new Material(element.substance.material);
				this.InitOpaqueMaterial(material, element);
				Material material2 = new Material(material);
				this.InitAlphaMaterial(material2, element);
				GroundRenderer.Materials value = new GroundRenderer.Materials(material, material2);
				this.elementMaterials[element.id] = value;
			}
		}
		if (this.worldChunks != null)
		{
			for (int i = 0; i < this.dirtyChunks.GetLength(1); i++)
			{
				for (int j = 0; j < this.dirtyChunks.GetLength(0); j++)
				{
					this.dirtyChunks[j, i] = true;
				}
			}
			GroundRenderer.WorldChunk[,] array = this.worldChunks;
			int upperBound = array.GetUpperBound(0);
			int upperBound2 = array.GetUpperBound(1);
			for (int k = array.GetLowerBound(0); k <= upperBound; k++)
			{
				for (int l = array.GetLowerBound(1); l <= upperBound2; l++)
				{
					GroundRenderer.WorldChunk worldChunk = array[k, l];
					worldChunk.Clear();
					worldChunk.Rebuild(this.biomeMasks, this.elementMaterials);
				}
			}
		}
	}

	// Token: 0x06008F7E RID: 36734 RVA: 0x00382A64 File Offset: 0x00380C64
	public void FreeResources()
	{
		this.FreeMaterials();
		this.elementMaterials.Clear();
		this.elementMaterials = null;
		if (this.worldChunks != null)
		{
			GroundRenderer.WorldChunk[,] array = this.worldChunks;
			int upperBound = array.GetUpperBound(0);
			int upperBound2 = array.GetUpperBound(1);
			for (int i = array.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = array.GetLowerBound(1); j <= upperBound2; j++)
				{
					GroundRenderer.WorldChunk worldChunk = array[i, j];
					worldChunk.FreeResources();
				}
			}
			this.worldChunks = null;
		}
	}

	// Token: 0x06008F7F RID: 36735 RVA: 0x00382AEC File Offset: 0x00380CEC
	private void FreeMaterials()
	{
		foreach (GroundRenderer.Materials materials in this.elementMaterials.Values)
		{
			UnityEngine.Object.Destroy(materials.opaque);
			UnityEngine.Object.Destroy(materials.alpha);
		}
		this.elementMaterials.Clear();
	}

	// Token: 0x04006C15 RID: 27669
	[SerializeField]
	private GroundMasks masks;

	// Token: 0x04006C16 RID: 27670
	private GroundMasks.BiomeMaskData[] biomeMasks;

	// Token: 0x04006C17 RID: 27671
	private Dictionary<SimHashes, GroundRenderer.Materials> elementMaterials = new Dictionary<SimHashes, GroundRenderer.Materials>();

	// Token: 0x04006C18 RID: 27672
	private bool[,] dirtyChunks;

	// Token: 0x04006C19 RID: 27673
	private GroundRenderer.WorldChunk[,] worldChunks;

	// Token: 0x04006C1A RID: 27674
	private const int ChunkEdgeSize = 16;

	// Token: 0x04006C1B RID: 27675
	private Vector2I size;

	// Token: 0x02001ACE RID: 6862
	[Serializable]
	private struct Materials
	{
		// Token: 0x06008F81 RID: 36737 RVA: 0x00102224 File Offset: 0x00100424
		public Materials(Material opaque, Material alpha)
		{
			this.opaque = opaque;
			this.alpha = alpha;
		}

		// Token: 0x04006C1C RID: 27676
		public Material opaque;

		// Token: 0x04006C1D RID: 27677
		public Material alpha;
	}

	// Token: 0x02001ACF RID: 6863
	private class ElementChunk
	{
		// Token: 0x06008F82 RID: 36738 RVA: 0x00382B5C File Offset: 0x00380D5C
		public ElementChunk(SimHashes element, Dictionary<SimHashes, GroundRenderer.Materials> materials)
		{
			this.element = element;
			GroundRenderer.Materials materials2 = materials[element];
			this.alpha = new GroundRenderer.ElementChunk.RenderData(materials2.alpha);
			this.opaque = new GroundRenderer.ElementChunk.RenderData(materials2.opaque);
			this.Clear();
		}

		// Token: 0x06008F83 RID: 36739 RVA: 0x00102234 File Offset: 0x00100434
		public void Clear()
		{
			this.opaque.Clear();
			this.alpha.Clear();
			this.tileCount = 0;
		}

		// Token: 0x06008F84 RID: 36740 RVA: 0x00102253 File Offset: 0x00100453
		public void AddOpaqueQuad(int x, int y, GroundMasks.UVData uvs)
		{
			this.opaque.AddQuad(x, y, uvs);
			this.tileCount++;
		}

		// Token: 0x06008F85 RID: 36741 RVA: 0x00102271 File Offset: 0x00100471
		public void AddAlphaQuad(int x, int y, GroundMasks.UVData uvs)
		{
			this.alpha.AddQuad(x, y, uvs);
			this.tileCount++;
		}

		// Token: 0x06008F86 RID: 36742 RVA: 0x0010228F File Offset: 0x0010048F
		public void Build()
		{
			this.opaque.Build();
			this.alpha.Build();
		}

		// Token: 0x06008F87 RID: 36743 RVA: 0x00382BA8 File Offset: 0x00380DA8
		public void Render(int layer, int element_idx)
		{
			float num = Grid.GetLayerZ(Grid.SceneLayer.Ground);
			num -= 0.0001f * (float)element_idx;
			this.opaque.Render(new Vector3(0f, 0f, num), layer);
			this.alpha.Render(new Vector3(0f, 0f, num), layer);
		}

		// Token: 0x06008F88 RID: 36744 RVA: 0x001022A7 File Offset: 0x001004A7
		public void FreeResources()
		{
			this.alpha.FreeResources();
			this.opaque.FreeResources();
			this.alpha = null;
			this.opaque = null;
		}

		// Token: 0x04006C1E RID: 27678
		public SimHashes element;

		// Token: 0x04006C1F RID: 27679
		private GroundRenderer.ElementChunk.RenderData alpha;

		// Token: 0x04006C20 RID: 27680
		private GroundRenderer.ElementChunk.RenderData opaque;

		// Token: 0x04006C21 RID: 27681
		public int tileCount;

		// Token: 0x02001AD0 RID: 6864
		private class RenderData
		{
			// Token: 0x06008F89 RID: 36745 RVA: 0x00382C00 File Offset: 0x00380E00
			public RenderData(Material material)
			{
				this.material = material;
				this.mesh = new Mesh();
				this.mesh.MarkDynamic();
				this.mesh.name = "ElementChunk";
				this.pos = new List<Vector3>();
				this.uv = new List<Vector2>();
				this.indices = new List<int>();
			}

			// Token: 0x06008F8A RID: 36746 RVA: 0x001022CD File Offset: 0x001004CD
			public void ClearMesh()
			{
				if (this.mesh != null)
				{
					this.mesh.Clear();
					UnityEngine.Object.DestroyImmediate(this.mesh);
					this.mesh = null;
				}
			}

			// Token: 0x06008F8B RID: 36747 RVA: 0x00382C64 File Offset: 0x00380E64
			public void Clear()
			{
				if (this.mesh != null)
				{
					this.mesh.Clear();
				}
				if (this.pos != null)
				{
					this.pos.Clear();
				}
				if (this.uv != null)
				{
					this.uv.Clear();
				}
				if (this.indices != null)
				{
					this.indices.Clear();
				}
			}

			// Token: 0x06008F8C RID: 36748 RVA: 0x001022FA File Offset: 0x001004FA
			public void FreeResources()
			{
				this.ClearMesh();
				this.Clear();
				this.pos = null;
				this.uv = null;
				this.indices = null;
				this.material = null;
			}

			// Token: 0x06008F8D RID: 36749 RVA: 0x00102324 File Offset: 0x00100524
			public void Build()
			{
				this.mesh.SetVertices(this.pos);
				this.mesh.SetUVs(0, this.uv);
				this.mesh.SetTriangles(this.indices, 0);
			}

			// Token: 0x06008F8E RID: 36750 RVA: 0x00382CC4 File Offset: 0x00380EC4
			public void AddQuad(int x, int y, GroundMasks.UVData uvs)
			{
				int count = this.pos.Count;
				this.indices.Add(count);
				this.indices.Add(count + 1);
				this.indices.Add(count + 3);
				this.indices.Add(count);
				this.indices.Add(count + 3);
				this.indices.Add(count + 2);
				this.pos.Add(new Vector3((float)x + -0.5f, (float)y + -0.5f, 0f));
				this.pos.Add(new Vector3((float)x + 1f + -0.5f, (float)y + -0.5f, 0f));
				this.pos.Add(new Vector3((float)x + -0.5f, (float)y + 1f + -0.5f, 0f));
				this.pos.Add(new Vector3((float)x + 1f + -0.5f, (float)y + 1f + -0.5f, 0f));
				this.uv.Add(uvs.bl);
				this.uv.Add(uvs.br);
				this.uv.Add(uvs.tl);
				this.uv.Add(uvs.tr);
			}

			// Token: 0x06008F8F RID: 36751 RVA: 0x00382E20 File Offset: 0x00381020
			public void Render(Vector3 position, int layer)
			{
				if (this.pos.Count != 0)
				{
					Graphics.DrawMesh(this.mesh, position, Quaternion.identity, this.material, layer, null, 0, null, ShadowCastingMode.Off, false, null, false);
				}
			}

			// Token: 0x04006C22 RID: 27682
			public Material material;

			// Token: 0x04006C23 RID: 27683
			public Mesh mesh;

			// Token: 0x04006C24 RID: 27684
			public List<Vector3> pos;

			// Token: 0x04006C25 RID: 27685
			public List<Vector2> uv;

			// Token: 0x04006C26 RID: 27686
			public List<int> indices;
		}
	}

	// Token: 0x02001AD1 RID: 6865
	private struct WorldChunk
	{
		// Token: 0x06008F90 RID: 36752 RVA: 0x0010235B File Offset: 0x0010055B
		public WorldChunk(int x, int y)
		{
			this.chunkX = x;
			this.chunkY = y;
			this.elementChunks = new List<GroundRenderer.ElementChunk>();
		}

		// Token: 0x06008F91 RID: 36753 RVA: 0x00102376 File Offset: 0x00100576
		public void Clear()
		{
			this.elementChunks.Clear();
		}

		// Token: 0x06008F92 RID: 36754 RVA: 0x00382E5C File Offset: 0x0038105C
		private static void InsertSorted(Element element, Element[] array, int size)
		{
			int id = (int)element.id;
			for (int i = 0; i < size; i++)
			{
				Element element2 = array[i];
				if (element2.id > (SimHashes)id)
				{
					array[i] = element;
					element = element2;
					id = (int)element2.id;
				}
			}
			array[size] = element;
		}

		// Token: 0x06008F93 RID: 36755 RVA: 0x00382E9C File Offset: 0x0038109C
		public void Rebuild(GroundMasks.BiomeMaskData[] biomeMasks, Dictionary<SimHashes, GroundRenderer.Materials> materials)
		{
			foreach (GroundRenderer.ElementChunk elementChunk in this.elementChunks)
			{
				elementChunk.Clear();
			}
			Vector2I vector2I = new Vector2I(this.chunkX * 16, this.chunkY * 16);
			Vector2I vector2I2 = new Vector2I(Math.Min(Grid.WidthInCells, (this.chunkX + 1) * 16), Math.Min(Grid.HeightInCells, (this.chunkY + 1) * 16));
			for (int i = vector2I.y; i < vector2I2.y; i++)
			{
				int num = Math.Max(0, i - 1);
				int num2 = i;
				for (int j = vector2I.x; j < vector2I2.x; j++)
				{
					int num3 = Math.Max(0, j - 1);
					int num4 = j;
					int num5 = num * Grid.WidthInCells + num3;
					int num6 = num * Grid.WidthInCells + num4;
					int num7 = num2 * Grid.WidthInCells + num3;
					int num8 = num2 * Grid.WidthInCells + num4;
					GroundRenderer.WorldChunk.elements[0] = Grid.Element[num5];
					GroundRenderer.WorldChunk.elements[1] = Grid.Element[num6];
					GroundRenderer.WorldChunk.elements[2] = Grid.Element[num7];
					GroundRenderer.WorldChunk.elements[3] = Grid.Element[num8];
					GroundRenderer.WorldChunk.substances[0] = ((Grid.RenderedByWorld[num5] && GroundRenderer.WorldChunk.elements[0].IsSolid) ? GroundRenderer.WorldChunk.elements[0].substance.idx : -1);
					GroundRenderer.WorldChunk.substances[1] = ((Grid.RenderedByWorld[num6] && GroundRenderer.WorldChunk.elements[1].IsSolid) ? GroundRenderer.WorldChunk.elements[1].substance.idx : -1);
					GroundRenderer.WorldChunk.substances[2] = ((Grid.RenderedByWorld[num7] && GroundRenderer.WorldChunk.elements[2].IsSolid) ? GroundRenderer.WorldChunk.elements[2].substance.idx : -1);
					GroundRenderer.WorldChunk.substances[3] = ((Grid.RenderedByWorld[num8] && GroundRenderer.WorldChunk.elements[3].IsSolid) ? GroundRenderer.WorldChunk.elements[3].substance.idx : -1);
					GroundRenderer.WorldChunk.uniqueElements[0] = GroundRenderer.WorldChunk.elements[0];
					GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[1], GroundRenderer.WorldChunk.uniqueElements, 1);
					GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[2], GroundRenderer.WorldChunk.uniqueElements, 2);
					GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[3], GroundRenderer.WorldChunk.uniqueElements, 3);
					int num9 = -1;
					int biomeIdx = GroundRenderer.WorldChunk.GetBiomeIdx(i * Grid.WidthInCells + j);
					GroundMasks.BiomeMaskData biomeMaskData = biomeMasks[biomeIdx];
					if (biomeMaskData == null)
					{
						biomeMaskData = biomeMasks[3];
					}
					for (int k = 0; k < GroundRenderer.WorldChunk.uniqueElements.Length; k++)
					{
						Element element = GroundRenderer.WorldChunk.uniqueElements[k];
						if (element.IsSolid)
						{
							int idx = element.substance.idx;
							if (idx != num9)
							{
								num9 = idx;
								int num10 = ((GroundRenderer.WorldChunk.substances[2] >= idx) ? 1 : 0) << 3 | ((GroundRenderer.WorldChunk.substances[3] >= idx) ? 1 : 0) << 2 | ((GroundRenderer.WorldChunk.substances[0] >= idx) ? 1 : 0) << 1 | ((GroundRenderer.WorldChunk.substances[1] >= idx) ? 1 : 0);
								if (num10 > 0)
								{
									GroundMasks.UVData[] variationUVs = biomeMaskData.tiles[num10].variationUVs;
									float staticRandom = GroundRenderer.WorldChunk.GetStaticRandom(j, i);
									int num11 = Mathf.Min(variationUVs.Length - 1, (int)((float)variationUVs.Length * staticRandom));
									GroundMasks.UVData uvs = variationUVs[num11 % variationUVs.Length];
									GroundRenderer.ElementChunk elementChunk2 = this.GetElementChunk(element.id, materials);
									if (num10 == 15)
									{
										elementChunk2.AddOpaqueQuad(j, i, uvs);
									}
									else
									{
										elementChunk2.AddAlphaQuad(j, i, uvs);
									}
								}
							}
						}
					}
				}
			}
			foreach (GroundRenderer.ElementChunk elementChunk3 in this.elementChunks)
			{
				elementChunk3.Build();
			}
			for (int l = this.elementChunks.Count - 1; l >= 0; l--)
			{
				if (this.elementChunks[l].tileCount == 0)
				{
					int index = this.elementChunks.Count - 1;
					this.elementChunks[l] = this.elementChunks[index];
					this.elementChunks.RemoveAt(index);
				}
			}
		}

		// Token: 0x06008F94 RID: 36756 RVA: 0x003832F8 File Offset: 0x003814F8
		private GroundRenderer.ElementChunk GetElementChunk(SimHashes elementID, Dictionary<SimHashes, GroundRenderer.Materials> materials)
		{
			GroundRenderer.ElementChunk elementChunk = null;
			for (int i = 0; i < this.elementChunks.Count; i++)
			{
				if (this.elementChunks[i].element == elementID)
				{
					elementChunk = this.elementChunks[i];
					break;
				}
			}
			if (elementChunk == null)
			{
				elementChunk = new GroundRenderer.ElementChunk(elementID, materials);
				this.elementChunks.Add(elementChunk);
			}
			return elementChunk;
		}

		// Token: 0x06008F95 RID: 36757 RVA: 0x00383358 File Offset: 0x00381558
		private static int GetBiomeIdx(int cell)
		{
			if (!Grid.IsValidCell(cell))
			{
				return 0;
			}
			SubWorld.ZoneType result = SubWorld.ZoneType.Sandstone;
			if (global::World.Instance != null && global::World.Instance.zoneRenderData != null)
			{
				result = global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell);
			}
			return (int)result;
		}

		// Token: 0x06008F96 RID: 36758 RVA: 0x00102383 File Offset: 0x00100583
		private static float GetStaticRandom(int x, int y)
		{
			return PerlinSimplexNoise.noise((float)x * GroundRenderer.WorldChunk.NoiseScale.x, (float)y * GroundRenderer.WorldChunk.NoiseScale.y);
		}

		// Token: 0x06008F97 RID: 36759 RVA: 0x003833A4 File Offset: 0x003815A4
		public void Render(int layer)
		{
			for (int i = 0; i < this.elementChunks.Count; i++)
			{
				GroundRenderer.ElementChunk elementChunk = this.elementChunks[i];
				elementChunk.Render(layer, ElementLoader.FindElementByHash(elementChunk.element).substance.idx);
			}
		}

		// Token: 0x06008F98 RID: 36760 RVA: 0x003833F0 File Offset: 0x003815F0
		public void FreeResources()
		{
			foreach (GroundRenderer.ElementChunk elementChunk in this.elementChunks)
			{
				elementChunk.FreeResources();
			}
			this.elementChunks.Clear();
			this.elementChunks = null;
		}

		// Token: 0x04006C27 RID: 27687
		public readonly int chunkX;

		// Token: 0x04006C28 RID: 27688
		public readonly int chunkY;

		// Token: 0x04006C29 RID: 27689
		private List<GroundRenderer.ElementChunk> elementChunks;

		// Token: 0x04006C2A RID: 27690
		private static Element[] elements = new Element[4];

		// Token: 0x04006C2B RID: 27691
		private static Element[] uniqueElements = new Element[4];

		// Token: 0x04006C2C RID: 27692
		private static int[] substances = new int[4];

		// Token: 0x04006C2D RID: 27693
		private static Vector2 NoiseScale = new Vector3(1f, 1f);
	}
}
