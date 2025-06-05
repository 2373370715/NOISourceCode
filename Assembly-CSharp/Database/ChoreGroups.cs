using System;
using Klei.AI;
using STRINGS;

namespace Database
{
	// Token: 0x02002192 RID: 8594
	public class ChoreGroups : ResourceSet<ChoreGroup>
	{
		// Token: 0x0600B757 RID: 46935 RVA: 0x00460940 File Offset: 0x0045EB40
		private ChoreGroup Add(string id, string name, Klei.AI.Attribute attribute, string sprite, int default_personal_priority, bool user_prioritizable = true)
		{
			ChoreGroup choreGroup = new ChoreGroup(id, name, attribute, sprite, default_personal_priority, user_prioritizable);
			base.Add(choreGroup);
			return choreGroup;
		}

		// Token: 0x0600B758 RID: 46936 RVA: 0x00460968 File Offset: 0x0045EB68
		public ChoreGroups(ResourceSet parent) : base("ChoreGroups", parent)
		{
			this.Combat = this.Add("Combat", DUPLICANTS.CHOREGROUPS.COMBAT.NAME, Db.Get().Attributes.Digging, "icon_errand_combat", 5, true);
			this.LifeSupport = this.Add("LifeSupport", DUPLICANTS.CHOREGROUPS.LIFESUPPORT.NAME, Db.Get().Attributes.LifeSupport, "icon_errand_life_support", 5, true);
			this.Toggle = this.Add("Toggle", DUPLICANTS.CHOREGROUPS.TOGGLE.NAME, Db.Get().Attributes.Toggle, "icon_errand_toggle", 5, true);
			this.MedicalAid = this.Add("MedicalAid", DUPLICANTS.CHOREGROUPS.MEDICALAID.NAME, Db.Get().Attributes.Caring, "icon_errand_care", 4, true);
			if (DlcManager.FeatureClusterSpaceEnabled())
			{
				this.Rocketry = this.Add("Rocketry", DUPLICANTS.CHOREGROUPS.ROCKETRY.NAME, Db.Get().Attributes.SpaceNavigation, "icon_errand_rocketry", 4, true);
			}
			this.Basekeeping = this.Add("Basekeeping", DUPLICANTS.CHOREGROUPS.BASEKEEPING.NAME, Db.Get().Attributes.Strength, "icon_errand_tidy", 4, true);
			this.Cook = this.Add("Cook", DUPLICANTS.CHOREGROUPS.COOK.NAME, Db.Get().Attributes.Cooking, "icon_errand_cook", 3, true);
			this.Art = this.Add("Art", DUPLICANTS.CHOREGROUPS.ART.NAME, Db.Get().Attributes.Art, "icon_errand_art", 3, true);
			this.Research = this.Add("Research", DUPLICANTS.CHOREGROUPS.RESEARCH.NAME, Db.Get().Attributes.Learning, "icon_errand_research", 3, true);
			this.MachineOperating = this.Add("MachineOperating", DUPLICANTS.CHOREGROUPS.MACHINEOPERATING.NAME, Db.Get().Attributes.Machinery, "icon_errand_operate", 3, true);
			this.Farming = this.Add("Farming", DUPLICANTS.CHOREGROUPS.FARMING.NAME, Db.Get().Attributes.Botanist, "icon_errand_farm", 3, true);
			this.Ranching = this.Add("Ranching", DUPLICANTS.CHOREGROUPS.RANCHING.NAME, Db.Get().Attributes.Ranching, "icon_errand_ranch", 3, true);
			this.Build = this.Add("Build", DUPLICANTS.CHOREGROUPS.BUILD.NAME, Db.Get().Attributes.Construction, "icon_errand_toggle", 2, true);
			this.Dig = this.Add("Dig", DUPLICANTS.CHOREGROUPS.DIG.NAME, Db.Get().Attributes.Digging, "icon_errand_dig", 2, true);
			this.Hauling = this.Add("Hauling", DUPLICANTS.CHOREGROUPS.HAULING.NAME, Db.Get().Attributes.Strength, "icon_errand_supply", 1, true);
			this.Storage = this.Add("Storage", DUPLICANTS.CHOREGROUPS.STORAGE.NAME, Db.Get().Attributes.Strength, "icon_errand_storage", 1, true);
			this.Recreation = this.Add("Recreation", DUPLICANTS.CHOREGROUPS.RECREATION.NAME, Db.Get().Attributes.Strength, "icon_errand_storage", 1, false);
			Debug.Assert(true);
		}

		// Token: 0x0600B759 RID: 46937 RVA: 0x00460CD0 File Offset: 0x0045EED0
		public ChoreGroup FindByHash(HashedString id)
		{
			ChoreGroup result = null;
			foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
			{
				if (choreGroup.IdHash == id)
				{
					result = choreGroup;
					break;
				}
			}
			return result;
		}

		// Token: 0x040092B2 RID: 37554
		public ChoreGroup Build;

		// Token: 0x040092B3 RID: 37555
		public ChoreGroup Basekeeping;

		// Token: 0x040092B4 RID: 37556
		public ChoreGroup Cook;

		// Token: 0x040092B5 RID: 37557
		public ChoreGroup Art;

		// Token: 0x040092B6 RID: 37558
		public ChoreGroup Dig;

		// Token: 0x040092B7 RID: 37559
		public ChoreGroup Research;

		// Token: 0x040092B8 RID: 37560
		public ChoreGroup Farming;

		// Token: 0x040092B9 RID: 37561
		public ChoreGroup Ranching;

		// Token: 0x040092BA RID: 37562
		public ChoreGroup Hauling;

		// Token: 0x040092BB RID: 37563
		public ChoreGroup Storage;

		// Token: 0x040092BC RID: 37564
		public ChoreGroup MachineOperating;

		// Token: 0x040092BD RID: 37565
		public ChoreGroup MedicalAid;

		// Token: 0x040092BE RID: 37566
		public ChoreGroup Combat;

		// Token: 0x040092BF RID: 37567
		public ChoreGroup LifeSupport;

		// Token: 0x040092C0 RID: 37568
		public ChoreGroup Toggle;

		// Token: 0x040092C1 RID: 37569
		public ChoreGroup Recreation;

		// Token: 0x040092C2 RID: 37570
		public ChoreGroup Rocketry;
	}
}
