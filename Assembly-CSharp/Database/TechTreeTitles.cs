using System;
using UnityEngine;

namespace Database
{
	// Token: 0x020021E4 RID: 8676
	public class TechTreeTitles : ResourceSet<TechTreeTitle>
	{
		// Token: 0x0600B8D6 RID: 47318 RVA: 0x0011BA71 File Offset: 0x00119C71
		public TechTreeTitles(ResourceSet parent) : base("TreeTitles", parent)
		{
		}

		// Token: 0x0600B8D7 RID: 47319 RVA: 0x00472C60 File Offset: 0x00470E60
		public void Load(TextAsset tree_file)
		{
			foreach (ResourceTreeNode resourceTreeNode in new ResourceTreeLoader<ResourceTreeNode>(tree_file))
			{
				if (string.Equals(resourceTreeNode.Id.Substring(0, 1), "_"))
				{
					new TechTreeTitle(resourceTreeNode.Id, this, Strings.Get("STRINGS.RESEARCH.TREES.TITLE" + resourceTreeNode.Id.ToUpper()), resourceTreeNode);
				}
			}
		}
	}
}
