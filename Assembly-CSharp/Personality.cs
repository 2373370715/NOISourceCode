using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x0200167E RID: 5758
public class Personality : Resource
{
	// Token: 0x17000780 RID: 1920
	// (get) Token: 0x060076FF RID: 30463 RVA: 0x000F2BBB File Offset: 0x000F0DBB
	public string description
	{
		get
		{
			return this.GetDescription();
		}
	}

	// Token: 0x06007700 RID: 30464 RVA: 0x00319D9C File Offset: 0x00317F9C
	[Obsolete("Modders: Use constructor with isStartingMinion parameter")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, string description) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, headShape, headShape, description, true, "", GameTags.Minions.Models.Standard)
	{
	}

	// Token: 0x06007701 RID: 30465 RVA: 0x00319D9C File Offset: 0x00317F9C
	[Obsolete("Modders: Added additional body part customization to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, string description, bool isStartingMinion) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, headShape, headShape, description, true, "", GameTags.Minions.Models.Standard)
	{
	}

	// Token: 0x06007702 RID: 30466 RVA: 0x00319DE0 File Offset: 0x00317FE0
	[Obsolete("Modders: Added a custom gravestone image to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, string description, bool isStartingMinion) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, headShape, headShape, description, isStartingMinion, "", GameTags.Minions.Models.Standard)
	{
	}

	// Token: 0x06007703 RID: 30467 RVA: 0x00319DE0 File Offset: 0x00317FE0
	[Obsolete("Modders: Added 'model', 'arm_skin' and 'leg skin' to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, string description, bool isStartingMinion, string graveStone) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, headShape, headShape, description, isStartingMinion, "", GameTags.Minions.Models.Standard)
	{
	}

	// Token: 0x06007704 RID: 30468 RVA: 0x00319E24 File Offset: 0x00318024
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, int arm_skin, int leg_skin, string description, bool isStartingMinion, string graveStone, Tag model) : base(name_string_key, name)
	{
		this.nameStringKey = name_string_key;
		this.genderStringKey = Gender;
		this.personalityType = PersonalityType;
		this.stresstrait = StressTrait;
		this.joyTrait = JoyTrait;
		this.stickerType = StickerType;
		this.congenitaltrait = CongenitalTrait;
		this.unformattedDescription = description;
		this.headShape = headShape;
		this.mouth = mouth;
		this.neck = neck;
		this.eyes = eyes;
		this.hair = hair;
		this.body = body;
		this.belt = belt;
		this.cuff = cuff;
		this.foot = foot;
		this.hand = hand;
		this.pelvis = pelvis;
		this.leg = leg;
		this.arm_skin = arm_skin;
		this.leg_skin = leg_skin;
		this.startingMinion = isStartingMinion;
		this.graveStone = graveStone;
		this.model = model;
	}

	// Token: 0x06007705 RID: 30469 RVA: 0x000F2BC3 File Offset: 0x000F0DC3
	public string GetDescription()
	{
		this.unformattedDescription = this.unformattedDescription.Replace("{0}", this.Name);
		return this.unformattedDescription;
	}

	// Token: 0x06007706 RID: 30470 RVA: 0x00319F18 File Offset: 0x00318118
	public void SetAttribute(Klei.AI.Attribute attribute, int value)
	{
		Personality.StartingAttribute item = new Personality.StartingAttribute(attribute, value);
		this.attributes.Add(item);
	}

	// Token: 0x06007707 RID: 30471 RVA: 0x000F2BE7 File Offset: 0x000F0DE7
	public void AddTrait(Trait trait)
	{
		this.traits.Add(trait);
	}

	// Token: 0x06007708 RID: 30472 RVA: 0x000F2BF5 File Offset: 0x000F0DF5
	public void SetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType outfitType, Option<string> outfit)
	{
		CustomClothingOutfits.Instance.Internal_SetDuplicantPersonalityOutfit(outfitType, this.Id, outfit);
	}

	// Token: 0x06007709 RID: 30473 RVA: 0x00319F3C File Offset: 0x0031813C
	public string GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType outfitType)
	{
		string result;
		if (CustomClothingOutfits.Instance.Internal_TryGetDuplicantPersonalityOutfit(outfitType, this.Id, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x0600770A RID: 30474 RVA: 0x00319F64 File Offset: 0x00318164
	public Sprite GetMiniIcon()
	{
		if (string.IsNullOrWhiteSpace(this.nameStringKey))
		{
			return Assets.GetSprite("unknown");
		}
		string str;
		if (this.nameStringKey == "MIMA")
		{
			str = "Mi-Ma";
		}
		else
		{
			str = this.nameStringKey[0].ToString() + this.nameStringKey.Substring(1).ToLower();
		}
		return Assets.GetSprite("dreamIcon_" + str);
	}

	// Token: 0x0400597F RID: 22911
	public List<Personality.StartingAttribute> attributes = new List<Personality.StartingAttribute>();

	// Token: 0x04005980 RID: 22912
	public List<Trait> traits = new List<Trait>();

	// Token: 0x04005981 RID: 22913
	public int headShape;

	// Token: 0x04005982 RID: 22914
	public int mouth;

	// Token: 0x04005983 RID: 22915
	public int neck;

	// Token: 0x04005984 RID: 22916
	public int eyes;

	// Token: 0x04005985 RID: 22917
	public int hair;

	// Token: 0x04005986 RID: 22918
	public int body;

	// Token: 0x04005987 RID: 22919
	public int belt;

	// Token: 0x04005988 RID: 22920
	public int cuff;

	// Token: 0x04005989 RID: 22921
	public int foot;

	// Token: 0x0400598A RID: 22922
	public int hand;

	// Token: 0x0400598B RID: 22923
	public int pelvis;

	// Token: 0x0400598C RID: 22924
	public int leg;

	// Token: 0x0400598D RID: 22925
	public int leg_skin;

	// Token: 0x0400598E RID: 22926
	public int arm_skin;

	// Token: 0x0400598F RID: 22927
	public string nameStringKey;

	// Token: 0x04005990 RID: 22928
	public string genderStringKey;

	// Token: 0x04005991 RID: 22929
	public string personalityType;

	// Token: 0x04005992 RID: 22930
	public Tag model;

	// Token: 0x04005993 RID: 22931
	public string stresstrait;

	// Token: 0x04005994 RID: 22932
	public string joyTrait;

	// Token: 0x04005995 RID: 22933
	public string stickerType;

	// Token: 0x04005996 RID: 22934
	public string congenitaltrait;

	// Token: 0x04005997 RID: 22935
	public string unformattedDescription;

	// Token: 0x04005998 RID: 22936
	public string graveStone;

	// Token: 0x04005999 RID: 22937
	public bool startingMinion;

	// Token: 0x0400599A RID: 22938
	public string requiredDlcId;

	// Token: 0x0200167F RID: 5759
	public class StartingAttribute
	{
		// Token: 0x0600770B RID: 30475 RVA: 0x000F2C09 File Offset: 0x000F0E09
		public StartingAttribute(Klei.AI.Attribute attribute, int value)
		{
			this.attribute = attribute;
			this.value = value;
		}

		// Token: 0x0400599B RID: 22939
		public Klei.AI.Attribute attribute;

		// Token: 0x0400599C RID: 22940
		public int value;
	}
}
