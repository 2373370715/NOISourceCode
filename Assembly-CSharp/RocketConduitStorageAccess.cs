using System;
using UnityEngine;

// Token: 0x02001835 RID: 6197
public class RocketConduitStorageAccess : KMonoBehaviour, ISim200ms
{
	// Token: 0x06007F60 RID: 32608 RVA: 0x0033BEF8 File Offset: 0x0033A0F8
	protected override void OnSpawn()
	{
		WorldContainer myWorld = this.GetMyWorld();
		this.craftModuleInterface = myWorld.GetComponent<CraftModuleInterface>();
	}

	// Token: 0x06007F61 RID: 32609 RVA: 0x0033BF18 File Offset: 0x0033A118
	public void Sim200ms(float dt)
	{
		if (this.operational != null && !this.operational.IsOperational)
		{
			return;
		}
		float num = this.storage.MassStored();
		if (num < this.targetLevel - 0.01f || num > this.targetLevel + 0.01f)
		{
			if (this.operational != null)
			{
				this.operational.SetActive(true, false);
			}
			float num2 = this.targetLevel - num;
			foreach (Ref<RocketModuleCluster> @ref in this.craftModuleInterface.ClusterModules)
			{
				CargoBayCluster component = @ref.Get().GetComponent<CargoBayCluster>();
				if (component != null && component.storageType == this.cargoType)
				{
					if (num2 > 0f && component.storage.MassStored() > 0f)
					{
						for (int i = component.storage.items.Count - 1; i >= 0; i--)
						{
							GameObject gameObject = component.storage.items[i];
							if (!(this.filterable != null) || !(this.filterable.SelectedTag != GameTags.Void) || !(gameObject.PrefabID() != this.filterable.SelectedTag))
							{
								Pickupable pickupable = gameObject.GetComponent<Pickupable>().Take(num2);
								if (pickupable != null)
								{
									num2 -= pickupable.PrimaryElement.Mass;
									this.storage.Store(pickupable.gameObject, true, false, true, false);
								}
								if (num2 <= 0f)
								{
									break;
								}
							}
						}
						if (num2 <= 0f)
						{
							break;
						}
					}
					if (num2 < 0f && component.storage.RemainingCapacity() > 0f)
					{
						Mathf.Min(-num2, component.storage.RemainingCapacity());
						for (int j = this.storage.items.Count - 1; j >= 0; j--)
						{
							Pickupable pickupable2 = this.storage.items[j].GetComponent<Pickupable>().Take(-num2);
							if (pickupable2 != null)
							{
								num2 += pickupable2.PrimaryElement.Mass;
								component.storage.Store(pickupable2.gameObject, true, false, true, false);
							}
							if (num2 >= 0f)
							{
								break;
							}
						}
						if (num2 >= 0f)
						{
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x040060DB RID: 24795
	[SerializeField]
	public Storage storage;

	// Token: 0x040060DC RID: 24796
	[SerializeField]
	public float targetLevel;

	// Token: 0x040060DD RID: 24797
	[SerializeField]
	public CargoBay.CargoType cargoType;

	// Token: 0x040060DE RID: 24798
	[MyCmpGet]
	private Filterable filterable;

	// Token: 0x040060DF RID: 24799
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040060E0 RID: 24800
	private const float TOLERANCE = 0.01f;

	// Token: 0x040060E1 RID: 24801
	private CraftModuleInterface craftModuleInterface;
}
