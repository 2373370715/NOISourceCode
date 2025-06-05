using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020007F3 RID: 2035
public class ModifierSet : ScriptableObject
{
	// Token: 0x060023E2 RID: 9186 RVA: 0x001D3770 File Offset: 0x001D1970
	public virtual void Initialize()
	{
		this.ResourceTable = new List<Resource>();
		this.Root = new ResourceSet<Resource>("Root", null);
		this.modifierInfos = new ModifierSet.ModifierInfos();
		this.modifierInfos.Load(this.modifiersFile);
		this.Attributes = new Database.Attributes(this.Root);
		this.BuildingAttributes = new BuildingAttributes(this.Root);
		this.CritterAttributes = new CritterAttributes(this.Root);
		this.PlantAttributes = new PlantAttributes(this.Root);
		this.effects = new ResourceSet<Effect>("Effects", this.Root);
		this.traits = new ModifierSet.TraitSet();
		this.traitGroups = new ModifierSet.TraitGroupSet();
		this.FertilityModifiers = new FertilityModifiers();
		this.Amounts = new Database.Amounts();
		this.Amounts.Load();
		this.AttributeConverters = new Database.AttributeConverters();
		this.LoadEffects();
		this.LoadFertilityModifiers();
	}

	// Token: 0x060023E3 RID: 9187 RVA: 0x000BBC62 File Offset: 0x000B9E62
	public static float ConvertValue(float value, Units units)
	{
		if (Units.PerDay == units)
		{
			return value * 0.0016666667f;
		}
		return value;
	}

	// Token: 0x060023E4 RID: 9188 RVA: 0x001D3860 File Offset: 0x001D1A60
	private void LoadEffects()
	{
		foreach (ModifierSet.ModifierInfo modifierInfo in this.modifierInfos)
		{
			if (!this.effects.Exists(modifierInfo.Id) && (modifierInfo.Type == "Effect" || modifierInfo.Type == "Base" || modifierInfo.Type == "Need"))
			{
				string text = Strings.Get(string.Format("STRINGS.DUPLICANTS.MODIFIERS.{0}.NAME", modifierInfo.Id.ToUpper()));
				string description = Strings.Get(string.Format("STRINGS.DUPLICANTS.MODIFIERS.{0}.TOOLTIP", modifierInfo.Id.ToUpper()));
				Effect effect = new Effect(modifierInfo.Id, text, description, modifierInfo.Duration * 600f, modifierInfo.ShowInUI && modifierInfo.Type != "Need", modifierInfo.TriggerFloatingText, modifierInfo.IsBad, modifierInfo.EmoteAnim, modifierInfo.EmoteCooldown, modifierInfo.StompGroup, modifierInfo.CustomIcon);
				effect.stompPriority = modifierInfo.StompPriority;
				foreach (ModifierSet.ModifierInfo modifierInfo2 in this.modifierInfos)
				{
					if (modifierInfo2.Id == modifierInfo.Id)
					{
						effect.Add(new AttributeModifier(modifierInfo2.Attribute, ModifierSet.ConvertValue(modifierInfo2.Value, modifierInfo2.Units), text, modifierInfo2.Multiplier, false, true));
					}
				}
				this.effects.Add(effect);
			}
		}
		Reactable.ReactablePrecondition precon = delegate(GameObject go, Navigator.ActiveTransition n)
		{
			int cell = Grid.PosToCell(go);
			return Grid.IsValidCell(cell) && Grid.IsGas(cell);
		};
		this.effects.Get("WetFeet").AddEmotePrecondition(precon);
		this.effects.Get("SoakingWet").AddEmotePrecondition(precon);
		Effect effect2 = new Effect("PassedOutSleep", DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.NAME, DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.TOOLTIP, 0f, true, true, true, null, 0f, null, true, "status_item_exhausted", -1f);
		effect2.Add(new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, 0.6666667f, DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.NAME, false, false, true));
		effect2.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.033333335f, DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.NAME, false, false, true));
		this.effects.Add(effect2);
		Effect resource = new Effect("WarmTouch", DUPLICANTS.MODIFIERS.WARMTOUCH.NAME, DUPLICANTS.MODIFIERS.WARMTOUCH.TOOLTIP, 120f, new string[]
		{
			"WetFeet"
		}, true, true, false, null, 0f, null, false, "", -1f);
		this.effects.Add(resource);
		Effect resource2 = new Effect("WarmTouchFood", DUPLICANTS.MODIFIERS.WARMTOUCHFOOD.NAME, DUPLICANTS.MODIFIERS.WARMTOUCHFOOD.TOOLTIP, 600f, new string[]
		{
			"WetFeet"
		}, true, true, false, null, 0f, null, false, "", -1f);
		this.effects.Add(resource2);
		Effect resource3 = new Effect("RefreshingTouch", DUPLICANTS.MODIFIERS.REFRESHINGTOUCH.NAME, DUPLICANTS.MODIFIERS.REFRESHINGTOUCH.TOOLTIP, 120f, true, true, false, null, -1f, 0f, null, "");
		this.effects.Add(resource3);
		Effect effect3 = new Effect("GunkSick", DUPLICANTS.MODIFIERS.GUNKSICK.NAME, DUPLICANTS.MODIFIERS.GUNKSICK.TOOLTIP, 0f, true, true, true, null, -1f, 0f, null, "");
		effect3.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.033333335f, DUPLICANTS.MODIFIERS.GUNKSICK.NAME, false, false, true));
		this.effects.Add(effect3);
		Effect effect4 = new Effect("ExpellingGunk", DUPLICANTS.MODIFIERS.EXPELLINGGUNK.NAME, DUPLICANTS.MODIFIERS.EXPELLINGGUNK.TOOLTIP, 0f, true, true, true, null, -1f, 0f, null, "");
		effect4.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.083333336f, DUPLICANTS.MODIFIERS.GUNKSICK.NAME, false, false, true));
		this.effects.Add(effect4);
		Effect effect5 = new Effect("GunkHungover", DUPLICANTS.MODIFIERS.GUNKHUNGOVER.NAME, DUPLICANTS.MODIFIERS.GUNKHUNGOVER.TOOLTIP, 600f, true, false, true, null, -1f, 0f, null, "");
		effect5.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.033333335f, DUPLICANTS.MODIFIERS.GUNKHUNGOVER.NAME, false, false, true));
		this.effects.Add(effect5);
		Effect effect6 = new Effect("NoLubricationMinor", DUPLICANTS.MODIFIERS.NOLUBRICATIONMINOR.NAME, DUPLICANTS.MODIFIERS.NOLUBRICATIONMINOR.TOOLTIP, 0f, true, true, true, null, -1f, 0f, null, "");
		effect6.Add(new AttributeModifier(Db.Get().Attributes.Athletics.Id, -4f, DUPLICANTS.MODIFIERS.NOLUBRICATIONMINOR.NAME, false, false, true));
		effect6.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.025f, DUPLICANTS.MODIFIERS.NOLUBRICATIONMINOR.NAME, false, false, true));
		this.effects.Add(effect6);
		Effect effect7 = new Effect("NoLubricationMajor", DUPLICANTS.MODIFIERS.NOLUBRICATIONMAJOR.NAME, DUPLICANTS.MODIFIERS.NOLUBRICATIONMAJOR.TOOLTIP, 0f, true, true, true, null, -1f, 0f, null, "");
		effect7.Add(new AttributeModifier(Db.Get().Attributes.Athletics.Id, -8f, DUPLICANTS.MODIFIERS.NOLUBRICATIONMAJOR.NAME, false, false, true));
		effect7.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.05f, DUPLICANTS.MODIFIERS.NOLUBRICATIONMINOR.NAME, false, false, true));
		this.effects.Add(effect7);
		Effect effect8 = new Effect("BionicOffline", DUPLICANTS.MODIFIERS.BIONICOFFLINE.NAME, DUPLICANTS.MODIFIERS.BIONICOFFLINE.TOOLTIP, 0f, false, true, true, null, -1f, 0f, null, "");
		effect8.Add(new AttributeModifier(Db.Get().Amounts.BionicOil.deltaAttribute.Id, 0f, DUPLICANTS.MODIFIERS.BIONICOFFLINE.NAME, false, false, true));
		this.effects.Add(effect8);
		Effect effect9 = new Effect("BionicBedTimeEffect", DUPLICANTS.MODIFIERS.BIONICBEDTIMEEFFECT.NAME, DUPLICANTS.MODIFIERS.BIONICBEDTIMEEFFECT.TOOLTIP, 0f, false, false, false, null, -1f, 0f, null, "");
		effect9.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.033333335f, DUPLICANTS.MODIFIERS.BIONICBEDTIMEEFFECT.NAME, false, false, true));
		this.effects.Add(effect9);
		Effect effect10 = new Effect("BionicWaterStress", DUPLICANTS.MODIFIERS.BIONICWATERSTRESS.NAME, DUPLICANTS.MODIFIERS.BIONICWATERSTRESS.TOOLTIP, 0f, true, true, true, null, -1f, 0f, null, "");
		effect10.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.33333334f, DUPLICANTS.MODIFIERS.BIONICWATERSTRESS.NAME, false, false, true));
		this.effects.Add(effect10);
		Effect resource4 = new Effect("RecentlySlippedTracker", DUPLICANTS.MODIFIERS.SLIPPED.NAME, DUPLICANTS.MODIFIERS.SLIPPED.TOOLTIP, 100f, false, false, true, null, -1f, 0f, null, "");
		this.effects.Add(resource4);
		foreach (Effect resource5 in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Values)
		{
			this.effects.Add(resource5);
		}
		this.CreateRoomEffects();
		this.CreateCritteEffects();
	}

	// Token: 0x060023E5 RID: 9189 RVA: 0x000AA038 File Offset: 0x000A8238
	private void CreateRoomEffects()
	{
	}

	// Token: 0x060023E6 RID: 9190 RVA: 0x001D4154 File Offset: 0x001D2354
	public void CreateCritteEffects()
	{
		Effect effect = new Effect("Ranched", STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, STRINGS.CREATURES.MODIFIERS.RANCHED.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, false, false, true));
		effect.Add(new AttributeModifier(Db.Get().Amounts.Wildness.deltaAttribute.Id, -0.09166667f, STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, false, false, true));
		this.effects.Add(effect);
		Effect effect2 = new Effect("HadMilk", STRINGS.CREATURES.MODIFIERS.GOTMILK.NAME, STRINGS.CREATURES.MODIFIERS.GOTMILK.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect2.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, STRINGS.CREATURES.MODIFIERS.GOTMILK.NAME, false, false, true));
		this.effects.Add(effect2);
		Effect effect3 = new Effect("EggSong", STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.NAME, STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.TOOLTIP, 600f, true, false, false, null, -1f, 0f, null, "");
		effect3.Add(new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, 4f, STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.NAME, true, false, true));
		this.effects.Add(effect3);
		Effect effect4 = new Effect("EggHug", STRINGS.CREATURES.MODIFIERS.EGGHUG.NAME, STRINGS.CREATURES.MODIFIERS.EGGHUG.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect4.Add(new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, 1f, STRINGS.CREATURES.MODIFIERS.EGGHUG.NAME, true, false, true));
		this.effects.Add(effect4);
		Effect resource = new Effect("HuggingFrenzy", STRINGS.CREATURES.MODIFIERS.HUGGINGFRENZY.NAME, STRINGS.CREATURES.MODIFIERS.HUGGINGFRENZY.TOOLTIP, 600f, true, false, false, null, -1f, 0f, null, "");
		this.effects.Add(resource);
		Effect effect5 = new Effect("DivergentCropTended", STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.NAME, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect5.Add(new AttributeModifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.05f, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.NAME, true, false, true));
		this.effects.Add(effect5);
		Effect effect6 = new Effect("DivergentCropTendedWorm", STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.NAME, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect6.Add(new AttributeModifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.5f, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.NAME, true, false, true));
		this.effects.Add(effect6);
		Effect effect7 = new Effect("MooWellFed", STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.MOOWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect7.Add(new AttributeModifier(Db.Get().Amounts.Beckoning.deltaAttribute.Id, MooTuning.WELLFED_EFFECT, STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME, false, false, true));
		effect7.Add(new AttributeModifier(Db.Get().Amounts.MilkProduction.deltaAttribute.Id, MooTuning.MILK_PRODUCTION_PERCENTAGE_PER_SECOND, STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME, false, false, true));
		this.effects.Add(effect7);
		Effect effect8 = new Effect("WoodDeerWellFed", STRINGS.CREATURES.MODIFIERS.WOODDEERWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.WOODDEERWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect8.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 100f / (WoodDeerConfig.ANTLER_GROWTH_TIME_IN_CYCLES * 600f), STRINGS.CREATURES.MODIFIERS.WOODDEERWELLFED.NAME, false, false, true));
		this.effects.Add(effect8);
		Effect effect9 = new Effect("IceBellyWellFed", STRINGS.CREATURES.MODIFIERS.ICEBELLYWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.ICEBELLYWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect9.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 100f / (IceBellyConfig.SCALE_GROWTH_TIME_IN_CYCLES * 600f), STRINGS.CREATURES.MODIFIERS.ICEBELLYWELLFED.NAME, false, false, true));
		this.effects.Add(effect9);
		Effect effect10 = new Effect("GoldBellyWellFed", STRINGS.CREATURES.MODIFIERS.GOLDBELLYWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.GOLDBELLYWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect10.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 100f / (GoldBellyConfig.SCALE_GROWTH_TIME_IN_CYCLES * 600f), STRINGS.CREATURES.MODIFIERS.GOLDBELLYWELLFED.NAME, false, false, true));
		this.effects.Add(effect10);
	}

	// Token: 0x060023E7 RID: 9191 RVA: 0x001D4710 File Offset: 0x001D2910
	public Trait CreateTrait(string id, string name, string description, string group_name, bool should_save, ChoreGroup[] disabled_chore_groups, bool positive_trait, bool is_valid_starter_trait)
	{
		return this.CreateTrait(id, name, description, group_name, should_save, disabled_chore_groups, positive_trait, is_valid_starter_trait, null, null);
	}

	// Token: 0x060023E8 RID: 9192 RVA: 0x001D4734 File Offset: 0x001D2934
	public Trait CreateTrait(string id, string name, string description, string group_name, bool should_save, ChoreGroup[] disabled_chore_groups, bool positive_trait, bool is_valid_starter_trait, string[] requiredDlcIds, string[] forbiddenDlcIds)
	{
		Trait trait = new Trait(id, name, description, 0f, should_save, disabled_chore_groups, positive_trait, is_valid_starter_trait, requiredDlcIds, forbiddenDlcIds);
		this.traits.Add(trait);
		if (group_name == "" || group_name == null)
		{
			group_name = "Default";
		}
		TraitGroup traitGroup = this.traitGroups.TryGet(group_name);
		if (traitGroup == null)
		{
			traitGroup = new TraitGroup(group_name, group_name, group_name != "Default");
			this.traitGroups.Add(traitGroup);
		}
		traitGroup.Add(trait);
		return trait;
	}

	// Token: 0x060023E9 RID: 9193 RVA: 0x001D47C0 File Offset: 0x001D29C0
	public FertilityModifier CreateFertilityModifier(string id, Tag targetTag, string name, string description, Func<string, string> tooltipCB, FertilityModifier.FertilityModFn applyFunction)
	{
		FertilityModifier fertilityModifier = new FertilityModifier(id, targetTag, name, description, tooltipCB, applyFunction);
		this.FertilityModifiers.Add(fertilityModifier);
		return fertilityModifier;
	}

	// Token: 0x060023EA RID: 9194 RVA: 0x000BBC71 File Offset: 0x000B9E71
	protected void LoadTraits()
	{
		TRAITS.TRAIT_CREATORS.ForEach(delegate(System.Action action)
		{
			action();
		});
	}

	// Token: 0x060023EB RID: 9195 RVA: 0x000BBC9C File Offset: 0x000B9E9C
	protected void LoadFertilityModifiers()
	{
		TUNING.CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.ForEach(delegate(System.Action action)
		{
			action();
		});
	}

	// Token: 0x04001835 RID: 6197
	public TextAsset modifiersFile;

	// Token: 0x04001836 RID: 6198
	public ModifierSet.ModifierInfos modifierInfos;

	// Token: 0x04001837 RID: 6199
	public ModifierSet.TraitSet traits;

	// Token: 0x04001838 RID: 6200
	public ResourceSet<Effect> effects;

	// Token: 0x04001839 RID: 6201
	public ModifierSet.TraitGroupSet traitGroups;

	// Token: 0x0400183A RID: 6202
	public FertilityModifiers FertilityModifiers;

	// Token: 0x0400183B RID: 6203
	public Database.Attributes Attributes;

	// Token: 0x0400183C RID: 6204
	public BuildingAttributes BuildingAttributes;

	// Token: 0x0400183D RID: 6205
	public CritterAttributes CritterAttributes;

	// Token: 0x0400183E RID: 6206
	public PlantAttributes PlantAttributes;

	// Token: 0x0400183F RID: 6207
	public Database.Amounts Amounts;

	// Token: 0x04001840 RID: 6208
	public Database.AttributeConverters AttributeConverters;

	// Token: 0x04001841 RID: 6209
	public ResourceSet Root;

	// Token: 0x04001842 RID: 6210
	public List<Resource> ResourceTable;

	// Token: 0x020007F4 RID: 2036
	public class ModifierInfo : Resource
	{
		// Token: 0x04001843 RID: 6211
		public string Type;

		// Token: 0x04001844 RID: 6212
		public string Attribute;

		// Token: 0x04001845 RID: 6213
		public float Value;

		// Token: 0x04001846 RID: 6214
		public Units Units;

		// Token: 0x04001847 RID: 6215
		public bool Multiplier;

		// Token: 0x04001848 RID: 6216
		public float Duration;

		// Token: 0x04001849 RID: 6217
		public bool ShowInUI;

		// Token: 0x0400184A RID: 6218
		public string StompGroup;

		// Token: 0x0400184B RID: 6219
		public int StompPriority;

		// Token: 0x0400184C RID: 6220
		public bool IsBad;

		// Token: 0x0400184D RID: 6221
		public string CustomIcon;

		// Token: 0x0400184E RID: 6222
		public bool TriggerFloatingText;

		// Token: 0x0400184F RID: 6223
		public string EmoteAnim;

		// Token: 0x04001850 RID: 6224
		public float EmoteCooldown;
	}

	// Token: 0x020007F5 RID: 2037
	[Serializable]
	public class ModifierInfos : ResourceLoader<ModifierSet.ModifierInfo>
	{
	}

	// Token: 0x020007F6 RID: 2038
	[Serializable]
	public class TraitSet : ResourceSet<Trait>
	{
	}

	// Token: 0x020007F7 RID: 2039
	[Serializable]
	public class TraitGroupSet : ResourceSet<TraitGroup>
	{
	}
}
