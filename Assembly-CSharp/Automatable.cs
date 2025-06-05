using System;
using KSerialization;
using UnityEngine;

// Token: 0x020009C5 RID: 2501
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Automatable")]
public class Automatable : KMonoBehaviour
{
	// Token: 0x06002CDF RID: 11487 RVA: 0x000C17E9 File Offset: 0x000BF9E9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Automatable>(-905833192, Automatable.OnCopySettingsDelegate);
	}

	// Token: 0x06002CE0 RID: 11488 RVA: 0x001FAEFC File Offset: 0x001F90FC
	private void OnCopySettings(object data)
	{
		Automatable component = ((GameObject)data).GetComponent<Automatable>();
		if (component != null)
		{
			this.automationOnly = component.automationOnly;
		}
	}

	// Token: 0x06002CE1 RID: 11489 RVA: 0x000C1802 File Offset: 0x000BFA02
	public bool GetAutomationOnly()
	{
		return this.automationOnly;
	}

	// Token: 0x06002CE2 RID: 11490 RVA: 0x000C180A File Offset: 0x000BFA0A
	public void SetAutomationOnly(bool only)
	{
		this.automationOnly = only;
	}

	// Token: 0x06002CE3 RID: 11491 RVA: 0x000C1813 File Offset: 0x000BFA13
	public bool AllowedByAutomation(bool is_transfer_arm)
	{
		return !this.GetAutomationOnly() || is_transfer_arm;
	}

	// Token: 0x04001EC3 RID: 7875
	[Serialize]
	private bool automationOnly = true;

	// Token: 0x04001EC4 RID: 7876
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001EC5 RID: 7877
	private static readonly EventSystem.IntraObjectHandler<Automatable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Automatable>(delegate(Automatable component, object data)
	{
		component.OnCopySettings(data);
	});
}
