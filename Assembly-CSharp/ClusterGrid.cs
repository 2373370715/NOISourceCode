using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using STRINGS;
using UnityEngine;

// Token: 0x020019A7 RID: 6567
public class ClusterGrid
{
	// Token: 0x060088F7 RID: 35063 RVA: 0x000FE15F File Offset: 0x000FC35F
	public static void DestroyInstance()
	{
		ClusterGrid.Instance = null;
	}

	// Token: 0x060088F8 RID: 35064 RVA: 0x000FE167 File Offset: 0x000FC367
	private ClusterFogOfWarManager.Instance GetFOWManager()
	{
		if (this.m_fowManager == null)
		{
			this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
		}
		return this.m_fowManager;
	}

	// Token: 0x060088F9 RID: 35065 RVA: 0x000FE187 File Offset: 0x000FC387
	public bool IsValidCell(AxialI cell)
	{
		return this.cellContents.ContainsKey(cell);
	}

	// Token: 0x060088FA RID: 35066 RVA: 0x000FE195 File Offset: 0x000FC395
	public ClusterGrid(int numRings)
	{
		ClusterGrid.Instance = this;
		this.GenerateGrid(numRings);
		this.m_onClusterLocationChangedDelegate = new Action<object>(this.OnClusterLocationChanged);
	}

	// Token: 0x060088FB RID: 35067 RVA: 0x000FE1C7 File Offset: 0x000FC3C7
	public ClusterRevealLevel GetCellRevealLevel(AxialI cell)
	{
		return this.GetFOWManager().GetCellRevealLevel(cell);
	}

	// Token: 0x060088FC RID: 35068 RVA: 0x000FE1D5 File Offset: 0x000FC3D5
	public bool IsCellVisible(AxialI cell)
	{
		return this.GetFOWManager().IsLocationRevealed(cell);
	}

	// Token: 0x060088FD RID: 35069 RVA: 0x000FE1E3 File Offset: 0x000FC3E3
	public float GetRevealCompleteFraction(AxialI cell)
	{
		return this.GetFOWManager().GetRevealCompleteFraction(cell);
	}

	// Token: 0x060088FE RID: 35070 RVA: 0x000FE1F1 File Offset: 0x000FC3F1
	public bool IsVisible(ClusterGridEntity entity)
	{
		return entity.IsVisible && this.IsCellVisible(entity.Location);
	}

	// Token: 0x060088FF RID: 35071 RVA: 0x0036544C File Offset: 0x0036364C
	public List<ClusterGridEntity> GetVisibleEntitiesAtCell(AxialI cell)
	{
		if (this.IsValidCell(cell) && this.GetFOWManager().IsLocationRevealed(cell))
		{
			return (from entity in this.cellContents[cell]
			where entity.IsVisible
			select entity).ToList<ClusterGridEntity>();
		}
		return new List<ClusterGridEntity>();
	}

	// Token: 0x06008900 RID: 35072 RVA: 0x003654AC File Offset: 0x003636AC
	public ClusterGridEntity GetVisibleEntityOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		if (this.IsValidCell(cell) && this.GetFOWManager().IsLocationRevealed(cell))
		{
			foreach (ClusterGridEntity clusterGridEntity in this.cellContents[cell])
			{
				if (clusterGridEntity.IsVisible && clusterGridEntity.Layer == entityLayer)
				{
					return clusterGridEntity;
				}
			}
		}
		return null;
	}

	// Token: 0x06008901 RID: 35073 RVA: 0x00365530 File Offset: 0x00363730
	public ClusterGridEntity GetVisibleEntityOfLayerAtAdjacentCell(AxialI cell, EntityLayer entityLayer)
	{
		return AxialUtil.GetRing(cell, 1).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetVisibleEntitiesAtCell)).FirstOrDefault((ClusterGridEntity entity) => entity.Layer == entityLayer);
	}

	// Token: 0x06008902 RID: 35074 RVA: 0x00365574 File Offset: 0x00363774
	public List<ClusterGridEntity> GetHiddenEntitiesOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		return (from entity in AxialUtil.GetRing(cell, 0).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetHiddenEntitiesAtCell))
		where entity.Layer == entityLayer
		select entity).ToList<ClusterGridEntity>();
	}

	// Token: 0x06008903 RID: 35075 RVA: 0x003655BC File Offset: 0x003637BC
	public List<ClusterGridEntity> GetEntitiesOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		return (from entity in AxialUtil.GetRing(cell, 0).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetEntitiesOnCell))
		where entity.Layer == entityLayer
		select entity).ToList<ClusterGridEntity>();
	}

	// Token: 0x06008904 RID: 35076 RVA: 0x00365604 File Offset: 0x00363804
	public ClusterGridEntity GetEntityOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		return AxialUtil.GetRing(cell, 0).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetEntitiesOnCell)).FirstOrDefault((ClusterGridEntity entity) => entity.Layer == entityLayer);
	}

	// Token: 0x06008905 RID: 35077 RVA: 0x00365648 File Offset: 0x00363848
	public List<ClusterGridEntity> GetHiddenEntitiesAtCell(AxialI cell)
	{
		if (this.cellContents.ContainsKey(cell) && !this.GetFOWManager().IsLocationRevealed(cell))
		{
			return (from entity in this.cellContents[cell]
			where entity.IsVisible
			select entity).ToList<ClusterGridEntity>();
		}
		return new List<ClusterGridEntity>();
	}

	// Token: 0x06008906 RID: 35078 RVA: 0x000FE209 File Offset: 0x000FC409
	public List<ClusterGridEntity> GetNotVisibleEntitiesAtAdjacentCell(AxialI cell)
	{
		return AxialUtil.GetRing(cell, 1).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetHiddenEntitiesAtCell)).ToList<ClusterGridEntity>();
	}

	// Token: 0x06008907 RID: 35079 RVA: 0x003656AC File Offset: 0x003638AC
	public List<ClusterGridEntity> GetNotVisibleEntitiesOfLayerAtAdjacentCell(AxialI cell, EntityLayer entityLayer)
	{
		return (from entity in AxialUtil.GetRing(cell, 1).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetHiddenEntitiesAtCell))
		where entity.Layer == entityLayer
		select entity).ToList<ClusterGridEntity>();
	}

	// Token: 0x06008908 RID: 35080 RVA: 0x003656F4 File Offset: 0x003638F4
	public bool GetVisibleUnidentifiedMeteorShowerWithinRadius(AxialI center, int radius, out ClusterMapMeteorShower.Instance result)
	{
		for (int i = 0; i <= radius; i++)
		{
			foreach (AxialI axialI in AxialUtil.GetRing(center, i))
			{
				if (this.IsValidCell(axialI) && this.GetFOWManager().IsLocationRevealed(axialI))
				{
					foreach (ClusterGridEntity cmp in this.GetEntitiesOfLayerAtCell(axialI, EntityLayer.Craft))
					{
						ClusterMapMeteorShower.Instance smi = cmp.GetSMI<ClusterMapMeteorShower.Instance>();
						if (smi != null && !smi.HasBeenIdentified)
						{
							result = smi;
							return true;
						}
					}
				}
			}
		}
		result = null;
		return false;
	}

	// Token: 0x06008909 RID: 35081 RVA: 0x003657CC File Offset: 0x003639CC
	public ClusterGridEntity GetAsteroidAtCell(AxialI cell)
	{
		if (!this.cellContents.ContainsKey(cell))
		{
			return null;
		}
		return (from e in this.cellContents[cell]
		where e.Layer == EntityLayer.Asteroid
		select e).FirstOrDefault<ClusterGridEntity>();
	}

	// Token: 0x0600890A RID: 35082 RVA: 0x000FE228 File Offset: 0x000FC428
	public bool HasVisibleAsteroidAtCell(AxialI cell)
	{
		return this.GetVisibleEntityOfLayerAtCell(cell, EntityLayer.Asteroid) != null;
	}

	// Token: 0x0600890B RID: 35083 RVA: 0x000FE238 File Offset: 0x000FC438
	public void RegisterEntity(ClusterGridEntity entity)
	{
		this.cellContents[entity.Location].Add(entity);
		entity.Subscribe(-1298331547, this.m_onClusterLocationChangedDelegate);
	}

	// Token: 0x0600890C RID: 35084 RVA: 0x000FE263 File Offset: 0x000FC463
	public void UnregisterEntity(ClusterGridEntity entity)
	{
		this.cellContents[entity.Location].Remove(entity);
		entity.Unsubscribe(-1298331547, this.m_onClusterLocationChangedDelegate);
	}

	// Token: 0x0600890D RID: 35085 RVA: 0x00365820 File Offset: 0x00363A20
	public void OnClusterLocationChanged(object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		global::Debug.Assert(this.IsValidCell(clusterLocationChangedEvent.oldLocation), string.Format("ChangeEntityCell move order FROM invalid location: {0} {1}", clusterLocationChangedEvent.oldLocation, clusterLocationChangedEvent.entity));
		global::Debug.Assert(this.IsValidCell(clusterLocationChangedEvent.newLocation), string.Format("ChangeEntityCell move order TO invalid location: {0} {1}", clusterLocationChangedEvent.newLocation, clusterLocationChangedEvent.entity));
		this.cellContents[clusterLocationChangedEvent.oldLocation].Remove(clusterLocationChangedEvent.entity);
		this.cellContents[clusterLocationChangedEvent.newLocation].Add(clusterLocationChangedEvent.entity);
	}

	// Token: 0x0600890E RID: 35086 RVA: 0x000FE28E File Offset: 0x000FC48E
	private AxialI GetNeighbor(AxialI cell, AxialI direction)
	{
		return cell + direction;
	}

	// Token: 0x0600890F RID: 35087 RVA: 0x003658C8 File Offset: 0x00363AC8
	public int GetCellRing(AxialI cell)
	{
		Vector3I vector3I = cell.ToCube();
		Vector3I vector3I2 = new Vector3I(vector3I.x, vector3I.y, vector3I.z);
		Vector3I vector3I3 = new Vector3I(0, 0, 0);
		return (int)((float)((Mathf.Abs(vector3I2.x - vector3I3.x) + Mathf.Abs(vector3I2.y - vector3I3.y) + Mathf.Abs(vector3I2.z - vector3I3.z)) / 2));
	}

	// Token: 0x06008910 RID: 35088 RVA: 0x000FE297 File Offset: 0x000FC497
	private void CleanUpGrid()
	{
		this.cellContents.Clear();
	}

	// Token: 0x06008911 RID: 35089 RVA: 0x0036593C File Offset: 0x00363B3C
	private int GetHexDistance(AxialI a, AxialI b)
	{
		Vector3I vector3I = a.ToCube();
		Vector3I vector3I2 = b.ToCube();
		return Mathf.Max(new int[]
		{
			Mathf.Abs(vector3I.x - vector3I2.x),
			Mathf.Abs(vector3I.y - vector3I2.y),
			Mathf.Abs(vector3I.z - vector3I2.z)
		});
	}

	// Token: 0x06008912 RID: 35090 RVA: 0x003659A4 File Offset: 0x00363BA4
	public List<ClusterGridEntity> GetEntitiesInRange(AxialI center, int range = 1)
	{
		List<ClusterGridEntity> list = new List<ClusterGridEntity>();
		foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in this.cellContents)
		{
			if (this.GetHexDistance(keyValuePair.Key, center) <= range)
			{
				list.AddRange(keyValuePair.Value);
			}
		}
		return list;
	}

	// Token: 0x06008913 RID: 35091 RVA: 0x000FE2A4 File Offset: 0x000FC4A4
	public List<ClusterGridEntity> GetEntitiesOnCell(AxialI cell)
	{
		return this.cellContents[cell];
	}

	// Token: 0x06008914 RID: 35092 RVA: 0x000FE2B2 File Offset: 0x000FC4B2
	public bool IsInRange(AxialI a, AxialI b, int range = 1)
	{
		return this.GetHexDistance(a, b) <= range;
	}

	// Token: 0x06008915 RID: 35093 RVA: 0x00365A18 File Offset: 0x00363C18
	private void GenerateGrid(int rings)
	{
		this.CleanUpGrid();
		this.numRings = rings;
		for (int i = -rings + 1; i < rings; i++)
		{
			for (int j = -rings + 1; j < rings; j++)
			{
				for (int k = -rings + 1; k < rings; k++)
				{
					if (i + j + k == 0)
					{
						AxialI key = new AxialI(i, j);
						this.cellContents.Add(key, new List<ClusterGridEntity>());
					}
				}
			}
		}
	}

	// Token: 0x06008916 RID: 35094 RVA: 0x00365A80 File Offset: 0x00363C80
	public AxialI GetRandomCellAtEdgeOfUniverse()
	{
		int num = this.numRings - 1;
		List<AxialI> rings = AxialUtil.GetRings(AxialI.ZERO, num, num);
		return rings.ElementAt(UnityEngine.Random.Range(0, rings.Count));
	}

	// Token: 0x06008917 RID: 35095 RVA: 0x00365AB8 File Offset: 0x00363CB8
	public Vector3 GetPosition(ClusterGridEntity entity)
	{
		float r = (float)entity.Location.R;
		float q = (float)entity.Location.Q;
		List<ClusterGridEntity> list = this.cellContents[entity.Location];
		if (list.Count <= 1 || !entity.SpaceOutInSameHex())
		{
			return AxialUtil.AxialToWorld(r, q);
		}
		int num = 0;
		int num2 = 0;
		foreach (ClusterGridEntity clusterGridEntity in list)
		{
			if (entity == clusterGridEntity)
			{
				num = num2;
			}
			if (clusterGridEntity.SpaceOutInSameHex())
			{
				num2++;
			}
		}
		if (list.Count > num2)
		{
			num2 += 5;
			num += 5;
		}
		else if (num2 > 0)
		{
			num2++;
			num++;
		}
		if (num2 == 0 || num2 == 1)
		{
			return AxialUtil.AxialToWorld(r, q);
		}
		float num3 = Mathf.Min(Mathf.Pow((float)num2, 0.5f), 1f) * 0.5f;
		float num4 = Mathf.Pow((float)num / (float)num2, 0.5f);
		float num5 = 0.81f;
		float num6 = Mathf.Pow((float)num2, 0.5f) * num5;
		float f = 6.2831855f * num6 * num4;
		float x = Mathf.Cos(f) * num3 * num4;
		float y = Mathf.Sin(f) * num3 * num4;
		return AxialUtil.AxialToWorld(r, q) + new Vector3(x, y, 0f);
	}

	// Token: 0x06008918 RID: 35096 RVA: 0x00365C3C File Offset: 0x00363E3C
	public List<AxialI> GetPath(AxialI start, AxialI end, ClusterDestinationSelector destination_selector)
	{
		string text;
		return this.GetPath(start, end, destination_selector, out text, false);
	}

	// Token: 0x06008919 RID: 35097 RVA: 0x00365C58 File Offset: 0x00363E58
	public List<AxialI> GetPath(AxialI start, AxialI end, ClusterDestinationSelector destination_selector, out string fail_reason, bool dodgeHiddenAsteroids = false)
	{
		ClusterGrid.<>c__DisplayClass41_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.destination_selector = destination_selector;
		CS$<>8__locals1.start = start;
		CS$<>8__locals1.end = end;
		CS$<>8__locals1.dodgeHiddenAsteroids = dodgeHiddenAsteroids;
		fail_reason = null;
		if (!CS$<>8__locals1.destination_selector.canNavigateFogOfWar && !this.IsCellVisible(CS$<>8__locals1.end))
		{
			fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_FOG_OF_WAR;
			return null;
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = this.GetVisibleEntityOfLayerAtCell(CS$<>8__locals1.end, EntityLayer.Asteroid);
		if (visibleEntityOfLayerAtCell != null && CS$<>8__locals1.destination_selector.requireLaunchPadOnAsteroidDestination)
		{
			bool flag = false;
			using (IEnumerator enumerator = Components.LaunchPads.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((LaunchPad)enumerator.Current).GetMyWorldLocation() == visibleEntityOfLayerAtCell.Location)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_NO_LAUNCH_PAD;
				return null;
			}
		}
		if (visibleEntityOfLayerAtCell == null && CS$<>8__locals1.destination_selector.requireAsteroidDestination)
		{
			fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_REQUIRE_ASTEROID;
			return null;
		}
		CS$<>8__locals1.frontier = new HashSet<AxialI>();
		CS$<>8__locals1.visited = new HashSet<AxialI>();
		CS$<>8__locals1.buffer = new HashSet<AxialI>();
		CS$<>8__locals1.cameFrom = new Dictionary<AxialI, AxialI>();
		CS$<>8__locals1.frontier.Add(CS$<>8__locals1.start);
		while (!CS$<>8__locals1.frontier.Contains(CS$<>8__locals1.end) && CS$<>8__locals1.frontier.Count > 0)
		{
			this.<GetPath>g__ExpandFrontier|41_0(ref CS$<>8__locals1);
		}
		if (CS$<>8__locals1.frontier.Contains(CS$<>8__locals1.end))
		{
			List<AxialI> list = new List<AxialI>();
			AxialI axialI = CS$<>8__locals1.end;
			while (axialI != CS$<>8__locals1.start)
			{
				list.Add(axialI);
				axialI = CS$<>8__locals1.cameFrom[axialI];
			}
			list.Reverse();
			return list;
		}
		fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_NO_PATH;
		return null;
	}

	// Token: 0x0600891A RID: 35098 RVA: 0x00365E48 File Offset: 0x00364048
	public void GetLocationDescription(AxialI location, out Sprite sprite, out string label, out string sublabel)
	{
		ClusterGridEntity clusterGridEntity = this.GetVisibleEntitiesAtCell(location).Find((ClusterGridEntity x) => x.Layer == EntityLayer.Asteroid);
		ClusterGridEntity visibleEntityOfLayerAtAdjacentCell = this.GetVisibleEntityOfLayerAtAdjacentCell(location, EntityLayer.Asteroid);
		if (clusterGridEntity != null)
		{
			sprite = clusterGridEntity.GetUISprite();
			label = clusterGridEntity.Name;
			WorldContainer component = clusterGridEntity.GetComponent<WorldContainer>();
			sublabel = Strings.Get(component.worldType);
			return;
		}
		if (visibleEntityOfLayerAtAdjacentCell != null)
		{
			sprite = visibleEntityOfLayerAtAdjacentCell.GetUISprite();
			label = UI.SPACEDESTINATIONS.ORBIT.NAME_FMT.Replace("{Name}", visibleEntityOfLayerAtAdjacentCell.Name);
			WorldContainer component2 = visibleEntityOfLayerAtAdjacentCell.GetComponent<WorldContainer>();
			sublabel = Strings.Get(component2.worldType);
			return;
		}
		if (this.IsCellVisible(location))
		{
			sprite = Assets.GetSprite("hex_unknown");
			label = UI.SPACEDESTINATIONS.EMPTY_SPACE.NAME;
			sublabel = "";
			return;
		}
		sprite = Assets.GetSprite("unknown_far");
		label = UI.SPACEDESTINATIONS.FOG_OF_WAR_SPACE.NAME;
		sublabel = "";
	}

	// Token: 0x0600891B RID: 35099 RVA: 0x00365F58 File Offset: 0x00364158
	[CompilerGenerated]
	private void <GetPath>g__ExpandFrontier|41_0(ref ClusterGrid.<>c__DisplayClass41_0 A_1)
	{
		A_1.buffer.Clear();
		foreach (AxialI axialI in A_1.frontier)
		{
			foreach (AxialI direction in AxialI.DIRECTIONS)
			{
				AxialI neighbor = this.GetNeighbor(axialI, direction);
				if (!A_1.visited.Contains(neighbor) && this.IsValidCell(neighbor) && (this.IsCellVisible(neighbor) || A_1.destination_selector.canNavigateFogOfWar) && (!this.HasVisibleAsteroidAtCell(neighbor) || !(neighbor != A_1.start) || !(neighbor != A_1.end)) && (!A_1.dodgeHiddenAsteroids || !(ClusterGrid.Instance.GetAsteroidAtCell(neighbor) != null) || ClusterGrid.Instance.GetAsteroidAtCell(neighbor).IsVisibleInFOW == ClusterRevealLevel.Visible || !(neighbor != A_1.start) || !(neighbor != A_1.end)))
				{
					A_1.buffer.Add(neighbor);
					if (!A_1.cameFrom.ContainsKey(neighbor))
					{
						A_1.cameFrom.Add(neighbor, axialI);
					}
				}
			}
			A_1.visited.Add(axialI);
		}
		HashSet<AxialI> frontier = A_1.frontier;
		A_1.frontier = A_1.buffer;
		A_1.buffer = frontier;
	}

	// Token: 0x0400679E RID: 26526
	public static ClusterGrid Instance;

	// Token: 0x0400679F RID: 26527
	public const float NodeDistanceScale = 600f;

	// Token: 0x040067A0 RID: 26528
	private const float MAX_OFFSET_RADIUS = 0.5f;

	// Token: 0x040067A1 RID: 26529
	public int numRings;

	// Token: 0x040067A2 RID: 26530
	private ClusterFogOfWarManager.Instance m_fowManager;

	// Token: 0x040067A3 RID: 26531
	private Action<object> m_onClusterLocationChangedDelegate;

	// Token: 0x040067A4 RID: 26532
	public Dictionary<AxialI, List<ClusterGridEntity>> cellContents = new Dictionary<AxialI, List<ClusterGridEntity>>();
}
