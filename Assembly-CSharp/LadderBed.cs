using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000E4E RID: 3662
public class LadderBed : GameStateMachine<LadderBed, LadderBed.Instance, IStateMachineTarget, LadderBed.Def>
{
	// Token: 0x0600479F RID: 18335 RVA: 0x000D2DC0 File Offset: 0x000D0FC0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
	}

	// Token: 0x0400322A RID: 12842
	public static string lightBedShakeSoundPath = GlobalAssets.GetSound("LadderBed_LightShake", false);

	// Token: 0x0400322B RID: 12843
	public static string noDupeBedShakeSoundPath = GlobalAssets.GetSound("LadderBed_Shake", false);

	// Token: 0x0400322C RID: 12844
	public static string LADDER_BED_COUNT_BELOW_PARAMETER = "bed_count";

	// Token: 0x02000E4F RID: 3663
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400322D RID: 12845
		public CellOffset[] offsets;
	}

	// Token: 0x02000E50 RID: 3664
	public new class Instance : GameStateMachine<LadderBed, LadderBed.Instance, IStateMachineTarget, LadderBed.Def>.GameInstance
	{
		// Token: 0x060047A3 RID: 18339 RVA: 0x002611A8 File Offset: 0x0025F3A8
		public Instance(IStateMachineTarget master, LadderBed.Def def) : base(master, def)
		{
			ScenePartitionerLayer scenePartitionerLayer = GameScenePartitioner.Instance.objectLayers[40];
			this.m_cell = Grid.PosToCell(master.gameObject);
			foreach (CellOffset offset in def.offsets)
			{
				int cell = Grid.OffsetCell(this.m_cell, offset);
				if (Grid.IsValidCell(this.m_cell) && Grid.IsValidCell(cell))
				{
					this.m_partitionEntires.Add(GameScenePartitioner.Instance.Add("LadderBed.Constructor", base.gameObject, cell, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnMoverChanged)));
					this.OnMoverChanged(null);
				}
			}
			AttachableBuilding attachable = this.m_attachable;
			attachable.onAttachmentNetworkChanged = (Action<object>)Delegate.Combine(attachable.onAttachmentNetworkChanged, new Action<object>(this.OnAttachmentChanged));
			this.OnAttachmentChanged(null);
			base.Subscribe(-717201811, new Action<object>(this.OnSleepDisturbedByMovement));
			master.GetComponent<KAnimControllerBase>().GetLayering().GetLink().syncTint = false;
		}

		// Token: 0x060047A4 RID: 18340 RVA: 0x002612C0 File Offset: 0x0025F4C0
		private void OnSleepDisturbedByMovement(object obj)
		{
			base.GetComponent<KAnimControllerBase>().Play("interrupt_light", KAnim.PlayMode.Once, 1f, 0f);
			EventInstance instance = SoundEvent.BeginOneShot(LadderBed.lightBedShakeSoundPath, base.smi.transform.GetPosition(), 1f, false);
			instance.setParameterByName(LadderBed.LADDER_BED_COUNT_BELOW_PARAMETER, (float)this.numBelow, false);
			SoundEvent.EndOneShot(instance);
		}

		// Token: 0x060047A5 RID: 18341 RVA: 0x000D2DFE File Offset: 0x000D0FFE
		private void OnAttachmentChanged(object data)
		{
			this.numBelow = AttachableBuilding.CountAttachedBelow(this.m_attachable);
		}

		// Token: 0x060047A6 RID: 18342 RVA: 0x0026132C File Offset: 0x0025F52C
		private void OnMoverChanged(object obj)
		{
			Pickupable pickupable = obj as Pickupable;
			if (pickupable != null && pickupable.gameObject != null && pickupable.KPrefabID.HasTag(GameTags.BaseMinion) && pickupable.GetComponent<Navigator>().CurrentNavType == NavType.Ladder)
			{
				if (this.m_sleepable.worker == null)
				{
					base.GetComponent<KAnimControllerBase>().Play("interrupt_light_nodupe", KAnim.PlayMode.Once, 1f, 0f);
					EventInstance instance = SoundEvent.BeginOneShot(LadderBed.noDupeBedShakeSoundPath, base.smi.transform.GetPosition(), 1f, false);
					instance.setParameterByName(LadderBed.LADDER_BED_COUNT_BELOW_PARAMETER, (float)this.numBelow, false);
					SoundEvent.EndOneShot(instance);
					return;
				}
				if (pickupable.gameObject != this.m_sleepable.worker.gameObject)
				{
					this.m_sleepable.worker.Trigger(-717201811, null);
				}
			}
		}

		// Token: 0x060047A7 RID: 18343 RVA: 0x00261428 File Offset: 0x0025F628
		protected override void OnCleanUp()
		{
			foreach (HandleVector<int>.Handle handle in this.m_partitionEntires)
			{
				GameScenePartitioner.Instance.Free(ref handle);
			}
			AttachableBuilding attachable = this.m_attachable;
			attachable.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(attachable.onAttachmentNetworkChanged, new Action<object>(this.OnAttachmentChanged));
			base.OnCleanUp();
		}

		// Token: 0x0400322E RID: 12846
		private List<HandleVector<int>.Handle> m_partitionEntires = new List<HandleVector<int>.Handle>();

		// Token: 0x0400322F RID: 12847
		private int m_cell;

		// Token: 0x04003230 RID: 12848
		[MyCmpGet]
		private Ownable m_ownable;

		// Token: 0x04003231 RID: 12849
		[MyCmpGet]
		private Sleepable m_sleepable;

		// Token: 0x04003232 RID: 12850
		[MyCmpGet]
		private AttachableBuilding m_attachable;

		// Token: 0x04003233 RID: 12851
		private int numBelow;
	}
}
