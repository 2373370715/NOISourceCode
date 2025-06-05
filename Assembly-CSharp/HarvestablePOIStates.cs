using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200193F RID: 6463
[AddComponentMenu("KMonoBehaviour/scripts/HarvestablePOIStates")]
public class HarvestablePOIStates : GameStateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>
{
	// Token: 0x06008680 RID: 34432 RVA: 0x0035A1B8 File Offset: 0x003583B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.Enter(delegate(HarvestablePOIStates.Instance smi)
		{
			if (smi.configuration == null || smi.configuration.typeId == HashedString.Invalid)
			{
				smi.configuration = smi.GetComponent<HarvestablePOIConfigurator>().MakeConfiguration();
				smi.poiCapacity = UnityEngine.Random.Range(0f, smi.configuration.GetMaxCapacity());
			}
		});
		this.idle.ParamTransition<float>(this.poiCapacity, this.recharging, (HarvestablePOIStates.Instance smi, float f) => f < smi.configuration.GetMaxCapacity());
		this.recharging.EventHandler(GameHashes.NewDay, (HarvestablePOIStates.Instance smi) => GameClock.Instance, delegate(HarvestablePOIStates.Instance smi)
		{
			smi.RechargePOI(600f);
		}).ParamTransition<float>(this.poiCapacity, this.idle, (HarvestablePOIStates.Instance smi, float f) => f >= smi.configuration.GetMaxCapacity());
	}

	// Token: 0x040065FA RID: 26106
	public GameStateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>.State idle;

	// Token: 0x040065FB RID: 26107
	public GameStateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>.State recharging;

	// Token: 0x040065FC RID: 26108
	public StateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>.FloatParameter poiCapacity = new StateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>.FloatParameter(1f);

	// Token: 0x02001940 RID: 6464
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001941 RID: 6465
	public new class Instance : GameStateMachine<HarvestablePOIStates, HarvestablePOIStates.Instance, IStateMachineTarget, HarvestablePOIStates.Def>.GameInstance, IGameObjectEffectDescriptor
	{
		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06008683 RID: 34435 RVA: 0x000FCC9D File Offset: 0x000FAE9D
		// (set) Token: 0x06008684 RID: 34436 RVA: 0x000FCCA5 File Offset: 0x000FAEA5
		public float poiCapacity
		{
			get
			{
				return this._poiCapacity;
			}
			set
			{
				this._poiCapacity = value;
				base.smi.sm.poiCapacity.Set(value, base.smi, false);
			}
		}

		// Token: 0x06008685 RID: 34437 RVA: 0x000FCCCC File Offset: 0x000FAECC
		public Instance(IStateMachineTarget target, HarvestablePOIStates.Def def) : base(target, def)
		{
		}

		// Token: 0x06008686 RID: 34438 RVA: 0x0035A2B8 File Offset: 0x003584B8
		public void RechargePOI(float dt)
		{
			float num = dt / this.configuration.GetRechargeTime();
			float delta = this.configuration.GetMaxCapacity() * num;
			this.DeltaPOICapacity(delta);
		}

		// Token: 0x06008687 RID: 34439 RVA: 0x000FCCD6 File Offset: 0x000FAED6
		public void DeltaPOICapacity(float delta)
		{
			this.poiCapacity += delta;
			this.poiCapacity = Mathf.Min(this.configuration.GetMaxCapacity(), this.poiCapacity);
		}

		// Token: 0x06008688 RID: 34440 RVA: 0x000FCD02 File Offset: 0x000FAF02
		public bool POICanBeHarvested()
		{
			return this.poiCapacity > 0f;
		}

		// Token: 0x06008689 RID: 34441 RVA: 0x0035A2E8 File Offset: 0x003584E8
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			foreach (KeyValuePair<SimHashes, float> keyValuePair in this.configuration.GetElementsWithWeights())
			{
				SimHashes key = keyValuePair.Key;
				string arg = ElementLoader.FindElementByHash(key).tag.ProperName();
				list.Add(new Descriptor(string.Format(UI.SPACEDESTINATIONS.HARVESTABLE_POI.POI_PRODUCTION, arg), string.Format(UI.SPACEDESTINATIONS.HARVESTABLE_POI.POI_PRODUCTION_TOOLTIP, key.ToString()), Descriptor.DescriptorType.Effect, false));
			}
			list.Add(new Descriptor(string.Format("{0}/{1}", GameUtil.GetFormattedMass(this.poiCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedMass(this.configuration.GetMaxCapacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), "Capacity", Descriptor.DescriptorType.Effect, false));
			return list;
		}

		// Token: 0x040065FD RID: 26109
		[Serialize]
		public HarvestablePOIConfigurator.HarvestablePOIInstanceConfiguration configuration;

		// Token: 0x040065FE RID: 26110
		[Serialize]
		private float _poiCapacity;
	}
}
