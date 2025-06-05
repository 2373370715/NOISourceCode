using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001043 RID: 4163
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/TravelTube")]
public class TravelTube : KMonoBehaviour, IFirstFrameCallback, ITravelTubePiece, IHaveUtilityNetworkMgr
{
	// Token: 0x06005473 RID: 21619 RVA: 0x000DB711 File Offset: 0x000D9911
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.travelTubeSystem;
	}

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x06005474 RID: 21620 RVA: 0x000C656E File Offset: 0x000C476E
	public Vector3 Position
	{
		get
		{
			return base.transform.GetPosition();
		}
	}

	// Token: 0x06005475 RID: 21621 RVA: 0x000DB71D File Offset: 0x000D991D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Grid.HasTube[Grid.PosToCell(this)] = true;
		Components.ITravelTubePieces.Add(this);
	}

	// Token: 0x06005476 RID: 21622 RVA: 0x002896B8 File Offset: 0x002878B8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Game.Instance.travelTubeSystem.AddToNetworks(cell, this, false);
		base.Subscribe<TravelTube>(-1041684577, TravelTube.OnConnectionsChangedDelegate);
	}

	// Token: 0x06005477 RID: 21623 RVA: 0x00289700 File Offset: 0x00287900
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			Game.Instance.travelTubeSystem.RemoveFromNetworks(cell, this, false);
		}
		base.Unsubscribe(-1041684577);
		Grid.HasTube[Grid.PosToCell(this)] = false;
		Components.ITravelTubePieces.Remove(this);
		GameScenePartitioner.Instance.Free(ref this.dirtyNavCellUpdatedEntry);
		base.OnCleanUp();
	}

	// Token: 0x06005478 RID: 21624 RVA: 0x002897A4 File Offset: 0x002879A4
	private void OnConnectionsChanged(object data)
	{
		this.connections = (UtilityConnections)data;
		bool flag = this.connections == UtilityConnections.Up || this.connections == UtilityConnections.Down || this.connections == UtilityConnections.Left || this.connections == UtilityConnections.Right;
		if (flag != this.isExitTube)
		{
			this.isExitTube = flag;
			this.UpdateExitListener(this.isExitTube);
			this.UpdateExitStatus();
		}
	}

	// Token: 0x06005479 RID: 21625 RVA: 0x00289808 File Offset: 0x00287A08
	private void UpdateExitListener(bool enable)
	{
		if (enable && !this.dirtyNavCellUpdatedEntry.IsValid())
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.dirtyNavCellUpdatedEntry = GameScenePartitioner.Instance.Add("TravelTube.OnDirtyNavCellUpdated", this, cell, GameScenePartitioner.Instance.dirtyNavCellUpdateLayer, new Action<object>(this.OnDirtyNavCellUpdated));
			this.OnDirtyNavCellUpdated(null);
			return;
		}
		if (!enable && this.dirtyNavCellUpdatedEntry.IsValid())
		{
			GameScenePartitioner.Instance.Free(ref this.dirtyNavCellUpdatedEntry);
		}
	}

	// Token: 0x0600547A RID: 21626 RVA: 0x0028988C File Offset: 0x00287A8C
	private void OnDirtyNavCellUpdated(object data)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		NavGrid navGrid = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
		int num2 = num * navGrid.maxLinksPerCell;
		bool flag = false;
		if (this.isExitTube)
		{
			NavGrid.Link link = navGrid.Links[num2];
			while (link.link != PathFinder.InvalidHandle)
			{
				if (link.startNavType == NavType.Tube)
				{
					if (link.endNavType != NavType.Tube)
					{
						flag = true;
						break;
					}
					UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(link.link, num);
					if (this.connections == utilityConnections)
					{
						flag = true;
						break;
					}
				}
				num2++;
				link = navGrid.Links[num2];
			}
		}
		if (flag != this.hasValidExitTransitions)
		{
			this.hasValidExitTransitions = flag;
			this.UpdateExitStatus();
		}
	}

	// Token: 0x0600547B RID: 21627 RVA: 0x00289948 File Offset: 0x00287B48
	private void UpdateExitStatus()
	{
		if (!this.isExitTube || this.hasValidExitTransitions)
		{
			this.connectedStatus = this.selectable.RemoveStatusItem(this.connectedStatus, false);
			return;
		}
		if (this.connectedStatus == Guid.Empty)
		{
			this.connectedStatus = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NoTubeExits, null);
		}
	}

	// Token: 0x0600547C RID: 21628 RVA: 0x000DB741 File Offset: 0x000D9941
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x0600547D RID: 21629 RVA: 0x000DB757 File Offset: 0x000D9957
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x04003B8D RID: 15245
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04003B8E RID: 15246
	private HandleVector<int>.Handle dirtyNavCellUpdatedEntry;

	// Token: 0x04003B8F RID: 15247
	private bool isExitTube;

	// Token: 0x04003B90 RID: 15248
	private bool hasValidExitTransitions;

	// Token: 0x04003B91 RID: 15249
	private UtilityConnections connections;

	// Token: 0x04003B92 RID: 15250
	private static readonly EventSystem.IntraObjectHandler<TravelTube> OnConnectionsChangedDelegate = new EventSystem.IntraObjectHandler<TravelTube>(delegate(TravelTube component, object data)
	{
		component.OnConnectionsChanged(data);
	});

	// Token: 0x04003B93 RID: 15251
	private Guid connectedStatus;

	// Token: 0x04003B94 RID: 15252
	private System.Action firstFrameCallback;
}
