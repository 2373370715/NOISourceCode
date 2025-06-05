using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B4E RID: 2894
public class StatusItemRenderer
{
	// Token: 0x1700024D RID: 589
	// (get) Token: 0x060035DB RID: 13787 RVA: 0x000C7961 File Offset: 0x000C5B61
	// (set) Token: 0x060035DC RID: 13788 RVA: 0x000C7969 File Offset: 0x000C5B69
	public int layer { get; private set; }

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x060035DD RID: 13789 RVA: 0x000C7972 File Offset: 0x000C5B72
	// (set) Token: 0x060035DE RID: 13790 RVA: 0x000C797A File Offset: 0x000C5B7A
	public int selectedHandle { get; private set; }

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x060035DF RID: 13791 RVA: 0x000C7983 File Offset: 0x000C5B83
	// (set) Token: 0x060035E0 RID: 13792 RVA: 0x000C798B File Offset: 0x000C5B8B
	public int highlightHandle { get; private set; }

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x060035E1 RID: 13793 RVA: 0x000C7994 File Offset: 0x000C5B94
	// (set) Token: 0x060035E2 RID: 13794 RVA: 0x000C799C File Offset: 0x000C5B9C
	public Color32 backgroundColor { get; private set; }

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x060035E3 RID: 13795 RVA: 0x000C79A5 File Offset: 0x000C5BA5
	// (set) Token: 0x060035E4 RID: 13796 RVA: 0x000C79AD File Offset: 0x000C5BAD
	public Color32 selectedColor { get; private set; }

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x060035E5 RID: 13797 RVA: 0x000C79B6 File Offset: 0x000C5BB6
	// (set) Token: 0x060035E6 RID: 13798 RVA: 0x000C79BE File Offset: 0x000C5BBE
	public Color32 neutralColor { get; private set; }

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x060035E7 RID: 13799 RVA: 0x000C79C7 File Offset: 0x000C5BC7
	// (set) Token: 0x060035E8 RID: 13800 RVA: 0x000C79CF File Offset: 0x000C5BCF
	public Sprite arrowSprite { get; private set; }

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x060035E9 RID: 13801 RVA: 0x000C79D8 File Offset: 0x000C5BD8
	// (set) Token: 0x060035EA RID: 13802 RVA: 0x000C79E0 File Offset: 0x000C5BE0
	public Sprite backgroundSprite { get; private set; }

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x060035EB RID: 13803 RVA: 0x000C79E9 File Offset: 0x000C5BE9
	// (set) Token: 0x060035EC RID: 13804 RVA: 0x000C79F1 File Offset: 0x000C5BF1
	public float scale { get; private set; }

	// Token: 0x060035ED RID: 13805 RVA: 0x0021D708 File Offset: 0x0021B908
	public StatusItemRenderer()
	{
		this.layer = LayerMask.NameToLayer("UI");
		this.entries = new StatusItemRenderer.Entry[100];
		this.shader = Shader.Find("Klei/StatusItem");
		for (int i = 0; i < this.entries.Length; i++)
		{
			StatusItemRenderer.Entry entry = default(StatusItemRenderer.Entry);
			entry.Init(this.shader);
			this.entries[i] = entry;
		}
		this.backgroundColor = new Color32(244, 74, 71, byte.MaxValue);
		this.selectedColor = new Color32(225, 181, 180, byte.MaxValue);
		this.neutralColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		this.arrowSprite = Assets.GetSprite("StatusBubbleTop");
		this.backgroundSprite = Assets.GetSprite("StatusBubble");
		this.scale = 1f;
		Game.Instance.Subscribe(2095258329, new Action<object>(this.OnHighlightObject));
	}

	// Token: 0x060035EE RID: 13806 RVA: 0x0021D83C File Offset: 0x0021BA3C
	public int GetIdx(Transform transform)
	{
		int instanceID = transform.GetInstanceID();
		int num = 0;
		if (!this.handleTable.TryGetValue(instanceID, out num))
		{
			int num2 = this.entryCount;
			this.entryCount = num2 + 1;
			num = num2;
			this.handleTable[instanceID] = num;
			StatusItemRenderer.Entry entry = this.entries[num];
			entry.handle = instanceID;
			entry.transform = transform;
			entry.buildingPos = transform.GetPosition();
			entry.building = transform.GetComponent<Building>();
			entry.isBuilding = (entry.building != null);
			entry.selectable = transform.GetComponent<KSelectable>();
			this.entries[num] = entry;
		}
		return num;
	}

	// Token: 0x060035EF RID: 13807 RVA: 0x0021D8EC File Offset: 0x0021BAEC
	public void Add(Transform transform, StatusItem status_item)
	{
		if (this.entryCount == this.entries.Length)
		{
			StatusItemRenderer.Entry[] array = new StatusItemRenderer.Entry[this.entries.Length * 2];
			for (int i = 0; i < this.entries.Length; i++)
			{
				array[i] = this.entries[i];
			}
			for (int j = this.entries.Length; j < array.Length; j++)
			{
				array[j].Init(this.shader);
			}
			this.entries = array;
		}
		int idx = this.GetIdx(transform);
		StatusItemRenderer.Entry entry = this.entries[idx];
		entry.Add(status_item);
		this.entries[idx] = entry;
	}

	// Token: 0x060035F0 RID: 13808 RVA: 0x0021D99C File Offset: 0x0021BB9C
	public void Remove(Transform transform, StatusItem status_item)
	{
		int instanceID = transform.GetInstanceID();
		int num = 0;
		if (!this.handleTable.TryGetValue(instanceID, out num))
		{
			return;
		}
		StatusItemRenderer.Entry entry = this.entries[num];
		if (entry.statusItems.Count == 0)
		{
			return;
		}
		entry.Remove(status_item);
		this.entries[num] = entry;
		if (entry.statusItems.Count == 0)
		{
			this.ClearIdx(num);
		}
	}

	// Token: 0x060035F1 RID: 13809 RVA: 0x0021DA08 File Offset: 0x0021BC08
	private void ClearIdx(int idx)
	{
		StatusItemRenderer.Entry entry = this.entries[idx];
		this.handleTable.Remove(entry.handle);
		if (idx != this.entryCount - 1)
		{
			entry.Replace(this.entries[this.entryCount - 1]);
			this.entries[idx] = entry;
			this.handleTable[entry.handle] = idx;
		}
		entry = this.entries[this.entryCount - 1];
		entry.Clear();
		this.entries[this.entryCount - 1] = entry;
		this.entryCount--;
	}

	// Token: 0x060035F2 RID: 13810 RVA: 0x000C79FA File Offset: 0x000C5BFA
	private HashedString GetMode()
	{
		if (OverlayScreen.Instance != null)
		{
			return OverlayScreen.Instance.mode;
		}
		return OverlayModes.None.ID;
	}

	// Token: 0x060035F3 RID: 13811 RVA: 0x0021DAB8 File Offset: 0x0021BCB8
	public void MarkAllDirty()
	{
		for (int i = 0; i < this.entryCount; i++)
		{
			this.entries[i].MarkDirty();
		}
	}

	// Token: 0x060035F4 RID: 13812 RVA: 0x0021DAE8 File Offset: 0x0021BCE8
	public void RenderEveryTick()
	{
		if (DebugHandler.HideUI)
		{
			return;
		}
		this.scale = 1f + Mathf.Sin(Time.unscaledTime * 8f) * 0.1f;
		Shader.SetGlobalVector("_StatusItemParameters", new Vector4(this.scale, 0f, 0f, 0f));
		Vector3 camera_tr = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 camera_bl = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		this.visibleEntries.Clear();
		Camera worldCamera = GameScreenManager.Instance.worldSpaceCanvas.GetComponent<Canvas>().worldCamera;
		for (int i = 0; i < this.entryCount; i++)
		{
			this.entries[i].Render(this, camera_bl, camera_tr, this.GetMode(), worldCamera);
		}
	}

	// Token: 0x060035F5 RID: 13813 RVA: 0x0021DBEC File Offset: 0x0021BDEC
	public void GetIntersections(Vector2 pos, List<InterfaceTool.Intersection> intersections)
	{
		foreach (StatusItemRenderer.Entry entry in this.visibleEntries)
		{
			entry.GetIntersection(pos, intersections, this.scale);
		}
	}

	// Token: 0x060035F6 RID: 13814 RVA: 0x0021DC48 File Offset: 0x0021BE48
	public void GetIntersections(Vector2 pos, List<KSelectable> selectables)
	{
		foreach (StatusItemRenderer.Entry entry in this.visibleEntries)
		{
			entry.GetIntersection(pos, selectables, this.scale);
		}
	}

	// Token: 0x060035F7 RID: 13815 RVA: 0x0021DCA4 File Offset: 0x0021BEA4
	public void SetOffset(Transform transform, Vector3 offset)
	{
		int num = 0;
		if (this.handleTable.TryGetValue(transform.GetInstanceID(), out num))
		{
			this.entries[num].offset = offset;
		}
	}

	// Token: 0x060035F8 RID: 13816 RVA: 0x0021DCDC File Offset: 0x0021BEDC
	private void OnSelectObject(object data)
	{
		int num = 0;
		if (this.handleTable.TryGetValue(this.selectedHandle, out num))
		{
			this.entries[num].MarkDirty();
		}
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			this.selectedHandle = gameObject.transform.GetInstanceID();
			if (this.handleTable.TryGetValue(this.selectedHandle, out num))
			{
				this.entries[num].MarkDirty();
				return;
			}
		}
		else
		{
			this.highlightHandle = -1;
		}
	}

	// Token: 0x060035F9 RID: 13817 RVA: 0x0021DD60 File Offset: 0x0021BF60
	private void OnHighlightObject(object data)
	{
		int num = 0;
		if (this.handleTable.TryGetValue(this.highlightHandle, out num))
		{
			StatusItemRenderer.Entry entry = this.entries[num];
			entry.MarkDirty();
			this.entries[num] = entry;
		}
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			this.highlightHandle = gameObject.transform.GetInstanceID();
			if (this.handleTable.TryGetValue(this.highlightHandle, out num))
			{
				StatusItemRenderer.Entry entry2 = this.entries[num];
				entry2.MarkDirty();
				this.entries[num] = entry2;
				return;
			}
		}
		else
		{
			this.highlightHandle = -1;
		}
	}

	// Token: 0x060035FA RID: 13818 RVA: 0x0021DE04 File Offset: 0x0021C004
	public void Destroy()
	{
		Game.Instance.Unsubscribe(-1503271301, new Action<object>(this.OnSelectObject));
		Game.Instance.Unsubscribe(-1201923725, new Action<object>(this.OnHighlightObject));
		foreach (StatusItemRenderer.Entry entry in this.entries)
		{
			entry.Clear();
			entry.FreeResources();
		}
	}

	// Token: 0x04002539 RID: 9529
	private StatusItemRenderer.Entry[] entries;

	// Token: 0x0400253A RID: 9530
	private int entryCount;

	// Token: 0x0400253B RID: 9531
	private Dictionary<int, int> handleTable = new Dictionary<int, int>();

	// Token: 0x04002545 RID: 9541
	private Shader shader;

	// Token: 0x04002546 RID: 9542
	public List<StatusItemRenderer.Entry> visibleEntries = new List<StatusItemRenderer.Entry>();

	// Token: 0x02000B4F RID: 2895
	public struct Entry
	{
		// Token: 0x060035FB RID: 13819 RVA: 0x000C7A19 File Offset: 0x000C5C19
		public void Init(Shader shader)
		{
			this.statusItems = new List<StatusItem>();
			this.mesh = new Mesh();
			this.mesh.name = "StatusItemRenderer";
			this.dirty = true;
			this.material = new Material(shader);
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x0021DE74 File Offset: 0x0021C074
		public void Render(StatusItemRenderer renderer, Vector3 camera_bl, Vector3 camera_tr, HashedString overlay, Camera camera)
		{
			if (this.transform == null)
			{
				string text = "Error cleaning up status items:";
				foreach (StatusItem statusItem in this.statusItems)
				{
					text += statusItem.Id;
				}
				global::Debug.LogWarning(text);
				return;
			}
			Vector3 vector = this.isBuilding ? this.buildingPos : this.transform.GetPosition();
			if (this.isBuilding)
			{
				vector.x += (float)((this.building.Def.WidthInCells - 1) % 2) / 2f;
			}
			if (vector.x < camera_bl.x || vector.x > camera_tr.x || vector.y < camera_bl.y || vector.y > camera_tr.y)
			{
				return;
			}
			int num = Grid.PosToCell(vector);
			if (Grid.IsValidCell(num) && (!Grid.IsVisible(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId))
			{
				return;
			}
			if (!this.selectable.IsSelectable)
			{
				return;
			}
			renderer.visibleEntries.Add(this);
			if (this.dirty)
			{
				int num2 = 0;
				StatusItemRenderer.Entry.spritesListedToRender.Clear();
				StatusItemRenderer.Entry.statusItemsToRender_Index.Clear();
				int num3 = -1;
				foreach (StatusItem statusItem2 in this.statusItems)
				{
					num3++;
					if (statusItem2.UseConditionalCallback(overlay, this.transform) || !(overlay != OverlayModes.None.ID) || !(statusItem2.render_overlay != overlay))
					{
						Sprite sprite = statusItem2.sprite.sprite;
						if (!statusItem2.unique)
						{
							if (StatusItemRenderer.Entry.spritesListedToRender.Contains(sprite) || StatusItemRenderer.Entry.spritesListedToRender.Count >= StatusItemRenderer.Entry.spritesListedToRender.Capacity)
							{
								continue;
							}
							StatusItemRenderer.Entry.spritesListedToRender.Add(sprite);
						}
						StatusItemRenderer.Entry.statusItemsToRender_Index.Add(num3);
						num2++;
					}
				}
				this.hasVisibleStatusItems = (num2 != 0);
				StatusItemRenderer.Entry.MeshBuilder meshBuilder = new StatusItemRenderer.Entry.MeshBuilder(num2 + 6, this.material);
				float num4 = 0.25f;
				float z = -5f;
				Vector2 b = new Vector2(0.05f, -0.05f);
				float num5 = 0.02f;
				Color32 c = new Color32(0, 0, 0, byte.MaxValue);
				Color32 c2 = new Color32(0, 0, 0, 75);
				Color32 c3 = renderer.neutralColor;
				if (renderer.selectedHandle == this.handle || renderer.highlightHandle == this.handle)
				{
					c3 = renderer.selectedColor;
				}
				else
				{
					for (int i = 0; i < this.statusItems.Count; i++)
					{
						if (this.statusItems[i].notificationType != NotificationType.Neutral)
						{
							c3 = renderer.backgroundColor;
							break;
						}
					}
				}
				meshBuilder.AddQuad(new Vector2(0f, 0.29f) + b, new Vector2(0.05f, 0.05f), z, renderer.arrowSprite, c2);
				meshBuilder.AddQuad(new Vector2(0f, 0f) + b, new Vector2(num4 * (float)num2, num4), z, renderer.backgroundSprite, c2);
				meshBuilder.AddQuad(new Vector2(0f, 0f), new Vector2(num4 * (float)num2 + num5, num4 + num5), z, renderer.backgroundSprite, c);
				meshBuilder.AddQuad(new Vector2(0f, 0f), new Vector2(num4 * (float)num2, num4), z, renderer.backgroundSprite, c3);
				for (int j = 0; j < StatusItemRenderer.Entry.statusItemsToRender_Index.Count; j++)
				{
					StatusItem statusItem3 = this.statusItems[StatusItemRenderer.Entry.statusItemsToRender_Index[j]];
					float x = (float)j * num4 * 2f - num4 * (float)(num2 - 1);
					if (statusItem3.sprite == null)
					{
						DebugUtil.DevLogError(string.Concat(new string[]
						{
							"Status Item ",
							statusItem3.Id,
							" has null sprite for icon '",
							statusItem3.iconName,
							"', you need to run Collect Sprites or manually add the sprite to the TintedSprites list in the GameAssets prefab."
						}));
						statusItem3.iconName = "status_item_exclamation";
						statusItem3.sprite = Assets.GetTintedSprite("status_item_exclamation");
					}
					Sprite sprite2 = statusItem3.sprite.sprite;
					meshBuilder.AddQuad(new Vector2(x, 0f), new Vector2(num4, num4), z, sprite2, c);
				}
				meshBuilder.AddQuad(new Vector2(0f, 0.29f + num5), new Vector2(0.05f + num5, 0.05f + num5), z, renderer.arrowSprite, c);
				meshBuilder.AddQuad(new Vector2(0f, 0.29f), new Vector2(0.05f, 0.05f), z, renderer.arrowSprite, c3);
				meshBuilder.End(this.mesh);
				this.dirty = false;
			}
			if (this.hasVisibleStatusItems && GameScreenManager.Instance != null)
			{
				Graphics.DrawMesh(this.mesh, vector + this.offset, Quaternion.identity, this.material, renderer.layer, camera, 0, null, false, false);
			}
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x000C7A54 File Offset: 0x000C5C54
		public void Add(StatusItem status_item)
		{
			this.statusItems.Add(status_item);
			this.dirty = true;
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x000C7A69 File Offset: 0x000C5C69
		public void Remove(StatusItem status_item)
		{
			this.statusItems.Remove(status_item);
			this.dirty = true;
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x0021E3FC File Offset: 0x0021C5FC
		public void Replace(StatusItemRenderer.Entry entry)
		{
			this.handle = entry.handle;
			this.transform = entry.transform;
			this.building = this.transform.GetComponent<Building>();
			this.buildingPos = this.transform.GetPosition();
			this.isBuilding = (this.building != null);
			this.selectable = this.transform.GetComponent<KSelectable>();
			this.offset = entry.offset;
			this.dirty = true;
			this.statusItems.Clear();
			this.statusItems.AddRange(entry.statusItems);
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x0021E498 File Offset: 0x0021C698
		private bool Intersects(Vector2 pos, float scale)
		{
			if (this.transform == null)
			{
				return false;
			}
			Bounds bounds = this.mesh.bounds;
			Vector3 vector = this.buildingPos + this.offset + bounds.center;
			Vector2 a = new Vector2(vector.x, vector.y);
			Vector3 size = bounds.size;
			Vector2 b = new Vector2(size.x * scale * 0.5f, size.y * scale * 0.5f);
			Vector2 vector2 = a - b;
			Vector2 vector3 = a + b;
			return pos.x >= vector2.x && pos.x <= vector3.x && pos.y >= vector2.y && pos.y <= vector3.y;
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x0021E570 File Offset: 0x0021C770
		public void GetIntersection(Vector2 pos, List<InterfaceTool.Intersection> intersections, float scale)
		{
			if (this.Intersects(pos, scale) && this.selectable.IsSelectable)
			{
				intersections.Add(new InterfaceTool.Intersection
				{
					component = this.selectable,
					distance = -100f
				});
			}
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x000C7A7F File Offset: 0x000C5C7F
		public void GetIntersection(Vector2 pos, List<KSelectable> selectables, float scale)
		{
			if (this.Intersects(pos, scale) && this.selectable.IsSelectable && !selectables.Contains(this.selectable))
			{
				selectables.Add(this.selectable);
			}
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x000C7AB2 File Offset: 0x000C5CB2
		public void Clear()
		{
			this.statusItems.Clear();
			this.offset = Vector3.zero;
			this.dirty = false;
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x000C7AD1 File Offset: 0x000C5CD1
		public void FreeResources()
		{
			if (this.mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(this.mesh);
				this.mesh = null;
			}
			if (this.material != null)
			{
				UnityEngine.Object.DestroyImmediate(this.material);
			}
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x000C7B0C File Offset: 0x000C5D0C
		public void MarkDirty()
		{
			this.dirty = true;
		}

		// Token: 0x04002547 RID: 9543
		public int handle;

		// Token: 0x04002548 RID: 9544
		public Transform transform;

		// Token: 0x04002549 RID: 9545
		public Building building;

		// Token: 0x0400254A RID: 9546
		public Vector3 buildingPos;

		// Token: 0x0400254B RID: 9547
		public KSelectable selectable;

		// Token: 0x0400254C RID: 9548
		public List<StatusItem> statusItems;

		// Token: 0x0400254D RID: 9549
		public Mesh mesh;

		// Token: 0x0400254E RID: 9550
		public bool dirty;

		// Token: 0x0400254F RID: 9551
		public int layer;

		// Token: 0x04002550 RID: 9552
		public Material material;

		// Token: 0x04002551 RID: 9553
		public Vector3 offset;

		// Token: 0x04002552 RID: 9554
		public bool hasVisibleStatusItems;

		// Token: 0x04002553 RID: 9555
		public bool isBuilding;

		// Token: 0x04002554 RID: 9556
		private const int STATUS_ICONS_LIMIT = 12;

		// Token: 0x04002555 RID: 9557
		public static List<Sprite> spritesListedToRender = new List<Sprite>(12);

		// Token: 0x04002556 RID: 9558
		public static List<int> statusItemsToRender_Index = new List<int>(12);

		// Token: 0x02000B50 RID: 2896
		private struct MeshBuilder
		{
			// Token: 0x06003607 RID: 13831 RVA: 0x0021E5BC File Offset: 0x0021C7BC
			public MeshBuilder(int quad_count, Material material)
			{
				this.vertices = new Vector3[4 * quad_count];
				this.uvs = new Vector2[4 * quad_count];
				this.uv2s = new Vector2[4 * quad_count];
				this.colors = new Color32[4 * quad_count];
				this.triangles = new int[6 * quad_count];
				this.material = material;
				this.quadIdx = 0;
			}

			// Token: 0x06003608 RID: 13832 RVA: 0x0021E620 File Offset: 0x0021C820
			public void AddQuad(Vector2 center, Vector2 half_size, float z, Sprite sprite, Color color)
			{
				if (this.quadIdx == StatusItemRenderer.Entry.MeshBuilder.textureIds.Length)
				{
					return;
				}
				Rect rect = sprite.rect;
				Rect textureRect = sprite.textureRect;
				float num = textureRect.width / rect.width;
				float num2 = textureRect.height / rect.height;
				int num3 = 4 * this.quadIdx;
				this.vertices[num3] = new Vector3((center.x - half_size.x) * num, (center.y - half_size.y) * num2, z);
				this.vertices[1 + num3] = new Vector3((center.x - half_size.x) * num, (center.y + half_size.y) * num2, z);
				this.vertices[2 + num3] = new Vector3((center.x + half_size.x) * num, (center.y - half_size.y) * num2, z);
				this.vertices[3 + num3] = new Vector3((center.x + half_size.x) * num, (center.y + half_size.y) * num2, z);
				float num4 = textureRect.x / (float)sprite.texture.width;
				float num5 = textureRect.y / (float)sprite.texture.height;
				float num6 = textureRect.width / (float)sprite.texture.width;
				float num7 = textureRect.height / (float)sprite.texture.height;
				this.uvs[num3] = new Vector2(num4, num5);
				this.uvs[1 + num3] = new Vector2(num4, num5 + num7);
				this.uvs[2 + num3] = new Vector2(num4 + num6, num5);
				this.uvs[3 + num3] = new Vector2(num4 + num6, num5 + num7);
				this.colors[num3] = color;
				this.colors[1 + num3] = color;
				this.colors[2 + num3] = color;
				this.colors[3 + num3] = color;
				float x = (float)this.quadIdx + 0.5f;
				this.uv2s[num3] = new Vector2(x, 0f);
				this.uv2s[1 + num3] = new Vector2(x, 0f);
				this.uv2s[2 + num3] = new Vector2(x, 0f);
				this.uv2s[3 + num3] = new Vector2(x, 0f);
				int num8 = 6 * this.quadIdx;
				this.triangles[num8] = num3;
				this.triangles[1 + num8] = num3 + 1;
				this.triangles[2 + num8] = num3 + 2;
				this.triangles[3 + num8] = num3 + 2;
				this.triangles[4 + num8] = num3 + 1;
				this.triangles[5 + num8] = num3 + 3;
				this.material.SetTexture(StatusItemRenderer.Entry.MeshBuilder.textureIds[this.quadIdx], sprite.texture);
				this.quadIdx++;
			}

			// Token: 0x06003609 RID: 13833 RVA: 0x0021E96C File Offset: 0x0021CB6C
			public void End(Mesh mesh)
			{
				mesh.Clear();
				mesh.vertices = this.vertices;
				mesh.uv = this.uvs;
				mesh.uv2 = this.uv2s;
				mesh.colors32 = this.colors;
				mesh.SetTriangles(this.triangles, 0);
				mesh.RecalculateBounds();
			}

			// Token: 0x04002557 RID: 9559
			private Vector3[] vertices;

			// Token: 0x04002558 RID: 9560
			private Vector2[] uvs;

			// Token: 0x04002559 RID: 9561
			private Vector2[] uv2s;

			// Token: 0x0400255A RID: 9562
			private int[] triangles;

			// Token: 0x0400255B RID: 9563
			private Color32[] colors;

			// Token: 0x0400255C RID: 9564
			private int quadIdx;

			// Token: 0x0400255D RID: 9565
			private Material material;

			// Token: 0x0400255E RID: 9566
			private static int[] textureIds = new int[]
			{
				Shader.PropertyToID("_Tex0"),
				Shader.PropertyToID("_Tex1"),
				Shader.PropertyToID("_Tex2"),
				Shader.PropertyToID("_Tex3"),
				Shader.PropertyToID("_Tex4"),
				Shader.PropertyToID("_Tex5"),
				Shader.PropertyToID("_Tex6"),
				Shader.PropertyToID("_Tex7"),
				Shader.PropertyToID("_Tex8"),
				Shader.PropertyToID("_Tex9"),
				Shader.PropertyToID("_Tex10")
			};
		}
	}
}
