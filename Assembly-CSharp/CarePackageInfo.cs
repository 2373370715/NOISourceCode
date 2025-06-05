using System;
using UnityEngine;

// Token: 0x0200144C RID: 5196
public class CarePackageInfo : ITelepadDeliverable
{
	// Token: 0x06006AE8 RID: 27368 RVA: 0x000EA96D File Offset: 0x000E8B6D
	public CarePackageInfo(string ID, float amount, Func<bool> requirement)
	{
		this.id = ID;
		this.quantity = amount;
		this.requirement = requirement;
	}

	// Token: 0x06006AE9 RID: 27369 RVA: 0x000EA98A File Offset: 0x000E8B8A
	public CarePackageInfo(string ID, float amount, Func<bool> requirement, string facadeID)
	{
		this.id = ID;
		this.quantity = amount;
		this.requirement = requirement;
		this.facadeID = facadeID;
	}

	// Token: 0x06006AEA RID: 27370 RVA: 0x002EE1B0 File Offset: 0x002EC3B0
	public GameObject Deliver(Vector3 location)
	{
		location += Vector3.right / 2f;
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(CarePackageConfig.ID), location);
		gameObject.SetActive(true);
		gameObject.GetComponent<CarePackage>().SetInfo(this);
		return gameObject;
	}

	// Token: 0x0400512B RID: 20779
	public readonly string id;

	// Token: 0x0400512C RID: 20780
	public readonly float quantity;

	// Token: 0x0400512D RID: 20781
	public readonly Func<bool> requirement;

	// Token: 0x0400512E RID: 20782
	public readonly string facadeID;
}
