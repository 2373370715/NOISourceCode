using System;
using UnityEngine;

// Token: 0x02001F08 RID: 7944
public class PlanSubCategoryToggle : KMonoBehaviour
{
	// Token: 0x0600A6FA RID: 42746 RVA: 0x00110BF2 File Offset: 0x0010EDF2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.open = !this.open;
			this.gridContainer.SetActive(this.open);
			this.toggle.ChangeState(this.open ? 0 : 1);
		}));
	}

	// Token: 0x040082E2 RID: 33506
	[SerializeField]
	private MultiToggle toggle;

	// Token: 0x040082E3 RID: 33507
	[SerializeField]
	private GameObject gridContainer;

	// Token: 0x040082E4 RID: 33508
	private bool open = true;
}
