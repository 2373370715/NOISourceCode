using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200185A RID: 6234
public class Sauna : StateMachineComponent<Sauna.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06008084 RID: 32900 RVA: 0x000F92D8 File Offset: 0x000F74D8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06008085 RID: 32901 RVA: 0x000F92EB File Offset: 0x000F74EB
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06008086 RID: 32902 RVA: 0x002C93D8 File Offset: 0x002C75D8
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x06008087 RID: 32903 RVA: 0x003411F0 File Offset: 0x0033F3F0
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Element element = ElementLoader.FindElementByHash(SimHashes.Steam);
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, element.name, GameUtil.GetFormattedMass(this.steamPerUseKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, element.name, GameUtil.GetFormattedMass(this.steamPerUseKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement, false));
		Element element2 = ElementLoader.FindElementByHash(SimHashes.Water);
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTEDPERUSE, element2.name, GameUtil.GetFormattedMass(this.steamPerUseKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTEDPERUSE, element2.name, GameUtil.GetFormattedMass(this.steamPerUseKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Effect, false));
		list.Add(new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_TOOLTIP"), Descriptor.DescriptorType.Effect, false));
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		return list;
	}

	// Token: 0x040061C4 RID: 25028
	public string specificEffect;

	// Token: 0x040061C5 RID: 25029
	public string trackingEffect;

	// Token: 0x040061C6 RID: 25030
	public float steamPerUseKG;

	// Token: 0x040061C7 RID: 25031
	public float waterOutputTemp;

	// Token: 0x040061C8 RID: 25032
	public static readonly Operational.Flag sufficientSteam = new Operational.Flag("sufficientSteam", Operational.Flag.Type.Requirement);

	// Token: 0x0200185B RID: 6235
	public class States : GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna>
	{
		// Token: 0x0600808A RID: 32906 RVA: 0x0034135C File Offset: 0x0033F55C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false).ToggleMainStatusItem(Db.Get().BuildingStatusItems.MissingRequirements, null);
			this.operational.TagTransition(GameTags.Operational, this.inoperational, true).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GettingReady, null).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.Transition.ConditionCallback(this.IsReady));
			this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<Sauna.StatesInstance, Chore>(this.CreateChore), this.inoperational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null);
			this.ready.idle.WorkableStartTransition((Sauna.StatesInstance smi) => smi.master.GetComponent<SaunaWorkable>(), this.ready.working).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.Not(new StateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.Transition.ConditionCallback(this.IsReady)));
			this.ready.working.WorkableCompleteTransition((Sauna.StatesInstance smi) => smi.master.GetComponent<SaunaWorkable>(), this.ready.idle).WorkableStopTransition((Sauna.StatesInstance smi) => smi.master.GetComponent<SaunaWorkable>(), this.ready.idle);
		}

		// Token: 0x0600808B RID: 32907 RVA: 0x0034150C File Offset: 0x0033F70C
		private Chore CreateChore(Sauna.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<SaunaWorkable>();
			WorkChore<SaunaWorkable> workChore = new WorkChore<SaunaWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x0600808C RID: 32908 RVA: 0x0034156C File Offset: 0x0033F76C
		private bool IsReady(Sauna.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Steam);
			return primaryElement != null && primaryElement.Mass >= smi.master.steamPerUseKG;
		}

		// Token: 0x040061C9 RID: 25033
		private GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.State inoperational;

		// Token: 0x040061CA RID: 25034
		private GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.State operational;

		// Token: 0x040061CB RID: 25035
		private Sauna.States.ReadyStates ready;

		// Token: 0x0200185C RID: 6236
		public class ReadyStates : GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.State
		{
			// Token: 0x040061CC RID: 25036
			public GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.State idle;

			// Token: 0x040061CD RID: 25037
			public GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.State working;
		}
	}

	// Token: 0x0200185E RID: 6238
	public class StatesInstance : GameStateMachine<Sauna.States, Sauna.StatesInstance, Sauna, object>.GameInstance
	{
		// Token: 0x06008094 RID: 32916 RVA: 0x000F9336 File Offset: 0x000F7536
		public StatesInstance(Sauna smi) : base(smi)
		{
		}
	}
}
