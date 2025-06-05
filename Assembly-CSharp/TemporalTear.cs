using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x0200199F RID: 6559
[SerializationConfig(MemberSerialization.OptIn)]
public class TemporalTear : ClusterGridEntity
{
	// Token: 0x17000901 RID: 2305
	// (get) Token: 0x060088C2 RID: 35010 RVA: 0x000FDFF2 File Offset: 0x000FC1F2
	public override string Name
	{
		get
		{
			return Db.Get().SpaceDestinationTypes.Wormhole.typeName;
		}
	}

	// Token: 0x17000902 RID: 2306
	// (get) Token: 0x060088C3 RID: 35011 RVA: 0x000AA7FE File Offset: 0x000A89FE
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x17000903 RID: 2307
	// (get) Token: 0x060088C4 RID: 35012 RVA: 0x003649F8 File Offset: 0x00362BF8
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("temporal_tear_kanim"),
					initialAnim = "closed_loop"
				}
			};
		}
	}

	// Token: 0x17000904 RID: 2308
	// (get) Token: 0x060088C5 RID: 35013 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000905 RID: 2309
	// (get) Token: 0x060088C6 RID: 35014 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x060088C7 RID: 35015 RVA: 0x000FE008 File Offset: 0x000FC208
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ClusterManager.Instance.GetComponent<ClusterPOIManager>().RegisterTemporalTear(this);
		this.UpdateStatus();
	}

	// Token: 0x060088C8 RID: 35016 RVA: 0x00364A3C File Offset: 0x00362C3C
	public void UpdateStatus()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		ClusterMapVisualizer clusterMapVisualizer = null;
		if (ClusterMapScreen.Instance != null)
		{
			clusterMapVisualizer = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		}
		if (this.IsOpen())
		{
			if (clusterMapVisualizer != null)
			{
				clusterMapVisualizer.PlayAnim("open_loop", KAnim.PlayMode.Loop);
			}
			component.RemoveStatusItem(Db.Get().MiscStatusItems.TearClosed, false);
			component.AddStatusItem(Db.Get().MiscStatusItems.TearOpen, null);
			return;
		}
		if (clusterMapVisualizer != null)
		{
			clusterMapVisualizer.PlayAnim("closed_loop", KAnim.PlayMode.Loop);
		}
		component.RemoveStatusItem(Db.Get().MiscStatusItems.TearOpen, false);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.TearClosed, null);
	}

	// Token: 0x060088C9 RID: 35017 RVA: 0x000FE026 File Offset: 0x000FC226
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060088CA RID: 35018 RVA: 0x00364B00 File Offset: 0x00362D00
	public void ConsumeCraft(Clustercraft craft)
	{
		if (this.m_open && craft.Location == base.Location && !craft.IsFlightInProgress())
		{
			for (int i = 0; i < Components.MinionIdentities.Count; i++)
			{
				MinionIdentity minionIdentity = Components.MinionIdentities[i];
				if (minionIdentity.GetMyWorldId() == craft.ModuleInterface.GetInteriorWorld().id)
				{
					Util.KDestroyGameObject(minionIdentity.gameObject);
				}
			}
			craft.DestroyCraftAndModules();
			this.m_hasConsumedCraft = true;
		}
	}

	// Token: 0x060088CB RID: 35019 RVA: 0x000FE02E File Offset: 0x000FC22E
	public void Open()
	{
		this.m_open = true;
		this.UpdateStatus();
	}

	// Token: 0x060088CC RID: 35020 RVA: 0x000FE03D File Offset: 0x000FC23D
	public bool IsOpen()
	{
		return this.m_open;
	}

	// Token: 0x060088CD RID: 35021 RVA: 0x000FE045 File Offset: 0x000FC245
	public bool HasConsumedCraft()
	{
		return this.m_hasConsumedCraft;
	}

	// Token: 0x04006789 RID: 26505
	[Serialize]
	private bool m_open;

	// Token: 0x0400678A RID: 26506
	[Serialize]
	private bool m_hasConsumedCraft;
}
