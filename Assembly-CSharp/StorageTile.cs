using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000FFC RID: 4092
public class StorageTile : GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>
{
	// Token: 0x0600526D RID: 21101 RVA: 0x002830A4 File Offset: 0x002812A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.PlayAnim("on").EventHandler(GameHashes.OnStorageChange, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.OnStorageChanged)).EventHandler(GameHashes.StorageTileTargetItemChanged, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals));
		this.idle.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals)).EventTransition(GameHashes.OnStorageChange, this.awaitingDelivery, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingDelivery)).EventTransition(GameHashes.StorageTileTargetItemChanged, this.change, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingForSettingChange));
		this.change.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals)).EventTransition(GameHashes.StorageTileTargetItemChanged, this.idle, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.NoLongerAwaitingForSettingChange)).DefaultState(this.change.awaitingSettingsChange);
		this.change.awaitingSettingsChange.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.StartWorkChore)).Exit(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.CancelWorkChore)).ToggleStatusItem(Db.Get().BuildingStatusItems.ChangeStorageTileTarget, null).WorkableCompleteTransition((StorageTile.Instance smi) => smi.GetWorkable(), this.change.complete);
		this.change.complete.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.ApplySettings)).Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.DropUndesiredItems)).EnterTransition(this.idle, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.HasAnyDesiredItemStored)).EnterTransition(this.awaitingDelivery, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingDelivery));
		this.awaitingDelivery.Enter(new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State.Callback(StorageTile.RefreshContentVisuals)).EventTransition(GameHashes.OnStorageChange, this.idle, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.HasAnyDesiredItemStored)).EventTransition(GameHashes.StorageTileTargetItemChanged, this.change, new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.Transition.ConditionCallback(StorageTile.IsAwaitingForSettingChange));
	}

	// Token: 0x0600526E RID: 21102 RVA: 0x000DA1F1 File Offset: 0x000D83F1
	public static void DropUndesiredItems(StorageTile.Instance smi)
	{
		smi.DropUndesiredItems();
	}

	// Token: 0x0600526F RID: 21103 RVA: 0x000DA1F9 File Offset: 0x000D83F9
	public static void ApplySettings(StorageTile.Instance smi)
	{
		smi.ApplySettings();
	}

	// Token: 0x06005270 RID: 21104 RVA: 0x000DA201 File Offset: 0x000D8401
	public static void StartWorkChore(StorageTile.Instance smi)
	{
		smi.StartChangeSettingChore();
	}

	// Token: 0x06005271 RID: 21105 RVA: 0x000DA209 File Offset: 0x000D8409
	public static void CancelWorkChore(StorageTile.Instance smi)
	{
		smi.CanceChangeSettingChore();
	}

	// Token: 0x06005272 RID: 21106 RVA: 0x000DA211 File Offset: 0x000D8411
	public static void RefreshContentVisuals(StorageTile.Instance smi)
	{
		smi.UpdateContentSymbol();
	}

	// Token: 0x06005273 RID: 21107 RVA: 0x000DA219 File Offset: 0x000D8419
	public static bool IsAwaitingForSettingChange(StorageTile.Instance smi)
	{
		return smi.IsPendingChange;
	}

	// Token: 0x06005274 RID: 21108 RVA: 0x000DA221 File Offset: 0x000D8421
	public static bool NoLongerAwaitingForSettingChange(StorageTile.Instance smi)
	{
		return !smi.IsPendingChange;
	}

	// Token: 0x06005275 RID: 21109 RVA: 0x000DA22C File Offset: 0x000D842C
	public static bool HasAnyDesiredItemStored(StorageTile.Instance smi)
	{
		return smi.HasAnyDesiredContents;
	}

	// Token: 0x06005276 RID: 21110 RVA: 0x000DA234 File Offset: 0x000D8434
	public static void OnStorageChanged(StorageTile.Instance smi)
	{
		smi.PlayDoorAnimation();
		StorageTile.RefreshContentVisuals(smi);
	}

	// Token: 0x06005277 RID: 21111 RVA: 0x000DA242 File Offset: 0x000D8442
	public static bool IsAwaitingDelivery(StorageTile.Instance smi)
	{
		return !smi.IsPendingChange && !smi.HasAnyDesiredContents;
	}

	// Token: 0x04003A32 RID: 14898
	public const string METER_TARGET = "meter_target";

	// Token: 0x04003A33 RID: 14899
	public const string METER_ANIMATION = "meter";

	// Token: 0x04003A34 RID: 14900
	public static HashedString DOOR_SYMBOL_NAME = new HashedString("storage_door");

	// Token: 0x04003A35 RID: 14901
	public static HashedString ITEM_SYMBOL_TARGET = new HashedString("meter_target_object");

	// Token: 0x04003A36 RID: 14902
	public static HashedString ITEM_SYMBOL_NAME = new HashedString("object");

	// Token: 0x04003A37 RID: 14903
	public const string ITEM_SYMBOL_ANIMATION = "meter_object";

	// Token: 0x04003A38 RID: 14904
	public static HashedString ITEM_PREVIEW_SYMBOL_TARGET = new HashedString("meter_target_object_ui");

	// Token: 0x04003A39 RID: 14905
	public static HashedString ITEM_PREVIEW_SYMBOL_NAME = new HashedString("object_ui");

	// Token: 0x04003A3A RID: 14906
	public const string ITEM_PREVIEW_SYMBOL_ANIMATION = "meter_object_ui";

	// Token: 0x04003A3B RID: 14907
	public static HashedString ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME = new HashedString("placeholder");

	// Token: 0x04003A3C RID: 14908
	public const string DEFAULT_ANIMATION_NAME = "on";

	// Token: 0x04003A3D RID: 14909
	public const string STORAGE_CHANGE_ANIMATION_NAME = "door";

	// Token: 0x04003A3E RID: 14910
	public const string SYMBOL_ANIMATION_NAME_AWAITING_DELIVERY = "ui";

	// Token: 0x04003A3F RID: 14911
	public static Tag INVALID_TAG = GameTags.Void;

	// Token: 0x04003A40 RID: 14912
	private StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.TagParameter TargetItemTag = new StateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.TagParameter(StorageTile.INVALID_TAG);

	// Token: 0x04003A41 RID: 14913
	public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State idle;

	// Token: 0x04003A42 RID: 14914
	public StorageTile.SettingsChangeStates change;

	// Token: 0x04003A43 RID: 14915
	public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State awaitingDelivery;

	// Token: 0x02000FFD RID: 4093
	public class SpecificItemTagSizeInstruction
	{
		// Token: 0x0600527A RID: 21114 RVA: 0x000DA26F File Offset: 0x000D846F
		public SpecificItemTagSizeInstruction(Tag tag, float size)
		{
			this.tag = tag;
			this.sizeMultiplier = size;
		}

		// Token: 0x04003A44 RID: 14916
		public Tag tag;

		// Token: 0x04003A45 RID: 14917
		public float sizeMultiplier;
	}

	// Token: 0x02000FFE RID: 4094
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600527B RID: 21115 RVA: 0x00283324 File Offset: 0x00281524
		public StorageTile.SpecificItemTagSizeInstruction GetSizeInstructionForObject(GameObject obj)
		{
			if (this.specialItemCases == null)
			{
				return null;
			}
			KPrefabID component = obj.GetComponent<KPrefabID>();
			foreach (StorageTile.SpecificItemTagSizeInstruction specificItemTagSizeInstruction in this.specialItemCases)
			{
				if (component.HasTag(specificItemTagSizeInstruction.tag))
				{
					return specificItemTagSizeInstruction;
				}
			}
			return null;
		}

		// Token: 0x04003A46 RID: 14918
		public float MaxCapacity;

		// Token: 0x04003A47 RID: 14919
		public StorageTile.SpecificItemTagSizeInstruction[] specialItemCases;
	}

	// Token: 0x02000FFF RID: 4095
	public class SettingsChangeStates : GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State
	{
		// Token: 0x04003A48 RID: 14920
		public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State awaitingSettingsChange;

		// Token: 0x04003A49 RID: 14921
		public GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.State complete;
	}

	// Token: 0x02001000 RID: 4096
	public new class Instance : GameStateMachine<StorageTile, StorageTile.Instance, IStateMachineTarget, StorageTile.Def>.GameInstance, IUserControlledCapacity
	{
		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x0600527E RID: 21118 RVA: 0x000DA28D File Offset: 0x000D848D
		public Tag TargetTag
		{
			get
			{
				return base.smi.sm.TargetItemTag.Get(base.smi);
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x0600527F RID: 21119 RVA: 0x000DA2AA File Offset: 0x000D84AA
		public bool HasContents
		{
			get
			{
				return this.storage.MassStored() > 0f;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06005280 RID: 21120 RVA: 0x000DA2BE File Offset: 0x000D84BE
		public bool HasAnyDesiredContents
		{
			get
			{
				if (!(this.TargetTag == StorageTile.INVALID_TAG))
				{
					return this.AmountOfDesiredContentStored > 0f;
				}
				return !this.HasContents;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06005281 RID: 21121 RVA: 0x000DA2E9 File Offset: 0x000D84E9
		public float AmountOfDesiredContentStored
		{
			get
			{
				if (!(this.TargetTag == StorageTile.INVALID_TAG))
				{
					return this.storage.GetMassAvailable(this.TargetTag);
				}
				return 0f;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06005282 RID: 21122 RVA: 0x000DA314 File Offset: 0x000D8514
		public bool IsPendingChange
		{
			get
			{
				return this.GetTreeFilterableCurrentTag() != this.TargetTag;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06005283 RID: 21123 RVA: 0x000DA327 File Offset: 0x000D8527
		// (set) Token: 0x06005284 RID: 21124 RVA: 0x000DA33F File Offset: 0x000D853F
		public float UserMaxCapacity
		{
			get
			{
				return Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
			}
			set
			{
				this.userMaxCapacity = value;
				this.filteredStorage.FilterChanged();
				this.RefreshAmountMeter();
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06005285 RID: 21125 RVA: 0x000DA359 File Offset: 0x000D8559
		public float AmountStored
		{
			get
			{
				return this.storage.MassStored();
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06005286 RID: 21126 RVA: 0x000C18F8 File Offset: 0x000BFAF8
		public float MinCapacity
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06005287 RID: 21127 RVA: 0x000DA366 File Offset: 0x000D8566
		public float MaxCapacity
		{
			get
			{
				return base.def.MaxCapacity;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06005288 RID: 21128 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool WholeValues
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06005289 RID: 21129 RVA: 0x000CDA3B File Offset: 0x000CBC3B
		public LocString CapacityUnits
		{
			get
			{
				return GameUtil.GetCurrentMassUnit(false);
			}
		}

		// Token: 0x0600528A RID: 21130 RVA: 0x000DA373 File Offset: 0x000D8573
		private Tag GetTreeFilterableCurrentTag()
		{
			if (this.treeFilterable.GetTags() != null && this.treeFilterable.GetTags().Count != 0)
			{
				return this.treeFilterable.GetTags().GetRandom<Tag>();
			}
			return StorageTile.INVALID_TAG;
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x000DA3AA File Offset: 0x000D85AA
		public StorageTileSwitchItemWorkable GetWorkable()
		{
			return base.smi.gameObject.GetComponent<StorageTileSwitchItemWorkable>();
		}

		// Token: 0x0600528C RID: 21132 RVA: 0x0028336C File Offset: 0x0028156C
		public Instance(IStateMachineTarget master, StorageTile.Def def) : base(master, def)
		{
			this.itemSymbol = this.CreateSymbolOverrideCapsule(StorageTile.ITEM_SYMBOL_TARGET, StorageTile.ITEM_SYMBOL_NAME, "meter_object");
			this.itemSymbol.usingNewSymbolOverrideSystem = true;
			this.itemSymbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(this.itemSymbol.gameObject);
			this.itemPreviewSymbol = this.CreateSymbolOverrideCapsule(StorageTile.ITEM_PREVIEW_SYMBOL_TARGET, StorageTile.ITEM_PREVIEW_SYMBOL_NAME, "meter_object_ui");
			this.defaultItemSymbolScale = this.itemSymbol.transform.localScale.x;
			this.defaultItemLocalPosition = this.itemSymbol.transform.localPosition;
			this.doorSymbol = this.CreateEmptyKAnimController(StorageTile.DOOR_SYMBOL_NAME.ToString());
			this.doorSymbol.initialAnim = "on";
			foreach (KAnim.Build.Symbol symbol in this.doorSymbol.AnimFiles[0].GetData().build.symbols)
			{
				this.doorSymbol.SetSymbolVisiblity(symbol.hash, symbol.hash == StorageTile.DOOR_SYMBOL_NAME);
			}
			this.doorSymbol.transform.SetParent(this.animController.transform, false);
			this.doorSymbol.transform.SetLocalPosition(-Vector3.forward * 0.05f);
			this.doorSymbol.onAnimComplete += this.OnDoorAnimationCompleted;
			this.doorSymbol.gameObject.SetActive(true);
			this.animController.SetSymbolVisiblity(StorageTile.DOOR_SYMBOL_NAME, false);
			this.doorAnimLink = new KAnimLink(this.animController, this.doorSymbol);
			this.amountMeter = new MeterController(this.animController, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
			this.filteredStorage = new FilteredStorage(this.storage, null, this, false, fetch_chore_type);
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			base.Subscribe(1606648047, new Action<object>(this.OnObjectReplaced));
		}

		// Token: 0x0600528D RID: 21133 RVA: 0x000DA3BC File Offset: 0x000D85BC
		public override void StartSM()
		{
			base.StartSM();
			this.filteredStorage.FilterChanged();
		}

		// Token: 0x0600528E RID: 21134 RVA: 0x002835D4 File Offset: 0x002817D4
		private void OnObjectReplaced(object data)
		{
			Constructable.ReplaceCallbackParameters replaceCallbackParameters = (Constructable.ReplaceCallbackParameters)data;
			List<GameObject> list = new List<GameObject>();
			Storage storage = this.storage;
			bool vent_gas = false;
			bool dump_liquid = false;
			List<GameObject> collect_dropped_items = list;
			storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
			if (replaceCallbackParameters.Worker != null)
			{
				foreach (GameObject gameObject in list)
				{
					gameObject.GetComponent<Pickupable>().Trigger(580035959, replaceCallbackParameters.Worker);
				}
			}
		}

		// Token: 0x0600528F RID: 21135 RVA: 0x000DA3CF File Offset: 0x000D85CF
		private void OnDoorAnimationCompleted(HashedString animName)
		{
			if (animName == "door")
			{
				this.doorSymbol.Play("on", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x00283668 File Offset: 0x00281868
		private KBatchedAnimController CreateEmptyKAnimController(string name)
		{
			GameObject gameObject = new GameObject(base.gameObject.name + "-" + name);
			gameObject.SetActive(false);
			KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
			kbatchedAnimController.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("storagetile_kanim")
			};
			kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingFront;
			return kbatchedAnimController;
		}

		// Token: 0x06005291 RID: 21137 RVA: 0x002836C4 File Offset: 0x002818C4
		private KBatchedAnimController CreateSymbolOverrideCapsule(HashedString symbolTarget, HashedString symbolName, string animationName)
		{
			KBatchedAnimController kbatchedAnimController = this.CreateEmptyKAnimController(symbolTarget.ToString());
			kbatchedAnimController.initialAnim = animationName;
			bool flag;
			Matrix4x4 symbolTransform = this.animController.GetSymbolTransform(symbolTarget, out flag);
			bool flag2;
			Matrix2x3 symbolLocalTransform = this.animController.GetSymbolLocalTransform(symbolTarget, out flag2);
			Vector3 position = symbolTransform.GetColumn(3);
			Vector3 localScale = Vector3.one * symbolLocalTransform.m00;
			kbatchedAnimController.transform.SetParent(base.transform, false);
			kbatchedAnimController.transform.SetPosition(position);
			Vector3 localPosition = kbatchedAnimController.transform.localPosition;
			localPosition.z = -0.0025f;
			kbatchedAnimController.transform.localPosition = localPosition;
			kbatchedAnimController.transform.localScale = localScale;
			kbatchedAnimController.gameObject.SetActive(false);
			this.animController.SetSymbolVisiblity(symbolTarget, false);
			return kbatchedAnimController;
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x0028379C File Offset: 0x0028199C
		private void OnCopySettings(object sourceOBJ)
		{
			if (sourceOBJ != null)
			{
				StorageTile.Instance smi = ((GameObject)sourceOBJ).GetSMI<StorageTile.Instance>();
				if (smi != null)
				{
					this.SetTargetItem(smi.TargetTag);
					this.UserMaxCapacity = smi.UserMaxCapacity;
				}
			}
		}

		// Token: 0x06005293 RID: 21139 RVA: 0x002837D4 File Offset: 0x002819D4
		public void RefreshAmountMeter()
		{
			float positionPercent = (this.UserMaxCapacity == 0f) ? 0f : Mathf.Clamp(this.AmountOfDesiredContentStored / this.UserMaxCapacity, 0f, 1f);
			this.amountMeter.SetPositionPercent(positionPercent);
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x000DA403 File Offset: 0x000D8603
		public void PlayDoorAnimation()
		{
			this.doorSymbol.Play("door", KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x000DA425 File Offset: 0x000D8625
		public void SetTargetItem(Tag tag)
		{
			base.sm.TargetItemTag.Set(tag, this, false);
			base.gameObject.Trigger(-2076953849, null);
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x00283820 File Offset: 0x00281A20
		public void ApplySettings()
		{
			Tag treeFilterableCurrentTag = this.GetTreeFilterableCurrentTag();
			this.treeFilterable.RemoveTagFromFilter(treeFilterableCurrentTag);
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x00283840 File Offset: 0x00281A40
		public void DropUndesiredItems()
		{
			Vector3 position = Grid.CellToPos(this.GetWorkable().LastCellWorkerUsed) + Vector3.right * Grid.CellSizeInMeters * 0.5f + Vector3.up * Grid.CellSizeInMeters * 0.5f;
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			if (this.TargetTag != StorageTile.INVALID_TAG)
			{
				this.treeFilterable.AddTagToFilter(this.TargetTag);
				GameObject[] array = this.storage.DropUnlessHasTag(this.TargetTag);
				if (array != null)
				{
					GameObject[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i].transform.SetPosition(position);
					}
				}
			}
			else
			{
				this.storage.DropAll(position, false, false, default(Vector3), true, null);
			}
			this.storage.DropUnlessHasTag(this.TargetTag);
		}

		// Token: 0x06005298 RID: 21144 RVA: 0x00283930 File Offset: 0x00281B30
		public void UpdateContentSymbol()
		{
			this.RefreshAmountMeter();
			bool flag = this.TargetTag == StorageTile.INVALID_TAG;
			if (flag && !this.HasContents)
			{
				this.itemSymbol.gameObject.SetActive(false);
				this.itemPreviewSymbol.gameObject.SetActive(false);
				this.animController.SetSymbolVisiblity(StorageTile.ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME, false);
				return;
			}
			bool flag2 = !flag && (this.IsPendingChange || !this.HasAnyDesiredContents);
			string text = "";
			GameObject gameObject = (this.TargetTag == StorageTile.INVALID_TAG) ? Assets.GetPrefab(this.storage.items[0].PrefabID()) : Assets.GetPrefab(this.TargetTag);
			KAnimFile animFileFromPrefabWithTag = global::Def.GetAnimFileFromPrefabWithTag(gameObject, flag2 ? "ui" : "", out text);
			this.animController.SetSymbolVisiblity(StorageTile.ITEM_PREVIEW_BACKGROUND_SYMBOL_NAME, flag2);
			this.itemPreviewSymbol.gameObject.SetActive(flag2);
			this.itemSymbol.gameObject.SetActive(!flag2);
			if (flag2)
			{
				this.itemPreviewSymbol.SwapAnims(new KAnimFile[]
				{
					animFileFromPrefabWithTag
				});
				this.itemPreviewSymbol.Play(text, KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			if (gameObject.HasTag(GameTags.Egg))
			{
				string text2 = text;
				if (!string.IsNullOrEmpty(text2))
				{
					this.itemSymbolOverrideController.ApplySymbolOverridesByAffix(animFileFromPrefabWithTag, text2, null, 0);
				}
				text = gameObject.GetComponent<KBatchedAnimController>().initialAnim;
			}
			else
			{
				this.itemSymbolOverrideController.RemoveAllSymbolOverrides(0);
				text = gameObject.GetComponent<KBatchedAnimController>().initialAnim;
			}
			this.itemSymbol.SwapAnims(new KAnimFile[]
			{
				animFileFromPrefabWithTag
			});
			this.itemSymbol.Play(text, KAnim.PlayMode.Once, 1f, 0f);
			StorageTile.SpecificItemTagSizeInstruction sizeInstructionForObject = base.def.GetSizeInstructionForObject(gameObject);
			this.itemSymbol.transform.localScale = Vector3.one * ((sizeInstructionForObject != null) ? sizeInstructionForObject.sizeMultiplier : this.defaultItemSymbolScale);
			KCollider2D component = gameObject.GetComponent<KCollider2D>();
			Vector3 localPosition = this.defaultItemLocalPosition;
			localPosition.y += ((component == null || component is KCircleCollider2D) ? 0f : (-component.offset.y * 0.5f));
			this.itemSymbol.transform.localPosition = localPosition;
		}

		// Token: 0x06005299 RID: 21145 RVA: 0x000DA44C File Offset: 0x000D864C
		private void AbortChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Change settings Chore aborted");
				this.chore = null;
			}
		}

		// Token: 0x0600529A RID: 21146 RVA: 0x00283B9C File Offset: 0x00281D9C
		public void StartChangeSettingChore()
		{
			this.AbortChore();
			this.chore = new WorkChore<StorageTileSwitchItemWorkable>(Db.Get().ChoreTypes.Toggle, this.GetWorkable(), null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x0600529B RID: 21147 RVA: 0x000DA46D File Offset: 0x000D866D
		public void CanceChangeSettingChore()
		{
			this.AbortChore();
		}

		// Token: 0x04003A4A RID: 14922
		[Serialize]
		private float userMaxCapacity = float.PositiveInfinity;

		// Token: 0x04003A4B RID: 14923
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04003A4C RID: 14924
		[MyCmpGet]
		private KBatchedAnimController animController;

		// Token: 0x04003A4D RID: 14925
		[MyCmpGet]
		private TreeFilterable treeFilterable;

		// Token: 0x04003A4E RID: 14926
		private FilteredStorage filteredStorage;

		// Token: 0x04003A4F RID: 14927
		private Chore chore;

		// Token: 0x04003A50 RID: 14928
		private MeterController amountMeter;

		// Token: 0x04003A51 RID: 14929
		private KBatchedAnimController doorSymbol;

		// Token: 0x04003A52 RID: 14930
		private KBatchedAnimController itemSymbol;

		// Token: 0x04003A53 RID: 14931
		private SymbolOverrideController itemSymbolOverrideController;

		// Token: 0x04003A54 RID: 14932
		private KBatchedAnimController itemPreviewSymbol;

		// Token: 0x04003A55 RID: 14933
		private KAnimLink doorAnimLink;

		// Token: 0x04003A56 RID: 14934
		private string choreTypeID = Db.Get().ChoreTypes.StorageFetch.Id;

		// Token: 0x04003A57 RID: 14935
		private float defaultItemSymbolScale = -1f;

		// Token: 0x04003A58 RID: 14936
		private Vector3 defaultItemLocalPosition = Vector3.zero;
	}
}
