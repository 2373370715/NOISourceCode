using System;
using Klei.AI;

// Token: 0x02001694 RID: 5780
public abstract class Need : KMonoBehaviour
{
	// Token: 0x17000787 RID: 1927
	// (get) Token: 0x06007770 RID: 30576 RVA: 0x000F3083 File Offset: 0x000F1283
	// (set) Token: 0x06007771 RID: 30577 RVA: 0x000F308B File Offset: 0x000F128B
	public string Name { get; protected set; }

	// Token: 0x17000788 RID: 1928
	// (get) Token: 0x06007772 RID: 30578 RVA: 0x000F3094 File Offset: 0x000F1294
	// (set) Token: 0x06007773 RID: 30579 RVA: 0x000F309C File Offset: 0x000F129C
	public string ExpectationTooltip { get; protected set; }

	// Token: 0x17000789 RID: 1929
	// (get) Token: 0x06007774 RID: 30580 RVA: 0x000F30A5 File Offset: 0x000F12A5
	// (set) Token: 0x06007775 RID: 30581 RVA: 0x000F30AD File Offset: 0x000F12AD
	public string Tooltip { get; protected set; }

	// Token: 0x06007776 RID: 30582 RVA: 0x000F30B6 File Offset: 0x000F12B6
	public Klei.AI.Attribute GetExpectationAttribute()
	{
		return this.expectationAttribute.Attribute;
	}

	// Token: 0x06007777 RID: 30583 RVA: 0x000F30C3 File Offset: 0x000F12C3
	protected void SetModifier(Need.ModifierType modifier)
	{
		if (this.currentStressModifier != modifier)
		{
			if (this.currentStressModifier != null)
			{
				this.UnapplyModifier(this.currentStressModifier);
			}
			if (modifier != null)
			{
				this.ApplyModifier(modifier);
			}
			this.currentStressModifier = modifier;
		}
	}

	// Token: 0x06007778 RID: 30584 RVA: 0x0031BB2C File Offset: 0x00319D2C
	private void ApplyModifier(Need.ModifierType modifier)
	{
		if (modifier.modifier != null)
		{
			this.GetAttributes().Add(modifier.modifier);
		}
		if (modifier.statusItem != null)
		{
			base.GetComponent<KSelectable>().AddStatusItem(modifier.statusItem, null);
		}
		if (modifier.thought != null)
		{
			this.GetSMI<ThoughtGraph.Instance>().AddThought(modifier.thought);
		}
	}

	// Token: 0x06007779 RID: 30585 RVA: 0x0031BB88 File Offset: 0x00319D88
	private void UnapplyModifier(Need.ModifierType modifier)
	{
		if (modifier.modifier != null)
		{
			this.GetAttributes().Remove(modifier.modifier);
		}
		if (modifier.statusItem != null)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(modifier.statusItem, false);
		}
		if (modifier.thought != null)
		{
			this.GetSMI<ThoughtGraph.Instance>().RemoveThought(modifier.thought);
		}
	}

	// Token: 0x040059F9 RID: 23033
	protected AttributeInstance expectationAttribute;

	// Token: 0x040059FA RID: 23034
	protected Need.ModifierType stressBonus;

	// Token: 0x040059FB RID: 23035
	protected Need.ModifierType stressNeutral;

	// Token: 0x040059FC RID: 23036
	protected Need.ModifierType stressPenalty;

	// Token: 0x040059FD RID: 23037
	protected Need.ModifierType currentStressModifier;

	// Token: 0x02001695 RID: 5781
	protected class ModifierType
	{
		// Token: 0x04005A01 RID: 23041
		public AttributeModifier modifier;

		// Token: 0x04005A02 RID: 23042
		public StatusItem statusItem;

		// Token: 0x04005A03 RID: 23043
		public Thought thought;
	}
}
