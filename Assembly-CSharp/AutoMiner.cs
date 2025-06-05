using System;
using FMODUnity;
using KSerialization;
using UnityEngine;

// Token: 0x02000CD6 RID: 3286
[SerializationConfig(MemberSerialization.OptIn)]
public class AutoMiner : StateMachineComponent<AutoMiner.Instance>, ISim1000ms
{
	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x06003EB9 RID: 16057 RVA: 0x000CD342 File Offset: 0x000CB542
	private bool HasDigCell
	{
		get
		{
			return this.dig_cell != Grid.InvalidCell;
		}
	}

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x06003EBA RID: 16058 RVA: 0x000CD354 File Offset: 0x000CB554
	private bool RotationComplete
	{
		get
		{
			return this.HasDigCell && this.rotation_complete;
		}
	}

	// Token: 0x06003EBB RID: 16059 RVA: 0x000CD366 File Offset: 0x000CB566
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.simRenderLoadBalance = true;
	}

	// Token: 0x06003EBC RID: 16060 RVA: 0x00243824 File Offset: 0x00241A24
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hitEffectPrefab = Assets.GetPrefab("fx_dig_splash");
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		string name = component.name + ".gun";
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
		this.arm_anim_ctrl.initialAnim = "gun";
		this.arm_anim_ctrl.isMovable = true;
		this.arm_anim_ctrl.sceneLayer = Grid.SceneLayer.TransferArm;
		component.SetSymbolVisiblity("gun_target", false);
		bool flag;
		Vector3 position = component.GetSymbolTransform(new HashedString("gun_target"), out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
		this.arm_go.transform.SetPosition(position);
		this.arm_go.SetActive(true);
		this.link = new KAnimLink(component, this.arm_anim_ctrl);
		base.Subscribe<AutoMiner>(-592767678, AutoMiner.OnOperationalChangedDelegate);
		this.RotateArm(this.rotatable.GetRotatedOffset(Quaternion.Euler(0f, 0f, -45f) * Vector3.up), true, 0f);
		this.StopDig();
		base.smi.StartSM();
	}

	// Token: 0x06003EBD RID: 16061 RVA: 0x000CD375 File Offset: 0x000CB575
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003EBE RID: 16062 RVA: 0x000CD37D File Offset: 0x000CB57D
	public void Sim1000ms(float dt)
	{
		if (!this.operational.IsOperational)
		{
			return;
		}
		this.RefreshDiggableCell();
		this.operational.SetActive(this.HasDigCell, false);
	}

	// Token: 0x06003EBF RID: 16063 RVA: 0x000CD3A5 File Offset: 0x000CB5A5
	private void OnOperationalChanged(object data)
	{
		if (!(bool)data)
		{
			this.dig_cell = Grid.InvalidCell;
			this.rotation_complete = false;
		}
	}

	// Token: 0x06003EC0 RID: 16064 RVA: 0x002439F4 File Offset: 0x00241BF4
	public void UpdateRotation(float dt)
	{
		if (this.HasDigCell)
		{
			Vector3 a = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.TileMain);
			a.z = 0f;
			Vector3 position = this.arm_go.transform.GetPosition();
			position.z = 0f;
			Vector3 target_dir = Vector3.Normalize(a - position);
			this.RotateArm(target_dir, false, dt);
		}
	}

	// Token: 0x06003EC1 RID: 16065 RVA: 0x000CD3C1 File Offset: 0x000CB5C1
	private Element GetTargetElement()
	{
		if (this.HasDigCell)
		{
			return Grid.Element[this.dig_cell];
		}
		return null;
	}

	// Token: 0x06003EC2 RID: 16066 RVA: 0x00243A58 File Offset: 0x00241C58
	public void StartDig()
	{
		Element targetElement = this.GetTargetElement();
		base.Trigger(-1762453998, targetElement);
		this.CreateHitEffect();
		this.arm_anim_ctrl.Play("gun_digging", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06003EC3 RID: 16067 RVA: 0x000CD3D9 File Offset: 0x000CB5D9
	public void StopDig()
	{
		base.Trigger(939543986, null);
		this.DestroyHitEffect();
		this.arm_anim_ctrl.Play("gun", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06003EC4 RID: 16068 RVA: 0x00243AA0 File Offset: 0x00241CA0
	public void UpdateDig(float dt)
	{
		if (!this.HasDigCell)
		{
			return;
		}
		if (!this.rotation_complete)
		{
			return;
		}
		Diggable.DoDigTick(this.dig_cell, dt, WorldDamage.DamageType.NoBuildingDamage);
		float percentComplete = Grid.Damage[this.dig_cell];
		this.mining_sounds.SetPercentComplete(percentComplete);
		Vector3 a = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.FXFront2);
		a.z = 0f;
		Vector3 position = this.arm_go.transform.GetPosition();
		position.z = 0f;
		float sqrMagnitude = (a - position).sqrMagnitude;
		this.arm_anim_ctrl.GetBatchInstanceData().SetClipRadius(position.x, position.y, sqrMagnitude, true);
		if (!AutoMiner.ValidDigCell(this.dig_cell))
		{
			this.dig_cell = Grid.InvalidCell;
			this.rotation_complete = false;
		}
	}

	// Token: 0x06003EC5 RID: 16069 RVA: 0x00243B6C File Offset: 0x00241D6C
	private void CreateHitEffect()
	{
		if (this.hitEffectPrefab == null)
		{
			return;
		}
		if (this.hitEffect != null)
		{
			this.DestroyHitEffect();
		}
		Vector3 position = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.FXFront2);
		this.hitEffect = GameUtil.KInstantiate(this.hitEffectPrefab, position, Grid.SceneLayer.FXFront2, null, 0);
		this.hitEffect.SetActive(true);
		KBatchedAnimController component = this.hitEffect.GetComponent<KBatchedAnimController>();
		component.sceneLayer = Grid.SceneLayer.FXFront2;
		component.initialMode = KAnim.PlayMode.Loop;
		component.enabled = false;
		component.enabled = true;
	}

	// Token: 0x06003EC6 RID: 16070 RVA: 0x000CD40D File Offset: 0x000CB60D
	private void DestroyHitEffect()
	{
		if (this.hitEffectPrefab == null)
		{
			return;
		}
		if (this.hitEffect != null)
		{
			this.hitEffect.DeleteObject();
			this.hitEffect = null;
		}
	}

	// Token: 0x06003EC7 RID: 16071 RVA: 0x00243BF4 File Offset: 0x00241DF4
	private void RefreshDiggableCell()
	{
		CellOffset rotatedCellOffset = this.vision_offset;
		if (this.rotatable)
		{
			rotatedCellOffset = this.rotatable.GetRotatedCellOffset(this.vision_offset);
		}
		int cell = Grid.PosToCell(base.transform.gameObject);
		int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
		int num;
		int num2;
		Grid.CellToXY(cell2, out num, out num2);
		float num3 = float.MaxValue;
		int num4 = Grid.InvalidCell;
		Vector3 a = Grid.CellToPos(cell2);
		bool flag = false;
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				CellOffset rotatedCellOffset2 = new CellOffset(this.x + j, this.y + i);
				if (this.rotatable)
				{
					rotatedCellOffset2 = this.rotatable.GetRotatedCellOffset(rotatedCellOffset2);
				}
				int num5 = Grid.OffsetCell(cell, rotatedCellOffset2);
				if (Grid.IsValidCell(num5))
				{
					int x;
					int y;
					Grid.CellToXY(num5, out x, out y);
					if (Grid.IsValidCell(num5) && AutoMiner.ValidDigCell(num5) && Grid.TestLineOfSight(num, num2, x, y, new Func<int, bool>(AutoMiner.DigBlockingCB), false, false))
					{
						if (num5 == this.dig_cell)
						{
							flag = true;
						}
						Vector3 b = Grid.CellToPos(num5);
						float num6 = Vector3.Distance(a, b);
						if (num6 < num3)
						{
							num3 = num6;
							num4 = num5;
						}
					}
				}
			}
		}
		if (!flag && this.dig_cell != num4)
		{
			this.dig_cell = num4;
			this.rotation_complete = false;
		}
	}

	// Token: 0x06003EC8 RID: 16072 RVA: 0x00243D64 File Offset: 0x00241F64
	private static bool ValidDigCell(int cell)
	{
		bool flag = Grid.HasDoor[cell] && Grid.Foundation[cell] && Grid.ObjectLayers[9].ContainsKey(cell);
		if (flag)
		{
			Door component = Grid.ObjectLayers[9][cell].GetComponent<Door>();
			flag = (component != null && component.IsOpen() && !component.IsPendingClose());
		}
		return Grid.Solid[cell] && (!Grid.Foundation[cell] || flag) && Grid.Element[cell].hardness < 150;
	}

	// Token: 0x06003EC9 RID: 16073 RVA: 0x00243E0C File Offset: 0x0024200C
	public static bool DigBlockingCB(int cell)
	{
		bool flag = Grid.HasDoor[cell] && Grid.Foundation[cell] && Grid.ObjectLayers[9].ContainsKey(cell);
		if (flag)
		{
			Door component = Grid.ObjectLayers[9][cell].GetComponent<Door>();
			flag = (component != null && component.IsOpen() && !component.IsPendingClose());
		}
		return (Grid.Foundation[cell] && Grid.Solid[cell] && !flag) || Grid.Element[cell].hardness >= 150;
	}

	// Token: 0x06003ECA RID: 16074 RVA: 0x00243EB0 File Offset: 0x002420B0
	private void RotateArm(Vector3 target_dir, bool warp, float dt)
	{
		if (this.rotation_complete)
		{
			return;
		}
		float num = MathUtil.AngleSigned(Vector3.up, target_dir, Vector3.forward) - this.arm_rot;
		num = MathUtil.Wrap(-180f, 180f, num);
		this.rotation_complete = Mathf.Approximately(num, 0f);
		float num2 = num;
		if (warp)
		{
			this.rotation_complete = true;
		}
		else
		{
			num2 = Mathf.Clamp(num2, -this.turn_rate * dt, this.turn_rate * dt);
		}
		this.arm_rot += num2;
		this.arm_rot = MathUtil.Wrap(-180f, 180f, this.arm_rot);
		this.arm_go.transform.rotation = Quaternion.Euler(0f, 0f, this.arm_rot);
		if (!this.rotation_complete)
		{
			this.StartRotateSound();
			this.looping_sounds.SetParameter(this.rotateSound, AutoMiner.HASH_ROTATION, this.arm_rot);
			return;
		}
		this.StopRotateSound();
	}

	// Token: 0x06003ECB RID: 16075 RVA: 0x000CD43E File Offset: 0x000CB63E
	private void StartRotateSound()
	{
		if (!this.rotate_sound_playing)
		{
			this.looping_sounds.StartSound(this.rotateSound);
			this.rotate_sound_playing = true;
		}
	}

	// Token: 0x06003ECC RID: 16076 RVA: 0x000CD461 File Offset: 0x000CB661
	private void StopRotateSound()
	{
		if (this.rotate_sound_playing)
		{
			this.looping_sounds.StopSound(this.rotateSound);
			this.rotate_sound_playing = false;
		}
	}

	// Token: 0x04002B60 RID: 11104
	private static HashedString HASH_ROTATION = "rotation";

	// Token: 0x04002B61 RID: 11105
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002B62 RID: 11106
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04002B63 RID: 11107
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04002B64 RID: 11108
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04002B65 RID: 11109
	[MyCmpReq]
	private MiningSounds mining_sounds;

	// Token: 0x04002B66 RID: 11110
	public int x;

	// Token: 0x04002B67 RID: 11111
	public int y;

	// Token: 0x04002B68 RID: 11112
	public int width;

	// Token: 0x04002B69 RID: 11113
	public int height;

	// Token: 0x04002B6A RID: 11114
	public CellOffset vision_offset;

	// Token: 0x04002B6B RID: 11115
	private KBatchedAnimController arm_anim_ctrl;

	// Token: 0x04002B6C RID: 11116
	private GameObject arm_go;

	// Token: 0x04002B6D RID: 11117
	private LoopingSounds looping_sounds;

	// Token: 0x04002B6E RID: 11118
	private string rotateSoundName = "AutoMiner_rotate";

	// Token: 0x04002B6F RID: 11119
	private EventReference rotateSound;

	// Token: 0x04002B70 RID: 11120
	private KAnimLink link;

	// Token: 0x04002B71 RID: 11121
	private float arm_rot = 45f;

	// Token: 0x04002B72 RID: 11122
	private float turn_rate = 180f;

	// Token: 0x04002B73 RID: 11123
	private bool rotation_complete;

	// Token: 0x04002B74 RID: 11124
	private bool rotate_sound_playing;

	// Token: 0x04002B75 RID: 11125
	private GameObject hitEffectPrefab;

	// Token: 0x04002B76 RID: 11126
	private GameObject hitEffect;

	// Token: 0x04002B77 RID: 11127
	private int dig_cell = Grid.InvalidCell;

	// Token: 0x04002B78 RID: 11128
	private static readonly EventSystem.IntraObjectHandler<AutoMiner> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<AutoMiner>(delegate(AutoMiner component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x02000CD7 RID: 3287
	public class Instance : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.GameInstance
	{
		// Token: 0x06003ECF RID: 16079 RVA: 0x000CD4E2 File Offset: 0x000CB6E2
		public Instance(AutoMiner master) : base(master)
		{
		}
	}

	// Token: 0x02000CD8 RID: 3288
	public class States : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner>
	{
		// Token: 0x06003ED0 RID: 16080 RVA: 0x00243FA8 File Offset: 0x002421A8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (AutoMiner.Instance smi) => smi.GetComponent<Operational>().IsOperational);
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (AutoMiner.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.moving, (AutoMiner.Instance smi) => smi.GetComponent<Operational>().IsActive);
			this.on.moving.Exit(delegate(AutoMiner.Instance smi)
			{
				smi.master.StopRotateSound();
			}).PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (AutoMiner.Instance smi) => !smi.GetComponent<Operational>().IsActive).Update(delegate(AutoMiner.Instance smi, float dt)
			{
				smi.master.UpdateRotation(dt);
			}, UpdateRate.SIM_33ms, false).Transition(this.on.digging, new StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback(AutoMiner.States.RotationComplete), UpdateRate.SIM_200ms);
			this.on.digging.Enter(delegate(AutoMiner.Instance smi)
			{
				smi.master.StartDig();
			}).Exit(delegate(AutoMiner.Instance smi)
			{
				smi.master.StopDig();
			}).PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (AutoMiner.Instance smi) => !smi.GetComponent<Operational>().IsActive).Update(delegate(AutoMiner.Instance smi, float dt)
			{
				smi.master.UpdateDig(dt);
			}, UpdateRate.SIM_200ms, false).Transition(this.on.moving, GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Not(new StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback(AutoMiner.States.RotationComplete)), UpdateRate.SIM_200ms);
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x000CD4EB File Offset: 0x000CB6EB
		public static bool RotationComplete(AutoMiner.Instance smi)
		{
			return smi.master.RotationComplete;
		}

		// Token: 0x04002B79 RID: 11129
		public StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.BoolParameter transferring;

		// Token: 0x04002B7A RID: 11130
		public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State off;

		// Token: 0x04002B7B RID: 11131
		public AutoMiner.States.ReadyStates on;

		// Token: 0x02000CD9 RID: 3289
		public class ReadyStates : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State
		{
			// Token: 0x04002B7C RID: 11132
			public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State idle;

			// Token: 0x04002B7D RID: 11133
			public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State moving;

			// Token: 0x04002B7E RID: 11134
			public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State digging;
		}
	}
}
