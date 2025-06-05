using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200219E RID: 8606
	public class Deaths : ResourceSet<Death>
	{
		// Token: 0x0600B7B7 RID: 47031 RVA: 0x00466A38 File Offset: 0x00464C38
		public Deaths(ResourceSet parent) : base("Deaths", parent)
		{
			this.Generic = new Death("Generic", this, DUPLICANTS.DEATHS.GENERIC.NAME, DUPLICANTS.DEATHS.GENERIC.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Frozen = new Death("Frozen", this, DUPLICANTS.DEATHS.FROZEN.NAME, DUPLICANTS.DEATHS.FROZEN.DESCRIPTION, "death_freeze_trans", "death_freeze_solid");
			this.Suffocation = new Death("Suffocation", this, DUPLICANTS.DEATHS.SUFFOCATION.NAME, DUPLICANTS.DEATHS.SUFFOCATION.DESCRIPTION, "death_suffocation", "dead_on_back");
			this.Starvation = new Death("Starvation", this, DUPLICANTS.DEATHS.STARVATION.NAME, DUPLICANTS.DEATHS.STARVATION.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Overheating = new Death("Overheating", this, DUPLICANTS.DEATHS.OVERHEATING.NAME, DUPLICANTS.DEATHS.OVERHEATING.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Drowned = new Death("Drowned", this, DUPLICANTS.DEATHS.DROWNED.NAME, DUPLICANTS.DEATHS.DROWNED.DESCRIPTION, "death_suffocation", "dead_on_back");
			this.Explosion = new Death("Explosion", this, DUPLICANTS.DEATHS.EXPLOSION.NAME, DUPLICANTS.DEATHS.EXPLOSION.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Slain = new Death("Combat", this, DUPLICANTS.DEATHS.COMBAT.NAME, DUPLICANTS.DEATHS.COMBAT.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.FatalDisease = new Death("FatalDisease", this, DUPLICANTS.DEATHS.FATALDISEASE.NAME, DUPLICANTS.DEATHS.FATALDISEASE.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.Radiation = new Death("Radiation", this, DUPLICANTS.DEATHS.RADIATION.NAME, DUPLICANTS.DEATHS.RADIATION.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.HitByHighEnergyParticle = new Death("HitByHighEnergyParticle", this, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.NAME, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.DESCRIPTION, "dead_on_back", "dead_on_back");
			this.DeadBattery = new Death("DeadBattery", this, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.NAME, DUPLICANTS.DEATHS.HITBYHIGHENERGYPARTICLE.DESCRIPTION, "dead_on_back", "dead_on_back");
		}

		// Token: 0x040093F1 RID: 37873
		public Death Generic;

		// Token: 0x040093F2 RID: 37874
		public Death Frozen;

		// Token: 0x040093F3 RID: 37875
		public Death Suffocation;

		// Token: 0x040093F4 RID: 37876
		public Death Starvation;

		// Token: 0x040093F5 RID: 37877
		public Death Slain;

		// Token: 0x040093F6 RID: 37878
		public Death Overheating;

		// Token: 0x040093F7 RID: 37879
		public Death Drowned;

		// Token: 0x040093F8 RID: 37880
		public Death Explosion;

		// Token: 0x040093F9 RID: 37881
		public Death FatalDisease;

		// Token: 0x040093FA RID: 37882
		public Death Radiation;

		// Token: 0x040093FB RID: 37883
		public Death HitByHighEnergyParticle;

		// Token: 0x040093FC RID: 37884
		public Death DeadBattery;

		// Token: 0x040093FD RID: 37885
		public Death DeadCyborgChargeExpired;
	}
}
