using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200191C RID: 6428
[SerializationConfig(MemberSerialization.OptIn)]
public class ClusterFXEntity : ClusterGridEntity
{
	// Token: 0x17000885 RID: 2181
	// (get) Token: 0x06008520 RID: 34080 RVA: 0x000FBF65 File Offset: 0x000FA165
	public override string Name
	{
		get
		{
			return UI.SPACEDESTINATIONS.TELESCOPE_TARGET.NAME;
		}
	}

	// Token: 0x17000886 RID: 2182
	// (get) Token: 0x06008521 RID: 34081 RVA: 0x000B1723 File Offset: 0x000AF923
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.FX;
		}
	}

	// Token: 0x17000887 RID: 2183
	// (get) Token: 0x06008522 RID: 34082 RVA: 0x003547BC File Offset: 0x003529BC
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim(this.kAnimName),
					initialAnim = this.animName,
					playMode = this.animPlayMode,
					animOffset = this.animOffset
				}
			};
		}
	}

	// Token: 0x17000888 RID: 2184
	// (get) Token: 0x06008523 RID: 34083 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000889 RID: 2185
	// (get) Token: 0x06008524 RID: 34084 RVA: 0x000AA7FE File Offset: 0x000A89FE
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x06008525 RID: 34085 RVA: 0x000FBF71 File Offset: 0x000FA171
	public void Init(AxialI location, Vector3 animOffset)
	{
		base.Location = location;
		this.animOffset = animOffset;
	}

	// Token: 0x0400655A RID: 25946
	[SerializeField]
	public string kAnimName;

	// Token: 0x0400655B RID: 25947
	[SerializeField]
	public string animName;

	// Token: 0x0400655C RID: 25948
	public KAnim.PlayMode animPlayMode = KAnim.PlayMode.Once;

	// Token: 0x0400655D RID: 25949
	public Vector3 animOffset;
}
