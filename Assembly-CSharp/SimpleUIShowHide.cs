using System;
using UnityEngine;

// Token: 0x020020D6 RID: 8406
[AddComponentMenu("KMonoBehaviour/scripts/SimpleUIShowHide")]
public class SimpleUIShowHide : KMonoBehaviour
{
	// Token: 0x0600B329 RID: 45865 RVA: 0x0044074C File Offset: 0x0043E94C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnClick));
		if (!this.saveStatePreferenceKey.IsNullOrWhiteSpace() && KPlayerPrefs.GetInt(this.saveStatePreferenceKey, 1) != 1 && this.toggle.CurrentState == 0)
		{
			this.OnClick();
		}
	}

	// Token: 0x0600B32A RID: 45866 RVA: 0x004407B8 File Offset: 0x0043E9B8
	private void OnClick()
	{
		this.toggle.NextState();
		this.content.SetActive(this.toggle.CurrentState == 0);
		if (!this.saveStatePreferenceKey.IsNullOrWhiteSpace())
		{
			KPlayerPrefs.SetInt(this.saveStatePreferenceKey, (this.toggle.CurrentState == 0) ? 1 : 0);
		}
	}

	// Token: 0x04008DCF RID: 36303
	[MyCmpReq]
	private MultiToggle toggle;

	// Token: 0x04008DD0 RID: 36304
	[SerializeField]
	public GameObject content;

	// Token: 0x04008DD1 RID: 36305
	[SerializeField]
	private string saveStatePreferenceKey;

	// Token: 0x04008DD2 RID: 36306
	private const int onState = 0;
}
