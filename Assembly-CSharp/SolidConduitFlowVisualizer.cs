using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02001B98 RID: 7064
public class SolidConduitFlowVisualizer
{
	// Token: 0x06009461 RID: 37985 RVA: 0x0039F048 File Offset: 0x0039D248
	public SolidConduitFlowVisualizer(SolidConduitFlow flow_manager, Game.ConduitVisInfo vis_info, EventReference overlay_sound, SolidConduitFlowVisualizer.Tuning tuning)
	{
		this.flowManager = flow_manager;
		this.visInfo = vis_info;
		this.overlaySound = overlay_sound;
		this.tuning = tuning;
		this.movingBallMesh = new SolidConduitFlowVisualizer.ConduitFlowMesh();
		this.staticBallMesh = new SolidConduitFlowVisualizer.ConduitFlowMesh();
	}

	// Token: 0x06009462 RID: 37986 RVA: 0x00105594 File Offset: 0x00103794
	public void FreeResources()
	{
		this.movingBallMesh.Cleanup();
		this.staticBallMesh.Cleanup();
	}

	// Token: 0x06009463 RID: 37987 RVA: 0x0039F0C4 File Offset: 0x0039D2C4
	private float CalculateMassScale(float mass)
	{
		float t = (mass - this.visInfo.overlayMassScaleRange.x) / (this.visInfo.overlayMassScaleRange.y - this.visInfo.overlayMassScaleRange.x);
		return Mathf.Lerp(this.visInfo.overlayMassScaleValues.x, this.visInfo.overlayMassScaleValues.y, t);
	}

	// Token: 0x06009464 RID: 37988 RVA: 0x0038B934 File Offset: 0x00389B34
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

	// Token: 0x06009465 RID: 37989 RVA: 0x0039F12C File Offset: 0x0039D32C
	private Color32 GetBackgroundColor(float insulation_lerp)
	{
		if (this.showContents)
		{
			return Color32.Lerp(GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayTintName), GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayInsulatedTintName), insulation_lerp);
		}
		return Color32.Lerp(this.visInfo.tint, this.visInfo.insulatedTint, insulation_lerp);
	}

	// Token: 0x06009466 RID: 37990 RVA: 0x0039F198 File Offset: 0x0039D398
	public void Render(float z, int render_layer, float lerp_percent, bool trigger_audio = false)
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		Vector2I v = new Vector2I(Mathf.Max(0, visibleArea.Min.x - 1), Mathf.Max(0, visibleArea.Min.y - 1));
		Vector2I v2 = new Vector2I(Mathf.Min(Grid.WidthInCells - 1, visibleArea.Max.x + 1), Mathf.Min(Grid.HeightInCells - 1, visibleArea.Max.y + 1));
		this.animTime += (double)Time.deltaTime;
		if (trigger_audio)
		{
			if (this.audioInfo == null)
			{
				this.audioInfo = new List<SolidConduitFlowVisualizer.AudioInfo>();
			}
			for (int i = 0; i < this.audioInfo.Count; i++)
			{
				SolidConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
				audioInfo.distance = float.PositiveInfinity;
				audioInfo.position = Vector3.zero;
				audioInfo.blobCount = (audioInfo.blobCount + 1) % SolidConduitFlowVisualizer.BLOB_SOUND_COUNT;
				this.audioInfo[i] = audioInfo;
			}
		}
		Vector3 position = CameraController.Instance.transform.GetPosition();
		Element element = null;
		if (this.tuning.renderMesh)
		{
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
			for (int j = 0; j < this.flowManager.GetSOAInfo().NumEntries; j++)
			{
				Vector2I u = Grid.CellToXY(this.flowManager.GetSOAInfo().GetCell(j));
				if (!(u < v) && !(u > v2))
				{
					SolidConduitFlow.Conduit conduit = this.flowManager.GetSOAInfo().GetConduit(j);
					SolidConduitFlow.ConduitFlowInfo lastFlowInfo = conduit.GetLastFlowInfo(this.flowManager);
					SolidConduitFlow.ConduitContents initialContents = conduit.GetInitialContents(this.flowManager);
					bool flag = lastFlowInfo.direction > SolidConduitFlow.FlowDirection.None;
					if (flag)
					{
						int cell = conduit.GetCell(this.flowManager);
						int cellFromDirection = SolidConduitFlow.GetCellFromDirection(cell, lastFlowInfo.direction);
						Vector2I vector2I = Grid.CellToXY(cell);
						Vector2I vector2I2 = Grid.CellToXY(cellFromDirection);
						Vector2 vector = vector2I;
						if (cell != -1)
						{
							vector = Vector2.Lerp(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y), lerp_percent);
						}
						float a = this.insulatedCells.Contains(cell) ? 1f : 0f;
						float b = this.insulatedCells.Contains(cellFromDirection) ? 1f : 0f;
						float insulation_lerp = Mathf.Lerp(a, b, lerp_percent);
						Color c = this.GetBackgroundColor(insulation_lerp);
						Vector2I uvbl = new Vector2I(0, 0);
						Vector2I uvtl = new Vector2I(0, 1);
						Vector2I uvbr = new Vector2I(1, 0);
						Vector2I uvtr = new Vector2I(1, 1);
						float highlight = 0f;
						if (this.showContents)
						{
							if (flag != initialContents.pickupableHandle.IsValid())
							{
								this.movingBallMesh.AddQuad(vector, c, this.tuning.size, 0f, 0f, uvbl, uvtl, uvbr, uvtr);
							}
						}
						else
						{
							element = null;
							if (Grid.PosToCell(new Vector3(vector.x + SolidConduitFlowVisualizer.GRID_OFFSET.x, vector.y + SolidConduitFlowVisualizer.GRID_OFFSET.y, 0f)) == this.highlightedCell)
							{
								highlight = 1f;
							}
						}
						Color32 contentsColor = this.GetContentsColor(element, c);
						float num = 1f;
						this.movingBallMesh.AddQuad(vector, contentsColor, this.tuning.size * num, 1f, highlight, uvbl, uvtl, uvbr, uvtr);
						if (trigger_audio)
						{
							this.AddAudioSource(conduit, position);
						}
					}
					if (initialContents.pickupableHandle.IsValid() && !flag)
					{
						int cell2 = conduit.GetCell(this.flowManager);
						Vector2 pos = Grid.CellToXY(cell2);
						float insulation_lerp2 = this.insulatedCells.Contains(cell2) ? 1f : 0f;
						Vector2I uvbl2 = new Vector2I(0, 0);
						Vector2I uvtl2 = new Vector2I(0, 1);
						Vector2I uvbr2 = new Vector2I(1, 0);
						Vector2I uvtr2 = new Vector2I(1, 1);
						float highlight2 = 0f;
						Color c2 = this.GetBackgroundColor(insulation_lerp2);
						float num2 = 1f;
						if (this.showContents)
						{
							this.staticBallMesh.AddQuad(pos, c2, this.tuning.size * num2, 0f, 0f, uvbl2, uvtl2, uvbr2, uvtr2);
						}
						else
						{
							element = null;
							if (cell2 == this.highlightedCell)
							{
								highlight2 = 1f;
							}
						}
						Color32 contentsColor2 = this.GetContentsColor(element, c2);
						this.staticBallMesh.AddQuad(pos, contentsColor2, this.tuning.size * num2, 1f, highlight2, uvbl2, uvtl2, uvbr2, uvtr2);
					}
				}
			}
			this.movingBallMesh.End(z, this.layer);
			this.staticBallMesh.End(z, this.layer);
		}
		if (trigger_audio)
		{
			this.TriggerAudio();
		}
	}

	// Token: 0x06009467 RID: 37991 RVA: 0x001055AC File Offset: 0x001037AC
	public void ColourizePipeContents(bool show_contents, bool move_to_overlay_layer)
	{
		this.showContents = show_contents;
		this.layer = ((show_contents && move_to_overlay_layer) ? LayerMask.NameToLayer("MaskedOverlay") : 0);
	}

	// Token: 0x06009468 RID: 37992 RVA: 0x0039F85C File Offset: 0x0039DA5C
	private void AddAudioSource(SolidConduitFlow.Conduit conduit, Vector3 camera_pos)
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
					SolidConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
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
					SolidConduitFlowVisualizer.AudioInfo item = default(SolidConduitFlowVisualizer.AudioInfo);
					item.networkID = network.id;
					item.position = vector;
					item.distance = num;
					item.blobCount = 0;
					this.audioInfo.Add(item);
				}
			}
		}
	}

	// Token: 0x06009469 RID: 37993 RVA: 0x0039F974 File Offset: 0x0039DB74
	private void TriggerAudio()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		CameraController instance = CameraController.Instance;
		int num = 0;
		List<SolidConduitFlowVisualizer.AudioInfo> list = new List<SolidConduitFlowVisualizer.AudioInfo>();
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
			SolidConduitFlowVisualizer.AudioInfo audioInfo = list[j];
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

	// Token: 0x0600946A RID: 37994 RVA: 0x001055CD File Offset: 0x001037CD
	public void SetInsulated(int cell, bool insulated)
	{
		if (insulated)
		{
			this.insulatedCells.Add(cell);
			return;
		}
		this.insulatedCells.Remove(cell);
	}

	// Token: 0x0600946B RID: 37995 RVA: 0x001055ED File Offset: 0x001037ED
	public void SetHighlightedCell(int cell)
	{
		this.highlightedCell = cell;
	}

	// Token: 0x04007080 RID: 28800
	private SolidConduitFlow flowManager;

	// Token: 0x04007081 RID: 28801
	private EventReference overlaySound;

	// Token: 0x04007082 RID: 28802
	private bool showContents;

	// Token: 0x04007083 RID: 28803
	private double animTime;

	// Token: 0x04007084 RID: 28804
	private int layer;

	// Token: 0x04007085 RID: 28805
	private static Vector2 GRID_OFFSET = new Vector2(0.5f, 0.5f);

	// Token: 0x04007086 RID: 28806
	private static int BLOB_SOUND_COUNT = 7;

	// Token: 0x04007087 RID: 28807
	private List<SolidConduitFlowVisualizer.AudioInfo> audioInfo;

	// Token: 0x04007088 RID: 28808
	private HashSet<int> insulatedCells = new HashSet<int>();

	// Token: 0x04007089 RID: 28809
	private Game.ConduitVisInfo visInfo;

	// Token: 0x0400708A RID: 28810
	private SolidConduitFlowVisualizer.ConduitFlowMesh movingBallMesh;

	// Token: 0x0400708B RID: 28811
	private SolidConduitFlowVisualizer.ConduitFlowMesh staticBallMesh;

	// Token: 0x0400708C RID: 28812
	private int highlightedCell = -1;

	// Token: 0x0400708D RID: 28813
	private Color32 highlightColour = new Color(0.2f, 0.2f, 0.2f, 0.2f);

	// Token: 0x0400708E RID: 28814
	private SolidConduitFlowVisualizer.Tuning tuning;

	// Token: 0x02001B99 RID: 7065
	[Serializable]
	public class Tuning
	{
		// Token: 0x0400708F RID: 28815
		public bool renderMesh;

		// Token: 0x04007090 RID: 28816
		public float size;

		// Token: 0x04007091 RID: 28817
		public float spriteCount;

		// Token: 0x04007092 RID: 28818
		public float framesPerSecond;

		// Token: 0x04007093 RID: 28819
		public Texture2D backgroundTexture;

		// Token: 0x04007094 RID: 28820
		public Texture2D foregroundTexture;
	}

	// Token: 0x02001B9A RID: 7066
	private class ConduitFlowMesh
	{
		// Token: 0x0600946E RID: 37998 RVA: 0x0039FA68 File Offset: 0x0039DC68
		public ConduitFlowMesh()
		{
			this.mesh = new Mesh();
			this.mesh.name = "ConduitMesh";
			this.material = new Material(Shader.Find("Klei/ConduitBall"));
		}

		// Token: 0x0600946F RID: 37999 RVA: 0x0039FAD8 File Offset: 0x0039DCD8
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

		// Token: 0x06009470 RID: 38000 RVA: 0x00105612 File Offset: 0x00103812
		public void SetTexture(string id, Texture2D texture)
		{
			this.material.SetTexture(id, texture);
		}

		// Token: 0x06009471 RID: 38001 RVA: 0x00105621 File Offset: 0x00103821
		public void SetVector(string id, Vector4 data)
		{
			this.material.SetVector(id, data);
		}

		// Token: 0x06009472 RID: 38002 RVA: 0x00105630 File Offset: 0x00103830
		public void Begin()
		{
			this.positions.Clear();
			this.uvs.Clear();
			this.triangles.Clear();
			this.colors.Clear();
			this.quadIndex = 0;
		}

		// Token: 0x06009473 RID: 38003 RVA: 0x0039FCCC File Offset: 0x0039DECC
		public void End(float z, int layer)
		{
			this.mesh.Clear();
			this.mesh.SetVertices(this.positions);
			this.mesh.SetUVs(0, this.uvs);
			this.mesh.SetColors(this.colors);
			this.mesh.SetTriangles(this.triangles, 0, false);
			Graphics.DrawMesh(this.mesh, new Vector3(SolidConduitFlowVisualizer.GRID_OFFSET.x, SolidConduitFlowVisualizer.GRID_OFFSET.y, z - 0.1f), Quaternion.identity, this.material, layer);
		}

		// Token: 0x06009474 RID: 38004 RVA: 0x00105665 File Offset: 0x00103865
		public void Cleanup()
		{
			UnityEngine.Object.Destroy(this.mesh);
			this.mesh = null;
			UnityEngine.Object.Destroy(this.material);
			this.material = null;
		}

		// Token: 0x04007095 RID: 28821
		private Mesh mesh;

		// Token: 0x04007096 RID: 28822
		private Material material;

		// Token: 0x04007097 RID: 28823
		private List<Vector3> positions = new List<Vector3>();

		// Token: 0x04007098 RID: 28824
		private List<Vector4> uvs = new List<Vector4>();

		// Token: 0x04007099 RID: 28825
		private List<int> triangles = new List<int>();

		// Token: 0x0400709A RID: 28826
		private List<Color32> colors = new List<Color32>();

		// Token: 0x0400709B RID: 28827
		private int quadIndex;
	}

	// Token: 0x02001B9B RID: 7067
	private struct AudioInfo
	{
		// Token: 0x0400709C RID: 28828
		public int networkID;

		// Token: 0x0400709D RID: 28829
		public int blobCount;

		// Token: 0x0400709E RID: 28830
		public float distance;

		// Token: 0x0400709F RID: 28831
		public Vector3 position;
	}
}
