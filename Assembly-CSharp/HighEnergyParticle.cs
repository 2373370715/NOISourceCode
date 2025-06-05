using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001422 RID: 5154
[SerializationConfig(MemberSerialization.OptIn)]
public class HighEnergyParticle : StateMachineComponent<HighEnergyParticle.StatesInstance>
{
	// Token: 0x060069A2 RID: 27042 RVA: 0x000E9B1C File Offset: 0x000E7D1C
	protected override void OnPrefabInit()
	{
		this.loopingSounds = base.gameObject.GetComponent<LoopingSounds>();
		this.flyingSound = GlobalAssets.GetSound("Radbolt_travel_LP", false);
		base.OnPrefabInit();
	}

	// Token: 0x060069A3 RID: 27043 RVA: 0x002E9AA0 File Offset: 0x002E7CA0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.HighEnergyParticles.Add(this);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.HighEnergyParticleCount, base.gameObject);
		this.emitter.SetEmitting(false);
		this.emitter.Refresh();
		this.SetDirection(this.direction);
		base.gameObject.layer = LayerMask.NameToLayer("PlaceWithDepth");
		this.StartLoopingSound();
		base.smi.StartSM();
	}

	// Token: 0x060069A4 RID: 27044 RVA: 0x002E9B28 File Offset: 0x002E7D28
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.StopLoopingSound();
		Components.HighEnergyParticles.Remove(this);
		if (this.capturedBy != null && this.capturedBy.currentParticle == this)
		{
			this.capturedBy.currentParticle = null;
		}
	}

	// Token: 0x060069A5 RID: 27045 RVA: 0x002E9B7C File Offset: 0x002E7D7C
	public void SetDirection(EightDirection direction)
	{
		this.direction = direction;
		float angle = EightDirectionUtil.GetAngle(direction);
		base.smi.master.transform.rotation = Quaternion.Euler(0f, 0f, angle);
	}

	// Token: 0x060069A6 RID: 27046 RVA: 0x002E9BBC File Offset: 0x002E7DBC
	public void Collide(HighEnergyParticle.CollisionType collisionType)
	{
		this.collision = collisionType;
		GameObject gameObject = new GameObject("HEPcollideFX");
		gameObject.SetActive(false);
		gameObject.transform.SetPosition(Grid.CellToPosCCC(Grid.PosToCell(base.smi.master.transform.position), Grid.SceneLayer.FXFront));
		KBatchedAnimController fxAnim = gameObject.AddComponent<KBatchedAnimController>();
		fxAnim.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("hep_impact_kanim")
		};
		fxAnim.initialAnim = "graze";
		gameObject.SetActive(true);
		switch (collisionType)
		{
		case HighEnergyParticle.CollisionType.Captured:
			fxAnim.Play("full", KAnim.PlayMode.Once, 1f, 0f);
			break;
		case HighEnergyParticle.CollisionType.CaptureAndRelease:
			fxAnim.Play("partial", KAnim.PlayMode.Once, 1f, 0f);
			break;
		case HighEnergyParticle.CollisionType.PassThrough:
			fxAnim.Play("graze", KAnim.PlayMode.Once, 1f, 0f);
			break;
		}
		fxAnim.onAnimComplete += delegate(HashedString arg)
		{
			Util.KDestroyGameObject(fxAnim);
		};
		if (collisionType == HighEnergyParticle.CollisionType.PassThrough)
		{
			this.collision = HighEnergyParticle.CollisionType.None;
			return;
		}
		base.smi.sm.destroySignal.Trigger(base.smi);
	}

	// Token: 0x060069A7 RID: 27047 RVA: 0x000E9B46 File Offset: 0x000E7D46
	public void DestroyNow()
	{
		base.smi.sm.destroySimpleSignal.Trigger(base.smi);
	}

	// Token: 0x060069A8 RID: 27048 RVA: 0x002E9D18 File Offset: 0x002E7F18
	private void Capture(HighEnergyParticlePort input)
	{
		if (input.currentParticle != null)
		{
			DebugUtil.LogArgs(new object[]
			{
				"Particle was backed up and caused an explosion!"
			});
			base.smi.sm.destroySignal.Trigger(base.smi);
			return;
		}
		this.capturedBy = input;
		input.currentParticle = this;
		input.Capture(this);
		if (input.currentParticle == this)
		{
			input.currentParticle = null;
			this.capturedBy = null;
			this.Collide(HighEnergyParticle.CollisionType.Captured);
			return;
		}
		this.capturedBy = null;
		this.Collide(HighEnergyParticle.CollisionType.CaptureAndRelease);
	}

	// Token: 0x060069A9 RID: 27049 RVA: 0x000E9B63 File Offset: 0x000E7D63
	public void Uncapture()
	{
		if (this.capturedBy != null)
		{
			this.capturedBy.currentParticle = null;
		}
		this.capturedBy = null;
	}

	// Token: 0x060069AA RID: 27050 RVA: 0x002E9DAC File Offset: 0x002E7FAC
	public void CheckCollision()
	{
		if (this.collision != HighEnergyParticle.CollisionType.None)
		{
			return;
		}
		int cell = Grid.PosToCell(base.smi.master.transform.GetPosition());
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			gameObject.GetComponent<Operational>();
			HighEnergyParticlePort component = gameObject.GetComponent<HighEnergyParticlePort>();
			if (component != null)
			{
				Vector2 pos = Grid.CellToPosCCC(component.GetHighEnergyParticleInputPortPosition(), Grid.SceneLayer.NoLayer);
				if (base.GetComponent<KCircleCollider2D>().Intersects(pos))
				{
					if (component.InputActive() && component.AllowCapture(this))
					{
						this.Capture(component);
						return;
					}
					this.Collide(HighEnergyParticle.CollisionType.PassThrough);
				}
			}
		}
		KCircleCollider2D component2 = base.GetComponent<KCircleCollider2D>();
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		ListPool<ScenePartitionerEntry, HighEnergyParticle>.PooledList pooledList = ListPool<ScenePartitionerEntry, HighEnergyParticle>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(num - 1, num2 - 1, 3, 3, GameScenePartitioner.Instance.collisionLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			KCollider2D kcollider2D = scenePartitionerEntry.obj as KCollider2D;
			HighEnergyParticle component3 = kcollider2D.gameObject.GetComponent<HighEnergyParticle>();
			if (!(component3 == null) && !(component3 == this) && component3.isCollideable)
			{
				bool flag = component2.Intersects(component3.transform.position);
				bool flag2 = kcollider2D.Intersects(base.transform.position);
				if (flag && flag2)
				{
					this.payload += component3.payload;
					component3.DestroyNow();
					this.Collide(HighEnergyParticle.CollisionType.HighEnergyParticle);
					return;
				}
			}
		}
		pooledList.Recycle();
		GameObject gameObject2 = Grid.Objects[cell, 3];
		if (gameObject2 != null)
		{
			ObjectLayerListItem objectLayerListItem = gameObject2.GetComponent<Pickupable>().objectLayerListItem;
			while (objectLayerListItem != null)
			{
				GameObject gameObject3 = objectLayerListItem.gameObject;
				objectLayerListItem = objectLayerListItem.nextItem;
				if (!(gameObject3 == null))
				{
					KPrefabID component4 = gameObject3.GetComponent<KPrefabID>();
					Health component5 = gameObject2.GetComponent<Health>();
					if (component5 != null && component4 != null && component4.HasTag(GameTags.Creature) && !component5.IsDefeated())
					{
						component5.Damage(20f);
						this.Collide(HighEnergyParticle.CollisionType.Creature);
						return;
					}
				}
			}
		}
		GameObject gameObject4 = Grid.Objects[cell, 0];
		if (gameObject4 != null)
		{
			Health component6 = gameObject4.GetComponent<Health>();
			if (component6 != null && !component6.IsDefeated() && !gameObject4.HasTag(GameTags.Dead) && !gameObject4.HasTag(GameTags.Dying))
			{
				component6.Damage(20f);
				WoundMonitor.Instance smi = gameObject4.GetSMI<WoundMonitor.Instance>();
				if (smi != null && !component6.IsDefeated())
				{
					smi.PlayKnockedOverImpactAnimation();
				}
				gameObject4.GetComponent<PrimaryElement>().AddDisease(Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id), Mathf.FloorToInt(this.payload * 0.5f / 0.01f), "HEPImpact");
				this.Collide(HighEnergyParticle.CollisionType.Minion);
				return;
			}
		}
		if (Grid.IsSolidCell(cell))
		{
			GameObject gameObject5 = Grid.Objects[cell, 9];
			if (gameObject5 == null || !gameObject5.HasTag(GameTags.HEPPassThrough) || this.capturedBy == null || this.capturedBy.gameObject != gameObject5)
			{
				this.Collide(HighEnergyParticle.CollisionType.Solid);
			}
			return;
		}
	}

	// Token: 0x060069AB RID: 27051 RVA: 0x002EA140 File Offset: 0x002E8340
	public void MovingUpdate(float dt)
	{
		if (this.collision != HighEnergyParticle.CollisionType.None)
		{
			return;
		}
		Vector3 position = base.transform.GetPosition();
		int num = Grid.PosToCell(position);
		Vector3 vector = position + EightDirectionUtil.GetNormal(this.direction) * this.speed * dt;
		int num2 = Grid.PosToCell(vector);
		SaveGame.Instance.ColonyAchievementTracker.radBoltTravelDistance += this.speed * dt;
		this.loopingSounds.UpdateVelocity(this.flyingSound, vector - position);
		if (!Grid.IsValidCell(num2))
		{
			base.smi.sm.destroySimpleSignal.Trigger(base.smi);
			return;
		}
		if (num != num2)
		{
			this.payload -= 0.1f;
			byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id);
			int disease_delta = Mathf.FloorToInt(5f);
			if (!Grid.Element[num2].IsVacuum)
			{
				SimMessages.ModifyDiseaseOnCell(num2, index, disease_delta);
			}
		}
		if (this.payload <= 0f)
		{
			base.smi.sm.destroySimpleSignal.Trigger(base.smi);
		}
		base.transform.SetPosition(vector);
	}

	// Token: 0x060069AC RID: 27052 RVA: 0x000E9B86 File Offset: 0x000E7D86
	private void StartLoopingSound()
	{
		this.loopingSounds.StartSound(this.flyingSound);
	}

	// Token: 0x060069AD RID: 27053 RVA: 0x000E9B9A File Offset: 0x000E7D9A
	private void StopLoopingSound()
	{
		this.loopingSounds.StopSound(this.flyingSound);
	}

	// Token: 0x0400500B RID: 20491
	[Serialize]
	private EightDirection direction;

	// Token: 0x0400500C RID: 20492
	[Serialize]
	public float speed;

	// Token: 0x0400500D RID: 20493
	[Serialize]
	public float payload;

	// Token: 0x0400500E RID: 20494
	[MyCmpReq]
	private RadiationEmitter emitter;

	// Token: 0x0400500F RID: 20495
	[Serialize]
	public float perCellFalloff;

	// Token: 0x04005010 RID: 20496
	[Serialize]
	public HighEnergyParticle.CollisionType collision;

	// Token: 0x04005011 RID: 20497
	[Serialize]
	public HighEnergyParticlePort capturedBy;

	// Token: 0x04005012 RID: 20498
	public short emitRadius;

	// Token: 0x04005013 RID: 20499
	public float emitRate;

	// Token: 0x04005014 RID: 20500
	public float emitSpeed;

	// Token: 0x04005015 RID: 20501
	private LoopingSounds loopingSounds;

	// Token: 0x04005016 RID: 20502
	public string flyingSound;

	// Token: 0x04005017 RID: 20503
	public bool isCollideable;

	// Token: 0x02001423 RID: 5155
	public enum CollisionType
	{
		// Token: 0x04005019 RID: 20505
		None,
		// Token: 0x0400501A RID: 20506
		Solid,
		// Token: 0x0400501B RID: 20507
		Creature,
		// Token: 0x0400501C RID: 20508
		Minion,
		// Token: 0x0400501D RID: 20509
		Captured,
		// Token: 0x0400501E RID: 20510
		HighEnergyParticle,
		// Token: 0x0400501F RID: 20511
		CaptureAndRelease,
		// Token: 0x04005020 RID: 20512
		PassThrough
	}

	// Token: 0x02001424 RID: 5156
	public class StatesInstance : GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.GameInstance
	{
		// Token: 0x060069AF RID: 27055 RVA: 0x000E9BB5 File Offset: 0x000E7DB5
		public StatesInstance(HighEnergyParticle smi) : base(smi)
		{
		}
	}

	// Token: 0x02001425 RID: 5157
	public class States : GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle>
	{
		// Token: 0x060069B0 RID: 27056 RVA: 0x002EA28C File Offset: 0x002E848C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready.pre;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.ready.OnSignal(this.destroySimpleSignal, this.destroying.instant).OnSignal(this.destroySignal, this.destroying.explode, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.Creature).OnSignal(this.destroySignal, this.destroying.explode, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.Minion).OnSignal(this.destroySignal, this.destroying.explode, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.Solid).OnSignal(this.destroySignal, this.destroying.blackhole, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.HighEnergyParticle).OnSignal(this.destroySignal, this.destroying.captured, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.Captured).OnSignal(this.destroySignal, this.catchAndRelease, (HighEnergyParticle.StatesInstance smi) => smi.master.collision == HighEnergyParticle.CollisionType.CaptureAndRelease).Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(true);
				smi.master.isCollideable = true;
			}).Update(delegate(HighEnergyParticle.StatesInstance smi, float dt)
			{
				smi.master.MovingUpdate(dt);
				smi.master.CheckCollision();
			}, UpdateRate.SIM_EVERY_TICK, false);
			this.ready.pre.PlayAnim("travel_pre").OnAnimQueueComplete(this.ready.moving);
			this.ready.moving.PlayAnim("travel_loop", KAnim.PlayMode.Loop);
			this.catchAndRelease.Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				smi.master.collision = HighEnergyParticle.CollisionType.None;
			}).PlayAnim("explode", KAnim.PlayMode.Once).OnAnimQueueComplete(this.ready.pre);
			this.destroying.Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				smi.master.isCollideable = false;
				smi.master.StopLoopingSound();
			});
			this.destroying.instant.Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				UnityEngine.Object.Destroy(smi.master.gameObject);
			});
			this.destroying.explode.PlayAnim("explode").Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				this.EmitRemainingPayload(smi);
			});
			this.destroying.blackhole.PlayAnim("collision").Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				this.EmitRemainingPayload(smi);
			});
			this.destroying.captured.PlayAnim("travel_pst").OnAnimQueueComplete(this.destroying.instant).Enter(delegate(HighEnergyParticle.StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(false);
			});
		}

		// Token: 0x060069B1 RID: 27057 RVA: 0x002EA5C4 File Offset: 0x002E87C4
		private void EmitRemainingPayload(HighEnergyParticle.StatesInstance smi)
		{
			smi.master.GetComponent<KBatchedAnimController>().GetCurrentAnim();
			smi.master.emitter.emitRadiusX = 6;
			smi.master.emitter.emitRadiusY = 6;
			smi.master.emitter.emitRads = smi.master.payload * 0.5f * 600f / 9f;
			smi.master.emitter.Refresh();
			SimMessages.AddRemoveSubstance(Grid.PosToCell(smi.master.gameObject), SimHashes.Fallout, CellEventLogger.Instance.ElementEmitted, smi.master.payload * 0.001f, 5000f, Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id), Mathf.FloorToInt(smi.master.payload * 0.5f / 0.01f), true, -1);
			smi.Schedule(1f, delegate(object obj)
			{
				UnityEngine.Object.Destroy(smi.master.gameObject);
			}, null);
		}

		// Token: 0x04005021 RID: 20513
		public HighEnergyParticle.States.ReadyStates ready;

		// Token: 0x04005022 RID: 20514
		public HighEnergyParticle.States.DestructionStates destroying;

		// Token: 0x04005023 RID: 20515
		public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State catchAndRelease;

		// Token: 0x04005024 RID: 20516
		public StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.Signal destroySignal;

		// Token: 0x04005025 RID: 20517
		public StateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.Signal destroySimpleSignal;

		// Token: 0x02001426 RID: 5158
		public class ReadyStates : GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State
		{
			// Token: 0x04005026 RID: 20518
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State pre;

			// Token: 0x04005027 RID: 20519
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State moving;
		}

		// Token: 0x02001427 RID: 5159
		public class DestructionStates : GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State
		{
			// Token: 0x04005028 RID: 20520
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State instant;

			// Token: 0x04005029 RID: 20521
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State explode;

			// Token: 0x0400502A RID: 20522
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State captured;

			// Token: 0x0400502B RID: 20523
			public GameStateMachine<HighEnergyParticle.States, HighEnergyParticle.StatesInstance, HighEnergyParticle, object>.State blackhole;
		}
	}
}
