using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020012B8 RID: 4792
[DebuggerDisplay("{name}")]
[Serializable]
public class Element : IComparable<Element>
{
	// Token: 0x060061F2 RID: 25074 RVA: 0x002C30EC File Offset: 0x002C12EC
	public float GetRelativeHeatLevel(float currentTemperature)
	{
		float num = this.lowTemp - 3f;
		float num2 = this.highTemp + 3f;
		return Mathf.Clamp01((currentTemperature - num) / (num2 - num));
	}

	// Token: 0x060061F3 RID: 25075 RVA: 0x000E44FD File Offset: 0x000E26FD
	public float PressureToMass(float pressure)
	{
		return pressure / this.defaultValues.pressure;
	}

	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x060061F4 RID: 25076 RVA: 0x000E450C File Offset: 0x000E270C
	public bool IsSlippery
	{
		get
		{
			return this.HasTag(GameTags.Slippery);
		}
	}

	// Token: 0x170005F7 RID: 1527
	// (get) Token: 0x060061F5 RID: 25077 RVA: 0x000E4519 File Offset: 0x000E2719
	public bool IsUnstable
	{
		get
		{
			return this.HasTag(GameTags.Unstable);
		}
	}

	// Token: 0x170005F8 RID: 1528
	// (get) Token: 0x060061F6 RID: 25078 RVA: 0x000E4526 File Offset: 0x000E2726
	public bool IsLiquid
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Liquid;
		}
	}

	// Token: 0x170005F9 RID: 1529
	// (get) Token: 0x060061F7 RID: 25079 RVA: 0x000E4533 File Offset: 0x000E2733
	public bool IsGas
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Gas;
		}
	}

	// Token: 0x170005FA RID: 1530
	// (get) Token: 0x060061F8 RID: 25080 RVA: 0x000E4540 File Offset: 0x000E2740
	public bool IsSolid
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Solid;
		}
	}

	// Token: 0x170005FB RID: 1531
	// (get) Token: 0x060061F9 RID: 25081 RVA: 0x000E454D File Offset: 0x000E274D
	public bool IsVacuum
	{
		get
		{
			return (this.state & Element.State.Solid) == Element.State.Vacuum;
		}
	}

	// Token: 0x170005FC RID: 1532
	// (get) Token: 0x060061FA RID: 25082 RVA: 0x000E455A File Offset: 0x000E275A
	public bool IsTemperatureInsulated
	{
		get
		{
			return (this.state & Element.State.TemperatureInsulated) > Element.State.Vacuum;
		}
	}

	// Token: 0x060061FB RID: 25083 RVA: 0x000E4568 File Offset: 0x000E2768
	public bool IsState(Element.State expected_state)
	{
		return (this.state & Element.State.Solid) == expected_state;
	}

	// Token: 0x170005FD RID: 1533
	// (get) Token: 0x060061FC RID: 25084 RVA: 0x000E4575 File Offset: 0x000E2775
	public bool HasTransitionUp
	{
		get
		{
			return this.highTempTransitionTarget != (SimHashes)0 && this.highTempTransitionTarget != SimHashes.Unobtanium && this.highTempTransition != null && this.highTempTransition != this;
		}
	}

	// Token: 0x170005FE RID: 1534
	// (get) Token: 0x060061FD RID: 25085 RVA: 0x000E45A2 File Offset: 0x000E27A2
	// (set) Token: 0x060061FE RID: 25086 RVA: 0x000E45AA File Offset: 0x000E27AA
	public string name { get; set; }

	// Token: 0x170005FF RID: 1535
	// (get) Token: 0x060061FF RID: 25087 RVA: 0x000E45B3 File Offset: 0x000E27B3
	// (set) Token: 0x06006200 RID: 25088 RVA: 0x000E45BB File Offset: 0x000E27BB
	public string nameUpperCase { get; set; }

	// Token: 0x17000600 RID: 1536
	// (get) Token: 0x06006201 RID: 25089 RVA: 0x000E45C4 File Offset: 0x000E27C4
	// (set) Token: 0x06006202 RID: 25090 RVA: 0x000E45CC File Offset: 0x000E27CC
	public string description { get; set; }

	// Token: 0x06006203 RID: 25091 RVA: 0x000E45D5 File Offset: 0x000E27D5
	public string GetStateString()
	{
		return Element.GetStateString(this.state);
	}

	// Token: 0x06006204 RID: 25092 RVA: 0x000E45E2 File Offset: 0x000E27E2
	public static string GetStateString(Element.State state)
	{
		if ((state & Element.State.Solid) == Element.State.Solid)
		{
			return ELEMENTS.STATE.SOLID;
		}
		if ((state & Element.State.Solid) == Element.State.Liquid)
		{
			return ELEMENTS.STATE.LIQUID;
		}
		if ((state & Element.State.Solid) == Element.State.Gas)
		{
			return ELEMENTS.STATE.GAS;
		}
		return ELEMENTS.STATE.VACUUM;
	}

	// Token: 0x06006205 RID: 25093 RVA: 0x002C3120 File Offset: 0x002C1320
	public string FullDescription(bool addHardnessColor = true)
	{
		string text = this.Description();
		if (this.IsSolid)
		{
			text += "\n\n";
			text += string.Format(ELEMENTS.ELEMENTDESCSOLID, this.GetMaterialCategoryTag().ProperName(), GameUtil.GetFormattedTemperature(this.highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetHardnessString(this, addHardnessColor));
		}
		else if (this.IsLiquid)
		{
			text += "\n\n";
			text += string.Format(ELEMENTS.ELEMENTDESCLIQUID, this.GetMaterialCategoryTag().ProperName(), GameUtil.GetFormattedTemperature(this.lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(this.highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		}
		else if (!this.IsVacuum)
		{
			text += "\n\n";
			text += string.Format(ELEMENTS.ELEMENTDESCGAS, this.GetMaterialCategoryTag().ProperName(), GameUtil.GetFormattedTemperature(this.lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		}
		string text2 = ELEMENTS.THERMALPROPERTIES;
		text2 = text2.Replace("{SPECIFIC_HEAT_CAPACITY}", GameUtil.GetFormattedSHC(this.specificHeatCapacity));
		text2 = text2.Replace("{THERMAL_CONDUCTIVITY}", GameUtil.GetFormattedThermalConductivity(this.thermalConductivity));
		text = text + "\n" + text2;
		if (DlcManager.FeatureRadiationEnabled())
		{
			text = text + "\n" + string.Format(ELEMENTS.RADIATIONPROPERTIES, this.radiationAbsorptionFactor, GameUtil.GetFormattedRads(this.radiationPer1000Mass * 1.1f / 600f, GameUtil.TimeSlice.PerCycle));
		}
		if (this.oreTags.Length != 0 && !this.IsVacuum)
		{
			text += "\n\n";
			string text3 = "";
			for (int i = 0; i < this.oreTags.Length; i++)
			{
				Tag item = new Tag(this.oreTags[i]);
				if (!GameTags.HiddenElementTags.Contains(item))
				{
					text3 += item.ProperName();
					if (i < this.oreTags.Length - 1)
					{
						text3 += ", ";
					}
				}
			}
			text += string.Format(ELEMENTS.ELEMENTPROPERTIES, text3);
		}
		if (this.attributeModifiers.Count > 0)
		{
			foreach (AttributeModifier attributeModifier in this.attributeModifiers)
			{
				string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
				string formattedString = attributeModifier.GetFormattedString();
				text = text + "\n" + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, name, formattedString);
			}
		}
		return text;
	}

	// Token: 0x06006206 RID: 25094 RVA: 0x000E4621 File Offset: 0x000E2821
	public string Description()
	{
		return this.description;
	}

	// Token: 0x06006207 RID: 25095 RVA: 0x000E4629 File Offset: 0x000E2829
	public bool HasTag(Tag search_tag)
	{
		return this.tag == search_tag || Array.IndexOf<Tag>(this.oreTags, search_tag) != -1;
	}

	// Token: 0x06006208 RID: 25096 RVA: 0x000E464D File Offset: 0x000E284D
	public Tag GetMaterialCategoryTag()
	{
		return this.materialCategory;
	}

	// Token: 0x06006209 RID: 25097 RVA: 0x000E4655 File Offset: 0x000E2855
	public int CompareTo(Element other)
	{
		return this.id - other.id;
	}

	// Token: 0x04004616 RID: 17942
	public const int INVALID_ID = 0;

	// Token: 0x04004617 RID: 17943
	public SimHashes id;

	// Token: 0x04004618 RID: 17944
	public Tag tag;

	// Token: 0x04004619 RID: 17945
	public ushort idx;

	// Token: 0x0400461A RID: 17946
	public float specificHeatCapacity;

	// Token: 0x0400461B RID: 17947
	public float thermalConductivity = 1f;

	// Token: 0x0400461C RID: 17948
	public float molarMass = 1f;

	// Token: 0x0400461D RID: 17949
	public float strength;

	// Token: 0x0400461E RID: 17950
	public float flow;

	// Token: 0x0400461F RID: 17951
	public float maxCompression;

	// Token: 0x04004620 RID: 17952
	public float viscosity;

	// Token: 0x04004621 RID: 17953
	public float minHorizontalFlow = float.PositiveInfinity;

	// Token: 0x04004622 RID: 17954
	public float minVerticalFlow = float.PositiveInfinity;

	// Token: 0x04004623 RID: 17955
	public float maxMass = 10000f;

	// Token: 0x04004624 RID: 17956
	public float solidSurfaceAreaMultiplier;

	// Token: 0x04004625 RID: 17957
	public float liquidSurfaceAreaMultiplier;

	// Token: 0x04004626 RID: 17958
	public float gasSurfaceAreaMultiplier;

	// Token: 0x04004627 RID: 17959
	public Element.State state;

	// Token: 0x04004628 RID: 17960
	public byte hardness;

	// Token: 0x04004629 RID: 17961
	public float lowTemp;

	// Token: 0x0400462A RID: 17962
	public SimHashes lowTempTransitionTarget;

	// Token: 0x0400462B RID: 17963
	public Element lowTempTransition;

	// Token: 0x0400462C RID: 17964
	public float highTemp;

	// Token: 0x0400462D RID: 17965
	public SimHashes highTempTransitionTarget;

	// Token: 0x0400462E RID: 17966
	public Element highTempTransition;

	// Token: 0x0400462F RID: 17967
	public SimHashes highTempTransitionOreID = SimHashes.Vacuum;

	// Token: 0x04004630 RID: 17968
	public float highTempTransitionOreMassConversion;

	// Token: 0x04004631 RID: 17969
	public SimHashes lowTempTransitionOreID = SimHashes.Vacuum;

	// Token: 0x04004632 RID: 17970
	public float lowTempTransitionOreMassConversion;

	// Token: 0x04004633 RID: 17971
	public SimHashes sublimateId;

	// Token: 0x04004634 RID: 17972
	public SimHashes convertId;

	// Token: 0x04004635 RID: 17973
	public SpawnFXHashes sublimateFX;

	// Token: 0x04004636 RID: 17974
	public float sublimateRate;

	// Token: 0x04004637 RID: 17975
	public float sublimateEfficiency;

	// Token: 0x04004638 RID: 17976
	public float sublimateProbability;

	// Token: 0x04004639 RID: 17977
	public float offGasPercentage;

	// Token: 0x0400463A RID: 17978
	public float lightAbsorptionFactor;

	// Token: 0x0400463B RID: 17979
	public float radiationAbsorptionFactor;

	// Token: 0x0400463C RID: 17980
	public float radiationPer1000Mass;

	// Token: 0x0400463D RID: 17981
	public Sim.PhysicsData defaultValues;

	// Token: 0x0400463E RID: 17982
	public float toxicity;

	// Token: 0x0400463F RID: 17983
	public Substance substance;

	// Token: 0x04004640 RID: 17984
	public Tag materialCategory;

	// Token: 0x04004641 RID: 17985
	public int buildMenuSort;

	// Token: 0x04004642 RID: 17986
	public ElementLoader.ElementComposition[] elementComposition;

	// Token: 0x04004643 RID: 17987
	public Tag[] oreTags = new Tag[0];

	// Token: 0x04004644 RID: 17988
	public List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();

	// Token: 0x04004645 RID: 17989
	public bool disabled;

	// Token: 0x04004646 RID: 17990
	public string dlcId;

	// Token: 0x04004647 RID: 17991
	public const byte StateMask = 3;

	// Token: 0x020012B9 RID: 4793
	[Serializable]
	public enum State : byte
	{
		// Token: 0x0400464C RID: 17996
		Vacuum,
		// Token: 0x0400464D RID: 17997
		Gas,
		// Token: 0x0400464E RID: 17998
		Liquid,
		// Token: 0x0400464F RID: 17999
		Solid,
		// Token: 0x04004650 RID: 18000
		Unbreakable,
		// Token: 0x04004651 RID: 18001
		Unstable = 8,
		// Token: 0x04004652 RID: 18002
		TemperatureInsulated = 16
	}
}
