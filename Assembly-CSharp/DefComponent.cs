using System;
using UnityEngine;

// Token: 0x02000855 RID: 2133
[Serializable]
public class DefComponent<T> where T : Component
{
	// Token: 0x060025AC RID: 9644 RVA: 0x000BD270 File Offset: 0x000BB470
	public DefComponent(T cmp)
	{
		this.cmp = cmp;
	}

	// Token: 0x060025AD RID: 9645 RVA: 0x001D9DE8 File Offset: 0x001D7FE8
	public T Get(StateMachine.Instance smi)
	{
		T[] components = this.cmp.GetComponents<T>();
		int num = 0;
		while (num < components.Length && !(components[num] == this.cmp))
		{
			num++;
		}
		return smi.gameObject.GetComponents<T>()[num];
	}

	// Token: 0x060025AE RID: 9646 RVA: 0x000BD27F File Offset: 0x000BB47F
	public static implicit operator DefComponent<T>(T cmp)
	{
		return new DefComponent<T>(cmp);
	}

	// Token: 0x040019F0 RID: 6640
	[SerializeField]
	private T cmp;
}
