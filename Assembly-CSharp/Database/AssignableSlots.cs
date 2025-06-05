using System;
using STRINGS;
using TUNING;

namespace Database
{
	// Token: 0x020021B5 RID: 8629
	public class AssignableSlots : ResourceSet<AssignableSlot>
	{
		// Token: 0x0600B842 RID: 47170 RVA: 0x0046DD54 File Offset: 0x0046BF54
		public AssignableSlots()
		{
			this.Bed = base.Add(new OwnableSlot("Bed", MISC.TAGS.BED));
			this.MessStation = base.Add(new OwnableSlot("MessStation", MISC.TAGS.MESSSTATION));
			this.Clinic = base.Add(new OwnableSlot("Clinic", MISC.TAGS.CLINIC));
			this.MedicalBed = base.Add(new OwnableSlot("MedicalBed", MISC.TAGS.CLINIC));
			this.MedicalBed.showInUI = false;
			this.GeneShuffler = base.Add(new OwnableSlot("GeneShuffler", MISC.TAGS.GENE_SHUFFLER));
			this.GeneShuffler.showInUI = false;
			this.Toilet = base.Add(new OwnableSlot("Toilet", MISC.TAGS.TOILET));
			this.MassageTable = base.Add(new OwnableSlot("MassageTable", MISC.TAGS.MASSAGE_TABLE));
			this.RocketCommandModule = base.Add(new OwnableSlot("RocketCommandModule", MISC.TAGS.COMMAND_MODULE));
			this.HabitatModule = base.Add(new OwnableSlot("HabitatModule", MISC.TAGS.HABITAT_MODULE));
			this.ResetSkillsStation = base.Add(new OwnableSlot("ResetSkillsStation", "ResetSkillsStation"));
			this.WarpPortal = base.Add(new OwnableSlot("WarpPortal", MISC.TAGS.WARP_PORTAL));
			this.WarpPortal.showInUI = false;
			this.BionicUpgrade = base.Add(new OwnableSlot("BionicUpgrade", MISC.TAGS.BIONICUPGRADE));
			this.Toy = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.TOYS.SLOT, MISC.TAGS.TOY, false));
			this.Suit = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.SUITS.SLOT, MISC.TAGS.SUIT, true));
			this.Tool = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.TOOLS.TOOLSLOT, MISC.TAGS.MULTITOOL, false));
			this.Outfit = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.CLOTHING.SLOT, UI.StripLinkFormatting(MISC.TAGS.CLOTHES), true));
		}

		// Token: 0x040095DB RID: 38363
		public AssignableSlot Bed;

		// Token: 0x040095DC RID: 38364
		public AssignableSlot MessStation;

		// Token: 0x040095DD RID: 38365
		public AssignableSlot Clinic;

		// Token: 0x040095DE RID: 38366
		public AssignableSlot GeneShuffler;

		// Token: 0x040095DF RID: 38367
		public AssignableSlot MedicalBed;

		// Token: 0x040095E0 RID: 38368
		public AssignableSlot Toilet;

		// Token: 0x040095E1 RID: 38369
		public AssignableSlot MassageTable;

		// Token: 0x040095E2 RID: 38370
		public AssignableSlot RocketCommandModule;

		// Token: 0x040095E3 RID: 38371
		public AssignableSlot HabitatModule;

		// Token: 0x040095E4 RID: 38372
		public AssignableSlot ResetSkillsStation;

		// Token: 0x040095E5 RID: 38373
		public AssignableSlot WarpPortal;

		// Token: 0x040095E6 RID: 38374
		public AssignableSlot Toy;

		// Token: 0x040095E7 RID: 38375
		public AssignableSlot Suit;

		// Token: 0x040095E8 RID: 38376
		public AssignableSlot Tool;

		// Token: 0x040095E9 RID: 38377
		public AssignableSlot Outfit;

		// Token: 0x040095EA RID: 38378
		public AssignableSlot BionicUpgrade;
	}
}
