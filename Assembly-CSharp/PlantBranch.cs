using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001731 RID: 5937
public class PlantBranch : GameStateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>
{
	// Token: 0x06007A13 RID: 31251 RVA: 0x000F4CFB File Offset: 0x000F2EFB
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x04005BE5 RID: 23525
	private StateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>.TargetParameter Trunk;

	// Token: 0x02001732 RID: 5938
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005BE6 RID: 23526
		public Action<PlantBranchGrower.Instance, PlantBranch.Instance> animationSetupCallback;

		// Token: 0x04005BE7 RID: 23527
		public Action<PlantBranch.Instance> onEarlySpawn;
	}

	// Token: 0x02001733 RID: 5939
	public new class Instance : GameStateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>.GameInstance, IWiltCause
	{
		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06007A16 RID: 31254 RVA: 0x000F4D14 File Offset: 0x000F2F14
		public bool HasTrunk
		{
			get
			{
				return this.trunk != null && !this.trunk.IsNullOrDestroyed() && !this.trunk.isMasterNull;
			}
		}

		// Token: 0x06007A17 RID: 31255 RVA: 0x000F4D3B File Offset: 0x000F2F3B
		public Instance(IStateMachineTarget master, PlantBranch.Def def) : base(master, def)
		{
			this.SetOccupyGridSpace(true);
			base.Subscribe(1272413801, new Action<object>(this.OnHarvest));
		}

		// Token: 0x06007A18 RID: 31256 RVA: 0x00325008 File Offset: 0x00323208
		public override void StartSM()
		{
			base.StartSM();
			Action<PlantBranch.Instance> onEarlySpawn = base.def.onEarlySpawn;
			if (onEarlySpawn != null)
			{
				onEarlySpawn(this);
			}
			this.trunk = this.GetTrunk();
			if (!this.HasTrunk)
			{
				global::Debug.LogWarning("Tree Branch loaded with missing trunk reference. Destroying...");
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			this.SubscribeToTrunk();
			Action<PlantBranchGrower.Instance, PlantBranch.Instance> animationSetupCallback = base.def.animationSetupCallback;
			if (animationSetupCallback == null)
			{
				return;
			}
			animationSetupCallback(this.trunk, this);
		}

		// Token: 0x06007A19 RID: 31257 RVA: 0x000F4D71 File Offset: 0x000F2F71
		private void OnHarvest(object data)
		{
			if (this.HasTrunk)
			{
				this.trunk.OnBrancHarvested(this);
			}
		}

		// Token: 0x06007A1A RID: 31258 RVA: 0x000F4D87 File Offset: 0x000F2F87
		protected override void OnCleanUp()
		{
			this.UnsubscribeToTrunk();
			this.SetOccupyGridSpace(false);
			base.OnCleanUp();
		}

		// Token: 0x06007A1B RID: 31259 RVA: 0x00325080 File Offset: 0x00323280
		private void SetOccupyGridSpace(bool active)
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (!active)
			{
				if (Grid.Objects[cell, 5] == base.gameObject)
				{
					Grid.Objects[cell, 5] = null;
				}
				return;
			}
			GameObject gameObject = Grid.Objects[cell, 5];
			if (gameObject != null && gameObject != base.gameObject)
			{
				global::Debug.LogWarningFormat(base.gameObject, "PlantBranch.SetOccupyGridSpace already occupied by {0}", new object[]
				{
					gameObject
				});
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			Grid.Objects[cell, 5] = base.gameObject;
		}

		// Token: 0x06007A1C RID: 31260 RVA: 0x00325120 File Offset: 0x00323320
		public void SetTrunk(PlantBranchGrower.Instance trunk)
		{
			this.trunk = trunk;
			base.smi.sm.Trunk.Set(trunk.gameObject, this, false);
			this.SubscribeToTrunk();
			Action<PlantBranchGrower.Instance, PlantBranch.Instance> animationSetupCallback = base.def.animationSetupCallback;
			if (animationSetupCallback == null)
			{
				return;
			}
			animationSetupCallback(trunk, this);
		}

		// Token: 0x06007A1D RID: 31261 RVA: 0x000F4D9C File Offset: 0x000F2F9C
		public PlantBranchGrower.Instance GetTrunk()
		{
			if (base.smi.sm.Trunk.IsNull(this))
			{
				return null;
			}
			return base.sm.Trunk.Get(this).GetSMI<PlantBranchGrower.Instance>();
		}

		// Token: 0x06007A1E RID: 31262 RVA: 0x00325170 File Offset: 0x00323370
		private void SubscribeToTrunk()
		{
			if (!this.HasTrunk)
			{
				return;
			}
			if (this.trunkWiltHandle == -1)
			{
				this.trunkWiltHandle = this.trunk.gameObject.Subscribe(-724860998, new Action<object>(this.OnTrunkWilt));
			}
			if (this.trunkWiltRecoverHandle == -1)
			{
				this.trunkWiltRecoverHandle = this.trunk.gameObject.Subscribe(712767498, new Action<object>(this.OnTrunkRecover));
			}
			base.Trigger(912965142, !this.trunk.GetComponent<WiltCondition>().IsWilting());
			ReceptacleMonitor component = base.GetComponent<ReceptacleMonitor>();
			PlantablePlot receptacle = this.trunk.GetComponent<ReceptacleMonitor>().GetReceptacle();
			component.SetReceptacle(receptacle);
			this.trunk.RefreshBranchZPositionOffset(base.gameObject);
			base.GetComponent<BudUprootedMonitor>().SetParentObject(this.trunk.GetComponent<KPrefabID>());
		}

		// Token: 0x06007A1F RID: 31263 RVA: 0x00325250 File Offset: 0x00323450
		private void UnsubscribeToTrunk()
		{
			if (!this.HasTrunk)
			{
				return;
			}
			this.trunk.gameObject.Unsubscribe(this.trunkWiltHandle);
			this.trunk.gameObject.Unsubscribe(this.trunkWiltRecoverHandle);
			this.trunkWiltHandle = -1;
			this.trunkWiltRecoverHandle = -1;
			this.trunk.OnBranchRemoved(base.gameObject);
		}

		// Token: 0x06007A20 RID: 31264 RVA: 0x000F4DCE File Offset: 0x000F2FCE
		private void OnTrunkWilt(object data = null)
		{
			base.Trigger(912965142, false);
		}

		// Token: 0x06007A21 RID: 31265 RVA: 0x000F4DE1 File Offset: 0x000F2FE1
		private void OnTrunkRecover(object data = null)
		{
			base.Trigger(912965142, true);
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06007A22 RID: 31266 RVA: 0x000F4DF4 File Offset: 0x000F2FF4
		public string WiltStateString
		{
			get
			{
				return "    • " + DUPLICANTS.STATS.TRUNKHEALTH.NAME;
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06007A23 RID: 31267 RVA: 0x000F4E0A File Offset: 0x000F300A
		public WiltCondition.Condition[] Conditions
		{
			get
			{
				return new WiltCondition.Condition[]
				{
					WiltCondition.Condition.UnhealthyRoot
				};
			}
		}

		// Token: 0x04005BE8 RID: 23528
		public PlantBranchGrower.Instance trunk;

		// Token: 0x04005BE9 RID: 23529
		private int trunkWiltHandle = -1;

		// Token: 0x04005BEA RID: 23530
		private int trunkWiltRecoverHandle = -1;
	}
}
