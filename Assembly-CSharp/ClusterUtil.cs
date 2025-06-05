using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020010D6 RID: 4310
public static class ClusterUtil
{
	// Token: 0x06005800 RID: 22528 RVA: 0x000DDDDA File Offset: 0x000DBFDA
	public static WorldContainer GetMyWorld(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyWorld();
	}

	// Token: 0x06005801 RID: 22529 RVA: 0x000DDDE7 File Offset: 0x000DBFE7
	public static WorldContainer GetMyWorld(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyWorld();
	}

	// Token: 0x06005802 RID: 22530 RVA: 0x00296124 File Offset: 0x00294324
	public static WorldContainer GetMyWorld(this GameObject gameObject)
	{
		int num = Grid.PosToCell(gameObject);
		if (Grid.IsValidCell(num) && Grid.WorldIdx[num] != 255)
		{
			return ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]);
		}
		return null;
	}

	// Token: 0x06005803 RID: 22531 RVA: 0x000DDDF4 File Offset: 0x000DBFF4
	public static int GetMyWorldId(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyWorldId();
	}

	// Token: 0x06005804 RID: 22532 RVA: 0x000DDE01 File Offset: 0x000DC001
	public static int GetMyWorldId(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyWorldId();
	}

	// Token: 0x06005805 RID: 22533 RVA: 0x00296164 File Offset: 0x00294364
	public static int GetMyWorldId(this GameObject gameObject)
	{
		int num = Grid.PosToCell(gameObject);
		if (Grid.IsValidCell(num) && Grid.WorldIdx[num] != 255)
		{
			return (int)Grid.WorldIdx[num];
		}
		return -1;
	}

	// Token: 0x06005806 RID: 22534 RVA: 0x000DDE0E File Offset: 0x000DC00E
	public static int GetMyParentWorldId(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyParentWorldId();
	}

	// Token: 0x06005807 RID: 22535 RVA: 0x000DDE1B File Offset: 0x000DC01B
	public static int GetMyParentWorldId(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyParentWorldId();
	}

	// Token: 0x06005808 RID: 22536 RVA: 0x00296198 File Offset: 0x00294398
	public static int GetMyParentWorldId(this GameObject gameObject)
	{
		WorldContainer myWorld = gameObject.GetMyWorld();
		if (myWorld == null)
		{
			return gameObject.GetMyWorldId();
		}
		return myWorld.ParentWorldId;
	}

	// Token: 0x06005809 RID: 22537 RVA: 0x000DDE28 File Offset: 0x000DC028
	public static AxialI GetMyWorldLocation(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyWorldLocation();
	}

	// Token: 0x0600580A RID: 22538 RVA: 0x000DDE35 File Offset: 0x000DC035
	public static AxialI GetMyWorldLocation(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyWorldLocation();
	}

	// Token: 0x0600580B RID: 22539 RVA: 0x002961C4 File Offset: 0x002943C4
	public static AxialI GetMyWorldLocation(this GameObject gameObject)
	{
		ClusterGridEntity component = gameObject.GetComponent<ClusterGridEntity>();
		if (component != null)
		{
			return component.Location;
		}
		WorldContainer myWorld = gameObject.GetMyWorld();
		DebugUtil.DevAssertArgs(myWorld != null, new object[]
		{
			"GetMyWorldLocation called on object with no world",
			gameObject
		});
		return myWorld.GetComponent<ClusterGridEntity>().Location;
	}

	// Token: 0x0600580C RID: 22540 RVA: 0x00296218 File Offset: 0x00294418
	public static bool IsMyWorld(this GameObject go, GameObject otherGo)
	{
		int otherCell = Grid.PosToCell(otherGo);
		return go.IsMyWorld(otherCell);
	}

	// Token: 0x0600580D RID: 22541 RVA: 0x00296234 File Offset: 0x00294434
	public static bool IsMyWorld(this GameObject go, int otherCell)
	{
		int num = Grid.PosToCell(go);
		return Grid.IsValidCell(num) && Grid.IsValidCell(otherCell) && Grid.WorldIdx[num] == Grid.WorldIdx[otherCell];
	}

	// Token: 0x0600580E RID: 22542 RVA: 0x0029626C File Offset: 0x0029446C
	public static bool IsMyParentWorld(this GameObject go, GameObject otherGo)
	{
		int otherCell = Grid.PosToCell(otherGo);
		return go.IsMyParentWorld(otherCell);
	}

	// Token: 0x0600580F RID: 22543 RVA: 0x00296288 File Offset: 0x00294488
	public static bool IsMyParentWorld(this GameObject go, int otherCell)
	{
		int num = Grid.PosToCell(go);
		if (Grid.IsValidCell(num) && Grid.IsValidCell(otherCell))
		{
			if (Grid.WorldIdx[num] == Grid.WorldIdx[otherCell])
			{
				return true;
			}
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]);
			WorldContainer world2 = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[otherCell]);
			if (world == null)
			{
				DebugUtil.DevLogError(string.Format("{0} at {1} has a valid cell but no world", go, num));
			}
			if (world2 == null)
			{
				DebugUtil.DevLogError(string.Format("{0} is a valid cell but no world", otherCell));
			}
			if (world != null && world2 != null && world.ParentWorldId == world2.ParentWorldId)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005810 RID: 22544 RVA: 0x00296348 File Offset: 0x00294548
	public static int GetAsteroidWorldIdAtLocation(AxialI location)
	{
		foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.cellContents[location])
		{
			if (clusterGridEntity.Layer == EntityLayer.Asteroid)
			{
				WorldContainer component = clusterGridEntity.GetComponent<WorldContainer>();
				if (component != null)
				{
					return component.id;
				}
			}
		}
		return -1;
	}

	// Token: 0x06005811 RID: 22545 RVA: 0x000DDE42 File Offset: 0x000DC042
	public static bool ActiveWorldIsRocketInterior()
	{
		return ClusterManager.Instance.activeWorld.IsModuleInterior;
	}

	// Token: 0x06005812 RID: 22546 RVA: 0x000DDE53 File Offset: 0x000DC053
	public static bool ActiveWorldHasPrinter()
	{
		return ClusterManager.Instance.activeWorld.IsModuleInterior || Components.Telepads.GetWorldItems(ClusterManager.Instance.activeWorldId, false).Count > 0;
	}

	// Token: 0x06005813 RID: 22547 RVA: 0x002963C4 File Offset: 0x002945C4
	public static float GetAmountFromRelatedWorlds(WorldInventory worldInventory, Tag element)
	{
		WorldContainer worldContainer = worldInventory.WorldContainer;
		float num = 0f;
		int parentWorldId = worldContainer.ParentWorldId;
		foreach (WorldContainer worldContainer2 in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer2.ParentWorldId == parentWorldId)
			{
				num += worldContainer2.worldInventory.GetAmount(element, false);
			}
		}
		return num;
	}

	// Token: 0x06005814 RID: 22548 RVA: 0x00296440 File Offset: 0x00294640
	public static List<Pickupable> GetPickupablesFromRelatedWorlds(WorldInventory worldInventory, Tag tag)
	{
		List<Pickupable> list = new List<Pickupable>();
		int parentWorldId = worldInventory.GetComponent<WorldContainer>().ParentWorldId;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer.ParentWorldId == parentWorldId)
			{
				ICollection<Pickupable> pickupables = worldContainer.worldInventory.GetPickupables(tag, false);
				if (pickupables != null)
				{
					list.AddRange(pickupables);
				}
			}
		}
		return list;
	}

	// Token: 0x06005815 RID: 22549 RVA: 0x002964CC File Offset: 0x002946CC
	public static string DebugGetMyWorldName(this GameObject gameObject)
	{
		WorldContainer myWorld = gameObject.GetMyWorld();
		if (myWorld != null)
		{
			return myWorld.worldName;
		}
		return string.Format("InvalidWorld(pos={0})", gameObject.transform.GetPosition());
	}

	// Token: 0x06005816 RID: 22550 RVA: 0x0029650C File Offset: 0x0029470C
	public static ClusterGridEntity ClosestVisibleAsteroidToLocation(AxialI location)
	{
		foreach (AxialI cell in AxialUtil.SpiralOut(location, ClusterGrid.Instance.numRings))
		{
			if (ClusterGrid.Instance.IsValidCell(cell) && ClusterGrid.Instance.IsCellVisible(cell))
			{
				ClusterGridEntity asteroidAtCell = ClusterGrid.Instance.GetAsteroidAtCell(cell);
				if (asteroidAtCell != null)
				{
					return asteroidAtCell;
				}
			}
		}
		return null;
	}
}
