using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000DA4 RID: 3492
[AddComponentMenu("KMonoBehaviour/scripts/Filterable")]
public class Filterable : KMonoBehaviour
{
	// Token: 0x14000012 RID: 18
	// (add) Token: 0x060043D5 RID: 17365 RVA: 0x002544DC File Offset: 0x002526DC
	// (remove) Token: 0x060043D6 RID: 17366 RVA: 0x00254514 File Offset: 0x00252714
	public event Action<Tag> onFilterChanged;

	// Token: 0x17000357 RID: 855
	// (get) Token: 0x060043D7 RID: 17367 RVA: 0x000D0466 File Offset: 0x000CE666
	// (set) Token: 0x060043D8 RID: 17368 RVA: 0x000D046E File Offset: 0x000CE66E
	public Tag SelectedTag
	{
		get
		{
			return this.selectedTag;
		}
		set
		{
			this.selectedTag = value;
			this.OnFilterChanged();
		}
	}

	// Token: 0x060043D9 RID: 17369 RVA: 0x0025454C File Offset: 0x0025274C
	public Dictionary<Tag, HashSet<Tag>> GetTagOptions()
	{
		Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
		if (this.filterElementState == Filterable.ElementState.Solid)
		{
			dictionary = DiscoveredResources.Instance.GetDiscoveredResourcesFromTagSet(Filterable.filterableCategories);
		}
		else
		{
			foreach (Element element in ElementLoader.elements)
			{
				if (!element.disabled && ((element.IsGas && this.filterElementState == Filterable.ElementState.Gas) || (element.IsLiquid && this.filterElementState == Filterable.ElementState.Liquid)))
				{
					Tag materialCategoryTag = element.GetMaterialCategoryTag();
					if (!dictionary.ContainsKey(materialCategoryTag))
					{
						dictionary[materialCategoryTag] = new HashSet<Tag>();
					}
					Tag item = GameTagExtensions.Create(element.id);
					dictionary[materialCategoryTag].Add(item);
				}
			}
		}
		dictionary.Add(GameTags.Void, new HashSet<Tag>
		{
			GameTags.Void
		});
		return dictionary;
	}

	// Token: 0x060043DA RID: 17370 RVA: 0x000D047D File Offset: 0x000CE67D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Filterable>(-905833192, Filterable.OnCopySettingsDelegate);
	}

	// Token: 0x060043DB RID: 17371 RVA: 0x0025463C File Offset: 0x0025283C
	private void OnCopySettings(object data)
	{
		Filterable component = ((GameObject)data).GetComponent<Filterable>();
		if (component != null)
		{
			this.SelectedTag = component.SelectedTag;
		}
	}

	// Token: 0x060043DC RID: 17372 RVA: 0x000D0496 File Offset: 0x000CE696
	protected override void OnSpawn()
	{
		this.OnFilterChanged();
	}

	// Token: 0x060043DD RID: 17373 RVA: 0x0025466C File Offset: 0x0025286C
	private void OnFilterChanged()
	{
		if (this.onFilterChanged != null)
		{
			this.onFilterChanged(this.selectedTag);
		}
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Filterable.filterSelected, this.selectedTag.IsValid);
		}
	}

	// Token: 0x04002EFB RID: 12027
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002EFC RID: 12028
	[Serialize]
	public Filterable.ElementState filterElementState;

	// Token: 0x04002EFD RID: 12029
	[Serialize]
	private Tag selectedTag = GameTags.Void;

	// Token: 0x04002EFF RID: 12031
	private static TagSet filterableCategories = new TagSet(new TagSet[]
	{
		GameTags.CalorieCategories,
		GameTags.UnitCategories,
		GameTags.MaterialCategories,
		GameTags.MaterialBuildingElements
	});

	// Token: 0x04002F00 RID: 12032
	private static readonly Operational.Flag filterSelected = new Operational.Flag("filterSelected", Operational.Flag.Type.Requirement);

	// Token: 0x04002F01 RID: 12033
	private static readonly EventSystem.IntraObjectHandler<Filterable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Filterable>(delegate(Filterable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02000DA5 RID: 3493
	public enum ElementState
	{
		// Token: 0x04002F03 RID: 12035
		None,
		// Token: 0x04002F04 RID: 12036
		Solid,
		// Token: 0x04002F05 RID: 12037
		Liquid,
		// Token: 0x04002F06 RID: 12038
		Gas
	}
}
