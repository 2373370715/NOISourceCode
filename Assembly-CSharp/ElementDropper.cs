using System;
using UnityEngine;

// Token: 0x02000D99 RID: 3481
[AddComponentMenu("KMonoBehaviour/scripts/ElementDropper")]
public class ElementDropper : KMonoBehaviour
{
	// Token: 0x060043A6 RID: 17318 RVA: 0x000D0296 File Offset: 0x000CE496
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ElementDropper>(-1697596308, ElementDropper.OnStorageChangedDelegate);
	}

	// Token: 0x060043A7 RID: 17319 RVA: 0x000D02AF File Offset: 0x000CE4AF
	private void OnStorageChanged(object data)
	{
		if (this.storage.GetMassAvailable(this.emitTag) >= this.emitMass)
		{
			this.storage.DropSome(this.emitTag, this.emitMass, false, false, this.emitOffset, true, true);
		}
	}

	// Token: 0x04002ED4 RID: 11988
	[SerializeField]
	public Tag emitTag;

	// Token: 0x04002ED5 RID: 11989
	[SerializeField]
	public float emitMass;

	// Token: 0x04002ED6 RID: 11990
	[SerializeField]
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x04002ED7 RID: 11991
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04002ED8 RID: 11992
	private static readonly EventSystem.IntraObjectHandler<ElementDropper> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ElementDropper>(delegate(ElementDropper component, object data)
	{
		component.OnStorageChanged(data);
	});
}
