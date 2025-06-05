using System;
using UnityEngine;

// Token: 0x02000B3D RID: 2877
[AddComponentMenu("KMonoBehaviour/scripts/SkillPerkMissingComplainer")]
public class SkillPerkMissingComplainer : KMonoBehaviour
{
	// Token: 0x06003564 RID: 13668 RVA: 0x000C740E File Offset: 0x000C560E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			this.skillUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x06003565 RID: 13669 RVA: 0x000C744C File Offset: 0x000C564C
	protected override void OnCleanUp()
	{
		if (this.skillUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillUpdateHandle);
		}
		base.OnCleanUp();
	}

	// Token: 0x06003566 RID: 13670 RVA: 0x0021B5C8 File Offset: 0x002197C8
	protected virtual void UpdateStatusItem(object data = null)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			return;
		}
		bool flag = MinionResume.AnyMinionHasPerk(this.requiredSkillPerk, this.GetMyWorldId());
		if (!flag && this.workStatusItemHandle == Guid.Empty)
		{
			this.workStatusItemHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.ColonyLacksRequiredSkillPerk, this.requiredSkillPerk);
			return;
		}
		if (flag && this.workStatusItemHandle != Guid.Empty)
		{
			component.RemoveStatusItem(this.workStatusItemHandle, false);
			this.workStatusItemHandle = Guid.Empty;
		}
	}

	// Token: 0x040024D9 RID: 9433
	public string requiredSkillPerk;

	// Token: 0x040024DA RID: 9434
	private int skillUpdateHandle = -1;

	// Token: 0x040024DB RID: 9435
	private Guid workStatusItemHandle;
}
