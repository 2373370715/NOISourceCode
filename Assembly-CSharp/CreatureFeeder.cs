using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02001CD7 RID: 7383
[AddComponentMenu("KMonoBehaviour/scripts/CreatureFeeder")]
public class CreatureFeeder : KMonoBehaviour
{
	// Token: 0x060099F2 RID: 39410 RVA: 0x00108872 File Offset: 0x00106A72
	protected override void OnSpawn()
	{
		this.storages = base.GetComponents<Storage>();
		Components.CreatureFeeders.Add(this.GetMyWorldId(), this);
		base.Subscribe<CreatureFeeder>(-1452790913, CreatureFeeder.OnAteFromStorageDelegate);
	}

	// Token: 0x060099F3 RID: 39411 RVA: 0x001088A2 File Offset: 0x00106AA2
	protected override void OnCleanUp()
	{
		Components.CreatureFeeders.Remove(this.GetMyWorldId(), this);
	}

	// Token: 0x060099F4 RID: 39412 RVA: 0x001088B5 File Offset: 0x00106AB5
	private void OnAteFromStorage(object data)
	{
		if (string.IsNullOrEmpty(this.effectId))
		{
			return;
		}
		(data as GameObject).GetComponent<Effects>().Add(this.effectId, true);
	}

	// Token: 0x060099F5 RID: 39413 RVA: 0x003C5C7C File Offset: 0x003C3E7C
	public bool StoragesAreEmpty()
	{
		foreach (Storage storage in this.storages)
		{
			if (!(storage == null) && storage.Count > 0)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060099F6 RID: 39414 RVA: 0x001088DD File Offset: 0x00106ADD
	public Vector2I GetTargetFeederCell()
	{
		return Grid.CellToXY(Grid.OffsetCell(Grid.PosToCell(this), this.feederOffset));
	}

	// Token: 0x0400781F RID: 30751
	public Storage[] storages;

	// Token: 0x04007820 RID: 30752
	public string effectId;

	// Token: 0x04007821 RID: 30753
	public CellOffset feederOffset = CellOffset.none;

	// Token: 0x04007822 RID: 30754
	private static readonly EventSystem.IntraObjectHandler<CreatureFeeder> OnAteFromStorageDelegate = new EventSystem.IntraObjectHandler<CreatureFeeder>(delegate(CreatureFeeder component, object data)
	{
		component.OnAteFromStorage(data);
	});
}
