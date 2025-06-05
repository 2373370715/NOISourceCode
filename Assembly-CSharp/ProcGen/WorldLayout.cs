using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Delaunay.Geo;
using KSerialization;
using ObjectCloner;
using ProcGen.Map;
using ProcGenGame;
using UnityEngine;
using VoronoiTree;

namespace ProcGen
{
	// Token: 0x020020EB RID: 8427
	[SerializationConfig(MemberSerialization.OptIn)]
	public class WorldLayout
	{
		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x0600B37D RID: 45949 RVA: 0x00119333 File Offset: 0x00117533
		// (set) Token: 0x0600B37E RID: 45950 RVA: 0x0011933B File Offset: 0x0011753B
		[Serialize]
		public int mapWidth { get; private set; }

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x0600B37F RID: 45951 RVA: 0x00119344 File Offset: 0x00117544
		// (set) Token: 0x0600B380 RID: 45952 RVA: 0x0011934C File Offset: 0x0011754C
		[Serialize]
		public int mapHeight { get; private set; }

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x0600B381 RID: 45953 RVA: 0x00119355 File Offset: 0x00117555
		// (set) Token: 0x0600B382 RID: 45954 RVA: 0x0011935D File Offset: 0x0011755D
		public bool layoutOK { get; private set; }

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x0600B383 RID: 45955 RVA: 0x00119366 File Offset: 0x00117566
		// (set) Token: 0x0600B384 RID: 45956 RVA: 0x0011936D File Offset: 0x0011756D
		public static LevelLayer levelLayerGradient { get; private set; }

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x0600B385 RID: 45957 RVA: 0x00119375 File Offset: 0x00117575
		// (set) Token: 0x0600B386 RID: 45958 RVA: 0x0011937D File Offset: 0x0011757D
		public WorldGen worldGen { get; private set; }

		// Token: 0x0600B387 RID: 45959 RVA: 0x00119386 File Offset: 0x00117586
		public WorldLayout(WorldGen worldGen, int seed)
		{
			this.worldGen = worldGen;
			this.localGraph = new MapGraph(seed);
			this.overworldGraph = new MapGraph(seed);
			this.SetSeed(seed);
		}

		// Token: 0x0600B388 RID: 45960 RVA: 0x001193B4 File Offset: 0x001175B4
		public WorldLayout(WorldGen worldGen, int width, int height, int seed) : this(worldGen, seed)
		{
			this.mapWidth = width;
			this.mapHeight = height;
		}

		// Token: 0x0600B389 RID: 45961 RVA: 0x001193CD File Offset: 0x001175CD
		public void SetSeed(int seed)
		{
			this.myRandom = new SeededRandom(seed);
			this.localGraph.SetSeed(seed);
			this.overworldGraph.SetSeed(seed);
		}

		// Token: 0x0600B38A RID: 45962 RVA: 0x001193F3 File Offset: 0x001175F3
		public Tree GetVoronoiTree()
		{
			return this.voronoiTree;
		}

		// Token: 0x0600B38B RID: 45963 RVA: 0x001193FB File Offset: 0x001175FB
		public static void SetLayerGradient(LevelLayer newGradient)
		{
			WorldLayout.levelLayerGradient = newGradient;
		}

		// Token: 0x0600B38C RID: 45964 RVA: 0x00442A8C File Offset: 0x00440C8C
		public static string GetNodeTypeFromLayers(Vector2 point, float mapHeight, SeededRandom rnd)
		{
			string result = WorldGenTags.TheVoid.Name;
			int index = rnd.RandomRange(0, WorldLayout.levelLayerGradient[WorldLayout.levelLayerGradient.Count - 1].content.Count);
			result = WorldLayout.levelLayerGradient[WorldLayout.levelLayerGradient.Count - 1].content[index];
			for (int i = 0; i < WorldLayout.levelLayerGradient.Count; i++)
			{
				if (point.y < WorldLayout.levelLayerGradient[i].maxValue * mapHeight)
				{
					int index2 = rnd.RandomRange(0, WorldLayout.levelLayerGradient[i].content.Count);
					result = WorldLayout.levelLayerGradient[i].content[index2];
					break;
				}
			}
			return result;
		}

		// Token: 0x0600B38D RID: 45965 RVA: 0x00442B5C File Offset: 0x00440D5C
		public Tree GenerateOverworld(bool usePD, bool isRunningDebugGen)
		{
			global::Debug.Assert(this.mapWidth != 0 && this.mapHeight != 0, "Map size has not been set");
			global::Debug.Assert(this.worldGen.Settings.world != null, "You need to set a world");
			Diagram.Site site = new Diagram.Site(0U, new Vector2((float)(this.mapWidth / 2), (float)(this.mapHeight / 2)), 1f);
			this.topEdge = new LineSegment(new Vector2?(new Vector2(0f, (float)(this.mapHeight - 5))), new Vector2?(new Vector2((float)this.mapWidth, (float)(this.mapHeight - 5))));
			this.bottomEdge = new LineSegment(new Vector2?(new Vector2(0f, 5f)), new Vector2?(new Vector2((float)this.mapWidth, 5f)));
			this.leftEdge = new LineSegment(new Vector2?(new Vector2(5f, 0f)), new Vector2?(new Vector2(5f, (float)this.mapHeight)));
			this.rightEdge = new LineSegment(new Vector2?(new Vector2((float)(this.mapWidth - 5), 0f)), new Vector2?(new Vector2((float)(this.mapWidth - 5), (float)this.mapHeight)));
			site.poly = new Polygon(new Rect(0f, 0f, (float)this.mapWidth, (float)this.mapHeight));
			this.voronoiTree = new Tree(site, null, this.myRandom.seed);
			Node.maxIndex = 0U;
			float floatSetting = this.worldGen.Settings.GetFloatSetting("OverworldDensityMin");
			float floatSetting2 = this.worldGen.Settings.GetFloatSetting("OverworldDensityMax");
			float density = this.myRandom.RandomRange(floatSetting, floatSetting2);
			float floatSetting3 = this.worldGen.Settings.GetFloatSetting("OverworldAvoidRadius");
			PointGenerator.SampleBehaviour enumSetting = this.worldGen.Settings.GetEnumSetting<PointGenerator.SampleBehaviour>("OverworldSampleBehaviour");
			Cell cell = null;
			if (!string.IsNullOrEmpty(this.worldGen.Settings.world.startSubworldName))
			{
				WeightedSubworldName weightedSubworldName = this.worldGen.Settings.world.subworldFiles.Find((WeightedSubworldName x) => x.name == this.worldGen.Settings.world.startSubworldName);
				global::Debug.Assert(weightedSubworldName != null, "The start subworld must be listed in the subworld files for a world.");
				Vector2 position = new Vector2((float)this.mapWidth * this.worldGen.Settings.world.startingBasePositionHorizontal.GetRandomValueWithinRange(this.myRandom), (float)this.mapHeight * this.worldGen.Settings.world.startingBasePositionVertical.GetRandomValueWithinRange(this.myRandom));
				cell = this.overworldGraph.AddNode(weightedSubworldName.name, position);
				SubWorld subWorld = this.worldGen.Settings.GetSubWorld(weightedSubworldName.name);
				float num = (weightedSubworldName.overridePower > 0f) ? weightedSubworldName.overridePower : subWorld.pdWeight;
				Node node = this.voronoiTree.AddSite(new Diagram.Site((uint)cell.NodeId, cell.position, num), Node.NodeType.Internal);
				node.AddTag(WorldGenTags.AtStart);
				this.ApplySubworldToNode(node, subWorld, num);
			}
			List<Vector2> list = new List<Vector2>();
			if (cell != null)
			{
				list.Add(cell.position);
			}
			List<Vector2> randomPoints = PointGenerator.GetRandomPoints(site.poly, density, floatSetting3, list, enumSetting, false, this.myRandom, false, true);
			int intSetting = this.worldGen.Settings.GetIntSetting("OverworldMinNodes");
			int intSetting2 = this.worldGen.Settings.GetIntSetting("OverworldMaxNodes");
			if (randomPoints.Count > intSetting2)
			{
				randomPoints.ShuffleSeeded(this.myRandom.RandomSource());
				randomPoints.RemoveRange(intSetting2, randomPoints.Count - intSetting2);
			}
			if (randomPoints.Count < intSetting)
			{
				throw new Exception(string.Format("World layout with fewer than {0} points.", intSetting));
			}
			for (int i = 0; i < randomPoints.Count; i++)
			{
				Cell cell2 = this.overworldGraph.AddNode(WorldGenTags.UnassignedNode.Name, randomPoints[i]);
				this.voronoiTree.AddSite(new Diagram.Site((uint)cell2.NodeId, cell2.position, 1f), Node.NodeType.Internal).tags.Add(WorldGenTags.UnassignedNode);
				cell2.tags.Add(WorldGenTags.UnassignedNode);
			}
			List<Diagram.Site> list2 = new List<Diagram.Site>();
			for (int j = 0; j < this.voronoiTree.ChildCount(); j++)
			{
				list2.Add(this.voronoiTree.GetChild(j).site);
			}
			if (usePD)
			{
				this.voronoiTree.ComputeNode(list2);
				this.voronoiTree.ComputeNodePD(list2, 500, 0.2f);
			}
			else
			{
				this.voronoiTree.ComputeChildren(this.myRandom.seed + 1, false, false);
			}
			this.voronoiTree.VisitAll(delegate(Node n)
			{
				global::Debug.Assert(n.site.poly != null, string.Format("Node {0} had a null poly after initial overworld compute!!", n.site.id));
			});
			this.voronoiTree.AddTagToChildren(WorldGenTags.Overworld);
			this.TagTopAndBottomSites(WorldGenTags.AtSurface, WorldGenTags.AtDepths);
			this.TagEdgeSites(WorldGenTags.AtEdge, WorldGenTags.AtEdge);
			this.TagEdgeSites(WorldGenTags.AtLeft, WorldGenTags.AtRight);
			WorldLayout.ResetMapGraphFromVoronoiTree(this.voronoiTree.ImmediateChildren(), this.overworldGraph, true);
			this.PropagateDistanceTags(this.voronoiTree, WorldGenTags.DistanceTags);
			this.ConvertUnknownCells(this.myRandom, isRunningDebugGen);
			if (this.worldGen.Settings.GetOverworldAddTags() != null)
			{
				foreach (string name in this.worldGen.Settings.GetOverworldAddTags())
				{
					int childIndex = this.myRandom.RandomSource().Next(this.voronoiTree.ChildCount());
					this.voronoiTree.GetChild(childIndex).AddTag(new Tag(name));
				}
			}
			if (usePD)
			{
				this.voronoiTree.ComputeNodePD(list2, 500, 0.2f);
			}
			this.voronoiTree.VisitAll(delegate(Node n)
			{
				global::Debug.Assert(n.site.poly != null, string.Format("Node {0} had a null poly after final overworld compute!!", n.site.id));
			});
			this.FlattenOverworld();
			return this.voronoiTree;
		}

		// Token: 0x0600B38E RID: 45966 RVA: 0x004431D8 File Offset: 0x004413D8
		public static void ResetMapGraphFromVoronoiTree(List<Node> nodes, MapGraph graph, bool clear)
		{
			if (clear)
			{
				graph.ClearEdgesAndCorners();
			}
			for (int i = 0; i < nodes.Count; i++)
			{
				Node node = nodes[i];
				Cell cell = graph.FindNodeByID(node.site.id);
				cell.tags.Union(node.tags);
				cell.SetPosition(node.site.position);
				foreach (Node node2 in node.GetNeighbors())
				{
					Cell cell2 = graph.FindNodeByID(node2.site.id);
					if (graph.GetArc(cell, cell2) == null)
					{
						int num = -1;
						LineSegment lineSegment;
						if (node.site.poly.SharesEdge(node2.site.poly, ref num, out lineSegment) == Polygon.Commonality.Edge)
						{
							Corner corner = graph.AddOrGetCorner(lineSegment.p0.Value);
							Corner corner2 = graph.AddOrGetCorner(lineSegment.p1.Value);
							graph.AddOrGetEdge(cell, cell2, corner, corner2);
						}
					}
				}
			}
		}

		// Token: 0x0600B38F RID: 45967 RVA: 0x00119403 File Offset: 0x00117603
		public void PopulateSubworlds()
		{
			this.AddSubworldChildren();
			this.GetStartLocation();
			this.PropagateStartTag();
		}

		// Token: 0x0600B390 RID: 45968 RVA: 0x00443304 File Offset: 0x00441504
		private void PropagateDistanceTags(Tree tree, TagSet tags)
		{
			foreach (Tag tag in tags)
			{
				Dictionary<uint, int> distanceToTag = this.overworldGraph.GetDistanceToTag(tag);
				if (distanceToTag != null)
				{
					int num = 0;
					for (int i = 0; i < tree.ChildCount(); i++)
					{
						Node child = tree.GetChild(i);
						uint id = child.site.id;
						if (distanceToTag.ContainsKey(id))
						{
							child.minDistanceToTag.Add(tag, distanceToTag[id]);
							num++;
							if (distanceToTag[id] > 0)
							{
								child.AddTag(new Tag(tag.Name + "_Distance" + distanceToTag[id].ToString()));
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B391 RID: 45969 RVA: 0x004433EC File Offset: 0x004415EC
		private HashSet<WeightedSubWorld> GetNameFilterSet(Node vn, World.AllowedCellsFilter filter, List<WeightedSubWorld> subworlds)
		{
			HashSet<WeightedSubWorld> hashSet = new HashSet<WeightedSubWorld>();
			switch (filter.tagcommand)
			{
			case World.AllowedCellsFilter.TagCommand.Default:
			{
				int j;
				int i;
				for (i = 0; i < filter.subworldNames.Count; i = j + 1)
				{
					hashSet.UnionWith(subworlds.FindAll((WeightedSubWorld f) => f.subWorld.name == filter.subworldNames[i]));
					j = i;
				}
				break;
			}
			case World.AllowedCellsFilter.TagCommand.AtTag:
				if (vn.tags.Contains(filter.tag))
				{
					int j;
					int i;
					for (i = 0; i < filter.subworldNames.Count; i = j + 1)
					{
						hashSet.UnionWith(subworlds.FindAll((WeightedSubWorld f) => f.subWorld.name == filter.subworldNames[i]));
						j = i;
					}
				}
				break;
			case World.AllowedCellsFilter.TagCommand.NotAtTag:
				if (!vn.tags.Contains(filter.tag))
				{
					int j;
					int i;
					for (i = 0; i < filter.subworldNames.Count; i = j + 1)
					{
						hashSet.UnionWith(subworlds.FindAll((WeightedSubWorld f) => f.subWorld.name == filter.subworldNames[i]));
						j = i;
					}
				}
				break;
			case World.AllowedCellsFilter.TagCommand.DistanceFromTag:
			{
				Tag tag = filter.tag.ToTag();
				bool flag = vn.minDistanceToTag.ContainsKey(tag);
				if (!flag && tag == WorldGenTags.AtStart && !filter.ignoreIfMissingTag)
				{
					DebugUtil.DevLogError("DistanceFromTag was used on a world without an AtStart tag, use ignoreIfMissingTag to skip it.");
				}
				else
				{
					global::Debug.Assert(flag || filter.ignoreIfMissingTag, "DistanceFromTag is missing tag " + filter.tag + ", use ignoreIfMissingTag.");
					if (flag && vn.minDistanceToTag[tag] >= filter.minDistance && vn.minDistanceToTag[tag] <= filter.maxDistance)
					{
						int j;
						int i;
						for (i = 0; i < filter.subworldNames.Count; i = j + 1)
						{
							hashSet.UnionWith(subworlds.FindAll((WeightedSubWorld f) => f.subWorld.name == filter.subworldNames[i]));
							j = i;
						}
					}
				}
				break;
			}
			}
			return hashSet;
		}

		// Token: 0x0600B392 RID: 45970 RVA: 0x004436CC File Offset: 0x004418CC
		private HashSet<WeightedSubWorld> GetZoneTypeFilterSet(Node vn, World.AllowedCellsFilter filter, Dictionary<string, List<WeightedSubWorld>> subworldsByZoneType)
		{
			HashSet<WeightedSubWorld> hashSet = new HashSet<WeightedSubWorld>();
			switch (filter.tagcommand)
			{
			case World.AllowedCellsFilter.TagCommand.Default:
				for (int i = 0; i < filter.zoneTypes.Count; i++)
				{
					hashSet.UnionWith(subworldsByZoneType[filter.zoneTypes[i].ToString()]);
				}
				break;
			case World.AllowedCellsFilter.TagCommand.AtTag:
				if (vn.tags.Contains(filter.tag))
				{
					for (int j = 0; j < filter.zoneTypes.Count; j++)
					{
						hashSet.UnionWith(subworldsByZoneType[filter.zoneTypes[j].ToString()]);
					}
				}
				break;
			case World.AllowedCellsFilter.TagCommand.NotAtTag:
				if (!vn.tags.Contains(filter.tag))
				{
					for (int k = 0; k < filter.zoneTypes.Count; k++)
					{
						hashSet.UnionWith(subworldsByZoneType[filter.zoneTypes[k].ToString()]);
					}
				}
				break;
			case World.AllowedCellsFilter.TagCommand.DistanceFromTag:
				global::Debug.Assert(vn.minDistanceToTag.ContainsKey(filter.tag.ToTag()), filter.tag);
				if (vn.minDistanceToTag[filter.tag.ToTag()] >= filter.minDistance && vn.minDistanceToTag[filter.tag.ToTag()] <= filter.maxDistance)
				{
					for (int l = 0; l < filter.zoneTypes.Count; l++)
					{
						hashSet.UnionWith(subworldsByZoneType[filter.zoneTypes[l].ToString()]);
					}
				}
				break;
			}
			return hashSet;
		}

		// Token: 0x0600B393 RID: 45971 RVA: 0x004438A4 File Offset: 0x00441AA4
		private HashSet<WeightedSubWorld> GetTemperatureFilterSet(Node vn, World.AllowedCellsFilter filter, Dictionary<string, List<WeightedSubWorld>> subworldsByTemperature)
		{
			HashSet<WeightedSubWorld> hashSet = new HashSet<WeightedSubWorld>();
			switch (filter.tagcommand)
			{
			case World.AllowedCellsFilter.TagCommand.Default:
				for (int i = 0; i < filter.temperatureRanges.Count; i++)
				{
					hashSet.UnionWith(subworldsByTemperature[filter.temperatureRanges[i].ToString()]);
				}
				break;
			case World.AllowedCellsFilter.TagCommand.AtTag:
				if (vn.tags.Contains(filter.tag))
				{
					for (int j = 0; j < filter.temperatureRanges.Count; j++)
					{
						hashSet.UnionWith(subworldsByTemperature[filter.temperatureRanges[j].ToString()]);
					}
				}
				break;
			case World.AllowedCellsFilter.TagCommand.NotAtTag:
				if (!vn.tags.Contains(filter.tag))
				{
					for (int k = 0; k < filter.temperatureRanges.Count; k++)
					{
						hashSet.UnionWith(subworldsByTemperature[filter.temperatureRanges[k].ToString()]);
					}
				}
				break;
			case World.AllowedCellsFilter.TagCommand.DistanceFromTag:
				global::Debug.Assert(vn.minDistanceToTag.ContainsKey(filter.tag.ToTag()), filter.tag);
				if (vn.minDistanceToTag[filter.tag.ToTag()] >= filter.minDistance && vn.minDistanceToTag[filter.tag.ToTag()] <= filter.maxDistance)
				{
					for (int l = 0; l < filter.temperatureRanges.Count; l++)
					{
						hashSet.UnionWith(subworldsByTemperature[filter.temperatureRanges[l].ToString()]);
					}
				}
				break;
			}
			return hashSet;
		}

		// Token: 0x0600B394 RID: 45972 RVA: 0x00443A7C File Offset: 0x00441C7C
		private void RunFilterClearCommand(Node vn, World.AllowedCellsFilter filter, HashSet<WeightedSubWorld> allowedSubworldsSet)
		{
			switch (filter.tagcommand)
			{
			case World.AllowedCellsFilter.TagCommand.Default:
				allowedSubworldsSet.Clear();
				return;
			case World.AllowedCellsFilter.TagCommand.AtTag:
				if (vn.tags.Contains(filter.tag))
				{
					allowedSubworldsSet.Clear();
					return;
				}
				break;
			case World.AllowedCellsFilter.TagCommand.NotAtTag:
				if (!vn.tags.Contains(filter.tag))
				{
					allowedSubworldsSet.Clear();
					return;
				}
				break;
			case World.AllowedCellsFilter.TagCommand.DistanceFromTag:
				global::Debug.Assert(vn.minDistanceToTag.ContainsKey(filter.tag.ToTag()), filter.tag);
				if (vn.minDistanceToTag[filter.tag.ToTag()] >= filter.minDistance && vn.minDistanceToTag[filter.tag.ToTag()] <= filter.maxDistance)
				{
					allowedSubworldsSet.Clear();
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600B395 RID: 45973 RVA: 0x00443B54 File Offset: 0x00441D54
		private HashSet<WeightedSubWorld> Filter(Node vn, List<WeightedSubWorld> allSubWorlds, Dictionary<string, List<WeightedSubWorld>> subworldsByTemperature, Dictionary<string, List<WeightedSubWorld>> subworldsByZoneType)
		{
			HashSet<WeightedSubWorld> hashSet = new HashSet<WeightedSubWorld>();
			World world = this.worldGen.Settings.world;
			string text = "";
			foreach (KeyValuePair<Tag, int> keyValuePair in vn.minDistanceToTag)
			{
				text = string.Concat(new string[]
				{
					text,
					keyValuePair.Key.Name,
					":",
					keyValuePair.Value.ToString(),
					", "
				});
			}
			foreach (World.AllowedCellsFilter allowedCellsFilter in world.unknownCellsAllowedSubworlds)
			{
				HashSet<WeightedSubWorld> hashSet2 = new HashSet<WeightedSubWorld>();
				if (allowedCellsFilter.subworldNames != null && allowedCellsFilter.subworldNames.Count > 0)
				{
					hashSet2.UnionWith(this.GetNameFilterSet(vn, allowedCellsFilter, allSubWorlds));
				}
				if (allowedCellsFilter.temperatureRanges != null && allowedCellsFilter.temperatureRanges.Count > 0)
				{
					hashSet2.UnionWith(this.GetTemperatureFilterSet(vn, allowedCellsFilter, subworldsByTemperature));
				}
				if (allowedCellsFilter.zoneTypes != null && allowedCellsFilter.zoneTypes.Count > 0)
				{
					hashSet2.UnionWith(this.GetZoneTypeFilterSet(vn, allowedCellsFilter, subworldsByZoneType));
				}
				switch (allowedCellsFilter.command)
				{
				case World.AllowedCellsFilter.Command.Clear:
					this.RunFilterClearCommand(vn, allowedCellsFilter, hashSet);
					break;
				case World.AllowedCellsFilter.Command.Replace:
					if (hashSet2.Count > 0)
					{
						hashSet.Clear();
						hashSet.UnionWith(hashSet2);
					}
					break;
				case World.AllowedCellsFilter.Command.UnionWith:
					hashSet.UnionWith(hashSet2);
					break;
				case World.AllowedCellsFilter.Command.IntersectWith:
					hashSet.IntersectWith(hashSet2);
					break;
				case World.AllowedCellsFilter.Command.ExceptWith:
					hashSet.ExceptWith(hashSet2);
					break;
				case World.AllowedCellsFilter.Command.SymmetricExceptWith:
					hashSet.SymmetricExceptWith(hashSet2);
					break;
				case World.AllowedCellsFilter.Command.All:
					global::Debug.LogError("Command.All is unsupported for unknownCellsAllowedSubworlds.");
					break;
				}
			}
			return hashSet;
		}

		// Token: 0x0600B396 RID: 45974 RVA: 0x00443D6C File Offset: 0x00441F6C
		private void ConvertUnknownCells(SeededRandom myRandom, bool isRunningDebugGen)
		{
			List<Node> list = new List<Node>();
			this.voronoiTree.GetNodesWithTag(WorldGenTags.UnassignedNode, list);
			list.ShuffleSeeded(myRandom.RandomSource());
			List<WeightedSubworldName> subworldList = new List<WeightedSubworldName>(this.worldGen.Settings.world.subworldFiles);
			List<WeightedSubWorld> subworldsForWorld = this.worldGen.Settings.GetSubworldsForWorld(subworldList);
			Dictionary<string, List<WeightedSubWorld>> dictionary = new Dictionary<string, List<WeightedSubWorld>>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(Temperature.Range)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Temperature.Range range = (Temperature.Range)enumerator.Current;
					dictionary.Add(range.ToString(), subworldsForWorld.FindAll((WeightedSubWorld sw) => sw.subWorld.temperatureRange == range));
				}
			}
			Dictionary<string, List<WeightedSubWorld>> dictionary2 = new Dictionary<string, List<WeightedSubWorld>>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(SubWorld.ZoneType)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SubWorld.ZoneType zt = (SubWorld.ZoneType)enumerator.Current;
					dictionary2.Add(zt.ToString(), subworldsForWorld.FindAll((WeightedSubWorld sw) => sw.subWorld.zoneType == zt));
				}
			}
			foreach (Node node in list)
			{
				Node node2 = this.overworldGraph.FindNodeByID(node.site.id);
				node.tags.Remove(WorldGenTags.UnassignedNode);
				node2.tags.Remove(WorldGenTags.UnassignedNode);
				List<WeightedSubWorld> list2 = new List<WeightedSubWorld>(this.Filter(node, subworldsForWorld, dictionary, dictionary2));
				List<WeightedSubWorld> list3 = list2.FindAll((WeightedSubWorld x) => x.minCount > 0);
				WeightedSubWorld weightedSubWorld;
				if (list3.Count > 0)
				{
					weightedSubWorld = list3[0];
					int priority = weightedSubWorld.priority;
					foreach (WeightedSubWorld weightedSubWorld2 in list3)
					{
						if (weightedSubWorld2.priority > priority || (weightedSubWorld2.priority == priority && weightedSubWorld2.minCount > weightedSubWorld.minCount))
						{
							weightedSubWorld = weightedSubWorld2;
							priority = weightedSubWorld2.priority;
						}
					}
					WeightedSubWorld weightedSubWorld3 = weightedSubWorld;
					int num = weightedSubWorld3.minCount;
					weightedSubWorld3.minCount = num - 1;
				}
				else
				{
					weightedSubWorld = WeightedRandom.Choose<WeightedSubWorld>(list2, myRandom);
				}
				if (weightedSubWorld != null)
				{
					this.ApplySubworldToNode(node, weightedSubWorld.subWorld, weightedSubWorld.overridePower);
					WeightedSubWorld weightedSubWorld4 = weightedSubWorld;
					int num = weightedSubWorld4.maxCount;
					weightedSubWorld4.maxCount = num - 1;
					if (weightedSubWorld.maxCount <= 0)
					{
						subworldsForWorld.Remove(weightedSubWorld);
					}
				}
				else
				{
					string text = "";
					foreach (KeyValuePair<Tag, int> keyValuePair in node.minDistanceToTag)
					{
						text = string.Concat(new string[]
						{
							text,
							keyValuePair.Key.Name,
							":",
							keyValuePair.Value.ToString(),
							", "
						});
					}
					DebugUtil.LogWarningArgs(new object[]
					{
						"No allowed Subworld types. Using default. ",
						node2.tags.ToString(),
						"Distances:",
						text
					});
					node2.SetType("Default");
				}
			}
			foreach (WeightedSubWorld weightedSubWorld5 in subworldsForWorld)
			{
				if (weightedSubWorld5.minCount > 0)
				{
					if (!isRunningDebugGen)
					{
						throw new Exception(string.Format("Could not guarantee minCount of Subworld {0}, {1} remaining on world {2}.", weightedSubWorld5.subWorld.name, weightedSubWorld5.minCount, this.worldGen.Settings.world.filePath));
					}
					DebugUtil.DevLogError(string.Format("Could not guarantee minCount of Subworld {0}, {1} remaining on world {2}.", weightedSubWorld5.subWorld.name, weightedSubWorld5.minCount, this.worldGen.Settings.world.filePath));
				}
			}
		}

		// Token: 0x0600B397 RID: 45975 RVA: 0x00444270 File Offset: 0x00442470
		private Node ApplySubworldToNode(Node vn, SubWorld subWorld, float overridePower = -1f)
		{
			Node node = this.overworldGraph.FindNodeByID(vn.site.id);
			node.SetType(subWorld.name);
			vn.site.weight = ((overridePower > 0f) ? overridePower : subWorld.pdWeight);
			foreach (string name in subWorld.tags)
			{
				vn.AddTag(new Tag(name));
			}
			vn.AddTag(subWorld.zoneType.ToString());
			return node;
		}

		// Token: 0x0600B398 RID: 45976 RVA: 0x00444328 File Offset: 0x00442528
		private void FlattenOverworld()
		{
			try
			{
				WorldLayout.ResetMapGraphFromVoronoiTree(this.voronoiTree.ImmediateChildren(), this.overworldGraph, true);
				foreach (Edge edge in this.overworldGraph.arcs)
				{
					List<Cell> nodes = this.overworldGraph.GetNodes(edge);
					Cell cell = nodes[0];
					Cell cell2 = nodes[1];
					SubWorld subWorld = this.worldGen.Settings.GetSubWorld(cell.type);
					global::Debug.Assert(subWorld != null, "SubWorld is null: " + cell.type);
					SubWorld subWorld2 = this.worldGen.Settings.GetSubWorld(cell2.type);
					global::Debug.Assert(subWorld2 != null, "other SubWorld is null: " + cell2.type);
					if (cell.type == cell2.type || subWorld.zoneType == subWorld2.zoneType)
					{
						edge.tags.Add(WorldGenTags.EdgeOpen);
					}
					else if (subWorld.borderOverride == "NONE" || subWorld2.borderOverride == "NONE")
					{
						edge.tags.Add(WorldGenTags.EdgeOpen);
					}
					else
					{
						edge.tags.Add(WorldGenTags.EdgeClosed);
					}
				}
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				string stackTrace = ex.StackTrace;
				global::Debug.LogError("ex: " + message + " " + stackTrace);
			}
		}

		// Token: 0x0600B399 RID: 45977 RVA: 0x004444E4 File Offset: 0x004426E4
		public static bool TestEdgeConsistency(MapGraph graph, Cell cell, out Edge problemEdge)
		{
			List<Edge> arcs = graph.GetArcs(cell);
			foreach (Edge edge in arcs)
			{
				int num = 0;
				int num2 = 0;
				foreach (Edge edge2 in arcs)
				{
					if (edge2.corner0 == edge.corner0 || edge2.corner1 == edge.corner0)
					{
						num++;
					}
					if (edge2.corner1 == edge.corner1 || edge2.corner1 == edge.corner1)
					{
						num2++;
					}
				}
				if (num != 2 || num2 != 2)
				{
					problemEdge = edge;
					return false;
				}
			}
			problemEdge = null;
			return true;
		}

		// Token: 0x0600B39A RID: 45978 RVA: 0x004445D4 File Offset: 0x004427D4
		private void AddSubworldChildren()
		{
			new TagSet().Add(WorldGenTags.Overworld);
			List<string> defaultMoveTags = this.worldGen.Settings.GetDefaultMoveTags();
			if (defaultMoveTags != null)
			{
				new TagSet(defaultMoveTags);
			}
			List<Feature> list = new List<Feature>();
			foreach (KeyValuePair<string, int> keyValuePair in this.worldGen.Settings.world.globalFeatures)
			{
				for (int i = 0; i < keyValuePair.Value; i++)
				{
					list.Add(new Feature
					{
						type = keyValuePair.Key
					});
				}
			}
			Dictionary<uint, List<Feature>> dictionary = new Dictionary<uint, List<Feature>>();
			List<Node> list2 = new List<Node>();
			this.voronoiTree.GetNodesWithoutTag(WorldGenTags.NoGlobalFeatureSpawning, list2);
			list2.ShuffleSeeded(this.myRandom.RandomSource());
			foreach (Feature item in list)
			{
				if (list2.Count == 0)
				{
					break;
				}
				Node node = list2[0];
				list2.RemoveAt(0);
				if (!dictionary.ContainsKey(node.site.id))
				{
					dictionary[node.site.id] = new List<Feature>();
				}
				dictionary[node.site.id].Add(item);
			}
			this.localGraph.ClearEdgesAndCorners();
			for (int j = 0; j < this.voronoiTree.ChildCount(); j++)
			{
				Node child2 = this.voronoiTree.GetChild(j);
				if (child2.type == Node.NodeType.Internal)
				{
					Tree child = child2 as Tree;
					Node node2 = this.overworldGraph.FindNodeByID(child.site.id);
					SubWorld subWorld = SerializingCloner.Copy<SubWorld>(this.worldGen.Settings.GetSubWorld(node2.type));
					child.AddTag(new Tag(node2.type));
					child.AddTag(new Tag(subWorld.temperatureRange.ToString()));
					child.AddTag(new Tag(subWorld.zoneType.ToString()));
					if (dictionary.ContainsKey(child2.site.id))
					{
						subWorld.features.AddRange(dictionary[child2.site.id]);
					}
					this.GenerateChildren(subWorld, child, this.localGraph, (float)this.mapHeight, j + this.myRandom.seed);
					child.RelaxRecursive(0, 10, 1f, this.worldGen.Settings.world.layoutMethod == World.LayoutMethod.PowerTree);
					child.VisitAll(delegate(Node n)
					{
						global::Debug.Assert(n.site.poly != null, string.Format("Node {0}, child of {1} had a null poly after final subworld relax!!", n.site.id, child.site.id));
					});
				}
			}
			Node.maxDepth = this.voronoiTree.MaxDepth(0);
		}

		// Token: 0x0600B39B RID: 45979 RVA: 0x00444908 File Offset: 0x00442B08
		private List<Vector2> GetPoints(string name, LoggerSSF log, int minPointCount, int maxPointCount, Polygon boundingArea, float density, float avoidRadius, List<Vector2> avoidPoints, PointGenerator.SampleBehaviour sampleBehaviour, bool testInsideBounds, SeededRandom rnd, bool doShuffle = true, bool testAvoidPoints = true)
		{
			int num = 0;
			List<Vector2> randomPoints;
			do
			{
				randomPoints = PointGenerator.GetRandomPoints(boundingArea, density, avoidRadius, avoidPoints, sampleBehaviour, testInsideBounds, rnd, doShuffle, testAvoidPoints);
				if (randomPoints.Count < minPointCount)
				{
					density *= 0.8f;
					avoidRadius *= 0.8f;
					bool isRunningDebugGen = this.worldGen.isRunningDebugGen;
				}
				num++;
			}
			while (randomPoints.Count < minPointCount && randomPoints.Count <= maxPointCount && num < 10);
			if (randomPoints.Count > maxPointCount)
			{
				randomPoints.RemoveRange(maxPointCount, randomPoints.Count - maxPointCount);
			}
			return randomPoints;
		}

		// Token: 0x0600B39C RID: 45980 RVA: 0x00444994 File Offset: 0x00442B94
		public void GenerateChildren(SubWorld sw, Tree node, MapGraph graph, float worldHeight, int seed)
		{
			SeededRandom seededRandom = new SeededRandom(seed);
			List<string> defaultMoveTags = this.worldGen.Settings.GetDefaultMoveTags();
			TagSet tagSet = (defaultMoveTags != null) ? new TagSet(defaultMoveTags) : null;
			TagSet tagSet2 = new TagSet();
			if (tagSet != null)
			{
				for (int i = 0; i < tagSet.Count; i++)
				{
					Tag item = tagSet[i];
					if (node.tags.Contains(item))
					{
						node.tags.Remove(item);
						tagSet2.Add(item);
					}
				}
			}
			TagSet tagSet3 = new TagSet(node.tags);
			tagSet3.Remove(WorldGenTags.Overworld);
			for (int j = 0; j < sw.tags.Count; j++)
			{
				tagSet3.Add(new Tag(sw.tags[j]));
			}
			float randomValueWithinRange = sw.density.GetRandomValueWithinRange(seededRandom);
			List<Vector2> list = new List<Vector2>();
			if (sw.centralFeature != null)
			{
				list.Add(node.site.poly.Centroid());
				this.CreateTreeNodeWithFeatureAndBiome(this.worldGen.Settings, sw, node, graph, sw.centralFeature, node.site.poly.Centroid(), tagSet3, -1).AddTag(WorldGenTags.CenteralFeature);
			}
			node.dontRelaxChildren = sw.dontRelaxChildren;
			int num = Mathf.Max(sw.features.Count + sw.extraBiomeChildren, sw.minChildCount);
			int maxPointCount = int.MaxValue;
			if (sw.singleChildCount)
			{
				num = 1;
				maxPointCount = 1;
			}
			List<Vector2> points = this.GetPoints(sw.name, node.log, num, maxPointCount, node.site.poly, randomValueWithinRange, sw.avoidRadius, list, sw.sampleBehaviour, true, seededRandom, true, sw.doAvoidPoints);
			global::Debug.Assert(points.Count >= num, string.Format("Overworld node {0} of subworld {1} generated {2} points of an expected minimum {3}\nThis probably means that either:\n* sampler density is too large (lower the number for tighter samples)\n* avoid radius is too large (only applies if there is a central feature, especialy if you get 0 points generated)\n* min point count is just plain too large.", new object[]
			{
				node.site.id,
				sw.name,
				points.Count,
				num
			}));
			for (int k = 0; k < sw.samplers.Count; k++)
			{
				list.AddRange(points);
				float randomValueWithinRange2 = sw.samplers[k].density.GetRandomValueWithinRange(seededRandom);
				List<Vector2> randomPoints = PointGenerator.GetRandomPoints(node.site.poly, randomValueWithinRange2, sw.samplers[k].avoidRadius, list, sw.samplers[k].sampleBehaviour, true, seededRandom, true, sw.samplers[k].doAvoidPoints);
				points.AddRange(randomPoints);
			}
			if (points.Count > 200)
			{
				points.RemoveRange(200, points.Count - 200);
			}
			if (points.Count < num)
			{
				string str = "";
				for (int l = 0; l < node.site.poly.Vertices.Count; l++)
				{
					str = str + node.site.poly.Vertices[l].ToString() + ", ";
				}
				if (this.worldGen.isRunningDebugGen)
				{
					global::Debug.Assert(points.Count >= num, "Error not enough points " + sw.name + " in node " + node.site.id.ToString());
				}
				return;
			}
			int count = sw.features.Count;
			int count2 = points.Count;
			for (int m = 0; m < points.Count; m++)
			{
				Feature feature = null;
				if (m < sw.features.Count)
				{
					feature = sw.features[m];
				}
				this.CreateTreeNodeWithFeatureAndBiome(this.worldGen.Settings, sw, node, graph, feature, points[m], tagSet3, m);
			}
			node.ComputeChildren(seededRandom.seed + 1, false, false);
			node.VisitAll(delegate(Node n)
			{
				global::Debug.Assert(n.site.poly != null, string.Format("Node {0}, child of {1} had a null poly after final subworld compute!!", n.site.id, node.site.id));
			});
			if (node.ChildCount() > 0)
			{
				for (int n2 = 0; n2 < tagSet2.Count; n2++)
				{
					global::Debug.Log(string.Format("Applying Moved Tag {0} to {1}", tagSet2[n2].Name, node.site.id));
					node.GetChild(seededRandom.RandomSource().Next(node.ChildCount())).AddTag(tagSet2[n2]);
				}
			}
		}

		// Token: 0x0600B39D RID: 45981 RVA: 0x00444E9C File Offset: 0x0044309C
		private Node CreateTreeNodeWithFeatureAndBiome(WorldGenSettings settings, SubWorld sw, Tree node, MapGraph graph, Feature feature, Vector2 pos, TagSet newTags, int i)
		{
			string text = null;
			bool flag = false;
			TagSet tagSet = new TagSet();
			TagSet tagSet2 = new TagSet();
			if (feature != null)
			{
				FeatureSettings feature2 = settings.GetFeature(feature.type);
				text = feature.type;
				tagSet2.Union(new TagSet(feature2.tags));
				if (feature.tags != null && feature.tags.Count > 0)
				{
					tagSet2.Union(new TagSet(feature.tags));
				}
				if (feature.excludesTags != null && feature.excludesTags.Count > 0)
				{
					tagSet2.Remove(new TagSet(feature.excludesTags));
				}
				tagSet2.Add(new Tag(feature.type));
				tagSet2.Add(WorldGenTags.Feature);
				if (feature2.forceBiome != null)
				{
					tagSet.Add(feature2.forceBiome);
					flag = true;
				}
				if (feature2.biomeTags != null)
				{
					tagSet.Union(new TagSet(feature2.biomeTags));
				}
			}
			if (!flag && sw.biomes.Count > 0)
			{
				WeightedBiome weightedBiome = WeightedRandom.Choose<WeightedBiome>(sw.biomes, this.myRandom);
				if (text == null)
				{
					text = weightedBiome.name;
				}
				tagSet.Add(weightedBiome.name);
				if (weightedBiome.tags != null && weightedBiome.tags.Count > 0)
				{
					tagSet.Union(new TagSet(weightedBiome.tags));
				}
				flag = true;
			}
			if (!flag)
			{
				text = "UNKNOWN";
				global::Debug.LogError("Couldn't get a biome for a cell in " + sw.name + ". Maybe it doesn't have any biomes configured?");
			}
			Cell cell = graph.AddNode(text, pos);
			cell.biomeSpecificTags = new TagSet(tagSet);
			cell.featureSpecificTags = new TagSet(tagSet2);
			Node node2 = node.AddSite(new Diagram.Site((uint)cell.NodeId, cell.position, 1f), Node.NodeType.Internal);
			node2.tags = new TagSet(newTags);
			node2.tags.Add(text);
			node2.tags.Union(tagSet);
			node2.tags.Union(tagSet2);
			return node2;
		}

		// Token: 0x0600B39E RID: 45982 RVA: 0x004450A0 File Offset: 0x004432A0
		private void TagTopAndBottomSites(Tag topTag, Tag bottomTag)
		{
			List<Diagram.Site> list = new List<Diagram.Site>();
			List<Diagram.Site> list2 = new List<Diagram.Site>();
			this.voronoiTree.GetIntersectingLeafSites(this.topEdge, list);
			this.voronoiTree.GetIntersectingLeafSites(this.bottomEdge, list2);
			for (int i = 0; i < list.Count; i++)
			{
				this.voronoiTree.GetNodeForSite(list[i]).AddTag(topTag);
			}
			for (int j = 0; j < list2.Count; j++)
			{
				this.voronoiTree.GetNodeForSite(list2[j]).AddTag(bottomTag);
			}
		}

		// Token: 0x0600B39F RID: 45983 RVA: 0x00445130 File Offset: 0x00443330
		private void TagEdgeSites(Tag leftTag, Tag rightTag)
		{
			List<Diagram.Site> list = new List<Diagram.Site>();
			List<Diagram.Site> list2 = new List<Diagram.Site>();
			this.voronoiTree.GetIntersectingLeafSites(this.leftEdge, list);
			this.voronoiTree.GetIntersectingLeafSites(this.rightEdge, list2);
			for (int i = 0; i < list.Count; i++)
			{
				this.voronoiTree.GetNodeForSite(list[i]).AddTag(leftTag);
			}
			for (int j = 0; j < list2.Count; j++)
			{
				this.voronoiTree.GetNodeForSite(list2[j]).AddTag(rightTag);
			}
		}

		// Token: 0x0600B3A0 RID: 45984 RVA: 0x00119418 File Offset: 0x00117618
		private bool StartAreaTooLarge(Node node)
		{
			return node.tags.Contains(WorldGenTags.AtStart) && node.site.poly.Area() > 2000f;
		}

		// Token: 0x0600B3A1 RID: 45985 RVA: 0x004451C0 File Offset: 0x004433C0
		private void PropagateStartTag()
		{
			foreach (Node node in this.GetStartNodes())
			{
				node.AddTagToNeighbors(WorldGenTags.NearStartLocation);
				node.AddTag(WorldGenTags.IgnoreCaveOverride);
			}
		}

		// Token: 0x0600B3A2 RID: 45986 RVA: 0x00119445 File Offset: 0x00117645
		public List<Node> GetStartNodes()
		{
			return this.GetLeafNodesWithTag(WorldGenTags.StartLocation);
		}

		// Token: 0x0600B3A3 RID: 45987 RVA: 0x00445220 File Offset: 0x00443420
		public List<Node> GetLeafNodesWithTag(Tag tag)
		{
			List<Node> list = new List<Node>();
			this.voronoiTree.GetLeafNodes(list, (Node node) => node.tags != null && node.tags.Contains(tag));
			return list;
		}

		// Token: 0x0600B3A4 RID: 45988 RVA: 0x0044525C File Offset: 0x0044345C
		public List<Node> GetInternalNonLeafNodesWithTag(Tag tag)
		{
			List<Node> list = new List<Node>();
			this.voronoiTree.GetInternalNonLeafNodes(list, (Node node) => node.tags != null && node.tags.Contains(tag));
			return list;
		}

		// Token: 0x0600B3A5 RID: 45989 RVA: 0x00445298 File Offset: 0x00443498
		public List<Node> GetTerrainNodesForTag(Tag tag)
		{
			List<Node> list = new List<Node>();
			foreach (Node node in this.GetLeafNodesWithTag(tag))
			{
				Node node2 = this.localGraph.FindNodeByID(node.site.id);
				if (node2 != null)
				{
					list.Add(node2);
				}
			}
			return list;
		}

		// Token: 0x0600B3A6 RID: 45990 RVA: 0x00445310 File Offset: 0x00443510
		private Node FindFirstNode(string nodeType)
		{
			return this.localGraph.FindNode((Cell node) => node.type == nodeType);
		}

		// Token: 0x0600B3A7 RID: 45991 RVA: 0x00445344 File Offset: 0x00443544
		private Node FindFirstNodeWithTag(Tag tag)
		{
			return this.localGraph.FindNode((Cell node) => node.tags != null && node.tags.Contains(tag));
		}

		// Token: 0x0600B3A8 RID: 45992 RVA: 0x00445378 File Offset: 0x00443578
		public Vector2I GetStartLocation()
		{
			if (string.IsNullOrEmpty(this.worldGen.Settings.world.startSubworldName))
			{
				return new Vector2I(this.mapWidth / 2, this.mapHeight / 2);
			}
			Node node2 = this.FindFirstNodeWithTag(WorldGenTags.StartLocation);
			if (node2 == null)
			{
				List<Node> nodes = this.GetStartNodes();
				if (nodes == null || nodes.Count == 0)
				{
					global::Debug.LogWarning("Couldnt find start node");
					return new Vector2I(this.mapWidth / 2, this.mapHeight / 2);
				}
				node2 = this.localGraph.FindNode((Cell node) => (uint)node.NodeId == nodes[0].site.id);
				node2.tags.Add(WorldGenTags.StartLocation);
			}
			if (node2 == null)
			{
				global::Debug.LogWarning("Couldnt find start node");
				return new Vector2I(this.mapWidth / 2, this.mapHeight / 2);
			}
			return new Vector2I((int)node2.position.x, (int)node2.position.y);
		}

		// Token: 0x0600B3A9 RID: 45993 RVA: 0x00445478 File Offset: 0x00443678
		private List<Diagram.Site> GetIntersectingSites(Node intersectingSiteSource, Tree sitesSource)
		{
			List<Diagram.Site> list = new List<Diagram.Site>();
			list = new List<Diagram.Site>();
			LineSegment edge;
			for (int i = 1; i < intersectingSiteSource.site.poly.Vertices.Count - 1; i++)
			{
				edge = new LineSegment(new Vector2?(intersectingSiteSource.site.poly.Vertices[i - 1]), new Vector2?(intersectingSiteSource.site.poly.Vertices[i]));
				sitesSource.GetIntersectingLeafSites(edge, list);
			}
			edge = new LineSegment(new Vector2?(intersectingSiteSource.site.poly.Vertices[intersectingSiteSource.site.poly.Vertices.Count - 1]), new Vector2?(intersectingSiteSource.site.poly.Vertices[0]));
			sitesSource.GetIntersectingLeafSites(edge, list);
			return list;
		}

		// Token: 0x0600B3AA RID: 45994 RVA: 0x00119452 File Offset: 0x00117652
		public void GetEdgeOfMapSites(Tree vt, List<Diagram.Site> topSites, List<Diagram.Site> bottomSites, List<Diagram.Site> leftSites, List<Diagram.Site> rightSites)
		{
			vt.GetIntersectingLeafSites(this.topEdge, topSites);
			vt.GetIntersectingLeafSites(this.bottomEdge, bottomSites);
			vt.GetIntersectingLeafSites(this.leftEdge, leftSites);
			vt.GetIntersectingLeafSites(this.rightEdge, rightSites);
		}

		// Token: 0x0600B3AB RID: 45995 RVA: 0x00445554 File Offset: 0x00443754
		[OnSerializing]
		internal void OnSerializingMethod()
		{
			try
			{
				this.extra = new WorldLayout.ExtraIO();
				if (this.voronoiTree != null)
				{
					this.extra.internals.Add(this.voronoiTree);
					this.voronoiTree.GetInternalNodes(this.extra.internals);
					List<Node> list = new List<Node>();
					this.voronoiTree.GetLeafNodes(list, null);
					using (List<Node>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Leaf ln = (Leaf)enumerator.Current;
							if (ln != null)
							{
								this.extra.leafInternalParent.Add(new KeyValuePair<int, int>(this.extra.leafs.Count, this.extra.internals.FindIndex(0, (Tree n) => n == ln.parent)));
								this.extra.leafs.Add(ln);
							}
						}
					}
					for (int i = 0; i < this.extra.internals.Count; i++)
					{
						Tree vt = this.extra.internals[i];
						if (vt.parent != null)
						{
							int num = this.extra.internals.FindIndex(0, (Tree n) => n == vt.parent);
							if (num >= 0)
							{
								this.extra.internalInternalParent.Add(new KeyValuePair<int, int>(i, num));
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				string stackTrace = ex.StackTrace;
				WorldGenLogger.LogException(message, stackTrace);
				global::Debug.Log("Error deserialising " + ex.Message);
			}
		}

		// Token: 0x0600B3AC RID: 45996 RVA: 0x0011948A File Offset: 0x0011768A
		[OnSerialized]
		internal void OnSerializedMethod()
		{
			this.extra = null;
		}

		// Token: 0x0600B3AD RID: 45997 RVA: 0x00119493 File Offset: 0x00117693
		[OnDeserializing]
		internal void OnDeserializingMethod()
		{
			this.extra = new WorldLayout.ExtraIO();
		}

		// Token: 0x0600B3AE RID: 45998 RVA: 0x00445744 File Offset: 0x00443944
		[OnDeserialized]
		internal void OnDeserializedMethod()
		{
			try
			{
				this.voronoiTree = this.extra.internals[0];
				for (int i = 0; i < this.extra.internalInternalParent.Count; i++)
				{
					KeyValuePair<int, int> keyValuePair = this.extra.internalInternalParent[i];
					Tree child = this.extra.internals[keyValuePair.Key];
					this.extra.internals[keyValuePair.Value].AddChild(child);
				}
				for (int j = 0; j < this.extra.leafInternalParent.Count; j++)
				{
					KeyValuePair<int, int> keyValuePair2 = this.extra.leafInternalParent[j];
					Node child2 = this.extra.leafs[keyValuePair2.Key];
					this.extra.internals[keyValuePair2.Value].AddChild(child2);
				}
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				string stackTrace = ex.StackTrace;
				WorldGenLogger.LogException(message, stackTrace);
				global::Debug.Log("Error deserialising " + ex.Message);
			}
			this.extra = null;
		}

		// Token: 0x04008E37 RID: 36407
		private Tree voronoiTree;

		// Token: 0x04008E38 RID: 36408
		[Serialize]
		public MapGraph localGraph;

		// Token: 0x04008E39 RID: 36409
		[Serialize]
		public MapGraph overworldGraph;

		// Token: 0x04008E3A RID: 36410
		[EnumFlags]
		public static WorldLayout.DebugFlags drawOptions;

		// Token: 0x04008E3C RID: 36412
		private LineSegment topEdge;

		// Token: 0x04008E3D RID: 36413
		private LineSegment bottomEdge;

		// Token: 0x04008E3E RID: 36414
		private LineSegment leftEdge;

		// Token: 0x04008E3F RID: 36415
		private LineSegment rightEdge;

		// Token: 0x04008E41 RID: 36417
		private SeededRandom myRandom;

		// Token: 0x04008E43 RID: 36419
		[Serialize]
		private WorldLayout.ExtraIO extra;

		// Token: 0x020020EC RID: 8428
		[Flags]
		public enum DebugFlags
		{
			// Token: 0x04008E45 RID: 36421
			LocalGraph = 1,
			// Token: 0x04008E46 RID: 36422
			OverworldGraph = 2,
			// Token: 0x04008E47 RID: 36423
			VoronoiTree = 4,
			// Token: 0x04008E48 RID: 36424
			PowerDiagram = 8
		}

		// Token: 0x020020ED RID: 8429
		[SerializationConfig(MemberSerialization.OptOut)]
		private class ExtraIO
		{
			// Token: 0x0600B3B0 RID: 46000 RVA: 0x001194C2 File Offset: 0x001176C2
			[OnDeserializing]
			internal void OnDeserializingMethod()
			{
				this.leafs = new List<Leaf>();
				this.internals = new List<Tree>();
				this.leafInternalParent = new List<KeyValuePair<int, int>>();
				this.internalInternalParent = new List<KeyValuePair<int, int>>();
			}

			// Token: 0x04008E49 RID: 36425
			public List<Leaf> leafs = new List<Leaf>();

			// Token: 0x04008E4A RID: 36426
			public List<Tree> internals = new List<Tree>();

			// Token: 0x04008E4B RID: 36427
			public List<KeyValuePair<int, int>> leafInternalParent = new List<KeyValuePair<int, int>>();

			// Token: 0x04008E4C RID: 36428
			public List<KeyValuePair<int, int>> internalInternalParent = new List<KeyValuePair<int, int>>();
		}
	}
}
