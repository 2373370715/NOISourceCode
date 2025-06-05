using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200142B RID: 5163
public class HighEnergyParticleConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060069C9 RID: 27081 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060069CA RID: 27082 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060069CB RID: 27083 RVA: 0x002EA71C File Offset: 0x002E891C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity("HighEnergyParticle", ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, ITEMS.RADIATION.HIGHENERGYPARITCLE.DESC, 1f, false, Assets.GetAnim("spark_radial_high_energy_particles_kanim"), "travel_pre", Grid.SceneLayer.FXFront2, SimHashes.Creature, null, 293f);
		EntityTemplates.AddCollision(gameObject, EntityTemplates.CollisionShape.CIRCLE, 0.2f, 0.2f);
		gameObject.AddOrGet<LoopingSounds>();
		RadiationEmitter radiationEmitter = gameObject.AddOrGet<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emitRadiusX = 3;
		radiationEmitter.emitRadiusY = 3;
		radiationEmitter.emitRads = 0.4f * ((float)radiationEmitter.emitRadiusX / 6f);
		gameObject.AddComponent<HighEnergyParticle>().speed = 8f;
		return gameObject;
	}

	// Token: 0x060069CC RID: 27084 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060069CD RID: 27085 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400503B RID: 20539
	public const int PARTICLE_SPEED = 8;

	// Token: 0x0400503C RID: 20540
	public const float PARTICLE_COLLISION_SIZE = 0.2f;

	// Token: 0x0400503D RID: 20541
	public const float PER_CELL_FALLOFF = 0.1f;

	// Token: 0x0400503E RID: 20542
	public const float FALLOUT_RATIO = 0.5f;

	// Token: 0x0400503F RID: 20543
	public const int MAX_PAYLOAD = 500;

	// Token: 0x04005040 RID: 20544
	public const int EXPLOSION_FALLOUT_TEMPERATURE = 5000;

	// Token: 0x04005041 RID: 20545
	public const float EXPLOSION_FALLOUT_MASS_PER_PARTICLE = 0.001f;

	// Token: 0x04005042 RID: 20546
	public const float EXPLOSION_EMIT_DURRATION = 1f;

	// Token: 0x04005043 RID: 20547
	public const short EXPLOSION_EMIT_RADIUS = 6;

	// Token: 0x04005044 RID: 20548
	public const string ID = "HighEnergyParticle";
}
