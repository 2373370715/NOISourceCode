using System;

namespace Database
{
	// Token: 0x020021A8 RID: 8616
	public class Expressions : ResourceSet<Expression>
	{
		// Token: 0x0600B7FD RID: 47101 RVA: 0x0046A3B4 File Offset: 0x004685B4
		public Expressions(ResourceSet parent) : base("Expressions", parent)
		{
			Faces faces = Db.Get().Faces;
			this.Angry = new Expression("Angry", this, faces.Angry);
			this.Suffocate = new Expression("Suffocate", this, faces.Suffocate);
			this.RecoverBreath = new Expression("RecoverBreath", this, faces.Uncomfortable);
			this.RedAlert = new Expression("RedAlert", this, faces.Hot);
			this.Hungry = new Expression("Hungry", this, faces.Hungry);
			this.Radiation1 = new Expression("Radiation1", this, faces.Radiation1);
			this.Radiation2 = new Expression("Radiation2", this, faces.Radiation2);
			this.Radiation3 = new Expression("Radiation3", this, faces.Radiation3);
			this.Radiation4 = new Expression("Radiation4", this, faces.Radiation4);
			this.SickSpores = new Expression("SickSpores", this, faces.SickSpores);
			this.Zombie = new Expression("Zombie", this, faces.Zombie);
			this.SickFierySkin = new Expression("SickFierySkin", this, faces.SickFierySkin);
			this.SickCold = new Expression("SickCold", this, faces.SickCold);
			this.Pollen = new Expression("Pollen", this, faces.Pollen);
			this.Sick = new Expression("Sick", this, faces.Sick);
			this.Cold = new Expression("Cold", this, faces.Cold);
			this.Hot = new Expression("Hot", this, faces.Hot);
			this.FullBladder = new Expression("FullBladder", this, faces.Uncomfortable);
			this.Tired = new Expression("Tired", this, faces.Tired);
			this.Unhappy = new Expression("Unhappy", this, faces.Uncomfortable);
			this.Uncomfortable = new Expression("Uncomfortable", this, faces.Uncomfortable);
			this.Productive = new Expression("Productive", this, faces.Productive);
			this.Determined = new Expression("Determined", this, faces.Determined);
			this.Sticker = new Expression("Sticker", this, faces.Sticker);
			this.Balloon = new Expression("Sticker", this, faces.Balloon);
			this.Sparkle = new Expression("Sticker", this, faces.Sparkle);
			this.Music = new Expression("Music", this, faces.Music);
			this.Tickled = new Expression("Tickled", this, faces.Tickled);
			this.BionicJoy = new Expression("Robodancer", this, faces.Robodancer);
			this.Happy = new Expression("Happy", this, faces.Happy);
			this.Relief = new Expression("Relief", this, faces.Happy);
			this.Neutral = new Expression("Neutral", this, faces.Neutral);
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.resources[i].priority = 100 * (this.Count - i);
			}
		}

		// Token: 0x040094E7 RID: 38119
		public Expression Neutral;

		// Token: 0x040094E8 RID: 38120
		public Expression Happy;

		// Token: 0x040094E9 RID: 38121
		public Expression Uncomfortable;

		// Token: 0x040094EA RID: 38122
		public Expression Cold;

		// Token: 0x040094EB RID: 38123
		public Expression Hot;

		// Token: 0x040094EC RID: 38124
		public Expression FullBladder;

		// Token: 0x040094ED RID: 38125
		public Expression Tired;

		// Token: 0x040094EE RID: 38126
		public Expression Hungry;

		// Token: 0x040094EF RID: 38127
		public Expression Angry;

		// Token: 0x040094F0 RID: 38128
		public Expression Unhappy;

		// Token: 0x040094F1 RID: 38129
		public Expression RedAlert;

		// Token: 0x040094F2 RID: 38130
		public Expression Suffocate;

		// Token: 0x040094F3 RID: 38131
		public Expression RecoverBreath;

		// Token: 0x040094F4 RID: 38132
		public Expression Sick;

		// Token: 0x040094F5 RID: 38133
		public Expression SickSpores;

		// Token: 0x040094F6 RID: 38134
		public Expression Zombie;

		// Token: 0x040094F7 RID: 38135
		public Expression SickFierySkin;

		// Token: 0x040094F8 RID: 38136
		public Expression SickCold;

		// Token: 0x040094F9 RID: 38137
		public Expression Pollen;

		// Token: 0x040094FA RID: 38138
		public Expression Relief;

		// Token: 0x040094FB RID: 38139
		public Expression Productive;

		// Token: 0x040094FC RID: 38140
		public Expression Determined;

		// Token: 0x040094FD RID: 38141
		public Expression Sticker;

		// Token: 0x040094FE RID: 38142
		public Expression Balloon;

		// Token: 0x040094FF RID: 38143
		public Expression Sparkle;

		// Token: 0x04009500 RID: 38144
		public Expression Music;

		// Token: 0x04009501 RID: 38145
		public Expression Tickled;

		// Token: 0x04009502 RID: 38146
		public Expression Radiation1;

		// Token: 0x04009503 RID: 38147
		public Expression Radiation2;

		// Token: 0x04009504 RID: 38148
		public Expression Radiation3;

		// Token: 0x04009505 RID: 38149
		public Expression Radiation4;

		// Token: 0x04009506 RID: 38150
		public Expression BionicJoy;
	}
}
