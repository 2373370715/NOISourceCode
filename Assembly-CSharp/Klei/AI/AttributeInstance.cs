using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C66 RID: 15462
	[DebuggerDisplay("{Attribute.Id}")]
	public class AttributeInstance : ModifierInstance<Attribute>
	{
		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x0600ED0B RID: 60683 RVA: 0x00143A3E File Offset: 0x00141C3E
		public string Id
		{
			get
			{
				return this.Attribute.Id;
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x0600ED0C RID: 60684 RVA: 0x00143A4B File Offset: 0x00141C4B
		public string Name
		{
			get
			{
				return this.Attribute.Name;
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x0600ED0D RID: 60685 RVA: 0x00143A58 File Offset: 0x00141C58
		public string Description
		{
			get
			{
				return this.Attribute.Description;
			}
		}

		// Token: 0x0600ED0E RID: 60686 RVA: 0x00143A65 File Offset: 0x00141C65
		public float GetBaseValue()
		{
			return this.Attribute.BaseValue;
		}

		// Token: 0x0600ED0F RID: 60687 RVA: 0x004DF7FC File Offset: 0x004DD9FC
		public float GetTotalDisplayValue()
		{
			float num = this.Attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != this.Modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = this.Modifiers[num3];
				if (!attributeModifier.IsMultiplier)
				{
					num += attributeModifier.Value;
				}
				else
				{
					num2 += attributeModifier.Value;
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x0600ED10 RID: 60688 RVA: 0x004DF870 File Offset: 0x004DDA70
		public float GetTotalValue()
		{
			float num = this.Attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != this.Modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = this.Modifiers[num3];
				if (!attributeModifier.UIOnly)
				{
					if (!attributeModifier.IsMultiplier)
					{
						num += attributeModifier.Value;
					}
					else
					{
						num2 += attributeModifier.Value;
					}
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x0600ED11 RID: 60689 RVA: 0x004DF8EC File Offset: 0x004DDAEC
		public static float GetTotalDisplayValue(Attribute attribute, List<AttributeModifier> modifiers)
		{
			float num = attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = modifiers[num3];
				if (!attributeModifier.IsMultiplier)
				{
					num += attributeModifier.Value;
				}
				else
				{
					num2 += attributeModifier.Value;
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x0600ED12 RID: 60690 RVA: 0x004DF950 File Offset: 0x004DDB50
		public static float GetTotalValue(Attribute attribute, List<AttributeModifier> modifiers)
		{
			float num = attribute.BaseValue;
			float num2 = 0f;
			for (int num3 = 0; num3 != modifiers.Count; num3++)
			{
				AttributeModifier attributeModifier = modifiers[num3];
				if (!attributeModifier.UIOnly)
				{
					if (!attributeModifier.IsMultiplier)
					{
						num += attributeModifier.Value;
					}
					else
					{
						num2 += attributeModifier.Value;
					}
				}
			}
			if (num2 != 0f)
			{
				num += Mathf.Abs(num) * num2;
			}
			return num;
		}

		// Token: 0x0600ED13 RID: 60691 RVA: 0x004DF9BC File Offset: 0x004DDBBC
		public float GetModifierContribution(AttributeModifier testModifier)
		{
			if (!testModifier.IsMultiplier)
			{
				return testModifier.Value;
			}
			float num = this.Attribute.BaseValue;
			for (int num2 = 0; num2 != this.Modifiers.Count; num2++)
			{
				AttributeModifier attributeModifier = this.Modifiers[num2];
				if (!attributeModifier.IsMultiplier)
				{
					num += attributeModifier.Value;
				}
			}
			return num * testModifier.Value;
		}

		// Token: 0x0600ED14 RID: 60692 RVA: 0x00143A72 File Offset: 0x00141C72
		public AttributeInstance(GameObject game_object, Attribute attribute) : base(game_object, attribute)
		{
			DebugUtil.Assert(attribute != null);
			this.Attribute = attribute;
		}

		// Token: 0x0600ED15 RID: 60693 RVA: 0x00143A8C File Offset: 0x00141C8C
		public void Add(AttributeModifier modifier)
		{
			this.Modifiers.Add(modifier);
			if (this.OnDirty != null)
			{
				this.OnDirty();
			}
		}

		// Token: 0x0600ED16 RID: 60694 RVA: 0x004DFA20 File Offset: 0x004DDC20
		public void Remove(AttributeModifier modifier)
		{
			int i = 0;
			while (i < this.Modifiers.Count)
			{
				if (this.Modifiers[i] == modifier)
				{
					this.Modifiers.RemoveAt(i);
					if (this.OnDirty != null)
					{
						this.OnDirty();
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x0600ED17 RID: 60695 RVA: 0x00143AAE File Offset: 0x00141CAE
		public void ClearModifiers()
		{
			if (this.Modifiers.Count > 0)
			{
				this.Modifiers.Clear();
				if (this.OnDirty != null)
				{
					this.OnDirty();
				}
			}
		}

		// Token: 0x0600ED18 RID: 60696 RVA: 0x00143ADC File Offset: 0x00141CDC
		public string GetDescription()
		{
			return string.Format(DUPLICANTS.ATTRIBUTES.VALUE, this.Name, this.GetFormattedValue());
		}

		// Token: 0x0600ED19 RID: 60697 RVA: 0x00143AF9 File Offset: 0x00141CF9
		public string GetFormattedValue()
		{
			return this.Attribute.formatter.GetFormattedAttribute(this);
		}

		// Token: 0x0600ED1A RID: 60698 RVA: 0x00143B0C File Offset: 0x00141D0C
		public string GetAttributeValueTooltip()
		{
			return this.Attribute.GetTooltip(this);
		}

		// Token: 0x0400E940 RID: 59712
		public Attribute Attribute;

		// Token: 0x0400E941 RID: 59713
		public System.Action OnDirty;

		// Token: 0x0400E942 RID: 59714
		public ArrayRef<AttributeModifier> Modifiers;

		// Token: 0x0400E943 RID: 59715
		public bool hide;
	}
}
