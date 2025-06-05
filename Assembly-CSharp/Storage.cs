using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B53 RID: 2899
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Storage")]
public class Storage : Workable, ISaveLoadableDetails, IGameObjectEffectDescriptor, IStorage
{
	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06003614 RID: 13844 RVA: 0x000C7BB6 File Offset: 0x000C5DB6
	public bool ShouldOnlyTransferFromLowerPriority
	{
		get
		{
			return this.onlyTransferFromLowerPriority || this.allowItemRemoval;
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06003615 RID: 13845 RVA: 0x000C7BC8 File Offset: 0x000C5DC8
	// (set) Token: 0x06003616 RID: 13846 RVA: 0x000C7BD0 File Offset: 0x000C5DD0
	public bool allowUIItemRemoval { get; set; }

	// Token: 0x17000258 RID: 600
	public GameObject this[int idx]
	{
		get
		{
			return this.items[idx];
		}
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06003618 RID: 13848 RVA: 0x000C7BE7 File Offset: 0x000C5DE7
	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06003619 RID: 13849 RVA: 0x000C7BF4 File Offset: 0x000C5DF4
	// (set) Token: 0x0600361A RID: 13850 RVA: 0x000C7BFC File Offset: 0x000C5DFC
	public bool ShouldSaveItems
	{
		get
		{
			return this.shouldSaveItems;
		}
		set
		{
			this.shouldSaveItems = value;
		}
	}

	// Token: 0x0600361B RID: 13851 RVA: 0x000C7C05 File Offset: 0x000C5E05
	public bool ShouldShowInUI()
	{
		return this.showInUI;
	}

	// Token: 0x0600361C RID: 13852 RVA: 0x000C7C0D File Offset: 0x000C5E0D
	public List<GameObject> GetItems()
	{
		return this.items;
	}

	// Token: 0x0600361D RID: 13853 RVA: 0x000C7C15 File Offset: 0x000C5E15
	public void SetDefaultStoredItemModifiers(List<Storage.StoredItemModifier> modifiers)
	{
		this.defaultStoredItemModifers = modifiers;
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x0600361E RID: 13854 RVA: 0x000C7C1E File Offset: 0x000C5E1E
	public PrioritySetting masterPriority
	{
		get
		{
			if (this.prioritizable)
			{
				return this.prioritizable.GetMasterPriority();
			}
			return Chore.DefaultPrioritySetting;
		}
	}

	// Token: 0x0600361F RID: 13855 RVA: 0x0021EB5C File Offset: 0x0021CD5C
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		if (this.useGunForDelivery && worker.UsesMultiTool())
		{
			Workable.AnimInfo anim = base.GetAnim(worker);
			anim.smi = new MultitoolController.Instance(this, worker, "store", Assets.GetPrefab(EffectConfigs.OreAbsorbId));
			return anim;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x06003620 RID: 13856 RVA: 0x0021EBB4 File Offset: 0x0021CDB4
	public override Vector3 GetTargetPoint()
	{
		Vector3 vector = base.GetTargetPoint();
		if (this.useGunForDelivery && this.gunTargetOffset != Vector2.zero)
		{
			if (this.rotatable != null)
			{
				vector += this.rotatable.GetRotatedOffset(this.gunTargetOffset);
			}
			else
			{
				vector += new Vector3(this.gunTargetOffset.x, this.gunTargetOffset.y, 0f);
			}
		}
		return vector;
	}

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06003621 RID: 13857 RVA: 0x0021EC38 File Offset: 0x0021CE38
	// (remove) Token: 0x06003622 RID: 13858 RVA: 0x0021EC70 File Offset: 0x0021CE70
	public event System.Action OnStorageIncreased;

	// Token: 0x06003623 RID: 13859 RVA: 0x0021ECA8 File Offset: 0x0021CEA8
	protected override void OnPrefabInit()
	{
		if (this.useWideOffsets)
		{
			base.SetOffsetTable(OffsetGroups.InvertedWideTable);
		}
		else
		{
			base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		}
		this.showProgressBar = false;
		this.faceTargetWhenWorking = true;
		base.OnPrefabInit();
		GameUtil.SubscribeToTags<Storage>(this, Storage.OnDeadTagAddedDelegate, true);
		base.Subscribe<Storage>(1502190696, Storage.OnQueueDestroyObjectDelegate);
		base.Subscribe<Storage>(-905833192, Storage.OnCopySettingsDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Storing;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.SetupStorageStatusItems();
	}

	// Token: 0x06003624 RID: 13860 RVA: 0x0021ED50 File Offset: 0x0021CF50
	private void SetupStorageStatusItems()
	{
		if (Storage.capacityStatusItem == null)
		{
			Storage.capacityStatusItem = new StatusItem("StorageLocker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Storage.capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				Storage storage = (Storage)data;
				float num = storage.MassStored();
				float num2 = storage.capacityKg;
				if (num > num2 - storage.storageFullMargin && num < num2)
				{
					num = num2;
				}
				else
				{
					num = Mathf.Floor(num);
				}
				string newValue = Util.FormatWholeNumber(num);
				IUserControlledCapacity component = storage.GetComponent<IUserControlledCapacity>();
				if (component != null)
				{
					num2 = Mathf.Min(component.UserMaxCapacity, num2);
				}
				string newValue2 = Util.FormatWholeNumber(num2);
				str = str.Replace("{Stored}", newValue);
				str = str.Replace("{Capacity}", newValue2);
				if (component != null)
				{
					str = str.Replace("{Units}", component.CapacityUnits);
				}
				else
				{
					str = str.Replace("{Units}", GameUtil.GetCurrentMassUnit(false));
				}
				return str;
			};
		}
		if (this.showCapacityStatusItem)
		{
			if (this.showCapacityAsMainStatus)
			{
				base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Storage.capacityStatusItem, this);
				return;
			}
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Stored, Storage.capacityStatusItem, this);
		}
	}

	// Token: 0x06003625 RID: 13861 RVA: 0x000C7C3E File Offset: 0x000C5E3E
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (!this.allowSettingOnlyFetchMarkedItems)
		{
			this.onlyFetchMarkedItems = false;
		}
		this.UpdateFetchCategory();
	}

	// Token: 0x06003626 RID: 13862 RVA: 0x0021EE08 File Offset: 0x0021D008
	protected override void OnSpawn()
	{
		base.SetWorkTime(this.storageWorkTime);
		foreach (GameObject go in this.items)
		{
			this.ApplyStoredItemModifiers(go, true, true);
			if (this.sendOnStoreOnSpawn)
			{
				go.Trigger(856640610, this);
			}
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.SetSymbolVisiblity("sweep", this.onlyFetchMarkedItems);
		}
		Prioritizable component2 = base.GetComponent<Prioritizable>();
		if (component2 != null)
		{
			Prioritizable prioritizable = component2;
			prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnPriorityChanged));
		}
		this.UpdateFetchCategory();
		if (this.showUnreachableStatus)
		{
			base.Subscribe<Storage>(-1432940121, Storage.OnReachableChangedDelegate);
			new ReachabilityMonitor.Instance(this).StartSM();
		}
	}

	// Token: 0x06003627 RID: 13863 RVA: 0x0021EF00 File Offset: 0x0021D100
	public GameObject Store(GameObject go, bool hide_popups = false, bool block_events = false, bool do_disease_transfer = true, bool is_deserializing = false)
	{
		if (go == null)
		{
			return null;
		}
		PrimaryElement component = go.GetComponent<PrimaryElement>();
		GameObject result = go;
		if (!hide_popups && PopFXManager.Instance != null)
		{
			LocString loc_string;
			Transform transform;
			if (this.fxPrefix == Storage.FXPrefix.Delivered)
			{
				loc_string = UI.DELIVERED;
				transform = base.transform;
			}
			else
			{
				loc_string = UI.PICKEDUP;
				transform = go.transform;
			}
			string text;
			if (!Assets.IsTagCountable(go.PrefabID()))
			{
				text = string.Format(loc_string, GameUtil.GetFormattedMass(component.Units, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), go.GetProperName());
			}
			else
			{
				text = string.Format(loc_string, (int)component.Units, go.GetProperName());
			}
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, text, transform, this.storageFXOffset, 1.5f, false, false);
		}
		go.transform.parent = base.transform;
		Vector3 position = Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Move);
		position.z = go.transform.GetPosition().z;
		go.transform.SetPosition(position);
		if (!block_events && do_disease_transfer)
		{
			this.TransferDiseaseWithObject(go);
		}
		if (!is_deserializing)
		{
			Pickupable component2 = go.GetComponent<Pickupable>();
			if (component2 != null)
			{
				if (component2 != null && component2.prevent_absorb_until_stored)
				{
					component2.prevent_absorb_until_stored = false;
				}
				foreach (GameObject gameObject in this.items)
				{
					if (gameObject != null)
					{
						Pickupable component3 = gameObject.GetComponent<Pickupable>();
						if (component3 != null && component3.TryAbsorb(component2, hide_popups, true))
						{
							if (!block_events)
							{
								base.Trigger(-1697596308, go);
								Action<GameObject> onStorageChange = this.OnStorageChange;
								if (onStorageChange != null)
								{
									onStorageChange(go);
								}
								base.Trigger(-778359855, this);
								if (this.OnStorageIncreased != null)
								{
									this.OnStorageIncreased();
								}
							}
							this.ApplyStoredItemModifiers(go, true, false);
							result = gameObject;
							go = null;
							break;
						}
					}
				}
			}
		}
		if (go != null)
		{
			this.items.Add(go);
			if (!is_deserializing)
			{
				this.ApplyStoredItemModifiers(go, true, false);
			}
			if (!block_events)
			{
				go.Trigger(856640610, this);
				base.Trigger(-1697596308, go);
				Action<GameObject> onStorageChange2 = this.OnStorageChange;
				if (onStorageChange2 != null)
				{
					onStorageChange2(go);
				}
				base.Trigger(-778359855, this);
				if (this.OnStorageIncreased != null)
				{
					this.OnStorageIncreased();
				}
			}
		}
		return result;
	}

	// Token: 0x06003628 RID: 13864 RVA: 0x0021F194 File Offset: 0x0021D394
	public PrimaryElement AddElement(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass = false, bool do_disease_transfer = true)
	{
		Element element2 = ElementLoader.FindElementByHash(element);
		if (element2.IsGas)
		{
			return this.AddGasChunk(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
		}
		if (element2.IsLiquid)
		{
			return this.AddLiquid(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
		}
		if (element2.IsSolid)
		{
			return this.AddOre(element, mass, temperature, disease_idx, disease_count, keep_zero_mass, do_disease_transfer);
		}
		return null;
	}

	// Token: 0x06003629 RID: 13865 RVA: 0x0021F1F8 File Offset: 0x0021D3F8
	public PrimaryElement AddOre(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass = false, bool do_disease_transfer = true)
	{
		if (mass <= 0f)
		{
			return null;
		}
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, mass);
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			primaryElement.Mass += mass;
			primaryElement.Temperature = finalTemperature;
			primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddOre");
			base.Trigger(-1697596308, primaryElement.gameObject);
			Action<GameObject> onStorageChange = this.OnStorageChange;
			if (onStorageChange != null)
			{
				onStorageChange(primaryElement.gameObject);
			}
		}
		else
		{
			Element element2 = ElementLoader.FindElementByHash(element);
			GameObject gameObject = element2.substance.SpawnResource(base.transform.GetPosition(), mass, temperature, disease_idx, disease_count, true, false, true);
			gameObject.GetComponent<Pickupable>().prevent_absorb_until_stored = true;
			element2.substance.ActivateSubstanceGameObject(gameObject, disease_idx, disease_count);
			this.Store(gameObject, true, false, do_disease_transfer, false);
		}
		return primaryElement;
	}

	// Token: 0x0600362A RID: 13866 RVA: 0x0021F2DC File Offset: 0x0021D4DC
	public PrimaryElement AddLiquid(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass = false, bool do_disease_transfer = true)
	{
		if (mass <= 0f)
		{
			return null;
		}
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, mass);
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			primaryElement.Mass += mass;
			primaryElement.Temperature = finalTemperature;
			primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddLiquid");
			base.Trigger(-1697596308, primaryElement.gameObject);
			Action<GameObject> onStorageChange = this.OnStorageChange;
			if (onStorageChange != null)
			{
				onStorageChange(primaryElement.gameObject);
			}
		}
		else
		{
			SubstanceChunk substanceChunk = LiquidSourceManager.Instance.CreateChunk(element, mass, temperature, disease_idx, disease_count, base.transform.GetPosition());
			primaryElement = substanceChunk.GetComponent<PrimaryElement>();
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			this.Store(substanceChunk.gameObject, true, false, do_disease_transfer, false);
		}
		return primaryElement;
	}

	// Token: 0x0600362B RID: 13867 RVA: 0x0021F3B0 File Offset: 0x0021D5B0
	public PrimaryElement AddGasChunk(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, bool keep_zero_mass, bool do_disease_transfer = true)
	{
		if (mass <= 0f)
		{
			return null;
		}
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float mass2 = primaryElement.Mass;
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, mass2, temperature, mass);
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			primaryElement.SetMassTemperature(mass2 + mass, finalTemperature);
			primaryElement.AddDisease(disease_idx, disease_count, "Storage.AddGasChunk");
			base.Trigger(-1697596308, primaryElement.gameObject);
			Action<GameObject> onStorageChange = this.OnStorageChange;
			if (onStorageChange != null)
			{
				onStorageChange(primaryElement.gameObject);
			}
		}
		else
		{
			SubstanceChunk substanceChunk = GasSourceManager.Instance.CreateChunk(element, mass, temperature, disease_idx, disease_count, base.transform.GetPosition());
			primaryElement = substanceChunk.GetComponent<PrimaryElement>();
			primaryElement.KeepZeroMassObject = keep_zero_mass;
			this.Store(substanceChunk.gameObject, true, false, do_disease_transfer, false);
		}
		return primaryElement;
	}

	// Token: 0x0600362C RID: 13868 RVA: 0x000C7C55 File Offset: 0x000C5E55
	public void Transfer(Storage target, bool block_events = false, bool hide_popups = false)
	{
		while (this.items.Count > 0)
		{
			this.Transfer(this.items[0], target, block_events, hide_popups);
		}
	}

	// Token: 0x0600362D RID: 13869 RVA: 0x0021F478 File Offset: 0x0021D678
	public bool TransferMass(Storage dest_storage, Tag tag, float amount, bool flatten = false, bool block_events = false, bool hide_popups = false)
	{
		float num = amount;
		while (num > 0f && this.GetAmountAvailable(tag) > 0f)
		{
			num -= this.Transfer(dest_storage, tag, num, block_events, hide_popups);
		}
		if (flatten)
		{
			dest_storage.Flatten(tag);
		}
		return num <= 0f;
	}

	// Token: 0x0600362E RID: 13870 RVA: 0x0021F4C8 File Offset: 0x0021D6C8
	public float Transfer(Storage dest_storage, Tag tag, float amount, bool block_events = false, bool hide_popups = false)
	{
		GameObject gameObject = this.FindFirst(tag);
		if (gameObject != null)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (amount < component.Units)
			{
				Pickupable component2 = gameObject.GetComponent<Pickupable>();
				Pickupable pickupable = component2.Take(amount);
				dest_storage.Store(pickupable.gameObject, hide_popups, block_events, true, false);
				if (!block_events)
				{
					base.Trigger(-1697596308, component2.gameObject);
					Action<GameObject> onStorageChange = this.OnStorageChange;
					if (onStorageChange != null)
					{
						onStorageChange(component2.gameObject);
					}
				}
			}
			else
			{
				this.Transfer(gameObject, dest_storage, block_events, hide_popups);
				amount = component.Units;
			}
			return amount;
		}
		return 0f;
	}

	// Token: 0x0600362F RID: 13871 RVA: 0x0021F564 File Offset: 0x0021D764
	public bool Transfer(GameObject go, Storage target, bool block_events = false, bool hide_popups = false)
	{
		this.items.RemoveAll((GameObject it) => it == null);
		int count = this.items.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.items[i] == go)
			{
				this.items.RemoveAt(i);
				this.ApplyStoredItemModifiers(go, false, false);
				target.Store(go, hide_popups, block_events, true, false);
				if (!block_events)
				{
					base.Trigger(-1697596308, go);
					Action<GameObject> onStorageChange = this.OnStorageChange;
					if (onStorageChange != null)
					{
						onStorageChange(go);
					}
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003630 RID: 13872 RVA: 0x0021F610 File Offset: 0x0021D810
	public bool DropSome(Tag tag, float amount, bool ventGas = false, bool dumpLiquid = false, Vector3 offset = default(Vector3), bool doDiseaseTransfer = true, bool showInWorldNotification = false)
	{
		bool result = false;
		float num = amount;
		ListPool<GameObject, Storage>.PooledList pooledList = ListPool<GameObject, Storage>.Allocate();
		this.Find(tag, pooledList);
		foreach (GameObject gameObject in pooledList)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			if (component)
			{
				Pickupable pickupable = component.Take(num);
				if (pickupable != null)
				{
					bool flag = false;
					if (ventGas || dumpLiquid)
					{
						Dumpable component2 = pickupable.GetComponent<Dumpable>();
						if (component2 != null)
						{
							if (ventGas && pickupable.GetComponent<PrimaryElement>().Element.IsGas)
							{
								component2.Dump(base.transform.GetPosition() + offset);
								flag = true;
								num -= pickupable.GetComponent<PrimaryElement>().Mass;
								base.Trigger(-1697596308, pickupable.gameObject);
								Action<GameObject> onStorageChange = this.OnStorageChange;
								if (onStorageChange != null)
								{
									onStorageChange(pickupable.gameObject);
								}
								result = true;
								if (showInWorldNotification)
								{
									PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, pickupable.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(pickupable.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.transform, this.storageFXOffset, 1.5f, false, false);
								}
							}
							if (dumpLiquid && pickupable.GetComponent<PrimaryElement>().Element.IsLiquid)
							{
								component2.Dump(base.transform.GetPosition() + offset);
								flag = true;
								num -= pickupable.GetComponent<PrimaryElement>().Mass;
								base.Trigger(-1697596308, pickupable.gameObject);
								Action<GameObject> onStorageChange2 = this.OnStorageChange;
								if (onStorageChange2 != null)
								{
									onStorageChange2(pickupable.gameObject);
								}
								result = true;
								if (showInWorldNotification)
								{
									PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, pickupable.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(pickupable.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.transform, this.storageFXOffset, 1.5f, false, false);
								}
							}
						}
					}
					if (!flag)
					{
						Vector3 position = Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Ore) + offset;
						pickupable.transform.SetPosition(position);
						KBatchedAnimController component3 = pickupable.GetComponent<KBatchedAnimController>();
						if (component3)
						{
							component3.SetSceneLayer(Grid.SceneLayer.Ore);
						}
						num -= pickupable.GetComponent<PrimaryElement>().Mass;
						this.MakeWorldActive(pickupable.gameObject);
						base.Trigger(-1697596308, pickupable.gameObject);
						Action<GameObject> onStorageChange3 = this.OnStorageChange;
						if (onStorageChange3 != null)
						{
							onStorageChange3(pickupable.gameObject);
						}
						result = true;
						if (showInWorldNotification)
						{
							PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, pickupable.GetComponent<PrimaryElement>().Element.name + " " + GameUtil.GetFormattedMass(pickupable.TotalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.transform, this.storageFXOffset, 1.5f, false, false);
						}
					}
				}
			}
			if (num <= 0f)
			{
				break;
			}
		}
		pooledList.Recycle();
		return result;
	}

	// Token: 0x06003631 RID: 13873 RVA: 0x0021F964 File Offset: 0x0021DB64
	public void DropAll(Vector3 position, bool vent_gas = false, bool dump_liquid = false, Vector3 offset = default(Vector3), bool do_disease_transfer = true, List<GameObject> collect_dropped_items = null)
	{
		while (this.items.Count > 0)
		{
			GameObject gameObject = this.items[0];
			if (do_disease_transfer)
			{
				this.TransferDiseaseWithObject(gameObject);
			}
			this.items.RemoveAt(0);
			if (gameObject != null)
			{
				bool flag = false;
				if (vent_gas || dump_liquid)
				{
					Dumpable component = gameObject.GetComponent<Dumpable>();
					if (component != null)
					{
						if (vent_gas && gameObject.GetComponent<PrimaryElement>().Element.IsGas)
						{
							component.Dump(position + offset);
							flag = true;
						}
						if (dump_liquid && gameObject.GetComponent<PrimaryElement>().Element.IsLiquid)
						{
							component.Dump(position + offset);
							flag = true;
						}
					}
				}
				if (!flag)
				{
					gameObject.transform.SetPosition(position + offset);
					KBatchedAnimController component2 = gameObject.GetComponent<KBatchedAnimController>();
					if (component2)
					{
						component2.SetSceneLayer(Grid.SceneLayer.Ore);
					}
					this.MakeWorldActive(gameObject);
					if (collect_dropped_items != null)
					{
						collect_dropped_items.Add(gameObject);
					}
				}
			}
		}
	}

	// Token: 0x06003632 RID: 13874 RVA: 0x000C7C7D File Offset: 0x000C5E7D
	public void DropAll(bool vent_gas = false, bool dump_liquid = false, Vector3 offset = default(Vector3), bool do_disease_transfer = true, List<GameObject> collect_dropped_items = null)
	{
		this.DropAll(Grid.CellToPosCCC(Grid.PosToCell(this), Grid.SceneLayer.Ore), vent_gas, dump_liquid, offset, do_disease_transfer, collect_dropped_items);
	}

	// Token: 0x06003633 RID: 13875 RVA: 0x0021FA5C File Offset: 0x0021DC5C
	public void Drop(Tag t, List<GameObject> obj_list)
	{
		this.Find(t, obj_list);
		foreach (GameObject go in obj_list)
		{
			this.Drop(go, true);
		}
	}

	// Token: 0x06003634 RID: 13876 RVA: 0x0021FAB8 File Offset: 0x0021DCB8
	public void Drop(Tag t)
	{
		ListPool<GameObject, Storage>.PooledList pooledList = ListPool<GameObject, Storage>.Allocate();
		this.Find(t, pooledList);
		foreach (GameObject go in pooledList)
		{
			this.Drop(go, true);
		}
		pooledList.Recycle();
	}

	// Token: 0x06003635 RID: 13877 RVA: 0x0021FB20 File Offset: 0x0021DD20
	public void DropUnlessMatching(FetchChore chore)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null))
			{
				KPrefabID component = this.items[i].GetComponent<KPrefabID>();
				if (!(((chore.criteria == FetchChore.MatchCriteria.MatchID && chore.tags.Contains(component.PrefabTag)) || (chore.criteria == FetchChore.MatchCriteria.MatchTags && component.HasTag(chore.tagsFirst))) & (!chore.requiredTag.IsValid || component.HasTag(chore.requiredTag)) & !component.HasAnyTags(chore.forbiddenTags)))
				{
					GameObject gameObject = this.items[i];
					this.items.RemoveAt(i);
					i--;
					this.TransferDiseaseWithObject(gameObject);
					this.MakeWorldActive(gameObject);
				}
			}
		}
	}

	// Token: 0x06003636 RID: 13878 RVA: 0x0021FC04 File Offset: 0x0021DE04
	public GameObject[] DropUnlessHasTag(Tag tag)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null) && !this.items[i].GetComponent<KPrefabID>().HasTag(tag))
			{
				GameObject gameObject = this.items[i];
				this.items.RemoveAt(i);
				i--;
				this.TransferDiseaseWithObject(gameObject);
				this.MakeWorldActive(gameObject);
				Dumpable component = gameObject.GetComponent<Dumpable>();
				if (component != null)
				{
					component.Dump(base.transform.GetPosition());
				}
				list.Add(gameObject);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06003637 RID: 13879 RVA: 0x0021FCBC File Offset: 0x0021DEBC
	public GameObject[] DropHasTags(Tag[] tag)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null) && this.items[i].GetComponent<KPrefabID>().HasAllTags(tag))
			{
				GameObject gameObject = this.items[i];
				this.items.RemoveAt(i);
				i--;
				this.TransferDiseaseWithObject(gameObject);
				this.MakeWorldActive(gameObject);
				Dumpable component = gameObject.GetComponent<Dumpable>();
				if (component != null)
				{
					component.Dump(base.transform.GetPosition());
				}
				list.Add(gameObject);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06003638 RID: 13880 RVA: 0x0021FD74 File Offset: 0x0021DF74
	public GameObject Drop(GameObject go, bool do_disease_transfer = true)
	{
		if (go == null)
		{
			return null;
		}
		int count = this.items.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(go != this.items[i]))
			{
				this.items[i] = this.items[count - 1];
				this.items.RemoveAt(count - 1);
				if (do_disease_transfer)
				{
					this.TransferDiseaseWithObject(go);
				}
				this.MakeWorldActive(go);
				break;
			}
		}
		return go;
	}

	// Token: 0x06003639 RID: 13881 RVA: 0x0021FDF4 File Offset: 0x0021DFF4
	public void RenotifyAll()
	{
		this.items.RemoveAll((GameObject it) => it == null);
		foreach (GameObject go in this.items)
		{
			go.Trigger(856640610, this);
		}
	}

	// Token: 0x0600363A RID: 13882 RVA: 0x0021FE78 File Offset: 0x0021E078
	private void TransferDiseaseWithObject(GameObject obj)
	{
		if (obj == null || !this.doDiseaseTransfer || this.primaryElement == null)
		{
			return;
		}
		PrimaryElement component = obj.GetComponent<PrimaryElement>();
		if (component == null)
		{
			return;
		}
		SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
		invalid.idx = component.DiseaseIdx;
		invalid.count = (int)((float)component.DiseaseCount * 0.05f);
		SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
		invalid2.idx = this.primaryElement.DiseaseIdx;
		invalid2.count = (int)((float)this.primaryElement.DiseaseCount * 0.05f);
		component.ModifyDiseaseCount(-invalid.count, "Storage.TransferDiseaseWithObject");
		this.primaryElement.ModifyDiseaseCount(-invalid2.count, "Storage.TransferDiseaseWithObject");
		if (invalid.count > 0)
		{
			this.primaryElement.AddDisease(invalid.idx, invalid.count, "Storage.TransferDiseaseWithObject");
		}
		if (invalid2.count > 0)
		{
			component.AddDisease(invalid2.idx, invalid2.count, "Storage.TransferDiseaseWithObject");
		}
	}

	// Token: 0x0600363B RID: 13883 RVA: 0x0021FF80 File Offset: 0x0021E180
	private void MakeWorldActive(GameObject go)
	{
		go.transform.parent = null;
		if (this.dropOffset != Vector2.zero)
		{
			go.transform.Translate(this.dropOffset);
		}
		go.Trigger(856640610, null);
		base.Trigger(-1697596308, go);
		Action<GameObject> onStorageChange = this.OnStorageChange;
		if (onStorageChange != null)
		{
			onStorageChange(go);
		}
		this.ApplyStoredItemModifiers(go, false, false);
		if (go != null)
		{
			PrimaryElement component = go.GetComponent<PrimaryElement>();
			if (component != null && component.KeepZeroMassObject)
			{
				component.KeepZeroMassObject = false;
				if (component.Mass <= 0f)
				{
					Util.KDestroyGameObject(go);
				}
			}
		}
	}

	// Token: 0x0600363C RID: 13884 RVA: 0x00220030 File Offset: 0x0021E230
	public List<GameObject> Find(Tag tag, List<GameObject> result)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				result.Add(gameObject);
			}
		}
		return result;
	}

	// Token: 0x0600363D RID: 13885 RVA: 0x0022007C File Offset: 0x0021E27C
	public GameObject FindFirst(Tag tag)
	{
		GameObject result = null;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				result = gameObject;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600363E RID: 13886 RVA: 0x002200C8 File Offset: 0x0021E2C8
	public PrimaryElement FindFirstWithMass(Tag tag, float mass = 0f)
	{
		PrimaryElement result = null;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.Mass > 0f && component.Mass >= mass)
				{
					result = component;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x0600363F RID: 13887 RVA: 0x00220130 File Offset: 0x0021E330
	private void Flatten(Tag tag_to_combine)
	{
		GameObject gameObject = this.FindFirst(tag_to_combine);
		if (gameObject == null)
		{
			return;
		}
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			GameObject gameObject2 = this.items[i];
			if (gameObject2.HasTag(tag_to_combine) && gameObject2 != gameObject)
			{
				PrimaryElement component2 = gameObject2.GetComponent<PrimaryElement>();
				component.Mass += component2.Mass;
				this.ConsumeIgnoringDisease(gameObject2);
			}
		}
	}

	// Token: 0x06003640 RID: 13888 RVA: 0x002201B0 File Offset: 0x0021E3B0
	public HashSet<Tag> GetAllIDsInStorage()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject go = this.items[i];
			hashSet.Add(go.PrefabID());
		}
		return hashSet;
	}

	// Token: 0x06003641 RID: 13889 RVA: 0x002201F4 File Offset: 0x0021E3F4
	public GameObject Find(int ID)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (ID == gameObject.PrefabID().GetHashCode())
			{
				return gameObject;
			}
		}
		return null;
	}

	// Token: 0x06003642 RID: 13890 RVA: 0x000C7C99 File Offset: 0x000C5E99
	public void ConsumeAllIgnoringDisease()
	{
		this.ConsumeAllIgnoringDisease(Tag.Invalid);
	}

	// Token: 0x06003643 RID: 13891 RVA: 0x00220240 File Offset: 0x0021E440
	public void ConsumeAllIgnoringDisease(Tag tag)
	{
		for (int i = this.items.Count - 1; i >= 0; i--)
		{
			if (!(tag != Tag.Invalid) || this.items[i].HasTag(tag))
			{
				this.ConsumeIgnoringDisease(this.items[i]);
			}
		}
	}

	// Token: 0x06003644 RID: 13892 RVA: 0x00220298 File Offset: 0x0021E498
	public void ConsumeAndGetDisease(Tag tag, float amount, out float amount_consumed, out SimUtil.DiseaseInfo disease_info, out float aggregate_temperature)
	{
		SimHashes simHashes;
		this.ConsumeAndGetDisease(tag, amount, out amount_consumed, out disease_info, out aggregate_temperature, out simHashes);
	}

	// Token: 0x06003645 RID: 13893 RVA: 0x002202B4 File Offset: 0x0021E4B4
	public void ConsumeAndGetDisease(Tag tag, float amount, out float amount_consumed, out SimUtil.DiseaseInfo disease_info, out float aggregate_temperature, out SimHashes mostRelevantItemElement)
	{
		DebugUtil.Assert(tag.IsValid);
		amount_consumed = 0f;
		disease_info = SimUtil.DiseaseInfo.Invalid;
		mostRelevantItemElement = SimHashes.Vacuum;
		aggregate_temperature = 0f;
		bool flag = false;
		float num = 0f;
		int num2 = 0;
		while (num2 < this.items.Count && amount > 0f)
		{
			GameObject gameObject = this.items[num2];
			if (!(gameObject == null) && gameObject.HasTag(tag))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.Units > 0f)
				{
					flag = true;
					float num3 = Math.Min(component.Units, amount);
					global::Debug.Assert(num3 > 0f, "Delta amount was zero, which should be impossible.");
					aggregate_temperature = SimUtil.CalculateFinalTemperature(amount_consumed, aggregate_temperature, num3, component.Temperature);
					SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(component, num3 / component.Units);
					disease_info = SimUtil.CalculateFinalDiseaseInfo(disease_info, percentOfDisease);
					component.Units -= num3;
					component.ModifyDiseaseCount(-percentOfDisease.count, "Storage.ConsumeAndGetDisease");
					amount -= num3;
					amount_consumed += num3;
					if (num3 > num)
					{
						num = num3;
						mostRelevantItemElement = component.ElementID;
					}
				}
				if (component.Units <= 0f && !component.KeepZeroMassObject)
				{
					if (this.deleted_objects == null)
					{
						this.deleted_objects = new List<GameObject>();
					}
					this.deleted_objects.Add(gameObject);
				}
				base.Trigger(-1697596308, gameObject);
				Action<GameObject> onStorageChange = this.OnStorageChange;
				if (onStorageChange != null)
				{
					onStorageChange(gameObject);
				}
			}
			num2++;
		}
		if (!flag)
		{
			aggregate_temperature = base.GetComponent<PrimaryElement>().Temperature;
		}
		if (this.deleted_objects != null)
		{
			for (int i = 0; i < this.deleted_objects.Count; i++)
			{
				this.items.Remove(this.deleted_objects[i]);
				Util.KDestroyGameObject(this.deleted_objects[i]);
			}
			this.deleted_objects.Clear();
		}
	}

	// Token: 0x06003646 RID: 13894 RVA: 0x002204C0 File Offset: 0x0021E6C0
	public void ConsumeAndGetDisease(Recipe.Ingredient ingredient, out SimUtil.DiseaseInfo disease_info, out float temperature)
	{
		float num;
		this.ConsumeAndGetDisease(ingredient.tag, ingredient.amount, out num, out disease_info, out temperature);
	}

	// Token: 0x06003647 RID: 13895 RVA: 0x002204E4 File Offset: 0x0021E6E4
	public void ConsumeIgnoringDisease(Tag tag, float amount)
	{
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		this.ConsumeAndGetDisease(tag, amount, out num, out diseaseInfo, out num2);
	}

	// Token: 0x06003648 RID: 13896 RVA: 0x00220500 File Offset: 0x0021E700
	public void ConsumeIgnoringDisease(GameObject item_go)
	{
		if (this.items.Contains(item_go))
		{
			PrimaryElement component = item_go.GetComponent<PrimaryElement>();
			if (component != null && component.KeepZeroMassObject)
			{
				component.Units = 0f;
				component.ModifyDiseaseCount(-component.DiseaseCount, "consume item");
				base.Trigger(-1697596308, item_go);
				Action<GameObject> onStorageChange = this.OnStorageChange;
				if (onStorageChange == null)
				{
					return;
				}
				onStorageChange(item_go);
				return;
			}
			else
			{
				this.items.Remove(item_go);
				base.Trigger(-1697596308, item_go);
				Action<GameObject> onStorageChange2 = this.OnStorageChange;
				if (onStorageChange2 != null)
				{
					onStorageChange2(item_go);
				}
				item_go.DeleteObject();
			}
		}
	}

	// Token: 0x06003649 RID: 13897 RVA: 0x000C7CA6 File Offset: 0x000C5EA6
	public GameObject Drop(int ID)
	{
		return this.Drop(this.Find(ID), true);
	}

	// Token: 0x0600364A RID: 13898 RVA: 0x002205A4 File Offset: 0x0021E7A4
	private void OnDeath(object data)
	{
		List<GameObject> list = new List<GameObject>();
		bool vent_gas = true;
		bool dump_liquid = true;
		List<GameObject> collect_dropped_items = list;
		this.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
		if (this.onDestroyItemsDropped != null)
		{
			this.onDestroyItemsDropped(list);
		}
	}

	// Token: 0x0600364B RID: 13899 RVA: 0x000C7CB6 File Offset: 0x000C5EB6
	public bool IsFull()
	{
		return this.RemainingCapacity() <= 0f;
	}

	// Token: 0x0600364C RID: 13900 RVA: 0x000C7CC8 File Offset: 0x000C5EC8
	public bool IsEmpty()
	{
		return this.items.Count == 0;
	}

	// Token: 0x0600364D RID: 13901 RVA: 0x000C7CD8 File Offset: 0x000C5ED8
	public float Capacity()
	{
		return this.capacityKg;
	}

	// Token: 0x0600364E RID: 13902 RVA: 0x000C7CE0 File Offset: 0x000C5EE0
	public bool IsEndOfLife()
	{
		return this.endOfLife;
	}

	// Token: 0x0600364F RID: 13903 RVA: 0x002205E0 File Offset: 0x0021E7E0
	public float ExactMassStored()
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null))
			{
				PrimaryElement component = this.items[i].GetComponent<PrimaryElement>();
				if (component != null)
				{
					num += component.Units * component.MassPerUnit;
				}
			}
		}
		return num;
	}

	// Token: 0x06003650 RID: 13904 RVA: 0x000C7CE8 File Offset: 0x000C5EE8
	public float MassStored()
	{
		return (float)Mathf.RoundToInt(this.ExactMassStored() * 1000f) / 1000f;
	}

	// Token: 0x06003651 RID: 13905 RVA: 0x0022064C File Offset: 0x0021E84C
	public float UnitsStored()
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			if (!(this.items[i] == null))
			{
				PrimaryElement component = this.items[i].GetComponent<PrimaryElement>();
				if (component != null)
				{
					num += component.Units;
				}
			}
		}
		return (float)Mathf.RoundToInt(num * 1000f) / 1000f;
	}

	// Token: 0x06003652 RID: 13906 RVA: 0x002206C0 File Offset: 0x0021E8C0
	public bool Has(Tag tag)
	{
		bool result = false;
		foreach (GameObject gameObject in this.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.HasTag(tag) && component.Mass > 0f)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003653 RID: 13907 RVA: 0x0022073C File Offset: 0x0021E93C
	public PrimaryElement AddToPrimaryElement(SimHashes element, float additional_mass, float temperature)
	{
		PrimaryElement primaryElement = this.FindPrimaryElement(element);
		if (primaryElement != null)
		{
			float finalTemperature = GameUtil.GetFinalTemperature(primaryElement.Temperature, primaryElement.Mass, temperature, additional_mass);
			primaryElement.Mass += additional_mass;
			primaryElement.Temperature = finalTemperature;
		}
		return primaryElement;
	}

	// Token: 0x06003654 RID: 13908 RVA: 0x00220784 File Offset: 0x0021E984
	public PrimaryElement FindPrimaryElement(SimHashes element)
	{
		PrimaryElement result = null;
		foreach (GameObject gameObject in this.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.ElementID == element)
				{
					result = component;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003655 RID: 13909 RVA: 0x000C7D02 File Offset: 0x000C5F02
	public float RemainingCapacity()
	{
		return this.capacityKg - this.MassStored();
	}

	// Token: 0x06003656 RID: 13910 RVA: 0x000C7D11 File Offset: 0x000C5F11
	public bool GetOnlyFetchMarkedItems()
	{
		return this.onlyFetchMarkedItems;
	}

	// Token: 0x06003657 RID: 13911 RVA: 0x000C7D19 File Offset: 0x000C5F19
	public void SetOnlyFetchMarkedItems(bool is_set)
	{
		if (is_set != this.onlyFetchMarkedItems)
		{
			this.onlyFetchMarkedItems = is_set;
			this.UpdateFetchCategory();
			base.Trigger(644822890, null);
			base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("sweep", is_set);
		}
	}

	// Token: 0x06003658 RID: 13912 RVA: 0x000C7D53 File Offset: 0x000C5F53
	private void UpdateFetchCategory()
	{
		if (this.fetchCategory == Storage.FetchCategory.Building)
		{
			return;
		}
		this.fetchCategory = (this.onlyFetchMarkedItems ? Storage.FetchCategory.StorageSweepOnly : Storage.FetchCategory.GeneralStorage);
	}

	// Token: 0x06003659 RID: 13913 RVA: 0x000C7D70 File Offset: 0x000C5F70
	protected override void OnCleanUp()
	{
		if (this.items.Count != 0)
		{
			global::Debug.LogWarning("Storage for [" + base.gameObject.name + "] is being destroyed but it still contains items!", base.gameObject);
		}
		base.OnCleanUp();
	}

	// Token: 0x0600365A RID: 13914 RVA: 0x002207F0 File Offset: 0x0021E9F0
	private void OnQueueDestroyObject(object data)
	{
		this.endOfLife = true;
		List<GameObject> list = new List<GameObject>();
		bool vent_gas = true;
		bool dump_liquid = false;
		List<GameObject> collect_dropped_items = list;
		this.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
		if (this.onDestroyItemsDropped != null)
		{
			this.onDestroyItemsDropped(list);
		}
		this.OnCleanUp();
	}

	// Token: 0x0600365B RID: 13915 RVA: 0x000C7DAA File Offset: 0x000C5FAA
	public void Remove(GameObject go, bool do_disease_transfer = true)
	{
		this.items.Remove(go);
		if (do_disease_transfer)
		{
			this.TransferDiseaseWithObject(go);
		}
		base.Trigger(-1697596308, go);
		Action<GameObject> onStorageChange = this.OnStorageChange;
		if (onStorageChange != null)
		{
			onStorageChange(go);
		}
		this.ApplyStoredItemModifiers(go, false, false);
	}

	// Token: 0x0600365C RID: 13916 RVA: 0x0022083C File Offset: 0x0021EA3C
	public bool ForceStore(Tag tag, float amount)
	{
		global::Debug.Assert(amount < PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT);
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				gameObject.GetComponent<PrimaryElement>().Mass += amount;
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600365D RID: 13917 RVA: 0x002208A4 File Offset: 0x0021EAA4
	public float GetAmountAvailable(Tag tag)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				num += gameObject.GetComponent<PrimaryElement>().Units;
			}
		}
		return num;
	}

	// Token: 0x0600365E RID: 13918 RVA: 0x002208FC File Offset: 0x0021EAFC
	public float GetAmountAvailable(Tag tag, Tag[] forbiddenTags = null)
	{
		if (forbiddenTags == null)
		{
			return this.GetAmountAvailable(tag);
		}
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag) && !gameObject.HasAnyTags(forbiddenTags))
			{
				num += gameObject.GetComponent<PrimaryElement>().Units;
			}
		}
		return num;
	}

	// Token: 0x0600365F RID: 13919 RVA: 0x002208A4 File Offset: 0x0021EAA4
	public float GetUnitsAvailable(Tag tag)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				num += gameObject.GetComponent<PrimaryElement>().Units;
			}
		}
		return num;
	}

	// Token: 0x06003660 RID: 13920 RVA: 0x00220968 File Offset: 0x0021EB68
	public float GetMassAvailable(Tag tag)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null && gameObject.HasTag(tag))
			{
				num += gameObject.GetComponent<PrimaryElement>().Mass;
			}
		}
		return num;
	}

	// Token: 0x06003661 RID: 13921 RVA: 0x002209C0 File Offset: 0x0021EBC0
	public float GetMassAvailable(SimHashes element)
	{
		float num = 0f;
		for (int i = 0; i < this.items.Count; i++)
		{
			GameObject gameObject = this.items[i];
			if (gameObject != null)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.ElementID == element)
				{
					num += component.Mass;
				}
			}
		}
		return num;
	}

	// Token: 0x06003662 RID: 13922 RVA: 0x00220A1C File Offset: 0x0021EC1C
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (this.showDescriptor)
		{
			descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.STORAGECAPACITY, GameUtil.GetFormattedMass(this.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.STORAGECAPACITY, GameUtil.GetFormattedMass(this.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect, false));
		}
		return descriptors;
	}

	// Token: 0x06003663 RID: 13923 RVA: 0x00220A8C File Offset: 0x0021EC8C
	public static void MakeItemTemperatureInsulated(GameObject go, bool is_stored, bool is_initializing)
	{
		SimTemperatureTransfer component = go.GetComponent<SimTemperatureTransfer>();
		if (component == null)
		{
			return;
		}
		component.enabled = !is_stored;
	}

	// Token: 0x06003664 RID: 13924 RVA: 0x00220AB4 File Offset: 0x0021ECB4
	public static void MakeItemInvisible(GameObject go, bool is_stored, bool is_initializing)
	{
		if (is_initializing)
		{
			return;
		}
		bool flag = !is_stored;
		KAnimControllerBase component = go.GetComponent<KAnimControllerBase>();
		if (component != null && component.enabled != flag)
		{
			component.enabled = flag;
		}
		KSelectable component2 = go.GetComponent<KSelectable>();
		if (component2 != null && component2.enabled != flag)
		{
			component2.enabled = flag;
		}
	}

	// Token: 0x06003665 RID: 13925 RVA: 0x000C7DEA File Offset: 0x000C5FEA
	public static void MakeItemSealed(GameObject go, bool is_stored, bool is_initializing)
	{
		if (go != null)
		{
			if (is_stored)
			{
				go.GetComponent<KPrefabID>().AddTag(GameTags.Sealed, false);
				return;
			}
			go.GetComponent<KPrefabID>().RemoveTag(GameTags.Sealed);
		}
	}

	// Token: 0x06003666 RID: 13926 RVA: 0x000C7E1A File Offset: 0x000C601A
	public static void MakeItemPreserved(GameObject go, bool is_stored, bool is_initializing)
	{
		if (go != null)
		{
			if (is_stored)
			{
				go.GetComponent<KPrefabID>().AddTag(GameTags.Preserved, false);
				return;
			}
			go.GetComponent<KPrefabID>().RemoveTag(GameTags.Preserved);
		}
	}

	// Token: 0x06003667 RID: 13927 RVA: 0x00220B0C File Offset: 0x0021ED0C
	private void ApplyStoredItemModifiers(GameObject go, bool is_stored, bool is_initializing)
	{
		List<Storage.StoredItemModifier> list = this.defaultStoredItemModifers;
		for (int i = 0; i < list.Count; i++)
		{
			Storage.StoredItemModifier storedItemModifier = list[i];
			for (int j = 0; j < Storage.StoredItemModifierHandlers.Count; j++)
			{
				Storage.StoredItemModifierInfo storedItemModifierInfo = Storage.StoredItemModifierHandlers[j];
				if (storedItemModifierInfo.modifier == storedItemModifier)
				{
					storedItemModifierInfo.toggleState(go, is_stored, is_initializing);
					break;
				}
			}
		}
	}

	// Token: 0x06003668 RID: 13928 RVA: 0x00220B78 File Offset: 0x0021ED78
	protected virtual void OnCopySettings(object data)
	{
		Storage component = ((GameObject)data).GetComponent<Storage>();
		if (component != null)
		{
			this.SetOnlyFetchMarkedItems(component.onlyFetchMarkedItems);
		}
	}

	// Token: 0x06003669 RID: 13929 RVA: 0x00220BA8 File Offset: 0x0021EDA8
	private void OnPriorityChanged(PrioritySetting priority)
	{
		foreach (GameObject go in this.items)
		{
			go.Trigger(-1626373771, this);
		}
	}

	// Token: 0x0600366A RID: 13930 RVA: 0x00220C00 File Offset: 0x0021EE00
	private void OnReachableChanged(object data)
	{
		bool flag = (bool)data;
		KSelectable component = base.GetComponent<KSelectable>();
		if (flag)
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.StorageUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().BuildingStatusItems.StorageUnreachable, this);
	}

	// Token: 0x0600366B RID: 13931 RVA: 0x00220C4C File Offset: 0x0021EE4C
	public void SetContentsDeleteOffGrid(bool delete_off_grid)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			Pickupable component = this.items[i].GetComponent<Pickupable>();
			if (component != null)
			{
				component.deleteOffGrid = delete_off_grid;
			}
			Storage component2 = this.items[i].GetComponent<Storage>();
			if (component2 != null)
			{
				component2.SetContentsDeleteOffGrid(delete_off_grid);
			}
		}
	}

	// Token: 0x0600366C RID: 13932 RVA: 0x00220CB4 File Offset: 0x0021EEB4
	private bool ShouldSaveItem(GameObject go)
	{
		if (!this.shouldSaveItems)
		{
			return false;
		}
		bool result = false;
		if (go != null && go.GetComponent<SaveLoadRoot>() != null && go.GetComponent<PrimaryElement>().Mass > 0f)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x0600366D RID: 13933 RVA: 0x00220CFC File Offset: 0x0021EEFC
	public void Serialize(BinaryWriter writer)
	{
		int num = 0;
		int count = this.items.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.ShouldSaveItem(this.items[i]))
			{
				num++;
			}
		}
		writer.Write(num);
		if (num == 0)
		{
			return;
		}
		if (this.items != null && this.items.Count > 0)
		{
			for (int j = 0; j < this.items.Count; j++)
			{
				GameObject gameObject = this.items[j];
				if (this.ShouldSaveItem(gameObject))
				{
					SaveLoadRoot component = gameObject.GetComponent<SaveLoadRoot>();
					if (component != null)
					{
						string name = gameObject.GetComponent<KPrefabID>().GetSaveLoadTag().Name;
						writer.WriteKleiString(name);
						component.Save(writer);
					}
					else
					{
						global::Debug.Log("Tried to save obj in storage but obj has no SaveLoadRoot", gameObject);
					}
				}
			}
		}
	}

	// Token: 0x0600366E RID: 13934 RVA: 0x00220DD8 File Offset: 0x0021EFD8
	public void Deserialize(IReader reader)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		this.ClearItems();
		int num4 = reader.ReadInt32();
		this.items = new List<GameObject>(num4);
		for (int i = 0; i < num4; i++)
		{
			float realtimeSinceStartup2 = Time.realtimeSinceStartup;
			Tag tag = TagManager.Create(reader.ReadKleiString());
			SaveLoadRoot saveLoadRoot = SaveLoadRoot.Load(tag, reader);
			num += Time.realtimeSinceStartup - realtimeSinceStartup2;
			if (saveLoadRoot != null)
			{
				KBatchedAnimController component = saveLoadRoot.GetComponent<KBatchedAnimController>();
				if (component != null)
				{
					component.enabled = false;
				}
				saveLoadRoot.SetRegistered(false);
				float realtimeSinceStartup3 = Time.realtimeSinceStartup;
				GameObject gameObject = this.Store(saveLoadRoot.gameObject, true, true, false, true);
				num2 += Time.realtimeSinceStartup - realtimeSinceStartup3;
				if (gameObject != null)
				{
					Pickupable component2 = gameObject.GetComponent<Pickupable>();
					if (component2 != null)
					{
						float realtimeSinceStartup4 = Time.realtimeSinceStartup;
						component2.OnStore(this);
						num3 += Time.realtimeSinceStartup - realtimeSinceStartup4;
					}
					Storable component3 = gameObject.GetComponent<Storable>();
					if (component3 != null)
					{
						float realtimeSinceStartup5 = Time.realtimeSinceStartup;
						component3.OnStore(this);
						num3 += Time.realtimeSinceStartup - realtimeSinceStartup5;
					}
					if (this.dropOnLoad)
					{
						this.Drop(saveLoadRoot.gameObject, true);
					}
				}
			}
			else
			{
				global::Debug.LogWarning("Tried to deserialize " + tag.ToString() + " into storage but failed", base.gameObject);
			}
		}
	}

	// Token: 0x0600366F RID: 13935 RVA: 0x00220F54 File Offset: 0x0021F154
	private void ClearItems()
	{
		foreach (GameObject go in this.items)
		{
			go.DeleteObject();
		}
		this.items.Clear();
	}

	// Token: 0x06003670 RID: 13936 RVA: 0x00220FB0 File Offset: 0x0021F1B0
	public void UpdateStoredItemCachedCells()
	{
		foreach (GameObject gameObject in this.items)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			if (component != null)
			{
				component.UpdateCachedCellFromStoragePosition();
			}
		}
	}

	// Token: 0x04002562 RID: 9570
	public bool allowItemRemoval;

	// Token: 0x04002563 RID: 9571
	public bool ignoreSourcePriority;

	// Token: 0x04002564 RID: 9572
	public bool onlyTransferFromLowerPriority;

	// Token: 0x04002565 RID: 9573
	public float capacityKg = 20000f;

	// Token: 0x04002566 RID: 9574
	public bool showDescriptor;

	// Token: 0x04002568 RID: 9576
	public bool doDiseaseTransfer = true;

	// Token: 0x04002569 RID: 9577
	public List<Tag> storageFilters;

	// Token: 0x0400256A RID: 9578
	public bool useGunForDelivery = true;

	// Token: 0x0400256B RID: 9579
	public bool sendOnStoreOnSpawn;

	// Token: 0x0400256C RID: 9580
	public bool showInUI = true;

	// Token: 0x0400256D RID: 9581
	public bool storeDropsFromButcherables;

	// Token: 0x0400256E RID: 9582
	public bool allowClearable;

	// Token: 0x0400256F RID: 9583
	public bool showCapacityStatusItem;

	// Token: 0x04002570 RID: 9584
	public bool showCapacityAsMainStatus;

	// Token: 0x04002571 RID: 9585
	public bool showUnreachableStatus;

	// Token: 0x04002572 RID: 9586
	public bool showSideScreenTitleBar;

	// Token: 0x04002573 RID: 9587
	public bool useWideOffsets;

	// Token: 0x04002574 RID: 9588
	public Action<List<GameObject>> onDestroyItemsDropped;

	// Token: 0x04002575 RID: 9589
	public Action<GameObject> OnStorageChange;

	// Token: 0x04002576 RID: 9590
	public Vector2 dropOffset = Vector2.zero;

	// Token: 0x04002577 RID: 9591
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04002578 RID: 9592
	public Vector2 gunTargetOffset;

	// Token: 0x04002579 RID: 9593
	public Storage.FetchCategory fetchCategory;

	// Token: 0x0400257A RID: 9594
	public int storageNetworkID = -1;

	// Token: 0x0400257B RID: 9595
	public Tag storageID = GameTags.StoragesIds.DefaultStorage;

	// Token: 0x0400257C RID: 9596
	public float storageFullMargin;

	// Token: 0x0400257D RID: 9597
	public Vector3 storageFXOffset = Vector3.zero;

	// Token: 0x0400257E RID: 9598
	private static readonly EventSystem.IntraObjectHandler<Storage> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Storage>(delegate(Storage component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x0400257F RID: 9599
	public Storage.FXPrefix fxPrefix;

	// Token: 0x04002580 RID: 9600
	public List<GameObject> items = new List<GameObject>();

	// Token: 0x04002581 RID: 9601
	[MyCmpGet]
	public Prioritizable prioritizable;

	// Token: 0x04002582 RID: 9602
	[MyCmpGet]
	public Automatable automatable;

	// Token: 0x04002583 RID: 9603
	[MyCmpGet]
	protected PrimaryElement primaryElement;

	// Token: 0x04002584 RID: 9604
	public bool dropOnLoad;

	// Token: 0x04002585 RID: 9605
	protected float maxKGPerItem = float.MaxValue;

	// Token: 0x04002586 RID: 9606
	private bool endOfLife;

	// Token: 0x04002587 RID: 9607
	public bool allowSettingOnlyFetchMarkedItems = true;

	// Token: 0x04002588 RID: 9608
	[Serialize]
	private bool onlyFetchMarkedItems;

	// Token: 0x04002589 RID: 9609
	[Serialize]
	private bool shouldSaveItems = true;

	// Token: 0x0400258A RID: 9610
	public float storageWorkTime = 1.5f;

	// Token: 0x0400258B RID: 9611
	private static readonly List<Storage.StoredItemModifierInfo> StoredItemModifierHandlers = new List<Storage.StoredItemModifierInfo>
	{
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Hide, new Action<GameObject, bool, bool>(Storage.MakeItemInvisible)),
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Insulate, new Action<GameObject, bool, bool>(Storage.MakeItemTemperatureInsulated)),
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Seal, new Action<GameObject, bool, bool>(Storage.MakeItemSealed)),
		new Storage.StoredItemModifierInfo(Storage.StoredItemModifier.Preserve, new Action<GameObject, bool, bool>(Storage.MakeItemPreserved))
	};

	// Token: 0x0400258C RID: 9612
	[SerializeField]
	private List<Storage.StoredItemModifier> defaultStoredItemModifers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide
	};

	// Token: 0x0400258D RID: 9613
	public static readonly List<Storage.StoredItemModifier> StandardSealedStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal
	};

	// Token: 0x0400258E RID: 9614
	public static readonly List<Storage.StoredItemModifier> StandardFabricatorStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve
	};

	// Token: 0x0400258F RID: 9615
	public static readonly List<Storage.StoredItemModifier> StandardInsulatedStorage = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal,
		Storage.StoredItemModifier.Insulate
	};

	// Token: 0x04002591 RID: 9617
	private static StatusItem capacityStatusItem;

	// Token: 0x04002592 RID: 9618
	private static readonly EventSystem.IntraObjectHandler<Storage> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<Storage>(GameTags.Dead, delegate(Storage component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x04002593 RID: 9619
	private static readonly EventSystem.IntraObjectHandler<Storage> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<Storage>(delegate(Storage component, object data)
	{
		component.OnQueueDestroyObject(data);
	});

	// Token: 0x04002594 RID: 9620
	private static readonly EventSystem.IntraObjectHandler<Storage> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Storage>(delegate(Storage component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04002595 RID: 9621
	private List<GameObject> deleted_objects;

	// Token: 0x02000B54 RID: 2900
	public enum StoredItemModifier
	{
		// Token: 0x04002597 RID: 9623
		Insulate,
		// Token: 0x04002598 RID: 9624
		Hide,
		// Token: 0x04002599 RID: 9625
		Seal,
		// Token: 0x0400259A RID: 9626
		Preserve
	}

	// Token: 0x02000B55 RID: 2901
	public enum FetchCategory
	{
		// Token: 0x0400259C RID: 9628
		Building,
		// Token: 0x0400259D RID: 9629
		GeneralStorage,
		// Token: 0x0400259E RID: 9630
		StorageSweepOnly
	}

	// Token: 0x02000B56 RID: 2902
	public enum FXPrefix
	{
		// Token: 0x040025A0 RID: 9632
		Delivered,
		// Token: 0x040025A1 RID: 9633
		PickedUp
	}

	// Token: 0x02000B57 RID: 2903
	private struct StoredItemModifierInfo
	{
		// Token: 0x06003673 RID: 13939 RVA: 0x000C7E4A File Offset: 0x000C604A
		public StoredItemModifierInfo(Storage.StoredItemModifier modifier, Action<GameObject, bool, bool> toggle_state)
		{
			this.modifier = modifier;
			this.toggleState = toggle_state;
		}

		// Token: 0x040025A2 RID: 9634
		public Storage.StoredItemModifier modifier;

		// Token: 0x040025A3 RID: 9635
		public Action<GameObject, bool, bool> toggleState;
	}
}
