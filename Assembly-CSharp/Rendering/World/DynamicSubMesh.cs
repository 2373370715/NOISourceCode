using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x0200213D RID: 8509
	public class DynamicSubMesh
	{
		// Token: 0x0600B544 RID: 46404 RVA: 0x004531C0 File Offset: 0x004513C0
		public DynamicSubMesh(string name, Bounds bounds, int idx_offset)
		{
			this.IdxOffset = idx_offset;
			this.Mesh = new Mesh();
			this.Mesh.name = name;
			this.Mesh.bounds = bounds;
			this.Mesh.MarkDynamic();
		}

		// Token: 0x0600B545 RID: 46405 RVA: 0x0045322C File Offset: 0x0045142C
		public void Reserve(int vertex_count, int triangle_count)
		{
			if (vertex_count > this.Vertices.Length)
			{
				this.Vertices = new Vector3[vertex_count];
				this.UVs = new Vector2[vertex_count];
				this.SetUVs = true;
			}
			else
			{
				this.SetUVs = false;
			}
			if (this.Triangles.Length != triangle_count)
			{
				this.Triangles = new int[triangle_count];
				this.SetTriangles = true;
				return;
			}
			this.SetTriangles = false;
		}

		// Token: 0x0600B546 RID: 46406 RVA: 0x0011A44B File Offset: 0x0011864B
		public bool AreTrianglesFull()
		{
			return this.Triangles.Length == this.TriangleIdx;
		}

		// Token: 0x0600B547 RID: 46407 RVA: 0x0011A45D File Offset: 0x0011865D
		public bool AreVerticesFull()
		{
			return this.Vertices.Length == this.VertexIdx;
		}

		// Token: 0x0600B548 RID: 46408 RVA: 0x0011A46F File Offset: 0x0011866F
		public bool AreUVsFull()
		{
			return this.UVs.Length == this.UVIdx;
		}

		// Token: 0x0600B549 RID: 46409 RVA: 0x00453294 File Offset: 0x00451494
		public void Commit()
		{
			if (this.SetTriangles)
			{
				this.Mesh.Clear();
			}
			this.Mesh.vertices = this.Vertices;
			if (this.SetUVs || this.SetTriangles)
			{
				this.Mesh.uv = this.UVs;
			}
			if (this.SetTriangles)
			{
				this.Mesh.triangles = this.Triangles;
			}
			this.VertexIdx = 0;
			this.UVIdx = 0;
			this.TriangleIdx = 0;
		}

		// Token: 0x0600B54A RID: 46410 RVA: 0x00453314 File Offset: 0x00451514
		public void AddTriangle(int triangle)
		{
			int[] triangles = this.Triangles;
			int triangleIdx = this.TriangleIdx;
			this.TriangleIdx = triangleIdx + 1;
			triangles[triangleIdx] = triangle + this.IdxOffset;
		}

		// Token: 0x0600B54B RID: 46411 RVA: 0x00453344 File Offset: 0x00451544
		public void AddUV(Vector2 uv)
		{
			Vector2[] uvs = this.UVs;
			int uvidx = this.UVIdx;
			this.UVIdx = uvidx + 1;
			uvs[uvidx] = uv;
		}

		// Token: 0x0600B54C RID: 46412 RVA: 0x00453370 File Offset: 0x00451570
		public void AddVertex(Vector3 vertex)
		{
			Vector3[] vertices = this.Vertices;
			int vertexIdx = this.VertexIdx;
			this.VertexIdx = vertexIdx + 1;
			vertices[vertexIdx] = vertex;
		}

		// Token: 0x0600B54D RID: 46413 RVA: 0x0045339C File Offset: 0x0045159C
		public void Render(Vector3 position, Quaternion rotation, Material material, int layer, MaterialPropertyBlock property_block)
		{
			Graphics.DrawMesh(this.Mesh, position, rotation, material, layer, null, 0, property_block, false, false);
		}

		// Token: 0x04008F7C RID: 36732
		public Vector3[] Vertices = new Vector3[0];

		// Token: 0x04008F7D RID: 36733
		public Vector2[] UVs = new Vector2[0];

		// Token: 0x04008F7E RID: 36734
		public int[] Triangles = new int[0];

		// Token: 0x04008F7F RID: 36735
		public Mesh Mesh;

		// Token: 0x04008F80 RID: 36736
		public bool SetUVs;

		// Token: 0x04008F81 RID: 36737
		public bool SetTriangles;

		// Token: 0x04008F82 RID: 36738
		private int VertexIdx;

		// Token: 0x04008F83 RID: 36739
		private int UVIdx;

		// Token: 0x04008F84 RID: 36740
		private int TriangleIdx;

		// Token: 0x04008F85 RID: 36741
		private int IdxOffset;
	}
}
