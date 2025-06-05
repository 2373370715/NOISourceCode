using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001DA0 RID: 7584
public class KleiItemDropScreen_PermitVis : KMonoBehaviour
{
	// Token: 0x06009E70 RID: 40560 RVA: 0x003DC7D8 File Offset: 0x003DA9D8
	public void ConfigureWith(DropScreenPresentationInfo info)
	{
		this.ResetState();
		this.equipmentVis.gameObject.SetActive(false);
		this.fallbackVis.gameObject.SetActive(false);
		if (info.UseEquipmentVis)
		{
			this.equipmentVis.gameObject.SetActive(true);
			this.equipmentVis.ConfigureWith(info);
			return;
		}
		this.fallbackVis.gameObject.SetActive(true);
		this.fallbackVis.ConfigureWith(info);
	}

	// Token: 0x06009E71 RID: 40561 RVA: 0x0010B953 File Offset: 0x00109B53
	public Promise AnimateIn()
	{
		return Updater.RunRoutine(this, this.AnimateInRoutine());
	}

	// Token: 0x06009E72 RID: 40562 RVA: 0x0010B961 File Offset: 0x00109B61
	public Promise AnimateOut()
	{
		return Updater.RunRoutine(this, this.AnimateOutRoutine());
	}

	// Token: 0x06009E73 RID: 40563 RVA: 0x0010B96F File Offset: 0x00109B6F
	private IEnumerator AnimateInRoutine()
	{
		this.root.gameObject.SetActive(true);
		yield return Updater.Ease(delegate(Vector3 v3)
		{
			this.root.transform.localScale = v3;
		}, this.root.transform.localScale, Vector3.one, 0.5f, Easing.EaseOutBack, -1f);
		yield break;
	}

	// Token: 0x06009E74 RID: 40564 RVA: 0x0010B97E File Offset: 0x00109B7E
	private IEnumerator AnimateOutRoutine()
	{
		yield return Updater.Ease(delegate(Vector3 v3)
		{
			this.root.transform.localScale = v3;
		}, this.root.transform.localScale, Vector3.zero, 0.25f, null, -1f);
		this.root.gameObject.SetActive(true);
		yield break;
	}

	// Token: 0x06009E75 RID: 40565 RVA: 0x0010B98D File Offset: 0x00109B8D
	public void ResetState()
	{
		this.root.transform.localScale = Vector3.zero;
	}

	// Token: 0x04007C87 RID: 31879
	[SerializeField]
	private RectTransform root;

	// Token: 0x04007C88 RID: 31880
	[Header("Different Permit Visualizers")]
	[SerializeField]
	private KleiItemDropScreen_PermitVis_Fallback fallbackVis;

	// Token: 0x04007C89 RID: 31881
	[SerializeField]
	private KleiItemDropScreen_PermitVis_DupeEquipment equipmentVis;
}
