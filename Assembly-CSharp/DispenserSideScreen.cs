using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FC2 RID: 8130
public class DispenserSideScreen : SideScreenContent
{
	// Token: 0x0600ABDB RID: 43995 RVA: 0x0011428E File Offset: 0x0011248E
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IDispenser>() != null;
	}

	// Token: 0x0600ABDC RID: 43996 RVA: 0x00114299 File Offset: 0x00112499
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetDispenser = target.GetComponent<IDispenser>();
		this.Refresh();
	}

	// Token: 0x0600ABDD RID: 43997 RVA: 0x0041B388 File Offset: 0x00419588
	private void Refresh()
	{
		this.dispenseButton.ClearOnClick();
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.rows)
		{
			UnityEngine.Object.Destroy(keyValuePair.Value);
		}
		this.rows.Clear();
		foreach (Tag tag in this.targetDispenser.DispensedItems())
		{
			GameObject gameObject = Util.KInstantiateUI(this.itemRowPrefab, this.itemRowContainer.gameObject, true);
			this.rows.Add(tag, gameObject);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<Image>("Icon").sprite = Def.GetUISprite(tag, "ui", false).first;
			component.GetReference<LocText>("Label").text = Assets.GetPrefab(tag).GetProperName();
			gameObject.GetComponent<MultiToggle>().ChangeState((tag == this.targetDispenser.SelectedItem()) ? 0 : 1);
		}
		if (this.targetDispenser.HasOpenChore())
		{
			this.dispenseButton.onClick += delegate()
			{
				this.targetDispenser.OnCancelDispense();
				this.Refresh();
			};
			this.dispenseButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.DISPENSERSIDESCREEN.BUTTON_CANCEL;
		}
		else
		{
			this.dispenseButton.onClick += delegate()
			{
				this.targetDispenser.OnOrderDispense();
				this.Refresh();
			};
			this.dispenseButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.DISPENSERSIDESCREEN.BUTTON_DISPENSE;
		}
		this.targetDispenser.OnStopWorkEvent -= this.Refresh;
		this.targetDispenser.OnStopWorkEvent += this.Refresh;
	}

	// Token: 0x0600ABDE RID: 43998 RVA: 0x001142B4 File Offset: 0x001124B4
	private void SelectTag(Tag tag)
	{
		this.targetDispenser.SelectItem(tag);
		this.Refresh();
	}

	// Token: 0x04008756 RID: 34646
	[SerializeField]
	private KButton dispenseButton;

	// Token: 0x04008757 RID: 34647
	[SerializeField]
	private RectTransform itemRowContainer;

	// Token: 0x04008758 RID: 34648
	[SerializeField]
	private GameObject itemRowPrefab;

	// Token: 0x04008759 RID: 34649
	private IDispenser targetDispenser;

	// Token: 0x0400875A RID: 34650
	private Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();
}
