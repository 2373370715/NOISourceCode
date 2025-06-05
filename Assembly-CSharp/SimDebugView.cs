using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

// Token: 0x020018C4 RID: 6340
[AddComponentMenu("KMonoBehaviour/scripts/SimDebugView")]
public class SimDebugView : KMonoBehaviour
{
	// Token: 0x060082EF RID: 33519 RVA: 0x000FA965 File Offset: 0x000F8B65
	public static void DestroyInstance()
	{
		SimDebugView.Instance = null;
	}

	// Token: 0x060082F0 RID: 33520 RVA: 0x000FA96D File Offset: 0x000F8B6D
	protected override void OnPrefabInit()
	{
		SimDebugView.Instance = this;
		this.material = UnityEngine.Object.Instantiate<Material>(this.material);
		this.diseaseMaterial = UnityEngine.Object.Instantiate<Material>(this.diseaseMaterial);
	}

	// Token: 0x060082F1 RID: 33521 RVA: 0x0034BF10 File Offset: 0x0034A110
	protected override void OnSpawn()
	{
		SimDebugViewCompositor.Instance.material.SetColor("_Color0", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[0].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color1", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[1].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color2", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[2].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color3", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[3].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color4", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[4].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color5", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[5].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color6", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[6].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color7", GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[7].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color0", GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[0].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color1", GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[1].colorName));
		SimDebugViewCompositor.Instance.material.SetColor("_Color2", GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[2].colorName));
		this.SetMode(global::OverlayModes.None.ID);
	}

	// Token: 0x060082F2 RID: 33522 RVA: 0x0034C19C File Offset: 0x0034A39C
	public void OnReset()
	{
		this.plane = SimDebugView.CreatePlane("SimDebugView", base.transform);
		this.tex = SimDebugView.CreateTexture(out this.texBytes, Grid.WidthInCells, Grid.HeightInCells);
		this.plane.GetComponent<Renderer>().sharedMaterial = this.material;
		this.plane.GetComponent<Renderer>().sharedMaterial.mainTexture = this.tex;
		this.plane.transform.SetLocalPosition(new Vector3(0f, 0f, -6f));
		this.SetMode(global::OverlayModes.None.ID);
	}

	// Token: 0x060082F3 RID: 33523 RVA: 0x000FA997 File Offset: 0x000F8B97
	public static Texture2D CreateTexture(int width, int height)
	{
		return new Texture2D(width, height)
		{
			name = "SimDebugView",
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Point
		};
	}

	// Token: 0x060082F4 RID: 33524 RVA: 0x000FA9B9 File Offset: 0x000F8BB9
	public static Texture2D CreateTexture(out byte[] textureBytes, int width, int height)
	{
		textureBytes = new byte[width * height * 4];
		return new Texture2D(width, height, TextureUtil.TextureFormatToGraphicsFormat(TextureFormat.RGBA32), TextureCreationFlags.None)
		{
			name = "SimDebugView",
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Point
		};
	}

	// Token: 0x060082F5 RID: 33525 RVA: 0x0034C23C File Offset: 0x0034A43C
	public static Texture2D CreateTexture(int width, int height, Color col)
	{
		Color[] array = new Color[width * height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = col;
		}
		Texture2D texture2D = new Texture2D(width, height);
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x060082F6 RID: 33526 RVA: 0x0034C27C File Offset: 0x0034A47C
	public static GameObject CreatePlane(string layer, Transform parent)
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "overlayViewDisplayPlane";
		gameObject.SetLayerRecursively(LayerMask.NameToLayer(layer));
		gameObject.transform.SetParent(parent);
		gameObject.transform.SetPosition(Vector3.zero);
		gameObject.AddComponent<MeshRenderer>().reflectionProbeUsage = ReflectionProbeUsage.Off;
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		meshFilter.mesh = mesh;
		int num = 4;
		Vector3[] vertices = new Vector3[num];
		Vector2[] uv = new Vector2[num];
		int[] triangles = new int[6];
		float y = 2f * (float)Grid.HeightInCells;
		vertices = new Vector3[]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3((float)Grid.WidthInCells, 0f, 0f),
			new Vector3(0f, y, 0f),
			new Vector3(Grid.WidthInMeters, y, 0f)
		};
		uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 2f),
			new Vector2(1f, 2f)
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
		Vector2 vector = new Vector2((float)Grid.WidthInCells, y);
		mesh.bounds = new Bounds(new Vector3(0.5f * vector.x, 0.5f * vector.y, 0f), new Vector3(vector.x, vector.y, 0f));
		return gameObject;
	}

	// Token: 0x060082F7 RID: 33527 RVA: 0x0034C454 File Offset: 0x0034A654
	private void Update()
	{
		if (this.plane == null)
		{
			return;
		}
		bool flag = this.mode != global::OverlayModes.None.ID;
		this.plane.SetActive(flag);
		SimDebugViewCompositor.Instance.Toggle(flag && !GameUtil.IsCapturingTimeLapse());
		SimDebugViewCompositor.Instance.material.SetVector("_Thresholds0", new Vector4(0.1f, 0.2f, 0.3f, 0.4f));
		SimDebugViewCompositor.Instance.material.SetVector("_Thresholds1", new Vector4(0.5f, 0.6f, 0.7f, 0.8f));
		float x = 0f;
		if (this.mode == global::OverlayModes.ThermalConductivity.ID || this.mode == global::OverlayModes.Temperature.ID)
		{
			x = 1f;
		}
		SimDebugViewCompositor.Instance.material.SetVector("_ThresholdParameters", new Vector4(x, this.thresholdRange, this.thresholdOpacity, 0f));
		if (flag)
		{
			this.UpdateData(this.tex, this.texBytes, this.mode, 192);
		}
	}

	// Token: 0x060082F8 RID: 33528 RVA: 0x000FA9EE File Offset: 0x000F8BEE
	private static void SetDefaultBilinear(SimDebugView instance, Texture texture)
	{
		Renderer component = instance.plane.GetComponent<Renderer>();
		component.sharedMaterial = instance.material;
		component.sharedMaterial.mainTexture = instance.tex;
		texture.filterMode = FilterMode.Bilinear;
	}

	// Token: 0x060082F9 RID: 33529 RVA: 0x000FAA1E File Offset: 0x000F8C1E
	private static void SetDefaultPoint(SimDebugView instance, Texture texture)
	{
		Renderer component = instance.plane.GetComponent<Renderer>();
		component.sharedMaterial = instance.material;
		component.sharedMaterial.mainTexture = instance.tex;
		texture.filterMode = FilterMode.Point;
	}

	// Token: 0x060082FA RID: 33530 RVA: 0x000FAA4E File Offset: 0x000F8C4E
	private static void SetDisease(SimDebugView instance, Texture texture)
	{
		Renderer component = instance.plane.GetComponent<Renderer>();
		component.sharedMaterial = instance.diseaseMaterial;
		component.sharedMaterial.mainTexture = instance.tex;
		texture.filterMode = FilterMode.Bilinear;
	}

	// Token: 0x060082FB RID: 33531 RVA: 0x0034C57C File Offset: 0x0034A77C
	public void UpdateData(Texture2D texture, byte[] textureBytes, HashedString viewMode, byte alpha)
	{
		Action<SimDebugView, Texture> action;
		if (!this.dataUpdateFuncs.TryGetValue(viewMode, out action))
		{
			action = new Action<SimDebugView, Texture>(SimDebugView.SetDefaultPoint);
		}
		action(this, texture);
		int x;
		int num;
		int x2;
		int num2;
		Grid.GetVisibleExtents(out x, out num, out x2, out num2);
		this.selectedPathProber = null;
		KSelectable selected = SelectTool.Instance.selected;
		if (selected != null)
		{
			this.selectedPathProber = selected.GetComponent<PathProber>();
		}
		this.updateSimViewWorkItems.Reset(new SimDebugView.UpdateSimViewSharedData(this, this.texBytes, viewMode, this));
		int num3 = 16;
		for (int i = num; i <= num2; i += num3)
		{
			int y = Math.Min(i + num3 - 1, num2);
			this.updateSimViewWorkItems.Add(new SimDebugView.UpdateSimViewWorkItem(x, i, x2, y));
		}
		this.currentFrame = Time.frameCount;
		this.selectedCell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		GlobalJobManager.Run(this.updateSimViewWorkItems);
		texture.LoadRawTextureData(textureBytes);
		texture.Apply();
	}

	// Token: 0x060082FC RID: 33532 RVA: 0x000FAA7E File Offset: 0x000F8C7E
	public void SetGameGridMode(SimDebugView.GameGridMode mode)
	{
		this.gameGridMode = mode;
	}

	// Token: 0x060082FD RID: 33533 RVA: 0x000FAA87 File Offset: 0x000F8C87
	public SimDebugView.GameGridMode GetGameGridMode()
	{
		return this.gameGridMode;
	}

	// Token: 0x060082FE RID: 33534 RVA: 0x000FAA8F File Offset: 0x000F8C8F
	public void SetMode(HashedString mode)
	{
		this.mode = mode;
		Game.Instance.gameObject.Trigger(1798162660, mode);
	}

	// Token: 0x060082FF RID: 33535 RVA: 0x000FAAB2 File Offset: 0x000F8CB2
	public HashedString GetMode()
	{
		return this.mode;
	}

	// Token: 0x06008300 RID: 33536 RVA: 0x0034C678 File Offset: 0x0034A878
	public static Color TemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float num = Mathf.Clamp((temperature - minTempExpected) / (maxTempExpected - minTempExpected), 0f, 1f);
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, 1f, 1f);
	}

	// Token: 0x06008301 RID: 33537 RVA: 0x0034C6C4 File Offset: 0x0034A8C4
	public static Color LiquidTemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float value = (temperature - minTempExpected) / (maxTempExpected - minTempExpected);
		float num = Mathf.Clamp(value, 0.5f, 1f);
		float s = Mathf.Clamp(value, 0f, 1f);
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, s, 1f);
	}

	// Token: 0x06008302 RID: 33538 RVA: 0x0034C720 File Offset: 0x0034A920
	public static Color SolidTemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float num = Mathf.Clamp((temperature - minTempExpected) / (maxTempExpected - minTempExpected), 0.5f, 1f);
		float s = 1f;
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, s, 1f);
	}

	// Token: 0x06008303 RID: 33539 RVA: 0x0034C770 File Offset: 0x0034A970
	public static Color GasTemperatureToColor(float temperature, float minTempExpected, float maxTempExpected)
	{
		float num = Mathf.Clamp((temperature - minTempExpected) / (maxTempExpected - minTempExpected), 0f, 0.5f);
		float s = 1f;
		return Color.HSVToRGB((10f + (1f - num) * 171f) / 360f, s, 1f);
	}

	// Token: 0x06008304 RID: 33540 RVA: 0x0034C7C0 File Offset: 0x0034A9C0
	public Color NormalizedTemperature(float actualTemperature)
	{
		float num = this.user_temperatureThresholds[0];
		float num2 = this.user_temperatureThresholds[1];
		float num3 = num2 - num;
		if (actualTemperature < num)
		{
			return GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[0].colorName);
		}
		if (actualTemperature > num2)
		{
			return GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[this.temperatureThresholds.Length - 1].colorName);
		}
		int num4 = 0;
		float t = 0f;
		Game.TemperatureOverlayModes temperatureOverlayMode = Game.Instance.temperatureOverlayMode;
		if (temperatureOverlayMode != Game.TemperatureOverlayModes.AbsoluteTemperature)
		{
			if (temperatureOverlayMode == Game.TemperatureOverlayModes.RelativeTemperature)
			{
				float num5 = num;
				for (int i = 0; i < SimDebugView.relativeTemperatureColorIntervals.Length; i++)
				{
					if (actualTemperature < num5 + SimDebugView.relativeTemperatureColorIntervals[i] * num3)
					{
						num4 = i;
						break;
					}
					num5 += SimDebugView.relativeTemperatureColorIntervals[i] * num3;
				}
				t = (actualTemperature - num5) / (SimDebugView.relativeTemperatureColorIntervals[num4] * num3);
			}
		}
		else
		{
			float num6 = num;
			for (int j = 0; j < SimDebugView.absoluteTemperatureColorIntervals.Length; j++)
			{
				if (actualTemperature < num6 + SimDebugView.absoluteTemperatureColorIntervals[j])
				{
					num4 = j;
					break;
				}
				num6 += SimDebugView.absoluteTemperatureColorIntervals[j];
			}
			t = (actualTemperature - num6) / SimDebugView.absoluteTemperatureColorIntervals[num4];
		}
		return Color.Lerp(GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[num4].colorName), GlobalAssets.Instance.colorSet.GetColorByName(this.temperatureThresholds[num4 + 1].colorName), t);
	}

	// Token: 0x06008305 RID: 33541 RVA: 0x0034C954 File Offset: 0x0034AB54
	public Color NormalizedHeatFlow(int cell)
	{
		int num = 0;
		int num2 = 0;
		float thermalComfort = GameUtil.GetThermalComfort(GameTags.Minions.Models.Standard, cell, -DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_BASE_GENERATION_KILOWATTS);
		for (int i = 0; i < this.heatFlowThresholds.Length; i++)
		{
			if (thermalComfort <= this.heatFlowThresholds[i].value)
			{
				num2 = i;
				break;
			}
			num = i;
			num2 = i;
		}
		float num3 = 0f;
		if (num != num2)
		{
			num3 = (thermalComfort - this.heatFlowThresholds[num].value) / (this.heatFlowThresholds[num2].value - this.heatFlowThresholds[num].value);
		}
		num3 = Mathf.Max(num3, 0f);
		num3 = Mathf.Min(num3, 1f);
		Color result = Color.Lerp(GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[num].colorName), GlobalAssets.Instance.colorSet.GetColorByName(this.heatFlowThresholds[num2].colorName), num3);
		if (Grid.Solid[cell])
		{
			result = Color.black;
		}
		return result;
	}

	// Token: 0x06008306 RID: 33542 RVA: 0x000FAABA File Offset: 0x000F8CBA
	private static bool IsInsulated(int cell)
	{
		return (Grid.Element[cell].state & Element.State.TemperatureInsulated) > Element.State.Vacuum;
	}

	// Token: 0x06008307 RID: 33543 RVA: 0x0034CA7C File Offset: 0x0034AC7C
	private static Color GetDiseaseColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (Grid.DiseaseIdx[cell] != 255)
		{
			Disease disease = Db.Get().Diseases[(int)Grid.DiseaseIdx[cell]];
			result = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
			result.a = SimUtil.DiseaseCountToAlpha(Grid.DiseaseCount[cell]);
		}
		else
		{
			result.a = 0f;
		}
		return result;
	}

	// Token: 0x06008308 RID: 33544 RVA: 0x000FAACE File Offset: 0x000F8CCE
	private static Color GetHeatFlowColour(SimDebugView instance, int cell)
	{
		return instance.NormalizedHeatFlow(cell);
	}

	// Token: 0x06008309 RID: 33545 RVA: 0x000FAAD7 File Offset: 0x000F8CD7
	private static Color GetBlack(SimDebugView instance, int cell)
	{
		return Color.black;
	}

	// Token: 0x0600830A RID: 33546 RVA: 0x0034CB00 File Offset: 0x0034AD00
	public static Color GetLightColour(SimDebugView instance, int cell)
	{
		Color result = GlobalAssets.Instance.colorSet.lightOverlay;
		result.a = Mathf.Clamp(Mathf.Sqrt((float)(Grid.LightIntensity[cell] + LightGridManager.previewLux[cell])) / Mathf.Sqrt(80000f), 0f, 1f);
		if (Grid.LightIntensity[cell] > DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN)
		{
			float num = ((float)Grid.LightIntensity[cell] + (float)LightGridManager.previewLux[cell] - (float)DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN) / (float)(80000 - DUPLICANTSTATS.STANDARD.Light.LUX_SUNBURN);
			num /= 10f;
			result.r += Mathf.Min(0.1f, PerlinSimplexNoise.noise(Grid.CellToPos2D(cell).x / 8f, Grid.CellToPos2D(cell).y / 8f + (float)instance.currentFrame / 32f) * num);
		}
		return result;
	}

	// Token: 0x0600830B RID: 33547 RVA: 0x0034CC10 File Offset: 0x0034AE10
	public static Color GetRadiationColour(SimDebugView instance, int cell)
	{
		float a = Mathf.Clamp(Mathf.Sqrt(Grid.Radiation[cell]) / 30f, 0f, 1f);
		return new Color(0.2f, 0.9f, 0.3f, a);
	}

	// Token: 0x0600830C RID: 33548 RVA: 0x0034CC58 File Offset: 0x0034AE58
	public static Color GetRoomsColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (Grid.IsValidCell(instance.selectedCell))
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			if (cavityForCell != null && cavityForCell.room != null)
			{
				Room room = cavityForCell.room;
				result = GlobalAssets.Instance.colorSet.GetColorByName(room.roomType.category.colorName);
				result.a = 0.45f;
				if (Game.Instance.roomProber.GetCavityForCell(instance.selectedCell) == cavityForCell)
				{
					result.a += 0.3f;
				}
			}
		}
		return result;
	}

	// Token: 0x0600830D RID: 33549 RVA: 0x0034CCF8 File Offset: 0x0034AEF8
	public static Color GetJoulesColour(SimDebugView instance, int cell)
	{
		float num = Grid.Element[cell].specificHeatCapacity * Grid.Temperature[cell] * (Grid.Mass[cell] * 1000f);
		float t = 0.5f * num / (ElementLoader.FindElementByHash(SimHashes.SandStone).specificHeatCapacity * 294f * 1000000f);
		return Color.Lerp(Color.black, Color.red, t);
	}

	// Token: 0x0600830E RID: 33550 RVA: 0x0034CD64 File Offset: 0x0034AF64
	public static Color GetNormalizedTemperatureColourMode(SimDebugView instance, int cell)
	{
		switch (Game.Instance.temperatureOverlayMode)
		{
		case Game.TemperatureOverlayModes.AbsoluteTemperature:
			return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
		case Game.TemperatureOverlayModes.AdaptiveTemperature:
			return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
		case Game.TemperatureOverlayModes.HeatFlow:
			return SimDebugView.GetHeatFlowColour(instance, cell);
		case Game.TemperatureOverlayModes.StateChange:
			return SimDebugView.GetStateChangeProximityColour(instance, cell);
		default:
			return SimDebugView.GetNormalizedTemperatureColour(instance, cell);
		}
	}

	// Token: 0x0600830F RID: 33551 RVA: 0x0034CDBC File Offset: 0x0034AFBC
	public static Color GetStateChangeProximityColour(SimDebugView instance, int cell)
	{
		float temperature = Grid.Temperature[cell];
		Element element = Grid.Element[cell];
		float num = element.lowTemp;
		float num2 = element.highTemp;
		if (element.IsGas)
		{
			num2 = Mathf.Min(num + 150f, num2);
			return SimDebugView.GasTemperatureToColor(temperature, num, num2);
		}
		if (element.IsSolid)
		{
			num = Mathf.Max(num2 - 150f, num);
			return SimDebugView.SolidTemperatureToColor(temperature, num, num2);
		}
		return SimDebugView.TemperatureToColor(temperature, num, num2);
	}

	// Token: 0x06008310 RID: 33552 RVA: 0x0034CE34 File Offset: 0x0034B034
	public static Color GetNormalizedTemperatureColour(SimDebugView instance, int cell)
	{
		float actualTemperature = Grid.Temperature[cell];
		return instance.NormalizedTemperature(actualTemperature);
	}

	// Token: 0x06008311 RID: 33553 RVA: 0x0034CE54 File Offset: 0x0034B054
	private static Color GetGameGridColour(SimDebugView instance, int cell)
	{
		Color result = new Color32(0, 0, 0, byte.MaxValue);
		switch (instance.gameGridMode)
		{
		case SimDebugView.GameGridMode.GameSolidMap:
			result = (Grid.Solid[cell] ? Color.white : Color.black);
			break;
		case SimDebugView.GameGridMode.Lighting:
			result = ((Grid.LightCount[cell] > 0 || LightGridManager.previewLux[cell] > 0) ? Color.white : Color.black);
			break;
		case SimDebugView.GameGridMode.DigAmount:
			if (Grid.Element[cell].IsSolid)
			{
				float num = Grid.Damage[cell] / 255f;
				result = Color.HSVToRGB(1f - num, 1f, 1f);
			}
			break;
		case SimDebugView.GameGridMode.DupePassable:
			result = (Grid.DupePassable[cell] ? Color.white : Color.black);
			break;
		}
		return result;
	}

	// Token: 0x06008312 RID: 33554 RVA: 0x000FAADE File Offset: 0x000F8CDE
	public Color32 GetColourForID(int id)
	{
		return this.networkColours[id % this.networkColours.Length];
	}

	// Token: 0x06008313 RID: 33555 RVA: 0x0034CF38 File Offset: 0x0034B138
	private static Color GetThermalConductivityColour(SimDebugView instance, int cell)
	{
		bool flag = SimDebugView.IsInsulated(cell);
		Color black = Color.black;
		float num = instance.maxThermalConductivity - instance.minThermalConductivity;
		if (!flag && num != 0f)
		{
			float num2 = (Grid.Element[cell].thermalConductivity - instance.minThermalConductivity) / num;
			num2 = Mathf.Max(num2, 0f);
			num2 = Mathf.Min(num2, 1f);
			black = new Color(num2, num2, num2);
		}
		return black;
	}

	// Token: 0x06008314 RID: 33556 RVA: 0x0034CFA4 File Offset: 0x0034B1A4
	private static Color GetPressureMapColour(SimDebugView instance, int cell)
	{
		Color32 c = Color.black;
		if (Grid.Pressure[cell] > 0f)
		{
			float num = Mathf.Clamp((Grid.Pressure[cell] - instance.minPressureExpected) / (instance.maxPressureExpected - instance.minPressureExpected), 0f, 1f) * 0.9f;
			c = new Color(num, num, num, 1f);
		}
		return c;
	}

	// Token: 0x06008315 RID: 33557 RVA: 0x0034D01C File Offset: 0x0034B21C
	private static Color GetOxygenMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (!Grid.IsLiquid(cell) && !Grid.Solid[cell])
		{
			if (Grid.Mass[cell] > SimDebugView.minimumBreathable && (Grid.Element[cell].id == SimHashes.Oxygen || Grid.Element[cell].id == SimHashes.ContaminatedOxygen))
			{
				float time = Mathf.Clamp((Grid.Mass[cell] - SimDebugView.minimumBreathable) / SimDebugView.optimallyBreathable, 0f, 1f);
				result = instance.breathableGradient.Evaluate(time);
			}
			else
			{
				result = instance.unbreathableColour;
			}
		}
		return result;
	}

	// Token: 0x06008316 RID: 33558 RVA: 0x0034D0C4 File Offset: 0x0034B2C4
	private static Color GetTileColour(SimDebugView instance, int cell)
	{
		float num = 0.33f;
		Color result = new Color(num, num, num);
		Element element = Grid.Element[cell];
		bool flag = false;
		foreach (Tag search_tag in Game.Instance.tileOverlayFilters)
		{
			if (element.HasTag(search_tag))
			{
				flag = true;
			}
		}
		if (flag)
		{
			result = element.substance.uiColour;
		}
		return result;
	}

	// Token: 0x06008317 RID: 33559 RVA: 0x000FAAF5 File Offset: 0x000F8CF5
	private static Color GetTileTypeColour(SimDebugView instance, int cell)
	{
		return Grid.Element[cell].substance.uiColour;
	}

	// Token: 0x06008318 RID: 33560 RVA: 0x0034D154 File Offset: 0x0034B354
	private static Color GetStateMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		switch (Grid.Element[cell].state & Element.State.Solid)
		{
		case Element.State.Gas:
			result = Color.yellow;
			break;
		case Element.State.Liquid:
			result = Color.green;
			break;
		case Element.State.Solid:
			result = Color.blue;
			break;
		}
		return result;
	}

	// Token: 0x06008319 RID: 33561 RVA: 0x0034D1A8 File Offset: 0x0034B3A8
	private static Color GetSolidLiquidMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		switch (Grid.Element[cell].state & Element.State.Solid)
		{
		case Element.State.Liquid:
			result = Color.green;
			break;
		case Element.State.Solid:
			result = Color.blue;
			break;
		}
		return result;
	}

	// Token: 0x0600831A RID: 33562 RVA: 0x0034D1F4 File Offset: 0x0034B3F4
	private static Color GetStateChangeColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		Element element = Grid.Element[cell];
		if (!element.IsVacuum)
		{
			float num = Grid.Temperature[cell];
			float num2 = element.lowTemp * 0.05f;
			float a = Mathf.Abs(num - element.lowTemp) / num2;
			float num3 = element.highTemp * 0.05f;
			float b = Mathf.Abs(num - element.highTemp) / num3;
			float t = Mathf.Max(0f, 1f - Mathf.Min(a, b));
			result = Color.Lerp(Color.black, Color.red, t);
		}
		return result;
	}

	// Token: 0x0600831B RID: 33563 RVA: 0x0034D28C File Offset: 0x0034B48C
	private static Color GetDecorColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (!Grid.Solid[cell])
		{
			float num = GameUtil.GetDecorAtCell(cell) / 100f;
			if (num > 0f)
			{
				result = Color.Lerp(GlobalAssets.Instance.colorSet.decorBaseline, GlobalAssets.Instance.colorSet.decorPositive, Mathf.Abs(num));
			}
			else
			{
				result = Color.Lerp(GlobalAssets.Instance.colorSet.decorBaseline, GlobalAssets.Instance.colorSet.decorNegative, Mathf.Abs(num));
			}
		}
		return result;
	}

	// Token: 0x0600831C RID: 33564 RVA: 0x0034D32C File Offset: 0x0034B52C
	private static Color GetDangerColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		SimDebugView.DangerAmount dangerAmount = SimDebugView.DangerAmount.None;
		if (!Grid.Element[cell].IsSolid)
		{
			float num = 0f;
			if (Grid.Temperature[cell] < SimDebugView.minMinionTemperature)
			{
				num = Mathf.Abs(Grid.Temperature[cell] - SimDebugView.minMinionTemperature);
			}
			if (Grid.Temperature[cell] > SimDebugView.maxMinionTemperature)
			{
				num = Mathf.Abs(Grid.Temperature[cell] - SimDebugView.maxMinionTemperature);
			}
			if (num > 0f)
			{
				if (num < 10f)
				{
					dangerAmount = SimDebugView.DangerAmount.VeryLow;
				}
				else if (num < 30f)
				{
					dangerAmount = SimDebugView.DangerAmount.Low;
				}
				else if (num < 100f)
				{
					dangerAmount = SimDebugView.DangerAmount.Moderate;
				}
				else if (num < 200f)
				{
					dangerAmount = SimDebugView.DangerAmount.High;
				}
				else if (num < 400f)
				{
					dangerAmount = SimDebugView.DangerAmount.VeryHigh;
				}
				else if (num > 800f)
				{
					dangerAmount = SimDebugView.DangerAmount.Extreme;
				}
			}
		}
		if (dangerAmount < SimDebugView.DangerAmount.VeryHigh && (Grid.Element[cell].IsVacuum || (Grid.Element[cell].IsGas && (Grid.Element[cell].id != SimHashes.Oxygen || Grid.Pressure[cell] < SimDebugView.minMinionPressure))))
		{
			dangerAmount++;
		}
		if (dangerAmount != SimDebugView.DangerAmount.None)
		{
			float num2 = (float)dangerAmount / 6f;
			result = Color.HSVToRGB((80f - num2 * 80f) / 360f, 1f, 1f);
		}
		return result;
	}

	// Token: 0x0600831D RID: 33565 RVA: 0x0034D474 File Offset: 0x0034B674
	private static Color GetSimCheckErrorMapColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		Element element = Grid.Element[cell];
		float num = Grid.Mass[cell];
		float num2 = Grid.Temperature[cell];
		if (float.IsNaN(num) || float.IsNaN(num2) || num > 10000f || num2 > 10000f)
		{
			return Color.red;
		}
		if (element.IsVacuum)
		{
			if (num2 != 0f)
			{
				result = Color.yellow;
			}
			else if (num != 0f)
			{
				result = Color.blue;
			}
			else
			{
				result = Color.gray;
			}
		}
		else if (num2 < 10f)
		{
			result = Color.red;
		}
		else if (Grid.Mass[cell] < 1f && Grid.Pressure[cell] < 1f)
		{
			result = Color.green;
		}
		else if (num2 > element.highTemp + 3f && element.highTempTransition != null)
		{
			result = Color.magenta;
		}
		else if (num2 < element.lowTemp + 3f && element.lowTempTransition != null)
		{
			result = Color.cyan;
		}
		return result;
	}

	// Token: 0x0600831E RID: 33566 RVA: 0x000FAB0D File Offset: 0x000F8D0D
	private static Color GetFakeFloorColour(SimDebugView instance, int cell)
	{
		if (!Grid.FakeFloor[cell])
		{
			return Color.black;
		}
		return Color.cyan;
	}

	// Token: 0x0600831F RID: 33567 RVA: 0x000FAB27 File Offset: 0x000F8D27
	private static Color GetFoundationColour(SimDebugView instance, int cell)
	{
		if (!Grid.Foundation[cell])
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06008320 RID: 33568 RVA: 0x000FAB41 File Offset: 0x000F8D41
	private static Color GetDupePassableColour(SimDebugView instance, int cell)
	{
		if (!Grid.DupePassable[cell])
		{
			return Color.black;
		}
		return Color.green;
	}

	// Token: 0x06008321 RID: 33569 RVA: 0x000FAB5B File Offset: 0x000F8D5B
	private static Color GetCritterImpassableColour(SimDebugView instance, int cell)
	{
		if (!Grid.CritterImpassable[cell])
		{
			return Color.black;
		}
		return Color.yellow;
	}

	// Token: 0x06008322 RID: 33570 RVA: 0x000FAB75 File Offset: 0x000F8D75
	private static Color GetDupeImpassableColour(SimDebugView instance, int cell)
	{
		if (!Grid.DupeImpassable[cell])
		{
			return Color.black;
		}
		return Color.red;
	}

	// Token: 0x06008323 RID: 33571 RVA: 0x000FAB8F File Offset: 0x000F8D8F
	private static Color GetMinionOccupiedColour(SimDebugView instance, int cell)
	{
		if (!(Grid.Objects[cell, 0] != null))
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06008324 RID: 33572 RVA: 0x000FABB0 File Offset: 0x000F8DB0
	private static Color GetMinionGroupProberColour(SimDebugView instance, int cell)
	{
		if (!MinionGroupProber.Get().IsReachable(cell))
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06008325 RID: 33573 RVA: 0x000FABCA File Offset: 0x000F8DCA
	private static Color GetPathProberColour(SimDebugView instance, int cell)
	{
		if (!(instance.selectedPathProber != null) || instance.selectedPathProber.GetCost(cell) == -1)
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06008326 RID: 33574 RVA: 0x000FABF4 File Offset: 0x000F8DF4
	private static Color GetReservedColour(SimDebugView instance, int cell)
	{
		if (!Grid.Reserved[cell])
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06008327 RID: 33575 RVA: 0x000FAC0E File Offset: 0x000F8E0E
	private static Color GetAllowPathFindingColour(SimDebugView instance, int cell)
	{
		if (!Grid.AllowPathfinding[cell])
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x06008328 RID: 33576 RVA: 0x0034D57C File Offset: 0x0034B77C
	private static Color GetMassColour(SimDebugView instance, int cell)
	{
		Color result = Color.black;
		if (!SimDebugView.IsInsulated(cell))
		{
			float num = Grid.Mass[cell];
			if (num > 0f)
			{
				float num2 = (num - SimDebugView.Instance.minMassExpected) / (SimDebugView.Instance.maxMassExpected - SimDebugView.Instance.minMassExpected);
				result = Color.HSVToRGB(1f - num2, 1f, 1f);
			}
		}
		return result;
	}

	// Token: 0x06008329 RID: 33577 RVA: 0x000FAC28 File Offset: 0x000F8E28
	public static Color GetScenePartitionerColour(SimDebugView instance, int cell)
	{
		if (!GameScenePartitioner.Instance.DoDebugLayersContainItemsOnCell(cell))
		{
			return Color.black;
		}
		return Color.white;
	}

	// Token: 0x04006396 RID: 25494
	[SerializeField]
	public Material material;

	// Token: 0x04006397 RID: 25495
	public Material diseaseMaterial;

	// Token: 0x04006398 RID: 25496
	public bool hideFOW;

	// Token: 0x04006399 RID: 25497
	public const int colourSize = 4;

	// Token: 0x0400639A RID: 25498
	private byte[] texBytes;

	// Token: 0x0400639B RID: 25499
	private int currentFrame;

	// Token: 0x0400639C RID: 25500
	[SerializeField]
	private Texture2D tex;

	// Token: 0x0400639D RID: 25501
	[SerializeField]
	private GameObject plane;

	// Token: 0x0400639E RID: 25502
	private HashedString mode = global::OverlayModes.Power.ID;

	// Token: 0x0400639F RID: 25503
	private SimDebugView.GameGridMode gameGridMode = SimDebugView.GameGridMode.DigAmount;

	// Token: 0x040063A0 RID: 25504
	private PathProber selectedPathProber;

	// Token: 0x040063A1 RID: 25505
	public float minTempExpected = 173.15f;

	// Token: 0x040063A2 RID: 25506
	public float maxTempExpected = 423.15f;

	// Token: 0x040063A3 RID: 25507
	public float minMassExpected = 1.0001f;

	// Token: 0x040063A4 RID: 25508
	public float maxMassExpected = 10000f;

	// Token: 0x040063A5 RID: 25509
	public float minPressureExpected = 1.300003f;

	// Token: 0x040063A6 RID: 25510
	public float maxPressureExpected = 201.3f;

	// Token: 0x040063A7 RID: 25511
	public float minThermalConductivity;

	// Token: 0x040063A8 RID: 25512
	public float maxThermalConductivity = 30f;

	// Token: 0x040063A9 RID: 25513
	public float thresholdRange = 0.001f;

	// Token: 0x040063AA RID: 25514
	public float thresholdOpacity = 0.8f;

	// Token: 0x040063AB RID: 25515
	public static float minimumBreathable = 0.05f;

	// Token: 0x040063AC RID: 25516
	public static float optimallyBreathable = 1f;

	// Token: 0x040063AD RID: 25517
	public SimDebugView.ColorThreshold[] temperatureThresholds;

	// Token: 0x040063AE RID: 25518
	public Vector2 user_temperatureThresholds = Vector2.zero;

	// Token: 0x040063AF RID: 25519
	public SimDebugView.ColorThreshold[] heatFlowThresholds;

	// Token: 0x040063B0 RID: 25520
	public Color32[] networkColours;

	// Token: 0x040063B1 RID: 25521
	public Gradient breathableGradient = new Gradient();

	// Token: 0x040063B2 RID: 25522
	public Color32 unbreathableColour = new Color(0.5f, 0f, 0f);

	// Token: 0x040063B3 RID: 25523
	public Color32[] toxicColour = new Color32[]
	{
		new Color(0.5f, 0f, 0.5f),
		new Color(1f, 0f, 1f)
	};

	// Token: 0x040063B4 RID: 25524
	public static SimDebugView Instance;

	// Token: 0x040063B5 RID: 25525
	private WorkItemCollection<SimDebugView.UpdateSimViewWorkItem, SimDebugView.UpdateSimViewSharedData> updateSimViewWorkItems = new WorkItemCollection<SimDebugView.UpdateSimViewWorkItem, SimDebugView.UpdateSimViewSharedData>();

	// Token: 0x040063B6 RID: 25526
	private int selectedCell;

	// Token: 0x040063B7 RID: 25527
	private Dictionary<HashedString, Action<SimDebugView, Texture>> dataUpdateFuncs = new Dictionary<HashedString, Action<SimDebugView, Texture>>
	{
		{
			global::OverlayModes.Temperature.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
		},
		{
			global::OverlayModes.Oxygen.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
		},
		{
			global::OverlayModes.Decor.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultBilinear)
		},
		{
			global::OverlayModes.TileMode.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDefaultPoint)
		},
		{
			global::OverlayModes.Disease.ID,
			new Action<SimDebugView, Texture>(SimDebugView.SetDisease)
		}
	};

	// Token: 0x040063B8 RID: 25528
	private static float[] relativeTemperatureColorIntervals = new float[]
	{
		0.4f,
		0.05f,
		0.05f,
		0.05f,
		0.05f,
		0.2f,
		0.2f
	};

	// Token: 0x040063B9 RID: 25529
	private static float[] absoluteTemperatureColorIntervals = new float[]
	{
		273.15f,
		10f,
		10f,
		10f,
		7f,
		63f,
		1700f,
		10000f
	};

	// Token: 0x040063BA RID: 25530
	private Dictionary<HashedString, Func<SimDebugView, int, Color>> getColourFuncs = new Dictionary<HashedString, Func<SimDebugView, int, Color>>
	{
		{
			global::OverlayModes.ThermalConductivity.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetThermalConductivityColour)
		},
		{
			global::OverlayModes.Temperature.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetNormalizedTemperatureColourMode)
		},
		{
			global::OverlayModes.Disease.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDiseaseColour)
		},
		{
			global::OverlayModes.Decor.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDecorColour)
		},
		{
			global::OverlayModes.Oxygen.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetOxygenMapColour)
		},
		{
			global::OverlayModes.Light.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetLightColour)
		},
		{
			global::OverlayModes.Radiation.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetRadiationColour)
		},
		{
			global::OverlayModes.Rooms.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetRoomsColour)
		},
		{
			global::OverlayModes.TileMode.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetTileColour)
		},
		{
			global::OverlayModes.Suit.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			global::OverlayModes.Priorities.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			global::OverlayModes.Crop.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			global::OverlayModes.Harvest.ID,
			new Func<SimDebugView, int, Color>(SimDebugView.GetBlack)
		},
		{
			SimDebugView.OverlayModes.GameGrid,
			new Func<SimDebugView, int, Color>(SimDebugView.GetGameGridColour)
		},
		{
			SimDebugView.OverlayModes.StateChange,
			new Func<SimDebugView, int, Color>(SimDebugView.GetStateChangeColour)
		},
		{
			SimDebugView.OverlayModes.SimCheckErrorMap,
			new Func<SimDebugView, int, Color>(SimDebugView.GetSimCheckErrorMapColour)
		},
		{
			SimDebugView.OverlayModes.Foundation,
			new Func<SimDebugView, int, Color>(SimDebugView.GetFoundationColour)
		},
		{
			SimDebugView.OverlayModes.FakeFloor,
			new Func<SimDebugView, int, Color>(SimDebugView.GetFakeFloorColour)
		},
		{
			SimDebugView.OverlayModes.DupePassable,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDupePassableColour)
		},
		{
			SimDebugView.OverlayModes.DupeImpassable,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDupeImpassableColour)
		},
		{
			SimDebugView.OverlayModes.CritterImpassable,
			new Func<SimDebugView, int, Color>(SimDebugView.GetCritterImpassableColour)
		},
		{
			SimDebugView.OverlayModes.MinionGroupProber,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMinionGroupProberColour)
		},
		{
			SimDebugView.OverlayModes.PathProber,
			new Func<SimDebugView, int, Color>(SimDebugView.GetPathProberColour)
		},
		{
			SimDebugView.OverlayModes.Reserved,
			new Func<SimDebugView, int, Color>(SimDebugView.GetReservedColour)
		},
		{
			SimDebugView.OverlayModes.AllowPathFinding,
			new Func<SimDebugView, int, Color>(SimDebugView.GetAllowPathFindingColour)
		},
		{
			SimDebugView.OverlayModes.Danger,
			new Func<SimDebugView, int, Color>(SimDebugView.GetDangerColour)
		},
		{
			SimDebugView.OverlayModes.MinionOccupied,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMinionOccupiedColour)
		},
		{
			SimDebugView.OverlayModes.Pressure,
			new Func<SimDebugView, int, Color>(SimDebugView.GetPressureMapColour)
		},
		{
			SimDebugView.OverlayModes.TileType,
			new Func<SimDebugView, int, Color>(SimDebugView.GetTileTypeColour)
		},
		{
			SimDebugView.OverlayModes.State,
			new Func<SimDebugView, int, Color>(SimDebugView.GetStateMapColour)
		},
		{
			SimDebugView.OverlayModes.SolidLiquid,
			new Func<SimDebugView, int, Color>(SimDebugView.GetSolidLiquidMapColour)
		},
		{
			SimDebugView.OverlayModes.Mass,
			new Func<SimDebugView, int, Color>(SimDebugView.GetMassColour)
		},
		{
			SimDebugView.OverlayModes.Joules,
			new Func<SimDebugView, int, Color>(SimDebugView.GetJoulesColour)
		},
		{
			SimDebugView.OverlayModes.ScenePartitioner,
			new Func<SimDebugView, int, Color>(SimDebugView.GetScenePartitionerColour)
		}
	};

	// Token: 0x040063BB RID: 25531
	public static readonly Color[] dbColours = new Color[]
	{
		new Color(0f, 0f, 0f, 0f),
		new Color(1f, 1f, 1f, 0.3f),
		new Color(0.7058824f, 0.8235294f, 1f, 0.2f),
		new Color(0f, 0.3137255f, 1f, 0.3f),
		new Color(0.7058824f, 1f, 0.7058824f, 0.5f),
		new Color(0.078431375f, 1f, 0f, 0.7f),
		new Color(1f, 0.9019608f, 0.7058824f, 0.9f),
		new Color(1f, 0.8235294f, 0f, 0.9f),
		new Color(1f, 0.7176471f, 0.3019608f, 0.9f),
		new Color(1f, 0.41568628f, 0f, 0.9f),
		new Color(1f, 0.7058824f, 0.7058824f, 1f),
		new Color(1f, 0f, 0f, 1f),
		new Color(1f, 0f, 0f, 1f)
	};

	// Token: 0x040063BC RID: 25532
	private static float minMinionTemperature = 260f;

	// Token: 0x040063BD RID: 25533
	private static float maxMinionTemperature = 310f;

	// Token: 0x040063BE RID: 25534
	private static float minMinionPressure = 80f;

	// Token: 0x020018C5 RID: 6341
	public static class OverlayModes
	{
		// Token: 0x040063BF RID: 25535
		public static readonly HashedString Mass = "Mass";

		// Token: 0x040063C0 RID: 25536
		public static readonly HashedString Pressure = "Pressure";

		// Token: 0x040063C1 RID: 25537
		public static readonly HashedString GameGrid = "GameGrid";

		// Token: 0x040063C2 RID: 25538
		public static readonly HashedString ScenePartitioner = "ScenePartitioner";

		// Token: 0x040063C3 RID: 25539
		public static readonly HashedString ConduitUpdates = "ConduitUpdates";

		// Token: 0x040063C4 RID: 25540
		public static readonly HashedString Flow = "Flow";

		// Token: 0x040063C5 RID: 25541
		public static readonly HashedString StateChange = "StateChange";

		// Token: 0x040063C6 RID: 25542
		public static readonly HashedString SimCheckErrorMap = "SimCheckErrorMap";

		// Token: 0x040063C7 RID: 25543
		public static readonly HashedString DupePassable = "DupePassable";

		// Token: 0x040063C8 RID: 25544
		public static readonly HashedString Foundation = "Foundation";

		// Token: 0x040063C9 RID: 25545
		public static readonly HashedString FakeFloor = "FakeFloor";

		// Token: 0x040063CA RID: 25546
		public static readonly HashedString CritterImpassable = "CritterImpassable";

		// Token: 0x040063CB RID: 25547
		public static readonly HashedString DupeImpassable = "DupeImpassable";

		// Token: 0x040063CC RID: 25548
		public static readonly HashedString MinionGroupProber = "MinionGroupProber";

		// Token: 0x040063CD RID: 25549
		public static readonly HashedString PathProber = "PathProber";

		// Token: 0x040063CE RID: 25550
		public static readonly HashedString Reserved = "Reserved";

		// Token: 0x040063CF RID: 25551
		public static readonly HashedString AllowPathFinding = "AllowPathFinding";

		// Token: 0x040063D0 RID: 25552
		public static readonly HashedString Danger = "Danger";

		// Token: 0x040063D1 RID: 25553
		public static readonly HashedString MinionOccupied = "MinionOccupied";

		// Token: 0x040063D2 RID: 25554
		public static readonly HashedString TileType = "TileType";

		// Token: 0x040063D3 RID: 25555
		public static readonly HashedString State = "State";

		// Token: 0x040063D4 RID: 25556
		public static readonly HashedString SolidLiquid = "SolidLiquid";

		// Token: 0x040063D5 RID: 25557
		public static readonly HashedString Joules = "Joules";
	}

	// Token: 0x020018C6 RID: 6342
	public enum GameGridMode
	{
		// Token: 0x040063D7 RID: 25559
		GameSolidMap,
		// Token: 0x040063D8 RID: 25560
		Lighting,
		// Token: 0x040063D9 RID: 25561
		RoomMap,
		// Token: 0x040063DA RID: 25562
		Style,
		// Token: 0x040063DB RID: 25563
		PlantDensity,
		// Token: 0x040063DC RID: 25564
		DigAmount,
		// Token: 0x040063DD RID: 25565
		DupePassable
	}

	// Token: 0x020018C7 RID: 6343
	[Serializable]
	public struct ColorThreshold
	{
		// Token: 0x040063DE RID: 25566
		public string colorName;

		// Token: 0x040063DF RID: 25567
		public float value;
	}

	// Token: 0x020018C8 RID: 6344
	private struct UpdateSimViewSharedData
	{
		// Token: 0x0600832D RID: 33581 RVA: 0x000FAC42 File Offset: 0x000F8E42
		public UpdateSimViewSharedData(SimDebugView instance, byte[] texture_bytes, HashedString sim_view_mode, SimDebugView sim_debug_view)
		{
			this.instance = instance;
			this.textureBytes = texture_bytes;
			this.simViewMode = sim_view_mode;
			this.simDebugView = sim_debug_view;
		}

		// Token: 0x040063E0 RID: 25568
		public SimDebugView instance;

		// Token: 0x040063E1 RID: 25569
		public HashedString simViewMode;

		// Token: 0x040063E2 RID: 25570
		public SimDebugView simDebugView;

		// Token: 0x040063E3 RID: 25571
		public byte[] textureBytes;
	}

	// Token: 0x020018C9 RID: 6345
	private struct UpdateSimViewWorkItem : IWorkItem<SimDebugView.UpdateSimViewSharedData>
	{
		// Token: 0x0600832E RID: 33582 RVA: 0x0034DE18 File Offset: 0x0034C018
		public UpdateSimViewWorkItem(int x0, int y0, int x1, int y1)
		{
			this.x0 = Mathf.Clamp(x0, 0, Grid.WidthInCells - 1);
			this.x1 = Mathf.Clamp(x1, 0, Grid.WidthInCells - 1);
			this.y0 = Mathf.Clamp(y0, 0, Grid.HeightInCells - 1);
			this.y1 = Mathf.Clamp(y1, 0, Grid.HeightInCells - 1);
		}

		// Token: 0x0600832F RID: 33583 RVA: 0x0034DE78 File Offset: 0x0034C078
		public void Run(SimDebugView.UpdateSimViewSharedData shared_data, int threadIndex)
		{
			Func<SimDebugView, int, Color> func;
			if (!shared_data.instance.getColourFuncs.TryGetValue(shared_data.simViewMode, out func))
			{
				func = new Func<SimDebugView, int, Color>(SimDebugView.GetBlack);
			}
			for (int i = this.y0; i <= this.y1; i++)
			{
				int num = Grid.XYToCell(this.x0, i);
				int num2 = Grid.XYToCell(this.x1, i);
				for (int j = num; j <= num2; j++)
				{
					int num3 = j * 4;
					if (Grid.IsActiveWorld(j))
					{
						Color color = func(shared_data.instance, j);
						shared_data.textureBytes[num3] = (byte)(Mathf.Min(color.r, 1f) * 255f);
						shared_data.textureBytes[num3 + 1] = (byte)(Mathf.Min(color.g, 1f) * 255f);
						shared_data.textureBytes[num3 + 2] = (byte)(Mathf.Min(color.b, 1f) * 255f);
						shared_data.textureBytes[num3 + 3] = (byte)(Mathf.Min(color.a, 1f) * 255f);
					}
					else
					{
						shared_data.textureBytes[num3] = 0;
						shared_data.textureBytes[num3 + 1] = 0;
						shared_data.textureBytes[num3 + 2] = 0;
						shared_data.textureBytes[num3 + 3] = 0;
					}
				}
			}
		}

		// Token: 0x040063E4 RID: 25572
		private int x0;

		// Token: 0x040063E5 RID: 25573
		private int y0;

		// Token: 0x040063E6 RID: 25574
		private int x1;

		// Token: 0x040063E7 RID: 25575
		private int y1;
	}

	// Token: 0x020018CA RID: 6346
	public enum DangerAmount
	{
		// Token: 0x040063E9 RID: 25577
		None,
		// Token: 0x040063EA RID: 25578
		VeryLow,
		// Token: 0x040063EB RID: 25579
		Low,
		// Token: 0x040063EC RID: 25580
		Moderate,
		// Token: 0x040063ED RID: 25581
		High,
		// Token: 0x040063EE RID: 25582
		VeryHigh,
		// Token: 0x040063EF RID: 25583
		Extreme,
		// Token: 0x040063F0 RID: 25584
		MAX_DANGERAMOUNT = 6
	}
}
