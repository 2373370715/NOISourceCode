using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class ConduitSleepStates : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>
{
	// Token: 0x060004BB RID: 1211 RVA: 0x0015FDC4 File Offset: 0x0015DFC4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.connector.moveToSleepLocation;
		this.root.EventTransition(GameHashes.NewDay, (ConduitSleepStates.Instance smi) => GameClock.Instance, this.behaviourcomplete, null).Exit(new StateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State.Callback(ConduitSleepStates.CleanUp));
		GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State moveToSleepLocation = this.connector.moveToSleepLocation;
		string name = CREATURES.STATUSITEMS.DROWSY.NAME;
		string tooltip = CREATURES.STATUSITEMS.DROWSY.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		moveToSleepLocation.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).MoveTo(delegate(ConduitSleepStates.Instance smi)
		{
			ConduitSleepMonitor.Instance smi2 = smi.GetSMI<ConduitSleepMonitor.Instance>();
			return smi2.sm.targetSleepCell.Get(smi2);
		}, this.drowsy, this.behaviourcomplete, false);
		GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State state = this.drowsy;
		string name2 = CREATURES.STATUSITEMS.DROWSY.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.DROWSY.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main).Enter(delegate(ConduitSleepStates.Instance smi)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Ceiling);
		}).Enter(delegate(ConduitSleepStates.Instance smi)
		{
			if (GameClock.Instance.IsNighttime())
			{
				smi.GoTo(this.connector.sleep);
			}
		}).DefaultState(this.drowsy.loop);
		this.drowsy.loop.PlayAnim("drowsy_pre").QueueAnim("drowsy_loop", true, null).EventTransition(GameHashes.Nighttime, (ConduitSleepStates.Instance smi) => GameClock.Instance, this.drowsy.pst, (ConduitSleepStates.Instance smi) => GameClock.Instance.IsNighttime());
		this.drowsy.pst.PlayAnim("drowsy_pst").OnAnimQueueComplete(this.connector.sleep);
		GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State sleep = this.connector.sleep;
		string name3 = CREATURES.STATUSITEMS.SLEEPING.NAME;
		string tooltip3 = CREATURES.STATUSITEMS.SLEEPING.TOOLTIP;
		string icon3 = "";
		StatusItem.IconType icon_type3 = StatusItem.IconType.Info;
		NotificationType notification_type3 = NotificationType.Neutral;
		bool allow_multiples3 = false;
		main = Db.Get().StatusItemCategories.Main;
		sleep.ToggleStatusItem(name3, tooltip3, icon3, icon_type3, notification_type3, allow_multiples3, default(HashedString), 129022, null, null, main).Enter(delegate(ConduitSleepStates.Instance smi)
		{
			if (!smi.staterpillar.IsConnectorBuildingSpawned())
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Ceiling);
			smi.staterpillar.EnableConnector();
			if (smi.staterpillar.IsConnected())
			{
				smi.GoTo(this.connector.sleep.connected);
				return;
			}
			smi.GoTo(this.connector.sleep.noConnection);
		});
		this.connector.sleep.connected.Enter(delegate(ConduitSleepStates.Instance smi)
		{
			smi.animController.SetSceneLayer(ConduitSleepStates.GetSleepingLayer(smi));
		}).Exit(delegate(ConduitSleepStates.Instance smi)
		{
			smi.animController.SetSceneLayer(Grid.SceneLayer.Creatures);
		}).EventTransition(GameHashes.NewDay, (ConduitSleepStates.Instance smi) => GameClock.Instance, this.connector.connectedWake, null).Transition(this.connector.sleep.noConnection, (ConduitSleepStates.Instance smi) => !smi.staterpillar.IsConnected(), UpdateRate.SIM_200ms).PlayAnim("sleep_charging_pre").QueueAnim("sleep_charging_loop", true, null).Update(new Action<ConduitSleepStates.Instance, float>(ConduitSleepStates.UpdateGulpSymbol), UpdateRate.SIM_1000ms, false).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.GameEvent.Callback(ConduitSleepStates.OnStorageChanged));
		this.connector.sleep.noConnection.PlayAnim("sleep_pre").QueueAnim("sleep_loop", true, null).ToggleStatusItem(new Func<ConduitSleepStates.Instance, StatusItem>(ConduitSleepStates.GetStatusItem), null).EventTransition(GameHashes.NewDay, (ConduitSleepStates.Instance smi) => GameClock.Instance, this.connector.noConnectionWake, null).Transition(this.connector.sleep.connected, (ConduitSleepStates.Instance smi) => smi.staterpillar.IsConnected(), UpdateRate.SIM_200ms);
		this.connector.connectedWake.QueueAnim("sleep_charging_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.connector.noConnectionWake.QueueAnim("sleep_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsConduitConnection, false);
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x0016022C File Offset: 0x0015E42C
	private static Grid.SceneLayer GetSleepingLayer(ConduitSleepStates.Instance smi)
	{
		ObjectLayer conduitLayer = smi.staterpillar.conduitLayer;
		Grid.SceneLayer result;
		if (conduitLayer != ObjectLayer.GasConduit)
		{
			if (conduitLayer != ObjectLayer.LiquidConduit)
			{
				if (conduitLayer == ObjectLayer.Wire)
				{
					result = Grid.SceneLayer.SolidConduitBridges;
				}
				else
				{
					result = Grid.SceneLayer.SolidConduitBridges;
				}
			}
			else
			{
				result = Grid.SceneLayer.GasConduitBridges;
			}
		}
		else
		{
			result = Grid.SceneLayer.Gas;
		}
		return result;
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x00160268 File Offset: 0x0015E468
	private static StatusItem GetStatusItem(ConduitSleepStates.Instance smi)
	{
		ObjectLayer conduitLayer = smi.staterpillar.conduitLayer;
		StatusItem result;
		if (conduitLayer != ObjectLayer.GasConduit)
		{
			if (conduitLayer != ObjectLayer.LiquidConduit)
			{
				if (conduitLayer == ObjectLayer.Wire)
				{
					result = Db.Get().BuildingStatusItems.NoWireConnected;
				}
				else
				{
					result = Db.Get().BuildingStatusItems.Normal;
				}
			}
			else
			{
				result = Db.Get().BuildingStatusItems.NeedLiquidOut;
			}
		}
		else
		{
			result = Db.Get().BuildingStatusItems.NeedGasOut;
		}
		return result;
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x001602D8 File Offset: 0x0015E4D8
	private static void OnStorageChanged(ConduitSleepStates.Instance smi, object obj)
	{
		GameObject gameObject = obj as GameObject;
		if (gameObject != null)
		{
			smi.amountDeposited += gameObject.GetComponent<PrimaryElement>().Mass;
		}
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x000ABDFA File Offset: 0x000A9FFA
	private static void UpdateGulpSymbol(ConduitSleepStates.Instance smi, float dt)
	{
		smi.SetGulpSymbolVisibility(smi.amountDeposited > 0f);
		smi.amountDeposited = 0f;
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00160310 File Offset: 0x0015E510
	private static void CleanUp(ConduitSleepStates.Instance smi)
	{
		ConduitSleepMonitor.Instance smi2 = smi.GetSMI<ConduitSleepMonitor.Instance>();
		if (smi2 != null)
		{
			smi2.sm.targetSleepCell.Set(Grid.InvalidCell, smi2, false);
		}
		smi.staterpillar.DestroyOrphanedConnectorBuilding();
	}

	// Token: 0x0400036E RID: 878
	public ConduitSleepStates.DrowsyStates drowsy;

	// Token: 0x0400036F RID: 879
	public ConduitSleepStates.HasConnectorStates connector;

	// Token: 0x04000370 RID: 880
	public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State behaviourcomplete;

	// Token: 0x02000145 RID: 325
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000371 RID: 881
		public HashedString gulpSymbol = "gulp";
	}

	// Token: 0x02000146 RID: 326
	public new class Instance : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.GameInstance
	{
		// Token: 0x060004C5 RID: 1221 RVA: 0x000ABE59 File Offset: 0x000AA059
		public Instance(Chore<ConduitSleepStates.Instance> chore, ConduitSleepStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsConduitConnection);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x001603C4 File Offset: 0x0015E5C4
		public void SetGulpSymbolVisibility(bool state)
		{
			string sound = GlobalAssets.GetSound("PlugSlug_Charging_Gulp_LP", false);
			if (this.gulpSymbolVisible != state)
			{
				this.gulpSymbolVisible = state;
				this.animController.SetSymbolVisiblity(base.def.gulpSymbol, state);
				if (state)
				{
					this.loopingSounds.StartSound(sound);
					return;
				}
				this.loopingSounds.StopSound(sound);
			}
		}

		// Token: 0x04000372 RID: 882
		[MyCmpReq]
		public KBatchedAnimController animController;

		// Token: 0x04000373 RID: 883
		[MyCmpReq]
		public Staterpillar staterpillar;

		// Token: 0x04000374 RID: 884
		[MyCmpAdd]
		private LoopingSounds loopingSounds;

		// Token: 0x04000375 RID: 885
		public bool gulpSymbolVisible;

		// Token: 0x04000376 RID: 886
		public float amountDeposited;
	}

	// Token: 0x02000147 RID: 327
	public class SleepStates : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State
	{
		// Token: 0x04000377 RID: 887
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State connected;

		// Token: 0x04000378 RID: 888
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State noConnection;
	}

	// Token: 0x02000148 RID: 328
	public class DrowsyStates : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State
	{
		// Token: 0x04000379 RID: 889
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State loop;

		// Token: 0x0400037A RID: 890
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State pst;
	}

	// Token: 0x02000149 RID: 329
	public class HasConnectorStates : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State
	{
		// Token: 0x0400037B RID: 891
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State moveToSleepLocation;

		// Token: 0x0400037C RID: 892
		public ConduitSleepStates.SleepStates sleep;

		// Token: 0x0400037D RID: 893
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State noConnectionWake;

		// Token: 0x0400037E RID: 894
		public GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>.State connectedWake;
	}
}
