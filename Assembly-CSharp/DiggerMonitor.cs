using System;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x0200119B RID: 4507
public class DiggerMonitor : GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>
{
	// Token: 0x06005BB0 RID: 23472 RVA: 0x002A6964 File Offset: 0x002A4B64
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		this.loop.EventTransition(GameHashes.BeginMeteorBombardment, (DiggerMonitor.Instance smi) => Game.Instance, this.dig, (DiggerMonitor.Instance smi) => smi.CanTunnel());
		this.dig.ToggleBehaviour(GameTags.Creatures.Tunnel, (DiggerMonitor.Instance smi) => true, null).GoTo(this.loop);
	}

	// Token: 0x0400413B RID: 16699
	public GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.State loop;

	// Token: 0x0400413C RID: 16700
	public GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.State dig;

	// Token: 0x0200119C RID: 4508
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06005BB2 RID: 23474 RVA: 0x000E02C1 File Offset: 0x000DE4C1
		// (set) Token: 0x06005BB3 RID: 23475 RVA: 0x000E02C9 File Offset: 0x000DE4C9
		public int depthToDig { get; set; }
	}

	// Token: 0x0200119D RID: 4509
	public new class Instance : GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.GameInstance
	{
		// Token: 0x06005BB5 RID: 23477 RVA: 0x002A6A0C File Offset: 0x002A4C0C
		public Instance(IStateMachineTarget master, DiggerMonitor.Def def) : base(master, def)
		{
			global::World instance = global::World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			this.OnDestinationReachedDelegate = new Action<object>(this.OnDestinationReached);
			master.Subscribe(387220196, this.OnDestinationReachedDelegate);
			master.Subscribe(-766531887, this.OnDestinationReachedDelegate);
		}

		// Token: 0x06005BB6 RID: 23478 RVA: 0x002A6A84 File Offset: 0x002A4C84
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			global::World instance = global::World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			base.master.Unsubscribe(387220196, this.OnDestinationReachedDelegate);
			base.master.Unsubscribe(-766531887, this.OnDestinationReachedDelegate);
		}

		// Token: 0x06005BB7 RID: 23479 RVA: 0x000E02D2 File Offset: 0x000DE4D2
		private void OnDestinationReached(object data)
		{
			this.CheckInSolid();
		}

		// Token: 0x06005BB8 RID: 23480 RVA: 0x002A6AEC File Offset: 0x002A4CEC
		private void CheckInSolid()
		{
			Navigator component = base.gameObject.GetComponent<Navigator>();
			if (component == null)
			{
				return;
			}
			int cell = Grid.PosToCell(base.gameObject);
			if (component.CurrentNavType != NavType.Solid && Grid.IsSolidCell(cell))
			{
				component.SetCurrentNavType(NavType.Solid);
				return;
			}
			if (component.CurrentNavType == NavType.Solid && !Grid.IsSolidCell(cell))
			{
				component.SetCurrentNavType(NavType.Floor);
				base.gameObject.AddTag(GameTags.Creatures.Falling);
			}
		}

		// Token: 0x06005BB9 RID: 23481 RVA: 0x000E02D2 File Offset: 0x000DE4D2
		private void OnSolidChanged(int cell)
		{
			this.CheckInSolid();
		}

		// Token: 0x06005BBA RID: 23482 RVA: 0x002A6B60 File Offset: 0x002A4D60
		public bool CanTunnel()
		{
			int num = Grid.PosToCell(this);
			if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(num) == SubWorld.ZoneType.Space)
			{
				int num2 = num;
				while (Grid.IsValidCell(num2) && !Grid.Solid[num2])
				{
					num2 = Grid.CellAbove(num2);
				}
				if (!Grid.IsValidCell(num2))
				{
					return this.FoundValidDigCell();
				}
			}
			return false;
		}

		// Token: 0x06005BBB RID: 23483 RVA: 0x002A6BB8 File Offset: 0x002A4DB8
		private bool FoundValidDigCell()
		{
			int num = base.smi.def.depthToDig;
			int num2 = Grid.PosToCell(base.smi.master.gameObject);
			this.lastDigCell = num2;
			int cell = Grid.CellBelow(num2);
			while (this.IsValidDigCell(cell, null) && num > 0)
			{
				cell = Grid.CellBelow(cell);
				num--;
			}
			if (num > 0)
			{
				cell = GameUtil.FloodFillFind<object>(new Func<int, object, bool>(this.IsValidDigCell), null, num2, base.smi.def.depthToDig, false, true);
			}
			this.lastDigCell = cell;
			return this.lastDigCell != -1;
		}

		// Token: 0x06005BBC RID: 23484 RVA: 0x002A6C54 File Offset: 0x002A4E54
		private bool IsValidDigCell(int cell, object arg = null)
		{
			if (Grid.IsValidCell(cell) && Grid.Solid[cell])
			{
				if (!Grid.HasDoor[cell] && !Grid.Foundation[cell])
				{
					ushort index = Grid.ElementIdx[cell];
					Element element = ElementLoader.elements[(int)index];
					return Grid.Element[cell].hardness < 150 && !element.HasTag(GameTags.RefinedMetal);
				}
				GameObject gameObject = Grid.Objects[cell, 1];
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					return Grid.Element[cell].hardness < 150 && !component.Element.HasTag(GameTags.RefinedMetal);
				}
			}
			return false;
		}

		// Token: 0x0400413E RID: 16702
		[Serialize]
		public int lastDigCell = -1;

		// Token: 0x0400413F RID: 16703
		private Action<object> OnDestinationReachedDelegate;
	}
}
