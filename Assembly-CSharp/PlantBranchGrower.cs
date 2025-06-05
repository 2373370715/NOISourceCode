using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001734 RID: 5940
public class PlantBranchGrower : GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>
{
	// Token: 0x06007A24 RID: 31268 RVA: 0x003252B4 File Offset: 0x003234B4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.wilt;
		this.worldgen.Update(new Action<PlantBranchGrower.Instance, float>(PlantBranchGrower.WorldGenUpdate), UpdateRate.RENDER_EVERY_TICK, false);
		this.wilt.TagTransition(GameTags.Wilting, this.maturing, true);
		this.maturing.TagTransition(GameTags.Wilting, this.wilt, false).EnterTransition(this.growingBranches, new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.IsMature)).EventTransition(GameHashes.Grow, this.growingBranches, null);
		this.growingBranches.TagTransition(GameTags.Wilting, this.wilt, false).EventTransition(GameHashes.ConsumePlant, this.maturing, GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Not(new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.IsMature))).EventTransition(GameHashes.TreeBranchCountChanged, this.fullyGrown, new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.AllBranchesCreated)).ToggleStatusItem((PlantBranchGrower.Instance smi) => smi.def.growingBranchesStatusItem, null).Update(new Action<PlantBranchGrower.Instance, float>(PlantBranchGrower.GrowBranchUpdate), UpdateRate.SIM_4000ms, false);
		this.fullyGrown.TagTransition(GameTags.Wilting, this.wilt, false).EventTransition(GameHashes.ConsumePlant, this.maturing, GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Not(new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.IsMature))).EventTransition(GameHashes.TreeBranchCountChanged, this.growingBranches, new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.NotAllBranchesCreated));
	}

	// Token: 0x06007A25 RID: 31269 RVA: 0x000F4E17 File Offset: 0x000F3017
	public static bool NotAllBranchesCreated(PlantBranchGrower.Instance smi)
	{
		return smi.CurrentBranchCount < smi.MaxBranchesAllowedAtOnce;
	}

	// Token: 0x06007A26 RID: 31270 RVA: 0x000F4E27 File Offset: 0x000F3027
	public static bool AllBranchesCreated(PlantBranchGrower.Instance smi)
	{
		return smi.CurrentBranchCount >= smi.MaxBranchesAllowedAtOnce;
	}

	// Token: 0x06007A27 RID: 31271 RVA: 0x000F4E3A File Offset: 0x000F303A
	public static bool IsMature(PlantBranchGrower.Instance smi)
	{
		return smi.IsGrown;
	}

	// Token: 0x06007A28 RID: 31272 RVA: 0x000F4E42 File Offset: 0x000F3042
	public static void GrowBranchUpdate(PlantBranchGrower.Instance smi, float dt)
	{
		smi.SpawnRandomBranch(0f);
	}

	// Token: 0x06007A29 RID: 31273 RVA: 0x00325424 File Offset: 0x00323624
	public static void WorldGenUpdate(PlantBranchGrower.Instance smi, float dt)
	{
		float growth_percentage = UnityEngine.Random.Range(0f, 1f);
		if (!smi.SpawnRandomBranch(growth_percentage))
		{
			smi.GoTo(smi.sm.defaultState);
		}
	}

	// Token: 0x04005BEB RID: 23531
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State worldgen;

	// Token: 0x04005BEC RID: 23532
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State wilt;

	// Token: 0x04005BED RID: 23533
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State maturing;

	// Token: 0x04005BEE RID: 23534
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State growingBranches;

	// Token: 0x04005BEF RID: 23535
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State fullyGrown;

	// Token: 0x02001735 RID: 5941
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005BF0 RID: 23536
		public string BRANCH_PREFAB_NAME;

		// Token: 0x04005BF1 RID: 23537
		public int MAX_BRANCH_COUNT = -1;

		// Token: 0x04005BF2 RID: 23538
		public CellOffset[] BRANCH_OFFSETS;

		// Token: 0x04005BF3 RID: 23539
		public bool harvestOnDrown;

		// Token: 0x04005BF4 RID: 23540
		public bool propagateHarvestDesignation = true;

		// Token: 0x04005BF5 RID: 23541
		public Func<int, bool> additionalBranchGrowRequirements;

		// Token: 0x04005BF6 RID: 23542
		public Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchHarvested;

		// Token: 0x04005BF7 RID: 23543
		public Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchSpawned;

		// Token: 0x04005BF8 RID: 23544
		public StatusItem growingBranchesStatusItem = Db.Get().MiscStatusItems.GrowingBranches;

		// Token: 0x04005BF9 RID: 23545
		public Action<PlantBranchGrower.Instance> onEarlySpawn;
	}

	// Token: 0x02001736 RID: 5942
	public new class Instance : GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.GameInstance
	{
		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06007A2C RID: 31276 RVA: 0x000F4E83 File Offset: 0x000F3083
		public bool IsUprooted
		{
			get
			{
				return this.uprootMonitor != null && this.uprootMonitor.IsUprooted;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06007A2D RID: 31277 RVA: 0x000F4EA0 File Offset: 0x000F30A0
		public bool IsGrown
		{
			get
			{
				return this.growing == null || this.growing.PercentGrown() >= 1f;
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06007A2E RID: 31278 RVA: 0x000F4EC1 File Offset: 0x000F30C1
		public int MaxBranchesAllowedAtOnce
		{
			get
			{
				if (base.def.MAX_BRANCH_COUNT >= 0)
				{
					return Mathf.Min(base.def.MAX_BRANCH_COUNT, base.def.BRANCH_OFFSETS.Length);
				}
				return base.def.BRANCH_OFFSETS.Length;
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06007A2F RID: 31279 RVA: 0x0032545C File Offset: 0x0032365C
		public int CurrentBranchCount
		{
			get
			{
				int num = 0;
				if (this.branches != null)
				{
					int i = 0;
					while (i < this.branches.Length)
					{
						num += ((this.GetBranch(i++) != null) ? 1 : 0);
					}
				}
				return num;
			}
		}

		// Token: 0x06007A30 RID: 31280 RVA: 0x003254A0 File Offset: 0x003236A0
		public GameObject GetBranch(int idx)
		{
			if (this.branches != null && this.branches[idx] != null)
			{
				KPrefabID kprefabID = this.branches[idx].Get();
				if (kprefabID != null)
				{
					return kprefabID.gameObject;
				}
			}
			return null;
		}

		// Token: 0x06007A31 RID: 31281 RVA: 0x000F4EFC File Offset: 0x000F30FC
		protected override void OnCleanUp()
		{
			this.SetTrunkOccupyingCellsAsPlant(false);
			base.OnCleanUp();
		}

		// Token: 0x06007A32 RID: 31282 RVA: 0x003254E0 File Offset: 0x003236E0
		public Instance(IStateMachineTarget master, PlantBranchGrower.Def def) : base(master, def)
		{
			this.growing = base.GetComponent<IManageGrowingStates>();
			this.growing = ((this.growing != null) ? this.growing : base.gameObject.GetSMI<IManageGrowingStates>());
			this.SetTrunkOccupyingCellsAsPlant(true);
			base.Subscribe(1119167081, new Action<object>(this.OnNewGameSpawn));
			base.Subscribe(144050788, new Action<object>(this.OnUpdateRoom));
		}

		// Token: 0x06007A33 RID: 31283 RVA: 0x00325558 File Offset: 0x00323758
		public override void StartSM()
		{
			base.StartSM();
			Action<PlantBranchGrower.Instance> onEarlySpawn = base.def.onEarlySpawn;
			if (onEarlySpawn != null)
			{
				onEarlySpawn(this);
			}
			this.DefineBranchArray();
			base.Subscribe(-216549700, new Action<object>(this.OnUprooted));
			base.Subscribe(-266953818, delegate(object obj)
			{
				this.UpdateAutoHarvestValue(null);
			});
			if (base.def.harvestOnDrown)
			{
				base.Subscribe(-750750377, new Action<object>(this.OnUprooted));
			}
		}

		// Token: 0x06007A34 RID: 31284 RVA: 0x003255DC File Offset: 0x003237DC
		private void OnUpdateRoom(object data)
		{
			if (this.branches == null)
			{
				return;
			}
			this.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(144050788, data);
			});
		}

		// Token: 0x06007A35 RID: 31285 RVA: 0x00325614 File Offset: 0x00323814
		private void SetTrunkOccupyingCellsAsPlant(bool doSet)
		{
			CellOffset[] occupiedCellsOffsets = base.GetComponent<OccupyArea>().OccupiedCellsOffsets;
			int cell = Grid.PosToCell(base.gameObject);
			for (int i = 0; i < occupiedCellsOffsets.Length; i++)
			{
				int cell2 = Grid.OffsetCell(cell, occupiedCellsOffsets[i]);
				if (doSet)
				{
					Grid.Objects[cell2, 5] = base.gameObject;
				}
				else if (Grid.Objects[cell2, 5] == base.gameObject)
				{
					Grid.Objects[cell2, 5] = null;
				}
			}
		}

		// Token: 0x06007A36 RID: 31286 RVA: 0x00325694 File Offset: 0x00323894
		private void OnNewGameSpawn(object data)
		{
			this.DefineBranchArray();
			float percentage = 1f;
			if ((double)UnityEngine.Random.value < 0.1)
			{
				percentage = UnityEngine.Random.Range(0.75f, 0.99f);
			}
			else
			{
				this.GoTo(base.sm.worldgen);
			}
			this.growing.OverrideMaturityLevel(percentage);
		}

		// Token: 0x06007A37 RID: 31287 RVA: 0x003256F0 File Offset: 0x003238F0
		public void ManuallyDefineBranchArray(KPrefabID[] _branches)
		{
			this.DefineBranchArray();
			for (int i = 0; i < Mathf.Min(this.branches.Length, _branches.Length); i++)
			{
				KPrefabID kprefabID = _branches[i];
				if (kprefabID != null)
				{
					if (this.branches[i] == null)
					{
						this.branches[i] = new Ref<KPrefabID>();
					}
					this.branches[i].Set(kprefabID);
				}
				else
				{
					this.branches[i] = null;
				}
			}
		}

		// Token: 0x06007A38 RID: 31288 RVA: 0x000F4F0B File Offset: 0x000F310B
		private void DefineBranchArray()
		{
			if (this.branches == null)
			{
				this.branches = new Ref<KPrefabID>[base.def.BRANCH_OFFSETS.Length];
			}
		}

		// Token: 0x06007A39 RID: 31289 RVA: 0x0032575C File Offset: 0x0032395C
		public void ActionPerBranch(Action<GameObject> action)
		{
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null && action != null)
				{
					action(branch.gameObject);
				}
			}
		}

		// Token: 0x06007A3A RID: 31290 RVA: 0x0032579C File Offset: 0x0032399C
		public GameObject[] GetExistingBranches()
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null)
				{
					list.Add(branch.gameObject);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06007A3B RID: 31291 RVA: 0x003257E8 File Offset: 0x003239E8
		public void OnBranchRemoved(GameObject _branch)
		{
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null && branch == _branch)
				{
					this.branches[i] = null;
				}
			}
			base.gameObject.Trigger(-1586842875, null);
		}

		// Token: 0x06007A3C RID: 31292 RVA: 0x000F4F2D File Offset: 0x000F312D
		public void OnBrancHarvested(PlantBranch.Instance branch)
		{
			Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchHarvested = base.def.onBranchHarvested;
			if (onBranchHarvested == null)
			{
				return;
			}
			onBranchHarvested(branch, this);
		}

		// Token: 0x06007A3D RID: 31293 RVA: 0x0032583C File Offset: 0x00323A3C
		private void OnUprooted(object data = null)
		{
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null)
				{
					branch.Trigger(-216549700, null);
				}
			}
		}

		// Token: 0x06007A3E RID: 31294 RVA: 0x0032587C File Offset: 0x00323A7C
		public List<int> GetAvailableSpawnPositions()
		{
			PlantBranchGrower.Instance.spawn_choices.Clear();
			int cell = Grid.PosToCell(this);
			for (int i = 0; i < base.def.BRANCH_OFFSETS.Length; i++)
			{
				int cell2 = Grid.OffsetCell(cell, base.def.BRANCH_OFFSETS[i]);
				if (this.GetBranch(i) == null && this.CanBranchGrowInCell(cell2))
				{
					PlantBranchGrower.Instance.spawn_choices.Add(i);
				}
			}
			return PlantBranchGrower.Instance.spawn_choices;
		}

		// Token: 0x06007A3F RID: 31295 RVA: 0x003258F4 File Offset: 0x00323AF4
		public void RefreshBranchZPositionOffset(GameObject _branch)
		{
			if (this.branches != null)
			{
				for (int i = 0; i < this.branches.Length; i++)
				{
					GameObject branch = this.GetBranch(i);
					if (branch != null && branch == _branch)
					{
						Vector3 position = branch.transform.position;
						position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront) - 0.8f / (float)this.branches.Length * (float)i;
						branch.transform.SetPosition(position);
					}
				}
			}
		}

		// Token: 0x06007A40 RID: 31296 RVA: 0x00325970 File Offset: 0x00323B70
		public bool SpawnRandomBranch(float growth_percentage = 0f)
		{
			if (this.IsUprooted)
			{
				return false;
			}
			if (this.CurrentBranchCount >= this.MaxBranchesAllowedAtOnce)
			{
				return false;
			}
			List<int> availableSpawnPositions = this.GetAvailableSpawnPositions();
			availableSpawnPositions.Shuffle<int>();
			if (availableSpawnPositions.Count > 0)
			{
				int idx = availableSpawnPositions[0];
				PlantBranch.Instance instance = this.SpawnBranchAtIndex(idx);
				IManageGrowingStates manageGrowingStates = instance.GetComponent<IManageGrowingStates>();
				manageGrowingStates = ((manageGrowingStates != null) ? manageGrowingStates : instance.gameObject.GetSMI<IManageGrowingStates>());
				if (manageGrowingStates != null)
				{
					manageGrowingStates.OverrideMaturityLevel(growth_percentage);
				}
				instance.StartSM();
				base.gameObject.Trigger(-1586842875, instance);
				Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchSpawned = base.def.onBranchSpawned;
				if (onBranchSpawned != null)
				{
					onBranchSpawned(instance, this);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06007A41 RID: 31297 RVA: 0x00325A14 File Offset: 0x00323C14
		private PlantBranch.Instance SpawnBranchAtIndex(int idx)
		{
			if (idx < 0 || idx >= this.branches.Length)
			{
				return null;
			}
			GameObject branch = this.GetBranch(idx);
			if (branch != null)
			{
				return branch.GetSMI<PlantBranch.Instance>();
			}
			Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), base.def.BRANCH_OFFSETS[idx]), Grid.SceneLayer.BuildingFront);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(base.def.BRANCH_PREFAB_NAME), position);
			gameObject.SetActive(true);
			PlantBranch.Instance smi = gameObject.GetSMI<PlantBranch.Instance>();
			MutantPlant component = base.GetComponent<MutantPlant>();
			if (component != null)
			{
				MutantPlant component2 = smi.GetComponent<MutantPlant>();
				if (component2 != null)
				{
					component.CopyMutationsTo(component2);
					PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = component2.GetSubSpeciesInfo();
					PlantSubSpeciesCatalog.Instance.DiscoverSubSpecies(subSpeciesInfo, component2);
					PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(subSpeciesInfo.ID);
				}
			}
			this.UpdateAutoHarvestValue(smi);
			smi.SetTrunk(this);
			this.branches[idx] = new Ref<KPrefabID>();
			this.branches[idx].Set(smi.GetComponent<KPrefabID>());
			return smi;
		}

		// Token: 0x06007A42 RID: 31298 RVA: 0x00325B18 File Offset: 0x00323D18
		private bool CanBranchGrowInCell(int cell)
		{
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			if (Grid.Solid[cell])
			{
				return false;
			}
			if (Grid.Objects[cell, 1] != null)
			{
				return false;
			}
			if (Grid.Objects[cell, 5] != null)
			{
				return false;
			}
			if (Grid.Foundation[cell])
			{
				return false;
			}
			int cell2 = Grid.CellAbove(cell);
			return Grid.IsValidCell(cell2) && !Grid.IsSubstantialLiquid(cell2, 0.35f) && (base.def.additionalBranchGrowRequirements == null || base.def.additionalBranchGrowRequirements(cell));
		}

		// Token: 0x06007A43 RID: 31299 RVA: 0x00325BBC File Offset: 0x00323DBC
		public void UpdateAutoHarvestValue(PlantBranch.Instance specificBranch = null)
		{
			HarvestDesignatable component = base.GetComponent<HarvestDesignatable>();
			if (component != null && this.branches != null)
			{
				if (specificBranch != null)
				{
					HarvestDesignatable component2 = specificBranch.GetComponent<HarvestDesignatable>();
					if (component2 != null)
					{
						component2.SetHarvestWhenReady(component.HarvestWhenReady);
					}
					return;
				}
				if (base.def.propagateHarvestDesignation)
				{
					for (int i = 0; i < this.branches.Length; i++)
					{
						GameObject branch = this.GetBranch(i);
						if (branch != null)
						{
							HarvestDesignatable component3 = branch.GetComponent<HarvestDesignatable>();
							if (component3 != null)
							{
								component3.SetHarvestWhenReady(component.HarvestWhenReady);
							}
						}
					}
				}
			}
		}

		// Token: 0x04005BFA RID: 23546
		private IManageGrowingStates growing;

		// Token: 0x04005BFB RID: 23547
		[MyCmpGet]
		private UprootedMonitor uprootMonitor;

		// Token: 0x04005BFC RID: 23548
		[Serialize]
		private Ref<KPrefabID>[] branches;

		// Token: 0x04005BFD RID: 23549
		private static List<int> spawn_choices = new List<int>();
	}
}
