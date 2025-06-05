using System;
using KSerialization;

// Token: 0x020017E9 RID: 6121
public class RepairableEquipment : KMonoBehaviour
{
	// Token: 0x170007F1 RID: 2033
	// (get) Token: 0x06007DE7 RID: 32231 RVA: 0x000F76D8 File Offset: 0x000F58D8
	// (set) Token: 0x06007DE8 RID: 32232 RVA: 0x000F76E5 File Offset: 0x000F58E5
	public EquipmentDef def
	{
		get
		{
			return this.defHandle.Get<EquipmentDef>();
		}
		set
		{
			this.defHandle.Set<EquipmentDef>(value);
		}
	}

	// Token: 0x06007DE9 RID: 32233 RVA: 0x00334A98 File Offset: 0x00332C98
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.def.AdditionalTags != null)
		{
			foreach (Tag tag in this.def.AdditionalTags)
			{
				base.GetComponent<KPrefabID>().AddTag(tag, false);
			}
		}
	}

	// Token: 0x06007DEA RID: 32234 RVA: 0x00334AE8 File Offset: 0x00332CE8
	protected override void OnSpawn()
	{
		if (!this.facadeID.IsNullOrWhiteSpace())
		{
			KAnim.Build.Symbol symbol = Db.GetEquippableFacades().Get(this.facadeID).AnimFile.GetData().build.GetSymbol("object");
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			component.TryRemoveSymbolOverride("object", 0);
			component.AddSymbolOverride("object", symbol, 0);
		}
	}

	// Token: 0x04005F9F RID: 24479
	public DefHandle defHandle;

	// Token: 0x04005FA0 RID: 24480
	[Serialize]
	public string facadeID;
}
