using System;
using UnityEngine;

// Token: 0x020009D5 RID: 2517
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Cancellable")]
public class Cancellable : KMonoBehaviour
{
	// Token: 0x06002D8E RID: 11662 RVA: 0x000C1F9B File Offset: 0x000C019B
	protected override void OnPrefabInit()
	{
		base.Subscribe<Cancellable>(2127324410, Cancellable.OnCancelDelegate);
	}

	// Token: 0x06002D8F RID: 11663 RVA: 0x000C1FAE File Offset: 0x000C01AE
	protected virtual void OnCancel(object data)
	{
		this.DeleteObject();
	}

	// Token: 0x04001F47 RID: 8007
	private static readonly EventSystem.IntraObjectHandler<Cancellable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Cancellable>(delegate(Cancellable component, object data)
	{
		component.OnCancel(data);
	});
}
