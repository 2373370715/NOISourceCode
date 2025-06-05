using System;
using System.Collections.Generic;
using FMODUnity;
using KSerialization;
using UnityEngine;

// Token: 0x0200196D RID: 6509
public class PassengerRocketModule : KMonoBehaviour
{
	// Token: 0x170008EC RID: 2284
	// (get) Token: 0x0600878A RID: 34698 RVA: 0x000FD4F1 File Offset: 0x000FB6F1
	public PassengerRocketModule.RequestCrewState PassengersRequested
	{
		get
		{
			return this.passengersRequested;
		}
	}

	// Token: 0x0600878B RID: 34699 RVA: 0x0035ECC0 File Offset: 0x0035CEC0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
		GameUtil.SubscribeToTags<PassengerRocketModule>(this, PassengerRocketModule.OnRocketOnGroundTagDelegate, false);
		base.Subscribe<PassengerRocketModule>(-1547247383, PassengerRocketModule.OnClustercraftStateChanged);
		base.Subscribe<PassengerRocketModule>(1655598572, PassengerRocketModule.RefreshDelegate);
		base.Subscribe<PassengerRocketModule>(191901966, PassengerRocketModule.RefreshDelegate);
		base.Subscribe<PassengerRocketModule>(-71801987, PassengerRocketModule.RefreshDelegate);
		base.Subscribe<PassengerRocketModule>(-1277991738, PassengerRocketModule.OnLaunchDelegate);
		base.Subscribe<PassengerRocketModule>(-1432940121, PassengerRocketModule.OnReachableChangedDelegate);
		new ReachabilityMonitor.Instance(base.GetComponent<Workable>()).StartSM();
	}

	// Token: 0x0600878C RID: 34700 RVA: 0x000FD4F9 File Offset: 0x000FB6F9
	protected override void OnCleanUp()
	{
		Game.Instance.Unsubscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
		base.OnCleanUp();
	}

	// Token: 0x0600878D RID: 34701 RVA: 0x000FD51C File Offset: 0x000FB71C
	private void OnAssignmentGroupChanged(object data)
	{
		this.RefreshOrders();
	}

	// Token: 0x0600878E RID: 34702 RVA: 0x0035ED74 File Offset: 0x0035CF74
	private void RefreshClusterStateForAudio()
	{
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			if (activeWorld != null && activeWorld.IsModuleInterior)
			{
				UnityEngine.Object craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
				Clustercraft component = activeWorld.GetComponent<Clustercraft>();
				if (craftInterface == component.ModuleInterface)
				{
					ClusterManager.Instance.UpdateRocketInteriorAudio();
				}
			}
		}
	}

	// Token: 0x0600878F RID: 34703 RVA: 0x0035EDD4 File Offset: 0x0035CFD4
	private void OnReachableChanged(object data)
	{
		bool flag = (bool)data;
		KSelectable component = base.GetComponent<KSelectable>();
		if (flag)
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.PassengerModuleUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().BuildingStatusItems.PassengerModuleUnreachable, this);
	}

	// Token: 0x06008790 RID: 34704 RVA: 0x000FD524 File Offset: 0x000FB724
	public void RequestCrewBoard(PassengerRocketModule.RequestCrewState requestBoard)
	{
		this.passengersRequested = requestBoard;
		this.RefreshOrders();
	}

	// Token: 0x06008791 RID: 34705 RVA: 0x0035EE20 File Offset: 0x0035D020
	public bool ShouldCrewGetIn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		return this.passengersRequested == PassengerRocketModule.RequestCrewState.Request || (craftInterface.IsLaunchRequested() && craftInterface.CheckPreppedForLaunch());
	}

	// Token: 0x06008792 RID: 34706 RVA: 0x0035EE54 File Offset: 0x0035D054
	private void RefreshOrders()
	{
		if (!this.HasTag(GameTags.RocketOnGround) || !base.GetComponent<ClustercraftExteriorDoor>().HasTargetWorld())
		{
			return;
		}
		int cell = base.GetComponent<NavTeleporter>().GetCell();
		int num = base.GetComponent<ClustercraftExteriorDoor>().TargetCell();
		bool flag = this.ShouldCrewGetIn();
		if (flag)
		{
			using (List<MinionIdentity>.Enumerator enumerator = Components.LiveMinionIdentities.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MinionIdentity minionIdentity = enumerator.Current;
					bool flag2 = Game.Instance.assignmentManager.assignment_groups[base.GetComponent<AssignmentGroupController>().AssignmentGroupID].HasMember(minionIdentity.assignableProxy.Get());
					bool flag3 = minionIdentity.GetMyWorldId() == (int)Grid.WorldIdx[num];
					RocketPassengerMonitor.Instance smi = minionIdentity.GetSMI<RocketPassengerMonitor.Instance>();
					if (smi != null)
					{
						if (!flag3 && flag2)
						{
							smi.SetMoveTarget(num);
						}
						else if (flag3 && !flag2)
						{
							smi.SetMoveTarget(cell);
						}
						else
						{
							smi.ClearMoveTarget(num);
						}
					}
				}
				goto IL_146;
			}
		}
		foreach (MinionIdentity cmp in Components.LiveMinionIdentities.Items)
		{
			RocketPassengerMonitor.Instance smi2 = cmp.GetSMI<RocketPassengerMonitor.Instance>();
			if (smi2 != null)
			{
				smi2.ClearMoveTarget(cell);
				smi2.ClearMoveTarget(num);
			}
		}
		IL_146:
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			this.RefreshAccessStatus(Components.LiveMinionIdentities[i], flag);
		}
	}

	// Token: 0x06008793 RID: 34707 RVA: 0x0035EFF0 File Offset: 0x0035D1F0
	private void RefreshAccessStatus(MinionIdentity minion, bool restrict)
	{
		Component interiorDoor = base.GetComponent<ClustercraftExteriorDoor>().GetInteriorDoor();
		AccessControl component = base.GetComponent<AccessControl>();
		AccessControl component2 = interiorDoor.GetComponent<AccessControl>();
		if (!restrict)
		{
			component.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
			component2.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
			return;
		}
		if (Game.Instance.assignmentManager.assignment_groups[base.GetComponent<AssignmentGroupController>().AssignmentGroupID].HasMember(minion.assignableProxy.Get()))
		{
			component.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
			component2.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Neither);
			return;
		}
		component.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Neither);
		component2.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
	}

	// Token: 0x06008794 RID: 34708 RVA: 0x0035F0B8 File Offset: 0x0035D2B8
	public bool CheckPilotBoarded()
	{
		ICollection<IAssignableIdentity> members = base.GetComponent<AssignmentGroupController>().GetMembers();
		if (members.Count == 0)
		{
			return false;
		}
		List<IAssignableIdentity> list = new List<IAssignableIdentity>();
		foreach (IAssignableIdentity assignableIdentity in members)
		{
			MinionAssignablesProxy minionAssignablesProxy = (MinionAssignablesProxy)assignableIdentity;
			if (minionAssignablesProxy != null)
			{
				MinionResume component = minionAssignablesProxy.GetTargetGameObject().GetComponent<MinionResume>();
				if (component != null && component.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
				{
					list.Add(assignableIdentity);
				}
			}
		}
		if (list.Count == 0)
		{
			return false;
		}
		using (List<IAssignableIdentity>.Enumerator enumerator2 = list.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				if (((MinionAssignablesProxy)enumerator2.Current).GetTargetGameObject().GetMyWorldId() == (int)Grid.WorldIdx[base.GetComponent<ClustercraftExteriorDoor>().TargetCell()])
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06008795 RID: 34709 RVA: 0x0035F1CC File Offset: 0x0035D3CC
	public global::Tuple<int, int> GetCrewBoardedFraction()
	{
		ICollection<IAssignableIdentity> members = base.GetComponent<AssignmentGroupController>().GetMembers();
		if (members.Count == 0)
		{
			return new global::Tuple<int, int>(0, 0);
		}
		int num = 0;
		using (IEnumerator<IAssignableIdentity> enumerator = members.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (((MinionAssignablesProxy)enumerator.Current).GetTargetGameObject().GetMyWorldId() != (int)Grid.WorldIdx[base.GetComponent<ClustercraftExteriorDoor>().TargetCell()])
				{
					num++;
				}
			}
		}
		return new global::Tuple<int, int>(members.Count - num, members.Count);
	}

	// Token: 0x06008796 RID: 34710 RVA: 0x000FD533 File Offset: 0x000FB733
	public bool HasCrewAssigned()
	{
		return ((ICollection<IAssignableIdentity>)base.GetComponent<AssignmentGroupController>().GetMembers()).Count > 0;
	}

	// Token: 0x06008797 RID: 34711 RVA: 0x0035F264 File Offset: 0x0035D464
	public bool CheckPassengersBoarded(bool require_pilot = true)
	{
		ICollection<IAssignableIdentity> members = base.GetComponent<AssignmentGroupController>().GetMembers();
		if (members.Count == 0)
		{
			return false;
		}
		if (require_pilot)
		{
			bool flag = false;
			foreach (IAssignableIdentity assignableIdentity in members)
			{
				MinionAssignablesProxy minionAssignablesProxy = (MinionAssignablesProxy)assignableIdentity;
				if (minionAssignablesProxy != null)
				{
					MinionResume component = minionAssignablesProxy.GetTargetGameObject().GetComponent<MinionResume>();
					if (component != null && component.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		using (IEnumerator<IAssignableIdentity> enumerator = members.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (((MinionAssignablesProxy)enumerator.Current).GetTargetGameObject().GetMyWorldId() != (int)Grid.WorldIdx[base.GetComponent<ClustercraftExteriorDoor>().TargetCell()])
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06008798 RID: 34712 RVA: 0x0035F360 File Offset: 0x0035D560
	public bool CheckExtraPassengers()
	{
		ClustercraftExteriorDoor component = base.GetComponent<ClustercraftExteriorDoor>();
		if (component.HasTargetWorld())
		{
			byte worldId = Grid.WorldIdx[component.TargetCell()];
			List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems((int)worldId, false);
			string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
			for (int i = 0; i < worldItems.Count; i++)
			{
				if (!Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].HasMember(worldItems[i].assignableProxy.Get()))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06008799 RID: 34713 RVA: 0x0035F3E8 File Offset: 0x0035D5E8
	public void RemoveRocketPassenger(MinionIdentity minion)
	{
		if (minion != null)
		{
			string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
			MinionAssignablesProxy member = minion.assignableProxy.Get();
			if (Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].HasMember(member))
			{
				Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].RemoveMember(member);
			}
			this.RefreshOrders();
		}
	}

	// Token: 0x0600879A RID: 34714 RVA: 0x0035F454 File Offset: 0x0035D654
	public void RemovePassengersOnOtherWorlds()
	{
		ClustercraftExteriorDoor component = base.GetComponent<ClustercraftExteriorDoor>();
		if (component.HasTargetWorld())
		{
			int myWorldId = component.GetMyWorldId();
			string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
			{
				MinionAssignablesProxy member = minionIdentity.assignableProxy.Get();
				if (Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].HasMember(member) && minionIdentity.GetMyParentWorldId() != myWorldId)
				{
					Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].RemoveMember(member);
				}
			}
		}
	}

	// Token: 0x0600879B RID: 34715 RVA: 0x0035F51C File Offset: 0x0035D71C
	public void ClearMinionAssignments(object data)
	{
		string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
		foreach (IAssignableIdentity minionIdentity in Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].GetMembers())
		{
			Game.Instance.assignmentManager.RemoveFromWorld(minionIdentity, this.GetMyWorldId());
		}
	}

	// Token: 0x040066B3 RID: 26291
	public EventReference interiorReverbSnapshot;

	// Token: 0x040066B4 RID: 26292
	[Serialize]
	private PassengerRocketModule.RequestCrewState passengersRequested;

	// Token: 0x040066B5 RID: 26293
	private static readonly EventSystem.IntraObjectHandler<PassengerRocketModule> OnRocketOnGroundTagDelegate = GameUtil.CreateHasTagHandler<PassengerRocketModule>(GameTags.RocketOnGround, delegate(PassengerRocketModule component, object data)
	{
		component.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Release);
	});

	// Token: 0x040066B6 RID: 26294
	private static readonly EventSystem.IntraObjectHandler<PassengerRocketModule> OnClustercraftStateChanged = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule cmp, object data)
	{
		cmp.RefreshClusterStateForAudio();
	});

	// Token: 0x040066B7 RID: 26295
	private static EventSystem.IntraObjectHandler<PassengerRocketModule> RefreshDelegate = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule cmp, object data)
	{
		cmp.RefreshOrders();
		cmp.RefreshClusterStateForAudio();
	});

	// Token: 0x040066B8 RID: 26296
	private static EventSystem.IntraObjectHandler<PassengerRocketModule> OnLaunchDelegate = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule component, object data)
	{
		component.ClearMinionAssignments(data);
	});

	// Token: 0x040066B9 RID: 26297
	private static readonly EventSystem.IntraObjectHandler<PassengerRocketModule> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x0200196E RID: 6510
	public enum RequestCrewState
	{
		// Token: 0x040066BB RID: 26299
		Release,
		// Token: 0x040066BC RID: 26300
		Request
	}
}
