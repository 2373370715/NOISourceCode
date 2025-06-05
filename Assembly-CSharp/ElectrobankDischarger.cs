using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D91 RID: 3473
public class ElectrobankDischarger : Generator
{
	// Token: 0x17000355 RID: 853
	// (get) Token: 0x06004379 RID: 17273 RVA: 0x00252EB0 File Offset: 0x002510B0
	public float ElectrobankJoulesStored
	{
		get
		{
			float num = 0f;
			foreach (Electrobank electrobank in this.storedCells)
			{
				num += electrobank.Charge;
			}
			return num;
		}
	}

	// Token: 0x0600437A RID: 17274 RVA: 0x00252F0C File Offset: 0x0025110C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new ElectrobankDischarger.StatesInstance(this);
		this.smi.StartSM();
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
		base.Subscribe(-592767678, new Action<object>(this.RefreshOperationalActive));
		this.RefreshCells(null);
		this.RefreshOperationalActive(null);
		this.filteredStorage = new FilteredStorage(this, null, null, false, Db.Get().ChoreTypes.PowerFetch);
		this.filteredStorage.SetHasMeter(false);
		this.filteredStorage.FilterChanged();
		Storage storage = this.storage;
		storage.onDestroyItemsDropped = (Action<List<GameObject>>)Delegate.Combine(storage.onDestroyItemsDropped, new Action<List<GameObject>>(this.OnBatteriesDroppedFromDeconstruction));
		this.UpdateSymbolSwap();
	}

	// Token: 0x0600437B RID: 17275 RVA: 0x00252FD8 File Offset: 0x002511D8
	private void OnBatteriesDroppedFromDeconstruction(List<GameObject> items)
	{
		if (items != null)
		{
			for (int i = 0; i < items.Count; i++)
			{
				Electrobank component = items[i].GetComponent<Electrobank>();
				if (component != null && component.HasTag(GameTags.ChargedPortableBattery) && !component.IsFullyCharged)
				{
					component.RemovePower(component.Charge, true);
				}
			}
		}
	}

	// Token: 0x0600437C RID: 17276 RVA: 0x000D00C8 File Offset: 0x000CE2C8
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
		base.OnCleanUp();
	}

	// Token: 0x0600437D RID: 17277 RVA: 0x000D00DB File Offset: 0x000CE2DB
	private void OnStorageChange(object data = null)
	{
		this.RefreshCells(null);
		this.RefreshOperationalActive(null);
		this.UpdateSymbolSwap();
	}

	// Token: 0x0600437E RID: 17278 RVA: 0x00253034 File Offset: 0x00251234
	public void UpdateMeter()
	{
		if (this.meterController == null)
		{
			this.meterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}
		this.meterController.SetPositionPercent(this.smi.master.ElectrobankJoulesStored / 120000f);
	}

	// Token: 0x0600437F RID: 17279 RVA: 0x00253090 File Offset: 0x00251290
	public void UpdateSymbolSwap()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component2 = component.GetComponent<SymbolOverrideController>();
		component.SetSymbolVisiblity("electrobank_l", false);
		if (this.storage.items.Count > 0)
		{
			KAnim.Build.Symbol source_symbol = this.storage.items[0].GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.symbols[0];
			component2.AddSymbolOverride("electrobank_s", source_symbol, 0);
			return;
		}
		component2.RemoveSymbolOverride("electrobank_s", 0);
	}

	// Token: 0x06004380 RID: 17280 RVA: 0x000D00F1 File Offset: 0x000CE2F1
	private void RefreshOperationalActive(object data = null)
	{
		if (this.operational.IsOperational)
		{
			if (this.storedCells.Count > 0)
			{
				this.operational.SetActive(true, false);
				return;
			}
			this.operational.SetActive(false, false);
		}
	}

	// Token: 0x06004381 RID: 17281 RVA: 0x00253124 File Offset: 0x00251324
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsActive)
		{
			return;
		}
		float num = 0f;
		float num2 = Mathf.Min(this.wattageRating * dt, this.Capacity - this.JoulesAvailable);
		for (int i = this.storedCells.Count - 1; i >= 0; i--)
		{
			num += this.storedCells[i].RemovePower(num2 - num, true);
			if (num >= num2)
			{
				break;
			}
		}
		if (num > 0f)
		{
			base.GenerateJoules(num, false);
		}
	}

	// Token: 0x06004382 RID: 17282 RVA: 0x002531D0 File Offset: 0x002513D0
	private void RefreshCells(object data = null)
	{
		this.storedCells.Clear();
		foreach (GameObject gameObject in this.storage.GetItems())
		{
			Electrobank component = gameObject.GetComponent<Electrobank>();
			if (component != null)
			{
				this.storedCells.Add(component);
			}
		}
	}

	// Token: 0x04002EAB RID: 11947
	public float wattageRating;

	// Token: 0x04002EAC RID: 11948
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04002EAD RID: 11949
	private ElectrobankDischarger.StatesInstance smi;

	// Token: 0x04002EAE RID: 11950
	private List<Electrobank> storedCells = new List<Electrobank>();

	// Token: 0x04002EAF RID: 11951
	private MeterController meterController;

	// Token: 0x04002EB0 RID: 11952
	protected FilteredStorage filteredStorage;

	// Token: 0x02000D92 RID: 3474
	public class StatesInstance : GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.GameInstance
	{
		// Token: 0x06004384 RID: 17284 RVA: 0x000D013C File Offset: 0x000CE33C
		public StatesInstance(ElectrobankDischarger master) : base(master)
		{
		}
	}

	// Token: 0x02000D93 RID: 3475
	public class States : GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger>
	{
		// Token: 0x06004385 RID: 17285 RVA: 0x00253248 File Offset: 0x00251448
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.noBattery;
			this.root.EventTransition(GameHashes.ActiveChanged, this.discharging, (ElectrobankDischarger.StatesInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.noBattery.PlayAnim("off").Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			});
			this.inoperational.PlayAnim("on").Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			}).EnterTransition(this.noBattery, (ElectrobankDischarger.StatesInstance smi) => smi.master.storage.items.Count == 0);
			this.discharging.Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			}).EventTransition(GameHashes.ActiveChanged, this.inoperational, (ElectrobankDischarger.StatesInstance smi) => !smi.GetComponent<Operational>().IsActive).QueueAnim("working_pre", false, null).QueueAnim("working_loop", true, null).Update(delegate(ElectrobankDischarger.StatesInstance smi, float dt)
			{
				smi.master.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.ElectrobankJoulesAvailable, smi.master);
				smi.master.UpdateMeter();
			}, UpdateRate.SIM_200ms, false);
			this.discharging_pst.Enter(delegate(ElectrobankDischarger.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			}).PlayAnim("working_pst");
		}

		// Token: 0x04002EB1 RID: 11953
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State noBattery;

		// Token: 0x04002EB2 RID: 11954
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State inoperational;

		// Token: 0x04002EB3 RID: 11955
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State discharging;

		// Token: 0x04002EB4 RID: 11956
		public GameStateMachine<ElectrobankDischarger.States, ElectrobankDischarger.StatesInstance, ElectrobankDischarger, object>.State discharging_pst;
	}
}
