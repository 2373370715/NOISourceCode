using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000B9E RID: 2974
[AddComponentMenu("KMonoBehaviour/Workable/Unsealable")]
public class Unsealable : Workable
{
	// Token: 0x060037EB RID: 14315 RVA: 0x000AF932 File Offset: 0x000ADB32
	private Unsealable()
	{
	}

	// Token: 0x060037EC RID: 14316 RVA: 0x000C8B63 File Offset: 0x000C6D63
	public override CellOffset[] GetOffsets(int cell)
	{
		if (this.facingRight)
		{
			return OffsetGroups.RightOnly;
		}
		return OffsetGroups.LeftOnly;
	}

	// Token: 0x060037ED RID: 14317 RVA: 0x000C8B78 File Offset: 0x000C6D78
	protected override void OnPrefabInit()
	{
		this.faceTargetWhenWorking = true;
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_door_poi_kanim")
		};
	}

	// Token: 0x060037EE RID: 14318 RVA: 0x00226D18 File Offset: 0x00224F18
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(3f);
		if (this.unsealed)
		{
			Deconstructable component = base.GetComponent<Deconstructable>();
			if (component != null)
			{
				component.allowDeconstruction = true;
			}
		}
	}

	// Token: 0x060037EF RID: 14319 RVA: 0x000AF929 File Offset: 0x000ADB29
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x060037F0 RID: 14320 RVA: 0x00226D58 File Offset: 0x00224F58
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.unsealed = true;
		base.OnCompleteWork(worker);
		Deconstructable component = base.GetComponent<Deconstructable>();
		if (component != null)
		{
			component.allowDeconstruction = true;
			Game.Instance.Trigger(1980521255, base.gameObject);
		}
	}

	// Token: 0x04002681 RID: 9857
	[Serialize]
	public bool facingRight;

	// Token: 0x04002682 RID: 9858
	[Serialize]
	public bool unsealed;
}
