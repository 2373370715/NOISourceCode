using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C94 RID: 15508
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/Effects")]
	public class Effects : KMonoBehaviour, ISaveLoadable, ISim1000ms
	{
		// Token: 0x0600EDF7 RID: 60919 RVA: 0x00144238 File Offset: 0x00142438
		protected override void OnPrefabInit()
		{
			this.autoRegisterSimRender = false;
		}

		// Token: 0x0600EDF8 RID: 60920 RVA: 0x004E4C38 File Offset: 0x004E2E38
		protected override void OnSpawn()
		{
			if (this.saveLoadImmunities != null)
			{
				foreach (Effects.SaveLoadImmunities saveLoadImmunities in this.saveLoadImmunities)
				{
					if (Db.Get().effects.Exists(saveLoadImmunities.effectID))
					{
						Effect effect = Db.Get().effects.Get(saveLoadImmunities.effectID);
						this.AddImmunity(effect, saveLoadImmunities.giverID, true);
					}
				}
			}
			if (this.saveLoadEffects != null)
			{
				foreach (Effects.SaveLoadEffect saveLoadEffect in this.saveLoadEffects)
				{
					if (Db.Get().effects.Exists(saveLoadEffect.id))
					{
						Effect newEffect = Db.Get().effects.Get(saveLoadEffect.id);
						EffectInstance effectInstance = this.Add(newEffect, true);
						if (effectInstance != null)
						{
							effectInstance.timeRemaining = saveLoadEffect.timeRemaining;
						}
					}
				}
			}
			if (this.effectsThatExpire.Count > 0)
			{
				SimAndRenderScheduler.instance.Add(this, this.simRenderLoadBalance);
			}
		}

		// Token: 0x0600EDF9 RID: 60921 RVA: 0x004E4D3C File Offset: 0x004E2F3C
		public EffectInstance Get(string effect_id)
		{
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.effect.Id == effect_id)
				{
					return effectInstance;
				}
			}
			return null;
		}

		// Token: 0x0600EDFA RID: 60922 RVA: 0x004E4DA4 File Offset: 0x004E2FA4
		public EffectInstance Get(HashedString effect_id)
		{
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.effect.IdHash == effect_id)
				{
					return effectInstance;
				}
			}
			return null;
		}

		// Token: 0x0600EDFB RID: 60923 RVA: 0x004E4E0C File Offset: 0x004E300C
		public EffectInstance Get(Effect effect)
		{
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.effect == effect)
				{
					return effectInstance;
				}
			}
			return null;
		}

		// Token: 0x0600EDFC RID: 60924 RVA: 0x004E4E68 File Offset: 0x004E3068
		public bool HasImmunityTo(Effect effect)
		{
			using (List<Effects.EffectImmunity>.Enumerator enumerator = this.effectImmunites.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect == effect)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600EDFD RID: 60925 RVA: 0x004E4EC4 File Offset: 0x004E30C4
		public EffectInstance Add(string effect_id, bool should_save)
		{
			Effect newEffect = Db.Get().effects.Get(effect_id);
			return this.Add(newEffect, should_save);
		}

		// Token: 0x0600EDFE RID: 60926 RVA: 0x004E4EEC File Offset: 0x004E30EC
		public EffectInstance Add(HashedString effect_id, bool should_save)
		{
			Effect newEffect = Db.Get().effects.Get(effect_id);
			return this.Add(newEffect, should_save);
		}

		// Token: 0x0600EDFF RID: 60927 RVA: 0x004E4F14 File Offset: 0x004E3114
		public EffectInstance Add(Effect newEffect, bool should_save)
		{
			if (this.HasImmunityTo(newEffect))
			{
				return null;
			}
			Traits component = base.GetComponent<Traits>();
			if (component != null && component.IsEffectIgnored(newEffect))
			{
				return null;
			}
			Attributes attributes = this.GetAttributes();
			EffectInstance effectInstance = this.Get(newEffect);
			if (!string.IsNullOrEmpty(newEffect.stompGroup))
			{
				for (int i = this.effects.Count - 1; i >= 0; i--)
				{
					if (this.effects[i] != effectInstance && !(this.effects[i].effect.stompGroup != newEffect.stompGroup) && this.effects[i].effect.stompPriority > newEffect.stompPriority)
					{
						return null;
					}
				}
				for (int j = this.effects.Count - 1; j >= 0; j--)
				{
					if (this.effects[j] != effectInstance && !(this.effects[j].effect.stompGroup != newEffect.stompGroup) && this.effects[j].effect.stompPriority <= newEffect.stompPriority)
					{
						this.Remove(this.effects[j].effect);
					}
				}
			}
			if (effectInstance == null)
			{
				effectInstance = new EffectInstance(base.gameObject, newEffect, should_save);
				newEffect.AddTo(attributes);
				this.effects.Add(effectInstance);
				if (newEffect.duration > 0f)
				{
					this.effectsThatExpire.Add(effectInstance);
					if (this.effectsThatExpire.Count == 1)
					{
						SimAndRenderScheduler.instance.Add(this, this.simRenderLoadBalance);
					}
				}
				base.Trigger(-1901442097, newEffect);
			}
			effectInstance.timeRemaining = newEffect.duration;
			return effectInstance;
		}

		// Token: 0x0600EE00 RID: 60928 RVA: 0x00144241 File Offset: 0x00142441
		public void Remove(Effect effect)
		{
			this.Remove(effect.IdHash);
		}

		// Token: 0x0600EE01 RID: 60929 RVA: 0x004E50CC File Offset: 0x004E32CC
		public void Remove(HashedString effect_id)
		{
			int i = 0;
			while (i < this.effectsThatExpire.Count)
			{
				if (this.effectsThatExpire[i].effect.IdHash == effect_id)
				{
					int index = this.effectsThatExpire.Count - 1;
					this.effectsThatExpire[i] = this.effectsThatExpire[index];
					this.effectsThatExpire.RemoveAt(index);
					if (this.effectsThatExpire.Count == 0)
					{
						SimAndRenderScheduler.instance.Remove(this);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < this.effects.Count; j++)
			{
				if (this.effects[j].effect.IdHash == effect_id)
				{
					Attributes attributes = this.GetAttributes();
					EffectInstance effectInstance = this.effects[j];
					effectInstance.OnCleanUp();
					Effect effect = effectInstance.effect;
					effect.RemoveFrom(attributes);
					int index2 = this.effects.Count - 1;
					this.effects[j] = this.effects[index2];
					this.effects.RemoveAt(index2);
					base.Trigger(-1157678353, effect);
					return;
				}
			}
		}

		// Token: 0x0600EE02 RID: 60930 RVA: 0x004E5200 File Offset: 0x004E3400
		public void Remove(string effect_id)
		{
			int i = 0;
			while (i < this.effectsThatExpire.Count)
			{
				if (this.effectsThatExpire[i].effect.Id == effect_id)
				{
					int index = this.effectsThatExpire.Count - 1;
					this.effectsThatExpire[i] = this.effectsThatExpire[index];
					this.effectsThatExpire.RemoveAt(index);
					if (this.effectsThatExpire.Count == 0)
					{
						SimAndRenderScheduler.instance.Remove(this);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < this.effects.Count; j++)
			{
				if (this.effects[j].effect.Id == effect_id)
				{
					Attributes attributes = this.GetAttributes();
					EffectInstance effectInstance = this.effects[j];
					effectInstance.OnCleanUp();
					Effect effect = effectInstance.effect;
					effect.RemoveFrom(attributes);
					int index2 = this.effects.Count - 1;
					this.effects[j] = this.effects[index2];
					this.effects.RemoveAt(index2);
					base.Trigger(-1157678353, effect);
					return;
				}
			}
		}

		// Token: 0x0600EE03 RID: 60931 RVA: 0x004E5334 File Offset: 0x004E3534
		public bool HasEffect(HashedString effect_id)
		{
			using (List<EffectInstance>.Enumerator enumerator = this.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect.IdHash == effect_id)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600EE04 RID: 60932 RVA: 0x004E5398 File Offset: 0x004E3598
		public bool HasEffect(string effect_id)
		{
			using (List<EffectInstance>.Enumerator enumerator = this.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect.Id == effect_id)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600EE05 RID: 60933 RVA: 0x004E53FC File Offset: 0x004E35FC
		public bool HasEffect(Effect effect)
		{
			using (List<EffectInstance>.Enumerator enumerator = this.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect == effect)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600EE06 RID: 60934 RVA: 0x004E5458 File Offset: 0x004E3658
		public void Sim1000ms(float dt)
		{
			for (int i = 0; i < this.effectsThatExpire.Count; i++)
			{
				EffectInstance effectInstance = this.effectsThatExpire[i];
				if (effectInstance.IsExpired())
				{
					this.Remove(effectInstance.effect);
				}
				effectInstance.timeRemaining -= dt;
			}
		}

		// Token: 0x0600EE07 RID: 60935 RVA: 0x004E54AC File Offset: 0x004E36AC
		public void AddImmunity(Effect effect, string giverID, bool shouldSave = true)
		{
			if (giverID != null)
			{
				foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
				{
					if (effectImmunity.giverID == giverID && effectImmunity.effect == effect)
					{
						return;
					}
				}
			}
			Effects.EffectImmunity effectImmunity2 = new Effects.EffectImmunity(effect, giverID, shouldSave);
			this.effectImmunites.Add(effectImmunity2);
			base.Trigger(1152870979, effectImmunity2);
		}

		// Token: 0x0600EE08 RID: 60936 RVA: 0x004E553C File Offset: 0x004E373C
		public void RemoveImmunity(Effect effect, string ID)
		{
			Effects.EffectImmunity effectImmunity = default(Effects.EffectImmunity);
			bool flag = false;
			foreach (Effects.EffectImmunity effectImmunity2 in this.effectImmunites)
			{
				if (effectImmunity2.effect == effect && (ID == null || ID == effectImmunity2.giverID))
				{
					effectImmunity = effectImmunity2;
					flag = true;
				}
			}
			if (flag)
			{
				this.effectImmunites.Remove(effectImmunity);
				base.Trigger(964452195, effectImmunity);
			}
		}

		// Token: 0x0600EE09 RID: 60937 RVA: 0x004E55D4 File Offset: 0x004E37D4
		[OnSerializing]
		internal void OnSerializing()
		{
			List<Effects.SaveLoadEffect> list = new List<Effects.SaveLoadEffect>();
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.shouldSave)
				{
					Effects.SaveLoadEffect item = new Effects.SaveLoadEffect
					{
						id = effectInstance.effect.Id,
						timeRemaining = effectInstance.timeRemaining,
						saved = true
					};
					list.Add(item);
				}
			}
			this.saveLoadEffects = list.ToArray();
			List<Effects.SaveLoadImmunities> list2 = new List<Effects.SaveLoadImmunities>();
			foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
			{
				if (effectImmunity.shouldSave)
				{
					Effect effect = effectImmunity.effect;
					Effects.SaveLoadImmunities item2 = new Effects.SaveLoadImmunities
					{
						effectID = effect.Id,
						giverID = effectImmunity.giverID,
						saved = true
					};
					list2.Add(item2);
				}
			}
			this.saveLoadImmunities = list2.ToArray();
		}

		// Token: 0x0600EE0A RID: 60938 RVA: 0x004E5710 File Offset: 0x004E3910
		public List<Effects.SaveLoadImmunities> GetAllImmunitiesForSerialization()
		{
			List<Effects.SaveLoadImmunities> list = new List<Effects.SaveLoadImmunities>();
			foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
			{
				Effect effect = effectImmunity.effect;
				Effects.SaveLoadImmunities item = new Effects.SaveLoadImmunities
				{
					effectID = effect.Id,
					giverID = effectImmunity.giverID,
					saved = effectImmunity.shouldSave
				};
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600EE0B RID: 60939 RVA: 0x004E57A8 File Offset: 0x004E39A8
		public List<Effects.SaveLoadEffect> GetAllEffectsForSerialization()
		{
			List<Effects.SaveLoadEffect> list = new List<Effects.SaveLoadEffect>();
			foreach (EffectInstance effectInstance in this.effects)
			{
				Effects.SaveLoadEffect item = new Effects.SaveLoadEffect
				{
					id = effectInstance.effect.Id,
					timeRemaining = effectInstance.timeRemaining,
					saved = effectInstance.shouldSave
				};
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600EE0C RID: 60940 RVA: 0x0014424F File Offset: 0x0014244F
		public List<EffectInstance> GetTimeLimitedEffects()
		{
			return this.effectsThatExpire;
		}

		// Token: 0x0600EE0D RID: 60941 RVA: 0x004E583C File Offset: 0x004E3A3C
		public void CopyEffects(Effects source)
		{
			foreach (EffectInstance effectInstance in source.effects)
			{
				this.Add(effectInstance.effect, effectInstance.shouldSave).timeRemaining = effectInstance.timeRemaining;
			}
			foreach (EffectInstance effectInstance2 in source.effectsThatExpire)
			{
				this.Add(effectInstance2.effect, effectInstance2.shouldSave).timeRemaining = effectInstance2.timeRemaining;
			}
		}

		// Token: 0x0400E9E2 RID: 59874
		[Serialize]
		private Effects.SaveLoadEffect[] saveLoadEffects;

		// Token: 0x0400E9E3 RID: 59875
		[Serialize]
		private Effects.SaveLoadImmunities[] saveLoadImmunities;

		// Token: 0x0400E9E4 RID: 59876
		private List<EffectInstance> effects = new List<EffectInstance>();

		// Token: 0x0400E9E5 RID: 59877
		private List<EffectInstance> effectsThatExpire = new List<EffectInstance>();

		// Token: 0x0400E9E6 RID: 59878
		private List<Effects.EffectImmunity> effectImmunites = new List<Effects.EffectImmunity>();

		// Token: 0x02003C95 RID: 15509
		[Serializable]
		public struct EffectImmunity
		{
			// Token: 0x0600EE0F RID: 60943 RVA: 0x00144280 File Offset: 0x00142480
			public EffectImmunity(Effect e, string id, bool save = true)
			{
				this.giverID = id;
				this.effect = e;
				this.shouldSave = save;
			}

			// Token: 0x0400E9E7 RID: 59879
			public string giverID;

			// Token: 0x0400E9E8 RID: 59880
			public Effect effect;

			// Token: 0x0400E9E9 RID: 59881
			public bool shouldSave;
		}

		// Token: 0x02003C96 RID: 15510
		[Serializable]
		public struct SaveLoadImmunities
		{
			// Token: 0x0400E9EA RID: 59882
			public string giverID;

			// Token: 0x0400E9EB RID: 59883
			public string effectID;

			// Token: 0x0400E9EC RID: 59884
			public bool saved;
		}

		// Token: 0x02003C97 RID: 15511
		[Serializable]
		public struct SaveLoadEffect
		{
			// Token: 0x0400E9ED RID: 59885
			public string id;

			// Token: 0x0400E9EE RID: 59886
			public float timeRemaining;

			// Token: 0x0400E9EF RID: 59887
			public bool saved;
		}
	}
}
