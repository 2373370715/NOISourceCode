using System;
using UnityEngine;

namespace FoodRehydrator
{
	// Token: 0x02002155 RID: 8533
	public class AccessabilityManager : KMonoBehaviour
	{
		// Token: 0x0600B5F8 RID: 46584 RVA: 0x0011A91F File Offset: 0x00118B1F
		protected override void OnSpawn()
		{
			base.OnSpawn();
			Components.FoodRehydrators.Add(base.gameObject);
			base.Subscribe(824508782, new Action<object>(this.ActiveChangedHandler));
		}

		// Token: 0x0600B5F9 RID: 46585 RVA: 0x0011A94F File Offset: 0x00118B4F
		protected override void OnCleanUp()
		{
			Components.FoodRehydrators.Remove(base.gameObject);
			base.OnCleanUp();
		}

		// Token: 0x0600B5FA RID: 46586 RVA: 0x0011A967 File Offset: 0x00118B67
		public void Reserve(GameObject reserver)
		{
			this.reserver = reserver;
			global::Debug.Assert(reserver != null && reserver.GetComponent<MinionResume>() != null);
		}

		// Token: 0x0600B5FB RID: 46587 RVA: 0x0011A98D File Offset: 0x00118B8D
		public void Unreserve()
		{
			this.activeWorkable = null;
			this.reserver = null;
		}

		// Token: 0x0600B5FC RID: 46588 RVA: 0x00455768 File Offset: 0x00453968
		public void SetActiveWorkable(Workable work)
		{
			DebugUtil.DevAssert(this.activeWorkable == null || work == null, "FoodRehydrator::AccessabilityManager activating a second workable", null);
			this.activeWorkable = work;
			this.operational.SetActive(this.activeWorkable != null, false);
		}

		// Token: 0x0600B5FD RID: 46589 RVA: 0x0011A99D File Offset: 0x00118B9D
		public bool CanAccess(GameObject worker)
		{
			return this.operational.IsOperational && (this.reserver == null || this.reserver == worker);
		}

		// Token: 0x0600B5FE RID: 46590 RVA: 0x0011A9CA File Offset: 0x00118BCA
		protected void ActiveChangedHandler(object obj)
		{
			if (!this.operational.IsActive)
			{
				this.CancelActiveWorkable();
			}
		}

		// Token: 0x0600B5FF RID: 46591 RVA: 0x0011A9DF File Offset: 0x00118BDF
		public void CancelActiveWorkable()
		{
			if (this.activeWorkable != null)
			{
				this.activeWorkable.StopWork(this.activeWorkable.worker, true);
			}
		}

		// Token: 0x04009006 RID: 36870
		[MyCmpReq]
		private Operational operational;

		// Token: 0x04009007 RID: 36871
		private GameObject reserver;

		// Token: 0x04009008 RID: 36872
		private Workable activeWorkable;
	}
}
