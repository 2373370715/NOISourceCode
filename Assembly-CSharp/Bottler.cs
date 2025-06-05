using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000CE8 RID: 3304
[AddComponentMenu("KMonoBehaviour/Workable/Bottler")]
public class Bottler : Workable, IUserControlledCapacity
{
	// Token: 0x170002FC RID: 764
	// (get) Token: 0x06003F4D RID: 16205 RVA: 0x000CD9E5 File Offset: 0x000CBBE5
	// (set) Token: 0x06003F4E RID: 16206 RVA: 0x000CDA11 File Offset: 0x000CBC11
	public float UserMaxCapacity
	{
		get
		{
			if (this.consumer != null)
			{
				return Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
			}
			return 0f;
		}
		set
		{
			this.userMaxCapacity = value;
			this.SetConsumerCapacity(value);
		}
	}

	// Token: 0x170002FD RID: 765
	// (get) Token: 0x06003F4F RID: 16207 RVA: 0x000CDA21 File Offset: 0x000CBC21
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x06003F50 RID: 16208 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06003F51 RID: 16209 RVA: 0x000CDA2E File Offset: 0x000CBC2E
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x06003F52 RID: 16210 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000301 RID: 769
	// (get) Token: 0x06003F53 RID: 16211 RVA: 0x000CDA3B File Offset: 0x000CBC3B
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x17000302 RID: 770
	// (get) Token: 0x06003F54 RID: 16212 RVA: 0x000CDA43 File Offset: 0x000CBC43
	private Tag SourceTag
	{
		get
		{
			if (this.smi.master.consumer.conduitType != ConduitType.Gas)
			{
				return GameTags.LiquidSource;
			}
			return GameTags.GasSource;
		}
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x06003F55 RID: 16213 RVA: 0x000CDA68 File Offset: 0x000CBC68
	private Tag ElementTag
	{
		get
		{
			if (this.smi.master.consumer.conduitType != ConduitType.Gas)
			{
				return GameTags.Liquid;
			}
			return GameTags.Gas;
		}
	}

	// Token: 0x06003F56 RID: 16214 RVA: 0x00244FA4 File Offset: 0x002431A4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_bottler_kanim")
		};
		this.workAnims = new HashedString[]
		{
			"pick_up"
		};
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.synchronizeAnims = true;
		base.SetOffsets(new CellOffset[]
		{
			this.workCellOffset
		});
		base.SetWorkTime(this.overrideAnims[0].GetData().GetAnim("pick_up").totalTime);
		this.resetProgressOnStop = true;
		this.showProgressBar = false;
	}

	// Token: 0x06003F57 RID: 16215 RVA: 0x00245050 File Offset: 0x00243250
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new Bottler.Controller.Instance(this);
		this.smi.StartSM();
		base.Subscribe<Bottler>(-905833192, Bottler.OnCopySettingsDelegate);
		this.UpdateStoredItemState();
		this.SetConsumerCapacity(this.userMaxCapacity);
	}

	// Token: 0x06003F58 RID: 16216 RVA: 0x002450A0 File Offset: 0x002432A0
	protected override void OnForcedCleanUp()
	{
		if (base.worker != null)
		{
			ChoreDriver component = base.worker.GetComponent<ChoreDriver>();
			if (component != null)
			{
				component.StopChore();
			}
			else
			{
				base.worker.StopWork();
			}
		}
		if (this.workerMeter != null)
		{
			this.CleanupBottleProxyObject();
		}
		base.OnForcedCleanUp();
	}

	// Token: 0x06003F59 RID: 16217 RVA: 0x000CDA8D File Offset: 0x000CBC8D
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.CreateBottleProxyObject(worker);
	}

	// Token: 0x06003F5A RID: 16218 RVA: 0x002450F8 File Offset: 0x002432F8
	private void CreateBottleProxyObject(WorkerBase worker)
	{
		if (this.workerMeter != null)
		{
			this.CleanupBottleProxyObject();
			KCrashReporter.ReportDevNotification("CreateBottleProxyObject called before cleanup", Environment.StackTrace, "", false, null);
		}
		PrimaryElement firstPrimaryElement = this.smi.master.GetFirstPrimaryElement();
		if (firstPrimaryElement == null)
		{
			KCrashReporter.ReportDevNotification("CreateBottleProxyObject on a null element", Environment.StackTrace, "", false, null);
			return;
		}
		this.workerMeter = new MeterController(worker.GetComponent<KBatchedAnimController>(), "snapto_chest", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"snapto_chest"
		});
		this.workerMeter.meterController.SwapAnims(firstPrimaryElement.Element.substance.anims);
		this.workerMeter.meterController.Play("empty", KAnim.PlayMode.Paused, 1f, 0f);
		Color32 colour = firstPrimaryElement.Element.substance.colour;
		colour.a = byte.MaxValue;
		this.workerMeter.SetSymbolTint(new KAnimHashedString("meter_fill"), colour);
		this.workerMeter.SetSymbolTint(new KAnimHashedString("water1"), colour);
		this.workerMeter.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
		this.workerMeter.SetSymbolTint(new KAnimHashedString("substance_tinter_cap"), colour);
	}

	// Token: 0x06003F5B RID: 16219 RVA: 0x00245240 File Offset: 0x00243440
	private void CleanupBottleProxyObject()
	{
		if (this.workerMeter != null && !this.workerMeter.gameObject.IsNullOrDestroyed())
		{
			this.workerMeter.Unlink();
			this.workerMeter.gameObject.DeleteObject();
		}
		else
		{
			string str = "Bottler finished work but could not clean up the proxy bottle object. workerMeter=";
			MeterController meterController = this.workerMeter;
			DebugUtil.DevLogError(str + ((meterController != null) ? meterController.ToString() : null));
			KCrashReporter.ReportDevNotification("Bottle emptier could not clean up proxy object", Environment.StackTrace, "", false, null);
		}
		this.workerMeter = null;
	}

	// Token: 0x06003F5C RID: 16220 RVA: 0x000CDA9D File Offset: 0x000CBC9D
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.CleanupBottleProxyObject();
	}

	// Token: 0x06003F5D RID: 16221 RVA: 0x000CDAAC File Offset: 0x000CBCAC
	protected override void OnAbortWork(WorkerBase worker)
	{
		base.OnAbortWork(worker);
		this.GetAnimController().Play("ready", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06003F5E RID: 16222 RVA: 0x002452C4 File Offset: 0x002434C4
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = worker.GetComponent<Storage>();
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		if (pickupableStartWorkInfo.amount > 0f)
		{
			this.storage.TransferMass(component, pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID(), pickupableStartWorkInfo.amount, false, false, false);
		}
		GameObject gameObject = component.FindFirst(pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID());
		if (gameObject != null)
		{
			Pickupable component2 = gameObject.GetComponent<Pickupable>();
			component2.targetWorkable = component2;
			component2.RemoveTag(this.SourceTag);
			FetchableMonitor.Instance instance = component2.GetSMI<FetchableMonitor.Instance>();
			if (instance != null)
			{
				instance.SetForceUnfetchable(false);
			}
			pickupableStartWorkInfo.setResultCb(gameObject);
		}
		else
		{
			pickupableStartWorkInfo.setResultCb(null);
		}
		base.OnCompleteWork(worker);
	}

	// Token: 0x06003F5F RID: 16223 RVA: 0x00245384 File Offset: 0x00243584
	private void OnReservationsChanged(Pickupable _ignore, bool _ignore2, Pickupable.Reservation _ignore3)
	{
		bool forceUnfetchable = false;
		using (List<GameObject>.Enumerator enumerator = this.storage.items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetComponent<Pickupable>().ReservedAmount > 0f)
				{
					forceUnfetchable = true;
					break;
				}
			}
		}
		foreach (GameObject go in this.storage.items)
		{
			FetchableMonitor.Instance instance = go.GetSMI<FetchableMonitor.Instance>();
			if (instance != null)
			{
				instance.SetForceUnfetchable(forceUnfetchable);
			}
		}
	}

	// Token: 0x06003F60 RID: 16224 RVA: 0x0024543C File Offset: 0x0024363C
	private void SetConsumerCapacity(float value)
	{
		if (this.consumer != null)
		{
			this.consumer.capacityKG = value;
			float num = this.storage.MassStored() - this.userMaxCapacity;
			if (num > 0f)
			{
				this.storage.DropSome(this.storage.FindFirstWithMass(this.smi.master.ElementTag, 0f).ElementID.CreateTag(), num, false, false, new Vector3(0.8f, 0f, 0f), true, false);
			}
		}
	}

	// Token: 0x06003F61 RID: 16225 RVA: 0x000CDAD5 File Offset: 0x000CBCD5
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("OnCleanUp");
		}
		base.OnCleanUp();
	}

	// Token: 0x06003F62 RID: 16226 RVA: 0x002454D0 File Offset: 0x002436D0
	private PrimaryElement GetFirstPrimaryElement()
	{
		for (int i = 0; i < this.storage.Count; i++)
		{
			GameObject gameObject = this.storage[i];
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null))
				{
					return component;
				}
			}
		}
		return null;
	}

	// Token: 0x06003F63 RID: 16227 RVA: 0x0024551C File Offset: 0x0024371C
	private void UpdateStoredItemState()
	{
		this.storage.allowItemRemoval = (this.smi != null && this.smi.GetCurrentState() == this.smi.sm.ready);
		foreach (GameObject gameObject in this.storage.items)
		{
			if (gameObject != null)
			{
				gameObject.Trigger(-778359855, this.storage);
			}
		}
	}

	// Token: 0x06003F64 RID: 16228 RVA: 0x002455BC File Offset: 0x002437BC
	private void OnCopySettings(object data)
	{
		Bottler component = ((GameObject)data).GetComponent<Bottler>();
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x04002BCA RID: 11210
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002BCB RID: 11211
	public Storage storage;

	// Token: 0x04002BCC RID: 11212
	public ConduitConsumer consumer;

	// Token: 0x04002BCD RID: 11213
	public CellOffset workCellOffset = new CellOffset(0, 0);

	// Token: 0x04002BCE RID: 11214
	[Serialize]
	public float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04002BCF RID: 11215
	private Bottler.Controller.Instance smi;

	// Token: 0x04002BD0 RID: 11216
	private int storageHandle;

	// Token: 0x04002BD1 RID: 11217
	private MeterController workerMeter;

	// Token: 0x04002BD2 RID: 11218
	private static readonly EventSystem.IntraObjectHandler<Bottler> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Bottler>(delegate(Bottler component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02000CE9 RID: 3305
	private class Controller : GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler>
	{
		// Token: 0x06003F67 RID: 16231 RVA: 0x002455E4 File Offset: 0x002437E4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.PlayAnim("off").EventHandlerTransition(GameHashes.OnStorageChange, this.filling, (Bottler.Controller.Instance smi, object o) => Bottler.Controller.IsFull(smi)).EnterTransition(this.ready, new StateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.Transition.ConditionCallback(Bottler.Controller.IsFull));
			this.filling.PlayAnim("working").Enter(delegate(Bottler.Controller.Instance smi)
			{
				smi.UpdateMeter();
			}).OnAnimQueueComplete(this.ready);
			this.ready.EventTransition(GameHashes.OnStorageChange, this.empty, GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.Not(new StateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.Transition.ConditionCallback(Bottler.Controller.IsFull))).PlayAnim("ready").Enter(delegate(Bottler.Controller.Instance smi)
			{
				smi.master.storage.allowItemRemoval = true;
				smi.UpdateMeter();
				foreach (GameObject gameObject in smi.master.storage.items)
				{
					Pickupable component = gameObject.GetComponent<Pickupable>();
					component.targetWorkable = smi.master;
					component.SetOffsets(new CellOffset[]
					{
						smi.master.workCellOffset
					});
					Pickupable pickupable = component;
					pickupable.OnReservationsChanged = (Action<Pickupable, bool, Pickupable.Reservation>)Delegate.Combine(pickupable.OnReservationsChanged, new Action<Pickupable, bool, Pickupable.Reservation>(smi.master.OnReservationsChanged));
					component.KPrefabID.AddTag(smi.master.SourceTag, false);
					gameObject.Trigger(-778359855, smi.master.storage);
				}
			}).Exit(delegate(Bottler.Controller.Instance smi)
			{
				smi.master.storage.allowItemRemoval = false;
				foreach (GameObject gameObject in smi.master.storage.items)
				{
					Pickupable component = gameObject.GetComponent<Pickupable>();
					component.targetWorkable = component;
					component.SetOffsetTable(OffsetGroups.InvertedStandardTable);
					component.OnReservationsChanged = (Action<Pickupable, bool, Pickupable.Reservation>)Delegate.Remove(component.OnReservationsChanged, new Action<Pickupable, bool, Pickupable.Reservation>(smi.master.OnReservationsChanged));
					component.KPrefabID.RemoveTag(smi.master.SourceTag);
					FetchableMonitor.Instance smi2 = component.GetSMI<FetchableMonitor.Instance>();
					if (smi2 != null)
					{
						smi2.SetForceUnfetchable(false);
					}
					gameObject.Trigger(-778359855, smi.master.storage);
				}
			});
		}

		// Token: 0x06003F68 RID: 16232 RVA: 0x000CDB31 File Offset: 0x000CBD31
		public static bool IsFull(Bottler.Controller.Instance smi)
		{
			return smi.master.storage.MassStored() >= smi.master.userMaxCapacity;
		}

		// Token: 0x04002BD3 RID: 11219
		public GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.State empty;

		// Token: 0x04002BD4 RID: 11220
		public GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.State filling;

		// Token: 0x04002BD5 RID: 11221
		public GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.State ready;

		// Token: 0x02000CEA RID: 3306
		public new class Instance : GameStateMachine<Bottler.Controller, Bottler.Controller.Instance, Bottler, object>.GameInstance
		{
			// Token: 0x17000304 RID: 772
			// (get) Token: 0x06003F6A RID: 16234 RVA: 0x000CDB5B File Offset: 0x000CBD5B
			// (set) Token: 0x06003F6B RID: 16235 RVA: 0x000CDB63 File Offset: 0x000CBD63
			public MeterController meter { get; private set; }

			// Token: 0x06003F6C RID: 16236 RVA: 0x0024570C File Offset: 0x0024390C
			public Instance(Bottler master) : base(master)
			{
				this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "bottle", "off", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, new string[]
				{
					"bottle",
					"substance_tinter",
					"substance_tinter_cap"
				});
			}

			// Token: 0x06003F6D RID: 16237 RVA: 0x0024575C File Offset: 0x0024395C
			public void UpdateMeter()
			{
				PrimaryElement firstPrimaryElement = base.smi.master.GetFirstPrimaryElement();
				if (firstPrimaryElement == null)
				{
					return;
				}
				this.meter.meterController.SwapAnims(firstPrimaryElement.Element.substance.anims);
				this.meter.meterController.Play(OreSizeVisualizerComponents.GetAnimForMass(firstPrimaryElement.Mass), KAnim.PlayMode.Paused, 1f, 0f);
				Color32 colour = firstPrimaryElement.Element.substance.colour;
				colour.a = byte.MaxValue;
				this.meter.SetSymbolTint(new KAnimHashedString("meter_fill"), colour);
				this.meter.SetSymbolTint(new KAnimHashedString("water1"), colour);
				this.meter.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
				this.meter.SetSymbolTint(new KAnimHashedString("substance_tinter_cap"), colour);
			}
		}
	}
}
