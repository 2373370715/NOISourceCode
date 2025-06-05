using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020017C7 RID: 6087
public class FossilDigsiteLampLight : Light2D
{
	// Token: 0x170007DA RID: 2010
	// (get) Token: 0x06007D14 RID: 32020 RVA: 0x000F6C5C File Offset: 0x000F4E5C
	// (set) Token: 0x06007D13 RID: 32019 RVA: 0x000F6C53 File Offset: 0x000F4E53
	public bool independent { get; private set; }

	// Token: 0x06007D15 RID: 32021 RVA: 0x000F6C64 File Offset: 0x000F4E64
	protected override void OnPrefabInit()
	{
		base.Subscribe<FossilDigsiteLampLight>(-592767678, FossilDigsiteLampLight.OnOperationalChangedDelegate);
		base.IntensityAnimation = 1f;
	}

	// Token: 0x06007D16 RID: 32022 RVA: 0x003304E8 File Offset: 0x0032E6E8
	public void SetIndependentState(bool isIndependent, bool checkOperational = true)
	{
		this.independent = isIndependent;
		Operational component = base.GetComponent<Operational>();
		if (component != null && this.independent && checkOperational && base.enabled != component.IsOperational)
		{
			base.enabled = component.IsOperational;
		}
	}

	// Token: 0x06007D17 RID: 32023 RVA: 0x000F6C82 File Offset: 0x000F4E82
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.independent || base.enabled)
		{
			return base.GetDescriptors(go);
		}
		return new List<Descriptor>();
	}

	// Token: 0x04005E3A RID: 24122
	private static readonly EventSystem.IntraObjectHandler<FossilDigsiteLampLight> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<FossilDigsiteLampLight>(delegate(FossilDigsiteLampLight light, object data)
	{
		if (light.independent)
		{
			light.enabled = (bool)data;
		}
	});
}
