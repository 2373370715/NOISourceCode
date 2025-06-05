using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x0200213C RID: 8508
	public class DynamicMesh
	{
		// Token: 0x0600B53C RID: 46396 RVA: 0x0011A40F File Offset: 0x0011860F
		public DynamicMesh(string name, Bounds bounds)
		{
			this.Name = name;
			this.Bounds = bounds;
		}

		// Token: 0x0600B53D RID: 46397 RVA: 0x00452F68 File Offset: 0x00451168
		public void Reserve(int vertex_count, int triangle_count)
		{
			if (vertex_count > this.VertexCount)
			{
				this.SetUVs = true;
			}
			else
			{
				this.SetUVs = false;
			}
			if (this.TriangleCount != triangle_count)
			{
				this.SetTriangles = true;
			}
			else
			{
				this.SetTriangles = false;
			}
			int num = (int)Mathf.Ceil((float)triangle_count / (float)DynamicMesh.TrianglesPerMesh);
			if (num != this.Meshes.Length)
			{
				this.Meshes = new DynamicSubMesh[num];
				for (int i = 0; i < this.Meshes.Length; i++)
				{
					int idx_offset = -i * DynamicMesh.VerticesPerMesh;
					this.Meshes[i] = new DynamicSubMesh(this.Name, this.Bounds, idx_offset);
				}
				this.SetUVs = true;
				this.SetTriangles = true;
			}
			for (int j = 0; j < this.Meshes.Length; j++)
			{
				if (j == this.Meshes.Length - 1)
				{
					this.Meshes[j].Reserve(vertex_count % DynamicMesh.VerticesPerMesh, triangle_count % DynamicMesh.TrianglesPerMesh);
				}
				else
				{
					this.Meshes[j].Reserve(DynamicMesh.VerticesPerMesh, DynamicMesh.TrianglesPerMesh);
				}
			}
			this.VertexCount = vertex_count;
			this.TriangleCount = triangle_count;
		}

		// Token: 0x0600B53E RID: 46398 RVA: 0x00453074 File Offset: 0x00451274
		public void Commit()
		{
			DynamicSubMesh[] meshes = this.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].Commit();
			}
			this.TriangleMeshIdx = 0;
			this.UVMeshIdx = 0;
			this.VertexMeshIdx = 0;
		}

		// Token: 0x0600B53F RID: 46399 RVA: 0x004530B4 File Offset: 0x004512B4
		public void AddTriangle(int triangle)
		{
			if (this.Meshes[this.TriangleMeshIdx].AreTrianglesFull())
			{
				DynamicSubMesh[] meshes = this.Meshes;
				int num = this.TriangleMeshIdx + 1;
				this.TriangleMeshIdx = num;
				object obj = meshes[num];
			}
			this.Meshes[this.TriangleMeshIdx].AddTriangle(triangle);
		}

		// Token: 0x0600B540 RID: 46400 RVA: 0x00453104 File Offset: 0x00451304
		public void AddUV(Vector2 uv)
		{
			DynamicSubMesh dynamicSubMesh = this.Meshes[this.UVMeshIdx];
			if (dynamicSubMesh.AreUVsFull())
			{
				DynamicSubMesh[] meshes = this.Meshes;
				int num = this.UVMeshIdx + 1;
				this.UVMeshIdx = num;
				dynamicSubMesh = meshes[num];
			}
			dynamicSubMesh.AddUV(uv);
		}

		// Token: 0x0600B541 RID: 46401 RVA: 0x00453148 File Offset: 0x00451348
		public void AddVertex(Vector3 vertex)
		{
			DynamicSubMesh dynamicSubMesh = this.Meshes[this.VertexMeshIdx];
			if (dynamicSubMesh.AreVerticesFull())
			{
				DynamicSubMesh[] meshes = this.Meshes;
				int num = this.VertexMeshIdx + 1;
				this.VertexMeshIdx = num;
				dynamicSubMesh = meshes[num];
			}
			dynamicSubMesh.AddVertex(vertex);
		}

		// Token: 0x0600B542 RID: 46402 RVA: 0x0045318C File Offset: 0x0045138C
		public void Render(Vector3 position, Quaternion rotation, Material material, int layer, MaterialPropertyBlock property_block)
		{
			DynamicSubMesh[] meshes = this.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].Render(position, rotation, material, layer, property_block);
			}
		}

		// Token: 0x04008F6D RID: 36717
		private static int TrianglesPerMesh = 65004;

		// Token: 0x04008F6E RID: 36718
		private static int VerticesPerMesh = 4 * DynamicMesh.TrianglesPerMesh / 6;

		// Token: 0x04008F6F RID: 36719
		public bool SetUVs;

		// Token: 0x04008F70 RID: 36720
		public bool SetTriangles;

		// Token: 0x04008F71 RID: 36721
		public string Name;

		// Token: 0x04008F72 RID: 36722
		public Bounds Bounds;

		// Token: 0x04008F73 RID: 36723
		public DynamicSubMesh[] Meshes = new DynamicSubMesh[0];

		// Token: 0x04008F74 RID: 36724
		private int VertexCount;

		// Token: 0x04008F75 RID: 36725
		private int TriangleCount;

		// Token: 0x04008F76 RID: 36726
		private int VertexIdx;

		// Token: 0x04008F77 RID: 36727
		private int UVIdx;

		// Token: 0x04008F78 RID: 36728
		private int TriangleIdx;

		// Token: 0x04008F79 RID: 36729
		private int TriangleMeshIdx;

		// Token: 0x04008F7A RID: 36730
		private int VertexMeshIdx;

		// Token: 0x04008F7B RID: 36731
		private int UVMeshIdx;
	}
}
