using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000A8F RID: 2703
public class FoodStorage : KMonoBehaviour
{
	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06003139 RID: 12601 RVA: 0x000C471C File Offset: 0x000C291C
	// (set) Token: 0x0600313A RID: 12602 RVA: 0x000C4724 File Offset: 0x000C2924
	public FilteredStorage FilteredStorage { get; set; }

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x0600313B RID: 12603 RVA: 0x000C472D File Offset: 0x000C292D
	// (set) Token: 0x0600313C RID: 12604 RVA: 0x0020C72C File Offset: 0x0020A92C
	public bool SpicedFoodOnly
	{
		get
		{
			return this.onlyStoreSpicedFood;
		}
		set
		{
			this.onlyStoreSpicedFood = value;
			base.Trigger(1163645216, this.onlyStoreSpicedFood);
			if (this.onlyStoreSpicedFood)
			{
				this.FilteredStorage.AddForbiddenTag(GameTags.UnspicedFood);
				this.storage.DropHasTags(new Tag[]
				{
					GameTags.Edible,
					GameTags.UnspicedFood
				});
				return;
			}
			this.FilteredStorage.RemoveForbiddenTag(GameTags.UnspicedFood);
		}
	}

	// Token: 0x0600313D RID: 12605 RVA: 0x000C4735 File Offset: 0x000C2935
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<FoodStorage>(-905833192, FoodStorage.OnCopySettingsDelegate);
	}

	// Token: 0x0600313E RID: 12606 RVA: 0x000C474E File Offset: 0x000C294E
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600313F RID: 12607 RVA: 0x0020C7AC File Offset: 0x0020A9AC
	private void OnCopySettings(object data)
	{
		FoodStorage component = ((GameObject)data).GetComponent<FoodStorage>();
		if (component != null)
		{
			this.SpicedFoodOnly = component.SpicedFoodOnly;
		}
	}

	// Token: 0x040021E1 RID: 8673
	[Serialize]
	private bool onlyStoreSpicedFood;

	// Token: 0x040021E2 RID: 8674
	[MyCmpReq]
	public Storage storage;

	// Token: 0x040021E4 RID: 8676
	private static readonly EventSystem.IntraObjectHandler<FoodStorage> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<FoodStorage>(delegate(FoodStorage component, object data)
	{
		component.OnCopySettings(data);
	});
}
