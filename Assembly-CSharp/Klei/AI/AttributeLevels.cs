using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C68 RID: 15464
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/AttributeLevels")]
	public class AttributeLevels : KMonoBehaviour, ISaveLoadable
	{
		// Token: 0x0600ED25 RID: 60709 RVA: 0x00143B5B File Offset: 0x00141D5B
		public IEnumerator<AttributeLevel> GetEnumerator()
		{
			return this.levels.GetEnumerator();
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x0600ED26 RID: 60710 RVA: 0x00143B6D File Offset: 0x00141D6D
		// (set) Token: 0x0600ED27 RID: 60711 RVA: 0x00143B75 File Offset: 0x00141D75
		public AttributeLevels.LevelSaveLoad[] SaveLoadLevels
		{
			get
			{
				return this.saveLoadLevels;
			}
			set
			{
				this.saveLoadLevels = value;
			}
		}

		// Token: 0x0600ED28 RID: 60712 RVA: 0x004DFD08 File Offset: 0x004DDF08
		protected override void OnPrefabInit()
		{
			foreach (AttributeInstance attributeInstance in this.GetAttributes())
			{
				if (attributeInstance.Attribute.IsTrainable)
				{
					AttributeLevel attributeLevel = new AttributeLevel(attributeInstance);
					this.levels.Add(attributeLevel);
					attributeLevel.maxGainedLevel = this.maxAttributeLevel;
					attributeLevel.Apply(this);
				}
			}
		}

		// Token: 0x0600ED29 RID: 60713 RVA: 0x004DFD84 File Offset: 0x004DDF84
		[OnSerializing]
		public void OnSerializing()
		{
			this.saveLoadLevels = new AttributeLevels.LevelSaveLoad[this.levels.Count];
			for (int i = 0; i < this.levels.Count; i++)
			{
				this.saveLoadLevels[i].attributeId = this.levels[i].attribute.Attribute.Id;
				this.saveLoadLevels[i].experience = this.levels[i].experience;
				this.saveLoadLevels[i].level = this.levels[i].level;
			}
		}

		// Token: 0x0600ED2A RID: 60714 RVA: 0x004DFE30 File Offset: 0x004DE030
		[OnDeserialized]
		public void OnDeserialized()
		{
			foreach (AttributeLevels.LevelSaveLoad levelSaveLoad in this.saveLoadLevels)
			{
				this.SetExperience(levelSaveLoad.attributeId, levelSaveLoad.experience);
				this.SetLevel(levelSaveLoad.attributeId, levelSaveLoad.level);
			}
		}

		// Token: 0x0600ED2B RID: 60715 RVA: 0x004DFE80 File Offset: 0x004DE080
		public int GetLevel(Attribute attribute)
		{
			foreach (AttributeLevel attributeLevel in this.levels)
			{
				if (attribute == attributeLevel.attribute.Attribute)
				{
					return attributeLevel.GetLevel();
				}
			}
			return 1;
		}

		// Token: 0x0600ED2C RID: 60716 RVA: 0x004DFEE8 File Offset: 0x004DE0E8
		public AttributeLevel GetAttributeLevel(string attribute_id)
		{
			foreach (AttributeLevel attributeLevel in this.levels)
			{
				if (attributeLevel.attribute.Attribute.Id == attribute_id)
				{
					return attributeLevel;
				}
			}
			return null;
		}

		// Token: 0x0600ED2D RID: 60717 RVA: 0x004DFF54 File Offset: 0x004DE154
		public bool AddExperience(string attribute_id, float time_spent, float multiplier)
		{
			if (this.maxAttributeLevel == 0)
			{
				return false;
			}
			AttributeLevel attributeLevel = this.GetAttributeLevel(attribute_id);
			if (attributeLevel == null)
			{
				global::Debug.LogWarning(attribute_id + " has no level.");
				return false;
			}
			time_spent *= multiplier;
			AttributeConverterInstance attributeConverterInstance = Db.Get().AttributeConverters.TrainingSpeed.Lookup(this);
			if (attributeConverterInstance != null)
			{
				float num = attributeConverterInstance.Evaluate();
				time_spent += time_spent * num;
			}
			bool result = attributeLevel.AddExperience(this, time_spent);
			attributeLevel.Apply(this);
			return result;
		}

		// Token: 0x0600ED2E RID: 60718 RVA: 0x004DFFC8 File Offset: 0x004DE1C8
		public void SetLevel(string attribute_id, int level)
		{
			AttributeLevel attributeLevel = this.GetAttributeLevel(attribute_id);
			if (attributeLevel != null)
			{
				attributeLevel.SetLevel(level);
				attributeLevel.Apply(this);
			}
		}

		// Token: 0x0600ED2F RID: 60719 RVA: 0x004DFFF0 File Offset: 0x004DE1F0
		public void SetExperience(string attribute_id, float experience)
		{
			AttributeLevel attributeLevel = this.GetAttributeLevel(attribute_id);
			if (attributeLevel != null)
			{
				attributeLevel.SetExperience(experience);
				attributeLevel.Apply(this);
			}
		}

		// Token: 0x0600ED30 RID: 60720 RVA: 0x00143B7E File Offset: 0x00141D7E
		public float GetPercentComplete(string attribute_id)
		{
			return this.GetAttributeLevel(attribute_id).GetPercentComplete();
		}

		// Token: 0x0600ED31 RID: 60721 RVA: 0x004E0018 File Offset: 0x004DE218
		public int GetMaxLevel()
		{
			int num = 0;
			foreach (AttributeLevel attributeLevel in this)
			{
				if (attributeLevel.GetLevel() > num)
				{
					num = attributeLevel.GetLevel();
				}
			}
			return num;
		}

		// Token: 0x0400E94A RID: 59722
		private List<AttributeLevel> levels = new List<AttributeLevel>();

		// Token: 0x0400E94B RID: 59723
		public int maxAttributeLevel = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MAX_GAINED_ATTRIBUTE_LEVEL;

		// Token: 0x0400E94C RID: 59724
		[Serialize]
		private AttributeLevels.LevelSaveLoad[] saveLoadLevels = new AttributeLevels.LevelSaveLoad[0];

		// Token: 0x02003C69 RID: 15465
		[Serializable]
		public struct LevelSaveLoad
		{
			// Token: 0x0400E94D RID: 59725
			public string attributeId;

			// Token: 0x0400E94E RID: 59726
			public float experience;

			// Token: 0x0400E94F RID: 59727
			public int level;
		}
	}
}
