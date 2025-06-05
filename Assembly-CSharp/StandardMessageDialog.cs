using System;
using UnityEngine;

// Token: 0x02001E5C RID: 7772
public class StandardMessageDialog : MessageDialog
{
	// Token: 0x0600A2CD RID: 41677 RVA: 0x0010E4A2 File Offset: 0x0010C6A2
	public override bool CanDisplay(Message message)
	{
		return typeof(Message).IsAssignableFrom(message.GetType());
	}

	// Token: 0x0600A2CE RID: 41678 RVA: 0x0010E4B9 File Offset: 0x0010C6B9
	public override void SetMessage(Message base_message)
	{
		this.message = base_message;
		this.description.text = this.message.GetMessageBody();
	}

	// Token: 0x0600A2CF RID: 41679 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void OnClickAction()
	{
	}

	// Token: 0x04007F60 RID: 32608
	[SerializeField]
	private LocText description;

	// Token: 0x04007F61 RID: 32609
	private Message message;
}
