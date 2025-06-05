using System;
using UnityEngine;

// Token: 0x020017C9 RID: 6089
public class FullScreenQuad
{
	// Token: 0x06007D1D RID: 32029 RVA: 0x00330534 File Offset: 0x0032E734
	public FullScreenQuad(string name, Camera camera, bool invert = false)
	{
		this.Camera = camera;
		this.Layer = LayerMask.NameToLayer("ForceDraw");
		this.Mesh = new Mesh();
		this.Mesh.name = name;
		this.Mesh.vertices = new Vector3[]
		{
			new Vector3(-1f, -1f, 0f),
			new Vector3(-1f, 1f, 0f),
			new Vector3(1f, -1f, 0f),
			new Vector3(1f, 1f, 0f)
		};
		float y = 1f;
		float y2 = 0f;
		if (invert)
		{
			y = 0f;
			y2 = 1f;
		}
		this.Mesh.uv = new Vector2[]
		{
			new Vector2(0f, y2),
			new Vector2(0f, y),
			new Vector2(1f, y2),
			new Vector2(1f, y)
		};
		this.Mesh.triangles = new int[]
		{
			0,
			1,
			2,
			2,
			1,
			3
		};
		this.Mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
		this.Material = new Material(Shader.Find("Klei/PostFX/FullScreen"));
		this.Camera.cullingMask = (this.Camera.cullingMask | LayerMask.GetMask(new string[]
		{
			"ForceDraw"
		}));
	}

	// Token: 0x06007D1E RID: 32030 RVA: 0x003306EC File Offset: 0x0032E8EC
	public void Draw(Texture texture)
	{
		this.Material.mainTexture = texture;
		Graphics.DrawMesh(this.Mesh, Vector3.zero, Quaternion.identity, this.Material, this.Layer, this.Camera, 0, null, false, false);
	}

	// Token: 0x04005E3C RID: 24124
	private Mesh Mesh;

	// Token: 0x04005E3D RID: 24125
	private Camera Camera;

	// Token: 0x04005E3E RID: 24126
	private Material Material;

	// Token: 0x04005E3F RID: 24127
	private int Layer;
}
