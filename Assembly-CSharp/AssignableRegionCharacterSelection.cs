using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001C51 RID: 7249
[AddComponentMenu("KMonoBehaviour/scripts/AssignableRegionCharacterSelection")]
public class AssignableRegionCharacterSelection : KMonoBehaviour
{
	// Token: 0x14000025 RID: 37
	// (add) Token: 0x0600969D RID: 38557 RVA: 0x003AD14C File Offset: 0x003AB34C
	// (remove) Token: 0x0600969E RID: 38558 RVA: 0x003AD184 File Offset: 0x003AB384
	public event Action<MinionIdentity> OnDuplicantSelected;

	// Token: 0x0600969F RID: 38559 RVA: 0x00106813 File Offset: 0x00104A13
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.buttonPool = new UIPool<KButton>(this.buttonPrefab);
		base.gameObject.SetActive(false);
	}

	// Token: 0x060096A0 RID: 38560 RVA: 0x003AD1BC File Offset: 0x003AB3BC
	public void Open()
	{
		base.gameObject.SetActive(true);
		this.buttonPool.ClearAll();
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
		{
			KButton btn = this.buttonPool.GetFreeElement(this.buttonParent, true);
			CrewPortrait componentInChildren = btn.GetComponentInChildren<CrewPortrait>();
			componentInChildren.SetIdentityObject(minionIdentity, true);
			this.portraitList.Add(componentInChildren);
			btn.ClearOnClick();
			btn.onClick += delegate()
			{
				this.SelectDuplicant(btn);
			};
			this.buttonIdentityMap.Add(btn, minionIdentity);
		}
	}

	// Token: 0x060096A1 RID: 38561 RVA: 0x00106838 File Offset: 0x00104A38
	public void Close()
	{
		this.buttonPool.DestroyAllActive();
		this.buttonIdentityMap.Clear();
		this.portraitList.Clear();
		base.gameObject.SetActive(false);
	}

	// Token: 0x060096A2 RID: 38562 RVA: 0x00106867 File Offset: 0x00104A67
	private void SelectDuplicant(KButton btn)
	{
		if (this.OnDuplicantSelected != null)
		{
			this.OnDuplicantSelected(this.buttonIdentityMap[btn]);
		}
		this.Close();
	}

	// Token: 0x04007506 RID: 29958
	[SerializeField]
	private KButton buttonPrefab;

	// Token: 0x04007507 RID: 29959
	[SerializeField]
	private GameObject buttonParent;

	// Token: 0x04007508 RID: 29960
	private UIPool<KButton> buttonPool;

	// Token: 0x04007509 RID: 29961
	private Dictionary<KButton, MinionIdentity> buttonIdentityMap = new Dictionary<KButton, MinionIdentity>();

	// Token: 0x0400750A RID: 29962
	private List<CrewPortrait> portraitList = new List<CrewPortrait>();
}
