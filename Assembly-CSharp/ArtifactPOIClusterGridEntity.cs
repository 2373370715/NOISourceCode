using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02001904 RID: 6404
[SerializationConfig(MemberSerialization.OptIn)]
public class ArtifactPOIClusterGridEntity : ClusterGridEntity
{
	// Token: 0x17000874 RID: 2164
	// (get) Token: 0x0600848B RID: 33931 RVA: 0x000FB95E File Offset: 0x000F9B5E
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x17000875 RID: 2165
	// (get) Token: 0x0600848C RID: 33932 RVA: 0x000AA7FE File Offset: 0x000A89FE
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x17000876 RID: 2166
	// (get) Token: 0x0600848D RID: 33933 RVA: 0x00352F08 File Offset: 0x00351108
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("gravitas_space_poi_kanim"),
					initialAnim = (this.m_Anim.IsNullOrWhiteSpace() ? "station_1" : this.m_Anim)
				}
			};
		}
	}

	// Token: 0x17000877 RID: 2167
	// (get) Token: 0x0600848E RID: 33934 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000878 RID: 2168
	// (get) Token: 0x0600848F RID: 33935 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x06008490 RID: 33936 RVA: 0x000FB966 File Offset: 0x000F9B66
	public void Init(AxialI location)
	{
		base.Location = location;
	}

	// Token: 0x040064ED RID: 25837
	public string m_name;

	// Token: 0x040064EE RID: 25838
	public string m_Anim;
}
