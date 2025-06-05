using System;
using UnityEngine;

// Token: 0x02001AE2 RID: 6882
public class BuildingCellVisualizerResources : ScriptableObject
{
	// Token: 0x1700097B RID: 2427
	// (get) Token: 0x06008FD4 RID: 36820 RVA: 0x00102614 File Offset: 0x00100814
	public string heatSourceAnimFile
	{
		get
		{
			return "heat_fx_kanim";
		}
	}

	// Token: 0x1700097C RID: 2428
	// (get) Token: 0x06008FD5 RID: 36821 RVA: 0x0010261B File Offset: 0x0010081B
	public string heatAnimName
	{
		get
		{
			return "heatfx_a";
		}
	}

	// Token: 0x1700097D RID: 2429
	// (get) Token: 0x06008FD6 RID: 36822 RVA: 0x00102614 File Offset: 0x00100814
	public string heatSinkAnimFile
	{
		get
		{
			return "heat_fx_kanim";
		}
	}

	// Token: 0x1700097E RID: 2430
	// (get) Token: 0x06008FD7 RID: 36823 RVA: 0x00102622 File Offset: 0x00100822
	public string heatSinkAnimName
	{
		get
		{
			return "heatfx_b";
		}
	}

	// Token: 0x1700097F RID: 2431
	// (get) Token: 0x06008FD8 RID: 36824 RVA: 0x00102629 File Offset: 0x00100829
	// (set) Token: 0x06008FD9 RID: 36825 RVA: 0x00102631 File Offset: 0x00100831
	public Material backgroundMaterial { get; set; }

	// Token: 0x17000980 RID: 2432
	// (get) Token: 0x06008FDA RID: 36826 RVA: 0x0010263A File Offset: 0x0010083A
	// (set) Token: 0x06008FDB RID: 36827 RVA: 0x00102642 File Offset: 0x00100842
	public Material iconBackgroundMaterial { get; set; }

	// Token: 0x17000981 RID: 2433
	// (get) Token: 0x06008FDC RID: 36828 RVA: 0x0010264B File Offset: 0x0010084B
	// (set) Token: 0x06008FDD RID: 36829 RVA: 0x00102653 File Offset: 0x00100853
	public Material powerInputMaterial { get; set; }

	// Token: 0x17000982 RID: 2434
	// (get) Token: 0x06008FDE RID: 36830 RVA: 0x0010265C File Offset: 0x0010085C
	// (set) Token: 0x06008FDF RID: 36831 RVA: 0x00102664 File Offset: 0x00100864
	public Material powerOutputMaterial { get; set; }

	// Token: 0x17000983 RID: 2435
	// (get) Token: 0x06008FE0 RID: 36832 RVA: 0x0010266D File Offset: 0x0010086D
	// (set) Token: 0x06008FE1 RID: 36833 RVA: 0x00102675 File Offset: 0x00100875
	public Material liquidInputMaterial { get; set; }

	// Token: 0x17000984 RID: 2436
	// (get) Token: 0x06008FE2 RID: 36834 RVA: 0x0010267E File Offset: 0x0010087E
	// (set) Token: 0x06008FE3 RID: 36835 RVA: 0x00102686 File Offset: 0x00100886
	public Material liquidOutputMaterial { get; set; }

	// Token: 0x17000985 RID: 2437
	// (get) Token: 0x06008FE4 RID: 36836 RVA: 0x0010268F File Offset: 0x0010088F
	// (set) Token: 0x06008FE5 RID: 36837 RVA: 0x00102697 File Offset: 0x00100897
	public Material gasInputMaterial { get; set; }

	// Token: 0x17000986 RID: 2438
	// (get) Token: 0x06008FE6 RID: 36838 RVA: 0x001026A0 File Offset: 0x001008A0
	// (set) Token: 0x06008FE7 RID: 36839 RVA: 0x001026A8 File Offset: 0x001008A8
	public Material gasOutputMaterial { get; set; }

	// Token: 0x17000987 RID: 2439
	// (get) Token: 0x06008FE8 RID: 36840 RVA: 0x001026B1 File Offset: 0x001008B1
	// (set) Token: 0x06008FE9 RID: 36841 RVA: 0x001026B9 File Offset: 0x001008B9
	public Material highEnergyParticleInputMaterial { get; set; }

	// Token: 0x17000988 RID: 2440
	// (get) Token: 0x06008FEA RID: 36842 RVA: 0x001026C2 File Offset: 0x001008C2
	// (set) Token: 0x06008FEB RID: 36843 RVA: 0x001026CA File Offset: 0x001008CA
	public Material highEnergyParticleOutputMaterial { get; set; }

	// Token: 0x17000989 RID: 2441
	// (get) Token: 0x06008FEC RID: 36844 RVA: 0x001026D3 File Offset: 0x001008D3
	// (set) Token: 0x06008FED RID: 36845 RVA: 0x001026DB File Offset: 0x001008DB
	public Mesh backgroundMesh { get; set; }

	// Token: 0x1700098A RID: 2442
	// (get) Token: 0x06008FEE RID: 36846 RVA: 0x001026E4 File Offset: 0x001008E4
	// (set) Token: 0x06008FEF RID: 36847 RVA: 0x001026EC File Offset: 0x001008EC
	public Mesh iconMesh { get; set; }

	// Token: 0x1700098B RID: 2443
	// (get) Token: 0x06008FF0 RID: 36848 RVA: 0x001026F5 File Offset: 0x001008F5
	// (set) Token: 0x06008FF1 RID: 36849 RVA: 0x001026FD File Offset: 0x001008FD
	public int backgroundLayer { get; set; }

	// Token: 0x1700098C RID: 2444
	// (get) Token: 0x06008FF2 RID: 36850 RVA: 0x00102706 File Offset: 0x00100906
	// (set) Token: 0x06008FF3 RID: 36851 RVA: 0x0010270E File Offset: 0x0010090E
	public int iconLayer { get; set; }

	// Token: 0x06008FF4 RID: 36852 RVA: 0x00102717 File Offset: 0x00100917
	public static void DestroyInstance()
	{
		BuildingCellVisualizerResources._Instance = null;
	}

	// Token: 0x06008FF5 RID: 36853 RVA: 0x0010271F File Offset: 0x0010091F
	public static BuildingCellVisualizerResources Instance()
	{
		if (BuildingCellVisualizerResources._Instance == null)
		{
			BuildingCellVisualizerResources._Instance = Resources.Load<BuildingCellVisualizerResources>("BuildingCellVisualizerResources");
			BuildingCellVisualizerResources._Instance.Initialize();
		}
		return BuildingCellVisualizerResources._Instance;
	}

	// Token: 0x06008FF6 RID: 36854 RVA: 0x003855AC File Offset: 0x003837AC
	private void Initialize()
	{
		Shader shader = Shader.Find("Klei/BuildingCell");
		this.backgroundMaterial = new Material(shader);
		this.backgroundMaterial.mainTexture = GlobalResources.Instance().WhiteTexture;
		this.iconBackgroundMaterial = new Material(shader);
		this.iconBackgroundMaterial.mainTexture = GlobalResources.Instance().WhiteTexture;
		this.powerInputMaterial = new Material(shader);
		this.powerOutputMaterial = new Material(shader);
		this.liquidInputMaterial = new Material(shader);
		this.liquidOutputMaterial = new Material(shader);
		this.gasInputMaterial = new Material(shader);
		this.gasOutputMaterial = new Material(shader);
		this.highEnergyParticleInputMaterial = new Material(shader);
		this.highEnergyParticleOutputMaterial = new Material(shader);
		this.backgroundMesh = this.CreateMesh("BuildingCellVisualizer", Vector2.zero, 0.5f);
		float num = 0.5f;
		this.iconMesh = this.CreateMesh("BuildingCellVisualizerIcon", Vector2.zero, num * 0.5f);
		this.backgroundLayer = LayerMask.NameToLayer("Default");
		this.iconLayer = LayerMask.NameToLayer("Place");
	}

	// Token: 0x06008FF7 RID: 36855 RVA: 0x003856C4 File Offset: 0x003838C4
	private Mesh CreateMesh(string name, Vector2 base_offset, float half_size)
	{
		Mesh mesh = new Mesh();
		mesh.name = name;
		mesh.vertices = new Vector3[]
		{
			new Vector3(-half_size + base_offset.x, -half_size + base_offset.y, 0f),
			new Vector3(half_size + base_offset.x, -half_size + base_offset.y, 0f),
			new Vector3(-half_size + base_offset.x, half_size + base_offset.y, 0f),
			new Vector3(half_size + base_offset.x, half_size + base_offset.y, 0f)
		};
		mesh.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		mesh.triangles = new int[]
		{
			0,
			1,
			2,
			2,
			1,
			3
		};
		mesh.RecalculateBounds();
		return mesh;
	}

	// Token: 0x04006C92 RID: 27794
	[Header("Electricity")]
	public Color electricityInputColor;

	// Token: 0x04006C93 RID: 27795
	public Color electricityOutputColor;

	// Token: 0x04006C94 RID: 27796
	public Sprite electricityInputIcon;

	// Token: 0x04006C95 RID: 27797
	public Sprite electricityOutputIcon;

	// Token: 0x04006C96 RID: 27798
	public Sprite electricityConnectedIcon;

	// Token: 0x04006C97 RID: 27799
	public Sprite electricityBridgeIcon;

	// Token: 0x04006C98 RID: 27800
	public Sprite electricityBridgeConnectedIcon;

	// Token: 0x04006C99 RID: 27801
	public Sprite electricityArrowIcon;

	// Token: 0x04006C9A RID: 27802
	public Sprite switchIcon;

	// Token: 0x04006C9B RID: 27803
	public Color32 switchColor;

	// Token: 0x04006C9C RID: 27804
	public Color32 switchOffColor = Color.red;

	// Token: 0x04006C9D RID: 27805
	[Header("Gas")]
	public Sprite gasInputIcon;

	// Token: 0x04006C9E RID: 27806
	public Sprite gasOutputIcon;

	// Token: 0x04006C9F RID: 27807
	public BuildingCellVisualizerResources.IOColours gasIOColours;

	// Token: 0x04006CA0 RID: 27808
	[Header("Liquid")]
	public Sprite liquidInputIcon;

	// Token: 0x04006CA1 RID: 27809
	public Sprite liquidOutputIcon;

	// Token: 0x04006CA2 RID: 27810
	public BuildingCellVisualizerResources.IOColours liquidIOColours;

	// Token: 0x04006CA3 RID: 27811
	[Header("High Energy Particle")]
	public Sprite highEnergyParticleInputIcon;

	// Token: 0x04006CA4 RID: 27812
	public Sprite[] highEnergyParticleOutputIcons;

	// Token: 0x04006CA5 RID: 27813
	public Color highEnergyParticleInputColour;

	// Token: 0x04006CA6 RID: 27814
	public Color highEnergyParticleOutputColour;

	// Token: 0x04006CA7 RID: 27815
	[Header("Heat Sources and Sinks")]
	public Sprite heatSourceIcon;

	// Token: 0x04006CA8 RID: 27816
	public Sprite heatSinkIcon;

	// Token: 0x04006CA9 RID: 27817
	[Header("Alternate IO Colours")]
	public BuildingCellVisualizerResources.IOColours alternateIOColours;

	// Token: 0x04006CB8 RID: 27832
	private static BuildingCellVisualizerResources _Instance;

	// Token: 0x02001AE3 RID: 6883
	[Serializable]
	public struct ConnectedDisconnectedColours
	{
		// Token: 0x04006CB9 RID: 27833
		public Color32 connected;

		// Token: 0x04006CBA RID: 27834
		public Color32 disconnected;
	}

	// Token: 0x02001AE4 RID: 6884
	[Serializable]
	public struct IOColours
	{
		// Token: 0x04006CBB RID: 27835
		public BuildingCellVisualizerResources.ConnectedDisconnectedColours input;

		// Token: 0x04006CBC RID: 27836
		public BuildingCellVisualizerResources.ConnectedDisconnectedColours output;
	}
}
