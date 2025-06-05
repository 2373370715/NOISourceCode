using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001442 RID: 5186
public interface IStorage
{
	// Token: 0x06006A53 RID: 27219
	bool ShouldShowInUI();

	// Token: 0x170006C7 RID: 1735
	// (get) Token: 0x06006A54 RID: 27220
	// (set) Token: 0x06006A55 RID: 27221
	bool allowUIItemRemoval { get; set; }

	// Token: 0x06006A56 RID: 27222
	GameObject Drop(GameObject go, bool do_disease_transfer = true);

	// Token: 0x06006A57 RID: 27223
	List<GameObject> GetItems();

	// Token: 0x06006A58 RID: 27224
	bool IsFull();

	// Token: 0x06006A59 RID: 27225
	bool IsEmpty();

	// Token: 0x06006A5A RID: 27226
	float Capacity();

	// Token: 0x06006A5B RID: 27227
	float RemainingCapacity();

	// Token: 0x06006A5C RID: 27228
	float GetAmountAvailable(Tag tag);

	// Token: 0x06006A5D RID: 27229
	void ConsumeIgnoringDisease(Tag tag, float amount);
}
