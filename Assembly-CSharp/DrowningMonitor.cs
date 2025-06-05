using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020011A3 RID: 4515
[AddComponentMenu("KMonoBehaviour/scripts/DrowningMonitor")]
public class DrowningMonitor : KMonoBehaviour, IWiltCause, ISlicedSim1000ms
{
	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x06005BD0 RID: 23504 RVA: 0x000E0359 File Offset: 0x000DE559
	private OccupyArea occupyArea
	{
		get
		{
			if (this._occupyArea == null)
			{
				this._occupyArea = base.GetComponent<OccupyArea>();
			}
			return this._occupyArea;
		}
	}

	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x06005BD1 RID: 23505 RVA: 0x000E037B File Offset: 0x000DE57B
	public bool Drowning
	{
		get
		{
			return this.drowning;
		}
	}

	// Token: 0x06005BD2 RID: 23506 RVA: 0x002A6FD4 File Offset: 0x002A51D4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.timeToDrown = 75f;
		if (DrowningMonitor.drowningEffect == null)
		{
			DrowningMonitor.drowningEffect = new Effect("Drowning", CREATURES.STATUSITEMS.DROWNING.NAME, CREATURES.STATUSITEMS.DROWNING.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
			DrowningMonitor.drowningEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -100f, CREATURES.STATUSITEMS.DROWNING.NAME, false, false, true));
		}
		if (DrowningMonitor.saturatedEffect == null)
		{
			DrowningMonitor.saturatedEffect = new Effect("Saturated", CREATURES.STATUSITEMS.SATURATED.NAME, CREATURES.STATUSITEMS.SATURATED.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
			DrowningMonitor.saturatedEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -100f, CREATURES.STATUSITEMS.SATURATED.NAME, false, false, true));
		}
	}

	// Token: 0x06005BD3 RID: 23507 RVA: 0x002A70E4 File Offset: 0x002A52E4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SlicedUpdaterSim1000ms<DrowningMonitor>.instance.RegisterUpdate1000ms(this);
		this.OnMove();
		this.CheckDrowning(null);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnMove), "DrowningMonitor.OnSpawn");
	}

	// Token: 0x06005BD4 RID: 23508 RVA: 0x002A7134 File Offset: 0x002A5334
	private void OnMove()
	{
		if (this.partitionerEntry.IsValid())
		{
			Extents ext = this.occupyArea.GetExtents();
			GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, ext);
		}
		else
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add("DrowningMonitor.OnSpawn", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
		}
		this.CheckDrowning(null);
	}

	// Token: 0x06005BD5 RID: 23509 RVA: 0x000E0383 File Offset: 0x000DE583
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnMove));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		SlicedUpdaterSim1000ms<DrowningMonitor>.instance.UnregisterUpdate1000ms(this);
		base.OnCleanUp();
	}

	// Token: 0x06005BD6 RID: 23510 RVA: 0x002A71B0 File Offset: 0x002A53B0
	private void CheckDrowning(object data = null)
	{
		if (this.drowned)
		{
			return;
		}
		int cell = Grid.PosToCell(base.gameObject.transform.GetPosition());
		if (!this.IsCellSafe(cell))
		{
			if (!this.drowning)
			{
				this.drowning = true;
				base.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Drowning, false);
				base.Trigger(1949704522, null);
			}
			if (this.timeToDrown <= 0f && this.canDrownToDeath)
			{
				DeathMonitor.Instance smi = this.GetSMI<DeathMonitor.Instance>();
				if (smi != null)
				{
					smi.Kill(Db.Get().Deaths.Drowned);
				}
				base.Trigger(-750750377, null);
				this.drowned = true;
			}
		}
		else if (this.drowning)
		{
			this.drowning = false;
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.Drowning);
			base.Trigger(99949694, null);
		}
		if (this.livesUnderWater)
		{
			this.saturatedStatusGuid = this.selectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Saturated, this.saturatedStatusGuid, this.drowning, this);
		}
		else
		{
			this.drowningStatusGuid = this.selectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Drowning, this.drowningStatusGuid, this.drowning, this);
		}
		if (this.effects != null)
		{
			if (this.drowning)
			{
				if (this.livesUnderWater)
				{
					this.effects.Add(DrowningMonitor.saturatedEffect, false);
					return;
				}
				this.effects.Add(DrowningMonitor.drowningEffect, false);
				return;
			}
			else
			{
				if (this.livesUnderWater)
				{
					this.effects.Remove(DrowningMonitor.saturatedEffect);
					return;
				}
				this.effects.Remove(DrowningMonitor.drowningEffect);
			}
		}
	}

	// Token: 0x06005BD7 RID: 23511 RVA: 0x000E03C2 File Offset: 0x000DE5C2
	private static bool CellSafeTest(int testCell, object data)
	{
		return !Grid.IsNavigatableLiquid(testCell);
	}

	// Token: 0x06005BD8 RID: 23512 RVA: 0x000E03CD File Offset: 0x000DE5CD
	public bool IsCellSafe(int cell)
	{
		return this.occupyArea.TestArea(cell, this, DrowningMonitor.CellSafeTestDelegate);
	}

	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x06005BD9 RID: 23513 RVA: 0x000E03E1 File Offset: 0x000DE5E1
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Drowning
			};
		}
	}

	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x06005BDA RID: 23514 RVA: 0x000E03ED File Offset: 0x000DE5ED
	public string WiltStateString
	{
		get
		{
			if (this.livesUnderWater)
			{
				return "    • " + CREATURES.STATUSITEMS.SATURATED.NAME;
			}
			return "    • " + CREATURES.STATUSITEMS.DROWNING.NAME;
		}
	}

	// Token: 0x06005BDB RID: 23515 RVA: 0x000E0420 File Offset: 0x000DE620
	private void OnLiquidChanged(object data)
	{
		this.CheckDrowning(null);
	}

	// Token: 0x06005BDC RID: 23516 RVA: 0x002A7358 File Offset: 0x002A5558
	public void SlicedSim1000ms(float dt)
	{
		this.CheckDrowning(null);
		if (this.drowning)
		{
			if (!this.drowned)
			{
				this.timeToDrown -= dt;
				if (this.timeToDrown <= 0f)
				{
					this.CheckDrowning(null);
					return;
				}
			}
		}
		else
		{
			this.timeToDrown += dt * 5f;
			this.timeToDrown = Mathf.Clamp(this.timeToDrown, 0f, 75f);
		}
	}

	// Token: 0x0400414F RID: 16719
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04004150 RID: 16720
	[MyCmpGet]
	private Effects effects;

	// Token: 0x04004151 RID: 16721
	private OccupyArea _occupyArea;

	// Token: 0x04004152 RID: 16722
	[Serialize]
	[SerializeField]
	private float timeToDrown;

	// Token: 0x04004153 RID: 16723
	[Serialize]
	private bool drowned;

	// Token: 0x04004154 RID: 16724
	private bool drowning;

	// Token: 0x04004155 RID: 16725
	protected const float MaxDrownTime = 75f;

	// Token: 0x04004156 RID: 16726
	protected const float RegenRate = 5f;

	// Token: 0x04004157 RID: 16727
	protected const float CellLiquidThreshold = 0.95f;

	// Token: 0x04004158 RID: 16728
	public bool canDrownToDeath = true;

	// Token: 0x04004159 RID: 16729
	public bool livesUnderWater;

	// Token: 0x0400415A RID: 16730
	private Guid drowningStatusGuid;

	// Token: 0x0400415B RID: 16731
	private Guid saturatedStatusGuid;

	// Token: 0x0400415C RID: 16732
	private Extents extents;

	// Token: 0x0400415D RID: 16733
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400415E RID: 16734
	public static Effect drowningEffect;

	// Token: 0x0400415F RID: 16735
	public static Effect saturatedEffect;

	// Token: 0x04004160 RID: 16736
	private static readonly Func<int, object, bool> CellSafeTestDelegate = (int testCell, object data) => DrowningMonitor.CellSafeTest(testCell, data);
}
