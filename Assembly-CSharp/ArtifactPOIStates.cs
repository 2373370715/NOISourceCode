using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001909 RID: 6409
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactPOIStates")]
public class ArtifactPOIStates : GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>
{
	// Token: 0x060084A4 RID: 33956 RVA: 0x003531A4 File Offset: 0x003513A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.Enter(delegate(ArtifactPOIStates.Instance smi)
		{
			if (smi.configuration == null || smi.configuration.typeId == HashedString.Invalid)
			{
				smi.configuration = smi.GetComponent<ArtifactPOIConfigurator>().MakeConfiguration();
				smi.PickNewArtifactToHarvest();
				smi.poiCharge = 1f;
			}
		});
		this.idle.ParamTransition<float>(this.poiCharge, this.recharging, (ArtifactPOIStates.Instance smi, float f) => f < 1f);
		this.recharging.ParamTransition<float>(this.poiCharge, this.idle, (ArtifactPOIStates.Instance smi, float f) => f >= 1f).EventHandler(GameHashes.NewDay, (ArtifactPOIStates.Instance smi) => GameClock.Instance, delegate(ArtifactPOIStates.Instance smi)
		{
			smi.RechargePOI(600f);
		});
	}

	// Token: 0x04006503 RID: 25859
	public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State idle;

	// Token: 0x04006504 RID: 25860
	public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State recharging;

	// Token: 0x04006505 RID: 25861
	public StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.FloatParameter poiCharge = new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.FloatParameter(1f);

	// Token: 0x0200190A RID: 6410
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200190B RID: 6411
	public new class Instance : GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.GameInstance, IGameObjectEffectDescriptor
	{
		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x060084A7 RID: 33959 RVA: 0x000FBA40 File Offset: 0x000F9C40
		// (set) Token: 0x060084A8 RID: 33960 RVA: 0x000FBA48 File Offset: 0x000F9C48
		public float poiCharge
		{
			get
			{
				return this._poiCharge;
			}
			set
			{
				this._poiCharge = value;
				base.smi.sm.poiCharge.Set(value, base.smi, false);
			}
		}

		// Token: 0x060084A9 RID: 33961 RVA: 0x000FBA6F File Offset: 0x000F9C6F
		public Instance(IStateMachineTarget target, ArtifactPOIStates.Def def) : base(target, def)
		{
		}

		// Token: 0x060084AA RID: 33962 RVA: 0x003532A4 File Offset: 0x003514A4
		public void PickNewArtifactToHarvest()
		{
			if (this.numHarvests <= 0 && !string.IsNullOrEmpty(this.configuration.GetArtifactID()))
			{
				this.artifactToHarvest = this.configuration.GetArtifactID();
				ArtifactSelector.Instance.ReserveArtifactID(this.artifactToHarvest, ArtifactType.Any);
				return;
			}
			this.artifactToHarvest = ArtifactSelector.Instance.GetUniqueArtifactID(ArtifactType.Space);
		}

		// Token: 0x060084AB RID: 33963 RVA: 0x000FBA79 File Offset: 0x000F9C79
		public string GetArtifactToHarvest()
		{
			if (this.CanHarvestArtifact())
			{
				if (string.IsNullOrEmpty(this.artifactToHarvest))
				{
					this.PickNewArtifactToHarvest();
				}
				return this.artifactToHarvest;
			}
			return null;
		}

		// Token: 0x060084AC RID: 33964 RVA: 0x000FBA9E File Offset: 0x000F9C9E
		public void HarvestArtifact()
		{
			if (this.CanHarvestArtifact())
			{
				this.numHarvests++;
				this.poiCharge = 0f;
				this.artifactToHarvest = null;
				this.PickNewArtifactToHarvest();
			}
		}

		// Token: 0x060084AD RID: 33965 RVA: 0x00353300 File Offset: 0x00351500
		public void RechargePOI(float dt)
		{
			float delta = dt / this.configuration.GetRechargeTime();
			this.DeltaPOICharge(delta);
		}

		// Token: 0x060084AE RID: 33966 RVA: 0x000FBACE File Offset: 0x000F9CCE
		public float RechargeTimeRemaining()
		{
			return (float)Mathf.CeilToInt((this.configuration.GetRechargeTime() - this.configuration.GetRechargeTime() * this.poiCharge) / 600f) * 600f;
		}

		// Token: 0x060084AF RID: 33967 RVA: 0x000FBB00 File Offset: 0x000F9D00
		public void DeltaPOICharge(float delta)
		{
			this.poiCharge += delta;
			this.poiCharge = Mathf.Min(1f, this.poiCharge);
		}

		// Token: 0x060084B0 RID: 33968 RVA: 0x000FBB26 File Offset: 0x000F9D26
		public bool CanHarvestArtifact()
		{
			return this.poiCharge >= 1f;
		}

		// Token: 0x060084B1 RID: 33969 RVA: 0x000CE880 File Offset: 0x000CCA80
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>();
		}

		// Token: 0x04006506 RID: 25862
		[Serialize]
		public ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration configuration;

		// Token: 0x04006507 RID: 25863
		[Serialize]
		private float _poiCharge;

		// Token: 0x04006508 RID: 25864
		[Serialize]
		public string artifactToHarvest;

		// Token: 0x04006509 RID: 25865
		[Serialize]
		private int numHarvests;
	}
}
