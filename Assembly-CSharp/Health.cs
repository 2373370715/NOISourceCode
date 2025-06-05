using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x0200141F RID: 5151
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Health")]
public class Health : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170006BC RID: 1724
	// (get) Token: 0x06006988 RID: 27016 RVA: 0x000E99E2 File Offset: 0x000E7BE2
	// (set) Token: 0x06006989 RID: 27017 RVA: 0x000E99EA File Offset: 0x000E7BEA
	[Serialize]
	public Health.HealthState State { get; private set; }

	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x0600698A RID: 27018 RVA: 0x000E99F3 File Offset: 0x000E7BF3
	// (set) Token: 0x0600698B RID: 27019 RVA: 0x000E99FB File Offset: 0x000E7BFB
	[Serialize]
	public Tag CauseOfIncapacitation { get; private set; }

	// Token: 0x170006BE RID: 1726
	// (get) Token: 0x0600698C RID: 27020 RVA: 0x000E9A04 File Offset: 0x000E7C04
	public AmountInstance GetAmountInstance
	{
		get
		{
			return this.amountInstance;
		}
	}

	// Token: 0x170006BF RID: 1727
	// (get) Token: 0x0600698D RID: 27021 RVA: 0x000E9A0C File Offset: 0x000E7C0C
	// (set) Token: 0x0600698E RID: 27022 RVA: 0x000E9A19 File Offset: 0x000E7C19
	public float hitPoints
	{
		get
		{
			return this.amountInstance.value;
		}
		set
		{
			this.amountInstance.value = value;
		}
	}

	// Token: 0x170006C0 RID: 1728
	// (get) Token: 0x0600698F RID: 27023 RVA: 0x000E9A27 File Offset: 0x000E7C27
	public float maxHitPoints
	{
		get
		{
			return this.amountInstance.GetMax();
		}
	}

	// Token: 0x06006990 RID: 27024 RVA: 0x000E9A34 File Offset: 0x000E7C34
	public float percent()
	{
		return this.hitPoints / this.maxHitPoints;
	}

	// Token: 0x06006991 RID: 27025 RVA: 0x002E95E4 File Offset: 0x002E77E4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Health.Add(this);
		this.amountInstance = Db.Get().Amounts.HitPoints.Lookup(base.gameObject);
		this.amountInstance.value = this.amountInstance.GetMax();
		AmountInstance amountInstance = this.amountInstance;
		amountInstance.OnDelta = (Action<float>)Delegate.Combine(amountInstance.OnDelta, new Action<float>(this.OnHealthChanged));
	}

	// Token: 0x06006992 RID: 27026 RVA: 0x002E9660 File Offset: 0x002E7860
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.State == Health.HealthState.Incapacitated || this.hitPoints == 0f)
		{
			if (this.canBeIncapacitated)
			{
				this.Incapacitate(GameTags.HitPointsDepleted);
			}
			else
			{
				this.Kill();
			}
		}
		if (this.State != Health.HealthState.Incapacitated && this.State != Health.HealthState.Dead)
		{
			this.UpdateStatus();
		}
		this.effects = base.GetComponent<Effects>();
		this.UpdateHealthBar();
		this.UpdateWoundEffects();
	}

	// Token: 0x06006993 RID: 27027 RVA: 0x000E9A43 File Offset: 0x000E7C43
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Health.Remove(this);
	}

	// Token: 0x06006994 RID: 27028 RVA: 0x002E96D4 File Offset: 0x002E78D4
	public void UpdateHealthBar()
	{
		if (NameDisplayScreen.Instance == null)
		{
			return;
		}
		bool flag = this.State == Health.HealthState.Dead || this.State == Health.HealthState.Incapacitated || this.hitPoints >= this.maxHitPoints || base.gameObject.HasTag("HideHealthBar");
		NameDisplayScreen.Instance.SetHealthDisplay(base.gameObject, new Func<float>(this.percent), !flag);
	}

	// Token: 0x06006995 RID: 27029 RVA: 0x000E9A56 File Offset: 0x000E7C56
	private void OnRecover()
	{
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);
	}

	// Token: 0x06006996 RID: 27030 RVA: 0x002E9748 File Offset: 0x002E7948
	public void OnHealthChanged(float delta)
	{
		base.Trigger(-1664904872, delta);
		if (this.State != Health.HealthState.Invincible)
		{
			if (this.hitPoints == 0f && !this.IsDefeated())
			{
				if (this.canBeIncapacitated)
				{
					this.Incapacitate(GameTags.HitPointsDepleted);
				}
				else
				{
					this.Kill();
				}
			}
			else
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);
			}
		}
		this.UpdateStatus();
		this.UpdateWoundEffects();
		this.UpdateHealthBar();
	}

	// Token: 0x06006997 RID: 27031 RVA: 0x000E9A68 File Offset: 0x000E7C68
	[ContextMenu("DoDamage")]
	public void DoDamage()
	{
		this.Damage(1f);
	}

	// Token: 0x06006998 RID: 27032 RVA: 0x000E9A75 File Offset: 0x000E7C75
	public void Damage(float amount)
	{
		if (this.State != Health.HealthState.Invincible)
		{
			this.hitPoints = Mathf.Max(0f, this.hitPoints - amount);
		}
		this.OnHealthChanged(-amount);
	}

	// Token: 0x06006999 RID: 27033 RVA: 0x002E97C4 File Offset: 0x002E79C4
	private void UpdateWoundEffects()
	{
		if (!this.effects)
		{
			return;
		}
		if (this.isCritter != this.isCritterPrev)
		{
			if (this.isCritterPrev)
			{
				this.effects.Remove("LightWoundsCritter");
				this.effects.Remove("ModerateWoundsCritter");
				this.effects.Remove("SevereWoundsCritter");
			}
			else
			{
				this.effects.Remove("LightWounds");
				this.effects.Remove("ModerateWounds");
				this.effects.Remove("SevereWounds");
			}
			this.isCritterPrev = this.isCritter;
		}
		string effect_id;
		string effect_id2;
		string effect_id3;
		if (this.isCritter)
		{
			effect_id = "LightWoundsCritter";
			effect_id2 = "ModerateWoundsCritter";
			effect_id3 = "SevereWoundsCritter";
		}
		else
		{
			effect_id = "LightWounds";
			effect_id2 = "ModerateWounds";
			effect_id3 = "SevereWounds";
		}
		switch (this.State)
		{
		case Health.HealthState.Perfect:
		case Health.HealthState.Alright:
		case Health.HealthState.Incapacitated:
		case Health.HealthState.Dead:
			this.effects.Remove(effect_id);
			this.effects.Remove(effect_id2);
			this.effects.Remove(effect_id3);
			break;
		case Health.HealthState.Scuffed:
			if (!this.effects.HasEffect(effect_id))
			{
				this.effects.Add(effect_id, true);
			}
			this.effects.Remove(effect_id2);
			this.effects.Remove(effect_id3);
			return;
		case Health.HealthState.Injured:
			this.effects.Remove(effect_id);
			if (!this.effects.HasEffect(effect_id2))
			{
				this.effects.Add(effect_id2, true);
			}
			this.effects.Remove(effect_id3);
			return;
		case Health.HealthState.Critical:
			this.effects.Remove(effect_id);
			this.effects.Remove(effect_id2);
			if (!this.effects.HasEffect(effect_id3))
			{
				this.effects.Add(effect_id3, true);
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600699A RID: 27034 RVA: 0x002E9980 File Offset: 0x002E7B80
	private void UpdateStatus()
	{
		float num = this.hitPoints / this.maxHitPoints;
		Health.HealthState healthState;
		if (this.State == Health.HealthState.Invincible)
		{
			healthState = Health.HealthState.Invincible;
		}
		else if (num >= 1f)
		{
			healthState = Health.HealthState.Perfect;
		}
		else if (num >= 0.85f)
		{
			healthState = Health.HealthState.Alright;
		}
		else if (num >= 0.66f)
		{
			healthState = Health.HealthState.Scuffed;
		}
		else if ((double)num >= 0.33)
		{
			healthState = Health.HealthState.Injured;
		}
		else if (num > 0f)
		{
			healthState = Health.HealthState.Critical;
		}
		else if (num == 0f)
		{
			healthState = Health.HealthState.Incapacitated;
		}
		else
		{
			healthState = Health.HealthState.Dead;
		}
		if (this.State != healthState)
		{
			if (this.State == Health.HealthState.Incapacitated && healthState != Health.HealthState.Dead)
			{
				this.OnRecover();
			}
			if (healthState == Health.HealthState.Perfect)
			{
				base.Trigger(-1491582671, this);
			}
			this.State = healthState;
			KSelectable component = base.GetComponent<KSelectable>();
			if (this.State != Health.HealthState.Dead && this.State != Health.HealthState.Perfect && this.State != Health.HealthState.Alright && !this.isCritter)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, Db.Get().CreatureStatusItems.HealthStatus, this.State);
				return;
			}
			component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, null, null);
		}
	}

	// Token: 0x0600699B RID: 27035 RVA: 0x000E9AA0 File Offset: 0x000E7CA0
	public bool IsIncapacitated()
	{
		return this.State == Health.HealthState.Incapacitated;
	}

	// Token: 0x0600699C RID: 27036 RVA: 0x000E9AAB File Offset: 0x000E7CAB
	public bool IsDefeated()
	{
		return this.State == Health.HealthState.Incapacitated || this.State == Health.HealthState.Dead;
	}

	// Token: 0x0600699D RID: 27037 RVA: 0x000E9AC1 File Offset: 0x000E7CC1
	public void Incapacitate(Tag cause)
	{
		this.CauseOfIncapacitation = cause;
		this.State = Health.HealthState.Incapacitated;
		this.Damage(this.hitPoints);
		base.gameObject.Trigger(-1506500077, null);
	}

	// Token: 0x0600699E RID: 27038 RVA: 0x000E9AEE File Offset: 0x000E7CEE
	private void Kill()
	{
		if (base.gameObject.GetSMI<DeathMonitor.Instance>() != null)
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Slain);
		}
	}

	// Token: 0x04004FFA RID: 20474
	[Serialize]
	public bool canBeIncapacitated;

	// Token: 0x04004FFD RID: 20477
	public HealthBar healthBar;

	// Token: 0x04004FFE RID: 20478
	public bool isCritter;

	// Token: 0x04004FFF RID: 20479
	private bool isCritterPrev;

	// Token: 0x04005000 RID: 20480
	private Effects effects;

	// Token: 0x04005001 RID: 20481
	private AmountInstance amountInstance;

	// Token: 0x02001420 RID: 5152
	public enum HealthState
	{
		// Token: 0x04005003 RID: 20483
		Perfect,
		// Token: 0x04005004 RID: 20484
		Alright,
		// Token: 0x04005005 RID: 20485
		Scuffed,
		// Token: 0x04005006 RID: 20486
		Injured,
		// Token: 0x04005007 RID: 20487
		Critical,
		// Token: 0x04005008 RID: 20488
		Incapacitated,
		// Token: 0x04005009 RID: 20489
		Dead,
		// Token: 0x0400500A RID: 20490
		Invincible
	}
}
