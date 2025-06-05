using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000201 RID: 513
public class SeedPlantingStates : GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>
{
	// Token: 0x060006EB RID: 1771 RVA: 0x00165F2C File Offset: 0x0016412C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.findSeed;
		GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.PLANTINGSEED.NAME;
		string tooltip = CREATURES.STATUSITEMS.PLANTINGSEED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.UnreserveSeed)).Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.DropAll)).Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.RemoveMouthOverride));
		this.findSeed.Enter(delegate(SeedPlantingStates.Instance smi)
		{
			SeedPlantingStates.FindSeed(smi);
			if (smi.targetSeed == null)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			SeedPlantingStates.ReserveSeed(smi);
			smi.GoTo(this.moveToSeed);
		});
		this.moveToSeed.MoveTo(new Func<SeedPlantingStates.Instance, int>(SeedPlantingStates.GetSeedCell), this.findPlantLocation, this.behaviourcomplete, false);
		this.findPlantLocation.Enter(delegate(SeedPlantingStates.Instance smi)
		{
			if (!smi.targetSeed)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			SeedPlantingStates.FindDirtPlot(smi);
			if (smi.targetPlot != null || smi.targetDirtPlotCell != Grid.InvalidCell)
			{
				smi.GoTo(this.pickupSeed);
				return;
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.pickupSeed.PlayAnim("gather").Enter(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.PickupComplete)).OnAnimQueueComplete(this.moveToPlantLocation);
		this.moveToPlantLocation.Enter(delegate(SeedPlantingStates.Instance smi)
		{
			if (smi.targetSeed == null)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			if (smi.targetPlot != null)
			{
				smi.GoTo(this.moveToPlot);
				return;
			}
			if (smi.targetDirtPlotCell != Grid.InvalidCell)
			{
				smi.GoTo(this.moveToDirt);
				return;
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.moveToDirt.MoveTo((SeedPlantingStates.Instance smi) => smi.targetDirtPlotCell, this.planting, this.behaviourcomplete, false);
		this.moveToPlot.Enter(delegate(SeedPlantingStates.Instance smi)
		{
			if (smi.targetPlot == null || smi.targetSeed == null)
			{
				smi.GoTo(this.behaviourcomplete);
			}
		}).MoveTo(new Func<SeedPlantingStates.Instance, int>(SeedPlantingStates.GetPlantableCell), this.planting, this.behaviourcomplete, false);
		this.planting.Enter(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.RemoveMouthOverride)).PlayAnim("plant").Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.PlantComplete)).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToPlantSeed, false);
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x00166118 File Offset: 0x00164318
	private static void AddMouthOverride(SeedPlantingStates.Instance smi)
	{
		SymbolOverrideController component = smi.GetComponent<SymbolOverrideController>();
		KAnim.Build.Symbol symbol = smi.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol(smi.def.prefix + "sq_mouth_cheeks");
		if (symbol != null)
		{
			component.AddSymbolOverride("sq_mouth", symbol, 1);
		}
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x000AD633 File Offset: 0x000AB833
	private static void RemoveMouthOverride(SeedPlantingStates.Instance smi)
	{
		smi.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride("sq_mouth", 1);
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x0016617C File Offset: 0x0016437C
	private static void PickupComplete(SeedPlantingStates.Instance smi)
	{
		if (!smi.targetSeed)
		{
			global::Debug.LogWarningFormat("PickupComplete seed {0} is null", new object[]
			{
				smi.targetSeed
			});
			return;
		}
		SeedPlantingStates.UnreserveSeed(smi);
		int num = Grid.PosToCell(smi.targetSeed);
		if (smi.seed_cell != num)
		{
			global::Debug.LogWarningFormat("PickupComplete seed {0} moved {1} != {2}", new object[]
			{
				smi.targetSeed,
				num,
				smi.seed_cell
			});
			smi.targetSeed = null;
			return;
		}
		if (smi.targetSeed.HasTag(GameTags.Stored))
		{
			global::Debug.LogWarningFormat("PickupComplete seed {0} was stored by {1}", new object[]
			{
				smi.targetSeed,
				smi.targetSeed.storage
			});
			smi.targetSeed = null;
			return;
		}
		smi.targetSeed = EntitySplitter.Split(smi.targetSeed, 1f, null);
		smi.GetComponent<Storage>().Store(smi.targetSeed.gameObject, false, false, true, false);
		SeedPlantingStates.AddMouthOverride(smi);
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x0016627C File Offset: 0x0016447C
	private static void PlantComplete(SeedPlantingStates.Instance smi)
	{
		PlantableSeed plantableSeed = smi.targetSeed ? smi.targetSeed.GetComponent<PlantableSeed>() : null;
		PlantablePlot plantablePlot;
		if (plantableSeed && SeedPlantingStates.CheckValidPlotCell(smi, plantableSeed, smi.targetDirtPlotCell, out plantablePlot))
		{
			if (plantablePlot)
			{
				if (plantablePlot.Occupant == null)
				{
					plantablePlot.ForceDeposit(smi.targetSeed.gameObject);
				}
			}
			else
			{
				plantableSeed.TryPlant(true);
			}
		}
		smi.targetSeed = null;
		smi.seed_cell = Grid.InvalidCell;
		smi.targetPlot = null;
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x0015EC9C File Offset: 0x0015CE9C
	private static void DropAll(SeedPlantingStates.Instance smi)
	{
		smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x00166308 File Offset: 0x00164508
	private static int GetPlantableCell(SeedPlantingStates.Instance smi)
	{
		int num = Grid.PosToCell(smi.targetPlot);
		if (Grid.IsValidCell(num))
		{
			return Grid.CellAbove(num);
		}
		return num;
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x00166334 File Offset: 0x00164534
	private static void FindDirtPlot(SeedPlantingStates.Instance smi)
	{
		smi.targetDirtPlotCell = Grid.InvalidCell;
		PlantableSeed component = smi.targetSeed.GetComponent<PlantableSeed>();
		PlantableCellQuery plantableCellQuery = PathFinderQueries.plantableCellQuery.Reset(component, 20);
		smi.GetComponent<Navigator>().RunQuery(plantableCellQuery);
		if (plantableCellQuery.result_cells.Count > 0)
		{
			smi.targetDirtPlotCell = plantableCellQuery.result_cells[UnityEngine.Random.Range(0, plantableCellQuery.result_cells.Count)];
		}
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x001663A4 File Offset: 0x001645A4
	private static bool CheckValidPlotCell(SeedPlantingStates.Instance smi, PlantableSeed seed, int cell, out PlantablePlot plot)
	{
		plot = null;
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int num;
		if (seed.Direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
		{
			num = Grid.CellAbove(cell);
		}
		else
		{
			num = Grid.CellBelow(cell);
		}
		if (!Grid.IsValidCell(num))
		{
			return false;
		}
		if (!Grid.Solid[num])
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[num, 1];
		if (gameObject)
		{
			plot = gameObject.GetComponent<PlantablePlot>();
			return plot != null;
		}
		return seed.TestSuitableGround(cell);
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x000AD64C File Offset: 0x000AB84C
	private static int GetSeedCell(SeedPlantingStates.Instance smi)
	{
		global::Debug.Assert(smi.targetSeed);
		global::Debug.Assert(smi.seed_cell != Grid.InvalidCell);
		return smi.seed_cell;
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x00166420 File Offset: 0x00164620
	private static void FindSeed(SeedPlantingStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		Pickupable targetSeed = null;
		int num = 100;
		foreach (object obj in Components.PlantableSeeds)
		{
			PlantableSeed plantableSeed = (PlantableSeed)obj;
			if ((plantableSeed.HasTag(GameTags.Seed) || plantableSeed.HasTag(GameTags.CropSeed)) && !plantableSeed.HasTag(GameTags.Creatures.ReservedByCreature) && Vector2.Distance(smi.transform.position, plantableSeed.transform.position) <= 25f)
			{
				int navigationCost = component.GetNavigationCost(Grid.PosToCell(plantableSeed));
				if (navigationCost != -1 && navigationCost < num)
				{
					targetSeed = plantableSeed.GetComponent<Pickupable>();
					num = navigationCost;
				}
			}
		}
		smi.targetSeed = targetSeed;
		smi.seed_cell = (smi.targetSeed ? Grid.PosToCell(smi.targetSeed) : Grid.InvalidCell);
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x00166530 File Offset: 0x00164730
	private static void ReserveSeed(SeedPlantingStates.Instance smi)
	{
		GameObject gameObject = smi.targetSeed ? smi.targetSeed.gameObject : null;
		if (gameObject != null)
		{
			DebugUtil.Assert(!gameObject.HasTag(GameTags.Creatures.ReservedByCreature));
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x00166580 File Offset: 0x00164780
	private static void UnreserveSeed(SeedPlantingStates.Instance smi)
	{
		GameObject go = smi.targetSeed ? smi.targetSeed.gameObject : null;
		if (smi.targetSeed != null)
		{
			go.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x04000519 RID: 1305
	private const int MAX_NAVIGATE_DISTANCE = 100;

	// Token: 0x0400051A RID: 1306
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State findSeed;

	// Token: 0x0400051B RID: 1307
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToSeed;

	// Token: 0x0400051C RID: 1308
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State pickupSeed;

	// Token: 0x0400051D RID: 1309
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State findPlantLocation;

	// Token: 0x0400051E RID: 1310
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToPlantLocation;

	// Token: 0x0400051F RID: 1311
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToPlot;

	// Token: 0x04000520 RID: 1312
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToDirt;

	// Token: 0x04000521 RID: 1313
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State planting;

	// Token: 0x04000522 RID: 1314
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State behaviourcomplete;

	// Token: 0x02000202 RID: 514
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060006FD RID: 1789 RVA: 0x000AD6E0 File Offset: 0x000AB8E0
		public Def(string prefix)
		{
			this.prefix = prefix;
		}

		// Token: 0x04000523 RID: 1315
		public string prefix;
	}

	// Token: 0x02000203 RID: 515
	public new class Instance : GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.GameInstance
	{
		// Token: 0x060006FE RID: 1790 RVA: 0x00166694 File Offset: 0x00164894
		public Instance(Chore<SeedPlantingStates.Instance> chore, SeedPlantingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToPlantSeed);
		}

		// Token: 0x04000524 RID: 1316
		public PlantablePlot targetPlot;

		// Token: 0x04000525 RID: 1317
		public int targetDirtPlotCell = Grid.InvalidCell;

		// Token: 0x04000526 RID: 1318
		public Element plantElement = ElementLoader.FindElementByHash(SimHashes.Dirt);

		// Token: 0x04000527 RID: 1319
		public Pickupable targetSeed;

		// Token: 0x04000528 RID: 1320
		public int seed_cell = Grid.InvalidCell;
	}
}
