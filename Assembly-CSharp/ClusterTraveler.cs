using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001924 RID: 6436
public class ClusterTraveler : KMonoBehaviour, ISim200ms
{
	// Token: 0x17000897 RID: 2199
	// (get) Token: 0x06008562 RID: 34146 RVA: 0x0035544C File Offset: 0x0035364C
	public List<AxialI> CurrentPath
	{
		get
		{
			if (this.m_cachedPath == null || this.m_destinationSelector.GetDestination() != this.m_cachedPathDestination)
			{
				this.m_cachedPathDestination = this.m_destinationSelector.GetDestination();
				this.m_cachedPath = ClusterGrid.Instance.GetPath(this.m_clusterGridEntity.Location, this.m_cachedPathDestination, this.m_destinationSelector);
			}
			return this.m_cachedPath;
		}
	}

	// Token: 0x06008563 RID: 34147 RVA: 0x000FC1D4 File Offset: 0x000FA3D4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.ClusterTravelers.Add(this);
	}

	// Token: 0x06008564 RID: 34148 RVA: 0x000FC1E7 File Offset: 0x000FA3E7
	protected override void OnCleanUp()
	{
		Components.ClusterTravelers.Remove(this);
		Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnClusterFogOfWarRevealed));
		base.OnCleanUp();
	}

	// Token: 0x06008565 RID: 34149 RVA: 0x000FC215 File Offset: 0x000FA415
	private void ForceRevealLocation(AxialI location)
	{
		if (!ClusterGrid.Instance.IsCellVisible(location))
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(location, 0);
		}
	}

	// Token: 0x06008566 RID: 34150 RVA: 0x003554B8 File Offset: 0x003536B8
	protected override void OnSpawn()
	{
		base.Subscribe<ClusterTraveler>(543433792, ClusterTraveler.ClusterDestinationChangedHandler);
		Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnClusterFogOfWarRevealed));
		this.UpdateAnimationTags();
		this.MarkPathDirty();
		this.RevalidatePath(false);
		if (this.revealsFogOfWarAsItTravels)
		{
			this.ForceRevealLocation(this.m_clusterGridEntity.Location);
		}
	}

	// Token: 0x06008567 RID: 34151 RVA: 0x000FC235 File Offset: 0x000FA435
	private void MarkPathDirty()
	{
		this.m_isPathDirty = true;
	}

	// Token: 0x06008568 RID: 34152 RVA: 0x000FC23E File Offset: 0x000FA43E
	private void OnClusterFogOfWarRevealed(object data)
	{
		this.MarkPathDirty();
	}

	// Token: 0x06008569 RID: 34153 RVA: 0x000FC246 File Offset: 0x000FA446
	private void OnClusterDestinationChanged(object data)
	{
		if (this.m_destinationSelector.IsAtDestination())
		{
			this.m_movePotential = 0f;
			if (this.CurrentPath != null)
			{
				this.CurrentPath.Clear();
			}
		}
		this.MarkPathDirty();
	}

	// Token: 0x0600856A RID: 34154 RVA: 0x000FC279 File Offset: 0x000FA479
	public int GetDestinationWorldID()
	{
		return this.m_destinationSelector.GetDestinationWorld();
	}

	// Token: 0x0600856B RID: 34155 RVA: 0x000FC286 File Offset: 0x000FA486
	public float TravelETA()
	{
		if (!this.IsTraveling() || this.getSpeedCB == null)
		{
			return 0f;
		}
		return this.RemainingTravelDistance() / this.getSpeedCB();
	}

	// Token: 0x0600856C RID: 34156 RVA: 0x00355520 File Offset: 0x00353720
	public float RemainingTravelDistance()
	{
		int num = this.RemainingTravelNodes();
		if (this.GetDestinationWorldID() >= 0)
		{
			num--;
			num = Mathf.Max(num, 0);
		}
		return (float)num * 600f - this.m_movePotential;
	}

	// Token: 0x0600856D RID: 34157 RVA: 0x00355558 File Offset: 0x00353758
	public int RemainingTravelNodes()
	{
		if (this.CurrentPath == null)
		{
			return 0;
		}
		int count = this.CurrentPath.Count;
		return Mathf.Max(0, count);
	}

	// Token: 0x0600856E RID: 34158 RVA: 0x000FC2B0 File Offset: 0x000FA4B0
	public float GetMoveProgress()
	{
		return this.m_movePotential / 600f;
	}

	// Token: 0x0600856F RID: 34159 RVA: 0x000FC2BE File Offset: 0x000FA4BE
	public bool IsTraveling()
	{
		return !this.m_destinationSelector.IsAtDestination();
	}

	// Token: 0x06008570 RID: 34160 RVA: 0x00355584 File Offset: 0x00353784
	public void Sim200ms(float dt)
	{
		if (!this.IsTraveling())
		{
			return;
		}
		bool flag = this.CurrentPath != null && this.CurrentPath.Count > 0;
		bool flag2 = this.m_destinationSelector.HasAsteroidDestination();
		bool arg = flag2 && flag && this.CurrentPath.Count == 1;
		if (this.getCanTravelCB != null && !this.getCanTravelCB(arg))
		{
			return;
		}
		AxialI location = this.m_clusterGridEntity.Location;
		if (flag)
		{
			if (flag2)
			{
				bool requireLaunchPadOnAsteroidDestination = this.m_destinationSelector.requireLaunchPadOnAsteroidDestination;
			}
			if (!flag2 || this.CurrentPath.Count > 1 || !this.quickTravelToAsteroidIfInOrbit)
			{
				float num = dt * this.getSpeedCB();
				this.m_movePotential += num;
				if (this.m_movePotential >= 600f)
				{
					this.m_movePotential = 600f;
					if (this.AdvancePathOneStep())
					{
						global::Debug.Assert(ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_clusterGridEntity.Location, EntityLayer.Asteroid) == null || (flag2 && this.CurrentPath.Count == 0), string.Format("Somehow this clustercraft pathed through an asteroid at {0}", this.m_clusterGridEntity.Location));
						this.m_movePotential -= 600f;
						if (this.onTravelCB != null)
						{
							this.onTravelCB();
						}
					}
				}
			}
			else
			{
				this.AdvancePathOneStep();
			}
		}
		this.RevalidatePath(true);
	}

	// Token: 0x06008571 RID: 34161 RVA: 0x003556FC File Offset: 0x003538FC
	public bool AdvancePathOneStep()
	{
		if (this.validateTravelCB != null && !this.validateTravelCB(this.CurrentPath[0]))
		{
			return false;
		}
		AxialI location = this.CurrentPath[0];
		this.CurrentPath.RemoveAt(0);
		if (this.revealsFogOfWarAsItTravels)
		{
			this.ForceRevealLocation(location);
		}
		this.m_clusterGridEntity.Location = location;
		this.UpdateAnimationTags();
		return true;
	}

	// Token: 0x06008572 RID: 34162 RVA: 0x00355768 File Offset: 0x00353968
	private void UpdateAnimationTags()
	{
		if (this.CurrentPath == null)
		{
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLaunching);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLanding);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityMoving);
			return;
		}
		if (!(ClusterGrid.Instance.GetAsteroidAtCell(this.m_clusterGridEntity.Location) != null))
		{
			this.m_clusterGridEntity.AddTag(GameTags.BallisticEntityMoving);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLanding);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLaunching);
			return;
		}
		if (this.CurrentPath.Count == 0 || this.m_clusterGridEntity.Location == this.CurrentPath[this.CurrentPath.Count - 1])
		{
			this.m_clusterGridEntity.AddTag(GameTags.BallisticEntityLanding);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLaunching);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityMoving);
			return;
		}
		this.m_clusterGridEntity.AddTag(GameTags.BallisticEntityLaunching);
		this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLanding);
		this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityMoving);
	}

	// Token: 0x06008573 RID: 34163 RVA: 0x00355898 File Offset: 0x00353A98
	public void RevalidatePath(bool react_to_change = true)
	{
		string reason;
		List<AxialI> cachedPath;
		if (this.HasCurrentPathChanged(out reason, out cachedPath))
		{
			if (this.stopAndNotifyWhenPathChanges && react_to_change)
			{
				this.m_destinationSelector.SetDestination(this.m_destinationSelector.GetMyWorldLocation());
				string message = MISC.NOTIFICATIONS.BADROCKETPATH.TOOLTIP;
				Notification notification = new Notification(MISC.NOTIFICATIONS.BADROCKETPATH.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => message + notificationList.ReduceMessages(false) + "\n\n" + reason, null, true, 0f, null, null, null, true, false, false);
				base.GetComponent<Notifier>().Add(notification, "");
				return;
			}
			this.m_cachedPath = cachedPath;
		}
	}

	// Token: 0x06008574 RID: 34164 RVA: 0x00355930 File Offset: 0x00353B30
	private bool HasCurrentPathChanged(out string reason, out List<AxialI> updatedPath)
	{
		if (!this.m_isPathDirty)
		{
			reason = null;
			updatedPath = null;
			return false;
		}
		this.m_isPathDirty = false;
		updatedPath = ClusterGrid.Instance.GetPath(this.m_clusterGridEntity.Location, this.m_cachedPathDestination, this.m_destinationSelector, out reason, this.m_destinationSelector.dodgesHiddenAsteroids);
		if (updatedPath == null)
		{
			return true;
		}
		if (updatedPath.Count != this.m_cachedPath.Count)
		{
			return true;
		}
		for (int i = 0; i < this.m_cachedPath.Count; i++)
		{
			if (this.m_cachedPath[i] != updatedPath[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06008575 RID: 34165 RVA: 0x000FC2CE File Offset: 0x000FA4CE
	[ContextMenu("Fill Move Potential")]
	public void FillMovePotential()
	{
		this.m_movePotential = 600f;
	}

	// Token: 0x0400657C RID: 25980
	[MyCmpReq]
	private ClusterDestinationSelector m_destinationSelector;

	// Token: 0x0400657D RID: 25981
	[MyCmpReq]
	private ClusterGridEntity m_clusterGridEntity;

	// Token: 0x0400657E RID: 25982
	[Serialize]
	private float m_movePotential;

	// Token: 0x0400657F RID: 25983
	public Func<float> getSpeedCB;

	// Token: 0x04006580 RID: 25984
	public Func<bool, bool> getCanTravelCB;

	// Token: 0x04006581 RID: 25985
	public Func<AxialI, bool> validateTravelCB;

	// Token: 0x04006582 RID: 25986
	public System.Action onTravelCB;

	// Token: 0x04006583 RID: 25987
	private AxialI m_cachedPathDestination;

	// Token: 0x04006584 RID: 25988
	private List<AxialI> m_cachedPath;

	// Token: 0x04006585 RID: 25989
	private bool m_isPathDirty;

	// Token: 0x04006586 RID: 25990
	public bool revealsFogOfWarAsItTravels = true;

	// Token: 0x04006587 RID: 25991
	public bool quickTravelToAsteroidIfInOrbit = true;

	// Token: 0x04006588 RID: 25992
	public bool stopAndNotifyWhenPathChanges;

	// Token: 0x04006589 RID: 25993
	private static EventSystem.IntraObjectHandler<ClusterTraveler> ClusterDestinationChangedHandler = new EventSystem.IntraObjectHandler<ClusterTraveler>(delegate(ClusterTraveler cmp, object data)
	{
		cmp.OnClusterDestinationChanged(data);
	});
}
