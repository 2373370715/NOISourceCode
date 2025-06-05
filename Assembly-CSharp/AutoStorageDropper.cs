using System;
using UnityEngine;

// Token: 0x020009C0 RID: 2496
public class AutoStorageDropper : GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>
{
	// Token: 0x06002CCB RID: 11467 RVA: 0x001FAA04 File Offset: 0x001F8C04
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.root.Update(delegate(AutoStorageDropper.Instance smi, float dt)
		{
			smi.UpdateBlockedStatus();
		}, UpdateRate.SIM_200ms, true);
		this.idle.EventTransition(GameHashes.OnStorageChange, this.pre_drop, null).OnSignal(this.checkCanDrop, this.pre_drop, (AutoStorageDropper.Instance smi) => !smi.GetComponent<Storage>().IsEmpty()).ParamTransition<bool>(this.isBlocked, this.blocked, GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.IsTrue);
		this.pre_drop.ScheduleGoTo((AutoStorageDropper.Instance smi) => smi.def.delay, this.dropping);
		this.dropping.Enter(delegate(AutoStorageDropper.Instance smi)
		{
			smi.Drop();
		}).GoTo(this.idle);
		this.blocked.ParamTransition<bool>(this.isBlocked, this.pre_drop, GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.OutputTileBlocked, null);
	}

	// Token: 0x04001EA1 RID: 7841
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State idle;

	// Token: 0x04001EA2 RID: 7842
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State pre_drop;

	// Token: 0x04001EA3 RID: 7843
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State dropping;

	// Token: 0x04001EA4 RID: 7844
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State blocked;

	// Token: 0x04001EA5 RID: 7845
	private StateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.BoolParameter isBlocked;

	// Token: 0x04001EA6 RID: 7846
	public StateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.Signal checkCanDrop;

	// Token: 0x020009C1 RID: 2497
	public class DropperFxConfig
	{
		// Token: 0x04001EA7 RID: 7847
		public string animFile;

		// Token: 0x04001EA8 RID: 7848
		public string animName;

		// Token: 0x04001EA9 RID: 7849
		public Grid.SceneLayer layer = Grid.SceneLayer.FXFront;

		// Token: 0x04001EAA RID: 7850
		public bool useElementTint = true;

		// Token: 0x04001EAB RID: 7851
		public bool flipX;

		// Token: 0x04001EAC RID: 7852
		public bool flipY;
	}

	// Token: 0x020009C2 RID: 2498
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04001EAD RID: 7853
		public CellOffset dropOffset;

		// Token: 0x04001EAE RID: 7854
		public bool asOre;

		// Token: 0x04001EAF RID: 7855
		public SimHashes[] elementFilter;

		// Token: 0x04001EB0 RID: 7856
		public bool invertElementFilterInitialValue;

		// Token: 0x04001EB1 RID: 7857
		public bool blockedBySubstantialLiquid;

		// Token: 0x04001EB2 RID: 7858
		public AutoStorageDropper.DropperFxConfig neutralFx;

		// Token: 0x04001EB3 RID: 7859
		public AutoStorageDropper.DropperFxConfig leftFx;

		// Token: 0x04001EB4 RID: 7860
		public AutoStorageDropper.DropperFxConfig rightFx;

		// Token: 0x04001EB5 RID: 7861
		public AutoStorageDropper.DropperFxConfig upFx;

		// Token: 0x04001EB6 RID: 7862
		public AutoStorageDropper.DropperFxConfig downFx;

		// Token: 0x04001EB7 RID: 7863
		public Vector3 fxOffset = Vector3.zero;

		// Token: 0x04001EB8 RID: 7864
		public float cooldown = 2f;

		// Token: 0x04001EB9 RID: 7865
		public float delay;
	}

	// Token: 0x020009C3 RID: 2499
	public new class Instance : GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.GameInstance
	{
		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06002CCF RID: 11471 RVA: 0x000C1760 File Offset: 0x000BF960
		// (set) Token: 0x06002CD0 RID: 11472 RVA: 0x000C1768 File Offset: 0x000BF968
		public bool isInvertElementFilter { get; private set; }

		// Token: 0x06002CD1 RID: 11473 RVA: 0x000C1771 File Offset: 0x000BF971
		public Instance(IStateMachineTarget master, AutoStorageDropper.Def def) : base(master, def)
		{
			this.isInvertElementFilter = def.invertElementFilterInitialValue;
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x000C1787 File Offset: 0x000BF987
		public void SetInvertElementFilter(bool value)
		{
			base.smi.isInvertElementFilter = value;
			base.smi.sm.checkCanDrop.Trigger(base.smi);
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x001FAB40 File Offset: 0x001F8D40
		public void UpdateBlockedStatus()
		{
			int cell = Grid.PosToCell(base.smi.GetDropPosition());
			bool value = Grid.IsSolidCell(cell) || (base.def.blockedBySubstantialLiquid && Grid.IsSubstantialLiquid(cell, 0.35f));
			base.sm.isBlocked.Set(value, base.smi, false);
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x001FABA0 File Offset: 0x001F8DA0
		private bool IsFilteredElement(SimHashes element)
		{
			for (int num = 0; num != base.def.elementFilter.Length; num++)
			{
				if (base.def.elementFilter[num] == element)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x001FABD8 File Offset: 0x001F8DD8
		private bool AllowedToDrop(SimHashes element)
		{
			return base.def.elementFilter == null || base.def.elementFilter.Length == 0 || (!this.isInvertElementFilter && this.IsFilteredElement(element)) || (this.isInvertElementFilter && !this.IsFilteredElement(element));
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x001FAC28 File Offset: 0x001F8E28
		public void Drop()
		{
			bool flag = false;
			Element element = null;
			for (int i = this.m_storage.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = this.m_storage.items[i];
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (this.AllowedToDrop(component.ElementID))
				{
					if (base.def.asOre)
					{
						this.m_storage.Drop(gameObject, true);
						gameObject.transform.SetPosition(this.GetDropPosition());
						element = component.Element;
						flag = true;
					}
					else
					{
						Dumpable component2 = gameObject.GetComponent<Dumpable>();
						if (!component2.IsNullOrDestroyed())
						{
							component2.Dump(this.GetDropPosition());
							element = component.Element;
							flag = true;
						}
					}
				}
			}
			AutoStorageDropper.DropperFxConfig dropperAnim = this.GetDropperAnim();
			if (flag && dropperAnim != null && GameClock.Instance.GetTime() > this.m_timeSinceLastDrop + base.def.cooldown)
			{
				this.m_timeSinceLastDrop = GameClock.Instance.GetTime();
				Vector3 vector = Grid.CellToPosCCC(Grid.PosToCell(this.GetDropPosition()), dropperAnim.layer);
				vector += ((this.m_rotatable != null) ? this.m_rotatable.GetRotatedOffset(base.def.fxOffset) : base.def.fxOffset);
				KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(dropperAnim.animFile, vector, null, false, dropperAnim.layer, false);
				kbatchedAnimController.destroyOnAnimComplete = false;
				kbatchedAnimController.FlipX = dropperAnim.flipX;
				kbatchedAnimController.FlipY = dropperAnim.flipY;
				if (dropperAnim.useElementTint)
				{
					kbatchedAnimController.TintColour = element.substance.colour;
				}
				kbatchedAnimController.Play(dropperAnim.animName, KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x001FADF0 File Offset: 0x001F8FF0
		public AutoStorageDropper.DropperFxConfig GetDropperAnim()
		{
			CellOffset cellOffset = (this.m_rotatable != null) ? this.m_rotatable.GetRotatedCellOffset(base.def.dropOffset) : base.def.dropOffset;
			if (cellOffset.x < 0)
			{
				return base.def.leftFx;
			}
			if (cellOffset.x > 0)
			{
				return base.def.rightFx;
			}
			if (cellOffset.y < 0)
			{
				return base.def.downFx;
			}
			if (cellOffset.y > 0)
			{
				return base.def.upFx;
			}
			return base.def.neutralFx;
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x001FAE90 File Offset: 0x001F9090
		public Vector3 GetDropPosition()
		{
			if (!(this.m_rotatable != null))
			{
				return base.transform.GetPosition() + base.def.dropOffset.ToVector3();
			}
			return base.transform.GetPosition() + this.m_rotatable.GetRotatedCellOffset(base.def.dropOffset).ToVector3();
		}

		// Token: 0x04001EBA RID: 7866
		[MyCmpGet]
		private Storage m_storage;

		// Token: 0x04001EBB RID: 7867
		[MyCmpGet]
		private Rotatable m_rotatable;

		// Token: 0x04001EBD RID: 7869
		private float m_timeSinceLastDrop;
	}
}
