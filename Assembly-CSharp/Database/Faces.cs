using System;

namespace Database
{
	// Token: 0x020021A9 RID: 8617
	public class Faces : ResourceSet<Face>
	{
		// Token: 0x0600B7FE RID: 47102 RVA: 0x0046A6E8 File Offset: 0x004688E8
		public Faces()
		{
			this.Neutral = base.Add(new Face("Neutral", null));
			this.Happy = base.Add(new Face("Happy", null));
			this.Uncomfortable = base.Add(new Face("Uncomfortable", null));
			this.Cold = base.Add(new Face("Cold", null));
			this.Hot = base.Add(new Face("Hot", "headfx_sweat"));
			this.Tired = base.Add(new Face("Tired", null));
			this.Sleep = base.Add(new Face("Sleep", null));
			this.Hungry = base.Add(new Face("Hungry", null));
			this.Angry = base.Add(new Face("Angry", null));
			this.Suffocate = base.Add(new Face("Suffocate", null));
			this.Sick = base.Add(new Face("Sick", "headfx_sick"));
			this.SickSpores = base.Add(new Face("Spores", "headfx_spores"));
			this.Zombie = base.Add(new Face("Zombie", null));
			this.SickFierySkin = base.Add(new Face("Fiery", "headfx_fiery"));
			this.SickCold = base.Add(new Face("SickCold", "headfx_sickcold"));
			this.Pollen = base.Add(new Face("Pollen", "headfx_pollen"));
			this.Dead = base.Add(new Face("Death", null));
			this.Productive = base.Add(new Face("Productive", null));
			this.Determined = base.Add(new Face("Determined", null));
			this.Sticker = base.Add(new Face("Sticker", null));
			this.Sparkle = base.Add(new Face("Sparkle", null));
			this.Balloon = base.Add(new Face("Balloon", null));
			this.Tickled = base.Add(new Face("Tickled", null));
			this.Music = base.Add(new Face("Music", null));
			this.Radiation1 = base.Add(new Face("Radiation1", "headfx_radiation1"));
			this.Radiation2 = base.Add(new Face("Radiation2", "headfx_radiation2"));
			this.Radiation3 = base.Add(new Face("Radiation3", "headfx_radiation3"));
			this.Radiation4 = base.Add(new Face("Radiation4", "headfx_radiation4"));
			this.Robodancer = base.Add(new Face("robotdance", null));
		}

		// Token: 0x04009507 RID: 38151
		public Face Neutral;

		// Token: 0x04009508 RID: 38152
		public Face Happy;

		// Token: 0x04009509 RID: 38153
		public Face Uncomfortable;

		// Token: 0x0400950A RID: 38154
		public Face Cold;

		// Token: 0x0400950B RID: 38155
		public Face Hot;

		// Token: 0x0400950C RID: 38156
		public Face Tired;

		// Token: 0x0400950D RID: 38157
		public Face Sleep;

		// Token: 0x0400950E RID: 38158
		public Face Hungry;

		// Token: 0x0400950F RID: 38159
		public Face Angry;

		// Token: 0x04009510 RID: 38160
		public Face Suffocate;

		// Token: 0x04009511 RID: 38161
		public Face Dead;

		// Token: 0x04009512 RID: 38162
		public Face Sick;

		// Token: 0x04009513 RID: 38163
		public Face SickSpores;

		// Token: 0x04009514 RID: 38164
		public Face Zombie;

		// Token: 0x04009515 RID: 38165
		public Face SickFierySkin;

		// Token: 0x04009516 RID: 38166
		public Face SickCold;

		// Token: 0x04009517 RID: 38167
		public Face Pollen;

		// Token: 0x04009518 RID: 38168
		public Face Productive;

		// Token: 0x04009519 RID: 38169
		public Face Determined;

		// Token: 0x0400951A RID: 38170
		public Face Sticker;

		// Token: 0x0400951B RID: 38171
		public Face Balloon;

		// Token: 0x0400951C RID: 38172
		public Face Sparkle;

		// Token: 0x0400951D RID: 38173
		public Face Tickled;

		// Token: 0x0400951E RID: 38174
		public Face Music;

		// Token: 0x0400951F RID: 38175
		public Face Radiation1;

		// Token: 0x04009520 RID: 38176
		public Face Radiation2;

		// Token: 0x04009521 RID: 38177
		public Face Radiation3;

		// Token: 0x04009522 RID: 38178
		public Face Radiation4;

		// Token: 0x04009523 RID: 38179
		public Face Robodancer;
	}
}
