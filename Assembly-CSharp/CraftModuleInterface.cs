using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001936 RID: 6454
[SerializationConfig(MemberSerialization.OptIn)]
public class CraftModuleInterface : KMonoBehaviour, ISim4000ms
{
	// Token: 0x170008A8 RID: 2216
	// (get) Token: 0x06008617 RID: 34327 RVA: 0x000FC938 File Offset: 0x000FAB38
	public IList<Ref<RocketModuleCluster>> ClusterModules
	{
		get
		{
			return this.clusterModules;
		}
	}

	// Token: 0x06008618 RID: 34328 RVA: 0x000FC940 File Offset: 0x000FAB40
	public LaunchPad GetPreferredLaunchPadForWorld(int world_id)
	{
		if (this.preferredLaunchPad.ContainsKey(world_id))
		{
			return this.preferredLaunchPad[world_id].Get();
		}
		return null;
	}

	// Token: 0x06008619 RID: 34329 RVA: 0x003583D4 File Offset: 0x003565D4
	private void SetPreferredLaunchPadForWorld(LaunchPad pad)
	{
		if (!this.preferredLaunchPad.ContainsKey(pad.GetMyWorldId()))
		{
			this.preferredLaunchPad.Add(this.CurrentPad.GetMyWorldId(), new Ref<LaunchPad>());
		}
		this.preferredLaunchPad[this.CurrentPad.GetMyWorldId()].Set(this.CurrentPad);
	}

	// Token: 0x170008A9 RID: 2217
	// (get) Token: 0x0600861A RID: 34330 RVA: 0x00358430 File Offset: 0x00356630
	public LaunchPad CurrentPad
	{
		get
		{
			if (this.m_clustercraft != null && this.m_clustercraft.Status != Clustercraft.CraftStatus.InFlight && this.clusterModules.Count > 0)
			{
				if (this.bottomModule == null)
				{
					this.SetBottomModule();
				}
				global::Debug.Assert(this.bottomModule != null && this.bottomModule.Get() != null, "More than one cluster module but no bottom module found.");
				int num = Grid.CellBelow(Grid.PosToCell(this.bottomModule.Get().transform.position));
				if (Grid.IsValidCell(num))
				{
					GameObject gameObject = null;
					Grid.ObjectLayers[1].TryGetValue(num, out gameObject);
					if (gameObject != null)
					{
						return gameObject.GetComponent<LaunchPad>();
					}
				}
			}
			return null;
		}
	}

	// Token: 0x170008AA RID: 2218
	// (get) Token: 0x0600861B RID: 34331 RVA: 0x000FC963 File Offset: 0x000FAB63
	public float Speed
	{
		get
		{
			return this.m_clustercraft.Speed;
		}
	}

	// Token: 0x170008AB RID: 2219
	// (get) Token: 0x0600861C RID: 34332 RVA: 0x003584EC File Offset: 0x003566EC
	public float Range
	{
		get
		{
			float num = 0f;
			RocketEngineCluster engine = this.GetEngine();
			if (engine != null)
			{
				num = this.BurnableMassRemaining / engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance;
			}
			bool flag;
			RocketModuleCluster primaryPilotModule = this.GetPrimaryPilotModule(out flag);
			if (flag)
			{
				num = Mathf.Min(primaryPilotModule.GetComponent<RoboPilotModule>().GetDataBankRange(), num);
			}
			return num;
		}
	}

	// Token: 0x170008AC RID: 2220
	// (get) Token: 0x0600861D RID: 34333 RVA: 0x000FC970 File Offset: 0x000FAB70
	public int RangeInTiles
	{
		get
		{
			return (int)Mathf.Floor((this.Range + 0.001f) / 600f);
		}
	}

	// Token: 0x170008AD RID: 2221
	// (get) Token: 0x0600861E RID: 34334 RVA: 0x00358548 File Offset: 0x00356748
	public float FuelPerHex
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (engine != null)
			{
				return engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance * 600f;
			}
			return float.PositiveInfinity;
		}
	}

	// Token: 0x170008AE RID: 2222
	// (get) Token: 0x0600861F RID: 34335 RVA: 0x00358584 File Offset: 0x00356784
	public float BurnableMassRemaining
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (!(engine != null))
			{
				return 0f;
			}
			if (!engine.requireOxidizer)
			{
				return this.FuelRemaining;
			}
			return Mathf.Min(this.FuelRemaining, this.OxidizerPowerRemaining);
		}
	}

	// Token: 0x170008AF RID: 2223
	// (get) Token: 0x06008620 RID: 34336 RVA: 0x003585C8 File Offset: 0x003567C8
	public float FuelRemaining
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (engine == null)
			{
				return 0f;
			}
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
			{
				IFuelTank component = @ref.Get().GetComponent<IFuelTank>();
				if (!component.IsNullOrDestroyed())
				{
					num += component.Storage.GetAmountAvailable(engine.fuelTag);
				}
			}
			return (float)Mathf.CeilToInt(num);
		}
	}

	// Token: 0x170008B0 RID: 2224
	// (get) Token: 0x06008621 RID: 34337 RVA: 0x00358660 File Offset: 0x00356860
	public float OxidizerPowerRemaining
	{
		get
		{
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
			{
				OxidizerTank component = @ref.Get().GetComponent<OxidizerTank>();
				if (component != null)
				{
					num += component.TotalOxidizerPower;
				}
			}
			return (float)Mathf.CeilToInt(num);
		}
	}

	// Token: 0x170008B1 RID: 2225
	// (get) Token: 0x06008622 RID: 34338 RVA: 0x003586D8 File Offset: 0x003568D8
	public int MaxHeight
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (engine != null)
			{
				return engine.maxHeight;
			}
			return -1;
		}
	}

	// Token: 0x170008B2 RID: 2226
	// (get) Token: 0x06008623 RID: 34339 RVA: 0x000FC98A File Offset: 0x000FAB8A
	public float TotalBurden
	{
		get
		{
			return this.m_clustercraft.TotalBurden;
		}
	}

	// Token: 0x170008B3 RID: 2227
	// (get) Token: 0x06008624 RID: 34340 RVA: 0x000FC997 File Offset: 0x000FAB97
	public float EnginePower
	{
		get
		{
			return this.m_clustercraft.EnginePower;
		}
	}

	// Token: 0x170008B4 RID: 2228
	// (get) Token: 0x06008625 RID: 34341 RVA: 0x00358700 File Offset: 0x00356900
	public int RocketHeight
	{
		get
		{
			int num = 0;
			foreach (Ref<RocketModuleCluster> @ref in this.ClusterModules)
			{
				num += @ref.Get().GetComponent<Building>().Def.HeightInCells;
			}
			return num;
		}
	}

	// Token: 0x170008B5 RID: 2229
	// (get) Token: 0x06008626 RID: 34342 RVA: 0x00358764 File Offset: 0x00356964
	public bool HasCargoModule
	{
		get
		{
			using (IEnumerator<Ref<RocketModuleCluster>> enumerator = this.ClusterModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Get().GetComponent<CargoBayCluster>() != null)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x06008627 RID: 34343 RVA: 0x000FC9A4 File Offset: 0x000FABA4
	protected override void OnPrefabInit()
	{
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Combine(instance.OnLoad, new Action<Game.GameSaveData>(this.OnLoad));
	}

	// Token: 0x06008628 RID: 34344 RVA: 0x003587BC File Offset: 0x003569BC
	protected override void OnSpawn()
	{
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Remove(instance.OnLoad, new Action<Game.GameSaveData>(this.OnLoad));
		if (this.m_clustercraft.Status != Clustercraft.CraftStatus.Grounded)
		{
			this.ForceAttachmentNetwork();
		}
		this.SetBottomModule();
		base.Subscribe(-1311384361, new Action<object>(this.CompleteSelfDestruct));
	}

	// Token: 0x06008629 RID: 34345 RVA: 0x00358820 File Offset: 0x00356A20
	private void OnLoad(Game.GameSaveData data)
	{
		foreach (Ref<RocketModule> @ref in this.modules)
		{
			this.clusterModules.Add(new Ref<RocketModuleCluster>(@ref.Get().GetComponent<RocketModuleCluster>()));
		}
		this.modules.Clear();
		foreach (Ref<RocketModuleCluster> ref2 in this.clusterModules)
		{
			if (!(ref2.Get() == null))
			{
				ref2.Get().CraftInterface = this;
			}
		}
		bool flag = false;
		for (int i = this.clusterModules.Count - 1; i >= 0; i--)
		{
			if (this.clusterModules[i] == null || this.clusterModules[i].Get() == null)
			{
				global::Debug.LogWarning(string.Format("Rocket {0} had a null module at index {1} on load! Why????", base.name, i), this);
				this.clusterModules.RemoveAt(i);
				flag = true;
			}
		}
		this.SetBottomModule();
		if (flag && this.m_clustercraft.Status == Clustercraft.CraftStatus.Grounded)
		{
			global::Debug.LogWarning("The module stack was broken. Collapsing " + base.name + "...", this);
			this.SortModuleListByPosition();
			LaunchPad currentPad = this.CurrentPad;
			if (currentPad != null)
			{
				int num = currentPad.RocketBottomPosition;
				for (int j = 0; j < this.clusterModules.Count; j++)
				{
					RocketModuleCluster rocketModuleCluster = this.clusterModules[j].Get();
					if (num != Grid.PosToCell(rocketModuleCluster.transform.GetPosition()))
					{
						global::Debug.LogWarning(string.Format("Collapsing space under module {0}:{1}", j, rocketModuleCluster.name));
						rocketModuleCluster.transform.SetPosition(Grid.CellToPos(num, CellAlignment.Bottom, Grid.SceneLayer.Building));
					}
					num = Grid.OffsetCell(num, 0, this.clusterModules[j].Get().GetComponent<Building>().Def.HeightInCells);
				}
			}
			for (int k = 0; k < this.clusterModules.Count - 1; k++)
			{
				BuildingAttachPoint component = this.clusterModules[k].Get().GetComponent<BuildingAttachPoint>();
				if (component != null)
				{
					AttachableBuilding component2 = this.clusterModules[k + 1].Get().GetComponent<AttachableBuilding>();
					if (component.points[0].attachedBuilding != component2)
					{
						global::Debug.LogWarning("Reattaching " + component.name + " & " + component2.name);
						component.points[0].attachedBuilding = component2;
					}
				}
			}
		}
	}

	// Token: 0x0600862A RID: 34346 RVA: 0x00358B14 File Offset: 0x00356D14
	public void AddModule(RocketModuleCluster newModule)
	{
		for (int i = 0; i < this.clusterModules.Count; i++)
		{
			if (this.clusterModules[i].Get() == newModule)
			{
				global::Debug.LogError(string.Concat(new string[]
				{
					"Adding module ",
					(newModule != null) ? newModule.ToString() : null,
					" to the same rocket (",
					this.m_clustercraft.Name,
					") twice"
				}));
			}
		}
		this.clusterModules.Add(new Ref<RocketModuleCluster>(newModule));
		newModule.CraftInterface = this;
		base.Trigger(1512695988, newModule);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			if (rocketModuleCluster != null && rocketModuleCluster != newModule)
			{
				rocketModuleCluster.Trigger(1512695988, newModule);
			}
		}
		newModule.Trigger(1512695988, newModule);
		this.SetBottomModule();
	}

	// Token: 0x0600862B RID: 34347 RVA: 0x00358C30 File Offset: 0x00356E30
	public void RemoveModule(RocketModuleCluster module)
	{
		for (int i = this.clusterModules.Count - 1; i >= 0; i--)
		{
			if (this.clusterModules[i].Get() == module)
			{
				this.clusterModules.RemoveAt(i);
				break;
			}
		}
		base.Trigger(1512695988, null);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger(1512695988, null);
		}
		this.SetBottomModule();
		if (this.clusterModules.Count == 0)
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x0600862C RID: 34348 RVA: 0x000FC9CC File Offset: 0x000FABCC
	private void SortModuleListByPosition()
	{
		this.clusterModules.Sort(delegate(Ref<RocketModuleCluster> a, Ref<RocketModuleCluster> b)
		{
			if (Grid.CellToPos(Grid.PosToCell(a.Get())).y >= Grid.CellToPos(Grid.PosToCell(b.Get())).y)
			{
				return 1;
			}
			return -1;
		});
	}

	// Token: 0x0600862D RID: 34349 RVA: 0x00358CF4 File Offset: 0x00356EF4
	private void SetBottomModule()
	{
		if (this.clusterModules.Count > 0)
		{
			this.bottomModule = this.clusterModules[0];
			Vector3 vector = this.bottomModule.Get().transform.position;
			using (List<Ref<RocketModuleCluster>>.Enumerator enumerator = this.clusterModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ref<RocketModuleCluster> @ref = enumerator.Current;
					Vector3 position = @ref.Get().transform.position;
					if (position.y < vector.y)
					{
						this.bottomModule = @ref;
						vector = position;
					}
				}
				return;
			}
		}
		this.bottomModule = null;
	}

	// Token: 0x0600862E RID: 34350 RVA: 0x00358DA8 File Offset: 0x00356FA8
	public int GetHeightOfModuleTop(GameObject module)
	{
		int num = 0;
		for (int i = 0; i < this.ClusterModules.Count; i++)
		{
			num += this.clusterModules[i].Get().GetComponent<Building>().Def.HeightInCells;
			if (this.clusterModules[i].Get().gameObject == module)
			{
				return num;
			}
		}
		global::Debug.LogError("Could not find module " + module.GetProperName() + " in CraftModuleInterface craft " + this.m_clustercraft.Name);
		return 0;
	}

	// Token: 0x0600862F RID: 34351 RVA: 0x00358E38 File Offset: 0x00357038
	public int GetModuleRelativeVerticalPosition(GameObject module)
	{
		int num = 0;
		for (int i = 0; i < this.ClusterModules.Count; i++)
		{
			if (this.clusterModules[i].Get().gameObject == module)
			{
				return num;
			}
			num += this.clusterModules[i].Get().GetComponent<Building>().Def.HeightInCells;
		}
		global::Debug.LogError("Could not find module " + module.GetProperName() + " in CraftModuleInterface craft " + this.m_clustercraft.Name);
		return 0;
	}

	// Token: 0x06008630 RID: 34352 RVA: 0x00358EC8 File Offset: 0x003570C8
	public void Sim4000ms(float dt)
	{
		int num = 0;
		foreach (ProcessCondition.ProcessConditionType conditionType in this.conditionsToCheck)
		{
			if (this.EvaluateConditionSet(conditionType) != ProcessCondition.Status.Failure)
			{
				num++;
			}
		}
		if (num != this.lastConditionTypeSucceeded)
		{
			this.lastConditionTypeSucceeded = num;
			this.TriggerEventOnCraftAndRocket(GameHashes.LaunchConditionChanged, null);
		}
	}

	// Token: 0x06008631 RID: 34353 RVA: 0x000FC9F8 File Offset: 0x000FABF8
	public bool IsLaunchRequested()
	{
		return this.m_clustercraft.LaunchRequested;
	}

	// Token: 0x06008632 RID: 34354 RVA: 0x000FCA05 File Offset: 0x000FAC05
	public bool CheckPreppedForLaunch()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketFlight) > ProcessCondition.Status.Failure;
	}

	// Token: 0x06008633 RID: 34355 RVA: 0x000FCA25 File Offset: 0x000FAC25
	public bool CheckReadyToLaunch()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketFlight) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketBoard) > ProcessCondition.Status.Failure;
	}

	// Token: 0x06008634 RID: 34356 RVA: 0x000FCA4E File Offset: 0x000FAC4E
	public bool HasLaunchWarnings()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) == ProcessCondition.Status.Warning || this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) == ProcessCondition.Status.Warning || this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketBoard) == ProcessCondition.Status.Warning;
	}

	// Token: 0x06008635 RID: 34357 RVA: 0x000FCA70 File Offset: 0x000FAC70
	public bool CheckReadyForAutomatedLaunchCommand()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) == ProcessCondition.Status.Ready && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) == ProcessCondition.Status.Ready;
	}

	// Token: 0x06008636 RID: 34358 RVA: 0x000FCA88 File Offset: 0x000FAC88
	public bool CheckReadyForAutomatedLaunch()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) == ProcessCondition.Status.Ready && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) == ProcessCondition.Status.Ready && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketBoard) == ProcessCondition.Status.Ready;
	}

	// Token: 0x06008637 RID: 34359 RVA: 0x00358F40 File Offset: 0x00357140
	public void TriggerEventOnCraftAndRocket(GameHashes evt, object data)
	{
		base.Trigger((int)evt, data);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger((int)evt, data);
		}
	}

	// Token: 0x06008638 RID: 34360 RVA: 0x000FCAAA File Offset: 0x000FACAA
	public void CancelLaunch()
	{
		this.m_clustercraft.CancelLaunch();
	}

	// Token: 0x06008639 RID: 34361 RVA: 0x000FCAB7 File Offset: 0x000FACB7
	public void TriggerLaunch(bool automated = false)
	{
		this.m_clustercraft.RequestLaunch(automated);
	}

	// Token: 0x0600863A RID: 34362 RVA: 0x00358FA0 File Offset: 0x003571A0
	public void DoLaunch()
	{
		this.SortModuleListByPosition();
		this.CurrentPad.Trigger(705820818, this);
		this.SetPreferredLaunchPadForWorld(this.CurrentPad);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger(705820818, this);
		}
	}

	// Token: 0x0600863B RID: 34363 RVA: 0x00359020 File Offset: 0x00357220
	public void DoLand(LaunchPad pad)
	{
		int num = pad.RocketBottomPosition;
		for (int i = 0; i < this.clusterModules.Count; i++)
		{
			this.clusterModules[i].Get().MoveToPad(num);
			num = Grid.OffsetCell(num, 0, this.clusterModules[i].Get().GetComponent<Building>().Def.HeightInCells);
		}
		this.SetBottomModule();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger(-1165815793, pad);
		}
		pad.Trigger(-1165815793, this);
	}

	// Token: 0x0600863C RID: 34364 RVA: 0x003590EC File Offset: 0x003572EC
	public LaunchConditionManager FindLaunchConditionManager()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			LaunchConditionManager component = @ref.Get().GetComponent<LaunchConditionManager>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x0600863D RID: 34365 RVA: 0x00359154 File Offset: 0x00357354
	public LaunchableRocketCluster FindLaunchableRocket()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			LaunchableRocketCluster component = rocketModuleCluster.GetComponent<LaunchableRocketCluster>();
			if (component != null && rocketModuleCluster.CraftInterface != null && rocketModuleCluster.CraftInterface.GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.Grounded)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x0600863E RID: 34366 RVA: 0x003591DC File Offset: 0x003573DC
	public List<GameObject> GetParts()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			list.Add(@ref.Get().gameObject);
		}
		return list;
	}

	// Token: 0x0600863F RID: 34367 RVA: 0x00359240 File Offset: 0x00357440
	public RocketEngineCluster GetEngine()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketEngineCluster component = @ref.Get().GetComponent<RocketEngineCluster>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x06008640 RID: 34368 RVA: 0x003592A8 File Offset: 0x003574A8
	public RocketModuleCluster GetPrimaryPilotModule(out bool is_robo_pilot)
	{
		is_robo_pilot = false;
		RocketModuleCluster result = null;
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			if (rocketModuleCluster.GetComponent<PassengerRocketModule>() != null)
			{
				result = rocketModuleCluster;
				is_robo_pilot = false;
				break;
			}
			if (rocketModuleCluster.GetComponent<RoboPilotModule>())
			{
				is_robo_pilot = true;
				result = rocketModuleCluster;
			}
		}
		return result;
	}

	// Token: 0x06008641 RID: 34369 RVA: 0x00359328 File Offset: 0x00357528
	public PassengerRocketModule GetPassengerModule()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			PassengerRocketModule component = @ref.Get().GetComponent<PassengerRocketModule>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x06008642 RID: 34370 RVA: 0x00359390 File Offset: 0x00357590
	public WorldContainer GetInteriorWorld()
	{
		PassengerRocketModule passengerModule = this.GetPassengerModule();
		if (passengerModule == null)
		{
			return null;
		}
		ClustercraftInteriorDoor interiorDoor = passengerModule.GetComponent<ClustercraftExteriorDoor>().GetInteriorDoor();
		if (interiorDoor == null)
		{
			return null;
		}
		return interiorDoor.GetMyWorld();
	}

	// Token: 0x06008643 RID: 34371 RVA: 0x003593CC File Offset: 0x003575CC
	public RoboPilotModule GetRobotPilotModule()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RoboPilotModule component = @ref.Get().GetComponent<RoboPilotModule>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x06008644 RID: 34372 RVA: 0x000FCAC5 File Offset: 0x000FACC5
	public RocketClusterDestinationSelector GetClusterDestinationSelector()
	{
		return base.GetComponent<RocketClusterDestinationSelector>();
	}

	// Token: 0x06008645 RID: 34373 RVA: 0x000FCACD File Offset: 0x000FACCD
	public bool HasClusterDestinationSelector()
	{
		return base.GetComponent<RocketClusterDestinationSelector>() != null;
	}

	// Token: 0x06008646 RID: 34374 RVA: 0x00359434 File Offset: 0x00357634
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		this.returnConditions.Clear();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			List<ProcessCondition> conditionSet = @ref.Get().GetConditionSet(conditionType);
			if (conditionSet != null)
			{
				this.returnConditions.AddRange(conditionSet);
			}
		}
		if (this.CurrentPad != null)
		{
			List<ProcessCondition> conditionSet2 = this.CurrentPad.GetComponent<LaunchPadConditions>().GetConditionSet(conditionType);
			if (conditionSet2 != null)
			{
				this.returnConditions.AddRange(conditionSet2);
			}
		}
		return this.returnConditions;
	}

	// Token: 0x06008647 RID: 34375 RVA: 0x003594DC File Offset: 0x003576DC
	private ProcessCondition.Status EvaluateConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		ProcessCondition.Status status = ProcessCondition.Status.Ready;
		foreach (ProcessCondition processCondition in this.GetConditionSet(conditionType))
		{
			ProcessCondition.Status status2 = processCondition.EvaluateCondition();
			if (status2 < status)
			{
				status = status2;
			}
			if (status == ProcessCondition.Status.Failure)
			{
				break;
			}
		}
		return status;
	}

	// Token: 0x06008648 RID: 34376 RVA: 0x0035953C File Offset: 0x0035773C
	private void ForceAttachmentNetwork()
	{
		RocketModuleCluster rocketModuleCluster = null;
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketModuleCluster rocketModuleCluster2 = @ref.Get();
			if (rocketModuleCluster != null)
			{
				BuildingAttachPoint component = rocketModuleCluster.GetComponent<BuildingAttachPoint>();
				AttachableBuilding component2 = rocketModuleCluster2.GetComponent<AttachableBuilding>();
				component.points[0].attachedBuilding = component2;
			}
			rocketModuleCluster = rocketModuleCluster2;
		}
	}

	// Token: 0x06008649 RID: 34377 RVA: 0x003595B8 File Offset: 0x003577B8
	public static Storage SpawnRocketDebris(string nameSuffix, SimHashes element)
	{
		Vector3 position = new Vector3(-1f, -1f, 0f);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("DebrisPayload"), position);
		gameObject.GetComponent<PrimaryElement>().SetElement(element, true);
		gameObject.name += nameSuffix;
		gameObject.SetActive(true);
		return gameObject.GetComponent<Storage>();
	}

	// Token: 0x0600864A RID: 34378 RVA: 0x0035961C File Offset: 0x0035781C
	public void CompleteSelfDestruct(object data = null)
	{
		global::Debug.Assert(this.HasTag(GameTags.RocketInSpace), "Self Destruct is only valid for in-space rockets!");
		List<RocketModule> list = new List<RocketModule>();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			list.Add(@ref.Get());
		}
		List<GameObject> list2 = new List<GameObject>();
		List<GameObject> list3 = new List<GameObject>();
		foreach (RocketModule rocketModule in list)
		{
			foreach (Storage storage in rocketModule.GetComponents<Storage>())
			{
				bool vent_gas = false;
				bool dump_liquid = false;
				List<GameObject> collect_dropped_items = list3;
				storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
				foreach (GameObject gameObject in list3)
				{
					if (gameObject.HasTag(GameTags.Creature))
					{
						Butcherable component = gameObject.GetComponent<Butcherable>();
						if (component != null && component.drops != null && component.drops.Length != 0)
						{
							GameObject[] collection = component.CreateDrops();
							list2.AddRange(collection);
						}
						gameObject.DeleteObject();
					}
					else
					{
						list2.Add(gameObject);
					}
				}
				list3.Clear();
			}
			Deconstructable component2 = rocketModule.GetComponent<Deconstructable>();
			list2.AddRange(component2.ForceDestroyAndGetMaterials());
		}
		bool flag;
		SimHashes elementID = this.GetPrimaryPilotModule(out flag).GetComponent<PrimaryElement>().ElementID;
		List<Storage> list4 = new List<Storage>();
		foreach (GameObject gameObject2 in list2)
		{
			Pickupable component3 = gameObject2.GetComponent<Pickupable>();
			if (component3 != null)
			{
				component3.PrimaryElement.Units = (float)Mathf.Max(1, Mathf.RoundToInt(component3.PrimaryElement.Units * 0.5f));
				if ((list4.Count == 0 || list4[list4.Count - 1].RemainingCapacity() == 0f) && component3.PrimaryElement.Mass > 0f)
				{
					list4.Add(CraftModuleInterface.SpawnRocketDebris(" from CMI", elementID));
				}
				Storage storage2 = list4[list4.Count - 1];
				while (component3.PrimaryElement.Mass > storage2.RemainingCapacity())
				{
					Pickupable pickupable = component3.Take(storage2.RemainingCapacity());
					storage2.Store(pickupable.gameObject, false, false, true, false);
					storage2 = CraftModuleInterface.SpawnRocketDebris(" from CMI", elementID);
					list4.Add(storage2);
				}
				if (component3.PrimaryElement.Mass > 0f)
				{
					storage2.Store(component3.gameObject, false, false, true, false);
				}
			}
		}
		foreach (Storage cmp in list4)
		{
			RailGunPayload.StatesInstance smi = cmp.GetSMI<RailGunPayload.StatesInstance>();
			smi.StartSM();
			smi.Travel(this.m_clustercraft.Location, ClusterUtil.ClosestVisibleAsteroidToLocation(this.m_clustercraft.Location).Location);
		}
		this.m_clustercraft.SetExploding();
	}

	// Token: 0x040065CC RID: 26060
	[Serialize]
	private List<Ref<RocketModule>> modules = new List<Ref<RocketModule>>();

	// Token: 0x040065CD RID: 26061
	[Serialize]
	private List<Ref<RocketModuleCluster>> clusterModules = new List<Ref<RocketModuleCluster>>();

	// Token: 0x040065CE RID: 26062
	private Ref<RocketModuleCluster> bottomModule;

	// Token: 0x040065CF RID: 26063
	[Serialize]
	private Dictionary<int, Ref<LaunchPad>> preferredLaunchPad = new Dictionary<int, Ref<LaunchPad>>();

	// Token: 0x040065D0 RID: 26064
	[MyCmpReq]
	private Clustercraft m_clustercraft;

	// Token: 0x040065D1 RID: 26065
	private List<ProcessCondition.ProcessConditionType> conditionsToCheck = new List<ProcessCondition.ProcessConditionType>
	{
		ProcessCondition.ProcessConditionType.RocketPrep,
		ProcessCondition.ProcessConditionType.RocketStorage,
		ProcessCondition.ProcessConditionType.RocketBoard,
		ProcessCondition.ProcessConditionType.RocketFlight
	};

	// Token: 0x040065D2 RID: 26066
	private int lastConditionTypeSucceeded = -1;

	// Token: 0x040065D3 RID: 26067
	private List<ProcessCondition> returnConditions = new List<ProcessCondition>();
}
