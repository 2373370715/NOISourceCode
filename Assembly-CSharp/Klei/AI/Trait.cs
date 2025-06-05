using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CE9 RID: 15593
	public class Trait : Modifier, IHasDlcRestrictions
	{
		// Token: 0x0600EF6C RID: 61292 RVA: 0x0014527C File Offset: 0x0014347C
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600EF6D RID: 61293 RVA: 0x00145284 File Offset: 0x00143484
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x0600EF6E RID: 61294 RVA: 0x004E9C38 File Offset: 0x004E7E38
		public Trait(string id, string name, string description, float rating, bool should_save, ChoreGroup[] disallowed_chore_groups, bool positive_trait, bool is_valid_starter_trait) : base(id, name, description)
		{
			this.Rating = rating;
			this.ShouldSave = should_save;
			this.disabledChoreGroups = disallowed_chore_groups;
			this.PositiveTrait = positive_trait;
			this.ValidStarterTrait = is_valid_starter_trait;
			this.ignoredEffects = new string[0];
			this.requiredDlcIds = null;
			this.forbiddenDlcIds = null;
		}

		// Token: 0x0600EF6F RID: 61295 RVA: 0x004E9C90 File Offset: 0x004E7E90
		public Trait(string id, string name, string description, float rating, bool should_save, ChoreGroup[] disallowed_chore_groups, bool positive_trait, bool is_valid_starter_trait, string[] requiredDlcIds, string[] forbiddenDlcIds) : base(id, name, description)
		{
			this.Rating = rating;
			this.ShouldSave = should_save;
			this.disabledChoreGroups = disallowed_chore_groups;
			this.PositiveTrait = positive_trait;
			this.ValidStarterTrait = is_valid_starter_trait;
			this.ignoredEffects = new string[0];
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
		}

		// Token: 0x0600EF70 RID: 61296 RVA: 0x004E9CEC File Offset: 0x004E7EEC
		public void AddIgnoredEffects(string[] effects)
		{
			List<string> list = new List<string>(this.ignoredEffects);
			list.AddRange(effects);
			this.ignoredEffects = list.ToArray();
		}

		// Token: 0x0600EF71 RID: 61297 RVA: 0x0014528C File Offset: 0x0014348C
		public string GetName()
		{
			if (this.NameCB != null)
			{
				return this.NameCB();
			}
			return this.Name;
		}

		// Token: 0x0600EF72 RID: 61298 RVA: 0x004E9D18 File Offset: 0x004E7F18
		public string GetTooltip()
		{
			string text;
			if (this.TooltipCB != null)
			{
				text = this.TooltipCB();
			}
			else
			{
				text = this.description;
				text += this.GetAttributeModifiersString(true);
				text += this.GetDisabledChoresString(true);
				text += this.GetIgnoredEffectsString(true);
				text += this.GetExtendedTooltipStr();
			}
			return text;
		}

		// Token: 0x0600EF73 RID: 61299 RVA: 0x004E9D7C File Offset: 0x004E7F7C
		public string GetAttributeModifiersString(bool list_entry)
		{
			string text = "";
			foreach (AttributeModifier attributeModifier in this.SelfModifiers)
			{
				Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
				if (list_entry)
				{
					text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
				}
				text += string.Format(DUPLICANTS.TRAITS.ATTRIBUTE_MODIFIERS, attribute.Name, attributeModifier.GetFormattedString());
			}
			return text;
		}

		// Token: 0x0600EF74 RID: 61300 RVA: 0x004E9E1C File Offset: 0x004E801C
		public string GetDisabledChoresString(bool list_entry)
		{
			string text = "";
			if (this.disabledChoreGroups != null)
			{
				string format = DUPLICANTS.TRAITS.CANNOT_DO_TASK;
				if (this.isTaskBeingRefused)
				{
					format = DUPLICANTS.TRAITS.REFUSES_TO_DO_TASK;
				}
				foreach (ChoreGroup choreGroup in this.disabledChoreGroups)
				{
					if (list_entry)
					{
						text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
					}
					text += string.Format(format, choreGroup.Name);
				}
			}
			return text;
		}

		// Token: 0x0600EF75 RID: 61301 RVA: 0x004E9E98 File Offset: 0x004E8098
		public string GetIgnoredEffectsString(bool list_entry)
		{
			string text = "";
			if (this.ignoredEffects != null && this.ignoredEffects.Length != 0)
			{
				for (int i = 0; i < this.ignoredEffects.Length; i++)
				{
					string text2 = this.ignoredEffects[i];
					if (list_entry)
					{
						text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
					}
					string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text2.ToUpper() + ".NAME");
					text += string.Format(DUPLICANTS.TRAITS.IGNORED_EFFECTS, arg);
					if (!list_entry && i < this.ignoredEffects.Length - 1)
					{
						text += "\n";
					}
				}
			}
			return text;
		}

		// Token: 0x0600EF76 RID: 61302 RVA: 0x004E9F48 File Offset: 0x004E8148
		public string GetExtendedTooltipStr()
		{
			string text = "";
			if (this.ExtendedTooltip != null)
			{
				foreach (Func<string> func in this.ExtendedTooltip.GetInvocationList())
				{
					text = text + "\n" + func();
				}
			}
			return text;
		}

		// Token: 0x0600EF77 RID: 61303 RVA: 0x004E9F9C File Offset: 0x004E819C
		public override void AddTo(Attributes attributes)
		{
			base.AddTo(attributes);
			ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
			if (component != null && this.disabledChoreGroups != null)
			{
				foreach (ChoreGroup chore_group in this.disabledChoreGroups)
				{
					component.SetPermittedByTraits(chore_group, false);
				}
			}
		}

		// Token: 0x0600EF78 RID: 61304 RVA: 0x004E9FF0 File Offset: 0x004E81F0
		public override void RemoveFrom(Attributes attributes)
		{
			base.RemoveFrom(attributes);
			ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
			if (component != null && this.disabledChoreGroups != null)
			{
				foreach (ChoreGroup chore_group in this.disabledChoreGroups)
				{
					component.SetPermittedByTraits(chore_group, true);
				}
			}
		}

		// Token: 0x0400EAEC RID: 60140
		public float Rating;

		// Token: 0x0400EAED RID: 60141
		public bool ShouldSave;

		// Token: 0x0400EAEE RID: 60142
		public bool PositiveTrait;

		// Token: 0x0400EAEF RID: 60143
		public bool ValidStarterTrait;

		// Token: 0x0400EAF0 RID: 60144
		public Action<GameObject> OnAddTrait;

		// Token: 0x0400EAF1 RID: 60145
		public Func<string> TooltipCB;

		// Token: 0x0400EAF2 RID: 60146
		public Func<string> ExtendedTooltip;

		// Token: 0x0400EAF3 RID: 60147
		public Func<string> ShortDescCB;

		// Token: 0x0400EAF4 RID: 60148
		public Func<string> ShortDescTooltipCB;

		// Token: 0x0400EAF5 RID: 60149
		public Func<string> NameCB;

		// Token: 0x0400EAF6 RID: 60150
		public ChoreGroup[] disabledChoreGroups;

		// Token: 0x0400EAF7 RID: 60151
		public bool isTaskBeingRefused;

		// Token: 0x0400EAF8 RID: 60152
		public string[] ignoredEffects;

		// Token: 0x0400EAF9 RID: 60153
		public string[] requiredDlcIds;

		// Token: 0x0400EAFA RID: 60154
		public string[] forbiddenDlcIds;
	}
}
