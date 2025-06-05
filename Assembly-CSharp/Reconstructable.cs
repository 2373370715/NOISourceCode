using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000B07 RID: 2823
public class Reconstructable : KMonoBehaviour
{
	// Token: 0x17000237 RID: 567
	// (get) Token: 0x0600344B RID: 13387 RVA: 0x000C691C File Offset: 0x000C4B1C
	public bool AllowReconstruct
	{
		get
		{
			return this.deconstructable.allowDeconstruction && (this.building.Def.ShowInBuildMenu || SelectModuleSideScreen.moduleButtonSortOrder.Contains(this.building.Def.PrefabID));
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x0600344C RID: 13388 RVA: 0x000C695B File Offset: 0x000C4B5B
	public Tag PrimarySelectedElementTag
	{
		get
		{
			return this.selectedElementsTags[0];
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x0600344D RID: 13389 RVA: 0x000C6969 File Offset: 0x000C4B69
	public bool ReconstructRequested
	{
		get
		{
			return this.reconstructRequested;
		}
	}

	// Token: 0x0600344E RID: 13390 RVA: 0x000C474E File Offset: 0x000C294E
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600344F RID: 13391 RVA: 0x00216EB8 File Offset: 0x002150B8
	public void RequestReconstruct(Tag newElement)
	{
		if (!this.deconstructable.allowDeconstruction)
		{
			return;
		}
		this.reconstructRequested = !this.reconstructRequested;
		if (this.reconstructRequested)
		{
			this.deconstructable.QueueDeconstruction(false);
			this.selectedElementsTags = new Tag[]
			{
				newElement
			};
		}
		else
		{
			this.deconstructable.CancelDeconstruction();
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06003450 RID: 13392 RVA: 0x000C6971 File Offset: 0x000C4B71
	public void CancelReconstructOrder()
	{
		this.reconstructRequested = false;
		this.deconstructable.CancelDeconstruction();
		base.Trigger(954267658, null);
	}

	// Token: 0x06003451 RID: 13393 RVA: 0x00216F2C File Offset: 0x0021512C
	public void TryCommenceReconstruct()
	{
		if (!this.deconstructable.allowDeconstruction)
		{
			return;
		}
		if (!this.reconstructRequested)
		{
			return;
		}
		string facadeID = this.building.GetComponent<BuildingFacade>().CurrentFacade;
		Vector3 position = this.building.transform.position;
		Orientation orientation = this.building.Orientation;
		GameScheduler.Instance.ScheduleNextFrame("Reconstruct", delegate(object data)
		{
			this.building.Def.TryPlace(null, position, orientation, this.selectedElementsTags, facadeID, false, 0);
		}, null, null);
	}

	// Token: 0x040023D0 RID: 9168
	[MyCmpReq]
	private Deconstructable deconstructable;

	// Token: 0x040023D1 RID: 9169
	[MyCmpReq]
	private Building building;

	// Token: 0x040023D2 RID: 9170
	[Serialize]
	private Tag[] selectedElementsTags;

	// Token: 0x040023D3 RID: 9171
	[Serialize]
	private bool reconstructRequested;
}
