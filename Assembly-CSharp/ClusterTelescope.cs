using System;
using System.Collections.Generic;
using Database;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000D11 RID: 3345
public class ClusterTelescope : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>
{
	// Token: 0x0600402B RID: 16427 RVA: 0x00247E84 File Offset: 0x00246084
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.ready.no_visibility;
		this.root.Update(delegate(ClusterTelescope.Instance smi, float dt)
		{
			KSelectable component = smi.GetComponent<KSelectable>();
			bool flag = Mathf.Approximately(0f, smi.PercentClear) || smi.PercentClear < 0f;
			bool flag2 = Mathf.Approximately(1f, smi.PercentClear) || smi.PercentClear > 1f;
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, flag, smi);
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, !flag && !flag2, smi);
		}, UpdateRate.SIM_200ms, false);
		this.ready.DoNothing();
		this.ready.no_visibility.UpdateTransition(this.ready.ready_to_work, (ClusterTelescope.Instance smi, float dt) => smi.HasSkyVisibility(), UpdateRate.SIM_200ms, false);
		this.ready.ready_to_work.UpdateTransition(this.ready.no_visibility, (ClusterTelescope.Instance smi, float dt) => !smi.HasSkyVisibility(), UpdateRate.SIM_200ms, false).DefaultState(this.ready.ready_to_work.decide);
		this.ready.ready_to_work.decide.EnterTransition(this.ready.ready_to_work.identifyMeteorShower, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnMeteorIdentification()).EnterTransition(this.ready.ready_to_work.revealTile, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnRevealingTile()).EnterTransition(this.all_work_complete, (ClusterTelescope.Instance smi) => !smi.IsAnyAvailableWorkToBeDone());
		this.ready.ready_to_work.identifyMeteorShower.OnSignal(this.MeteorIdenificationPriorityChangeSignal, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => !smi.ShouldBeWorkingOnMeteorIdentification()).ParamTransition<GameObject>(this.meteorShowerTarget, this.ready.ready_to_work.decide, GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.IsNull).EventTransition(GameHashes.ClusterMapMeteorShowerIdentified, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => !smi.ShouldBeWorkingOnMeteorIdentification()).EventTransition(GameHashes.ClusterMapMeteorShowerMoved, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => !smi.ShouldBeWorkingOnMeteorIdentification()).ToggleChore((ClusterTelescope.Instance smi) => smi.CreateIdentifyMeteorChore(), this.ready.no_visibility);
		this.ready.ready_to_work.revealTile.OnSignal(this.MeteorIdenificationPriorityChangeSignal, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnMeteorIdentification()).EventTransition(GameHashes.ClusterFogOfWarRevealed, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => !smi.ShouldBeWorkingOnRevealingTile()).EventTransition(GameHashes.ClusterMapMeteorShowerMoved, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.ready_to_work.decide, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnMeteorIdentification()).ToggleChore((ClusterTelescope.Instance smi) => smi.CreateRevealTileChore(), this.ready.no_visibility);
		this.all_work_complete.OnSignal(this.MeteorIdenificationPriorityChangeSignal, this.ready.no_visibility, (ClusterTelescope.Instance smi) => smi.IsAnyAvailableWorkToBeDone()).ToggleMainStatusItem(Db.Get().BuildingStatusItems.ClusterTelescopeAllWorkComplete, null).EventTransition(GameHashes.ClusterLocationChanged, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.no_visibility, (ClusterTelescope.Instance smi) => smi.IsAnyAvailableWorkToBeDone()).EventTransition(GameHashes.ClusterMapMeteorShowerMoved, (ClusterTelescope.Instance smi) => Game.Instance, this.ready.no_visibility, (ClusterTelescope.Instance smi) => smi.ShouldBeWorkingOnMeteorIdentification());
	}

	// Token: 0x04002C65 RID: 11365
	public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State all_work_complete;

	// Token: 0x04002C66 RID: 11366
	public ClusterTelescope.ReadyStates ready;

	// Token: 0x04002C67 RID: 11367
	public StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.TargetParameter meteorShowerTarget;

	// Token: 0x04002C68 RID: 11368
	public StateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.Signal MeteorIdenificationPriorityChangeSignal;

	// Token: 0x02000D12 RID: 3346
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04002C69 RID: 11369
		public int clearScanCellRadius = 15;

		// Token: 0x04002C6A RID: 11370
		public int analyzeClusterRadius = 3;

		// Token: 0x04002C6B RID: 11371
		public KAnimFile[] workableOverrideAnims;

		// Token: 0x04002C6C RID: 11372
		public bool providesOxygen;

		// Token: 0x04002C6D RID: 11373
		public SkyVisibilityInfo skyVisibilityInfo;
	}

	// Token: 0x02000D13 RID: 3347
	public class WorkStates : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State
	{
		// Token: 0x04002C6E RID: 11374
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State decide;

		// Token: 0x04002C6F RID: 11375
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State identifyMeteorShower;

		// Token: 0x04002C70 RID: 11376
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State revealTile;
	}

	// Token: 0x02000D14 RID: 3348
	public class ReadyStates : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State
	{
		// Token: 0x04002C71 RID: 11377
		public GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.State no_visibility;

		// Token: 0x04002C72 RID: 11378
		public ClusterTelescope.WorkStates ready_to_work;
	}

	// Token: 0x02000D15 RID: 3349
	public new class Instance : GameStateMachine<ClusterTelescope, ClusterTelescope.Instance, IStateMachineTarget, ClusterTelescope.Def>.GameInstance, ICheckboxControl, BuildingStatusItems.ISkyVisInfo
	{
		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06004030 RID: 16432 RVA: 0x000CE249 File Offset: 0x000CC449
		public float PercentClear
		{
			get
			{
				return this.m_percentClear;
			}
		}

		// Token: 0x06004031 RID: 16433 RVA: 0x000CE249 File Offset: 0x000CC449
		float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01()
		{
			return this.m_percentClear;
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06004032 RID: 16434 RVA: 0x000CE251 File Offset: 0x000CC451
		private bool hasMeteorShowerTarget
		{
			get
			{
				return this.meteorShowerTarget != null;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06004033 RID: 16435 RVA: 0x000CE25C File Offset: 0x000CC45C
		private ClusterMapMeteorShower.Instance meteorShowerTarget
		{
			get
			{
				GameObject gameObject = base.sm.meteorShowerTarget.Get(this);
				if (gameObject == null)
				{
					return null;
				}
				return gameObject.GetSMI<ClusterMapMeteorShower.Instance>();
			}
		}

		// Token: 0x06004034 RID: 16436 RVA: 0x000CE27A File Offset: 0x000CC47A
		public Instance(IStateMachineTarget smi, ClusterTelescope.Def def) : base(smi, def)
		{
			this.workableOverrideAnims = def.workableOverrideAnims;
			this.providesOxygen = def.providesOxygen;
		}

		// Token: 0x06004035 RID: 16437 RVA: 0x000CE2A3 File Offset: 0x000CC4A3
		public bool ShouldBeWorkingOnRevealingTile()
		{
			return this.CheckHasAnalyzeTarget() && (!this.allowMeteorIdentification || !this.CheckHasValidMeteorTarget());
		}

		// Token: 0x06004036 RID: 16438 RVA: 0x000CE2C2 File Offset: 0x000CC4C2
		public bool ShouldBeWorkingOnMeteorIdentification()
		{
			return this.allowMeteorIdentification && this.CheckHasValidMeteorTarget();
		}

		// Token: 0x06004037 RID: 16439 RVA: 0x000CE2D4 File Offset: 0x000CC4D4
		public bool IsAnyAvailableWorkToBeDone()
		{
			return this.CheckHasAnalyzeTarget() || this.ShouldBeWorkingOnMeteorIdentification();
		}

		// Token: 0x06004038 RID: 16440 RVA: 0x0024837C File Offset: 0x0024657C
		public bool CheckHasValidMeteorTarget()
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			if (this.HasValidMeteor())
			{
				return true;
			}
			ClusterMapMeteorShower.Instance instance = null;
			AxialI myWorldLocation = this.GetMyWorldLocation();
			ClusterGrid.Instance.GetVisibleUnidentifiedMeteorShowerWithinRadius(myWorldLocation, base.def.analyzeClusterRadius, out instance);
			base.sm.meteorShowerTarget.Set((instance == null) ? null : instance.gameObject, this, false);
			return instance != null;
		}

		// Token: 0x06004039 RID: 16441 RVA: 0x002483E4 File Offset: 0x002465E4
		public bool CheckHasAnalyzeTarget()
		{
			ClusterFogOfWarManager.Instance smi = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			if (this.m_hasAnalyzeTarget && !smi.IsLocationRevealed(this.m_analyzeTarget))
			{
				return true;
			}
			AxialI myWorldLocation = this.GetMyWorldLocation();
			this.m_hasAnalyzeTarget = smi.GetUnrevealedLocationWithinRadius(myWorldLocation, base.def.analyzeClusterRadius, out this.m_analyzeTarget);
			return this.m_hasAnalyzeTarget;
		}

		// Token: 0x0600403A RID: 16442 RVA: 0x00248440 File Offset: 0x00246640
		private bool HasValidMeteor()
		{
			if (!this.hasMeteorShowerTarget)
			{
				return false;
			}
			AxialI myWorldLocation = this.GetMyWorldLocation();
			bool flag = ClusterGrid.Instance.IsInRange(this.meteorShowerTarget.ClusterGridPosition(), myWorldLocation, base.def.analyzeClusterRadius);
			bool flag2 = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().IsLocationRevealed(this.meteorShowerTarget.ClusterGridPosition());
			bool hasBeenIdentified = this.meteorShowerTarget.HasBeenIdentified;
			return flag && flag2 && !hasBeenIdentified;
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x002484B8 File Offset: 0x002466B8
		public Chore CreateRevealTileChore()
		{
			WorkChore<ClusterTelescope.ClusterTelescopeWorkable> workChore = new WorkChore<ClusterTelescope.ClusterTelescopeWorkable>(Db.Get().ChoreTypes.Research, this.m_workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			if (this.providesOxygen)
			{
				workChore.AddPrecondition(Telescope.ContainsOxygen, null);
			}
			return workChore;
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x00248508 File Offset: 0x00246708
		public Chore CreateIdentifyMeteorChore()
		{
			WorkChore<ClusterTelescope.ClusterTelescopeIdentifyMeteorWorkable> workChore = new WorkChore<ClusterTelescope.ClusterTelescopeIdentifyMeteorWorkable>(Db.Get().ChoreTypes.Research, this.m_identifyMeteorWorkable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			if (this.providesOxygen)
			{
				workChore.AddPrecondition(Telescope.ContainsOxygen, null);
			}
			return workChore;
		}

		// Token: 0x0600403D RID: 16445 RVA: 0x000CE2E6 File Offset: 0x000CC4E6
		public ClusterMapMeteorShower.Instance GetMeteorTarget()
		{
			return this.meteorShowerTarget;
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x000CE2EE File Offset: 0x000CC4EE
		public AxialI GetAnalyzeTarget()
		{
			global::Debug.Assert(this.m_hasAnalyzeTarget, "GetAnalyzeTarget called but this telescope has no target assigned.");
			return this.m_analyzeTarget;
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x00248558 File Offset: 0x00246758
		public bool HasSkyVisibility()
		{
			ValueTuple<bool, float> visibilityOf = base.def.skyVisibilityInfo.GetVisibilityOf(base.gameObject);
			bool item = visibilityOf.Item1;
			float item2 = visibilityOf.Item2;
			this.m_percentClear = item2;
			return item;
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06004040 RID: 16448 RVA: 0x000CE306 File Offset: 0x000CC506
		public string CheckboxTitleKey
		{
			get
			{
				return "STRINGS.UI.UISIDESCREENS.CLUSTERTELESCOPESIDESCREEN.TITLE";
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06004041 RID: 16449 RVA: 0x000CE30D File Offset: 0x000CC50D
		public string CheckboxLabel
		{
			get
			{
				return UI.UISIDESCREENS.CLUSTERTELESCOPESIDESCREEN.CHECKBOX_METEORS;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06004042 RID: 16450 RVA: 0x000CE319 File Offset: 0x000CC519
		public string CheckboxTooltip
		{
			get
			{
				return UI.UISIDESCREENS.CLUSTERTELESCOPESIDESCREEN.CHECKBOX_TOOLTIP_METEORS;
			}
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x000CE325 File Offset: 0x000CC525
		public bool GetCheckboxValue()
		{
			return this.allowMeteorIdentification;
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x000CE32D File Offset: 0x000CC52D
		public void SetCheckboxValue(bool value)
		{
			this.allowMeteorIdentification = value;
			base.sm.MeteorIdenificationPriorityChangeSignal.Trigger(this);
		}

		// Token: 0x04002C73 RID: 11379
		private float m_percentClear;

		// Token: 0x04002C74 RID: 11380
		[Serialize]
		public bool allowMeteorIdentification = true;

		// Token: 0x04002C75 RID: 11381
		[Serialize]
		private bool m_hasAnalyzeTarget;

		// Token: 0x04002C76 RID: 11382
		[Serialize]
		private AxialI m_analyzeTarget;

		// Token: 0x04002C77 RID: 11383
		[MyCmpAdd]
		private ClusterTelescope.ClusterTelescopeWorkable m_workable;

		// Token: 0x04002C78 RID: 11384
		[MyCmpAdd]
		private ClusterTelescope.ClusterTelescopeIdentifyMeteorWorkable m_identifyMeteorWorkable;

		// Token: 0x04002C79 RID: 11385
		public KAnimFile[] workableOverrideAnims;

		// Token: 0x04002C7A RID: 11386
		public bool providesOxygen;
	}

	// Token: 0x02000D16 RID: 3350
	public class ClusterTelescopeWorkable : Workable, OxygenBreather.IGasProvider
	{
		// Token: 0x06004045 RID: 16453 RVA: 0x00248590 File Offset: 0x00246790
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
			this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
			this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
			this.requiredSkillPerk = Db.Get().SkillPerks.CanUseClusterTelescope.Id;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.radiationShielding = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, FIXEDTRAITS.COSMICRADIATION.TELESCOPE_RADIATION_SHIELDING, STRINGS.BUILDINGS.PREFABS.CLUSTERTELESCOPEENCLOSED.NAME, false, false, true);
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x000CE347 File Offset: 0x000CC547
		protected override void OnCleanUp()
		{
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.OnCleanUp();
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x0024863C File Offset: 0x0024683C
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(this.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
			this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			base.SetWorkTime(float.PositiveInfinity);
			this.overrideAnims = this.m_telescope.workableOverrideAnims;
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x002486A0 File Offset: 0x002468A0
		private void OnWorkableEvent(Workable workable, Workable.WorkableEvent ev)
		{
			WorkerBase worker = base.worker;
			if (worker == null)
			{
				return;
			}
			KPrefabID component = worker.GetComponent<KPrefabID>();
			OxygenBreather component2 = worker.GetComponent<OxygenBreather>();
			Klei.AI.Attributes attributes = worker.GetAttributes();
			KSelectable component3 = base.GetComponent<KSelectable>();
			if (ev == Workable.WorkableEvent.WorkStarted)
			{
				base.ShowProgressBar(true);
				this.progressBar.SetUpdateFunc(() => this.m_fowManager.GetRevealCompleteFraction(this.currentTarget));
				this.currentTarget = this.m_telescope.GetAnalyzeTarget();
				if (!ClusterGrid.Instance.GetEntityOfLayerAtCell(this.currentTarget, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(this.currentTarget);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().SetTargetMeteorShower(null);
				}
				if (this.m_telescope.providesOxygen)
				{
					attributes.Add(this.radiationShielding);
					if (component2 != null)
					{
						component2.AddGasProvider(this);
					}
					worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
					component.AddTag(GameTags.Shaded, false);
				}
				base.GetComponent<Operational>().SetActive(true, false);
				this.checkMarkerFrequency = UnityEngine.Random.Range(2f, 5f);
				component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
				return;
			}
			if (ev != Workable.WorkableEvent.WorkStopped)
			{
				return;
			}
			if (this.m_telescope.providesOxygen)
			{
				attributes.Remove(this.radiationShielding);
				if (component2 != null)
				{
					component2.RemoveGasProvider(this);
				}
				worker.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
				component.RemoveTag(GameTags.Shaded);
			}
			base.GetComponent<Operational>().SetActive(false, false);
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.ShowProgressBar(false);
			component3.RemoveStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x00248884 File Offset: 0x00246A84
		public override List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> descriptors = base.GetDescriptors(go);
			Element element = ElementLoader.FindElementByHash(SimHashes.Oxygen);
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(element.tag.ProperName(), string.Format(STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, element.tag.ProperName()), Descriptor.DescriptorType.Requirement);
			descriptors.Add(item);
			return descriptors;
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x000CE368 File Offset: 0x000CC568
		public override float GetEfficiencyMultiplier(WorkerBase worker)
		{
			return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.m_telescope.PercentClear);
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x002488E0 File Offset: 0x00246AE0
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			AxialI analyzeTarget = this.m_telescope.GetAnalyzeTarget();
			bool flag = false;
			if (analyzeTarget != this.currentTarget)
			{
				if (this.telescopeTargetMarker)
				{
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(analyzeTarget);
				}
				this.currentTarget = analyzeTarget;
				flag = true;
			}
			if (!flag && this.checkMarkerTimer > this.checkMarkerFrequency)
			{
				this.checkMarkerTimer = 0f;
				if (!this.telescopeTargetMarker && !ClusterGrid.Instance.GetEntityOfLayerAtCell(this.currentTarget, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(this.currentTarget);
				}
			}
			this.checkMarkerTimer += dt;
			float num = ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL / ROCKETRY.CLUSTER_FOW.DEFAULT_CYCLES_PER_REVEAL / 600f;
			float points = dt * num;
			this.m_fowManager.EarnRevealPointsForLocation(this.currentTarget, points);
			return base.OnWorkTick(worker, dt);
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x000AA038 File Offset: 0x000A8238
		public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x000AA038 File Offset: 0x000A8238
		public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool ShouldEmitCO2()
		{
			return false;
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool ShouldStoreCO2()
		{
			return false;
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x002489F0 File Offset: 0x00246BF0
		public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
		{
			if (this.storage.items.Count <= 0)
			{
				return false;
			}
			GameObject gameObject = this.storage.items[0];
			if (gameObject == null)
			{
				return false;
			}
			float mass = gameObject.GetComponent<PrimaryElement>().Mass;
			float num = 0f;
			float temperature = 0f;
			SimHashes elementConsumed = SimHashes.Vacuum;
			SimUtil.DiseaseInfo diseaseInfo;
			this.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out num, out diseaseInfo, out temperature, out elementConsumed);
			bool result = num >= amount;
			OxygenBreather.BreathableGasConsumed(oxygen_breather, elementConsumed, num, temperature, diseaseInfo.idx, diseaseInfo.count);
			return result;
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x00248A84 File Offset: 0x00246C84
		public bool IsLowOxygen()
		{
			if (this.storage.items.Count <= 0)
			{
				return true;
			}
			PrimaryElement primaryElement = this.storage.FindFirstWithMass(GameTags.Breathable, 0f);
			return primaryElement == null || primaryElement.Mass == 0f;
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x00248AD4 File Offset: 0x00246CD4
		public bool HasOxygen()
		{
			if (this.storage.items.Count <= 0)
			{
				return false;
			}
			PrimaryElement primaryElement = this.storage.FindFirstWithMass(GameTags.Breathable, 0f);
			return primaryElement != null && primaryElement.Mass > 0f;
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool IsBlocked()
		{
			return false;
		}

		// Token: 0x04002C7B RID: 11387
		[MySmiReq]
		private ClusterTelescope.Instance m_telescope;

		// Token: 0x04002C7C RID: 11388
		private ClusterFogOfWarManager.Instance m_fowManager;

		// Token: 0x04002C7D RID: 11389
		private GameObject telescopeTargetMarker;

		// Token: 0x04002C7E RID: 11390
		private AxialI currentTarget;

		// Token: 0x04002C7F RID: 11391
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04002C80 RID: 11392
		private AttributeModifier radiationShielding;

		// Token: 0x04002C81 RID: 11393
		private float checkMarkerTimer;

		// Token: 0x04002C82 RID: 11394
		private float checkMarkerFrequency = 1f;
	}

	// Token: 0x02000D17 RID: 3351
	public class ClusterTelescopeIdentifyMeteorWorkable : Workable, OxygenBreather.IGasProvider
	{
		// Token: 0x06004056 RID: 16470 RVA: 0x00248B24 File Offset: 0x00246D24
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
			this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
			this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
			this.requiredSkillPerk = Db.Get().SkillPerks.CanUseClusterTelescope.Id;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.radiationShielding = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, FIXEDTRAITS.COSMICRADIATION.TELESCOPE_RADIATION_SHIELDING, STRINGS.BUILDINGS.PREFABS.CLUSTERTELESCOPEENCLOSED.NAME, false, false, true);
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x000CE3A8 File Offset: 0x000CC5A8
		protected override void OnCleanUp()
		{
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.OnCleanUp();
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x00248BD0 File Offset: 0x00246DD0
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(this.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
			this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
			base.SetWorkTime(float.PositiveInfinity);
			this.overrideAnims = this.m_telescope.workableOverrideAnims;
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x00248C34 File Offset: 0x00246E34
		private void OnWorkableEvent(Workable workable, Workable.WorkableEvent ev)
		{
			WorkerBase worker = base.worker;
			if (worker == null)
			{
				return;
			}
			KPrefabID component = worker.GetComponent<KPrefabID>();
			OxygenBreather component2 = worker.GetComponent<OxygenBreather>();
			Klei.AI.Attributes attributes = worker.GetAttributes();
			KSelectable component3 = base.GetComponent<KSelectable>();
			if (ev == Workable.WorkableEvent.WorkStarted)
			{
				base.ShowProgressBar(true);
				this.progressBar.SetUpdateFunc(delegate
				{
					if (this.currentTarget == null)
					{
						return 0f;
					}
					return this.currentTarget.IdentifyingProgress;
				});
				this.currentTarget = this.m_telescope.GetMeteorTarget();
				AxialI axialI = this.currentTarget.ClusterGridPosition();
				if (!ClusterGrid.Instance.GetEntityOfLayerAtCell(axialI, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					TelescopeTarget component4 = this.telescopeTargetMarker.GetComponent<TelescopeTarget>();
					component4.Init(axialI);
					component4.SetTargetMeteorShower(this.currentTarget);
				}
				if (this.m_telescope.providesOxygen)
				{
					attributes.Add(this.radiationShielding);
					component2.AddGasProvider(this);
					component2.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
					component.AddTag(GameTags.Shaded, false);
				}
				base.GetComponent<Operational>().SetActive(true, false);
				this.checkMarkerFrequency = UnityEngine.Random.Range(2f, 5f);
				component3.AddStatusItem(Db.Get().BuildingStatusItems.ClusterTelescopeMeteorWorking, this);
				return;
			}
			if (ev != Workable.WorkableEvent.WorkStopped)
			{
				return;
			}
			if (this.m_telescope.providesOxygen)
			{
				attributes.Remove(this.radiationShielding);
				component2.RemoveGasProvider(this);
				component2.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
				component.RemoveTag(GameTags.Shaded);
			}
			base.GetComponent<Operational>().SetActive(false, false);
			if (this.telescopeTargetMarker != null)
			{
				Util.KDestroyGameObject(this.telescopeTargetMarker);
			}
			base.ShowProgressBar(false);
			component3.RemoveStatusItem(Db.Get().BuildingStatusItems.ClusterTelescopeMeteorWorking, this);
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x00248884 File Offset: 0x00246A84
		public override List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> descriptors = base.GetDescriptors(go);
			Element element = ElementLoader.FindElementByHash(SimHashes.Oxygen);
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(element.tag.ProperName(), string.Format(STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, element.tag.ProperName()), Descriptor.DescriptorType.Requirement);
			descriptors.Add(item);
			return descriptors;
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x00248E08 File Offset: 0x00247008
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			ClusterMapMeteorShower.Instance meteorTarget = this.m_telescope.GetMeteorTarget();
			AxialI axialI = meteorTarget.ClusterGridPosition();
			bool flag = false;
			if (meteorTarget != this.currentTarget)
			{
				if (this.telescopeTargetMarker)
				{
					TelescopeTarget component = this.telescopeTargetMarker.GetComponent<TelescopeTarget>();
					component.Init(axialI);
					component.SetTargetMeteorShower(meteorTarget);
				}
				this.currentTarget = meteorTarget;
				flag = true;
			}
			if (!flag && this.checkMarkerTimer > this.checkMarkerFrequency)
			{
				this.checkMarkerTimer = 0f;
				if (!this.telescopeTargetMarker && !ClusterGrid.Instance.GetEntityOfLayerAtCell(axialI, EntityLayer.Telescope))
				{
					this.telescopeTargetMarker = GameUtil.KInstantiate(Assets.GetPrefab("TelescopeTarget"), Grid.SceneLayer.Background, null, 0);
					this.telescopeTargetMarker.SetActive(true);
					this.telescopeTargetMarker.GetComponent<TelescopeTarget>().Init(axialI);
				}
			}
			this.checkMarkerTimer += dt;
			float num = 20f;
			float points = dt / num;
			this.currentTarget.ProgressIdentifiction(points);
			return base.OnWorkTick(worker, dt);
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x000AA038 File Offset: 0x000A8238
		public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x000AA038 File Offset: 0x000A8238
		public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool ShouldEmitCO2()
		{
			return false;
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool ShouldStoreCO2()
		{
			return false;
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x00248F08 File Offset: 0x00247108
		public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
		{
			if (this.storage.items.Count <= 0)
			{
				return false;
			}
			GameObject gameObject = this.storage.items[0];
			if (gameObject == null)
			{
				return false;
			}
			float mass = gameObject.GetComponent<PrimaryElement>().Mass;
			float num = 0f;
			float temperature = 0f;
			SimHashes elementConsumed = SimHashes.Vacuum;
			SimUtil.DiseaseInfo diseaseInfo;
			this.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out num, out diseaseInfo, out temperature, out elementConsumed);
			bool result = num >= amount;
			OxygenBreather.BreathableGasConsumed(oxygen_breather, elementConsumed, num, temperature, diseaseInfo.idx, diseaseInfo.count);
			return result;
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x00248F9C File Offset: 0x0024719C
		public bool IsLowOxygen()
		{
			if (this.storage.items.Count <= 0)
			{
				return true;
			}
			GameObject gameObject = this.storage.items[0];
			return !(gameObject == null) && gameObject.GetComponent<PrimaryElement>().Mass > 0f;
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x00248FF0 File Offset: 0x002471F0
		public bool HasOxygen()
		{
			if (this.storage.items.Count <= 0)
			{
				return false;
			}
			GameObject gameObject = this.storage.items[0];
			return !(gameObject == null) && gameObject.GetComponent<PrimaryElement>().Mass > 0f;
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool IsBlocked()
		{
			return false;
		}

		// Token: 0x04002C83 RID: 11395
		[MySmiReq]
		private ClusterTelescope.Instance m_telescope;

		// Token: 0x04002C84 RID: 11396
		private ClusterFogOfWarManager.Instance m_fowManager;

		// Token: 0x04002C85 RID: 11397
		private GameObject telescopeTargetMarker;

		// Token: 0x04002C86 RID: 11398
		private ClusterMapMeteorShower.Instance currentTarget;

		// Token: 0x04002C87 RID: 11399
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04002C88 RID: 11400
		private AttributeModifier radiationShielding;

		// Token: 0x04002C89 RID: 11401
		private float checkMarkerTimer;

		// Token: 0x04002C8A RID: 11402
		private float checkMarkerFrequency = 1f;
	}
}
