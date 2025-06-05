using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000DBA RID: 3514
public class FoodDehydrator : GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>
{
	// Token: 0x06004466 RID: 17510 RVA: 0x00256648 File Offset: 0x00254848
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		this.waitingForFuelStatus.resolveStringCallback = ((string str, object obj) => string.Format(str, FOODDEHYDRATORTUNING.FUEL_TAG.ProperName(), GameUtil.GetFormattedMass(5.0000005f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")));
		default_state = this.waitingForFuel;
		this.waitingForFuel.Enter(delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.operational.SetFlag(FoodDehydrator.foodDehydratorFlag, false);
		}).EventTransition(GameHashes.OnStorageChange, this.working, (FoodDehydrator.StatesInstance smi) => smi.GetAvailableFuel() >= 5.0000005f).ToggleStatusItem(this.waitingForFuelStatus, null);
		this.working.Enter(delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.complexFabricator.SetQueueDirty();
			smi.operational.SetFlag(FoodDehydrator.foodDehydratorFlag, true);
		}).EventHandler(GameHashes.FabricatorOrdersUpdated, delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.UpdateFoodSymbol();
		}).EnterTransition(this.requestEmpty, (FoodDehydrator.StatesInstance smi) => smi.RequiresEmptying()).EventTransition(GameHashes.OnStorageChange, this.waitingForFuel, (FoodDehydrator.StatesInstance smi) => smi.GetAvailableFuel() <= 0f).EventHandlerTransition(GameHashes.FabricatorOrderCompleted, this.requestEmpty, (FoodDehydrator.StatesInstance smi, object data) => smi.RequiresEmptying()).EventHandler(GameHashes.FabricatorOrderStarted, delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.UpdateFoodSymbol();
		});
		this.requestEmpty.ToggleRecurringChore(new Func<FoodDehydrator.StatesInstance, Chore>(this.CreateChore), (FoodDehydrator.StatesInstance smi) => smi.RequiresEmptying()).EventHandlerTransition(GameHashes.OnStorageChange, this.working, (FoodDehydrator.StatesInstance smi, object data) => !smi.RequiresEmptying()).Enter(delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.operational.SetFlag(FoodDehydrator.foodDehydratorFlag, false);
			smi.UpdateFoodSymbol();
		}).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingEmptyBuilding, null);
	}

	// Token: 0x06004467 RID: 17511 RVA: 0x00256894 File Offset: 0x00254A94
	private Chore CreateChore(FoodDehydrator.StatesInstance smi)
	{
		WorkChore<FoodDehydratorWorkableEmpty> workChore = new WorkChore<FoodDehydratorWorkableEmpty>(Db.Get().ChoreTypes.FoodFetch, smi.master.GetComponent<FoodDehydratorWorkableEmpty>(), null, true, new Action<Chore>(smi.OnEmptyComplete), null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		return workChore;
	}

	// Token: 0x04002F72 RID: 12146
	private StatusItem waitingForFuelStatus = new StatusItem("waitingForFuelStatus", BUILDING.STATUSITEMS.ENOUGH_FUEL.NAME, BUILDING.STATUSITEMS.ENOUGH_FUEL.TOOLTIP, "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);

	// Token: 0x04002F73 RID: 12147
	private static readonly Operational.Flag foodDehydratorFlag = new Operational.Flag("food_dehydrator", Operational.Flag.Type.Requirement);

	// Token: 0x04002F74 RID: 12148
	private GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State waitingForFuel;

	// Token: 0x04002F75 RID: 12149
	private GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State working;

	// Token: 0x04002F76 RID: 12150
	private GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State requestEmpty;

	// Token: 0x02000DBB RID: 3515
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600446A RID: 17514 RVA: 0x0025693C File Offset: 0x00254B3C
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			Descriptor item = new Descriptor(UI.BUILDINGEFFECTS.FOOD_DEHYDRATOR_WATER_OUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.FOOD_DEHYDRATOR_WATER_OUTPUT, Descriptor.DescriptorType.Effect, false);
			list.Add(item);
			return list;
		}
	}

	// Token: 0x02000DBC RID: 3516
	public class StatesInstance : GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.GameInstance
	{
		// Token: 0x0600446C RID: 17516 RVA: 0x000D0A76 File Offset: 0x000CEC76
		public StatesInstance(IStateMachineTarget master, FoodDehydrator.Def def) : base(master, def)
		{
			this.SetupFoodSymbol();
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x000D0A86 File Offset: 0x000CEC86
		public float GetAvailableFuel()
		{
			return this.complexFabricator.inStorage.GetMassAvailable(FOODDEHYDRATORTUNING.FUEL_TAG);
		}

		// Token: 0x0600446E RID: 17518 RVA: 0x000D0A9D File Offset: 0x000CEC9D
		public bool RequiresEmptying()
		{
			return !this.complexFabricator.outStorage.IsEmpty();
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x00256974 File Offset: 0x00254B74
		public void OnEmptyComplete(Chore obj)
		{
			Vector3 position = Grid.CellToPosLCC(Grid.PosToCell(this), Grid.SceneLayer.Ore);
			this.complexFabricator.outStorage.DropAll(position, false, true, default(Vector3), true, null);
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x002569B0 File Offset: 0x00254BB0
		public void SetupFoodSymbol()
		{
			GameObject gameObject = Util.NewGameObject(base.gameObject, "food_symbol");
			gameObject.SetActive(false);
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			bool flag;
			Vector3 position = component.GetSymbolTransform(FoodDehydrator.StatesInstance.HASH_FOOD, out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			gameObject.transform.SetPosition(position);
			this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
			this.foodKBAC.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("mushbar_kanim")
			};
			this.foodKBAC.initialAnim = "object";
			component.SetSymbolVisiblity(FoodDehydrator.StatesInstance.HASH_FOOD, false);
			this.foodKBAC.sceneLayer = Grid.SceneLayer.BuildingUse;
			KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.symbol = new HashedString("food");
			kbatchedAnimTracker.offset = Vector3.zero;
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x00256A98 File Offset: 0x00254C98
		public void UpdateFoodSymbol()
		{
			ComplexRecipe currentWorkingOrder = this.complexFabricator.CurrentWorkingOrder;
			if (this.complexFabricator.CurrentWorkingOrder != null)
			{
				this.foodKBAC.gameObject.SetActive(true);
				GameObject prefab = Assets.GetPrefab(currentWorkingOrder.ingredients[this.foodIngredientIdx].material);
				this.foodKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
				this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
			if (this.complexFabricator.outStorage.items.Count > 0)
			{
				this.foodKBAC.gameObject.SetActive(true);
				this.foodKBAC.SwapAnims(this.complexFabricator.outStorage.items[0].GetComponent<KBatchedAnimController>().AnimFiles);
				this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
			this.foodKBAC.gameObject.SetActive(false);
		}

		// Token: 0x04002F77 RID: 12151
		[MyCmpReq]
		public Operational operational;

		// Token: 0x04002F78 RID: 12152
		[MyCmpReq]
		public ComplexFabricator complexFabricator;

		// Token: 0x04002F79 RID: 12153
		private static string HASH_FOOD = "food";

		// Token: 0x04002F7A RID: 12154
		private KBatchedAnimController foodKBAC;

		// Token: 0x04002F7B RID: 12155
		private int foodIngredientIdx;
	}
}
