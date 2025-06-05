using System;

namespace Database
{
	// Token: 0x020021D8 RID: 8664
	public class StatusItemCategories : ResourceSet<StatusItemCategory>
	{
		// Token: 0x0600B8B4 RID: 47284 RVA: 0x00472240 File Offset: 0x00470440
		public StatusItemCategories(ResourceSet parent) : base("StatusItemCategories", parent)
		{
			this.Main = new StatusItemCategory("Main", this, "Main");
			this.Role = new StatusItemCategory("Role", this, "Role");
			this.Power = new StatusItemCategory("Power", this, "Power");
			this.Toilet = new StatusItemCategory("Toilet", this, "Toilet");
			this.Research = new StatusItemCategory("Research", this, "Research");
			this.Hitpoints = new StatusItemCategory("Hitpoints", this, "Hitpoints");
			this.Suffocation = new StatusItemCategory("Suffocation", this, "Suffocation");
			this.WoundEffects = new StatusItemCategory("WoundEffects", this, "WoundEffects");
			this.EntityReceptacle = new StatusItemCategory("EntityReceptacle", this, "EntityReceptacle");
			this.PreservationState = new StatusItemCategory("PreservationState", this, "PreservationState");
			this.PreservationTemperature = new StatusItemCategory("PreservationTemperature", this, "PreservationTemperature");
			this.PreservationAtmosphere = new StatusItemCategory("PreservationAtmosphere", this, "PreservationAtmosphere");
			this.ExhaustTemperature = new StatusItemCategory("ExhaustTemperature", this, "ExhaustTemperature");
			this.OperatingEnergy = new StatusItemCategory("OperatingEnergy", this, "OperatingEnergy");
			this.AccessControl = new StatusItemCategory("AccessControl", this, "AccessControl");
			this.RequiredRoom = new StatusItemCategory("RequiredRoom", this, "RequiredRoom");
			this.Yield = new StatusItemCategory("Yield", this, "Yield");
			this.Heat = new StatusItemCategory("Heat", this, "Heat");
			this.Stored = new StatusItemCategory("Stored", this, "Stored");
			this.Ownable = new StatusItemCategory("Ownable", this, "Ownable");
		}

		// Token: 0x040096D5 RID: 38613
		public StatusItemCategory Main;

		// Token: 0x040096D6 RID: 38614
		public StatusItemCategory Role;

		// Token: 0x040096D7 RID: 38615
		public StatusItemCategory Power;

		// Token: 0x040096D8 RID: 38616
		public StatusItemCategory Toilet;

		// Token: 0x040096D9 RID: 38617
		public StatusItemCategory Research;

		// Token: 0x040096DA RID: 38618
		public StatusItemCategory Hitpoints;

		// Token: 0x040096DB RID: 38619
		public StatusItemCategory Suffocation;

		// Token: 0x040096DC RID: 38620
		public StatusItemCategory WoundEffects;

		// Token: 0x040096DD RID: 38621
		public StatusItemCategory EntityReceptacle;

		// Token: 0x040096DE RID: 38622
		public StatusItemCategory PreservationState;

		// Token: 0x040096DF RID: 38623
		public StatusItemCategory PreservationAtmosphere;

		// Token: 0x040096E0 RID: 38624
		public StatusItemCategory PreservationTemperature;

		// Token: 0x040096E1 RID: 38625
		public StatusItemCategory ExhaustTemperature;

		// Token: 0x040096E2 RID: 38626
		public StatusItemCategory OperatingEnergy;

		// Token: 0x040096E3 RID: 38627
		public StatusItemCategory AccessControl;

		// Token: 0x040096E4 RID: 38628
		public StatusItemCategory RequiredRoom;

		// Token: 0x040096E5 RID: 38629
		public StatusItemCategory Yield;

		// Token: 0x040096E6 RID: 38630
		public StatusItemCategory Heat;

		// Token: 0x040096E7 RID: 38631
		public StatusItemCategory Stored;

		// Token: 0x040096E8 RID: 38632
		public StatusItemCategory Ownable;
	}
}
