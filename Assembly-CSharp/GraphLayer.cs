using System;
using UnityEngine;

// Token: 0x02001D44 RID: 7492
[RequireComponent(typeof(GraphBase))]
[AddComponentMenu("KMonoBehaviour/scripts/GraphLayer")]
public class GraphLayer : KMonoBehaviour
{
	// Token: 0x17000A50 RID: 2640
	// (get) Token: 0x06009C7B RID: 40059 RVA: 0x0010A396 File Offset: 0x00108596
	public GraphBase graph
	{
		get
		{
			if (this.graph_base == null)
			{
				this.graph_base = base.GetComponent<GraphBase>();
			}
			return this.graph_base;
		}
	}

	// Token: 0x04007A84 RID: 31364
	[MyCmpReq]
	private GraphBase graph_base;
}
