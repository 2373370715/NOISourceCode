using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001836 RID: 6198
public class RocketUsageRestriction : GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>
{
	// Token: 0x06007F63 RID: 32611 RVA: 0x0033C1AC File Offset: 0x0033A3AC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(delegate(RocketUsageRestriction.StatesInstance smi)
		{
			if (DlcManager.FeatureClusterSpaceEnabled() && smi.master.gameObject.GetMyWorld().IsModuleInterior)
			{
				smi.Subscribe(493375141, new Action<object>(smi.OnRefreshUserMenu));
				smi.GoToRestrictionState();
				return;
			}
			smi.StopSM("Not inside rocket or no cluster space");
		});
		this.restriction.Enter(new StateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State.Callback(this.AquireRocketControlStation)).Enter(delegate(RocketUsageRestriction.StatesInstance smi)
		{
			Components.RocketControlStations.OnAdd += new Action<RocketControlStation>(smi.ControlStationBuilt);
		}).Exit(delegate(RocketUsageRestriction.StatesInstance smi)
		{
			Components.RocketControlStations.OnAdd -= new Action<RocketControlStation>(smi.ControlStationBuilt);
		});
		this.restriction.uncontrolled.ToggleStatusItem(Db.Get().BuildingStatusItems.NoRocketRestriction, null).Enter(delegate(RocketUsageRestriction.StatesInstance smi)
		{
			this.RestrictUsage(smi, false);
		});
		this.restriction.controlled.DefaultState(this.restriction.controlled.nostation);
		this.restriction.controlled.nostation.Enter(new StateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State.Callback(this.OnRocketRestrictionChanged)).ParamTransition<GameObject>(this.rocketControlStation, this.restriction.controlled.controlled, GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.IsNotNull);
		this.restriction.controlled.controlled.OnTargetLost(this.rocketControlStation, this.restriction.controlled.nostation).Enter(new StateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State.Callback(this.OnRocketRestrictionChanged)).Target(this.rocketControlStation).EventHandler(GameHashes.RocketRestrictionChanged, new StateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State.Callback(this.OnRocketRestrictionChanged)).Target(this.masterTarget);
	}

	// Token: 0x06007F64 RID: 32612 RVA: 0x000F8617 File Offset: 0x000F6817
	private void OnRocketRestrictionChanged(RocketUsageRestriction.StatesInstance smi)
	{
		this.RestrictUsage(smi, smi.BuildingRestrictionsActive());
	}

	// Token: 0x06007F65 RID: 32613 RVA: 0x0033C354 File Offset: 0x0033A554
	private void RestrictUsage(RocketUsageRestriction.StatesInstance smi, bool restrict)
	{
		smi.master.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.RocketRestrictionInactive, !restrict && smi.isControlled, null);
		if (smi.isRestrictionApplied == restrict)
		{
			return;
		}
		smi.isRestrictionApplied = restrict;
		smi.operational.SetFlag(RocketUsageRestriction.rocketUsageAllowed, !smi.def.restrictOperational || !restrict);
		smi.master.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.RocketRestrictionActive, restrict, null);
		Storage[] components = smi.master.gameObject.GetComponents<Storage>();
		if (components != null && components.Length != 0)
		{
			for (int i = 0; i < components.Length; i++)
			{
				if (restrict)
				{
					smi.previousStorageAllowItemRemovalStates = new bool[components.Length];
					smi.previousStorageAllowItemRemovalStates[i] = components[i].allowItemRemoval;
					components[i].allowItemRemoval = false;
				}
				else if (smi.previousStorageAllowItemRemovalStates != null && i < smi.previousStorageAllowItemRemovalStates.Length)
				{
					components[i].allowItemRemoval = smi.previousStorageAllowItemRemovalStates[i];
				}
				foreach (GameObject go in components[i].items)
				{
					go.Trigger(-778359855, components[i]);
				}
			}
		}
		Ownable component = smi.master.GetComponent<Ownable>();
		if (restrict && component != null && component.IsAssigned())
		{
			component.Unassign();
		}
	}

	// Token: 0x06007F66 RID: 32614 RVA: 0x0033C4DC File Offset: 0x0033A6DC
	private void AquireRocketControlStation(RocketUsageRestriction.StatesInstance smi)
	{
		if (!this.rocketControlStation.IsNull(smi))
		{
			return;
		}
		foreach (object obj in Components.RocketControlStations)
		{
			RocketControlStation rocketControlStation = (RocketControlStation)obj;
			if (rocketControlStation.GetMyWorldId() == smi.GetMyWorldId())
			{
				this.rocketControlStation.Set(rocketControlStation, smi);
			}
		}
	}

	// Token: 0x040060E2 RID: 24802
	public static readonly Operational.Flag rocketUsageAllowed = new Operational.Flag("rocketUsageAllowed", Operational.Flag.Type.Requirement);

	// Token: 0x040060E3 RID: 24803
	private StateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.TargetParameter rocketControlStation;

	// Token: 0x040060E4 RID: 24804
	public RocketUsageRestriction.RestrictionStates restriction;

	// Token: 0x02001837 RID: 6199
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06007F6A RID: 32618 RVA: 0x000F864A File Offset: 0x000F684A
		public override void Configure(GameObject prefab)
		{
			RocketControlStation.CONTROLLED_BUILDINGS.Add(prefab.PrefabID());
		}

		// Token: 0x040060E5 RID: 24805
		public bool initialControlledStateWhenBuilt = true;

		// Token: 0x040060E6 RID: 24806
		public bool restrictOperational = true;
	}

	// Token: 0x02001838 RID: 6200
	public class ControlledStates : GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State
	{
		// Token: 0x040060E7 RID: 24807
		public GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State nostation;

		// Token: 0x040060E8 RID: 24808
		public GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State controlled;
	}

	// Token: 0x02001839 RID: 6201
	public class RestrictionStates : GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State
	{
		// Token: 0x040060E9 RID: 24809
		public GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.State uncontrolled;

		// Token: 0x040060EA RID: 24810
		public RocketUsageRestriction.ControlledStates controlled;
	}

	// Token: 0x0200183A RID: 6202
	public class StatesInstance : GameStateMachine<RocketUsageRestriction, RocketUsageRestriction.StatesInstance, IStateMachineTarget, RocketUsageRestriction.Def>.GameInstance
	{
		// Token: 0x06007F6E RID: 32622 RVA: 0x000F867A File Offset: 0x000F687A
		public StatesInstance(IStateMachineTarget master, RocketUsageRestriction.Def def) : base(master, def)
		{
			this.isControlled = def.initialControlledStateWhenBuilt;
		}

		// Token: 0x06007F6F RID: 32623 RVA: 0x0033C558 File Offset: 0x0033A758
		public void OnRefreshUserMenu(object data)
		{
			KIconButtonMenu.ButtonInfo button;
			if (this.isControlled)
			{
				button = new KIconButtonMenu.ButtonInfo("action_rocket_restriction_uncontrolled", UI.USERMENUACTIONS.ROCKETUSAGERESTRICTION.NAME_UNCONTROLLED, new System.Action(this.OnChange), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ROCKETUSAGERESTRICTION.TOOLTIP_UNCONTROLLED, true);
			}
			else
			{
				button = new KIconButtonMenu.ButtonInfo("action_rocket_restriction_controlled", UI.USERMENUACTIONS.ROCKETUSAGERESTRICTION.NAME_CONTROLLED, new System.Action(this.OnChange), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ROCKETUSAGERESTRICTION.TOOLTIP_CONTROLLED, true);
			}
			Game.Instance.userMenu.AddButton(base.gameObject, button, 11f);
		}

		// Token: 0x06007F70 RID: 32624 RVA: 0x000F8697 File Offset: 0x000F6897
		public void ControlStationBuilt(object o)
		{
			base.sm.AquireRocketControlStation(base.smi);
		}

		// Token: 0x06007F71 RID: 32625 RVA: 0x000F86AA File Offset: 0x000F68AA
		private void OnChange()
		{
			this.isControlled = !this.isControlled;
			this.GoToRestrictionState();
		}

		// Token: 0x06007F72 RID: 32626 RVA: 0x0033C5F4 File Offset: 0x0033A7F4
		public void GoToRestrictionState()
		{
			if (base.smi.isControlled)
			{
				base.smi.GoTo(base.sm.restriction.controlled);
				return;
			}
			base.smi.GoTo(base.sm.restriction.uncontrolled);
		}

		// Token: 0x06007F73 RID: 32627 RVA: 0x000F86C1 File Offset: 0x000F68C1
		public bool BuildingRestrictionsActive()
		{
			return this.isControlled && !base.sm.rocketControlStation.IsNull(base.smi) && base.sm.rocketControlStation.Get<RocketControlStation>(base.smi).BuildingRestrictionsActive;
		}

		// Token: 0x040060EB RID: 24811
		[MyCmpGet]
		public Operational operational;

		// Token: 0x040060EC RID: 24812
		public bool[] previousStorageAllowItemRemovalStates;

		// Token: 0x040060ED RID: 24813
		[Serialize]
		public bool isControlled = true;

		// Token: 0x040060EE RID: 24814
		public bool isRestrictionApplied;
	}
}
