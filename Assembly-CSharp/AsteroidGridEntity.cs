using System;
using System.Collections;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;

// Token: 0x020019A6 RID: 6566
public class AsteroidGridEntity : ClusterGridEntity
{
	// Token: 0x060088E7 RID: 35047 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowName()
	{
		return true;
	}

	// Token: 0x17000907 RID: 2311
	// (get) Token: 0x060088E8 RID: 35048 RVA: 0x000FE106 File Offset: 0x000FC306
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x17000908 RID: 2312
	// (get) Token: 0x060088E9 RID: 35049 RVA: 0x000B1628 File Offset: 0x000AF828
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Asteroid;
		}
	}

	// Token: 0x17000909 RID: 2313
	// (get) Token: 0x060088EA RID: 35050 RVA: 0x00364FFC File Offset: 0x003631FC
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			List<ClusterGridEntity.AnimConfig> list = new List<ClusterGridEntity.AnimConfig>();
			ClusterGridEntity.AnimConfig item = new ClusterGridEntity.AnimConfig
			{
				animFile = Assets.GetAnim(this.m_asteroidAnim.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : this.m_asteroidAnim),
				initialAnim = "idle_loop"
			};
			list.Add(item);
			item = new ClusterGridEntity.AnimConfig
			{
				animFile = Assets.GetAnim("orbit_kanim"),
				initialAnim = "orbit"
			};
			list.Add(item);
			item = new ClusterGridEntity.AnimConfig
			{
				animFile = Assets.GetAnim("shower_asteroid_current_kanim"),
				initialAnim = "off",
				playMode = KAnim.PlayMode.Once
			};
			list.Add(item);
			return list;
		}
	}

	// Token: 0x1700090A RID: 2314
	// (get) Token: 0x060088EB RID: 35051 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700090B RID: 2315
	// (get) Token: 0x060088EC RID: 35052 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x060088ED RID: 35053 RVA: 0x000FE10E File Offset: 0x000FC30E
	public void Init(string name, AxialI location, string asteroidTypeId)
	{
		this.m_name = name;
		this.m_location = location;
		this.m_asteroidAnim = asteroidTypeId;
	}

	// Token: 0x060088EE RID: 35054 RVA: 0x003650C0 File Offset: 0x003632C0
	protected override void OnSpawn()
	{
		KAnimFile kanimFile;
		if (!Assets.TryGetAnim(this.m_asteroidAnim, out kanimFile))
		{
			this.m_asteroidAnim = AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM;
		}
		Game.Instance.Subscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
		Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
		Game.Instance.Subscribe(78366336, new Action<object>(this.OnMeteorShowerEventChanged));
		Game.Instance.Subscribe(1749562766, new Action<object>(this.OnMeteorShowerEventChanged));
		if (ClusterGrid.Instance.IsCellVisible(this.m_location))
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(this.m_location, 1);
		}
		base.OnSpawn();
	}

	// Token: 0x060088EF RID: 35055 RVA: 0x0036518C File Offset: 0x0036338C
	protected override void OnCleanUp()
	{
		Game.Instance.Unsubscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
		Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
		Game.Instance.Unsubscribe(78366336, new Action<object>(this.OnMeteorShowerEventChanged));
		Game.Instance.Unsubscribe(1749562766, new Action<object>(this.OnMeteorShowerEventChanged));
		base.OnCleanUp();
	}

	// Token: 0x060088F0 RID: 35056 RVA: 0x0036520C File Offset: 0x0036340C
	public void OnClusterLocationChanged(object data)
	{
		if (this.m_worldContainer.IsDiscovered)
		{
			return;
		}
		if (!ClusterGrid.Instance.IsCellVisible(base.Location))
		{
			return;
		}
		Clustercraft component = ((ClusterLocationChangedEvent)data).entity.GetComponent<Clustercraft>();
		if (component == null)
		{
			return;
		}
		if (component.GetOrbitAsteroid() == this)
		{
			this.m_worldContainer.SetDiscovered(true);
		}
	}

	// Token: 0x060088F1 RID: 35057 RVA: 0x000FE125 File Offset: 0x000FC325
	public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
		base.OnClusterMapIconShown(levelUsed);
		if (levelUsed == ClusterRevealLevel.Visible)
		{
			this.RefreshMeteorShowerEffect();
		}
	}

	// Token: 0x060088F2 RID: 35058 RVA: 0x000FE138 File Offset: 0x000FC338
	private void OnMeteorShowerEventChanged(object _worldID)
	{
		if ((int)_worldID == this.m_worldContainer.id)
		{
			this.RefreshMeteorShowerEffect();
		}
	}

	// Token: 0x060088F3 RID: 35059 RVA: 0x00365270 File Offset: 0x00363470
	public void RefreshMeteorShowerEffect()
	{
		if (ClusterMapScreen.Instance == null)
		{
			return;
		}
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		if (entityVisAnim == null)
		{
			return;
		}
		KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
		if (animController != null)
		{
			List<GameplayEventInstance> list = new List<GameplayEventInstance>();
			GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(this.m_worldContainer.id, ref list);
			bool flag = false;
			string s = "off";
			foreach (GameplayEventInstance gameplayEventInstance in list)
			{
				if (gameplayEventInstance != null && gameplayEventInstance.smi is MeteorShowerEvent.StatesInstance)
				{
					MeteorShowerEvent.StatesInstance statesInstance = gameplayEventInstance.smi as MeteorShowerEvent.StatesInstance;
					if (statesInstance.IsInsideState(statesInstance.sm.running.bombarding))
					{
						flag = true;
						s = "idle_loop";
						break;
					}
				}
			}
			animController.Play(s, flag ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x060088F4 RID: 35060 RVA: 0x00365378 File Offset: 0x00363578
	public void OnFogOfWarRevealed(object data = null)
	{
		if (data == null)
		{
			return;
		}
		if ((AxialI)data != this.m_location)
		{
			return;
		}
		if (!ClusterGrid.Instance.IsCellVisible(base.Location))
		{
			return;
		}
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			WorldDetectedMessage message = new WorldDetectedMessage(this.m_worldContainer);
			MusicManager.instance.PlaySong("Stinger_WorldDetected", false);
			Messenger.Instance.QueueMessage(message);
			if (!this.m_worldContainer.IsDiscovered)
			{
				using (IEnumerator enumerator = Components.Clustercrafts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((Clustercraft)enumerator.Current).GetOrbitAsteroid() == this)
						{
							this.m_worldContainer.SetDiscovered(true);
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x0400679A RID: 26522
	public static string DEFAULT_ASTEROID_ICON_ANIM = "asteroid_sandstone_start_kanim";

	// Token: 0x0400679B RID: 26523
	[MyCmpReq]
	private WorldContainer m_worldContainer;

	// Token: 0x0400679C RID: 26524
	[Serialize]
	private string m_name;

	// Token: 0x0400679D RID: 26525
	[Serialize]
	private string m_asteroidAnim;
}
