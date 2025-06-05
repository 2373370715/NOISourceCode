using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020011BF RID: 4543
public class FertilizationMonitor : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>
{
	// Token: 0x06005C58 RID: 23640 RVA: 0x002A8E4C File Offset: 0x002A704C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.wild;
		base.serializable = StateMachine.SerializeType.Never;
		this.wild.ParamTransition<GameObject>(this.fertilizerStorage, this.unfertilizable, (FertilizationMonitor.Instance smi, GameObject p) => p != null);
		this.unfertilizable.Enter(delegate(FertilizationMonitor.Instance smi)
		{
			if (smi.AcceptsFertilizer())
			{
				smi.GoTo(this.replanted.fertilized);
			}
		});
		this.replanted.Enter(delegate(FertilizationMonitor.Instance smi)
		{
			ManualDeliveryKG[] components = smi.gameObject.GetComponents<ManualDeliveryKG>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].Pause(false, "replanted");
			}
			smi.UpdateFertilization(0.033333335f);
		}).Target(this.fertilizerStorage).EventHandler(GameHashes.OnStorageChange, delegate(FertilizationMonitor.Instance smi)
		{
			smi.UpdateFertilization(0.2f);
		}).Target(this.masterTarget);
		this.replanted.fertilized.DefaultState(this.replanted.fertilized.decaying).TriggerOnEnter(this.ResourceRecievedEvent, null);
		this.replanted.fertilized.decaying.DefaultState(this.replanted.fertilized.decaying.normal).ToggleAttributeModifier("Consuming", (FertilizationMonitor.Instance smi) => smi.consumptionRate, null).ParamTransition<bool>(this.hasCorrectFertilizer, this.replanted.fertilized.absorbing, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue).Update("Decaying", delegate(FertilizationMonitor.Instance smi, float dt)
		{
			if (smi.Starved())
			{
				smi.GoTo(this.replanted.starved);
			}
		}, UpdateRate.SIM_200ms, false);
		this.replanted.fertilized.decaying.normal.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.decaying.wrongFert, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.fertilized.decaying.wrongFert.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.decaying.normal, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse);
		this.replanted.fertilized.absorbing.DefaultState(this.replanted.fertilized.absorbing.normal).ParamTransition<bool>(this.hasCorrectFertilizer, this.replanted.fertilized.decaying, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse).ToggleAttributeModifier("Absorbing", (FertilizationMonitor.Instance smi) => smi.absorptionRate, null).Enter(delegate(FertilizationMonitor.Instance smi)
		{
			smi.StartAbsorbing();
		}).EventHandler(GameHashes.Wilt, delegate(FertilizationMonitor.Instance smi)
		{
			smi.StopAbsorbing();
		}).EventHandler(GameHashes.WiltRecover, delegate(FertilizationMonitor.Instance smi)
		{
			smi.StartAbsorbing();
		}).Exit(delegate(FertilizationMonitor.Instance smi)
		{
			smi.StopAbsorbing();
		});
		this.replanted.fertilized.absorbing.normal.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.absorbing.wrongFert, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.fertilized.absorbing.wrongFert.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.fertilized.absorbing.normal, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse);
		this.replanted.starved.DefaultState(this.replanted.starved.normal).TriggerOnEnter(this.ResourceDepletedEvent, null).ParamTransition<bool>(this.hasCorrectFertilizer, this.replanted.fertilized, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.starved.normal.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.starved.wrongFert, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsTrue);
		this.replanted.starved.wrongFert.ParamTransition<bool>(this.hasIncorrectFertilizer, this.replanted.starved.normal, GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.IsFalse);
	}

	// Token: 0x040041C3 RID: 16835
	public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.TargetParameter fertilizerStorage;

	// Token: 0x040041C4 RID: 16836
	public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.BoolParameter hasCorrectFertilizer;

	// Token: 0x040041C5 RID: 16837
	public StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.BoolParameter hasIncorrectFertilizer;

	// Token: 0x040041C6 RID: 16838
	public GameHashes ResourceRecievedEvent = GameHashes.Fertilized;

	// Token: 0x040041C7 RID: 16839
	public GameHashes ResourceDepletedEvent = GameHashes.Unfertilized;

	// Token: 0x040041C8 RID: 16840
	public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wild;

	// Token: 0x040041C9 RID: 16841
	public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State unfertilizable;

	// Token: 0x040041CA RID: 16842
	public FertilizationMonitor.ReplantedStates replanted;

	// Token: 0x020011C0 RID: 4544
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06005C5C RID: 23644 RVA: 0x002A9280 File Offset: 0x002A7480
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

		// Token: 0x040041CB RID: 16843
		public Tag wrongFertilizerTestTag;

		// Token: 0x040041CC RID: 16844
		public PlantElementAbsorber.ConsumeInfo[] consumedElements;
	}

	// Token: 0x020011C1 RID: 4545
	public class VariableFertilizerStates : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
	{
		// Token: 0x040041CD RID: 16845
		public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State normal;

		// Token: 0x040041CE RID: 16846
		public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wrongFert;
	}

	// Token: 0x020011C2 RID: 4546
	public class FertilizedStates : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
	{
		// Token: 0x040041CF RID: 16847
		public FertilizationMonitor.VariableFertilizerStates decaying;

		// Token: 0x040041D0 RID: 16848
		public FertilizationMonitor.VariableFertilizerStates absorbing;

		// Token: 0x040041D1 RID: 16849
		public GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State wilting;
	}

	// Token: 0x020011C3 RID: 4547
	public class ReplantedStates : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.State
	{
		// Token: 0x040041D2 RID: 16850
		public FertilizationMonitor.FertilizedStates fertilized;

		// Token: 0x040041D3 RID: 16851
		public FertilizationMonitor.VariableFertilizerStates starved;
	}

	// Token: 0x020011C4 RID: 4548
	public new class Instance : GameStateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.GameInstance, IWiltCause
	{
		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06005C61 RID: 23649 RVA: 0x000E0AA4 File Offset: 0x000DECA4
		public float total_fertilizer_available
		{
			get
			{
				return this.total_available_mass;
			}
		}

		// Token: 0x06005C62 RID: 23650 RVA: 0x000E0AAC File Offset: 0x000DECAC
		public Instance(IStateMachineTarget master, FertilizationMonitor.Def def) : base(master, def)
		{
			this.AddAmounts(base.gameObject);
			this.MakeModifiers();
			master.Subscribe(1309017699, new Action<object>(this.SetStorage));
		}

		// Token: 0x06005C63 RID: 23651 RVA: 0x000E0AEB File Offset: 0x000DECEB
		public virtual StatusItem GetStarvedStatusItem()
		{
			return Db.Get().CreatureStatusItems.NeedsFertilizer;
		}

		// Token: 0x06005C64 RID: 23652 RVA: 0x000E0AFC File Offset: 0x000DECFC
		public virtual StatusItem GetIncorrectFertStatusItem()
		{
			return Db.Get().CreatureStatusItems.WrongFertilizer;
		}

		// Token: 0x06005C65 RID: 23653 RVA: 0x000E0B0D File Offset: 0x000DED0D
		public virtual StatusItem GetIncorrectFertStatusItemMajor()
		{
			return Db.Get().CreatureStatusItems.WrongFertilizerMajor;
		}

		// Token: 0x06005C66 RID: 23654 RVA: 0x002A9348 File Offset: 0x002A7548
		protected virtual void AddAmounts(GameObject gameObject)
		{
			Amounts amounts = gameObject.GetAmounts();
			this.fertilization = amounts.Add(new AmountInstance(Db.Get().Amounts.Fertilization, gameObject));
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06005C67 RID: 23655 RVA: 0x000E0B1E File Offset: 0x000DED1E
		public WiltCondition.Condition[] Conditions
		{
			get
			{
				return new WiltCondition.Condition[]
				{
					WiltCondition.Condition.Fertilized
				};
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06005C68 RID: 23656 RVA: 0x002A9380 File Offset: 0x002A7580
		public string WiltStateString
		{
			get
			{
				string result = "";
				if (base.smi.IsInsideState(base.smi.sm.replanted.fertilized.decaying.wrongFert))
				{
					result = this.GetIncorrectFertStatusItemMajor().resolveStringCallback(CREATURES.STATUSITEMS.WRONGFERTILIZERMAJOR.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.fertilized.absorbing.wrongFert))
				{
					result = this.GetIncorrectFertStatusItem().resolveStringCallback(CREATURES.STATUSITEMS.WRONGFERTILIZER.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.starved))
				{
					result = this.GetStarvedStatusItem().resolveStringCallback(CREATURES.STATUSITEMS.NEEDSFERTILIZER.NAME, this);
				}
				else if (base.smi.IsInsideState(base.smi.sm.replanted.starved.wrongFert))
				{
					result = this.GetIncorrectFertStatusItemMajor().resolveStringCallback(CREATURES.STATUSITEMS.WRONGFERTILIZERMAJOR.NAME, this);
				}
				return result;
			}
		}

		// Token: 0x06005C69 RID: 23657 RVA: 0x002A94B4 File Offset: 0x002A76B4
		protected virtual void MakeModifiers()
		{
			this.consumptionRate = new AttributeModifier(Db.Get().Amounts.Fertilization.deltaAttribute.Id, -0.16666667f, CREATURES.STATS.FERTILIZATION.CONSUME_MODIFIER, false, false, true);
			this.absorptionRate = new AttributeModifier(Db.Get().Amounts.Fertilization.deltaAttribute.Id, 1.6666666f, CREATURES.STATS.FERTILIZATION.ABSORBING_MODIFIER, false, false, true);
		}

		// Token: 0x06005C6A RID: 23658 RVA: 0x002A9530 File Offset: 0x002A7730
		public void SetStorage(object obj)
		{
			this.storage = (Storage)obj;
			base.sm.fertilizerStorage.Set(this.storage, base.smi);
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
					manualDeliveryKG.enabled = true;
				}
			}
		}

		// Token: 0x06005C6B RID: 23659 RVA: 0x002A95FC File Offset: 0x002A77FC
		public virtual bool AcceptsFertilizer()
		{
			PlantablePlot component = base.sm.fertilizerStorage.Get(this).GetComponent<PlantablePlot>();
			return component != null && component.AcceptsFertilizer;
		}

		// Token: 0x06005C6C RID: 23660 RVA: 0x000E0B2A File Offset: 0x000DED2A
		public bool Starved()
		{
			return this.fertilization.value == 0f;
		}

		// Token: 0x06005C6D RID: 23661 RVA: 0x002A9634 File Offset: 0x002A7834
		public void UpdateFertilization(float dt)
		{
			if (base.def.consumedElements == null)
			{
				return;
			}
			if (this.storage == null)
			{
				return;
			}
			bool value = true;
			bool value2 = false;
			List<GameObject> items = this.storage.items;
			for (int i = 0; i < base.def.consumedElements.Length; i++)
			{
				PlantElementAbsorber.ConsumeInfo consumeInfo = base.def.consumedElements[i];
				float num = 0f;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (gameObject.HasTag(consumeInfo.tag))
					{
						num += gameObject.GetComponent<PrimaryElement>().Mass;
					}
					else if (gameObject.HasTag(base.def.wrongFertilizerTestTag))
					{
						value2 = true;
					}
				}
				this.total_available_mass = num;
				float totalValue = base.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
				if (num < consumeInfo.massConsumptionRate * totalValue * dt)
				{
					value = false;
					break;
				}
			}
			base.sm.hasCorrectFertilizer.Set(value, base.smi, false);
			base.sm.hasIncorrectFertilizer.Set(value2, base.smi, false);
		}

		// Token: 0x06005C6E RID: 23662 RVA: 0x002A9774 File Offset: 0x002A7974
		public void StartAbsorbing()
		{
			if (this.absorberHandle.IsValid())
			{
				return;
			}
			if (base.def.consumedElements == null || base.def.consumedElements.Length == 0)
			{
				return;
			}
			GameObject gameObject = base.smi.gameObject;
			float totalValue = base.gameObject.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
			PlantElementAbsorber.ConsumeInfo[] array = new PlantElementAbsorber.ConsumeInfo[base.def.consumedElements.Length];
			for (int i = 0; i < base.def.consumedElements.Length; i++)
			{
				PlantElementAbsorber.ConsumeInfo consumeInfo = base.def.consumedElements[i];
				consumeInfo.massConsumptionRate *= totalValue;
				array[i] = consumeInfo;
			}
			this.absorberHandle = Game.Instance.plantElementAbsorbers.Add(this.storage, array);
		}

		// Token: 0x06005C6F RID: 23663 RVA: 0x000E0B3E File Offset: 0x000DED3E
		public void StopAbsorbing()
		{
			if (!this.absorberHandle.IsValid())
			{
				return;
			}
			this.absorberHandle = Game.Instance.plantElementAbsorbers.Remove(this.absorberHandle);
		}

		// Token: 0x040041D4 RID: 16852
		public AttributeModifier consumptionRate;

		// Token: 0x040041D5 RID: 16853
		public AttributeModifier absorptionRate;

		// Token: 0x040041D6 RID: 16854
		protected AmountInstance fertilization;

		// Token: 0x040041D7 RID: 16855
		private Storage storage;

		// Token: 0x040041D8 RID: 16856
		private HandleVector<int>.Handle absorberHandle = HandleVector<int>.InvalidHandle;

		// Token: 0x040041D9 RID: 16857
		private float total_available_mass;
	}
}
