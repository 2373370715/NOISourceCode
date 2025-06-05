using System;
using UnityEngine;

// Token: 0x02000CED RID: 3309
[AddComponentMenu("KMonoBehaviour/scripts/BubbleSpawner")]
public class BubbleSpawner : KMonoBehaviour
{
	// Token: 0x06003F77 RID: 16247 RVA: 0x000CDB9D File Offset: 0x000CBD9D
	protected override void OnSpawn()
	{
		this.emitMass += (UnityEngine.Random.value - 0.5f) * this.emitVariance * this.emitMass;
		base.OnSpawn();
		base.Subscribe<BubbleSpawner>(-1697596308, BubbleSpawner.OnStorageChangedDelegate);
	}

	// Token: 0x06003F78 RID: 16248 RVA: 0x00245A20 File Offset: 0x00243C20
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = this.storage.FindFirst(ElementLoader.FindElementByHash(this.element).tag);
		if (gameObject == null)
		{
			return;
		}
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		if (component.Mass >= this.emitMass)
		{
			gameObject.GetComponent<PrimaryElement>().Mass -= this.emitMass;
			BubbleManager.instance.SpawnBubble(base.transform.GetPosition(), this.initialVelocity, component.ElementID, this.emitMass, component.Temperature);
		}
	}

	// Token: 0x04002BDD RID: 11229
	public SimHashes element;

	// Token: 0x04002BDE RID: 11230
	public float emitMass;

	// Token: 0x04002BDF RID: 11231
	public float emitVariance;

	// Token: 0x04002BE0 RID: 11232
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x04002BE1 RID: 11233
	public Vector2 initialVelocity;

	// Token: 0x04002BE2 RID: 11234
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04002BE3 RID: 11235
	private static readonly EventSystem.IntraObjectHandler<BubbleSpawner> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<BubbleSpawner>(delegate(BubbleSpawner component, object data)
	{
		component.OnStorageChanged(data);
	});
}
