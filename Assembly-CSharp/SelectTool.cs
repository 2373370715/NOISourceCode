using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02001490 RID: 5264
public class SelectTool : InterfaceTool
{
	// Token: 0x06006D0E RID: 27918 RVA: 0x000EC19E File Offset: 0x000EA39E
	public static void DestroyInstance()
	{
		SelectTool.Instance = null;
	}

	// Token: 0x06006D0F RID: 27919 RVA: 0x002F7048 File Offset: 0x002F5248
	protected override void OnPrefabInit()
	{
		this.defaultLayerMask = (1 | LayerMask.GetMask(new string[]
		{
			"World",
			"Pickupable",
			"Place",
			"PlaceWithDepth",
			"BlockSelection",
			"Construction",
			"Selection"
		}));
		this.layerMask = this.defaultLayerMask;
		this.selectMarker = global::Util.KInstantiateUI<SelectMarker>(EntityPrefabs.Instance.SelectMarker, GameScreenManager.Instance.worldSpaceCanvas, false);
		this.selectMarker.gameObject.SetActive(false);
		this.populateHitsList = true;
		SelectTool.Instance = this;
	}

	// Token: 0x06006D10 RID: 27920 RVA: 0x000EC1A6 File Offset: 0x000EA3A6
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
		ToolMenu.Instance.PriorityScreen.ResetPriority();
		this.Select(null, false);
	}

	// Token: 0x06006D11 RID: 27921 RVA: 0x000EC1CA File Offset: 0x000EA3CA
	public void SetLayerMask(int mask)
	{
		this.layerMask = mask;
		base.ClearHover();
		this.LateUpdate();
	}

	// Token: 0x06006D12 RID: 27922 RVA: 0x000EC1DF File Offset: 0x000EA3DF
	public void ClearLayerMask()
	{
		this.layerMask = this.defaultLayerMask;
	}

	// Token: 0x06006D13 RID: 27923 RVA: 0x000EC1ED File Offset: 0x000EA3ED
	public int GetDefaultLayerMask()
	{
		return this.defaultLayerMask;
	}

	// Token: 0x06006D14 RID: 27924 RVA: 0x000EC1F5 File Offset: 0x000EA3F5
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		base.ClearHover();
		this.Select(null, false);
	}

	// Token: 0x06006D15 RID: 27925 RVA: 0x002F70EC File Offset: 0x002F52EC
	public void Focus(Vector3 pos, KSelectable selectable, Vector3 offset)
	{
		if (selectable != null)
		{
			pos = selectable.transform.GetPosition();
		}
		pos.z = -40f;
		pos += offset;
		WorldContainer worldFromPosition = ClusterManager.Instance.GetWorldFromPosition(pos);
		if (worldFromPosition != null)
		{
			GameUtil.FocusCameraOnWorld(worldFromPosition.id, pos, 10f, null, true);
			return;
		}
		DebugUtil.DevLogError("DevError: specified camera focus position has null world - possible out of bounds location");
	}

	// Token: 0x06006D16 RID: 27926 RVA: 0x000EC20C File Offset: 0x000EA40C
	public void SelectAndFocus(Vector3 pos, KSelectable selectable, Vector3 offset)
	{
		this.Focus(pos, selectable, offset);
		this.Select(selectable, false);
	}

	// Token: 0x06006D17 RID: 27927 RVA: 0x000EC21F File Offset: 0x000EA41F
	public void SelectAndFocus(Vector3 pos, KSelectable selectable)
	{
		this.SelectAndFocus(pos, selectable, Vector3.zero);
	}

	// Token: 0x06006D18 RID: 27928 RVA: 0x000EC22E File Offset: 0x000EA42E
	public void SelectNextFrame(KSelectable new_selected, bool skipSound = false)
	{
		this.delayedNextSelection = new_selected;
		this.delayedSkipSound = skipSound;
		UIScheduler.Instance.ScheduleNextFrame("DelayedSelect", new Action<object>(this.DoSelectNextFrame), null, null);
	}

	// Token: 0x06006D19 RID: 27929 RVA: 0x000EC25C File Offset: 0x000EA45C
	private void DoSelectNextFrame(object data)
	{
		this.Select(this.delayedNextSelection, this.delayedSkipSound);
		this.delayedNextSelection = null;
	}

	// Token: 0x06006D1A RID: 27930 RVA: 0x002F7158 File Offset: 0x002F5358
	public void Select(KSelectable new_selected, bool skipSound = false)
	{
		if (new_selected == this.previousSelection)
		{
			return;
		}
		this.previousSelection = new_selected;
		if (this.selected != null)
		{
			this.selected.Unselect();
		}
		GameObject gameObject = null;
		if (new_selected != null && new_selected.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
		{
			SelectToolHoverTextCard component = base.GetComponent<SelectToolHoverTextCard>();
			if (component != null)
			{
				int num = component.currentSelectedSelectableIndex;
				int recentNumberOfDisplayedSelectables = component.recentNumberOfDisplayedSelectables;
				if (recentNumberOfDisplayedSelectables != 0)
				{
					num = (num + 1) % recentNumberOfDisplayedSelectables;
					if (!skipSound)
					{
						if (recentNumberOfDisplayedSelectables == 1)
						{
							KFMOD.PlayUISound(GlobalAssets.GetSound("Select_empty", false));
						}
						else
						{
							EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("Select_full", false), Vector3.zero, 1f);
							instance.setParameterByName("selection", (float)num, false);
							SoundEvent.EndOneShot(instance);
						}
						this.playedSoundThisFrame = true;
					}
				}
			}
			if (new_selected == this.hover)
			{
				base.ClearHover();
			}
			new_selected.Select();
			gameObject = new_selected.gameObject;
			this.selectMarker.SetTargetTransform(gameObject.transform);
			this.selectMarker.gameObject.SetActive(!new_selected.DisableSelectMarker);
		}
		else if (this.selectMarker != null)
		{
			this.selectMarker.gameObject.SetActive(false);
		}
		this.selected = ((gameObject == null) ? null : new_selected);
		Game.Instance.Trigger(-1503271301, gameObject);
	}

	// Token: 0x06006D1B RID: 27931 RVA: 0x002F72C4 File Offset: 0x002F54C4
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		KSelectable objectUnderCursor = base.GetObjectUnderCursor<KSelectable>(true, (KSelectable s) => s.GetComponent<KSelectable>().IsSelectable, this.selected);
		this.selectedCell = Grid.PosToCell(cursor_pos);
		this.Select(objectUnderCursor, false);
		if (DevToolSimDebug.Instance != null)
		{
			DevToolSimDebug.Instance.SetCell(this.selectedCell);
		}
		if (DevToolNavGrid.Instance != null)
		{
			DevToolNavGrid.Instance.SetCell(this.selectedCell);
		}
	}

	// Token: 0x06006D1C RID: 27932 RVA: 0x000EC277 File Offset: 0x000EA477
	public int GetSelectedCell()
	{
		return this.selectedCell;
	}

	// Token: 0x04005241 RID: 21057
	public KSelectable selected;

	// Token: 0x04005242 RID: 21058
	protected int cell_new;

	// Token: 0x04005243 RID: 21059
	private int selectedCell;

	// Token: 0x04005244 RID: 21060
	protected int defaultLayerMask;

	// Token: 0x04005245 RID: 21061
	public static SelectTool Instance;

	// Token: 0x04005246 RID: 21062
	private KSelectable delayedNextSelection;

	// Token: 0x04005247 RID: 21063
	private bool delayedSkipSound;

	// Token: 0x04005248 RID: 21064
	private KSelectable previousSelection;
}
