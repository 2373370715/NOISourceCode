using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A6B RID: 2667
[AddComponentMenu("KMonoBehaviour/Workable/Edible")]
public class Edible : Workable, IGameObjectEffectDescriptor, ISaveLoadable, IExtendSplitting
{
	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06003058 RID: 12376 RVA: 0x000C3E0C File Offset: 0x000C200C
	// (set) Token: 0x06003059 RID: 12377 RVA: 0x000C3E19 File Offset: 0x000C2019
	public float Units
	{
		get
		{
			return this.primaryElement.Units;
		}
		set
		{
			this.primaryElement.Units = value;
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x0600305A RID: 12378 RVA: 0x000C3E27 File Offset: 0x000C2027
	public float MassPerUnit
	{
		get
		{
			return this.primaryElement.MassPerUnit;
		}
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x0600305B RID: 12379 RVA: 0x000C3E34 File Offset: 0x000C2034
	// (set) Token: 0x0600305C RID: 12380 RVA: 0x000C3E48 File Offset: 0x000C2048
	public float Calories
	{
		get
		{
			return this.Units * this.foodInfo.CaloriesPerUnit;
		}
		set
		{
			this.Units = value / this.foodInfo.CaloriesPerUnit;
		}
	}

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x0600305D RID: 12381 RVA: 0x000C3E5D File Offset: 0x000C205D
	// (set) Token: 0x0600305E RID: 12382 RVA: 0x000C3E65 File Offset: 0x000C2065
	public EdiblesManager.FoodInfo FoodInfo
	{
		get
		{
			return this.foodInfo;
		}
		set
		{
			this.foodInfo = value;
			this.FoodID = this.foodInfo.Id;
		}
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x0600305F RID: 12383 RVA: 0x000C3E7F File Offset: 0x000C207F
	// (set) Token: 0x06003060 RID: 12384 RVA: 0x000C3E87 File Offset: 0x000C2087
	public bool isBeingConsumed { get; private set; }

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x06003061 RID: 12385 RVA: 0x000C3E90 File Offset: 0x000C2090
	public List<SpiceInstance> Spices
	{
		get
		{
			return this.spices;
		}
	}

	// Token: 0x06003062 RID: 12386 RVA: 0x00209010 File Offset: 0x00207210
	protected override void OnPrefabInit()
	{
		this.primaryElement = base.GetComponent<PrimaryElement>();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.shouldTransferDiseaseWithWorker = false;
		base.OnPrefabInit();
		if (this.foodInfo == null)
		{
			if (this.FoodID == null)
			{
				global::Debug.LogError("No food FoodID");
			}
			this.foodInfo = EdiblesManager.GetFoodInfo(this.FoodID);
		}
		base.Subscribe<Edible>(748399584, Edible.OnCraftDelegate);
		base.Subscribe<Edible>(1272413801, Edible.OnCraftDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Eating;
		this.synchronizeAnims = false;
		Components.Edibles.Add(this);
	}

	// Token: 0x06003063 RID: 12387 RVA: 0x002090C4 File Offset: 0x002072C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ToggleGenericSpicedTag(base.gameObject.HasTag(GameTags.SpicedFood));
		if (this.spices != null)
		{
			for (int i = 0; i < this.spices.Count; i++)
			{
				this.ApplySpiceEffects(this.spices[i], SpiceGrinderConfig.SpicedStatus);
			}
		}
		if (base.GetComponent<KPrefabID>().HasTag(GameTags.Rehydrated))
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.RehydratedFood, null);
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.Edible, this);
	}

	// Token: 0x06003064 RID: 12388 RVA: 0x0020917C File Offset: 0x0020737C
	public override HashedString[] GetWorkAnims(WorkerBase worker)
	{
		EatChore.StatesInstance smi = worker.GetSMI<EatChore.StatesInstance>();
		bool flag = smi != null && smi.UseSalt();
		MinionResume component = worker.GetComponent<MinionResume>();
		if (component != null && component.CurrentHat != null)
		{
			if (!flag)
			{
				return Edible.hatWorkAnims;
			}
			return Edible.saltHatWorkAnims;
		}
		else
		{
			if (!flag)
			{
				return Edible.normalWorkAnims;
			}
			return Edible.saltWorkAnims;
		}
	}

	// Token: 0x06003065 RID: 12389 RVA: 0x002091D4 File Offset: 0x002073D4
	public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		EatChore.StatesInstance smi = worker.GetSMI<EatChore.StatesInstance>();
		bool flag = smi != null && smi.UseSalt();
		MinionResume component = worker.GetComponent<MinionResume>();
		if (component != null && component.CurrentHat != null)
		{
			if (!flag)
			{
				return Edible.hatWorkPstAnim;
			}
			return Edible.saltHatWorkPstAnim;
		}
		else
		{
			if (!flag)
			{
				return Edible.normalWorkPstAnim;
			}
			return Edible.saltWorkPstAnim;
		}
	}

	// Token: 0x06003066 RID: 12390 RVA: 0x000C3E98 File Offset: 0x000C2098
	private void OnCraft(object data)
	{
		WorldResourceAmountTracker<RationTracker>.Get().RegisterAmountProduced(this.Calories);
	}

	// Token: 0x06003067 RID: 12391 RVA: 0x0020922C File Offset: 0x0020742C
	public float GetFeedingTime(WorkerBase worker)
	{
		float num = this.Calories * 2E-05f;
		if (worker != null)
		{
			BingeEatChore.StatesInstance smi = worker.GetSMI<BingeEatChore.StatesInstance>();
			if (smi != null && smi.IsBingeEating())
			{
				num /= 2f;
			}
		}
		return num;
	}

	// Token: 0x06003068 RID: 12392 RVA: 0x0020926C File Offset: 0x0020746C
	protected override void OnStartWork(WorkerBase worker)
	{
		this.totalFeedingTime = this.GetFeedingTime(worker);
		base.SetWorkTime(this.totalFeedingTime);
		this.caloriesConsumed = 0f;
		this.unitsConsumed = 0f;
		this.totalUnits = this.Units;
		worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		this.totalConsumableCalories = this.Units * this.foodInfo.CaloriesPerUnit;
		this.StartConsuming();
	}

	// Token: 0x06003069 RID: 12393 RVA: 0x002092E4 File Offset: 0x002074E4
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.currentlyLit)
		{
			if (this.currentModifier != this.caloriesLitSpaceModifier)
			{
				worker.GetAttributes().Remove(this.currentModifier);
				worker.GetAttributes().Add(this.caloriesLitSpaceModifier);
				this.currentModifier = this.caloriesLitSpaceModifier;
			}
		}
		else if (this.currentModifier != this.caloriesModifier)
		{
			worker.GetAttributes().Remove(this.currentModifier);
			worker.GetAttributes().Add(this.caloriesModifier);
			this.currentModifier = this.caloriesModifier;
		}
		return this.OnTickConsume(worker, dt);
	}

	// Token: 0x0600306A RID: 12394 RVA: 0x000C3EAA File Offset: 0x000C20AA
	protected override void OnStopWork(WorkerBase worker)
	{
		if (this.currentModifier != null)
		{
			worker.GetAttributes().Remove(this.currentModifier);
			this.currentModifier = null;
		}
		worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
		this.StopConsuming(worker);
	}

	// Token: 0x0600306B RID: 12395 RVA: 0x0020937C File Offset: 0x0020757C
	private bool OnTickConsume(WorkerBase worker, float dt)
	{
		if (!this.isBeingConsumed)
		{
			DebugUtil.DevLogError("OnTickConsume while we're not eating, this would set a NaN mass on this Edible");
			return true;
		}
		bool result = false;
		float num = dt / this.totalFeedingTime;
		float num2 = num * this.totalConsumableCalories;
		if (this.caloriesConsumed + num2 > this.totalConsumableCalories)
		{
			num2 = this.totalConsumableCalories - this.caloriesConsumed;
		}
		this.caloriesConsumed += num2;
		worker.GetAmounts().Get("Calories").value += num2;
		float num3 = this.totalUnits * num;
		if (this.Units - num3 < 0f)
		{
			num3 = this.Units;
		}
		this.Units -= num3;
		this.unitsConsumed += num3;
		if (this.Units <= 0f)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x0600306C RID: 12396 RVA: 0x000C3EE3 File Offset: 0x000C20E3
	public void SpiceEdible(SpiceInstance spice, StatusItem status)
	{
		this.spices.Add(spice);
		this.ApplySpiceEffects(spice, status);
	}

	// Token: 0x0600306D RID: 12397 RVA: 0x00209448 File Offset: 0x00207648
	protected virtual void ApplySpiceEffects(SpiceInstance spice, StatusItem status)
	{
		base.GetComponent<KPrefabID>().AddTag(spice.Id, true);
		this.ToggleGenericSpicedTag(true);
		base.GetComponent<KSelectable>().AddStatusItem(status, this.spices);
		if (spice.FoodModifier != null)
		{
			base.gameObject.GetAttributes().Add(spice.FoodModifier);
		}
		if (spice.CalorieModifier != null)
		{
			this.Calories += spice.CalorieModifier.Value;
		}
	}

	// Token: 0x0600306E RID: 12398 RVA: 0x002094C4 File Offset: 0x002076C4
	private void ToggleGenericSpicedTag(bool isSpiced)
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (isSpiced)
		{
			component.RemoveTag(GameTags.UnspicedFood);
			component.AddTag(GameTags.SpicedFood, true);
			return;
		}
		component.RemoveTag(GameTags.SpicedFood);
		component.AddTag(GameTags.UnspicedFood, false);
	}

	// Token: 0x0600306F RID: 12399 RVA: 0x0020950C File Offset: 0x0020770C
	public bool CanAbsorb(Edible other)
	{
		bool flag = this.spices.Count == other.spices.Count;
		flag &= (base.gameObject.HasTag(GameTags.Rehydrated) == other.gameObject.HasTag(GameTags.Rehydrated));
		flag &= (!base.gameObject.HasTag(GameTags.Dehydrated) && !other.gameObject.HasTag(GameTags.Dehydrated));
		int num = 0;
		while (flag && num < this.spices.Count)
		{
			int num2 = 0;
			while (flag && num2 < other.spices.Count)
			{
				flag = (this.spices[num].Id == other.spices[num2].Id);
				num2++;
			}
			num++;
		}
		return flag;
	}

	// Token: 0x06003070 RID: 12400 RVA: 0x000C3EF9 File Offset: 0x000C20F9
	private void StartConsuming()
	{
		DebugUtil.DevAssert(!this.isBeingConsumed, "Can't StartConsuming()...we've already started", null);
		this.isBeingConsumed = true;
		base.worker.Trigger(1406130139, this);
	}

	// Token: 0x06003071 RID: 12401 RVA: 0x002095E0 File Offset: 0x002077E0
	private void StopConsuming(WorkerBase worker)
	{
		DebugUtil.DevAssert(this.isBeingConsumed, "StopConsuming() called without StartConsuming()", null);
		this.isBeingConsumed = false;
		for (int i = 0; i < this.foodInfo.Effects.Count; i++)
		{
			worker.GetComponent<Effects>().Add(this.foodInfo.Effects[i], true);
		}
		ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -this.caloriesConsumed, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.EATEN, "{0}", this.GetProperName()), worker.GetProperName());
		this.AddOnConsumeEffects(worker);
		worker.Trigger(1121894420, this);
		base.Trigger(-10536414, worker.gameObject);
		this.unitsConsumed = float.NaN;
		this.caloriesConsumed = float.NaN;
		this.totalUnits = float.NaN;
		if (this.Units < 0.001f)
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x06003072 RID: 12402 RVA: 0x000C3F27 File Offset: 0x000C2127
	public static string GetEffectForFoodQuality(int qualityLevel)
	{
		qualityLevel = Mathf.Clamp(qualityLevel, -1, 5);
		return Edible.qualityEffects[qualityLevel];
	}

	// Token: 0x06003073 RID: 12403 RVA: 0x002096D0 File Offset: 0x002078D0
	private void AddOnConsumeEffects(WorkerBase worker)
	{
		int num = Mathf.RoundToInt(worker.GetAttributes().Add(Db.Get().Attributes.FoodExpectation).GetTotalValue());
		int qualityLevel = this.FoodInfo.Quality + num;
		Effects component = worker.GetComponent<Effects>();
		component.Add(Edible.GetEffectForFoodQuality(qualityLevel), true);
		for (int i = 0; i < this.spices.Count; i++)
		{
			Effect statBonus = this.spices[i].StatBonus;
			if (statBonus != null)
			{
				float duration = statBonus.duration;
				statBonus.duration = this.caloriesConsumed * 0.001f / 1000f * 600f;
				component.Add(statBonus, true);
				statBonus.duration = duration;
			}
		}
		if (base.gameObject.HasTag(GameTags.Rehydrated))
		{
			component.Add(FoodRehydratorConfig.RehydrationEffect, true);
		}
	}

	// Token: 0x06003074 RID: 12404 RVA: 0x000C3F3E File Offset: 0x000C213E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Edibles.Remove(this);
	}

	// Token: 0x06003075 RID: 12405 RVA: 0x000C3F51 File Offset: 0x000C2151
	public int GetQuality()
	{
		return this.foodInfo.Quality;
	}

	// Token: 0x06003076 RID: 12406 RVA: 0x002097B0 File Offset: 0x002079B0
	public int GetMorale()
	{
		int num = 0;
		string effectForFoodQuality = Edible.GetEffectForFoodQuality(this.foodInfo.Quality);
		foreach (AttributeModifier attributeModifier in Db.Get().effects.Get(effectForFoodQuality).SelfModifiers)
		{
			if (attributeModifier.AttributeId == Db.Get().Attributes.QualityOfLife.Id)
			{
				num += Mathf.RoundToInt(attributeModifier.Value);
			}
		}
		return num;
	}

	// Token: 0x06003077 RID: 12407 RVA: 0x00209850 File Offset: 0x00207A50
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.CALORIES, GameUtil.GetFormattedCalories(this.foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.CALORIES, GameUtil.GetFormattedCalories(this.foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), Descriptor.DescriptorType.Information, false));
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(this.foodInfo.Quality)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(this.foodInfo.Quality)), Descriptor.DescriptorType.Effect, false));
		int morale = this.GetMorale();
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.FOOD_MORALE, GameUtil.AddPositiveSign(morale.ToString(), morale > 0)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.FOOD_MORALE, GameUtil.AddPositiveSign(morale.ToString(), morale > 0)), Descriptor.DescriptorType.Effect, false));
		foreach (string text in this.foodInfo.Effects)
		{
			string text2 = "";
			foreach (AttributeModifier attributeModifier in Db.Get().effects.Get(text).SelfModifiers)
			{
				text2 = string.Concat(new string[]
				{
					text2,
					"\n    • ",
					Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME"),
					": ",
					attributeModifier.GetFormattedString()
				});
			}
			list.Add(new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".DESCRIPTION") + text2, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x06003078 RID: 12408 RVA: 0x00209AAC File Offset: 0x00207CAC
	public void ApplySpicesToOtherEdible(Edible other)
	{
		if (this.spices != null && other != null)
		{
			for (int i = 0; i < this.spices.Count; i++)
			{
				other.SpiceEdible(this.spices[i], SpiceGrinderConfig.SpicedStatus);
			}
		}
	}

	// Token: 0x06003079 RID: 12409 RVA: 0x00209AF8 File Offset: 0x00207CF8
	public void OnSplitTick(Pickupable thePieceTaken)
	{
		Edible component = thePieceTaken.GetComponent<Edible>();
		this.ApplySpicesToOtherEdible(component);
		if (base.GetComponent<KPrefabID>().HasTag(GameTags.Rehydrated))
		{
			component.AddTag(GameTags.Rehydrated);
		}
	}

	// Token: 0x04002133 RID: 8499
	private PrimaryElement primaryElement;

	// Token: 0x04002134 RID: 8500
	public string FoodID;

	// Token: 0x04002135 RID: 8501
	private EdiblesManager.FoodInfo foodInfo;

	// Token: 0x04002137 RID: 8503
	public float unitsConsumed = float.NaN;

	// Token: 0x04002138 RID: 8504
	public float caloriesConsumed = float.NaN;

	// Token: 0x04002139 RID: 8505
	private float totalFeedingTime = float.NaN;

	// Token: 0x0400213A RID: 8506
	private float totalUnits = float.NaN;

	// Token: 0x0400213B RID: 8507
	private float totalConsumableCalories = float.NaN;

	// Token: 0x0400213C RID: 8508
	[Serialize]
	private List<SpiceInstance> spices = new List<SpiceInstance>();

	// Token: 0x0400213D RID: 8509
	private AttributeModifier caloriesModifier = new AttributeModifier("CaloriesDelta", 50000f, DUPLICANTS.MODIFIERS.EATINGCALORIES.NAME, false, true, true);

	// Token: 0x0400213E RID: 8510
	private AttributeModifier caloriesLitSpaceModifier = new AttributeModifier("CaloriesDelta", (1f + DUPLICANTSTATS.STANDARD.Light.LIGHT_WORK_EFFICIENCY_BONUS) / 2E-05f, DUPLICANTS.MODIFIERS.EATINGCALORIES.NAME, false, true, true);

	// Token: 0x0400213F RID: 8511
	private AttributeModifier currentModifier;

	// Token: 0x04002140 RID: 8512
	private static readonly EventSystem.IntraObjectHandler<Edible> OnCraftDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		component.OnCraft(data);
	});

	// Token: 0x04002141 RID: 8513
	private static readonly HashedString[] normalWorkAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04002142 RID: 8514
	private static readonly HashedString[] hatWorkAnims = new HashedString[]
	{
		"hat_pre",
		"working_loop"
	};

	// Token: 0x04002143 RID: 8515
	private static readonly HashedString[] saltWorkAnims = new HashedString[]
	{
		"salt_pre",
		"salt_loop"
	};

	// Token: 0x04002144 RID: 8516
	private static readonly HashedString[] saltHatWorkAnims = new HashedString[]
	{
		"salt_hat_pre",
		"salt_hat_loop"
	};

	// Token: 0x04002145 RID: 8517
	private static readonly HashedString[] normalWorkPstAnim = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x04002146 RID: 8518
	private static readonly HashedString[] hatWorkPstAnim = new HashedString[]
	{
		"hat_pst"
	};

	// Token: 0x04002147 RID: 8519
	private static readonly HashedString[] saltWorkPstAnim = new HashedString[]
	{
		"salt_pst"
	};

	// Token: 0x04002148 RID: 8520
	private static readonly HashedString[] saltHatWorkPstAnim = new HashedString[]
	{
		"salt_hat_pst"
	};

	// Token: 0x04002149 RID: 8521
	private static Dictionary<int, string> qualityEffects = new Dictionary<int, string>
	{
		{
			-1,
			"EdibleMinus3"
		},
		{
			0,
			"EdibleMinus2"
		},
		{
			1,
			"EdibleMinus1"
		},
		{
			2,
			"Edible0"
		},
		{
			3,
			"Edible1"
		},
		{
			4,
			"Edible2"
		},
		{
			5,
			"Edible3"
		}
	};

	// Token: 0x02000A6C RID: 2668
	public class EdibleStartWorkInfo : WorkerBase.StartWorkInfo
	{
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600307C RID: 12412 RVA: 0x000C3F5E File Offset: 0x000C215E
		// (set) Token: 0x0600307D RID: 12413 RVA: 0x000C3F66 File Offset: 0x000C2166
		public float amount { get; private set; }

		// Token: 0x0600307E RID: 12414 RVA: 0x000C3F6F File Offset: 0x000C216F
		public EdibleStartWorkInfo(Workable workable, float amount) : base(workable)
		{
			this.amount = amount;
		}
	}
}
