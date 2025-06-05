using System;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C93 RID: 15507
	[DebuggerDisplay("{effect.Id}")]
	public class EffectInstance : ModifierInstance<Effect>
	{
		// Token: 0x0600EDEA RID: 60906 RVA: 0x004E4720 File Offset: 0x004E2920
		public EffectInstance(GameObject game_object, Effect effect, bool should_save) : base(game_object, effect)
		{
			this.effect = effect;
			this.shouldSave = should_save;
			this.DefineEffectImmunities();
			this.ApplyImmunities();
			this.ConfigureStatusItem();
			if (effect.showInUI)
			{
				KSelectable component = base.gameObject.GetComponent<KSelectable>();
				if (!component.GetStatusItemGroup().HasStatusItem(this.statusItem))
				{
					component.AddStatusItem(this.statusItem, this);
				}
			}
			if (effect.triggerFloatingText && PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, effect.Name, game_object.transform, 1.5f, false);
			}
			if (effect.emote != null)
			{
				this.RegisterEmote(effect.emote, effect.emoteCooldown);
			}
			if (!string.IsNullOrEmpty(effect.emoteAnim))
			{
				this.RegisterEmote(effect.emoteAnim, effect.emoteCooldown);
			}
		}

		// Token: 0x0600EDEB RID: 60907 RVA: 0x004E4800 File Offset: 0x004E2A00
		protected void DefineEffectImmunities()
		{
			if (this.immunityEffects == null && this.effect.immunityEffectsNames != null)
			{
				this.immunityEffects = new Effect[this.effect.immunityEffectsNames.Length];
				for (int i = 0; i < this.immunityEffects.Length; i++)
				{
					this.immunityEffects[i] = Db.Get().effects.Get(this.effect.immunityEffectsNames[i]);
				}
			}
		}

		// Token: 0x0600EDEC RID: 60908 RVA: 0x004E4874 File Offset: 0x004E2A74
		protected void ApplyImmunities()
		{
			if (base.gameObject != null && this.immunityEffects != null)
			{
				Effects component = base.gameObject.GetComponent<Effects>();
				for (int i = 0; i < this.immunityEffects.Length; i++)
				{
					component.Remove(this.immunityEffects[i]);
					component.AddImmunity(this.immunityEffects[i], this.effect.IdHash.ToString(), false);
				}
			}
		}

		// Token: 0x0600EDED RID: 60909 RVA: 0x004E48EC File Offset: 0x004E2AEC
		protected void RemoveImmunities()
		{
			if (base.gameObject != null && this.immunityEffects != null)
			{
				Effects component = base.gameObject.GetComponent<Effects>();
				for (int i = 0; i < this.immunityEffects.Length; i++)
				{
					component.RemoveImmunity(this.immunityEffects[i], this.effect.IdHash.ToString());
				}
			}
		}

		// Token: 0x0600EDEE RID: 60910 RVA: 0x004E4954 File Offset: 0x004E2B54
		public void RegisterEmote(string emoteAnim, float cooldown = -1f)
		{
			ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
			if (smi == null)
			{
				return;
			}
			bool flag = cooldown < 0f;
			float globalCooldown = flag ? 100000f : cooldown;
			EmoteReactable emoteReactable = smi.AddSelfEmoteReactable(base.gameObject, this.effect.Name + "_Emote", emoteAnim, flag, Db.Get().ChoreTypes.Emote, globalCooldown, 20f, float.NegativeInfinity, this.effect.maxInitialDelay, this.effect.emotePreconditions);
			if (emoteReactable == null)
			{
				return;
			}
			emoteReactable.InsertPrecondition(0, new Reactable.ReactablePrecondition(this.NotInATube));
			if (!flag)
			{
				this.reactable = emoteReactable;
			}
		}

		// Token: 0x0600EDEF RID: 60911 RVA: 0x004E49FC File Offset: 0x004E2BFC
		public void RegisterEmote(Emote emote, float cooldown = -1f)
		{
			ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
			if (smi == null)
			{
				return;
			}
			bool flag = cooldown < 0f;
			float globalCooldown = flag ? 100000f : cooldown;
			EmoteReactable emoteReactable = smi.AddSelfEmoteReactable(base.gameObject, this.effect.Name + "_Emote", emote, flag, Db.Get().ChoreTypes.Emote, globalCooldown, 20f, float.NegativeInfinity, this.effect.maxInitialDelay, this.effect.emotePreconditions);
			if (emoteReactable == null)
			{
				return;
			}
			emoteReactable.InsertPrecondition(0, new Reactable.ReactablePrecondition(this.NotInATube));
			if (!flag)
			{
				this.reactable = emoteReactable;
			}
		}

		// Token: 0x0600EDF0 RID: 60912 RVA: 0x001441E7 File Offset: 0x001423E7
		private bool NotInATube(GameObject go, Navigator.ActiveTransition transition)
		{
			return transition.navGridTransition.start != NavType.Tube && transition.navGridTransition.end != NavType.Tube;
		}

		// Token: 0x0600EDF1 RID: 60913 RVA: 0x004E4AA8 File Offset: 0x004E2CA8
		public override void OnCleanUp()
		{
			if (this.statusItem != null)
			{
				base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.statusItem, false);
				this.statusItem = null;
			}
			if (this.reactable != null)
			{
				this.reactable.Cleanup();
				this.reactable = null;
			}
			this.RemoveImmunities();
		}

		// Token: 0x0600EDF2 RID: 60914 RVA: 0x0014420A File Offset: 0x0014240A
		public float GetTimeRemaining()
		{
			return this.timeRemaining;
		}

		// Token: 0x0600EDF3 RID: 60915 RVA: 0x00144212 File Offset: 0x00142412
		public bool IsExpired()
		{
			return this.effect.duration > 0f && this.timeRemaining <= 0f;
		}

		// Token: 0x0600EDF4 RID: 60916 RVA: 0x004E4AFC File Offset: 0x004E2CFC
		private void ConfigureStatusItem()
		{
			StatusItem.IconType iconType = this.effect.isBad ? StatusItem.IconType.Exclamation : StatusItem.IconType.Info;
			if (!this.effect.customIcon.IsNullOrWhiteSpace())
			{
				iconType = StatusItem.IconType.Custom;
			}
			string id = this.effect.Id;
			string name = this.effect.Name;
			string description = this.effect.description;
			string customIcon = this.effect.customIcon;
			StatusItem.IconType icon_type = iconType;
			NotificationType notification_type = this.effect.isBad ? NotificationType.Bad : NotificationType.Neutral;
			bool allow_multiples = false;
			bool showStatusInWorld = this.effect.showStatusInWorld;
			this.statusItem = new StatusItem(id, name, description, customIcon, icon_type, notification_type, allow_multiples, OverlayModes.None.ID, 2, showStatusInWorld, null);
			this.statusItem.resolveStringCallback = new Func<string, object, string>(this.ResolveString);
			this.statusItem.resolveTooltipCallback = new Func<string, object, string>(this.ResolveTooltip);
		}

		// Token: 0x0600EDF5 RID: 60917 RVA: 0x000B64D6 File Offset: 0x000B46D6
		private string ResolveString(string str, object data)
		{
			return str;
		}

		// Token: 0x0600EDF6 RID: 60918 RVA: 0x004E4BBC File Offset: 0x004E2DBC
		private string ResolveTooltip(string str, object data)
		{
			string text = str;
			EffectInstance effectInstance = (EffectInstance)data;
			string text2 = Effect.CreateTooltip(effectInstance.effect, false, "\n    • ", true);
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + "\n\n" + text2;
			}
			if (effectInstance.effect.duration > 0f)
			{
				text = text + "\n\n" + string.Format(DUPLICANTS.MODIFIERS.TIME_REMAINING, GameUtil.GetFormattedCycles(this.GetTimeRemaining(), "F1", false));
			}
			return text;
		}

		// Token: 0x0400E9DC RID: 59868
		public Effect effect;

		// Token: 0x0400E9DD RID: 59869
		public bool shouldSave;

		// Token: 0x0400E9DE RID: 59870
		public StatusItem statusItem;

		// Token: 0x0400E9DF RID: 59871
		public float timeRemaining;

		// Token: 0x0400E9E0 RID: 59872
		public EmoteReactable reactable;

		// Token: 0x0400E9E1 RID: 59873
		protected Effect[] immunityEffects;
	}
}
