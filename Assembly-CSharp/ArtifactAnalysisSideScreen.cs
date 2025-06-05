using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F8C RID: 8076
public class ArtifactAnalysisSideScreen : SideScreenContent
{
	// Token: 0x0600AA99 RID: 43673 RVA: 0x001134A0 File Offset: 0x001116A0
	public override string GetTitle()
	{
		if (this.targetArtifactStation != null)
		{
			return string.Format(base.GetTitle(), this.targetArtifactStation.GetProperName());
		}
		return base.GetTitle();
	}

	// Token: 0x0600AA9A RID: 43674 RVA: 0x001134CD File Offset: 0x001116CD
	public override void ClearTarget()
	{
		this.targetArtifactStation = null;
		base.ClearTarget();
	}

	// Token: 0x0600AA9B RID: 43675 RVA: 0x001134DC File Offset: 0x001116DC
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<ArtifactAnalysisStation.StatesInstance>() != null;
	}

	// Token: 0x0600AA9C RID: 43676 RVA: 0x00415DDC File Offset: 0x00413FDC
	private void RefreshRows()
	{
		if (this.undiscoveredRow == null)
		{
			this.undiscoveredRow = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
			HierarchyReferences component = this.undiscoveredRow.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("label").SetText(UI.UISIDESCREENS.ARTIFACTANALYSISSIDESCREEN.NO_ARTIFACTS_DISCOVERED);
			component.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.ARTIFACTANALYSISSIDESCREEN.NO_ARTIFACTS_DISCOVERED_TOOLTIP);
			component.GetReference<Image>("icon").sprite = Assets.GetSprite("unknown");
			component.GetReference<Image>("icon").color = Color.grey;
		}
		List<string> analyzedArtifactIDs = ArtifactSelector.Instance.GetAnalyzedArtifactIDs();
		this.undiscoveredRow.SetActive(analyzedArtifactIDs.Count == 0);
		foreach (string text in analyzedArtifactIDs)
		{
			if (!this.rows.ContainsKey(text))
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
				this.rows.Add(text, gameObject);
				GameObject artifactPrefab = Assets.GetPrefab(text);
				HierarchyReferences component2 = gameObject.GetComponent<HierarchyReferences>();
				component2.GetReference<LocText>("label").SetText(artifactPrefab.GetProperName());
				component2.GetReference<Image>("icon").sprite = Def.GetUISprite(artifactPrefab, text, false).first;
				component2.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenEvent(artifactPrefab);
				};
			}
		}
	}

	// Token: 0x0600AA9D RID: 43677 RVA: 0x00415F8C File Offset: 0x0041418C
	private void OpenEvent(GameObject artifactPrefab)
	{
		SimpleEvent.StatesInstance statesInstance = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.ArtifactReveal, -1, null).smi as SimpleEvent.StatesInstance;
		statesInstance.artifact = artifactPrefab;
		artifactPrefab.GetComponent<KPrefabID>();
		artifactPrefab.GetComponent<InfoDescription>();
		string text = artifactPrefab.PrefabID().Name.ToUpper();
		text = text.Replace("ARTIFACT_", "");
		string key = "STRINGS.UI.SPACEARTIFACTS." + text + ".ARTIFACT";
		string text2 = string.Format("<b>{0}</b>", artifactPrefab.GetProperName());
		StringEntry stringEntry;
		Strings.TryGet(key, out stringEntry);
		if (stringEntry != null && !stringEntry.String.IsNullOrWhiteSpace())
		{
			text2 = text2 + "\n\n" + stringEntry.String;
		}
		if (text2 != null && !text2.IsNullOrWhiteSpace())
		{
			statesInstance.SetTextParameter("desc", text2);
		}
		statesInstance.ShowEventPopup();
	}

	// Token: 0x0600AA9E RID: 43678 RVA: 0x001134E7 File Offset: 0x001116E7
	public override void SetTarget(GameObject target)
	{
		this.targetArtifactStation = target;
		base.SetTarget(target);
		this.RefreshRows();
	}

	// Token: 0x04008646 RID: 34374
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x04008647 RID: 34375
	private GameObject targetArtifactStation;

	// Token: 0x04008648 RID: 34376
	[SerializeField]
	private GameObject rowContainer;

	// Token: 0x04008649 RID: 34377
	private Dictionary<string, GameObject> rows = new Dictionary<string, GameObject>();

	// Token: 0x0400864A RID: 34378
	private GameObject undiscoveredRow;
}
