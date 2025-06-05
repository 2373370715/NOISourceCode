using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000FC3 RID: 4035
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduit")]
public class SolidConduit : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr
{
	// Token: 0x06005143 RID: 20803 RVA: 0x000D968C File Offset: 0x000D788C
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06005144 RID: 20804 RVA: 0x000D96A2 File Offset: 0x000D78A2
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06005145 RID: 20805 RVA: 0x000D96B1 File Offset: 0x000D78B1
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.solidConduitSystem;
	}

	// Token: 0x06005146 RID: 20806 RVA: 0x000D96BD File Offset: 0x000D78BD
	public UtilityNetwork GetNetwork()
	{
		return this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
	}

	// Token: 0x06005147 RID: 20807 RVA: 0x000D96D0 File Offset: 0x000D78D0
	public static SolidConduitFlow GetFlowManager()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x06005148 RID: 20808 RVA: 0x000C656E File Offset: 0x000C476E
	public Vector3 Position
	{
		get
		{
			return base.transform.GetPosition();
		}
	}

	// Token: 0x06005149 RID: 20809 RVA: 0x000D96DC File Offset: 0x000D78DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Conveyor, this);
	}

	// Token: 0x0600514A RID: 20810 RVA: 0x0027FB78 File Offset: 0x0027DD78
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(this);
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			this.GetNetworkManager().RemoveFromNetworks(cell, this, false);
			SolidConduit.GetFlowManager().EmptyConduit(cell);
		}
		base.OnCleanUp();
	}

	// Token: 0x04003935 RID: 14645
	[MyCmpReq]
	private KAnimGraphTileVisualizer graphTileDependency;

	// Token: 0x04003936 RID: 14646
	private System.Action firstFrameCallback;
}
