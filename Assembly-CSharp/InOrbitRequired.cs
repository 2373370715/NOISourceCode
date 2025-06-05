using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A9A RID: 2714
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/InOrbitRequired")]
public class InOrbitRequired : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06003169 RID: 12649 RVA: 0x0020CA14 File Offset: 0x0020AC14
	protected override void OnSpawn()
	{
		WorldContainer myWorld = this.GetMyWorld();
		this.craftModuleInterface = myWorld.GetComponent<CraftModuleInterface>();
		base.OnSpawn();
		bool newInOrbit = this.craftModuleInterface.HasTag(GameTags.RocketNotOnGround);
		this.UpdateFlag(newInOrbit);
		this.craftModuleInterface.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
	}

	// Token: 0x0600316A RID: 12650 RVA: 0x000C489A File Offset: 0x000C2A9A
	protected override void OnCleanUp()
	{
		if (this.craftModuleInterface != null)
		{
			this.craftModuleInterface.Unsubscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		}
	}

	// Token: 0x0600316B RID: 12651 RVA: 0x0020CA70 File Offset: 0x0020AC70
	private void OnTagsChanged(object data)
	{
		TagChangedEventData tagChangedEventData = (TagChangedEventData)data;
		if (tagChangedEventData.tag == GameTags.RocketNotOnGround)
		{
			this.UpdateFlag(tagChangedEventData.added);
		}
	}

	// Token: 0x0600316C RID: 12652 RVA: 0x000C48C6 File Offset: 0x000C2AC6
	private void UpdateFlag(bool newInOrbit)
	{
		this.operational.SetFlag(InOrbitRequired.inOrbitFlag, newInOrbit);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.InOrbitRequired, !newInOrbit, this);
	}

	// Token: 0x0600316D RID: 12653 RVA: 0x000C48F9 File Offset: 0x000C2AF9
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.BUILDINGEFFECTS.IN_ORBIT_REQUIRED, UI.BUILDINGEFFECTS.TOOLTIPS.IN_ORBIT_REQUIRED, Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x040021ED RID: 8685
	[MyCmpReq]
	private Building building;

	// Token: 0x040021EE RID: 8686
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040021EF RID: 8687
	public static readonly Operational.Flag inOrbitFlag = new Operational.Flag("in_orbit", Operational.Flag.Type.Requirement);

	// Token: 0x040021F0 RID: 8688
	private CraftModuleInterface craftModuleInterface;
}
