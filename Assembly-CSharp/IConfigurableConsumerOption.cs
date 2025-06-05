using System;
using UnityEngine;

// Token: 0x02001FBE RID: 8126
public interface IConfigurableConsumerOption
{
	// Token: 0x0600ABBC RID: 43964
	Tag GetID();

	// Token: 0x0600ABBD RID: 43965
	string GetName();

	// Token: 0x0600ABBE RID: 43966
	string GetDetailedDescription();

	// Token: 0x0600ABBF RID: 43967
	string GetDescription();

	// Token: 0x0600ABC0 RID: 43968
	Sprite GetIcon();

	// Token: 0x0600ABC1 RID: 43969
	IConfigurableConsumerIngredient[] GetIngredients();
}
