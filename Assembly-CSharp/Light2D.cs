using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020017CB RID: 6091
[AddComponentMenu("KMonoBehaviour/scripts/Light2D")]
public class Light2D : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06007D27 RID: 32039 RVA: 0x000F6D5B File Offset: 0x000F4F5B
	private T MaybeDirty<T>(T old_value, T new_value, ref bool dirty)
	{
		if (!EqualityComparer<T>.Default.Equals(old_value, new_value))
		{
			dirty = true;
			return new_value;
		}
		return old_value;
	}

	// Token: 0x170007DB RID: 2011
	// (get) Token: 0x06007D28 RID: 32040 RVA: 0x000F6D71 File Offset: 0x000F4F71
	// (set) Token: 0x06007D29 RID: 32041 RVA: 0x000F6D7E File Offset: 0x000F4F7E
	public global::LightShape shape
	{
		get
		{
			return this.pending_emitter_state.shape;
		}
		set
		{
			this.pending_emitter_state.shape = this.MaybeDirty<global::LightShape>(this.pending_emitter_state.shape, value, ref this.dirty_shape);
		}
	}

	// Token: 0x170007DC RID: 2012
	// (get) Token: 0x06007D2A RID: 32042 RVA: 0x000F6DA3 File Offset: 0x000F4FA3
	// (set) Token: 0x06007D2B RID: 32043 RVA: 0x000F6DAB File Offset: 0x000F4FAB
	public LightGridManager.LightGridEmitter emitter { get; private set; }

	// Token: 0x170007DD RID: 2013
	// (get) Token: 0x06007D2C RID: 32044 RVA: 0x000F6DB4 File Offset: 0x000F4FB4
	// (set) Token: 0x06007D2D RID: 32045 RVA: 0x000F6DC1 File Offset: 0x000F4FC1
	public Color Color
	{
		get
		{
			return this.pending_emitter_state.colour;
		}
		set
		{
			this.pending_emitter_state.colour = value;
		}
	}

	// Token: 0x170007DE RID: 2014
	// (get) Token: 0x06007D2E RID: 32046 RVA: 0x000F6DCF File Offset: 0x000F4FCF
	// (set) Token: 0x06007D2F RID: 32047 RVA: 0x000F6DDC File Offset: 0x000F4FDC
	public int Lux
	{
		get
		{
			return this.pending_emitter_state.intensity;
		}
		set
		{
			this.pending_emitter_state.intensity = value;
		}
	}

	// Token: 0x170007DF RID: 2015
	// (get) Token: 0x06007D30 RID: 32048 RVA: 0x000F6DEA File Offset: 0x000F4FEA
	// (set) Token: 0x06007D31 RID: 32049 RVA: 0x000F6DF7 File Offset: 0x000F4FF7
	public DiscreteShadowCaster.Direction LightDirection
	{
		get
		{
			return this.pending_emitter_state.direction;
		}
		set
		{
			this.pending_emitter_state.direction = this.MaybeDirty<DiscreteShadowCaster.Direction>(this.pending_emitter_state.direction, value, ref this.dirty_shape);
		}
	}

	// Token: 0x170007E0 RID: 2016
	// (get) Token: 0x06007D32 RID: 32050 RVA: 0x000F6E1C File Offset: 0x000F501C
	// (set) Token: 0x06007D33 RID: 32051 RVA: 0x000F6E29 File Offset: 0x000F5029
	public int Width
	{
		get
		{
			return this.pending_emitter_state.width;
		}
		set
		{
			this.pending_emitter_state.width = this.MaybeDirty<int>(this.pending_emitter_state.width, value, ref this.dirty_shape);
		}
	}

	// Token: 0x170007E1 RID: 2017
	// (get) Token: 0x06007D34 RID: 32052 RVA: 0x000F6E4E File Offset: 0x000F504E
	// (set) Token: 0x06007D35 RID: 32053 RVA: 0x000F6E5B File Offset: 0x000F505B
	public float Range
	{
		get
		{
			return this.pending_emitter_state.radius;
		}
		set
		{
			this.pending_emitter_state.radius = this.MaybeDirty<float>(this.pending_emitter_state.radius, value, ref this.dirty_shape);
		}
	}

	// Token: 0x170007E2 RID: 2018
	// (get) Token: 0x06007D36 RID: 32054 RVA: 0x000F6E80 File Offset: 0x000F5080
	// (set) Token: 0x06007D37 RID: 32055 RVA: 0x000F6E8D File Offset: 0x000F508D
	private int origin
	{
		get
		{
			return this.pending_emitter_state.origin;
		}
		set
		{
			this.pending_emitter_state.origin = this.MaybeDirty<int>(this.pending_emitter_state.origin, value, ref this.dirty_position);
		}
	}

	// Token: 0x170007E3 RID: 2019
	// (get) Token: 0x06007D38 RID: 32056 RVA: 0x000F6EB2 File Offset: 0x000F50B2
	// (set) Token: 0x06007D39 RID: 32057 RVA: 0x000F6EBF File Offset: 0x000F50BF
	public float FalloffRate
	{
		get
		{
			return this.pending_emitter_state.falloffRate;
		}
		set
		{
			this.pending_emitter_state.falloffRate = this.MaybeDirty<float>(this.pending_emitter_state.falloffRate, value, ref this.dirty_falloff);
		}
	}

	// Token: 0x170007E4 RID: 2020
	// (get) Token: 0x06007D3A RID: 32058 RVA: 0x000F6EE4 File Offset: 0x000F50E4
	// (set) Token: 0x06007D3B RID: 32059 RVA: 0x000F6EEC File Offset: 0x000F50EC
	public float IntensityAnimation { get; set; }

	// Token: 0x170007E5 RID: 2021
	// (get) Token: 0x06007D3C RID: 32060 RVA: 0x000F6EF5 File Offset: 0x000F50F5
	// (set) Token: 0x06007D3D RID: 32061 RVA: 0x000F6EFD File Offset: 0x000F50FD
	public Vector2 Offset
	{
		get
		{
			return this._offset;
		}
		set
		{
			if (this._offset != value)
			{
				this._offset = value;
				this.origin = Grid.PosToCell(base.transform.GetPosition() + this._offset);
			}
		}
	}

	// Token: 0x170007E6 RID: 2022
	// (get) Token: 0x06007D3E RID: 32062 RVA: 0x000F6F3A File Offset: 0x000F513A
	private bool isRegistered
	{
		get
		{
			return this.solidPartitionerEntry != HandleVector<int>.InvalidHandle;
		}
	}

	// Token: 0x06007D3F RID: 32063 RVA: 0x00330730 File Offset: 0x0032E930
	public Light2D()
	{
		this.emitter = new LightGridManager.LightGridEmitter();
		this.Range = 5f;
		this.Lux = 1000;
	}

	// Token: 0x06007D40 RID: 32064 RVA: 0x000F6F4C File Offset: 0x000F514C
	protected override void OnPrefabInit()
	{
		base.Subscribe<Light2D>(-592767678, Light2D.OnOperationalChangedDelegate);
		if (this.disableOnStore)
		{
			base.Subscribe(856640610, new Action<object>(this.OnStore));
		}
		this.IntensityAnimation = 1f;
	}

	// Token: 0x06007D41 RID: 32065 RVA: 0x0033078C File Offset: 0x0032E98C
	private void OnStore(object data)
	{
		global::Debug.Assert(this.disableOnStore, "Only Light2Ds that are disabled on storage should be subscribed to OnStore.");
		Storage storage = data as Storage;
		if (storage != null)
		{
			base.enabled = (storage.GetComponent<ItemPedestal>() != null || storage.GetComponent<MinionIdentity>() != null);
			return;
		}
		base.enabled = true;
	}

	// Token: 0x06007D42 RID: 32066 RVA: 0x003307E4 File Offset: 0x0032E9E4
	protected override void OnCmpEnable()
	{
		this.materialPropertyBlock = new MaterialPropertyBlock();
		base.OnCmpEnable();
		Components.Light2Ds.Add(this);
		if (base.isSpawned)
		{
			this.AddToScenePartitioner();
			this.emitter.Refresh(this.pending_emitter_state, true);
		}
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnMoved), "Light2D.OnMoved");
	}

	// Token: 0x06007D43 RID: 32067 RVA: 0x000F6F8A File Offset: 0x000F518A
	protected override void OnCmpDisable()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnMoved));
		Components.Light2Ds.Remove(this);
		base.OnCmpDisable();
		this.FullRemove();
	}

	// Token: 0x06007D44 RID: 32068 RVA: 0x00330850 File Offset: 0x0032EA50
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.origin = Grid.PosToCell(base.transform.GetPosition() + this.Offset);
		if (base.isActiveAndEnabled)
		{
			this.AddToScenePartitioner();
			this.emitter.Refresh(this.pending_emitter_state, true);
		}
	}

	// Token: 0x06007D45 RID: 32069 RVA: 0x000F6FBF File Offset: 0x000F51BF
	protected override void OnCleanUp()
	{
		this.FullRemove();
	}

	// Token: 0x06007D46 RID: 32070 RVA: 0x000F6FC7 File Offset: 0x000F51C7
	private void OnMoved()
	{
		if (base.isSpawned)
		{
			this.FullRefresh();
		}
	}

	// Token: 0x06007D47 RID: 32071 RVA: 0x000F6FD7 File Offset: 0x000F51D7
	private HandleVector<int>.Handle AddToLayer(Extents ext, ScenePartitionerLayer layer)
	{
		return GameScenePartitioner.Instance.Add("Light2D", base.gameObject, ext, layer, new Action<object>(this.OnWorldChanged));
	}

	// Token: 0x06007D48 RID: 32072 RVA: 0x003308AC File Offset: 0x0032EAAC
	private Extents ComputeExtents()
	{
		Vector2I vector2I = Grid.CellToXY(this.origin);
		int x = 0;
		int y = 0;
		int width = 0;
		int num = 0;
		global::LightShape shape = this.shape;
		if (shape > global::LightShape.Cone)
		{
			if (shape == global::LightShape.Quad)
			{
				width = this.Width;
				num = (int)this.Range;
				int num2 = (this.Width % 2 == 0) ? (this.Width / 2 - 1) : Mathf.FloorToInt((float)(this.Width - 1) * 0.5f);
				Vector2I vector2I2 = vector2I - DiscreteShadowCaster.TravelDirectionToOrtogonalDiractionVector(this.LightDirection) * num2;
				x = vector2I2.x;
				switch (this.LightDirection)
				{
				case DiscreteShadowCaster.Direction.North:
					y = vector2I2.y;
					goto IL_119;
				case DiscreteShadowCaster.Direction.South:
					y = vector2I2.y - num;
					goto IL_119;
				}
				y = vector2I2.y - DiscreteShadowCaster.TravelDirectionToOrtogonalDiractionVector(this.LightDirection).y * num2;
			}
		}
		else
		{
			int num3 = (int)this.Range;
			int num4 = num3 * 2;
			x = vector2I.x - num3;
			y = vector2I.y - num3;
			width = num4;
			num = ((this.shape == global::LightShape.Circle) ? num4 : num3);
		}
		IL_119:
		return new Extents(x, y, width, num);
	}

	// Token: 0x06007D49 RID: 32073 RVA: 0x003309DC File Offset: 0x0032EBDC
	private void AddToScenePartitioner()
	{
		Extents ext = this.ComputeExtents();
		this.solidPartitionerEntry = this.AddToLayer(ext, GameScenePartitioner.Instance.solidChangedLayer);
		this.liquidPartitionerEntry = this.AddToLayer(ext, GameScenePartitioner.Instance.liquidChangedLayer);
	}

	// Token: 0x06007D4A RID: 32074 RVA: 0x000F6FFC File Offset: 0x000F51FC
	private void RemoveFromScenePartitioner()
	{
		if (this.isRegistered)
		{
			GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
			GameScenePartitioner.Instance.Free(ref this.liquidPartitionerEntry);
		}
	}

	// Token: 0x06007D4B RID: 32075 RVA: 0x000F7026 File Offset: 0x000F5226
	private void MoveInScenePartitioner()
	{
		GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, this.ComputeExtents());
		GameScenePartitioner.Instance.UpdatePosition(this.liquidPartitionerEntry, this.ComputeExtents());
	}

	// Token: 0x06007D4C RID: 32076 RVA: 0x000F7054 File Offset: 0x000F5254
	private void EmitterRefresh()
	{
		this.emitter.Refresh(this.pending_emitter_state, true);
	}

	// Token: 0x06007D4D RID: 32077 RVA: 0x000F7069 File Offset: 0x000F5269
	[ContextMenu("Refresh")]
	public void FullRefresh()
	{
		if (!base.isSpawned || !base.isActiveAndEnabled)
		{
			return;
		}
		DebugUtil.DevAssert(this.isRegistered, "shouldn't be refreshing if we aren't spawned and enabled", null);
		this.RefreshShapeAndPosition();
		this.EmitterRefresh();
	}

	// Token: 0x06007D4E RID: 32078 RVA: 0x000F709A File Offset: 0x000F529A
	public void FullRemove()
	{
		this.RemoveFromScenePartitioner();
		this.emitter.RemoveFromGrid();
	}

	// Token: 0x06007D4F RID: 32079 RVA: 0x00330A20 File Offset: 0x0032EC20
	public Light2D.RefreshResult RefreshShapeAndPosition()
	{
		if (!base.isSpawned)
		{
			return Light2D.RefreshResult.None;
		}
		if (!base.isActiveAndEnabled)
		{
			this.FullRemove();
			return Light2D.RefreshResult.Removed;
		}
		int num = Grid.PosToCell(base.transform.GetPosition() + this.Offset);
		if (!Grid.IsValidCell(num))
		{
			this.FullRemove();
			return Light2D.RefreshResult.Removed;
		}
		this.origin = num;
		if (this.dirty_shape)
		{
			this.RemoveFromScenePartitioner();
			this.AddToScenePartitioner();
		}
		else if (this.dirty_position)
		{
			this.MoveInScenePartitioner();
		}
		if (this.dirty_falloff)
		{
			this.EmitterRefresh();
		}
		this.dirty_shape = false;
		this.dirty_position = false;
		this.dirty_falloff = false;
		return Light2D.RefreshResult.Updated;
	}

	// Token: 0x06007D50 RID: 32080 RVA: 0x000F70AD File Offset: 0x000F52AD
	private void OnWorldChanged(object data)
	{
		this.FullRefresh();
	}

	// Token: 0x06007D51 RID: 32081 RVA: 0x00330AC8 File Offset: 0x0032ECC8
	public virtual List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, this.Range), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT, Descriptor.DescriptorType.Effect, false),
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT_LUX, this.Lux), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT_LUX, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x04005E44 RID: 24132
	public bool autoRespondToOperational = true;

	// Token: 0x04005E45 RID: 24133
	private bool dirty_shape;

	// Token: 0x04005E46 RID: 24134
	private bool dirty_position;

	// Token: 0x04005E47 RID: 24135
	private bool dirty_falloff;

	// Token: 0x04005E48 RID: 24136
	[SerializeField]
	private LightGridManager.LightGridEmitter.State pending_emitter_state = LightGridManager.LightGridEmitter.State.DEFAULT;

	// Token: 0x04005E4B RID: 24139
	public float Angle;

	// Token: 0x04005E4C RID: 24140
	public Vector2 Direction;

	// Token: 0x04005E4D RID: 24141
	[SerializeField]
	private Vector2 _offset;

	// Token: 0x04005E4E RID: 24142
	public bool drawOverlay;

	// Token: 0x04005E4F RID: 24143
	public Color overlayColour;

	// Token: 0x04005E50 RID: 24144
	public MaterialPropertyBlock materialPropertyBlock;

	// Token: 0x04005E51 RID: 24145
	private HandleVector<int>.Handle solidPartitionerEntry = HandleVector<int>.InvalidHandle;

	// Token: 0x04005E52 RID: 24146
	private HandleVector<int>.Handle liquidPartitionerEntry = HandleVector<int>.InvalidHandle;

	// Token: 0x04005E53 RID: 24147
	public bool disableOnStore;

	// Token: 0x04005E54 RID: 24148
	private static readonly EventSystem.IntraObjectHandler<Light2D> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Light2D>(delegate(Light2D light, object data)
	{
		if (light.autoRespondToOperational)
		{
			light.enabled = (bool)data;
		}
	});

	// Token: 0x020017CC RID: 6092
	public enum RefreshResult
	{
		// Token: 0x04005E56 RID: 24150
		None,
		// Token: 0x04005E57 RID: 24151
		Removed,
		// Token: 0x04005E58 RID: 24152
		Updated
	}
}
