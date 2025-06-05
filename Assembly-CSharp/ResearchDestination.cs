using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

// Token: 0x02001974 RID: 6516
[SerializationConfig(MemberSerialization.OptIn)]
public class ResearchDestination : ClusterGridEntity
{
	// Token: 0x170008ED RID: 2285
	// (get) Token: 0x060087B1 RID: 34737 RVA: 0x000FD625 File Offset: 0x000FB825
	public override string Name
	{
		get
		{
			return UI.SPACEDESTINATIONS.RESEARCHDESTINATION.NAME;
		}
	}

	// Token: 0x170008EE RID: 2286
	// (get) Token: 0x060087B2 RID: 34738 RVA: 0x000AA7FE File Offset: 0x000A89FE
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x170008EF RID: 2287
	// (get) Token: 0x060087B3 RID: 34739 RVA: 0x000FD631 File Offset: 0x000FB831
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>();
		}
	}

	// Token: 0x170008F0 RID: 2288
	// (get) Token: 0x060087B4 RID: 34740 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool IsVisible
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170008F1 RID: 2289
	// (get) Token: 0x060087B5 RID: 34741 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x060087B6 RID: 34742 RVA: 0x000FD638 File Offset: 0x000FB838
	public void Init(AxialI location)
	{
		this.m_location = location;
	}
}
