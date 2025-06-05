using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FCA RID: 8138
public class FewOptionSideScreen : SideScreenContent
{
	// Token: 0x0600ABFA RID: 44026 RVA: 0x0011436B File Offset: 0x0011256B
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshOptions();
		}
	}

	// Token: 0x0600ABFB RID: 44027 RVA: 0x0041BBAC File Offset: 0x00419DAC
	private void RefreshOptions()
	{
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.rows)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == this.targetFewOptions.GetSelectedOption()) ? 1 : 0);
		}
	}

	// Token: 0x0600ABFC RID: 44028 RVA: 0x0041BC28 File Offset: 0x00419E28
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x0600ABFD RID: 44029 RVA: 0x0041BC6C File Offset: 0x00419E6C
	private void SpawnRows()
	{
		FewOptionSideScreen.IFewOptionSideScreen.Option[] options = this.targetFewOptions.GetOptions();
		for (int i = 0; i < options.Length; i++)
		{
			FewOptionSideScreen.IFewOptionSideScreen.Option option = options[i];
			GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("label").SetText(option.labelText);
			component.GetReference<Image>("icon").sprite = option.iconSpriteColorTuple.first;
			component.GetReference<Image>("icon").color = option.iconSpriteColorTuple.second;
			gameObject.GetComponent<ToolTip>().toolTip = option.tooltipText;
			gameObject.GetComponent<MultiToggle>().onClick = delegate()
			{
				this.targetFewOptions.OnOptionSelected(option);
				this.RefreshOptions();
			};
			this.rows.Add(option.tag, gameObject);
		}
		this.RefreshOptions();
	}

	// Token: 0x0600ABFE RID: 44030 RVA: 0x0011437D File Offset: 0x0011257D
	public override void SetTarget(GameObject target)
	{
		this.ClearRows();
		this.targetFewOptions = target.GetComponent<FewOptionSideScreen.IFewOptionSideScreen>();
		this.SpawnRows();
	}

	// Token: 0x0600ABFF RID: 44031 RVA: 0x00114397 File Offset: 0x00112597
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<FewOptionSideScreen.IFewOptionSideScreen>() != null;
	}

	// Token: 0x0400876A RID: 34666
	public GameObject rowPrefab;

	// Token: 0x0400876B RID: 34667
	public RectTransform rowContainer;

	// Token: 0x0400876C RID: 34668
	public Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();

	// Token: 0x0400876D RID: 34669
	private FewOptionSideScreen.IFewOptionSideScreen targetFewOptions;

	// Token: 0x02001FCB RID: 8139
	public interface IFewOptionSideScreen
	{
		// Token: 0x0600AC01 RID: 44033
		FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions();

		// Token: 0x0600AC02 RID: 44034
		void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option);

		// Token: 0x0600AC03 RID: 44035
		Tag GetSelectedOption();

		// Token: 0x02001FCC RID: 8140
		public struct Option
		{
			// Token: 0x0600AC04 RID: 44036 RVA: 0x001143B5 File Offset: 0x001125B5
			public Option(Tag tag, string labelText, global::Tuple<Sprite, Color> iconSpriteColorTuple, string tooltipText = "")
			{
				this.tag = tag;
				this.labelText = labelText;
				this.iconSpriteColorTuple = iconSpriteColorTuple;
				this.tooltipText = tooltipText;
			}

			// Token: 0x0400876E RID: 34670
			public Tag tag;

			// Token: 0x0400876F RID: 34671
			public string labelText;

			// Token: 0x04008770 RID: 34672
			public string tooltipText;

			// Token: 0x04008771 RID: 34673
			public global::Tuple<Sprite, Color> iconSpriteColorTuple;
		}
	}
}
