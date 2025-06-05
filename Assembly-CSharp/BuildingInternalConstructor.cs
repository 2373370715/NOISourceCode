using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000CB9 RID: 3257
public class BuildingInternalConstructor : GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>
{
	// Token: 0x06003E0F RID: 15887 RVA: 0x002416D4 File Offset: 0x0023F8D4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (BuildingInternalConstructor.Instance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(BuildingInternalConstructor.Instance smi)
		{
			smi.ShowConstructionSymbol(false);
		});
		this.operational.DefaultState(this.operational.constructionRequired).EventTransition(GameHashes.OperationalChanged, this.inoperational, (BuildingInternalConstructor.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.operational.constructionRequired.EventTransition(GameHashes.OnStorageChange, this.operational.constructionHappening, (BuildingInternalConstructor.Instance smi) => smi.GetMassForConstruction() != null).EventTransition(GameHashes.OnStorageChange, this.operational.constructionSatisfied, (BuildingInternalConstructor.Instance smi) => smi.HasOutputInStorage()).ToggleFetch((BuildingInternalConstructor.Instance smi) => smi.CreateFetchList(), this.operational.constructionHappening).ParamTransition<bool>(this.constructionRequested, this.operational.constructionSatisfied, GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.IsFalse).Enter(delegate(BuildingInternalConstructor.Instance smi)
		{
			smi.ShowConstructionSymbol(true);
		}).Exit(delegate(BuildingInternalConstructor.Instance smi)
		{
			smi.ShowConstructionSymbol(false);
		});
		this.operational.constructionHappening.EventTransition(GameHashes.OnStorageChange, this.operational.constructionSatisfied, (BuildingInternalConstructor.Instance smi) => smi.HasOutputInStorage()).EventTransition(GameHashes.OnStorageChange, this.operational.constructionRequired, (BuildingInternalConstructor.Instance smi) => smi.GetMassForConstruction() == null).ToggleChore((BuildingInternalConstructor.Instance smi) => smi.CreateWorkChore(), this.operational.constructionHappening, this.operational.constructionHappening).ParamTransition<bool>(this.constructionRequested, this.operational.constructionSatisfied, GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.IsFalse).Enter(delegate(BuildingInternalConstructor.Instance smi)
		{
			smi.ShowConstructionSymbol(true);
		}).Exit(delegate(BuildingInternalConstructor.Instance smi)
		{
			smi.ShowConstructionSymbol(false);
		});
		this.operational.constructionSatisfied.EventTransition(GameHashes.OnStorageChange, this.operational.constructionRequired, (BuildingInternalConstructor.Instance smi) => !smi.HasOutputInStorage() && this.constructionRequested.Get(smi)).ParamTransition<bool>(this.constructionRequested, this.operational.constructionRequired, (BuildingInternalConstructor.Instance smi, bool p) => p && !smi.HasOutputInStorage());
	}

	// Token: 0x04002AD3 RID: 10963
	public GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.State inoperational;

	// Token: 0x04002AD4 RID: 10964
	public BuildingInternalConstructor.OperationalStates operational;

	// Token: 0x04002AD5 RID: 10965
	public StateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.BoolParameter constructionRequested = new StateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.BoolParameter(true);

	// Token: 0x02000CBA RID: 3258
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04002AD6 RID: 10966
		public DefComponent<Storage> storage;

		// Token: 0x04002AD7 RID: 10967
		public float constructionMass;

		// Token: 0x04002AD8 RID: 10968
		public List<string> outputIDs;

		// Token: 0x04002AD9 RID: 10969
		public bool spawnIntoStorage;

		// Token: 0x04002ADA RID: 10970
		public string constructionSymbol;
	}

	// Token: 0x02000CBB RID: 3259
	public class OperationalStates : GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.State
	{
		// Token: 0x04002ADB RID: 10971
		public GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.State constructionRequired;

		// Token: 0x04002ADC RID: 10972
		public GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.State constructionHappening;

		// Token: 0x04002ADD RID: 10973
		public GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.State constructionSatisfied;
	}

	// Token: 0x02000CBC RID: 3260
	public new class Instance : GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x06003E14 RID: 15892 RVA: 0x000CCBA5 File Offset: 0x000CADA5
		public Instance(IStateMachineTarget master, BuildingInternalConstructor.Def def) : base(master, def)
		{
			this.storage = def.storage.Get(this);
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new InternalConstructionCompleteCondition(this));
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x00241A0C File Offset: 0x0023FC0C
		protected override void OnCleanUp()
		{
			Element element = null;
			float num = 0f;
			float num2 = 0f;
			byte maxValue = byte.MaxValue;
			int disease_count = 0;
			foreach (string s in base.def.outputIDs)
			{
				GameObject gameObject = this.storage.FindFirst(s);
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					global::Debug.Assert(element == null || element == component.Element);
					element = component.Element;
					num2 = GameUtil.GetFinalTemperature(num, num2, component.Mass, component.Temperature);
					num += component.Mass;
					gameObject.DeleteObject();
				}
			}
			if (element != null)
			{
				element.substance.SpawnResource(base.transform.GetPosition(), num, num2, maxValue, disease_count, false, false, false);
			}
			base.OnCleanUp();
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x00241B0C File Offset: 0x0023FD0C
		public FetchList2 CreateFetchList()
		{
			FetchList2 fetchList = new FetchList2(this.storage, Db.Get().ChoreTypes.Fetch);
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			fetchList.Add(component.Element.tag, null, base.def.constructionMass, Operational.State.None);
			return fetchList;
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x00241B58 File Offset: 0x0023FD58
		public PrimaryElement GetMassForConstruction()
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			return this.storage.FindFirstWithMass(component.Element.tag, base.def.constructionMass);
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x000CCBD4 File Offset: 0x000CADD4
		public bool HasOutputInStorage()
		{
			return this.storage.FindFirst(base.def.outputIDs[0].ToTag());
		}

		// Token: 0x06003E19 RID: 15897 RVA: 0x000CCBFC File Offset: 0x000CADFC
		public bool IsRequestingConstruction()
		{
			base.sm.constructionRequested.Get(this);
			return base.smi.sm.constructionRequested.Get(base.smi);
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x00241B90 File Offset: 0x0023FD90
		public void ConstructionComplete(bool force = false)
		{
			SimHashes element_id;
			if (!force)
			{
				PrimaryElement massForConstruction = this.GetMassForConstruction();
				element_id = massForConstruction.ElementID;
				float mass = massForConstruction.Mass;
				float num = massForConstruction.Temperature * massForConstruction.Mass;
				massForConstruction.Mass -= base.def.constructionMass;
				Mathf.Clamp(num / mass, 0f, 318.15f);
			}
			else
			{
				element_id = SimHashes.Cuprite;
				float temperature = base.GetComponent<PrimaryElement>().Temperature;
			}
			foreach (string s in base.def.outputIDs)
			{
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(s), base.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0);
				gameObject.GetComponent<PrimaryElement>().SetElement(element_id, false);
				gameObject.SetActive(true);
				if (base.def.spawnIntoStorage)
				{
					this.storage.Store(gameObject, false, false, true, false);
				}
			}
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x00241C98 File Offset: 0x0023FE98
		public WorkChore<BuildingInternalConstructorWorkable> CreateWorkChore()
		{
			return new WorkChore<BuildingInternalConstructorWorkable>(Db.Get().ChoreTypes.Build, base.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x06003E1C RID: 15900 RVA: 0x00241CD0 File Offset: 0x0023FED0
		public void ShowConstructionSymbol(bool show)
		{
			KBatchedAnimController component = base.master.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.SetSymbolVisiblity(base.def.constructionSymbol, show);
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06003E1D RID: 15901 RVA: 0x00241D0C File Offset: 0x0023FF0C
		public string SidescreenButtonText
		{
			get
			{
				if (!base.smi.sm.constructionRequested.Get(base.smi))
				{
					return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.ALLOW_INTERNAL_CONSTRUCTOR.text, Assets.GetPrefab(base.def.outputIDs[0]).GetProperName());
				}
				return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.DISALLOW_INTERNAL_CONSTRUCTOR.text, Assets.GetPrefab(base.def.outputIDs[0]).GetProperName());
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06003E1E RID: 15902 RVA: 0x00241D98 File Offset: 0x0023FF98
		public string SidescreenButtonTooltip
		{
			get
			{
				if (!base.smi.sm.constructionRequested.Get(base.smi))
				{
					return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.ALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP.text, Assets.GetPrefab(base.def.outputIDs[0]).GetProperName());
				}
				return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.DISALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP.text, Assets.GetPrefab(base.def.outputIDs[0]).GetProperName());
			}
		}

		// Token: 0x06003E1F RID: 15903 RVA: 0x00241E24 File Offset: 0x00240024
		public void OnSidescreenButtonPressed()
		{
			base.smi.sm.constructionRequested.Set(!base.smi.sm.constructionRequested.Get(base.smi), base.smi, false);
			if (DebugHandler.InstantBuildMode && base.smi.sm.constructionRequested.Get(base.smi) && !this.HasOutputInStorage())
			{
				this.ConstructionComplete(true);
			}
		}

		// Token: 0x06003E20 RID: 15904 RVA: 0x000AFECA File Offset: 0x000AE0CA
		public void SetButtonTextOverride(ButtonMenuTextOverride text)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public bool SidescreenEnabled()
		{
			return true;
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public bool SidescreenButtonInteractable()
		{
			return true;
		}

		// Token: 0x06003E23 RID: 15907 RVA: 0x000AFED1 File Offset: 0x000AE0D1
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x000AFE89 File Offset: 0x000AE089
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x04002ADE RID: 10974
		private Storage storage;

		// Token: 0x04002ADF RID: 10975
		[Serialize]
		private float constructionElapsed;

		// Token: 0x04002AE0 RID: 10976
		private ProgressBar progressBar;
	}
}
