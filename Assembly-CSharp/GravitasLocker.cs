using System;
using UnityEngine;

// Token: 0x020013CF RID: 5071
public class GravitasLocker : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>
{
	// Token: 0x06006814 RID: 26644 RVA: 0x002E5828 File Offset: 0x002E3A28
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.close;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.close.ParamTransition<bool>(this.IsOpen, this.open, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsTrue).DefaultState(this.close.idle);
		this.close.idle.PlayAnim("on").ParamTransition<bool>(this.WorkOrderGiven, this.close.work, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsTrue);
		this.close.work.DefaultState(this.close.work.waitingForDupe);
		this.close.work.waitingForDupe.Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StartlWorkChore_OpenLocker)).Exit(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StopWorkChore)).WorkableCompleteTransition((GravitasLocker.Instance smi) => smi.GetWorkable(), this.close.work.complete).ParamTransition<bool>(this.WorkOrderGiven, this.close, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsFalse);
		this.close.work.complete.Enter(delegate(GravitasLocker.Instance smi)
		{
			this.WorkOrderGiven.Set(false, smi, false);
		}).Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.Open)).TriggerOnEnter(GameHashes.UIRefresh, null);
		this.open.ParamTransition<bool>(this.IsOpen, this.close, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsFalse).DefaultState(this.open.opening);
		this.open.opening.PlayAnim("working").OnAnimQueueComplete(this.open.idle);
		this.open.idle.PlayAnim("empty").Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.SpawnLoot)).ParamTransition<bool>(this.WorkOrderGiven, this.open.work, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsTrue);
		this.open.work.DefaultState(this.open.work.waitingForDupe);
		this.open.work.waitingForDupe.Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StartWorkChore_CloseLocker)).Exit(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StopWorkChore)).WorkableCompleteTransition((GravitasLocker.Instance smi) => smi.GetWorkable(), this.open.work.complete).ParamTransition<bool>(this.WorkOrderGiven, this.open.idle, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsFalse);
		this.open.work.complete.Enter(delegate(GravitasLocker.Instance smi)
		{
			this.WorkOrderGiven.Set(false, smi, false);
		}).Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.Close)).TriggerOnEnter(GameHashes.UIRefresh, null);
	}

	// Token: 0x06006815 RID: 26645 RVA: 0x000E87DA File Offset: 0x000E69DA
	public static void Open(GravitasLocker.Instance smi)
	{
		smi.Open();
	}

	// Token: 0x06006816 RID: 26646 RVA: 0x000E87E2 File Offset: 0x000E69E2
	public static void Close(GravitasLocker.Instance smi)
	{
		smi.Close();
	}

	// Token: 0x06006817 RID: 26647 RVA: 0x000E87EA File Offset: 0x000E69EA
	public static void SpawnLoot(GravitasLocker.Instance smi)
	{
		smi.SpawnLoot();
	}

	// Token: 0x06006818 RID: 26648 RVA: 0x000E87F2 File Offset: 0x000E69F2
	public static void StartWorkChore_CloseLocker(GravitasLocker.Instance smi)
	{
		smi.CreateWorkChore_CloseLocker();
	}

	// Token: 0x06006819 RID: 26649 RVA: 0x000E87FA File Offset: 0x000E69FA
	public static void StartlWorkChore_OpenLocker(GravitasLocker.Instance smi)
	{
		smi.CreateWorkChore_OpenLocker();
	}

	// Token: 0x0600681A RID: 26650 RVA: 0x000E8802 File Offset: 0x000E6A02
	public static void StopWorkChore(GravitasLocker.Instance smi)
	{
		smi.StopWorkChore();
	}

	// Token: 0x04004E9B RID: 20123
	public const float CLOSE_WORKTIME = 1f;

	// Token: 0x04004E9C RID: 20124
	public const float OPEN_WORKTIME = 1.5f;

	// Token: 0x04004E9D RID: 20125
	public const string CLOSED_ANIM_NAME = "on";

	// Token: 0x04004E9E RID: 20126
	public const string OPENING_ANIM_NAME = "working";

	// Token: 0x04004E9F RID: 20127
	public const string OPENED = "empty";

	// Token: 0x04004EA0 RID: 20128
	private StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.BoolParameter IsOpen;

	// Token: 0x04004EA1 RID: 20129
	private StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.BoolParameter WasEmptied;

	// Token: 0x04004EA2 RID: 20130
	private StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.BoolParameter WorkOrderGiven;

	// Token: 0x04004EA3 RID: 20131
	public GravitasLocker.CloseStates close;

	// Token: 0x04004EA4 RID: 20132
	public GravitasLocker.OpenStates open;

	// Token: 0x020013D0 RID: 5072
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04004EA5 RID: 20133
		public bool CanBeClosed;

		// Token: 0x04004EA6 RID: 20134
		public string SideScreen_OpenButtonText;

		// Token: 0x04004EA7 RID: 20135
		public string SideScreen_OpenButtonTooltip;

		// Token: 0x04004EA8 RID: 20136
		public string SideScreen_CancelOpenButtonText;

		// Token: 0x04004EA9 RID: 20137
		public string SideScreen_CancelOpenButtonTooltip;

		// Token: 0x04004EAA RID: 20138
		public string SideScreen_CloseButtonText;

		// Token: 0x04004EAB RID: 20139
		public string SideScreen_CloseButtonTooltip;

		// Token: 0x04004EAC RID: 20140
		public string SideScreen_CancelCloseButtonText;

		// Token: 0x04004EAD RID: 20141
		public string SideScreen_CancelCloseButtonTooltip;

		// Token: 0x04004EAE RID: 20142
		public string OPEN_INTERACT_ANIM_NAME = "anim_interacts_clothingfactory_kanim";

		// Token: 0x04004EAF RID: 20143
		public string CLOSE_INTERACT_ANIM_NAME = "anim_interacts_clothingfactory_kanim";

		// Token: 0x04004EB0 RID: 20144
		public string[] ObjectsToSpawn = new string[0];

		// Token: 0x04004EB1 RID: 20145
		public string[] LootSymbols = new string[0];
	}

	// Token: 0x020013D1 RID: 5073
	public class WorkStates : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State
	{
		// Token: 0x04004EB2 RID: 20146
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State waitingForDupe;

		// Token: 0x04004EB3 RID: 20147
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State complete;
	}

	// Token: 0x020013D2 RID: 5074
	public class CloseStates : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State
	{
		// Token: 0x04004EB4 RID: 20148
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State idle;

		// Token: 0x04004EB5 RID: 20149
		public GravitasLocker.WorkStates work;
	}

	// Token: 0x020013D3 RID: 5075
	public class OpenStates : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State
	{
		// Token: 0x04004EB6 RID: 20150
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State opening;

		// Token: 0x04004EB7 RID: 20151
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State idle;

		// Token: 0x04004EB8 RID: 20152
		public GravitasLocker.WorkStates work;
	}

	// Token: 0x020013D4 RID: 5076
	public new class Instance : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06006822 RID: 26658 RVA: 0x000E8861 File Offset: 0x000E6A61
		public bool WorkOrderGiven
		{
			get
			{
				return base.smi.sm.WorkOrderGiven.Get(base.smi);
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06006823 RID: 26659 RVA: 0x000E887E File Offset: 0x000E6A7E
		public bool IsOpen
		{
			get
			{
				return base.smi.sm.IsOpen.Get(base.smi);
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06006824 RID: 26660 RVA: 0x000E889B File Offset: 0x000E6A9B
		public bool HasContents
		{
			get
			{
				return !base.smi.sm.WasEmptied.Get(base.smi) && base.def.ObjectsToSpawn.Length != 0;
			}
		}

		// Token: 0x06006825 RID: 26661 RVA: 0x000E88CB File Offset: 0x000E6ACB
		public Workable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x06006826 RID: 26662 RVA: 0x000E88D3 File Offset: 0x000E6AD3
		public void Open()
		{
			base.smi.sm.IsOpen.Set(true, base.smi, false);
		}

		// Token: 0x06006827 RID: 26663 RVA: 0x000E88F3 File Offset: 0x000E6AF3
		public void Close()
		{
			base.smi.sm.IsOpen.Set(false, base.smi, false);
		}

		// Token: 0x06006828 RID: 26664 RVA: 0x000E8913 File Offset: 0x000E6B13
		public Instance(IStateMachineTarget master, GravitasLocker.Def def) : base(master, def)
		{
		}

		// Token: 0x06006829 RID: 26665 RVA: 0x000E891D File Offset: 0x000E6B1D
		public override void StartSM()
		{
			this.DefineDropSpawnPositions();
			base.StartSM();
			this.UpdateContentPreviewSymbols();
		}

		// Token: 0x0600682A RID: 26666 RVA: 0x002E5AF4 File Offset: 0x002E3CF4
		public void DefineDropSpawnPositions()
		{
			if (this.dropSpawnPositions == null && base.def.LootSymbols.Length != 0)
			{
				this.dropSpawnPositions = new Vector3[base.def.LootSymbols.Length];
				for (int i = 0; i < this.dropSpawnPositions.Length; i++)
				{
					bool flag;
					Vector3 vector = this.animController.GetSymbolTransform(base.def.LootSymbols[i], out flag).GetColumn(3);
					vector.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
					this.dropSpawnPositions[i] = (flag ? vector : base.gameObject.transform.GetPosition());
				}
			}
		}

		// Token: 0x0600682B RID: 26667 RVA: 0x002E5BA8 File Offset: 0x002E3DA8
		public void CreateWorkChore_CloseLocker()
		{
			if (this.chore == null)
			{
				this.workable.SetWorkTime(1f);
				this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.Repair, this.workable, null, true, null, null, null, true, null, false, true, Assets.GetAnim(base.def.CLOSE_INTERACT_ANIM_NAME), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
			}
		}

		// Token: 0x0600682C RID: 26668 RVA: 0x002E5C14 File Offset: 0x002E3E14
		public void CreateWorkChore_OpenLocker()
		{
			if (this.chore == null)
			{
				this.workable.SetWorkTime(1.5f);
				this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, this.workable, null, true, null, null, null, true, null, false, true, Assets.GetAnim(base.def.OPEN_INTERACT_ANIM_NAME), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
			}
		}

		// Token: 0x0600682D RID: 26669 RVA: 0x000E8931 File Offset: 0x000E6B31
		public void StopWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Canceled by user");
				this.chore = null;
			}
		}

		// Token: 0x0600682E RID: 26670 RVA: 0x002E5C80 File Offset: 0x002E3E80
		public void SpawnLoot()
		{
			if (this.HasContents)
			{
				for (int i = 0; i < base.def.ObjectsToSpawn.Length; i++)
				{
					string name = base.def.ObjectsToSpawn[i];
					GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), 0, 0, name, Grid.SceneLayer.Ore);
					gameObject.SetActive(true);
					if (this.dropSpawnPositions != null && i < this.dropSpawnPositions.Length)
					{
						gameObject.transform.position = this.dropSpawnPositions[i];
					}
				}
				base.smi.sm.WasEmptied.Set(true, base.smi, false);
				this.UpdateContentPreviewSymbols();
			}
		}

		// Token: 0x0600682F RID: 26671 RVA: 0x002E5D2C File Offset: 0x002E3F2C
		public void UpdateContentPreviewSymbols()
		{
			for (int i = 0; i < base.def.LootSymbols.Length; i++)
			{
				this.animController.SetSymbolVisiblity(base.def.LootSymbols[i], false);
			}
			if (this.HasContents)
			{
				for (int j = 0; j < Mathf.Min(base.def.LootSymbols.Length, base.def.ObjectsToSpawn.Length); j++)
				{
					KAnim.Build.Symbol symbolByIndex = Assets.GetPrefab(base.def.ObjectsToSpawn[j]).GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
					SymbolOverrideController component = base.gameObject.GetComponent<SymbolOverrideController>();
					string text = base.def.LootSymbols[j];
					component.AddSymbolOverride(text, symbolByIndex, 0);
					this.animController.SetSymbolVisiblity(text, true);
				}
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06006830 RID: 26672 RVA: 0x002E5E14 File Offset: 0x002E4014
		public string SidescreenButtonText
		{
			get
			{
				if (!this.IsOpen)
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_OpenButtonText;
					}
					return base.def.SideScreen_CancelOpenButtonText;
				}
				else
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_CloseButtonText;
					}
					return base.def.SideScreen_CancelCloseButtonText;
				}
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06006831 RID: 26673 RVA: 0x002E5E68 File Offset: 0x002E4068
		public string SidescreenButtonTooltip
		{
			get
			{
				if (!this.IsOpen)
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_OpenButtonTooltip;
					}
					return base.def.SideScreen_CancelOpenButtonTooltip;
				}
				else
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_CloseButtonTooltip;
					}
					return base.def.SideScreen_CancelCloseButtonTooltip;
				}
			}
		}

		// Token: 0x06006832 RID: 26674 RVA: 0x000E8952 File Offset: 0x000E6B52
		public bool SidescreenEnabled()
		{
			return !this.IsOpen || base.def.CanBeClosed;
		}

		// Token: 0x06006833 RID: 26675 RVA: 0x000E8952 File Offset: 0x000E6B52
		public bool SidescreenButtonInteractable()
		{
			return !this.IsOpen || base.def.CanBeClosed;
		}

		// Token: 0x06006834 RID: 26676 RVA: 0x000B1628 File Offset: 0x000AF828
		public int HorizontalGroupID()
		{
			return 0;
		}

		// Token: 0x06006835 RID: 26677 RVA: 0x000AFED1 File Offset: 0x000AE0D1
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x06006836 RID: 26678 RVA: 0x000AFECA File Offset: 0x000AE0CA
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006837 RID: 26679 RVA: 0x000E8969 File Offset: 0x000E6B69
		public void OnSidescreenButtonPressed()
		{
			base.smi.sm.WorkOrderGiven.Set(!base.smi.sm.WorkOrderGiven.Get(base.smi), base.smi, false);
		}

		// Token: 0x04004EB9 RID: 20153
		[MyCmpGet]
		private Workable workable;

		// Token: 0x04004EBA RID: 20154
		[MyCmpGet]
		private KBatchedAnimController animController;

		// Token: 0x04004EBB RID: 20155
		private Chore chore;

		// Token: 0x04004EBC RID: 20156
		private Vector3[] dropSpawnPositions;
	}
}
