using System;
using STRINGS;

// Token: 0x02000D6D RID: 3437
public class DevLightGenerator : Light2D, IMultiSliderControl
{
	// Token: 0x0600429C RID: 17052 RVA: 0x000CF7CE File Offset: 0x000CD9CE
	public DevLightGenerator()
	{
		this.sliderControls = new ISliderControl[]
		{
			new DevLightGenerator.LuxController(this),
			new DevLightGenerator.RangeController(this),
			new DevLightGenerator.FalloffController(this)
		};
	}

	// Token: 0x17000344 RID: 836
	// (get) Token: 0x0600429D RID: 17053 RVA: 0x000CF7FD File Offset: 0x000CD9FD
	string IMultiSliderControl.SidescreenTitleKey
	{
		get
		{
			return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.NAME";
		}
	}

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x0600429E RID: 17054 RVA: 0x000CF804 File Offset: 0x000CDA04
	ISliderControl[] IMultiSliderControl.sliderControls
	{
		get
		{
			return this.sliderControls;
		}
	}

	// Token: 0x0600429F RID: 17055 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	bool IMultiSliderControl.SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x04002DFD RID: 11773
	protected ISliderControl[] sliderControls;

	// Token: 0x02000D6E RID: 3438
	protected class LuxController : ISingleSliderControl, ISliderControl
	{
		// Token: 0x060042A0 RID: 17056 RVA: 0x000CF80C File Offset: 0x000CDA0C
		public LuxController(Light2D t)
		{
			this.target = t;
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060042A1 RID: 17057 RVA: 0x000CF81B File Offset: 0x000CDA1B
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.BRIGHTNESS_LABEL";
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060042A2 RID: 17058 RVA: 0x000CF822 File Offset: 0x000CDA22
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.LIGHT.LUX;
			}
		}

		// Token: 0x060042A3 RID: 17059 RVA: 0x000CEF18 File Offset: 0x000CD118
		public float GetSliderMax(int index)
		{
			return 100000f;
		}

		// Token: 0x060042A4 RID: 17060 RVA: 0x000C18F8 File Offset: 0x000BFAF8
		public float GetSliderMin(int index)
		{
			return 0f;
		}

		// Token: 0x060042A5 RID: 17061 RVA: 0x000CF82E File Offset: 0x000CDA2E
		public string GetSliderTooltip(int index)
		{
			return string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT_LUX, this.target.Lux);
		}

		// Token: 0x060042A6 RID: 17062 RVA: 0x000CF84F File Offset: 0x000CDA4F
		public string GetSliderTooltipKey(int index)
		{
			return "<unused>";
		}

		// Token: 0x060042A7 RID: 17063 RVA: 0x000CF856 File Offset: 0x000CDA56
		public float GetSliderValue(int index)
		{
			return (float)this.target.Lux;
		}

		// Token: 0x060042A8 RID: 17064 RVA: 0x000CF864 File Offset: 0x000CDA64
		public void SetSliderValue(float value, int index)
		{
			this.target.Lux = (int)value;
			this.target.FullRefresh();
		}

		// Token: 0x060042A9 RID: 17065 RVA: 0x000B1628 File Offset: 0x000AF828
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x04002DFE RID: 11774
		protected Light2D target;
	}

	// Token: 0x02000D6F RID: 3439
	protected class RangeController : ISingleSliderControl, ISliderControl
	{
		// Token: 0x060042AA RID: 17066 RVA: 0x000CF87E File Offset: 0x000CDA7E
		public RangeController(Light2D t)
		{
			this.target = t;
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060042AB RID: 17067 RVA: 0x000CF88D File Offset: 0x000CDA8D
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.RANGE_LABEL";
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060042AC RID: 17068 RVA: 0x000CF894 File Offset: 0x000CDA94
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.TILES;
			}
		}

		// Token: 0x060042AD RID: 17069 RVA: 0x000CF8A0 File Offset: 0x000CDAA0
		public float GetSliderMax(int index)
		{
			return 20f;
		}

		// Token: 0x060042AE RID: 17070 RVA: 0x000B95A1 File Offset: 0x000B77A1
		public float GetSliderMin(int index)
		{
			return 1f;
		}

		// Token: 0x060042AF RID: 17071 RVA: 0x000CF8A7 File Offset: 0x000CDAA7
		public string GetSliderTooltip(int index)
		{
			return string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, this.target.Range);
		}

		// Token: 0x060042B0 RID: 17072 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
		public string GetSliderTooltipKey(int index)
		{
			return "";
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x000CF8C8 File Offset: 0x000CDAC8
		public float GetSliderValue(int index)
		{
			return this.target.Range;
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x000CF8D6 File Offset: 0x000CDAD6
		public void SetSliderValue(float value, int index)
		{
			this.target.Range = (float)((int)value);
			this.target.FullRefresh();
		}

		// Token: 0x060042B3 RID: 17075 RVA: 0x000B1628 File Offset: 0x000AF828
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x04002DFF RID: 11775
		protected Light2D target;
	}

	// Token: 0x02000D70 RID: 3440
	protected class FalloffController : ISingleSliderControl, ISliderControl
	{
		// Token: 0x060042B4 RID: 17076 RVA: 0x000CF8F1 File Offset: 0x000CDAF1
		public FalloffController(Light2D t)
		{
			this.target = t;
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060042B5 RID: 17077 RVA: 0x000CF900 File Offset: 0x000CDB00
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.BUILDINGS.PREFABS.DEVLIGHTGENERATOR.FALLOFF_LABEL";
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060042B6 RID: 17078 RVA: 0x000CF907 File Offset: 0x000CDB07
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.PERCENT;
			}
		}

		// Token: 0x060042B7 RID: 17079 RVA: 0x000CD7B4 File Offset: 0x000CB9B4
		public float GetSliderMax(int index)
		{
			return 100f;
		}

		// Token: 0x060042B8 RID: 17080 RVA: 0x000B95A1 File Offset: 0x000B77A1
		public float GetSliderMin(int index)
		{
			return 1f;
		}

		// Token: 0x060042B9 RID: 17081 RVA: 0x000CF913 File Offset: 0x000CDB13
		public string GetSliderTooltip(int index)
		{
			return string.Format("{0}", this.target.FalloffRate * 100f);
		}

		// Token: 0x060042BA RID: 17082 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
		public string GetSliderTooltipKey(int index)
		{
			return "";
		}

		// Token: 0x060042BB RID: 17083 RVA: 0x000CF935 File Offset: 0x000CDB35
		public float GetSliderValue(int index)
		{
			return this.target.FalloffRate * 100f;
		}

		// Token: 0x060042BC RID: 17084 RVA: 0x000CF949 File Offset: 0x000CDB49
		public void SetSliderValue(float value, int index)
		{
			this.target.FalloffRate = value / 100f;
			this.target.FullRefresh();
		}

		// Token: 0x060042BD RID: 17085 RVA: 0x000B1628 File Offset: 0x000AF828
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x04002E00 RID: 11776
		protected Light2D target;
	}
}
