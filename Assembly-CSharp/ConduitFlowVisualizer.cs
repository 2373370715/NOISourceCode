﻿using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02001B0C RID: 6924
public class ConduitFlowVisualizer
{
	// Token: 0x0600911C RID: 37148 RVA: 0x0038B840 File Offset: 0x00389A40
	public ConduitFlowVisualizer(ConduitFlow flow_manager, Game.ConduitVisInfo vis_info, EventReference overlay_sound, ConduitFlowVisualizer.Tuning tuning)
	{
		this.flowManager = flow_manager;
		this.visInfo = vis_info;
		this.overlaySound = overlay_sound;
		this.tuning = tuning;
		this.movingBallMesh = new ConduitFlowVisualizer.ConduitFlowMesh();
		this.staticBallMesh = new ConduitFlowVisualizer.ConduitFlowMesh();
		ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.InitializeResources();
	}

	// Token: 0x0600911D RID: 37149 RVA: 0x001033E0 File Offset: 0x001015E0
	public void FreeResources()
	{
		this.movingBallMesh.Cleanup();
		this.staticBallMesh.Cleanup();
	}

	// Token: 0x0600911E RID: 37150 RVA: 0x0038B8CC File Offset: 0x00389ACC
	private float CalculateMassScale(float mass)
	{
		float t = (mass - this.visInfo.overlayMassScaleRange.x) / (this.visInfo.overlayMassScaleRange.y - this.visInfo.overlayMassScaleRange.x);
		return Mathf.Lerp(this.visInfo.overlayMassScaleValues.x, this.visInfo.overlayMassScaleValues.y, t);
	}

	// Token: 0x0600911F RID: 37151 RVA: 0x0038B934 File Offset: 0x00389B34
	private Color32 GetContentsColor(Element element, Color32 default_color)
	{
		if (element != null)
		{
			Color c = element.substance.conduitColour;
			c.a = 128f;
			return c;
		}
		return default_color;
	}

	// Token: 0x06009120 RID: 37152 RVA: 0x001033F8 File Offset: 0x001015F8
	private Color32 GetTintColour()
	{
		if (!this.showContents)
		{
			return this.visInfo.tint;
		}
		return GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayTintName);
	}

	// Token: 0x06009121 RID: 37153 RVA: 0x00103428 File Offset: 0x00101628
	private Color32 GetInsulatedTintColour()
	{
		if (!this.showContents)
		{
			return this.visInfo.insulatedTint;
		}
		return GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayInsulatedTintName);
	}

	// Token: 0x06009122 RID: 37154 RVA: 0x00103458 File Offset: 0x00101658
	private Color32 GetRadiantTintColour()
	{
		if (!this.showContents)
		{
			return this.visInfo.radiantTint;
		}
		return GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayRadiantTintName);
	}

	// Token: 0x06009123 RID: 37155 RVA: 0x0038B96C File Offset: 0x00389B6C
	private Color32 GetCellTintColour(int cell)
	{
		Color32 result;
		if (this.insulatedCells.Contains(cell))
		{
			result = this.GetInsulatedTintColour();
		}
		else if (this.radiantCells.Contains(cell))
		{
			result = this.GetRadiantTintColour();
		}
		else
		{
			result = this.GetTintColour();
		}
		return result;
	}

	// Token: 0x06009124 RID: 37156 RVA: 0x0038B9B0 File Offset: 0x00389BB0
	public void Render(float z, int render_layer, float lerp_percent, bool trigger_audio = false)
	{
		this.animTime += (double)Time.deltaTime;
		if (trigger_audio)
		{
			if (this.audioInfo == null)
			{
				this.audioInfo = new List<ConduitFlowVisualizer.AudioInfo>();
			}
			for (int i = 0; i < this.audioInfo.Count; i++)
			{
				ConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
				audioInfo.distance = float.PositiveInfinity;
				audioInfo.position = Vector3.zero;
				audioInfo.blobCount = (audioInfo.blobCount + 1) % 10;
				this.audioInfo[i] = audioInfo;
			}
		}
		if (this.tuning.renderMesh)
		{
			this.RenderMesh(z, render_layer, lerp_percent, trigger_audio);
		}
		if (trigger_audio)
		{
			this.TriggerAudio();
		}
	}

	// Token: 0x06009125 RID: 37157 RVA: 0x0038BA64 File Offset: 0x00389C64
	private void RenderMesh(float z, int render_layer, float lerp_percent, bool trigger_audio)
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		Vector2I min = new Vector2I(Mathf.Max(0, visibleArea.Min.x - 1), Mathf.Max(0, visibleArea.Min.y - 1));
		Vector2I max = new Vector2I(Mathf.Min(Grid.WidthInCells - 1, visibleArea.Max.x + 1), Mathf.Min(Grid.HeightInCells - 1, visibleArea.Max.y + 1));
		ConduitFlowVisualizer.RenderMeshContext renderMeshContext = new ConduitFlowVisualizer.RenderMeshContext(this, lerp_percent, min, max);
		if (renderMeshContext.visible_conduits.Count == 0)
		{
			renderMeshContext.Finish();
			return;
		}
		ConduitFlowVisualizer.RenderMeshBatchJob.Instance.Reset(renderMeshContext);
		GlobalJobManager.Run(ConduitFlowVisualizer.RenderMeshBatchJob.Instance);
		float z2 = 0f;
		if (this.showContents)
		{
			z2 = 1f;
		}
		float w = (float)((int)(this.animTime / (1.0 / (double)this.tuning.framesPerSecond)) % (int)this.tuning.spriteCount) * (1f / this.tuning.spriteCount);
		this.movingBallMesh.Begin();
		this.movingBallMesh.SetTexture("_BackgroundTex", this.tuning.backgroundTexture);
		this.movingBallMesh.SetTexture("_ForegroundTex", this.tuning.foregroundTexture);
		this.movingBallMesh.SetVector("_SpriteSettings", new Vector4(1f / this.tuning.spriteCount, 1f, z2, w));
		this.movingBallMesh.SetVector("_Highlight", new Vector4((float)this.highlightColour.r / 255f, (float)this.highlightColour.g / 255f, (float)this.highlightColour.b / 255f, 0f));
		this.staticBallMesh.Begin();
		this.staticBallMesh.SetTexture("_BackgroundTex", this.tuning.backgroundTexture);
		this.staticBallMesh.SetTexture("_ForegroundTex", this.tuning.foregroundTexture);
		this.staticBallMesh.SetVector("_SpriteSettings", new Vector4(1f / this.tuning.spriteCount, 1f, z2, 0f));
		this.staticBallMesh.SetVector("_Highlight", new Vector4((float)this.highlightColour.r / 255f, (float)this.highlightColour.g / 255f, (float)this.highlightColour.b / 255f, 0f));
		Vector3 position = CameraController.Instance.transform.GetPosition();
		ConduitFlowVisualizer visualizer = trigger_audio ? this : null;
		ConduitFlowVisualizer.RenderMeshBatchJob.Instance.Finish(this.movingBallMesh, this.staticBallMesh, position, visualizer);
		this.movingBallMesh.End(z, this.layer);
		this.staticBallMesh.End(z, this.layer);
		ConduitFlowVisualizer.RenderMeshBatchJob.Instance.Reset(ConduitFlowVisualizer.RenderMeshContext.EmptyContext);
	}

	// Token: 0x06009126 RID: 37158 RVA: 0x00103488 File Offset: 0x00101688
	public void ColourizePipeContents(bool show_contents, bool move_to_overlay_layer)
	{
		this.showContents = show_contents;
		this.layer = ((show_contents && move_to_overlay_layer) ? LayerMask.NameToLayer("MaskedOverlay") : 0);
	}

	// Token: 0x06009127 RID: 37159 RVA: 0x0038BD58 File Offset: 0x00389F58
	private void AddAudioSource(ConduitFlow.Conduit conduit, Vector3 camera_pos)
	{
		using (new KProfiler.Region("AddAudioSource", null))
		{
			UtilityNetwork network = this.flowManager.GetNetwork(conduit);
			if (network != null)
			{
				Vector3 vector = Grid.CellToPosCCC(conduit.GetCell(this.flowManager), Grid.SceneLayer.Building);
				float num = Vector3.SqrMagnitude(vector - camera_pos);
				bool flag = false;
				for (int i = 0; i < this.audioInfo.Count; i++)
				{
					ConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
					if (audioInfo.networkID == network.id)
					{
						if (num < audioInfo.distance)
						{
							audioInfo.distance = num;
							audioInfo.position = vector;
							this.audioInfo[i] = audioInfo;
						}
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					ConduitFlowVisualizer.AudioInfo item = default(ConduitFlowVisualizer.AudioInfo);
					item.networkID = network.id;
					item.position = vector;
					item.distance = num;
					item.blobCount = 0;
					this.audioInfo.Add(item);
				}
			}
		}
	}

	// Token: 0x06009128 RID: 37160 RVA: 0x0038BE70 File Offset: 0x0038A070
	private void TriggerAudio()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		CameraController instance = CameraController.Instance;
		int num = 0;
		List<ConduitFlowVisualizer.AudioInfo> list = new List<ConduitFlowVisualizer.AudioInfo>();
		for (int i = 0; i < this.audioInfo.Count; i++)
		{
			if (instance.IsVisiblePos(this.audioInfo[i].position))
			{
				list.Add(this.audioInfo[i]);
				num++;
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			ConduitFlowVisualizer.AudioInfo audioInfo = list[j];
			if (audioInfo.distance != float.PositiveInfinity)
			{
				Vector3 position = audioInfo.position;
				position.z = 0f;
				EventInstance instance2 = SoundEvent.BeginOneShot(this.overlaySound, position, 1f, false);
				instance2.setParameterByName("blobCount", (float)audioInfo.blobCount, false);
				instance2.setParameterByName("networkCount", (float)num, false);
				SoundEvent.EndOneShot(instance2);
			}
		}
	}

	// Token: 0x06009129 RID: 37161 RVA: 0x001034A9 File Offset: 0x001016A9
	public void AddThermalConductivity(int cell, float conductivity)
	{
		if (conductivity < 1f)
		{
			this.insulatedCells.Add(cell);
			return;
		}
		if (conductivity > 1f)
		{
			this.radiantCells.Add(cell);
		}
	}

	// Token: 0x0600912A RID: 37162 RVA: 0x001034D6 File Offset: 0x001016D6
	public void RemoveThermalConductivity(int cell, float conductivity)
	{
		if (conductivity < 1f)
		{
			this.insulatedCells.Remove(cell);
			return;
		}
		if (conductivity > 1f)
		{
			this.radiantCells.Remove(cell);
		}
	}

	// Token: 0x0600912B RID: 37163 RVA: 0x00103503 File Offset: 0x00101703
	public void SetHighlightedCell(int cell)
	{
		this.highlightedCell = cell;
	}

	// Token: 0x04006DB6 RID: 28086
	private ConduitFlow flowManager;

	// Token: 0x04006DB7 RID: 28087
	private EventReference overlaySound;

	// Token: 0x04006DB8 RID: 28088
	private bool showContents;

	// Token: 0x04006DB9 RID: 28089
	private double animTime;

	// Token: 0x04006DBA RID: 28090
	private int layer;

	// Token: 0x04006DBB RID: 28091
	private static Vector2 GRID_OFFSET = new Vector2(0.5f, 0.5f);

	// Token: 0x04006DBC RID: 28092
	private List<ConduitFlowVisualizer.AudioInfo> audioInfo;

	// Token: 0x04006DBD RID: 28093
	private HashSet<int> insulatedCells = new HashSet<int>();

	// Token: 0x04006DBE RID: 28094
	private HashSet<int> radiantCells = new HashSet<int>();

	// Token: 0x04006DBF RID: 28095
	private Game.ConduitVisInfo visInfo;

	// Token: 0x04006DC0 RID: 28096
	private ConduitFlowVisualizer.ConduitFlowMesh movingBallMesh;

	// Token: 0x04006DC1 RID: 28097
	private ConduitFlowVisualizer.ConduitFlowMesh staticBallMesh;

	// Token: 0x04006DC2 RID: 28098
	private int highlightedCell = -1;

	// Token: 0x04006DC3 RID: 28099
	private Color32 highlightColour = new Color(0.2f, 0.2f, 0.2f, 0.2f);

	// Token: 0x04006DC4 RID: 28100
	private ConduitFlowVisualizer.Tuning tuning;

	// Token: 0x02001B0D RID: 6925
	[Serializable]
	public class Tuning
	{
		// Token: 0x04006DC5 RID: 28101
		public bool renderMesh;

		// Token: 0x04006DC6 RID: 28102
		public float size;

		// Token: 0x04006DC7 RID: 28103
		public float spriteCount;

		// Token: 0x04006DC8 RID: 28104
		public float framesPerSecond;

		// Token: 0x04006DC9 RID: 28105
		public Texture2D backgroundTexture;

		// Token: 0x04006DCA RID: 28106
		public Texture2D foregroundTexture;
	}

	// Token: 0x02001B0E RID: 6926
	private class ConduitFlowMesh
	{
		// Token: 0x0600912E RID: 37166 RVA: 0x0038BF64 File Offset: 0x0038A164
		public ConduitFlowMesh()
		{
			this.mesh = new Mesh();
			this.mesh.name = "ConduitMesh";
			this.material = new Material(Shader.Find("Klei/ConduitBall"));
		}

		// Token: 0x0600912F RID: 37167 RVA: 0x0038BFD4 File Offset: 0x0038A1D4
		public void AddQuad(Vector2 pos, Color32 color, float size, float is_foreground, float highlight, Vector2I uvbl, Vector2I uvtl, Vector2I uvbr, Vector2I uvtr)
		{
			float num = size * 0.5f;
			this.positions.Add(new Vector3(pos.x - num, pos.y - num, 0f));
			this.positions.Add(new Vector3(pos.x - num, pos.y + num, 0f));
			this.positions.Add(new Vector3(pos.x + num, pos.y - num, 0f));
			this.positions.Add(new Vector3(pos.x + num, pos.y + num, 0f));
			this.uvs.Add(new Vector4((float)uvbl.x, (float)uvbl.y, is_foreground, highlight));
			this.uvs.Add(new Vector4((float)uvtl.x, (float)uvtl.y, is_foreground, highlight));
			this.uvs.Add(new Vector4((float)uvbr.x, (float)uvbr.y, is_foreground, highlight));
			this.uvs.Add(new Vector4((float)uvtr.x, (float)uvtr.y, is_foreground, highlight));
			this.colors.Add(color);
			this.colors.Add(color);
			this.colors.Add(color);
			this.colors.Add(color);
			this.triangles.Add(this.quadIndex * 4);
			this.triangles.Add(this.quadIndex * 4 + 1);
			this.triangles.Add(this.quadIndex * 4 + 2);
			this.triangles.Add(this.quadIndex * 4 + 2);
			this.triangles.Add(this.quadIndex * 4 + 1);
			this.triangles.Add(this.quadIndex * 4 + 3);
			this.quadIndex++;
		}

		// Token: 0x06009130 RID: 37168 RVA: 0x00103522 File Offset: 0x00101722
		public void SetTexture(string id, Texture2D texture)
		{
			this.material.SetTexture(id, texture);
		}

		// Token: 0x06009131 RID: 37169 RVA: 0x00103531 File Offset: 0x00101731
		public void SetVector(string id, Vector4 data)
		{
			this.material.SetVector(id, data);
		}

		// Token: 0x06009132 RID: 37170 RVA: 0x00103540 File Offset: 0x00101740
		public void Begin()
		{
			this.positions.Clear();
			this.uvs.Clear();
			this.triangles.Clear();
			this.colors.Clear();
			this.quadIndex = 0;
		}

		// Token: 0x06009133 RID: 37171 RVA: 0x0038C1C8 File Offset: 0x0038A3C8
		public void End(float z, int layer)
		{
			this.mesh.Clear();
			this.mesh.SetVertices(this.positions);
			this.mesh.SetUVs(0, this.uvs);
			this.mesh.SetColors(this.colors);
			this.mesh.SetTriangles(this.triangles, 0, false);
			Graphics.DrawMesh(this.mesh, new Vector3(ConduitFlowVisualizer.GRID_OFFSET.x, ConduitFlowVisualizer.GRID_OFFSET.y, z - 0.1f), Quaternion.identity, this.material, layer);
		}

		// Token: 0x06009134 RID: 37172 RVA: 0x00103575 File Offset: 0x00101775
		public void Cleanup()
		{
			UnityEngine.Object.Destroy(this.mesh);
			this.mesh = null;
			UnityEngine.Object.Destroy(this.material);
			this.material = null;
		}

		// Token: 0x04006DCB RID: 28107
		private Mesh mesh;

		// Token: 0x04006DCC RID: 28108
		private Material material;

		// Token: 0x04006DCD RID: 28109
		private List<Vector3> positions = new List<Vector3>();

		// Token: 0x04006DCE RID: 28110
		private List<Vector4> uvs = new List<Vector4>();

		// Token: 0x04006DCF RID: 28111
		private List<int> triangles = new List<int>();

		// Token: 0x04006DD0 RID: 28112
		private List<Color32> colors = new List<Color32>();

		// Token: 0x04006DD1 RID: 28113
		private int quadIndex;
	}

	// Token: 0x02001B0F RID: 6927
	private struct AudioInfo
	{
		// Token: 0x04006DD2 RID: 28114
		public int networkID;

		// Token: 0x04006DD3 RID: 28115
		public int blobCount;

		// Token: 0x04006DD4 RID: 28116
		public float distance;

		// Token: 0x04006DD5 RID: 28117
		public Vector3 position;
	}

	// Token: 0x02001B10 RID: 6928
	private struct RenderMeshContext
	{
		// Token: 0x06009135 RID: 37173 RVA: 0x0038C260 File Offset: 0x0038A460
		public RenderMeshContext(ConduitFlowVisualizer outer, float lerp_percent, Vector2I min, Vector2I max)
		{
			this.outer = outer;
			this.lerp_percent = lerp_percent;
			this.visible_conduits = ListPool<int, ConduitFlowVisualizer>.Allocate();
			this.visible_conduits.Capacity = Math.Max(outer.flowManager.soaInfo.NumEntries, this.visible_conduits.Capacity);
			for (int num = 0; num != outer.flowManager.soaInfo.NumEntries; num++)
			{
				Vector2I vector2I = Grid.CellToXY(outer.flowManager.soaInfo.GetCell(num));
				if (min <= vector2I && vector2I <= max)
				{
					this.visible_conduits.Add(num);
				}
			}
		}

		// Token: 0x06009136 RID: 37174 RVA: 0x0010359B File Offset: 0x0010179B
		public void Finish()
		{
			this.visible_conduits.Recycle();
		}

		// Token: 0x04006DD6 RID: 28118
		public static ConduitFlowVisualizer.RenderMeshContext EmptyContext;

		// Token: 0x04006DD7 RID: 28119
		public ListPool<int, ConduitFlowVisualizer>.PooledList visible_conduits;

		// Token: 0x04006DD8 RID: 28120
		public ConduitFlowVisualizer outer;

		// Token: 0x04006DD9 RID: 28121
		public float lerp_percent;
	}

	// Token: 0x02001B11 RID: 6929
	private class RenderMeshPerThreadData
	{
		// Token: 0x06009138 RID: 37176 RVA: 0x0038C304 File Offset: 0x0038A504
		public void Finish(ConduitFlowVisualizer.ConduitFlowMesh moving_ball_mesh, ConduitFlowVisualizer.ConduitFlowMesh static_ball_mesh, Vector3 camera_pos, ConduitFlowVisualizer visualizer)
		{
			for (int num = 0; num != this.moving_balls.Count; num++)
			{
				this.moving_balls[num].Consume(moving_ball_mesh);
			}
			this.moving_balls.Clear();
			for (int num2 = 0; num2 != this.static_balls.Count; num2++)
			{
				this.static_balls[num2].Consume(static_ball_mesh);
			}
			this.static_balls.Clear();
			if (visualizer != null)
			{
				foreach (ConduitFlow.Conduit conduit in this.moving_conduits)
				{
					visualizer.AddAudioSource(conduit, camera_pos);
				}
			}
			this.moving_conduits.Clear();
		}

		// Token: 0x04006DDA RID: 28122
		public List<ConduitFlowVisualizer.RenderMeshPerThreadData.Ball> moving_balls = new List<ConduitFlowVisualizer.RenderMeshPerThreadData.Ball>();

		// Token: 0x04006DDB RID: 28123
		public List<ConduitFlowVisualizer.RenderMeshPerThreadData.Ball> static_balls = new List<ConduitFlowVisualizer.RenderMeshPerThreadData.Ball>();

		// Token: 0x04006DDC RID: 28124
		public List<ConduitFlow.Conduit> moving_conduits = new List<ConduitFlow.Conduit>();

		// Token: 0x02001B12 RID: 6930
		public struct Ball
		{
			// Token: 0x0600913A RID: 37178 RVA: 0x001035D1 File Offset: 0x001017D1
			public Ball(ConduitFlow.FlowDirections direction, Vector2 pos, Color32 color, float size, bool foreground, bool highlight)
			{
				this.pos = pos;
				this.size = size;
				this.color = color;
				this.direction = direction;
				this.foreground = foreground;
				this.highlight = highlight;
			}

			// Token: 0x0600913B RID: 37179 RVA: 0x0038C3D8 File Offset: 0x0038A5D8
			public static void InitializeResources()
			{
				ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.None] = new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack
				{
					bl = new Vector2I(0, 0),
					tl = new Vector2I(0, 1),
					br = new Vector2I(1, 0),
					tr = new Vector2I(1, 1)
				};
				ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Left] = new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack
				{
					bl = new Vector2I(0, 0),
					tl = new Vector2I(0, 1),
					br = new Vector2I(1, 0),
					tr = new Vector2I(1, 1)
				};
				ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Right] = ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Left];
				ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Up] = new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack
				{
					bl = new Vector2I(1, 0),
					tl = new Vector2I(0, 0),
					br = new Vector2I(1, 1),
					tr = new Vector2I(0, 1)
				};
				ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Down] = ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[ConduitFlow.FlowDirections.Up];
			}

			// Token: 0x0600913C RID: 37180 RVA: 0x00103600 File Offset: 0x00101800
			private static ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack GetUVPack(ConduitFlow.FlowDirections direction)
			{
				return ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.uv_packs[direction];
			}

			// Token: 0x0600913D RID: 37181 RVA: 0x0038C4E0 File Offset: 0x0038A6E0
			public void Consume(ConduitFlowVisualizer.ConduitFlowMesh mesh)
			{
				ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack uvpack = ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.GetUVPack(this.direction);
				mesh.AddQuad(this.pos, this.color, this.size, (float)(this.foreground ? 1 : 0), (float)(this.highlight ? 1 : 0), uvpack.bl, uvpack.tl, uvpack.br, uvpack.tr);
			}

			// Token: 0x04006DDD RID: 28125
			private Vector2 pos;

			// Token: 0x04006DDE RID: 28126
			private float size;

			// Token: 0x04006DDF RID: 28127
			private Color32 color;

			// Token: 0x04006DE0 RID: 28128
			private ConduitFlow.FlowDirections direction;

			// Token: 0x04006DE1 RID: 28129
			private bool foreground;

			// Token: 0x04006DE2 RID: 28130
			private bool highlight;

			// Token: 0x04006DE3 RID: 28131
			private static Dictionary<ConduitFlow.FlowDirections, ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack> uv_packs = new Dictionary<ConduitFlow.FlowDirections, ConduitFlowVisualizer.RenderMeshPerThreadData.Ball.UVPack>();

			// Token: 0x02001B13 RID: 6931
			private class UVPack
			{
				// Token: 0x04006DE4 RID: 28132
				public Vector2I bl;

				// Token: 0x04006DE5 RID: 28133
				public Vector2I tl;

				// Token: 0x04006DE6 RID: 28134
				public Vector2I br;

				// Token: 0x04006DE7 RID: 28135
				public Vector2I tr;
			}
		}
	}

	// Token: 0x02001B14 RID: 6932
	private class RenderMeshBatchJob : WorkItemCollectionWithThreadContex<ConduitFlowVisualizer.RenderMeshContext, ConduitFlowVisualizer.RenderMeshPerThreadData>
	{
		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06009140 RID: 37184 RVA: 0x00103619 File Offset: 0x00101819
		public static ConduitFlowVisualizer.RenderMeshBatchJob Instance
		{
			get
			{
				if (ConduitFlowVisualizer.RenderMeshBatchJob.instance == null || ConduitFlowVisualizer.RenderMeshBatchJob.instance.threadContexts.Count != GlobalJobManager.ThreadCount)
				{
					ConduitFlowVisualizer.RenderMeshBatchJob.instance = new ConduitFlowVisualizer.RenderMeshBatchJob();
				}
				return ConduitFlowVisualizer.RenderMeshBatchJob.instance;
			}
		}

		// Token: 0x06009141 RID: 37185 RVA: 0x0038C544 File Offset: 0x0038A744
		public RenderMeshBatchJob()
		{
			this.threadContexts = new List<ConduitFlowVisualizer.RenderMeshPerThreadData>();
			for (int i = 0; i < GlobalJobManager.ThreadCount; i++)
			{
				this.threadContexts.Add(new ConduitFlowVisualizer.RenderMeshPerThreadData());
			}
		}

		// Token: 0x06009142 RID: 37186 RVA: 0x00103647 File Offset: 0x00101847
		public void Reset(ConduitFlowVisualizer.RenderMeshContext context)
		{
			this.sharedData = context;
			if (context.visible_conduits == null)
			{
				this.count = 0;
				return;
			}
			this.count = (context.visible_conduits.Count + 32 - 1) / 32;
		}

		// Token: 0x06009143 RID: 37187 RVA: 0x0038C584 File Offset: 0x0038A784
		public override void RunItem(int item, ref ConduitFlowVisualizer.RenderMeshContext shared_data, ConduitFlowVisualizer.RenderMeshPerThreadData thread_context, int threadIndex)
		{
			Element element = null;
			int num = item * 32;
			int num2 = Math.Min(shared_data.visible_conduits.Count, num + 32);
			for (int i = num; i < num2; i++)
			{
				ConduitFlow.Conduit conduit = shared_data.outer.flowManager.soaInfo.GetConduit(shared_data.visible_conduits[i]);
				ConduitFlow.ConduitFlowInfo lastFlowInfo = conduit.GetLastFlowInfo(shared_data.outer.flowManager);
				ConduitFlow.ConduitContents initialContents = conduit.GetInitialContents(shared_data.outer.flowManager);
				if (lastFlowInfo.contents.mass > 0f)
				{
					int cell = conduit.GetCell(shared_data.outer.flowManager);
					int cellFromDirection = ConduitFlow.GetCellFromDirection(cell, lastFlowInfo.direction);
					Vector2I vector2I = Grid.CellToXY(cell);
					Vector2I vector2I2 = Grid.CellToXY(cellFromDirection);
					Vector2 vector = (cell == -1) ? vector2I : Vector2.Lerp(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y), shared_data.lerp_percent);
					Color32 cellTintColour = shared_data.outer.GetCellTintColour(cell);
					Color32 cellTintColour2 = shared_data.outer.GetCellTintColour(cellFromDirection);
					Color32 color = Color32.Lerp(cellTintColour, cellTintColour2, shared_data.lerp_percent);
					bool highlight = false;
					if (shared_data.outer.showContents)
					{
						if (lastFlowInfo.contents.mass >= initialContents.mass)
						{
							thread_context.moving_balls.Add(new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball(lastFlowInfo.direction, vector, color, shared_data.outer.tuning.size, false, false));
						}
						if (element == null || lastFlowInfo.contents.element != element.id)
						{
							element = ElementLoader.FindElementByHash(lastFlowInfo.contents.element);
						}
					}
					else
					{
						element = null;
						highlight = (Grid.PosToCell(new Vector3(vector.x + ConduitFlowVisualizer.GRID_OFFSET.x, vector.y + ConduitFlowVisualizer.GRID_OFFSET.y, 0f)) == shared_data.outer.highlightedCell);
					}
					Color32 contentsColor = shared_data.outer.GetContentsColor(element, color);
					float num3 = 1f;
					if (shared_data.outer.showContents || lastFlowInfo.contents.mass < initialContents.mass)
					{
						num3 = shared_data.outer.CalculateMassScale(lastFlowInfo.contents.mass);
					}
					thread_context.moving_balls.Add(new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball(lastFlowInfo.direction, vector, contentsColor, shared_data.outer.tuning.size * num3, true, highlight));
					thread_context.moving_conduits.Add(conduit);
				}
				if (initialContents.mass > lastFlowInfo.contents.mass && initialContents.mass > 0f)
				{
					int cell2 = conduit.GetCell(shared_data.outer.flowManager);
					Vector2 pos = Grid.CellToXY(cell2);
					float mass = initialContents.mass - lastFlowInfo.contents.mass;
					bool highlight2 = false;
					Color32 cellTintColour3 = shared_data.outer.GetCellTintColour(cell2);
					float num4 = shared_data.outer.CalculateMassScale(mass);
					if (shared_data.outer.showContents)
					{
						thread_context.static_balls.Add(new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball(ConduitFlow.FlowDirections.None, pos, cellTintColour3, shared_data.outer.tuning.size * num4, false, false));
						if (element == null || initialContents.element != element.id)
						{
							element = ElementLoader.FindElementByHash(initialContents.element);
						}
					}
					else
					{
						element = null;
						highlight2 = (cell2 == shared_data.outer.highlightedCell);
					}
					Color32 contentsColor2 = shared_data.outer.GetContentsColor(element, cellTintColour3);
					thread_context.static_balls.Add(new ConduitFlowVisualizer.RenderMeshPerThreadData.Ball(ConduitFlow.FlowDirections.None, pos, contentsColor2, shared_data.outer.tuning.size * num4, true, highlight2));
				}
			}
		}

		// Token: 0x06009144 RID: 37188 RVA: 0x0038C940 File Offset: 0x0038AB40
		public void Finish(ConduitFlowVisualizer.ConduitFlowMesh moving_ball_mesh, ConduitFlowVisualizer.ConduitFlowMesh static_ball_mesh, Vector3 camera_pos, ConduitFlowVisualizer visualizer)
		{
			foreach (ConduitFlowVisualizer.RenderMeshPerThreadData renderMeshPerThreadData in this.threadContexts)
			{
				renderMeshPerThreadData.Finish(moving_ball_mesh, static_ball_mesh, camera_pos, visualizer);
			}
			this.sharedData.Finish();
		}

		// Token: 0x04006DE8 RID: 28136
		private const int kBatchSize = 32;

		// Token: 0x04006DE9 RID: 28137
		private static ConduitFlowVisualizer.RenderMeshBatchJob instance;
	}
}
