using System;
using System.Collections.Generic;

// Token: 0x02000A98 RID: 2712
public class AssignmentGroup : IAssignableIdentity
{
	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x06003159 RID: 12633 RVA: 0x000C480F File Offset: 0x000C2A0F
	// (set) Token: 0x0600315A RID: 12634 RVA: 0x000C4817 File Offset: 0x000C2A17
	public string id { get; private set; }

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x0600315B RID: 12635 RVA: 0x000C4820 File Offset: 0x000C2A20
	// (set) Token: 0x0600315C RID: 12636 RVA: 0x000C4828 File Offset: 0x000C2A28
	public string name { get; private set; }

	// Token: 0x0600315D RID: 12637 RVA: 0x0020C810 File Offset: 0x0020AA10
	public AssignmentGroup(string id, IAssignableIdentity[] members, string name)
	{
		this.id = id;
		this.name = name;
		foreach (IAssignableIdentity item in members)
		{
			this.members.Add(item);
		}
		if (Game.Instance != null)
		{
			Game.Instance.assignmentManager.assignment_groups.Add(id, this);
			Game.Instance.Trigger(-1123234494, this);
		}
	}

	// Token: 0x0600315E RID: 12638 RVA: 0x000C4831 File Offset: 0x000C2A31
	public void AddMember(IAssignableIdentity member)
	{
		if (!this.members.Contains(member))
		{
			this.members.Add(member);
		}
		Game.Instance.Trigger(-1123234494, this);
	}

	// Token: 0x0600315F RID: 12639 RVA: 0x000C485D File Offset: 0x000C2A5D
	public void RemoveMember(IAssignableIdentity member)
	{
		this.members.Remove(member);
		Game.Instance.Trigger(-1123234494, this);
	}

	// Token: 0x06003160 RID: 12640 RVA: 0x000C487C File Offset: 0x000C2A7C
	public string GetProperName()
	{
		return this.name;
	}

	// Token: 0x06003161 RID: 12641 RVA: 0x000C4884 File Offset: 0x000C2A84
	public bool HasMember(IAssignableIdentity member)
	{
		return this.members.Contains(member);
	}

	// Token: 0x06003162 RID: 12642 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool IsNull()
	{
		return false;
	}

	// Token: 0x06003163 RID: 12643 RVA: 0x000C4892 File Offset: 0x000C2A92
	public List<IAssignableIdentity> GetMembers()
	{
		return this.members;
	}

	// Token: 0x06003164 RID: 12644 RVA: 0x0020C89C File Offset: 0x0020AA9C
	public List<Ownables> GetOwners()
	{
		this.current_owners.Clear();
		foreach (IAssignableIdentity assignableIdentity in this.members)
		{
			this.current_owners.AddRange(assignableIdentity.GetOwners());
		}
		return this.current_owners;
	}

	// Token: 0x06003165 RID: 12645 RVA: 0x0020C90C File Offset: 0x0020AB0C
	public Ownables GetSoleOwner()
	{
		if (this.members.Count == 1)
		{
			return this.members[0] as Ownables;
		}
		Debug.LogWarningFormat("GetSoleOwner called on AssignmentGroup with {0} members", new object[]
		{
			this.members.Count
		});
		return null;
	}

	// Token: 0x06003166 RID: 12646 RVA: 0x0020C960 File Offset: 0x0020AB60
	public bool HasOwner(Assignables owner)
	{
		using (List<IAssignableIdentity>.Enumerator enumerator = this.members.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasOwner(owner))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003167 RID: 12647 RVA: 0x0020C9BC File Offset: 0x0020ABBC
	public int NumOwners()
	{
		int num = 0;
		foreach (IAssignableIdentity assignableIdentity in this.members)
		{
			num += assignableIdentity.NumOwners();
		}
		return num;
	}

	// Token: 0x040021EB RID: 8683
	private List<IAssignableIdentity> members = new List<IAssignableIdentity>();

	// Token: 0x040021EC RID: 8684
	public List<Ownables> current_owners = new List<Ownables>();
}
