using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020017E4 RID: 6116
public class TextureLerper
{
	// Token: 0x06007DB7 RID: 32183 RVA: 0x003335F0 File Offset: 0x003317F0
	public TextureLerper(Texture target_texture, string name, FilterMode filter_mode = FilterMode.Bilinear, TextureFormat texture_format = TextureFormat.ARGB32)
	{
		this.name = name;
		this.Init(target_texture.width, target_texture.height, name, filter_mode, texture_format);
		this.Material.SetTexture("_TargetTex", target_texture);
	}

	// Token: 0x06007DB8 RID: 32184 RVA: 0x00333648 File Offset: 0x00331848
	private void Init(int width, int height, string name, FilterMode filter_mode, TextureFormat texture_format)
	{
		for (int i = 0; i < 2; i++)
		{
			this.BlendTextures[i] = new RenderTexture(width, height, 0, TextureUtil.GetRenderTextureFormat(texture_format));
			this.BlendTextures[i].filterMode = filter_mode;
			this.BlendTextures[i].name = name;
		}
		this.Material = new Material(Shader.Find("Klei/LerpEffect"));
		this.Material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
		this.mesh = new Mesh();
		this.mesh.name = "LerpEffect";
		this.mesh.vertices = new Vector3[]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(1f, 1f, 0f),
			new Vector3(0f, 1f, 0f),
			new Vector3(1f, 0f, 0f)
		};
		this.mesh.triangles = new int[]
		{
			0,
			1,
			2,
			0,
			3,
			1
		};
		this.mesh.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 1f),
			new Vector2(0f, 1f),
			new Vector2(1f, 0f)
		};
		int layer = LayerMask.NameToLayer("RTT");
		int mask = LayerMask.GetMask(new string[]
		{
			"RTT"
		});
		this.cameraGO = new GameObject();
		this.cameraGO.name = "TextureLerper_" + name;
		this.textureCam = this.cameraGO.AddComponent<Camera>();
		this.textureCam.transform.SetPosition(new Vector3((float)TextureLerper.offsetCounter + 0.5f, 0.5f, 0f));
		this.textureCam.clearFlags = CameraClearFlags.Nothing;
		this.textureCam.depth = -100f;
		this.textureCam.allowHDR = false;
		this.textureCam.orthographic = true;
		this.textureCam.orthographicSize = 0.5f;
		this.textureCam.cullingMask = mask;
		this.textureCam.targetTexture = this.dest;
		this.textureCam.nearClipPlane = -5f;
		this.textureCam.farClipPlane = 5f;
		this.textureCam.useOcclusionCulling = false;
		this.textureCam.aspect = 1f;
		this.textureCam.rect = new Rect(0f, 0f, 1f, 1f);
		this.meshGO = new GameObject();
		this.meshGO.name = "mesh";
		this.meshGO.transform.parent = this.cameraGO.transform;
		this.meshGO.transform.SetLocalPosition(new Vector3(-0.5f, -0.5f, 0f));
		this.meshGO.isStatic = true;
		MeshRenderer meshRenderer = this.meshGO.AddComponent<MeshRenderer>();
		meshRenderer.receiveShadows = false;
		meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		meshRenderer.lightProbeUsage = LightProbeUsage.Off;
		meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
		this.meshGO.AddComponent<MeshFilter>().mesh = this.mesh;
		meshRenderer.sharedMaterial = this.Material;
		this.cameraGO.SetLayerRecursively(layer);
		TextureLerper.offsetCounter++;
	}

	// Token: 0x06007DB9 RID: 32185 RVA: 0x000F7553 File Offset: 0x000F5753
	public void LongUpdate(float dt)
	{
		this.BlendDt = dt;
		this.BlendTime = 0f;
	}

	// Token: 0x06007DBA RID: 32186 RVA: 0x003339D8 File Offset: 0x00331BD8
	public Texture Update()
	{
		float num = Time.deltaTime * this.Speed;
		if (Time.deltaTime == 0f)
		{
			num = Time.unscaledDeltaTime * this.Speed;
		}
		float value = Mathf.Min(num / Mathf.Max(this.BlendDt - this.BlendTime, 0f), 1f);
		this.BlendTime += num;
		if (GameUtil.IsCapturingTimeLapse())
		{
			value = 1f;
		}
		this.source = this.BlendTextures[this.BlendIdx];
		this.BlendIdx = (this.BlendIdx + 1) % 2;
		this.dest = this.BlendTextures[this.BlendIdx];
		Vector4 visibleCellRange = this.GetVisibleCellRange();
		visibleCellRange = new Vector4(0f, 0f, (float)Grid.WidthInCells, (float)Grid.HeightInCells);
		this.Material.SetFloat("_Lerp", value);
		this.Material.SetTexture("_SourceTex", this.source);
		this.Material.SetVector("_MeshParams", visibleCellRange);
		this.textureCam.targetTexture = this.dest;
		return this.dest;
	}

	// Token: 0x06007DBB RID: 32187 RVA: 0x00333AF4 File Offset: 0x00331CF4
	private Vector4 GetVisibleCellRange()
	{
		Camera main = Camera.main;
		float cellSizeInMeters = Grid.CellSizeInMeters;
		Ray ray = main.ViewportPointToRay(Vector3.zero);
		float distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		Vector3 vector = ray.GetPoint(distance);
		int cell = Grid.PosToCell(vector);
		float num = -Grid.HalfCellSizeInMeters;
		vector = Grid.CellToPos(cell, num, num, num);
		int num2 = Math.Max(0, (int)(vector.x / cellSizeInMeters));
		int num3 = Math.Max(0, (int)(vector.y / cellSizeInMeters));
		ray = main.ViewportPointToRay(Vector3.one);
		distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		vector = ray.GetPoint(distance);
		int num4 = Mathf.CeilToInt(vector.x / cellSizeInMeters);
		int num5 = Mathf.CeilToInt(vector.y / cellSizeInMeters);
		num4 = Mathf.Min(num4, Grid.WidthInCells - 1);
		num5 = Mathf.Min(num5, Grid.HeightInCells - 1);
		return new Vector4((float)num2, (float)num3, (float)num4, (float)num5);
	}

	// Token: 0x04005F70 RID: 24432
	private static int offsetCounter;

	// Token: 0x04005F71 RID: 24433
	public string name;

	// Token: 0x04005F72 RID: 24434
	private RenderTexture[] BlendTextures = new RenderTexture[2];

	// Token: 0x04005F73 RID: 24435
	private float BlendDt;

	// Token: 0x04005F74 RID: 24436
	private float BlendTime;

	// Token: 0x04005F75 RID: 24437
	private int BlendIdx;

	// Token: 0x04005F76 RID: 24438
	private Material Material;

	// Token: 0x04005F77 RID: 24439
	public float Speed = 1f;

	// Token: 0x04005F78 RID: 24440
	private Mesh mesh;

	// Token: 0x04005F79 RID: 24441
	private RenderTexture source;

	// Token: 0x04005F7A RID: 24442
	private RenderTexture dest;

	// Token: 0x04005F7B RID: 24443
	private GameObject meshGO;

	// Token: 0x04005F7C RID: 24444
	private GameObject cameraGO;

	// Token: 0x04005F7D RID: 24445
	private Camera textureCam;

	// Token: 0x04005F7E RID: 24446
	private float blend;
}
