using System;
using TUNING;
using UnityEngine;

// Token: 0x02000CCE RID: 3278
[AddComponentMenu("KMonoBehaviour/Workable/AlgaeHabitatEmpty")]
public class AlgaeHabitatEmpty : Workable
{
	// Token: 0x06003E93 RID: 16019 RVA: 0x0024310C File Offset: 0x0024130C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.workAnims = AlgaeHabitatEmpty.CLEAN_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			AlgaeHabitatEmpty.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			AlgaeHabitatEmpty.PST_ANIM
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x04002B4A RID: 11082
	private static readonly HashedString[] CLEAN_ANIMS = new HashedString[]
	{
		"sponge_pre",
		"sponge_loop"
	};

	// Token: 0x04002B4B RID: 11083
	private static readonly HashedString PST_ANIM = new HashedString("sponge_pst");
}
