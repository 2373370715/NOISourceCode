using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001E4D RID: 7757
public class DiscoveredSpaceMessage : Message
{
	// Token: 0x0600A253 RID: 41555 RVA: 0x0010DE4C File Offset: 0x0010C04C
	public DiscoveredSpaceMessage()
	{
	}

	// Token: 0x0600A254 RID: 41556 RVA: 0x0010DF5E File Offset: 0x0010C15E
	public DiscoveredSpaceMessage(Vector3 pos)
	{
		this.cameraFocusPos = pos;
		this.cameraFocusPos.z = -40f;
	}

	// Token: 0x0600A255 RID: 41557 RVA: 0x0010DF7D File Offset: 0x0010C17D
	public override string GetSound()
	{
		return "Discover_Space";
	}

	// Token: 0x0600A256 RID: 41558 RVA: 0x0010DF84 File Offset: 0x0010C184
	public override string GetMessageBody()
	{
		return MISC.NOTIFICATIONS.DISCOVERED_SPACE.TOOLTIP;
	}

	// Token: 0x0600A257 RID: 41559 RVA: 0x0010DF90 File Offset: 0x0010C190
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.DISCOVERED_SPACE.NAME;
	}

	// Token: 0x0600A258 RID: 41560 RVA: 0x000AA765 File Offset: 0x000A8965
	public override string GetTooltip()
	{
		return null;
	}

	// Token: 0x0600A259 RID: 41561 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x0600A25A RID: 41562 RVA: 0x0010DF9C File Offset: 0x0010C19C
	public override void OnClick()
	{
		this.OnDiscoveredSpaceClicked();
	}

	// Token: 0x0600A25B RID: 41563 RVA: 0x0010DFA4 File Offset: 0x0010C1A4
	private void OnDiscoveredSpaceClicked()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound(this.GetSound(), false));
		MusicManager.instance.PlaySong("Stinger_Surface", false);
		CameraController.Instance.SetTargetPos(this.cameraFocusPos, 8f, true);
	}

	// Token: 0x04007F3A RID: 32570
	[Serialize]
	private Vector3 cameraFocusPos;

	// Token: 0x04007F3B RID: 32571
	private const string MUSIC_STINGER = "Stinger_Surface";
}
