using System;
using UnityEngine;

// Token: 0x02001E4A RID: 7754
public class CodexMessageDialog : MessageDialog
{
	// Token: 0x0600A23F RID: 41535 RVA: 0x0010DE54 File Offset: 0x0010C054
	public override bool CanDisplay(Message message)
	{
		return typeof(CodexUnlockedMessage).IsAssignableFrom(message.GetType());
	}

	// Token: 0x0600A240 RID: 41536 RVA: 0x0010DE6B File Offset: 0x0010C06B
	public override void SetMessage(Message base_message)
	{
		this.message = (CodexUnlockedMessage)base_message;
		this.description.text = this.message.GetMessageBody();
	}

	// Token: 0x0600A241 RID: 41537 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void OnClickAction()
	{
	}

	// Token: 0x0600A242 RID: 41538 RVA: 0x0010DE8F File Offset: 0x0010C08F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.message.OnCleanUp();
	}

	// Token: 0x04007F35 RID: 32565
	[SerializeField]
	private LocText description;

	// Token: 0x04007F36 RID: 32566
	private CodexUnlockedMessage message;
}
