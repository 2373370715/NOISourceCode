using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CEE RID: 7406
public class DebugPaintElementScreen : KScreen
{
	// Token: 0x17000A2E RID: 2606
	// (get) Token: 0x06009A7E RID: 39550 RVA: 0x00108FDB File Offset: 0x001071DB
	// (set) Token: 0x06009A7F RID: 39551 RVA: 0x00108FE2 File Offset: 0x001071E2
	public static DebugPaintElementScreen Instance { get; private set; }

	// Token: 0x06009A80 RID: 39552 RVA: 0x00108FEA File Offset: 0x001071EA
	public static void DestroyInstance()
	{
		DebugPaintElementScreen.Instance = null;
	}

	// Token: 0x06009A81 RID: 39553 RVA: 0x003C7C44 File Offset: 0x003C5E44
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DebugPaintElementScreen.Instance = this;
		this.SetupLocText();
		this.inputFields.Add(this.massInput);
		this.inputFields.Add(this.temperatureInput);
		this.inputFields.Add(this.diseaseCountInput);
		this.inputFields.Add(this.filterInput);
		foreach (KInputTextField kinputTextField in this.inputFields)
		{
			kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
			{
				base.isEditing = true;
			}));
			kinputTextField.onEndEdit.AddListener(delegate(string value)
			{
				base.isEditing = false;
			});
		}
		this.temperatureInput.onEndEdit.AddListener(delegate(string value)
		{
			this.OnChangeTemperature();
		});
		this.massInput.onEndEdit.AddListener(delegate(string value)
		{
			this.OnChangeMass();
		});
		this.diseaseCountInput.onEndEdit.AddListener(delegate(string value)
		{
			this.OnDiseaseCountChange();
		});
		base.gameObject.SetActive(false);
		this.activateOnSpawn = true;
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06009A82 RID: 39554 RVA: 0x003C7D8C File Offset: 0x003C5F8C
	private void SetupLocText()
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("Title").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.TITLE;
		component.GetReference<LocText>("ElementLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ELEMENT;
		component.GetReference<LocText>("MassLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.MASS_KG;
		component.GetReference<LocText>("TemperatureLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.TEMPERATURE_KELVIN;
		component.GetReference<LocText>("DiseaseLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE;
		component.GetReference<LocText>("DiseaseCountLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE_COUNT;
		component.GetReference<LocText>("AddFoWMaskLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ADD_FOW_MASK;
		component.GetReference<LocText>("RemoveFoWMaskLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.REMOVE_FOW_MASK;
		this.elementButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ELEMENT;
		this.diseaseButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE;
		this.paintButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.PAINT;
		this.fillButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.FILL;
		this.spawnButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.SPAWN_ALL;
		this.sampleButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.SAMPLE;
		this.storeButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.STORE;
		this.affectBuildings.transform.parent.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.BUILDINGS;
		this.affectCells.transform.parent.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.CELLS;
	}

	// Token: 0x06009A83 RID: 39555 RVA: 0x003C7F80 File Offset: 0x003C6180
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.element = SimHashes.Ice;
		this.diseaseIdx = byte.MaxValue;
		this.ConfigureElements();
		List<string> list = new List<string>();
		list.Insert(0, "None");
		foreach (Disease disease in Db.Get().Diseases.resources)
		{
			list.Add(disease.Name);
		}
		this.diseasePopup.SetOptions(list.ToArray());
		KPopupMenu kpopupMenu = this.diseasePopup;
		kpopupMenu.OnSelect = (Action<string, int>)Delegate.Combine(kpopupMenu.OnSelect, new Action<string, int>(this.OnSelectDisease));
		this.SelectDiseaseOption((int)this.diseaseIdx);
		this.paintButton.onClick += this.OnClickPaint;
		this.fillButton.onClick += this.OnClickFill;
		this.sampleButton.onClick += this.OnClickSample;
		this.storeButton.onClick += this.OnClickStore;
		if (SaveGame.Instance.worldGenSpawner.SpawnsRemain())
		{
			this.spawnButton.onClick += this.OnClickSpawn;
		}
		KPopupMenu kpopupMenu2 = this.elementPopup;
		kpopupMenu2.OnSelect = (Action<string, int>)Delegate.Combine(kpopupMenu2.OnSelect, new Action<string, int>(this.OnSelectElement));
		this.elementButton.onClick += this.elementPopup.OnClick;
		this.diseaseButton.onClick += this.diseasePopup.OnClick;
	}

	// Token: 0x06009A84 RID: 39556 RVA: 0x003C813C File Offset: 0x003C633C
	private void FilterElements(string filterValue)
	{
		if (string.IsNullOrEmpty(filterValue))
		{
			foreach (KButtonMenu.ButtonInfo buttonInfo in this.elementPopup.GetButtons())
			{
				buttonInfo.uibutton.gameObject.SetActive(true);
			}
			return;
		}
		filterValue = this.filter.ToLower();
		foreach (KButtonMenu.ButtonInfo buttonInfo2 in this.elementPopup.GetButtons())
		{
			buttonInfo2.uibutton.gameObject.SetActive(buttonInfo2.text.ToLower().Contains(filterValue));
		}
	}

	// Token: 0x06009A85 RID: 39557 RVA: 0x003C8208 File Offset: 0x003C6408
	private void ConfigureElements()
	{
		if (this.filter != null)
		{
			this.filter = this.filter.ToLower();
		}
		List<DebugPaintElementScreen.ElemDisplayInfo> list = new List<DebugPaintElementScreen.ElemDisplayInfo>();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.name != "Element Not Loaded" && element.substance != null && element.substance.showInEditor && (string.IsNullOrEmpty(this.filter) || element.name.ToLower().Contains(this.filter)))
			{
				list.Add(new DebugPaintElementScreen.ElemDisplayInfo
				{
					id = element.id,
					displayStr = element.name + " (" + element.GetStateString() + ")"
				});
			}
		}
		list.Sort((DebugPaintElementScreen.ElemDisplayInfo a, DebugPaintElementScreen.ElemDisplayInfo b) => a.displayStr.CompareTo(b.displayStr));
		if (string.IsNullOrEmpty(this.filter))
		{
			SimHashes[] array = new SimHashes[]
			{
				SimHashes.SlimeMold,
				SimHashes.Vacuum,
				SimHashes.Dirt,
				SimHashes.CarbonDioxide,
				SimHashes.Water,
				SimHashes.Oxygen
			};
			for (int i = 0; i < array.Length; i++)
			{
				Element element2 = ElementLoader.FindElementByHash(array[i]);
				list.Insert(0, new DebugPaintElementScreen.ElemDisplayInfo
				{
					id = element2.id,
					displayStr = element2.name + " (" + element2.GetStateString() + ")"
				});
			}
		}
		this.options_list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (DebugPaintElementScreen.ElemDisplayInfo elemDisplayInfo in list)
		{
			list2.Add(elemDisplayInfo.displayStr);
			this.options_list.Add(elemDisplayInfo.id.ToString());
		}
		this.elementPopup.SetOptions(list2);
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].id == this.element)
			{
				this.elementPopup.SelectOption(list2[j], j);
			}
		}
		this.elementPopup.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0f, 1f);
	}

	// Token: 0x06009A86 RID: 39558 RVA: 0x003C8488 File Offset: 0x003C6688
	private void OnClickSpawn()
	{
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			worldContainer.SetDiscovered(true);
		}
		SaveGame.Instance.worldGenSpawner.SpawnEverything();
		this.spawnButton.GetComponent<KButton>().isInteractable = false;
	}

	// Token: 0x06009A87 RID: 39559 RVA: 0x00108FF2 File Offset: 0x001071F2
	private void OnClickPaint()
	{
		this.OnChangeMass();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		this.OnChangeFOWReveal();
		DebugTool.Instance.Activate(DebugTool.Type.ReplaceSubstance);
	}

	// Token: 0x06009A88 RID: 39560 RVA: 0x00109017 File Offset: 0x00107217
	private void OnClickStore()
	{
		this.OnChangeMass();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		this.OnChangeFOWReveal();
		DebugTool.Instance.Activate(DebugTool.Type.StoreSubstance);
	}

	// Token: 0x06009A89 RID: 39561 RVA: 0x0010903C File Offset: 0x0010723C
	private void OnClickSample()
	{
		this.OnChangeMass();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		this.OnChangeFOWReveal();
		DebugTool.Instance.Activate(DebugTool.Type.Sample);
	}

	// Token: 0x06009A8A RID: 39562 RVA: 0x00109061 File Offset: 0x00107261
	private void OnClickFill()
	{
		this.OnChangeMass();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		DebugTool.Instance.Activate(DebugTool.Type.FillReplaceSubstance);
	}

	// Token: 0x06009A8B RID: 39563 RVA: 0x00109080 File Offset: 0x00107280
	private void OnSelectElement(string str, int index)
	{
		this.element = (SimHashes)Enum.Parse(typeof(SimHashes), this.options_list[index]);
		this.elementButton.GetComponentInChildren<LocText>().text = str;
	}

	// Token: 0x06009A8C RID: 39564 RVA: 0x001090B9 File Offset: 0x001072B9
	private void OnSelectElement(SimHashes element)
	{
		this.element = element;
		this.elementButton.GetComponentInChildren<LocText>().text = ElementLoader.FindElementByHash(element).name;
	}

	// Token: 0x06009A8D RID: 39565 RVA: 0x003C8500 File Offset: 0x003C6700
	private void OnSelectDisease(string str, int index)
	{
		this.diseaseIdx = byte.MaxValue;
		for (int i = 0; i < Db.Get().Diseases.Count; i++)
		{
			if (Db.Get().Diseases[i].Name == str)
			{
				this.diseaseIdx = (byte)i;
			}
		}
		this.SelectDiseaseOption((int)this.diseaseIdx);
	}

	// Token: 0x06009A8E RID: 39566 RVA: 0x003C8564 File Offset: 0x003C6764
	private void SelectDiseaseOption(int diseaseIdx)
	{
		if (diseaseIdx == 255)
		{
			this.diseaseButton.GetComponentInChildren<LocText>().text = "None";
			return;
		}
		string name = Db.Get().Diseases[diseaseIdx].Name;
		this.diseaseButton.GetComponentInChildren<LocText>().text = name;
	}

	// Token: 0x06009A8F RID: 39567 RVA: 0x003C85B8 File Offset: 0x003C67B8
	private void OnChangeFOWReveal()
	{
		if (this.paintPreventFOWReveal.isOn)
		{
			this.paintAllowFOWReveal.isOn = false;
		}
		if (this.paintAllowFOWReveal.isOn)
		{
			this.paintPreventFOWReveal.isOn = false;
		}
		this.set_prevent_fow_reveal = this.paintPreventFOWReveal.isOn;
		this.set_allow_fow_reveal = this.paintAllowFOWReveal.isOn;
	}

	// Token: 0x06009A90 RID: 39568 RVA: 0x003C861C File Offset: 0x003C681C
	public void OnChangeMass()
	{
		float num;
		try
		{
			num = Convert.ToSingle(this.massInput.text);
		}
		catch
		{
			num = -1f;
		}
		if (num <= 0f)
		{
			num = 1f;
			this.massInput.text = "1";
		}
		this.mass = num;
	}

	// Token: 0x06009A91 RID: 39569 RVA: 0x003C867C File Offset: 0x003C687C
	public void OnChangeTemperature()
	{
		float num;
		try
		{
			num = Convert.ToSingle(this.temperatureInput.text);
		}
		catch
		{
			num = -1f;
		}
		if (num <= 0f)
		{
			num = 1f;
			this.temperatureInput.text = "1";
		}
		this.temperature = num;
	}

	// Token: 0x06009A92 RID: 39570 RVA: 0x003C86DC File Offset: 0x003C68DC
	public void OnDiseaseCountChange()
	{
		int num;
		int.TryParse(this.diseaseCountInput.text, out num);
		if (num < 0)
		{
			num = 0;
			this.diseaseCountInput.text = "0";
		}
		this.diseaseCount = num;
	}

	// Token: 0x06009A93 RID: 39571 RVA: 0x001090DD File Offset: 0x001072DD
	public void OnElementsFilterEdited(string new_filter)
	{
		this.filter = (string.IsNullOrEmpty(this.filterInput.text) ? null : this.filterInput.text);
		this.FilterElements(this.filter);
	}

	// Token: 0x06009A94 RID: 39572 RVA: 0x003C871C File Offset: 0x003C691C
	public void SampleCell(int cell)
	{
		this.massInput.text = Grid.Mass[cell].ToString();
		this.temperatureInput.text = Grid.Temperature[cell].ToString();
		this.OnSelectElement(ElementLoader.GetElementID(Grid.Element[cell].tag));
		this.OnChangeMass();
		this.OnChangeTemperature();
	}

	// Token: 0x04007894 RID: 30868
	[Header("Current State")]
	public SimHashes element;

	// Token: 0x04007895 RID: 30869
	[NonSerialized]
	public float mass = 1000f;

	// Token: 0x04007896 RID: 30870
	[NonSerialized]
	public float temperature = -1f;

	// Token: 0x04007897 RID: 30871
	[NonSerialized]
	public bool set_prevent_fow_reveal;

	// Token: 0x04007898 RID: 30872
	[NonSerialized]
	public bool set_allow_fow_reveal;

	// Token: 0x04007899 RID: 30873
	[NonSerialized]
	public int diseaseCount;

	// Token: 0x0400789A RID: 30874
	public byte diseaseIdx;

	// Token: 0x0400789B RID: 30875
	[Header("Popup Buttons")]
	[SerializeField]
	private KButton elementButton;

	// Token: 0x0400789C RID: 30876
	[SerializeField]
	private KButton diseaseButton;

	// Token: 0x0400789D RID: 30877
	[Header("Popup Menus")]
	[SerializeField]
	private KPopupMenu elementPopup;

	// Token: 0x0400789E RID: 30878
	[SerializeField]
	private KPopupMenu diseasePopup;

	// Token: 0x0400789F RID: 30879
	[Header("Value Inputs")]
	[SerializeField]
	private KInputTextField massInput;

	// Token: 0x040078A0 RID: 30880
	[SerializeField]
	private KInputTextField temperatureInput;

	// Token: 0x040078A1 RID: 30881
	[SerializeField]
	private KInputTextField diseaseCountInput;

	// Token: 0x040078A2 RID: 30882
	[SerializeField]
	private KInputTextField filterInput;

	// Token: 0x040078A3 RID: 30883
	[Header("Tool Buttons")]
	[SerializeField]
	private KButton paintButton;

	// Token: 0x040078A4 RID: 30884
	[SerializeField]
	private KButton fillButton;

	// Token: 0x040078A5 RID: 30885
	[SerializeField]
	private KButton sampleButton;

	// Token: 0x040078A6 RID: 30886
	[SerializeField]
	private KButton spawnButton;

	// Token: 0x040078A7 RID: 30887
	[SerializeField]
	private KButton storeButton;

	// Token: 0x040078A8 RID: 30888
	[Header("Parameter Toggles")]
	public Toggle paintElement;

	// Token: 0x040078A9 RID: 30889
	public Toggle paintMass;

	// Token: 0x040078AA RID: 30890
	public Toggle paintTemperature;

	// Token: 0x040078AB RID: 30891
	public Toggle paintDisease;

	// Token: 0x040078AC RID: 30892
	public Toggle paintDiseaseCount;

	// Token: 0x040078AD RID: 30893
	public Toggle affectBuildings;

	// Token: 0x040078AE RID: 30894
	public Toggle affectCells;

	// Token: 0x040078AF RID: 30895
	public Toggle paintPreventFOWReveal;

	// Token: 0x040078B0 RID: 30896
	public Toggle paintAllowFOWReveal;

	// Token: 0x040078B1 RID: 30897
	private List<KInputTextField> inputFields = new List<KInputTextField>();

	// Token: 0x040078B2 RID: 30898
	private List<string> options_list = new List<string>();

	// Token: 0x040078B3 RID: 30899
	private string filter;

	// Token: 0x02001CEF RID: 7407
	private struct ElemDisplayInfo
	{
		// Token: 0x040078B4 RID: 30900
		public SimHashes id;

		// Token: 0x040078B5 RID: 30901
		public string displayStr;
	}
}
