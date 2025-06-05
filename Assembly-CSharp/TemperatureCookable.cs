using System;
using UnityEngine;

// Token: 0x0200121F RID: 4639
[AddComponentMenu("KMonoBehaviour/scripts/TemperatureCookable")]
public class TemperatureCookable : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06005E10 RID: 24080 RVA: 0x000E1D65 File Offset: 0x000DFF65
	public void Sim1000ms(float dt)
	{
		if (this.element.Temperature > this.cookTemperature && this.cookedID != null)
		{
			this.Cook();
		}
	}

	// Token: 0x06005E11 RID: 24081 RVA: 0x002AEA18 File Offset: 0x002ACC18
	private void Cook()
	{
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(this.cookedID), position);
		gameObject.SetActive(true);
		KSelectable component = base.gameObject.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
		}
		PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
		component2.Temperature = this.element.Temperature;
		component2.Mass = this.element.Mass;
		base.gameObject.DeleteObject();
	}

	// Token: 0x0400431D RID: 17181
	[MyCmpReq]
	private PrimaryElement element;

	// Token: 0x0400431E RID: 17182
	public float cookTemperature = 273150f;

	// Token: 0x0400431F RID: 17183
	public string cookedID;
}
