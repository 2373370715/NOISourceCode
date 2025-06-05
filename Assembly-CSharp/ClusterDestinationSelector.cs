using System;
using KSerialization;

// Token: 0x0200191A RID: 6426
public class ClusterDestinationSelector : KMonoBehaviour
{
	// Token: 0x06008515 RID: 34069 RVA: 0x000FBEA9 File Offset: 0x000FA0A9
	protected override void OnPrefabInit()
	{
		base.Subscribe<ClusterDestinationSelector>(-1298331547, this.OnClusterLocationChangedDelegate);
	}

	// Token: 0x06008516 RID: 34070 RVA: 0x000FBEBD File Offset: 0x000FA0BD
	protected virtual void OnClusterLocationChanged(object data)
	{
		if (((ClusterLocationChangedEvent)data).newLocation == this.m_destination)
		{
			base.Trigger(1796608350, data);
		}
	}

	// Token: 0x06008517 RID: 34071 RVA: 0x000FBEE3 File Offset: 0x000FA0E3
	public int GetDestinationWorld()
	{
		return ClusterUtil.GetAsteroidWorldIdAtLocation(this.m_destination);
	}

	// Token: 0x06008518 RID: 34072 RVA: 0x000FBEF0 File Offset: 0x000FA0F0
	public AxialI GetDestination()
	{
		return this.m_destination;
	}

	// Token: 0x06008519 RID: 34073 RVA: 0x0035476C File Offset: 0x0035296C
	public virtual void SetDestination(AxialI location)
	{
		if (this.requireAsteroidDestination)
		{
			Debug.Assert(ClusterUtil.GetAsteroidWorldIdAtLocation(location) != -1, string.Format("Cannot SetDestination to {0} as there is no world there", location));
		}
		this.m_destination = location;
		base.Trigger(543433792, location);
	}

	// Token: 0x0600851A RID: 34074 RVA: 0x000FBEF8 File Offset: 0x000FA0F8
	public bool HasAsteroidDestination()
	{
		return ClusterUtil.GetAsteroidWorldIdAtLocation(this.m_destination) != -1;
	}

	// Token: 0x0600851B RID: 34075 RVA: 0x000FBF0B File Offset: 0x000FA10B
	public virtual bool IsAtDestination()
	{
		return this.GetMyWorldLocation() == this.m_destination;
	}

	// Token: 0x04006550 RID: 25936
	[Serialize]
	protected AxialI m_destination;

	// Token: 0x04006551 RID: 25937
	public bool assignable;

	// Token: 0x04006552 RID: 25938
	public bool requireAsteroidDestination;

	// Token: 0x04006553 RID: 25939
	[Serialize]
	public bool canNavigateFogOfWar;

	// Token: 0x04006554 RID: 25940
	public bool dodgesHiddenAsteroids;

	// Token: 0x04006555 RID: 25941
	public bool requireLaunchPadOnAsteroidDestination;

	// Token: 0x04006556 RID: 25942
	public bool shouldPointTowardsPath;

	// Token: 0x04006557 RID: 25943
	private EventSystem.IntraObjectHandler<ClusterDestinationSelector> OnClusterLocationChangedDelegate = new EventSystem.IntraObjectHandler<ClusterDestinationSelector>(delegate(ClusterDestinationSelector cmp, object data)
	{
		cmp.OnClusterLocationChanged(data);
	});
}
