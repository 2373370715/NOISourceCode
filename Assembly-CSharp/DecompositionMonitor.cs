using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200159D RID: 5533
public class DecompositionMonitor : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance>
{
	// Token: 0x0600730A RID: 29450 RVA: 0x0030E32C File Offset: 0x0030C52C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.Update("UpdateDecomposition", delegate(DecompositionMonitor.Instance smi, float dt)
		{
			smi.UpdateDecomposition(dt);
		}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.decomposition, this.rotten, GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.IsGTEOne).ToggleAttributeModifier("Dead", (DecompositionMonitor.Instance smi) => smi.satisfiedDecorModifier, null).ToggleAttributeModifier("Dead", (DecompositionMonitor.Instance smi) => smi.satisfiedDecorRadiusModifier, null);
		this.rotten.DefaultState(this.rotten.exposed).ToggleStatusItem(Db.Get().DuplicantStatusItems.Rotten, null).ToggleAttributeModifier("Rotten", (DecompositionMonitor.Instance smi) => smi.rottenDecorModifier, null).ToggleAttributeModifier("Rotten", (DecompositionMonitor.Instance smi) => smi.rottenDecorRadiusModifier, null);
		this.rotten.exposed.DefaultState(this.rotten.exposed.openair).EventTransition(GameHashes.OnStore, this.rotten.stored, (DecompositionMonitor.Instance smi) => !smi.IsExposed());
		this.rotten.exposed.openair.Enter(delegate(DecompositionMonitor.Instance smi)
		{
			if (smi.spawnsRotMonsters)
			{
				smi.ScheduleGoTo(UnityEngine.Random.Range(150f, 300f), this.rotten.spawningmonster);
			}
		}).Transition(this.rotten.exposed.submerged, (DecompositionMonitor.Instance smi) => smi.IsSubmerged(), UpdateRate.SIM_200ms).ToggleFX((DecompositionMonitor.Instance smi) => this.CreateFX(smi));
		this.rotten.exposed.submerged.DefaultState(this.rotten.exposed.submerged.idle).Transition(this.rotten.exposed.openair, (DecompositionMonitor.Instance smi) => !smi.IsSubmerged(), UpdateRate.SIM_200ms);
		this.rotten.exposed.submerged.idle.ScheduleGoTo(0.25f, this.rotten.exposed.submerged.dirtywater);
		this.rotten.exposed.submerged.dirtywater.Enter("DirtyWater", delegate(DecompositionMonitor.Instance smi)
		{
			smi.DirtyWater(smi.dirtyWaterMaxRange);
		}).GoTo(this.rotten.exposed.submerged.idle);
		this.rotten.spawningmonster.Enter(delegate(DecompositionMonitor.Instance smi)
		{
			if (this.remainingRotMonsters > 0)
			{
				this.remainingRotMonsters--;
				GameUtil.KInstantiate(Assets.GetPrefab(new Tag("Glom")), smi.transform.GetPosition(), Grid.SceneLayer.Creatures, null, 0).SetActive(true);
			}
			smi.GoTo(this.rotten.exposed);
		});
		this.rotten.stored.EventTransition(GameHashes.OnStore, this.rotten.exposed, (DecompositionMonitor.Instance smi) => smi.IsExposed());
	}

	// Token: 0x0600730B RID: 29451 RVA: 0x000EFD5C File Offset: 0x000EDF5C
	private FliesFX.Instance CreateFX(DecompositionMonitor.Instance smi)
	{
		if (!smi.isMasterNull)
		{
			return new FliesFX.Instance(smi.master, new Vector3(0f, 0f, -0.1f));
		}
		return null;
	}

	// Token: 0x0400563E RID: 22078
	public StateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.FloatParameter decomposition;

	// Token: 0x0400563F RID: 22079
	[SerializeField]
	public int remainingRotMonsters = 3;

	// Token: 0x04005640 RID: 22080
	public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04005641 RID: 22081
	public DecompositionMonitor.RottenState rotten;

	// Token: 0x0200159E RID: 5534
	public class SubmergedState : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04005642 RID: 22082
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04005643 RID: 22083
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State dirtywater;
	}

	// Token: 0x0200159F RID: 5535
	public class ExposedState : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04005644 RID: 22084
		public DecompositionMonitor.SubmergedState submerged;

		// Token: 0x04005645 RID: 22085
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State openair;
	}

	// Token: 0x020015A0 RID: 5536
	public class RottenState : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04005646 RID: 22086
		public DecompositionMonitor.ExposedState exposed;

		// Token: 0x04005647 RID: 22087
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State stored;

		// Token: 0x04005648 RID: 22088
		public GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.State spawningmonster;
	}

	// Token: 0x020015A1 RID: 5537
	public new class Instance : GameStateMachine<DecompositionMonitor, DecompositionMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007313 RID: 29459 RVA: 0x0030E6CC File Offset: 0x0030C8CC
		public Instance(IStateMachineTarget master, Disease disease, float decompositionRate = 0.00083333335f, bool spawnRotMonsters = true) : base(master)
		{
			base.gameObject.AddComponent<DecorProvider>();
			this.decompositionRate = decompositionRate;
			this.disease = disease;
			this.spawnsRotMonsters = spawnRotMonsters;
		}

		// Token: 0x06007314 RID: 29460 RVA: 0x0030E7D4 File Offset: 0x0030C9D4
		public void UpdateDecomposition(float dt)
		{
			float delta_value = dt * this.decompositionRate;
			base.sm.decomposition.Delta(delta_value, base.smi);
		}

		// Token: 0x06007315 RID: 29461 RVA: 0x0030E804 File Offset: 0x0030CA04
		public bool IsExposed()
		{
			KPrefabID component = base.smi.GetComponent<KPrefabID>();
			return component == null || !component.HasTag(GameTags.Preserved);
		}

		// Token: 0x06007316 RID: 29462 RVA: 0x000EFDD1 File Offset: 0x000EDFD1
		public bool IsRotten()
		{
			return base.IsInsideState(base.sm.rotten);
		}

		// Token: 0x06007317 RID: 29463 RVA: 0x000EFDE4 File Offset: 0x000EDFE4
		public bool IsSubmerged()
		{
			return PathFinder.IsSubmerged(Grid.PosToCell(base.master.transform.GetPosition()));
		}

		// Token: 0x06007318 RID: 29464 RVA: 0x0030E838 File Offset: 0x0030CA38
		public void DirtyWater(int maxCellRange = 3)
		{
			int num = Grid.PosToCell(base.master.transform.GetPosition());
			if (Grid.Element[num].id == SimHashes.Water)
			{
				SimMessages.ReplaceElement(num, SimHashes.DirtyWater, CellEventLogger.Instance.DecompositionDirtyWater, Grid.Mass[num], Grid.Temperature[num], Grid.DiseaseIdx[num], Grid.DiseaseCount[num], -1);
				return;
			}
			if (Grid.Element[num].id == SimHashes.DirtyWater)
			{
				int[] array = new int[4];
				for (int i = 0; i < maxCellRange; i++)
				{
					for (int j = 0; j < maxCellRange; j++)
					{
						array[0] = Grid.OffsetCell(num, new CellOffset(-i, j));
						array[1] = Grid.OffsetCell(num, new CellOffset(i, j));
						array[2] = Grid.OffsetCell(num, new CellOffset(-i, -j));
						array[3] = Grid.OffsetCell(num, new CellOffset(i, -j));
						array.Shuffle<int>();
						foreach (int num2 in array)
						{
							if (Grid.GetCellDistance(num, num2) < maxCellRange - 1 && Grid.IsValidCell(num2) && Grid.Element[num2].id == SimHashes.Water)
							{
								SimMessages.ReplaceElement(num2, SimHashes.DirtyWater, CellEventLogger.Instance.DecompositionDirtyWater, Grid.Mass[num2], Grid.Temperature[num2], Grid.DiseaseIdx[num2], Grid.DiseaseCount[num2], -1);
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x04005649 RID: 22089
		public float decompositionRate;

		// Token: 0x0400564A RID: 22090
		public Disease disease;

		// Token: 0x0400564B RID: 22091
		public int dirtyWaterMaxRange = 3;

		// Token: 0x0400564C RID: 22092
		public bool spawnsRotMonsters = true;

		// Token: 0x0400564D RID: 22093
		public AttributeModifier satisfiedDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -65f, DUPLICANTS.MODIFIERS.DEAD.NAME, false, false, true);

		// Token: 0x0400564E RID: 22094
		public AttributeModifier satisfiedDecorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 4f, DUPLICANTS.MODIFIERS.DEAD.NAME, false, false, true);

		// Token: 0x0400564F RID: 22095
		public AttributeModifier rottenDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, -100f, DUPLICANTS.MODIFIERS.ROTTING.NAME, false, false, true);

		// Token: 0x04005650 RID: 22096
		public AttributeModifier rottenDecorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 4f, DUPLICANTS.MODIFIERS.ROTTING.NAME, false, false, true);
	}
}
