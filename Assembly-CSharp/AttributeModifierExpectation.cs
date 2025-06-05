using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02001314 RID: 4884
public class AttributeModifierExpectation : Expectation
{
	// Token: 0x06006415 RID: 25621 RVA: 0x002CA8F8 File Offset: 0x002C8AF8
	public AttributeModifierExpectation(string id, string name, string description, AttributeModifier modifier, Sprite icon) : base(id, name, description, delegate(MinionResume resume)
	{
		resume.GetAttributes().Get(modifier.AttributeId).Add(modifier);
	}, delegate(MinionResume resume)
	{
		resume.GetAttributes().Get(modifier.AttributeId).Remove(modifier);
	})
	{
		this.modifier = modifier;
		this.icon = icon;
	}

	// Token: 0x040047E7 RID: 18407
	public AttributeModifier modifier;

	// Token: 0x040047E8 RID: 18408
	public Sprite icon;
}
