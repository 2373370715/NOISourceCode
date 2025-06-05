using System;
using System.Diagnostics;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C5B RID: 15451
	[DebuggerDisplay("{Id}")]
	public class Amount : Resource
	{
		// Token: 0x0600ECCE RID: 60622 RVA: 0x004DF12C File Offset: 0x004DD32C
		public Amount(string id, string name, string description, Attribute min_attribute, Attribute max_attribute, Attribute delta_attribute, bool show_max, Units units, float visual_delta_threshold, bool show_in_ui, string uiSprite = null, string thoughtSprite = null)
		{
			this.Id = id;
			this.Name = name;
			this.description = description;
			this.minAttribute = min_attribute;
			this.maxAttribute = max_attribute;
			this.deltaAttribute = delta_attribute;
			this.showMax = show_max;
			this.units = units;
			this.visualDeltaThreshold = visual_delta_threshold;
			this.showInUI = show_in_ui;
			this.uiSprite = uiSprite;
			this.thoughtSprite = thoughtSprite;
		}

		// Token: 0x0600ECCF RID: 60623 RVA: 0x001436EF File Offset: 0x001418EF
		public void SetDisplayer(IAmountDisplayer displayer)
		{
			this.displayer = displayer;
			this.minAttribute.SetFormatter(displayer.Formatter);
			this.maxAttribute.SetFormatter(displayer.Formatter);
			this.deltaAttribute.SetFormatter(displayer.Formatter);
		}

		// Token: 0x0600ECD0 RID: 60624 RVA: 0x0014372B File Offset: 0x0014192B
		public AmountInstance Lookup(Component cmp)
		{
			return this.Lookup(cmp.gameObject);
		}

		// Token: 0x0600ECD1 RID: 60625 RVA: 0x004DF19C File Offset: 0x004DD39C
		public AmountInstance Lookup(GameObject go)
		{
			Amounts amounts = go.GetAmounts();
			if (amounts != null)
			{
				return amounts.Get(this);
			}
			return null;
		}

		// Token: 0x0600ECD2 RID: 60626 RVA: 0x004DF1BC File Offset: 0x004DD3BC
		public void Copy(GameObject to, GameObject from)
		{
			AmountInstance amountInstance = this.Lookup(to);
			AmountInstance amountInstance2 = this.Lookup(from);
			amountInstance.value = amountInstance2.value;
		}

		// Token: 0x0600ECD3 RID: 60627 RVA: 0x00143739 File Offset: 0x00141939
		public string GetValueString(AmountInstance instance)
		{
			return this.displayer.GetValueString(this, instance);
		}

		// Token: 0x0600ECD4 RID: 60628 RVA: 0x00143748 File Offset: 0x00141948
		public string GetDescription(AmountInstance instance)
		{
			return this.displayer.GetDescription(this, instance);
		}

		// Token: 0x0600ECD5 RID: 60629 RVA: 0x00143757 File Offset: 0x00141957
		public string GetTooltip(AmountInstance instance)
		{
			return this.displayer.GetTooltip(this, instance);
		}

		// Token: 0x0600ECD6 RID: 60630 RVA: 0x00143766 File Offset: 0x00141966
		public void DebugSetValue(AmountInstance instance, float value)
		{
			if (this.debugSetValue != null)
			{
				this.debugSetValue(instance, value);
				return;
			}
			instance.SetValue(value);
		}

		// Token: 0x0400E907 RID: 59655
		public string description;

		// Token: 0x0400E908 RID: 59656
		public bool showMax;

		// Token: 0x0400E909 RID: 59657
		public Units units;

		// Token: 0x0400E90A RID: 59658
		public float visualDeltaThreshold;

		// Token: 0x0400E90B RID: 59659
		public Attribute minAttribute;

		// Token: 0x0400E90C RID: 59660
		public Attribute maxAttribute;

		// Token: 0x0400E90D RID: 59661
		public Attribute deltaAttribute;

		// Token: 0x0400E90E RID: 59662
		public Action<AmountInstance, float> debugSetValue;

		// Token: 0x0400E90F RID: 59663
		public bool showInUI;

		// Token: 0x0400E910 RID: 59664
		public string uiSprite;

		// Token: 0x0400E911 RID: 59665
		public string thoughtSprite;

		// Token: 0x0400E912 RID: 59666
		public IAmountDisplayer displayer;
	}
}
