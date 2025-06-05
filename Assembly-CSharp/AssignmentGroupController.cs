using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x0200190D RID: 6413
public class AssignmentGroupController : KMonoBehaviour
{
	// Token: 0x1700087B RID: 2171
	// (get) Token: 0x060084B9 RID: 33977 RVA: 0x000FBB51 File Offset: 0x000F9D51
	// (set) Token: 0x060084BA RID: 33978 RVA: 0x000FBB59 File Offset: 0x000F9D59
	public string AssignmentGroupID
	{
		get
		{
			return this._assignmentGroupID;
		}
		private set
		{
			this._assignmentGroupID = value;
		}
	}

	// Token: 0x060084BB RID: 33979 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060084BC RID: 33980 RVA: 0x000FBB62 File Offset: 0x000F9D62
	[OnDeserialized]
	protected void CreateOrRestoreGroupID()
	{
		if (string.IsNullOrEmpty(this.AssignmentGroupID))
		{
			this.GenerateGroupID();
			return;
		}
		Game.Instance.assignmentManager.TryCreateAssignmentGroup(this.AssignmentGroupID, new IAssignableIdentity[0], base.gameObject.GetProperName());
	}

	// Token: 0x060084BD RID: 33981 RVA: 0x000FBB9F File Offset: 0x000F9D9F
	public void SetGroupID(string id)
	{
		DebugUtil.DevAssert(!string.IsNullOrEmpty(id), "Trying to set Assignment group on " + base.gameObject.name + " to null or empty.", null);
		if (!string.IsNullOrEmpty(id))
		{
			this.AssignmentGroupID = id;
		}
	}

	// Token: 0x060084BE RID: 33982 RVA: 0x000FBBD9 File Offset: 0x000F9DD9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreGroupAssignees();
	}

	// Token: 0x060084BF RID: 33983 RVA: 0x00353374 File Offset: 0x00351574
	private void GenerateGroupID()
	{
		if (!this.generateGroupOnStart)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.AssignmentGroupID))
		{
			return;
		}
		this.SetGroupID(base.GetComponent<KPrefabID>().PrefabID().ToString() + "_" + base.GetComponent<KPrefabID>().InstanceID.ToString() + "_assignmentGroup");
		Game.Instance.assignmentManager.TryCreateAssignmentGroup(this.AssignmentGroupID, new IAssignableIdentity[0], base.gameObject.GetProperName());
	}

	// Token: 0x060084C0 RID: 33984 RVA: 0x00353400 File Offset: 0x00351600
	private void RestoreGroupAssignees()
	{
		if (!this.generateGroupOnStart)
		{
			return;
		}
		this.CreateOrRestoreGroupID();
		if (this.minionsInGroupAtLoad == null)
		{
			this.minionsInGroupAtLoad = new Ref<MinionAssignablesProxy>[0];
		}
		for (int i = 0; i < this.minionsInGroupAtLoad.Length; i++)
		{
			Game.Instance.assignmentManager.AddToAssignmentGroup(this.AssignmentGroupID, this.minionsInGroupAtLoad[i].Get());
		}
		Ownable component = base.GetComponent<Ownable>();
		if (component != null)
		{
			component.Assign(Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID]);
			component.SetCanBeAssigned(false);
		}
	}

	// Token: 0x060084C1 RID: 33985 RVA: 0x000FBBE7 File Offset: 0x000F9DE7
	public bool CheckMinionIsMember(MinionAssignablesProxy minion)
	{
		if (string.IsNullOrEmpty(this.AssignmentGroupID))
		{
			this.GenerateGroupID();
		}
		return Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID].HasMember(minion);
	}

	// Token: 0x060084C2 RID: 33986 RVA: 0x0035349C File Offset: 0x0035169C
	public void SetMember(MinionAssignablesProxy minion, bool isAllowed)
	{
		Debug.Assert(DlcManager.IsExpansion1Active());
		if (!isAllowed)
		{
			Game.Instance.assignmentManager.RemoveFromAssignmentGroup(this.AssignmentGroupID, minion);
			return;
		}
		if (!this.CheckMinionIsMember(minion))
		{
			Game.Instance.assignmentManager.AddToAssignmentGroup(this.AssignmentGroupID, minion);
		}
	}

	// Token: 0x060084C3 RID: 33987 RVA: 0x000FBC1C File Offset: 0x000F9E1C
	protected override void OnCleanUp()
	{
		if (this.generateGroupOnStart)
		{
			Game.Instance.assignmentManager.RemoveAssignmentGroup(this.AssignmentGroupID);
		}
		base.OnCleanUp();
	}

	// Token: 0x060084C4 RID: 33988 RVA: 0x003534EC File Offset: 0x003516EC
	[OnSerializing]
	private void OnSerialize()
	{
		Debug.Assert(!string.IsNullOrEmpty(this.AssignmentGroupID), "Assignment group on " + base.gameObject.name + " has null or empty ID");
		List<IAssignableIdentity> members = Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID].GetMembers();
		this.minionsInGroupAtLoad = new Ref<MinionAssignablesProxy>[members.Count];
		for (int i = 0; i < members.Count; i++)
		{
			this.minionsInGroupAtLoad[i] = new Ref<MinionAssignablesProxy>((MinionAssignablesProxy)members[i]);
		}
	}

	// Token: 0x060084C5 RID: 33989 RVA: 0x000FBC41 File Offset: 0x000F9E41
	public List<IAssignableIdentity> GetMembers()
	{
		return Game.Instance.assignmentManager.assignment_groups[this.AssignmentGroupID].GetMembers();
	}

	// Token: 0x04006510 RID: 25872
	public bool generateGroupOnStart;

	// Token: 0x04006511 RID: 25873
	[Serialize]
	private string _assignmentGroupID;

	// Token: 0x04006512 RID: 25874
	[Serialize]
	private Ref<MinionAssignablesProxy>[] minionsInGroupAtLoad;
}
