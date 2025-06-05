using System;
using UnityEngine;

// Token: 0x02001A37 RID: 6711
[AddComponentMenu("KMonoBehaviour/scripts/TerrainBG")]
public class TerrainBG : KMonoBehaviour
{
	// Token: 0x06008BCE RID: 35790 RVA: 0x0036EFE0 File Offset: 0x0036D1E0
	protected override void OnSpawn()
	{
		this.layer = LayerMask.NameToLayer("Default");
		this.noiseVolume = this.CreateTexture3D(32);
		this.starsPlane = this.CreateStarsPlane("StarsPlane");
		this.northernLightsPlane = this.CreateNorthernLightsPlane("NorthernLightsPlane");
		this.worldPlane = this.CreateWorldPlane("WorldPlane");
		this.gasPlane = this.CreateGasPlane("GasPlane");
		this.propertyBlocks = new MaterialPropertyBlock[Lighting.Instance.Settings.BackgroundLayers];
		for (int i = 0; i < this.propertyBlocks.Length; i++)
		{
			this.propertyBlocks[i] = new MaterialPropertyBlock();
		}
	}

	// Token: 0x06008BCF RID: 35791 RVA: 0x0036F08C File Offset: 0x0036D28C
	private Texture3D CreateTexture3D(int size)
	{
		Color32[] array = new Color32[size * size * size];
		Texture3D texture3D = new Texture3D(size, size, size, TextureFormat.RGBA32, true);
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				for (int k = 0; k < size; k++)
				{
					Color32 color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255));
					array[i + j * size + k * size * size] = color;
				}
			}
		}
		texture3D.SetPixels32(array);
		texture3D.Apply();
		return texture3D;
	}

	// Token: 0x06008BD0 RID: 35792 RVA: 0x0036F130 File Offset: 0x0036D330
	public Mesh CreateGasPlane(string name)
	{
		Mesh mesh = new Mesh();
		mesh.name = name;
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		int[] triangles = new int[6];
		vertices = new Vector3[]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3((float)Grid.WidthInCells, 0f, 0f),
			new Vector3(0f, Grid.HeightInMeters, 0f),
			new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, 0f)
		};
		uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		triangles = new int[]
		{
			0,
			2,
			1,
			1,
			2,
			3
		};
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.bounds = new Bounds(new Vector3((float)Grid.WidthInCells * 0.5f, (float)Grid.HeightInCells * 0.5f, 0f), new Vector3((float)Grid.WidthInCells, (float)Grid.HeightInCells, 0f));
		return mesh;
	}

	// Token: 0x06008BD1 RID: 35793 RVA: 0x0036F2A0 File Offset: 0x0036D4A0
	public Mesh CreateWorldPlane(string name)
	{
		Mesh mesh = new Mesh();
		mesh.name = name;
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		int[] triangles = new int[6];
		vertices = new Vector3[]
		{
			new Vector3((float)(-(float)Grid.WidthInCells), (float)(-(float)Grid.HeightInCells), 0f),
			new Vector3((float)Grid.WidthInCells * 2f, (float)(-(float)Grid.HeightInCells), 0f),
			new Vector3((float)(-(float)Grid.WidthInCells), Grid.HeightInMeters * 2f, 0f),
			new Vector3(Grid.WidthInMeters * 2f, Grid.HeightInMeters * 2f, 0f)
		};
		uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		triangles = new int[]
		{
			0,
			2,
			1,
			1,
			2,
			3
		};
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.bounds = new Bounds(new Vector3((float)Grid.WidthInCells * 0.5f, (float)Grid.HeightInCells * 0.5f, 0f), new Vector3((float)Grid.WidthInCells, (float)Grid.HeightInCells, 0f));
		return mesh;
	}

	// Token: 0x06008BD2 RID: 35794 RVA: 0x0036F430 File Offset: 0x0036D630
	public Mesh CreateStarsPlane(string name)
	{
		Mesh mesh = new Mesh();
		mesh.name = name;
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		int[] triangles = new int[6];
		vertices = new Vector3[]
		{
			new Vector3((float)(-(float)Grid.WidthInCells), (float)(-(float)Grid.HeightInCells), 0f),
			new Vector3((float)Grid.WidthInCells * 2f, (float)(-(float)Grid.HeightInCells), 0f),
			new Vector3((float)(-(float)Grid.WidthInCells), Grid.HeightInMeters * 2f, 0f),
			new Vector3(Grid.WidthInMeters * 2f, Grid.HeightInMeters * 2f, 0f)
		};
		uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		triangles = new int[]
		{
			0,
			2,
			1,
			1,
			2,
			3
		};
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		Vector2 vector = new Vector2((float)Grid.WidthInCells, 2f * (float)Grid.HeightInCells);
		mesh.bounds = new Bounds(new Vector3(0.5f * vector.x, 0.5f * vector.y, 0f), new Vector3(vector.x, vector.y, 0f));
		return mesh;
	}

	// Token: 0x06008BD3 RID: 35795 RVA: 0x0036F5E0 File Offset: 0x0036D7E0
	public Mesh CreateNorthernLightsPlane(string name)
	{
		Mesh mesh = new Mesh();
		mesh.name = name;
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		int[] triangles = new int[6];
		float num2 = 1f;
		float num3 = this.northernLightSkySize * 0.5f;
		vertices = new Vector3[]
		{
			new Vector3(-num2, -num3, 0f),
			new Vector3(num2, -num3, 0f),
			new Vector3(-num2, num3, 0f),
			new Vector3(num2, num3, 0f)
		};
		uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		triangles = new int[]
		{
			0,
			2,
			1,
			1,
			2,
			3
		};
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		return mesh;
	}

	// Token: 0x06008BD4 RID: 35796 RVA: 0x0036F70C File Offset: 0x0036D90C
	private void LateUpdate()
	{
		if (!this.doDraw)
		{
			return;
		}
		Material material = this.starsMaterial_surface;
		if (ClusterManager.Instance.activeWorld.IsModuleInterior)
		{
			Clustercraft component = ClusterManager.Instance.activeWorld.GetComponent<Clustercraft>();
			if (component.Status != Clustercraft.CraftStatus.InFlight)
			{
				material = this.starsMaterial_surface;
			}
			else if (ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(component.Location, EntityLayer.Asteroid) != null)
			{
				material = this.starsMaterial_orbit;
			}
			else
			{
				material = this.starsMaterial_space;
			}
		}
		material.renderQueue = RenderQueues.Stars;
		material.SetTexture("_NoiseVolume", this.noiseVolume);
		Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.Background) + 1f);
		Graphics.DrawMesh(this.starsPlane, position, Quaternion.identity, material, this.layer);
		if (ClusterManager.Instance.activeWorld != null && ClusterManager.Instance.activeWorld.northernlights > 0)
		{
			Vector3 position2 = new Vector3(CameraController.Instance.transform.position.x, CameraController.Instance.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.Background) + 0.8f);
			Graphics.DrawMesh(this.northernLightsPlane, position2, Quaternion.identity, this.northernLightMaterial_ceres, this.layer);
		}
		this.backgroundMaterial.renderQueue = RenderQueues.Backwall;
		for (int i = 0; i < Lighting.Instance.Settings.BackgroundLayers; i++)
		{
			if (i >= Lighting.Instance.Settings.BackgroundLayers - 1)
			{
				float t = (float)i / (float)(Lighting.Instance.Settings.BackgroundLayers - 1);
				float x = Mathf.Lerp(1f, Lighting.Instance.Settings.BackgroundDarkening, t);
				float z = Mathf.Lerp(1f, Lighting.Instance.Settings.BackgroundUVScale, t);
				float w = 1f;
				if (i == Lighting.Instance.Settings.BackgroundLayers - 1)
				{
					w = 0f;
				}
				MaterialPropertyBlock materialPropertyBlock = this.propertyBlocks[i];
				materialPropertyBlock.SetVector("_BackWallParameters", new Vector4(x, Lighting.Instance.Settings.BackgroundClip, z, w));
				Vector3 position3 = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.Background));
				Graphics.DrawMesh(this.worldPlane, position3, Quaternion.identity, this.backgroundMaterial, this.layer, null, 0, materialPropertyBlock);
			}
		}
		this.gasMaterial.renderQueue = RenderQueues.Gas;
		Vector3 position4 = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.Gas));
		Graphics.DrawMesh(this.gasPlane, position4, Quaternion.identity, this.gasMaterial, this.layer);
		Vector3 position5 = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.GasFront));
		Graphics.DrawMesh(this.gasPlane, position5, Quaternion.identity, this.gasMaterial, this.layer);
	}

	// Token: 0x0400697E RID: 27006
	public Material northernLightMaterial_ceres;

	// Token: 0x0400697F RID: 27007
	public Material starsMaterial_surface;

	// Token: 0x04006980 RID: 27008
	public Material starsMaterial_orbit;

	// Token: 0x04006981 RID: 27009
	public Material starsMaterial_space;

	// Token: 0x04006982 RID: 27010
	public Material backgroundMaterial;

	// Token: 0x04006983 RID: 27011
	public Material gasMaterial;

	// Token: 0x04006984 RID: 27012
	public bool doDraw = true;

	// Token: 0x04006985 RID: 27013
	[SerializeField]
	private Texture3D noiseVolume;

	// Token: 0x04006986 RID: 27014
	private Mesh starsPlane;

	// Token: 0x04006987 RID: 27015
	private Mesh northernLightsPlane;

	// Token: 0x04006988 RID: 27016
	private Mesh worldPlane;

	// Token: 0x04006989 RID: 27017
	private Mesh gasPlane;

	// Token: 0x0400698A RID: 27018
	private int layer;

	// Token: 0x0400698B RID: 27019
	private float northernLightSkySize = 2f;

	// Token: 0x0400698C RID: 27020
	private MaterialPropertyBlock[] propertyBlocks;
}
