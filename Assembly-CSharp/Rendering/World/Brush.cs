using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x0200213B RID: 8507
	public class Brush
	{
		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x0600B533 RID: 46387 RVA: 0x0011A38C File Offset: 0x0011858C
		// (set) Token: 0x0600B534 RID: 46388 RVA: 0x0011A394 File Offset: 0x00118594
		public int Id { get; private set; }

		// Token: 0x0600B535 RID: 46389 RVA: 0x00452C34 File Offset: 0x00450E34
		public Brush(int id, string name, Material material, Mask mask, List<Brush> active_brushes, List<Brush> dirty_brushes, int width_in_tiles, MaterialPropertyBlock property_block)
		{
			this.Id = id;
			this.material = material;
			this.mask = mask;
			this.mesh = new DynamicMesh(name, new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, 0f)));
			this.activeBrushes = active_brushes;
			this.dirtyBrushes = dirty_brushes;
			this.layer = LayerMask.NameToLayer("World");
			this.widthInTiles = width_in_tiles;
			this.propertyBlock = property_block;
		}

		// Token: 0x0600B536 RID: 46390 RVA: 0x0011A39D File Offset: 0x0011859D
		public void Add(int tile_idx)
		{
			this.tiles.Add(tile_idx);
			if (!this.dirty)
			{
				this.dirtyBrushes.Add(this);
				this.dirty = true;
			}
		}

		// Token: 0x0600B537 RID: 46391 RVA: 0x0011A3C7 File Offset: 0x001185C7
		public void Remove(int tile_idx)
		{
			this.tiles.Remove(tile_idx);
			if (!this.dirty)
			{
				this.dirtyBrushes.Add(this);
				this.dirty = true;
			}
		}

		// Token: 0x0600B538 RID: 46392 RVA: 0x0011A3F1 File Offset: 0x001185F1
		public void SetMaskOffset(int offset)
		{
			this.mask.SetOffset(offset);
		}

		// Token: 0x0600B539 RID: 46393 RVA: 0x00452CC4 File Offset: 0x00450EC4
		public void Refresh()
		{
			bool flag = this.mesh.Meshes.Length != 0;
			int count = this.tiles.Count;
			int vertex_count = count * 4;
			int triangle_count = count * 6;
			this.mesh.Reserve(vertex_count, triangle_count);
			if (this.mesh.SetTriangles)
			{
				int num = 0;
				for (int i = 0; i < count; i++)
				{
					this.mesh.AddTriangle(num);
					this.mesh.AddTriangle(2 + num);
					this.mesh.AddTriangle(1 + num);
					this.mesh.AddTriangle(1 + num);
					this.mesh.AddTriangle(2 + num);
					this.mesh.AddTriangle(3 + num);
					num += 4;
				}
			}
			foreach (int num2 in this.tiles)
			{
				float num3 = (float)(num2 % this.widthInTiles);
				float num4 = (float)(num2 / this.widthInTiles);
				float z = 0f;
				this.mesh.AddVertex(new Vector3(num3 - 0.5f, num4 - 0.5f, z));
				this.mesh.AddVertex(new Vector3(num3 + 0.5f, num4 - 0.5f, z));
				this.mesh.AddVertex(new Vector3(num3 - 0.5f, num4 + 0.5f, z));
				this.mesh.AddVertex(new Vector3(num3 + 0.5f, num4 + 0.5f, z));
			}
			if (this.mesh.SetUVs)
			{
				for (int j = 0; j < count; j++)
				{
					this.mesh.AddUV(this.mask.UV0);
					this.mesh.AddUV(this.mask.UV1);
					this.mesh.AddUV(this.mask.UV2);
					this.mesh.AddUV(this.mask.UV3);
				}
			}
			this.dirty = false;
			this.mesh.Commit();
			if (this.mesh.Meshes.Length != 0)
			{
				if (!flag)
				{
					this.activeBrushes.Add(this);
					return;
				}
			}
			else if (flag)
			{
				this.activeBrushes.Remove(this);
			}
		}

		// Token: 0x0600B53A RID: 46394 RVA: 0x00452F20 File Offset: 0x00451120
		public void Render()
		{
			Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.Ground));
			this.mesh.Render(position, Quaternion.identity, this.material, this.layer, this.propertyBlock);
		}

		// Token: 0x0600B53B RID: 46395 RVA: 0x0011A3FF File Offset: 0x001185FF
		public void SetMaterial(Material material, MaterialPropertyBlock property_block)
		{
			this.material = material;
			this.propertyBlock = property_block;
		}

		// Token: 0x04008F63 RID: 36707
		private bool dirty;

		// Token: 0x04008F64 RID: 36708
		private Material material;

		// Token: 0x04008F65 RID: 36709
		private int layer;

		// Token: 0x04008F66 RID: 36710
		private HashSet<int> tiles = new HashSet<int>();

		// Token: 0x04008F67 RID: 36711
		private List<Brush> activeBrushes;

		// Token: 0x04008F68 RID: 36712
		private List<Brush> dirtyBrushes;

		// Token: 0x04008F69 RID: 36713
		private int widthInTiles;

		// Token: 0x04008F6A RID: 36714
		private Mask mask;

		// Token: 0x04008F6B RID: 36715
		private DynamicMesh mesh;

		// Token: 0x04008F6C RID: 36716
		private MaterialPropertyBlock propertyBlock;
	}
}
