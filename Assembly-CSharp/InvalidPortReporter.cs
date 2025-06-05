using System;

// Token: 0x020011E2 RID: 4578
public class InvalidPortReporter : KMonoBehaviour
{
	// Token: 0x06005D14 RID: 23828 RVA: 0x000E12D3 File Offset: 0x000DF4D3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OnTagsChanged(null);
		base.Subscribe<InvalidPortReporter>(-1582839653, InvalidPortReporter.OnTagsChangedDelegate);
	}

	// Token: 0x06005D15 RID: 23829 RVA: 0x000C4795 File Offset: 0x000C2995
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06005D16 RID: 23830 RVA: 0x002AB4FC File Offset: 0x002A96FC
	private void OnTagsChanged(object data)
	{
		bool flag = base.gameObject.HasTag(GameTags.HasInvalidPorts);
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(InvalidPortReporter.portsNotOverlapping, !flag);
		}
		KSelectable component2 = base.GetComponent<KSelectable>();
		if (component2 != null)
		{
			component2.ToggleStatusItem(Db.Get().BuildingStatusItems.InvalidPortOverlap, flag, base.gameObject);
		}
	}

	// Token: 0x04004250 RID: 16976
	public static readonly Operational.Flag portsNotOverlapping = new Operational.Flag("ports_not_overlapping", Operational.Flag.Type.Functional);

	// Token: 0x04004251 RID: 16977
	private static readonly EventSystem.IntraObjectHandler<InvalidPortReporter> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<InvalidPortReporter>(delegate(InvalidPortReporter component, object data)
	{
		component.OnTagsChanged(data);
	});
}
