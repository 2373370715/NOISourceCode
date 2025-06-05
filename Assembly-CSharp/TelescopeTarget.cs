using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

// Token: 0x0200199E RID: 6558
[SerializationConfig(MemberSerialization.OptIn)]
public class TelescopeTarget : ClusterGridEntity
{
	// Token: 0x170008FC RID: 2300
	// (get) Token: 0x060088B7 RID: 34999 RVA: 0x000FBF65 File Offset: 0x000FA165
	public override string Name
	{
		get
		{
			return UI.SPACEDESTINATIONS.TELESCOPE_TARGET.NAME;
		}
	}

	// Token: 0x170008FD RID: 2301
	// (get) Token: 0x060088B8 RID: 35000 RVA: 0x000B1693 File Offset: 0x000AF893
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Telescope;
		}
	}

	// Token: 0x170008FE RID: 2302
	// (get) Token: 0x060088B9 RID: 35001 RVA: 0x003649B4 File Offset: 0x00362BB4
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("telescope_target_kanim"),
					initialAnim = "idle"
				}
			};
		}
	}

	// Token: 0x170008FF RID: 2303
	// (get) Token: 0x060088BA RID: 35002 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000900 RID: 2304
	// (get) Token: 0x060088BB RID: 35003 RVA: 0x000AA7FE File Offset: 0x000A89FE
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x060088BC RID: 35004 RVA: 0x000FB966 File Offset: 0x000F9B66
	public void Init(AxialI location)
	{
		base.Location = location;
	}

	// Token: 0x060088BD RID: 35005 RVA: 0x000FDFBE File Offset: 0x000FC1BE
	public void SetTargetMeteorShower(ClusterMapMeteorShower.Instance meteorShower)
	{
		this.targetMeteorShower = meteorShower;
	}

	// Token: 0x060088BE RID: 35006 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowName()
	{
		return true;
	}

	// Token: 0x060088BF RID: 35007 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowProgressBar()
	{
		return true;
	}

	// Token: 0x060088C0 RID: 35008 RVA: 0x000FDFC7 File Offset: 0x000FC1C7
	public override float GetProgress()
	{
		if (this.targetMeteorShower != null)
		{
			return this.targetMeteorShower.IdentifyingProgress;
		}
		return SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().GetRevealCompleteFraction(base.Location);
	}

	// Token: 0x04006788 RID: 26504
	private ClusterMapMeteorShower.Instance targetMeteorShower;
}
