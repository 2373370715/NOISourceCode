using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000AF7 RID: 2807
[AddComponentMenu("KMonoBehaviour/Workable/Pickupable")]
public class Pickupable : Workable, IHasSortOrder
{
	// Token: 0x17000227 RID: 551
	// (get) Token: 0x060033B4 RID: 13236 RVA: 0x000C639C File Offset: 0x000C459C
	public PrimaryElement PrimaryElement
	{
		get
		{
			return this.primaryElement;
		}
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x060033B5 RID: 13237 RVA: 0x000C63A4 File Offset: 0x000C45A4
	// (set) Token: 0x060033B6 RID: 13238 RVA: 0x000C63AC File Offset: 0x000C45AC
	public int sortOrder
	{
		get
		{
			return this._sortOrder;
		}
		set
		{
			this._sortOrder = value;
		}
	}

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x060033B7 RID: 13239 RVA: 0x000C63B5 File Offset: 0x000C45B5
	// (set) Token: 0x060033B8 RID: 13240 RVA: 0x000C63BD File Offset: 0x000C45BD
	public Storage storage { get; set; }

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x060033B9 RID: 13241 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float MinTakeAmount
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060033BA RID: 13242 RVA: 0x000C63C6 File Offset: 0x000C45C6
	public bool isChoreAllowedToPickup(ChoreType choreType)
	{
		return this.allowedChoreTypes == null || this.allowedChoreTypes.Contains(choreType);
	}

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x060033BB RID: 13243 RVA: 0x000C63DE File Offset: 0x000C45DE
	// (set) Token: 0x060033BC RID: 13244 RVA: 0x000C63E6 File Offset: 0x000C45E6
	public bool prevent_absorb_until_stored { get; set; }

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x060033BD RID: 13245 RVA: 0x000C63EF File Offset: 0x000C45EF
	// (set) Token: 0x060033BE RID: 13246 RVA: 0x000C63F7 File Offset: 0x000C45F7
	public bool isKinematic { get; set; }

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x060033BF RID: 13247 RVA: 0x000C6400 File Offset: 0x000C4600
	// (set) Token: 0x060033C0 RID: 13248 RVA: 0x000C6408 File Offset: 0x000C4608
	public bool wasAbsorbed { get; private set; }

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x060033C1 RID: 13249 RVA: 0x000C6411 File Offset: 0x000C4611
	// (set) Token: 0x060033C2 RID: 13250 RVA: 0x000C6419 File Offset: 0x000C4619
	public int cachedCell { get; private set; }

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x060033C3 RID: 13251 RVA: 0x000C6422 File Offset: 0x000C4622
	// (set) Token: 0x060033C4 RID: 13252 RVA: 0x0021455C File Offset: 0x0021275C
	public bool IsEntombed
	{
		get
		{
			return this.isEntombed;
		}
		set
		{
			if (value != this.isEntombed)
			{
				this.isEntombed = value;
				if (this.isEntombed)
				{
					base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
				}
				else
				{
					base.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
				}
				base.Trigger(-1089732772, null);
				this.UpdateEntombedVisualizer();
			}
		}
	}

	// Token: 0x060033C5 RID: 13253 RVA: 0x000C642A File Offset: 0x000C462A
	[Obsolete("Use Instance ID")]
	private bool CouldBePickedUpCommon(GameObject carrier)
	{
		return this.CouldBePickedUpCommon(carrier.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x060033C6 RID: 13254 RVA: 0x000C643D File Offset: 0x000C463D
	private bool CouldBePickedUpCommon(int carrierID)
	{
		return this.UnreservedAmount >= this.MinTakeAmount && (this.UnreservedAmount > 0f || this.FindReservedAmount(carrierID) > 0f);
	}

	// Token: 0x060033C7 RID: 13255 RVA: 0x000C646C File Offset: 0x000C466C
	[Obsolete("Use Instance ID")]
	public bool CouldBePickedUpByMinion(GameObject carrier)
	{
		return this.CouldBePickedUpByMinion(carrier.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x060033C8 RID: 13256 RVA: 0x002145B8 File Offset: 0x002127B8
	public bool CouldBePickedUpByMinion(int carrierID)
	{
		return this.CouldBePickedUpCommon(carrierID) && (this.storage == null || !this.storage.automatable || !this.storage.automatable.GetAutomationOnly());
	}

	// Token: 0x060033C9 RID: 13257 RVA: 0x000C647F File Offset: 0x000C467F
	[Obsolete("Use Instance ID")]
	public bool CouldBePickedUpByTransferArm(GameObject carrier)
	{
		return this.CouldBePickedUpByTransferArm(carrier.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x060033CA RID: 13258 RVA: 0x000C6492 File Offset: 0x000C4692
	public bool CouldBePickedUpByTransferArm(int carrierID)
	{
		return this.CouldBePickedUpCommon(carrierID) && (this.fetchable_monitor == null || this.fetchable_monitor.IsFetchable());
	}

	// Token: 0x060033CB RID: 13259 RVA: 0x000C64B4 File Offset: 0x000C46B4
	[Obsolete("Use Instance ID")]
	public float FindReservedAmount(GameObject reserver)
	{
		return this.FindReservedAmount(reserver.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x060033CC RID: 13260 RVA: 0x00214608 File Offset: 0x00212808
	public float FindReservedAmount(int reserverID)
	{
		for (int i = 0; i < this.reservations.Count; i++)
		{
			if (this.reservations[i].reserverID == reserverID)
			{
				return this.reservations[i].amount;
			}
		}
		return 0f;
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x060033CD RID: 13261 RVA: 0x000C64C7 File Offset: 0x000C46C7
	public float UnreservedAmount
	{
		get
		{
			return this.TotalAmount - this.ReservedAmount;
		}
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x060033CE RID: 13262 RVA: 0x000C64D6 File Offset: 0x000C46D6
	// (set) Token: 0x060033CF RID: 13263 RVA: 0x000C64DE File Offset: 0x000C46DE
	public float ReservedAmount { get; private set; }

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x060033D0 RID: 13264 RVA: 0x000C64E7 File Offset: 0x000C46E7
	// (set) Token: 0x060033D1 RID: 13265 RVA: 0x00214658 File Offset: 0x00212858
	public float TotalAmount
	{
		get
		{
			return this.primaryElement.Units;
		}
		set
		{
			DebugUtil.Assert(this.primaryElement != null);
			this.primaryElement.Units = value;
			if (value < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT && !this.primaryElement.KeepZeroMassObject)
			{
				base.gameObject.DeleteObject();
			}
			this.NotifyChanged(Grid.PosToCell(this));
		}
	}

	// Token: 0x060033D2 RID: 13266 RVA: 0x002146B0 File Offset: 0x002128B0
	private void RefreshReservedAmount()
	{
		this.ReservedAmount = 0f;
		for (int i = 0; i < this.reservations.Count; i++)
		{
			this.ReservedAmount += this.reservations[i].amount;
		}
	}

	// Token: 0x060033D3 RID: 13267 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("UNITY_EDITOR")]
	private void Log(string evt, string param, float value)
	{
	}

	// Token: 0x060033D4 RID: 13268 RVA: 0x000C64F4 File Offset: 0x000C46F4
	public void ClearReservations()
	{
		this.reservations.Clear();
		this.RefreshReservedAmount();
	}

	// Token: 0x060033D5 RID: 13269 RVA: 0x002146FC File Offset: 0x002128FC
	[ContextMenu("Print Reservations")]
	public void PrintReservations()
	{
		foreach (Pickupable.Reservation reservation in this.reservations)
		{
			global::Debug.Log(reservation.ToString());
		}
	}

	// Token: 0x060033D6 RID: 13270 RVA: 0x0021475C File Offset: 0x0021295C
	public int Reserve(string context, int reserverID, float amount)
	{
		int num = this.nextTicketNumber;
		this.nextTicketNumber = num + 1;
		int num2 = num;
		Pickupable.Reservation reservation = new Pickupable.Reservation(reserverID, amount, num2);
		this.reservations.Add(reservation);
		this.RefreshReservedAmount();
		if (this.OnReservationsChanged != null)
		{
			this.OnReservationsChanged(this, true, reservation);
		}
		return num2;
	}

	// Token: 0x060033D7 RID: 13271 RVA: 0x002147B0 File Offset: 0x002129B0
	public void Unreserve(string context, int ticket)
	{
		int i = 0;
		while (i < this.reservations.Count)
		{
			if (this.reservations[i].ticket == ticket)
			{
				Pickupable.Reservation arg = this.reservations[i];
				this.reservations.RemoveAt(i);
				this.RefreshReservedAmount();
				if (this.OnReservationsChanged != null)
				{
					this.OnReservationsChanged(this, false, arg);
					return;
				}
				break;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x060033D8 RID: 13272 RVA: 0x00214820 File Offset: 0x00212A20
	private Pickupable()
	{
		this.showProgressBar = false;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.shouldTransferDiseaseWithWorker = false;
	}

	// Token: 0x060033D9 RID: 13273 RVA: 0x002148A0 File Offset: 0x00212AA0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.log = new LoggerFSSF("Pickupable");
		this.workerStatusItem = Db.Get().DuplicantStatusItems.PickingUp;
		base.SetWorkTime(1.5f);
		this.targetWorkable = this;
		this.resetProgressOnStop = true;
		base.gameObject.layer = Game.PickupableLayer;
		Vector3 position = base.transform.GetPosition();
		this.UpdateCachedCell(Grid.PosToCell(position));
		base.Subscribe<Pickupable>(856640610, Pickupable.OnStoreDelegate);
		base.Subscribe<Pickupable>(1188683690, Pickupable.OnLandedDelegate);
		base.Subscribe<Pickupable>(1807976145, Pickupable.OnOreSizeChangedDelegate);
		base.Subscribe<Pickupable>(-1432940121, Pickupable.OnReachableChangedDelegate);
		base.Subscribe<Pickupable>(-778359855, Pickupable.RefreshStorageTagsDelegate);
		base.Subscribe<Pickupable>(580035959, Pickupable.OnWorkableEntombOffset);
		this.KPrefabID.AddTag(GameTags.Pickupable, false);
		Components.Pickupables.Add(this);
	}

	// Token: 0x060033DA RID: 13274 RVA: 0x000C6507 File Offset: 0x000C4707
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
	}

	// Token: 0x060033DB RID: 13275 RVA: 0x002149AC File Offset: 0x00212BAC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int num = Grid.PosToCell(this);
		if (!Grid.IsValidCell(num) && this.deleteOffGrid)
		{
			base.gameObject.DeleteObject();
			return;
		}
		if (base.GetComponent<Health>() != null)
		{
			this.handleFallerComponents = false;
		}
		this.UpdateCachedCell(num);
		new ReachabilityMonitor.Instance(this).StartSM();
		this.fetchable_monitor = new FetchableMonitor.Instance(this);
		this.fetchable_monitor.StartSM();
		base.SetWorkTime(1.5f);
		this.faceTargetWhenWorking = true;
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.SetStatusIndicatorOffset(new Vector3(0f, -0.65f, 0f));
		}
		this.OnTagsChanged(null);
		this.TryToOffsetIfBuried(CellOffset.none);
		DecorProvider component2 = base.GetComponent<DecorProvider>();
		if (component2 != null && string.IsNullOrEmpty(component2.overrideName))
		{
			component2.overrideName = UI.OVERLAYS.DECOR.CLUTTER;
		}
		this.UpdateEntombedVisualizer();
		base.Subscribe<Pickupable>(-1582839653, Pickupable.OnTagsChangedDelegate);
		this.NotifyChanged(num);
	}

	// Token: 0x060033DC RID: 13276 RVA: 0x00214ABC File Offset: 0x00212CBC
	[OnDeserialized]
	public void OnDeserialize()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 28) && base.transform.position.z == 0f)
		{
			KBatchedAnimController component = base.transform.GetComponent<KBatchedAnimController>();
			component.SetSceneLayer(component.sceneLayer);
		}
	}

	// Token: 0x060033DD RID: 13277 RVA: 0x00214B10 File Offset: 0x00212D10
	public void UpdateListeners(bool worldSpace)
	{
		if (this.cleaningUp)
		{
			return;
		}
		int num = Grid.PosToCell(this);
		if (worldSpace)
		{
			if (this.solidPartitionerEntry.IsValid())
			{
				return;
			}
			GameScenePartitioner.Instance.Free(ref this.storedPartitionerEntry);
			this.objectLayerListItem = new ObjectLayerListItem(base.gameObject, ObjectLayer.Pickupables, num);
			this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterSolidListener", base.gameObject, num, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
			this.worldPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterPickupable", this, num, GameScenePartitioner.Instance.pickupablesLayer, null);
			Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "Pickupable.OnCellChange");
			Singleton<CellChangeMonitor>.Instance.MarkDirty(base.transform);
			Singleton<CellChangeMonitor>.Instance.ClearLastKnownCell(base.transform);
			return;
		}
		else
		{
			if (this.storedPartitionerEntry.IsValid())
			{
				return;
			}
			this.storedPartitionerEntry = GameScenePartitioner.Instance.Add("Pickupable.RegisterStoredPickupable", this, num, GameScenePartitioner.Instance.storedPickupablesLayer, null);
			if (this.objectLayerListItem != null)
			{
				this.objectLayerListItem.Clear();
				this.objectLayerListItem = null;
			}
			GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
			GameScenePartitioner.Instance.Free(ref this.worldPartitionerEntry);
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
			return;
		}
	}

	// Token: 0x060033DE RID: 13278 RVA: 0x000C650F File Offset: 0x000C470F
	public void RegisterListeners()
	{
		this.UpdateListeners(true);
	}

	// Token: 0x060033DF RID: 13279 RVA: 0x00214C84 File Offset: 0x00212E84
	public void UnregisterListeners()
	{
		if (this.objectLayerListItem != null)
		{
			this.objectLayerListItem.Clear();
			this.objectLayerListItem = null;
		}
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.worldPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.storedPartitionerEntry);
		base.Unsubscribe<Pickupable>(856640610, Pickupable.OnStoreDelegate, false);
		base.Unsubscribe<Pickupable>(1188683690, Pickupable.OnLandedDelegate, false);
		base.Unsubscribe<Pickupable>(1807976145, Pickupable.OnOreSizeChangedDelegate, false);
		base.Unsubscribe<Pickupable>(-1432940121, Pickupable.OnReachableChangedDelegate, false);
		base.Unsubscribe<Pickupable>(-778359855, Pickupable.RefreshStorageTagsDelegate, false);
		base.Unsubscribe<Pickupable>(580035959, Pickupable.OnWorkableEntombOffset, false);
		if (base.isSpawned)
		{
			base.Unsubscribe<Pickupable>(-1582839653, Pickupable.OnTagsChangedDelegate, false);
		}
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
	}

	// Token: 0x060033E0 RID: 13280 RVA: 0x000C6518 File Offset: 0x000C4718
	private void OnSolidChanged(object data)
	{
		this.TryToOffsetIfBuried(CellOffset.none);
	}

	// Token: 0x060033E1 RID: 13281 RVA: 0x00214D78 File Offset: 0x00212F78
	private void SetWorkableOffset(object data)
	{
		CellOffset offset = CellOffset.none;
		WorkerBase workerBase = data as WorkerBase;
		if (workerBase != null)
		{
			int num = Grid.PosToCell(workerBase);
			int base_cell = Grid.PosToCell(this);
			offset = (Grid.IsValidCell(num) ? Grid.GetCellOffsetDirection(base_cell, num) : CellOffset.none);
		}
		this.TryToOffsetIfBuried(offset);
	}

	// Token: 0x060033E2 RID: 13282 RVA: 0x00214DC8 File Offset: 0x00212FC8
	private CellOffset[] GetPreferedOffsets(CellOffset preferedDirectionOffset)
	{
		if (preferedDirectionOffset == CellOffset.left || preferedDirectionOffset == CellOffset.leftup)
		{
			return new CellOffset[]
			{
				CellOffset.up,
				CellOffset.left,
				CellOffset.leftup
			};
		}
		if (preferedDirectionOffset == CellOffset.right || preferedDirectionOffset == CellOffset.rightup)
		{
			return new CellOffset[]
			{
				CellOffset.up,
				CellOffset.right,
				CellOffset.rightup
			};
		}
		if (preferedDirectionOffset == CellOffset.up)
		{
			return new CellOffset[]
			{
				CellOffset.up,
				CellOffset.rightup,
				CellOffset.leftup
			};
		}
		if (preferedDirectionOffset == CellOffset.leftdown)
		{
			return new CellOffset[]
			{
				CellOffset.down,
				CellOffset.leftdown,
				CellOffset.left
			};
		}
		if (preferedDirectionOffset == CellOffset.rightdown)
		{
			return new CellOffset[]
			{
				CellOffset.down,
				CellOffset.rightdown,
				CellOffset.right
			};
		}
		if (preferedDirectionOffset == CellOffset.down)
		{
			return new CellOffset[]
			{
				CellOffset.down,
				CellOffset.leftdown,
				CellOffset.rightdown
			};
		}
		return new CellOffset[0];
	}

	// Token: 0x060033E3 RID: 13283 RVA: 0x00214F48 File Offset: 0x00213148
	public void TryToOffsetIfBuried(CellOffset offset)
	{
		if (this.KPrefabID.HasTag(GameTags.Stored) || this.KPrefabID.HasTag(GameTags.Equipped))
		{
			return;
		}
		int num = Grid.PosToCell(this);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		DeathMonitor.Instance smi = base.gameObject.GetSMI<DeathMonitor.Instance>();
		if ((smi == null || smi.IsDead()) && ((Grid.Solid[num] && Grid.Foundation[num]) || Grid.Properties[num] != 0))
		{
			CellOffset[] array = this.GetPreferedOffsets(offset).Concat(Pickupable.displacementOffsets);
			for (int i = 0; i < array.Length; i++)
			{
				int num2 = Grid.OffsetCell(num, array[i]);
				if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
				{
					Vector3 position = Grid.CellToPosCBC(num2, Grid.SceneLayer.Move);
					KCollider2D component = base.GetComponent<KCollider2D>();
					if (component != null)
					{
						position.y += base.transform.GetPosition().y - component.bounds.min.y;
					}
					base.transform.SetPosition(position);
					num = num2;
					this.RemoveFaller();
					this.AddFaller(Vector2.zero);
					break;
				}
			}
		}
		this.HandleSolidCell(num);
	}

	// Token: 0x060033E4 RID: 13284 RVA: 0x0021509C File Offset: 0x0021329C
	private bool HandleSolidCell(int cell)
	{
		bool flag = this.IsEntombed;
		bool flag2 = false;
		if (Grid.IsValidCell(cell) && Grid.Solid[cell])
		{
			DeathMonitor.Instance smi = base.gameObject.GetSMI<DeathMonitor.Instance>();
			if (smi == null || smi.IsDead())
			{
				this.Clearable.CancelClearing();
				flag2 = true;
			}
		}
		if (flag2 != flag && !this.KPrefabID.HasTag(GameTags.Stored))
		{
			this.IsEntombed = flag2;
			base.GetComponent<KSelectable>().IsSelectable = !this.IsEntombed;
		}
		this.UpdateEntombedVisualizer();
		return this.IsEntombed;
	}

	// Token: 0x060033E5 RID: 13285 RVA: 0x0021512C File Offset: 0x0021332C
	private void OnCellChange()
	{
		Vector3 position = base.transform.GetPosition();
		int num = Grid.PosToCell(position);
		if (!Grid.IsValidCell(num))
		{
			Vector2 vector = new Vector2(-0.1f * (float)Grid.WidthInCells, 1.1f * (float)Grid.WidthInCells);
			Vector2 vector2 = new Vector2(-0.1f * (float)Grid.HeightInCells, 1.1f * (float)Grid.HeightInCells);
			if (this.deleteOffGrid && (position.x < vector.x || vector.y < position.x || position.y < vector2.x || vector2.y < position.y))
			{
				this.DeleteObject();
				return;
			}
		}
		else
		{
			this.ReleaseEntombedVisualizerAndAddFaller(true);
			if (this.HandleSolidCell(num))
			{
				return;
			}
			this.objectLayerListItem.Update(num);
			bool flag = false;
			if (this.absorbable && !this.KPrefabID.HasTag(GameTags.Stored))
			{
				int num2 = Grid.CellBelow(num);
				if (Grid.IsValidCell(num2) && Grid.Solid[num2])
				{
					ObjectLayerListItem nextItem = this.objectLayerListItem.nextItem;
					while (nextItem != null)
					{
						GameObject gameObject = nextItem.gameObject;
						nextItem = nextItem.nextItem;
						Pickupable component = gameObject.GetComponent<Pickupable>();
						if (component != null)
						{
							flag = component.TryAbsorb(this, false, false);
							if (flag)
							{
								break;
							}
						}
					}
				}
			}
			GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, num);
			GameScenePartitioner.Instance.UpdatePosition(this.worldPartitionerEntry, num);
			int cachedCell = this.cachedCell;
			this.UpdateCachedCell(num);
			if (!flag)
			{
				this.NotifyChanged(num);
			}
			if (Grid.IsValidCell(cachedCell) && num != cachedCell)
			{
				this.NotifyChanged(cachedCell);
			}
		}
	}

	// Token: 0x060033E6 RID: 13286 RVA: 0x002152D8 File Offset: 0x002134D8
	private void OnTagsChanged(object data)
	{
		if (!this.KPrefabID.HasTag(GameTags.Stored) && !this.KPrefabID.HasTag(GameTags.Equipped))
		{
			this.UpdateListeners(true);
			this.AddFaller(Vector2.zero);
			return;
		}
		this.UpdateListeners(false);
		this.RemoveFaller();
	}

	// Token: 0x060033E7 RID: 13287 RVA: 0x000C6525 File Offset: 0x000C4725
	private void NotifyChanged(int new_cell)
	{
		GameScenePartitioner.Instance.TriggerEvent(new_cell, GameScenePartitioner.Instance.pickupablesChangedLayer, this);
	}

	// Token: 0x060033E8 RID: 13288 RVA: 0x0021532C File Offset: 0x0021352C
	public bool TryAbsorb(Pickupable other, bool hide_effects, bool allow_cross_storage = false)
	{
		if (other == null)
		{
			return false;
		}
		if (other.wasAbsorbed)
		{
			return false;
		}
		if (this.wasAbsorbed)
		{
			return false;
		}
		if (!other.CanAbsorb(this))
		{
			return false;
		}
		if (this.prevent_absorb_until_stored)
		{
			return false;
		}
		if (!allow_cross_storage && this.storage == null != (other.storage == null))
		{
			return false;
		}
		this.Absorb(other);
		if (!hide_effects && EffectPrefabs.Instance != null && !this.storage)
		{
			Vector3 position = base.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
			global::Util.KInstantiate(Assets.GetPrefab(EffectConfigs.OreAbsorbId), position, Quaternion.identity, null, null, true, 0).SetActive(true);
		}
		return true;
	}

	// Token: 0x060033E9 RID: 13289 RVA: 0x002153F4 File Offset: 0x002135F4
	protected override void OnCleanUp()
	{
		this.cleaningUp = true;
		this.ReleaseEntombedVisualizerAndAddFaller(false);
		this.RemoveFaller();
		if (this.storage)
		{
			this.storage.Remove(base.gameObject, true);
		}
		this.UnregisterListeners();
		this.fetchable_monitor = null;
		Components.Pickupables.Remove(this);
		if (this.reservations.Count > 0)
		{
			Pickupable.Reservation[] array = this.reservations.ToArray();
			this.reservations.Clear();
			if (this.OnReservationsChanged != null)
			{
				foreach (Pickupable.Reservation arg in array)
				{
					this.OnReservationsChanged(this, false, arg);
				}
			}
		}
		if (Grid.IsValidCell(this.cachedCell))
		{
			this.NotifyChanged(this.cachedCell);
		}
		base.OnCleanUp();
	}

	// Token: 0x060033EA RID: 13290 RVA: 0x002154C0 File Offset: 0x002136C0
	public Pickupable Take(float amount)
	{
		if (amount <= 0f)
		{
			return null;
		}
		if (this.OnTake == null)
		{
			if (this.storage != null)
			{
				this.storage.Remove(base.gameObject, true);
			}
			return this;
		}
		if (amount >= this.TotalAmount && this.storage != null && !this.primaryElement.KeepZeroMassObject)
		{
			this.storage.Remove(base.gameObject, true);
		}
		float num = Math.Min(this.TotalAmount, amount);
		if (num <= 0f)
		{
			return null;
		}
		return this.OnTake(this, num);
	}

	// Token: 0x060033EB RID: 13291 RVA: 0x0021555C File Offset: 0x0021375C
	private void Absorb(Pickupable pickupable)
	{
		global::Debug.Assert(!this.wasAbsorbed);
		global::Debug.Assert(!pickupable.wasAbsorbed);
		base.Trigger(-2064133523, pickupable);
		pickupable.Trigger(-1940207677, base.gameObject);
		pickupable.wasAbsorbed = true;
		KSelectable component = base.GetComponent<KSelectable>();
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == pickupable.GetComponent<KSelectable>())
		{
			SelectTool.Instance.Select(component, false);
		}
		pickupable.gameObject.DeleteObject();
		this.NotifyChanged(Grid.PosToCell(this));
	}

	// Token: 0x060033EC RID: 13292 RVA: 0x0021560C File Offset: 0x0021380C
	private void RefreshStorageTags(object data = null)
	{
		bool flag = data is Storage || (data != null && (bool)data);
		if (flag && data is Storage && ((Storage)data).gameObject == base.gameObject)
		{
			return;
		}
		if (!flag)
		{
			this.KPrefabID.RemoveTag(GameTags.Stored);
			this.KPrefabID.RemoveTag(GameTags.StoredPrivate);
			return;
		}
		this.KPrefabID.AddTag(GameTags.Stored, false);
		if (this.storage == null || !this.storage.allowItemRemoval)
		{
			this.KPrefabID.AddTag(GameTags.StoredPrivate, false);
			return;
		}
		this.KPrefabID.RemoveTag(GameTags.StoredPrivate);
	}

	// Token: 0x060033ED RID: 13293 RVA: 0x002156C8 File Offset: 0x002138C8
	public void OnStore(object data)
	{
		this.storage = (data as Storage);
		bool flag = data is Storage || (data != null && (bool)data);
		SaveLoadRoot component = base.GetComponent<SaveLoadRoot>();
		if (this.carryAnimOverride != null && this.lastCarrier != null)
		{
			this.lastCarrier.RemoveAnimOverrides(this.carryAnimOverride);
			this.lastCarrier = null;
		}
		KSelectable component2 = base.GetComponent<KSelectable>();
		if (component2)
		{
			component2.IsSelectable = !flag;
		}
		if (flag)
		{
			int cachedCell = this.cachedCell;
			this.RefreshStorageTags(data);
			this.RemoveFaller();
			if (this.storage != null)
			{
				if (this.carryAnimOverride != null && this.storage.GetComponent<Navigator>() != null)
				{
					this.lastCarrier = this.storage.GetComponent<KBatchedAnimController>();
					if (this.lastCarrier != null && this.lastCarrier.HasTag(GameTags.BaseMinion))
					{
						this.lastCarrier.AddAnimOverrides(this.carryAnimOverride, 0f);
					}
				}
				this.UpdateCachedCell(Grid.PosToCell(this.storage));
			}
			this.NotifyChanged(cachedCell);
			if (component != null)
			{
				component.SetRegistered(false);
				return;
			}
		}
		else
		{
			if (component != null)
			{
				component.SetRegistered(true);
			}
			this.RemovedFromStorage();
		}
	}

	// Token: 0x060033EE RID: 13294 RVA: 0x00215818 File Offset: 0x00213A18
	private void RemovedFromStorage()
	{
		this.storage = null;
		this.UpdateCachedCell(Grid.PosToCell(this));
		this.RefreshStorageTags(null);
		this.AddFaller(Vector2.zero);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.enabled = true;
		base.gameObject.transform.rotation = Quaternion.identity;
		this.UpdateListeners(true);
		component.GetBatchInstanceData().ClearOverrideTransformMatrix();
	}

	// Token: 0x060033EF RID: 13295 RVA: 0x000C653D File Offset: 0x000C473D
	public void UpdateCachedCellFromStoragePosition()
	{
		global::Debug.Assert(this.storage != null, "Only call UpdateCachedCellFromStoragePosition on pickupables in storage!");
		this.UpdateCachedCell(Grid.PosToCell(this.storage));
	}

	// Token: 0x060033F0 RID: 13296 RVA: 0x00215880 File Offset: 0x00213A80
	public void UpdateCachedCell(int cell)
	{
		if (this.cachedCell != cell && this.storedPartitionerEntry.IsValid())
		{
			GameScenePartitioner.Instance.UpdatePosition(this.storedPartitionerEntry, cell);
		}
		this.cachedCell = cell;
		this.GetOffsets(this.cachedCell);
		if (this.KPrefabID.HasTag(GameTags.PickupableStorage))
		{
			base.GetComponent<Storage>().UpdateStoredItemCachedCells();
		}
	}

	// Token: 0x060033F1 RID: 13297 RVA: 0x000C6566 File Offset: 0x000C4766
	public override int GetCell()
	{
		return this.cachedCell;
	}

	// Token: 0x060033F2 RID: 13298 RVA: 0x002158E8 File Offset: 0x00213AE8
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		if (this.useGunforPickup && worker.UsesMultiTool())
		{
			Workable.AnimInfo anim = base.GetAnim(worker);
			anim.smi = new MultitoolController.Instance(this, worker, "pickup", Assets.GetPrefab(EffectConfigs.OreAbsorbId));
			return anim;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x060033F3 RID: 13299 RVA: 0x00215940 File Offset: 0x00213B40
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = worker.GetComponent<Storage>();
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		float amount = pickupableStartWorkInfo.amount;
		if (!(this != null))
		{
			pickupableStartWorkInfo.setResultCb(null);
			return;
		}
		Pickupable pickupable = this.Take(amount);
		if (pickupable != null)
		{
			component.Store(pickupable.gameObject, false, false, true, false);
			worker.SetWorkCompleteData(pickupable);
			pickupableStartWorkInfo.setResultCb(pickupable.gameObject);
			return;
		}
		pickupableStartWorkInfo.setResultCb(null);
	}

	// Token: 0x060033F4 RID: 13300 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x060033F5 RID: 13301 RVA: 0x000C656E File Offset: 0x000C476E
	public override Vector3 GetTargetPoint()
	{
		return base.transform.GetPosition();
	}

	// Token: 0x060033F6 RID: 13302 RVA: 0x000C657B File Offset: 0x000C477B
	public bool IsReachable()
	{
		return this.isReachable;
	}

	// Token: 0x060033F7 RID: 13303 RVA: 0x002159CC File Offset: 0x00213BCC
	private void OnReachableChanged(object data)
	{
		this.isReachable = (bool)data;
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.isReachable)
		{
			component.RemoveStatusItem(Db.Get().MiscStatusItems.PickupableUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().MiscStatusItems.PickupableUnreachable, this);
	}

	// Token: 0x060033F8 RID: 13304 RVA: 0x000C6583 File Offset: 0x000C4783
	private void AddFaller(Vector2 initial_velocity)
	{
		if (!this.handleFallerComponents)
		{
			return;
		}
		if (!GameComps.Fallers.Has(base.gameObject))
		{
			GameComps.Fallers.Add(base.gameObject, initial_velocity);
		}
	}

	// Token: 0x060033F9 RID: 13305 RVA: 0x000C65B2 File Offset: 0x000C47B2
	private void RemoveFaller()
	{
		if (!this.handleFallerComponents)
		{
			return;
		}
		if (GameComps.Fallers.Has(base.gameObject))
		{
			GameComps.Fallers.Remove(base.gameObject);
		}
	}

	// Token: 0x060033FA RID: 13306 RVA: 0x00215A24 File Offset: 0x00213C24
	private void OnOreSizeChanged(object data)
	{
		Vector3 v = Vector3.zero;
		HandleVector<int>.Handle handle = GameComps.Gravities.GetHandle(base.gameObject);
		if (handle.IsValid())
		{
			v = GameComps.Gravities.GetData(handle).velocity;
		}
		this.RemoveFaller();
		if (!this.KPrefabID.HasTag(GameTags.Stored))
		{
			this.AddFaller(v);
		}
	}

	// Token: 0x060033FB RID: 13307 RVA: 0x00215A8C File Offset: 0x00213C8C
	private void OnLanded(object data)
	{
		if (CameraController.Instance == null)
		{
			return;
		}
		Vector3 position = base.transform.GetPosition();
		Vector2I vector2I = Grid.PosToXY(position);
		if (vector2I.x < 0 || Grid.WidthInCells <= vector2I.x || vector2I.y < 0 || Grid.HeightInCells <= vector2I.y)
		{
			this.DeleteObject();
			return;
		}
		Vector2 vector = (Vector2)data;
		if (vector.sqrMagnitude <= 0.2f || SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		Element element = this.primaryElement.Element;
		if (element.substance != null)
		{
			string text = element.substance.GetOreBumpSound();
			if (text == null)
			{
				if (element.HasTag(GameTags.RefinedMetal))
				{
					text = "RefinedMetal";
				}
				else if (element.HasTag(GameTags.Metal))
				{
					text = "RawMetal";
				}
				else
				{
					text = "Rock";
				}
			}
			if (element.tag.ToString() == "Creature" && !base.gameObject.HasTag(GameTags.Seed))
			{
				text = "Bodyfall_rock";
			}
			else
			{
				text = "Ore_bump_" + text;
			}
			string text2 = GlobalAssets.GetSound(text, true);
			text2 = ((text2 != null) ? text2 : GlobalAssets.GetSound("Ore_bump_rock", false));
			if (CameraController.Instance.IsAudibleSound(base.transform.GetPosition(), text2))
			{
				int num = Grid.PosToCell(position);
				bool isLiquid = Grid.Element[num].IsLiquid;
				float value = 0f;
				if (isLiquid)
				{
					value = SoundUtil.GetLiquidDepth(num);
				}
				FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(text2, CameraController.Instance.GetVerticallyScaledPosition(base.transform.GetPosition(), false), 1f);
				instance.setParameterByName("velocity", vector.magnitude, false);
				instance.setParameterByName("liquidDepth", value, false);
				KFMOD.EndOneShot(instance);
			}
		}
	}

	// Token: 0x060033FC RID: 13308 RVA: 0x00215C68 File Offset: 0x00213E68
	private void UpdateEntombedVisualizer()
	{
		if (this.IsEntombed)
		{
			if (this.entombedCell == -1)
			{
				int cell = Grid.PosToCell(this);
				if (EntombedItemManager.CanEntomb(this))
				{
					SaveGame.Instance.entombedItemManager.Add(this);
				}
				if (Grid.Objects[cell, 1] == null)
				{
					KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
					if (component != null && Game.Instance.GetComponent<EntombedItemVisualizer>().AddItem(cell))
					{
						this.entombedCell = cell;
						component.enabled = false;
						this.RemoveFaller();
						return;
					}
				}
			}
		}
		else
		{
			this.ReleaseEntombedVisualizerAndAddFaller(true);
		}
	}

	// Token: 0x060033FD RID: 13309 RVA: 0x00215CF8 File Offset: 0x00213EF8
	private void ReleaseEntombedVisualizerAndAddFaller(bool add_faller_if_necessary)
	{
		if (this.entombedCell != -1)
		{
			Game.Instance.GetComponent<EntombedItemVisualizer>().RemoveItem(this.entombedCell);
			this.entombedCell = -1;
			base.GetComponent<KBatchedAnimController>().enabled = true;
			if (add_faller_if_necessary)
			{
				this.AddFaller(Vector2.zero);
			}
		}
	}

	// Token: 0x04002369 RID: 9065
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400236A RID: 9066
	public const float WorkTime = 1.5f;

	// Token: 0x0400236B RID: 9067
	[SerializeField]
	private int _sortOrder;

	// Token: 0x0400236D RID: 9069
	[MyCmpReq]
	[NonSerialized]
	public KPrefabID KPrefabID;

	// Token: 0x0400236E RID: 9070
	[MyCmpAdd]
	[NonSerialized]
	public Clearable Clearable;

	// Token: 0x0400236F RID: 9071
	[MyCmpAdd]
	[NonSerialized]
	public Prioritizable prioritizable;

	// Token: 0x04002370 RID: 9072
	[SerializeField]
	public List<ChoreType> allowedChoreTypes;

	// Token: 0x04002371 RID: 9073
	public bool absorbable;

	// Token: 0x04002373 RID: 9075
	public Func<Pickupable, bool> CanAbsorb = (Pickupable other) => false;

	// Token: 0x04002374 RID: 9076
	public Func<Pickupable, float, Pickupable> OnTake;

	// Token: 0x04002375 RID: 9077
	public Action<Pickupable, bool, Pickupable.Reservation> OnReservationsChanged;

	// Token: 0x04002376 RID: 9078
	public ObjectLayerListItem objectLayerListItem;

	// Token: 0x04002377 RID: 9079
	public Workable targetWorkable;

	// Token: 0x04002378 RID: 9080
	public KAnimFile carryAnimOverride;

	// Token: 0x04002379 RID: 9081
	private KBatchedAnimController lastCarrier;

	// Token: 0x0400237A RID: 9082
	public bool useGunforPickup = true;

	// Token: 0x0400237C RID: 9084
	private static CellOffset[] displacementOffsets = new CellOffset[]
	{
		new CellOffset(0, 1),
		new CellOffset(0, -1),
		new CellOffset(1, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 1),
		new CellOffset(1, -1),
		new CellOffset(-1, 1),
		new CellOffset(-1, -1)
	};

	// Token: 0x0400237D RID: 9085
	private bool isReachable;

	// Token: 0x0400237E RID: 9086
	private bool isEntombed;

	// Token: 0x0400237F RID: 9087
	private bool cleaningUp;

	// Token: 0x04002381 RID: 9089
	public bool trackOnPickup = true;

	// Token: 0x04002383 RID: 9091
	private int nextTicketNumber;

	// Token: 0x04002384 RID: 9092
	[Serialize]
	public bool deleteOffGrid = true;

	// Token: 0x04002385 RID: 9093
	private List<Pickupable.Reservation> reservations = new List<Pickupable.Reservation>();

	// Token: 0x04002386 RID: 9094
	private HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x04002387 RID: 9095
	private HandleVector<int>.Handle worldPartitionerEntry;

	// Token: 0x04002388 RID: 9096
	private HandleVector<int>.Handle storedPartitionerEntry;

	// Token: 0x04002389 RID: 9097
	private FetchableMonitor.Instance fetchable_monitor;

	// Token: 0x0400238A RID: 9098
	public bool handleFallerComponents = true;

	// Token: 0x0400238B RID: 9099
	private LoggerFSSF log;

	// Token: 0x0400238D RID: 9101
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x0400238E RID: 9102
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnLandedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnLanded(data);
	});

	// Token: 0x0400238F RID: 9103
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnOreSizeChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnOreSizeChanged(data);
	});

	// Token: 0x04002390 RID: 9104
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x04002391 RID: 9105
	private static readonly EventSystem.IntraObjectHandler<Pickupable> RefreshStorageTagsDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.RefreshStorageTags(data);
	});

	// Token: 0x04002392 RID: 9106
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnWorkableEntombOffset = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.SetWorkableOffset(data);
	});

	// Token: 0x04002393 RID: 9107
	private static readonly EventSystem.IntraObjectHandler<Pickupable> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Pickupable>(delegate(Pickupable component, object data)
	{
		component.OnTagsChanged(data);
	});

	// Token: 0x04002394 RID: 9108
	private int entombedCell = -1;

	// Token: 0x02000AF8 RID: 2808
	public struct Reservation
	{
		// Token: 0x060033FF RID: 13311 RVA: 0x000C65DF File Offset: 0x000C47DF
		public Reservation(int reserverID, float amount, int ticket)
		{
			this.reserverID = reserverID;
			this.amount = amount;
			this.ticket = ticket;
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x00215E84 File Offset: 0x00214084
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.reserverID.ToString(),
				", ",
				this.amount.ToString(),
				", ",
				this.ticket.ToString()
			});
		}

		// Token: 0x04002395 RID: 9109
		public int reserverID;

		// Token: 0x04002396 RID: 9110
		public float amount;

		// Token: 0x04002397 RID: 9111
		public int ticket;
	}

	// Token: 0x02000AF9 RID: 2809
	public class PickupableStartWorkInfo : WorkerBase.StartWorkInfo
	{
		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06003401 RID: 13313 RVA: 0x000C65F6 File Offset: 0x000C47F6
		// (set) Token: 0x06003402 RID: 13314 RVA: 0x000C65FE File Offset: 0x000C47FE
		public float amount { get; private set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06003403 RID: 13315 RVA: 0x000C6607 File Offset: 0x000C4807
		// (set) Token: 0x06003404 RID: 13316 RVA: 0x000C660F File Offset: 0x000C480F
		public Pickupable originalPickupable { get; private set; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06003405 RID: 13317 RVA: 0x000C6618 File Offset: 0x000C4818
		// (set) Token: 0x06003406 RID: 13318 RVA: 0x000C6620 File Offset: 0x000C4820
		public Action<GameObject> setResultCb { get; private set; }

		// Token: 0x06003407 RID: 13319 RVA: 0x000C6629 File Offset: 0x000C4829
		public PickupableStartWorkInfo(Pickupable pickupable, float amount, Action<GameObject> set_result_cb) : base(pickupable.targetWorkable)
		{
			this.originalPickupable = pickupable;
			this.amount = amount;
			this.setResultCb = set_result_cb;
		}
	}
}
