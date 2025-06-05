using System;
using UnityEngine;

// Token: 0x02001E2D RID: 7725
public class DividerColumn : TableColumn
{
	// Token: 0x0600A18B RID: 41355 RVA: 0x003E9CB8 File Offset: 0x003E7EB8
	public DividerColumn(Func<bool> revealed = null, string scrollerID = "") : base(delegate(IAssignableIdentity minion, GameObject widget_go)
	{
		if (revealed != null)
		{
			if (revealed())
			{
				if (!widget_go.activeSelf)
				{
					widget_go.SetActive(true);
					return;
				}
			}
			else if (widget_go.activeSelf)
			{
				widget_go.SetActive(false);
				return;
			}
		}
		else
		{
			widget_go.SetActive(true);
		}
	}, null, null, null, revealed, false, scrollerID)
	{
	}

	// Token: 0x0600A18C RID: 41356 RVA: 0x0010D830 File Offset: 0x0010BA30
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
	}

	// Token: 0x0600A18D RID: 41357 RVA: 0x0010D830 File Offset: 0x0010BA30
	public override GameObject GetMinionWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
	}

	// Token: 0x0600A18E RID: 41358 RVA: 0x0010D830 File Offset: 0x0010BA30
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
	}
}
