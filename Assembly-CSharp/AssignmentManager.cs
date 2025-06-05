using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C68 RID: 3176
[AddComponentMenu("KMonoBehaviour/scripts/AssignmentManager")]
public class AssignmentManager : KMonoBehaviour
{
	// Token: 0x06003C39 RID: 15417 RVA: 0x000CB50A File Offset: 0x000C970A
	public IEnumerator<Assignable> GetEnumerator()
	{
		return this.assignables.GetEnumerator();
	}

	// Token: 0x06003C3A RID: 15418 RVA: 0x000CB51C File Offset: 0x000C971C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe<AssignmentManager>(586301400, AssignmentManager.MinionMigrationDelegate);
	}

	// Token: 0x06003C3B RID: 15419 RVA: 0x0023B394 File Offset: 0x00239594
	protected void MinionMigration(object data)
	{
		MinionMigrationEventArgs minionMigrationEventArgs = data as MinionMigrationEventArgs;
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.assignee != null)
			{
				Ownables soleOwner = assignable.assignee.GetSoleOwner();
				if (soleOwner != null && soleOwner.GetComponent<MinionAssignablesProxy>() != null && assignable.assignee.GetSoleOwner().GetComponent<MinionAssignablesProxy>().GetTargetGameObject() == minionMigrationEventArgs.minionId.gameObject)
				{
					assignable.Unassign();
				}
			}
		}
	}

	// Token: 0x06003C3C RID: 15420 RVA: 0x000CB539 File Offset: 0x000C9739
	public void Add(Assignable assignable)
	{
		this.assignables.Add(assignable);
	}

	// Token: 0x06003C3D RID: 15421 RVA: 0x000CB547 File Offset: 0x000C9747
	public void Remove(Assignable assignable)
	{
		this.assignables.Remove(assignable);
	}

	// Token: 0x06003C3E RID: 15422 RVA: 0x000CB556 File Offset: 0x000C9756
	public AssignmentGroup TryCreateAssignmentGroup(string id, IAssignableIdentity[] members, string name)
	{
		if (this.assignment_groups.ContainsKey(id))
		{
			return this.assignment_groups[id];
		}
		return new AssignmentGroup(id, members, name);
	}

	// Token: 0x06003C3F RID: 15423 RVA: 0x000CB57B File Offset: 0x000C977B
	public void RemoveAssignmentGroup(string id)
	{
		if (!this.assignment_groups.ContainsKey(id))
		{
			global::Debug.LogError("Assignment group with id " + id + " doesn't exists");
			return;
		}
		this.assignment_groups.Remove(id);
	}

	// Token: 0x06003C40 RID: 15424 RVA: 0x000CB5AE File Offset: 0x000C97AE
	public void AddToAssignmentGroup(string group_id, IAssignableIdentity member)
	{
		global::Debug.Assert(this.assignment_groups.ContainsKey(group_id));
		this.assignment_groups[group_id].AddMember(member);
	}

	// Token: 0x06003C41 RID: 15425 RVA: 0x000CB5D3 File Offset: 0x000C97D3
	public void RemoveFromAssignmentGroup(string group_id, IAssignableIdentity member)
	{
		global::Debug.Assert(this.assignment_groups.ContainsKey(group_id));
		this.assignment_groups[group_id].RemoveMember(member);
	}

	// Token: 0x06003C42 RID: 15426 RVA: 0x0023B440 File Offset: 0x00239640
	public void RemoveFromAllGroups(IAssignableIdentity member)
	{
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.assignee == member)
			{
				assignable.Unassign();
			}
		}
		foreach (KeyValuePair<string, AssignmentGroup> keyValuePair in this.assignment_groups)
		{
			if (keyValuePair.Value.HasMember(member))
			{
				keyValuePair.Value.RemoveMember(member);
			}
		}
	}

	// Token: 0x06003C43 RID: 15427 RVA: 0x0023B4F4 File Offset: 0x002396F4
	public void RemoveFromWorld(IAssignableIdentity minionIdentity, int world_id)
	{
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.assignee != null && assignable.assignee.GetOwners().Count == 1)
			{
				Ownables soleOwner = assignable.assignee.GetSoleOwner();
				if (soleOwner != null && soleOwner.GetComponent<MinionAssignablesProxy>() != null && assignable.assignee == minionIdentity && assignable.GetMyWorldId() == world_id)
				{
					assignable.Unassign();
				}
			}
		}
	}

	// Token: 0x06003C44 RID: 15428 RVA: 0x0023B598 File Offset: 0x00239798
	public List<Assignable> GetPreferredAssignables(Assignables owner, AssignableSlot slot)
	{
		List<Assignable> preferredAssignableResults = this.PreferredAssignableResults;
		List<Assignable> preferredAssignableResults2;
		lock (preferredAssignableResults)
		{
			this.PreferredAssignableResults.Clear();
			int num = int.MaxValue;
			foreach (Assignable assignable in this.assignables)
			{
				if (assignable.slot == slot && assignable.assignee != null && assignable.assignee.HasOwner(owner))
				{
					Room room = assignable.assignee as Room;
					if (room != null && room.roomType.priority_building_use)
					{
						this.PreferredAssignableResults.Clear();
						this.PreferredAssignableResults.Add(assignable);
						return this.PreferredAssignableResults;
					}
					int num2 = assignable.assignee.NumOwners();
					if (num2 == num)
					{
						this.PreferredAssignableResults.Add(assignable);
					}
					else if (num2 < num)
					{
						num = num2;
						this.PreferredAssignableResults.Clear();
						this.PreferredAssignableResults.Add(assignable);
					}
				}
			}
			preferredAssignableResults2 = this.PreferredAssignableResults;
		}
		return preferredAssignableResults2;
	}

	// Token: 0x06003C45 RID: 15429 RVA: 0x0023B6F8 File Offset: 0x002398F8
	public bool IsPreferredAssignable(Assignables owner, Assignable candidate)
	{
		IAssignableIdentity assignee = candidate.assignee;
		if (assignee == null || !assignee.HasOwner(owner))
		{
			return false;
		}
		int num = assignee.NumOwners();
		Room room = assignee as Room;
		if (room != null && room.roomType.priority_building_use)
		{
			return true;
		}
		foreach (Assignable assignable in this.assignables)
		{
			if (assignable.slot == candidate.slot && assignable.assignee != assignee)
			{
				Room room2 = assignable.assignee as Room;
				if (room2 != null && room2.roomType.priority_building_use && assignable.assignee.HasOwner(owner))
				{
					return false;
				}
				if (assignable.assignee.NumOwners() < num && assignable.assignee.HasOwner(owner))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x040029E2 RID: 10722
	private List<Assignable> assignables = new List<Assignable>();

	// Token: 0x040029E3 RID: 10723
	public const string PUBLIC_GROUP_ID = "public";

	// Token: 0x040029E4 RID: 10724
	public Dictionary<string, AssignmentGroup> assignment_groups = new Dictionary<string, AssignmentGroup>
	{
		{
			"public",
			new AssignmentGroup("public", new IAssignableIdentity[0], UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.PUBLIC)
		}
	};

	// Token: 0x040029E5 RID: 10725
	private static readonly EventSystem.IntraObjectHandler<AssignmentManager> MinionMigrationDelegate = new EventSystem.IntraObjectHandler<AssignmentManager>(delegate(AssignmentManager component, object data)
	{
		component.MinionMigration(data);
	});

	// Token: 0x040029E6 RID: 10726
	private List<Assignable> PreferredAssignableResults = new List<Assignable>();
}
