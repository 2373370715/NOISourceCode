using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x020012E3 RID: 4835
[AddComponentMenu("KMonoBehaviour/scripts/EntombedItemManager")]
public class EntombedItemManager : KMonoBehaviour, ISim33ms
{
	// Token: 0x0600632E RID: 25390 RVA: 0x000E5202 File Offset: 0x000E3402
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.SpawnUncoveredObjects();
		this.AddMassToWorldIfPossible();
		this.PopulateEntombedItemVisualizers();
	}

	// Token: 0x0600632F RID: 25391 RVA: 0x002C7460 File Offset: 0x002C5660
	public static bool CanEntomb(Pickupable pickupable)
	{
		if (pickupable == null)
		{
			return false;
		}
		if (pickupable.storage != null)
		{
			return false;
		}
		int num = Grid.PosToCell(pickupable);
		return Grid.IsValidCell(num) && Grid.Solid[num] && !(Grid.Objects[num, 9] != null) && (pickupable.PrimaryElement.Element.IsSolid && pickupable.GetComponent<ElementChunk>() != null);
	}

	// Token: 0x06006330 RID: 25392 RVA: 0x000E5216 File Offset: 0x000E3416
	public void Add(Pickupable pickupable)
	{
		this.pickupables.Add(pickupable);
	}

	// Token: 0x06006331 RID: 25393 RVA: 0x002C74E4 File Offset: 0x002C56E4
	public void Sim33ms(float dt)
	{
		EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
		HashSetPool<Pickupable, EntombedItemManager>.PooledHashSet pooledHashSet = HashSetPool<Pickupable, EntombedItemManager>.Allocate();
		foreach (Pickupable pickupable in this.pickupables)
		{
			if (EntombedItemManager.CanEntomb(pickupable))
			{
				pooledHashSet.Add(pickupable);
			}
		}
		this.pickupables.Clear();
		foreach (Pickupable pickupable2 in pooledHashSet)
		{
			int num = Grid.PosToCell(pickupable2);
			PrimaryElement primaryElement = pickupable2.PrimaryElement;
			SimHashes elementID = primaryElement.ElementID;
			float mass = primaryElement.Mass;
			float temperature = primaryElement.Temperature;
			byte diseaseIdx = primaryElement.DiseaseIdx;
			int diseaseCount = primaryElement.DiseaseCount;
			Element element = Grid.Element[num];
			if (elementID == element.id && mass > 0.010000001f && Grid.Mass[num] + mass < element.maxMass)
			{
				SimMessages.AddRemoveSubstance(num, ElementLoader.FindElementByHash(elementID).idx, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature, diseaseIdx, diseaseCount, true, -1);
			}
			else
			{
				component.AddItem(num);
				this.cells.Add(num);
				this.elementIds.Add((int)elementID);
				this.masses.Add(mass);
				this.temperatures.Add(temperature);
				this.diseaseIndices.Add(diseaseIdx);
				this.diseaseCounts.Add(diseaseCount);
			}
			Util.KDestroyGameObject(pickupable2.gameObject);
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x06006332 RID: 25394 RVA: 0x002C76AC File Offset: 0x002C58AC
	public void OnSolidChanged(List<int> solid_changed_cells)
	{
		ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
		foreach (int num in solid_changed_cells)
		{
			if (!Grid.Solid[num])
			{
				pooledList.Add(num);
			}
		}
		ListPool<int, EntombedItemManager>.PooledList pooledList2 = ListPool<int, EntombedItemManager>.Allocate();
		for (int i = 0; i < this.cells.Count; i++)
		{
			int num2 = this.cells[i];
			foreach (int num3 in pooledList)
			{
				if (num2 == num3)
				{
					pooledList2.Add(i);
					break;
				}
			}
		}
		pooledList.Recycle();
		this.SpawnObjects(pooledList2);
		pooledList2.Recycle();
	}

	// Token: 0x06006333 RID: 25395 RVA: 0x002C7798 File Offset: 0x002C5998
	private void SpawnUncoveredObjects()
	{
		ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
		for (int i = 0; i < this.cells.Count; i++)
		{
			int i2 = this.cells[i];
			if (!Grid.Solid[i2])
			{
				pooledList.Add(i);
			}
		}
		this.SpawnObjects(pooledList);
		pooledList.Recycle();
	}

	// Token: 0x06006334 RID: 25396 RVA: 0x002C77F0 File Offset: 0x002C59F0
	private void AddMassToWorldIfPossible()
	{
		ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
		for (int i = 0; i < this.cells.Count; i++)
		{
			int num = this.cells[i];
			if (Grid.Solid[num] && Grid.Element[num].id == (SimHashes)this.elementIds[i])
			{
				pooledList.Add(i);
			}
		}
		pooledList.Sort();
		pooledList.Reverse();
		foreach (int item_idx in pooledList)
		{
			EntombedItemManager.Item item = this.GetItem(item_idx);
			this.RemoveItem(item_idx);
			if (item.mass > 1E-45f)
			{
				SimMessages.AddRemoveSubstance(item.cell, ElementLoader.FindElementByHash((SimHashes)item.elementId).idx, CellEventLogger.Instance.ElementConsumerSimUpdate, item.mass, item.temperature, item.diseaseIdx, item.diseaseCount, false, -1);
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06006335 RID: 25397 RVA: 0x002C7908 File Offset: 0x002C5B08
	private void RemoveItem(int item_idx)
	{
		this.cells.RemoveAt(item_idx);
		this.elementIds.RemoveAt(item_idx);
		this.masses.RemoveAt(item_idx);
		this.temperatures.RemoveAt(item_idx);
		this.diseaseIndices.RemoveAt(item_idx);
		this.diseaseCounts.RemoveAt(item_idx);
	}

	// Token: 0x06006336 RID: 25398 RVA: 0x002C7960 File Offset: 0x002C5B60
	private EntombedItemManager.Item GetItem(int item_idx)
	{
		return new EntombedItemManager.Item
		{
			cell = this.cells[item_idx],
			elementId = this.elementIds[item_idx],
			mass = this.masses[item_idx],
			temperature = this.temperatures[item_idx],
			diseaseIdx = this.diseaseIndices[item_idx],
			diseaseCount = this.diseaseCounts[item_idx]
		};
	}

	// Token: 0x06006337 RID: 25399 RVA: 0x002C79E8 File Offset: 0x002C5BE8
	private void SpawnObjects(List<int> uncovered_item_indices)
	{
		uncovered_item_indices.Sort();
		uncovered_item_indices.Reverse();
		EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
		foreach (int item_idx in uncovered_item_indices)
		{
			EntombedItemManager.Item item = this.GetItem(item_idx);
			component.RemoveItem(item.cell);
			this.RemoveItem(item_idx);
			Element element = ElementLoader.FindElementByHash((SimHashes)item.elementId);
			if (element != null)
			{
				element.substance.SpawnResource(Grid.CellToPosCCC(item.cell, Grid.SceneLayer.Ore), item.mass, item.temperature, item.diseaseIdx, item.diseaseCount, false, false, false);
			}
		}
	}

	// Token: 0x06006338 RID: 25400 RVA: 0x002C7AA8 File Offset: 0x002C5CA8
	private void PopulateEntombedItemVisualizers()
	{
		EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
		foreach (int cell in this.cells)
		{
			component.AddItem(cell);
		}
	}

	// Token: 0x04004718 RID: 18200
	[Serialize]
	private List<int> cells = new List<int>();

	// Token: 0x04004719 RID: 18201
	[Serialize]
	private List<int> elementIds = new List<int>();

	// Token: 0x0400471A RID: 18202
	[Serialize]
	private List<float> masses = new List<float>();

	// Token: 0x0400471B RID: 18203
	[Serialize]
	private List<float> temperatures = new List<float>();

	// Token: 0x0400471C RID: 18204
	[Serialize]
	private List<byte> diseaseIndices = new List<byte>();

	// Token: 0x0400471D RID: 18205
	[Serialize]
	private List<int> diseaseCounts = new List<int>();

	// Token: 0x0400471E RID: 18206
	private List<Pickupable> pickupables = new List<Pickupable>();

	// Token: 0x020012E4 RID: 4836
	private struct Item
	{
		// Token: 0x0400471F RID: 18207
		public int cell;

		// Token: 0x04004720 RID: 18208
		public int elementId;

		// Token: 0x04004721 RID: 18209
		public float mass;

		// Token: 0x04004722 RID: 18210
		public float temperature;

		// Token: 0x04004723 RID: 18211
		public byte diseaseIdx;

		// Token: 0x04004724 RID: 18212
		public int diseaseCount;
	}
}
