using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x020021E1 RID: 8673
	public class TechItems : ResourceSet<TechItem>
	{
		// Token: 0x0600B8C9 RID: 47305 RVA: 0x0011B9EA File Offset: 0x00119BEA
		public TechItems(ResourceSet parent) : base("TechItems", parent)
		{
		}

		// Token: 0x0600B8CA RID: 47306 RVA: 0x00472710 File Offset: 0x00470910
		public void Init()
		{
			this.automationOverlay = this.AddTechItem("AutomationOverlay", RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_logic"), null, null, false);
			this.suitsOverlay = this.AddTechItem("SuitsOverlay", RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_suit"), null, null, false);
			this.betaResearchPoint = this.AddTechItem("BetaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_beta_icon"), null, null, false);
			this.gammaResearchPoint = this.AddTechItem("GammaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_gamma_icon"), null, null, false);
			this.orbitalResearchPoint = this.AddTechItem("OrbitalResearchPoint", RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_orbital_icon"), null, null, false);
			this.conveyorOverlay = this.AddTechItem("ConveyorOverlay", RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_conveyor"), null, null, false);
			this.jetSuit = this.AddTechItem("JetSuit", RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Jet_Suit".ToTag()), null, null, false);
			this.atmoSuit = this.AddTechItem("AtmoSuit", RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Atmo_Suit".ToTag()), null, null, false);
			this.oxygenMask = this.AddTechItem("OxygenMask", RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.NAME, RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.DESC, this.GetPrefabSpriteFnBuilder("Oxygen_Mask".ToTag()), null, null, false);
			this.deltaResearchPoint = this.AddTechItem("DeltaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_delta_icon"), DlcManager.EXPANSION1, null, false);
			this.leadSuit = this.AddTechItem("LeadSuit", RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Lead_Suit".ToTag()), DlcManager.EXPANSION1, null, false);
			this.disposableElectrobankMetalOre = this.AddTechItem("DisposableElectrobank_RawMetal", RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_METAL_ORE.NAME, RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_METAL_ORE.DESC, this.GetPrefabSpriteFnBuilder("DisposableElectrobank_RawMetal".ToTag()), DlcManager.DLC3, null, false);
			this.lubricationStick = this.AddTechItem("LubricationStick", RESEARCH.OTHER_TECH_ITEMS.LUBRICATION_STICK.NAME, RESEARCH.OTHER_TECH_ITEMS.LUBRICATION_STICK.DESC, this.GetPrefabSpriteFnBuilder("LubricationStick".ToTag()), DlcManager.DLC3, null, false);
			if (this.lubricationStick != null)
			{
				this.lubricationStick.AddSearchTerms(SEARCH_TERMS.MEDICINE);
			}
			this.disposableElectrobankUraniumOre = this.AddTechItem("DisposableElectrobank_UraniumOre", RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_URANIUM_ORE.NAME, RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_URANIUM_ORE.DESC, this.GetPrefabSpriteFnBuilder("DisposableElectrobank_UraniumOre".ToTag()), new string[]
			{
				"EXPANSION1_ID",
				"DLC3_ID"
			}, null, false);
			this.electrobank = this.AddTechItem("Electrobank", RESEARCH.OTHER_TECH_ITEMS.ELECTROBANK.NAME, RESEARCH.OTHER_TECH_ITEMS.ELECTROBANK.DESC, this.GetPrefabSpriteFnBuilder("Electrobank".ToTag()), DlcManager.DLC3, null, false);
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
		}

		// Token: 0x0600B8CB RID: 47307 RVA: 0x0011B9F8 File Offset: 0x00119BF8
		private Func<string, bool, Sprite> GetSpriteFnBuilder(string spriteName)
		{
			return (string anim, bool centered) => Assets.GetSprite(spriteName);
		}

		// Token: 0x0600B8CC RID: 47308 RVA: 0x0011BA11 File Offset: 0x00119C11
		private Func<string, bool, Sprite> GetPrefabSpriteFnBuilder(Tag prefabTag)
		{
			return (string anim, bool centered) => Def.GetUISprite(prefabTag, "ui", false).first;
		}

		// Token: 0x0600B8CD RID: 47309 RVA: 0x00472B30 File Offset: 0x00470D30
		[Obsolete("Used AddTechItem with requiredDlcIds and forbiddenDlcIds instead.")]
		public TechItem AddTechItem(string id, string name, string description, Func<string, bool, Sprite> getUISprite, string[] DLCIds, bool poi_unlock = false)
		{
			string[] requiredDlcIds;
			string[] forbiddenDlcIds;
			DlcManager.ConvertAvailableToRequireAndForbidden(DLCIds, out requiredDlcIds, out forbiddenDlcIds);
			return this.AddTechItem(id, name, description, getUISprite, requiredDlcIds, forbiddenDlcIds, poi_unlock);
		}

		// Token: 0x0600B8CE RID: 47310 RVA: 0x00472B58 File Offset: 0x00470D58
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

		// Token: 0x0600B8CF RID: 47311 RVA: 0x00472BD8 File Offset: 0x00470DD8
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

		// Token: 0x0600B8D0 RID: 47312 RVA: 0x0011BA2A File Offset: 0x00119C2A
		public Tech GetTechFromItemID(string itemId)
		{
			Techs techs = Db.Get().Techs;
			if (techs == null)
			{
				return null;
			}
			return techs.TryGetTechForTechItem(itemId);
		}

		// Token: 0x0600B8D1 RID: 47313 RVA: 0x00472C40 File Offset: 0x00470E40
		public int GetTechTierForItem(string itemId)
		{
			Tech techFromItemID = this.GetTechFromItemID(itemId);
			if (techFromItemID != null)
			{
				return Techs.GetTier(techFromItemID);
			}
			return 0;
		}

		// Token: 0x04009704 RID: 38660
		public const string AUTOMATION_OVERLAY_ID = "AutomationOverlay";

		// Token: 0x04009705 RID: 38661
		public TechItem automationOverlay;

		// Token: 0x04009706 RID: 38662
		public const string SUITS_OVERLAY_ID = "SuitsOverlay";

		// Token: 0x04009707 RID: 38663
		public TechItem suitsOverlay;

		// Token: 0x04009708 RID: 38664
		public const string JET_SUIT_ID = "JetSuit";

		// Token: 0x04009709 RID: 38665
		public TechItem jetSuit;

		// Token: 0x0400970A RID: 38666
		public const string ATMO_SUIT_ID = "AtmoSuit";

		// Token: 0x0400970B RID: 38667
		public TechItem atmoSuit;

		// Token: 0x0400970C RID: 38668
		public const string OXYGEN_MASK_ID = "OxygenMask";

		// Token: 0x0400970D RID: 38669
		public TechItem oxygenMask;

		// Token: 0x0400970E RID: 38670
		public const string LEAD_SUIT_ID = "LeadSuit";

		// Token: 0x0400970F RID: 38671
		public TechItem leadSuit;

		// Token: 0x04009710 RID: 38672
		public TechItem disposableElectrobankMetalOre;

		// Token: 0x04009711 RID: 38673
		public TechItem lubricationStick;

		// Token: 0x04009712 RID: 38674
		public TechItem disposableElectrobankUraniumOre;

		// Token: 0x04009713 RID: 38675
		public TechItem electrobank;

		// Token: 0x04009714 RID: 38676
		public TechItem fetchDrone;

		// Token: 0x04009715 RID: 38677
		public TechItem selfChargingElectrobank;

		// Token: 0x04009716 RID: 38678
		public const string BETA_RESEARCH_POINT_ID = "BetaResearchPoint";

		// Token: 0x04009717 RID: 38679
		public TechItem betaResearchPoint;

		// Token: 0x04009718 RID: 38680
		public const string GAMMA_RESEARCH_POINT_ID = "GammaResearchPoint";

		// Token: 0x04009719 RID: 38681
		public TechItem gammaResearchPoint;

		// Token: 0x0400971A RID: 38682
		public const string DELTA_RESEARCH_POINT_ID = "DeltaResearchPoint";

		// Token: 0x0400971B RID: 38683
		public TechItem deltaResearchPoint;

		// Token: 0x0400971C RID: 38684
		public const string ORBITAL_RESEARCH_POINT_ID = "OrbitalResearchPoint";

		// Token: 0x0400971D RID: 38685
		public TechItem orbitalResearchPoint;

		// Token: 0x0400971E RID: 38686
		public const string CONVEYOR_OVERLAY_ID = "ConveyorOverlay";

		// Token: 0x0400971F RID: 38687
		public TechItem conveyorOverlay;
	}
}
