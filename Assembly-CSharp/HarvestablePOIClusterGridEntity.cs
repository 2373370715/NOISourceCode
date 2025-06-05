using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x0200193A RID: 6458
[SerializationConfig(MemberSerialization.OptIn)]
public class HarvestablePOIClusterGridEntity : ClusterGridEntity
{
	// Token: 0x170008BF RID: 2239
	// (get) Token: 0x06008667 RID: 34407 RVA: 0x000FCBD9 File Offset: 0x000FADD9
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x170008C0 RID: 2240
	// (get) Token: 0x06008668 RID: 34408 RVA: 0x000AA7FE File Offset: 0x000A89FE
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x170008C1 RID: 2241
	// (get) Token: 0x06008669 RID: 34409 RVA: 0x00359F44 File Offset: 0x00358144
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("harvestable_space_poi_kanim"),
					initialAnim = (this.m_Anim.IsNullOrWhiteSpace() ? "cloud" : this.m_Anim)
				}
			};
		}
	}

	// Token: 0x170008C2 RID: 2242
	// (get) Token: 0x0600866A RID: 34410 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170008C3 RID: 2243
	// (get) Token: 0x0600866B RID: 34411 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x0600866C RID: 34412 RVA: 0x000FB966 File Offset: 0x000F9B66
	public void Init(AxialI location)
	{
		base.Location = location;
	}

	// Token: 0x040065E0 RID: 26080
	public string m_name;

	// Token: 0x040065E1 RID: 26081
	public string m_Anim;
}
