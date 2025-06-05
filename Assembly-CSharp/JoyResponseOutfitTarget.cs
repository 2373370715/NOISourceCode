using System;
using UnityEngine;

// Token: 0x020014AF RID: 5295
public readonly struct JoyResponseOutfitTarget
{
	// Token: 0x06006D9F RID: 28063 RVA: 0x000EC75C File Offset: 0x000EA95C
	public JoyResponseOutfitTarget(JoyResponseOutfitTarget.Implementation impl)
	{
		this.impl = impl;
	}

	// Token: 0x06006DA0 RID: 28064 RVA: 0x000EC765 File Offset: 0x000EA965
	public Option<string> ReadFacadeId()
	{
		return this.impl.ReadFacadeId();
	}

	// Token: 0x06006DA1 RID: 28065 RVA: 0x000EC772 File Offset: 0x000EA972
	public void WriteFacadeId(Option<string> facadeId)
	{
		this.impl.WriteFacadeId(facadeId);
	}

	// Token: 0x06006DA2 RID: 28066 RVA: 0x000EC780 File Offset: 0x000EA980
	public string GetMinionName()
	{
		return this.impl.GetMinionName();
	}

	// Token: 0x06006DA3 RID: 28067 RVA: 0x000EC78D File Offset: 0x000EA98D
	public Personality GetPersonality()
	{
		return this.impl.GetPersonality();
	}

	// Token: 0x06006DA4 RID: 28068 RVA: 0x000EC79A File Offset: 0x000EA99A
	public static JoyResponseOutfitTarget FromMinion(GameObject minionInstance)
	{
		return new JoyResponseOutfitTarget(new JoyResponseOutfitTarget.MinionInstanceTarget(minionInstance));
	}

	// Token: 0x06006DA5 RID: 28069 RVA: 0x000EC7AC File Offset: 0x000EA9AC
	public static JoyResponseOutfitTarget FromPersonality(Personality personality)
	{
		return new JoyResponseOutfitTarget(new JoyResponseOutfitTarget.PersonalityTarget(personality));
	}

	// Token: 0x0400529D RID: 21149
	private readonly JoyResponseOutfitTarget.Implementation impl;

	// Token: 0x020014B0 RID: 5296
	public interface Implementation
	{
		// Token: 0x06006DA6 RID: 28070
		Option<string> ReadFacadeId();

		// Token: 0x06006DA7 RID: 28071
		void WriteFacadeId(Option<string> permitId);

		// Token: 0x06006DA8 RID: 28072
		string GetMinionName();

		// Token: 0x06006DA9 RID: 28073
		Personality GetPersonality();
	}

	// Token: 0x020014B1 RID: 5297
	public readonly struct MinionInstanceTarget : JoyResponseOutfitTarget.Implementation
	{
		// Token: 0x06006DAA RID: 28074 RVA: 0x000EC7BE File Offset: 0x000EA9BE
		public MinionInstanceTarget(GameObject minionInstance)
		{
			this.minionInstance = minionInstance;
			this.wearableAccessorizer = minionInstance.GetComponent<WearableAccessorizer>();
		}

		// Token: 0x06006DAB RID: 28075 RVA: 0x000EC7D3 File Offset: 0x000EA9D3
		public string GetMinionName()
		{
			return this.minionInstance.GetProperName();
		}

		// Token: 0x06006DAC RID: 28076 RVA: 0x000EC7E0 File Offset: 0x000EA9E0
		public Personality GetPersonality()
		{
			return Db.Get().Personalities.Get(this.minionInstance.GetComponent<MinionIdentity>().personalityResourceId);
		}

		// Token: 0x06006DAD RID: 28077 RVA: 0x000EC801 File Offset: 0x000EAA01
		public Option<string> ReadFacadeId()
		{
			return this.wearableAccessorizer.GetJoyResponseId();
		}

		// Token: 0x06006DAE RID: 28078 RVA: 0x000EC80E File Offset: 0x000EAA0E
		public void WriteFacadeId(Option<string> permitId)
		{
			this.wearableAccessorizer.SetJoyResponseId(permitId);
		}

		// Token: 0x0400529E RID: 21150
		public readonly GameObject minionInstance;

		// Token: 0x0400529F RID: 21151
		public readonly WearableAccessorizer wearableAccessorizer;
	}

	// Token: 0x020014B2 RID: 5298
	public readonly struct PersonalityTarget : JoyResponseOutfitTarget.Implementation
	{
		// Token: 0x06006DAF RID: 28079 RVA: 0x000EC81C File Offset: 0x000EAA1C
		public PersonalityTarget(Personality personality)
		{
			this.personality = personality;
		}

		// Token: 0x06006DB0 RID: 28080 RVA: 0x000EC825 File Offset: 0x000EAA25
		public string GetMinionName()
		{
			return this.personality.Name;
		}

		// Token: 0x06006DB1 RID: 28081 RVA: 0x000EC832 File Offset: 0x000EAA32
		public Personality GetPersonality()
		{
			return this.personality;
		}

		// Token: 0x06006DB2 RID: 28082 RVA: 0x000EC83A File Offset: 0x000EAA3A
		public Option<string> ReadFacadeId()
		{
			return this.personality.GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.JoyResponse);
		}

		// Token: 0x06006DB3 RID: 28083 RVA: 0x000EC84D File Offset: 0x000EAA4D
		public void WriteFacadeId(Option<string> facadeId)
		{
			this.personality.SetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.JoyResponse, facadeId);
		}

		// Token: 0x040052A0 RID: 21152
		public readonly Personality personality;
	}
}
