using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200142C RID: 5164
public class HighEnergyParticlePort : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x060069CF RID: 27087 RVA: 0x000E9CEB File Offset: 0x000E7EEB
	public int GetHighEnergyParticleInputPortPosition()
	{
		return this.m_building.GetHighEnergyParticleInputCell();
	}

	// Token: 0x060069D0 RID: 27088 RVA: 0x000E9CF8 File Offset: 0x000E7EF8
	public int GetHighEnergyParticleOutputPortPosition()
	{
		return this.m_building.GetHighEnergyParticleOutputCell();
	}

	// Token: 0x060069D1 RID: 27089 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060069D2 RID: 27090 RVA: 0x000E9D05 File Offset: 0x000E7F05
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.HighEnergyParticlePorts.Add(this);
	}

	// Token: 0x060069D3 RID: 27091 RVA: 0x000E9D18 File Offset: 0x000E7F18
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.HighEnergyParticlePorts.Remove(this);
	}

	// Token: 0x060069D4 RID: 27092 RVA: 0x002EA7D4 File Offset: 0x002E89D4
	public bool InputActive()
	{
		Operational component = base.GetComponent<Operational>();
		return this.particleInputEnabled && component != null && component.IsFunctional && (!this.requireOperational || component.IsOperational);
	}

	// Token: 0x060069D5 RID: 27093 RVA: 0x000E9D2B File Offset: 0x000E7F2B
	public bool AllowCapture(HighEnergyParticle particle)
	{
		return this.onParticleCaptureAllowed == null || this.onParticleCaptureAllowed(particle);
	}

	// Token: 0x060069D6 RID: 27094 RVA: 0x000E9D43 File Offset: 0x000E7F43
	public void Capture(HighEnergyParticle particle)
	{
		this.currentParticle = particle;
		if (this.onParticleCapture != null)
		{
			this.onParticleCapture(particle);
		}
	}

	// Token: 0x060069D7 RID: 27095 RVA: 0x000E9D60 File Offset: 0x000E7F60
	public void Uncapture(HighEnergyParticle particle)
	{
		if (this.onParticleUncapture != null)
		{
			this.onParticleUncapture(particle);
		}
		this.currentParticle = null;
	}

	// Token: 0x060069D8 RID: 27096 RVA: 0x002EA814 File Offset: 0x002E8A14
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.particleInputEnabled)
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.PARTICLE_PORT_INPUT, UI.BUILDINGEFFECTS.TOOLTIPS.PARTICLE_PORT_INPUT, Descriptor.DescriptorType.Requirement, false));
		}
		if (this.particleOutputEnabled)
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.PARTICLE_PORT_OUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.PARTICLE_PORT_OUTPUT, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x04005045 RID: 20549
	[MyCmpGet]
	private Building m_building;

	// Token: 0x04005046 RID: 20550
	public HighEnergyParticlePort.OnParticleCapture onParticleCapture;

	// Token: 0x04005047 RID: 20551
	public HighEnergyParticlePort.OnParticleCaptureAllowed onParticleCaptureAllowed;

	// Token: 0x04005048 RID: 20552
	public HighEnergyParticlePort.OnParticleCapture onParticleUncapture;

	// Token: 0x04005049 RID: 20553
	public HighEnergyParticle currentParticle;

	// Token: 0x0400504A RID: 20554
	public bool requireOperational = true;

	// Token: 0x0400504B RID: 20555
	public bool particleInputEnabled;

	// Token: 0x0400504C RID: 20556
	public bool particleOutputEnabled;

	// Token: 0x0400504D RID: 20557
	public CellOffset particleInputOffset;

	// Token: 0x0400504E RID: 20558
	public CellOffset particleOutputOffset;

	// Token: 0x0200142D RID: 5165
	// (Invoke) Token: 0x060069DB RID: 27099
	public delegate void OnParticleCapture(HighEnergyParticle particle);

	// Token: 0x0200142E RID: 5166
	// (Invoke) Token: 0x060069DF RID: 27103
	public delegate bool OnParticleCaptureAllowed(HighEnergyParticle particle);
}
