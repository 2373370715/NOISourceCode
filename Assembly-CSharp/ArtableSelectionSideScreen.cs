using System;
using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F89 RID: 8073
public class ArtableSelectionSideScreen : SideScreenContent
{
	// Token: 0x0600AA8A RID: 43658 RVA: 0x00415A18 File Offset: 0x00413C18
	public override bool IsValidForTarget(GameObject target)
	{
		Artable component = target.GetComponent<Artable>();
		return !(component == null) && !(component.CurrentStage == "Default");
	}

	// Token: 0x0600AA8B RID: 43659 RVA: 0x00113358 File Offset: 0x00111558
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.applyButton.onClick += delegate()
		{
			this.target.SetUserChosenTargetState(this.selectedStage);
			SelectTool.Instance.Select(null, true);
		};
		this.clearButton.onClick += delegate()
		{
			this.selectedStage = "";
			this.target.SetDefault();
			SelectTool.Instance.Select(null, true);
		};
	}

	// Token: 0x0600AA8C RID: 43660 RVA: 0x00415A4C File Offset: 0x00413C4C
	public override void SetTarget(GameObject target)
	{
		if (this.workCompleteSub != -1)
		{
			target.Unsubscribe(this.workCompleteSub);
			this.workCompleteSub = -1;
		}
		base.SetTarget(target);
		this.target = target.GetComponent<Artable>();
		this.workCompleteSub = target.Subscribe(-2011693419, new Action<object>(this.OnRefreshTarget));
		this.OnRefreshTarget(null);
	}

	// Token: 0x0600AA8D RID: 43661 RVA: 0x0011338E File Offset: 0x0011158E
	public override void ClearTarget()
	{
		this.target.Unsubscribe(-2011693419);
		this.workCompleteSub = -1;
		base.ClearTarget();
	}

	// Token: 0x0600AA8E RID: 43662 RVA: 0x001133AD File Offset: 0x001115AD
	private void OnRefreshTarget(object data = null)
	{
		if (this.target == null)
		{
			return;
		}
		this.GenerateStateButtons();
		this.selectedStage = this.target.CurrentStage;
		this.RefreshButtons();
	}

	// Token: 0x0600AA8F RID: 43663 RVA: 0x00415AAC File Offset: 0x00413CAC
	public void GenerateStateButtons()
	{
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.buttons)
		{
			Util.KDestroyGameObject(keyValuePair.Value.gameObject);
		}
		this.buttons.Clear();
		foreach (ArtableStage artableStage in Db.GetArtableStages().GetPrefabStages(this.target.GetComponent<KPrefabID>().PrefabID()))
		{
			if (!(artableStage.id == "Default"))
			{
				GameObject gameObject = Util.KInstantiateUI(this.stateButtonPrefab, this.buttonContainer.gameObject, true);
				Sprite sprite = artableStage.GetPermitPresentationInfo().sprite;
				MultiToggle component = gameObject.GetComponent<MultiToggle>();
				component.GetComponent<ToolTip>().SetSimpleTooltip(artableStage.Name);
				component.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = sprite;
				this.buttons.Add(artableStage.id, component);
			}
		}
	}

	// Token: 0x0600AA90 RID: 43664 RVA: 0x00415BE4 File Offset: 0x00413DE4
	private void RefreshButtons()
	{
		List<ArtableStage> prefabStages = Db.GetArtableStages().GetPrefabStages(this.target.GetComponent<KPrefabID>().PrefabID());
		ArtableStage artableStage = prefabStages.Find((ArtableStage match) => match.id == this.target.CurrentStage);
		int num = 0;
		using (Dictionary<string, MultiToggle>.Enumerator enumerator = this.buttons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ArtableSelectionSideScreen.<>c__DisplayClass16_0 CS$<>8__locals1 = new ArtableSelectionSideScreen.<>c__DisplayClass16_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.kvp = enumerator.Current;
				ArtableStage stage = prefabStages.Find((ArtableStage match) => match.id == CS$<>8__locals1.kvp.Key);
				if (stage != null && artableStage != null && stage.statusItem.StatusType != artableStage.statusItem.StatusType)
				{
					CS$<>8__locals1.kvp.Value.gameObject.SetActive(false);
				}
				else if (!stage.IsUnlocked())
				{
					CS$<>8__locals1.kvp.Value.gameObject.SetActive(false);
				}
				else
				{
					num++;
					CS$<>8__locals1.kvp.Value.gameObject.SetActive(true);
					CS$<>8__locals1.kvp.Value.ChangeState((this.selectedStage == CS$<>8__locals1.kvp.Key) ? 1 : 0);
					MultiToggle value = CS$<>8__locals1.kvp.Value;
					value.onClick = (System.Action)Delegate.Combine(value.onClick, new System.Action(delegate()
					{
						CS$<>8__locals1.<>4__this.selectedStage = stage.id;
						CS$<>8__locals1.<>4__this.RefreshButtons();
					}));
				}
			}
		}
		this.scrollTransoform.GetComponent<LayoutElement>().preferredHeight = (float)((num > 3) ? 200 : 100);
	}

	// Token: 0x04008638 RID: 34360
	private Artable target;

	// Token: 0x04008639 RID: 34361
	public KButton applyButton;

	// Token: 0x0400863A RID: 34362
	public KButton clearButton;

	// Token: 0x0400863B RID: 34363
	public GameObject stateButtonPrefab;

	// Token: 0x0400863C RID: 34364
	private Dictionary<string, MultiToggle> buttons = new Dictionary<string, MultiToggle>();

	// Token: 0x0400863D RID: 34365
	[SerializeField]
	private RectTransform scrollTransoform;

	// Token: 0x0400863E RID: 34366
	private string selectedStage = "";

	// Token: 0x0400863F RID: 34367
	private const int INVALID_SUBSCRIPTION = -1;

	// Token: 0x04008640 RID: 34368
	private int workCompleteSub = -1;

	// Token: 0x04008641 RID: 34369
	[SerializeField]
	private RectTransform buttonContainer;
}
