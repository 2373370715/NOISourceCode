using System;
using KSerialization;
using STRINGS;

// Token: 0x02001E61 RID: 7777
public class WorldDetectedMessage : Message
{
	// Token: 0x0600A2E5 RID: 41701 RVA: 0x0010DE4C File Offset: 0x0010C04C
	public WorldDetectedMessage()
	{
	}

	// Token: 0x0600A2E6 RID: 41702 RVA: 0x0010E595 File Offset: 0x0010C795
	public WorldDetectedMessage(WorldContainer world)
	{
		this.worldID = world.id;
	}

	// Token: 0x0600A2E7 RID: 41703 RVA: 0x0010DE07 File Offset: 0x0010C007
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x0600A2E8 RID: 41704 RVA: 0x003EE574 File Offset: 0x003EC774
	public override string GetMessageBody()
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(this.worldID);
		return string.Format(MISC.NOTIFICATIONS.WORLDDETECTED.MESSAGEBODY, world.GetProperName());
	}

	// Token: 0x0600A2E9 RID: 41705 RVA: 0x0010E5A9 File Offset: 0x0010C7A9
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.WORLDDETECTED.NAME;
	}

	// Token: 0x0600A2EA RID: 41706 RVA: 0x003EE5A8 File Offset: 0x003EC7A8
	public override string GetTooltip()
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(this.worldID);
		return string.Format(MISC.NOTIFICATIONS.WORLDDETECTED.TOOLTIP, world.GetProperName());
	}

	// Token: 0x0600A2EB RID: 41707 RVA: 0x0010E5B5 File Offset: 0x0010C7B5
	public override bool IsValid()
	{
		return this.worldID != 255;
	}

	// Token: 0x04007F70 RID: 32624
	[Serialize]
	private int worldID;
}
