using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C61 RID: 15457
	public class Attribute : Resource, IHasDlcRestrictions
	{
		// Token: 0x0600ECF5 RID: 60661 RVA: 0x004DF46C File Offset: 0x004DD66C
		public Attribute(string id, bool is_trainable, Attribute.Display show_in_ui, bool is_profession, float base_value = 0f, string uiSprite = null, string thoughtSprite = null, string uiFullColourSprite = null, string[] overrideDLCIDs = null) : base(id, null, null)
		{
			string str = "STRINGS.DUPLICANTS.ATTRIBUTES." + id.ToUpper();
			this.Name = Strings.Get(new StringKey(str + ".NAME"));
			this.ProfessionName = Strings.Get(new StringKey(str + ".NAME"));
			this.Description = Strings.Get(new StringKey(str + ".DESC"));
			this.IsTrainable = is_trainable;
			this.IsProfession = is_profession;
			this.ShowInUI = show_in_ui;
			this.BaseValue = base_value;
			this.formatter = Attribute.defaultFormatter;
			this.uiSprite = uiSprite;
			this.thoughtSprite = thoughtSprite;
			this.uiFullColourSprite = uiFullColourSprite;
			this.requiredDlcIds = overrideDLCIDs;
		}

		// Token: 0x0600ECF6 RID: 60662 RVA: 0x004DF548 File Offset: 0x004DD748
		public Attribute(string id, string name, string profession_name, string attribute_description, float base_value, Attribute.Display show_in_ui, bool is_trainable, string uiSprite = null, string thoughtSprite = null, string uiFullColourSprite = null) : base(id, name)
		{
			this.Description = attribute_description;
			this.ProfessionName = profession_name;
			this.BaseValue = base_value;
			this.ShowInUI = show_in_ui;
			this.IsTrainable = is_trainable;
			this.uiSprite = uiSprite;
			this.thoughtSprite = thoughtSprite;
			this.uiFullColourSprite = uiFullColourSprite;
			if (this.ProfessionName == "")
			{
				this.ProfessionName = null;
			}
		}

		// Token: 0x0600ECF7 RID: 60663 RVA: 0x0014394A File Offset: 0x00141B4A
		public void SetFormatter(IAttributeFormatter formatter)
		{
			this.formatter = formatter;
		}

		// Token: 0x0600ECF8 RID: 60664 RVA: 0x00143953 File Offset: 0x00141B53
		public AttributeInstance Lookup(Component cmp)
		{
			return this.Lookup(cmp.gameObject);
		}

		// Token: 0x0600ECF9 RID: 60665 RVA: 0x004DF5C0 File Offset: 0x004DD7C0
		public AttributeInstance Lookup(GameObject go)
		{
			Attributes attributes = go.GetAttributes();
			if (attributes != null)
			{
				return attributes.Get(this);
			}
			return null;
		}

		// Token: 0x0600ECFA RID: 60666 RVA: 0x00143961 File Offset: 0x00141B61
		public string GetDescription(AttributeInstance instance)
		{
			return instance.GetDescription();
		}

		// Token: 0x0600ECFB RID: 60667 RVA: 0x00143969 File Offset: 0x00141B69
		public string GetTooltip(AttributeInstance instance)
		{
			return this.formatter.GetTooltip(this, instance);
		}

		// Token: 0x0600ECFC RID: 60668 RVA: 0x00143978 File Offset: 0x00141B78
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600ECFD RID: 60669 RVA: 0x000AA765 File Offset: 0x000A8965
		public string[] GetForbiddenDlcIds()
		{
			return null;
		}

		// Token: 0x0400E923 RID: 59683
		private static readonly StandardAttributeFormatter defaultFormatter = new StandardAttributeFormatter(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None);

		// Token: 0x0400E924 RID: 59684
		public string Description;

		// Token: 0x0400E925 RID: 59685
		public float BaseValue;

		// Token: 0x0400E926 RID: 59686
		public Attribute.Display ShowInUI;

		// Token: 0x0400E927 RID: 59687
		public bool IsTrainable;

		// Token: 0x0400E928 RID: 59688
		public bool IsProfession;

		// Token: 0x0400E929 RID: 59689
		public string ProfessionName;

		// Token: 0x0400E92A RID: 59690
		public List<AttributeConverter> converters = new List<AttributeConverter>();

		// Token: 0x0400E92B RID: 59691
		public string uiSprite;

		// Token: 0x0400E92C RID: 59692
		public string thoughtSprite;

		// Token: 0x0400E92D RID: 59693
		public string uiFullColourSprite;

		// Token: 0x0400E92E RID: 59694
		public string[] requiredDlcIds;

		// Token: 0x0400E92F RID: 59695
		public string[] forbiddenDlcIds;

		// Token: 0x0400E930 RID: 59696
		public IAttributeFormatter formatter;

		// Token: 0x02003C62 RID: 15458
		public enum Display
		{
			// Token: 0x0400E932 RID: 59698
			Normal,
			// Token: 0x0400E933 RID: 59699
			Skill,
			// Token: 0x0400E934 RID: 59700
			Expectation,
			// Token: 0x0400E935 RID: 59701
			General,
			// Token: 0x0400E936 RID: 59702
			Details,
			// Token: 0x0400E937 RID: 59703
			Never
		}
	}
}
