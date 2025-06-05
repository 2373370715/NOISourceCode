using System;
using System.Collections.Generic;
using ProcGen.Map;
using ProcGenGame;
using VoronoiTree;

namespace Klei
{
	// Token: 0x02003C4D RID: 15437
	public class TerrainCellLogged : TerrainCell
	{
		// Token: 0x0600EC72 RID: 60530 RVA: 0x0014338A File Offset: 0x0014158A
		public TerrainCellLogged()
		{
		}

		// Token: 0x0600EC73 RID: 60531 RVA: 0x00143392 File Offset: 0x00141592
		public TerrainCellLogged(Cell node, Diagram.Site site, Dictionary<Tag, int> distancesToTags) : base(node, site, distancesToTags)
		{
		}

		// Token: 0x0600EC74 RID: 60532 RVA: 0x000AA038 File Offset: 0x000A8238
		public override void LogInfo(string evt, string param, float value)
		{
		}
	}
}
