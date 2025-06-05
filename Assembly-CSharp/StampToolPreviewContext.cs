using System;
using UnityEngine;

// Token: 0x02001495 RID: 5269
public class StampToolPreviewContext
{
	// Token: 0x04005254 RID: 21076
	public Transform previewParent;

	// Token: 0x04005255 RID: 21077
	public InterfaceTool tool;

	// Token: 0x04005256 RID: 21078
	public TemplateContainer stampTemplate;

	// Token: 0x04005257 RID: 21079
	public System.Action frameAfterSetupFn;

	// Token: 0x04005258 RID: 21080
	public Action<int> refreshFn;

	// Token: 0x04005259 RID: 21081
	public System.Action onPlaceFn;

	// Token: 0x0400525A RID: 21082
	public Action<string> onErrorChangeFn;

	// Token: 0x0400525B RID: 21083
	public System.Action cleanupFn;
}
