using System;
using UnityEngine;

// Token: 0x02001E12 RID: 7698
public class PortraitTableColumn : TableColumn
{
	// Token: 0x0600A0C8 RID: 41160 RVA: 0x0010D39A File Offset: 0x0010B59A
	public PortraitTableColumn(Action<IAssignableIdentity, GameObject> on_load_action, Comparison<IAssignableIdentity> sort_comparison, bool double_click_to_target = true) : base(on_load_action, sort_comparison, null, null, null, false, "")
	{
		this.double_click_to_target = double_click_to_target;
	}

	// Token: 0x0600A0C9 RID: 41161 RVA: 0x0010D3C9 File Offset: 0x0010B5C9
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(this.prefab_portrait, parent, true);
		gameObject.GetComponent<CrewPortrait>().targetImage.enabled = true;
		return gameObject;
	}

	// Token: 0x0600A0CA RID: 41162 RVA: 0x0010D3E9 File Offset: 0x0010B5E9
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return Util.KInstantiateUI(this.prefab_portrait, parent, true);
	}

	// Token: 0x0600A0CB RID: 41163 RVA: 0x003E4FC4 File Offset: 0x003E31C4
	public override GameObject GetMinionWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(this.prefab_portrait, parent, true);
		if (this.double_click_to_target)
		{
			gameObject.GetComponent<KButton>().onClick += delegate()
			{
				parent.GetComponent<TableRow>().SelectMinion();
			};
			gameObject.GetComponent<KButton>().onDoubleClick += delegate()
			{
				parent.GetComponent<TableRow>().SelectAndFocusMinion();
			};
		}
		return gameObject;
	}

	// Token: 0x04007E55 RID: 32341
	public GameObject prefab_portrait = Assets.UIPrefabs.TableScreenWidgets.MinionPortrait;

	// Token: 0x04007E56 RID: 32342
	private bool double_click_to_target;
}
