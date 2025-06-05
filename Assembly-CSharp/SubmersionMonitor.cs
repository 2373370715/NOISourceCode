using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200121E RID: 4638
[AddComponentMenu("KMonoBehaviour/scripts/SubmersionMonitor")]
public class SubmersionMonitor : KMonoBehaviour, IGameObjectEffectDescriptor, IWiltCause, ISim1000ms
{
	// Token: 0x17000592 RID: 1426
	// (get) Token: 0x06005E01 RID: 24065 RVA: 0x000E1C67 File Offset: 0x000DFE67
	public bool Dry
	{
		get
		{
			return this.dry;
		}
	}

	// Token: 0x06005E02 RID: 24066 RVA: 0x000E1C6F File Offset: 0x000DFE6F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnMove();
		this.CheckDry();
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnMove), "SubmersionMonitor.OnSpawn");
	}

	// Token: 0x06005E03 RID: 24067 RVA: 0x002AE8F4 File Offset: 0x002ACAF4
	private void OnMove()
	{
		this.position = Grid.PosToCell(base.gameObject);
		if (this.partitionerEntry.IsValid())
		{
			GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, this.position);
		}
		else
		{
			Vector2I vector2I = Grid.PosToXY(base.transform.GetPosition());
			Extents extents = new Extents(vector2I.x, vector2I.y, 1, 2);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("DrowningMonitor.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
		}
		this.CheckDry();
	}

	// Token: 0x06005E04 RID: 24068 RVA: 0x000AA038 File Offset: 0x000A8238
	private void OnDrawGizmosSelected()
	{
	}

	// Token: 0x06005E05 RID: 24069 RVA: 0x000E1CA5 File Offset: 0x000DFEA5
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnMove));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06005E06 RID: 24070 RVA: 0x000E1CD9 File Offset: 0x000DFED9
	public void Configure(float _maxStamina, float _staminaRegenRate, float _cellLiquidThreshold = 0.95f)
	{
		this.cellLiquidThreshold = _cellLiquidThreshold;
	}

	// Token: 0x06005E07 RID: 24071 RVA: 0x000E1CE2 File Offset: 0x000DFEE2
	public void Sim1000ms(float dt)
	{
		this.CheckDry();
	}

	// Token: 0x06005E08 RID: 24072 RVA: 0x002AE998 File Offset: 0x002ACB98
	private void CheckDry()
	{
		if (!this.IsCellSafe())
		{
			if (!this.dry)
			{
				this.dry = true;
				base.Trigger(-2057657673, null);
				return;
			}
		}
		else if (this.dry)
		{
			this.dry = false;
			base.Trigger(1555379996, null);
		}
	}

	// Token: 0x06005E09 RID: 24073 RVA: 0x002AE9E4 File Offset: 0x002ACBE4
	public bool IsCellSafe()
	{
		int cell = Grid.PosToCell(base.gameObject);
		return Grid.IsValidCell(cell) && Grid.IsSubstantialLiquid(cell, this.cellLiquidThreshold);
	}

	// Token: 0x06005E0A RID: 24074 RVA: 0x000E1CE2 File Offset: 0x000DFEE2
	private void OnLiquidChanged(object data)
	{
		this.CheckDry();
	}

	// Token: 0x17000593 RID: 1427
	// (get) Token: 0x06005E0B RID: 24075 RVA: 0x000E1CEA File Offset: 0x000DFEEA
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.DryingOut
			};
		}
	}

	// Token: 0x17000594 RID: 1428
	// (get) Token: 0x06005E0C RID: 24076 RVA: 0x000E1CF6 File Offset: 0x000DFEF6
	public string WiltStateString
	{
		get
		{
			if (this.Dry)
			{
				return Db.Get().CreatureStatusItems.DryingOut.resolveStringCallback(CREATURES.STATUSITEMS.DRYINGOUT.NAME, this);
			}
			return "";
		}
	}

	// Token: 0x06005E0D RID: 24077 RVA: 0x000AA038 File Offset: 0x000A8238
	public void SetIncapacitated(bool state)
	{
	}

	// Token: 0x06005E0E RID: 24078 RVA: 0x000E1D2A File Offset: 0x000DFF2A
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_SUBMERSION, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_SUBMERSION, Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x04004318 RID: 17176
	private int position;

	// Token: 0x04004319 RID: 17177
	private bool dry;

	// Token: 0x0400431A RID: 17178
	protected float cellLiquidThreshold = 0.2f;

	// Token: 0x0400431B RID: 17179
	private Extents extents;

	// Token: 0x0400431C RID: 17180
	private HandleVector<int>.Handle partitionerEntry;
}
