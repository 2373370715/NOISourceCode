using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F9E RID: 8094
public class ButtonMenuSideScreen : SideScreenContent
{
	// Token: 0x0600AB0D RID: 43789 RVA: 0x00417AB4 File Offset: 0x00415CB4
	public override bool IsValidForTarget(GameObject target)
	{
		ISidescreenButtonControl sidescreenButtonControl = target.GetComponent<ISidescreenButtonControl>();
		if (sidescreenButtonControl == null)
		{
			sidescreenButtonControl = target.GetSMI<ISidescreenButtonControl>();
		}
		return sidescreenButtonControl != null && sidescreenButtonControl.SidescreenEnabled();
	}

	// Token: 0x0600AB0E RID: 43790 RVA: 0x00113978 File Offset: 0x00111B78
	public override int GetSideScreenSortOrder()
	{
		if (this.targets == null)
		{
			return 20;
		}
		return this.targets[0].ButtonSideScreenSortOrder();
	}

	// Token: 0x0600AB0F RID: 43791 RVA: 0x00113996 File Offset: 0x00111B96
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.targets = new_target.GetAllSMI<ISidescreenButtonControl>();
		this.targets.AddRange(new_target.GetComponents<ISidescreenButtonControl>());
		this.Refresh();
	}

	// Token: 0x0600AB10 RID: 43792 RVA: 0x001139CF File Offset: 0x00111BCF
	public GameObject GetHorizontalGroup(int id)
	{
		if (!this.horizontalGroups.ContainsKey(id))
		{
			this.horizontalGroups.Add(id, Util.KInstantiateUI(this.horizontalGroupPrefab, this.buttonContainer.gameObject, true));
		}
		return this.horizontalGroups[id];
	}

	// Token: 0x0600AB11 RID: 43793 RVA: 0x00417AE0 File Offset: 0x00415CE0
	public void CopyLayoutSettings(LayoutElement to, LayoutElement from)
	{
		to.ignoreLayout = from.ignoreLayout;
		to.minWidth = from.minWidth;
		to.minHeight = from.minHeight;
		to.preferredWidth = from.preferredWidth;
		to.preferredHeight = from.preferredHeight;
		to.flexibleWidth = from.flexibleWidth;
		to.flexibleHeight = from.flexibleHeight;
		to.layoutPriority = from.layoutPriority;
	}

	// Token: 0x0600AB12 RID: 43794 RVA: 0x00417B50 File Offset: 0x00415D50
	private void Refresh()
	{
		while (this.liveButtons.Count < this.targets.Count)
		{
			this.liveButtons.Add(Util.KInstantiateUI(this.buttonPrefab.gameObject, this.buttonContainer.gameObject, true));
		}
		foreach (int key in this.horizontalGroups.Keys)
		{
			this.horizontalGroups[key].SetActive(false);
		}
		for (int i = 0; i < this.liveButtons.Count; i++)
		{
			if (i >= this.targets.Count)
			{
				this.liveButtons[i].SetActive(false);
			}
			else
			{
				if (!this.liveButtons[i].activeSelf)
				{
					this.liveButtons[i].SetActive(true);
				}
				int num = this.targets[i].HorizontalGroupID();
				LayoutElement component = this.liveButtons[i].GetComponent<LayoutElement>();
				KButton componentInChildren = this.liveButtons[i].GetComponentInChildren<KButton>();
				ToolTip componentInChildren2 = this.liveButtons[i].GetComponentInChildren<ToolTip>();
				LocText componentInChildren3 = this.liveButtons[i].GetComponentInChildren<LocText>();
				if (num >= 0)
				{
					GameObject horizontalGroup = this.GetHorizontalGroup(num);
					horizontalGroup.SetActive(true);
					this.liveButtons[i].transform.SetParent(horizontalGroup.transform, false);
					this.CopyLayoutSettings(component, this.horizontalButtonPrefab);
				}
				else
				{
					this.liveButtons[i].transform.SetParent(this.buttonContainer, false);
					this.CopyLayoutSettings(component, this.buttonPrefab);
				}
				componentInChildren.isInteractable = this.targets[i].SidescreenButtonInteractable();
				componentInChildren.ClearOnClick();
				componentInChildren.onClick += this.targets[i].OnSidescreenButtonPressed;
				componentInChildren.onClick += this.Refresh;
				componentInChildren3.SetText(this.targets[i].SidescreenButtonText);
				componentInChildren2.SetSimpleTooltip(this.targets[i].SidescreenButtonTooltip);
			}
		}
	}

	// Token: 0x040086AA RID: 34474
	public const int DefaultButtonMenuSideScreenSortOrder = 20;

	// Token: 0x040086AB RID: 34475
	public LayoutElement buttonPrefab;

	// Token: 0x040086AC RID: 34476
	public LayoutElement horizontalButtonPrefab;

	// Token: 0x040086AD RID: 34477
	public GameObject horizontalGroupPrefab;

	// Token: 0x040086AE RID: 34478
	public RectTransform buttonContainer;

	// Token: 0x040086AF RID: 34479
	private List<GameObject> liveButtons = new List<GameObject>();

	// Token: 0x040086B0 RID: 34480
	private Dictionary<int, GameObject> horizontalGroups = new Dictionary<int, GameObject>();

	// Token: 0x040086B1 RID: 34481
	private List<ISidescreenButtonControl> targets;
}
