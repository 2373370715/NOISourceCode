using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CEA RID: 15594
	public class TraitUtil
	{
		// Token: 0x0600EF79 RID: 61305 RVA: 0x001452A8 File Offset: 0x001434A8
		public static System.Action CreateDisabledTaskTrait(string id, string name, string desc, string disabled_chore_group, bool is_valid_starter_trait)
		{
			return delegate()
			{
				ChoreGroup[] disabled_chore_groups = new ChoreGroup[]
				{
					Db.Get().ChoreGroups.Get(disabled_chore_group)
				};
				Db.Get().CreateTrait(id, name, desc, null, true, disabled_chore_groups, false, is_valid_starter_trait);
			};
		}

		// Token: 0x0600EF7A RID: 61306 RVA: 0x004EA044 File Offset: 0x004E8244
		public static System.Action CreateTrait(string id, string name, string desc, string attributeId, float delta, string[] chore_groups, bool positiveTrait = false)
		{
			return delegate()
			{
				List<ChoreGroup> list = new List<ChoreGroup>();
				foreach (string id2 in chore_groups)
				{
					list.Add(Db.Get().ChoreGroups.Get(id2));
				}
				Db.Get().CreateTrait(id, name, desc, null, true, list.ToArray(), positiveTrait, true).Add(new AttributeModifier(attributeId, delta, name, false, false, true));
			};
		}

		// Token: 0x0600EF7B RID: 61307 RVA: 0x004EA098 File Offset: 0x004E8298
		public static System.Action CreateAttributeEffectTrait(string id, string name, string desc, string attributeId, float delta, string attributeId2, float delta2, bool positiveTrait = false)
		{
			return delegate()
			{
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, true);
				trait.Add(new AttributeModifier(attributeId, delta, name, false, false, true));
				trait.Add(new AttributeModifier(attributeId2, delta2, name, false, false, true));
			};
		}

		// Token: 0x0600EF7C RID: 61308 RVA: 0x001452DE File Offset: 0x001434DE
		public static System.Action CreateAttributeEffectTrait(string id, string name, string desc, string[] attributeIds, float[] deltas, bool positiveTrait = false)
		{
			return delegate()
			{
				global::Debug.Assert(attributeIds.Length == deltas.Length, "CreateAttributeEffectTrait must have an equal number of attributeIds and deltas");
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, true);
				for (int i = 0; i < attributeIds.Length; i++)
				{
					trait.Add(new AttributeModifier(attributeIds[i], deltas[i], name, false, false, true));
				}
			};
		}

		// Token: 0x0600EF7D RID: 61309 RVA: 0x004EA0F4 File Offset: 0x004E82F4
		public static System.Action CreateAttributeEffectTrait(string id, string name, string desc, string attributeId, float delta, bool positiveTrait = false, Action<GameObject> on_add = null, bool is_valid_starter_trait = true)
		{
			return delegate()
			{
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, is_valid_starter_trait);
				trait.Add(new AttributeModifier(attributeId, delta, name, false, false, true));
				trait.OnAddTrait = on_add;
			};
		}

		// Token: 0x0600EF7E RID: 61310 RVA: 0x0014531C File Offset: 0x0014351C
		public static System.Action CreateEffectModifierTrait(string id, string name, string desc, string[] ignoredEffects, bool positiveTrait = false)
		{
			return delegate()
			{
				Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, true).AddIgnoredEffects(ignoredEffects);
			};
		}

		// Token: 0x0600EF7F RID: 61311 RVA: 0x00145352 File Offset: 0x00143552
		public static System.Action CreateNamedTrait(string id, string name, string desc, bool positiveTrait = false)
		{
			return delegate()
			{
				Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, true);
			};
		}

		// Token: 0x0600EF80 RID: 61312 RVA: 0x004EA150 File Offset: 0x004E8350
		public static System.Action CreateTrait(string id, string name, string desc, Action<GameObject> on_add, ChoreGroup[] disabled_chore_groups = null, bool positiveTrait = false, Func<string> extendedDescFn = null)
		{
			return TraitUtil.CreateTrait(id, name, desc, on_add, null, null, disabled_chore_groups, positiveTrait, extendedDescFn);
		}

		// Token: 0x0600EF81 RID: 61313 RVA: 0x004EA170 File Offset: 0x004E8370
		public static System.Action CreateTrait(string id, string name, string desc, Action<GameObject> on_add, string[] requiredDlcIds, string[] forbiddenDlcIds = null, ChoreGroup[] disabled_chore_groups = null, bool positiveTrait = false, Func<string> extendedDescFn = null)
		{
			return delegate()
			{
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, disabled_chore_groups, positiveTrait, true, requiredDlcIds, forbiddenDlcIds);
				trait.OnAddTrait = on_add;
				if (extendedDescFn != null)
				{
					Trait trait2 = trait;
					trait2.ExtendedTooltip = (Func<string>)Delegate.Combine(trait2.ExtendedTooltip, extendedDescFn);
				}
			};
		}

		// Token: 0x0600EF82 RID: 61314 RVA: 0x00145380 File Offset: 0x00143580
		public static System.Action CreateComponentTrait<T>(string id, string name, string desc, bool positiveTrait = false, Func<string> extendedDescFn = null) where T : KMonoBehaviour
		{
			return delegate()
			{
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, positiveTrait, true);
				trait.OnAddTrait = delegate(GameObject go)
				{
					go.FindOrAddUnityComponent<T>();
				};
				if (extendedDescFn != null)
				{
					Trait trait2 = trait;
					trait2.ExtendedTooltip = (Func<string>)Delegate.Combine(trait2.ExtendedTooltip, extendedDescFn);
				}
			};
		}

		// Token: 0x0600EF83 RID: 61315 RVA: 0x001453B6 File Offset: 0x001435B6
		public static System.Action CreateSkillGrantingTrait(string id, string name, string desc, string skillId)
		{
			return delegate()
			{
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, true, true);
				trait.TooltipCB = (() => string.Format(DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_DESC, desc, SkillWidget.SkillPerksString(Db.Get().Skills.Get(skillId))));
				trait.OnAddTrait = delegate(GameObject go)
				{
					MinionResume component = go.GetComponent<MinionResume>();
					if (component != null)
					{
						component.GrantSkill(skillId);
					}
				};
			};
		}

		// Token: 0x0600EF84 RID: 61316 RVA: 0x004EA1D4 File Offset: 0x004E83D4
		public static string GetSkillGrantingTraitNameById(string id)
		{
			string result = "";
			StringEntry stringEntry;
			if (Strings.TryGet("STRINGS.DUPLICANTS.TRAITS.GRANTSKILL_" + id.ToUpper() + ".NAME", out stringEntry))
			{
				result = stringEntry.String;
			}
			return result;
		}

		// Token: 0x0600EF85 RID: 61317 RVA: 0x001453E4 File Offset: 0x001435E4
		public static System.Action CreateBionicUpgradeTrait(string id, string effectsDescription)
		{
			return delegate()
			{
				string name = Strings.Get("STRINGS.DUPLICANTS.TRAITS." + id.ToUpper() + ".NAME");
				string desc = Strings.Get("STRINGS.DUPLICANTS.TRAITS." + id.ToUpper() + ".DESC");
				Trait trait = Db.Get().CreateTrait(id, name, desc, null, true, null, true, true);
				trait.TooltipCB = (() => desc + "\n\n" + effectsDescription);
				trait.NameCB = (() => name);
				string shortDescTooltip = Strings.Get("STRINGS.DUPLICANTS.TRAITS." + trait.Id.ToUpper() + ".SHORT_DESC_TOOLTIP");
				trait.ShortDescTooltipCB = (() => shortDescTooltip + "\n\n" + effectsDescription);
			};
		}
	}
}
