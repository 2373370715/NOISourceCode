using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;

namespace ProcGenGame
{
	// Token: 0x02002109 RID: 8457
	public class TemplateSpawning
	{
		// Token: 0x0600B3F5 RID: 46069 RVA: 0x00447388 File Offset: 0x00445588
		public static List<TemplateSpawning.TemplateSpawner> DetermineTemplatesForWorld(WorldGenSettings settings, List<TerrainCell> terrainCells, SeededRandom myRandom, ref List<RectInt> placedPOIBounds, bool isRunningDebugGen, ref List<WorldTrait> placedStoryTraits, WorldGen.OfflineCallbackFunction successCallbackFn)
		{
			successCallbackFn(UI.WORLDGEN.PLACINGTEMPLATES.key, 0f, WorldGenProgressStages.Stages.PlaceTemplates);
			List<TemplateSpawning.TemplateSpawner> result = new List<TemplateSpawning.TemplateSpawner>();
			TemplateSpawning.s_poiPadding = settings.GetIntSetting("POIPadding");
			TemplateSpawning.s_minProgressPercent = 0f;
			TemplateSpawning.s_maxProgressPercent = 0.33f;
			TemplateSpawning.SpawnStartingTemplate(settings, terrainCells, ref result, ref placedPOIBounds, isRunningDebugGen, successCallbackFn);
			TemplateSpawning.s_minProgressPercent = TemplateSpawning.s_maxProgressPercent;
			TemplateSpawning.s_maxProgressPercent = 0.66f;
			TemplateSpawning.SpawnTemplatesFromTemplateRules(settings, terrainCells, myRandom, ref result, ref placedPOIBounds, isRunningDebugGen, successCallbackFn);
			TemplateSpawning.s_minProgressPercent = TemplateSpawning.s_maxProgressPercent;
			TemplateSpawning.s_maxProgressPercent = 1f;
			TemplateSpawning.SpawnStoryTraitTemplates(settings, terrainCells, myRandom, ref result, ref placedPOIBounds, ref placedStoryTraits, isRunningDebugGen, successCallbackFn);
			successCallbackFn(UI.WORLDGEN.PLACINGTEMPLATES.key, 1f, WorldGenProgressStages.Stages.PlaceTemplates);
			return result;
		}

		// Token: 0x0600B3F6 RID: 46070 RVA: 0x001197E0 File Offset: 0x001179E0
		private static float ProgressPercent(float stagePercent)
		{
			return MathUtil.ReRange(stagePercent, 0f, 1f, TemplateSpawning.s_minProgressPercent, TemplateSpawning.s_maxProgressPercent);
		}

		// Token: 0x0600B3F7 RID: 46071 RVA: 0x00447448 File Offset: 0x00445648
		private static void SpawnStartingTemplate(WorldGenSettings settings, List<TerrainCell> terrainCells, ref List<TemplateSpawning.TemplateSpawner> templateSpawnTargets, ref List<RectInt> placedPOIBounds, bool isRunningDebugGen, WorldGen.OfflineCallbackFunction successCallbackFn)
		{
			TerrainCell terrainCell = terrainCells.Find((TerrainCell tc) => tc.node.tags.Contains(WorldGenTags.StartLocation));
			if (settings.world.startingBaseTemplate.IsNullOrWhiteSpace())
			{
				return;
			}
			TemplateContainer template = TemplateCache.GetTemplate(settings.world.startingBaseTemplate);
			Vector2I position = new Vector2I((int)terrainCell.poly.Centroid().x, (int)terrainCell.poly.Centroid().y);
			RectInt templateBounds = template.GetTemplateBounds(position, TemplateSpawning.s_poiPadding);
			TemplateSpawning.TemplateSpawner item = new TemplateSpawning.TemplateSpawner(position, templateBounds, template, terrainCell);
			if (TemplateSpawning.IsPOIOverlappingBounds(placedPOIBounds, templateBounds))
			{
				string text = "TemplateSpawning: Starting template overlaps world boundaries in world '" + settings.world.filePath + "'";
				DebugUtil.DevLogError(text);
				if (!isRunningDebugGen)
				{
					throw new Exception(text);
				}
			}
			successCallbackFn(UI.WORLDGEN.PLACINGTEMPLATES.key, TemplateSpawning.ProgressPercent(1f), WorldGenProgressStages.Stages.PlaceTemplates);
			templateSpawnTargets.Add(item);
			placedPOIBounds.Add(templateBounds);
		}

		// Token: 0x0600B3F8 RID: 46072 RVA: 0x00447548 File Offset: 0x00445748
		private static void SpawnTemplatesFromTemplateRules(WorldGenSettings settings, List<TerrainCell> terrainCells, SeededRandom myRandom, ref List<TemplateSpawning.TemplateSpawner> templateSpawnTargets, ref List<RectInt> placedPOIBounds, bool isRunningDebugGen, WorldGen.OfflineCallbackFunction successCallbackFn)
		{
			List<ProcGen.World.TemplateSpawnRules> list = new List<ProcGen.World.TemplateSpawnRules>();
			if (settings.world.worldTemplateRules != null)
			{
				list.AddRange(settings.world.worldTemplateRules);
			}
			foreach (WeightedSubworldName weightedSubworldName in settings.world.subworldFiles)
			{
				SubWorld subWorld = settings.GetSubWorld(weightedSubworldName.name);
				if (subWorld.subworldTemplateRules != null)
				{
					list.AddRange(subWorld.subworldTemplateRules);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			int num = 0;
			float num2 = (float)list.Count;
			list.Sort((ProcGen.World.TemplateSpawnRules a, ProcGen.World.TemplateSpawnRules b) => b.priority.CompareTo(a.priority));
			List<TemplateSpawning.TemplateSpawner> list2 = new List<TemplateSpawning.TemplateSpawner>();
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ProcGen.World.TemplateSpawnRules rule in list)
			{
				successCallbackFn(UI.WORLDGEN.PLACINGTEMPLATES.key, TemplateSpawning.ProgressPercent((float)num++ / num2), WorldGenProgressStages.Stages.PlaceTemplates);
				string text;
				if (!TemplateSpawning.ApplyTemplateRule(settings, terrainCells, myRandom, ref templateSpawnTargets, ref placedPOIBounds, rule, ref hashSet, out text, ref list2))
				{
					DebugUtil.LogErrorArgs(new object[]
					{
						text
					});
					if (!isRunningDebugGen)
					{
						throw new WorldgenException(text, UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE);
					}
				}
			}
		}

		// Token: 0x0600B3F9 RID: 46073 RVA: 0x004476C0 File Offset: 0x004458C0
		private static void SpawnStoryTraitTemplates(WorldGenSettings settings, List<TerrainCell> terrainCells, SeededRandom myRandom, ref List<TemplateSpawning.TemplateSpawner> templateSpawnTargets, ref List<RectInt> placedPOIBounds, ref List<WorldTrait> placedStoryTraits, bool isRunningDebugGen, WorldGen.OfflineCallbackFunction successCallbackFn)
		{
			Queue<WorldTrait> queue = new Queue<WorldTrait>(settings.GetStoryTraitCandiates());
			int count = queue.Count;
			List<WorldTrait> list = new List<WorldTrait>();
			HashSet<string> hashSet = new HashSet<string>();
			while (queue.Count > 0 && list.Count < count)
			{
				WorldTrait worldTrait = queue.Dequeue();
				bool flag = false;
				List<TemplateSpawning.TemplateSpawner> list2 = new List<TemplateSpawning.TemplateSpawner>();
				string text = "";
				List<ProcGen.World.TemplateSpawnRules> list3 = new List<ProcGen.World.TemplateSpawnRules>();
				list3.AddRange(worldTrait.additionalWorldTemplateRules);
				list3.Sort((ProcGen.World.TemplateSpawnRules a, ProcGen.World.TemplateSpawnRules b) => b.priority.CompareTo(a.priority));
				foreach (ProcGen.World.TemplateSpawnRules rule in list3)
				{
					flag = TemplateSpawning.ApplyTemplateRule(settings, terrainCells, myRandom, ref templateSpawnTargets, ref placedPOIBounds, rule, ref hashSet, out text, ref list2);
					if (!flag)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					placedStoryTraits.Add(worldTrait);
					list.Add(worldTrait);
					settings.ApplyStoryTrait(worldTrait);
					DebugUtil.LogArgs(new object[]
					{
						"Applied story trait '" + worldTrait.filePath + "'"
					});
				}
				else
				{
					foreach (TemplateSpawning.TemplateSpawner templateSpawner in list2)
					{
						TemplateSpawning.RemoveTemplate(templateSpawner, settings, terrainCells, ref templateSpawnTargets, ref placedPOIBounds);
						hashSet.Remove(templateSpawner.container.name);
					}
					if (DlcManager.FeatureClusterSpaceEnabled())
					{
						DebugUtil.LogArgs(new object[]
						{
							string.Concat(new string[]
							{
								"Cannot place story trait on '",
								worldTrait.filePath,
								"' and will try another world. error='",
								text,
								"'."
							})
						});
					}
					else
					{
						DebugUtil.LogArgs(new object[]
						{
							string.Concat(new string[]
							{
								"Cannot place story trait '",
								worldTrait.filePath,
								"' error='",
								text,
								"'"
							})
						});
					}
				}
			}
		}

		// Token: 0x0600B3FA RID: 46074 RVA: 0x004478D8 File Offset: 0x00445AD8
		private static void RemoveTemplate(TemplateSpawning.TemplateSpawner toRemove, WorldGenSettings settings, List<TerrainCell> terrainCells, ref List<TemplateSpawning.TemplateSpawner> templateSpawnTargets, ref List<RectInt> placedPOIBounds)
		{
			TemplateSpawning.UpdateNodeTags(toRemove.terrainCell.node, toRemove.container.name, true);
			templateSpawnTargets.Remove(toRemove);
			placedPOIBounds.RemoveAll((RectInt bound) => bound.center == toRemove.position);
		}

		// Token: 0x0600B3FB RID: 46075 RVA: 0x0044793C File Offset: 0x00445B3C
		private static bool ApplyTemplateRule(WorldGenSettings settings, List<TerrainCell> terrainCells, SeededRandom myRandom, ref List<TemplateSpawning.TemplateSpawner> templateSpawnTargets, ref List<RectInt> placedPOIBounds, ProcGen.World.TemplateSpawnRules rule, ref HashSet<string> usedTemplates, out string errorMessage, ref List<TemplateSpawning.TemplateSpawner> newTemplateSpawnTargets)
		{
			int i = 0;
			Predicate<TerrainCell> <>9__0;
			while (i < rule.times)
			{
				ListPool<string, TemplateSpawning>.PooledList pooledList = ListPool<string, TemplateSpawning>.Allocate();
				if (!rule.allowDuplicates)
				{
					using (List<string>.Enumerator enumerator = rule.names.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string text = enumerator.Current;
							if (!usedTemplates.Contains(text))
							{
								if (!TemplateCache.TemplateExists(text))
								{
									DebugUtil.DevLogError(string.Concat(new string[]
									{
										"TemplateSpawning: Missing template '",
										text,
										"' in world '",
										settings.world.filePath,
										"'"
									}));
								}
								else
								{
									pooledList.Add(text);
								}
							}
						}
						goto IL_CC;
					}
				}
				goto IL_BB;
				IL_CC:
				pooledList.ShuffleSeeded(myRandom.RandomSource());
				if (pooledList.Count != 0)
				{
					int num = 0;
					if (rule.listRule == ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeRange || rule.listRule == ProcGen.World.TemplateSpawnRules.ListRule.TryRange)
					{
						num = myRandom.RandomRange(rule.range.x, rule.range.y);
					}
					int num2 = 0;
					int num3 = 0;
					switch (rule.listRule)
					{
					case ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeOne:
						num2 = 1;
						num3 = 1;
						break;
					case ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeSome:
						num2 = rule.someCount;
						num3 = rule.someCount;
						break;
					case ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeSomeTryMore:
						num2 = rule.someCount;
						num3 = rule.someCount + rule.moreCount;
						break;
					case ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeAll:
						num2 = pooledList.Count;
						num3 = pooledList.Count;
						break;
					case ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeRange:
						num2 = num;
						num3 = num;
						break;
					case ProcGen.World.TemplateSpawnRules.ListRule.TryOne:
						num3 = 1;
						break;
					case ProcGen.World.TemplateSpawnRules.ListRule.TrySome:
						num3 = rule.someCount;
						break;
					case ProcGen.World.TemplateSpawnRules.ListRule.TryRange:
						num3 = num;
						break;
					case ProcGen.World.TemplateSpawnRules.ListRule.TryAll:
						num3 = pooledList.Count;
						break;
					}
					string text2 = "";
					foreach (string text3 in pooledList)
					{
						if (num3 <= 0)
						{
							break;
						}
						TemplateContainer template = TemplateCache.GetTemplate(text3);
						if (template != null)
						{
							bool guarantee = num2 > 0;
							Vector2I vector2I = Vector2I.zero;
							TerrainCell terrainCell;
							if (rule.overridePlacement != Vector2I.minusone)
							{
								vector2I = rule.overridePlacement;
								Predicate<TerrainCell> match;
								if ((match = <>9__0) == null)
								{
									match = (<>9__0 = ((TerrainCell x) => x.poly.Contains(rule.overridePlacement)));
								}
								terrainCell = terrainCells.Find(match);
								if (num2 > 0 && terrainCell.node.templateTag != Tag.Invalid)
								{
									errorMessage = string.Format("Tried to place '{0}' at ({1},{2}) using overridePlacement but '{3}' is already there.", new object[]
									{
										text3,
										vector2I.x,
										vector2I.y,
										terrainCell.node.templateTag
									});
									return false;
								}
							}
							else
							{
								terrainCell = TemplateSpawning.FindTargetForTemplate(template, rule, terrainCells, myRandom, ref templateSpawnTargets, ref placedPOIBounds, guarantee, settings);
								if (terrainCell != null)
								{
									vector2I = new Vector2I((int)terrainCell.poly.Centroid().x + rule.overrideOffset.x, (int)terrainCell.poly.Centroid().y + rule.overrideOffset.y);
								}
							}
							if (terrainCell != null)
							{
								RectInt templateBounds = template.GetTemplateBounds(vector2I, TemplateSpawning.s_poiPadding);
								TemplateSpawning.TemplateSpawner item = new TemplateSpawning.TemplateSpawner(vector2I, templateBounds, template, terrainCell);
								templateSpawnTargets.Add(item);
								newTemplateSpawnTargets.Add(item);
								placedPOIBounds.Add(templateBounds);
								TemplateSpawning.UpdateNodeTags(terrainCell.node, text3, false);
								usedTemplates.Add(text3);
								num3--;
								num2--;
							}
							else
							{
								text2 = text2 + "\n    - " + text3;
							}
						}
					}
					pooledList.Recycle();
					if (num2 > 0)
					{
						string text4 = string.Join(", ", settings.GetWorldTraitIDs());
						string text5 = string.Join(", ", settings.GetStoryTraitIDs());
						errorMessage = string.Concat(new string[]
						{
							"TemplateSpawning: Guaranteed placement failure on ",
							settings.world.filePath,
							"\n",
							string.Format("    listRule={0} someCount={1} moreCount={2} count={3}\n", new object[]
							{
								rule.listRule,
								rule.someCount,
								rule.moreCount,
								pooledList.Count
							}),
							"    Could not place templates:",
							text2,
							"\n    world traits=",
							text4,
							"\n    story traits=",
							text5
						});
						return false;
					}
					goto IL_4E3;
				}
				pooledList.Recycle();
				IL_4E3:
				i++;
				continue;
				IL_BB:
				pooledList.AddRange(rule.names);
				goto IL_CC;
			}
			errorMessage = "";
			return true;
		}

		// Token: 0x0600B3FC RID: 46076 RVA: 0x00447E84 File Offset: 0x00446084
		private static void UpdateNodeTags(Node node, string template, bool remove = false)
		{
			Tag tag = template.ToTag();
			if (remove)
			{
				node.templateTag = Tag.Invalid;
				node.tags.Remove(tag);
				node.tags.Remove(WorldGenTags.POI);
				return;
			}
			node.templateTag = tag;
			node.tags.Add(template.ToTag());
			node.tags.Add(WorldGenTags.POI);
		}

		// Token: 0x0600B3FD RID: 46077 RVA: 0x00447EF0 File Offset: 0x004460F0
		private static TerrainCell FindTargetForTemplate(TemplateContainer template, ProcGen.World.TemplateSpawnRules rule, List<TerrainCell> terrainCells, SeededRandom myRandom, ref List<TemplateSpawning.TemplateSpawner> templateSpawnTargets, ref List<RectInt> placedPOIBounds, bool guarantee, WorldGenSettings settings)
		{
			List<TerrainCell> list;
			if (rule.allowNearStart)
			{
				list = terrainCells.FindAll(delegate(TerrainCell tc)
				{
					tc.LogInfo("Filtering Near Start", template.name, 0f);
					return tc.IsSafeToSpawnPOINearStart(terrainCells, true) && TemplateSpawning.DoesCellMatchFilters(tc, rule.allowedCellsFilter);
				});
			}
			else if (!rule.useRelaxedFiltering)
			{
				list = terrainCells.FindAll(delegate(TerrainCell tc)
				{
					tc.LogInfo("Filtering", template.name, 0f);
					return tc.IsSafeToSpawnPOI(terrainCells, true) && TemplateSpawning.DoesCellMatchFilters(tc, rule.allowedCellsFilter);
				});
			}
			else
			{
				list = terrainCells.FindAll(delegate(TerrainCell tc)
				{
					tc.LogInfo("Filtering Relaxed (replace features)", template.name, 0f);
					return tc.IsSafeToSpawnPOIRelaxed(terrainCells, true) && TemplateSpawning.DoesCellMatchFilters(tc, rule.allowedCellsFilter);
				});
			}
			TemplateSpawning.RemoveOverlappingPOIs(ref list, ref terrainCells, ref placedPOIBounds, template, settings, rule.allowExtremeTemperatureOverlap, rule.overrideOffset);
			if (list.Count == 0 && guarantee)
			{
				if (rule.allowNearStart && rule.useRelaxedFiltering)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Could not place " + template.name + " using normal rules, trying relaxed near start"
					});
					list = terrainCells.FindAll(delegate(TerrainCell tc)
					{
						tc.LogInfo("Filtering Near Start Relaxed", template.name, 0f);
						return tc.IsSafeToSpawnPOINearStartRelaxed(terrainCells, true) && TemplateSpawning.DoesCellMatchFilters(tc, rule.allowedCellsFilter);
					});
					TemplateSpawning.RemoveOverlappingPOIs(ref list, ref terrainCells, ref placedPOIBounds, template, settings, rule.allowExtremeTemperatureOverlap, rule.overrideOffset);
				}
				else if (!rule.useRelaxedFiltering)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Could not place " + template.name + " using normal rules, trying relaxed"
					});
					list = terrainCells.FindAll(delegate(TerrainCell tc)
					{
						tc.LogInfo("Filtering Relaxed", template.name, 0f);
						return tc.IsSafeToSpawnPOIRelaxed(terrainCells, true) && TemplateSpawning.DoesCellMatchFilters(tc, rule.allowedCellsFilter);
					});
					TemplateSpawning.RemoveOverlappingPOIs(ref list, ref terrainCells, ref placedPOIBounds, template, settings, rule.allowExtremeTemperatureOverlap, rule.overrideOffset);
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			list.ShuffleSeeded(myRandom.RandomSource());
			return list[list.Count - 1];
		}

		// Token: 0x0600B3FE RID: 46078 RVA: 0x004480F8 File Offset: 0x004462F8
		private static bool IsPOIOverlappingBounds(List<RectInt> placedPOIBounds, RectInt templateBounds)
		{
			foreach (RectInt other in placedPOIBounds)
			{
				if (templateBounds.Overlaps(other))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B3FF RID: 46079 RVA: 0x00448150 File Offset: 0x00446350
		private static bool IsPOIOverlappingHighTemperatureDelta(RectInt paddedTemplateBounds, SubWorld subworld, ref List<TerrainCell> allCells, WorldGenSettings settings)
		{
			Vector2 b = 2f * Vector2.one * (float)TemplateSpawning.s_poiPadding;
			Vector2 b2 = 2f * Vector2.one * 3f;
			Rect rect = new Rect(paddedTemplateBounds.position, paddedTemplateBounds.size - b + b2);
			Temperature temperature = SettingsCache.temperatures[subworld.temperatureRange];
			foreach (TerrainCell terrainCell in allCells)
			{
				SubWorld subWorld = settings.GetSubWorld(terrainCell.node.GetSubworld());
				Temperature temperature2 = SettingsCache.temperatures[subWorld.temperatureRange];
				if (subWorld.temperatureRange != subworld.temperatureRange)
				{
					float num = Mathf.Min(temperature.min, temperature2.min);
					float num2 = Mathf.Max(temperature.max, temperature2.max) - num;
					bool flag = rect.Overlaps(terrainCell.poly.bounds);
					bool flag2 = num2 > 100f;
					if (flag && flag2)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600B400 RID: 46080 RVA: 0x004482A0 File Offset: 0x004464A0
		private static void RemoveOverlappingPOIs(ref List<TerrainCell> filteredTerrainCells, ref List<TerrainCell> allCells, ref List<RectInt> placedPOIBounds, TemplateContainer container, WorldGenSettings settings, bool allowExtremeTemperatureOverlap, Vector2 poiOffset)
		{
			for (int i = filteredTerrainCells.Count - 1; i >= 0; i--)
			{
				TerrainCell terrainCell = filteredTerrainCells[i];
				int index = i;
				SubWorld subWorld = settings.GetSubWorld(terrainCell.node.GetSubworld());
				RectInt templateBounds = container.GetTemplateBounds(terrainCell.poly.Centroid() + poiOffset, TemplateSpawning.s_poiPadding);
				bool flag = false;
				if (TemplateSpawning.IsPOIOverlappingBounds(placedPOIBounds, templateBounds))
				{
					terrainCell.LogInfo("-> Removed due to overlapping POIs", "", 0f);
					flag = true;
				}
				else if (!allowExtremeTemperatureOverlap && TemplateSpawning.IsPOIOverlappingHighTemperatureDelta(templateBounds, subWorld, ref allCells, settings))
				{
					terrainCell.LogInfo("-> Removed due to overlapping extreme temperature delta", "", 0f);
					flag = true;
				}
				if (flag)
				{
					filteredTerrainCells.RemoveAt(index);
				}
			}
		}

		// Token: 0x0600B401 RID: 46081 RVA: 0x00448364 File Offset: 0x00446564
		private static bool DoesCellMatchFilters(TerrainCell cell, List<ProcGen.World.AllowedCellsFilter> filters)
		{
			bool flag = false;
			foreach (ProcGen.World.AllowedCellsFilter allowedCellsFilter in filters)
			{
				bool flag3;
				bool flag2 = TemplateSpawning.DoesCellMatchFilter(cell, allowedCellsFilter, out flag3);
				if (flag3)
				{
					switch (allowedCellsFilter.command)
					{
					case ProcGen.World.AllowedCellsFilter.Command.Clear:
						flag = false;
						break;
					case ProcGen.World.AllowedCellsFilter.Command.Replace:
						flag = flag2;
						break;
					case ProcGen.World.AllowedCellsFilter.Command.UnionWith:
						flag = (flag2 || flag);
						break;
					case ProcGen.World.AllowedCellsFilter.Command.IntersectWith:
						flag = (flag2 && flag);
						break;
					case ProcGen.World.AllowedCellsFilter.Command.ExceptWith:
					case ProcGen.World.AllowedCellsFilter.Command.SymmetricExceptWith:
						if (flag2)
						{
							flag = false;
						}
						break;
					case ProcGen.World.AllowedCellsFilter.Command.All:
						flag = true;
						break;
					}
					cell.LogInfo("-> DoesCellMatchFilter " + allowedCellsFilter.command.ToString(), flag2 ? "1" : "0", (float)(flag ? 1 : 0));
				}
			}
			cell.LogInfo("> Final match", flag ? "true" : "false", 0f);
			return flag;
		}

		// Token: 0x0600B402 RID: 46082 RVA: 0x0044846C File Offset: 0x0044666C
		private static bool DoesCellMatchFilter(TerrainCell cell, ProcGen.World.AllowedCellsFilter filter, out bool applied)
		{
			applied = true;
			if (!TemplateSpawning.ValidateFilter(filter))
			{
				return false;
			}
			if (filter.tagcommand == ProcGen.World.AllowedCellsFilter.TagCommand.Default)
			{
				if (filter.subworldNames != null && filter.subworldNames.Count > 0)
				{
					foreach (string s in filter.subworldNames)
					{
						if (cell.node.tags.Contains(s))
						{
							return true;
						}
					}
					return false;
				}
				if (filter.zoneTypes != null && filter.zoneTypes.Count > 0)
				{
					foreach (SubWorld.ZoneType zoneType in filter.zoneTypes)
					{
						if (cell.node.tags.Contains(zoneType.ToString()))
						{
							return true;
						}
					}
					return false;
				}
				if (filter.temperatureRanges != null && filter.temperatureRanges.Count > 0)
				{
					foreach (Temperature.Range range in filter.temperatureRanges)
					{
						if (cell.node.tags.Contains(range.ToString()))
						{
							return true;
						}
					}
					return false;
				}
				return true;
			}
			switch (filter.tagcommand)
			{
			case ProcGen.World.AllowedCellsFilter.TagCommand.Default:
				return true;
			case ProcGen.World.AllowedCellsFilter.TagCommand.AtTag:
				return cell.node.tags.Contains(filter.tag);
			case ProcGen.World.AllowedCellsFilter.TagCommand.NotAtTag:
				return !cell.node.tags.Contains(filter.tag);
			case ProcGen.World.AllowedCellsFilter.TagCommand.DistanceFromTag:
			{
				Tag tag = filter.tag.ToTag();
				bool flag = cell.distancesToTags.ContainsKey(tag);
				if (!flag && tag == WorldGenTags.AtStart && !filter.ignoreIfMissingTag)
				{
					DebugUtil.DevLogError("DistanceFromTag was used on a world without an AtStart tag, use ignoreIfMissingTag to skip it.");
				}
				else
				{
					global::Debug.Assert(flag || filter.ignoreIfMissingTag, "DistanceFromTag is missing tag " + filter.tag + ", use ignoreIfMissingTag to skip it.");
				}
				if (flag)
				{
					int num = cell.DistanceToTag(tag);
					return num >= filter.minDistance && num <= filter.maxDistance;
				}
				applied = false;
				return true;
			}
			}
			return true;
		}

		// Token: 0x0600B403 RID: 46083 RVA: 0x004486F8 File Offset: 0x004468F8
		private static bool ValidateFilter(ProcGen.World.AllowedCellsFilter filter)
		{
			if (filter.command == ProcGen.World.AllowedCellsFilter.Command.All)
			{
				return true;
			}
			int num = 0;
			if (filter.tagcommand != ProcGen.World.AllowedCellsFilter.TagCommand.Default)
			{
				num++;
			}
			if (filter.subworldNames != null && filter.subworldNames.Count > 0)
			{
				num++;
			}
			if (filter.zoneTypes != null && filter.zoneTypes.Count > 0)
			{
				num++;
			}
			if (filter.temperatureRanges != null && filter.temperatureRanges.Count > 0)
			{
				num++;
			}
			if (num != 1)
			{
				string text = "BAD ALLOWED CELLS FILTER in FEATURE RULES!";
				text += "\nA filter can only specify one of `tagcommand`, `subworldNames`, `zoneTypes`, or `temperatureRanges`.";
				text += "\nFound a filter with the following:";
				if (filter.tagcommand != ProcGen.World.AllowedCellsFilter.TagCommand.Default)
				{
					text += "\ntagcommand:\n\t";
					text += filter.tagcommand.ToString();
					text += "\ntag:\n\t";
					text += filter.tag;
				}
				if (filter.subworldNames != null && filter.subworldNames.Count > 0)
				{
					text += "\nsubworldNames:\n\t";
					text += string.Join(", ", filter.subworldNames);
				}
				if (filter.zoneTypes != null && filter.zoneTypes.Count > 0)
				{
					text += "\nzoneTypes:\n";
					text += string.Join<SubWorld.ZoneType>(", ", filter.zoneTypes);
				}
				if (filter.temperatureRanges != null && filter.temperatureRanges.Count > 0)
				{
					text += "\ntemperatureRanges:\n";
					text += string.Join<Temperature.Range>(", ", filter.temperatureRanges);
				}
				global::Debug.LogError(text);
				return false;
			}
			return true;
		}

		// Token: 0x04008E7C RID: 36476
		private static float s_minProgressPercent;

		// Token: 0x04008E7D RID: 36477
		private static float s_maxProgressPercent;

		// Token: 0x04008E7E RID: 36478
		private static int s_poiPadding;

		// Token: 0x04008E7F RID: 36479
		private const int TEMPERATURE_PADDING = 3;

		// Token: 0x04008E80 RID: 36480
		private const float EXTREME_POI_OVERLAP_TEMPERATURE_RANGE = 100f;

		// Token: 0x0200210A RID: 8458
		public class TemplateSpawner
		{
			// Token: 0x0600B405 RID: 46085 RVA: 0x001197FC File Offset: 0x001179FC
			public TemplateSpawner(Vector2I position, RectInt bounds, TemplateContainer container, TerrainCell terrainCell)
			{
				this.position = position;
				this.container = container;
				this.terrainCell = terrainCell;
				this.bounds = bounds;
			}

			// Token: 0x04008E81 RID: 36481
			public Vector2I position;

			// Token: 0x04008E82 RID: 36482
			public TemplateContainer container;

			// Token: 0x04008E83 RID: 36483
			public TerrainCell terrainCell;

			// Token: 0x04008E84 RID: 36484
			public RectInt bounds;
		}
	}
}
