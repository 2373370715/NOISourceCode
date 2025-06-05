using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020018B4 RID: 6324
public class SetLocker : StateMachineComponent<SetLocker.StatesInstance>, ISidescreenButtonControl
{
	// Token: 0x0600829B RID: 33435 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600829C RID: 33436 RVA: 0x000FA669 File Offset: 0x000F8869
	public void ChooseContents()
	{
		this.contents = this.possible_contents_ids[UnityEngine.Random.Range(0, this.possible_contents_ids.GetLength(0))];
	}

	// Token: 0x0600829D RID: 33437 RVA: 0x0034B0B8 File Offset: 0x003492B8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.contents == null)
		{
			this.ChooseContents();
		}
		else
		{
			string[] array = this.contents;
			for (int i = 0; i < array.Length; i++)
			{
				if (Assets.GetPrefab(array[i]) == null)
				{
					this.ChooseContents();
					break;
				}
			}
		}
		if (this.pendingRummage)
		{
			this.ActivateChore(null);
		}
	}

	// Token: 0x0600829E RID: 33438 RVA: 0x0034B128 File Offset: 0x00349328
	public void DropContents()
	{
		if (this.contents == null)
		{
			return;
		}
		if (DlcManager.IsExpansion1Active() && this.numDataBanks.Length >= 2)
		{
			int num = UnityEngine.Random.Range(this.numDataBanks[0], this.numDataBanks[1]);
			for (int i = 0; i <= num; i++)
			{
				Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), this.dropOffset.x, this.dropOffset.y, "OrbitalResearchDatabank", Grid.SceneLayer.Front).SetActive(true);
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab("OrbitalResearchDatabank".ToTag()).GetProperName(), base.smi.master.transform, 1.5f, false);
			}
		}
		for (int j = 0; j < this.contents.Length; j++)
		{
			GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), this.dropOffset.x, this.dropOffset.y, this.contents[j], Grid.SceneLayer.Front);
			if (gameObject != null)
			{
				gameObject.SetActive(true);
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab(this.contents[j].ToTag()).GetProperName(), base.smi.master.transform, 1.5f, false);
			}
		}
		base.gameObject.Trigger(-372600542, this);
	}

	// Token: 0x0600829F RID: 33439 RVA: 0x000FA68A File Offset: 0x000F888A
	private void OnClickOpen()
	{
		this.ActivateChore(null);
	}

	// Token: 0x060082A0 RID: 33440 RVA: 0x000FA693 File Offset: 0x000F8893
	private void OnClickCancel()
	{
		this.CancelChore(null);
	}

	// Token: 0x060082A1 RID: 33441 RVA: 0x0034B298 File Offset: 0x00349498
	public void ActivateChore(object param = null)
	{
		if (this.chore != null)
		{
			return;
		}
		Prioritizable.AddRef(base.gameObject);
		base.Trigger(1980521255, null);
		this.pendingRummage = true;
		base.GetComponent<Workable>().SetWorkTime(1.5f);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, false, true, Assets.GetAnim(this.overrideAnim), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x060082A2 RID: 33442 RVA: 0x000FA69C File Offset: 0x000F889C
	public void CancelChore(object param = null)
	{
		if (this.chore == null)
		{
			return;
		}
		this.pendingRummage = false;
		Prioritizable.RemoveRef(base.gameObject);
		base.Trigger(1980521255, null);
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x060082A3 RID: 33443 RVA: 0x0034B324 File Offset: 0x00349524
	private void CompleteChore()
	{
		this.used = true;
		base.smi.GoTo(base.smi.sm.open);
		this.chore = null;
		this.pendingRummage = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x17000850 RID: 2128
	// (get) Token: 0x060082A4 RID: 33444 RVA: 0x000FA6DC File Offset: 0x000F88DC
	public string SidescreenButtonText
	{
		get
		{
			return (this.chore == null) ? UI.USERMENUACTIONS.OPENPOI.NAME : UI.USERMENUACTIONS.OPENPOI.NAME_OFF;
		}
	}

	// Token: 0x17000851 RID: 2129
	// (get) Token: 0x060082A5 RID: 33445 RVA: 0x000FA6F7 File Offset: 0x000F88F7
	public string SidescreenButtonTooltip
	{
		get
		{
			return (this.chore == null) ? UI.USERMENUACTIONS.OPENPOI.TOOLTIP : UI.USERMENUACTIONS.OPENPOI.TOOLTIP_OFF;
		}
	}

	// Token: 0x060082A6 RID: 33446 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x060082A7 RID: 33447 RVA: 0x000AFE89 File Offset: 0x000AE089
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x060082A8 RID: 33448 RVA: 0x000FA712 File Offset: 0x000F8912
	public void OnSidescreenButtonPressed()
	{
		if (this.chore == null)
		{
			this.OnClickOpen();
			return;
		}
		this.OnClickCancel();
	}

	// Token: 0x060082A9 RID: 33449 RVA: 0x000FA729 File Offset: 0x000F8929
	public bool SidescreenButtonInteractable()
	{
		return !this.used;
	}

	// Token: 0x060082AA RID: 33450 RVA: 0x000AFED1 File Offset: 0x000AE0D1
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x060082AB RID: 33451 RVA: 0x000AFECA File Offset: 0x000AE0CA
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x04006360 RID: 25440
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x04006361 RID: 25441
	public string[][] possible_contents_ids;

	// Token: 0x04006362 RID: 25442
	public string machineSound;

	// Token: 0x04006363 RID: 25443
	public string overrideAnim;

	// Token: 0x04006364 RID: 25444
	public Vector2I dropOffset = Vector2I.zero;

	// Token: 0x04006365 RID: 25445
	public int[] numDataBanks;

	// Token: 0x04006366 RID: 25446
	[Serialize]
	private string[] contents;

	// Token: 0x04006367 RID: 25447
	public bool dropOnDeconstruct;

	// Token: 0x04006368 RID: 25448
	[Serialize]
	private bool pendingRummage;

	// Token: 0x04006369 RID: 25449
	[Serialize]
	private bool used;

	// Token: 0x0400636A RID: 25450
	private Chore chore;

	// Token: 0x020018B5 RID: 6325
	public class StatesInstance : GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.GameInstance
	{
		// Token: 0x060082AE RID: 33454 RVA: 0x000FA74F File Offset: 0x000F894F
		public StatesInstance(SetLocker master) : base(master)
		{
		}

		// Token: 0x060082AF RID: 33455 RVA: 0x000FA758 File Offset: 0x000F8958
		public override void StartSM()
		{
			base.StartSM();
			base.smi.Subscribe(-702296337, delegate(object o)
			{
				if (base.smi.master.dropOnDeconstruct && base.smi.IsInsideState(base.smi.sm.closed))
				{
					base.smi.master.DropContents();
				}
			});
		}
	}

	// Token: 0x020018B6 RID: 6326
	public class States : GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker>
	{
		// Token: 0x060082B1 RID: 33457 RVA: 0x0034B3D0 File Offset: 0x003495D0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.closed;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.closed.PlayAnim("on").Enter(delegate(SetLocker.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StartSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
			this.open.PlayAnim("working_pre").QueueAnim("working_loop", false, null).QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.off).Exit(delegate(SetLocker.StatesInstance smi)
			{
				smi.master.DropContents();
			});
			this.off.PlayAnim("off").Enter(delegate(SetLocker.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StopSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
		}

		// Token: 0x0400636B RID: 25451
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State closed;

		// Token: 0x0400636C RID: 25452
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State open;

		// Token: 0x0400636D RID: 25453
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State off;
	}
}
