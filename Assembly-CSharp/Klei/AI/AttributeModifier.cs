using System;
using System.Diagnostics;

namespace Klei.AI
{
	// Token: 0x02003C6A RID: 15466
	[DebuggerDisplay("{AttributeId}")]
	public class AttributeModifier
	{
		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x0600ED33 RID: 60723 RVA: 0x00143BB6 File Offset: 0x00141DB6
		// (set) Token: 0x0600ED34 RID: 60724 RVA: 0x00143BBE File Offset: 0x00141DBE
		public string AttributeId { get; private set; }

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x0600ED35 RID: 60725 RVA: 0x00143BC7 File Offset: 0x00141DC7
		// (set) Token: 0x0600ED36 RID: 60726 RVA: 0x00143BCF File Offset: 0x00141DCF
		public float Value { get; private set; }

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x0600ED37 RID: 60727 RVA: 0x00143BD8 File Offset: 0x00141DD8
		// (set) Token: 0x0600ED38 RID: 60728 RVA: 0x00143BE0 File Offset: 0x00141DE0
		public bool IsMultiplier { get; private set; }

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x0600ED39 RID: 60729 RVA: 0x00143BE9 File Offset: 0x00141DE9
		// (set) Token: 0x0600ED3A RID: 60730 RVA: 0x00143BF1 File Offset: 0x00141DF1
		public GameUtil.TimeSlice? OverrideTimeSlice { get; set; }

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x0600ED3B RID: 60731 RVA: 0x00143BFA File Offset: 0x00141DFA
		// (set) Token: 0x0600ED3C RID: 60732 RVA: 0x00143C02 File Offset: 0x00141E02
		public bool UIOnly { get; private set; }

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x0600ED3D RID: 60733 RVA: 0x00143C0B File Offset: 0x00141E0B
		// (set) Token: 0x0600ED3E RID: 60734 RVA: 0x00143C13 File Offset: 0x00141E13
		public bool IsReadonly { get; private set; }

		// Token: 0x0600ED3F RID: 60735 RVA: 0x004E006C File Offset: 0x004DE26C
		public AttributeModifier(string attribute_id, float value, string description = null, bool is_multiplier = false, bool uiOnly = false, bool is_readonly = true)
		{
			this.AttributeId = attribute_id;
			this.Value = value;
			this.Description = ((description == null) ? attribute_id : description);
			this.DescriptionCB = null;
			this.IsMultiplier = is_multiplier;
			this.UIOnly = uiOnly;
			this.IsReadonly = is_readonly;
			this.OverrideTimeSlice = null;
		}

		// Token: 0x0600ED40 RID: 60736 RVA: 0x004E00C8 File Offset: 0x004DE2C8
		public AttributeModifier(string attribute_id, float value, Func<string> description_cb, bool is_multiplier = false, bool uiOnly = false)
		{
			this.AttributeId = attribute_id;
			this.Value = value;
			this.DescriptionCB = description_cb;
			this.Description = null;
			this.IsMultiplier = is_multiplier;
			this.UIOnly = uiOnly;
			this.OverrideTimeSlice = null;
			if (description_cb == null)
			{
				global::Debug.LogWarning("AttributeModifier being constructed without a description callback: " + attribute_id);
			}
		}

		// Token: 0x0600ED41 RID: 60737 RVA: 0x00143C1C File Offset: 0x00141E1C
		public void SetValue(float value)
		{
			this.Value = value;
		}

		// Token: 0x0600ED42 RID: 60738 RVA: 0x004E012C File Offset: 0x004DE32C
		public string GetName()
		{
			Attribute attribute = Db.Get().Attributes.TryGet(this.AttributeId);
			if (attribute != null && attribute.ShowInUI != Attribute.Display.Never)
			{
				return attribute.Name;
			}
			return "";
		}

		// Token: 0x0600ED43 RID: 60739 RVA: 0x00143C25 File Offset: 0x00141E25
		public string GetDescription()
		{
			if (this.DescriptionCB == null)
			{
				return this.Description;
			}
			return this.DescriptionCB();
		}

		// Token: 0x0600ED44 RID: 60740 RVA: 0x004E0168 File Offset: 0x004DE368
		public string GetFormattedString()
		{
			IAttributeFormatter attributeFormatter = null;
			Attribute attribute = Db.Get().Attributes.TryGet(this.AttributeId);
			if (!this.IsMultiplier)
			{
				if (attribute != null)
				{
					attributeFormatter = attribute.formatter;
				}
				else
				{
					attribute = Db.Get().BuildingAttributes.TryGet(this.AttributeId);
					if (attribute != null)
					{
						attributeFormatter = attribute.formatter;
					}
					else
					{
						attribute = Db.Get().PlantAttributes.TryGet(this.AttributeId);
						if (attribute != null)
						{
							attributeFormatter = attribute.formatter;
						}
					}
				}
			}
			string text = "";
			if (attributeFormatter != null)
			{
				text = attributeFormatter.GetFormattedModifier(this);
			}
			else if (this.IsMultiplier)
			{
				text += GameUtil.GetFormattedPercent(this.Value * 100f, GameUtil.TimeSlice.None);
			}
			else
			{
				text += GameUtil.GetFormattedSimple(this.Value, GameUtil.TimeSlice.None, null);
			}
			if (text != null && text.Length > 0 && text[0] != '-')
			{
				GameUtil.TimeSlice? overrideTimeSlice = this.OverrideTimeSlice;
				GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None;
				if (!(overrideTimeSlice.GetValueOrDefault() == timeSlice & overrideTimeSlice != null))
				{
					text = GameUtil.AddPositiveSign(text, this.Value > 0f);
				}
			}
			return text;
		}

		// Token: 0x0600ED45 RID: 60741 RVA: 0x00143C41 File Offset: 0x00141E41
		public AttributeModifier Clone()
		{
			return new AttributeModifier(this.AttributeId, this.Value, this.Description, false, false, true);
		}

		// Token: 0x0400E956 RID: 59734
		public string Description;

		// Token: 0x0400E957 RID: 59735
		public Func<string> DescriptionCB;
	}
}
