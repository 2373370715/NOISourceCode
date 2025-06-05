using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020017E0 RID: 6112
[AddComponentMenu("KMonoBehaviour/scripts/Radiator")]
public class Radiator : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06007D9E RID: 32158 RVA: 0x00332D58 File Offset: 0x00330F58
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.emitter = new RadiationGridEmitter(Grid.PosToCell(base.gameObject), this.intensity);
		this.emitter.projectionCount = this.projectionCount;
		this.emitter.direction = this.direction;
		this.emitter.angle = this.angle;
		if (base.GetComponent<Operational>() == null)
		{
			this.emitter.enabled = true;
		}
		else
		{
			base.Subscribe(824508782, new Action<object>(this.OnOperationalChanged));
		}
		RadiationGridManager.emitters.Add(this.emitter);
	}

	// Token: 0x06007D9F RID: 32159 RVA: 0x000F73E2 File Offset: 0x000F55E2
	protected override void OnCleanUp()
	{
		RadiationGridManager.emitters.Remove(this.emitter);
		base.OnCleanUp();
	}

	// Token: 0x06007DA0 RID: 32160 RVA: 0x00332E00 File Offset: 0x00331000
	private void OnOperationalChanged(object data)
	{
		bool isActive = base.GetComponent<Operational>().IsActive;
		this.emitter.enabled = isActive;
	}

	// Token: 0x06007DA1 RID: 32161 RVA: 0x000F73FB File Offset: 0x000F55FB
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, this.intensity), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x06007DA2 RID: 32162 RVA: 0x000F7433 File Offset: 0x000F5633
	private void Update()
	{
		this.emitter.originCell = Grid.PosToCell(base.gameObject);
	}

	// Token: 0x04005F5E RID: 24414
	public RadiationGridEmitter emitter;

	// Token: 0x04005F5F RID: 24415
	public int intensity;

	// Token: 0x04005F60 RID: 24416
	public int projectionCount;

	// Token: 0x04005F61 RID: 24417
	public int direction;

	// Token: 0x04005F62 RID: 24418
	public int angle = 360;
}
