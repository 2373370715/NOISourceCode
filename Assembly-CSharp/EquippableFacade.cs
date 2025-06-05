using System;
using KSerialization;

// Token: 0x020012F8 RID: 4856
public class EquippableFacade : KMonoBehaviour
{
	// Token: 0x0600638C RID: 25484 RVA: 0x000E5675 File Offset: 0x000E3875
	public static void AddFacadeToEquippable(Equippable equippable, string facadeID)
	{
		EquippableFacade equippableFacade = equippable.gameObject.AddOrGet<EquippableFacade>();
		equippableFacade.FacadeID = facadeID;
		equippableFacade.BuildOverride = Db.GetEquippableFacades().Get(facadeID).BuildOverride;
		equippableFacade.ApplyAnimOverride();
	}

	// Token: 0x0600638D RID: 25485 RVA: 0x000E56A4 File Offset: 0x000E38A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OverrideName();
		this.ApplyAnimOverride();
	}

	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x0600638E RID: 25486 RVA: 0x000E56B8 File Offset: 0x000E38B8
	// (set) Token: 0x0600638F RID: 25487 RVA: 0x000E56C0 File Offset: 0x000E38C0
	public string FacadeID
	{
		get
		{
			return this._facadeID;
		}
		private set
		{
			this._facadeID = value;
			this.OverrideName();
		}
	}

	// Token: 0x06006390 RID: 25488 RVA: 0x000E56CF File Offset: 0x000E38CF
	public void ApplyAnimOverride()
	{
		if (this.FacadeID.IsNullOrWhiteSpace())
		{
			return;
		}
		base.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[]
		{
			Db.GetEquippableFacades().Get(this.FacadeID).AnimFile
		});
	}

	// Token: 0x06006391 RID: 25489 RVA: 0x000E5708 File Offset: 0x000E3908
	private void OverrideName()
	{
		base.GetComponent<KSelectable>().SetName(EquippableFacade.GetNameOverride(base.GetComponent<Equippable>().def.Id, this.FacadeID));
	}

	// Token: 0x06006392 RID: 25490 RVA: 0x000E5730 File Offset: 0x000E3930
	public static string GetNameOverride(string defID, string facadeID)
	{
		if (facadeID.IsNullOrWhiteSpace())
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + defID.ToUpper() + ".NAME");
		}
		return Db.GetEquippableFacades().Get(facadeID).Name;
	}

	// Token: 0x04004760 RID: 18272
	[Serialize]
	private string _facadeID;

	// Token: 0x04004761 RID: 18273
	[Serialize]
	public string BuildOverride;
}
