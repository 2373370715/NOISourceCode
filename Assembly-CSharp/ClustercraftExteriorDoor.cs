using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200192C RID: 6444
public class ClustercraftExteriorDoor : KMonoBehaviour
{
	// Token: 0x060085D2 RID: 34258 RVA: 0x0035738C File Offset: 0x0035558C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.targetWorldId < 0)
		{
			GameObject gameObject = base.GetComponent<RocketModuleCluster>().CraftInterface.gameObject;
			WorldContainer worldContainer = ClusterManager.Instance.CreateRocketInteriorWorld(gameObject, this.interiorTemplateName, delegate
			{
				this.PairWithInteriorDoor();
			});
			if (worldContainer != null)
			{
				this.targetWorldId = worldContainer.id;
			}
		}
		else
		{
			this.PairWithInteriorDoor();
		}
		base.Subscribe<ClustercraftExteriorDoor>(-1277991738, ClustercraftExteriorDoor.OnLaunchDelegate);
		base.Subscribe<ClustercraftExteriorDoor>(-887025858, ClustercraftExteriorDoor.OnLandDelegate);
	}

	// Token: 0x060085D3 RID: 34259 RVA: 0x000FC66D File Offset: 0x000FA86D
	protected override void OnCleanUp()
	{
		ClusterManager.Instance.DestoryRocketInteriorWorld(this.targetWorldId, this);
		base.OnCleanUp();
	}

	// Token: 0x060085D4 RID: 34260 RVA: 0x00357418 File Offset: 0x00355618
	private void PairWithInteriorDoor()
	{
		foreach (object obj in Components.ClusterCraftInteriorDoors)
		{
			ClustercraftInteriorDoor clustercraftInteriorDoor = (ClustercraftInteriorDoor)obj;
			if (clustercraftInteriorDoor.GetMyWorldId() == this.targetWorldId)
			{
				this.SetTarget(clustercraftInteriorDoor);
				break;
			}
		}
		if (this.targetDoor == null)
		{
			global::Debug.LogWarning("No ClusterCraftInteriorDoor found on world");
		}
		WorldContainer targetWorld = this.GetTargetWorld();
		int myWorldId = this.GetMyWorldId();
		if (targetWorld != null && myWorldId != -1)
		{
			targetWorld.SetParentIdx(myWorldId);
		}
		if (base.gameObject.GetComponent<KSelectable>().IsSelected)
		{
			RocketModuleSideScreen.instance.UpdateButtonStates();
		}
		base.Trigger(-1118736034, null);
		targetWorld.gameObject.Trigger(-1118736034, null);
	}

	// Token: 0x060085D5 RID: 34261 RVA: 0x000FC686 File Offset: 0x000FA886
	public void SetTarget(ClustercraftInteriorDoor target)
	{
		this.targetDoor = target;
		target.GetComponent<AssignmentGroupController>().SetGroupID(base.GetComponent<AssignmentGroupController>().AssignmentGroupID);
		base.GetComponent<NavTeleporter>().TwoWayTarget(target.GetComponent<NavTeleporter>());
	}

	// Token: 0x060085D6 RID: 34262 RVA: 0x000FC6B6 File Offset: 0x000FA8B6
	public bool HasTargetWorld()
	{
		return this.targetDoor != null;
	}

	// Token: 0x060085D7 RID: 34263 RVA: 0x000FC6C4 File Offset: 0x000FA8C4
	public WorldContainer GetTargetWorld()
	{
		global::Debug.Assert(this.targetDoor != null, "Clustercraft Exterior Door has no targetDoor");
		return this.targetDoor.GetMyWorld();
	}

	// Token: 0x060085D8 RID: 34264 RVA: 0x003574F8 File Offset: 0x003556F8
	public void FerryMinion(GameObject minion)
	{
		Vector3 b = Vector3.left * 3f;
		minion.transform.SetPosition(Grid.CellToPos(Grid.PosToCell(this.targetDoor.transform.position + b), CellAlignment.Bottom, Grid.SceneLayer.Move));
		ClusterManager.Instance.MigrateMinion(minion.GetComponent<MinionIdentity>(), this.targetDoor.GetMyWorldId());
	}

	// Token: 0x060085D9 RID: 34265 RVA: 0x00357560 File Offset: 0x00355760
	private void OnLaunch(object data)
	{
		NavTeleporter component = base.GetComponent<NavTeleporter>();
		component.EnableTwoWayTarget(false);
		component.Deregister();
		WorldContainer targetWorld = this.GetTargetWorld();
		if (targetWorld != null)
		{
			targetWorld.SetParentIdx(targetWorld.id);
		}
	}

	// Token: 0x060085DA RID: 34266 RVA: 0x0035759C File Offset: 0x0035579C
	private void OnLand(object data)
	{
		base.GetComponent<NavTeleporter>().EnableTwoWayTarget(true);
		WorldContainer targetWorld = this.GetTargetWorld();
		if (targetWorld != null)
		{
			int myWorldId = this.GetMyWorldId();
			targetWorld.SetParentIdx(myWorldId);
		}
	}

	// Token: 0x060085DB RID: 34267 RVA: 0x000FC6E7 File Offset: 0x000FA8E7
	public int TargetCell()
	{
		return this.targetDoor.GetComponent<NavTeleporter>().GetCell();
	}

	// Token: 0x060085DC RID: 34268 RVA: 0x000FC6F9 File Offset: 0x000FA8F9
	public ClustercraftInteriorDoor GetInteriorDoor()
	{
		return this.targetDoor;
	}

	// Token: 0x040065AE RID: 26030
	public string interiorTemplateName;

	// Token: 0x040065AF RID: 26031
	private ClustercraftInteriorDoor targetDoor;

	// Token: 0x040065B0 RID: 26032
	[Serialize]
	private int targetWorldId = -1;

	// Token: 0x040065B1 RID: 26033
	private static readonly EventSystem.IntraObjectHandler<ClustercraftExteriorDoor> OnLaunchDelegate = new EventSystem.IntraObjectHandler<ClustercraftExteriorDoor>(delegate(ClustercraftExteriorDoor component, object data)
	{
		component.OnLaunch(data);
	});

	// Token: 0x040065B2 RID: 26034
	private static readonly EventSystem.IntraObjectHandler<ClustercraftExteriorDoor> OnLandDelegate = new EventSystem.IntraObjectHandler<ClustercraftExteriorDoor>(delegate(ClustercraftExteriorDoor component, object data)
	{
		component.OnLand(data);
	});
}
