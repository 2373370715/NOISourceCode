using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C65 RID: 15461
	[AddComponentMenu("KMonoBehaviour/scripts/AttributeConverters")]
	public class AttributeConverters : KMonoBehaviour
	{
		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x0600ED06 RID: 60678 RVA: 0x00143A1E File Offset: 0x00141C1E
		public int Count
		{
			get
			{
				return this.converters.Count;
			}
		}

		// Token: 0x0600ED07 RID: 60679 RVA: 0x004DF694 File Offset: 0x004DD894
		protected override void OnPrefabInit()
		{
			foreach (AttributeInstance attributeInstance in this.GetAttributes())
			{
				foreach (AttributeConverter converter in attributeInstance.Attribute.converters)
				{
					AttributeConverterInstance item = new AttributeConverterInstance(base.gameObject, converter, attributeInstance);
					this.converters.Add(item);
				}
			}
		}

		// Token: 0x0600ED08 RID: 60680 RVA: 0x004DF738 File Offset: 0x004DD938
		public AttributeConverterInstance Get(AttributeConverter converter)
		{
			foreach (AttributeConverterInstance attributeConverterInstance in this.converters)
			{
				if (attributeConverterInstance.converter == converter)
				{
					return attributeConverterInstance;
				}
			}
			return null;
		}

		// Token: 0x0600ED09 RID: 60681 RVA: 0x004DF794 File Offset: 0x004DD994
		public AttributeConverterInstance GetConverter(string id)
		{
			foreach (AttributeConverterInstance attributeConverterInstance in this.converters)
			{
				if (attributeConverterInstance.converter.Id == id)
				{
					return attributeConverterInstance;
				}
			}
			return null;
		}

		// Token: 0x0400E93F RID: 59711
		public List<AttributeConverterInstance> converters = new List<AttributeConverterInstance>();
	}
}
