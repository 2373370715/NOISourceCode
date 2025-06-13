﻿using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	public class TechItems : ResourceSet<TechItem>
	{
		public TechItems(ResourceSet parent) : base("TechItems", parent)
		{
		}

		public void Init()
		{
			this.automationOverlay = this.AddTechItem("AutomationOverlay", RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_logic"), null, null, false);
			this.suitsOverlay = this.AddTechItem("SuitsOverlay", RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_suit"), null, null, false);
			this.betaResearchPoint = this.AddTechItem("BetaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_beta_icon"), null, null, false);
			this.gammaResearchPoint = this.AddTechItem("GammaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_gamma_icon"), null, null, false);
			this.orbitalResearchPoint = this.AddTechItem("OrbitalResearchPoint", RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_orbital_icon"), null, null, false);
			this.conveyorOverlay = this.AddTechItem("ConveyorOverlay", RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_conveyor"), null, null, false);
			this.jetSuit = this.AddTechItem("JetSuit", RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Jet_Suit".ToTag()), null, null, false);
			if (this.jetSuit != null)
			{
				this.jetSuit.AddSearchTerms(SEARCH_TERMS.ATMOSUIT);
			}
			this.atmoSuit = this.AddTechItem("AtmoSuit", RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Atmo_Suit".ToTag()), null, null, false);
			if (this.atmoSuit != null)
			{
				this.atmoSuit.AddSearchTerms(SEARCH_TERMS.ATMOSUIT);
			}
			this.oxygenMask = this.AddTechItem("OxygenMask", RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.NAME, RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.DESC, this.GetPrefabSpriteFnBuilder("Oxygen_Mask".ToTag()), null, null, false);
			if (this.oxygenMask != null)
			{
				this.oxygenMask.AddSearchTerms(SEARCH_TERMS.OXYGEN);
			}
			this.superLiquids = this.AddTechItem("SUPER_LIQUIDS", RESEARCH.OTHER_TECH_ITEMS.SUPER_LIQUIDS.NAME, RESEARCH.OTHER_TECH_ITEMS.SUPER_LIQUIDS.DESC, this.GetPrefabSpriteFnBuilder(SimHashes.ViscoGel.CreateTag()), null, null, false);
			this.deltaResearchPoint = this.AddTechItem("DeltaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_delta_icon"), DlcManager.EXPANSION1, null, false);
			this.leadSuit = this.AddTechItem("LeadSuit", RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Lead_Suit".ToTag()), DlcManager.EXPANSION1, null, false);
			this.disposableElectrobankMetalOre = this.AddTechItem("DisposableElectrobank_RawMetal", RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_METAL_ORE.NAME, RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_METAL_ORE.DESC, this.GetPrefabSpriteFnBuilder("DisposableElectrobank_RawMetal".ToTag()), DlcManager.DLC3, null, false);
			if (this.disposableElectrobankMetalOre != null)
			{
				this.disposableElectrobankMetalOre.AddSearchTerms(SEARCH_TERMS.BATTERY);
			}
			this.lubricationStick = this.AddTechItem("LubricationStick", RESEARCH.OTHER_TECH_ITEMS.LUBRICATION_STICK.NAME, RESEARCH.OTHER_TECH_ITEMS.LUBRICATION_STICK.DESC, this.GetPrefabSpriteFnBuilder("LubricationStick".ToTag()), DlcManager.DLC3, null, false);
			if (this.lubricationStick != null)
			{
				this.lubricationStick.AddSearchTerms(SEARCH_TERMS.MEDICINE);
				this.lubricationStick.AddSearchTerms(SEARCH_TERMS.BIONIC);
			}
			this.disposableElectrobankUraniumOre = this.AddTechItem("DisposableElectrobank_UraniumOre", RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_URANIUM_ORE.NAME, RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_URANIUM_ORE.DESC, this.GetPrefabSpriteFnBuilder("DisposableElectrobank_UraniumOre".ToTag()), new string[]
			{
				"EXPANSION1_ID",
				"DLC3_ID"
			}, null, false);
			if (this.disposableElectrobankUraniumOre != null)
			{
				this.disposableElectrobankUraniumOre.AddSearchTerms(SEARCH_TERMS.BATTERY);
			}
			this.electrobank = this.AddTechItem("Electrobank", RESEARCH.OTHER_TECH_ITEMS.ELECTROBANK.NAME, RESEARCH.OTHER_TECH_ITEMS.ELECTROBANK.DESC, this.GetPrefabSpriteFnBuilder("Electrobank".ToTag()), DlcManager.DLC3, null, false);
			if (this.electrobank != null)
			{
				this.electrobank.AddSearchTerms(SEARCH_TERMS.BATTERY);
			}
			this.fetchDrone = this.AddTechItem("FetchDrone", RESEARCH.OTHER_TECH_ITEMS.FETCHDRONE.NAME, RESEARCH.OTHER_TECH_ITEMS.FETCHDRONE.DESC, this.GetPrefabSpriteFnBuilder("FetchDrone".ToTag()), DlcManager.DLC3, null, false);
			if (this.fetchDrone != null)
			{
				this.fetchDrone.AddSearchTerms(SEARCH_TERMS.ROBOT);
			}
			this.selfChargingElectrobank = this.AddTechItem("SelfChargingElectrobank", RESEARCH.OTHER_TECH_ITEMS.SELFCHARGINGELECTROBANK.NAME, RESEARCH.OTHER_TECH_ITEMS.SELFCHARGINGELECTROBANK.DESC, this.GetPrefabSpriteFnBuilder("SelfChargingElectrobank".ToTag()), new string[]
			{
				"EXPANSION1_ID",
				"DLC3_ID"
			}, null, false);
			if (this.selfChargingElectrobank != null)
			{
				this.selfChargingElectrobank.AddSearchTerms(SEARCH_TERMS.BATTERY);
			}
		}

		private Func<string, bool, Sprite> GetSpriteFnBuilder(string spriteName)
		{
			return (string anim, bool centered) => Assets.GetSprite(spriteName);
		}

		private Func<string, bool, Sprite> GetPrefabSpriteFnBuilder(Tag prefabTag)
		{
			return (string anim, bool centered) => Def.GetUISprite(prefabTag, "ui", false).first;
		}

		[Obsolete("Used AddTechItem with requiredDlcIds and forbiddenDlcIds instead.")]
		public TechItem AddTechItem(string id, string name, string description, Func<string, bool, Sprite> getUISprite, string[] DLCIds, bool poi_unlock = false)
		{
			string[] requiredDlcIds;
			string[] forbiddenDlcIds;
			DlcManager.ConvertAvailableToRequireAndForbidden(DLCIds, out requiredDlcIds, out forbiddenDlcIds);
			return this.AddTechItem(id, name, description, getUISprite, requiredDlcIds, forbiddenDlcIds, poi_unlock);
		}

		public TechItem AddTechItem(string id, string name, string description, Func<string, bool, Sprite> getUISprite, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null, bool poi_unlock = false)
		{
			if (!DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
			{
				return null;
			}
			if (base.TryGet(id) != null)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Tried adding a tech item called",
					id,
					name,
					"but it was already added!"
				});
				return base.Get(id);
			}
			Tech techFromItemID = this.GetTechFromItemID(id);
			if (techFromItemID == null)
			{
				return null;
			}
			TechItem techItem = new TechItem(id, this, name, description, getUISprite, techFromItemID.Id, requiredDlcIds, forbiddenDlcIds, poi_unlock);
			techFromItemID.unlockedItems.Add(techItem);
			return techItem;
		}

		public bool IsTechItemComplete(string id)
		{
			bool result = true;
			foreach (TechItem techItem in this.resources)
			{
				if (techItem.Id == id)
				{
					result = techItem.IsComplete();
					break;
				}
			}
			return result;
		}

		public Tech GetTechFromItemID(string itemId)
		{
			Techs techs = Db.Get().Techs;
			if (techs == null)
			{
				return null;
			}
			return techs.TryGetTechForTechItem(itemId);
		}

		public int GetTechTierForItem(string itemId)
		{
			Tech techFromItemID = this.GetTechFromItemID(itemId);
			if (techFromItemID != null)
			{
				return Techs.GetTier(techFromItemID);
			}
			return 0;
		}

		public const string AUTOMATION_OVERLAY_ID = "AutomationOverlay";

		public TechItem automationOverlay;

		public const string SUITS_OVERLAY_ID = "SuitsOverlay";

		public TechItem suitsOverlay;

		public const string JET_SUIT_ID = "JetSuit";

		public TechItem jetSuit;

		public const string ATMO_SUIT_ID = "AtmoSuit";

		public TechItem atmoSuit;

		public const string OXYGEN_MASK_ID = "OxygenMask";

		public TechItem oxygenMask;

		public const string LEAD_SUIT_ID = "LeadSuit";

		public TechItem leadSuit;

		public TechItem disposableElectrobankMetalOre;

		public TechItem lubricationStick;

		public TechItem disposableElectrobankUraniumOre;

		public TechItem electrobank;

		public TechItem fetchDrone;

		public TechItem selfChargingElectrobank;

		public TechItem superLiquids;

		public const string BETA_RESEARCH_POINT_ID = "BetaResearchPoint";

		public TechItem betaResearchPoint;

		public const string GAMMA_RESEARCH_POINT_ID = "GammaResearchPoint";

		public TechItem gammaResearchPoint;

		public const string DELTA_RESEARCH_POINT_ID = "DeltaResearchPoint";

		public TechItem deltaResearchPoint;

		public const string ORBITAL_RESEARCH_POINT_ID = "OrbitalResearchPoint";

		public TechItem orbitalResearchPoint;

		public const string CONVEYOR_OVERLAY_ID = "ConveyorOverlay";

		public TechItem conveyorOverlay;
	}
}
