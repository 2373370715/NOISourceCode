using System;
using UnityEngine;

// Token: 0x02001A47 RID: 6727
public class TrapTrigger : KMonoBehaviour
{
	// Token: 0x06008C2E RID: 35886 RVA: 0x00370DC8 File Offset: 0x0036EFC8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameObject gameObject = base.gameObject;
		this.SetTriggerCell(Grid.PosToCell(gameObject));
		foreach (GameObject gameObject2 in this.storage.items)
		{
			this.SetStoredPosition(gameObject2);
			KBoxCollider2D component = gameObject2.GetComponent<KBoxCollider2D>();
			if (component != null)
			{
				component.enabled = true;
			}
		}
	}

	// Token: 0x06008C2F RID: 35887 RVA: 0x00370E50 File Offset: 0x0036F050
	public void SetTriggerCell(int cell)
	{
		HandleVector<int>.Handle handle = this.partitionerEntry;
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Trap", base.gameObject, cell, GameScenePartitioner.Instance.trapsLayer, new Action<object>(this.OnCreatureOnTrap));
	}

	// Token: 0x06008C30 RID: 35888 RVA: 0x00370EA8 File Offset: 0x0036F0A8
	public void SetStoredPosition(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		Vector3 vector = Grid.CellToPosCBC(Grid.PosToCell(base.transform.GetPosition()), Grid.SceneLayer.BuildingBack);
		if (this.addTrappedAnimationOffset)
		{
			vector.x += this.trappedOffset.x - component.Offset.x;
			vector.y += this.trappedOffset.y - component.Offset.y;
		}
		else
		{
			vector.x += this.trappedOffset.x;
			vector.y += this.trappedOffset.y;
		}
		go.transform.SetPosition(vector);
		go.GetComponent<Pickupable>().UpdateCachedCell(Grid.PosToCell(vector));
		component.SetSceneLayer(Grid.SceneLayer.BuildingFront);
	}

	// Token: 0x06008C31 RID: 35889 RVA: 0x00370F80 File Offset: 0x0036F180
	public void OnCreatureOnTrap(object data)
	{
		if (!base.enabled)
		{
			return;
		}
		if (!this.storage.IsEmpty())
		{
			return;
		}
		Trappable trappable = (Trappable)data;
		if (trappable.HasTag(GameTags.Stored))
		{
			return;
		}
		if (trappable.HasTag(GameTags.Trapped))
		{
			return;
		}
		if (trappable.HasTag(GameTags.Creatures.Bagged))
		{
			return;
		}
		bool flag = false;
		foreach (Tag tag in this.trappableCreatures)
		{
			if (trappable.HasTag(tag))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		this.storage.Store(trappable.gameObject, true, false, true, false);
		this.SetStoredPosition(trappable.gameObject);
		base.Trigger(-358342870, trappable.gameObject);
	}

	// Token: 0x06008C32 RID: 35890 RVA: 0x0010033E File Offset: 0x000FE53E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x040069D9 RID: 27097
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040069DA RID: 27098
	public Tag[] trappableCreatures;

	// Token: 0x040069DB RID: 27099
	public Vector2 trappedOffset = Vector2.zero;

	// Token: 0x040069DC RID: 27100
	public bool addTrappedAnimationOffset = true;

	// Token: 0x040069DD RID: 27101
	[MyCmpReq]
	private Storage storage;
}
