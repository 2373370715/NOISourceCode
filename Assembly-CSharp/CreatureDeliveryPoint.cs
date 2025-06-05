using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000D47 RID: 3399
public class CreatureDeliveryPoint : StateMachineComponent<CreatureDeliveryPoint.SMInstance>
{
	// Token: 0x060041EC RID: 16876 RVA: 0x0024D988 File Offset: 0x0024BB88
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.fetches = new List<FetchOrder2>();
		TreeFilterable component = base.GetComponent<TreeFilterable>();
		component.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(component.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		base.GetComponent<Storage>().SetOffsets(this.deliveryOffsets);
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x060041ED RID: 16877 RVA: 0x0024D9EC File Offset: 0x0024BBEC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		base.Subscribe<CreatureDeliveryPoint>(-905833192, CreatureDeliveryPoint.OnCopySettingsDelegate);
		base.Subscribe<CreatureDeliveryPoint>(643180843, CreatureDeliveryPoint.RefreshCreatureCountDelegate);
		this.critterCapacity = base.GetComponent<BaggableCritterCapacityTracker>();
		BaggableCritterCapacityTracker baggableCritterCapacityTracker = this.critterCapacity;
		baggableCritterCapacityTracker.onCountChanged = (System.Action)Delegate.Combine(baggableCritterCapacityTracker.onCountChanged, new System.Action(this.RebalanceFetches));
		this.critterCapacity.RefreshCreatureCount(null);
		this.logicPorts = base.GetComponent<LogicPorts>();
		if (this.logicPorts != null)
		{
			this.logicPorts.Subscribe(-801688580, new Action<object>(this.OnLogicChanged));
		}
	}

	// Token: 0x060041EE RID: 16878 RVA: 0x0024DAA4 File Offset: 0x0024BCA4
	private void OnLogicChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == "CritterDropOffInput")
		{
			if (logicValueChanged.newValue > 0)
			{
				this.RebalanceFetches();
				return;
			}
			this.ClearFetches();
		}
	}

	// Token: 0x060041EF RID: 16879 RVA: 0x000CF117 File Offset: 0x000CD317
	[Obsolete]
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.critterCapacity != null && this.creatureLimit > 0)
		{
			this.critterCapacity.creatureLimit = this.creatureLimit;
			this.creatureLimit = -1;
		}
	}

	// Token: 0x060041F0 RID: 16880 RVA: 0x0024DAE8 File Offset: 0x0024BCE8
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		if (gameObject.GetComponent<CreatureDeliveryPoint>() == null)
		{
			return;
		}
		this.RebalanceFetches();
	}

	// Token: 0x060041F1 RID: 16881 RVA: 0x000CF148 File Offset: 0x000CD348
	private void OnFilterChanged(HashSet<Tag> tags)
	{
		this.ClearFetches();
		this.RebalanceFetches();
	}

	// Token: 0x060041F2 RID: 16882 RVA: 0x0024DB1C File Offset: 0x0024BD1C
	private void ClearFetches()
	{
		for (int i = this.fetches.Count - 1; i >= 0; i--)
		{
			this.fetches[i].Cancel("clearing all fetches");
		}
		this.fetches.Clear();
	}

	// Token: 0x060041F3 RID: 16883 RVA: 0x0024DB64 File Offset: 0x0024BD64
	private void RebalanceFetches()
	{
		if (!this.LogicEnabled())
		{
			return;
		}
		HashSet<Tag> tags = base.GetComponent<TreeFilterable>().GetTags();
		ChoreType creatureFetch = Db.Get().ChoreTypes.CreatureFetch;
		Storage component = base.GetComponent<Storage>();
		int num = this.critterCapacity.creatureLimit - this.critterCapacity.storedCreatureCount;
		int count = this.fetches.Count;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		for (int i = this.fetches.Count - 1; i >= 0; i--)
		{
			if (this.fetches[i].IsComplete())
			{
				this.fetches.RemoveAt(i);
				num2++;
			}
		}
		int num6 = 0;
		for (int j = 0; j < this.fetches.Count; j++)
		{
			if (!this.fetches[j].InProgress)
			{
				num6++;
			}
		}
		if (num6 == 0 && this.fetches.Count < num)
		{
			FetchOrder2 fetchOrder = new FetchOrder2(creatureFetch, tags, FetchChore.MatchCriteria.MatchID, GameTags.Creatures.Deliverable, null, component, 1f, Operational.State.Operational, 0);
			fetchOrder.validateRequiredTagOnTagChange = true;
			fetchOrder.Submit(new Action<FetchOrder2, Pickupable>(this.OnFetchComplete), false, new Action<FetchOrder2, Pickupable>(this.OnFetchBegun));
			this.fetches.Add(fetchOrder);
			num3++;
		}
		int num7 = this.fetches.Count - num;
		for (int k = this.fetches.Count - 1; k >= 0; k--)
		{
			if (num7 <= 0)
			{
				break;
			}
			if (!this.fetches[k].InProgress)
			{
				this.fetches[k].Cancel("fewer creatures in room");
				this.fetches.RemoveAt(k);
				num7--;
				num4++;
			}
		}
		while (num7 > 0 && this.fetches.Count > 0)
		{
			this.fetches[this.fetches.Count - 1].Cancel("fewer creatures in room");
			this.fetches.RemoveAt(this.fetches.Count - 1);
			num7--;
			num5++;
		}
	}

	// Token: 0x060041F4 RID: 16884 RVA: 0x000CF156 File Offset: 0x000CD356
	private void OnFetchComplete(FetchOrder2 fetchOrder, Pickupable fetchedItem)
	{
		this.RebalanceFetches();
	}

	// Token: 0x060041F5 RID: 16885 RVA: 0x000CF156 File Offset: 0x000CD356
	private void OnFetchBegun(FetchOrder2 fetchOrder, Pickupable fetchedItem)
	{
		this.RebalanceFetches();
	}

	// Token: 0x060041F6 RID: 16886 RVA: 0x000CF15E File Offset: 0x000CD35E
	protected override void OnCleanUp()
	{
		base.smi.StopSM("OnCleanUp");
		TreeFilterable component = base.GetComponent<TreeFilterable>();
		component.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(component.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		base.OnCleanUp();
	}

	// Token: 0x060041F7 RID: 16887 RVA: 0x0024DD7C File Offset: 0x0024BF7C
	public bool LogicEnabled()
	{
		return this.logicPorts == null || !this.logicPorts.IsPortConnected("CritterDropOffInput") || this.logicPorts.GetInputValue("CritterDropOffInput") == 1;
	}

	// Token: 0x04002D74 RID: 11636
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x04002D75 RID: 11637
	[MyCmpReq]
	public BaggableCritterCapacityTracker critterCapacity;

	// Token: 0x04002D76 RID: 11638
	[Obsolete]
	[Serialize]
	private int creatureLimit = 20;

	// Token: 0x04002D77 RID: 11639
	public CellOffset[] deliveryOffsets = new CellOffset[1];

	// Token: 0x04002D78 RID: 11640
	public CellOffset spawnOffset = new CellOffset(0, 0);

	// Token: 0x04002D79 RID: 11641
	private List<FetchOrder2> fetches;

	// Token: 0x04002D7A RID: 11642
	public bool playAnimsOnFetch;

	// Token: 0x04002D7B RID: 11643
	private LogicPorts logicPorts;

	// Token: 0x04002D7C RID: 11644
	private static readonly EventSystem.IntraObjectHandler<CreatureDeliveryPoint> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CreatureDeliveryPoint>(delegate(CreatureDeliveryPoint component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04002D7D RID: 11645
	private static readonly EventSystem.IntraObjectHandler<CreatureDeliveryPoint> RefreshCreatureCountDelegate = new EventSystem.IntraObjectHandler<CreatureDeliveryPoint>(delegate(CreatureDeliveryPoint component, object data)
	{
		component.critterCapacity.RefreshCreatureCount(data);
	});

	// Token: 0x02000D48 RID: 3400
	public class SMInstance : GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.GameInstance
	{
		// Token: 0x060041FA RID: 16890 RVA: 0x000CF1FC File Offset: 0x000CD3FC
		public SMInstance(CreatureDeliveryPoint master) : base(master)
		{
		}
	}

	// Token: 0x02000D49 RID: 3401
	public class States : GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint>
	{
		// Token: 0x060041FB RID: 16891 RVA: 0x0024DDC8 File Offset: 0x0024BFC8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.operational.waiting;
			this.root.Update("RefreshCreatureCount", delegate(CreatureDeliveryPoint.SMInstance smi, float dt)
			{
				smi.master.critterCapacity.RefreshCreatureCount(null);
			}, UpdateRate.SIM_1000ms, false).EventHandler(GameHashes.OnStorageChange, new StateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State.Callback(CreatureDeliveryPoint.States.DropAllCreatures));
			this.unoperational.EventTransition(GameHashes.LogicEvent, this.operational, (CreatureDeliveryPoint.SMInstance smi) => smi.master.LogicEnabled());
			this.operational.EventTransition(GameHashes.LogicEvent, this.unoperational, (CreatureDeliveryPoint.SMInstance smi) => !smi.master.LogicEnabled());
			this.operational.waiting.EnterTransition(this.operational.interact_waiting, (CreatureDeliveryPoint.SMInstance smi) => smi.master.playAnimsOnFetch);
			this.operational.interact_waiting.WorkableStartTransition((CreatureDeliveryPoint.SMInstance smi) => smi.master.GetComponent<Storage>(), this.operational.interact_delivery);
			this.operational.interact_delivery.PlayAnim("working_pre").QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.operational.interact_waiting);
		}

		// Token: 0x060041FC RID: 16892 RVA: 0x0024DF40 File Offset: 0x0024C140
		public static void DropAllCreatures(CreatureDeliveryPoint.SMInstance smi)
		{
			Storage component = smi.master.GetComponent<Storage>();
			if (component.IsEmpty())
			{
				return;
			}
			List<GameObject> items = component.items;
			int count = items.Count;
			Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(smi.transform.GetPosition()), smi.master.spawnOffset), Grid.SceneLayer.Creatures);
			for (int i = count - 1; i >= 0; i--)
			{
				GameObject gameObject = items[i];
				component.Drop(gameObject, true);
				gameObject.transform.SetPosition(position);
				gameObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
			}
			smi.master.critterCapacity.RefreshCreatureCount(null);
		}

		// Token: 0x04002D7E RID: 11646
		public CreatureDeliveryPoint.States.OperationalState operational;

		// Token: 0x04002D7F RID: 11647
		public GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State unoperational;

		// Token: 0x02000D4A RID: 3402
		public class OperationalState : GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State
		{
			// Token: 0x04002D80 RID: 11648
			public GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State waiting;

			// Token: 0x04002D81 RID: 11649
			public GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State interact_waiting;

			// Token: 0x04002D82 RID: 11650
			public GameStateMachine<CreatureDeliveryPoint.States, CreatureDeliveryPoint.SMInstance, CreatureDeliveryPoint, object>.State interact_delivery;
		}
	}
}
