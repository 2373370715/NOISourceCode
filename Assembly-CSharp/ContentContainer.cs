using System;
using System.Collections.Generic;
using KSerialization.Converters;
using UnityEngine;

// Token: 0x02001CA2 RID: 7330
public class ContentContainer : IHasDlcRestrictions
{
	// Token: 0x060098C7 RID: 39111 RVA: 0x00107C23 File Offset: 0x00105E23
	public ContentContainer()
	{
		this.content = new List<ICodexWidget>();
	}

	// Token: 0x060098C8 RID: 39112 RVA: 0x00107C36 File Offset: 0x00105E36
	public ContentContainer(List<ICodexWidget> content, ContentContainer.ContentLayout contentLayout)
	{
		this.content = content;
		this.contentLayout = contentLayout;
	}

	// Token: 0x170009FF RID: 2559
	// (get) Token: 0x060098C9 RID: 39113 RVA: 0x00107C4C File Offset: 0x00105E4C
	// (set) Token: 0x060098CA RID: 39114 RVA: 0x00107C54 File Offset: 0x00105E54
	public List<ICodexWidget> content { get; set; }

	// Token: 0x17000A00 RID: 2560
	// (get) Token: 0x060098CB RID: 39115 RVA: 0x00107C5D File Offset: 0x00105E5D
	// (set) Token: 0x060098CC RID: 39116 RVA: 0x00107C65 File Offset: 0x00105E65
	public string lockID { get; set; }

	// Token: 0x17000A01 RID: 2561
	// (get) Token: 0x060098CD RID: 39117 RVA: 0x00107C6E File Offset: 0x00105E6E
	// (set) Token: 0x060098CE RID: 39118 RVA: 0x00107C76 File Offset: 0x00105E76
	public string[] requiredDlcIds { get; set; }

	// Token: 0x17000A02 RID: 2562
	// (get) Token: 0x060098CF RID: 39119 RVA: 0x00107C7F File Offset: 0x00105E7F
	// (set) Token: 0x060098D0 RID: 39120 RVA: 0x00107C87 File Offset: 0x00105E87
	public string[] forbiddenDlcIds { get; set; }

	// Token: 0x17000A03 RID: 2563
	// (get) Token: 0x060098D1 RID: 39121 RVA: 0x00107C90 File Offset: 0x00105E90
	// (set) Token: 0x060098D2 RID: 39122 RVA: 0x00107C98 File Offset: 0x00105E98
	[StringEnumConverter]
	public ContentContainer.ContentLayout contentLayout { get; set; }

	// Token: 0x17000A04 RID: 2564
	// (get) Token: 0x060098D3 RID: 39123 RVA: 0x00107CA1 File Offset: 0x00105EA1
	// (set) Token: 0x060098D4 RID: 39124 RVA: 0x00107CA9 File Offset: 0x00105EA9
	public bool showBeforeGeneratedContent { get; set; }

	// Token: 0x060098D5 RID: 39125 RVA: 0x00107CB2 File Offset: 0x00105EB2
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x060098D6 RID: 39126 RVA: 0x00107CBA File Offset: 0x00105EBA
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x040076F6 RID: 30454
	public GameObject go;

	// Token: 0x02001CA3 RID: 7331
	public enum ContentLayout
	{
		// Token: 0x040076FE RID: 30462
		Vertical,
		// Token: 0x040076FF RID: 30463
		Horizontal,
		// Token: 0x04007700 RID: 30464
		Grid,
		// Token: 0x04007701 RID: 30465
		GridTwoColumn,
		// Token: 0x04007702 RID: 30466
		GridTwoColumnTall
	}
}
