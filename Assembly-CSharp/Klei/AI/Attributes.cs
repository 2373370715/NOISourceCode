using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C6B RID: 15467
	public class Attributes
	{
		// Token: 0x0600ED46 RID: 60742 RVA: 0x00143C5D File Offset: 0x00141E5D
		public IEnumerator<AttributeInstance> GetEnumerator()
		{
			return this.AttributeTable.GetEnumerator();
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x0600ED47 RID: 60743 RVA: 0x00143C6F File Offset: 0x00141E6F
		public int Count
		{
			get
			{
				return this.AttributeTable.Count;
			}
		}

		// Token: 0x0600ED48 RID: 60744 RVA: 0x00143C7C File Offset: 0x00141E7C
		public Attributes(GameObject game_object)
		{
			this.gameObject = game_object;
		}

		// Token: 0x0600ED49 RID: 60745 RVA: 0x004E0278 File Offset: 0x004DE478
		public AttributeInstance Add(Attribute attribute)
		{
			AttributeInstance attributeInstance = this.Get(attribute.Id);
			if (attributeInstance == null)
			{
				attributeInstance = new AttributeInstance(this.gameObject, attribute);
				this.AttributeTable.Add(attributeInstance);
			}
			return attributeInstance;
		}

		// Token: 0x0600ED4A RID: 60746 RVA: 0x004E02B0 File Offset: 0x004DE4B0
		public void Add(AttributeModifier modifier)
		{
			AttributeInstance attributeInstance = this.Get(modifier.AttributeId);
			if (attributeInstance != null)
			{
				attributeInstance.Add(modifier);
			}
		}

		// Token: 0x0600ED4B RID: 60747 RVA: 0x004E02D4 File Offset: 0x004DE4D4
		public void Remove(AttributeModifier modifier)
		{
			if (modifier == null)
			{
				return;
			}
			AttributeInstance attributeInstance = this.Get(modifier.AttributeId);
			if (attributeInstance != null)
			{
				attributeInstance.Remove(modifier);
			}
		}

		// Token: 0x0600ED4C RID: 60748 RVA: 0x004E02FC File Offset: 0x004DE4FC
		public float GetValuePercent(string attribute_id)
		{
			float result = 1f;
			AttributeInstance attributeInstance = this.Get(attribute_id);
			if (attributeInstance != null)
			{
				result = attributeInstance.GetTotalValue() / attributeInstance.GetBaseValue();
			}
			else
			{
				global::Debug.LogError("Could not find attribute " + attribute_id);
			}
			return result;
		}

		// Token: 0x0600ED4D RID: 60749 RVA: 0x004E033C File Offset: 0x004DE53C
		public AttributeInstance Get(string attribute_id)
		{
			for (int i = 0; i < this.AttributeTable.Count; i++)
			{
				if (this.AttributeTable[i].Id == attribute_id)
				{
					return this.AttributeTable[i];
				}
			}
			return null;
		}

		// Token: 0x0600ED4E RID: 60750 RVA: 0x00143C96 File Offset: 0x00141E96
		public AttributeInstance Get(Attribute attribute)
		{
			return this.Get(attribute.Id);
		}

		// Token: 0x0600ED4F RID: 60751 RVA: 0x004E0388 File Offset: 0x004DE588
		public float GetValue(string id)
		{
			float result = 0f;
			AttributeInstance attributeInstance = this.Get(id);
			if (attributeInstance != null)
			{
				result = attributeInstance.GetTotalValue();
			}
			else
			{
				global::Debug.LogError("Could not find attribute " + id);
			}
			return result;
		}

		// Token: 0x0600ED50 RID: 60752 RVA: 0x004E03C0 File Offset: 0x004DE5C0
		public AttributeInstance GetProfession()
		{
			AttributeInstance attributeInstance = null;
			foreach (AttributeInstance attributeInstance2 in this)
			{
				if (attributeInstance2.modifier.IsProfession)
				{
					if (attributeInstance == null)
					{
						attributeInstance = attributeInstance2;
					}
					else if (attributeInstance.GetTotalValue() < attributeInstance2.GetTotalValue())
					{
						attributeInstance = attributeInstance2;
					}
				}
			}
			return attributeInstance;
		}

		// Token: 0x0600ED51 RID: 60753 RVA: 0x004E0428 File Offset: 0x004DE628
		public string GetProfessionString(bool longform = true)
		{
			AttributeInstance profession = this.GetProfession();
			if ((int)profession.GetTotalValue() == 0)
			{
				return string.Format(longform ? UI.ATTRIBUTELEVEL : UI.ATTRIBUTELEVEL_SHORT, 0, DUPLICANTS.ATTRIBUTES.UNPROFESSIONAL_NAME);
			}
			return string.Format(longform ? UI.ATTRIBUTELEVEL : UI.ATTRIBUTELEVEL_SHORT, (int)profession.GetTotalValue(), profession.modifier.ProfessionName);
		}

		// Token: 0x0600ED52 RID: 60754 RVA: 0x004E049C File Offset: 0x004DE69C
		public string GetProfessionDescriptionString()
		{
			AttributeInstance profession = this.GetProfession();
			if ((int)profession.GetTotalValue() == 0)
			{
				return DUPLICANTS.ATTRIBUTES.UNPROFESSIONAL_DESC;
			}
			return string.Format(DUPLICANTS.ATTRIBUTES.PROFESSION_DESC, profession.modifier.Name);
		}

		// Token: 0x0400E958 RID: 59736
		public List<AttributeInstance> AttributeTable = new List<AttributeInstance>();

		// Token: 0x0400E959 RID: 59737
		public GameObject gameObject;
	}
}
