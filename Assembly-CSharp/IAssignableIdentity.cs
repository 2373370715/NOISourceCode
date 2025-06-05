using System;
using System.Collections.Generic;

// Token: 0x02000A97 RID: 2711
public interface IAssignableIdentity
{
	// Token: 0x06003153 RID: 12627
	string GetProperName();

	// Token: 0x06003154 RID: 12628
	List<Ownables> GetOwners();

	// Token: 0x06003155 RID: 12629
	Ownables GetSoleOwner();

	// Token: 0x06003156 RID: 12630
	bool IsNull();

	// Token: 0x06003157 RID: 12631
	bool HasOwner(Assignables owner);

	// Token: 0x06003158 RID: 12632
	int NumOwners();
}
