using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C64 RID: 15460
	public class AttributeConverterInstance : ModifierInstance<AttributeConverter>
	{
		// Token: 0x0600ED03 RID: 60675 RVA: 0x001439CD File Offset: 0x00141BCD
		public AttributeConverterInstance(GameObject game_object, AttributeConverter converter, AttributeInstance attribute_instance) : base(game_object, converter)
		{
			this.converter = converter;
			this.attributeInstance = attribute_instance;
		}

		// Token: 0x0600ED04 RID: 60676 RVA: 0x001439E5 File Offset: 0x00141BE5
		public float Evaluate()
		{
			return this.converter.multiplier * this.attributeInstance.GetTotalValue() + this.converter.baseValue;
		}

		// Token: 0x0600ED05 RID: 60677 RVA: 0x00143A0A File Offset: 0x00141C0A
		public string DescriptionFromAttribute(float value, GameObject go)
		{
			return this.converter.DescriptionFromAttribute(this.Evaluate(), go);
		}

		// Token: 0x0400E93D RID: 59709
		public AttributeConverter converter;

		// Token: 0x0400E93E RID: 59710
		public AttributeInstance attributeInstance;
	}
}
