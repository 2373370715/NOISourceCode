using System;
using UnityEngine;

// Token: 0x02001804 RID: 6148
[AddComponentMenu("KMonoBehaviour/scripts/Reservable")]
public class Reservable : KMonoBehaviour
{
	// Token: 0x17000804 RID: 2052
	// (get) Token: 0x06007E8B RID: 32395 RVA: 0x000F7C6E File Offset: 0x000F5E6E
	public GameObject ReservedBy
	{
		get
		{
			return this.reservedBy;
		}
	}

	// Token: 0x17000805 RID: 2053
	// (get) Token: 0x06007E8C RID: 32396 RVA: 0x000F7C76 File Offset: 0x000F5E76
	public bool isReserved
	{
		get
		{
			return !(this.reservedBy == null);
		}
	}

	// Token: 0x06007E8D RID: 32397 RVA: 0x000F7C87 File Offset: 0x000F5E87
	public bool Reserve(GameObject reserver)
	{
		if (this.reservedBy == null)
		{
			this.reservedBy = reserver;
			return true;
		}
		return false;
	}

	// Token: 0x06007E8E RID: 32398 RVA: 0x000F7CA1 File Offset: 0x000F5EA1
	public void ClearReservation(GameObject reserver)
	{
		if (this.reservedBy == reserver)
		{
			this.reservedBy = null;
		}
	}

	// Token: 0x04006024 RID: 24612
	private GameObject reservedBy;
}
