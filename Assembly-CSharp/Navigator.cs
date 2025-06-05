using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using STRINGS;
using UnityEngine;

// Token: 0x02000AD7 RID: 2775
public class Navigator : StateMachineComponent<Navigator.StatesInstance>, ISaveLoadableDetails
{
	// Token: 0x17000213 RID: 531
	// (get) Token: 0x060032ED RID: 13037 RVA: 0x000C58E8 File Offset: 0x000C3AE8
	// (set) Token: 0x060032EE RID: 13038 RVA: 0x000C58F0 File Offset: 0x000C3AF0
	public KMonoBehaviour target { get; set; }

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x060032EF RID: 13039 RVA: 0x000C58F9 File Offset: 0x000C3AF9
	// (set) Token: 0x060032F0 RID: 13040 RVA: 0x000C5901 File Offset: 0x000C3B01
	public CellOffset[] targetOffsets { get; private set; }

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x060032F1 RID: 13041 RVA: 0x000C590A File Offset: 0x000C3B0A
	// (set) Token: 0x060032F2 RID: 13042 RVA: 0x000C5912 File Offset: 0x000C3B12
	public NavGrid NavGrid { get; private set; }

	// Token: 0x060032F3 RID: 13043 RVA: 0x00212954 File Offset: 0x00210B54
	public void Serialize(BinaryWriter writer)
	{
		byte currentNavType = (byte)this.CurrentNavType;
		writer.Write(currentNavType);
		writer.Write(this.distanceTravelledByNavType.Count);
		foreach (KeyValuePair<NavType, int> keyValuePair in this.distanceTravelledByNavType)
		{
			byte key = (byte)keyValuePair.Key;
			writer.Write(key);
			writer.Write(keyValuePair.Value);
		}
	}

	// Token: 0x060032F4 RID: 13044 RVA: 0x002129DC File Offset: 0x00210BDC
	public void Deserialize(IReader reader)
	{
		NavType navType = (NavType)reader.ReadByte();
		if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 11))
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				NavType key = (NavType)reader.ReadByte();
				int value = reader.ReadInt32();
				if (this.distanceTravelledByNavType.ContainsKey(key))
				{
					this.distanceTravelledByNavType[key] = value;
				}
			}
		}
		bool flag = false;
		NavType[] validNavTypes = this.NavGrid.ValidNavTypes;
		for (int j = 0; j < validNavTypes.Length; j++)
		{
			if (validNavTypes[j] == navType)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.CurrentNavType = navType;
		}
	}

	// Token: 0x060032F5 RID: 13045 RVA: 0x00212A84 File Offset: 0x00210C84
	protected override void OnPrefabInit()
	{
		this.transitionDriver = new TransitionDriver(this);
		this.targetLocator = Util.KInstantiate(Assets.GetPrefab(TargetLocator.ID), null, null).GetComponent<KPrefabID>();
		this.targetLocator.gameObject.SetActive(true);
		this.log = new LoggerFSS("Navigator", 35);
		this.simRenderLoadBalance = true;
		this.autoRegisterSimRender = false;
		this.NavGrid = Pathfinding.Instance.GetNavGrid(this.NavGridName);
		base.GetComponent<PathProber>().SetValidNavTypes(this.NavGrid.ValidNavTypes, this.maxProbingRadius);
		this.distanceTravelledByNavType = new Dictionary<NavType, int>();
		for (int i = 0; i < 11; i++)
		{
			this.distanceTravelledByNavType.Add((NavType)i, 0);
		}
	}

	// Token: 0x060032F6 RID: 13046 RVA: 0x00212B48 File Offset: 0x00210D48
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Navigator>(1623392196, Navigator.OnDefeatedDelegate);
		base.Subscribe<Navigator>(-1506500077, Navigator.OnDefeatedDelegate);
		base.Subscribe<Navigator>(493375141, Navigator.OnRefreshUserMenuDelegate);
		base.Subscribe<Navigator>(-1503271301, Navigator.OnSelectObjectDelegate);
		base.Subscribe<Navigator>(856640610, Navigator.OnStoreDelegate);
		if (this.updateProber)
		{
			SimAndRenderScheduler.instance.Add(this, false);
		}
		this.pathProbeTask = new Navigator.PathProbeTask(this);
		this.SetCurrentNavType(this.CurrentNavType);
		this.SubscribeUnstuckFunctions();
	}

	// Token: 0x060032F7 RID: 13047 RVA: 0x000C591B File Offset: 0x000C3B1B
	private void SubscribeUnstuckFunctions()
	{
		if (this.CurrentNavType == NavType.Tube)
		{
			GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingTileChanged));
		}
	}

	// Token: 0x060032F8 RID: 13048 RVA: 0x000C5948 File Offset: 0x000C3B48
	private void UnsubscribeUnstuckFunctions()
	{
		GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingTileChanged));
	}

	// Token: 0x060032F9 RID: 13049 RVA: 0x00212BE4 File Offset: 0x00210DE4
	private void OnBuildingTileChanged(int cell, object building)
	{
		if (this.CurrentNavType == NavType.Tube && building == null)
		{
			bool flag = cell == Grid.PosToCell(this);
			if (base.smi != null && flag)
			{
				this.SetCurrentNavType(NavType.Floor);
				this.UnsubscribeUnstuckFunctions();
			}
		}
	}

	// Token: 0x060032FA RID: 13050 RVA: 0x000C596C File Offset: 0x000C3B6C
	protected override void OnCleanUp()
	{
		this.UnsubscribeUnstuckFunctions();
		base.OnCleanUp();
	}

	// Token: 0x060032FB RID: 13051 RVA: 0x000C597A File Offset: 0x000C3B7A
	public bool IsMoving()
	{
		return base.smi.IsInsideState(base.smi.sm.normal.moving);
	}

	// Token: 0x060032FC RID: 13052 RVA: 0x000C599C File Offset: 0x000C3B9C
	public bool GoTo(int cell, CellOffset[] offsets = null)
	{
		if (offsets == null)
		{
			offsets = new CellOffset[1];
		}
		this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		return this.GoTo(this.targetLocator, offsets, NavigationTactics.ReduceTravelDistance);
	}

	// Token: 0x060032FD RID: 13053 RVA: 0x000C59D4 File Offset: 0x000C3BD4
	public bool GoTo(int cell, CellOffset[] offsets, NavTactic tactic)
	{
		if (offsets == null)
		{
			offsets = new CellOffset[1];
		}
		this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		return this.GoTo(this.targetLocator, offsets, tactic);
	}

	// Token: 0x060032FE RID: 13054 RVA: 0x000C5A08 File Offset: 0x000C3C08
	public void UpdateTarget(int cell)
	{
		this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
	}

	// Token: 0x060032FF RID: 13055 RVA: 0x00212C24 File Offset: 0x00210E24
	public bool GoTo(KMonoBehaviour target, CellOffset[] offsets, NavTactic tactic)
	{
		if (tactic == null)
		{
			tactic = NavigationTactics.ReduceTravelDistance;
		}
		base.smi.GoTo(base.smi.sm.normal.moving);
		base.smi.sm.moveTarget.Set(target.gameObject, base.smi, false);
		this.tactic = tactic;
		this.target = target;
		this.targetOffsets = offsets;
		this.ClearReservedCell();
		this.AdvancePath(true);
		return this.IsMoving();
	}

	// Token: 0x06003300 RID: 13056 RVA: 0x000C5A23 File Offset: 0x000C3C23
	public void BeginTransition(NavGrid.Transition transition)
	{
		this.transitionDriver.EndTransition();
		base.smi.GoTo(base.smi.sm.normal.moving);
		this.transitionDriver.BeginTransition(this, transition, this.defaultSpeed);
	}

	// Token: 0x06003301 RID: 13057 RVA: 0x00212CA8 File Offset: 0x00210EA8
	private bool ValidatePath(ref PathFinder.Path path, out bool atNextNode)
	{
		atNextNode = false;
		bool flag = false;
		if (path.IsValid())
		{
			int target_cell = Grid.PosToCell(this.target);
			flag = (this.reservedCell != NavigationReservations.InvalidReservation && this.CanReach(this.reservedCell));
			flag &= Grid.IsCellOffsetOf(this.reservedCell, target_cell, this.targetOffsets);
		}
		if (flag)
		{
			int num = Grid.PosToCell(this);
			flag = (num == path.nodes[0].cell && this.CurrentNavType == path.nodes[0].navType);
			flag |= (atNextNode = (num == path.nodes[1].cell && this.CurrentNavType == path.nodes[1].navType));
		}
		if (!flag)
		{
			return false;
		}
		PathFinderAbilities currentAbilities = this.GetCurrentAbilities();
		return PathFinder.ValidatePath(this.NavGrid, currentAbilities, ref path);
	}

	// Token: 0x06003302 RID: 13058 RVA: 0x00212D90 File Offset: 0x00210F90
	public void AdvancePath(bool trigger_advance = true)
	{
		int num = Grid.PosToCell(this);
		if (this.target == null)
		{
			base.Trigger(-766531887, null);
			this.Stop(false, true);
		}
		else if (num == this.reservedCell && this.CurrentNavType != NavType.Tube)
		{
			this.Stop(true, true);
		}
		else
		{
			bool flag2;
			bool flag = !this.ValidatePath(ref this.path, out flag2);
			if (flag2)
			{
				this.path.nodes.RemoveAt(0);
			}
			if (flag)
			{
				int root = Grid.PosToCell(this.target);
				int cellPreferences = this.tactic.GetCellPreferences(root, this.targetOffsets, this);
				this.SetReservedCell(cellPreferences);
				if (this.reservedCell == NavigationReservations.InvalidReservation)
				{
					this.Stop(false, true);
				}
				else
				{
					PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(num, this.CurrentNavType, this.flags);
					PathFinder.UpdatePath(this.NavGrid, this.GetCurrentAbilities(), potential_path, PathFinderQueries.cellQuery.Reset(this.reservedCell), ref this.path);
				}
			}
			if (this.path.IsValid())
			{
				this.BeginTransition(this.NavGrid.transitions[(int)this.path.nodes[1].transitionId]);
				this.distanceTravelledByNavType[this.CurrentNavType] = Mathf.Max(this.distanceTravelledByNavType[this.CurrentNavType] + 1, this.distanceTravelledByNavType[this.CurrentNavType]);
			}
			else if (this.path.HasArrived())
			{
				this.Stop(true, true);
			}
			else
			{
				this.ClearReservedCell();
				this.Stop(false, true);
			}
		}
		if (trigger_advance)
		{
			base.Trigger(1347184327, null);
		}
	}

	// Token: 0x06003303 RID: 13059 RVA: 0x000C5A63 File Offset: 0x000C3C63
	public NavGrid.Transition GetNextTransition()
	{
		return this.NavGrid.transitions[(int)this.path.nodes[1].transitionId];
	}

	// Token: 0x06003304 RID: 13060 RVA: 0x00212F38 File Offset: 0x00211138
	public void Stop(bool arrived_at_destination = false, bool play_idle = true)
	{
		this.target = null;
		this.targetOffsets = null;
		this.path.Clear();
		base.smi.sm.moveTarget.Set(null, base.smi);
		this.transitionDriver.EndTransition();
		if (play_idle)
		{
			HashedString idleAnim = this.NavGrid.GetIdleAnim(this.CurrentNavType);
			this.animController.Play(idleAnim, KAnim.PlayMode.Loop, 1f, 0f);
		}
		if (arrived_at_destination)
		{
			base.smi.GoTo(base.smi.sm.normal.arrived);
			return;
		}
		if (base.smi.GetCurrentState() == base.smi.sm.normal.moving)
		{
			this.ClearReservedCell();
			base.smi.GoTo(base.smi.sm.normal.failed);
		}
	}

	// Token: 0x06003305 RID: 13061 RVA: 0x000C5A8B File Offset: 0x000C3C8B
	private void SimEveryTick(float dt)
	{
		if (this.IsMoving())
		{
			this.transitionDriver.UpdateTransition(dt);
		}
	}

	// Token: 0x06003306 RID: 13062 RVA: 0x000C5AA1 File Offset: 0x000C3CA1
	public void Sim4000ms(float dt)
	{
		this.UpdateProbe(true);
	}

	// Token: 0x06003307 RID: 13063 RVA: 0x000C5AAA File Offset: 0x000C3CAA
	public void UpdateProbe(bool forceUpdate = false)
	{
		if (forceUpdate || !this.executePathProbeTaskAsync)
		{
			this.pathProbeTask.Update();
			this.pathProbeTask.Run(null, 0);
		}
	}

	// Token: 0x06003308 RID: 13064 RVA: 0x000C5ACF File Offset: 0x000C3CCF
	public void DrawPath()
	{
		if (base.gameObject.activeInHierarchy && this.IsMoving())
		{
			NavPathDrawer.Instance.DrawPath(this.animController.GetPivotSymbolPosition(), this.path);
		}
	}

	// Token: 0x06003309 RID: 13065 RVA: 0x000C5B01 File Offset: 0x000C3D01
	public void Pause(string reason)
	{
		base.smi.sm.isPaused.Set(true, base.smi, false);
	}

	// Token: 0x0600330A RID: 13066 RVA: 0x000C5B21 File Offset: 0x000C3D21
	public void Unpause(string reason)
	{
		base.smi.sm.isPaused.Set(false, base.smi, false);
	}

	// Token: 0x0600330B RID: 13067 RVA: 0x000C5B41 File Offset: 0x000C3D41
	private void OnDefeated(object data)
	{
		this.ClearReservedCell();
		this.Stop(false, false);
	}

	// Token: 0x0600330C RID: 13068 RVA: 0x000C5B51 File Offset: 0x000C3D51
	private void ClearReservedCell()
	{
		if (this.reservedCell != NavigationReservations.InvalidReservation)
		{
			NavigationReservations.Instance.RemoveOccupancy(this.reservedCell);
			this.reservedCell = NavigationReservations.InvalidReservation;
		}
	}

	// Token: 0x0600330D RID: 13069 RVA: 0x000C5B7B File Offset: 0x000C3D7B
	private void SetReservedCell(int cell)
	{
		this.ClearReservedCell();
		this.reservedCell = cell;
		NavigationReservations.Instance.AddOccupancy(cell);
	}

	// Token: 0x0600330E RID: 13070 RVA: 0x000C5B95 File Offset: 0x000C3D95
	public int GetReservedCell()
	{
		return this.reservedCell;
	}

	// Token: 0x0600330F RID: 13071 RVA: 0x000C5B9D File Offset: 0x000C3D9D
	public int GetAnchorCell()
	{
		return this.AnchorCell;
	}

	// Token: 0x06003310 RID: 13072 RVA: 0x000C5BA5 File Offset: 0x000C3DA5
	public bool IsValidNavType(NavType nav_type)
	{
		return this.NavGrid.HasNavTypeData(nav_type);
	}

	// Token: 0x06003311 RID: 13073 RVA: 0x00213020 File Offset: 0x00211220
	public void SetCurrentNavType(NavType nav_type)
	{
		this.CurrentNavType = nav_type;
		this.AnchorCell = NavTypeHelper.GetAnchorCell(nav_type, Grid.PosToCell(this));
		NavGrid.NavTypeData navTypeData = this.NavGrid.GetNavTypeData(this.CurrentNavType);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Vector2 one = Vector2.one;
		if (navTypeData.flipX)
		{
			one.x = -1f;
		}
		if (navTypeData.flipY)
		{
			one.y = -1f;
		}
		component.navMatrix = Matrix2x3.Translate(navTypeData.animControllerOffset * 200f) * Matrix2x3.Rotate(navTypeData.rotation) * Matrix2x3.Scale(one);
	}

	// Token: 0x06003312 RID: 13074 RVA: 0x002130C4 File Offset: 0x002112C4
	private void OnRefreshUserMenu(object data)
	{
		if (base.gameObject.HasTag(GameTags.Dead))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (NavPathDrawer.Instance.GetNavigator() != this) ? new KIconButtonMenu.ButtonInfo("action_navigable_regions", UI.USERMENUACTIONS.DRAWPATHS.NAME, new System.Action(this.OnDrawPaths), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DRAWPATHS.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_navigable_regions", UI.USERMENUACTIONS.DRAWPATHS.NAME_OFF, new System.Action(this.OnDrawPaths), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DRAWPATHS.TOOLTIP_OFF, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 0.1f);
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_follow_cam", UI.USERMENUACTIONS.FOLLOWCAM.NAME, new System.Action(this.OnFollowCam), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.FOLLOWCAM.TOOLTIP, true), 0.3f);
	}

	// Token: 0x06003313 RID: 13075 RVA: 0x000C5BB3 File Offset: 0x000C3DB3
	private void OnFollowCam()
	{
		if (CameraController.Instance.followTarget == base.transform)
		{
			CameraController.Instance.ClearFollowTarget();
			return;
		}
		CameraController.Instance.SetFollowTarget(base.transform);
	}

	// Token: 0x06003314 RID: 13076 RVA: 0x000C5BE7 File Offset: 0x000C3DE7
	private void OnDrawPaths()
	{
		if (NavPathDrawer.Instance.GetNavigator() != this)
		{
			NavPathDrawer.Instance.SetNavigator(this);
			return;
		}
		NavPathDrawer.Instance.ClearNavigator();
	}

	// Token: 0x06003315 RID: 13077 RVA: 0x000C5C11 File Offset: 0x000C3E11
	private void OnSelectObject(object data)
	{
		NavPathDrawer.Instance.ClearNavigator();
	}

	// Token: 0x06003316 RID: 13078 RVA: 0x000C5C1D File Offset: 0x000C3E1D
	public void OnStore(object data)
	{
		if (data is Storage || (data != null && (bool)data))
		{
			this.Stop(false, true);
		}
	}

	// Token: 0x06003317 RID: 13079 RVA: 0x000C5C40 File Offset: 0x000C3E40
	public PathFinderAbilities GetCurrentAbilities()
	{
		this.abilities.Refresh();
		return this.abilities;
	}

	// Token: 0x06003318 RID: 13080 RVA: 0x000C5C53 File Offset: 0x000C3E53
	public void SetAbilities(PathFinderAbilities abilities)
	{
		this.abilities = abilities;
	}

	// Token: 0x06003319 RID: 13081 RVA: 0x000C5C5C File Offset: 0x000C3E5C
	public bool CanReach(IApproachable approachable)
	{
		return this.CanReach(approachable.GetCell(), approachable.GetOffsets());
	}

	// Token: 0x0600331A RID: 13082 RVA: 0x002131C8 File Offset: 0x002113C8
	public bool CanReach(int cell, CellOffset[] offsets)
	{
		foreach (CellOffset offset in offsets)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			if (this.CanReach(cell2))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600331B RID: 13083 RVA: 0x000C5C70 File Offset: 0x000C3E70
	public bool CanReach(int cell)
	{
		return this.GetNavigationCost(cell) != -1;
	}

	// Token: 0x0600331C RID: 13084 RVA: 0x000C5C7F File Offset: 0x000C3E7F
	public int GetNavigationCost(int cell)
	{
		if (Grid.IsValidCell(cell))
		{
			return this.PathProber.GetCost(cell);
		}
		return -1;
	}

	// Token: 0x0600331D RID: 13085 RVA: 0x000C5C97 File Offset: 0x000C3E97
	public int GetNavigationCostIgnoreProberOffset(int cell, CellOffset[] offsets)
	{
		return this.PathProber.GetNavigationCostIgnoreProberOffset(cell, offsets);
	}

	// Token: 0x0600331E RID: 13086 RVA: 0x00213204 File Offset: 0x00211404
	public int GetNavigationCost(int cell, CellOffset[] offsets)
	{
		int num = -1;
		int num2 = offsets.Length;
		for (int i = 0; i < num2; i++)
		{
			int cell2 = Grid.OffsetCell(cell, offsets[i]);
			int navigationCost = this.GetNavigationCost(cell2);
			if (navigationCost != -1 && (num == -1 || navigationCost < num))
			{
				num = navigationCost;
			}
		}
		return num;
	}

	// Token: 0x0600331F RID: 13087 RVA: 0x000C5CA6 File Offset: 0x000C3EA6
	public int GetNavigationCost(IApproachable approachable)
	{
		return this.GetNavigationCost(approachable.GetCell(), approachable.GetOffsets());
	}

	// Token: 0x06003320 RID: 13088 RVA: 0x0021324C File Offset: 0x0021144C
	public void RunQuery(PathFinderQuery query)
	{
		int cell = Grid.PosToCell(this);
		PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(cell, this.CurrentNavType, this.flags);
		PathFinder.Run(this.NavGrid, this.GetCurrentAbilities(), potential_path, query);
	}

	// Token: 0x06003321 RID: 13089 RVA: 0x000C5CBA File Offset: 0x000C3EBA
	public void SetFlags(PathFinder.PotentialPath.Flags new_flags)
	{
		this.flags |= new_flags;
	}

	// Token: 0x06003322 RID: 13090 RVA: 0x000C5CCA File Offset: 0x000C3ECA
	public void ClearFlags(PathFinder.PotentialPath.Flags new_flags)
	{
		this.flags &= ~new_flags;
	}

	// Token: 0x06003323 RID: 13091 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_DETAILED_NAVIGATOR_PROFILE_INFO")]
	public static void BeginDetailedSample(string region_name)
	{
	}

	// Token: 0x06003324 RID: 13092 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_DETAILED_NAVIGATOR_PROFILE_INFO")]
	public static void EndDetailedSample(string region_name)
	{
	}

	// Token: 0x040022D1 RID: 8913
	public bool DebugDrawPath;

	// Token: 0x040022D5 RID: 8917
	[MyCmpAdd]
	public PathProber PathProber;

	// Token: 0x040022D6 RID: 8918
	[MyCmpAdd]
	public Facing facing;

	// Token: 0x040022D7 RID: 8919
	public float defaultSpeed = 1f;

	// Token: 0x040022D8 RID: 8920
	public TransitionDriver transitionDriver;

	// Token: 0x040022D9 RID: 8921
	public string NavGridName;

	// Token: 0x040022DA RID: 8922
	public bool updateProber;

	// Token: 0x040022DB RID: 8923
	public int maxProbingRadius;

	// Token: 0x040022DC RID: 8924
	public PathFinder.PotentialPath.Flags flags;

	// Token: 0x040022DD RID: 8925
	private LoggerFSS log;

	// Token: 0x040022DE RID: 8926
	public Dictionary<NavType, int> distanceTravelledByNavType;

	// Token: 0x040022DF RID: 8927
	public Grid.SceneLayer sceneLayer = Grid.SceneLayer.Move;

	// Token: 0x040022E0 RID: 8928
	private PathFinderAbilities abilities;

	// Token: 0x040022E1 RID: 8929
	[MyCmpReq]
	public KBatchedAnimController animController;

	// Token: 0x040022E2 RID: 8930
	[NonSerialized]
	public PathFinder.Path path;

	// Token: 0x040022E3 RID: 8931
	public NavType CurrentNavType;

	// Token: 0x040022E4 RID: 8932
	private int AnchorCell;

	// Token: 0x040022E5 RID: 8933
	private KPrefabID targetLocator;

	// Token: 0x040022E6 RID: 8934
	private int reservedCell = NavigationReservations.InvalidReservation;

	// Token: 0x040022E7 RID: 8935
	private NavTactic tactic;

	// Token: 0x040022E8 RID: 8936
	public Navigator.PathProbeTask pathProbeTask;

	// Token: 0x040022E9 RID: 8937
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnDefeatedDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnDefeated(data);
	});

	// Token: 0x040022EA RID: 8938
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040022EB RID: 8939
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnSelectObjectDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnSelectObject(data);
	});

	// Token: 0x040022EC RID: 8940
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnStoreDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x040022ED RID: 8941
	public bool executePathProbeTaskAsync;

	// Token: 0x02000AD8 RID: 2776
	public class ActiveTransition
	{
		// Token: 0x06003327 RID: 13095 RVA: 0x00213300 File Offset: 0x00211500
		public void Init(NavGrid.Transition transition, float default_speed)
		{
			this.x = transition.x;
			this.y = transition.y;
			this.isLooping = transition.isLooping;
			this.start = transition.start;
			this.end = transition.end;
			this.preAnim = transition.preAnim;
			this.anim = transition.anim;
			this.speed = default_speed;
			this.animSpeed = transition.animSpeed;
			this.navGridTransition = transition;
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x00213388 File Offset: 0x00211588
		public void Copy(Navigator.ActiveTransition other)
		{
			this.x = other.x;
			this.y = other.y;
			this.isLooping = other.isLooping;
			this.start = other.start;
			this.end = other.end;
			this.preAnim = other.preAnim;
			this.anim = other.anim;
			this.speed = other.speed;
			this.animSpeed = other.animSpeed;
			this.navGridTransition = other.navGridTransition;
		}

		// Token: 0x040022EE RID: 8942
		public int x;

		// Token: 0x040022EF RID: 8943
		public int y;

		// Token: 0x040022F0 RID: 8944
		public bool isLooping;

		// Token: 0x040022F1 RID: 8945
		public NavType start;

		// Token: 0x040022F2 RID: 8946
		public NavType end;

		// Token: 0x040022F3 RID: 8947
		public HashedString preAnim;

		// Token: 0x040022F4 RID: 8948
		public HashedString anim;

		// Token: 0x040022F5 RID: 8949
		public float speed;

		// Token: 0x040022F6 RID: 8950
		public float animSpeed = 1f;

		// Token: 0x040022F7 RID: 8951
		public Func<bool> isCompleteCB;

		// Token: 0x040022F8 RID: 8952
		public NavGrid.Transition navGridTransition;
	}

	// Token: 0x02000AD9 RID: 2777
	public class StatesInstance : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.GameInstance
	{
		// Token: 0x0600332A RID: 13098 RVA: 0x000C5D15 File Offset: 0x000C3F15
		public StatesInstance(Navigator master) : base(master)
		{
		}
	}

	// Token: 0x02000ADA RID: 2778
	public class States : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator>
	{
		// Token: 0x0600332B RID: 13099 RVA: 0x00213410 File Offset: 0x00211610
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.normal.stopped;
			this.saveHistory = true;
			this.normal.ParamTransition<bool>(this.isPaused, this.paused, GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.IsTrue).Update("NavigatorProber", delegate(Navigator.StatesInstance smi, float dt)
			{
				smi.master.Sim4000ms(dt);
			}, UpdateRate.SIM_4000ms, false);
			this.normal.moving.Enter(delegate(Navigator.StatesInstance smi)
			{
				smi.Trigger(1027377649, GameHashes.ObjectMovementWakeUp);
			}).Update("UpdateNavigator", delegate(Navigator.StatesInstance smi, float dt)
			{
				smi.master.SimEveryTick(dt);
			}, UpdateRate.SIM_EVERY_TICK, true).Exit(delegate(Navigator.StatesInstance smi)
			{
				smi.Trigger(1027377649, GameHashes.ObjectMovementSleep);
			});
			this.normal.arrived.TriggerOnEnter(GameHashes.DestinationReached, null).GoTo(this.normal.stopped);
			this.normal.failed.TriggerOnEnter(GameHashes.NavigationFailed, null).GoTo(this.normal.stopped);
			this.normal.stopped.Enter(delegate(Navigator.StatesInstance smi)
			{
				smi.master.SubscribeUnstuckFunctions();
			}).DoNothing().Exit(delegate(Navigator.StatesInstance smi)
			{
				smi.master.UnsubscribeUnstuckFunctions();
			});
			this.paused.ParamTransition<bool>(this.isPaused, this.normal, GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.IsFalse);
		}

		// Token: 0x040022F9 RID: 8953
		public StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.TargetParameter moveTarget;

		// Token: 0x040022FA RID: 8954
		public StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.BoolParameter isPaused = new StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.BoolParameter(false);

		// Token: 0x040022FB RID: 8955
		public Navigator.States.NormalStates normal;

		// Token: 0x040022FC RID: 8956
		public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State paused;

		// Token: 0x02000ADB RID: 2779
		public class NormalStates : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State
		{
			// Token: 0x040022FD RID: 8957
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State moving;

			// Token: 0x040022FE RID: 8958
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State arrived;

			// Token: 0x040022FF RID: 8959
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State failed;

			// Token: 0x04002300 RID: 8960
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State stopped;
		}
	}

	// Token: 0x02000ADD RID: 2781
	public struct PathProbeTask : IWorkItem<object>
	{
		// Token: 0x06003336 RID: 13110 RVA: 0x000C5DAA File Offset: 0x000C3FAA
		public PathProbeTask(Navigator navigator)
		{
			this.navigator = navigator;
			this.cell = -1;
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x000C5DBA File Offset: 0x000C3FBA
		public void Update()
		{
			this.cell = Grid.PosToCell(this.navigator);
			this.navigator.abilities.Refresh();
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x002135BC File Offset: 0x002117BC
		public void Run(object sharedData, int threadIndex)
		{
			this.navigator.PathProber.UpdateProbe(this.navigator.NavGrid, this.cell, this.navigator.CurrentNavType, this.navigator.abilities, this.navigator.flags);
		}

		// Token: 0x04002308 RID: 8968
		private int cell;

		// Token: 0x04002309 RID: 8969
		private Navigator navigator;
	}
}
