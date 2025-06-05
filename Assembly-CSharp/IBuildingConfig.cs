using System;
using UnityEngine;

// Token: 0x02000CF0 RID: 3312
public abstract class IBuildingConfig : IHasDlcRestrictions
{
	// Token: 0x06003F80 RID: 16256
	public abstract BuildingDef CreateBuildingDef();

	// Token: 0x06003F81 RID: 16257 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
	}

	// Token: 0x06003F82 RID: 16258
	public abstract void DoPostConfigureComplete(GameObject go);

	// Token: 0x06003F83 RID: 16259 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06003F84 RID: 16260 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06003F85 RID: 16261 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x06003F86 RID: 16262 RVA: 0x000AA765 File Offset: 0x000A8965
	[Obsolete("Implement GetRequiredDlcIds and/or GetForbiddenDlcIds instead")]
	public virtual string[] GetDlcIds()
	{
		return null;
	}

	// Token: 0x06003F87 RID: 16263 RVA: 0x000AA765 File Offset: 0x000A8965
	public virtual string[] GetRequiredDlcIds()
	{
		return null;
	}

	// Token: 0x06003F88 RID: 16264 RVA: 0x000AA765 File Offset: 0x000A8965
	public virtual string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06003F89 RID: 16265 RVA: 0x000B1628 File Offset: 0x000AF828
	public virtual bool ForbidFromLoading()
	{
		return false;
	}
}
