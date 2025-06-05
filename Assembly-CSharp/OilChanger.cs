using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000F22 RID: 3874
public class OilChanger : GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>
{
	// Token: 0x06004DAC RID: 19884 RVA: 0x00274858 File Offset: 0x00272A58
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.inoperational;
		this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.UpdateStorageMeter));
		this.inoperational.PlayAnim("off").Enter(new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.LED_Off)).Enter(new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.UpdateStorageMeter)).TagTransition(GameTags.Operational, this.operational, false);
		this.operational.PlayAnim("on").Enter(new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.UpdateStorageMeter)).TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.operational.oilNeeded);
		this.operational.oilNeeded.Enter(new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.LED_Off)).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForMaterials, null).EventTransition(GameHashes.OnStorageChange, this.operational.ready, new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.Transition.ConditionCallback(OilChanger.HasEnoughLubricant));
		this.operational.ready.Enter(new StateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State.Callback(OilChanger.LED_On)).ToggleChore(new Func<OilChanger.Instance, Chore>(OilChanger.CreateChore), this.operational.oilNeeded);
	}

	// Token: 0x06004DAD RID: 19885 RVA: 0x000D6CC7 File Offset: 0x000D4EC7
	public static bool HasEnoughLubricant(OilChanger.Instance smi)
	{
		return smi.OilAmount >= smi.def.MIN_LUBRICANT_MASS_TO_WORK;
	}

	// Token: 0x06004DAE RID: 19886 RVA: 0x000D6CDF File Offset: 0x000D4EDF
	private static bool IsOperational(OilChanger.Instance smi)
	{
		return smi.IsOperational;
	}

	// Token: 0x06004DAF RID: 19887 RVA: 0x000D6CE7 File Offset: 0x000D4EE7
	public static void UpdateStorageMeter(OilChanger.Instance smi)
	{
		smi.UpdateStorageMeter();
	}

	// Token: 0x06004DB0 RID: 19888 RVA: 0x000D6CEF File Offset: 0x000D4EEF
	public static void LED_On(OilChanger.Instance smi)
	{
		smi.SetLEDState(true);
	}

	// Token: 0x06004DB1 RID: 19889 RVA: 0x000D6CF8 File Offset: 0x000D4EF8
	public static void LED_Off(OilChanger.Instance smi)
	{
		smi.SetLEDState(false);
	}

	// Token: 0x06004DB2 RID: 19890 RVA: 0x002749A4 File Offset: 0x00272BA4
	private static WorkChore<OilChangerWorkableUse> CreateChore(OilChanger.Instance smi)
	{
		return new WorkChore<OilChangerWorkableUse>(Db.Get().ChoreTypes.OilChange, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
	}

	// Token: 0x04003684 RID: 13956
	public const string STORAGE_METER_TARGET_NAME = "meter_target";

	// Token: 0x04003685 RID: 13957
	public const string STORAGE_METER_ANIM_NAME = "meter";

	// Token: 0x04003686 RID: 13958
	public const string LED_METER_TARGET_NAME = "light_target";

	// Token: 0x04003687 RID: 13959
	public const string LED_METER_ANIM_ON_NAME = "light_on";

	// Token: 0x04003688 RID: 13960
	public const string LED_METER_ANIM_OFF_NAME = "light_off";

	// Token: 0x04003689 RID: 13961
	public GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State inoperational;

	// Token: 0x0400368A RID: 13962
	public OilChanger.OperationalStates operational;

	// Token: 0x02000F23 RID: 3875
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400368B RID: 13963
		public float MIN_LUBRICANT_MASS_TO_WORK = 200f;
	}

	// Token: 0x02000F24 RID: 3876
	public class OperationalStates : GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State
	{
		// Token: 0x0400368C RID: 13964
		public GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State oilNeeded;

		// Token: 0x0400368D RID: 13965
		public GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.State ready;
	}

	// Token: 0x02000F25 RID: 3877
	public new class Instance : GameStateMachine<OilChanger, OilChanger.Instance, IStateMachineTarget, OilChanger.Def>.GameInstance, IFetchList
	{
		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06004DB6 RID: 19894 RVA: 0x000D6D24 File Offset: 0x000D4F24
		public bool IsOperational
		{
			get
			{
				return this.operational.IsOperational;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06004DB7 RID: 19895 RVA: 0x000D6D31 File Offset: 0x000D4F31
		public float OilAmount
		{
			get
			{
				return this.storage.GetMassAvailable(GameTags.LubricatingOil);
			}
		}

		// Token: 0x06004DB8 RID: 19896 RVA: 0x002749DC File Offset: 0x00272BDC
		public Instance(IStateMachineTarget master, OilChanger.Def def)
		{
			Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
			Tag lubricatingOil = GameTags.LubricatingOil;
			dictionary[lubricatingOil] = 0f;
			this.remainingLubricationMass = dictionary;
			base..ctor(master, def);
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			this.storage = base.GetComponent<Storage>();
			this.operational = base.GetComponent<Operational>();
			this.oilStorageMeter = new MeterController(component, "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
			this.readyLightMeter = new MeterController(component, "light_target", "light_off", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
		}

		// Token: 0x06004DB9 RID: 19897 RVA: 0x00274A6C File Offset: 0x00272C6C
		public void SetLEDState(bool isOn)
		{
			string s = isOn ? "light_on" : "light_off";
			this.readyLightMeter.meterController.Play(s, KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x06004DBA RID: 19898 RVA: 0x00274AAC File Offset: 0x00272CAC
		public void UpdateStorageMeter()
		{
			float positionPercent = this.OilAmount / this.storage.capacityKg;
			this.oilStorageMeter.SetPositionPercent(positionPercent);
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06004DBB RID: 19899 RVA: 0x000D6D43 File Offset: 0x000D4F43
		public Storage Destination
		{
			get
			{
				return this.storage;
			}
		}

		// Token: 0x06004DBC RID: 19900 RVA: 0x000D6D4B File Offset: 0x000D4F4B
		public float GetMinimumAmount(Tag tag)
		{
			return base.def.MIN_LUBRICANT_MASS_TO_WORK;
		}

		// Token: 0x06004DBD RID: 19901 RVA: 0x000D6D58 File Offset: 0x000D4F58
		public Dictionary<Tag, float> GetRemaining()
		{
			this.remainingLubricationMass[GameTags.LubricatingOil] = Mathf.Clamp(base.def.MIN_LUBRICANT_MASS_TO_WORK - this.OilAmount, 0f, base.def.MIN_LUBRICANT_MASS_TO_WORK);
			return this.remainingLubricationMass;
		}

		// Token: 0x06004DBE RID: 19902 RVA: 0x000AFECA File Offset: 0x000AE0CA
		public Dictionary<Tag, float> GetRemainingMinimum()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0400368E RID: 13966
		private Storage storage;

		// Token: 0x0400368F RID: 13967
		private Operational operational;

		// Token: 0x04003690 RID: 13968
		private MeterController oilStorageMeter;

		// Token: 0x04003691 RID: 13969
		private MeterController readyLightMeter;

		// Token: 0x04003692 RID: 13970
		private Dictionary<Tag, float> remainingLubricationMass;
	}
}
