using System;

namespace Database
{
	// Token: 0x020021E9 RID: 8681
	public class Urges : ResourceSet<Urge>
	{
		// Token: 0x0600B8E8 RID: 47336 RVA: 0x00475C4C File Offset: 0x00473E4C
		public Urges()
		{
			this.HealCritical = base.Add(new Urge("HealCritical"));
			this.BeOffline = base.Add(new Urge("BeOffline"));
			this.BeIncapacitated = base.Add(new Urge("BeIncapacitated"));
			this.PacifyEat = base.Add(new Urge("PacifyEat"));
			this.PacifySleep = base.Add(new Urge("PacifySleep"));
			this.PacifyIdle = base.Add(new Urge("PacifyIdle"));
			this.EmoteHighPriority = base.Add(new Urge("EmoteHighPriority"));
			this.RecoverBreath = base.Add(new Urge("RecoverBreath"));
			this.RecoverWarmth = base.Add(new Urge("RecoverWarmth"));
			this.Aggression = base.Add(new Urge("Aggression"));
			this.MoveToQuarantine = base.Add(new Urge("MoveToQuarantine"));
			this.WashHands = base.Add(new Urge("WashHands"));
			this.Shower = base.Add(new Urge("Shower"));
			this.Eat = base.Add(new Urge("Eat"));
			this.ReloadElectrobank = base.Add(new Urge("ReloadElectrobank"));
			this.Pee = base.Add(new Urge("Pee"));
			this.RestDueToDisease = base.Add(new Urge("RestDueToDisease"));
			this.Sleep = base.Add(new Urge("Sleep"));
			this.Narcolepsy = base.Add(new Urge("Narcolepsy"));
			this.Doctor = base.Add(new Urge("Doctor"));
			this.Heal = base.Add(new Urge("Heal"));
			this.Feed = base.Add(new Urge("Feed"));
			this.PacifyRelocate = base.Add(new Urge("PacifyRelocate"));
			this.Emote = base.Add(new Urge("Emote"));
			this.MoveToSafety = base.Add(new Urge("MoveToSafety"));
			this.WarmUp = base.Add(new Urge("WarmUp"));
			this.CoolDown = base.Add(new Urge("CoolDown"));
			this.LearnSkill = base.Add(new Urge("LearnSkill"));
			this.EmoteIdle = base.Add(new Urge("EmoteIdle"));
			this.OilRefill = base.Add(new Urge("OilRefill"));
			this.GunkPee = base.Add(new Urge("GunkPee"));
			this.FindOxygenRefill = base.Add(new Urge("FindOxygenRefill"));
		}

		// Token: 0x04009740 RID: 38720
		public Urge BeIncapacitated;

		// Token: 0x04009741 RID: 38721
		public Urge BeOffline;

		// Token: 0x04009742 RID: 38722
		public Urge Sleep;

		// Token: 0x04009743 RID: 38723
		public Urge Narcolepsy;

		// Token: 0x04009744 RID: 38724
		public Urge Eat;

		// Token: 0x04009745 RID: 38725
		public Urge ReloadElectrobank;

		// Token: 0x04009746 RID: 38726
		public Urge WashHands;

		// Token: 0x04009747 RID: 38727
		public Urge Shower;

		// Token: 0x04009748 RID: 38728
		public Urge Pee;

		// Token: 0x04009749 RID: 38729
		public Urge MoveToQuarantine;

		// Token: 0x0400974A RID: 38730
		public Urge HealCritical;

		// Token: 0x0400974B RID: 38731
		public Urge RecoverBreath;

		// Token: 0x0400974C RID: 38732
		public Urge FindOxygenRefill;

		// Token: 0x0400974D RID: 38733
		public Urge RecoverWarmth;

		// Token: 0x0400974E RID: 38734
		public Urge Emote;

		// Token: 0x0400974F RID: 38735
		public Urge Feed;

		// Token: 0x04009750 RID: 38736
		public Urge Doctor;

		// Token: 0x04009751 RID: 38737
		public Urge Flee;

		// Token: 0x04009752 RID: 38738
		public Urge Heal;

		// Token: 0x04009753 RID: 38739
		public Urge PacifyIdle;

		// Token: 0x04009754 RID: 38740
		public Urge PacifyEat;

		// Token: 0x04009755 RID: 38741
		public Urge PacifySleep;

		// Token: 0x04009756 RID: 38742
		public Urge PacifyRelocate;

		// Token: 0x04009757 RID: 38743
		public Urge RestDueToDisease;

		// Token: 0x04009758 RID: 38744
		public Urge EmoteHighPriority;

		// Token: 0x04009759 RID: 38745
		public Urge Aggression;

		// Token: 0x0400975A RID: 38746
		public Urge MoveToSafety;

		// Token: 0x0400975B RID: 38747
		public Urge WarmUp;

		// Token: 0x0400975C RID: 38748
		public Urge CoolDown;

		// Token: 0x0400975D RID: 38749
		public Urge LearnSkill;

		// Token: 0x0400975E RID: 38750
		public Urge EmoteIdle;

		// Token: 0x0400975F RID: 38751
		public Urge OilRefill;

		// Token: 0x04009760 RID: 38752
		public Urge GunkPee;
	}
}
