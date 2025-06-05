using System;
using System.Collections.Generic;
using FMOD.Studio;
using KSerialization;
using UnityEngine;

// Token: 0x02001520 RID: 5408
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MiniComet")]
public class MiniComet : KMonoBehaviour, ISim33ms
{
	// Token: 0x17000730 RID: 1840
	// (get) Token: 0x06007052 RID: 28754 RVA: 0x000EDFDB File Offset: 0x000EC1DB
	public Vector3 TargetPosition
	{
		get
		{
			return this.anim.PositionIncludingOffset;
		}
	}

	// Token: 0x17000731 RID: 1841
	// (get) Token: 0x06007053 RID: 28755 RVA: 0x000EDFE8 File Offset: 0x000EC1E8
	// (set) Token: 0x06007054 RID: 28756 RVA: 0x000EDFF0 File Offset: 0x000EC1F0
	public Vector2 Velocity
	{
		get
		{
			return this.velocity;
		}
		set
		{
			this.velocity = value;
		}
	}

	// Token: 0x06007055 RID: 28757 RVA: 0x00304860 File Offset: 0x00302A60
	private float GetVolume(GameObject gameObject)
	{
		float result = 1f;
		if (gameObject != null && this.selectable != null && this.selectable.IsSelected)
		{
			result = 1f;
		}
		return result;
	}

	// Token: 0x06007056 RID: 28758 RVA: 0x000EDFF9 File Offset: 0x000EC1F9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.loopingSounds = base.gameObject.GetComponent<LoopingSounds>();
		this.flyingSound = GlobalAssets.GetSound("Meteor_LP", false);
		this.RandomizeVelocity();
	}

	// Token: 0x06007057 RID: 28759 RVA: 0x003048A0 File Offset: 0x00302AA0
	protected override void OnSpawn()
	{
		this.anim.Offset = this.offsetPosition;
		if (this.spawnWithOffset)
		{
			this.SetupOffset();
		}
		base.OnSpawn();
		this.StartLoopingSound();
		bool flag = this.offsetPosition.x != 0f || this.offsetPosition.y != 0f;
		this.selectable.enabled = !flag;
		this.typeID = base.GetComponent<KPrefabID>().PrefabTag;
	}

	// Token: 0x06007058 RID: 28760 RVA: 0x000C4795 File Offset: 0x000C2995
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06007059 RID: 28761 RVA: 0x00304924 File Offset: 0x00302B24
	protected void SetupOffset()
	{
		Vector3 position = base.transform.GetPosition();
		Vector3 position2 = base.transform.GetPosition();
		position2.z = 0f;
		Vector3 vector = new Vector3(this.velocity.x, this.velocity.y, 0f);
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		float num = (float)(myWorld.WorldOffset.y + myWorld.Height + MissileLauncher.Def.launchRange.y) * Grid.CellSizeInMeters - position2.y;
		float f = Vector3.Angle(Vector3.up, -vector) * 0.017453292f;
		float d = Mathf.Abs(num / Mathf.Cos(f));
		Vector3 vector2 = position2 - vector.normalized * d;
		float num2 = (float)(myWorld.WorldOffset.x + myWorld.Width) * Grid.CellSizeInMeters;
		if (vector2.x < (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters || vector2.x > num2)
		{
			float num3 = (vector.x < 0f) ? (num2 - position2.x) : (position2.x - (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters);
			f = Vector3.Angle((vector.x < 0f) ? Vector3.right : Vector3.left, -vector) * 0.017453292f;
			d = Mathf.Abs(num3 / Mathf.Cos(f));
		}
		Vector3 b = -vector.normalized * d;
		(position2 + b).z = position.z;
		this.offsetPosition = b;
		this.anim.Offset = this.offsetPosition;
	}

	// Token: 0x0600705A RID: 28762 RVA: 0x00304AE8 File Offset: 0x00302CE8
	public virtual void RandomizeVelocity()
	{
		float num = UnityEngine.Random.Range(this.spawnAngle.x, this.spawnAngle.y);
		float f = num * 3.1415927f / 180f;
		float num2 = UnityEngine.Random.Range(this.spawnVelocity.x, this.spawnVelocity.y);
		this.velocity = new Vector2(-Mathf.Cos(f) * num2, Mathf.Sin(f) * num2);
		base.GetComponent<KBatchedAnimController>().Rotation = -num - 90f;
	}

	// Token: 0x0600705B RID: 28763 RVA: 0x000EE029 File Offset: 0x000EC229
	public int GetRandomNumOres()
	{
		return UnityEngine.Random.Range(this.explosionOreCount.x, this.explosionOreCount.y + 1);
	}

	// Token: 0x0600705C RID: 28764 RVA: 0x00304B6C File Offset: 0x00302D6C
	[ContextMenu("Explode")]
	private void Explode(Vector3 pos, int cell, int prev_cell, Element element)
	{
		byte b = Grid.WorldIdx[cell];
		this.PlayImpactSound(pos);
		Vector3 vector = pos;
		vector.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
		if (this.explosionEffectHash != SpawnFXHashes.None)
		{
			Game.Instance.SpawnFX(this.explosionEffectHash, vector, 0f);
		}
		if (element != null)
		{
			Substance substance = element.substance;
			int randomNumOres = this.GetRandomNumOres();
			Vector2 vector2 = -this.velocity.normalized;
			Vector2 a = new Vector2(vector2.y, -vector2.x);
			float mass = (randomNumOres > 0) ? (this.pe.Mass / (float)randomNumOres) : 1f;
			for (int i = 0; i < randomNumOres; i++)
			{
				Vector2 normalized = (vector2 + a * UnityEngine.Random.Range(-1f, 1f)).normalized;
				Vector3 v = normalized * UnityEngine.Random.Range(this.explosionSpeedRange.x, this.explosionSpeedRange.y);
				Vector3 position = vector + normalized.normalized * 1.25f;
				GameObject go = substance.SpawnResource(position, mass, this.pe.Temperature, this.pe.DiseaseIdx, this.pe.DiseaseCount / randomNumOres, false, false, false);
				if (GameComps.Fallers.Has(go))
				{
					GameComps.Fallers.Remove(go);
				}
				GameComps.Fallers.Add(go, v);
			}
		}
		if (this.OnImpact != null)
		{
			this.OnImpact();
		}
	}

	// Token: 0x0600705D RID: 28765 RVA: 0x00304D04 File Offset: 0x00302F04
	public float GetDistanceFromImpact()
	{
		float num = this.velocity.x / this.velocity.y;
		Vector3 position = base.transform.GetPosition();
		float num2 = 0f;
		while (num2 > -6f)
		{
			num2 -= 1f;
			num2 = Mathf.Ceil(position.y + num2) - 0.2f - position.y;
			float x = num2 * num;
			Vector3 b = new Vector3(x, num2, 0f);
			int num3 = Grid.PosToCell(position + b);
			if (Grid.IsValidCell(num3) && Grid.Solid[num3])
			{
				return b.magnitude;
			}
		}
		return 6f;
	}

	// Token: 0x0600705E RID: 28766 RVA: 0x000EE048 File Offset: 0x000EC248
	public float GetSoundDistance()
	{
		return this.GetDistanceFromImpact();
	}

	// Token: 0x0600705F RID: 28767 RVA: 0x00304DB0 File Offset: 0x00302FB0
	public void Sim33ms(float dt)
	{
		if (this.hasExploded)
		{
			return;
		}
		if (this.offsetPosition.y > 0f)
		{
			Vector3 b = new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0f);
			Vector3 vector = this.offsetPosition + b;
			this.offsetPosition = vector;
			this.anim.Offset = this.offsetPosition;
		}
		else
		{
			if (this.anim.Offset != Vector3.zero)
			{
				this.anim.Offset = Vector3.zero;
			}
			if (!this.selectable.enabled)
			{
				this.selectable.enabled = true;
			}
			Vector2 vector2 = new Vector2((float)Grid.WidthInCells, (float)Grid.HeightInCells) * -0.1f;
			Vector2 vector3 = new Vector2((float)Grid.WidthInCells, (float)Grid.HeightInCells) * 1.1f;
			Vector3 position = base.transform.GetPosition();
			Vector3 vector4 = position + new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0f);
			Grid.PosToCell(vector4);
			this.loopingSounds.UpdateVelocity(this.flyingSound, vector4 - position);
			if (vector4.x < vector2.x || vector3.x < vector4.x || vector4.y < vector2.y)
			{
				global::Util.KDestroyGameObject(base.gameObject);
			}
			int num = Grid.PosToCell(this);
			int num2 = Grid.PosToCell(this.previousPosition);
			if (num != num2 && Grid.IsValidCell(num) && Grid.Solid[num])
			{
				PrimaryElement component = base.GetComponent<PrimaryElement>();
				this.Explode(position, num, num2, component.Element);
				this.hasExploded = true;
				global::Util.KDestroyGameObject(base.gameObject);
				return;
			}
			this.previousPosition = position;
			base.transform.SetPosition(vector4);
		}
		this.age += dt;
	}

	// Token: 0x06007060 RID: 28768 RVA: 0x00304FC0 File Offset: 0x003031C0
	private void PlayImpactSound(Vector3 pos)
	{
		if (this.impactSound == null)
		{
			this.impactSound = "Meteor_Large_Impact";
		}
		this.loopingSounds.StopSound(this.flyingSound);
		string sound = GlobalAssets.GetSound(this.impactSound, false);
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId)
		{
			float volume = this.GetVolume(base.gameObject);
			pos.z = 0f;
			EventInstance instance = KFMOD.BeginOneShot(sound, pos, volume);
			instance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"), false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x06007061 RID: 28769 RVA: 0x000EE050 File Offset: 0x000EC250
	private void StartLoopingSound()
	{
		this.loopingSounds.StartSound(this.flyingSound);
		this.loopingSounds.UpdateFirstParameter(this.flyingSound, this.FLYING_SOUND_ID_PARAMETER, (float)this.flyingSoundID);
	}

	// Token: 0x06007062 RID: 28770 RVA: 0x00305064 File Offset: 0x00303264
	public void Explode()
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Vector3 position = base.transform.GetPosition();
		int num = Grid.PosToCell(position);
		this.Explode(position, num, num, component.Element);
		this.hasExploded = true;
		global::Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x04005469 RID: 21609
	[MyCmpGet]
	private PrimaryElement pe;

	// Token: 0x0400546A RID: 21610
	public Vector2 spawnVelocity = new Vector2(7f, 9f);

	// Token: 0x0400546B RID: 21611
	public Vector2 spawnAngle = new Vector2(30f, 150f);

	// Token: 0x0400546C RID: 21612
	public SpawnFXHashes explosionEffectHash;

	// Token: 0x0400546D RID: 21613
	public int addDiseaseCount;

	// Token: 0x0400546E RID: 21614
	public byte diseaseIdx = byte.MaxValue;

	// Token: 0x0400546F RID: 21615
	public Vector2I explosionOreCount = new Vector2I(1, 1);

	// Token: 0x04005470 RID: 21616
	public Vector2 explosionSpeedRange = new Vector2(0f, 0f);

	// Token: 0x04005471 RID: 21617
	public string impactSound;

	// Token: 0x04005472 RID: 21618
	public string flyingSound;

	// Token: 0x04005473 RID: 21619
	public int flyingSoundID;

	// Token: 0x04005474 RID: 21620
	private HashedString FLYING_SOUND_ID_PARAMETER = "meteorType";

	// Token: 0x04005475 RID: 21621
	public bool Targeted;

	// Token: 0x04005476 RID: 21622
	[Serialize]
	protected Vector3 offsetPosition;

	// Token: 0x04005477 RID: 21623
	[Serialize]
	protected Vector2 velocity;

	// Token: 0x04005478 RID: 21624
	private Vector3 previousPosition;

	// Token: 0x04005479 RID: 21625
	private bool hasExploded;

	// Token: 0x0400547A RID: 21626
	public string[] craterPrefabs;

	// Token: 0x0400547B RID: 21627
	public bool spawnWithOffset;

	// Token: 0x0400547C RID: 21628
	private float age;

	// Token: 0x0400547D RID: 21629
	public System.Action OnImpact;

	// Token: 0x0400547E RID: 21630
	public Ref<KPrefabID> ignoreObstacleForDamage = new Ref<KPrefabID>();

	// Token: 0x0400547F RID: 21631
	[MyCmpGet]
	private KBatchedAnimController anim;

	// Token: 0x04005480 RID: 21632
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04005481 RID: 21633
	public Tag typeID;

	// Token: 0x04005482 RID: 21634
	private LoopingSounds loopingSounds;

	// Token: 0x04005483 RID: 21635
	private List<GameObject> damagedEntities = new List<GameObject>();

	// Token: 0x04005484 RID: 21636
	private List<int> destroyedCells = new List<int>();

	// Token: 0x04005485 RID: 21637
	private const float MAX_DISTANCE_TEST = 6f;
}
