using System;
using System.Linq;
using KSerialization;

// Token: 0x0200107B RID: 4219
public class WarpReceiver : Workable
{
	// Token: 0x060055C0 RID: 21952 RVA: 0x000C1333 File Offset: 0x000BF533
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060055C1 RID: 21953 RVA: 0x000DC62D File Offset: 0x000DA82D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.warpReceiverSMI = new WarpReceiver.WarpReceiverSM.Instance(this);
		this.warpReceiverSMI.StartSM();
		Components.WarpReceivers.Add(this);
	}

	// Token: 0x060055C2 RID: 21954 RVA: 0x0028DCE8 File Offset: 0x0028BEE8
	public void ReceiveWarpedDuplicant(WorkerBase dupe)
	{
		dupe.transform.SetPosition(Grid.CellToPos(Grid.PosToCell(this), CellAlignment.Bottom, Grid.SceneLayer.Move));
		Debug.Assert(this.chore == null);
		KAnimFile anim = Assets.GetAnim("anim_interacts_warp_portal_receiver_kanim");
		ChoreType migrate = Db.Get().ChoreTypes.Migrate;
		KAnimFile override_anims = anim;
		this.chore = new WorkChore<Workable>(migrate, this, dupe.GetComponent<ChoreProvider>(), true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, true, true, override_anims, false, true, false, PriorityScreen.PriorityClass.compulsory, 5, false, true);
		Workable component = base.GetComponent<Workable>();
		component.workLayer = Grid.SceneLayer.Building;
		component.workAnims = new HashedString[]
		{
			"printing_pre",
			"printing_loop"
		};
		component.workingPstComplete = new HashedString[]
		{
			"printing_pst"
		};
		component.workingPstFailed = new HashedString[]
		{
			"printing_pst"
		};
		component.synchronizeAnims = true;
		float num = 0f;
		KAnimFileData data = anim.GetData();
		for (int i = 0; i < data.animCount; i++)
		{
			KAnim.Anim anim2 = data.GetAnim(i);
			if (component.workAnims.Contains(anim2.hash))
			{
				num += anim2.totalTime;
			}
		}
		component.SetWorkTime(num);
		this.Used = true;
	}

	// Token: 0x060055C3 RID: 21955 RVA: 0x000DC657 File Offset: 0x000DA857
	private void CompleteChore()
	{
		this.chore.Cleanup();
		this.chore = null;
		this.warpReceiverSMI.GoTo(this.warpReceiverSMI.sm.idle);
	}

	// Token: 0x060055C4 RID: 21956 RVA: 0x000DC686 File Offset: 0x000DA886
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.WarpReceivers.Remove(this);
	}

	// Token: 0x04003CB3 RID: 15539
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04003CB4 RID: 15540
	private WarpReceiver.WarpReceiverSM.Instance warpReceiverSMI;

	// Token: 0x04003CB5 RID: 15541
	private Notification notification;

	// Token: 0x04003CB6 RID: 15542
	[Serialize]
	public bool IsConsumed;

	// Token: 0x04003CB7 RID: 15543
	private Chore chore;

	// Token: 0x04003CB8 RID: 15544
	[Serialize]
	public bool Used;

	// Token: 0x0200107C RID: 4220
	public class WarpReceiverSM : GameStateMachine<WarpReceiver.WarpReceiverSM, WarpReceiver.WarpReceiverSM.Instance, WarpReceiver>
	{
		// Token: 0x060055C7 RID: 21959 RVA: 0x000DC6A1 File Offset: 0x000DA8A1
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("idle");
		}

		// Token: 0x04003CB9 RID: 15545
		public GameStateMachine<WarpReceiver.WarpReceiverSM, WarpReceiver.WarpReceiverSM.Instance, WarpReceiver, object>.State idle;

		// Token: 0x0200107D RID: 4221
		public new class Instance : GameStateMachine<WarpReceiver.WarpReceiverSM, WarpReceiver.WarpReceiverSM.Instance, WarpReceiver, object>.GameInstance
		{
			// Token: 0x060055C9 RID: 21961 RVA: 0x000DC6C4 File Offset: 0x000DA8C4
			public Instance(WarpReceiver master) : base(master)
			{
			}
		}
	}
}
