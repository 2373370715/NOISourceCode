using System;
using UnityEngine;

// Token: 0x0200061A RID: 1562
public class GameObjectPool : ObjectPool<GameObject>
{
	// Token: 0x06001BC0 RID: 7104 RVA: 0x000B68E1 File Offset: 0x000B4AE1
	public GameObjectPool(Func<GameObject> instantiator, int initial_count = 0) : base(instantiator, initial_count)
	{
	}

	// Token: 0x06001BC1 RID: 7105 RVA: 0x000B68EB File Offset: 0x000B4AEB
	public override GameObject GetInstance()
	{
		return base.GetInstance();
	}

	// Token: 0x06001BC2 RID: 7106 RVA: 0x001B7F24 File Offset: 0x001B6124
	public void Destroy()
	{
		for (int i = this.unused.Count - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.unused.Pop());
		}
	}
}
