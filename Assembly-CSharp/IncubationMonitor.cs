using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020011DE RID: 4574
public class IncubationMonitor : GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>
{
	// Token: 0x06005CFA RID: 23802 RVA: 0x002AAD00 File Offset: 0x002A8F00
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.incubating;
		this.root.Enter(delegate(IncubationMonitor.Instance smi)
		{
			smi.OnOperationalChanged(null);
		}).Enter(delegate(IncubationMonitor.Instance smi)
		{
			Components.IncubationMonitors.Add(smi);
		}).Exit(delegate(IncubationMonitor.Instance smi)
		{
			Components.IncubationMonitors.Remove(smi);
		});
		this.incubating.PlayAnim("idle", KAnim.PlayMode.Loop).Transition(this.hatching_pre, new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.Transition.ConditionCallback(IncubationMonitor.IsReadyToHatch), UpdateRate.SIM_1000ms).TagTransition(GameTags.Entombed, this.entombed, false).ParamTransition<bool>(this.isSuppressed, this.suppressed, GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.IsTrue).ToggleEffect((IncubationMonitor.Instance smi) => smi.incubatingEffect);
		this.entombed.TagTransition(GameTags.Entombed, this.incubating, true);
		this.suppressed.ToggleEffect((IncubationMonitor.Instance smi) => this.suppressedEffect).ParamTransition<bool>(this.isSuppressed, this.incubating, GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.IsFalse).TagTransition(GameTags.Entombed, this.entombed, false).Transition(this.not_viable, new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.Transition.ConditionCallback(IncubationMonitor.NoLongerViable), UpdateRate.SIM_1000ms);
		this.hatching_pre.Enter(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.DropSelfFromStorage)).PlayAnim("hatching_pre").OnAnimQueueComplete(this.hatching_pst);
		this.hatching_pst.Enter(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.SpawnBaby)).PlayAnim("hatching_pst").OnAnimQueueComplete(null).Exit(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.DeleteSelf));
		this.not_viable.Enter(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.SpawnGenericEgg)).GoTo(null).Exit(new StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State.Callback(IncubationMonitor.DeleteSelf));
		this.suppressedEffect = new Effect("IncubationSuppressed", CREATURES.MODIFIERS.INCUBATING_SUPPRESSED.NAME, CREATURES.MODIFIERS.INCUBATING_SUPPRESSED.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.suppressedEffect.Add(new AttributeModifier(Db.Get().Amounts.Viability.deltaAttribute.Id, -0.016666668f, CREATURES.MODIFIERS.INCUBATING_SUPPRESSED.NAME, false, false, true));
	}

	// Token: 0x06005CFB RID: 23803 RVA: 0x000E11D0 File Offset: 0x000DF3D0
	private static bool IsReadyToHatch(IncubationMonitor.Instance smi)
	{
		return !smi.gameObject.HasTag(GameTags.Entombed) && smi.incubation.value >= smi.incubation.GetMax();
	}

	// Token: 0x06005CFC RID: 23804 RVA: 0x002AAF7C File Offset: 0x002A917C
	private static void SpawnBaby(IncubationMonitor.Instance smi)
	{
		Vector3 position = smi.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(smi.def.spawnedCreature), position);
		gameObject.SetActive(true);
		gameObject.GetSMI<AnimInterruptMonitor.Instance>().Play("hatching_pst", KAnim.PlayMode.Once);
		KSelectable component = smi.gameObject.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
		}
		Db.Get().Amounts.Wildness.Copy(gameObject, smi.gameObject);
		if (smi.incubator != null)
		{
			smi.incubator.StoreBaby(gameObject);
		}
		IncubationMonitor.SpawnShell(smi);
		SaveLoader.Instance.saveManager.Unregister(smi.GetComponent<SaveLoadRoot>());
	}

	// Token: 0x06005CFD RID: 23805 RVA: 0x000E1201 File Offset: 0x000DF401
	private static bool NoLongerViable(IncubationMonitor.Instance smi)
	{
		return !smi.gameObject.HasTag(GameTags.Entombed) && smi.viability.value <= smi.viability.GetMin();
	}

	// Token: 0x06005CFE RID: 23806 RVA: 0x002AB074 File Offset: 0x002A9274
	private static GameObject SpawnShell(IncubationMonitor.Instance smi)
	{
		Vector3 position = smi.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("EggShell"), position);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		PrimaryElement component2 = smi.GetComponent<PrimaryElement>();
		component.Mass = component2.Mass * 0.5f;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06005CFF RID: 23807 RVA: 0x002AB0C8 File Offset: 0x002A92C8
	private static GameObject SpawnEggInnards(IncubationMonitor.Instance smi)
	{
		Vector3 position = smi.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("RawEgg"), position);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		PrimaryElement component2 = smi.GetComponent<PrimaryElement>();
		component.Mass = component2.Mass * 0.5f;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06005D00 RID: 23808 RVA: 0x002AB11C File Offset: 0x002A931C
	private static void SpawnGenericEgg(IncubationMonitor.Instance smi)
	{
		IncubationMonitor.SpawnShell(smi);
		GameObject gameObject = IncubationMonitor.SpawnEggInnards(smi);
		KSelectable component = smi.gameObject.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x06005D01 RID: 23809 RVA: 0x000E1232 File Offset: 0x000DF432
	private static void DeleteSelf(IncubationMonitor.Instance smi)
	{
		smi.gameObject.DeleteObject();
	}

	// Token: 0x06005D02 RID: 23810 RVA: 0x002AB188 File Offset: 0x002A9388
	private static void DropSelfFromStorage(IncubationMonitor.Instance smi)
	{
		if (!smi.sm.inIncubator.Get(smi))
		{
			Storage storage = smi.GetStorage();
			if (storage)
			{
				storage.Drop(smi.gameObject, true);
			}
			smi.gameObject.AddTag(GameTags.StoredPrivate);
		}
	}

	// Token: 0x0400423A RID: 16954
	public StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.BoolParameter incubatorIsActive;

	// Token: 0x0400423B RID: 16955
	public StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.BoolParameter inIncubator;

	// Token: 0x0400423C RID: 16956
	public StateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.BoolParameter isSuppressed;

	// Token: 0x0400423D RID: 16957
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State incubating;

	// Token: 0x0400423E RID: 16958
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State entombed;

	// Token: 0x0400423F RID: 16959
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State suppressed;

	// Token: 0x04004240 RID: 16960
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State hatching_pre;

	// Token: 0x04004241 RID: 16961
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State hatching_pst;

	// Token: 0x04004242 RID: 16962
	public GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.State not_viable;

	// Token: 0x04004243 RID: 16963
	private Effect suppressedEffect;

	// Token: 0x020011DF RID: 4575
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06005D05 RID: 23813 RVA: 0x002AB1D8 File Offset: 0x002A93D8
		public override void Configure(GameObject prefab)
		{
			List<string> initialAmounts = prefab.GetComponent<Modifiers>().initialAmounts;
			initialAmounts.Add(Db.Get().Amounts.Wildness.Id);
			initialAmounts.Add(Db.Get().Amounts.Incubation.Id);
			initialAmounts.Add(Db.Get().Amounts.Viability.Id);
		}

		// Token: 0x04004244 RID: 16964
		public float baseIncubationRate;

		// Token: 0x04004245 RID: 16965
		public Tag spawnedCreature;
	}

	// Token: 0x020011E0 RID: 4576
	public new class Instance : GameStateMachine<IncubationMonitor, IncubationMonitor.Instance, IStateMachineTarget, IncubationMonitor.Def>.GameInstance
	{
		// Token: 0x06005D07 RID: 23815 RVA: 0x002AB240 File Offset: 0x002A9440
		public Instance(IStateMachineTarget master, IncubationMonitor.Def def) : base(master, def)
		{
			this.incubation = Db.Get().Amounts.Incubation.Lookup(base.gameObject);
			Action<object> handler = new Action<object>(this.OnStore);
			master.Subscribe(856640610, handler);
			master.Subscribe(1309017699, handler);
			Action<object> handler2 = new Action<object>(this.OnOperationalChanged);
			master.Subscribe(1628751838, handler2);
			master.Subscribe(960378201, handler2);
			this.wildness = Db.Get().Amounts.Wildness.Lookup(base.gameObject);
			this.wildness.value = this.wildness.GetMax();
			this.viability = Db.Get().Amounts.Viability.Lookup(base.gameObject);
			this.viability.value = this.viability.GetMax();
			float value = def.baseIncubationRate;
			if (GenericGameSettings.instance.acceleratedLifecycle)
			{
				value = 33.333332f;
			}
			AttributeModifier modifier = new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, value, CREATURES.MODIFIERS.BASE_INCUBATION_RATE.NAME, false, false, true);
			this.incubatingEffect = new Effect("Incubating", CREATURES.MODIFIERS.INCUBATING.NAME, CREATURES.MODIFIERS.INCUBATING.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
			this.incubatingEffect.Add(modifier);
		}

		// Token: 0x06005D08 RID: 23816 RVA: 0x000E124F File Offset: 0x000DF44F
		public Storage GetStorage()
		{
			if (!(base.transform.parent != null))
			{
				return null;
			}
			return base.transform.parent.GetComponent<Storage>();
		}

		// Token: 0x06005D09 RID: 23817 RVA: 0x002AB3C0 File Offset: 0x002A95C0
		public void OnStore(object data)
		{
			Storage storage = data as Storage;
			bool stored = storage || (data != null && (bool)data);
			EggIncubator eggIncubator = storage ? storage.GetComponent<EggIncubator>() : null;
			this.UpdateIncubationState(stored, eggIncubator);
		}

		// Token: 0x06005D0A RID: 23818 RVA: 0x002AB408 File Offset: 0x002A9608
		public void OnOperationalChanged(object data = null)
		{
			bool stored = base.gameObject.HasTag(GameTags.Stored);
			Storage storage = this.GetStorage();
			EggIncubator eggIncubator = storage ? storage.GetComponent<EggIncubator>() : null;
			this.UpdateIncubationState(stored, eggIncubator);
		}

		// Token: 0x06005D0B RID: 23819 RVA: 0x002AB448 File Offset: 0x002A9648
		private void UpdateIncubationState(bool stored, EggIncubator incubator)
		{
			this.incubator = incubator;
			base.smi.sm.inIncubator.Set(incubator != null, base.smi, false);
			bool value = stored && !incubator;
			base.smi.sm.isSuppressed.Set(value, base.smi, false);
			Operational operational = incubator ? incubator.GetComponent<Operational>() : null;
			bool value2 = incubator && (operational == null || operational.IsOperational);
			base.smi.sm.incubatorIsActive.Set(value2, base.smi, false);
		}

		// Token: 0x06005D0C RID: 23820 RVA: 0x000E1276 File Offset: 0x000DF476
		public void ApplySongBuff()
		{
			base.GetComponent<Effects>().Add("EggSong", true);
		}

		// Token: 0x06005D0D RID: 23821 RVA: 0x000E128A File Offset: 0x000DF48A
		public bool HasSongBuff()
		{
			return base.GetComponent<Effects>().HasEffect("EggSong");
		}

		// Token: 0x04004246 RID: 16966
		public AmountInstance incubation;

		// Token: 0x04004247 RID: 16967
		public AmountInstance wildness;

		// Token: 0x04004248 RID: 16968
		public AmountInstance viability;

		// Token: 0x04004249 RID: 16969
		public EggIncubator incubator;

		// Token: 0x0400424A RID: 16970
		public Effect incubatingEffect;
	}
}
