using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FA9 RID: 8105
public class ClusterLocationFilterSideScreen : SideScreenContent
{
	// Token: 0x0600AB5B RID: 43867 RVA: 0x00113D1E File Offset: 0x00111F1E
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicClusterLocationSensor>() != null;
	}

	// Token: 0x0600AB5C RID: 43868 RVA: 0x00113D2C File Offset: 0x00111F2C
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.sensor = target.GetComponent<LogicClusterLocationSensor>();
		this.Build();
	}

	// Token: 0x0600AB5D RID: 43869 RVA: 0x00418BCC File Offset: 0x00416DCC
	private void ClearRows()
	{
		if (this.emptySpaceRow != null)
		{
			Util.KDestroyGameObject(this.emptySpaceRow);
		}
		foreach (KeyValuePair<AxialI, GameObject> keyValuePair in this.worldRows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.worldRows.Clear();
	}

	// Token: 0x0600AB5E RID: 43870 RVA: 0x00418C48 File Offset: 0x00416E48
	private void Build()
	{
		this.headerLabel.SetText(UI.UISIDESCREENS.CLUSTERLOCATIONFILTERSIDESCREEN.HEADER);
		this.ClearRows();
		this.emptySpaceRow = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
		this.emptySpaceRow.SetActive(true);
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (!worldContainer.IsModuleInterior)
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
				gameObject.gameObject.name = worldContainer.GetProperName();
				AxialI myWorldLocation = worldContainer.GetMyWorldLocation();
				global::Debug.Assert(!this.worldRows.ContainsKey(myWorldLocation), "Adding two worlds/POI with the same cluster location to ClusterLocationFilterSideScreen UI: " + worldContainer.GetProperName());
				this.worldRows.Add(myWorldLocation, gameObject);
			}
		}
		this.Refresh();
	}

	// Token: 0x0600AB5F RID: 43871 RVA: 0x00418D44 File Offset: 0x00416F44
	private void Refresh()
	{
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(UI.UISIDESCREENS.CLUSTERLOCATIONFILTERSIDESCREEN.EMPTY_SPACE_ROW);
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite("hex_soft", "ui", false).first;
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Color.black;
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
		{
			this.sensor.SetSpaceEnabled(!this.sensor.ActiveInSpace);
			this.Refresh();
		};
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.sensor.ActiveInSpace ? 1 : 0);
		using (Dictionary<AxialI, GameObject>.Enumerator enumerator = this.worldRows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<AxialI, GameObject> kvp = enumerator.Current;
				ClusterGridEntity clusterGridEntity = ClusterGrid.Instance.cellContents[kvp.Key][0];
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(clusterGridEntity.GetProperName());
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite(clusterGridEntity, "ui", false).first;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite(clusterGridEntity, "ui", false).second;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.sensor.SetLocationEnabled(kvp.Key, !this.sensor.CheckLocationSelected(kvp.Key));
					this.Refresh();
				};
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.sensor.CheckLocationSelected(kvp.Key) ? 1 : 0);
				kvp.Value.SetActive(ClusterGrid.Instance.GetCellRevealLevel(kvp.Key) == ClusterRevealLevel.Visible);
			}
		}
	}

	// Token: 0x040086DF RID: 34527
	private LogicClusterLocationSensor sensor;

	// Token: 0x040086E0 RID: 34528
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x040086E1 RID: 34529
	[SerializeField]
	private GameObject listContainer;

	// Token: 0x040086E2 RID: 34530
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x040086E3 RID: 34531
	private Dictionary<AxialI, GameObject> worldRows = new Dictionary<AxialI, GameObject>();

	// Token: 0x040086E4 RID: 34532
	private GameObject emptySpaceRow;
}
