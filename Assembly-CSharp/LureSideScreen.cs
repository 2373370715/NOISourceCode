using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FEB RID: 8171
public class LureSideScreen : SideScreenContent
{
	// Token: 0x0600ACAB RID: 44203 RVA: 0x00114B30 File Offset: 0x00112D30
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<CreatureLure>() != null;
	}

	// Token: 0x0600ACAC RID: 44204 RVA: 0x0041ED88 File Offset: 0x0041CF88
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.target_lure = target.GetComponent<CreatureLure>();
		using (List<Tag>.Enumerator enumerator = this.target_lure.baitTypes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Tag bait = enumerator.Current;
				Tag bait3 = bait;
				if (!this.toggles_by_tag.ContainsKey(bait))
				{
					GameObject gameObject = Util.KInstantiateUI(this.prefab_toggle, this.toggle_container, true);
					Image reference = gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("FGImage");
					gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").text = ElementLoader.GetElement(bait).name;
					reference.sprite = Def.GetUISpriteFromMultiObjectAnim(ElementLoader.GetElement(bait).substance.anim, "ui", false, "");
					MultiToggle component = gameObject.GetComponent<MultiToggle>();
					this.toggles_by_tag.Add(bait3, component);
				}
				this.toggles_by_tag[bait].onClick = delegate()
				{
					Tag bait2 = bait;
					this.SelectToggle(bait2);
				};
			}
		}
		this.RefreshToggles();
	}

	// Token: 0x0600ACAD RID: 44205 RVA: 0x00114B3E File Offset: 0x00112D3E
	public void SelectToggle(Tag tag)
	{
		if (this.target_lure.activeBaitSetting != tag)
		{
			this.target_lure.ChangeBaitSetting(tag);
		}
		else
		{
			this.target_lure.ChangeBaitSetting(Tag.Invalid);
		}
		this.RefreshToggles();
	}

	// Token: 0x0600ACAE RID: 44206 RVA: 0x0041EED4 File Offset: 0x0041D0D4
	private void RefreshToggles()
	{
		foreach (KeyValuePair<Tag, MultiToggle> keyValuePair in this.toggles_by_tag)
		{
			if (this.target_lure.activeBaitSetting == keyValuePair.Key)
			{
				keyValuePair.Value.ChangeState(2);
			}
			else
			{
				keyValuePair.Value.ChangeState(1);
			}
			keyValuePair.Value.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.UISIDESCREENS.LURE.ATTRACTS, ElementLoader.GetElement(keyValuePair.Key).name, this.baitAttractionStrings[keyValuePair.Key]));
		}
	}

	// Token: 0x040087F2 RID: 34802
	protected CreatureLure target_lure;

	// Token: 0x040087F3 RID: 34803
	public GameObject prefab_toggle;

	// Token: 0x040087F4 RID: 34804
	public GameObject toggle_container;

	// Token: 0x040087F5 RID: 34805
	public Dictionary<Tag, MultiToggle> toggles_by_tag = new Dictionary<Tag, MultiToggle>();

	// Token: 0x040087F6 RID: 34806
	private Dictionary<Tag, string> baitAttractionStrings = new Dictionary<Tag, string>
	{
		{
			GameTags.SlimeMold,
			CREATURES.SPECIES.PUFT.NAME
		},
		{
			GameTags.Phosphorite,
			CREATURES.SPECIES.LIGHTBUG.NAME
		}
	};
}
