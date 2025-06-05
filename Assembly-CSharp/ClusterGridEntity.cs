using System;
using System.Collections.Generic;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x020019B0 RID: 6576
public abstract class ClusterGridEntity : KMonoBehaviour
{
	// Token: 0x1700090C RID: 2316
	// (get) Token: 0x0600892C RID: 35116
	public abstract string Name { get; }

	// Token: 0x1700090D RID: 2317
	// (get) Token: 0x0600892D RID: 35117
	public abstract EntityLayer Layer { get; }

	// Token: 0x1700090E RID: 2318
	// (get) Token: 0x0600892E RID: 35118
	public abstract List<ClusterGridEntity.AnimConfig> AnimConfigs { get; }

	// Token: 0x1700090F RID: 2319
	// (get) Token: 0x0600892F RID: 35119
	public abstract bool IsVisible { get; }

	// Token: 0x06008930 RID: 35120 RVA: 0x000B1628 File Offset: 0x000AF828
	public virtual bool ShowName()
	{
		return false;
	}

	// Token: 0x06008931 RID: 35121 RVA: 0x000B1628 File Offset: 0x000AF828
	public virtual bool ShowProgressBar()
	{
		return false;
	}

	// Token: 0x06008932 RID: 35122 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public virtual float GetProgress()
	{
		return 0f;
	}

	// Token: 0x06008933 RID: 35123 RVA: 0x000B1628 File Offset: 0x000AF828
	public virtual bool SpaceOutInSameHex()
	{
		return false;
	}

	// Token: 0x06008934 RID: 35124 RVA: 0x000B1628 File Offset: 0x000AF828
	public virtual bool KeepRotationWhenSpacingOutInHex()
	{
		return false;
	}

	// Token: 0x06008935 RID: 35125 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public virtual bool ShowPath()
	{
		return true;
	}

	// Token: 0x06008936 RID: 35126 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
	}

	// Token: 0x17000910 RID: 2320
	// (get) Token: 0x06008937 RID: 35127
	public abstract ClusterRevealLevel IsVisibleInFOW { get; }

	// Token: 0x17000911 RID: 2321
	// (get) Token: 0x06008938 RID: 35128 RVA: 0x000FE331 File Offset: 0x000FC531
	// (set) Token: 0x06008939 RID: 35129 RVA: 0x0036611C File Offset: 0x0036431C
	public AxialI Location
	{
		get
		{
			return this.m_location;
		}
		set
		{
			if (value != this.m_location)
			{
				AxialI location = this.m_location;
				this.m_location = value;
				if (base.gameObject.GetSMI<StateMachine.Instance>() == null)
				{
					this.positionDirty = true;
				}
				this.SendClusterLocationChangedEvent(location, this.m_location);
			}
		}
	}

	// Token: 0x0600893A RID: 35130 RVA: 0x00366168 File Offset: 0x00364368
	protected override void OnSpawn()
	{
		ClusterGrid.Instance.RegisterEntity(this);
		if (this.m_selectable != null)
		{
			this.m_selectable.SetName(this.Name);
		}
		if (!this.isWorldEntity)
		{
			this.m_transform.SetLocalPosition(new Vector3(-1f, 0f, 0f));
		}
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapScreen.Instance.Trigger(1980521255, null);
		}
	}

	// Token: 0x0600893B RID: 35131 RVA: 0x000FE339 File Offset: 0x000FC539
	protected override void OnCleanUp()
	{
		ClusterGrid.Instance.UnregisterEntity(this);
	}

	// Token: 0x0600893C RID: 35132 RVA: 0x003661E4 File Offset: 0x003643E4
	public virtual Sprite GetUISprite()
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			List<ClusterGridEntity.AnimConfig> animConfigs = this.AnimConfigs;
			if (animConfigs.Count > 0)
			{
				return Def.GetUISpriteFromMultiObjectAnim(animConfigs[0].animFile, "ui", false, "");
			}
		}
		else
		{
			WorldContainer component = base.GetComponent<WorldContainer>();
			if (component != null)
			{
				ProcGen.World worldData = SettingsCache.worlds.GetWorldData(component.worldName);
				if (worldData == null)
				{
					return null;
				}
				return Assets.GetSprite(worldData.asteroidIcon);
			}
		}
		return null;
	}

	// Token: 0x0600893D RID: 35133 RVA: 0x00366260 File Offset: 0x00364460
	public void SendClusterLocationChangedEvent(AxialI oldLocation, AxialI newLocation)
	{
		ClusterLocationChangedEvent data = new ClusterLocationChangedEvent
		{
			entity = this,
			oldLocation = oldLocation,
			newLocation = newLocation
		};
		base.Trigger(-1298331547, data);
		Game.Instance.Trigger(-1298331547, data);
		if (this.m_selectable != null && this.m_selectable.IsSelected)
		{
			DetailsScreen.Instance.Refresh(base.gameObject);
		}
	}

	// Token: 0x040067BF RID: 26559
	[Serialize]
	protected AxialI m_location;

	// Token: 0x040067C0 RID: 26560
	public bool positionDirty;

	// Token: 0x040067C1 RID: 26561
	[MyCmpGet]
	protected KSelectable m_selectable;

	// Token: 0x040067C2 RID: 26562
	[MyCmpReq]
	private Transform m_transform;

	// Token: 0x040067C3 RID: 26563
	public bool isWorldEntity;

	// Token: 0x020019B1 RID: 6577
	public struct AnimConfig
	{
		// Token: 0x040067C4 RID: 26564
		public KAnimFile animFile;

		// Token: 0x040067C5 RID: 26565
		public string initialAnim;

		// Token: 0x040067C6 RID: 26566
		public KAnim.PlayMode playMode;

		// Token: 0x040067C7 RID: 26567
		public string symbolSwapTarget;

		// Token: 0x040067C8 RID: 26568
		public string symbolSwapSymbol;

		// Token: 0x040067C9 RID: 26569
		public Vector3 animOffset;

		// Token: 0x040067CA RID: 26570
		public float animPlaySpeedModifier;
	}
}
