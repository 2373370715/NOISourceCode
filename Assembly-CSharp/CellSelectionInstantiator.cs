using System;
using UnityEngine;

// Token: 0x0200109E RID: 4254
public class CellSelectionInstantiator : MonoBehaviour
{
	// Token: 0x06005664 RID: 22116 RVA: 0x0028FA7C File Offset: 0x0028DC7C
	private void Awake()
	{
		GameObject gameObject = Util.KInstantiate(this.CellSelectionPrefab, null, "WorldSelectionCollider");
		GameObject gameObject2 = Util.KInstantiate(this.CellSelectionPrefab, null, "WorldSelectionCollider");
		CellSelectionObject component = gameObject.GetComponent<CellSelectionObject>();
		CellSelectionObject component2 = gameObject2.GetComponent<CellSelectionObject>();
		component.alternateSelectionObject = component2;
		component2.alternateSelectionObject = component;
	}

	// Token: 0x04003D26 RID: 15654
	public GameObject CellSelectionPrefab;
}
