using System;
using System.Collections.Generic;
using System.Linq;
using Klei.Input;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200146F RID: 5231
[AddComponentMenu("KMonoBehaviour/scripts/InterfaceTool")]
public class InterfaceTool : KMonoBehaviour
{
	// Token: 0x170006D0 RID: 1744
	// (get) Token: 0x06006C02 RID: 27650 RVA: 0x000EB6C9 File Offset: 0x000E98C9
	public static InterfaceToolConfig ActiveConfig
	{
		get
		{
			if (InterfaceTool.interfaceConfigMap == null)
			{
				InterfaceTool.InitializeConfigs(global::Action.Invalid, null);
			}
			return InterfaceTool.activeConfigs[InterfaceTool.activeConfigs.Count - 1];
		}
	}

	// Token: 0x06006C03 RID: 27651 RVA: 0x002F2FE4 File Offset: 0x002F11E4
	public static void ToggleConfig(global::Action configKey)
	{
		if (InterfaceTool.interfaceConfigMap == null)
		{
			InterfaceTool.InitializeConfigs(global::Action.Invalid, null);
		}
		InterfaceToolConfig item;
		if (!InterfaceTool.interfaceConfigMap.TryGetValue(configKey, out item))
		{
			global::Debug.LogWarning(string.Format("[InterfaceTool] No config is associated with Key: {0}!", configKey) + " Are you sure the configs were initialized properly!");
			return;
		}
		if (InterfaceTool.activeConfigs.BinarySearch(item, InterfaceToolConfig.ConfigComparer) <= 0)
		{
			global::Debug.Log(string.Format("[InterfaceTool] Pushing config with key: {0}", configKey));
			InterfaceTool.activeConfigs.Add(item);
			InterfaceTool.activeConfigs.Sort(InterfaceToolConfig.ConfigComparer);
			return;
		}
		global::Debug.Log(string.Format("[InterfaceTool] Popping config with key: {0}", configKey));
		InterfaceTool.activeConfigs.Remove(item);
	}

	// Token: 0x06006C04 RID: 27652 RVA: 0x002F3094 File Offset: 0x002F1294
	public static void InitializeConfigs(global::Action defaultKey, List<InterfaceToolConfig> configs)
	{
		string arg = (configs == null) ? "null" : configs.Count.ToString();
		global::Debug.Log(string.Format("[InterfaceTool] Initializing configs with values of DefaultKey: {0} Configs: {1}", defaultKey, arg));
		if (configs == null || configs.Count == 0)
		{
			InterfaceToolConfig interfaceToolConfig = ScriptableObject.CreateInstance<InterfaceToolConfig>();
			InterfaceTool.interfaceConfigMap = new Dictionary<global::Action, InterfaceToolConfig>();
			InterfaceTool.interfaceConfigMap[interfaceToolConfig.InputAction] = interfaceToolConfig;
			return;
		}
		InterfaceTool.interfaceConfigMap = configs.ToDictionary((InterfaceToolConfig x) => x.InputAction);
		InterfaceTool.ToggleConfig(defaultKey);
	}

	// Token: 0x170006D1 RID: 1745
	// (get) Token: 0x06006C05 RID: 27653 RVA: 0x000EB6EF File Offset: 0x000E98EF
	public HashedString ViewMode
	{
		get
		{
			return this.viewMode;
		}
	}

	// Token: 0x06006C06 RID: 27654 RVA: 0x000EB6F7 File Offset: 0x000E98F7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hoverTextConfiguration = base.GetComponent<HoverTextConfiguration>();
	}

	// Token: 0x06006C07 RID: 27655 RVA: 0x000EB70B File Offset: 0x000E990B
	public void ActivateTool()
	{
		this.OnActivateTool();
		this.OnMouseMove(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
		Game.Instance.Trigger(1174281782, this);
	}

	// Token: 0x06006C08 RID: 27656 RVA: 0x002F3130 File Offset: 0x002F1330
	public virtual bool ShowHoverUI()
	{
		if (ManagementMenu.Instance == null || ManagementMenu.Instance.IsFullscreenUIActive())
		{
			return false;
		}
		Vector3 vector = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
		if (OverlayScreen.Instance == null || !ClusterManager.Instance.IsPositionInActiveWorld(vector) || vector.x < 0f || vector.x > Grid.WidthInMeters || vector.y < 0f || vector.y > Grid.HeightInMeters)
		{
			return false;
		}
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		return current != null && !current.IsPointerOverGameObject();
	}

	// Token: 0x06006C09 RID: 27657 RVA: 0x002F31D4 File Offset: 0x002F13D4
	protected virtual void OnActivateTool()
	{
		if (OverlayScreen.Instance != null && this.viewMode != OverlayModes.None.ID && OverlayScreen.Instance.mode != this.viewMode)
		{
			OverlayScreen.Instance.ToggleOverlay(this.viewMode, true);
			InterfaceTool.toolActivatedViewMode = this.viewMode;
		}
		this.SetCursor(this.cursor, this.cursorOffset, CursorMode.Auto);
	}

	// Token: 0x06006C0A RID: 27658 RVA: 0x002F3248 File Offset: 0x002F1448
	public void SetCurrentVirtualInputModuleMousMovementMode(bool mouseMovementOnly, Action<VirtualInputModule> extraActions = null)
	{
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current != null && current.currentInputModule != null)
		{
			VirtualInputModule virtualInputModule = current.currentInputModule as VirtualInputModule;
			if (virtualInputModule != null)
			{
				virtualInputModule.mouseMovementOnly = mouseMovementOnly;
				if (extraActions != null)
				{
					extraActions(virtualInputModule);
				}
			}
		}
	}

	// Token: 0x06006C0B RID: 27659 RVA: 0x002F3298 File Offset: 0x002F1498
	public void DeactivateTool(InterfaceTool new_tool = null)
	{
		this.OnDeactivateTool(new_tool);
		if ((new_tool == null || new_tool == SelectTool.Instance) && InterfaceTool.toolActivatedViewMode != OverlayModes.None.ID && InterfaceTool.toolActivatedViewMode == SimDebugView.Instance.GetMode())
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
			InterfaceTool.toolActivatedViewMode = OverlayModes.None.ID;
		}
	}

	// Token: 0x06006C0C RID: 27660 RVA: 0x000BE729 File Offset: 0x000BC929
	public virtual void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = null;
	}

	// Token: 0x06006C0D RID: 27661 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnDeactivateTool(InterfaceTool new_tool)
	{
	}

	// Token: 0x06006C0E RID: 27662 RVA: 0x000EB733 File Offset: 0x000E9933
	private void OnApplicationFocus(bool focusStatus)
	{
		this.isAppFocused = focusStatus;
	}

	// Token: 0x06006C0F RID: 27663 RVA: 0x000EACF5 File Offset: 0x000E8EF5
	public virtual string GetDeactivateSound()
	{
		return "Tile_Cancel";
	}

	// Token: 0x06006C10 RID: 27664 RVA: 0x002F3304 File Offset: 0x002F1504
	public virtual void OnMouseMove(Vector3 cursor_pos)
	{
		if (this.visualizer == null || !this.isAppFocused)
		{
			return;
		}
		cursor_pos = Grid.CellToPosCBC(Grid.PosToCell(cursor_pos), this.visualizerLayer);
		cursor_pos.z += -0.15f;
		this.visualizer.transform.SetLocalPosition(cursor_pos);
	}

	// Token: 0x06006C11 RID: 27665 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void OnKeyDown(KButtonEvent e)
	{
	}

	// Token: 0x06006C12 RID: 27666 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void OnKeyUp(KButtonEvent e)
	{
	}

	// Token: 0x06006C13 RID: 27667 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void OnLeftClickDown(Vector3 cursor_pos)
	{
	}

	// Token: 0x06006C14 RID: 27668 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void OnLeftClickUp(Vector3 cursor_pos)
	{
	}

	// Token: 0x06006C15 RID: 27669 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void OnRightClickDown(Vector3 cursor_pos, KButtonEvent e)
	{
	}

	// Token: 0x06006C16 RID: 27670 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void OnRightClickUp(Vector3 cursor_pos)
	{
	}

	// Token: 0x06006C17 RID: 27671 RVA: 0x000EB73C File Offset: 0x000E993C
	public virtual void OnFocus(bool focus)
	{
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(focus);
		}
		this.hasFocus = focus;
	}

	// Token: 0x06006C18 RID: 27672 RVA: 0x002F3360 File Offset: 0x002F1560
	protected Vector2 GetRegularizedPos(Vector2 input, bool minimize)
	{
		Vector3 vector = new Vector3(Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, 0f);
		return Grid.CellToPosCCC(Grid.PosToCell(input), Grid.SceneLayer.Background) + (minimize ? (-vector) : vector);
	}

	// Token: 0x06006C19 RID: 27673 RVA: 0x002F33A8 File Offset: 0x002F15A8
	protected Vector2 GetWorldRestrictedPosition(Vector2 input)
	{
		input.x = Mathf.Clamp(input.x, ClusterManager.Instance.activeWorld.minimumBounds.x, ClusterManager.Instance.activeWorld.maximumBounds.x);
		input.y = Mathf.Clamp(input.y, ClusterManager.Instance.activeWorld.minimumBounds.y, ClusterManager.Instance.activeWorld.maximumBounds.y);
		return input;
	}

	// Token: 0x06006C1A RID: 27674 RVA: 0x002F342C File Offset: 0x002F162C
	protected void SetCursor(Texture2D new_cursor, Vector2 offset, CursorMode mode)
	{
		if (new_cursor != InterfaceTool.activeCursor && new_cursor != null)
		{
			InterfaceTool.activeCursor = new_cursor;
			try
			{
				Cursor.SetCursor(new_cursor, offset, mode);
				if (PlayerController.Instance.vim != null)
				{
					PlayerController.Instance.vim.SetCursor(new_cursor);
				}
			}
			catch (Exception ex)
			{
				string details = string.Format("SetCursor Failed new_cursor={0} offset={1} mode={2}", new_cursor, offset, mode);
				KCrashReporter.ReportDevNotification("SetCursor Failed", ex.StackTrace, details, false, null);
			}
		}
	}

	// Token: 0x06006C1B RID: 27675 RVA: 0x000EB75F File Offset: 0x000E995F
	protected void UpdateHoverElements(List<KSelectable> hits)
	{
		if (this.hoverTextConfiguration != null)
		{
			this.hoverTextConfiguration.UpdateHoverElements(hits);
		}
	}

	// Token: 0x06006C1C RID: 27676 RVA: 0x002F34C0 File Offset: 0x002F16C0
	public virtual void LateUpdate()
	{
		if (!this.populateHitsList)
		{
			this.UpdateHoverElements(null);
			return;
		}
		if (!this.isAppFocused)
		{
			return;
		}
		if (!Grid.IsValidCell(Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()))))
		{
			return;
		}
		this.hits.Clear();
		this.GetSelectablesUnderCursor(this.hits);
		KSelectable objectUnderCursor = this.GetObjectUnderCursor<KSelectable>(false, (KSelectable s) => s.GetComponent<KSelectable>().IsSelectable, null);
		this.UpdateHoverElements(this.hits);
		if (!this.hasFocus && this.hoverOverride == null)
		{
			this.ClearHover();
		}
		else if (objectUnderCursor != this.hover)
		{
			this.ClearHover();
			this.hover = objectUnderCursor;
			if (objectUnderCursor != null)
			{
				Game.Instance.Trigger(2095258329, objectUnderCursor.gameObject);
				objectUnderCursor.Hover(!this.playedSoundThisFrame);
				this.playedSoundThisFrame = true;
			}
		}
		this.playedSoundThisFrame = false;
	}

	// Token: 0x06006C1D RID: 27677 RVA: 0x002F35C4 File Offset: 0x002F17C4
	public void GetSelectablesUnderCursor(List<KSelectable> hits)
	{
		if (this.hoverOverride != null)
		{
			hits.Add(this.hoverOverride);
		}
		Camera main = Camera.main;
		Vector3 position = new Vector3(KInputManager.GetMousePos().x, KInputManager.GetMousePos().y, -main.transform.GetPosition().z);
		Vector3 vector = main.ScreenToWorldPoint(position);
		Vector2 vector2 = new Vector2(vector.x, vector.y);
		int cell = Grid.PosToCell(vector);
		if (!Grid.IsValidCell(cell) || !Grid.IsVisible(cell))
		{
			return;
		}
		Game.Instance.statusItemRenderer.GetIntersections(vector2, hits);
		ListPool<ScenePartitionerEntry, SelectTool>.PooledList pooledList = ListPool<ScenePartitionerEntry, SelectTool>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)vector2.x, (int)vector2.y, 1, 1, GameScenePartitioner.Instance.collisionLayer, pooledList);
		pooledList.Sort((ScenePartitionerEntry x, ScenePartitionerEntry y) => this.SortHoverCards(x, y));
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			KCollider2D kcollider2D = scenePartitionerEntry.obj as KCollider2D;
			if (!(kcollider2D == null) && kcollider2D.Intersects(new Vector2(vector2.x, vector2.y)))
			{
				KSelectable kselectable = kcollider2D.GetComponent<KSelectable>();
				if (kselectable == null)
				{
					kselectable = kcollider2D.GetComponentInParent<KSelectable>();
				}
				if (!(kselectable == null) && kselectable.isActiveAndEnabled && !hits.Contains(kselectable) && kselectable.IsSelectable)
				{
					hits.Add(kselectable);
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06006C1E RID: 27678 RVA: 0x000EB77B File Offset: 0x000E997B
	public void SetLinkCursor(bool set)
	{
		this.SetCursor(set ? Assets.GetTexture("cursor_hand") : this.cursor, set ? Vector2.zero : this.cursorOffset, CursorMode.Auto);
	}

	// Token: 0x06006C1F RID: 27679 RVA: 0x002F3768 File Offset: 0x002F1968
	protected T GetObjectUnderCursor<T>(bool cycleSelection, Func<T, bool> condition = null, Component previous_selection = null) where T : MonoBehaviour
	{
		this.intersections.Clear();
		this.GetObjectUnderCursor2D<T>(this.intersections, condition, this.layerMask);
		this.intersections.RemoveAll(new Predicate<InterfaceTool.Intersection>(InterfaceTool.is_component_null));
		if (this.intersections.Count <= 0)
		{
			this.prevIntersectionGroup.Clear();
			return default(T);
		}
		this.curIntersectionGroup.Clear();
		foreach (InterfaceTool.Intersection intersection in this.intersections)
		{
			this.curIntersectionGroup.Add(intersection.component);
		}
		if (!this.prevIntersectionGroup.Equals(this.curIntersectionGroup))
		{
			this.hitCycleCount = 0;
			this.prevIntersectionGroup = this.curIntersectionGroup;
		}
		this.intersections.Sort((InterfaceTool.Intersection a, InterfaceTool.Intersection b) => this.SortSelectables(a.component as KMonoBehaviour, b.component as KMonoBehaviour));
		int index = 0;
		if (cycleSelection)
		{
			index = this.hitCycleCount % this.intersections.Count;
			if (this.intersections[index].component != previous_selection || previous_selection == null)
			{
				index = 0;
				this.hitCycleCount = 0;
			}
			else
			{
				int num = this.hitCycleCount + 1;
				this.hitCycleCount = num;
				index = num % this.intersections.Count;
			}
		}
		return this.intersections[index].component as T;
	}

	// Token: 0x06006C20 RID: 27680 RVA: 0x002F38E8 File Offset: 0x002F1AE8
	private void GetObjectUnderCursor2D<T>(List<InterfaceTool.Intersection> intersections, Func<T, bool> condition, int layer_mask) where T : MonoBehaviour
	{
		Camera main = Camera.main;
		Vector3 position = new Vector3(KInputManager.GetMousePos().x, KInputManager.GetMousePos().y, -main.transform.GetPosition().z);
		Vector3 vector = main.ScreenToWorldPoint(position);
		Vector2 pos = new Vector2(vector.x, vector.y);
		if (this.hoverOverride != null)
		{
			intersections.Add(new InterfaceTool.Intersection
			{
				component = this.hoverOverride,
				distance = -100f
			});
		}
		int cell = Grid.PosToCell(vector);
		if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
		{
			Game.Instance.statusItemRenderer.GetIntersections(pos, intersections);
			ListPool<ScenePartitionerEntry, SelectTool>.PooledList pooledList = ListPool<ScenePartitionerEntry, SelectTool>.Allocate();
			int x_bottomLeft = 0;
			int y_bottomLeft = 0;
			Grid.CellToXY(cell, out x_bottomLeft, out y_bottomLeft);
			GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, 1, 1, GameScenePartitioner.Instance.collisionLayer, pooledList);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				KCollider2D kcollider2D = scenePartitionerEntry.obj as KCollider2D;
				if (!(kcollider2D == null) && kcollider2D.Intersects(new Vector2(vector.x, vector.y)))
				{
					T t = kcollider2D.GetComponent<T>();
					if (t == null)
					{
						t = kcollider2D.GetComponentInParent<T>();
					}
					if (!(t == null) && (1 << t.gameObject.layer & layer_mask) != 0 && !(t == null) && (condition == null || condition(t)))
					{
						float num = t.transform.GetPosition().z - vector.z;
						bool flag = false;
						for (int i = 0; i < intersections.Count; i++)
						{
							InterfaceTool.Intersection intersection = intersections[i];
							if (intersection.component.gameObject == t.gameObject)
							{
								intersection.distance = Mathf.Min(intersection.distance, num);
								intersections[i] = intersection;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							intersections.Add(new InterfaceTool.Intersection
							{
								component = t,
								distance = num
							});
						}
					}
				}
			}
			pooledList.Recycle();
		}
	}

	// Token: 0x06006C21 RID: 27681 RVA: 0x002F3B8C File Offset: 0x002F1D8C
	private int SortSelectables(KMonoBehaviour x, KMonoBehaviour y)
	{
		if (x == null && y == null)
		{
			return 0;
		}
		if (x == null)
		{
			return -1;
		}
		if (y == null)
		{
			return 1;
		}
		int num = x.transform.GetPosition().z.CompareTo(y.transform.GetPosition().z);
		if (num != 0)
		{
			return num;
		}
		return x.GetInstanceID().CompareTo(y.GetInstanceID());
	}

	// Token: 0x06006C22 RID: 27682 RVA: 0x000EB7A9 File Offset: 0x000E99A9
	public void SetHoverOverride(KSelectable hover_override)
	{
		this.hoverOverride = hover_override;
	}

	// Token: 0x06006C23 RID: 27683 RVA: 0x002F3C08 File Offset: 0x002F1E08
	private int SortHoverCards(ScenePartitionerEntry x, ScenePartitionerEntry y)
	{
		KMonoBehaviour x2 = x.obj as KMonoBehaviour;
		KMonoBehaviour y2 = y.obj as KMonoBehaviour;
		return this.SortSelectables(x2, y2);
	}

	// Token: 0x06006C24 RID: 27684 RVA: 0x000EB7B2 File Offset: 0x000E99B2
	private static bool is_component_null(InterfaceTool.Intersection intersection)
	{
		return !intersection.component;
	}

	// Token: 0x06006C25 RID: 27685 RVA: 0x000EB7C2 File Offset: 0x000E99C2
	protected void ClearHover()
	{
		if (this.hover != null)
		{
			KSelectable kselectable = this.hover;
			this.hover = null;
			kselectable.Unhover();
			Game.Instance.Trigger(-1201923725, null);
		}
	}

	// Token: 0x040051C8 RID: 20936
	private static Dictionary<global::Action, InterfaceToolConfig> interfaceConfigMap = null;

	// Token: 0x040051C9 RID: 20937
	private static List<InterfaceToolConfig> activeConfigs = new List<InterfaceToolConfig>();

	// Token: 0x040051CA RID: 20938
	public const float MaxClickDistance = 0.02f;

	// Token: 0x040051CB RID: 20939
	public const float DepthBias = -0.15f;

	// Token: 0x040051CC RID: 20940
	public GameObject visualizer;

	// Token: 0x040051CD RID: 20941
	public Grid.SceneLayer visualizerLayer = Grid.SceneLayer.Move;

	// Token: 0x040051CE RID: 20942
	public string placeSound;

	// Token: 0x040051CF RID: 20943
	protected bool populateHitsList;

	// Token: 0x040051D0 RID: 20944
	[NonSerialized]
	public bool hasFocus;

	// Token: 0x040051D1 RID: 20945
	[SerializeField]
	protected Texture2D cursor;

	// Token: 0x040051D2 RID: 20946
	public Vector2 cursorOffset = new Vector2(2f, 2f);

	// Token: 0x040051D3 RID: 20947
	public System.Action OnDeactivate;

	// Token: 0x040051D4 RID: 20948
	private static Texture2D activeCursor = null;

	// Token: 0x040051D5 RID: 20949
	private static HashedString toolActivatedViewMode = OverlayModes.None.ID;

	// Token: 0x040051D6 RID: 20950
	protected HashedString viewMode = OverlayModes.None.ID;

	// Token: 0x040051D7 RID: 20951
	private HoverTextConfiguration hoverTextConfiguration;

	// Token: 0x040051D8 RID: 20952
	private KSelectable hoverOverride;

	// Token: 0x040051D9 RID: 20953
	public KSelectable hover;

	// Token: 0x040051DA RID: 20954
	protected int layerMask;

	// Token: 0x040051DB RID: 20955
	protected SelectMarker selectMarker;

	// Token: 0x040051DC RID: 20956
	private List<RaycastResult> castResults = new List<RaycastResult>();

	// Token: 0x040051DD RID: 20957
	private bool isAppFocused = true;

	// Token: 0x040051DE RID: 20958
	private List<KSelectable> hits = new List<KSelectable>();

	// Token: 0x040051DF RID: 20959
	protected bool playedSoundThisFrame;

	// Token: 0x040051E0 RID: 20960
	private List<InterfaceTool.Intersection> intersections = new List<InterfaceTool.Intersection>();

	// Token: 0x040051E1 RID: 20961
	private HashSet<Component> prevIntersectionGroup = new HashSet<Component>();

	// Token: 0x040051E2 RID: 20962
	private HashSet<Component> curIntersectionGroup = new HashSet<Component>();

	// Token: 0x040051E3 RID: 20963
	private int hitCycleCount;

	// Token: 0x02001470 RID: 5232
	public struct Intersection
	{
		// Token: 0x040051E4 RID: 20964
		public MonoBehaviour component;

		// Token: 0x040051E5 RID: 20965
		public float distance;
	}
}
