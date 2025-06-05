using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020011E4 RID: 4580
public class IrrigationMonitor : GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>
{
	// Token: 0x06005D1C RID: 23836 RVA: 0x002AB568 File Offset: 0x002A9768
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.wild;
		base.serializable = StateMachine.SerializeType.Never;
		this.wild.ParamTransition<GameObject>(this.resourceStorage, this.unfertilizable, (IrrigationMonitor.Instance smi, GameObject p) => p != null);
		this.unfertilizable.Enter(delegate(IrrigationMonitor.Instance smi)
		{
			if (smi.AcceptsLiquid())
			{
				smi.GoTo(this.replanted.irrigated);
			}
		});
		this.replanted.Enter(delegate(IrrigationMonitor.Instance smi)
		{
			ManualDeliveryKG[] components = smi.gameObject.GetComponents<ManualDeliveryKG>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].Pause(false, "replanted");
			}
			smi.UpdateIrrigation(0.033333335f);
		}).Target(this.resourceStorage).EventHandler(GameHashes.OnStorageChange, delegate(IrrigationMonitor.Instance smi)
		{
			smi.UpdateIrrigation(0.2f);
		}).Target(this.masterTarget);
		this.replanted.irrigated.DefaultState(this.replanted.irrigated.absorbing).TriggerOnEnter(this.ResourceRecievedEvent, null);
		this.replanted.irrigated.absorbing.DefaultState(this.replanted.irrigated.absorbing.normal).ParamTransition<bool>(this.hasCorrectLiquid, this.replanted.starved, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsFalse).ToggleAttributeModifier("Absorbing", (IrrigationMonitor.Instance smi) => smi.absorptionRate, null).Enter(delegate(IrrigationMonitor.Instance smi)
		{
			smi.UpdateAbsorbing(true);
		}).EventHandler(GameHashes.TagsChanged, delegate(IrrigationMonitor.Instance smi)
		{
			smi.UpdateAbsorbing(true);
		}).Exit(delegate(IrrigationMonitor.Instance smi)
		{
			smi.UpdateAbsorbing(false);
		});
		this.replanted.irrigated.absorbing.normal.ParamTransition<bool>(this.hasIncorrectLiquid, this.replanted.irrigated.absorbing.wrongLiquid, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsTrue);
		this.replanted.irrigated.absorbing.wrongLiquid.ParamTransition<bool>(this.hasIncorrectLiquid, this.replanted.irrigated.absorbing.normal, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsFalse);
		this.replanted.starved.DefaultState(this.replanted.starved.normal).TriggerOnEnter(this.ResourceDepletedEvent, null).ParamTransition<bool>(this.enoughCorrectLiquidToRecover, this.replanted.irrigated.absorbing, (IrrigationMonitor.Instance smi, bool p) => p && this.hasCorrectLiquid.Get(smi)).ParamTransition<bool>(this.hasCorrectLiquid, this.replanted.irrigated.absorbing, (IrrigationMonitor.Instance smi, bool p) => p && this.enoughCorrectLiquidToRecover.Get(smi));
		this.replanted.starved.normal.ParamTransition<bool>(this.hasIncorrectLiquid, this.replanted.starved.wrongLiquid, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsTrue);
		this.replanted.starved.wrongLiquid.ParamTransition<bool>(this.hasIncorrectLiquid, this.replanted.starved.normal, GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.IsFalse);
	}

	// Token: 0x04004253 RID: 16979
	public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.TargetParameter resourceStorage;

	// Token: 0x04004254 RID: 16980
	public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.BoolParameter hasCorrectLiquid;

	// Token: 0x04004255 RID: 16981
	public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.BoolParameter hasIncorrectLiquid;

	// Token: 0x04004256 RID: 16982
	public StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.BoolParameter enoughCorrectLiquidToRecover;

	// Token: 0x04004257 RID: 16983
	public GameHashes ResourceRecievedEvent = GameHashes.LiquidResourceRecieved;

	// Token: 0x04004258 RID: 16984
	public GameHashes ResourceDepletedEvent = GameHashes.LiquidResourceEmpty;

	// Token: 0x04004259 RID: 16985
	public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State wild;

	// Token: 0x0400425A RID: 16986
	public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State unfertilizable;

	// Token: 0x0400425B RID: 16987
	public IrrigationMonitor.ReplantedStates replanted;

	// Token: 0x020011E5 RID: 4581
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06005D21 RID: 23841 RVA: 0x002AB8A0 File Offset: 0x002A9AA0
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			if (this.consumedElements.Length != 0)
			{
				List<Descriptor> list = new List<Descriptor>();
				float preModifiedAttributeValue = obj.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.FertilizerUsageMod);
				foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in this.consumedElements)
				{
					float num = consumeInfo.massConsumptionRate * preModifiedAttributeValue;
					list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.IDEAL_FERTILIZER, consumeInfo.tag.ProperName(), GameUtil.GetFormattedMass(-num, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.IDEAL_FERTILIZER, consumeInfo.tag.ProperName(), GameUtil.GetFormattedMass(num, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
				}
				return list;
			}
			return null;
		}

		// Token: 0x0400425C RID: 16988
		public Tag wrongIrrigationTestTag;

		// Token: 0x0400425D RID: 16989
		public PlantElementAbsorber.ConsumeInfo[] consumedElements;
	}

	// Token: 0x020011E6 RID: 4582
	public class VariableIrrigationStates : GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State
	{
		// Token: 0x0400425E RID: 16990
		public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State normal;

		// Token: 0x0400425F RID: 16991
		public GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State wrongLiquid;
	}

	// Token: 0x020011E7 RID: 4583
	public class Irrigated : GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State
	{
		// Token: 0x04004260 RID: 16992
		public IrrigationMonitor.VariableIrrigationStates absorbing;
	}

	// Token: 0x020011E8 RID: 4584
	public class ReplantedStates : GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.State
	{
		// Token: 0x04004261 RID: 16993
		public IrrigationMonitor.Irrigated irrigated;

		// Token: 0x04004262 RID: 16994
		public IrrigationMonitor.VariableIrrigationStates starved;
	}

	// Token: 0x020011E9 RID: 4585
	public new class Instance : GameStateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.GameInstance, IWiltCause
	{
		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06005D26 RID: 23846 RVA: 0x000E139B File Offset: 0x000DF59B
		public float total_fertilizer_available
		{
			get
			{
				return this.total_available_mass;
			}
		}

		// Token: 0x06005D27 RID: 23847 RVA: 0x000E13A3 File Offset: 0x000DF5A3
		public Instance(IStateMachineTarget master, IrrigationMonitor.Def def) : base(master, def)
		{
			this.AddAmounts(base.gameObject);
			this.MakeModifiers();
			master.Subscribe(1309017699, new Action<object>(this.SetStorage));
		}

		// Token: 0x06005D28 RID: 23848 RVA: 0x000E13E2 File Offset: 0x000DF5E2
		public virtual StatusItem GetStarvedStatusItem()
		{
			return Db.Get().CreatureStatusItems.NeedsIrrigation;
		}

		// Token: 0x06005D29 RID: 23849 RVA: 0x000E13F3 File Offset: 0x000DF5F3
		public virtual StatusItem GetIncorrectLiquidStatusItem()
		{
			return Db.Get().CreatureStatusItems.WrongIrrigation;
		}

		// Token: 0x06005D2A RID: 23850 RVA: 0x000E1404 File Offset: 0x000DF604
		public virtual StatusItem GetIncorrectLiquidStatusItemMajor()
		{
			return Db.Get().CreatureStatusItems.WrongIrrigationMajor;
		}

		// Token: 0x06005D2B RID: 23851 RVA: 0x002AB968 File Offset: 0x002A9B68
		protected virtual void AddAmounts(GameObject gameObject)
		{
			Amounts amounts = gameObject.GetAmounts();
			this.irrigation = amounts.Add(new AmountInstance(Db.Get().Amounts.Irrigation, gameObject));
		}

		// Token: 0x06005D2C RID: 23852 RVA: 0x002AB9A0 File Offset: 0x002A9BA0
		protected virtual void MakeModifiers()
		{
			this.consumptionRate = new AttributeModifier(Db.Get().Amounts.Irrigation.deltaAttribute.Id, -0.16666667f, CREATURES.STATS.IRRIGATION.CONSUME_MODIFIER, false, false, true);
			this.absorptionRate = new AttributeModifier(Db.Get().Amounts.Irrigation.deltaAttribute.Id, 1.6666666f, CREATURES.STATS.IRRIGATION.ABSORBING_MODIFIER, false, false, true);
		}

		// Token: 0x06005D2D RID: 23853 RVA: 0x002ABA1C File Offset: 0x002A9C1C
		public static void DumpIncorrectFertilizers(Storage storage, GameObject go)
		{
			if (storage == null)
			{
				return;
			}
			if (go == null)
			{
				return;
			}
			IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
			PlantElementAbsorber.ConsumeInfo[] consumed_infos = null;
			if (smi != null)
			{
				consumed_infos = smi.def.consumedElements;
			}
			IrrigationMonitor.Instance.DumpIncorrectFertilizers(storage, consumed_infos, false);
			FertilizationMonitor.Instance smi2 = go.GetSMI<FertilizationMonitor.Instance>();
			PlantElementAbsorber.ConsumeInfo[] consumed_infos2 = null;
			if (smi2 != null)
			{
				consumed_infos2 = smi2.def.consumedElements;
			}
			IrrigationMonitor.Instance.DumpIncorrectFertilizers(storage, consumed_infos2, true);
		}

		// Token: 0x06005D2E RID: 23854 RVA: 0x002ABA80 File Offset: 0x002A9C80
		private static void DumpIncorrectFertilizers(Storage storage, PlantElementAbsorber.ConsumeInfo[] consumed_infos, bool validate_solids)
		{
			if (storage == null)
			{
				return;
			}
			for (int i = storage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = storage.items[i];
				if (!(gameObject == null))
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					if (!(component == null) && !(gameObject.GetComponent<ElementChunk>() == null))
					{
						if (validate_solids)
						{
							if (!component.Element.IsSolid)
							{
								goto IL_C1;
							}
						}
						else if (!component.Element.IsLiquid)
						{
							goto IL_C1;
						}
						bool flag = false;
						KPrefabID component2 = component.GetComponent<KPrefabID>();
						if (consumed_infos != null)
						{
							foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in consumed_infos)
							{
								if (component2.HasTag(consumeInfo.tag))
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							storage.Drop(gameObject, true);
						}
					}
				}
				IL_C1:;
			}
		}

		// Token: 0x06005D2F RID: 23855 RVA: 0x002ABB5C File Offset: 0x002A9D5C
		public void SetStorage(object obj)
		{
			this.storage = (Storage)obj;
			base.sm.resourceStorage.Set(this.storage, base.smi);
			IrrigationMonitor.Instance.DumpIncorrectFertilizers(this.storage, base.smi.gameObject);
			foreach (ManualDeliveryKG manualDeliveryKG in base.smi.gameObject.GetComponents<ManualDeliveryKG>())
			{
				bool flag = false;
				foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in base.def.consumedElements)
				{
					if (manualDeliveryKG.RequestedItemTag == consumeInfo.tag)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					manualDeliveryKG.SetStorage(this.storage);
					manualDeliveryKG.enabled = !this.storage.gameObject.GetComponent<PlantablePlot>().has_liquid_pipe_input;
				}
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06005D30 RID: 23856 RVA: 0x000E1415 File Offset: 0x000DF615
		public WiltCondition.Condition[] Conditions
		{
			get
			{
				return new WiltCondition.Condition[]
				{
					WiltCondition.Condition.Irrigation
				};
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06005D31 RID: 23857 RVA: 0x002ABC3C File Offset: 0x002A9E3C
		public string WiltStateString
		{
			get
			{
				string result = "";
				if (base.smi.IsInsideState(base.smi.sm.replanted.irrigated.absorbing.wrongLiquid))
				{
					result = this.GetIncorrectLiquidStatusItem().resolveStringCallback(CREATURES.STATUSITEMS.WRONGIRRIGATION.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.starved.wrongLiquid))
				{
					result = this.GetIncorrectLiquidStatusItemMajor().resolveStringCallback(CREATURES.STATUSITEMS.WRONGIRRIGATIONMAJOR.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.starved))
				{
					result = this.GetStarvedStatusItem().resolveStringCallback(CREATURES.STATUSITEMS.NEEDSIRRIGATION.NAME, this);
				}
				return result;
			}
		}

		// Token: 0x06005D32 RID: 23858 RVA: 0x002ABD20 File Offset: 0x002A9F20
		public virtual bool AcceptsLiquid()
		{
			PlantablePlot component = base.sm.resourceStorage.Get(this).GetComponent<PlantablePlot>();
			return component != null && component.AcceptsIrrigation;
		}

		// Token: 0x06005D33 RID: 23859 RVA: 0x000E1421 File Offset: 0x000DF621
		public bool Starved()
		{
			return this.irrigation.value == 0f;
		}

		// Token: 0x06005D34 RID: 23860 RVA: 0x002ABD58 File Offset: 0x002A9F58
		public void UpdateIrrigation(float dt)
		{
			if (base.def.consumedElements == null)
			{
				return;
			}
			Storage storage = base.sm.resourceStorage.Get<Storage>(base.smi);
			bool flag = true;
			bool value = false;
			bool flag2 = true;
			if (storage != null)
			{
				List<GameObject> items = storage.items;
				for (int i = 0; i < base.def.consumedElements.Length; i++)
				{
					float num = 0f;
					PlantElementAbsorber.ConsumeInfo consumeInfo = base.def.consumedElements[i];
					for (int j = 0; j < items.Count; j++)
					{
						GameObject gameObject = items[j];
						if (gameObject.HasTag(consumeInfo.tag))
						{
							num += gameObject.GetComponent<PrimaryElement>().Mass;
						}
						else if (gameObject.HasTag(base.def.wrongIrrigationTestTag))
						{
							value = true;
						}
					}
					this.total_available_mass = num;
					float totalValue = base.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
					if (num < consumeInfo.massConsumptionRate * totalValue * dt)
					{
						flag = false;
						break;
					}
					if (num < consumeInfo.massConsumptionRate * totalValue * (dt * 30f))
					{
						flag2 = false;
						break;
					}
				}
			}
			else
			{
				flag = false;
				flag2 = false;
				value = false;
			}
			base.sm.hasCorrectLiquid.Set(flag, base.smi, false);
			base.sm.hasIncorrectLiquid.Set(value, base.smi, false);
			base.sm.enoughCorrectLiquidToRecover.Set(flag2 && flag, base.smi, false);
		}

		// Token: 0x06005D35 RID: 23861 RVA: 0x002ABEEC File Offset: 0x002AA0EC
		public void UpdateAbsorbing(bool allow)
		{
			bool flag = allow && !base.smi.gameObject.HasTag(GameTags.Wilting);
			if (flag != this.absorberHandle.IsValid())
			{
				if (flag)
				{
					if (base.def.consumedElements == null || base.def.consumedElements.Length == 0)
					{
						return;
					}
					float totalValue = base.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
					PlantElementAbsorber.ConsumeInfo[] array = new PlantElementAbsorber.ConsumeInfo[base.def.consumedElements.Length];
					for (int i = 0; i < base.def.consumedElements.Length; i++)
					{
						PlantElementAbsorber.ConsumeInfo consumeInfo = base.def.consumedElements[i];
						consumeInfo.massConsumptionRate *= totalValue;
						array[i] = consumeInfo;
					}
					this.absorberHandle = Game.Instance.plantElementAbsorbers.Add(this.storage, array);
					return;
				}
				else
				{
					this.absorberHandle = Game.Instance.plantElementAbsorbers.Remove(this.absorberHandle);
				}
			}
		}

		// Token: 0x04004263 RID: 16995
		public AttributeModifier consumptionRate;

		// Token: 0x04004264 RID: 16996
		public AttributeModifier absorptionRate;

		// Token: 0x04004265 RID: 16997
		protected AmountInstance irrigation;

		// Token: 0x04004266 RID: 16998
		private float total_available_mass;

		// Token: 0x04004267 RID: 16999
		private Storage storage;

		// Token: 0x04004268 RID: 17000
		private HandleVector<int>.Handle absorberHandle = HandleVector<int>.InvalidHandle;
	}
}
