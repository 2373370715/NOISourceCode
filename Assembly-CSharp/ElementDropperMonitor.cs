using System;
using UnityEngine;

// Token: 0x020011AD RID: 4525
public class ElementDropperMonitor : GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>
{
	// Token: 0x06005BFC RID: 23548 RVA: 0x002A79F0 File Offset: 0x002A5BF0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.EventHandler(GameHashes.DeathAnimComplete, delegate(ElementDropperMonitor.Instance smi)
		{
			smi.DropDeathElement();
		});
		this.satisfied.OnSignal(this.cellChangedSignal, this.readytodrop, (ElementDropperMonitor.Instance smi) => smi.ShouldDropElement());
		this.readytodrop.ToggleBehaviour(GameTags.Creatures.WantsToDropElements, (ElementDropperMonitor.Instance smi) => true, delegate(ElementDropperMonitor.Instance smi)
		{
			smi.GoTo(this.satisfied);
		}).EventHandler(GameHashes.ObjectMovementStateChanged, delegate(ElementDropperMonitor.Instance smi, object d)
		{
			if ((GameHashes)d == GameHashes.ObjectMovementWakeUp)
			{
				smi.GoTo(this.satisfied);
			}
		});
	}

	// Token: 0x0400417B RID: 16763
	public GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.State satisfied;

	// Token: 0x0400417C RID: 16764
	public GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.State readytodrop;

	// Token: 0x0400417D RID: 16765
	public StateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.Signal cellChangedSignal;

	// Token: 0x020011AE RID: 4526
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400417E RID: 16766
		public SimHashes dirtyEmitElement;

		// Token: 0x0400417F RID: 16767
		public float dirtyProbabilityPercent;

		// Token: 0x04004180 RID: 16768
		public float dirtyCellToTargetMass;

		// Token: 0x04004181 RID: 16769
		public float dirtyMassPerDirty;

		// Token: 0x04004182 RID: 16770
		public float dirtyMassReleaseOnDeath;

		// Token: 0x04004183 RID: 16771
		public byte emitDiseaseIdx = byte.MaxValue;

		// Token: 0x04004184 RID: 16772
		public float emitDiseasePerKg;
	}

	// Token: 0x020011AF RID: 4527
	public new class Instance : GameStateMachine<ElementDropperMonitor, ElementDropperMonitor.Instance, IStateMachineTarget, ElementDropperMonitor.Def>.GameInstance
	{
		// Token: 0x06005C01 RID: 23553 RVA: 0x000E05FB File Offset: 0x000DE7FB
		public Instance(IStateMachineTarget master, ElementDropperMonitor.Def def) : base(master, def)
		{
			Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "ElementDropperMonitor.Instance");
		}

		// Token: 0x06005C02 RID: 23554 RVA: 0x000E0627 File Offset: 0x000DE827
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		}

		// Token: 0x06005C03 RID: 23555 RVA: 0x000E064C File Offset: 0x000DE84C
		private void OnCellChange()
		{
			base.sm.cellChangedSignal.Trigger(this);
		}

		// Token: 0x06005C04 RID: 23556 RVA: 0x000E065F File Offset: 0x000DE85F
		public bool ShouldDropElement()
		{
			return this.IsValidDropCell() && UnityEngine.Random.Range(0f, 100f) < base.def.dirtyProbabilityPercent;
		}

		// Token: 0x06005C05 RID: 23557 RVA: 0x002A7AC0 File Offset: 0x002A5CC0
		public void DropDeathElement()
		{
			this.DropElement(base.def.dirtyMassReleaseOnDeath, base.def.dirtyEmitElement, base.def.emitDiseaseIdx, Mathf.RoundToInt(base.def.dirtyMassReleaseOnDeath * base.def.dirtyMassPerDirty));
		}

		// Token: 0x06005C06 RID: 23558 RVA: 0x002A7B10 File Offset: 0x002A5D10
		public void DropPeriodicElement()
		{
			this.DropElement(base.def.dirtyMassPerDirty, base.def.dirtyEmitElement, base.def.emitDiseaseIdx, Mathf.RoundToInt(base.def.emitDiseasePerKg * base.def.dirtyMassPerDirty));
		}

		// Token: 0x06005C07 RID: 23559 RVA: 0x002A7B60 File Offset: 0x002A5D60
		public void DropElement(float mass, SimHashes element_id, byte disease_idx, int disease_count)
		{
			if (mass <= 0f)
			{
				return;
			}
			Element element = ElementLoader.FindElementByHash(element_id);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			if (element.IsGas || element.IsLiquid)
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), element_id, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature, disease_idx, disease_count, true, -1);
			}
			else if (element.IsSolid)
			{
				element.substance.SpawnResource(base.transform.GetPosition() + new Vector3(0f, 0.5f, 0f), mass, temperature, disease_idx, disease_count, false, true, false);
			}
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, element.name, base.gameObject.transform, 1.5f, false);
		}

		// Token: 0x06005C08 RID: 23560 RVA: 0x002A6F8C File Offset: 0x002A518C
		public bool IsValidDropCell()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			return Grid.IsValidCell(num) && Grid.IsGas(num) && Grid.Mass[num] <= 1f;
		}
	}
}
