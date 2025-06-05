using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using STRINGS;
using UnityEngine;

// Token: 0x02001AA3 RID: 6819
[AddComponentMenu("KMonoBehaviour/scripts/WorldDamage")]
public class WorldDamage : KMonoBehaviour
{
	// Token: 0x17000966 RID: 2406
	// (get) Token: 0x06008E94 RID: 36500 RVA: 0x00101A1B File Offset: 0x000FFC1B
	// (set) Token: 0x06008E95 RID: 36501 RVA: 0x00101A22 File Offset: 0x000FFC22
	public static WorldDamage Instance { get; private set; }

	// Token: 0x06008E96 RID: 36502 RVA: 0x00101A2A File Offset: 0x000FFC2A
	public static void DestroyInstance()
	{
		WorldDamage.Instance = null;
	}

	// Token: 0x06008E97 RID: 36503 RVA: 0x00101A32 File Offset: 0x000FFC32
	protected override void OnPrefabInit()
	{
		WorldDamage.Instance = this;
	}

	// Token: 0x06008E98 RID: 36504 RVA: 0x00101A3A File Offset: 0x000FFC3A
	public void RestoreDamageToValue(int cell, float amount)
	{
		if (Grid.Damage[cell] > amount)
		{
			Grid.Damage[cell] = amount;
		}
	}

	// Token: 0x06008E99 RID: 36505 RVA: 0x00101A4E File Offset: 0x000FFC4E
	public float ApplyDamage(Sim.WorldDamageInfo damage_info)
	{
		return this.ApplyDamage(damage_info.gameCell, this.damageAmount, damage_info.damageSourceOffset, BUILDINGS.DAMAGESOURCES.LIQUID_PRESSURE, UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.LIQUID_PRESSURE);
	}

	// Token: 0x06008E9A RID: 36506 RVA: 0x0037A878 File Offset: 0x00378A78
	public float ApplyDamage(int cell, float amount, int src_cell, WorldDamage.DamageType damageType, string source_name = null, string pop_text = null)
	{
		float result = 0f;
		if (Grid.Solid[cell])
		{
			float num = Grid.Damage[cell];
			result = Mathf.Min(amount, 1f - num);
			num += amount;
			bool flag = num > 0.15f;
			if (flag && damageType != WorldDamage.DamageType.NoBuildingDamage)
			{
				GameObject gameObject = Grid.Objects[cell, 9];
				if (gameObject != null)
				{
					BuildingHP component = gameObject.GetComponent<BuildingHP>();
					if (component != null)
					{
						if (!component.invincible)
						{
							int damage = Mathf.RoundToInt(Mathf.Max((float)component.HitPoints - (1f - num) * (float)component.MaxHitPoints, 0f));
							gameObject.Trigger(-794517298, new BuildingHP.DamageSourceInfo
							{
								damage = damage,
								source = source_name,
								popString = pop_text
							});
						}
						else
						{
							num = 0f;
						}
					}
				}
			}
			Grid.Damage[cell] = Mathf.Min(1f, num);
			if (Grid.Damage[cell] >= 1f)
			{
				this.DestroyCell(cell);
			}
			else if (Grid.IsValidCell(src_cell) && flag)
			{
				Element element = Grid.Element[src_cell];
				if (element.IsLiquid && Grid.Mass[src_cell] > 1f)
				{
					int num2 = cell - src_cell;
					if (num2 == 1 || num2 == -1 || num2 == Grid.WidthInCells || num2 == -Grid.WidthInCells)
					{
						int num3 = cell + num2;
						if (Grid.IsValidCell(num3))
						{
							Element element2 = Grid.Element[num3];
							if (!element2.IsSolid && (!element2.IsLiquid || (element2.id == element.id && Grid.Mass[num3] <= 100f)) && (Grid.Properties[num3] & 2) == 0 && !this.spawnTimes.ContainsKey(num3))
							{
								this.spawnTimes[num3] = Time.realtimeSinceStartup;
								ushort idx = element.idx;
								float temperature = Grid.Temperature[src_cell];
								base.StartCoroutine(this.DelayedSpawnFX(src_cell, num3, num2, element, idx, temperature));
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06008E9B RID: 36507 RVA: 0x00101A7C File Offset: 0x000FFC7C
	public float ApplyDamage(int cell, float amount, int src_cell, string source_name = null, string pop_text = null)
	{
		return this.ApplyDamage(cell, amount, src_cell, WorldDamage.DamageType.Absolute, source_name, pop_text);
	}

	// Token: 0x06008E9C RID: 36508 RVA: 0x00101A8C File Offset: 0x000FFC8C
	private void ReleaseGO(GameObject go)
	{
		go.DeleteObject();
	}

	// Token: 0x06008E9D RID: 36509 RVA: 0x00101A94 File Offset: 0x000FFC94
	private IEnumerator DelayedSpawnFX(int src_cell, int dest_cell, int offset, Element elem, ushort idx, float temperature)
	{
		float seconds = UnityEngine.Random.value * 0.25f;
		yield return new WaitForSeconds(seconds);
		Vector3 position = Grid.CellToPosCCC(dest_cell, Grid.SceneLayer.Front);
		GameObject gameObject = GameUtil.KInstantiate(this.leakEffect.gameObject, position, Grid.SceneLayer.Front, null, 0);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		component.TintColour = elem.substance.colour;
		component.onDestroySelf = new Action<GameObject>(this.ReleaseGO);
		SimMessages.AddRemoveSubstance(src_cell, idx, CellEventLogger.Instance.WorldDamageDelayedSpawnFX, -1f, temperature, byte.MaxValue, 0, true, -1);
		if (offset == -1)
		{
			component.Play("side", KAnim.PlayMode.Once, 1f, 0f);
			component.FlipX = true;
			component.enabled = false;
			component.enabled = true;
			gameObject.transform.SetPosition(gameObject.transform.GetPosition() + Vector3.right * 0.5f);
			FallingWater.instance.AddParticle(dest_cell, idx, 1f, temperature, byte.MaxValue, 0, true, false, false, false);
		}
		else if (offset == Grid.WidthInCells)
		{
			gameObject.transform.SetPosition(gameObject.transform.GetPosition() - Vector3.up * 0.5f);
			component.Play("floor", KAnim.PlayMode.Once, 1f, 0f);
			component.enabled = false;
			component.enabled = true;
			SimMessages.AddRemoveSubstance(dest_cell, idx, CellEventLogger.Instance.WorldDamageDelayedSpawnFX, 1f, temperature, byte.MaxValue, 0, true, -1);
		}
		else if (offset == -Grid.WidthInCells)
		{
			component.Play("ceiling", KAnim.PlayMode.Once, 1f, 0f);
			component.enabled = false;
			component.enabled = true;
			gameObject.transform.SetPosition(gameObject.transform.GetPosition() + Vector3.up * 0.5f);
			FallingWater.instance.AddParticle(dest_cell, idx, 1f, temperature, byte.MaxValue, 0, true, false, false, false);
		}
		else
		{
			component.Play("side", KAnim.PlayMode.Once, 1f, 0f);
			component.enabled = false;
			component.enabled = true;
			gameObject.transform.SetPosition(gameObject.transform.GetPosition() - Vector3.right * 0.5f);
			FallingWater.instance.AddParticle(dest_cell, idx, 1f, temperature, byte.MaxValue, 0, true, false, false, false);
		}
		if (CameraController.Instance.IsAudibleSound(gameObject.transform.GetPosition(), this.leakSoundMigrated))
		{
			SoundEvent.PlayOneShot(this.leakSoundMigrated, gameObject.transform.GetPosition(), 1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06008E9E RID: 36510 RVA: 0x0037AAA8 File Offset: 0x00378CA8
	private void Update()
	{
		this.expiredCells.Clear();
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		foreach (KeyValuePair<int, float> keyValuePair in this.spawnTimes)
		{
			if (realtimeSinceStartup - keyValuePair.Value > 1f)
			{
				this.expiredCells.Add(keyValuePair.Key);
			}
		}
		foreach (int key in this.expiredCells)
		{
			this.spawnTimes.Remove(key);
		}
		this.expiredCells.Clear();
	}

	// Token: 0x06008E9F RID: 36511 RVA: 0x00101AD0 File Offset: 0x000FFCD0
	public void DestroyCell(int cell)
	{
		if (Grid.Solid[cell])
		{
			SimMessages.Dig(cell, -1, false);
		}
	}

	// Token: 0x06008EA0 RID: 36512 RVA: 0x00101AE7 File Offset: 0x000FFCE7
	public void OnSolidStateChanged(int cell)
	{
		Grid.Damage[cell] = 0f;
	}

	// Token: 0x06008EA1 RID: 36513 RVA: 0x0037AB7C File Offset: 0x00378D7C
	public void OnDigComplete(int cell, float mass, float temperature, ushort element_idx, byte disease_idx, int disease_count)
	{
		Vector3 vector = Grid.CellToPos(cell, CellAlignment.RandomInternal, Grid.SceneLayer.Ore);
		Element element = ElementLoader.elements[(int)element_idx];
		Grid.Damage[cell] = 0f;
		WorldDamage.Instance.PlaySoundForSubstance(element, vector);
		float num = mass * 0.5f;
		if (num <= 0f)
		{
			return;
		}
		GameObject gameObject = element.substance.SpawnResource(vector, num, temperature, disease_idx, disease_count, false, false, false);
		Pickupable component = gameObject.GetComponent<Pickupable>();
		if (component != null && component.GetMyWorld() != null && component.GetMyWorld().worldInventory.IsReachable(component))
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, Mathf.RoundToInt(num).ToString() + " " + element.name, gameObject.transform, 1.5f, false);
		}
	}

	// Token: 0x06008EA2 RID: 36514 RVA: 0x0037AC58 File Offset: 0x00378E58
	private void PlaySoundForSubstance(Element element, Vector3 pos)
	{
		string text = element.substance.GetMiningBreakSound();
		if (text == null)
		{
			if (element.HasTag(GameTags.RefinedMetal))
			{
				text = "RefinedMetal";
			}
			else if (element.HasTag(GameTags.Metal))
			{
				text = "RawMetal";
			}
			else
			{
				text = "Rock";
			}
		}
		text = "Break_" + text;
		text = GlobalAssets.GetSound(text, false);
		if (CameraController.Instance && CameraController.Instance.IsAudibleSound(pos, text))
		{
			KFMOD.PlayOneShot(text, CameraController.Instance.GetVerticallyScaledPosition(pos, false), 1f);
		}
	}

	// Token: 0x04006B7C RID: 27516
	public KBatchedAnimController leakEffect;

	// Token: 0x04006B7D RID: 27517
	[SerializeField]
	private FMODAsset leakSound;

	// Token: 0x04006B7E RID: 27518
	[SerializeField]
	private EventReference leakSoundMigrated;

	// Token: 0x04006B7F RID: 27519
	private float damageAmount = 0.00083333335f;

	// Token: 0x04006B81 RID: 27521
	private const float SPAWN_DELAY = 1f;

	// Token: 0x04006B82 RID: 27522
	private Dictionary<int, float> spawnTimes = new Dictionary<int, float>();

	// Token: 0x04006B83 RID: 27523
	private List<int> expiredCells = new List<int>();

	// Token: 0x02001AA4 RID: 6820
	public enum DamageType
	{
		// Token: 0x04006B85 RID: 27525
		Absolute,
		// Token: 0x04006B86 RID: 27526
		NoBuildingDamage
	}
}
