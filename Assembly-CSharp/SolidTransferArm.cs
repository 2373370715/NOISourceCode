using System;
using System.Collections.Generic;
using System.Diagnostics;
using Database;
using FMODUnity;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000FD9 RID: 4057
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidTransferArm : StateMachineComponent<SolidTransferArm.SMInstance>, ISim1000ms, IRenderEveryTick
{
	// Token: 0x06005193 RID: 20883 RVA: 0x0028044C File Offset: 0x0027E64C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreConsumer.AddProvider(GlobalChoreProvider.Instance);
		this.choreConsumer.SetReach(this.pickupRange);
		Klei.AI.Attributes attributes = this.GetAttributes();
		if (attributes.Get(Db.Get().Attributes.CarryAmount) == null)
		{
			attributes.Add(Db.Get().Attributes.CarryAmount);
		}
		AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.CarryAmount.Id, this.max_carry_weight, base.gameObject.GetProperName(), false, false, true);
		this.GetAttributes().Add(modifier);
		this.worker.usesMultiTool = false;
		this.storage.fxPrefix = Storage.FXPrefix.PickedUp;
		this.simRenderLoadBalance = false;
	}

	// Token: 0x06005194 RID: 20884 RVA: 0x00280510 File Offset: 0x0027E710
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		string name = component.name + ".arm";
		this.arm_go = new GameObject(name);
		this.arm_go.SetActive(false);
		this.arm_go.transform.parent = component.transform;
		this.looping_sounds = this.arm_go.AddComponent<LoopingSounds>();
		string sound = GlobalAssets.GetSound(this.rotateSoundName, false);
		this.rotateSound = RuntimeManager.PathToEventReference(sound);
		this.arm_go.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
		this.arm_anim_ctrl = this.arm_go.AddComponent<KBatchedAnimController>();
		this.arm_anim_ctrl.AnimFiles = new KAnimFile[]
		{
			component.AnimFiles[0]
		};
		this.arm_anim_ctrl.initialAnim = "arm";
		this.arm_anim_ctrl.isMovable = true;
		this.arm_anim_ctrl.sceneLayer = Grid.SceneLayer.TransferArm;
		component.SetSymbolVisiblity("arm_target", false);
		bool flag;
		Vector3 position = component.GetSymbolTransform(new HashedString("arm_target"), out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
		this.arm_go.transform.SetPosition(position);
		this.arm_go.SetActive(true);
		this.gameCell = Grid.PosToCell(this.arm_go);
		this.link = new KAnimLink(component, this.arm_anim_ctrl);
		ChoreGroups choreGroups = Db.Get().ChoreGroups;
		for (int i = 0; i < choreGroups.Count; i++)
		{
			this.choreConsumer.SetPermittedByUser(choreGroups[i], true);
		}
		base.Subscribe<SolidTransferArm>(-592767678, SolidTransferArm.OnOperationalChangedDelegate);
		base.Subscribe<SolidTransferArm>(1745615042, SolidTransferArm.OnEndChoreDelegate);
		this.RotateArm(this.rotatable.GetRotatedOffset(Vector3.up), true, 0f);
		this.DropLeftovers();
		component.enabled = false;
		component.enabled = true;
		MinionGroupProber.Get().SetValidSerialNos(this, this.serial_no, this.serial_no);
		base.smi.StartSM();
	}

	// Token: 0x06005195 RID: 20885 RVA: 0x000D9999 File Offset: 0x000D7B99
	protected override void OnCleanUp()
	{
		MinionGroupProber.Get().ReleaseProber(this);
		base.OnCleanUp();
	}

	// Token: 0x06005196 RID: 20886 RVA: 0x000D99AD File Offset: 0x000D7BAD
	public static void BatchUpdate(List<UpdateBucketWithUpdater<ISim1000ms>.Entry> solid_transfer_arms, float time_delta)
	{
		SolidTransferArm.SolidTransferArmBatchUpdater.Instance.Reset(solid_transfer_arms);
		GlobalJobManager.Run(SolidTransferArm.SolidTransferArmBatchUpdater.Instance);
		SolidTransferArm.SolidTransferArmBatchUpdater.Instance.Finish();
	}

	// Token: 0x06005197 RID: 20887 RVA: 0x00280730 File Offset: 0x0027E930
	private void Sim()
	{
		Chore.Precondition.Context context = default(Chore.Precondition.Context);
		if (this.choreConsumer.FindNextChore(ref context))
		{
			if (context.chore is FetchChore)
			{
				this.choreDriver.SetChore(context);
				FetchChore chore = context.chore as FetchChore;
				this.storage.DropUnlessMatching(chore);
				this.arm_anim_ctrl.enabled = false;
				this.arm_anim_ctrl.enabled = true;
			}
			else
			{
				bool condition = false;
				string str = "I am but a lowly transfer arm. I should only acquire FetchChores: ";
				Chore chore2 = context.chore;
				global::Debug.Assert(condition, str + ((chore2 != null) ? chore2.ToString() : null));
			}
		}
		this.operational.SetActive(this.choreDriver.HasChore(), false);
	}

	// Token: 0x06005198 RID: 20888 RVA: 0x000AA038 File Offset: 0x000A8238
	public void Sim1000ms(float dt)
	{
	}

	// Token: 0x06005199 RID: 20889 RVA: 0x002807D8 File Offset: 0x0027E9D8
	private void UpdateArmAnim()
	{
		FetchAreaChore fetchAreaChore = this.choreDriver.GetCurrentChore() as FetchAreaChore;
		if (this.worker.GetWorkable() && fetchAreaChore != null && this.rotation_complete)
		{
			this.StopRotateSound();
			this.SetArmAnim(fetchAreaChore.IsDelivering ? SolidTransferArm.ArmAnim.Drop : SolidTransferArm.ArmAnim.Pickup);
			return;
		}
		this.SetArmAnim(SolidTransferArm.ArmAnim.Idle);
	}

	// Token: 0x0600519A RID: 20890 RVA: 0x00280834 File Offset: 0x0027EA34
	private static bool AsyncUpdateVisitor(object obj, SolidTransferArm arm)
	{
		Pickupable pickupable = obj as Pickupable;
		if (Grid.GetCellRange(arm.gameCell, pickupable.cachedCell) <= arm.pickupRange && arm.IsPickupableRelevantToMyInterests(pickupable.KPrefabID, pickupable.cachedCell) && pickupable.CouldBePickedUpByTransferArm(arm.kPrefabID.InstanceID))
		{
			arm.pickupables.Add(pickupable);
		}
		return true;
	}

	// Token: 0x0600519B RID: 20891 RVA: 0x00280898 File Offset: 0x0027EA98
	private bool AsyncUpdate()
	{
		int num;
		int num2;
		Grid.CellToXY(this.gameCell, out num, out num2);
		bool flag = false;
		for (int i = num2 - this.pickupRange; i < num2 + this.pickupRange + 1; i++)
		{
			for (int j = num - this.pickupRange; j < num + this.pickupRange + 1; j++)
			{
				int num3 = Grid.XYToCell(j, i);
				if ((Grid.IsValidCell(num3) && Grid.IsPhysicallyAccessible(num, num2, j, i, true)) != this.reachableCells.Contains(num3))
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.reachableCells.Clear();
			for (int k = num2 - this.pickupRange; k < num2 + this.pickupRange + 1; k++)
			{
				for (int l = num - this.pickupRange; l < num + this.pickupRange + 1; l++)
				{
					int num4 = Grid.XYToCell(l, k);
					if (Grid.IsValidCell(num4) && Grid.IsPhysicallyAccessible(num, num2, l, k, true))
					{
						this.reachableCells.Add(num4);
					}
				}
			}
			this.IncrementSerialNo();
		}
		this.pickupables.Clear();
		GameScenePartitioner.Instance.AsyncSafeVisit<SolidTransferArm>(num - this.pickupRange, num2 - this.pickupRange, 2 * this.pickupRange + 1, 2 * this.pickupRange + 1, GameScenePartitioner.Instance.pickupablesLayer, SolidTransferArm.AsyncUpdateVisitor_s, this);
		GameScenePartitioner.Instance.AsyncSafeVisit<SolidTransferArm>(num - this.pickupRange, num2 - this.pickupRange, 2 * this.pickupRange + 1, 2 * this.pickupRange + 1, GameScenePartitioner.Instance.storedPickupablesLayer, SolidTransferArm.AsyncUpdateVisitor_s, this);
		return flag;
	}

	// Token: 0x0600519C RID: 20892 RVA: 0x000D99CE File Offset: 0x000D7BCE
	private void IncrementSerialNo()
	{
		this.serial_no += 1;
		MinionGroupProber.Get().SetValidSerialNos(this, this.serial_no, this.serial_no);
		MinionGroupProber.Get().Occupy(this, this.serial_no, this.reachableCells);
	}

	// Token: 0x0600519D RID: 20893 RVA: 0x000D9A0D File Offset: 0x000D7C0D
	public bool IsCellReachable(int cell)
	{
		return this.reachableCells.Contains(cell);
	}

	// Token: 0x0600519E RID: 20894 RVA: 0x000D9A1B File Offset: 0x000D7C1B
	private bool IsPickupableRelevantToMyInterests(KPrefabID prefabID, int storage_cell)
	{
		return Assets.IsTagSolidTransferArmConveyable(prefabID.PrefabTag) && this.IsCellReachable(storage_cell);
	}

	// Token: 0x0600519F RID: 20895 RVA: 0x000D9A33 File Offset: 0x000D7C33
	public Pickupable FindFetchTarget(Storage destination, FetchChore chore)
	{
		return FetchManager.FindFetchTarget(this.pickupables, destination, chore);
	}

	// Token: 0x060051A0 RID: 20896 RVA: 0x00280A34 File Offset: 0x0027EC34
	public void RenderEveryTick(float dt)
	{
		if (this.worker.GetWorkable())
		{
			Vector3 targetPoint = this.worker.GetWorkable().GetTargetPoint();
			targetPoint.z = 0f;
			Vector3 position = base.transform.GetPosition();
			position.z = 0f;
			Vector3 target_dir = Vector3.Normalize(targetPoint - position);
			this.RotateArm(target_dir, false, dt);
		}
		this.UpdateArmAnim();
	}

	// Token: 0x060051A1 RID: 20897 RVA: 0x000D9A42 File Offset: 0x000D7C42
	private void OnEndChore(object data)
	{
		this.DropLeftovers();
	}

	// Token: 0x060051A2 RID: 20898 RVA: 0x00280AA4 File Offset: 0x0027ECA4
	private void DropLeftovers()
	{
		if (!this.storage.IsEmpty() && !this.choreDriver.HasChore())
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x060051A3 RID: 20899 RVA: 0x00280AE4 File Offset: 0x0027ECE4
	private void SetArmAnim(SolidTransferArm.ArmAnim new_anim)
	{
		if (new_anim == this.arm_anim)
		{
			return;
		}
		this.arm_anim = new_anim;
		switch (this.arm_anim)
		{
		case SolidTransferArm.ArmAnim.Idle:
			this.arm_anim_ctrl.Play("arm", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		case SolidTransferArm.ArmAnim.Pickup:
			this.arm_anim_ctrl.Play("arm_pickup", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		case SolidTransferArm.ArmAnim.Drop:
			this.arm_anim_ctrl.Play("arm_drop", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		default:
			return;
		}
	}

	// Token: 0x060051A4 RID: 20900 RVA: 0x000D9A4A File Offset: 0x000D7C4A
	private void OnOperationalChanged(object data)
	{
		if (!(bool)data)
		{
			if (this.choreDriver.HasChore())
			{
				this.choreDriver.StopChore();
			}
			this.UpdateArmAnim();
		}
	}

	// Token: 0x060051A5 RID: 20901 RVA: 0x000D9A72 File Offset: 0x000D7C72
	private void SetArmRotation(float rot)
	{
		this.arm_rot = rot;
		this.arm_go.transform.rotation = Quaternion.Euler(0f, 0f, this.arm_rot);
	}

	// Token: 0x060051A6 RID: 20902 RVA: 0x00280B80 File Offset: 0x0027ED80
	private void RotateArm(Vector3 target_dir, bool warp, float dt)
	{
		float num = MathUtil.AngleSigned(Vector3.up, target_dir, Vector3.forward) - this.arm_rot;
		if (num < -180f)
		{
			num += 360f;
		}
		if (num > 180f)
		{
			num -= 360f;
		}
		if (!warp)
		{
			num = Mathf.Clamp(num, -this.turn_rate * dt, this.turn_rate * dt);
		}
		this.arm_rot += num;
		this.SetArmRotation(this.arm_rot);
		this.rotation_complete = Mathf.Approximately(num, 0f);
		if (!warp && !this.rotation_complete)
		{
			if (!this.rotateSoundPlaying)
			{
				this.StartRotateSound();
			}
			this.SetRotateSoundParameter(this.arm_rot);
			return;
		}
		this.StopRotateSound();
	}

	// Token: 0x060051A7 RID: 20903 RVA: 0x000D9AA0 File Offset: 0x000D7CA0
	private void StartRotateSound()
	{
		if (!this.rotateSoundPlaying)
		{
			this.looping_sounds.StartSound(this.rotateSound);
			this.rotateSoundPlaying = true;
		}
	}

	// Token: 0x060051A8 RID: 20904 RVA: 0x000D9AC3 File Offset: 0x000D7CC3
	private void SetRotateSoundParameter(float arm_rot)
	{
		if (this.rotateSoundPlaying)
		{
			this.looping_sounds.SetParameter(this.rotateSound, SolidTransferArm.HASH_ROTATION, arm_rot);
		}
	}

	// Token: 0x060051A9 RID: 20905 RVA: 0x000D9AE4 File Offset: 0x000D7CE4
	private void StopRotateSound()
	{
		if (this.rotateSoundPlaying)
		{
			this.looping_sounds.StopSound(this.rotateSound);
			this.rotateSoundPlaying = false;
		}
	}

	// Token: 0x060051AA RID: 20906 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name)
	{
	}

	// Token: 0x060051AB RID: 20907 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x060051AC RID: 20908 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name)
	{
	}

	// Token: 0x060051AD RID: 20909 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x04003970 RID: 14704
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003971 RID: 14705
	[MyCmpReq]
	private KPrefabID kPrefabID;

	// Token: 0x04003972 RID: 14706
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04003973 RID: 14707
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04003974 RID: 14708
	[MyCmpAdd]
	private StandardWorker worker;

	// Token: 0x04003975 RID: 14709
	[MyCmpAdd]
	private ChoreConsumer choreConsumer;

	// Token: 0x04003976 RID: 14710
	[MyCmpAdd]
	private ChoreDriver choreDriver;

	// Token: 0x04003977 RID: 14711
	public int pickupRange = 4;

	// Token: 0x04003978 RID: 14712
	private float max_carry_weight = 1000f;

	// Token: 0x04003979 RID: 14713
	private List<Pickupable> pickupables = new List<Pickupable>();

	// Token: 0x0400397A RID: 14714
	private KBatchedAnimController arm_anim_ctrl;

	// Token: 0x0400397B RID: 14715
	private GameObject arm_go;

	// Token: 0x0400397C RID: 14716
	private LoopingSounds looping_sounds;

	// Token: 0x0400397D RID: 14717
	private bool rotateSoundPlaying;

	// Token: 0x0400397E RID: 14718
	private string rotateSoundName = "TransferArm_rotate";

	// Token: 0x0400397F RID: 14719
	private EventReference rotateSound;

	// Token: 0x04003980 RID: 14720
	private KAnimLink link;

	// Token: 0x04003981 RID: 14721
	private float arm_rot = 45f;

	// Token: 0x04003982 RID: 14722
	private float turn_rate = 360f;

	// Token: 0x04003983 RID: 14723
	private bool rotation_complete;

	// Token: 0x04003984 RID: 14724
	private int gameCell;

	// Token: 0x04003985 RID: 14725
	private SolidTransferArm.ArmAnim arm_anim;

	// Token: 0x04003986 RID: 14726
	private HashSet<int> reachableCells = new HashSet<int>();

	// Token: 0x04003987 RID: 14727
	private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SolidTransferArm>(delegate(SolidTransferArm component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04003988 RID: 14728
	private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnEndChoreDelegate = new EventSystem.IntraObjectHandler<SolidTransferArm>(delegate(SolidTransferArm component, object data)
	{
		component.OnEndChore(data);
	});

	// Token: 0x04003989 RID: 14729
	private static Func<object, SolidTransferArm, bool> AsyncUpdateVisitor_s = new Func<object, SolidTransferArm, bool>(SolidTransferArm.AsyncUpdateVisitor);

	// Token: 0x0400398A RID: 14730
	private short serial_no;

	// Token: 0x0400398B RID: 14731
	private static HashedString HASH_ROTATION = "rotation";

	// Token: 0x02000FDA RID: 4058
	private enum ArmAnim
	{
		// Token: 0x0400398D RID: 14733
		Idle,
		// Token: 0x0400398E RID: 14734
		Pickup,
		// Token: 0x0400398F RID: 14735
		Drop
	}

	// Token: 0x02000FDB RID: 4059
	public class SMInstance : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.GameInstance
	{
		// Token: 0x060051B0 RID: 20912 RVA: 0x000D9B06 File Offset: 0x000D7D06
		public SMInstance(SolidTransferArm master) : base(master)
		{
		}
	}

	// Token: 0x02000FDC RID: 4060
	public class States : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm>
	{
		// Token: 0x060051B1 RID: 20913 RVA: 0x00280CF8 File Offset: 0x0027EEF8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (SolidTransferArm.SMInstance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(SolidTransferArm.SMInstance smi)
			{
				smi.master.StopRotateSound();
			});
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (SolidTransferArm.SMInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.working, (SolidTransferArm.SMInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.on.working.PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (SolidTransferArm.SMInstance smi) => !smi.GetComponent<Operational>().IsActive);
		}

		// Token: 0x04003990 RID: 14736
		public StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.BoolParameter transferring;

		// Token: 0x04003991 RID: 14737
		public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State off;

		// Token: 0x04003992 RID: 14738
		public SolidTransferArm.States.ReadyStates on;

		// Token: 0x02000FDD RID: 4061
		public class ReadyStates : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State
		{
			// Token: 0x04003993 RID: 14739
			public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State idle;

			// Token: 0x04003994 RID: 14740
			public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State working;
		}
	}

	// Token: 0x02000FDF RID: 4063
	private class SolidTransferArmBatchUpdater : WorkItemCollection<List<UpdateBucketWithUpdater<ISim1000ms>.Entry>>
	{
		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x060051BB RID: 20923 RVA: 0x000D9B38 File Offset: 0x000D7D38
		public static SolidTransferArm.SolidTransferArmBatchUpdater Instance
		{
			get
			{
				if (SolidTransferArm.SolidTransferArmBatchUpdater.instance == null)
				{
					SolidTransferArm.SolidTransferArmBatchUpdater.instance = new SolidTransferArm.SolidTransferArmBatchUpdater();
				}
				return SolidTransferArm.SolidTransferArmBatchUpdater.instance;
			}
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x000D9B50 File Offset: 0x000D7D50
		public void Reset(List<UpdateBucketWithUpdater<ISim1000ms>.Entry> entries)
		{
			this.sharedData = entries;
			this.count = (entries.Count + 8 - 1) / 8;
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x00280E58 File Offset: 0x0027F058
		public override void RunItem(int item, ref List<UpdateBucketWithUpdater<ISim1000ms>.Entry> shared_data, int threadIndex)
		{
			int num = item * 8;
			int num2 = Math.Min(shared_data.Count, num + 8);
			for (int i = num; i < num2; i++)
			{
				SolidTransferArm solidTransferArm = (SolidTransferArm)shared_data[i].data;
				if (solidTransferArm.operational.IsOperational)
				{
					solidTransferArm.AsyncUpdate();
				}
			}
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x00280EAC File Offset: 0x0027F0AC
		public void Finish()
		{
			foreach (UpdateBucketWithUpdater<ISim1000ms>.Entry entry in this.sharedData)
			{
				SolidTransferArm solidTransferArm = (SolidTransferArm)entry.data;
				if (solidTransferArm.operational.IsOperational)
				{
					solidTransferArm.Sim();
				}
			}
			this.Reset(SolidTransferArm.SolidTransferArmBatchUpdater.EmptyList);
		}

		// Token: 0x0400399B RID: 14747
		private static readonly List<UpdateBucketWithUpdater<ISim1000ms>.Entry> EmptyList = new List<UpdateBucketWithUpdater<ISim1000ms>.Entry>();

		// Token: 0x0400399C RID: 14748
		private const int kBatchSize = 8;

		// Token: 0x0400399D RID: 14749
		private static SolidTransferArm.SolidTransferArmBatchUpdater instance;
	}

	// Token: 0x02000FE0 RID: 4064
	public struct CachedPickupable
	{
		// Token: 0x0400399E RID: 14750
		public Pickupable pickupable;

		// Token: 0x0400399F RID: 14751
		public int storage_cell;
	}
}
