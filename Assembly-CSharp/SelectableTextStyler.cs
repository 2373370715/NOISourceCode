using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001F79 RID: 8057
public class SelectableTextStyler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x0600AA29 RID: 43561 RVA: 0x00112EA0 File Offset: 0x001110A0
	private void Start()
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
	}

	// Token: 0x0600AA2A RID: 43562 RVA: 0x00112EAF File Offset: 0x001110AF
	private void SetState(SelectableTextStyler.State state, SelectableTextStyler.HoverState hover_state)
	{
		if (state == SelectableTextStyler.State.Normal)
		{
			if (hover_state != SelectableTextStyler.HoverState.Normal)
			{
				if (hover_state == SelectableTextStyler.HoverState.Hovered)
				{
					this.target.textStyleSetting = this.normalHovered;
				}
			}
			else
			{
				this.target.textStyleSetting = this.normalNormal;
			}
		}
		this.target.ApplySettings();
	}

	// Token: 0x0600AA2B RID: 43563 RVA: 0x00112EEC File Offset: 0x001110EC
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Hovered);
	}

	// Token: 0x0600AA2C RID: 43564 RVA: 0x00112EA0 File Offset: 0x001110A0
	public void OnPointerExit(PointerEventData eventData)
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
	}

	// Token: 0x0600AA2D RID: 43565 RVA: 0x00112EA0 File Offset: 0x001110A0
	public void OnPointerClick(PointerEventData eventData)
	{
		this.SetState(this.state, SelectableTextStyler.HoverState.Normal);
	}

	// Token: 0x040085EF RID: 34287
	[SerializeField]
	private LocText target;

	// Token: 0x040085F0 RID: 34288
	[SerializeField]
	private SelectableTextStyler.State state;

	// Token: 0x040085F1 RID: 34289
	[SerializeField]
	private TextStyleSetting normalNormal;

	// Token: 0x040085F2 RID: 34290
	[SerializeField]
	private TextStyleSetting normalHovered;

	// Token: 0x02001F7A RID: 8058
	public enum State
	{
		// Token: 0x040085F4 RID: 34292
		Normal
	}

	// Token: 0x02001F7B RID: 8059
	public enum HoverState
	{
		// Token: 0x040085F6 RID: 34294
		Normal,
		// Token: 0x040085F7 RID: 34295
		Hovered
	}
}
