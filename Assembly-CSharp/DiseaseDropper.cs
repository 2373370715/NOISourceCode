using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200119F RID: 4511
public class DiseaseDropper : GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>
{
	// Token: 0x06005BC2 RID: 23490 RVA: 0x002A6D1C File Offset: 0x002A4F1C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.stopped;
		this.root.EventHandler(GameHashes.BurstEmitDisease, delegate(DiseaseDropper.Instance smi)
		{
			smi.DropSingleEmit();
		});
		this.working.TagTransition(GameTags.PreventEmittingDisease, this.stopped, false).Update(delegate(DiseaseDropper.Instance smi, float dt)
		{
			smi.DropPeriodic(dt);
		}, UpdateRate.SIM_200ms, false);
		this.stopped.TagTransition(GameTags.PreventEmittingDisease, this.working, true);
	}

	// Token: 0x04004144 RID: 16708
	public GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.State working;

	// Token: 0x04004145 RID: 16709
	public GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.State stopped;

	// Token: 0x04004146 RID: 16710
	public StateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.Signal cellChangedSignal;

	// Token: 0x020011A0 RID: 4512
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06005BC4 RID: 23492 RVA: 0x002A6DB8 File Offset: 0x002A4FB8
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			if (this.singleEmitQuantity > 0)
			{
				list.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_BURST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.singleEmitQuantity, GameUtil.TimeSlice.None)), UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.DISEASE_DROPPER_BURST.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.singleEmitQuantity, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			}
			if (this.averageEmitPerSecond > 0)
			{
				list.Add(new Descriptor(UI.UISIDESCREENS.PLANTERSIDESCREEN.DISEASE_DROPPER_CONSTANT.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.averageEmitPerSecond, GameUtil.TimeSlice.PerSecond)), UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.DISEASE_DROPPER_CONSTANT.Replace("{Disease}", GameUtil.GetFormattedDiseaseName(this.diseaseIdx, false)).Replace("{DiseaseAmount}", GameUtil.GetFormattedDiseaseAmount(this.averageEmitPerSecond, GameUtil.TimeSlice.PerSecond)), Descriptor.DescriptorType.Effect, false));
			}
			return list;
		}

		// Token: 0x04004147 RID: 16711
		public byte diseaseIdx = byte.MaxValue;

		// Token: 0x04004148 RID: 16712
		public int singleEmitQuantity;

		// Token: 0x04004149 RID: 16713
		public int averageEmitPerSecond;

		// Token: 0x0400414A RID: 16714
		public float emitFrequency = 1f;
	}

	// Token: 0x020011A1 RID: 4513
	public new class Instance : GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.GameInstance
	{
		// Token: 0x06005BC6 RID: 23494 RVA: 0x000E0314 File Offset: 0x000DE514
		public Instance(IStateMachineTarget master, DiseaseDropper.Def def) : base(master, def)
		{
		}

		// Token: 0x06005BC7 RID: 23495 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public bool ShouldDropDisease()
		{
			return true;
		}

		// Token: 0x06005BC8 RID: 23496 RVA: 0x000E031E File Offset: 0x000DE51E
		public void DropSingleEmit()
		{
			this.DropDisease(base.def.diseaseIdx, base.def.singleEmitQuantity);
		}

		// Token: 0x06005BC9 RID: 23497 RVA: 0x002A6EBC File Offset: 0x002A50BC
		public void DropPeriodic(float dt)
		{
			this.timeSinceLastDrop += dt;
			if (base.def.averageEmitPerSecond > 0 && base.def.emitFrequency > 0f)
			{
				while (this.timeSinceLastDrop > base.def.emitFrequency)
				{
					this.DropDisease(base.def.diseaseIdx, (int)((float)base.def.averageEmitPerSecond * base.def.emitFrequency));
					this.timeSinceLastDrop -= base.def.emitFrequency;
				}
			}
		}

		// Token: 0x06005BCA RID: 23498 RVA: 0x002A6F50 File Offset: 0x002A5150
		public void DropDisease(byte disease_idx, int disease_count)
		{
			if (disease_count <= 0 || disease_idx == 255)
			{
				return;
			}
			int num = Grid.PosToCell(base.transform.GetPosition());
			if (!Grid.IsValidCell(num))
			{
				return;
			}
			SimMessages.ModifyDiseaseOnCell(num, disease_idx, disease_count);
		}

		// Token: 0x06005BCB RID: 23499 RVA: 0x002A6F8C File Offset: 0x002A518C
		public bool IsValidDropCell()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			return Grid.IsValidCell(num) && Grid.IsGas(num) && Grid.Mass[num] <= 1f;
		}

		// Token: 0x0400414B RID: 16715
		private float timeSinceLastDrop;
	}
}
