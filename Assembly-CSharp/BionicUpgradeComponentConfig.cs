using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class BionicUpgradeComponentConfig : IMultiEntityConfig
{
	public static string GenerateTooltipForBooster(BionicUpgradeComponent booster)
	{
		string str = "<b>" + booster.GetProperName() + "</b>";
		InfoDescription component = booster.gameObject.GetComponent<InfoDescription>();
		if (component != null)
		{
			str = str + "\n" + component.description;
		}
		return str + "\n\n" + BionicUpgradeComponentConfig.UpgradesData[booster.PrefabID()].stateMachineDescription;
	}

	public static Tag[] GetBoostersWithSkillPerk(string perkID)
	{
		return (from data in BionicUpgradeComponentConfig.UpgradesData
		where data.Value.skillPerks.Contains(perkID)
		select data into kvp
		select kvp.Key).ToArray<Tag>();
	}

	public AttributeModifier[] CreateBoosterModifiers(string name, Dictionary<string, float> attributes)
	{
		AttributeModifier[] array = new AttributeModifier[attributes.Count];
		string description = Strings.Get("STRINGS.ITEMS.BIONIC_BOOSTERS." + name.ToUpper() + ".NAME");
		int num = 0;
		foreach (KeyValuePair<string, float> keyValuePair in attributes)
		{
			Klei.AI.Attribute attribute = Db.Get().Attributes.Get(keyValuePair.Key);
			array[num] = new AttributeModifier(attribute.Id, keyValuePair.Value, description, false, false, true);
			num++;
		}
		return array;
	}

	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		if (!DlcManager.IsContentSubscribed("DLC3_ID"))
		{
			return list;
		}
		BionicUpgradeComponentConfig.<>c__DisplayClass27_0 CS$<>8__locals1 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_0();
		string text = "Booster_Dig1";
		AttributeModifier[] array = this.CreateBoosterModifiers(text, new Dictionary<string, float>
		{
			{
				Db.Get().Attributes.Digging.Id,
				5f
			},
			{
				Db.Get().Attributes.Athletics.Id,
				2f
			}
		});
		SkillPerk[] skillPerks = new SkillPerk[]
		{
			Db.Get().SkillPerks.CanDigVeryFirm
		};
		BionicUpgradeComponentConfig.<>c__DisplayClass27_0 CS$<>8__locals2 = CS$<>8__locals1;
		string upgradeID = text;
		AttributeModifier[] modifiers = array;
		CS$<>8__locals2.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID, Db.Get().Attributes.Digging.Id, modifiers, skillPerks, new string[]
		{
			"hat_role_mining1",
			"hat_role_mining2"
		});
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals1.skill_worker_def), CS$<>8__locals1.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "basic_excavation_0", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Basic, true, true, skillPerks));
		BionicUpgradeComponentConfig.<>c__DisplayClass27_1 CS$<>8__locals3 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_1();
		string text2 = "Booster_Construct1";
		AttributeModifier[] array2 = this.CreateBoosterModifiers(text2, new Dictionary<string, float>
		{
			{
				Db.Get().Attributes.Construction.Id,
				5f
			},
			{
				Db.Get().Attributes.Athletics.Id,
				2f
			}
		});
		SkillPerk[] skillPerks2 = new SkillPerk[]
		{
			Db.Get().SkillPerks.CanDemolish
		};
		BionicUpgradeComponentConfig.<>c__DisplayClass27_1 CS$<>8__locals4 = CS$<>8__locals3;
		string upgradeID2 = text2;
		modifiers = array2;
		CS$<>8__locals4.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID2, Db.Get().Attributes.Construction.Id, modifiers, skillPerks2, new string[]
		{
			"hat_role_building1",
			"hat_role_building2",
			"hat_role_building3"
		});
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text2, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals3.skill_worker_def), CS$<>8__locals3.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "basic_construction_0", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Basic, true, true, skillPerks2));
		BionicUpgradeComponentConfig.<>c__DisplayClass27_2 CS$<>8__locals5 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_2();
		string text3 = "Booster_Carry1";
		AttributeModifier[] array3 = this.CreateBoosterModifiers(text3, new Dictionary<string, float>
		{
			{
				Db.Get().Attributes.Strength.Id,
				5f
			},
			{
				Db.Get().Attributes.Athletics.Id,
				2f
			}
		});
		SkillPerk[] skillPerks3 = new SkillPerk[]
		{
			Db.Get().SkillPerks.IncreasedCarryBionics
		};
		BionicUpgradeComponentConfig.<>c__DisplayClass27_2 CS$<>8__locals6 = CS$<>8__locals5;
		string upgradeID3 = text3;
		modifiers = array3;
		CS$<>8__locals6.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID3, Db.Get().Attributes.Athletics.Id, modifiers, skillPerks3, new string[]
		{
			"hat_role_hauling1",
			"hat_role_hauling2"
		});
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text3, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals5.skill_worker_def), CS$<>8__locals5.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "basic_strength_0", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Basic, false, true, skillPerks3));
		BionicUpgradeComponentConfig.<>c__DisplayClass27_3 CS$<>8__locals7 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_3();
		string text4 = "Booster_Research1";
		AttributeModifier[] array4 = this.CreateBoosterModifiers(text4, new Dictionary<string, float>
		{
			{
				Db.Get().Attributes.Learning.Id,
				5f
			},
			{
				Db.Get().Attributes.Athletics.Id,
				2f
			}
		});
		SkillPerk[] skillPerks4 = new SkillPerk[]
		{
			Db.Get().SkillPerks.AllowAdvancedResearch,
			Db.Get().SkillPerks.CanStudyWorldObjects,
			Db.Get().SkillPerks.AllowGeyserTuning,
			Db.Get().SkillPerks.AllowChemistry
		};
		BionicUpgradeComponentConfig.<>c__DisplayClass27_3 CS$<>8__locals8 = CS$<>8__locals7;
		string upgradeID4 = text4;
		modifiers = array4;
		CS$<>8__locals8.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID4, Db.Get().Attributes.Learning.Id, modifiers, skillPerks4, new string[]
		{
			"hat_role_research1",
			"hat_role_research2"
		});
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text4, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals7.skill_worker_def), CS$<>8__locals7.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "science_4", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Basic, false, true, skillPerks4));
		BionicUpgradeComponentConfig.<>c__DisplayClass27_4 CS$<>8__locals9 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_4();
		string text5 = "Booster_Medicine1";
		AttributeModifier[] array5 = this.CreateBoosterModifiers(text5, new Dictionary<string, float>
		{
			{
				Db.Get().Attributes.Caring.Id,
				5f
			},
			{
				Db.Get().Attributes.Athletics.Id,
				2f
			}
		});
		SkillPerk[] skillPerks5 = new SkillPerk[]
		{
			Db.Get().SkillPerks.CanCompound,
			Db.Get().SkillPerks.CanDoctor,
			Db.Get().SkillPerks.CanAdvancedMedicine
		};
		BionicUpgradeComponentConfig.<>c__DisplayClass27_4 CS$<>8__locals10 = CS$<>8__locals9;
		string upgradeID5 = text5;
		modifiers = array5;
		CS$<>8__locals10.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID5, Db.Get().Attributes.DoctoredLevel.Id, modifiers, skillPerks5, new string[]
		{
			"hat_role_medicalaid1",
			"hat_role_medicalaid2",
			"hat_role_medicalaid3"
		});
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text5, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals9.skill_worker_def), CS$<>8__locals9.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.CRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "medicine_0", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Basic, true, true, skillPerks5));
		BionicUpgradeComponentConfig.<>c__DisplayClass27_5 CS$<>8__locals11 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_5();
		string text6 = "Booster_Dig2";
		SkillPerk[] array7;
		if (!DlcManager.IsExpansion1Active())
		{
			SkillPerk[] array6 = new SkillPerk[2];
			array6[0] = Db.Get().SkillPerks.CanDigNearlyImpenetrable;
			array7 = array6;
			array6[1] = Db.Get().SkillPerks.CanDigSuperDuperHard;
		}
		else
		{
			SkillPerk[] array8 = new SkillPerk[3];
			array8[0] = Db.Get().SkillPerks.CanDigNearlyImpenetrable;
			array8[1] = Db.Get().SkillPerks.CanDigSuperDuperHard;
			array7 = array8;
			array8[2] = Db.Get().SkillPerks.CanDigRadioactiveMaterials;
		}
		SkillPerk[] skillPerks6 = array7;
		AttributeModifier[] array9 = this.CreateBoosterModifiers(text6, new Dictionary<string, float>
		{
			{
				Db.Get().Attributes.Digging.Id,
				5f
			},
			{
				Db.Get().Attributes.Athletics.Id,
				2f
			}
		});
		string[] array10;
		if (!DlcManager.IsExpansion1Active())
		{
			(array10 = new string[1])[0] = "hat_role_mining3";
		}
		else
		{
			string[] array11 = new string[2];
			array11[0] = "hat_role_mining3";
			array10 = array11;
			array11[1] = "hat_role_mining4";
		}
		string[] hats = array10;
		BionicUpgradeComponentConfig.<>c__DisplayClass27_5 CS$<>8__locals12 = CS$<>8__locals11;
		string upgradeID6 = text6;
		modifiers = array9;
		CS$<>8__locals12.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID6, Db.Get().Attributes.Digging.Id, modifiers, skillPerks6, hats);
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text6, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals11.skill_worker_def), CS$<>8__locals11.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "excavation_1", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Intermediate, true, true, skillPerks6));
		BionicUpgradeComponentConfig.<>c__DisplayClass27_6 CS$<>8__locals13 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_6();
		string text7 = "Booster_Farm1";
		List<SkillPerk> list2 = new List<SkillPerk>
		{
			Db.Get().SkillPerks.CanFarmTinker,
			Db.Get().SkillPerks.CanFarmStation
		};
		if (DlcManager.IsExpansion1Active())
		{
			list2.Add(Db.Get().SkillPerks.CanIdentifyMutantSeeds);
		}
		AttributeModifier[] array12 = this.CreateBoosterModifiers(text7, new Dictionary<string, float>
		{
			{
				Db.Get().Attributes.Botanist.Id,
				5f
			},
			{
				Db.Get().Attributes.Athletics.Id,
				2f
			}
		});
		BionicUpgradeComponentConfig.<>c__DisplayClass27_6 CS$<>8__locals14 = CS$<>8__locals13;
		string upgradeID7 = text7;
		modifiers = array12;
		CS$<>8__locals14.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID7, Db.Get().Attributes.Botanist.Id, modifiers, list2.ToArray(), new string[]
		{
			"hat_role_farming1",
			"hat_role_farming2",
			"hat_role_farming3"
		});
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text7, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals13.skill_worker_def), CS$<>8__locals13.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "agriculture_0", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Intermediate, true, false, list2.ToArray()));
		BionicUpgradeComponentConfig.<>c__DisplayClass27_7 CS$<>8__locals15 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_7();
		string text8 = "Booster_Ranch1";
		AttributeModifier[] array13 = this.CreateBoosterModifiers(text8, new Dictionary<string, float>
		{
			{
				Db.Get().Attributes.Ranching.Id,
				5f
			},
			{
				Db.Get().Attributes.Athletics.Id,
				2f
			}
		});
		SkillPerk[] skillPerks7 = new SkillPerk[]
		{
			Db.Get().SkillPerks.CanWrangleCreatures,
			Db.Get().SkillPerks.CanUseRanchStation,
			Db.Get().SkillPerks.CanUseMilkingStation
		};
		BionicUpgradeComponentConfig.<>c__DisplayClass27_7 CS$<>8__locals16 = CS$<>8__locals15;
		string upgradeID8 = text8;
		modifiers = array13;
		CS$<>8__locals16.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID8, Db.Get().Attributes.Ranching.Id, modifiers, skillPerks7, new string[]
		{
			"hat_role_rancher1",
			"hat_role_rancher2"
		});
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text8, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals15.skill_worker_def), CS$<>8__locals15.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "ranching_0", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Intermediate, true, false, skillPerks7));
		BionicUpgradeComponentConfig.<>c__DisplayClass27_8 CS$<>8__locals17 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_8();
		string text9 = "Booster_Cook1";
		AttributeModifier[] array14 = this.CreateBoosterModifiers(text9, new Dictionary<string, float>
		{
			{
				Db.Get().Attributes.Cooking.Id,
				5f
			},
			{
				Db.Get().Attributes.Athletics.Id,
				2f
			}
		});
		SkillPerk[] skillPerks8 = new SkillPerk[]
		{
			Db.Get().SkillPerks.CanElectricGrill,
			Db.Get().SkillPerks.CanDeepFry,
			Db.Get().SkillPerks.CanGasRange,
			Db.Get().SkillPerks.CanSpiceGrinder
		};
		BionicUpgradeComponentConfig.<>c__DisplayClass27_8 CS$<>8__locals18 = CS$<>8__locals17;
		string upgradeID9 = text9;
		modifiers = array14;
		CS$<>8__locals18.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID9, Db.Get().Attributes.Cooking.Id, modifiers, skillPerks8, new string[]
		{
			"hat_role_cooking1",
			"hat_role_cooking2"
		});
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text9, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals17.skill_worker_def), CS$<>8__locals17.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "cooking_0", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Intermediate, true, true, skillPerks8));
		BionicUpgradeComponentConfig.<>c__DisplayClass27_9 CS$<>8__locals19 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_9();
		string text10 = "Booster_Art1";
		List<SkillPerk> list3 = new List<SkillPerk>
		{
			Db.Get().SkillPerks.CanArt,
			Db.Get().SkillPerks.CanClothingAlteration,
			Db.Get().SkillPerks.CanArtGreat
		};
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			list3.Add(Db.Get().SkillPerks.CanStudyArtifact);
		}
		AttributeModifier[] array15 = this.CreateBoosterModifiers(text10, new Dictionary<string, float>
		{
			{
				Db.Get().Attributes.Art.Id,
				5f
			},
			{
				Db.Get().Attributes.Athletics.Id,
				2f
			}
		});
		BionicUpgradeComponentConfig.<>c__DisplayClass27_9 CS$<>8__locals20 = CS$<>8__locals19;
		string upgradeID10 = text10;
		modifiers = array15;
		CS$<>8__locals20.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID10, Db.Get().Attributes.Art.Id, modifiers, list3.ToArray(), new string[]
		{
			"hat_role_art1",
			"hat_role_art2",
			"hat_role_art3"
		});
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text10, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals19.skill_worker_def), CS$<>8__locals19.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "creativity_0", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Intermediate, true, false, list3.ToArray()));
		BionicUpgradeComponentConfig.<>c__DisplayClass27_10 CS$<>8__locals21 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_10();
		string text11 = "Booster_Research2";
		List<SkillPerk> list4 = new List<SkillPerk>
		{
			Db.Get().SkillPerks.CanMissionControl
		};
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			list4.Add(Db.Get().SkillPerks.CanUseClusterTelescope);
			list4.Add(Db.Get().SkillPerks.AllowOrbitalResearch);
		}
		else
		{
			list4.Add(Db.Get().SkillPerks.AllowInterstellarResearch);
		}
		string[] array16;
		if (!DlcManager.IsExpansion1Active())
		{
			(array16 = new string[1])[0] = "hat_role_research3";
		}
		else
		{
			string[] array17 = new string[2];
			array17[0] = "hat_role_research3";
			array16 = array17;
			array17[1] = "hat_role_research4";
		}
		string[] hats2 = array16;
		AttributeModifier[] array18 = this.CreateBoosterModifiers(text11, new Dictionary<string, float>
		{
			{
				Db.Get().Attributes.Learning.Id,
				5f
			},
			{
				Db.Get().Attributes.Athletics.Id,
				2f
			}
		});
		BionicUpgradeComponentConfig.<>c__DisplayClass27_10 CS$<>8__locals22 = CS$<>8__locals21;
		string upgradeID11 = text11;
		modifiers = array18;
		CS$<>8__locals22.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID11, Db.Get().Attributes.Learning.Id, modifiers, list4.ToArray(), hats2);
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text11, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals21.skill_worker_def), CS$<>8__locals21.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "science_2", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Intermediate, false, false, list4.ToArray()));
		if (DlcManager.IsExpansion1Active())
		{
			BionicUpgradeComponentConfig.<>c__DisplayClass27_11 CS$<>8__locals23 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_11();
			string text12 = "Booster_Research3";
			AttributeModifier[] array19 = this.CreateBoosterModifiers(text12, new Dictionary<string, float>
			{
				{
					Db.Get().Attributes.Learning.Id,
					5f
				},
				{
					Db.Get().Attributes.Athletics.Id,
					2f
				}
			});
			SkillPerk[] skillPerks9 = new SkillPerk[]
			{
				Db.Get().SkillPerks.AllowNuclearResearch
			};
			BionicUpgradeComponentConfig.<>c__DisplayClass27_11 CS$<>8__locals24 = CS$<>8__locals23;
			string upgradeID12 = text12;
			modifiers = array19;
			CS$<>8__locals24.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID12, Db.Get().Attributes.Learning.Id, modifiers, skillPerks9, new string[]
			{
				"hat_role_research5"
			});
			list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text12, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals23.skill_worker_def), CS$<>8__locals23.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "science_3", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Intermediate, false, false, skillPerks9));
		}
		if (DlcManager.IsExpansion1Active())
		{
			BionicUpgradeComponentConfig.<>c__DisplayClass27_12 CS$<>8__locals25 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_12();
			string text13 = "Booster_Pilot1";
			AttributeModifier[] array20 = this.CreateBoosterModifiers(text13, new Dictionary<string, float>
			{
				{
					Db.Get().Attributes.SpaceNavigation.Id,
					5f
				},
				{
					Db.Get().Attributes.Athletics.Id,
					2f
				}
			});
			SkillPerk[] skillPerks10 = new SkillPerk[]
			{
				Db.Get().SkillPerks.CanUseRocketControlStation
			};
			BionicUpgradeComponentConfig.<>c__DisplayClass27_12 CS$<>8__locals26 = CS$<>8__locals25;
			string upgradeID13 = text13;
			modifiers = array20;
			CS$<>8__locals26.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID13, Db.Get().Attributes.SpaceNavigation.Id, modifiers, skillPerks10, new string[]
			{
				"hat_role_astronaut1",
				"hat_role_astronaut2"
			});
			list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text13, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals25.skill_worker_def), CS$<>8__locals25.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "piloting_0", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Intermediate, false, false, skillPerks10));
		}
		if (DlcManager.IsPureVanilla())
		{
			string text14 = "Booster_PilotVanilla1";
			AttributeModifier[] modifiers2 = this.CreateBoosterModifiers(text14, new Dictionary<string, float>
			{
				{
					Db.Get().Attributes.Athletics.Id,
					3f
				}
			});
			SkillPerk[] skillPerks11 = new SkillPerk[]
			{
				Db.Get().SkillPerks.CanUseRockets
			};
			BionicUpgrade_SkilledWorker.Def skill_worker_def = new BionicUpgrade_SkilledWorker.Def(text14, null, modifiers2, skillPerks11, new string[]
			{
				"hat_role_astronaut1",
				"hat_role_astronaut2"
			});
			list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text14, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), skill_worker_def), skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "piloting_vanilla_0", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Intermediate, false, false, skillPerks11));
		}
		BionicUpgradeComponentConfig.<>c__DisplayClass27_14 CS$<>8__locals28 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_14();
		string text15 = "Booster_Suits1";
		AttributeModifier[] array21 = this.CreateBoosterModifiers(text15, new Dictionary<string, float>
		{
			{
				Db.Get().Attributes.Athletics.Id,
				5f
			}
		});
		SkillPerk[] skillPerks12 = new SkillPerk[]
		{
			Db.Get().SkillPerks.ExosuitDurability,
			Db.Get().SkillPerks.ExosuitExpertise
		};
		BionicUpgradeComponentConfig.<>c__DisplayClass27_14 CS$<>8__locals29 = CS$<>8__locals28;
		string upgradeID14 = text15;
		modifiers = array21;
		CS$<>8__locals29.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID14, Db.Get().Attributes.Athletics.Id, modifiers, skillPerks12, new string[]
		{
			"hat_role_suits1",
			"hat_role_suits2"
		});
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text15, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals28.skill_worker_def), CS$<>8__locals28.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "suits_0", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Intermediate, true, false, skillPerks12));
		BionicUpgradeComponentConfig.<>c__DisplayClass27_15 CS$<>8__locals30 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_15();
		string text16 = "Booster_Tidy1";
		AttributeModifier[] array22 = this.CreateBoosterModifiers(text16, new Dictionary<string, float>
		{
			{
				Db.Get().Attributes.Strength.Id,
				5f
			},
			{
				Db.Get().Attributes.Athletics.Id,
				2f
			}
		});
		SkillPerk[] skillPerks13 = new SkillPerk[]
		{
			Db.Get().SkillPerks.CanDoPlumbing,
			Db.Get().SkillPerks.CanMakeMissiles
		};
		BionicUpgradeComponentConfig.<>c__DisplayClass27_15 CS$<>8__locals31 = CS$<>8__locals30;
		string upgradeID15 = text16;
		modifiers = array22;
		CS$<>8__locals31.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID15, Db.Get().Attributes.Strength.Id, modifiers, skillPerks13, new string[]
		{
			"hat_role_basekeeping1",
			"hat_role_basekeeping2",
			"hat_role_pyrotechnics"
		});
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text16, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals30.skill_worker_def), CS$<>8__locals30.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "tidy_0", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Intermediate, false, false, skillPerks13));
		BionicUpgradeComponentConfig.<>c__DisplayClass27_16 CS$<>8__locals32 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_16();
		string text17 = "Booster_Op1";
		AttributeModifier[] array23 = this.CreateBoosterModifiers(text17, new Dictionary<string, float>
		{
			{
				Db.Get().Attributes.Machinery.Id,
				5f
			},
			{
				Db.Get().Attributes.Athletics.Id,
				2f
			}
		});
		SkillPerk[] skillPerks14 = new SkillPerk[]
		{
			Db.Get().SkillPerks.CanPowerTinker,
			Db.Get().SkillPerks.CanCraftElectronics
		};
		BionicUpgradeComponentConfig.<>c__DisplayClass27_16 CS$<>8__locals33 = CS$<>8__locals32;
		string upgradeID16 = text17;
		modifiers = array23;
		CS$<>8__locals33.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID16, Db.Get().Attributes.Machinery.Id, modifiers, skillPerks14, new string[]
		{
			"hat_role_technicals1",
			"hat_role_technicals2"
		});
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text17, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals32.skill_worker_def), CS$<>8__locals32.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "machinery_0", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Intermediate, true, false, skillPerks14));
		BionicUpgradeComponentConfig.<>c__DisplayClass27_17 CS$<>8__locals34 = new BionicUpgradeComponentConfig.<>c__DisplayClass27_17();
		string text18 = "Booster_Op2";
		AttributeModifier[] array24 = this.CreateBoosterModifiers(text18, new Dictionary<string, float>
		{
			{
				Db.Get().Attributes.Machinery.Id,
				5f
			},
			{
				Db.Get().Attributes.Athletics.Id,
				2f
			}
		});
		SkillPerk[] skillPerks15 = new SkillPerk[]
		{
			Db.Get().SkillPerks.ConveyorBuild
		};
		BionicUpgradeComponentConfig.<>c__DisplayClass27_17 CS$<>8__locals35 = CS$<>8__locals34;
		string upgradeID17 = text18;
		modifiers = array24;
		CS$<>8__locals35.skill_worker_def = new BionicUpgrade_SkilledWorker.Def(upgradeID17, Db.Get().Attributes.Machinery.Id, modifiers, skillPerks15, new string[]
		{
			"hat_role_engineering1"
		});
		list.Add(BionicUpgradeComponentConfig.CreateNewUpgradeComponent(text18, null, null, 0f, (StateMachine.Instance smi) => new BionicUpgrade_SkilledWorker.Instance(smi.GetMaster(), CS$<>8__locals34.skill_worker_def), CS$<>8__locals34.skill_worker_def.GetDescription() + "\n\n" + string.Format(STRINGS.ITEMS.BIONIC_BOOSTERS.FABRICATION_SOURCE, STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME), DlcManager.DLC3, "upgrade_disc_kanim", "machinery_1", SimHashes.Creature, null, BionicUpgradeComponentConfig.BoosterType.Advanced, false, false, skillPerks15));
		list.RemoveAll((GameObject t) => t == null);
		return list;
	}

	public void OnPrefabInit(GameObject inst)
	{
	}

	public void OnSpawn(GameObject inst)
	{
	}

	public static Tag GetBionicUpgradePrefabIDWithTraitID(string traitID)
	{
		foreach (Tag tag in BionicUpgradeComponentConfig.UpgradesData.Keys)
		{
			BionicUpgradeComponentConfig.BionicUpgradeData bionicUpgradeData = BionicUpgradeComponentConfig.UpgradesData[tag];
			if (bionicUpgradeData.relatedTrait != null && bionicUpgradeData.relatedTrait == traitID)
			{
				return tag;
			}
		}
		return Tag.Invalid;
	}

	public static GameObject CreateNewUpgradeComponent(string id, string name = null, string desc = null, float wattageCost = 0f, Func<StateMachine.Instance, StateMachine.Instance> stateMachine = null, string sm_description = "", string[] dlcIDs = null, string animFile = "upgrade_disc_kanim", string animStateName = "object", SimHashes element = SimHashes.Creature, string craftTechUnlockID = null, BionicUpgradeComponentConfig.BoosterType booster = BionicUpgradeComponentConfig.BoosterType.Basic, bool isStartingBooster = false, bool isCarePackage = false, SkillPerk[] skillPerks = null)
	{
		if (!DlcManager.IsAllContentSubscribed(dlcIDs))
		{
			return null;
		}
		if (name == null)
		{
			name = Strings.Get("STRINGS.ITEMS.BIONIC_BOOSTERS." + id.ToUpper() + ".NAME");
		}
		if (desc == null)
		{
			desc = Strings.Get("STRINGS.ITEMS.BIONIC_BOOSTERS." + id.ToUpper() + ".DESC");
		}
		string ID = id;
		TechItem techItem = new TechItem(ID, Db.Get().TechItems, Strings.Get("STRINGS.RESEARCH.OTHER_TECH_ITEMS." + id.ToUpper() + ".NAME"), Strings.Get("STRINGS.RESEARCH.OTHER_TECH_ITEMS." + id.ToUpper() + ".DESC"), (string a, bool b) => Def.GetUISprite(Assets.GetPrefab(ID), "ui", false).first, craftTechUnlockID, DlcManager.DLC3, null, false);
		if (!craftTechUnlockID.IsNullOrWhiteSpace())
		{
			Db.Get().Techs.Get(craftTechUnlockID).AddUnlockedItemIDs(new string[]
			{
				techItem.Id
			});
		}
		GameObject gameObject = EntityTemplates.CreateLooseEntity(ID, name, desc, 25f, true, Assets.GetAnim(animFile), animStateName, Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.45f, true, SORTORDER.ARTIFACTS, element, new List<Tag>
		{
			GameTags.BionicUpgrade,
			GameTags.MiscPickupable,
			GameTags.NotRoomAssignable
		});
		gameObject.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
		DecorProvider decorProvider = gameObject.AddOrGet<DecorProvider>();
		decorProvider.SetValues(DECOR.NONE);
		decorProvider.overrideName = gameObject.GetProperName();
		gameObject.AddOrGet<BionicUpgradeComponent>().slotID = Db.Get().AssignableSlots.BionicUpgrade.Id;
		gameObject.AddOrGet<KSelectable>();
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.PedestalDisplayable, false);
		component.requiredDlcIds = dlcIDs;
		component.SetUnityEditorConfigOverride("BionicUpgradeComponentConfig");
		string text = null;
		if (isStartingBooster)
		{
			text = "StartWith" + id;
			DUPLICANTSTATS.BIONICUPGRADETRAITS.Add(new DUPLICANTSTATS.TraitVal
			{
				id = text,
				requiredDlcIds = DlcManager.DLC3
			});
			TraitUtil.CreateBionicUpgradeTrait(text, sm_description)();
		}
		BionicUpgradeComponentConfig.UpgradesData.Add(component.PrefabTag, new BionicUpgradeComponentConfig.BionicUpgradeData(wattageCost, animStateName, text, booster, stateMachine, sm_description, isCarePackage, (from perk in skillPerks
		select perk.Id).ToArray<string>()));
		if (!BionicUpgradeComponentConfig.BASIC_BOOSTERS.Contains(ID))
		{
			ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("PowerStationTools", 8f)
			};
			ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(ID.ToTag(), 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
			};
			ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array, array2), array, array2);
			complexRecipe.time = INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME;
			complexRecipe.description = string.Format(STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.BIONIC_COMPONENT_RECIPE_DESC, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME, name) + "\n\n" + BionicUpgradeComponentConfig.UpgradesData[ID].stateMachineDescription;
			complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
			complexRecipe.fabricators = new List<Tag>
			{
				"AdvancedCraftingTable"
			};
			complexRecipe.requiredTech = craftTechUnlockID;
			complexRecipe.sortOrder = 3;
			complexRecipe.runTimeDescription = (() => BionicUpgradeComponentConfig.GetColonyBoosterAssignmentString(ID));
		}
		else
		{
			ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement("PowerStationTools", (float)((booster == BionicUpgradeComponentConfig.BoosterType.Basic) ? 2 : 4), true)
			};
			ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[]
			{
				new ComplexRecipe.RecipeElement(ID, 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
			};
			ComplexRecipe complexRecipe2 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(ID, array3, array4), array3, array4, DlcManager.DLC3);
			complexRecipe2.time = INDUSTRIAL.RECIPES.STANDARD_FABRICATION_TIME * 2f;
			complexRecipe2.description = string.Format(STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.BIONIC_COMPONENT_RECIPE_DESC, STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME, name) + "\n\n" + BionicUpgradeComponentConfig.UpgradesData[ID].stateMachineDescription;
			complexRecipe2.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
			complexRecipe2.fabricators = new List<Tag>
			{
				"CraftingTable"
			};
			complexRecipe2.sortOrder = 1;
			complexRecipe2.runTimeDescription = (() => BionicUpgradeComponentConfig.GetColonyBoosterAssignmentString(ID));
		}
		return gameObject;
	}

	public static string GetColonyBoosterAssignmentString(string boosterID)
	{
		int num = 0;
		foreach (MinionIdentity cmp in Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId, false))
		{
			if (cmp.HasTag(GameTags.Minions.Models.Bionic))
			{
				BionicUpgradesMonitor.Instance smi = cmp.GetSMI<BionicUpgradesMonitor.Instance>();
				if (smi != null && smi.upgradeComponentSlots != null)
				{
					foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in smi.upgradeComponentSlots)
					{
						if (upgradeComponentSlot.HasUpgradeComponentAssigned && upgradeComponentSlot.assignedUpgradeComponent.PrefabID() == boosterID)
						{
							num++;
							break;
						}
					}
				}
			}
		}
		if (num == 0)
		{
			return string.Format(STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.COLONY_HAS_BOOSTER_ASSIGNED_NONE, Array.Empty<object>());
		}
		return string.Format(STRINGS.BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.COLONY_HAS_BOOSTER_ASSIGNED_COUNT, num);
	}

	public const string DEFAULT_ANIM_FILE_NAME = "upgrade_disc_kanim";

	public const string STARTING_TRAIT_PREFIX = "StartWith";

	public const string Booster_Dig1 = "Booster_Dig1";

	public const string Booster_Construct1 = "Booster_Construct1";

	public const string Booster_Dig2 = "Booster_Dig2";

	public const string Booster_Farm1 = "Booster_Farm1";

	public const string Booster_Ranch1 = "Booster_Ranch1";

	public const string Booster_Cook1 = "Booster_Cook1";

	public const string Booster_Art1 = "Booster_Art1";

	public const string Booster_Research1 = "Booster_Research1";

	public const string Booster_Research2 = "Booster_Research2";

	public const string Booster_Research3 = "Booster_Research3";

	public const string Booster_Pilot1 = "Booster_Pilot1";

	public const string Booster_PilotVanilla1 = "Booster_PilotVanilla1";

	public const string Booster_Suits1 = "Booster_Suits1";

	public const string Booster_Carry1 = "Booster_Carry1";

	public const string Booster_Op1 = "Booster_Op1";

	public const string Booster_Op2 = "Booster_Op2";

	public const string Booster_Medicine1 = "Booster_Medicine1";

	public const string Booster_Tidy1 = "Booster_Tidy1";

	public static List<string> BASIC_BOOSTERS = new List<string>
	{
		"Booster_Dig1",
		"Booster_Construct1",
		"Booster_Carry1",
		"Booster_Research1",
		"Booster_Medicine1"
	};

	public static Dictionary<Tag, BionicUpgradeComponentConfig.BionicUpgradeData> UpgradesData = new Dictionary<Tag, BionicUpgradeComponentConfig.BionicUpgradeData>();

	public enum BoosterType
	{
		Basic,
		Intermediate,
		Advanced,
		Sleep,
		Space,
		Special
	}

	public class BionicUpgradeData
	{
		public float WattageCost { get; private set; }

		public Func<StateMachine.Instance, StateMachine.Instance> stateMachine { get; private set; }

		public string uiAnimName
		{
			get
			{
				if (!(this.animStateName == "object"))
				{
					return "ui_" + this.animStateName;
				}
				return "ui";
			}
		}

		public string relatedTrait { get; private set; }

		public BionicUpgradeComponentConfig.BoosterType Booster { get; private set; }

		public bool isCarePackage { get; private set; }

		public BionicUpgradeData(float cost, string animStateName, string relatedTrait, BionicUpgradeComponentConfig.BoosterType booster, Func<StateMachine.Instance, StateMachine.Instance> smi, string stateMachineDescription, bool isCarePackage, string[] skillPerkIds = null)
		{
			this.WattageCost = cost;
			this.stateMachine = smi;
			this.stateMachineDescription = stateMachineDescription;
			this.animStateName = animStateName;
			this.relatedTrait = relatedTrait;
			this.Booster = booster;
			this.isCarePackage = isCarePackage;
			this.skillPerks = skillPerkIds;
		}

		private const string DEFAULT_ANIM_STATE_NAME = "object";

		public string stateMachineDescription;

		public string animStateName = "object";

		public string[] skillPerks = new string[0];
	}
}
