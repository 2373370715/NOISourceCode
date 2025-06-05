using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001CBA RID: 7354
public class CodexCollapsibleHeader : CodexWidget<CodexCollapsibleHeader>
{
	// Token: 0x17000A18 RID: 2584
	// (get) Token: 0x0600994A RID: 39242 RVA: 0x00108119 File Offset: 0x00106319
	// (set) Token: 0x0600994B RID: 39243 RVA: 0x00108140 File Offset: 0x00106340
	protected GameObject ContentsGameObject
	{
		get
		{
			if (this.contentsGameObject == null)
			{
				this.contentsGameObject = this.contents.go;
			}
			return this.contentsGameObject;
		}
		set
		{
			this.contentsGameObject = value;
		}
	}

	// Token: 0x0600994C RID: 39244 RVA: 0x00108149 File Offset: 0x00106349
	public CodexCollapsibleHeader(string label, ContentContainer contents)
	{
		this.label = label;
		this.contents = contents;
	}

	// Token: 0x0600994D RID: 39245 RVA: 0x003C2C2C File Offset: 0x003C0E2C
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		LocText reference = component.GetReference<LocText>("Label");
		reference.text = this.label;
		reference.textStyleSetting = textStyles[CodexTextStyle.Subtitle];
		reference.ApplySettings();
		MultiToggle reference2 = component.GetReference<MultiToggle>("ExpandToggle");
		reference2.ChangeState(1);
		reference2.onClick = delegate()
		{
			this.ToggleCategoryOpen(contentGameObject, !this.ContentsGameObject.activeSelf);
		};
	}

	// Token: 0x0600994E RID: 39246 RVA: 0x0010815F File Offset: 0x0010635F
	private void ToggleCategoryOpen(GameObject header, bool open)
	{
		header.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("ExpandToggle").ChangeState(open ? 1 : 0);
		this.ContentsGameObject.SetActive(open);
	}

	// Token: 0x04007741 RID: 30529
	protected ContentContainer contents;

	// Token: 0x04007742 RID: 30530
	private string label;

	// Token: 0x04007743 RID: 30531
	private GameObject contentsGameObject;
}
