using System;
using KSerialization;

// Token: 0x020016DD RID: 5853
[SerializationConfig(MemberSerialization.OptIn)]
public class Ownables : Assignables
{
	// Token: 0x060078B6 RID: 30902 RVA: 0x000F3D4B File Offset: 0x000F1F4B
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060078B7 RID: 30903 RVA: 0x002C83E8 File Offset: 0x002C65E8
	public void UnassignAll()
	{
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			if (assignableSlotInstance.assignable != null)
			{
				assignableSlotInstance.assignable.Unassign();
			}
		}
	}
}
