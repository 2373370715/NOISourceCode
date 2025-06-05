using System;

// Token: 0x02000AF4 RID: 2804
public class Painting : Artable
{
	// Token: 0x060033A0 RID: 13216 RVA: 0x000C629F File Offset: 0x000C449F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.multitoolContext = "paint";
		this.multitoolHitEffectTag = "fx_paint_splash";
	}

	// Token: 0x060033A1 RID: 13217 RVA: 0x000C62D2 File Offset: 0x000C44D2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Paintings.Add(this);
	}

	// Token: 0x060033A2 RID: 13218 RVA: 0x000C62E5 File Offset: 0x000C44E5
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Paintings.Remove(this);
	}

	// Token: 0x060033A3 RID: 13219 RVA: 0x000C62F8 File Offset: 0x000C44F8
	public override void SetStage(string stage_id, bool skip_effect)
	{
		base.SetStage(stage_id, skip_effect);
		if (Db.GetArtableStages().Get(stage_id) == null)
		{
			Debug.LogError("Missing stage: " + stage_id);
		}
	}
}
