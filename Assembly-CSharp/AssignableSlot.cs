using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C63 RID: 3171
[DebuggerDisplay("{Id}")]
[Serializable]
public class AssignableSlot : Resource
{
	// Token: 0x06003C1D RID: 15389 RVA: 0x000CB3DE File Offset: 0x000C95DE
	public AssignableSlot(string id, string name, bool showInUI = true) : base(id, name)
	{
		this.showInUI = showInUI;
	}

	// Token: 0x06003C1E RID: 15390 RVA: 0x0023B048 File Offset: 0x00239248
	public AssignableSlotInstance Lookup(GameObject go)
	{
		Assignables component = go.GetComponent<Assignables>();
		if (component != null)
		{
			return component.GetSlot(this);
		}
		return null;
	}

	// Token: 0x040029D8 RID: 10712
	public bool showInUI = true;
}
