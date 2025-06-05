using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FB9 RID: 8121
public class ConditionListSideScreen : SideScreenContent
{
	// Token: 0x0600ABA3 RID: 43939 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool IsValidForTarget(GameObject target)
	{
		return false;
	}

	// Token: 0x0600ABA4 RID: 43940 RVA: 0x00114017 File Offset: 0x00112217
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		if (target != null)
		{
			this.targetConditionSet = target.GetComponent<IProcessConditionSet>();
		}
	}

	// Token: 0x0600ABA5 RID: 43941 RVA: 0x00114035 File Offset: 0x00112235
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.Refresh();
		}
	}

	// Token: 0x0600ABA6 RID: 43942 RVA: 0x0041ABAC File Offset: 0x00418DAC
	private void Refresh()
	{
		bool flag = false;
		List<ProcessCondition> conditionSet = this.targetConditionSet.GetConditionSet(ProcessCondition.ProcessConditionType.All);
		foreach (ProcessCondition key in conditionSet)
		{
			if (!this.rows.ContainsKey(key))
			{
				flag = true;
				break;
			}
		}
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.rows)
		{
			if (!conditionSet.Contains(keyValuePair.Key))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.Rebuild();
		}
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair2 in this.rows)
		{
			ConditionListSideScreen.SetRowState(keyValuePair2.Value, keyValuePair2.Key);
		}
	}

	// Token: 0x0600ABA7 RID: 43943 RVA: 0x0041ACC0 File Offset: 0x00418EC0
	public static void SetRowState(GameObject row, ProcessCondition condition)
	{
		HierarchyReferences component = row.GetComponent<HierarchyReferences>();
		ProcessCondition.Status status = condition.EvaluateCondition();
		component.GetReference<LocText>("Label").text = condition.GetStatusMessage(status);
		switch (status)
		{
		case ProcessCondition.Status.Failure:
			component.GetReference<LocText>("Label").color = ConditionListSideScreen.failedColor;
			component.GetReference<Image>("Box").color = ConditionListSideScreen.failedColor;
			break;
		case ProcessCondition.Status.Warning:
			component.GetReference<LocText>("Label").color = ConditionListSideScreen.warningColor;
			component.GetReference<Image>("Box").color = ConditionListSideScreen.warningColor;
			break;
		case ProcessCondition.Status.Ready:
			component.GetReference<LocText>("Label").color = ConditionListSideScreen.readyColor;
			component.GetReference<Image>("Box").color = ConditionListSideScreen.readyColor;
			break;
		}
		component.GetReference<Image>("Check").gameObject.SetActive(status == ProcessCondition.Status.Ready);
		component.GetReference<Image>("Dash").gameObject.SetActive(false);
		row.GetComponent<ToolTip>().SetSimpleTooltip(condition.GetStatusTooltip(status));
	}

	// Token: 0x0600ABA8 RID: 43944 RVA: 0x00114047 File Offset: 0x00112247
	private void Rebuild()
	{
		this.ClearRows();
		this.BuildRows();
	}

	// Token: 0x0600ABA9 RID: 43945 RVA: 0x0041ADCC File Offset: 0x00418FCC
	private void ClearRows()
	{
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.rows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.rows.Clear();
	}

	// Token: 0x0600ABAA RID: 43946 RVA: 0x0041AE30 File Offset: 0x00419030
	private void BuildRows()
	{
		foreach (ProcessCondition processCondition in this.targetConditionSet.GetConditionSet(ProcessCondition.ProcessConditionType.All))
		{
			if (processCondition.ShowInUI())
			{
				GameObject value = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
				this.rows.Add(processCondition, value);
			}
		}
	}

	// Token: 0x04008734 RID: 34612
	public GameObject rowPrefab;

	// Token: 0x04008735 RID: 34613
	public GameObject rowContainer;

	// Token: 0x04008736 RID: 34614
	[Tooltip("This list is indexed by the ProcessCondition.Status enum")]
	public static Color readyColor = Color.black;

	// Token: 0x04008737 RID: 34615
	public static Color failedColor = Color.red;

	// Token: 0x04008738 RID: 34616
	public static Color warningColor = new Color(1f, 0.3529412f, 0f, 1f);

	// Token: 0x04008739 RID: 34617
	private IProcessConditionSet targetConditionSet;

	// Token: 0x0400873A RID: 34618
	private Dictionary<ProcessCondition, GameObject> rows = new Dictionary<ProcessCondition, GameObject>();
}
