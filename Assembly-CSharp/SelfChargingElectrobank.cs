using System;
using KSerialization;
using UnityEngine;

// Token: 0x020018AE RID: 6318
public class SelfChargingElectrobank : Electrobank
{
	// Token: 0x1700084F RID: 2127
	// (get) Token: 0x06008283 RID: 33411 RVA: 0x000FA513 File Offset: 0x000F8713
	public float LifetimeRemaining
	{
		get
		{
			return this.lifetimeRemaining;
		}
	}

	// Token: 0x06008284 RID: 33412 RVA: 0x0034A9EC File Offset: 0x00348BEC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.selectable = base.GetComponent<KSelectable>();
		this.selectable.AddStatusItem(Db.Get().MiscStatusItems.ElectrobankSelfCharging, 60f);
		this.lifetimeStatus = this.selectable.AddStatusItem(Db.Get().MiscStatusItems.ElectrobankLifetimeRemaining, this);
		Components.SelfChargingElectrobanks.Add(base.gameObject.GetMyWorldId(), this);
		if (this.lifetimeRemaining <= 0f)
		{
			this.Delete();
		}
	}

	// Token: 0x06008285 RID: 33413 RVA: 0x000FA51B File Offset: 0x000F871B
	public override void Sim200ms(float dt)
	{
		base.Sim200ms(dt);
		if (this.lifetimeRemaining > 0f)
		{
			base.AddPower(dt * 60f);
			this.lifetimeRemaining -= dt;
			return;
		}
		this.Explode();
	}

	// Token: 0x06008286 RID: 33414 RVA: 0x0034AA7C File Offset: 0x00348C7C
	public override void Explode()
	{
		Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactMetal, base.gameObject.transform.position, 0f);
		KFMOD.PlayOneShot(GlobalAssets.GetSound("Battery_explode", false), base.gameObject.transform.position, 1f);
		base.LaunchNearbyStuff();
		SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.position), SimHashes.NuclearWaste, CellEventLogger.Instance.ElementEmitted, 10f, 3000f, Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id), Mathf.RoundToInt(5000000f), true, -1);
		if (base.transform.parent != null)
		{
			Storage component = base.transform.parent.GetComponent<Storage>();
			if (component != null)
			{
				Health component2 = component.GetComponent<Health>();
				if (component2 != null)
				{
					component2.Damage(500f);
				}
			}
		}
		this.Delete();
	}

	// Token: 0x06008287 RID: 33415 RVA: 0x000FA554 File Offset: 0x000F8754
	private void Delete()
	{
		if (!this.IsNullOrDestroyed() && !base.gameObject.IsNullOrDestroyed())
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x06008288 RID: 33416 RVA: 0x000FA576 File Offset: 0x000F8776
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.SelfChargingElectrobanks.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x04006355 RID: 25429
	[Serialize]
	private float lifetimeRemaining = 90000f;

	// Token: 0x04006356 RID: 25430
	private KSelectable selectable;

	// Token: 0x04006357 RID: 25431
	private Guid lifetimeStatus = Guid.Empty;
}
