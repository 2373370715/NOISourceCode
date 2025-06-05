using System;
using UnityEngine;

// Token: 0x02001E5E RID: 7774
public class TargetMessageDialog : MessageDialog
{
	// Token: 0x0600A2D5 RID: 41685 RVA: 0x0010E501 File Offset: 0x0010C701
	public override bool CanDisplay(Message message)
	{
		return typeof(TargetMessage).IsAssignableFrom(message.GetType());
	}

	// Token: 0x0600A2D6 RID: 41686 RVA: 0x0010E518 File Offset: 0x0010C718
	public override void SetMessage(Message base_message)
	{
		this.message = (TargetMessage)base_message;
		this.description.text = this.message.GetMessageBody();
	}

	// Token: 0x0600A2D7 RID: 41687 RVA: 0x003EE40C File Offset: 0x003EC60C
	public override void OnClickAction()
	{
		MessageTarget target = this.message.GetTarget();
		SelectTool.Instance.SelectAndFocus(target.GetPosition(), target.GetSelectable());
	}

	// Token: 0x0600A2D8 RID: 41688 RVA: 0x0010E53C File Offset: 0x0010C73C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.message.OnCleanUp();
	}

	// Token: 0x04007F63 RID: 32611
	[SerializeField]
	private LocText description;

	// Token: 0x04007F64 RID: 32612
	private TargetMessage message;
}
