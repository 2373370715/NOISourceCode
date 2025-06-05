using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x020012EF RID: 4847
public class EquipmentDef : Def
{
	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x06006363 RID: 25443 RVA: 0x000E53E7 File Offset: 0x000E35E7
	public override string Name
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".NAME");
		}
	}

	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x06006364 RID: 25444 RVA: 0x000E540D File Offset: 0x000E360D
	public string Desc
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".DESC");
		}
	}

	// Token: 0x17000632 RID: 1586
	// (get) Token: 0x06006365 RID: 25445 RVA: 0x000E5433 File Offset: 0x000E3633
	public string Effect
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".EFFECT");
		}
	}

	// Token: 0x17000633 RID: 1587
	// (get) Token: 0x06006366 RID: 25446 RVA: 0x000E5459 File Offset: 0x000E3659
	public string GenericName
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".GENERICNAME");
		}
	}

	// Token: 0x17000634 RID: 1588
	// (get) Token: 0x06006367 RID: 25447 RVA: 0x000E547F File Offset: 0x000E367F
	public string WornName
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".WORN_NAME");
		}
	}

	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x06006368 RID: 25448 RVA: 0x000E54A5 File Offset: 0x000E36A5
	public string WornDesc
	{
		get
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + this.Id.ToUpper() + ".WORN_DESC");
		}
	}

	// Token: 0x04004735 RID: 18229
	public string Id;

	// Token: 0x04004736 RID: 18230
	public string Slot;

	// Token: 0x04004737 RID: 18231
	public string FabricatorId;

	// Token: 0x04004738 RID: 18232
	public float FabricationTime;

	// Token: 0x04004739 RID: 18233
	public string RecipeTechUnlock;

	// Token: 0x0400473A RID: 18234
	public SimHashes OutputElement;

	// Token: 0x0400473B RID: 18235
	public Dictionary<string, float> InputElementMassMap;

	// Token: 0x0400473C RID: 18236
	public float Mass;

	// Token: 0x0400473D RID: 18237
	public KAnimFile Anim;

	// Token: 0x0400473E RID: 18238
	public string SnapOn;

	// Token: 0x0400473F RID: 18239
	public string SnapOn1;

	// Token: 0x04004740 RID: 18240
	public KAnimFile BuildOverride;

	// Token: 0x04004741 RID: 18241
	public int BuildOverridePriority;

	// Token: 0x04004742 RID: 18242
	public bool IsBody;

	// Token: 0x04004743 RID: 18243
	public List<AttributeModifier> AttributeModifiers;

	// Token: 0x04004744 RID: 18244
	public string RecipeDescription;

	// Token: 0x04004745 RID: 18245
	public List<Effect> EffectImmunites = new List<Effect>();

	// Token: 0x04004746 RID: 18246
	public Action<Equippable> OnEquipCallBack;

	// Token: 0x04004747 RID: 18247
	public Action<Equippable> OnUnequipCallBack;

	// Token: 0x04004748 RID: 18248
	public EntityTemplates.CollisionShape CollisionShape;

	// Token: 0x04004749 RID: 18249
	public float width;

	// Token: 0x0400474A RID: 18250
	public float height = 0.325f;

	// Token: 0x0400474B RID: 18251
	public Tag[] AdditionalTags;

	// Token: 0x0400474C RID: 18252
	public string wornID;

	// Token: 0x0400474D RID: 18253
	public List<Descriptor> additionalDescriptors = new List<Descriptor>();
}
