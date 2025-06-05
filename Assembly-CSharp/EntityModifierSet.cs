using System;
using Database;

// Token: 0x02001AE5 RID: 6885
public class EntityModifierSet : ModifierSet
{
	// Token: 0x06008FF9 RID: 36857 RVA: 0x00102764 File Offset: 0x00100964
	public override void Initialize()
	{
		base.Initialize();
		this.DuplicantStatusItems = new DuplicantStatusItems(this.Root);
		this.ChoreGroups = new ChoreGroups(this.Root);
		base.LoadTraits();
	}

	// Token: 0x04006CBD RID: 27837
	public DuplicantStatusItems DuplicantStatusItems;

	// Token: 0x04006CBE RID: 27838
	public ChoreGroups ChoreGroups;
}
