using System;
using UnityEngine;

// Token: 0x02001C68 RID: 7272
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/CharacterOverlay")]
public class CharacterOverlay : KMonoBehaviour
{
	// Token: 0x06009732 RID: 38706 RVA: 0x00106F04 File Offset: 0x00105104
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
	}

	// Token: 0x06009733 RID: 38707 RVA: 0x00106F12 File Offset: 0x00105112
	public void Register()
	{
		if (this.registered)
		{
			return;
		}
		this.registered = true;
		NameDisplayScreen.Instance.AddNewEntry(base.gameObject);
	}

	// Token: 0x040075AF RID: 30127
	public bool shouldShowName;

	// Token: 0x040075B0 RID: 30128
	private bool registered;
}
