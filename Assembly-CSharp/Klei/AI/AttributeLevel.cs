using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C67 RID: 15463
	public class AttributeLevel
	{
		// Token: 0x0600ED1B RID: 60699 RVA: 0x004DFA74 File Offset: 0x004DDC74
		public AttributeLevel(AttributeInstance attribute)
		{
			this.notification = new Notification(MISC.NOTIFICATIONS.LEVELUP.NAME, NotificationType.Good, new Func<List<Notification>, object, string>(AttributeLevel.OnLevelUpTooltip), null, true, 0f, null, null, null, true, false, false);
			this.attribute = attribute;
		}

		// Token: 0x0600ED1C RID: 60700 RVA: 0x00143B1A File Offset: 0x00141D1A
		public int GetLevel()
		{
			return this.level;
		}

		// Token: 0x0600ED1D RID: 60701 RVA: 0x004DFAC8 File Offset: 0x004DDCC8
		public void Apply(AttributeLevels levels)
		{
			Attributes attributes = levels.GetAttributes();
			if (this.modifier != null)
			{
				attributes.Remove(this.modifier);
				this.modifier = null;
			}
			this.modifier = new AttributeModifier(this.attribute.Id, (float)this.GetLevel(), DUPLICANTS.MODIFIERS.SKILLLEVEL.NAME, false, false, true);
			attributes.Add(this.modifier);
		}

		// Token: 0x0600ED1E RID: 60702 RVA: 0x00143B22 File Offset: 0x00141D22
		public void SetExperience(float experience)
		{
			this.experience = experience;
		}

		// Token: 0x0600ED1F RID: 60703 RVA: 0x00143B2B File Offset: 0x00141D2B
		public void SetLevel(int level)
		{
			this.level = level;
		}

		// Token: 0x0600ED20 RID: 60704 RVA: 0x004DFB30 File Offset: 0x004DDD30
		public float GetExperienceForNextLevel()
		{
			float num = Mathf.Pow((float)this.level / (float)this.maxGainedLevel, DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER) * (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE * 600f;
			return Mathf.Pow(((float)this.level + 1f) / (float)this.maxGainedLevel, DUPLICANTSTATS.ATTRIBUTE_LEVELING.EXPERIENCE_LEVEL_POWER) * (float)DUPLICANTSTATS.ATTRIBUTE_LEVELING.TARGET_MAX_LEVEL_CYCLE * 600f - num;
		}

		// Token: 0x0600ED21 RID: 60705 RVA: 0x00143B34 File Offset: 0x00141D34
		public float GetPercentComplete()
		{
			return this.experience / this.GetExperienceForNextLevel();
		}

		// Token: 0x0600ED22 RID: 60706 RVA: 0x004DFB94 File Offset: 0x004DDD94
		public void LevelUp(AttributeLevels levels)
		{
			this.level++;
			this.experience = 0f;
			this.Apply(levels);
			this.experience = 0f;
			if (PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, this.attribute.modifier.Name, levels.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
			}
			levels.GetComponent<Notifier>().Add(this.notification, string.Format(MISC.NOTIFICATIONS.LEVELUP.SUFFIX, this.attribute.modifier.Name, this.level));
			StateMachine.Instance instance = new UpgradeFX.Instance(levels.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, -0.1f));
			ReportManager.Instance.ReportValue(ReportManager.ReportType.LevelUp, 1f, levels.GetProperName(), null);
			instance.StartSM();
			levels.Trigger(-110704193, this.attribute.Id);
		}

		// Token: 0x0600ED23 RID: 60707 RVA: 0x004DFCAC File Offset: 0x004DDEAC
		public bool AddExperience(AttributeLevels levels, float experience)
		{
			if (this.level >= this.maxGainedLevel)
			{
				return false;
			}
			this.experience += experience;
			this.experience = Mathf.Max(0f, this.experience);
			if (this.experience >= this.GetExperienceForNextLevel())
			{
				this.LevelUp(levels);
				return true;
			}
			return false;
		}

		// Token: 0x0600ED24 RID: 60708 RVA: 0x00143B43 File Offset: 0x00141D43
		private static string OnLevelUpTooltip(List<Notification> notifications, object data)
		{
			return MISC.NOTIFICATIONS.LEVELUP.TOOLTIP + notifications.ReduceMessages(false);
		}

		// Token: 0x0400E944 RID: 59716
		public float experience;

		// Token: 0x0400E945 RID: 59717
		public int level;

		// Token: 0x0400E946 RID: 59718
		public AttributeInstance attribute;

		// Token: 0x0400E947 RID: 59719
		public AttributeModifier modifier;

		// Token: 0x0400E948 RID: 59720
		public Notification notification;

		// Token: 0x0400E949 RID: 59721
		public int maxGainedLevel = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL;
	}
}
