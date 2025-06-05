using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000D79 RID: 3449
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Door")]
public class Door : Workable, ISaveLoadable, ISim200ms, INavDoor
{
	// Token: 0x060042E8 RID: 17128 RVA: 0x00250804 File Offset: 0x0024EA04
	private void OnCopySettings(object data)
	{
		Door component = ((GameObject)data).GetComponent<Door>();
		if (component != null)
		{
			this.QueueStateChange(component.requestedState);
		}
	}

	// Token: 0x060042E9 RID: 17129 RVA: 0x000CFB16 File Offset: 0x000CDD16
	public Door()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x1700034F RID: 847
	// (get) Token: 0x060042EA RID: 17130 RVA: 0x000CFB46 File Offset: 0x000CDD46
	public Door.ControlState CurrentState
	{
		get
		{
			return this.controlState;
		}
	}

	// Token: 0x17000350 RID: 848
	// (get) Token: 0x060042EB RID: 17131 RVA: 0x000CFB4E File Offset: 0x000CDD4E
	public Door.ControlState RequestedState
	{
		get
		{
			return this.requestedState;
		}
	}

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x060042EC RID: 17132 RVA: 0x000CFB56 File Offset: 0x000CDD56
	public bool ShouldBlockFallingSand
	{
		get
		{
			return this.rotatable.GetOrientation() != this.verticalOrientation;
		}
	}

	// Token: 0x17000352 RID: 850
	// (get) Token: 0x060042ED RID: 17133 RVA: 0x000CFB6E File Offset: 0x000CDD6E
	public bool isSealed
	{
		get
		{
			return this.controller != null && this.controller.sm.isSealed.Get(this.controller);
		}
	}

	// Token: 0x060042EE RID: 17134 RVA: 0x00250834 File Offset: 0x0024EA34
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = Door.OVERRIDE_ANIMS;
		this.synchronizeAnims = false;
		base.SetWorkTime(3f);
		if (!string.IsNullOrEmpty(this.doorClosingSoundEventName))
		{
			this.doorClosingSound = GlobalAssets.GetSound(this.doorClosingSoundEventName, false);
		}
		if (!string.IsNullOrEmpty(this.doorOpeningSoundEventName))
		{
			this.doorOpeningSound = GlobalAssets.GetSound(this.doorOpeningSoundEventName, false);
		}
		base.Subscribe<Door>(-905833192, Door.OnCopySettingsDelegate);
	}

	// Token: 0x060042EF RID: 17135 RVA: 0x000CFB95 File Offset: 0x000CDD95
	private Door.ControlState GetNextState(Door.ControlState wantedState)
	{
		return (wantedState + 1) % Door.ControlState.NumStates;
	}

	// Token: 0x060042F0 RID: 17136 RVA: 0x000CFB9C File Offset: 0x000CDD9C
	private static bool DisplacesGas(Door.DoorType type)
	{
		return type != Door.DoorType.Internal;
	}

	// Token: 0x060042F1 RID: 17137 RVA: 0x002508B4 File Offset: 0x0024EAB4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (base.GetComponent<KPrefabID>() != null)
		{
			this.log = new LoggerFSS("Door", 35);
		}
		if (!this.allowAutoControl && this.controlState == Door.ControlState.Auto)
		{
			this.controlState = Door.ControlState.Locked;
		}
		StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
		HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
		if (Door.DisplacesGas(this.doorType))
		{
			structureTemperatures.Bypass(handle);
		}
		this.controller = new Door.Controller.Instance(this);
		this.controller.StartSM();
		if (this.doorType == Door.DoorType.Sealed && !this.hasBeenUnsealed)
		{
			this.Seal();
		}
		this.UpdateDoorSpeed(this.operational.IsOperational);
		base.Subscribe<Door>(-592767678, Door.OnOperationalChangedDelegate);
		base.Subscribe<Door>(824508782, Door.OnOperationalChangedDelegate);
		base.Subscribe<Door>(-801688580, Door.OnLogicValueChangedDelegate);
		this.requestedState = this.CurrentState;
		this.ApplyRequestedControlState(true);
		int num = (this.rotatable.GetOrientation() == Orientation.Neutral) ? (this.building.Def.WidthInCells * (this.building.Def.HeightInCells - 1)) : 0;
		int num2 = (this.rotatable.GetOrientation() == Orientation.Neutral) ? this.building.Def.WidthInCells : this.building.Def.HeightInCells;
		for (int num3 = 0; num3 != num2; num3++)
		{
			int num4 = this.building.PlacementCells[num + num3];
			Grid.FakeFloor.Add(num4);
			Pathfinding.Instance.AddDirtyNavGridCell(num4);
		}
		List<int> list = new List<int>();
		foreach (int num5 in this.building.PlacementCells)
		{
			Grid.HasDoor[num5] = true;
			if (this.rotatable.IsRotated)
			{
				list.Add(Grid.CellAbove(num5));
				list.Add(Grid.CellBelow(num5));
			}
			else
			{
				list.Add(Grid.CellLeft(num5));
				list.Add(Grid.CellRight(num5));
			}
			SimMessages.SetCellProperties(num5, 8);
			if (Door.DisplacesGas(this.doorType))
			{
				Grid.RenderedByWorld[num5] = false;
			}
		}
	}

	// Token: 0x060042F2 RID: 17138 RVA: 0x00250AF4 File Offset: 0x0024ECF4
	protected override void OnCleanUp()
	{
		this.UpdateDoorState(true);
		List<int> list = new List<int>();
		foreach (int num in this.building.PlacementCells)
		{
			SimMessages.ClearCellProperties(num, 12);
			Grid.RenderedByWorld[num] = Grid.Element[num].substance.renderedByWorld;
			Grid.FakeFloor.Remove(num);
			if (Grid.Element[num].IsSolid)
			{
				SimMessages.ReplaceAndDisplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.DoorOpen, 0f, -1f, byte.MaxValue, 0, -1);
			}
			Pathfinding.Instance.AddDirtyNavGridCell(num);
			if (this.rotatable.IsRotated)
			{
				list.Add(Grid.CellAbove(num));
				list.Add(Grid.CellBelow(num));
			}
			else
			{
				list.Add(Grid.CellLeft(num));
				list.Add(Grid.CellRight(num));
			}
		}
		foreach (int num2 in this.building.PlacementCells)
		{
			Grid.HasDoor[num2] = false;
			Game.Instance.SetDupePassableSolid(num2, false, Grid.Solid[num2]);
			Grid.CritterImpassable[num2] = false;
			Grid.DupeImpassable[num2] = false;
			Pathfinding.Instance.AddDirtyNavGridCell(num2);
		}
		base.OnCleanUp();
	}

	// Token: 0x060042F3 RID: 17139 RVA: 0x000CFBA5 File Offset: 0x000CDDA5
	public void Seal()
	{
		this.controller.sm.isSealed.Set(true, this.controller, false);
	}

	// Token: 0x060042F4 RID: 17140 RVA: 0x000CFBC5 File Offset: 0x000CDDC5
	public void OrderUnseal()
	{
		this.controller.GoTo(this.controller.sm.Sealed.awaiting_unlock);
	}

	// Token: 0x060042F5 RID: 17141 RVA: 0x00250C50 File Offset: 0x0024EE50
	private void RefreshControlState()
	{
		switch (this.controlState)
		{
		case Door.ControlState.Auto:
			this.controller.sm.isLocked.Set(false, this.controller, false);
			break;
		case Door.ControlState.Opened:
			this.controller.sm.isLocked.Set(false, this.controller, false);
			break;
		case Door.ControlState.Locked:
			this.controller.sm.isLocked.Set(true, this.controller, false);
			break;
		}
		base.Trigger(279163026, this.controlState);
		this.SetWorldState();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CurrentDoorControlState, this);
	}

	// Token: 0x060042F6 RID: 17142 RVA: 0x00250D20 File Offset: 0x0024EF20
	private void OnOperationalChanged(object data)
	{
		bool isOperational = this.operational.IsOperational;
		if (isOperational != this.on)
		{
			this.UpdateDoorSpeed(isOperational);
			if (this.on && base.GetComponent<KPrefabID>().HasTag(GameTags.Transition))
			{
				this.SetActive(true);
				return;
			}
			this.SetActive(false);
		}
	}

	// Token: 0x060042F7 RID: 17143 RVA: 0x00250D74 File Offset: 0x0024EF74
	private void UpdateDoorSpeed(bool powered)
	{
		this.on = powered;
		this.UpdateAnimAndSoundParams(powered);
		float positionPercent = this.animController.GetPositionPercent();
		this.animController.Play(this.animController.CurrentAnim.hash, this.animController.PlayMode, 1f, 0f);
		this.animController.SetPositionPercent(positionPercent);
	}

	// Token: 0x060042F8 RID: 17144 RVA: 0x00250DD8 File Offset: 0x0024EFD8
	private void UpdateAnimAndSoundParams(bool powered)
	{
		if (powered)
		{
			this.animController.PlaySpeedMultiplier = this.poweredAnimSpeed;
			if (this.doorClosingSound != null)
			{
				this.loopingSounds.UpdateFirstParameter(this.doorClosingSound, Door.SOUND_POWERED_PARAMETER, 1f);
			}
			if (this.doorOpeningSound != null)
			{
				this.loopingSounds.UpdateFirstParameter(this.doorOpeningSound, Door.SOUND_POWERED_PARAMETER, 1f);
				return;
			}
		}
		else
		{
			this.animController.PlaySpeedMultiplier = this.unpoweredAnimSpeed;
			if (this.doorClosingSound != null)
			{
				this.loopingSounds.UpdateFirstParameter(this.doorClosingSound, Door.SOUND_POWERED_PARAMETER, 0f);
			}
			if (this.doorOpeningSound != null)
			{
				this.loopingSounds.UpdateFirstParameter(this.doorOpeningSound, Door.SOUND_POWERED_PARAMETER, 0f);
			}
		}
	}

	// Token: 0x060042F9 RID: 17145 RVA: 0x000CFBE7 File Offset: 0x000CDDE7
	private void SetActive(bool active)
	{
		if (this.operational.IsOperational)
		{
			this.operational.SetActive(active, false);
		}
	}

	// Token: 0x060042FA RID: 17146 RVA: 0x00250E98 File Offset: 0x0024F098
	private void SetWorldState()
	{
		int[] placementCells = this.building.PlacementCells;
		bool is_door_open = this.IsOpen();
		this.SetPassableState(is_door_open, placementCells);
		this.SetSimState(is_door_open, placementCells);
	}

	// Token: 0x060042FB RID: 17147 RVA: 0x00250EC8 File Offset: 0x0024F0C8
	private void SetPassableState(bool is_door_open, IList<int> cells)
	{
		for (int i = 0; i < cells.Count; i++)
		{
			int num = cells[i];
			switch (this.doorType)
			{
			case Door.DoorType.Pressure:
			case Door.DoorType.ManualPressure:
			case Door.DoorType.Sealed:
			{
				Grid.CritterImpassable[num] = (this.controlState != Door.ControlState.Opened);
				bool solid = !is_door_open;
				bool passable = this.controlState != Door.ControlState.Locked;
				Game.Instance.SetDupePassableSolid(num, passable, solid);
				if (this.controlState == Door.ControlState.Opened)
				{
					this.doorOpenLiquidRefreshHack = true;
					this.doorOpenLiquidRefreshTime = 1f;
				}
				break;
			}
			case Door.DoorType.Internal:
				Grid.CritterImpassable[num] = (this.controlState != Door.ControlState.Opened);
				Grid.DupeImpassable[num] = (this.controlState == Door.ControlState.Locked);
				break;
			}
			Pathfinding.Instance.AddDirtyNavGridCell(num);
		}
	}

	// Token: 0x060042FC RID: 17148 RVA: 0x00250FA0 File Offset: 0x0024F1A0
	private void SetSimState(bool is_door_open, IList<int> cells)
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		float mass = component.Mass / (float)cells.Count;
		for (int i = 0; i < cells.Count; i++)
		{
			int num = cells[i];
			Door.DoorType doorType = this.doorType;
			if (doorType <= Door.DoorType.ManualPressure || doorType == Door.DoorType.Sealed)
			{
				World.Instance.groundRenderer.MarkDirty(num);
				if (is_door_open)
				{
					SimMessages.Dig(num, Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimDoorOpened), false)).index, true);
					if (this.ShouldBlockFallingSand)
					{
						SimMessages.ClearCellProperties(num, 4);
					}
					else
					{
						SimMessages.SetCellProperties(num, 4);
					}
				}
				else
				{
					HandleVector<Game.CallbackInfo>.Handle handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimDoorClosed), false));
					float temperature = component.Temperature;
					if (temperature <= 0f)
					{
						temperature = component.Temperature;
					}
					SimMessages.ReplaceAndDisplaceElement(num, component.ElementID, CellEventLogger.Instance.DoorClose, mass, temperature, byte.MaxValue, 0, handle.index);
					SimMessages.SetCellProperties(num, 4);
				}
			}
		}
	}

	// Token: 0x060042FD RID: 17149 RVA: 0x002510C0 File Offset: 0x0024F2C0
	private void UpdateDoorState(bool cleaningUp)
	{
		foreach (int num in this.building.PlacementCells)
		{
			if (Grid.IsValidCell(num))
			{
				Grid.Foundation[num] = !cleaningUp;
			}
		}
	}

	// Token: 0x060042FE RID: 17150 RVA: 0x00251104 File Offset: 0x0024F304
	public void QueueStateChange(Door.ControlState nextState)
	{
		if (this.requestedState != nextState)
		{
			this.requestedState = nextState;
		}
		else
		{
			this.requestedState = this.controlState;
		}
		if (this.requestedState == this.controlState)
		{
			if (this.changeStateChore != null)
			{
				this.changeStateChore.Cancel("Change state");
				this.changeStateChore = null;
				base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, false);
			}
			return;
		}
		if (DebugHandler.InstantBuildMode)
		{
			this.controlState = this.requestedState;
			this.RefreshControlState();
			this.OnOperationalChanged(null);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, false);
			this.Open();
			this.Close();
			return;
		}
		if (this.changeStateChore != null)
		{
			this.changeStateChore.Cancel("Change state");
		}
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, this);
		this.changeStateChore = new WorkChore<Door>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x060042FF RID: 17151 RVA: 0x00251224 File Offset: 0x0024F424
	private void OnSimDoorOpened()
	{
		if (this == null || !Door.DisplacesGas(this.doorType))
		{
			return;
		}
		StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
		HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
		structureTemperatures.UnBypass(handle);
		this.do_melt_check = false;
	}

	// Token: 0x06004300 RID: 17152 RVA: 0x00251268 File Offset: 0x0024F468
	private void OnSimDoorClosed()
	{
		if (this == null || !Door.DisplacesGas(this.doorType))
		{
			return;
		}
		StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
		HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
		structureTemperatures.Bypass(handle);
		this.do_melt_check = true;
	}

	// Token: 0x06004301 RID: 17153 RVA: 0x000CFC03 File Offset: 0x000CDE03
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.changeStateChore = null;
		this.ApplyRequestedControlState(false);
	}

	// Token: 0x06004302 RID: 17154 RVA: 0x002512AC File Offset: 0x0024F4AC
	public void Open()
	{
		if (this.openCount == 0 && Door.DisplacesGas(this.doorType))
		{
			StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
			HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
			if (handle.IsValid() && structureTemperatures.IsBypassed(handle))
			{
				int[] placementCells = this.building.PlacementCells;
				float num = 0f;
				int num2 = 0;
				foreach (int i2 in placementCells)
				{
					if (Grid.Mass[i2] > 0f)
					{
						num2++;
						num += Grid.Temperature[i2];
					}
				}
				if (num2 > 0)
				{
					num /= (float)placementCells.Length;
					PrimaryElement component = base.GetComponent<PrimaryElement>();
					KCrashReporter.Assert(num > 0f, "Door has calculated an invalid temperature", null);
					component.Temperature = num;
				}
			}
		}
		this.openCount++;
		Door.ControlState controlState = this.controlState;
		if (controlState > Door.ControlState.Opened)
		{
			return;
		}
		this.controller.sm.isOpen.Set(true, this.controller, false);
	}

	// Token: 0x06004303 RID: 17155 RVA: 0x002513C0 File Offset: 0x0024F5C0
	public void Close()
	{
		this.openCount = Mathf.Max(0, this.openCount - 1);
		if (this.openCount == 0 && Door.DisplacesGas(this.doorType))
		{
			StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
			HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			if (handle.IsValid() && !structureTemperatures.IsBypassed(handle))
			{
				float temperature = structureTemperatures.GetPayload(handle).Temperature;
				component.Temperature = temperature;
			}
		}
		switch (this.controlState)
		{
		case Door.ControlState.Auto:
			if (this.openCount == 0)
			{
				this.controller.sm.isOpen.Set(false, this.controller, false);
				Game.Instance.userMenu.Refresh(base.gameObject);
			}
			break;
		case Door.ControlState.Opened:
			break;
		case Door.ControlState.Locked:
			this.controller.sm.isOpen.Set(false, this.controller, false);
			return;
		default:
			return;
		}
	}

	// Token: 0x06004304 RID: 17156 RVA: 0x000CFC1A File Offset: 0x000CDE1A
	public bool IsPendingClose()
	{
		return this.controller.IsInsideState(this.controller.sm.closedelay);
	}

	// Token: 0x06004305 RID: 17157 RVA: 0x002514B4 File Offset: 0x0024F6B4
	public bool IsOpen()
	{
		return this.controller.IsInsideState(this.controller.sm.open) || this.controller.IsInsideState(this.controller.sm.closedelay) || this.controller.IsInsideState(this.controller.sm.closeblocked);
	}

	// Token: 0x06004306 RID: 17158 RVA: 0x00251518 File Offset: 0x0024F718
	private void ApplyRequestedControlState(bool force = false)
	{
		if (this.requestedState == this.controlState && !force)
		{
			return;
		}
		this.controlState = this.requestedState;
		this.RefreshControlState();
		this.OnOperationalChanged(null);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, false);
		base.Trigger(1734268753, this);
		if (!force)
		{
			this.Open();
			this.Close();
		}
	}

	// Token: 0x06004307 RID: 17159 RVA: 0x00251588 File Offset: 0x0024F788
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != Door.OPEN_CLOSE_PORT_ID)
		{
			return;
		}
		int newValue = logicValueChanged.newValue;
		if (this.changeStateChore != null)
		{
			this.changeStateChore.Cancel("Change state");
			this.changeStateChore = null;
		}
		this.requestedState = (LogicCircuitNetwork.IsBitActive(0, newValue) ? Door.ControlState.Opened : Door.ControlState.Locked);
		this.applyLogicChange = true;
	}

	// Token: 0x06004308 RID: 17160 RVA: 0x002515F4 File Offset: 0x0024F7F4
	public void Sim200ms(float dt)
	{
		if (this == null)
		{
			return;
		}
		if (this.doorOpenLiquidRefreshHack)
		{
			this.doorOpenLiquidRefreshTime -= dt;
			if (this.doorOpenLiquidRefreshTime <= 0f)
			{
				this.doorOpenLiquidRefreshHack = false;
				foreach (int cell in this.building.PlacementCells)
				{
					Pathfinding.Instance.AddDirtyNavGridCell(cell);
				}
			}
		}
		if (this.applyLogicChange)
		{
			this.applyLogicChange = false;
			this.ApplyRequestedControlState(false);
		}
		if (this.do_melt_check)
		{
			StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
			HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
			if (handle.IsValid() && structureTemperatures.IsBypassed(handle))
			{
				foreach (int i2 in this.building.PlacementCells)
				{
					if (!Grid.Solid[i2])
					{
						Util.KDestroyGameObject(this);
						return;
					}
				}
			}
		}
	}

	// Token: 0x0600430A RID: 17162 RVA: 0x000CFC37 File Offset: 0x000CDE37
	bool INavDoor.get_isSpawned()
	{
		return base.isSpawned;
	}

	// Token: 0x04002E20 RID: 11808
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002E21 RID: 11809
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04002E22 RID: 11810
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x04002E23 RID: 11811
	[MyCmpReq]
	public Building building;

	// Token: 0x04002E24 RID: 11812
	[MyCmpGet]
	private EnergyConsumer consumer;

	// Token: 0x04002E25 RID: 11813
	[MyCmpAdd]
	private LoopingSounds loopingSounds;

	// Token: 0x04002E26 RID: 11814
	public Orientation verticalOrientation;

	// Token: 0x04002E27 RID: 11815
	[SerializeField]
	public bool hasComplexUserControls;

	// Token: 0x04002E28 RID: 11816
	[SerializeField]
	public float unpoweredAnimSpeed = 0.25f;

	// Token: 0x04002E29 RID: 11817
	[SerializeField]
	public float poweredAnimSpeed = 1f;

	// Token: 0x04002E2A RID: 11818
	[SerializeField]
	public Door.DoorType doorType;

	// Token: 0x04002E2B RID: 11819
	[SerializeField]
	public bool allowAutoControl = true;

	// Token: 0x04002E2C RID: 11820
	[SerializeField]
	public string doorClosingSoundEventName;

	// Token: 0x04002E2D RID: 11821
	[SerializeField]
	public string doorOpeningSoundEventName;

	// Token: 0x04002E2E RID: 11822
	private string doorClosingSound;

	// Token: 0x04002E2F RID: 11823
	private string doorOpeningSound;

	// Token: 0x04002E30 RID: 11824
	private static readonly HashedString SOUND_POWERED_PARAMETER = "doorPowered";

	// Token: 0x04002E31 RID: 11825
	private static readonly HashedString SOUND_PROGRESS_PARAMETER = "doorProgress";

	// Token: 0x04002E32 RID: 11826
	[Serialize]
	private bool hasBeenUnsealed;

	// Token: 0x04002E33 RID: 11827
	[Serialize]
	private Door.ControlState controlState;

	// Token: 0x04002E34 RID: 11828
	private bool on;

	// Token: 0x04002E35 RID: 11829
	private bool do_melt_check;

	// Token: 0x04002E36 RID: 11830
	private int openCount;

	// Token: 0x04002E37 RID: 11831
	private Door.ControlState requestedState;

	// Token: 0x04002E38 RID: 11832
	private Chore changeStateChore;

	// Token: 0x04002E39 RID: 11833
	private Door.Controller.Instance controller;

	// Token: 0x04002E3A RID: 11834
	private LoggerFSS log;

	// Token: 0x04002E3B RID: 11835
	private const float REFRESH_HACK_DELAY = 1f;

	// Token: 0x04002E3C RID: 11836
	private bool doorOpenLiquidRefreshHack;

	// Token: 0x04002E3D RID: 11837
	private float doorOpenLiquidRefreshTime;

	// Token: 0x04002E3E RID: 11838
	private static readonly EventSystem.IntraObjectHandler<Door> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Door>(delegate(Door component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04002E3F RID: 11839
	public static readonly HashedString OPEN_CLOSE_PORT_ID = new HashedString("DoorOpenClose");

	// Token: 0x04002E40 RID: 11840
	private static readonly KAnimFile[] OVERRIDE_ANIMS = new KAnimFile[]
	{
		Assets.GetAnim("anim_use_remote_kanim")
	};

	// Token: 0x04002E41 RID: 11841
	private static readonly EventSystem.IntraObjectHandler<Door> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Door>(delegate(Door component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04002E42 RID: 11842
	private static readonly EventSystem.IntraObjectHandler<Door> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Door>(delegate(Door component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04002E43 RID: 11843
	private bool applyLogicChange;

	// Token: 0x02000D7A RID: 3450
	public enum DoorType
	{
		// Token: 0x04002E45 RID: 11845
		Pressure,
		// Token: 0x04002E46 RID: 11846
		ManualPressure,
		// Token: 0x04002E47 RID: 11847
		Internal,
		// Token: 0x04002E48 RID: 11848
		Sealed
	}

	// Token: 0x02000D7B RID: 3451
	public enum ControlState
	{
		// Token: 0x04002E4A RID: 11850
		Auto,
		// Token: 0x04002E4B RID: 11851
		Opened,
		// Token: 0x04002E4C RID: 11852
		Locked,
		// Token: 0x04002E4D RID: 11853
		NumStates
	}

	// Token: 0x02000D7C RID: 3452
	public class Controller : GameStateMachine<Door.Controller, Door.Controller.Instance, Door>
	{
		// Token: 0x0600430B RID: 17163 RVA: 0x00251780 File Offset: 0x0024F980
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.closed;
			this.root.Update("RefreshIsBlocked", delegate(Door.Controller.Instance smi, float dt)
			{
				smi.RefreshIsBlocked();
			}, UpdateRate.SIM_200ms, false).ParamTransition<bool>(this.isSealed, this.Sealed.closed, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue);
			this.closeblocked.PlayAnim("open").ParamTransition<bool>(this.isOpen, this.open, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ParamTransition<bool>(this.isBlocked, this.closedelay, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsFalse);
			this.closedelay.PlayAnim("open").ScheduleGoTo(0.5f, this.closing).ParamTransition<bool>(this.isOpen, this.open, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ParamTransition<bool>(this.isBlocked, this.closeblocked, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue);
			this.closing.ParamTransition<bool>(this.isBlocked, this.closeblocked, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ToggleTag(GameTags.Transition).ToggleLoopingSound("Closing loop", (Door.Controller.Instance smi) => smi.master.doorClosingSound, (Door.Controller.Instance smi) => !string.IsNullOrEmpty(smi.master.doorClosingSound)).Enter("SetParams", delegate(Door.Controller.Instance smi)
			{
				smi.master.UpdateAnimAndSoundParams(smi.master.on);
			}).Update(delegate(Door.Controller.Instance smi, float dt)
			{
				if (smi.master.doorClosingSound != null)
				{
					smi.master.loopingSounds.UpdateSecondParameter(smi.master.doorClosingSound, Door.SOUND_PROGRESS_PARAMETER, smi.Get<KBatchedAnimController>().GetPositionPercent());
				}
			}, UpdateRate.SIM_33ms, false).Enter("SetActive", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetActive(true);
			}).Exit("SetActive", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetActive(false);
			}).PlayAnim("closing").OnAnimQueueComplete(this.closed);
			this.open.PlayAnim("open").ParamTransition<bool>(this.isOpen, this.closeblocked, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsFalse).Enter("SetWorldStateOpen", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetWorldState();
			});
			this.closed.PlayAnim("closed").ParamTransition<bool>(this.isOpen, this.opening, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ParamTransition<bool>(this.isLocked, this.locking, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).Enter("SetWorldStateClosed", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetWorldState();
			});
			this.locking.PlayAnim("locked_pre").OnAnimQueueComplete(this.locked).Enter("SetWorldStateClosed", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetWorldState();
			});
			this.locked.PlayAnim("locked").ParamTransition<bool>(this.isLocked, this.unlocking, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsFalse);
			this.unlocking.PlayAnim("locked_pst").OnAnimQueueComplete(this.closed);
			this.opening.ToggleTag(GameTags.Transition).ToggleLoopingSound("Opening loop", (Door.Controller.Instance smi) => smi.master.doorOpeningSound, (Door.Controller.Instance smi) => !string.IsNullOrEmpty(smi.master.doorOpeningSound)).Enter("SetParams", delegate(Door.Controller.Instance smi)
			{
				smi.master.UpdateAnimAndSoundParams(smi.master.on);
			}).Update(delegate(Door.Controller.Instance smi, float dt)
			{
				if (smi.master.doorOpeningSound != null)
				{
					smi.master.loopingSounds.UpdateSecondParameter(smi.master.doorOpeningSound, Door.SOUND_PROGRESS_PARAMETER, smi.Get<KBatchedAnimController>().GetPositionPercent());
				}
			}, UpdateRate.SIM_33ms, false).Enter("SetActive", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetActive(true);
			}).Exit("SetActive", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetActive(false);
			}).PlayAnim("opening").OnAnimQueueComplete(this.open);
			this.Sealed.Enter(delegate(Door.Controller.Instance smi)
			{
				OccupyArea component = smi.master.GetComponent<OccupyArea>();
				for (int i = 0; i < component.OccupiedCellsOffsets.Length; i++)
				{
					Grid.PreventFogOfWarReveal[Grid.OffsetCell(Grid.PosToCell(smi.master.gameObject), component.OccupiedCellsOffsets[i])] = false;
				}
				smi.sm.isLocked.Set(true, smi, false);
				smi.master.controlState = Door.ControlState.Locked;
				smi.master.RefreshControlState();
				if (smi.master.GetComponent<Unsealable>().facingRight)
				{
					smi.master.GetComponent<KBatchedAnimController>().FlipX = true;
				}
			}).Enter("SetWorldStateClosed", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetWorldState();
			}).Exit(delegate(Door.Controller.Instance smi)
			{
				smi.sm.isLocked.Set(false, smi, false);
				smi.master.GetComponent<AccessControl>().controlEnabled = true;
				smi.master.controlState = Door.ControlState.Opened;
				smi.master.RefreshControlState();
				smi.sm.isOpen.Set(true, smi, false);
				smi.sm.isLocked.Set(false, smi, false);
				smi.sm.isSealed.Set(false, smi, false);
			});
			this.Sealed.closed.PlayAnim("sealed", KAnim.PlayMode.Once);
			this.Sealed.awaiting_unlock.ToggleChore((Door.Controller.Instance smi) => this.CreateUnsealChore(smi, true), this.Sealed.chore_pst);
			this.Sealed.chore_pst.Enter(delegate(Door.Controller.Instance smi)
			{
				smi.master.hasBeenUnsealed = true;
				if (smi.master.GetComponent<Unsealable>().unsealed)
				{
					smi.GoTo(this.opening);
					FogOfWarMask.ClearMask(Grid.CellRight(Grid.PosToCell(smi.master.gameObject)));
					FogOfWarMask.ClearMask(Grid.CellLeft(Grid.PosToCell(smi.master.gameObject)));
					return;
				}
				smi.GoTo(this.Sealed.closed);
			});
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x00251CBC File Offset: 0x0024FEBC
		private Chore CreateUnsealChore(Door.Controller.Instance smi, bool approach_right)
		{
			return new WorkChore<Unsealable>(Db.Get().ChoreTypes.Toggle, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x04002E4E RID: 11854
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State open;

		// Token: 0x04002E4F RID: 11855
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State opening;

		// Token: 0x04002E50 RID: 11856
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closed;

		// Token: 0x04002E51 RID: 11857
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closing;

		// Token: 0x04002E52 RID: 11858
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closedelay;

		// Token: 0x04002E53 RID: 11859
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closeblocked;

		// Token: 0x04002E54 RID: 11860
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State locking;

		// Token: 0x04002E55 RID: 11861
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State locked;

		// Token: 0x04002E56 RID: 11862
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State unlocking;

		// Token: 0x04002E57 RID: 11863
		public Door.Controller.SealedStates Sealed;

		// Token: 0x04002E58 RID: 11864
		public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isOpen;

		// Token: 0x04002E59 RID: 11865
		public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isLocked;

		// Token: 0x04002E5A RID: 11866
		public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isBlocked;

		// Token: 0x04002E5B RID: 11867
		public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isSealed;

		// Token: 0x04002E5C RID: 11868
		public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter sealDirectionRight;

		// Token: 0x02000D7D RID: 3453
		public class SealedStates : GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State
		{
			// Token: 0x04002E5D RID: 11869
			public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closed;

			// Token: 0x04002E5E RID: 11870
			public Door.Controller.SealedStates.AwaitingUnlock awaiting_unlock;

			// Token: 0x04002E5F RID: 11871
			public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State chore_pst;

			// Token: 0x02000D7E RID: 3454
			public class AwaitingUnlock : GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State
			{
				// Token: 0x04002E60 RID: 11872
				public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State awaiting_arrival;

				// Token: 0x04002E61 RID: 11873
				public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State unlocking;
			}
		}

		// Token: 0x02000D7F RID: 3455
		public new class Instance : GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.GameInstance
		{
			// Token: 0x06004312 RID: 17170 RVA: 0x000CFC59 File Offset: 0x000CDE59
			public Instance(Door door) : base(door)
			{
			}

			// Token: 0x06004313 RID: 17171 RVA: 0x00251D74 File Offset: 0x0024FF74
			public void RefreshIsBlocked()
			{
				bool value = false;
				foreach (int cell in this.building.PlacementCells)
				{
					if (Grid.Objects[cell, 40] != null)
					{
						value = true;
						break;
					}
				}
				base.sm.isBlocked.Set(value, base.smi, false);
			}

			// Token: 0x04002E62 RID: 11874
			[MyCmpReq]
			public Building building;
		}
	}
}
