using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001BAB RID: 7083
public class UserMenuScreen : KIconButtonMenu
{
	// Token: 0x060094DB RID: 38107 RVA: 0x003A2460 File Offset: 0x003A0660
	protected override void OnPrefabInit()
	{
		this.keepMenuOpen = true;
		base.OnPrefabInit();
		this.priorityScreen = Util.KInstantiateUI<PriorityScreen>(this.priorityScreenPrefab.gameObject, this.priorityScreenParent, false);
		this.priorityScreen.InstantiateButtons(new Action<PrioritySetting>(this.OnPriorityClicked), true);
		this.buttonParent.transform.SetAsLastSibling();
	}

	// Token: 0x060094DC RID: 38108 RVA: 0x00105AF4 File Offset: 0x00103CF4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe(1980521255, new Action<object>(this.OnUIRefresh));
		KInputManager.InputChange.AddListener(new UnityAction(base.RefreshButtonTooltip));
	}

	// Token: 0x060094DD RID: 38109 RVA: 0x00105B2E File Offset: 0x00103D2E
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(base.RefreshButtonTooltip));
		base.OnForcedCleanUp();
	}

	// Token: 0x060094DE RID: 38110 RVA: 0x00105B4C File Offset: 0x00103D4C
	public void SetSelected(GameObject go)
	{
		this.ClearPrioritizable();
		this.selected = go;
		this.RefreshPrioritizable();
	}

	// Token: 0x060094DF RID: 38111 RVA: 0x003A24C0 File Offset: 0x003A06C0
	private void ClearPrioritizable()
	{
		if (this.selected != null)
		{
			Prioritizable component = this.selected.GetComponent<Prioritizable>();
			if (component != null)
			{
				Prioritizable prioritizable = component;
				prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Remove(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnPriorityChanged));
			}
		}
	}

	// Token: 0x060094E0 RID: 38112 RVA: 0x003A2514 File Offset: 0x003A0714
	private void RefreshPrioritizable()
	{
		if (this.selected != null)
		{
			Prioritizable component = this.selected.GetComponent<Prioritizable>();
			if (component != null && component.IsPrioritizable())
			{
				Prioritizable prioritizable = component;
				prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnPriorityChanged));
				this.priorityScreen.gameObject.SetActive(true);
				this.priorityScreen.SetScreenPriority(component.GetMasterPriority(), false);
				return;
			}
			this.priorityScreen.gameObject.SetActive(false);
		}
	}

	// Token: 0x060094E1 RID: 38113 RVA: 0x003A25A4 File Offset: 0x003A07A4
	public void Refresh(GameObject go)
	{
		if (go != this.selected)
		{
			return;
		}
		this.buttonInfos.Clear();
		this.slidersInfos.Clear();
		Game.Instance.userMenu.AppendToScreen(go, this);
		base.SetButtons(this.buttonInfos);
		base.RefreshButtons();
		this.RefreshSliders();
		this.ClearPrioritizable();
		this.RefreshPrioritizable();
		if ((this.sliders == null || this.sliders.Count == 0) && (this.buttonInfos == null || this.buttonInfos.Count == 0) && !this.priorityScreen.gameObject.activeSelf)
		{
			base.transform.parent.gameObject.SetActive(false);
			return;
		}
		base.transform.parent.gameObject.SetActive(true);
	}

	// Token: 0x060094E2 RID: 38114 RVA: 0x00105B61 File Offset: 0x00103D61
	public void AddSliders(IList<UserMenu.SliderInfo> sliders)
	{
		this.slidersInfos.AddRange(sliders);
	}

	// Token: 0x060094E3 RID: 38115 RVA: 0x00105B6F File Offset: 0x00103D6F
	public void AddButtons(IList<KIconButtonMenu.ButtonInfo> buttons)
	{
		this.buttonInfos.AddRange(buttons);
	}

	// Token: 0x060094E4 RID: 38116 RVA: 0x00105B7D File Offset: 0x00103D7D
	private void OnUIRefresh(object data)
	{
		this.Refresh(data as GameObject);
	}

	// Token: 0x060094E5 RID: 38117 RVA: 0x003A2674 File Offset: 0x003A0874
	public void RefreshSliders()
	{
		if (this.sliders != null)
		{
			for (int i = 0; i < this.sliders.Count; i++)
			{
				UnityEngine.Object.Destroy(this.sliders[i].gameObject);
			}
			this.sliders = null;
		}
		if (this.slidersInfos == null || this.slidersInfos.Count == 0)
		{
			return;
		}
		this.sliders = new List<MinMaxSlider>();
		for (int j = 0; j < this.slidersInfos.Count; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.sliderPrefab.gameObject, Vector3.zero, Quaternion.identity);
			this.slidersInfos[j].sliderGO = gameObject;
			MinMaxSlider component = gameObject.GetComponent<MinMaxSlider>();
			this.sliders.Add(component);
			Transform parent = (this.sliderParent != null) ? this.sliderParent.transform : base.transform;
			gameObject.transform.SetParent(parent, false);
			gameObject.SetActive(true);
			gameObject.name = "Slider";
			if (component.toolTip)
			{
				component.toolTip.toolTip = this.slidersInfos[j].toolTip;
			}
			component.lockType = this.slidersInfos[j].lockType;
			component.interactable = this.slidersInfos[j].interactable;
			component.minLimit = this.slidersInfos[j].minLimit;
			component.maxLimit = this.slidersInfos[j].maxLimit;
			component.currentMinValue = this.slidersInfos[j].currentMinValue;
			component.currentMaxValue = this.slidersInfos[j].currentMaxValue;
			component.onMinChange = this.slidersInfos[j].onMinChange;
			component.onMaxChange = this.slidersInfos[j].onMaxChange;
			component.direction = this.slidersInfos[j].direction;
			component.SetMode(this.slidersInfos[j].mode);
			component.SetMinMaxValue(this.slidersInfos[j].currentMinValue, this.slidersInfos[j].currentMaxValue, this.slidersInfos[j].minLimit, this.slidersInfos[j].maxLimit);
		}
	}

	// Token: 0x060094E6 RID: 38118 RVA: 0x003A28D8 File Offset: 0x003A0AD8
	private void OnPriorityClicked(PrioritySetting priority)
	{
		if (this.selected != null)
		{
			Prioritizable component = this.selected.GetComponent<Prioritizable>();
			if (component != null)
			{
				component.SetMasterPriority(priority);
			}
		}
	}

	// Token: 0x060094E7 RID: 38119 RVA: 0x00105B8B File Offset: 0x00103D8B
	private void OnPriorityChanged(PrioritySetting priority)
	{
		this.priorityScreen.SetScreenPriority(priority, false);
	}

	// Token: 0x0400710B RID: 28939
	private GameObject selected;

	// Token: 0x0400710C RID: 28940
	public MinMaxSlider sliderPrefab;

	// Token: 0x0400710D RID: 28941
	public GameObject sliderParent;

	// Token: 0x0400710E RID: 28942
	public PriorityScreen priorityScreenPrefab;

	// Token: 0x0400710F RID: 28943
	public GameObject priorityScreenParent;

	// Token: 0x04007110 RID: 28944
	private List<MinMaxSlider> sliders = new List<MinMaxSlider>();

	// Token: 0x04007111 RID: 28945
	private List<UserMenu.SliderInfo> slidersInfos = new List<UserMenu.SliderInfo>();

	// Token: 0x04007112 RID: 28946
	private List<KIconButtonMenu.ButtonInfo> buttonInfos = new List<KIconButtonMenu.ButtonInfo>();

	// Token: 0x04007113 RID: 28947
	private PriorityScreen priorityScreen;
}
