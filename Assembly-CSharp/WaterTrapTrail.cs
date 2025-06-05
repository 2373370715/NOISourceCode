using System;

// Token: 0x02001A96 RID: 6806
public class WaterTrapTrail : GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>
{
	// Token: 0x06008DF3 RID: 36339 RVA: 0x00377F78 File Offset: 0x00376178
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.retracted;
		base.serializable = StateMachine.SerializeType.Never;
		this.retracted.EventHandler(GameHashes.TrapArmWorkPST, delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		}).EventHandlerTransition(GameHashes.TagsChanged, this.loose, new Func<WaterTrapTrail.Instance, object, bool>(WaterTrapTrail.ShouldBeVisible)).Enter(delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		});
		this.loose.EventHandlerTransition(GameHashes.TagsChanged, this.retracted, new Func<WaterTrapTrail.Instance, object, bool>(WaterTrapTrail.OnTagsChangedWhenOnLooseState)).EventHandler(GameHashes.TrapCaptureCompleted, delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		}).Enter(delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		});
	}

	// Token: 0x06008DF4 RID: 36340 RVA: 0x00378074 File Offset: 0x00376274
	public static bool OnTagsChangedWhenOnLooseState(WaterTrapTrail.Instance smi, object tagOBJ)
	{
		ReusableTrap.Instance smi2 = smi.gameObject.GetSMI<ReusableTrap.Instance>();
		if (smi2 != null)
		{
			smi2.CAPTURING_SYMBOL_NAME = WaterTrapTrail.CAPTURING_SYMBOL_OVERRIDE_NAME + smi.sm.depthAvailable.Get(smi).ToString();
		}
		return WaterTrapTrail.ShouldBeInvisible(smi, tagOBJ);
	}

	// Token: 0x06008DF5 RID: 36341 RVA: 0x00101376 File Offset: 0x000FF576
	public static bool ShouldBeInvisible(WaterTrapTrail.Instance smi, object tagOBJ)
	{
		return !WaterTrapTrail.ShouldBeVisible(smi, tagOBJ);
	}

	// Token: 0x06008DF6 RID: 36342 RVA: 0x003780C0 File Offset: 0x003762C0
	public static bool ShouldBeVisible(WaterTrapTrail.Instance smi, object tagOBJ)
	{
		ReusableTrap.Instance smi2 = smi.gameObject.GetSMI<ReusableTrap.Instance>();
		bool isOperational = smi.IsOperational;
		bool flag = smi.HasTag(GameTags.TrapArmed);
		bool flag2 = smi2 != null && smi2.IsInsideState(smi2.sm.operational.capture) && !smi2.IsInsideState(smi2.sm.operational.capture.idle) && !smi2.IsInsideState(smi2.sm.operational.capture.release);
		bool flag3 = smi2 != null && smi2.IsInsideState(smi2.sm.operational.unarmed) && smi2.GetWorkable().WorkInPstAnimation;
		return isOperational && (flag || flag2 || flag3);
	}

	// Token: 0x06008DF7 RID: 36343 RVA: 0x00378178 File Offset: 0x00376378
	public static void RefreshDepthAvailable(WaterTrapTrail.Instance smi, float dt)
	{
		bool flag = WaterTrapTrail.ShouldBeVisible(smi, null);
		int num = Grid.PosToCell(smi);
		int num2 = flag ? WaterTrapGuide.GetDepthAvailable(num, smi.gameObject) : 0;
		int num3 = 4;
		if (num2 != smi.sm.depthAvailable.Get(smi))
		{
			KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
			for (int i = 1; i <= num3; i++)
			{
				component.SetSymbolVisiblity("pipe" + i.ToString(), i <= num2);
				component.SetSymbolVisiblity(WaterTrapTrail.CAPTURING_SYMBOL_OVERRIDE_NAME + i.ToString(), i == num2);
			}
			int cell = Grid.OffsetCell(num, 0, -num2);
			smi.ChangeTrapCellPosition(cell);
			WaterTrapGuide.OccupyArea(smi.gameObject, num2);
			smi.sm.depthAvailable.Set(num2, smi, false);
		}
		smi.SetRangeVisualizerOffset(new Vector2I(0, -num2));
		smi.SetRangeVisualizerVisibility(flag);
	}

	// Token: 0x04006B2D RID: 27437
	private static string CAPTURING_SYMBOL_OVERRIDE_NAME = "creatureSymbol";

	// Token: 0x04006B2E RID: 27438
	public GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State retracted;

	// Token: 0x04006B2F RID: 27439
	public GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State loose;

	// Token: 0x04006B30 RID: 27440
	private StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.IntParameter depthAvailable = new StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.IntParameter(-1);

	// Token: 0x02001A97 RID: 6807
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001A98 RID: 6808
	public new class Instance : GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.GameInstance
	{
		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06008DFB RID: 36347 RVA: 0x001013A2 File Offset: 0x000FF5A2
		public bool IsOperational
		{
			get
			{
				return this.operational.IsOperational;
			}
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06008DFC RID: 36348 RVA: 0x001013AF File Offset: 0x000FF5AF
		public Lure.Instance lureSMI
		{
			get
			{
				if (this._lureSMI == null)
				{
					this._lureSMI = base.gameObject.GetSMI<Lure.Instance>();
				}
				return this._lureSMI;
			}
		}

		// Token: 0x06008DFD RID: 36349 RVA: 0x001013D0 File Offset: 0x000FF5D0
		public Instance(IStateMachineTarget master, WaterTrapTrail.Def def) : base(master, def)
		{
		}

		// Token: 0x06008DFE RID: 36350 RVA: 0x001013DA File Offset: 0x000FF5DA
		public override void StartSM()
		{
			base.StartSM();
			this.RegisterListenersToCellChanges();
		}

		// Token: 0x06008DFF RID: 36351 RVA: 0x00378268 File Offset: 0x00376468
		private void RegisterListenersToCellChanges()
		{
			int widthInCells = base.GetComponent<BuildingComplete>().Def.WidthInCells;
			CellOffset[] array = new CellOffset[widthInCells * 4];
			for (int i = 0; i < 4; i++)
			{
				int y = -(i + 1);
				for (int j = 0; j < widthInCells; j++)
				{
					array[i * widthInCells + j] = new CellOffset(j, y);
				}
			}
			Extents extents = new Extents(Grid.PosToCell(base.transform.GetPosition()), array);
			this.partitionerEntry_solids = GameScenePartitioner.Instance.Add("WaterTrapTrail", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnLowerCellChanged));
			this.partitionerEntry_buildings = GameScenePartitioner.Instance.Add("WaterTrapTrail", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnLowerCellChanged));
		}

		// Token: 0x06008E00 RID: 36352 RVA: 0x001013E8 File Offset: 0x000FF5E8
		private void UnregisterListenersToCellChanges()
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry_solids);
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry_buildings);
		}

		// Token: 0x06008E01 RID: 36353 RVA: 0x0010140A File Offset: 0x000FF60A
		private void OnLowerCellChanged(object o)
		{
			WaterTrapTrail.RefreshDepthAvailable(base.smi, 0f);
		}

		// Token: 0x06008E02 RID: 36354 RVA: 0x0010141C File Offset: 0x000FF61C
		protected override void OnCleanUp()
		{
			this.UnregisterListenersToCellChanges();
			base.OnCleanUp();
		}

		// Token: 0x06008E03 RID: 36355 RVA: 0x0010142A File Offset: 0x000FF62A
		public void SetRangeVisualizerVisibility(bool visible)
		{
			this.rangeVisualizer.RangeMax.x = (visible ? 0 : -1);
		}

		// Token: 0x06008E04 RID: 36356 RVA: 0x00101443 File Offset: 0x000FF643
		public void SetRangeVisualizerOffset(Vector2I offset)
		{
			this.rangeVisualizer.OriginOffset = offset;
		}

		// Token: 0x06008E05 RID: 36357 RVA: 0x00101451 File Offset: 0x000FF651
		public void ChangeTrapCellPosition(int cell)
		{
			if (this.lureSMI != null)
			{
				this.lureSMI.ChangeLureCellPosition(cell);
			}
			base.gameObject.GetComponent<TrapTrigger>().SetTriggerCell(cell);
		}

		// Token: 0x04006B31 RID: 27441
		[MyCmpGet]
		private Operational operational;

		// Token: 0x04006B32 RID: 27442
		[MyCmpGet]
		private RangeVisualizer rangeVisualizer;

		// Token: 0x04006B33 RID: 27443
		private HandleVector<int>.Handle partitionerEntry_buildings;

		// Token: 0x04006B34 RID: 27444
		private HandleVector<int>.Handle partitionerEntry_solids;

		// Token: 0x04006B35 RID: 27445
		private Lure.Instance _lureSMI;
	}
}
