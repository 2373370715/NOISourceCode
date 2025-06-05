using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001CE2 RID: 7394
public abstract class CustomGameSettingsPanelBase : MonoBehaviour
{
	// Token: 0x06009A35 RID: 39477 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void Init()
	{
	}

	// Token: 0x06009A36 RID: 39478 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void Uninit()
	{
	}

	// Token: 0x06009A37 RID: 39479 RVA: 0x00108C6B File Offset: 0x00106E6B
	private void OnEnable()
	{
		this.isDirty = true;
	}

	// Token: 0x06009A38 RID: 39480 RVA: 0x00108C74 File Offset: 0x00106E74
	private void Update()
	{
		if (this.isDirty)
		{
			this.isDirty = false;
			this.Refresh();
		}
	}

	// Token: 0x06009A39 RID: 39481 RVA: 0x00108C8B File Offset: 0x00106E8B
	protected void AddWidget(CustomGameSettingWidget widget)
	{
		widget.onSettingChanged += this.OnWidgetChanged;
		this.widgets.Add(widget);
	}

	// Token: 0x06009A3A RID: 39482 RVA: 0x00108C6B File Offset: 0x00106E6B
	private void OnWidgetChanged(CustomGameSettingWidget widget)
	{
		this.isDirty = true;
	}

	// Token: 0x06009A3B RID: 39483 RVA: 0x003C66FC File Offset: 0x003C48FC
	public virtual void Refresh()
	{
		foreach (CustomGameSettingWidget customGameSettingWidget in this.widgets)
		{
			customGameSettingWidget.Refresh();
		}
	}

	// Token: 0x04007857 RID: 30807
	protected List<CustomGameSettingWidget> widgets = new List<CustomGameSettingWidget>();

	// Token: 0x04007858 RID: 30808
	private bool isDirty;
}
