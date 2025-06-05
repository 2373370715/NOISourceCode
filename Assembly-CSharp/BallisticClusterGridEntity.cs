using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200043A RID: 1082
public class BallisticClusterGridEntity : ClusterGridEntity
{
	// Token: 0x17000067 RID: 103
	// (get) Token: 0x06001214 RID: 4628 RVA: 0x000B2601 File Offset: 0x000B0801
	public override string Name
	{
		get
		{
			return Strings.Get(this.nameKey);
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06001215 RID: 4629 RVA: 0x000B16D2 File Offset: 0x000AF8D2
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Payload;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06001216 RID: 4630 RVA: 0x001920EC File Offset: 0x001902EC
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim(this.clusterAnimName),
					initialAnim = "idle_loop",
					symbolSwapTarget = this.clusterAnimSymbolSwapTarget,
					symbolSwapSymbol = this.clusterAnimSymbolSwapSymbol
				}
			};
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06001217 RID: 4631 RVA: 0x000B2613 File Offset: 0x000B0813
	public override bool IsVisible
	{
		get
		{
			return !base.gameObject.HasTag(GameTags.ClusterEntityGrounded);
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06001218 RID: 4632 RVA: 0x000AA7FE File Offset: 0x000A89FE
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x06001219 RID: 4633 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool SpaceOutInSameHex()
	{
		return true;
	}

	// Token: 0x0600121A RID: 4634 RVA: 0x0019214C File Offset: 0x0019034C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.m_clusterTraveler.getSpeedCB = new Func<float>(this.GetSpeed);
		this.m_clusterTraveler.getCanTravelCB = new Func<bool, bool>(this.CanTravel);
		this.m_clusterTraveler.onTravelCB = null;
	}

	// Token: 0x0600121B RID: 4635 RVA: 0x000B2628 File Offset: 0x000B0828
	private float GetSpeed()
	{
		return 10f;
	}

	// Token: 0x0600121C RID: 4636 RVA: 0x000B262F File Offset: 0x000B082F
	private bool CanTravel(bool tryingToLand)
	{
		return this.HasTag(GameTags.EntityInSpace);
	}

	// Token: 0x0600121D RID: 4637 RVA: 0x000B263C File Offset: 0x000B083C
	public void Configure(AxialI source, AxialI destination)
	{
		this.m_location = source;
		this.m_destionationSelector.SetDestination(destination);
	}

	// Token: 0x0600121E RID: 4638 RVA: 0x000B2651 File Offset: 0x000B0851
	public override bool ShowPath()
	{
		return this.m_selectable.IsSelected;
	}

	// Token: 0x0600121F RID: 4639 RVA: 0x000B265E File Offset: 0x000B085E
	public override bool ShowProgressBar()
	{
		return this.m_selectable.IsSelected && this.m_clusterTraveler.IsTraveling();
	}

	// Token: 0x06001220 RID: 4640 RVA: 0x000B267A File Offset: 0x000B087A
	public override float GetProgress()
	{
		return this.m_clusterTraveler.GetMoveProgress();
	}

	// Token: 0x06001221 RID: 4641 RVA: 0x000B2687 File Offset: 0x000B0887
	public void SwapSymbolFromSameAnim(string targetSymbolName, string swappedSymbolName)
	{
		this.clusterAnimSymbolSwapTarget = targetSymbolName;
		this.clusterAnimSymbolSwapSymbol = swappedSymbolName;
	}

	// Token: 0x04000C9D RID: 3229
	[MyCmpReq]
	private ClusterDestinationSelector m_destionationSelector;

	// Token: 0x04000C9E RID: 3230
	[MyCmpReq]
	private ClusterTraveler m_clusterTraveler;

	// Token: 0x04000C9F RID: 3231
	[SerializeField]
	public string clusterAnimName;

	// Token: 0x04000CA0 RID: 3232
	[SerializeField]
	public StringKey nameKey;

	// Token: 0x04000CA1 RID: 3233
	private string clusterAnimSymbolSwapTarget;

	// Token: 0x04000CA2 RID: 3234
	private string clusterAnimSymbolSwapSymbol;
}
