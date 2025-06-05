using System;
using UnityEngine;

// Token: 0x02000D71 RID: 3441
public class DevPump : Filterable, ISim1000ms
{
	// Token: 0x060042BE RID: 17086 RVA: 0x002500B8 File Offset: 0x0024E2B8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.elementState == Filterable.ElementState.Liquid)
		{
			base.SelectedTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
			return;
		}
		if (this.elementState == Filterable.ElementState.Gas)
		{
			base.SelectedTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		}
	}

	// Token: 0x060042BF RID: 17087 RVA: 0x000CF968 File Offset: 0x000CDB68
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.filterElementState = this.elementState;
	}

	// Token: 0x060042C0 RID: 17088 RVA: 0x00250108 File Offset: 0x0024E308
	public void Sim1000ms(float dt)
	{
		if (!base.SelectedTag.IsValid)
		{
			return;
		}
		float num = 10f - this.storage.GetAmountAvailable(base.SelectedTag);
		if (num <= 0f)
		{
			return;
		}
		Element element = ElementLoader.GetElement(base.SelectedTag);
		GameObject gameObject = Assets.TryGetPrefab(base.SelectedTag);
		if (element != null)
		{
			this.storage.AddElement(element.id, num, element.defaultValues.temperature, byte.MaxValue, 0, false, false);
			return;
		}
		if (gameObject != null)
		{
			Grid.SceneLayer sceneLayer = gameObject.GetComponent<KBatchedAnimController>().sceneLayer;
			GameObject gameObject2 = GameUtil.KInstantiate(gameObject, sceneLayer, null, 0);
			gameObject2.GetComponent<PrimaryElement>().Units = num;
			gameObject2.SetActive(true);
			this.storage.Store(gameObject2, true, false, true, false);
		}
	}

	// Token: 0x04002E01 RID: 11777
	public Filterable.ElementState elementState = Filterable.ElementState.Liquid;

	// Token: 0x04002E02 RID: 11778
	[MyCmpReq]
	private Storage storage;
}
