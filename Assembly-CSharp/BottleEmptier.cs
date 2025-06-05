using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000C99 RID: 3225
[SerializationConfig(MemberSerialization.OptIn)]
public class BottleEmptier : StateMachineComponent<BottleEmptier.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06003D37 RID: 15671 RVA: 0x000CC0FE File Offset: 0x000CA2FE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.DefineManualPumpingAffectedBuildings();
		base.Subscribe<BottleEmptier>(493375141, BottleEmptier.OnRefreshUserMenuDelegate);
		base.Subscribe<BottleEmptier>(-905833192, BottleEmptier.OnCopySettingsDelegate);
	}

	// Token: 0x06003D38 RID: 15672 RVA: 0x000AA765 File Offset: 0x000A8965
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x06003D39 RID: 15673 RVA: 0x0023E3EC File Offset: 0x0023C5EC
	private void DefineManualPumpingAffectedBuildings()
	{
		if (BottleEmptier.manualPumpingAffectedBuildings.ContainsKey(this.isGasEmptier))
		{
			return;
		}
		List<string> list = new List<string>();
		Tag tag = this.isGasEmptier ? GameTags.GasSource : GameTags.LiquidSource;
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			if (buildingDef.BuildingComplete.HasTag(tag))
			{
				list.Add(buildingDef.Name);
			}
		}
		BottleEmptier.manualPumpingAffectedBuildings.Add(this.isGasEmptier, list.ToArray());
	}

	// Token: 0x06003D3A RID: 15674 RVA: 0x000CC139 File Offset: 0x000CA339
	private void OnChangeAllowManualPumpingStationFetching()
	{
		this.allowManualPumpingStationFetching = !this.allowManualPumpingStationFetching;
		base.smi.RefreshChore();
	}

	// Token: 0x06003D3B RID: 15675 RVA: 0x0023E498 File Offset: 0x0023C698
	private void OnRefreshUserMenu(object data)
	{
		string text = this.isGasEmptier ? UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED_GAS.TOOLTIP : UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.TOOLTIP;
		string text2 = this.isGasEmptier ? UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED_GAS.TOOLTIP : UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED.TOOLTIP;
		if (BottleEmptier.manualPumpingAffectedBuildings.ContainsKey(this.isGasEmptier))
		{
			foreach (string arg in BottleEmptier.manualPumpingAffectedBuildings[this.isGasEmptier])
			{
				string str = string.Format(UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.ITEM, arg);
				text += str;
				text2 += str;
			}
		}
		if (this.isGasEmptier)
		{
			KIconButtonMenu.ButtonInfo button = this.allowManualPumpingStationFetching ? new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED_GAS.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text2, true) : new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED_GAS.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 0.4f);
			return;
		}
		KIconButtonMenu.ButtonInfo button2 = this.allowManualPumpingStationFetching ? new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.DENIED.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text2, true) : new KIconButtonMenu.ButtonInfo("action_bottler_delivery", UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY.ALLOWED.NAME, new System.Action(this.OnChangeAllowManualPumpingStationFetching), global::Action.NumActions, null, null, null, text, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button2, 0.4f);
	}

	// Token: 0x06003D3C RID: 15676 RVA: 0x0023E638 File Offset: 0x0023C838
	private void OnCopySettings(object data)
	{
		BottleEmptier component = ((GameObject)data).GetComponent<BottleEmptier>();
		this.allowManualPumpingStationFetching = component.allowManualPumpingStationFetching;
		base.smi.RefreshChore();
	}

	// Token: 0x04002A49 RID: 10825
	public float emptyRate = 10f;

	// Token: 0x04002A4A RID: 10826
	[Serialize]
	public bool allowManualPumpingStationFetching;

	// Token: 0x04002A4B RID: 10827
	[Serialize]
	public bool emit = true;

	// Token: 0x04002A4C RID: 10828
	public bool isGasEmptier;

	// Token: 0x04002A4D RID: 10829
	private static Dictionary<bool, string[]> manualPumpingAffectedBuildings = new Dictionary<bool, string[]>();

	// Token: 0x04002A4E RID: 10830
	private static readonly EventSystem.IntraObjectHandler<BottleEmptier> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<BottleEmptier>(delegate(BottleEmptier component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04002A4F RID: 10831
	private static readonly EventSystem.IntraObjectHandler<BottleEmptier> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<BottleEmptier>(delegate(BottleEmptier component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02000C9A RID: 3226
	public class StatesInstance : GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.GameInstance
	{
		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06003D3F RID: 15679 RVA: 0x000CC1AF File Offset: 0x000CA3AF
		// (set) Token: 0x06003D40 RID: 15680 RVA: 0x000CC1B7 File Offset: 0x000CA3B7
		public MeterController meter { get; private set; }

		// Token: 0x06003D41 RID: 15681 RVA: 0x0023E668 File Offset: 0x0023C868
		public StatesInstance(BottleEmptier smi) : base(smi)
		{
			TreeFilterable component = base.master.GetComponent<TreeFilterable>();
			component.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(component.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target",
				"meter_arrow",
				"meter_scale"
			});
			this.meter.meterController.GetComponent<KBatchedAnimTracker>().synchronizeEnabledState = false;
			this.meter.meterController.enabled = false;
			base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
			base.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x0023E73C File Offset: 0x0023C93C
		public void CreateChore()
		{
			HashSet<Tag> tags = base.GetComponent<TreeFilterable>().GetTags();
			Tag[] forbidden_tags;
			if (!base.master.allowManualPumpingStationFetching)
			{
				forbidden_tags = new Tag[]
				{
					GameTags.LiquidSource,
					GameTags.GasSource
				};
			}
			else
			{
				forbidden_tags = new Tag[0];
			}
			Storage component = base.GetComponent<Storage>();
			this.chore = new FetchChore(Db.Get().ChoreTypes.StorageFetch, component, component.Capacity(), tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, forbidden_tags, null, true, null, null, null, Operational.State.Operational, 0);
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x000CC1C0 File Offset: 0x000CA3C0
		public void CancelChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Storage Changed");
				this.chore = null;
			}
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x000CC1E1 File Offset: 0x000CA3E1
		public void RefreshChore()
		{
			this.GoTo(base.sm.unoperational);
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x000CC1F4 File Offset: 0x000CA3F4
		private void OnFilterChanged(HashSet<Tag> tags)
		{
			this.RefreshChore();
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x0023E7C4 File Offset: 0x0023C9C4
		private void OnStorageChange(object data)
		{
			this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.RemainingCapacity() / this.storage.capacityKg));
			this.meter.meterController.enabled = (this.storage.ExactMassStored() > 0f);
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x000CC1F4 File Offset: 0x000CA3F4
		private void OnOnlyFetchMarkedItemsSettingChanged(object data)
		{
			this.RefreshChore();
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x0023E81C File Offset: 0x0023CA1C
		public void StartMeter()
		{
			PrimaryElement firstPrimaryElement = this.GetFirstPrimaryElement();
			if (firstPrimaryElement == null)
			{
				return;
			}
			base.GetComponent<KBatchedAnimController>().SetSymbolTint(new KAnimHashedString("leak_ceiling"), firstPrimaryElement.Element.substance.colour);
			this.meter.meterController.SwapAnims(firstPrimaryElement.Element.substance.anims);
			this.meter.meterController.Play("empty", KAnim.PlayMode.Paused, 1f, 0f);
			Color32 colour = firstPrimaryElement.Element.substance.colour;
			colour.a = byte.MaxValue;
			this.meter.SetSymbolTint(new KAnimHashedString("meter_fill"), colour);
			this.meter.SetSymbolTint(new KAnimHashedString("water1"), colour);
			this.meter.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
			this.meter.SetSymbolTint(new KAnimHashedString("substance_tinter_cap"), colour);
			this.OnStorageChange(null);
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x0023E928 File Offset: 0x0023CB28
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

		// Token: 0x06003D4A RID: 15690 RVA: 0x0023E974 File Offset: 0x0023CB74
		public void Emit(float dt)
		{
			if (!base.smi.master.emit)
			{
				return;
			}
			PrimaryElement firstPrimaryElement = this.GetFirstPrimaryElement();
			if (firstPrimaryElement == null)
			{
				return;
			}
			float num = Mathf.Min(firstPrimaryElement.Mass, base.master.emptyRate * dt);
			if (num <= 0f)
			{
				return;
			}
			Tag prefabTag = firstPrimaryElement.GetComponent<KPrefabID>().PrefabTag;
			float num2;
			SimUtil.DiseaseInfo diseaseInfo;
			float temperature;
			this.storage.ConsumeAndGetDisease(prefabTag, num, out num2, out diseaseInfo, out temperature);
			Vector3 position = base.transform.GetPosition();
			position.y += 1.8f;
			bool flag = base.GetComponent<Rotatable>().GetOrientation() == Orientation.FlipH;
			position.x += (flag ? -0.2f : 0.2f);
			int num3 = Grid.PosToCell(position) + (flag ? -1 : 1);
			if (Grid.Solid[num3])
			{
				num3 += (flag ? 1 : -1);
			}
			Element element = firstPrimaryElement.Element;
			ushort idx = element.idx;
			if (element.IsLiquid)
			{
				FallingWater.instance.AddParticle(num3, idx, num2, temperature, diseaseInfo.idx, diseaseInfo.count, true, false, false, false);
				return;
			}
			SimMessages.ModifyCell(num3, idx, temperature, num2, diseaseInfo.idx, diseaseInfo.count, SimMessages.ReplaceType.None, false, -1);
		}

		// Token: 0x04002A50 RID: 10832
		[MyCmpGet]
		public Storage storage;

		// Token: 0x04002A51 RID: 10833
		private FetchChore chore;
	}

	// Token: 0x02000C9B RID: 3227
	public class States : GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier>
	{
		// Token: 0x06003D4B RID: 15691 RVA: 0x0023EAB0 File Offset: 0x0023CCB0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.waitingfordelivery;
			this.statusItem = new StatusItem("BottleEmptier", "", "", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItem.resolveStringCallback = delegate(string str, object data)
			{
				BottleEmptier bottleEmptier = (BottleEmptier)data;
				if (bottleEmptier == null)
				{
					return str;
				}
				if (bottleEmptier.allowManualPumpingStationFetching)
				{
					return bottleEmptier.isGasEmptier ? BUILDING.STATUSITEMS.CANISTER_EMPTIER.ALLOWED.NAME : BUILDING.STATUSITEMS.BOTTLE_EMPTIER.ALLOWED.NAME;
				}
				return bottleEmptier.isGasEmptier ? BUILDING.STATUSITEMS.CANISTER_EMPTIER.DENIED.NAME : BUILDING.STATUSITEMS.BOTTLE_EMPTIER.DENIED.NAME;
			};
			this.statusItem.resolveTooltipCallback = delegate(string str, object data)
			{
				BottleEmptier bottleEmptier = (BottleEmptier)data;
				if (bottleEmptier == null)
				{
					return str;
				}
				string result;
				if (bottleEmptier.allowManualPumpingStationFetching)
				{
					if (bottleEmptier.isGasEmptier)
					{
						result = BUILDING.STATUSITEMS.CANISTER_EMPTIER.ALLOWED.TOOLTIP;
					}
					else
					{
						result = BUILDING.STATUSITEMS.BOTTLE_EMPTIER.ALLOWED.TOOLTIP;
					}
				}
				else if (bottleEmptier.isGasEmptier)
				{
					result = BUILDING.STATUSITEMS.CANISTER_EMPTIER.DENIED.TOOLTIP;
				}
				else
				{
					result = BUILDING.STATUSITEMS.BOTTLE_EMPTIER.DENIED.TOOLTIP;
				}
				return result;
			};
			this.root.ToggleStatusItem(this.statusItem, (BottleEmptier.StatesInstance smi) => smi.master);
			this.unoperational.TagTransition(GameTags.Operational, this.waitingfordelivery, false).PlayAnim("off");
			this.waitingfordelivery.TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.emptying, (BottleEmptier.StatesInstance smi) => smi.GetComponent<Storage>().ExactMassStored() > 0f).Enter("CreateChore", delegate(BottleEmptier.StatesInstance smi)
			{
				smi.CreateChore();
			}).Exit("CancelChore", delegate(BottleEmptier.StatesInstance smi)
			{
				smi.CancelChore();
			}).PlayAnim("on");
			this.emptying.TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.waitingfordelivery, (BottleEmptier.StatesInstance smi) => smi.GetComponent<Storage>().ExactMassStored() == 0f).Enter("StartMeter", delegate(BottleEmptier.StatesInstance smi)
			{
				smi.StartMeter();
			}).Update("Emit", delegate(BottleEmptier.StatesInstance smi, float dt)
			{
				smi.Emit(dt);
			}, UpdateRate.SIM_200ms, false).PlayAnim("working_loop", KAnim.PlayMode.Loop);
		}

		// Token: 0x04002A53 RID: 10835
		private StatusItem statusItem;

		// Token: 0x04002A54 RID: 10836
		public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State unoperational;

		// Token: 0x04002A55 RID: 10837
		public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State waitingfordelivery;

		// Token: 0x04002A56 RID: 10838
		public GameStateMachine<BottleEmptier.States, BottleEmptier.StatesInstance, BottleEmptier, object>.State emptying;
	}
}
