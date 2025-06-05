using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000254 RID: 596
public class MorbRoverConfig : IEntityConfig
{
	// Token: 0x06000867 RID: 2151 RVA: 0x0016BCB0 File Offset: 0x00169EB0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseRoverConfig.BaseRover("MorbRover", STRINGS.ROBOTS.MODELS.MORB.NAME, GameTags.Robots.Models.MorbRover, STRINGS.ROBOTS.MODELS.MORB.DESC, "morbRover_kanim", 300f, 1f, 2f, TUNING.ROBOTS.MORBBOT.CARRY_CAPACITY, 1f, 1f, 3f, TUNING.ROBOTS.MORBBOT.HIT_POINTS, 180000f, 30f, Db.Get().Amounts.InternalBioBattery, false);
		gameObject.GetComponent<PrimaryElement>().SetElement(SimHashes.Steel, false);
		gameObject.GetComponent<Deconstructable>().customWorkTime = 10f;
		return gameObject;
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x000AE2FC File Offset: 0x000AC4FC
	public void OnPrefabInit(GameObject inst)
	{
		BaseRoverConfig.OnPrefabInit(inst, Db.Get().Amounts.InternalBioBattery);
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x000AE313 File Offset: 0x000AC513
	public void OnSpawn(GameObject inst)
	{
		BaseRoverConfig.OnSpawn(inst);
		inst.Subscribe(1623392196, new Action<object>(this.TriggerDeconstructChoreOnDeath));
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x0016BD48 File Offset: 0x00169F48
	public void TriggerDeconstructChoreOnDeath(object obj)
	{
		if (obj != null)
		{
			Deconstructable component = ((GameObject)obj).GetComponent<Deconstructable>();
			if (!component.IsMarkedForDeconstruction())
			{
				component.QueueDeconstruction(false);
			}
		}
	}

	// Token: 0x04000663 RID: 1635
	public const string ID = "MorbRover";

	// Token: 0x04000664 RID: 1636
	public const SimHashes MATERIAL = SimHashes.Steel;

	// Token: 0x04000665 RID: 1637
	public const float MASS = 300f;

	// Token: 0x04000666 RID: 1638
	private const float WIDTH = 1f;

	// Token: 0x04000667 RID: 1639
	private const float HEIGHT = 2f;
}
