using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C63 RID: 15459
	public class AttributeConverter : Resource
	{
		// Token: 0x0600ECFF RID: 60671 RVA: 0x0014398E File Offset: 0x00141B8E
		public AttributeConverter(string id, string name, string description, float multiplier, float base_value, Attribute attribute, IAttributeFormatter formatter = null) : base(id, name)
		{
			this.description = description;
			this.multiplier = multiplier;
			this.baseValue = base_value;
			this.attribute = attribute;
			this.formatter = formatter;
		}

		// Token: 0x0600ED00 RID: 60672 RVA: 0x001439BF File Offset: 0x00141BBF
		public AttributeConverterInstance Lookup(Component cmp)
		{
			return this.Lookup(cmp.gameObject);
		}

		// Token: 0x0600ED01 RID: 60673 RVA: 0x004DF5E0 File Offset: 0x004DD7E0
		public AttributeConverterInstance Lookup(GameObject go)
		{
			AttributeConverters component = go.GetComponent<AttributeConverters>();
			if (component != null)
			{
				return component.Get(this);
			}
			return null;
		}

		// Token: 0x0600ED02 RID: 60674 RVA: 0x004DF608 File Offset: 0x004DD808
		public string DescriptionFromAttribute(float value, GameObject go)
		{
			string text;
			if (this.formatter != null)
			{
				text = this.formatter.GetFormattedValue(value, this.formatter.DeltaTimeSlice);
			}
			else if (this.attribute.formatter != null)
			{
				text = this.attribute.formatter.GetFormattedValue(value, this.attribute.formatter.DeltaTimeSlice);
			}
			else
			{
				text = GameUtil.GetFormattedSimple(value, GameUtil.TimeSlice.None, null);
			}
			if (text != null)
			{
				text = GameUtil.AddPositiveSign(text, value > 0f);
				return string.Format(this.description, text);
			}
			return null;
		}

		// Token: 0x0400E938 RID: 59704
		public string description;

		// Token: 0x0400E939 RID: 59705
		public float multiplier;

		// Token: 0x0400E93A RID: 59706
		public float baseValue;

		// Token: 0x0400E93B RID: 59707
		public Attribute attribute;

		// Token: 0x0400E93C RID: 59708
		public IAttributeFormatter formatter;
	}
}
