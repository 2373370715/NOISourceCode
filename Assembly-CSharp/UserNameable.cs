using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001A5D RID: 6749
[AddComponentMenu("KMonoBehaviour/scripts/UserNameable")]
public class UserNameable : KMonoBehaviour
{
	// Token: 0x06008CA1 RID: 36001 RVA: 0x001007A7 File Offset: 0x000FE9A7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (string.IsNullOrEmpty(this.savedName))
		{
			this.SetName(base.gameObject.GetProperName());
			return;
		}
		this.SetName(this.savedName);
	}

	// Token: 0x06008CA2 RID: 36002 RVA: 0x00373334 File Offset: 0x00371534
	public void SetName(string name)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		base.name = name;
		if (component != null)
		{
			component.SetName(name);
		}
		base.gameObject.name = name;
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
		if (base.GetComponent<CommandModule>() != null)
		{
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(base.GetComponent<LaunchConditionManager>()).SetRocketName(name);
		}
		else if (base.GetComponent<Clustercraft>() != null)
		{
			ClusterNameDisplayScreen.Instance.UpdateName(base.GetComponent<Clustercraft>());
		}
		this.savedName = name;
		base.Trigger(1102426921, name);
	}

	// Token: 0x04006A31 RID: 27185
	[Serialize]
	public string savedName = "";
}
