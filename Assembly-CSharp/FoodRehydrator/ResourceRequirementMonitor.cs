using System;

namespace FoodRehydrator
{
	// Token: 0x02002156 RID: 8534
	public class ResourceRequirementMonitor : KMonoBehaviour
	{
		// Token: 0x0600B601 RID: 46593 RVA: 0x004557B8 File Offset: 0x004539B8
		protected override void OnSpawn()
		{
			base.OnSpawn();
			Storage[] components = base.GetComponents<Storage>();
			DebugUtil.DevAssert(components.Length == 2, "Incorrect number of storages on foodrehydrator", null);
			this.packages = components[0];
			this.water = components[1];
			base.Subscribe<ResourceRequirementMonitor>(-1697596308, ResourceRequirementMonitor.OnStorageChangedDelegate);
		}

		// Token: 0x0600B602 RID: 46594 RVA: 0x0011AA06 File Offset: 0x00118C06
		protected float GetAvailableWater()
		{
			return this.water.GetMassAvailable(GameTags.Water);
		}

		// Token: 0x0600B603 RID: 46595 RVA: 0x0011AA18 File Offset: 0x00118C18
		protected bool HasSufficientResources()
		{
			return this.packages.items.Count > 0 && this.GetAvailableWater() > 1f;
		}

		// Token: 0x0600B604 RID: 46596 RVA: 0x0011AA3C File Offset: 0x00118C3C
		protected void OnStorageChanged(object _)
		{
			this.operational.SetFlag(ResourceRequirementMonitor.flag, this.HasSufficientResources());
		}

		// Token: 0x04009009 RID: 36873
		[MyCmpReq]
		private Operational operational;

		// Token: 0x0400900A RID: 36874
		private Storage packages;

		// Token: 0x0400900B RID: 36875
		private Storage water;

		// Token: 0x0400900C RID: 36876
		private static readonly Operational.Flag flag = new Operational.Flag("HasSufficientResources", Operational.Flag.Type.Requirement);

		// Token: 0x0400900D RID: 36877
		private static readonly EventSystem.IntraObjectHandler<ResourceRequirementMonitor> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ResourceRequirementMonitor>(delegate(ResourceRequirementMonitor component, object data)
		{
			component.OnStorageChanged(data);
		});
	}
}
