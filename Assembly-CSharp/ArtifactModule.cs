using System;
using UnityEngine;

// Token: 0x02000C59 RID: 3161
public class ArtifactModule : SingleEntityReceptacle, IRenderEveryTick
{
	// Token: 0x06003BB3 RID: 15283 RVA: 0x00239814 File Offset: 0x00237A14
	protected override void OnSpawn()
	{
		this.craft = this.module.CraftInterface.GetComponent<Clustercraft>();
		if (this.craft.Status == Clustercraft.CraftStatus.InFlight && base.occupyingObject != null)
		{
			base.occupyingObject.SetActive(false);
		}
		base.OnSpawn();
		base.Subscribe(705820818, new Action<object>(this.OnEnterSpace));
		base.Subscribe(-1165815793, new Action<object>(this.OnExitSpace));
	}

	// Token: 0x06003BB4 RID: 15284 RVA: 0x000CB010 File Offset: 0x000C9210
	public void RenderEveryTick(float dt)
	{
		this.ArtifactTrackModulePosition();
	}

	// Token: 0x06003BB5 RID: 15285 RVA: 0x00239898 File Offset: 0x00237A98
	private void ArtifactTrackModulePosition()
	{
		this.occupyingObjectRelativePosition = this.animController.Offset + Vector3.up * 0.5f + new Vector3(0f, 0f, -1f);
		if (base.occupyingObject != null)
		{
			this.PositionOccupyingObject();
		}
	}

	// Token: 0x06003BB6 RID: 15286 RVA: 0x000CB018 File Offset: 0x000C9218
	private void OnEnterSpace(object data)
	{
		if (base.occupyingObject != null)
		{
			base.occupyingObject.SetActive(false);
		}
	}

	// Token: 0x06003BB7 RID: 15287 RVA: 0x000CB034 File Offset: 0x000C9234
	private void OnExitSpace(object data)
	{
		if (base.occupyingObject != null)
		{
			base.occupyingObject.SetActive(true);
		}
	}

	// Token: 0x04002965 RID: 10597
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x04002966 RID: 10598
	[MyCmpReq]
	private RocketModuleCluster module;

	// Token: 0x04002967 RID: 10599
	private Clustercraft craft;
}
