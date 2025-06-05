using System;
using Klei.AI;
using STRINGS;

namespace Database
{
	// Token: 0x0200222C RID: 8748
	public class SkillAttributePerk : SkillPerk
	{
		// Token: 0x0600BA09 RID: 47625 RVA: 0x0047A360 File Offset: 0x00478560
		public SkillAttributePerk(string id, string attributeId, float modifierBonus, string modifierDesc, bool modifierCanStack = false) : base(id, "", null, null, delegate(MinionResume identity)
		{
		}, null, false)
		{
			SkillAttributePerk <>4__this = this;
			Klei.AI.Attribute attribute = Db.Get().Attributes.Get(attributeId);
			this.modifier = new AttributeModifier(attributeId, modifierBonus, modifierDesc, false, false, true);
			this.Name = string.Format(UI.ROLES_SCREEN.PERKS.ATTRIBUTE_EFFECT_FMT, this.modifier.GetFormattedString(), attribute.Name);
			Predicate<AttributeModifier> <>9__3;
			base.OnApply = delegate(MinionResume identity)
			{
				if (!modifierCanStack)
				{
					AttributeInstance attributeInstance = identity.GetAttributes().Get(<>4__this.modifier.AttributeId);
					Predicate<AttributeModifier> match;
					if ((match = <>9__3) == null)
					{
						match = (<>9__3 = ((AttributeModifier mod) => mod == <>4__this.modifier));
					}
					if (attributeInstance.Modifiers.FindIndex(match) != -1)
					{
						return;
					}
				}
				identity.GetAttributes().Add(<>4__this.modifier);
			};
			base.OnRemove = delegate(MinionResume identity)
			{
				identity.GetAttributes().Remove(<>4__this.modifier);
			};
		}

		// Token: 0x040097E7 RID: 38887
		public AttributeModifier modifier;
	}
}
