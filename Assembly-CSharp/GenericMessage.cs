using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001E50 RID: 7760
public class GenericMessage : Message
{
	// Token: 0x0600A26A RID: 41578 RVA: 0x0010E04C File Offset: 0x0010C24C
	public GenericMessage(string _title, string _body, string _tooltip, KMonoBehaviour click_focus = null)
	{
		this.title = _title;
		this.body = _body;
		this.tooltip = _tooltip;
		this.clickFocus.Set(click_focus);
	}

	// Token: 0x0600A26B RID: 41579 RVA: 0x0010E081 File Offset: 0x0010C281
	public GenericMessage()
	{
	}

	// Token: 0x0600A26C RID: 41580 RVA: 0x000AA765 File Offset: 0x000A8965
	public override string GetSound()
	{
		return null;
	}

	// Token: 0x0600A26D RID: 41581 RVA: 0x0010E094 File Offset: 0x0010C294
	public override string GetMessageBody()
	{
		return this.body;
	}

	// Token: 0x0600A26E RID: 41582 RVA: 0x0010E09C File Offset: 0x0010C29C
	public override string GetTooltip()
	{
		return this.tooltip;
	}

	// Token: 0x0600A26F RID: 41583 RVA: 0x0010E0A4 File Offset: 0x0010C2A4
	public override string GetTitle()
	{
		return this.title;
	}

	// Token: 0x0600A270 RID: 41584 RVA: 0x003EDB24 File Offset: 0x003EBD24
	public override void OnClick()
	{
		KMonoBehaviour kmonoBehaviour = this.clickFocus.Get();
		if (kmonoBehaviour == null)
		{
			return;
		}
		Transform transform = kmonoBehaviour.transform;
		if (transform == null)
		{
			return;
		}
		Vector3 position = transform.GetPosition();
		position.z = -40f;
		CameraController.Instance.SetTargetPos(position, 8f, true);
		if (transform.GetComponent<KSelectable>() != null)
		{
			SelectTool.Instance.Select(transform.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x04007F3F RID: 32575
	[Serialize]
	private string title;

	// Token: 0x04007F40 RID: 32576
	[Serialize]
	private string tooltip;

	// Token: 0x04007F41 RID: 32577
	[Serialize]
	private string body;

	// Token: 0x04007F42 RID: 32578
	[Serialize]
	private Ref<KMonoBehaviour> clickFocus = new Ref<KMonoBehaviour>();
}
