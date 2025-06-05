using System;
using System.Collections.Generic;
using Database;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005C1 RID: 1473
public class SpiceGrinder : GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>
{
	// Token: 0x0600199A RID: 6554 RVA: 0x001AF564 File Offset: 0x001AD764
	public static void InitializeSpices()
	{
		Spices spices = Db.Get().Spices;
		SpiceGrinder.SettingOptions = new Dictionary<Tag, SpiceGrinder.Option>();
		for (int i = 0; i < spices.Count; i++)
		{
			Spice spice = spices[i];
			if (DlcManager.IsCorrectDlcSubscribed(spice))
			{
				SpiceGrinder.SettingOptions.Add(spice.Id, new SpiceGrinder.Option(spice));
			}
		}
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x001AF5C4 File Offset: 0x001AD7C4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.root.Enter(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.OnEnterRoot)).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.GameEvent.Callback(this.OnStorageChanged));
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.ready, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational)).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).Enter(delegate(SpiceGrinder.StatesInstance smi)
		{
			smi.Play((smi.SelectedOption != null) ? "off" : "default", KAnim.PlayMode.Once);
			smi.CancelFetches("inoperational");
			if (smi.SelectedOption == null)
			{
				smi.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSpiceSelected, null);
			}
		}).Exit(delegate(SpiceGrinder.StatesInstance smi)
		{
			smi.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoSpiceSelected, false);
		});
		this.operational.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Not(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational))).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).ParamTransition<bool>(this.isReady, this.ready, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.IsTrue).Update(delegate(SpiceGrinder.StatesInstance smi, float dt)
		{
			if (smi.CurrentFood != null && !smi.HasOpenFetches)
			{
				bool value = smi.CanSpice(smi.CurrentFood.Calories);
				this.isReady.Set(value, smi, false);
			}
		}, UpdateRate.SIM_1000ms, false).PlayAnim("on");
		this.ready.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Not(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational))).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).ParamTransition<bool>(this.isReady, this.operational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.IsFalse).ToggleRecurringChore(new Func<SpiceGrinder.StatesInstance, Chore>(this.CreateChore), null);
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x000B54E3 File Offset: 0x000B36E3
	private void UpdateInKitchen(SpiceGrinder.StatesInstance smi)
	{
		smi.GetComponent<Operational>().SetFlag(SpiceGrinder.inKitchen, smi.roomTracker.IsInCorrectRoom());
	}

	// Token: 0x0600199D RID: 6557 RVA: 0x000B5500 File Offset: 0x000B3700
	private void OnEnterRoot(SpiceGrinder.StatesInstance smi)
	{
		smi.Initialize();
	}

	// Token: 0x0600199E RID: 6558 RVA: 0x000B5508 File Offset: 0x000B3708
	private bool IsOperational(SpiceGrinder.StatesInstance smi)
	{
		return smi.IsOperational;
	}

	// Token: 0x0600199F RID: 6559 RVA: 0x001AF760 File Offset: 0x001AD960
	private void OnStorageChanged(SpiceGrinder.StatesInstance smi, object data)
	{
		smi.UpdateMeter();
		smi.UpdateFoodSymbol();
		if (smi.SelectedOption == null)
		{
			return;
		}
		bool value = smi.AvailableFood > 0f && smi.CanSpice(smi.CurrentFood.Calories);
		smi.sm.isReady.Set(value, smi, false);
	}

	// Token: 0x060019A0 RID: 6560 RVA: 0x001AF7B8 File Offset: 0x001AD9B8
	private Chore CreateChore(SpiceGrinder.StatesInstance smi)
	{
		return new WorkChore<SpiceGrinderWorkable>(Db.Get().ChoreTypes.Cook, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x040010A2 RID: 4258
	public static Dictionary<Tag, SpiceGrinder.Option> SettingOptions = null;

	// Token: 0x040010A3 RID: 4259
	public static readonly Operational.Flag spiceSet = new Operational.Flag("spiceSet", Operational.Flag.Type.Functional);

	// Token: 0x040010A4 RID: 4260
	public static Operational.Flag inKitchen = new Operational.Flag("inKitchen", Operational.Flag.Type.Functional);

	// Token: 0x040010A5 RID: 4261
	public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State inoperational;

	// Token: 0x040010A6 RID: 4262
	public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State operational;

	// Token: 0x040010A7 RID: 4263
	public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State ready;

	// Token: 0x040010A8 RID: 4264
	public StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.BoolParameter isReady;

	// Token: 0x020005C2 RID: 1474
	public class Option : IConfigurableConsumerOption
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060019A4 RID: 6564 RVA: 0x001AF834 File Offset: 0x001ADA34
		public Effect StatBonus
		{
			get
			{
				if (this.statBonus == null)
				{
					return null;
				}
				if (string.IsNullOrEmpty(this.spiceDescription))
				{
					this.CreateDescription();
					this.GetName();
				}
				this.statBonus.Name = this.name;
				this.statBonus.description = this.spiceDescription;
				return this.statBonus;
			}
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x001AF890 File Offset: 0x001ADA90
		public Option(Spice spice)
		{
			this.Id = new Tag(spice.Id);
			this.Spice = spice;
			if (spice.StatBonus != null)
			{
				this.statBonus = new Effect(spice.Id, this.GetName(), this.spiceDescription, 600f, true, false, false, null, -1f, 0f, null, "");
				this.statBonus.Add(spice.StatBonus);
				Db.Get().effects.Add(this.statBonus);
			}
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x000B5540 File Offset: 0x000B3740
		public Tag GetID()
		{
			return this.Spice.Id;
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x001AF920 File Offset: 0x001ADB20
		public string GetName()
		{
			if (string.IsNullOrEmpty(this.name))
			{
				string text = "STRINGS.ITEMS.SPICES." + this.Spice.Id.ToUpper() + ".NAME";
				StringEntry stringEntry;
				Strings.TryGet(text, out stringEntry);
				this.name = "MISSING " + text;
				if (stringEntry != null)
				{
					this.name = stringEntry;
				}
			}
			return this.name;
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x000B5552 File Offset: 0x000B3752
		public string GetDetailedDescription()
		{
			if (string.IsNullOrEmpty(this.fullDescription))
			{
				this.CreateDescription();
			}
			return this.fullDescription;
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x001AF98C File Offset: 0x001ADB8C
		public string GetDescription()
		{
			if (!string.IsNullOrEmpty(this.spiceDescription))
			{
				return this.spiceDescription;
			}
			string text = "STRINGS.ITEMS.SPICES." + this.Spice.Id.ToUpper() + ".DESC";
			StringEntry stringEntry;
			Strings.TryGet(text, out stringEntry);
			this.spiceDescription = "MISSING " + text;
			if (stringEntry != null)
			{
				this.spiceDescription = stringEntry.String;
			}
			return this.spiceDescription;
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x001AF9FC File Offset: 0x001ADBFC
		private void CreateDescription()
		{
			string text = "STRINGS.ITEMS.SPICES." + this.Spice.Id.ToUpper() + ".DESC";
			StringEntry stringEntry;
			Strings.TryGet(text, out stringEntry);
			this.spiceDescription = "MISSING " + text;
			if (stringEntry != null)
			{
				this.spiceDescription = stringEntry.String;
			}
			this.ingredientDescriptions = string.Format("\n\n<b>{0}</b>", BUILDINGS.PREFABS.SPICEGRINDER.INGREDIENTHEADER);
			for (int i = 0; i < this.Spice.Ingredients.Length; i++)
			{
				Spice.Ingredient ingredient = this.Spice.Ingredients[i];
				GameObject prefab = Assets.GetPrefab((ingredient.IngredientSet != null && ingredient.IngredientSet.Length != 0) ? ingredient.IngredientSet[0] : null);
				this.ingredientDescriptions += string.Format("\n{0}{1} {2}{3}", new object[]
				{
					"    • ",
					prefab.GetProperName(),
					ingredient.AmountKG,
					GameUtil.GetUnitTypeMassOrUnit(prefab)
				});
			}
			this.fullDescription = this.spiceDescription + this.ingredientDescriptions;
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x000B556D File Offset: 0x000B376D
		public Sprite GetIcon()
		{
			return Assets.GetSprite(this.Spice.Image);
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x001AFB24 File Offset: 0x001ADD24
		public IConfigurableConsumerIngredient[] GetIngredients()
		{
			return this.Spice.Ingredients;
		}

		// Token: 0x040010A9 RID: 4265
		public readonly Tag Id;

		// Token: 0x040010AA RID: 4266
		public readonly Spice Spice;

		// Token: 0x040010AB RID: 4267
		private string name;

		// Token: 0x040010AC RID: 4268
		private string fullDescription;

		// Token: 0x040010AD RID: 4269
		private string spiceDescription;

		// Token: 0x040010AE RID: 4270
		private string ingredientDescriptions;

		// Token: 0x040010AF RID: 4271
		private Effect statBonus;
	}

	// Token: 0x020005C3 RID: 1475
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020005C4 RID: 1476
	public class StatesInstance : GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.GameInstance
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060019AE RID: 6574 RVA: 0x000B5584 File Offset: 0x000B3784
		public bool IsOperational
		{
			get
			{
				return this.operational != null && this.operational.IsOperational;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060019AF RID: 6575 RVA: 0x000B55A1 File Offset: 0x000B37A1
		public float AvailableFood
		{
			get
			{
				if (!(this.foodStorage == null))
				{
					return this.foodStorage.MassStored();
				}
				return 0f;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060019B0 RID: 6576 RVA: 0x000B55C2 File Offset: 0x000B37C2
		public SpiceGrinder.Option SelectedOption
		{
			get
			{
				if (!(this.currentSpice.Id == Tag.Invalid))
				{
					return SpiceGrinder.SettingOptions[this.currentSpice.Id];
				}
				return null;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060019B1 RID: 6577 RVA: 0x001AFB40 File Offset: 0x001ADD40
		public Edible CurrentFood
		{
			get
			{
				GameObject gameObject = this.foodStorage.FindFirst(GameTags.Edible);
				this.currentFood = ((gameObject != null) ? gameObject.GetComponent<Edible>() : null);
				return this.currentFood;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060019B2 RID: 6578 RVA: 0x000B55F2 File Offset: 0x000B37F2
		public bool HasOpenFetches
		{
			get
			{
				return Array.Exists<FetchChore>(this.SpiceFetches, (FetchChore fetch) => fetch != null);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060019B3 RID: 6579 RVA: 0x000B561E File Offset: 0x000B381E
		// (set) Token: 0x060019B4 RID: 6580 RVA: 0x000B5626 File Offset: 0x000B3826
		public bool AllowMutantSeeds
		{
			get
			{
				return this.allowMutantSeeds;
			}
			set
			{
				this.allowMutantSeeds = value;
				this.ToggleMutantSeedFetches(this.allowMutantSeeds);
			}
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x001AFB7C File Offset: 0x001ADD7C
		public StatesInstance(IStateMachineTarget master, SpiceGrinder.Def def) : base(master, def)
		{
			this.workable.Grinder = this;
			Storage[] components = base.gameObject.GetComponents<Storage>();
			this.foodStorage = components[0];
			this.seedStorage = components[1];
			this.operational = base.GetComponent<Operational>();
			this.kbac = base.GetComponent<KBatchedAnimController>();
			this.foodStorageFilter = new FilteredStorage(base.GetComponent<KPrefabID>(), this.foodFilter, null, false, Db.Get().ChoreTypes.CookFetch);
			this.foodStorageFilter.SetHasMeter(false);
			this.meter = new MeterController(this.kbac, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_frame",
				"meter_level"
			});
			this.SetupFoodSymbol();
			this.UpdateFoodSymbol();
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			base.sm.UpdateInKitchen(this);
			Prioritizable.AddRef(base.gameObject);
			base.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x000B563B File Offset: 0x000B383B
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Prioritizable.RemoveRef(base.gameObject);
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x001AFCAC File Offset: 0x001ADEAC
		public void Initialize()
		{
			if (DlcManager.IsExpansion1Active())
			{
				this.mutantSeedStatusItem = new StatusItem("SPICEGRINDERACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
				if (this.AllowMutantSeeds)
				{
					KSelectable component = base.GetComponent<KSelectable>();
					if (component != null)
					{
						component.AddStatusItem(this.mutantSeedStatusItem, null);
					}
				}
			}
			SpiceGrinder.Option spiceOption;
			SpiceGrinder.SettingOptions.TryGetValue(new Tag(this.spiceHash), out spiceOption);
			this.OnOptionSelected(spiceOption);
			base.sm.OnStorageChanged(this, null);
			this.UpdateMeter();
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x001AFD44 File Offset: 0x001ADF44
		private void OnRefreshUserMenu(object data)
		{
			if (DlcManager.FeatureRadiationEnabled())
			{
				Game.Instance.userMenu.AddButton(base.smi.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", base.smi.AllowMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT, delegate()
				{
					base.smi.AllowMutantSeeds = !base.smi.AllowMutantSeeds;
					this.OnRefreshUserMenu(base.smi);
				}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.TOOLTIP, true), 1f);
			}
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x001AFDC0 File Offset: 0x001ADFC0
		public void ToggleMutantSeedFetches(bool allow)
		{
			if (DlcManager.IsExpansion1Active())
			{
				this.UpdateMutantSeedFetches();
				if (allow)
				{
					this.seedStorage.storageFilters.Add(GameTags.MutatedSeed);
					KSelectable component = base.GetComponent<KSelectable>();
					if (component != null)
					{
						component.AddStatusItem(this.mutantSeedStatusItem, null);
						return;
					}
				}
				else
				{
					if (this.seedStorage.GetMassAvailable(GameTags.MutatedSeed) > 0f)
					{
						this.seedStorage.Drop(GameTags.MutatedSeed);
					}
					this.seedStorage.storageFilters.Remove(GameTags.MutatedSeed);
					KSelectable component2 = base.GetComponent<KSelectable>();
					if (component2 != null)
					{
						component2.RemoveStatusItem(this.mutantSeedStatusItem, false);
					}
				}
			}
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x001AFE70 File Offset: 0x001AE070
		private void UpdateMutantSeedFetches()
		{
			if (this.SpiceFetches != null)
			{
				Tag[] tags = new Tag[]
				{
					GameTags.Seed,
					GameTags.CropSeed
				};
				for (int i = this.SpiceFetches.Length - 1; i >= 0; i--)
				{
					FetchChore fetchChore = this.SpiceFetches[i];
					if (fetchChore != null)
					{
						using (HashSet<Tag>.Enumerator enumerator = this.SpiceFetches[i].tags.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (Assets.GetPrefab(enumerator.Current).HasAnyTags(tags))
								{
									fetchChore.Cancel("MutantSeedChanges");
									this.SpiceFetches[i] = this.CreateFetchChore(fetchChore.tags, fetchChore.amount);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x001AFF40 File Offset: 0x001AE140
		private void OnCopySettings(object data)
		{
			SpiceGrinderWorkable component = ((GameObject)data).GetComponent<SpiceGrinderWorkable>();
			if (component != null)
			{
				this.currentSpice = component.Grinder.currentSpice;
				SpiceGrinder.Option spiceOption;
				SpiceGrinder.SettingOptions.TryGetValue(new Tag(component.Grinder.spiceHash), out spiceOption);
				this.OnOptionSelected(spiceOption);
				this.allowMutantSeeds = component.Grinder.AllowMutantSeeds;
			}
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x001AFFA8 File Offset: 0x001AE1A8
		public void SetupFoodSymbol()
		{
			GameObject gameObject = Util.NewGameObject(base.gameObject, "foodSymbol");
			gameObject.SetActive(false);
			bool flag;
			Vector3 position = this.kbac.GetSymbolTransform(SpiceGrinder.StatesInstance.HASH_FOOD, out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			gameObject.transform.SetPosition(position);
			this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
			this.foodKBAC.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("mushbar_kanim")
			};
			this.foodKBAC.initialAnim = "object";
			this.kbac.SetSymbolVisiblity(SpiceGrinder.StatesInstance.HASH_FOOD, false);
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x001B0064 File Offset: 0x001AE264
		public void UpdateFoodSymbol()
		{
			bool flag = this.AvailableFood > 0f && this.CurrentFood != null;
			this.foodKBAC.gameObject.SetActive(flag);
			if (flag)
			{
				this.foodKBAC.SwapAnims(this.CurrentFood.GetComponent<KBatchedAnimController>().AnimFiles);
				this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x000B564E File Offset: 0x000B384E
		public void UpdateMeter()
		{
			this.meter.SetPositionPercent(this.seedStorage.MassStored() / this.seedStorage.capacityKg);
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x001B00E0 File Offset: 0x001AE2E0
		public void SpiceFood()
		{
			float num = this.CurrentFood.Calories / 1000f;
			this.CurrentFood.SpiceEdible(this.currentSpice, SpiceGrinderConfig.SpicedStatus);
			this.foodStorage.Drop(this.CurrentFood.gameObject, true);
			this.currentFood = null;
			this.UpdateFoodSymbol();
			foreach (Spice.Ingredient ingredient in SpiceGrinder.SettingOptions[this.currentSpice.Id].Spice.Ingredients)
			{
				float num2 = num * ingredient.AmountKG / 1000f;
				int num3 = ingredient.IngredientSet.Length - 1;
				while (num2 > 0f && num3 >= 0)
				{
					Tag tag = ingredient.IngredientSet[num3];
					float num4;
					SimUtil.DiseaseInfo diseaseInfo;
					float num5;
					this.seedStorage.ConsumeAndGetDisease(tag, num2, out num4, out diseaseInfo, out num5);
					num2 -= num4;
					num3--;
				}
			}
			base.sm.isReady.Set(false, this, false);
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x001B01E0 File Offset: 0x001AE3E0
		public bool CanSpice(float kcalToSpice)
		{
			bool flag = true;
			float num = kcalToSpice / 1000f;
			Spice.Ingredient[] ingredients = SpiceGrinder.SettingOptions[this.currentSpice.Id].Spice.Ingredients;
			Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
			for (int i = 0; i < ingredients.Length; i++)
			{
				Spice.Ingredient ingredient = ingredients[i];
				float num2 = 0f;
				int num3 = 0;
				while (ingredient.IngredientSet != null && num3 < ingredient.IngredientSet.Length)
				{
					num2 += this.seedStorage.GetMassAvailable(ingredient.IngredientSet[num3]);
					num3++;
				}
				float num4 = num * ingredient.AmountKG / 1000f;
				flag &= (num4 <= num2);
				if (num4 > num2)
				{
					dictionary.Add(ingredient.IngredientSet[0], num4 - num2);
					if (this.SpiceFetches != null && this.SpiceFetches[i] == null)
					{
						this.SpiceFetches[i] = this.CreateFetchChore(ingredient.IngredientSet, ingredient.AmountKG * 10f);
					}
				}
			}
			this.UpdateSpiceIngredientStatus(flag, dictionary);
			return flag;
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x000B5672 File Offset: 0x000B3872
		private FetchChore CreateFetchChore(Tag[] ingredientIngredientSet, float amount)
		{
			return this.CreateFetchChore(new HashSet<Tag>(ingredientIngredientSet), amount);
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x001B02FC File Offset: 0x001AE4FC
		private FetchChore CreateFetchChore(HashSet<Tag> ingredients, float amount)
		{
			float num = Mathf.Max(amount, 1f);
			ChoreType cookFetch = Db.Get().ChoreTypes.CookFetch;
			Storage destination = this.seedStorage;
			float amount2 = num;
			FetchChore.MatchCriteria criteria = FetchChore.MatchCriteria.MatchID;
			Tag invalid = Tag.Invalid;
			Action<Chore> on_complete = new Action<Chore>(this.ClearFetchChore);
			Tag[] forbidden_tags;
			if (!this.AllowMutantSeeds)
			{
				(forbidden_tags = new Tag[1])[0] = GameTags.MutatedSeed;
			}
			else
			{
				forbidden_tags = null;
			}
			return new FetchChore(cookFetch, destination, amount2, ingredients, criteria, invalid, forbidden_tags, null, true, on_complete, null, null, Operational.State.Operational, 0);
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x001B0368 File Offset: 0x001AE568
		private void ClearFetchChore(Chore obj)
		{
			FetchChore fetchChore = obj as FetchChore;
			if (fetchChore == null || !fetchChore.isComplete || this.SpiceFetches == null)
			{
				return;
			}
			int i = this.SpiceFetches.Length - 1;
			while (i >= 0)
			{
				if (this.SpiceFetches[i] == fetchChore)
				{
					float num = fetchChore.originalAmount - fetchChore.amount;
					if (num > 0f)
					{
						this.SpiceFetches[i] = this.CreateFetchChore(fetchChore.tags, num);
						return;
					}
					this.SpiceFetches[i] = null;
					return;
				}
				else
				{
					i--;
				}
			}
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x001B03E8 File Offset: 0x001AE5E8
		private void UpdateSpiceIngredientStatus(bool can_spice, Dictionary<Tag, float> missing_spices)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			if (can_spice)
			{
				this.missingResourceStatusItem = component.RemoveStatusItem(this.missingResourceStatusItem, false);
				return;
			}
			if (this.missingResourceStatusItem != Guid.Empty)
			{
				this.missingResourceStatusItem = component.ReplaceStatusItem(this.missingResourceStatusItem, Db.Get().BuildingStatusItems.MaterialsUnavailable, missing_spices);
				return;
			}
			this.missingResourceStatusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailable, missing_spices);
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x001B0464 File Offset: 0x001AE664
		public void OnOptionSelected(SpiceGrinder.Option spiceOption)
		{
			base.smi.GetComponent<Operational>().SetFlag(SpiceGrinder.spiceSet, spiceOption != null);
			if (spiceOption == null)
			{
				this.kbac.Play("default", KAnim.PlayMode.Once, 1f, 0f);
				this.kbac.SetSymbolTint("stripe_anim2", Color.white);
			}
			else
			{
				this.kbac.Play(this.IsOperational ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
			}
			this.CancelFetches("SpiceChanged");
			if (this.currentSpice.Id != Tag.Invalid)
			{
				this.seedStorage.DropAll(false, false, default(Vector3), true, null);
				this.UpdateMeter();
				base.sm.isReady.Set(false, this, false);
			}
			if (this.missingResourceStatusItem != Guid.Empty)
			{
				this.missingResourceStatusItem = base.GetComponent<KSelectable>().RemoveStatusItem(this.missingResourceStatusItem, false);
			}
			if (spiceOption != null)
			{
				this.currentSpice = new SpiceInstance
				{
					Id = spiceOption.Id,
					TotalKG = spiceOption.Spice.TotalKG
				};
				this.SetSpiceSymbolColours(spiceOption.Spice);
				this.spiceHash = this.currentSpice.Id.GetHash();
				this.seedStorage.capacityKg = this.currentSpice.TotalKG * 10f;
				Spice.Ingredient[] ingredients = spiceOption.Spice.Ingredients;
				this.SpiceFetches = new FetchChore[ingredients.Length];
				Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
				for (int i = 0; i < ingredients.Length; i++)
				{
					Spice.Ingredient ingredient = ingredients[i];
					float num = (this.CurrentFood != null) ? (this.CurrentFood.Calories * ingredient.AmountKG / 1000000f) : 0f;
					if (this.seedStorage.GetMassAvailable(ingredient.IngredientSet[0]) < num)
					{
						this.SpiceFetches[i] = this.CreateFetchChore(ingredient.IngredientSet, ingredient.AmountKG * 10f);
					}
					if (this.CurrentFood != null)
					{
						dictionary.Add(ingredient.IngredientSet[0], num);
					}
				}
				if (this.CurrentFood != null)
				{
					this.UpdateSpiceIngredientStatus(false, dictionary);
				}
				this.foodFilter[0] = this.currentSpice.Id;
				this.foodStorageFilter.FilterChanged();
			}
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x001B06F0 File Offset: 0x001AE8F0
		public void CancelFetches(string reason)
		{
			if (this.SpiceFetches != null)
			{
				for (int i = 0; i < this.SpiceFetches.Length; i++)
				{
					if (this.SpiceFetches[i] != null)
					{
						this.SpiceFetches[i].Cancel(reason);
						this.SpiceFetches[i] = null;
					}
				}
			}
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x001B073C File Offset: 0x001AE93C
		private void SetSpiceSymbolColours(Spice spice)
		{
			this.kbac.SetSymbolTint("stripe_anim2", spice.PrimaryColor);
			this.kbac.SetSymbolTint("stripe_anim1", spice.SecondaryColor);
			this.kbac.SetSymbolTint("grinder", spice.PrimaryColor);
		}

		// Token: 0x040010B0 RID: 4272
		private static string HASH_FOOD = "food";

		// Token: 0x040010B1 RID: 4273
		private KBatchedAnimController kbac;

		// Token: 0x040010B2 RID: 4274
		private KBatchedAnimController foodKBAC;

		// Token: 0x040010B3 RID: 4275
		[MyCmpReq]
		public RoomTracker roomTracker;

		// Token: 0x040010B4 RID: 4276
		[MyCmpReq]
		public SpiceGrinderWorkable workable;

		// Token: 0x040010B5 RID: 4277
		[Serialize]
		private int spiceHash;

		// Token: 0x040010B6 RID: 4278
		private SpiceInstance currentSpice;

		// Token: 0x040010B7 RID: 4279
		private Edible currentFood;

		// Token: 0x040010B8 RID: 4280
		private Storage seedStorage;

		// Token: 0x040010B9 RID: 4281
		private Storage foodStorage;

		// Token: 0x040010BA RID: 4282
		private MeterController meter;

		// Token: 0x040010BB RID: 4283
		private Tag[] foodFilter = new Tag[1];

		// Token: 0x040010BC RID: 4284
		private FilteredStorage foodStorageFilter;

		// Token: 0x040010BD RID: 4285
		private Operational operational;

		// Token: 0x040010BE RID: 4286
		private Guid missingResourceStatusItem = Guid.Empty;

		// Token: 0x040010BF RID: 4287
		private StatusItem mutantSeedStatusItem;

		// Token: 0x040010C0 RID: 4288
		private FetchChore[] SpiceFetches;

		// Token: 0x040010C1 RID: 4289
		[Serialize]
		private bool allowMutantSeeds = true;
	}
}
