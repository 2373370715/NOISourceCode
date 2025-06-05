using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FE9 RID: 8169
public class LogicBroadcastChannelSideScreen : SideScreenContent
{
	// Token: 0x0600ACA3 RID: 44195 RVA: 0x00114ACC File Offset: 0x00112CCC
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicBroadcastReceiver>() != null;
	}

	// Token: 0x0600ACA4 RID: 44196 RVA: 0x00114ADA File Offset: 0x00112CDA
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.sensor = target.GetComponent<LogicBroadcastReceiver>();
		this.Build();
	}

	// Token: 0x0600ACA5 RID: 44197 RVA: 0x0041E98C File Offset: 0x0041CB8C
	private void ClearRows()
	{
		if (this.emptySpaceRow != null)
		{
			Util.KDestroyGameObject(this.emptySpaceRow);
		}
		foreach (KeyValuePair<LogicBroadcaster, GameObject> keyValuePair in this.broadcasterRows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.broadcasterRows.Clear();
	}

	// Token: 0x0600ACA6 RID: 44198 RVA: 0x0041EA08 File Offset: 0x0041CC08
	private void Build()
	{
		this.headerLabel.SetText(UI.UISIDESCREENS.LOGICBROADCASTCHANNELSIDESCREEN.HEADER);
		this.ClearRows();
		foreach (object obj in Components.LogicBroadcasters)
		{
			LogicBroadcaster logicBroadcaster = (LogicBroadcaster)obj;
			if (!logicBroadcaster.IsNullOrDestroyed())
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
				gameObject.gameObject.name = logicBroadcaster.gameObject.GetProperName();
				global::Debug.Assert(!this.broadcasterRows.ContainsKey(logicBroadcaster), "Adding two of the same broadcaster to LogicBroadcastChannelSideScreen UI: " + logicBroadcaster.gameObject.GetProperName());
				this.broadcasterRows.Add(logicBroadcaster, gameObject);
				gameObject.SetActive(true);
			}
		}
		this.noChannelRow.SetActive(Components.LogicBroadcasters.Count == 0);
		this.Refresh();
	}

	// Token: 0x0600ACA7 RID: 44199 RVA: 0x0041EB04 File Offset: 0x0041CD04
	private void Refresh()
	{
		using (Dictionary<LogicBroadcaster, GameObject>.Enumerator enumerator = this.broadcasterRows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<LogicBroadcaster, GameObject> kvp = enumerator.Current;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(kvp.Key.gameObject.GetProperName());
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("DistanceLabel").SetText(LogicBroadcastReceiver.CheckRange(this.sensor.gameObject, kvp.Key.gameObject) ? UI.UISIDESCREENS.LOGICBROADCASTCHANNELSIDESCREEN.IN_RANGE : UI.UISIDESCREENS.LOGICBROADCASTCHANNELSIDESCREEN.OUT_OF_RANGE);
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite(kvp.Key.gameObject, "ui", false).first;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite(kvp.Key.gameObject, "ui", false).second;
				WorldContainer myWorld = kvp.Key.GetMyWorld();
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("WorldIcon").sprite = (myWorld.IsModuleInterior ? Assets.GetSprite("icon_category_rocketry") : Def.GetUISprite(myWorld.GetComponent<ClusterGridEntity>(), "ui", false).first);
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("WorldIcon").color = (myWorld.IsModuleInterior ? Color.white : Def.GetUISprite(myWorld.GetComponent<ClusterGridEntity>(), "ui", false).second);
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.sensor.SetChannel(kvp.Key);
					this.Refresh();
				};
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState((this.sensor.GetChannel() == kvp.Key) ? 1 : 0);
			}
		}
	}

	// Token: 0x040087E9 RID: 34793
	private LogicBroadcastReceiver sensor;

	// Token: 0x040087EA RID: 34794
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x040087EB RID: 34795
	[SerializeField]
	private GameObject listContainer;

	// Token: 0x040087EC RID: 34796
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x040087ED RID: 34797
	[SerializeField]
	private GameObject noChannelRow;

	// Token: 0x040087EE RID: 34798
	private Dictionary<LogicBroadcaster, GameObject> broadcasterRows = new Dictionary<LogicBroadcaster, GameObject>();

	// Token: 0x040087EF RID: 34799
	private GameObject emptySpaceRow;
}
