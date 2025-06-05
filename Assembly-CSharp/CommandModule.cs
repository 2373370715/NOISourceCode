using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x0200192F RID: 6447
[SerializationConfig(MemberSerialization.OptIn)]
public class CommandModule : StateMachineComponent<CommandModule.StatesInstance>
{
	// Token: 0x060085EF RID: 34287 RVA: 0x000FC7D5 File Offset: 0x000FA9D5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.rocketStats = new RocketStats(this);
		this.conditions = base.GetComponent<CommandConditions>();
	}

	// Token: 0x060085F0 RID: 34288 RVA: 0x00357610 File Offset: 0x00355810
	public void ReleaseAstronaut(bool fill_bladder)
	{
		if (this.releasingAstronaut || this.robotPilotControlled)
		{
			return;
		}
		this.releasingAstronaut = true;
		MinionStorage component = base.GetComponent<MinionStorage>();
		List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
		for (int i = storedMinionInfo.Count - 1; i >= 0; i--)
		{
			MinionStorage.Info info = storedMinionInfo[i];
			GameObject gameObject = component.DeserializeMinion(info.id, Grid.CellToPos(Grid.PosToCell(base.smi.master.transform.GetPosition())));
			if (!(gameObject == null))
			{
				if (Grid.FakeFloor[Grid.OffsetCell(Grid.PosToCell(base.smi.master.gameObject), 0, -1)])
				{
					gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
				}
				if (fill_bladder)
				{
					AmountInstance amountInstance = Db.Get().Amounts.Bladder.Lookup(gameObject);
					if (amountInstance != null)
					{
						amountInstance.value = amountInstance.GetMax();
					}
				}
			}
		}
		this.releasingAstronaut = false;
	}

	// Token: 0x060085F1 RID: 34289 RVA: 0x00357704 File Offset: 0x00355904
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.storage = base.GetComponent<Storage>();
		if (!this.robotPilotControlled)
		{
			this.assignable = base.GetComponent<Assignable>();
			this.assignable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanAssignTo));
			int cell = Grid.PosToCell(base.gameObject);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("CommandModule.gantryChanged", base.gameObject, cell, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnGantryChanged));
			this.OnGantryChanged(null);
		}
		base.smi.StartSM();
	}

	// Token: 0x060085F2 RID: 34290 RVA: 0x003577A0 File Offset: 0x003559A0
	private bool CanAssignTo(MinionAssignablesProxy worker)
	{
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			return minionIdentity.GetComponent<MinionResume>().HasPerk(Db.Get().SkillPerks.CanUseRockets);
		}
		StoredMinionIdentity storedMinionIdentity = worker.target as StoredMinionIdentity;
		if (storedMinionIdentity != null)
		{
			if (storedMinionIdentity.model == BionicMinionConfig.MODEL)
			{
				MinionStorageDataHolder component = storedMinionIdentity.GetComponent<MinionStorageDataHolder>();
				if (component != null)
				{
					MinionStorageDataHolder.DataPack dataPack = component.GetDataPack<BionicUpgradesMonitor.Instance>();
					if (dataPack != null)
					{
						MinionStorageDataHolder.DataPackData dataPackData = dataPack.PeekData();
						if (dataPackData != null && dataPackData.Tags != null)
						{
							Tag[] tags = dataPackData.Tags;
							for (int i = 0; i < tags.Length; i++)
							{
								if (tags[i] == "Booster_PilotVanilla1")
								{
									return true;
								}
							}
						}
					}
				}
			}
			return storedMinionIdentity.HasPerk(Db.Get().SkillPerks.CanUseRockets);
		}
		return false;
	}

	// Token: 0x060085F3 RID: 34291 RVA: 0x0035787C File Offset: 0x00355A7C
	private static bool HasValidGantry(GameObject go)
	{
		int num = Grid.OffsetCell(Grid.PosToCell(go), 0, -1);
		return Grid.IsValidCell(num) && Grid.FakeFloor[num];
	}

	// Token: 0x060085F4 RID: 34292 RVA: 0x003578AC File Offset: 0x00355AAC
	private void OnGantryChanged(object data)
	{
		if (base.gameObject != null)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.HasGantry, false);
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.MissingGantry, false);
			if (CommandModule.HasValidGantry(base.smi.master.gameObject))
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.HasGantry, null);
			}
			else
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.MissingGantry, null);
			}
			base.smi.sm.gantryChanged.Trigger(base.smi);
		}
	}

	// Token: 0x060085F5 RID: 34293 RVA: 0x00357964 File Offset: 0x00355B64
	private Chore CreateWorkChore()
	{
		WorkChore<CommandModuleWorkable> workChore = new WorkChore<CommandModuleWorkable>(Db.Get().ChoreTypes.Astronaut, this, null, true, null, null, null, false, null, false, true, Assets.GetAnim("anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanUseRockets);
		workChore.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, this.assignable);
		return workChore;
	}

	// Token: 0x060085F6 RID: 34294 RVA: 0x000FC7F5 File Offset: 0x000FA9F5
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		this.partitionerEntry.Clear();
		this.ReleaseAstronaut(false);
		base.smi.StopSM("cleanup");
	}

	// Token: 0x040065B4 RID: 26036
	public Storage storage;

	// Token: 0x040065B5 RID: 26037
	public RocketStats rocketStats;

	// Token: 0x040065B6 RID: 26038
	public CommandConditions conditions;

	// Token: 0x040065B7 RID: 26039
	private bool releasingAstronaut;

	// Token: 0x040065B8 RID: 26040
	private const Sim.Cell.Properties floorCellProperties = (Sim.Cell.Properties)39;

	// Token: 0x040065B9 RID: 26041
	public Assignable assignable;

	// Token: 0x040065BA RID: 26042
	public bool robotPilotControlled;

	// Token: 0x040065BB RID: 26043
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x02001930 RID: 6448
	public class StatesInstance : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.GameInstance
	{
		// Token: 0x060085F8 RID: 34296 RVA: 0x000FC831 File Offset: 0x000FAA31
		public StatesInstance(CommandModule master) : base(master)
		{
		}

		// Token: 0x060085F9 RID: 34297 RVA: 0x003579DC File Offset: 0x00355BDC
		public void SetSuspended(bool suspended)
		{
			Storage component = base.GetComponent<Storage>();
			if (component != null)
			{
				component.allowItemRemoval = !suspended;
			}
			ManualDeliveryKG component2 = base.GetComponent<ManualDeliveryKG>();
			if (component2 != null)
			{
				component2.Pause(suspended, "Rocket is suspended");
			}
		}

		// Token: 0x060085FA RID: 34298 RVA: 0x00357A20 File Offset: 0x00355C20
		public bool CheckStoredMinionIsAssignee()
		{
			if (base.smi.master.robotPilotControlled)
			{
				return true;
			}
			foreach (MinionStorage.Info info in base.GetComponent<MinionStorage>().GetStoredMinionInfo())
			{
				if (info.serializedMinion != null)
				{
					KPrefabID kprefabID = info.serializedMinion.Get();
					if (!(kprefabID == null))
					{
						StoredMinionIdentity component = kprefabID.GetComponent<StoredMinionIdentity>();
						if (base.GetComponent<Assignable>().assignee == component.assignableProxy.Get())
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}

	// Token: 0x02001931 RID: 6449
	public class States : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule>
	{
		// Token: 0x060085FB RID: 34299 RVA: 0x00357ACC File Offset: 0x00355CCC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grounded;
			this.grounded.PlayAnim("grounded", KAnim.PlayMode.Loop).DefaultState(this.grounded.awaitingAstronaut).TagTransition(GameTags.RocketNotOnGround, this.spaceborne, false);
			this.grounded.refreshChore.GoTo(this.grounded.awaitingAstronaut);
			this.grounded.awaitingAstronaut.Enter(delegate(CommandModule.StatesInstance smi)
			{
				if (smi.CheckStoredMinionIsAssignee())
				{
					smi.GoTo(this.grounded.hasAstronaut);
				}
				Game.Instance.userMenu.Refresh(smi.gameObject);
			}).EventHandler(GameHashes.AssigneeChanged, delegate(CommandModule.StatesInstance smi)
			{
				if (smi.CheckStoredMinionIsAssignee())
				{
					smi.GoTo(this.grounded.hasAstronaut);
				}
				else
				{
					smi.GoTo(this.grounded.refreshChore);
				}
				Game.Instance.userMenu.Refresh(smi.gameObject);
			}).ToggleChore((CommandModule.StatesInstance smi) => smi.master.CreateWorkChore(), this.grounded.hasAstronaut);
			this.grounded.hasAstronaut.EventHandler(GameHashes.AssigneeChanged, delegate(CommandModule.StatesInstance smi)
			{
				if (!smi.CheckStoredMinionIsAssignee())
				{
					smi.GoTo(this.grounded.waitingToRelease);
				}
			});
			this.grounded.waitingToRelease.ToggleStatusItem(Db.Get().BuildingStatusItems.DisembarkingDuplicant, null).OnSignal(this.gantryChanged, this.grounded.awaitingAstronaut, delegate(CommandModule.StatesInstance smi)
			{
				if (CommandModule.HasValidGantry(smi.gameObject))
				{
					smi.master.ReleaseAstronaut(this.accumulatedPee.Get(smi));
					this.accumulatedPee.Set(false, smi, false);
					Game.Instance.userMenu.Refresh(smi.gameObject);
					return true;
				}
				return false;
			});
			this.spaceborne.DefaultState(this.spaceborne.launch);
			this.spaceborne.launch.Enter(delegate(CommandModule.StatesInstance smi)
			{
				smi.SetSuspended(true);
			}).GoTo(this.spaceborne.idle);
			this.spaceborne.idle.TagTransition(GameTags.RocketNotOnGround, this.spaceborne.land, true);
			this.spaceborne.land.Enter(delegate(CommandModule.StatesInstance smi)
			{
				smi.SetSuspended(false);
				Game.Instance.userMenu.Refresh(smi.gameObject);
				this.accumulatedPee.Set(true, smi, false);
			}).GoTo(this.grounded.waitingToRelease);
		}

		// Token: 0x040065BC RID: 26044
		public StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.Signal gantryChanged;

		// Token: 0x040065BD RID: 26045
		public StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.BoolParameter accumulatedPee;

		// Token: 0x040065BE RID: 26046
		public CommandModule.States.GroundedStates grounded;

		// Token: 0x040065BF RID: 26047
		public CommandModule.States.SpaceborneStates spaceborne;

		// Token: 0x02001932 RID: 6450
		public class GroundedStates : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State
		{
			// Token: 0x040065C0 RID: 26048
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State refreshChore;

			// Token: 0x040065C1 RID: 26049
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State awaitingAstronaut;

			// Token: 0x040065C2 RID: 26050
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State hasAstronaut;

			// Token: 0x040065C3 RID: 26051
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State waitingToRelease;
		}

		// Token: 0x02001933 RID: 6451
		public class SpaceborneStates : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State
		{
			// Token: 0x040065C4 RID: 26052
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State launch;

			// Token: 0x040065C5 RID: 26053
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State idle;

			// Token: 0x040065C6 RID: 26054
			public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State land;
		}
	}
}
