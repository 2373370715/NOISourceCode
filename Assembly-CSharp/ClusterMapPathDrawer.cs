using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02001C6F RID: 7279
public class ClusterMapPathDrawer : MonoBehaviour
{
	// Token: 0x0600975B RID: 38747 RVA: 0x001070CC File Offset: 0x001052CC
	public ClusterMapPath AddPath()
	{
		ClusterMapPath clusterMapPath = UnityEngine.Object.Instantiate<ClusterMapPath>(this.pathPrefab, this.pathContainer);
		clusterMapPath.Init();
		return clusterMapPath;
	}

	// Token: 0x0600975C RID: 38748 RVA: 0x001070E5 File Offset: 0x001052E5
	public static List<Vector2> GetDrawPathList(Vector2 startLocation, List<AxialI> pathPoints)
	{
		List<Vector2> list = new List<Vector2>();
		list.Add(startLocation);
		list.AddRange(from point in pathPoints
		select point.ToWorld2D());
		return list;
	}

	// Token: 0x040075D5 RID: 30165
	public ClusterMapPath pathPrefab;

	// Token: 0x040075D6 RID: 30166
	public Transform pathContainer;
}
