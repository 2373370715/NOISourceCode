using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020016D5 RID: 5845
[AddComponentMenu("KMonoBehaviour/scripts/OrbitalObject")]
[SerializationConfig(MemberSerialization.OptIn)]
public class OrbitalObject : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06007899 RID: 30873 RVA: 0x00320184 File Offset: 0x0031E384
	public void Init(string orbit_data_name, WorldContainer orbiting_world, List<Ref<OrbitalObject>> orbiting_obj)
	{
		OrbitalData orbitalData = Db.Get().OrbitalTypeCategories.Get(orbit_data_name);
		if (orbiting_world != null)
		{
			this.orbitingWorldId = orbiting_world.id;
			this.world = orbiting_world;
			this.worldOrbitingOrigin = this.GetWorldOrigin(this.world, orbitalData);
		}
		else
		{
			this.worldOrbitingOrigin = new Vector3((float)Grid.WidthInCells * 0.5f, (float)Grid.HeightInCells * orbitalData.yGridPercent, 0f);
		}
		this.animFilename = orbitalData.animFile;
		this.initialAnim = this.GetInitialAnim(orbitalData);
		this.angle = this.GetAngle(orbitalData);
		this.timeoffset = this.GetTimeOffset(orbiting_obj);
		this.orbitalDBId = orbitalData.Id;
	}

	// Token: 0x0600789A RID: 30874 RVA: 0x0032023C File Offset: 0x0031E43C
	protected override void OnSpawn()
	{
		this.world = ClusterManager.Instance.GetWorld(this.orbitingWorldId);
		this.orbitData = Db.Get().OrbitalTypeCategories.Get(this.orbitalDBId);
		base.gameObject.SetActive(false);
		KBatchedAnimController kbatchedAnimController = base.gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialAnim = this.initialAnim;
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(this.animFilename)
		};
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
		this.animController = kbatchedAnimController;
	}

	// Token: 0x0600789B RID: 30875 RVA: 0x003202DC File Offset: 0x0031E4DC
	public void RenderEveryTick(float dt)
	{
		float time = GameClock.Instance.GetTime();
		bool flag;
		Vector3 vector = this.CalculateWorldPos(time, out flag);
		Vector3 vector2 = vector;
		if (this.orbitData.periodInCycles > 0f)
		{
			vector2.x = vector.x / (float)Grid.WidthInCells;
			vector2.y = vector.y / (float)Grid.HeightInCells;
			vector2.x = Camera.main.ViewportToWorldPoint(vector2).x;
			vector2.y = Camera.main.ViewportToWorldPoint(vector2).y;
		}
		bool flag2 = (!this.orbitData.rotatesBehind || !flag) && (this.world == null || ClusterManager.Instance.activeWorldId == this.world.id);
		Vector3 offset = vector2 - base.gameObject.transform.position;
		offset.z = 0f;
		this.animController.Offset = offset;
		Vector3 position = vector2;
		position.x = this.worldOrbitingOrigin.x;
		position.y = this.worldOrbitingOrigin.y;
		base.gameObject.transform.SetPosition(position);
		if (this.orbitData.periodInCycles > 0f)
		{
			base.gameObject.transform.localScale = Vector3.one * (CameraController.Instance.baseCamera.orthographicSize / this.orbitData.distance);
		}
		else
		{
			base.gameObject.transform.localScale = Vector3.one * this.orbitData.distance;
		}
		if (base.gameObject.activeSelf != flag2)
		{
			base.gameObject.SetActive(flag2);
		}
	}

	// Token: 0x0600789C RID: 30876 RVA: 0x0032049C File Offset: 0x0031E69C
	private Vector3 CalculateWorldPos(float time, out bool behind)
	{
		Vector3 result;
		if (this.orbitData.periodInCycles > 0f)
		{
			float num = this.orbitData.periodInCycles * 600f;
			float f = ((time + (float)this.timeoffset) / num - (float)((int)((time + (float)this.timeoffset) / num))) * 2f * 3.1415927f;
			float d = 0.5f * this.orbitData.radiusScale * (float)this.world.WorldSize.x;
			Vector3 vector = new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f));
			behind = (vector.z > this.orbitData.behindZ);
			Vector3 b = Quaternion.Euler(this.angle, 0f, 0f) * (vector * d);
			result = this.worldOrbitingOrigin + b;
			result.z = ((this.orbitData.GetRenderZ == null) ? this.orbitData.renderZ : this.orbitData.GetRenderZ());
		}
		else
		{
			behind = false;
			result = this.worldOrbitingOrigin;
			result.z = ((this.orbitData.GetRenderZ == null) ? this.orbitData.renderZ : this.orbitData.GetRenderZ());
		}
		return result;
	}

	// Token: 0x0600789D RID: 30877 RVA: 0x003205EC File Offset: 0x0031E7EC
	private string GetInitialAnim(OrbitalData data)
	{
		if (data.initialAnim.IsNullOrWhiteSpace())
		{
			KAnimFileData data2 = Assets.GetAnim(data.animFile).GetData();
			int index = new KRandom().Next(0, data2.animCount - 1);
			return data2.GetAnim(index).name;
		}
		return data.initialAnim;
	}

	// Token: 0x0600789E RID: 30878 RVA: 0x00320644 File Offset: 0x0031E844
	private Vector3 GetWorldOrigin(WorldContainer wc, OrbitalData data)
	{
		if (wc != null)
		{
			float x = (float)wc.WorldOffset.x + (float)wc.WorldSize.x * data.xGridPercent;
			float y = (float)wc.WorldOffset.y + (float)wc.WorldSize.y * data.yGridPercent;
			return new Vector3(x, y, 0f);
		}
		return new Vector3((float)Grid.WidthInCells * data.xGridPercent, (float)Grid.HeightInCells * data.yGridPercent, 0f);
	}

	// Token: 0x0600789F RID: 30879 RVA: 0x000F3CCF File Offset: 0x000F1ECF
	private float GetAngle(OrbitalData data)
	{
		return UnityEngine.Random.Range(data.minAngle, data.maxAngle);
	}

	// Token: 0x060078A0 RID: 30880 RVA: 0x003206CC File Offset: 0x0031E8CC
	private int GetTimeOffset(List<Ref<OrbitalObject>> orbiting_obj)
	{
		List<int> list = new List<int>();
		foreach (Ref<OrbitalObject> @ref in orbiting_obj)
		{
			if (@ref.Get().world == this.world)
			{
				list.Add(@ref.Get().timeoffset);
			}
		}
		int num = UnityEngine.Random.Range(0, 600);
		while (list.Contains(num))
		{
			num = UnityEngine.Random.Range(0, 600);
		}
		return num;
	}

	// Token: 0x04005A94 RID: 23188
	private WorldContainer world;

	// Token: 0x04005A95 RID: 23189
	private OrbitalData orbitData;

	// Token: 0x04005A96 RID: 23190
	private KBatchedAnimController animController;

	// Token: 0x04005A97 RID: 23191
	[Serialize]
	private string animFilename;

	// Token: 0x04005A98 RID: 23192
	[Serialize]
	private string initialAnim;

	// Token: 0x04005A99 RID: 23193
	[Serialize]
	private Vector3 worldOrbitingOrigin;

	// Token: 0x04005A9A RID: 23194
	[Serialize]
	private int orbitingWorldId;

	// Token: 0x04005A9B RID: 23195
	[Serialize]
	private float angle;

	// Token: 0x04005A9C RID: 23196
	[Serialize]
	public int timeoffset;

	// Token: 0x04005A9D RID: 23197
	[Serialize]
	public string orbitalDBId;
}
