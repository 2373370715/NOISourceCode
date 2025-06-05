using System;
using KSerialization;

// Token: 0x02000B61 RID: 2913
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class SubstanceSource : KMonoBehaviour
{
	// Token: 0x060036CD RID: 14029 RVA: 0x000C8187 File Offset: 0x000C6387
	protected override void OnPrefabInit()
	{
		this.pickupable.SetWorkTime(SubstanceSource.MaxPickupTime);
	}

	// Token: 0x060036CE RID: 14030 RVA: 0x000C8199 File Offset: 0x000C6399
	protected override void OnSpawn()
	{
		this.pickupable.SetWorkTime(10f);
	}

	// Token: 0x060036CF RID: 14031
	protected abstract CellOffset[] GetOffsetGroup();

	// Token: 0x060036D0 RID: 14032
	protected abstract IChunkManager GetChunkManager();

	// Token: 0x060036D1 RID: 14033 RVA: 0x000C81AB File Offset: 0x000C63AB
	public SimHashes GetElementID()
	{
		return this.primaryElement.ElementID;
	}

	// Token: 0x060036D2 RID: 14034 RVA: 0x00222314 File Offset: 0x00220514
	public Tag GetElementTag()
	{
		Tag result = Tag.Invalid;
		if (base.gameObject != null && this.primaryElement != null && this.primaryElement.Element != null)
		{
			result = this.primaryElement.Element.tag;
		}
		return result;
	}

	// Token: 0x060036D3 RID: 14035 RVA: 0x00222364 File Offset: 0x00220564
	public Tag GetMaterialCategoryTag()
	{
		Tag result = Tag.Invalid;
		if (base.gameObject != null && this.primaryElement != null && this.primaryElement.Element != null)
		{
			result = this.primaryElement.Element.GetMaterialCategoryTag();
		}
		return result;
	}

	// Token: 0x040025E4 RID: 9700
	private bool enableRefresh;

	// Token: 0x040025E5 RID: 9701
	private static readonly float MaxPickupTime = 8f;

	// Token: 0x040025E6 RID: 9702
	[MyCmpReq]
	public Pickupable pickupable;

	// Token: 0x040025E7 RID: 9703
	[MyCmpReq]
	private PrimaryElement primaryElement;
}
