using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x020021BE RID: 8638
	public class Personalities : ResourceSet<Personality>
	{
		// Token: 0x0600B85A RID: 47194 RVA: 0x0046E5E4 File Offset: 0x0046C7E4
		public Personalities()
		{
			foreach (Personalities.PersonalityInfo personalityInfo in AsyncLoadManager<IGlobalAsyncLoader>.AsyncLoader<Personalities.PersonalityLoader>.Get().entries)
			{
				if (string.IsNullOrEmpty(personalityInfo.RequiredDlcId) || DlcManager.IsContentSubscribed(personalityInfo.RequiredDlcId))
				{
					base.Add(new Personality(personalityInfo.Name.ToUpper(), Strings.Get(string.Format("STRINGS.DUPLICANTS.PERSONALITIES.{0}.NAME", personalityInfo.Name.ToUpper())), personalityInfo.Gender.ToUpper(), personalityInfo.PersonalityType, personalityInfo.StressTrait, personalityInfo.JoyTrait, personalityInfo.StickerType, personalityInfo.CongenitalTrait, personalityInfo.HeadShape, personalityInfo.Mouth, personalityInfo.Neck, personalityInfo.Eyes, personalityInfo.Hair, personalityInfo.Body, personalityInfo.Belt, personalityInfo.Cuff, personalityInfo.Foot, personalityInfo.Hand, personalityInfo.Pelvis, personalityInfo.Leg, personalityInfo.Leg_Skin, personalityInfo.Arm_Skin, Strings.Get(string.Format("STRINGS.DUPLICANTS.PERSONALITIES.{0}.DESC", personalityInfo.Name.ToUpper())), personalityInfo.ValidStarter, personalityInfo.Grave, personalityInfo.Model)
					{
						requiredDlcId = personalityInfo.RequiredDlcId
					});
				}
			}
		}

		// Token: 0x0600B85B RID: 47195 RVA: 0x0046E730 File Offset: 0x0046C930
		private void AddTrait(Personality personality, string trait_name)
		{
			Trait trait = Db.Get().traits.TryGet(trait_name);
			if (trait != null)
			{
				personality.AddTrait(trait);
			}
		}

		// Token: 0x0600B85C RID: 47196 RVA: 0x0046E758 File Offset: 0x0046C958
		private void SetAttribute(Personality personality, string attribute_name, int value)
		{
			Klei.AI.Attribute attribute = Db.Get().Attributes.TryGet(attribute_name);
			if (attribute == null)
			{
				Debug.LogWarning("Attribute does not exist: " + attribute_name);
				return;
			}
			personality.SetAttribute(attribute, value);
		}

		// Token: 0x0600B85D RID: 47197 RVA: 0x0011B700 File Offset: 0x00119900
		public List<Personality> GetStartingPersonalities()
		{
			return this.resources.FindAll((Personality x) => x.startingMinion);
		}

		// Token: 0x0600B85E RID: 47198 RVA: 0x0046E794 File Offset: 0x0046C994
		public List<Personality> GetAll(bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.resources.FindAll((Personality personality) => (!onlyStartingMinions || personality.startingMinion) && (!onlyEnabledMinions || !personality.Disabled) && (!(Game.Instance != null) || Game.IsDlcActiveForCurrentSave(personality.requiredDlcId)));
		}

		// Token: 0x0600B85F RID: 47199 RVA: 0x0011B72C File Offset: 0x0011992C
		public Personality GetRandom(bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.GetAll(onlyEnabledMinions, onlyStartingMinions).GetRandom<Personality>();
		}

		// Token: 0x0600B860 RID: 47200 RVA: 0x0046E7CC File Offset: 0x0046C9CC
		public Personality GetRandom(Tag model, bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.GetAll(onlyEnabledMinions, onlyStartingMinions).FindAll((Personality personality) => personality.model == model || model == null).GetRandom<Personality>();
		}

		// Token: 0x0600B861 RID: 47201 RVA: 0x0046E804 File Offset: 0x0046CA04
		public Personality GetRandom(List<Tag> models, bool onlyEnabledMinions, bool onlyStartingMinions)
		{
			return this.GetAll(onlyEnabledMinions, onlyStartingMinions).FindAll((Personality personality) => models.Contains(personality.model)).GetRandom<Personality>();
		}

		// Token: 0x0600B862 RID: 47202 RVA: 0x0046E83C File Offset: 0x0046CA3C
		public Personality GetPersonalityFromNameStringKey(string name_string_key)
		{
			foreach (Personality personality in Db.Get().Personalities.resources)
			{
				if (personality.nameStringKey.Equals(name_string_key, StringComparison.CurrentCultureIgnoreCase))
				{
					return personality;
				}
			}
			return null;
		}

		// Token: 0x020021BF RID: 8639
		public class PersonalityLoader : AsyncCsvLoader<Personalities.PersonalityLoader, Personalities.PersonalityInfo>
		{
			// Token: 0x0600B863 RID: 47203 RVA: 0x0011B73B File Offset: 0x0011993B
			public PersonalityLoader() : base(Assets.instance.personalitiesFile)
			{
			}

			// Token: 0x0600B864 RID: 47204 RVA: 0x0011B74D File Offset: 0x0011994D
			public override void Run()
			{
				base.Run();
			}
		}

		// Token: 0x020021C0 RID: 8640
		public class PersonalityInfo : Resource
		{
			// Token: 0x0400961C RID: 38428
			public int HeadShape;

			// Token: 0x0400961D RID: 38429
			public int Mouth;

			// Token: 0x0400961E RID: 38430
			public int Neck;

			// Token: 0x0400961F RID: 38431
			public int Eyes;

			// Token: 0x04009620 RID: 38432
			public int Hair;

			// Token: 0x04009621 RID: 38433
			public int Body;

			// Token: 0x04009622 RID: 38434
			public int Belt;

			// Token: 0x04009623 RID: 38435
			public int Cuff;

			// Token: 0x04009624 RID: 38436
			public int Foot;

			// Token: 0x04009625 RID: 38437
			public int Hand;

			// Token: 0x04009626 RID: 38438
			public int Pelvis;

			// Token: 0x04009627 RID: 38439
			public int Leg;

			// Token: 0x04009628 RID: 38440
			public int Arm_Skin;

			// Token: 0x04009629 RID: 38441
			public int Leg_Skin;

			// Token: 0x0400962A RID: 38442
			public string Gender;

			// Token: 0x0400962B RID: 38443
			public string PersonalityType;

			// Token: 0x0400962C RID: 38444
			public string StressTrait;

			// Token: 0x0400962D RID: 38445
			public string JoyTrait;

			// Token: 0x0400962E RID: 38446
			public string StickerType;

			// Token: 0x0400962F RID: 38447
			public string CongenitalTrait;

			// Token: 0x04009630 RID: 38448
			public string Design;

			// Token: 0x04009631 RID: 38449
			public bool ValidStarter;

			// Token: 0x04009632 RID: 38450
			public string Grave;

			// Token: 0x04009633 RID: 38451
			public string Model;

			// Token: 0x04009634 RID: 38452
			public string RequiredDlcId;
		}
	}
}
