using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001E47 RID: 7751
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MaterialSelectorSerializer")]
public class MaterialSelectorSerializer : KMonoBehaviour
{
	// Token: 0x0600A22F RID: 41519 RVA: 0x003ED9D0 File Offset: 0x003EBBD0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.previouslySelectedElementsPerWorld == null)
		{
			this.previouslySelectedElementsPerWorld = new List<Dictionary<Tag, Tag>>[255];
			if (this.previouslySelectedElements != null)
			{
				foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
				{
					List<Dictionary<Tag, Tag>> list = this.previouslySelectedElements.ConvertAll<Dictionary<Tag, Tag>>((Dictionary<Tag, Tag> input) => new Dictionary<Tag, Tag>(input));
					this.previouslySelectedElementsPerWorld[worldContainer.id] = list;
				}
				this.previouslySelectedElements = null;
			}
		}
	}

	// Token: 0x0600A230 RID: 41520 RVA: 0x0010DDE8 File Offset: 0x0010BFE8
	public void WipeWorldSelectionData(int worldID)
	{
		this.previouslySelectedElementsPerWorld[worldID] = null;
	}

	// Token: 0x0600A231 RID: 41521 RVA: 0x003EDA8C File Offset: 0x003EBC8C
	public void SetSelectedElement(int worldID, int selectorIndex, Tag recipe, Tag element)
	{
		if (this.previouslySelectedElementsPerWorld[worldID] == null)
		{
			this.previouslySelectedElementsPerWorld[worldID] = new List<Dictionary<Tag, Tag>>();
		}
		List<Dictionary<Tag, Tag>> list = this.previouslySelectedElementsPerWorld[worldID];
		while (list.Count <= selectorIndex)
		{
			list.Add(new Dictionary<Tag, Tag>());
		}
		list[selectorIndex][recipe] = element;
	}

	// Token: 0x0600A232 RID: 41522 RVA: 0x003EDAE0 File Offset: 0x003EBCE0
	public Tag GetPreviousElement(int worldID, int selectorIndex, Tag recipe)
	{
		Tag invalid = Tag.Invalid;
		if (this.previouslySelectedElementsPerWorld[worldID] == null)
		{
			return invalid;
		}
		List<Dictionary<Tag, Tag>> list = this.previouslySelectedElementsPerWorld[worldID];
		if (list.Count <= selectorIndex)
		{
			return invalid;
		}
		list[selectorIndex].TryGetValue(recipe, out invalid);
		return invalid;
	}

	// Token: 0x04007F31 RID: 32561
	[Serialize]
	private List<Dictionary<Tag, Tag>> previouslySelectedElements;

	// Token: 0x04007F32 RID: 32562
	[Serialize]
	private List<Dictionary<Tag, Tag>>[] previouslySelectedElementsPerWorld;
}
