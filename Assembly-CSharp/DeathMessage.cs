using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001E4C RID: 7756
public class DeathMessage : TargetMessage
{
	// Token: 0x0600A24C RID: 41548 RVA: 0x0010DEF3 File Offset: 0x0010C0F3
	public DeathMessage()
	{
	}

	// Token: 0x0600A24D RID: 41549 RVA: 0x0010DF06 File Offset: 0x0010C106
	public DeathMessage(GameObject go, Death death) : base(go.GetComponent<KPrefabID>())
	{
		this.death.Set(death);
	}

	// Token: 0x0600A24E RID: 41550 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	public override string GetSound()
	{
		return "";
	}

	// Token: 0x0600A24F RID: 41551 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool PlayNotificationSound()
	{
		return false;
	}

	// Token: 0x0600A250 RID: 41552 RVA: 0x0010DF2B File Offset: 0x0010C12B
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.DUPLICANTDIED.NAME;
	}

	// Token: 0x0600A251 RID: 41553 RVA: 0x0010DEEB File Offset: 0x0010C0EB
	public override string GetTooltip()
	{
		return this.GetMessageBody();
	}

	// Token: 0x0600A252 RID: 41554 RVA: 0x0010DF37 File Offset: 0x0010C137
	public override string GetMessageBody()
	{
		return this.death.Get().description.Replace("{Target}", base.GetTarget().GetName());
	}

	// Token: 0x04007F39 RID: 32569
	[Serialize]
	private ResourceRef<Death> death = new ResourceRef<Death>();
}
