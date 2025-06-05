using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001CBC RID: 7356
public class CodexVideo : CodexWidget<CodexVideo>
{
	// Token: 0x17000A19 RID: 2585
	// (get) Token: 0x06009951 RID: 39249 RVA: 0x001081AF File Offset: 0x001063AF
	// (set) Token: 0x06009952 RID: 39250 RVA: 0x001081B7 File Offset: 0x001063B7
	public string name { get; set; }

	// Token: 0x17000A1A RID: 2586
	// (get) Token: 0x06009954 RID: 39252 RVA: 0x001081C9 File Offset: 0x001063C9
	// (set) Token: 0x06009953 RID: 39251 RVA: 0x001081C0 File Offset: 0x001063C0
	public string videoName
	{
		get
		{
			return "--> " + (this.name ?? "NULL");
		}
		set
		{
			this.name = value;
		}
	}

	// Token: 0x17000A1B RID: 2587
	// (get) Token: 0x06009955 RID: 39253 RVA: 0x001081E4 File Offset: 0x001063E4
	// (set) Token: 0x06009956 RID: 39254 RVA: 0x001081EC File Offset: 0x001063EC
	public string overlayName { get; set; }

	// Token: 0x17000A1C RID: 2588
	// (get) Token: 0x06009957 RID: 39255 RVA: 0x001081F5 File Offset: 0x001063F5
	// (set) Token: 0x06009958 RID: 39256 RVA: 0x001081FD File Offset: 0x001063FD
	public List<string> overlayTexts { get; set; }

	// Token: 0x06009959 RID: 39257 RVA: 0x00108206 File Offset: 0x00106406
	public void ConfigureVideo(VideoWidget videoWidget, string clipName, string overlayName = null, List<string> overlayTexts = null)
	{
		videoWidget.SetClip(Assets.GetVideo(clipName), overlayName, overlayTexts);
	}

	// Token: 0x0600995A RID: 39258 RVA: 0x00108217 File Offset: 0x00106417
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureVideo(contentGameObject.GetComponent<VideoWidget>(), this.name, this.overlayName, this.overlayTexts);
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
