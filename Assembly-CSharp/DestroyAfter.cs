using System;
using UnityEngine;

// Token: 0x02000A61 RID: 2657
[AddComponentMenu("KMonoBehaviour/scripts/DestroyAfter")]
public class DestroyAfter : KMonoBehaviour
{
	// Token: 0x06003010 RID: 12304 RVA: 0x000C3ADB File Offset: 0x000C1CDB
	protected override void OnSpawn()
	{
		this.particleSystems = base.gameObject.GetComponentsInChildren<ParticleSystem>(true);
	}

	// Token: 0x06003011 RID: 12305 RVA: 0x00207F64 File Offset: 0x00206164
	private bool IsAlive()
	{
		for (int i = 0; i < this.particleSystems.Length; i++)
		{
			if (this.particleSystems[i].IsAlive(false))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003012 RID: 12306 RVA: 0x000C3AEF File Offset: 0x000C1CEF
	private void Update()
	{
		if (this.particleSystems != null && !this.IsAlive())
		{
			this.DeleteObject();
		}
	}

	// Token: 0x0400210D RID: 8461
	private ParticleSystem[] particleSystems;
}
