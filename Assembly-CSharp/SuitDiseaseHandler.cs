using System;
using UnityEngine;

// Token: 0x02001A25 RID: 6693
[AddComponentMenu("KMonoBehaviour/scripts/SuitDiseaseHandler")]
public class SuitDiseaseHandler : KMonoBehaviour
{
	// Token: 0x06008B61 RID: 35681 RVA: 0x000FFBE0 File Offset: 0x000FDDE0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<SuitDiseaseHandler>(-1617557748, SuitDiseaseHandler.OnEquippedDelegate);
		base.Subscribe<SuitDiseaseHandler>(-170173755, SuitDiseaseHandler.OnUnequippedDelegate);
	}

	// Token: 0x06008B62 RID: 35682 RVA: 0x0036DAD0 File Offset: 0x0036BCD0
	private PrimaryElement GetPrimaryElement(object data)
	{
		GameObject targetGameObject = ((Equipment)data).GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		if (targetGameObject)
		{
			return targetGameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06008B63 RID: 35683 RVA: 0x0036DB00 File Offset: 0x0036BD00
	private void OnEquipped(object data)
	{
		PrimaryElement primaryElement = this.GetPrimaryElement(data);
		if (primaryElement != null)
		{
			primaryElement.ForcePermanentDiseaseContainer(true);
			primaryElement.RedirectDisease(base.gameObject);
		}
	}

	// Token: 0x06008B64 RID: 35684 RVA: 0x0036DB34 File Offset: 0x0036BD34
	private void OnUnequipped(object data)
	{
		PrimaryElement primaryElement = this.GetPrimaryElement(data);
		if (primaryElement != null)
		{
			primaryElement.ForcePermanentDiseaseContainer(false);
			primaryElement.RedirectDisease(null);
		}
	}

	// Token: 0x06008B65 RID: 35685 RVA: 0x000FFC0A File Offset: 0x000FDE0A
	private void OnModifyDiseaseCount(int delta, string reason)
	{
		base.GetComponent<PrimaryElement>().ModifyDiseaseCount(delta, reason);
	}

	// Token: 0x06008B66 RID: 35686 RVA: 0x000FFC19 File Offset: 0x000FDE19
	private void OnAddDisease(byte disease_idx, int delta, string reason)
	{
		base.GetComponent<PrimaryElement>().AddDisease(disease_idx, delta, reason);
	}

	// Token: 0x04006943 RID: 26947
	private static readonly EventSystem.IntraObjectHandler<SuitDiseaseHandler> OnEquippedDelegate = new EventSystem.IntraObjectHandler<SuitDiseaseHandler>(delegate(SuitDiseaseHandler component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x04006944 RID: 26948
	private static readonly EventSystem.IntraObjectHandler<SuitDiseaseHandler> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<SuitDiseaseHandler>(delegate(SuitDiseaseHandler component, object data)
	{
		component.OnUnequipped(data);
	});
}
