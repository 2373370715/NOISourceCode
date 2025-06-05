using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using KSerialization;
using TemplateClasses;

namespace ProcGenGame
{
	// Token: 0x020020FF RID: 8447
	[SerializationConfig(MemberSerialization.OptOut)]
	public class GameSpawnData
	{
		// Token: 0x0600B3D6 RID: 46038 RVA: 0x0044591C File Offset: 0x00443B1C
		public void AddRange(IEnumerable<KeyValuePair<int, string>> newItems)
		{
			foreach (KeyValuePair<int, string> keyValuePair in newItems)
			{
				Vector2I vector2I = Grid.CellToXY(keyValuePair.Key);
				Prefab item = new Prefab(keyValuePair.Value, Prefab.Type.Other, vector2I.x, vector2I.y, (SimHashes)0, -1f, 1f, null, 0, Orientation.Neutral, null, null, 0, null);
				this.otherEntities.Add(item);
			}
		}

		// Token: 0x0600B3D7 RID: 46039 RVA: 0x004459A4 File Offset: 0x00443BA4
		public void AddTemplate(TemplateContainer template, Vector2I position, ref Dictionary<int, int> claimedCells)
		{
			int cell = Grid.XYToCell(position.x, position.y);
			bool flag = true;
			if (DlcManager.IsExpansion1Active() && CustomGameSettings.Instance != null)
			{
				flag = (CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Teleporters).id == "Enabled");
			}
			if (template.buildings != null)
			{
				foreach (Prefab prefab in template.buildings)
				{
					if (!claimedCells.ContainsKey(Grid.OffsetCell(cell, prefab.location_x, prefab.location_y)) && (flag || !this.IsWarpTeleporter(prefab)))
					{
						this.buildings.Add(prefab.Clone(position));
					}
				}
			}
			if (template.pickupables != null)
			{
				foreach (Prefab prefab2 in template.pickupables)
				{
					if (!claimedCells.ContainsKey(Grid.OffsetCell(cell, prefab2.location_x, prefab2.location_y)))
					{
						this.pickupables.Add(prefab2.Clone(position));
					}
				}
			}
			if (template.elementalOres != null)
			{
				foreach (Prefab prefab3 in template.elementalOres)
				{
					if (!claimedCells.ContainsKey(Grid.OffsetCell(cell, prefab3.location_x, prefab3.location_y)))
					{
						this.elementalOres.Add(prefab3.Clone(position));
					}
				}
			}
			if (template.otherEntities != null)
			{
				foreach (Prefab prefab4 in template.otherEntities)
				{
					if (!claimedCells.ContainsKey(Grid.OffsetCell(cell, prefab4.location_x, prefab4.location_y)) && (flag || !this.IsWarpTeleporter(prefab4)))
					{
						this.otherEntities.Add(prefab4.Clone(position));
					}
				}
			}
			if (template.cells != null)
			{
				for (int i = 0; i < template.cells.Count; i++)
				{
					int num = Grid.XYToCell(position.x + template.cells[i].location_x, position.y + template.cells[i].location_y);
					if (!claimedCells.ContainsKey(num))
					{
						claimedCells[num] = 1;
						this.preventFoWReveal.Add(new KeyValuePair<Vector2I, bool>(new Vector2I(position.x + template.cells[i].location_x, position.y + template.cells[i].location_y), template.cells[i].preventFoWReveal));
					}
					else
					{
						Dictionary<int, int> dictionary = claimedCells;
						int j = num;
						dictionary[j]++;
					}
				}
			}
			if (template.info != null && template.info.discover_tags != null)
			{
				foreach (Tag item in template.info.discover_tags)
				{
					this.discoveredResources.Add(item);
				}
			}
		}

		// Token: 0x0600B3D8 RID: 46040 RVA: 0x00445D20 File Offset: 0x00443F20
		private bool IsWarpTeleporter(Prefab prefab)
		{
			return prefab.id == "WarpPortal" || prefab.id == WarpReceiverConfig.ID || prefab.id == "WarpConduitSender" || prefab.id == "WarpConduitReceiver";
		}

		// Token: 0x04008E65 RID: 36453
		public Vector2I baseStartPos;

		// Token: 0x04008E66 RID: 36454
		public List<Prefab> buildings = new List<Prefab>();

		// Token: 0x04008E67 RID: 36455
		public List<Prefab> pickupables = new List<Prefab>();

		// Token: 0x04008E68 RID: 36456
		public List<Prefab> elementalOres = new List<Prefab>();

		// Token: 0x04008E69 RID: 36457
		public List<Prefab> otherEntities = new List<Prefab>();

		// Token: 0x04008E6A RID: 36458
		public List<Tag> discoveredResources = new List<Tag>();

		// Token: 0x04008E6B RID: 36459
		public List<KeyValuePair<Vector2I, bool>> preventFoWReveal = new List<KeyValuePair<Vector2I, bool>>();
	}
}
