using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200209A RID: 8346
[AddComponentMenu("KMonoBehaviour/scripts/ToolParameterMenu")]
public class ToolParameterMenu : KMonoBehaviour
{
	// Token: 0x14000038 RID: 56
	// (add) Token: 0x0600B201 RID: 45569 RVA: 0x0043B618 File Offset: 0x00439818
	// (remove) Token: 0x0600B202 RID: 45570 RVA: 0x0043B650 File Offset: 0x00439850
	public event System.Action onParametersChanged;

	// Token: 0x0600B203 RID: 45571 RVA: 0x0011842C File Offset: 0x0011662C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ClearMenu();
	}

	// Token: 0x0600B204 RID: 45572 RVA: 0x0043B688 File Offset: 0x00439888
	public void PopulateMenu(Dictionary<string, ToolParameterMenu.ToggleState> parameters)
	{
		this.ClearMenu();
		this.currentParameters = parameters;
		foreach (KeyValuePair<string, ToolParameterMenu.ToggleState> keyValuePair in parameters)
		{
			GameObject gameObject = Util.KInstantiateUI(this.widgetPrefab, this.widgetContainer, true);
			gameObject.GetComponentInChildren<LocText>().text = Strings.Get("STRINGS.UI.TOOLS.FILTERLAYERS." + keyValuePair.Key + ".NAME");
			ToolTip componentInChildren = gameObject.GetComponentInChildren<ToolTip>();
			if (componentInChildren != null)
			{
				componentInChildren.SetSimpleTooltip(Strings.Get("STRINGS.UI.TOOLS.FILTERLAYERS." + keyValuePair.Key + ".TOOLTIP"));
			}
			this.widgets.Add(keyValuePair.Key, gameObject);
			MultiToggle toggle = gameObject.GetComponentInChildren<MultiToggle>();
			ToolParameterMenu.ToggleState value = keyValuePair.Value;
			if (value == ToolParameterMenu.ToggleState.Disabled)
			{
				toggle.ChangeState(2);
			}
			else if (value == ToolParameterMenu.ToggleState.On)
			{
				toggle.ChangeState(1);
				this.lastEnabledFilter = keyValuePair.Key;
			}
			else
			{
				toggle.ChangeState(0);
			}
			MultiToggle toggle2 = toggle;
			toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
			{
				foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.widgets)
				{
					if (keyValuePair2.Value == toggle.transform.parent.gameObject)
					{
						if (this.currentParameters[keyValuePair2.Key] == ToolParameterMenu.ToggleState.Disabled)
						{
							break;
						}
						this.ChangeToSetting(keyValuePair2.Key);
						this.OnChange();
						break;
					}
				}
			}));
		}
		this.content.SetActive(true);
	}

	// Token: 0x0600B205 RID: 45573 RVA: 0x0043B80C File Offset: 0x00439A0C
	public void ClearMenu()
	{
		this.content.SetActive(false);
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.widgets)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.widgets.Clear();
	}

	// Token: 0x0600B206 RID: 45574 RVA: 0x0043B87C File Offset: 0x00439A7C
	private void ChangeToSetting(string key)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.widgets)
		{
			if (this.currentParameters[keyValuePair.Key] != ToolParameterMenu.ToggleState.Disabled)
			{
				this.currentParameters[keyValuePair.Key] = ToolParameterMenu.ToggleState.Off;
			}
		}
		this.currentParameters[key] = ToolParameterMenu.ToggleState.On;
	}

	// Token: 0x0600B207 RID: 45575 RVA: 0x0043B900 File Offset: 0x00439B00
	private void OnChange()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.widgets)
		{
			switch (this.currentParameters[keyValuePair.Key])
			{
			case ToolParameterMenu.ToggleState.On:
				keyValuePair.Value.GetComponentInChildren<MultiToggle>().ChangeState(1);
				this.lastEnabledFilter = keyValuePair.Key;
				break;
			case ToolParameterMenu.ToggleState.Off:
				keyValuePair.Value.GetComponentInChildren<MultiToggle>().ChangeState(0);
				break;
			case ToolParameterMenu.ToggleState.Disabled:
				keyValuePair.Value.GetComponentInChildren<MultiToggle>().ChangeState(2);
				break;
			}
		}
		if (this.onParametersChanged != null)
		{
			this.onParametersChanged();
		}
	}

	// Token: 0x0600B208 RID: 45576 RVA: 0x0011843A File Offset: 0x0011663A
	public string GetLastEnabledFilter()
	{
		return this.lastEnabledFilter;
	}

	// Token: 0x04008C66 RID: 35942
	public GameObject content;

	// Token: 0x04008C67 RID: 35943
	public GameObject widgetContainer;

	// Token: 0x04008C68 RID: 35944
	public GameObject widgetPrefab;

	// Token: 0x04008C6A RID: 35946
	private Dictionary<string, GameObject> widgets = new Dictionary<string, GameObject>();

	// Token: 0x04008C6B RID: 35947
	private Dictionary<string, ToolParameterMenu.ToggleState> currentParameters;

	// Token: 0x04008C6C RID: 35948
	private string lastEnabledFilter;

	// Token: 0x0200209B RID: 8347
	public class FILTERLAYERS
	{
		// Token: 0x04008C6D RID: 35949
		public static string BUILDINGS = "BUILDINGS";

		// Token: 0x04008C6E RID: 35950
		public static string TILES = "TILES";

		// Token: 0x04008C6F RID: 35951
		public static string WIRES = "WIRES";

		// Token: 0x04008C70 RID: 35952
		public static string LIQUIDCONDUIT = "LIQUIDPIPES";

		// Token: 0x04008C71 RID: 35953
		public static string GASCONDUIT = "GASPIPES";

		// Token: 0x04008C72 RID: 35954
		public static string SOLIDCONDUIT = "SOLIDCONDUITS";

		// Token: 0x04008C73 RID: 35955
		public static string CLEANANDCLEAR = "CLEANANDCLEAR";

		// Token: 0x04008C74 RID: 35956
		public static string DIGPLACER = "DIGPLACER";

		// Token: 0x04008C75 RID: 35957
		public static string LOGIC = "LOGIC";

		// Token: 0x04008C76 RID: 35958
		public static string BACKWALL = "BACKWALL";

		// Token: 0x04008C77 RID: 35959
		public static string CONSTRUCTION = "CONSTRUCTION";

		// Token: 0x04008C78 RID: 35960
		public static string DIG = "DIG";

		// Token: 0x04008C79 RID: 35961
		public static string CLEAN = "CLEAN";

		// Token: 0x04008C7A RID: 35962
		public static string OPERATE = "OPERATE";

		// Token: 0x04008C7B RID: 35963
		public static string METAL = "METAL";

		// Token: 0x04008C7C RID: 35964
		public static string BUILDABLE = "BUILDABLE";

		// Token: 0x04008C7D RID: 35965
		public static string FILTER = "FILTER";

		// Token: 0x04008C7E RID: 35966
		public static string LIQUIFIABLE = "LIQUIFIABLE";

		// Token: 0x04008C7F RID: 35967
		public static string LIQUID = "LIQUID";

		// Token: 0x04008C80 RID: 35968
		public static string CONSUMABLEORE = "CONSUMABLEORE";

		// Token: 0x04008C81 RID: 35969
		public static string ORGANICS = "ORGANICS";

		// Token: 0x04008C82 RID: 35970
		public static string FARMABLE = "FARMABLE";

		// Token: 0x04008C83 RID: 35971
		public static string GAS = "GAS";

		// Token: 0x04008C84 RID: 35972
		public static string MISC = "MISC";

		// Token: 0x04008C85 RID: 35973
		public static string HEATFLOW = "HEATFLOW";

		// Token: 0x04008C86 RID: 35974
		public static string ABSOLUTETEMPERATURE = "ABSOLUTETEMPERATURE";

		// Token: 0x04008C87 RID: 35975
		public static string RELATIVETEMPERATURE = "RELATIVETEMPERATURE";

		// Token: 0x04008C88 RID: 35976
		public static string ADAPTIVETEMPERATURE = "ADAPTIVETEMPERATURE";

		// Token: 0x04008C89 RID: 35977
		public static string STATECHANGE = "STATECHANGE";

		// Token: 0x04008C8A RID: 35978
		public static string ALL = "ALL";
	}

	// Token: 0x0200209C RID: 8348
	public enum ToggleState
	{
		// Token: 0x04008C8C RID: 35980
		On,
		// Token: 0x04008C8D RID: 35981
		Off,
		// Token: 0x04008C8E RID: 35982
		Disabled
	}
}
