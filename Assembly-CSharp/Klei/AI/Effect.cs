using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C92 RID: 15506
	[DebuggerDisplay("{Id}")]
	public class Effect : Modifier
	{
		// Token: 0x0600EDDE RID: 60894 RVA: 0x004E4360 File Offset: 0x004E2560
		public Effect(string id, string name, string description, float duration, bool show_in_ui, bool trigger_floating_text, bool is_bad, Emote emote = null, float emote_cooldown = -1f, float max_initial_delay = 0f, string stompGroup = null, string custom_icon = "") : this(id, name, description, duration, show_in_ui, trigger_floating_text, is_bad, emote, max_initial_delay, stompGroup, false, custom_icon, emote_cooldown)
		{
		}

		// Token: 0x0600EDDF RID: 60895 RVA: 0x004E438C File Offset: 0x004E258C
		public Effect(string id, string name, string description, float duration, bool show_in_ui, bool trigger_floating_text, bool is_bad, Emote emote, float max_initial_delay, string stompGroup, bool showStatusInWorld, string custom_icon = "", float emote_cooldown = -1f) : this(id, name, description, duration, null, show_in_ui, trigger_floating_text, is_bad, emote, max_initial_delay, stompGroup, showStatusInWorld, custom_icon, emote_cooldown)
		{
		}

		// Token: 0x0600EDE0 RID: 60896 RVA: 0x004E43B8 File Offset: 0x004E25B8
		public Effect(string id, string name, string description, float duration, string[] immunityEffects, bool show_in_ui, bool trigger_floating_text, bool is_bad, Emote emote, float max_initial_delay, string stompGroup, bool showStatusInWorld, string custom_icon = "", float emote_cooldown = -1f) : base(id, name, description)
		{
			this.duration = duration;
			this.showInUI = show_in_ui;
			this.triggerFloatingText = trigger_floating_text;
			this.isBad = is_bad;
			this.emote = emote;
			this.emoteCooldown = emote_cooldown;
			this.maxInitialDelay = max_initial_delay;
			this.stompGroup = stompGroup;
			this.customIcon = custom_icon;
			this.showStatusInWorld = showStatusInWorld;
			this.immunityEffectsNames = immunityEffects;
		}

		// Token: 0x0600EDE1 RID: 60897 RVA: 0x004E4428 File Offset: 0x004E2628
		public Effect(string id, string name, string description, float duration, bool show_in_ui, bool trigger_floating_text, bool is_bad, string emoteAnim, float emote_cooldown = -1f, string stompGroup = null, string custom_icon = "") : base(id, name, description)
		{
			this.duration = duration;
			this.showInUI = show_in_ui;
			this.triggerFloatingText = trigger_floating_text;
			this.isBad = is_bad;
			this.emoteAnim = emoteAnim;
			this.emoteCooldown = emote_cooldown;
			this.stompGroup = stompGroup;
			this.customIcon = custom_icon;
		}

		// Token: 0x0600EDE2 RID: 60898 RVA: 0x00144156 File Offset: 0x00142356
		public override void AddTo(Attributes attributes)
		{
			base.AddTo(attributes);
		}

		// Token: 0x0600EDE3 RID: 60899 RVA: 0x0014415F File Offset: 0x0014235F
		public override void RemoveFrom(Attributes attributes)
		{
			base.RemoveFrom(attributes);
		}

		// Token: 0x0600EDE4 RID: 60900 RVA: 0x00144168 File Offset: 0x00142368
		public void SetEmote(Emote emote, float emoteCooldown = -1f)
		{
			this.emote = emote;
			this.emoteCooldown = emoteCooldown;
		}

		// Token: 0x0600EDE5 RID: 60901 RVA: 0x00144178 File Offset: 0x00142378
		public void AddEmotePrecondition(Reactable.ReactablePrecondition precon)
		{
			if (this.emotePreconditions == null)
			{
				this.emotePreconditions = new List<Reactable.ReactablePrecondition>();
			}
			this.emotePreconditions.Add(precon);
		}

		// Token: 0x0600EDE6 RID: 60902 RVA: 0x004E4480 File Offset: 0x004E2680
		public static string CreateTooltip(Effect effect, bool showDuration, string linePrefix = "\n    • ", bool showHeader = true)
		{
			StringEntry stringEntry;
			Strings.TryGet("STRINGS.DUPLICANTS.MODIFIERS." + effect.Id.ToUpper() + ".ADDITIONAL_EFFECTS", out stringEntry);
			string text = (showHeader && (effect.SelfModifiers.Count > 0 || stringEntry != null)) ? DUPLICANTS.MODIFIERS.EFFECT_HEADER.text : "";
			foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
			{
				Attribute attribute = Db.Get().Attributes.TryGet(attributeModifier.AttributeId);
				if (attribute == null)
				{
					attribute = Db.Get().CritterAttributes.TryGet(attributeModifier.AttributeId);
				}
				if (attribute != null && attribute.ShowInUI != Attribute.Display.Never)
				{
					text = text + linePrefix + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, attribute.Name, attributeModifier.GetFormattedString());
				}
			}
			if (effect.immunityEffectsNames != null)
			{
				text += (string.IsNullOrEmpty(text) ? "" : (linePrefix + linePrefix));
				text += ((showHeader && effect.immunityEffectsNames != null && effect.immunityEffectsNames.Length != 0) ? DUPLICANTS.MODIFIERS.EFFECT_IMMUNITIES_HEADER.text : "");
				foreach (string id in effect.immunityEffectsNames)
				{
					Effect effect2 = Db.Get().effects.TryGet(id);
					if (effect2 != null)
					{
						text = text + linePrefix + string.Format(DUPLICANTS.MODIFIERS.IMMUNITY_FORMAT, effect2.Name);
					}
				}
			}
			if (stringEntry != null)
			{
				text = text + linePrefix + stringEntry;
			}
			if (showDuration && effect.duration > 0f)
			{
				text = text + "\n" + string.Format(DUPLICANTS.MODIFIERS.TIME_TOTAL, GameUtil.GetFormattedCycles(effect.duration, "F1", false));
			}
			return text;
		}

		// Token: 0x0600EDE7 RID: 60903 RVA: 0x00144199 File Offset: 0x00142399
		public static string CreateFullTooltip(Effect effect, bool showDuration)
		{
			return string.Concat(new string[]
			{
				effect.Name,
				"\n\n",
				effect.description,
				"\n\n",
				Effect.CreateTooltip(effect, showDuration, "\n    • ", true)
			});
		}

		// Token: 0x0600EDE8 RID: 60904 RVA: 0x001441D8 File Offset: 0x001423D8
		public static void AddModifierDescriptions(GameObject parent, List<Descriptor> descs, string effect_id, bool increase_indent = false)
		{
			Effect.AddModifierDescriptions(descs, effect_id, increase_indent, "STRINGS.DUPLICANTS.ATTRIBUTES.");
		}

		// Token: 0x0600EDE9 RID: 60905 RVA: 0x004E4674 File Offset: 0x004E2874
		public static void AddModifierDescriptions(List<Descriptor> descs, string effect_id, bool increase_indent = false, string prefix = "STRINGS.DUPLICANTS.ATTRIBUTES.")
		{
			foreach (AttributeModifier attributeModifier in Db.Get().effects.Get(effect_id).SelfModifiers)
			{
				Descriptor item = new Descriptor(Strings.Get(prefix + attributeModifier.AttributeId.ToUpper() + ".NAME") + ": " + attributeModifier.GetFormattedString(), "", Descriptor.DescriptorType.Effect, false);
				if (increase_indent)
				{
					item.IncreaseIndent();
				}
				descs.Add(item);
			}
		}

		// Token: 0x0400E9CE RID: 59854
		public float duration;

		// Token: 0x0400E9CF RID: 59855
		public bool showInUI;

		// Token: 0x0400E9D0 RID: 59856
		public bool triggerFloatingText;

		// Token: 0x0400E9D1 RID: 59857
		public bool isBad;

		// Token: 0x0400E9D2 RID: 59858
		public bool showStatusInWorld;

		// Token: 0x0400E9D3 RID: 59859
		public string customIcon;

		// Token: 0x0400E9D4 RID: 59860
		public string[] immunityEffectsNames;

		// Token: 0x0400E9D5 RID: 59861
		public string emoteAnim;

		// Token: 0x0400E9D6 RID: 59862
		public Emote emote;

		// Token: 0x0400E9D7 RID: 59863
		public float emoteCooldown;

		// Token: 0x0400E9D8 RID: 59864
		public float maxInitialDelay;

		// Token: 0x0400E9D9 RID: 59865
		public List<Reactable.ReactablePrecondition> emotePreconditions;

		// Token: 0x0400E9DA RID: 59866
		public string stompGroup;

		// Token: 0x0400E9DB RID: 59867
		public int stompPriority;
	}
}
