using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020012C0 RID: 4800
[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConverter : StateMachineComponent<ElementConverter.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06006238 RID: 25144 RVA: 0x000E48A8 File Offset: 0x000E2AA8
	public void SetWorkSpeedMultiplier(float speed)
	{
		this.workSpeedMultiplier = speed;
	}

	// Token: 0x06006239 RID: 25145 RVA: 0x002C3D04 File Offset: 0x002C1F04
	public void SetConsumedElementActive(Tag elementId, bool active)
	{
		int i = 0;
		while (i < this.consumedElements.Length)
		{
			if (!(this.consumedElements[i].Tag != elementId))
			{
				this.consumedElements[i].IsActive = active;
				if (!this.ShowInUI)
				{
					break;
				}
				ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
				if (active)
				{
					base.smi.AddStatusItem<ElementConverter.ConsumedElement, Tag>(consumedElement, consumedElement.Tag, ElementConverter.ElementConverterInput, this.consumedElementStatusHandles);
					return;
				}
				base.smi.RemoveStatusItem<Tag>(consumedElement.Tag, this.consumedElementStatusHandles);
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600623A RID: 25146 RVA: 0x002C3DA0 File Offset: 0x002C1FA0
	public void SetOutputElementActive(SimHashes element, bool active)
	{
		int i = 0;
		while (i < this.outputElements.Length)
		{
			if (this.outputElements[i].elementHash == element)
			{
				this.outputElements[i].IsActive = active;
				ElementConverter.OutputElement outputElement = this.outputElements[i];
				if (active)
				{
					base.smi.AddStatusItem<ElementConverter.OutputElement, SimHashes>(outputElement, outputElement.elementHash, ElementConverter.ElementConverterOutput, this.outputElementStatusHandles);
					return;
				}
				base.smi.RemoveStatusItem<SimHashes>(outputElement.elementHash, this.outputElementStatusHandles);
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600623B RID: 25147 RVA: 0x000E48B1 File Offset: 0x000E2AB1
	public void SetStorage(Storage storage)
	{
		this.storage = storage;
	}

	// Token: 0x17000603 RID: 1539
	// (get) Token: 0x0600623C RID: 25148 RVA: 0x000E48BA File Offset: 0x000E2ABA
	// (set) Token: 0x0600623D RID: 25149 RVA: 0x000E48C2 File Offset: 0x000E2AC2
	public float OutputMultiplier
	{
		get
		{
			return this.outputMultiplier;
		}
		set
		{
			this.outputMultiplier = value;
		}
	}

	// Token: 0x17000604 RID: 1540
	// (get) Token: 0x0600623E RID: 25150 RVA: 0x000E48CB File Offset: 0x000E2ACB
	public float AverageConvertRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.outputElements[0].accumulator);
		}
	}

	// Token: 0x0600623F RID: 25151 RVA: 0x002C3E2C File Offset: 0x002C202C
	public bool HasEnoughMass(Tag tag, bool includeInactive = false)
	{
		bool result = false;
		List<GameObject> items = this.storage.items;
		foreach (ElementConverter.ConsumedElement consumedElement in this.consumedElements)
		{
			if (!(tag != consumedElement.Tag) && (includeInactive || consumedElement.IsActive))
			{
				float num = 0f;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (!(gameObject == null) && gameObject.HasTag(tag))
					{
						num += gameObject.GetComponent<PrimaryElement>().Mass;
					}
				}
				result = (num >= consumedElement.MassConsumptionRate);
				break;
			}
		}
		return result;
	}

	// Token: 0x06006240 RID: 25152 RVA: 0x002C3EE4 File Offset: 0x002C20E4
	public bool HasEnoughMassToStartConverting(bool includeInactive = false)
	{
		float speedMultiplier = this.GetSpeedMultiplier();
		float num = 1f * speedMultiplier;
		bool flag = includeInactive || this.consumedElements.Length == 0;
		bool flag2 = true;
		List<GameObject> items = this.storage.items;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
			flag |= consumedElement.IsActive;
			if (includeInactive || consumedElement.IsActive)
			{
				float num2 = 0f;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (!(gameObject == null) && gameObject.HasTag(consumedElement.Tag))
					{
						num2 += gameObject.GetComponent<PrimaryElement>().Mass;
					}
				}
				if (num2 < consumedElement.MassConsumptionRate * num)
				{
					flag2 = false;
					break;
				}
			}
		}
		return flag && flag2;
	}

	// Token: 0x06006241 RID: 25153 RVA: 0x002C3FCC File Offset: 0x002C21CC
	public bool CanConvertAtAll()
	{
		bool flag = this.consumedElements.Length == 0;
		bool flag2 = true;
		List<GameObject> items = this.storage.items;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
			flag |= consumedElement.IsActive;
			if (consumedElement.IsActive)
			{
				bool flag3 = false;
				for (int j = 0; j < items.Count; j++)
				{
					GameObject gameObject = items[j];
					if (!(gameObject == null) && gameObject.HasTag(consumedElement.Tag) && gameObject.GetComponent<PrimaryElement>().Mass > 0f)
					{
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					flag2 = false;
					break;
				}
			}
		}
		return flag && flag2;
	}

	// Token: 0x06006242 RID: 25154 RVA: 0x000E48ED File Offset: 0x000E2AED
	private float GetSpeedMultiplier()
	{
		return this.machinerySpeedAttribute.GetTotalValue() * this.workSpeedMultiplier;
	}

	// Token: 0x06006243 RID: 25155 RVA: 0x002C408C File Offset: 0x002C228C
	private void ConvertMass()
	{
		float speedMultiplier = this.GetSpeedMultiplier();
		float num = 1f * speedMultiplier;
		bool flag = this.consumedElements.Length == 0;
		float num2 = 1f;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ElementConverter.ConsumedElement consumedElement = this.consumedElements[i];
			flag |= consumedElement.IsActive;
			if (consumedElement.IsActive)
			{
				float num3 = consumedElement.MassConsumptionRate * num * num2;
				if (num3 <= 0f)
				{
					num2 = 0f;
					break;
				}
				float num4 = 0f;
				for (int j = 0; j < this.storage.items.Count; j++)
				{
					GameObject gameObject = this.storage.items[j];
					if (!(gameObject == null) && gameObject.HasTag(consumedElement.Tag))
					{
						PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
						float num5 = Mathf.Min(num3, component.Mass);
						num4 += num5 / num3;
					}
				}
				num2 = Mathf.Min(num2, num4);
			}
		}
		if (!flag || num2 <= 0f)
		{
			return;
		}
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		diseaseInfo.idx = byte.MaxValue;
		diseaseInfo.count = 0;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		for (int k = 0; k < this.consumedElements.Length; k++)
		{
			ElementConverter.ConsumedElement consumedElement2 = this.consumedElements[k];
			if (consumedElement2.IsActive)
			{
				float num9 = consumedElement2.MassConsumptionRate * num * num2;
				Game.Instance.accumulators.Accumulate(consumedElement2.Accumulator, num9);
				for (int l = 0; l < this.storage.items.Count; l++)
				{
					GameObject gameObject2 = this.storage.items[l];
					if (!(gameObject2 == null))
					{
						if (gameObject2.HasTag(consumedElement2.Tag))
						{
							PrimaryElement component2 = gameObject2.GetComponent<PrimaryElement>();
							component2.KeepZeroMassObject = true;
							float num10 = Mathf.Min(num9, component2.Mass);
							int num11 = (int)(num10 / component2.Mass * (float)component2.DiseaseCount);
							float num12 = num10 * component2.Element.specificHeatCapacity;
							num8 += num12;
							num7 += num12 * component2.Temperature;
							component2.Mass -= num10;
							component2.ModifyDiseaseCount(-num11, "ElementConverter.ConvertMass");
							num6 += num10;
							diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo.idx, diseaseInfo.count, component2.DiseaseIdx, num11);
							num9 -= num10;
							if (num9 <= 0f)
							{
								break;
							}
						}
						if (num9 <= 0f)
						{
							global::Debug.Assert(num9 <= 0f);
						}
					}
				}
			}
		}
		float num13 = (num8 > 0f) ? (num7 / num8) : 0f;
		if (this.onConvertMass != null && num6 > 0f)
		{
			this.onConvertMass(num6);
		}
		for (int m = 0; m < this.outputElements.Length; m++)
		{
			ElementConverter.OutputElement outputElement = this.outputElements[m];
			if (outputElement.IsActive)
			{
				SimUtil.DiseaseInfo diseaseInfo2 = diseaseInfo;
				if (this.totalDiseaseWeight <= 0f)
				{
					diseaseInfo2.idx = byte.MaxValue;
					diseaseInfo2.count = 0;
				}
				else
				{
					float num14 = outputElement.diseaseWeight / this.totalDiseaseWeight;
					diseaseInfo2.count = (int)((float)diseaseInfo2.count * num14);
				}
				if (outputElement.addedDiseaseIdx != 255)
				{
					diseaseInfo2 = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo2, new SimUtil.DiseaseInfo
					{
						idx = outputElement.addedDiseaseIdx,
						count = outputElement.addedDiseaseCount
					});
				}
				float num15 = outputElement.massGenerationRate * this.OutputMultiplier * num * num2;
				Game.Instance.accumulators.Accumulate(outputElement.accumulator, num15);
				float temperature;
				if (outputElement.useEntityTemperature || (num13 == 0f && outputElement.minOutputTemperature == 0f))
				{
					temperature = base.GetComponent<PrimaryElement>().Temperature;
				}
				else
				{
					temperature = Mathf.Max(outputElement.minOutputTemperature, num13);
				}
				Element element = ElementLoader.FindElementByHash(outputElement.elementHash);
				if (outputElement.storeOutput)
				{
					PrimaryElement primaryElement = this.storage.AddToPrimaryElement(outputElement.elementHash, num15, temperature);
					if (primaryElement == null)
					{
						if (element.IsGas)
						{
							this.storage.AddGasChunk(outputElement.elementHash, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, true);
						}
						else if (element.IsLiquid)
						{
							this.storage.AddLiquid(outputElement.elementHash, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, true);
						}
						else
						{
							GameObject go = element.substance.SpawnResource(base.transform.GetPosition(), num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, false, false);
							this.storage.Store(go, true, false, true, false);
						}
					}
					else
					{
						primaryElement.AddDisease(diseaseInfo2.idx, diseaseInfo2.count, "ElementConverter.ConvertMass");
					}
				}
				else
				{
					Vector3 vector = new Vector3(base.transform.GetPosition().x + outputElement.outputElementOffset.x, base.transform.GetPosition().y + outputElement.outputElementOffset.y, 0f);
					int num16 = Grid.PosToCell(vector);
					if (element.IsLiquid)
					{
						FallingWater.instance.AddParticle(num16, element.idx, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, false, false, false);
					}
					else if (element.IsSolid)
					{
						element.substance.SpawnResource(vector, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, false, false, false);
					}
					else
					{
						SimMessages.AddRemoveSubstance(num16, outputElement.elementHash, CellEventLogger.Instance.OxygenModifierSimUpdate, num15, temperature, diseaseInfo2.idx, diseaseInfo2.count, true, -1);
					}
				}
				if (outputElement.elementHash == SimHashes.Oxygen || outputElement.elementHash == SimHashes.ContaminatedOxygen)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, num15, base.gameObject.GetProperName(), null);
				}
			}
		}
		this.storage.Trigger(-1697596308, base.gameObject);
	}

	// Token: 0x06006244 RID: 25156 RVA: 0x002C46F8 File Offset: 0x002C28F8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Attributes attributes = base.gameObject.GetAttributes();
		this.machinerySpeedAttribute = attributes.Add(Db.Get().Attributes.MachinerySpeed);
		if (ElementConverter.ElementConverterInput == null)
		{
			ElementConverter.ElementConverterInput = new StatusItem("ElementConverterInput", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				ElementConverter.ConsumedElement consumedElement = (ElementConverter.ConsumedElement)data;
				str = str.Replace("{ElementTypes}", consumedElement.Name);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedByTag(consumedElement.Tag, consumedElement.Rate, GameUtil.TimeSlice.PerSecond));
				return str;
			});
		}
		if (ElementConverter.ElementConverterOutput == null)
		{
			ElementConverter.ElementConverterOutput = new StatusItem("ElementConverterOutput", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				ElementConverter.OutputElement outputElement = (ElementConverter.OutputElement)data;
				str = str.Replace("{ElementTypes}", outputElement.Name);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(outputElement.Rate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			});
		}
	}

	// Token: 0x06006245 RID: 25157 RVA: 0x002C47D8 File Offset: 0x002C29D8
	public void SetAllConsumedActive(bool active)
	{
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			this.consumedElements[i].IsActive = active;
		}
		base.smi.sm.canConvert.Set(active, base.smi, false);
	}

	// Token: 0x06006246 RID: 25158 RVA: 0x002C4828 File Offset: 0x002C2A28
	public void SetConsumedActive(Tag id, bool active)
	{
		bool flag = this.consumedElements.Length == 0;
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			ref ElementConverter.ConsumedElement ptr = ref this.consumedElements[i];
			if (ptr.Tag == id)
			{
				ptr.IsActive = active;
				if (active)
				{
					flag = true;
					break;
				}
			}
			flag |= ptr.IsActive;
		}
		base.smi.sm.canConvert.Set(flag, base.smi, false);
	}

	// Token: 0x06006247 RID: 25159 RVA: 0x002C48A4 File Offset: 0x002C2AA4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			this.consumedElements[i].Accumulator = Game.Instance.accumulators.Add("ElementsConsumed", this);
		}
		this.totalDiseaseWeight = 0f;
		for (int j = 0; j < this.outputElements.Length; j++)
		{
			this.outputElements[j].accumulator = Game.Instance.accumulators.Add("OutputElements", this);
			this.totalDiseaseWeight += this.outputElements[j].diseaseWeight;
		}
		base.smi.StartSM();
	}

	// Token: 0x06006248 RID: 25160 RVA: 0x002C4960 File Offset: 0x002C2B60
	protected override void OnCleanUp()
	{
		for (int i = 0; i < this.consumedElements.Length; i++)
		{
			Game.Instance.accumulators.Remove(this.consumedElements[i].Accumulator);
		}
		for (int j = 0; j < this.outputElements.Length; j++)
		{
			Game.Instance.accumulators.Remove(this.outputElements[j].accumulator);
		}
		base.OnCleanUp();
	}

	// Token: 0x06006249 RID: 25161 RVA: 0x002C49DC File Offset: 0x002C2BDC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (!this.showDescriptors)
		{
			return list;
		}
		if (this.consumedElements != null)
		{
			foreach (ElementConverter.ConsumedElement consumedElement in this.consumedElements)
			{
				if (consumedElement.IsActive)
				{
					Descriptor item = default(Descriptor);
					item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, consumedElement.Name, GameUtil.GetFormattedMass(consumedElement.MassConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, consumedElement.Name, GameUtil.GetFormattedMass(consumedElement.MassConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
					list.Add(item);
				}
			}
		}
		if (this.outputElements != null)
		{
			foreach (ElementConverter.OutputElement outputElement in this.outputElements)
			{
				if (outputElement.IsActive)
				{
					LocString loc_string = UI.BUILDINGEFFECTS.ELEMENTEMITTED_INPUTTEMP;
					LocString loc_string2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_INPUTTEMP;
					if (outputElement.useEntityTemperature)
					{
						loc_string = UI.BUILDINGEFFECTS.ELEMENTEMITTED_ENTITYTEMP;
						loc_string2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_ENTITYTEMP;
					}
					else if (outputElement.minOutputTemperature > 0f)
					{
						loc_string = UI.BUILDINGEFFECTS.ELEMENTEMITTED_MINTEMP;
						loc_string2 = UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_MINTEMP;
					}
					Descriptor item2 = new Descriptor(string.Format(loc_string, outputElement.Name, GameUtil.GetFormattedMass(outputElement.massGenerationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(outputElement.minOutputTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(loc_string2, outputElement.Name, GameUtil.GetFormattedMass(outputElement.massGenerationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(outputElement.minOutputTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false);
					list.Add(item2);
				}
			}
		}
		return list;
	}

	// Token: 0x04004674 RID: 18036
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04004675 RID: 18037
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04004676 RID: 18038
	public Action<float> onConvertMass;

	// Token: 0x04004677 RID: 18039
	private float totalDiseaseWeight = float.MaxValue;

	// Token: 0x04004678 RID: 18040
	public Operational.State OperationalRequirement = Operational.State.Active;

	// Token: 0x04004679 RID: 18041
	private AttributeInstance machinerySpeedAttribute;

	// Token: 0x0400467A RID: 18042
	private float workSpeedMultiplier = 1f;

	// Token: 0x0400467B RID: 18043
	public bool showDescriptors = true;

	// Token: 0x0400467C RID: 18044
	private const float BASE_INTERVAL = 1f;

	// Token: 0x0400467D RID: 18045
	public ElementConverter.ConsumedElement[] consumedElements;

	// Token: 0x0400467E RID: 18046
	public ElementConverter.OutputElement[] outputElements;

	// Token: 0x0400467F RID: 18047
	public bool ShowInUI = true;

	// Token: 0x04004680 RID: 18048
	private float outputMultiplier = 1f;

	// Token: 0x04004681 RID: 18049
	private Dictionary<Tag, Guid> consumedElementStatusHandles = new Dictionary<Tag, Guid>();

	// Token: 0x04004682 RID: 18050
	private Dictionary<SimHashes, Guid> outputElementStatusHandles = new Dictionary<SimHashes, Guid>();

	// Token: 0x04004683 RID: 18051
	private static StatusItem ElementConverterInput;

	// Token: 0x04004684 RID: 18052
	private static StatusItem ElementConverterOutput;

	// Token: 0x020012C1 RID: 4801
	[DebuggerDisplay("{tag} {massConsumptionRate}")]
	[Serializable]
	public struct ConsumedElement
	{
		// Token: 0x0600624B RID: 25163 RVA: 0x000E4901 File Offset: 0x000E2B01
		public ConsumedElement(Tag tag, float kgPerSecond, bool isActive = true)
		{
			this.Tag = tag;
			this.MassConsumptionRate = kgPerSecond;
			this.IsActive = isActive;
			this.Accumulator = HandleVector<int>.InvalidHandle;
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x0600624C RID: 25164 RVA: 0x000E4923 File Offset: 0x000E2B23
		public string Name
		{
			get
			{
				return this.Tag.ProperName();
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x0600624D RID: 25165 RVA: 0x000E4930 File Offset: 0x000E2B30
		public float Rate
		{
			get
			{
				return Game.Instance.accumulators.GetAverageRate(this.Accumulator);
			}
		}

		// Token: 0x04004685 RID: 18053
		public Tag Tag;

		// Token: 0x04004686 RID: 18054
		public float MassConsumptionRate;

		// Token: 0x04004687 RID: 18055
		public bool IsActive;

		// Token: 0x04004688 RID: 18056
		public HandleVector<int>.Handle Accumulator;
	}

	// Token: 0x020012C2 RID: 4802
	[Serializable]
	public struct OutputElement
	{
		// Token: 0x0600624E RID: 25166 RVA: 0x002C4BF8 File Offset: 0x002C2DF8
		public OutputElement(float kgPerSecond, SimHashes element, float minOutputTemperature, bool useEntityTemperature = false, bool storeOutput = false, float outputElementOffsetx = 0f, float outputElementOffsety = 0.5f, float diseaseWeight = 1f, byte addedDiseaseIdx = 255, int addedDiseaseCount = 0, bool isActive = true)
		{
			this.elementHash = element;
			this.minOutputTemperature = minOutputTemperature;
			this.useEntityTemperature = useEntityTemperature;
			this.storeOutput = storeOutput;
			this.massGenerationRate = kgPerSecond;
			this.outputElementOffset = new Vector2(outputElementOffsetx, outputElementOffsety);
			this.accumulator = HandleVector<int>.InvalidHandle;
			this.diseaseWeight = diseaseWeight;
			this.addedDiseaseIdx = addedDiseaseIdx;
			this.addedDiseaseCount = addedDiseaseCount;
			this.IsActive = isActive;
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x0600624F RID: 25167 RVA: 0x000E4947 File Offset: 0x000E2B47
		public string Name
		{
			get
			{
				return ElementLoader.FindElementByHash(this.elementHash).tag.ProperName();
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06006250 RID: 25168 RVA: 0x000E495E File Offset: 0x000E2B5E
		public float Rate
		{
			get
			{
				return Game.Instance.accumulators.GetAverageRate(this.accumulator);
			}
		}

		// Token: 0x04004689 RID: 18057
		public bool IsActive;

		// Token: 0x0400468A RID: 18058
		public SimHashes elementHash;

		// Token: 0x0400468B RID: 18059
		public float minOutputTemperature;

		// Token: 0x0400468C RID: 18060
		public bool useEntityTemperature;

		// Token: 0x0400468D RID: 18061
		public float massGenerationRate;

		// Token: 0x0400468E RID: 18062
		public bool storeOutput;

		// Token: 0x0400468F RID: 18063
		public Vector2 outputElementOffset;

		// Token: 0x04004690 RID: 18064
		public HandleVector<int>.Handle accumulator;

		// Token: 0x04004691 RID: 18065
		public float diseaseWeight;

		// Token: 0x04004692 RID: 18066
		public byte addedDiseaseIdx;

		// Token: 0x04004693 RID: 18067
		public int addedDiseaseCount;
	}

	// Token: 0x020012C3 RID: 4803
	public class StatesInstance : GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.GameInstance
	{
		// Token: 0x06006251 RID: 25169 RVA: 0x000E4975 File Offset: 0x000E2B75
		public StatesInstance(ElementConverter smi) : base(smi)
		{
			this.selectable = base.GetComponent<KSelectable>();
		}

		// Token: 0x06006252 RID: 25170 RVA: 0x002C4C64 File Offset: 0x002C2E64
		public void AddStatusItems()
		{
			if (!base.master.ShowInUI)
			{
				return;
			}
			foreach (ElementConverter.ConsumedElement consumedElement in base.master.consumedElements)
			{
				if (consumedElement.IsActive)
				{
					this.AddStatusItem<ElementConverter.ConsumedElement, Tag>(consumedElement, consumedElement.Tag, ElementConverter.ElementConverterInput, base.master.consumedElementStatusHandles);
				}
			}
			foreach (ElementConverter.OutputElement outputElement in base.master.outputElements)
			{
				if (outputElement.IsActive)
				{
					this.AddStatusItem<ElementConverter.OutputElement, SimHashes>(outputElement, outputElement.elementHash, ElementConverter.ElementConverterOutput, base.master.outputElementStatusHandles);
				}
			}
		}

		// Token: 0x06006253 RID: 25171 RVA: 0x002C4D14 File Offset: 0x002C2F14
		public void RemoveStatusItems()
		{
			if (!base.master.ShowInUI)
			{
				return;
			}
			for (int i = 0; i < base.master.consumedElements.Length; i++)
			{
				ElementConverter.ConsumedElement consumedElement = base.master.consumedElements[i];
				this.RemoveStatusItem<Tag>(consumedElement.Tag, base.master.consumedElementStatusHandles);
			}
			for (int j = 0; j < base.master.outputElements.Length; j++)
			{
				ElementConverter.OutputElement outputElement = base.master.outputElements[j];
				this.RemoveStatusItem<SimHashes>(outputElement.elementHash, base.master.outputElementStatusHandles);
			}
			base.master.consumedElementStatusHandles.Clear();
			base.master.outputElementStatusHandles.Clear();
		}

		// Token: 0x06006254 RID: 25172 RVA: 0x002C4DD4 File Offset: 0x002C2FD4
		public void AddStatusItem<ElementType, IDType>(ElementType element, IDType id, StatusItem status, Dictionary<IDType, Guid> collection)
		{
			Guid value = this.selectable.AddStatusItem(status, element);
			collection[id] = value;
		}

		// Token: 0x06006255 RID: 25173 RVA: 0x002C4E00 File Offset: 0x002C3000
		public void RemoveStatusItem<IDType>(IDType id, Dictionary<IDType, Guid> collection)
		{
			Guid guid;
			if (!collection.TryGetValue(id, out guid))
			{
				return;
			}
			this.selectable.RemoveStatusItem(guid, false);
		}

		// Token: 0x06006256 RID: 25174 RVA: 0x002C4E28 File Offset: 0x002C3028
		public void OnOperationalRequirementChanged(object data)
		{
			Operational operational = data as Operational;
			bool value = (operational == null) ? ((bool)data) : operational.IsActive;
			base.sm.canConvert.Set(value, this, false);
		}

		// Token: 0x04004694 RID: 18068
		private KSelectable selectable;
	}

	// Token: 0x020012C4 RID: 4804
	public class States : GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter>
	{
		// Token: 0x06006257 RID: 25175 RVA: 0x002C4E68 File Offset: 0x002C3068
		private bool ValidateStateTransition(ElementConverter.StatesInstance smi, bool _)
		{
			bool flag = smi.GetCurrentState() == smi.sm.disabled;
			if (smi.master.operational == null)
			{
				return flag;
			}
			bool flag2 = smi.master.consumedElements.Length == 0;
			bool flag3 = this.canConvert.Get(smi);
			int num = 0;
			while (!flag2 && num < smi.master.consumedElements.Length)
			{
				flag2 = smi.master.consumedElements[num].IsActive;
				num++;
			}
			if (flag3 && !flag2)
			{
				this.canConvert.Set(false, smi, true);
				return false;
			}
			return smi.master.operational.MeetsRequirements(smi.master.OperationalRequirement) == flag;
		}

		// Token: 0x06006258 RID: 25176 RVA: 0x002C4F24 File Offset: 0x002C3124
		private void OnEnterRoot(ElementConverter.StatesInstance smi)
		{
			int eventForState = (int)Operational.GetEventForState(smi.master.OperationalRequirement);
			smi.Subscribe(eventForState, new Action<object>(smi.OnOperationalRequirementChanged));
		}

		// Token: 0x06006259 RID: 25177 RVA: 0x002C4F58 File Offset: 0x002C3158
		private void OnExitRoot(ElementConverter.StatesInstance smi)
		{
			int eventForState = (int)Operational.GetEventForState(smi.master.OperationalRequirement);
			smi.Unsubscribe(eventForState, new Action<object>(smi.OnOperationalRequirementChanged));
		}

		// Token: 0x0600625A RID: 25178 RVA: 0x002C4F8C File Offset: 0x002C318C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.Enter(new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State.Callback(this.OnEnterRoot)).Exit(new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State.Callback(this.OnExitRoot));
			this.disabled.ParamTransition<bool>(this.canConvert, this.converting, new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.Parameter<bool>.Callback(this.ValidateStateTransition));
			this.converting.Enter("AddStatusItems", delegate(ElementConverter.StatesInstance smi)
			{
				smi.AddStatusItems();
			}).Exit("RemoveStatusItems", delegate(ElementConverter.StatesInstance smi)
			{
				smi.RemoveStatusItems();
			}).ParamTransition<bool>(this.canConvert, this.disabled, new StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.Parameter<bool>.Callback(this.ValidateStateTransition)).Update("ConvertMass", delegate(ElementConverter.StatesInstance smi, float dt)
			{
				smi.master.ConvertMass();
			}, UpdateRate.SIM_1000ms, true);
		}

		// Token: 0x04004695 RID: 18069
		public GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State disabled;

		// Token: 0x04004696 RID: 18070
		public GameStateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.State converting;

		// Token: 0x04004697 RID: 18071
		public StateMachine<ElementConverter.States, ElementConverter.StatesInstance, ElementConverter, object>.BoolParameter canConvert;
	}
}
