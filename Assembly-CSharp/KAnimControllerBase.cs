using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200093E RID: 2366
public abstract class KAnimControllerBase : MonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x06002986 RID: 10630 RVA: 0x001E3588 File Offset: 0x001E1788
	protected KAnimControllerBase()
	{
		this.previousFrame = -1;
		this.currentFrame = -1;
		this.PlaySpeedMultiplier = 1f;
		this.synchronizer = new KAnimSynchronizer(this);
		this.layering = new KAnimLayering(this, this.fgLayer);
		this.isVisible = true;
	}

	// Token: 0x06002987 RID: 10631
	public abstract KAnim.Anim GetAnim(int index);

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06002988 RID: 10632 RVA: 0x000BF765 File Offset: 0x000BD965
	// (set) Token: 0x06002989 RID: 10633 RVA: 0x000BF76D File Offset: 0x000BD96D
	public string debugName { get; private set; }

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x0600298A RID: 10634 RVA: 0x000BF776 File Offset: 0x000BD976
	// (set) Token: 0x0600298B RID: 10635 RVA: 0x000BF77E File Offset: 0x000BD97E
	public KAnim.Build curBuild { get; protected set; }

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x0600298C RID: 10636 RVA: 0x001E3694 File Offset: 0x001E1894
	// (remove) Token: 0x0600298D RID: 10637 RVA: 0x001E36CC File Offset: 0x001E18CC
	public event Action<Color32> OnOverlayColourChanged;

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x0600298E RID: 10638 RVA: 0x000BF787 File Offset: 0x000BD987
	// (set) Token: 0x0600298F RID: 10639 RVA: 0x000BF78F File Offset: 0x000BD98F
	public new bool enabled
	{
		get
		{
			return this._enabled;
		}
		set
		{
			this._enabled = value;
			if (!this.hasAwakeRun)
			{
				return;
			}
			if (this._enabled)
			{
				this.Enable();
				return;
			}
			this.Disable();
		}
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06002990 RID: 10640 RVA: 0x000BF7B6 File Offset: 0x000BD9B6
	public bool HasBatchInstanceData
	{
		get
		{
			return this.batchInstanceData != null;
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06002991 RID: 10641 RVA: 0x000BF7C1 File Offset: 0x000BD9C1
	// (set) Token: 0x06002992 RID: 10642 RVA: 0x000BF7C9 File Offset: 0x000BD9C9
	public SymbolInstanceGpuData symbolInstanceGpuData { get; protected set; }

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06002993 RID: 10643 RVA: 0x000BF7D2 File Offset: 0x000BD9D2
	// (set) Token: 0x06002994 RID: 10644 RVA: 0x000BF7DA File Offset: 0x000BD9DA
	public SymbolOverrideInfoGpuData symbolOverrideInfoGpuData { get; protected set; }

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06002995 RID: 10645 RVA: 0x000BF7E3 File Offset: 0x000BD9E3
	// (set) Token: 0x06002996 RID: 10646 RVA: 0x001E3704 File Offset: 0x001E1904
	public Color32 TintColour
	{
		get
		{
			return this.batchInstanceData.GetTintColour();
		}
		set
		{
			if (this.batchInstanceData != null && this.batchInstanceData.SetTintColour(value))
			{
				this.SetDirty();
				this.SuspendUpdates(false);
				if (this.OnTintChanged != null)
				{
					this.OnTintChanged(value);
				}
			}
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06002997 RID: 10647 RVA: 0x000BF7F5 File Offset: 0x000BD9F5
	// (set) Token: 0x06002998 RID: 10648 RVA: 0x000BF807 File Offset: 0x000BDA07
	public Color32 HighlightColour
	{
		get
		{
			return this.batchInstanceData.GetHighlightcolour();
		}
		set
		{
			if (this.batchInstanceData.SetHighlightColour(value))
			{
				this.SetDirty();
				this.SuspendUpdates(false);
				if (this.OnHighlightChanged != null)
				{
					this.OnHighlightChanged(value);
				}
			}
		}
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06002999 RID: 10649 RVA: 0x000BF842 File Offset: 0x000BDA42
	// (set) Token: 0x0600299A RID: 10650 RVA: 0x000BF84F File Offset: 0x000BDA4F
	public Color OverlayColour
	{
		get
		{
			return this.batchInstanceData.GetOverlayColour();
		}
		set
		{
			if (this.batchInstanceData.SetOverlayColour(value))
			{
				this.SetDirty();
				this.SuspendUpdates(false);
				if (this.OnOverlayColourChanged != null)
				{
					this.OnOverlayColourChanged(value);
				}
			}
		}
	}

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x0600299B RID: 10651 RVA: 0x001E3754 File Offset: 0x001E1954
	// (remove) Token: 0x0600299C RID: 10652 RVA: 0x001E378C File Offset: 0x001E198C
	public event KAnimControllerBase.KAnimEvent onAnimEnter;

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x0600299D RID: 10653 RVA: 0x001E37C4 File Offset: 0x001E19C4
	// (remove) Token: 0x0600299E RID: 10654 RVA: 0x001E37FC File Offset: 0x001E19FC
	public event KAnimControllerBase.KAnimEvent onAnimComplete;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x0600299F RID: 10655 RVA: 0x001E3834 File Offset: 0x001E1A34
	// (remove) Token: 0x060029A0 RID: 10656 RVA: 0x001E386C File Offset: 0x001E1A6C
	public event Action<int> onLayerChanged;

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x060029A1 RID: 10657 RVA: 0x000BF885 File Offset: 0x000BDA85
	// (set) Token: 0x060029A2 RID: 10658 RVA: 0x000BF88D File Offset: 0x000BDA8D
	public int previousFrame { get; protected set; }

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x060029A3 RID: 10659 RVA: 0x000BF896 File Offset: 0x000BDA96
	// (set) Token: 0x060029A4 RID: 10660 RVA: 0x000BF89E File Offset: 0x000BDA9E
	public int currentFrame { get; protected set; }

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x060029A5 RID: 10661 RVA: 0x001E38A4 File Offset: 0x001E1AA4
	public HashedString currentAnim
	{
		get
		{
			if (this.curAnim == null)
			{
				return default(HashedString);
			}
			return this.curAnim.hash;
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x060029A7 RID: 10663 RVA: 0x000BF8B0 File Offset: 0x000BDAB0
	// (set) Token: 0x060029A6 RID: 10662 RVA: 0x000BF8A7 File Offset: 0x000BDAA7
	public float PlaySpeedMultiplier { get; set; }

	// Token: 0x060029A8 RID: 10664 RVA: 0x000BF8B8 File Offset: 0x000BDAB8
	public void SetFGLayer(Grid.SceneLayer layer)
	{
		this.fgLayer = layer;
		this.GetLayering();
		if (this.layering != null)
		{
			this.layering.SetLayer(this.fgLayer);
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x060029A9 RID: 10665 RVA: 0x000BF8E1 File Offset: 0x000BDAE1
	// (set) Token: 0x060029AA RID: 10666 RVA: 0x000BF8E9 File Offset: 0x000BDAE9
	public KAnim.PlayMode PlayMode
	{
		get
		{
			return this.mode;
		}
		set
		{
			this.mode = value;
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x060029AB RID: 10667 RVA: 0x000BF8F2 File Offset: 0x000BDAF2
	// (set) Token: 0x060029AC RID: 10668 RVA: 0x000BF8FA File Offset: 0x000BDAFA
	public bool FlipX
	{
		get
		{
			return this.flipX;
		}
		set
		{
			this.flipX = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x060029AD RID: 10669 RVA: 0x000BF91C File Offset: 0x000BDB1C
	// (set) Token: 0x060029AE RID: 10670 RVA: 0x000BF924 File Offset: 0x000BDB24
	public bool FlipY
	{
		get
		{
			return this.flipY;
		}
		set
		{
			this.flipY = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x060029AF RID: 10671 RVA: 0x000BF946 File Offset: 0x000BDB46
	// (set) Token: 0x060029B0 RID: 10672 RVA: 0x000BF94E File Offset: 0x000BDB4E
	public Vector3 Offset
	{
		get
		{
			return this.offset;
		}
		set
		{
			this.offset = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.DeRegister();
			this.Register();
			this.RefreshVisibilityListener();
			this.SetDirty();
		}
	}

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x060029B1 RID: 10673 RVA: 0x000BF982 File Offset: 0x000BDB82
	// (set) Token: 0x060029B2 RID: 10674 RVA: 0x000BF98A File Offset: 0x000BDB8A
	public float Rotation
	{
		get
		{
			return this.rotation;
		}
		set
		{
			this.rotation = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x060029B3 RID: 10675 RVA: 0x000BF9AC File Offset: 0x000BDBAC
	// (set) Token: 0x060029B4 RID: 10676 RVA: 0x000BF9B4 File Offset: 0x000BDBB4
	public Vector3 Pivot
	{
		get
		{
			return this.pivot;
		}
		set
		{
			this.pivot = value;
			if (this.layering != null)
			{
				this.layering.Dirty();
			}
			this.SetDirty();
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x060029B5 RID: 10677 RVA: 0x000BF9D6 File Offset: 0x000BDBD6
	public Vector3 PositionIncludingOffset
	{
		get
		{
			return base.transform.GetPosition() + this.Offset;
		}
	}

	// Token: 0x060029B6 RID: 10678 RVA: 0x000BF9EE File Offset: 0x000BDBEE
	public KAnimBatchGroup.MaterialType GetMaterialType()
	{
		return this.materialType;
	}

	// Token: 0x060029B7 RID: 10679 RVA: 0x001E38D0 File Offset: 0x001E1AD0
	public Vector3 GetWorldPivot()
	{
		Vector3 position = base.transform.GetPosition();
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component != null)
		{
			position.x += component.offset.x;
			position.y += component.offset.y - component.size.y / 2f;
		}
		return position;
	}

	// Token: 0x060029B8 RID: 10680 RVA: 0x000BF9F6 File Offset: 0x000BDBF6
	public KAnim.Anim GetCurrentAnim()
	{
		return this.curAnim;
	}

	// Token: 0x060029B9 RID: 10681 RVA: 0x000BF9FE File Offset: 0x000BDBFE
	public KAnimHashedString GetBuildHash()
	{
		if (this.curBuild == null)
		{
			return KAnimBatchManager.NO_BATCH;
		}
		return this.curBuild.fileHash;
	}

	// Token: 0x060029BA RID: 10682 RVA: 0x000BFA1E File Offset: 0x000BDC1E
	protected float GetDuration()
	{
		if (this.curAnim != null)
		{
			return (float)this.curAnim.numFrames / this.curAnim.frameRate;
		}
		return 0f;
	}

	// Token: 0x060029BB RID: 10683 RVA: 0x001E3938 File Offset: 0x001E1B38
	protected int GetFrameIdxFromOffset(int offset)
	{
		int result = -1;
		if (this.curAnim != null)
		{
			result = offset + this.curAnim.firstFrameIdx;
		}
		return result;
	}

	// Token: 0x060029BC RID: 10684 RVA: 0x001E3960 File Offset: 0x001E1B60
	public int GetFrameIdx(float time, bool absolute)
	{
		int result = -1;
		if (this.curAnim != null)
		{
			result = this.curAnim.GetFrameIdx(this.mode, time) + (absolute ? this.curAnim.firstFrameIdx : 0);
		}
		return result;
	}

	// Token: 0x060029BD RID: 10685 RVA: 0x000BFA46 File Offset: 0x000BDC46
	public bool IsStopped()
	{
		return this.stopped;
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x060029BE RID: 10686 RVA: 0x000BF9F6 File Offset: 0x000BDBF6
	public KAnim.Anim CurrentAnim
	{
		get
		{
			return this.curAnim;
		}
	}

	// Token: 0x060029BF RID: 10687 RVA: 0x000BFA4E File Offset: 0x000BDC4E
	public KAnimSynchronizer GetSynchronizer()
	{
		return this.synchronizer;
	}

	// Token: 0x060029C0 RID: 10688 RVA: 0x000BFA56 File Offset: 0x000BDC56
	public KAnimLayering GetLayering()
	{
		if (this.layering == null && this.fgLayer != Grid.SceneLayer.NoLayer)
		{
			this.layering = new KAnimLayering(this, this.fgLayer);
		}
		return this.layering;
	}

	// Token: 0x060029C1 RID: 10689 RVA: 0x000BF8E1 File Offset: 0x000BDAE1
	public KAnim.PlayMode GetMode()
	{
		return this.mode;
	}

	// Token: 0x060029C2 RID: 10690 RVA: 0x000BFA82 File Offset: 0x000BDC82
	public static string GetModeString(KAnim.PlayMode mode)
	{
		switch (mode)
		{
		case KAnim.PlayMode.Loop:
			return "Loop";
		case KAnim.PlayMode.Once:
			return "Once";
		case KAnim.PlayMode.Paused:
			return "Paused";
		default:
			return "Unknown";
		}
	}

	// Token: 0x060029C3 RID: 10691 RVA: 0x000BFAAF File Offset: 0x000BDCAF
	public float GetPlaySpeed()
	{
		return this.playSpeed;
	}

	// Token: 0x060029C4 RID: 10692 RVA: 0x000BFAB7 File Offset: 0x000BDCB7
	public void SetElapsedTime(float value)
	{
		this.elapsedTime = value;
	}

	// Token: 0x060029C5 RID: 10693 RVA: 0x000BFAC0 File Offset: 0x000BDCC0
	public float GetElapsedTime()
	{
		return this.elapsedTime;
	}

	// Token: 0x060029C6 RID: 10694
	protected abstract void SuspendUpdates(bool suspend);

	// Token: 0x060029C7 RID: 10695
	protected abstract void OnStartQueuedAnim();

	// Token: 0x060029C8 RID: 10696
	public abstract void SetDirty();

	// Token: 0x060029C9 RID: 10697
	protected abstract void RefreshVisibilityListener();

	// Token: 0x060029CA RID: 10698
	protected abstract void DeRegister();

	// Token: 0x060029CB RID: 10699
	protected abstract void Register();

	// Token: 0x060029CC RID: 10700
	protected abstract void OnAwake();

	// Token: 0x060029CD RID: 10701
	protected abstract void OnStart();

	// Token: 0x060029CE RID: 10702
	protected abstract void OnStop();

	// Token: 0x060029CF RID: 10703
	protected abstract void Enable();

	// Token: 0x060029D0 RID: 10704
	protected abstract void Disable();

	// Token: 0x060029D1 RID: 10705
	protected abstract void UpdateFrame(float t);

	// Token: 0x060029D2 RID: 10706
	public abstract Matrix2x3 GetTransformMatrix();

	// Token: 0x060029D3 RID: 10707
	public abstract Matrix2x3 GetSymbolLocalTransform(HashedString symbol, out bool symbolVisible);

	// Token: 0x060029D4 RID: 10708
	public abstract void UpdateAllHiddenSymbols();

	// Token: 0x060029D5 RID: 10709
	public abstract void UpdateHiddenSymbol(KAnimHashedString specificSymbol);

	// Token: 0x060029D6 RID: 10710
	public abstract void UpdateHiddenSymbolSet(HashSet<KAnimHashedString> specificSymbols);

	// Token: 0x060029D7 RID: 10711
	public abstract void TriggerStop();

	// Token: 0x060029D8 RID: 10712 RVA: 0x000BFAC8 File Offset: 0x000BDCC8
	public virtual void SetLayer(int layer)
	{
		if (this.onLayerChanged != null)
		{
			this.onLayerChanged(layer);
		}
	}

	// Token: 0x060029D9 RID: 10713 RVA: 0x001E39A0 File Offset: 0x001E1BA0
	public Vector3 GetPivotSymbolPosition()
	{
		bool flag = false;
		Matrix4x4 symbolTransform = this.GetSymbolTransform(KAnimControllerBase.snaptoPivot, out flag);
		Vector3 position = base.transform.GetPosition();
		if (flag)
		{
			position = new Vector3(symbolTransform[0, 3], symbolTransform[1, 3], symbolTransform[2, 3]);
		}
		return position;
	}

	// Token: 0x060029DA RID: 10714 RVA: 0x000BFADE File Offset: 0x000BDCDE
	public virtual Matrix4x4 GetSymbolTransform(HashedString symbol, out bool symbolVisible)
	{
		symbolVisible = false;
		return Matrix4x4.identity;
	}

	// Token: 0x060029DB RID: 10715 RVA: 0x001E39F0 File Offset: 0x001E1BF0
	private void Awake()
	{
		this.aem = Singleton<AnimEventManager>.Instance;
		this.debugName = base.name;
		this.SetFGLayer(this.fgLayer);
		this.OnAwake();
		if (!string.IsNullOrEmpty(this.initialAnim))
		{
			this.SetDirty();
			this.Play(this.initialAnim, this.initialMode, 1f, 0f);
		}
		this.hasAwakeRun = true;
	}

	// Token: 0x060029DC RID: 10716 RVA: 0x000BFAE8 File Offset: 0x000BDCE8
	private void Start()
	{
		this.OnStart();
	}

	// Token: 0x060029DD RID: 10717 RVA: 0x001E3A64 File Offset: 0x001E1C64
	protected virtual void OnDestroy()
	{
		this.animFiles = null;
		this.curAnim = null;
		this.curBuild = null;
		this.synchronizer = null;
		this.layering = null;
		this.animQueue = null;
		this.overrideAnims = null;
		this.anims = null;
		this.synchronizer = null;
		this.layering = null;
		this.overrideAnimFiles = null;
	}

	// Token: 0x060029DE RID: 10718 RVA: 0x000BFAF0 File Offset: 0x000BDCF0
	protected void AnimEnter(HashedString hashed_name)
	{
		if (this.onAnimEnter != null)
		{
			this.onAnimEnter(hashed_name);
		}
	}

	// Token: 0x060029DF RID: 10719 RVA: 0x000BFB06 File Offset: 0x000BDD06
	public void Play(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		if (!this.stopped)
		{
			this.Stop();
		}
		this.Queue(anim_name, mode, speed, time_offset);
	}

	// Token: 0x060029E0 RID: 10720 RVA: 0x001E3AC0 File Offset: 0x001E1CC0
	public void Play(HashedString[] anim_names, KAnim.PlayMode mode = KAnim.PlayMode.Once)
	{
		if (!this.stopped)
		{
			this.Stop();
		}
		for (int i = 0; i < anim_names.Length - 1; i++)
		{
			this.Queue(anim_names[i], KAnim.PlayMode.Once, 1f, 0f);
		}
		global::Debug.Assert(anim_names.Length != 0, "Play was called with an empty anim array");
		this.Queue(anim_names[anim_names.Length - 1], mode, 1f, 0f);
	}

	// Token: 0x060029E1 RID: 10721 RVA: 0x001E3B30 File Offset: 0x001E1D30
	public void Queue(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		this.animQueue.Enqueue(new KAnimControllerBase.AnimData
		{
			anim = anim_name,
			mode = mode,
			speed = speed,
			timeOffset = time_offset
		});
		this.mode = ((mode == KAnim.PlayMode.Paused) ? KAnim.PlayMode.Paused : KAnim.PlayMode.Once);
		if (this.aem != null)
		{
			this.aem.SetMode(this.eventManagerHandle, this.mode);
		}
		if (this.animQueue.Count == 1 && this.stopped)
		{
			this.StartQueuedAnim();
		}
	}

	// Token: 0x060029E2 RID: 10722 RVA: 0x000BFB21 File Offset: 0x000BDD21
	public void QueueAndSyncTransition(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		this.SyncTransition();
		this.Queue(anim_name, mode, speed, time_offset);
	}

	// Token: 0x060029E3 RID: 10723 RVA: 0x000BFB34 File Offset: 0x000BDD34
	public void SyncTransition()
	{
		this.elapsedTime %= Mathf.Max(float.Epsilon, this.GetDuration());
	}

	// Token: 0x060029E4 RID: 10724 RVA: 0x000BFB53 File Offset: 0x000BDD53
	public void ClearQueue()
	{
		this.animQueue.Clear();
	}

	// Token: 0x060029E5 RID: 10725 RVA: 0x001E3BBC File Offset: 0x001E1DBC
	private void Restart(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0f)
	{
		if (this.curBuild == null)
		{
			string[] array = new string[5];
			array[0] = "[";
			array[1] = base.gameObject.name;
			array[2] = "] Missing build while trying to play anim [";
			int num = 3;
			HashedString hashedString = anim_name;
			array[num] = hashedString.ToString();
			array[4] = "]";
			global::Debug.LogWarning(string.Concat(array), base.gameObject);
			return;
		}
		Queue<KAnimControllerBase.AnimData> queue = new Queue<KAnimControllerBase.AnimData>();
		queue.Enqueue(new KAnimControllerBase.AnimData
		{
			anim = anim_name,
			mode = mode,
			speed = speed,
			timeOffset = time_offset
		});
		while (this.animQueue.Count > 0)
		{
			queue.Enqueue(this.animQueue.Dequeue());
		}
		this.animQueue = queue;
		if (this.animQueue.Count == 1 && this.stopped)
		{
			this.StartQueuedAnim();
		}
	}

	// Token: 0x060029E6 RID: 10726 RVA: 0x001E3C9C File Offset: 0x001E1E9C
	protected void StartQueuedAnim()
	{
		this.StopAnimEventSequence();
		this.previousFrame = -1;
		this.currentFrame = -1;
		this.SuspendUpdates(false);
		this.stopped = false;
		this.OnStartQueuedAnim();
		KAnimControllerBase.AnimData animData = this.animQueue.Dequeue();
		while (animData.mode == KAnim.PlayMode.Loop && this.animQueue.Count > 0)
		{
			animData = this.animQueue.Dequeue();
		}
		KAnimControllerBase.AnimLookupData animLookupData;
		if (this.overrideAnims == null || !this.overrideAnims.TryGetValue(animData.anim, out animLookupData))
		{
			if (!this.anims.TryGetValue(animData.anim, out animLookupData))
			{
				bool flag = true;
				if (this.showWhenMissing != null)
				{
					this.showWhenMissing.SetActive(true);
				}
				if (flag)
				{
					this.TriggerStop();
					return;
				}
			}
			else if (this.showWhenMissing != null)
			{
				this.showWhenMissing.SetActive(false);
			}
		}
		this.curAnim = this.GetAnim(animLookupData.animIndex);
		int num = 0;
		if (animData.mode == KAnim.PlayMode.Loop && this.randomiseLoopedOffset)
		{
			num = UnityEngine.Random.Range(0, this.curAnim.numFrames - 1);
		}
		this.prevAnimFrame = -1;
		this.curAnimFrameIdx = this.GetFrameIdxFromOffset(num);
		this.currentFrame = this.curAnimFrameIdx;
		this.mode = animData.mode;
		this.playSpeed = animData.speed * this.PlaySpeedMultiplier;
		this.SetElapsedTime((float)num / this.curAnim.frameRate + animData.timeOffset);
		this.synchronizer.Sync();
		this.StartAnimEventSequence();
		this.AnimEnter(animData.anim);
	}

	// Token: 0x060029E7 RID: 10727 RVA: 0x000BFB60 File Offset: 0x000BDD60
	public bool GetSymbolVisiblity(KAnimHashedString symbol)
	{
		return !this.hiddenSymbolsSet.Contains(symbol);
	}

	// Token: 0x060029E8 RID: 10728 RVA: 0x000BFB71 File Offset: 0x000BDD71
	public void SetSymbolVisiblity(KAnimHashedString symbol, bool is_visible)
	{
		if (is_visible)
		{
			this.hiddenSymbolsSet.Remove(symbol);
		}
		else if (!this.hiddenSymbolsSet.Contains(symbol))
		{
			this.hiddenSymbolsSet.Add(symbol);
		}
		if (this.curBuild != null)
		{
			this.UpdateHiddenSymbol(symbol);
		}
	}

	// Token: 0x060029E9 RID: 10729 RVA: 0x001E3E20 File Offset: 0x001E2020
	public void BatchSetSymbolsVisiblity(HashSet<KAnimHashedString> symbols, bool is_visible)
	{
		foreach (KAnimHashedString item in symbols)
		{
			if (is_visible)
			{
				this.hiddenSymbolsSet.Remove(item);
			}
			else if (!this.hiddenSymbolsSet.Contains(item))
			{
				this.hiddenSymbolsSet.Add(item);
			}
		}
		if (this.curBuild != null)
		{
			this.UpdateHiddenSymbolSet(symbols);
		}
	}

	// Token: 0x060029EA RID: 10730 RVA: 0x001E3EA4 File Offset: 0x001E20A4
	public void AddAnimOverrides(KAnimFile kanim_file, float priority = 0f)
	{
		if (kanim_file == null)
		{
			global::Debug.LogError(string.Format("AddAnimOverrides tried to add a null override to {0} at position {1}", base.gameObject.name, base.transform.position));
		}
		if (kanim_file.GetData().build != null && kanim_file.GetData().build.symbols.Length != 0)
		{
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			DebugUtil.Assert(component != null, "Anim overrides containing additional symbols require a symbol override controller.");
			component.AddBuildOverride(kanim_file.GetData(), 0);
		}
		this.overrideAnimFiles.Add(new KAnimControllerBase.OverrideAnimFileData
		{
			priority = priority,
			file = kanim_file
		});
		this.overrideAnimFiles.Sort((KAnimControllerBase.OverrideAnimFileData a, KAnimControllerBase.OverrideAnimFileData b) => b.priority.CompareTo(a.priority));
		this.RebuildOverrides(kanim_file);
	}

	// Token: 0x060029EB RID: 10731 RVA: 0x001E3F7C File Offset: 0x001E217C
	public void RemoveAnimOverrides(KAnimFile kanim_file)
	{
		if (kanim_file == null)
		{
			global::Debug.LogError(string.Format("RemoveAnimOverrides tried to remove a null override to {0} at position {1}", base.gameObject.name, base.transform.position));
		}
		if (kanim_file.GetData().build != null && kanim_file.GetData().build.symbols.Length != 0)
		{
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			DebugUtil.Assert(component != null, "Anim overrides containing additional symbols require a symbol override controller.");
			component.TryRemoveBuildOverride(kanim_file.GetData(), 0);
		}
		for (int i = 0; i < this.overrideAnimFiles.Count; i++)
		{
			if (this.overrideAnimFiles[i].file == kanim_file)
			{
				this.overrideAnimFiles.RemoveAt(i);
				break;
			}
		}
		this.RebuildOverrides(kanim_file);
	}

	// Token: 0x060029EC RID: 10732 RVA: 0x001E4044 File Offset: 0x001E2244
	private void RebuildOverrides(KAnimFile kanim_file)
	{
		bool flag = false;
		this.overrideAnims.Clear();
		for (int i = 0; i < this.overrideAnimFiles.Count; i++)
		{
			KAnimControllerBase.OverrideAnimFileData overrideAnimFileData = this.overrideAnimFiles[i];
			KAnimFileData data = overrideAnimFileData.file.GetData();
			for (int j = 0; j < data.animCount; j++)
			{
				KAnim.Anim anim = data.GetAnim(j);
				if (anim.animFile.hashName != data.hashName)
				{
					global::Debug.LogError(string.Format("How did we get an anim from another file? [{0}] != [{1}] for anim [{2}]", data.name, anim.animFile.name, j));
				}
				KAnimControllerBase.AnimLookupData value = default(KAnimControllerBase.AnimLookupData);
				value.animIndex = anim.index;
				HashedString hashedString = new HashedString(anim.name);
				if (!this.overrideAnims.ContainsKey(hashedString))
				{
					this.overrideAnims[hashedString] = value;
				}
				if (this.curAnim != null && this.curAnim.hash == hashedString && overrideAnimFileData.file == kanim_file)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.Restart(this.curAnim.name, this.mode, this.playSpeed, 0f);
		}
	}

	// Token: 0x060029ED RID: 10733 RVA: 0x001E4194 File Offset: 0x001E2394
	public bool HasAnimation(HashedString anim_name)
	{
		bool flag = anim_name.IsValid;
		if (flag)
		{
			bool flag2 = this.anims.ContainsKey(anim_name);
			bool flag3 = !flag2 && this.overrideAnims.ContainsKey(anim_name);
			flag = (flag2 || flag3);
		}
		return flag;
	}

	// Token: 0x060029EE RID: 10734 RVA: 0x001E41D0 File Offset: 0x001E23D0
	public bool HasAnimationFile(KAnimHashedString anim_file_name)
	{
		KAnimFile kanimFile = null;
		return this.TryGetAnimationFile(anim_file_name, out kanimFile);
	}

	// Token: 0x060029EF RID: 10735 RVA: 0x001E41E8 File Offset: 0x001E23E8
	public bool TryGetAnimationFile(KAnimHashedString anim_file_name, out KAnimFile match)
	{
		match = null;
		if (!anim_file_name.IsValid())
		{
			return false;
		}
		KAnimFileData kanimFileData = null;
		int num = 0;
		int num2 = this.overrideAnimFiles.Count - 1;
		int num3 = (int)((float)this.overrideAnimFiles.Count * 0.5f);
		while (num3 > 0 && match == null && num < num3)
		{
			if (this.overrideAnimFiles[num].file != null)
			{
				kanimFileData = this.overrideAnimFiles[num].file.GetData();
			}
			if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
			{
				match = this.overrideAnimFiles[num].file;
				break;
			}
			if (this.overrideAnimFiles[num2].file != null)
			{
				kanimFileData = this.overrideAnimFiles[num2].file.GetData();
			}
			if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
			{
				match = this.overrideAnimFiles[num2].file;
			}
			num++;
			num2--;
		}
		if (match == null && this.overrideAnimFiles.Count % 2 != 0)
		{
			if (this.overrideAnimFiles[num].file != null)
			{
				kanimFileData = this.overrideAnimFiles[num].file.GetData();
			}
			if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
			{
				match = this.overrideAnimFiles[num].file;
			}
		}
		kanimFileData = null;
		if (match == null && this.animFiles != null)
		{
			num = 0;
			num2 = this.animFiles.Length - 1;
			num3 = (int)((float)this.animFiles.Length * 0.5f);
			while (num3 > 0 && match == null && num < num3)
			{
				if (this.animFiles[num] != null)
				{
					kanimFileData = this.animFiles[num].GetData();
				}
				if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
				{
					match = this.animFiles[num];
					break;
				}
				if (this.animFiles[num2] != null)
				{
					kanimFileData = this.animFiles[num2].GetData();
				}
				if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
				{
					match = this.animFiles[num2];
				}
				num++;
				num2--;
			}
			if (match == null && this.animFiles.Length % 2 != 0)
			{
				if (this.animFiles[num] != null)
				{
					kanimFileData = this.animFiles[num].GetData();
				}
				if (kanimFileData != null && kanimFileData.hashName.HashValue == anim_file_name.HashValue)
				{
					match = this.animFiles[num];
				}
			}
		}
		return match != null;
	}

	// Token: 0x060029F0 RID: 10736 RVA: 0x001E44C4 File Offset: 0x001E26C4
	public void AddAnims(KAnimFile anim_file)
	{
		KAnimFileData data = anim_file.GetData();
		if (data == null)
		{
			global::Debug.LogError("AddAnims() Null animfile data");
			return;
		}
		this.maxSymbols = Mathf.Max(this.maxSymbols, data.maxVisSymbolFrames);
		for (int i = 0; i < data.animCount; i++)
		{
			KAnim.Anim anim = data.GetAnim(i);
			if (anim.animFile.hashName != data.hashName)
			{
				global::Debug.LogErrorFormat("How did we get an anim from another file? [{0}] != [{1}] for anim [{2}]", new object[]
				{
					data.name,
					anim.animFile.name,
					i
				});
			}
			this.anims[anim.hash] = new KAnimControllerBase.AnimLookupData
			{
				animIndex = anim.index
			};
		}
		if (this.usingNewSymbolOverrideSystem && data.buildIndex != -1 && data.build.symbols != null && data.build.symbols.Length != 0)
		{
			base.GetComponent<SymbolOverrideController>().AddBuildOverride(anim_file.GetData(), -1);
		}
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x060029F1 RID: 10737 RVA: 0x000BFBAF File Offset: 0x000BDDAF
	// (set) Token: 0x060029F2 RID: 10738 RVA: 0x001E45C8 File Offset: 0x001E27C8
	public KAnimFile[] AnimFiles
	{
		get
		{
			return this.animFiles;
		}
		set
		{
			DebugUtil.AssertArgs(value.Length != 0, new object[]
			{
				"Controller has no anim files.",
				base.gameObject
			});
			DebugUtil.AssertArgs(value[0] != null, new object[]
			{
				"First anim file needs to be non-null.",
				base.gameObject
			});
			DebugUtil.AssertArgs(value[0].IsBuildLoaded, new object[]
			{
				"First anim file needs to be the build file.",
				base.gameObject
			});
			for (int i = 0; i < value.Length; i++)
			{
				DebugUtil.AssertArgs(value[i] != null, new object[]
				{
					"Anim file is null",
					base.gameObject
				});
			}
			this.animFiles = new KAnimFile[value.Length];
			for (int j = 0; j < value.Length; j++)
			{
				this.animFiles[j] = value[j];
			}
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x060029F3 RID: 10739 RVA: 0x000BFBB7 File Offset: 0x000BDDB7
	public IReadOnlyList<KAnimControllerBase.OverrideAnimFileData> OverrideAnimFiles
	{
		get
		{
			return this.overrideAnimFiles;
		}
	}

	// Token: 0x060029F4 RID: 10740 RVA: 0x001E469C File Offset: 0x001E289C
	public void Stop()
	{
		if (this.curAnim != null)
		{
			this.StopAnimEventSequence();
		}
		this.animQueue.Clear();
		this.stopped = true;
		if (this.onAnimComplete != null)
		{
			this.onAnimComplete((this.curAnim == null) ? HashedString.Invalid : this.curAnim.hash);
		}
		this.OnStop();
	}

	// Token: 0x060029F5 RID: 10741 RVA: 0x001E46FC File Offset: 0x001E28FC
	public void StopAndClear()
	{
		if (!this.stopped)
		{
			this.Stop();
		}
		this.bounds.center = Vector3.zero;
		this.bounds.extents = Vector3.zero;
		if (this.OnUpdateBounds != null)
		{
			this.OnUpdateBounds(this.bounds);
		}
	}

	// Token: 0x060029F6 RID: 10742 RVA: 0x000BFBBF File Offset: 0x000BDDBF
	public float GetPositionPercent()
	{
		return this.GetElapsedTime() / this.GetDuration();
	}

	// Token: 0x060029F7 RID: 10743 RVA: 0x001E4750 File Offset: 0x001E2950
	public void SetPositionPercent(float percent)
	{
		if (this.curAnim == null)
		{
			return;
		}
		this.SetElapsedTime(percent * (float)this.curAnim.numFrames / this.curAnim.frameRate);
		int frameIdx = this.curAnim.GetFrameIdx(this.mode, this.elapsedTime);
		if (this.currentFrame != frameIdx)
		{
			this.SetDirty();
			this.UpdateAnimEventSequenceTime();
			this.SuspendUpdates(false);
		}
	}

	// Token: 0x060029F8 RID: 10744 RVA: 0x001E47BC File Offset: 0x001E29BC
	protected void StartAnimEventSequence()
	{
		if (!this.layering.GetIsForeground() && this.aem != null)
		{
			this.eventManagerHandle = this.aem.PlayAnim(this, this.curAnim, this.mode, this.elapsedTime, this.visibilityType == KAnimControllerBase.VisibilityType.Always);
		}
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x000BFBCE File Offset: 0x000BDDCE
	protected void UpdateAnimEventSequenceTime()
	{
		if (this.eventManagerHandle.IsValid() && this.aem != null)
		{
			this.aem.SetElapsedTime(this.eventManagerHandle, this.elapsedTime);
		}
	}

	// Token: 0x060029FA RID: 10746 RVA: 0x001E480C File Offset: 0x001E2A0C
	protected void StopAnimEventSequence()
	{
		if (this.eventManagerHandle.IsValid() && this.aem != null)
		{
			if (!this.stopped && this.mode != KAnim.PlayMode.Paused)
			{
				this.SetElapsedTime(this.aem.GetElapsedTime(this.eventManagerHandle));
			}
			this.aem.StopAnim(this.eventManagerHandle);
			this.eventManagerHandle = HandleVector<int>.InvalidHandle;
		}
	}

	// Token: 0x060029FB RID: 10747 RVA: 0x000BFBFC File Offset: 0x000BDDFC
	protected void DestroySelf()
	{
		if (this.onDestroySelf != null)
		{
			this.onDestroySelf(base.gameObject);
			return;
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060029FC RID: 10748 RVA: 0x000BFC23 File Offset: 0x000BDE23
	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		this.hiddenSymbols.Clear();
		this.hiddenSymbols = new List<KAnimHashedString>(this.hiddenSymbolsSet);
	}

	// Token: 0x060029FD RID: 10749 RVA: 0x000BFC41 File Offset: 0x000BDE41
	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		this.hiddenSymbolsSet = new HashSet<KAnimHashedString>(this.hiddenSymbols);
		this.hiddenSymbols.Clear();
	}

	// Token: 0x04001C3F RID: 7231
	[NonSerialized]
	public GameObject showWhenMissing;

	// Token: 0x04001C40 RID: 7232
	[SerializeField]
	public KAnimBatchGroup.MaterialType materialType;

	// Token: 0x04001C41 RID: 7233
	[SerializeField]
	public string initialAnim;

	// Token: 0x04001C42 RID: 7234
	[SerializeField]
	public KAnim.PlayMode initialMode = KAnim.PlayMode.Once;

	// Token: 0x04001C43 RID: 7235
	[SerializeField]
	protected KAnimFile[] animFiles = new KAnimFile[0];

	// Token: 0x04001C44 RID: 7236
	[SerializeField]
	protected Vector3 offset;

	// Token: 0x04001C45 RID: 7237
	[SerializeField]
	protected Vector3 pivot;

	// Token: 0x04001C46 RID: 7238
	[SerializeField]
	protected float rotation;

	// Token: 0x04001C47 RID: 7239
	[SerializeField]
	public bool destroyOnAnimComplete;

	// Token: 0x04001C48 RID: 7240
	[SerializeField]
	public bool inactiveDisable;

	// Token: 0x04001C49 RID: 7241
	[SerializeField]
	protected bool flipX;

	// Token: 0x04001C4A RID: 7242
	[SerializeField]
	protected bool flipY;

	// Token: 0x04001C4B RID: 7243
	[SerializeField]
	public bool forceUseGameTime;

	// Token: 0x04001C4C RID: 7244
	public string defaultAnim;

	// Token: 0x04001C4E RID: 7246
	protected KAnim.Anim curAnim;

	// Token: 0x04001C4F RID: 7247
	protected int curAnimFrameIdx = -1;

	// Token: 0x04001C50 RID: 7248
	protected int prevAnimFrame = -1;

	// Token: 0x04001C51 RID: 7249
	public bool usingNewSymbolOverrideSystem;

	// Token: 0x04001C53 RID: 7251
	protected HandleVector<int>.Handle eventManagerHandle = HandleVector<int>.InvalidHandle;

	// Token: 0x04001C54 RID: 7252
	protected List<KAnimControllerBase.OverrideAnimFileData> overrideAnimFiles = new List<KAnimControllerBase.OverrideAnimFileData>();

	// Token: 0x04001C55 RID: 7253
	protected DeepProfiler DeepProfiler = new DeepProfiler(false);

	// Token: 0x04001C56 RID: 7254
	public bool randomiseLoopedOffset;

	// Token: 0x04001C57 RID: 7255
	protected float elapsedTime;

	// Token: 0x04001C58 RID: 7256
	protected float playSpeed = 1f;

	// Token: 0x04001C59 RID: 7257
	protected KAnim.PlayMode mode = KAnim.PlayMode.Once;

	// Token: 0x04001C5A RID: 7258
	protected bool stopped = true;

	// Token: 0x04001C5B RID: 7259
	public float animHeight = 1f;

	// Token: 0x04001C5C RID: 7260
	public float animWidth = 1f;

	// Token: 0x04001C5D RID: 7261
	protected bool isVisible;

	// Token: 0x04001C5E RID: 7262
	protected Bounds bounds;

	// Token: 0x04001C5F RID: 7263
	public Action<Bounds> OnUpdateBounds;

	// Token: 0x04001C60 RID: 7264
	public Action<Color> OnTintChanged;

	// Token: 0x04001C61 RID: 7265
	public Action<Color> OnHighlightChanged;

	// Token: 0x04001C63 RID: 7267
	protected KAnimSynchronizer synchronizer;

	// Token: 0x04001C64 RID: 7268
	protected KAnimLayering layering;

	// Token: 0x04001C65 RID: 7269
	[SerializeField]
	protected bool _enabled = true;

	// Token: 0x04001C66 RID: 7270
	protected bool hasEnableRun;

	// Token: 0x04001C67 RID: 7271
	protected bool hasAwakeRun;

	// Token: 0x04001C68 RID: 7272
	protected KBatchedAnimInstanceData batchInstanceData;

	// Token: 0x04001C6B RID: 7275
	public KAnimControllerBase.VisibilityType visibilityType;

	// Token: 0x04001C6F RID: 7279
	public Action<GameObject> onDestroySelf;

	// Token: 0x04001C72 RID: 7282
	[SerializeField]
	protected List<KAnimHashedString> hiddenSymbols = new List<KAnimHashedString>();

	// Token: 0x04001C73 RID: 7283
	[SerializeField]
	protected HashSet<KAnimHashedString> hiddenSymbolsSet = new HashSet<KAnimHashedString>();

	// Token: 0x04001C74 RID: 7284
	protected Dictionary<HashedString, KAnimControllerBase.AnimLookupData> anims = new Dictionary<HashedString, KAnimControllerBase.AnimLookupData>();

	// Token: 0x04001C75 RID: 7285
	protected Dictionary<HashedString, KAnimControllerBase.AnimLookupData> overrideAnims = new Dictionary<HashedString, KAnimControllerBase.AnimLookupData>();

	// Token: 0x04001C76 RID: 7286
	protected Queue<KAnimControllerBase.AnimData> animQueue = new Queue<KAnimControllerBase.AnimData>();

	// Token: 0x04001C77 RID: 7287
	protected int maxSymbols;

	// Token: 0x04001C79 RID: 7289
	public Grid.SceneLayer fgLayer = Grid.SceneLayer.NoLayer;

	// Token: 0x04001C7A RID: 7290
	protected AnimEventManager aem;

	// Token: 0x04001C7B RID: 7291
	private static HashedString snaptoPivot = new HashedString("snapTo_pivot");

	// Token: 0x0200093F RID: 2367
	public struct OverrideAnimFileData
	{
		// Token: 0x04001C7C RID: 7292
		public float priority;

		// Token: 0x04001C7D RID: 7293
		public KAnimFile file;
	}

	// Token: 0x02000940 RID: 2368
	public struct AnimLookupData
	{
		// Token: 0x04001C7E RID: 7294
		public int animIndex;
	}

	// Token: 0x02000941 RID: 2369
	public struct AnimData
	{
		// Token: 0x04001C7F RID: 7295
		public HashedString anim;

		// Token: 0x04001C80 RID: 7296
		public KAnim.PlayMode mode;

		// Token: 0x04001C81 RID: 7297
		public float speed;

		// Token: 0x04001C82 RID: 7298
		public float timeOffset;
	}

	// Token: 0x02000942 RID: 2370
	public enum VisibilityType
	{
		// Token: 0x04001C84 RID: 7300
		Default,
		// Token: 0x04001C85 RID: 7301
		OffscreenUpdate,
		// Token: 0x04001C86 RID: 7302
		Always
	}

	// Token: 0x02000943 RID: 2371
	// (Invoke) Token: 0x06002A00 RID: 10752
	public delegate void KAnimEvent(HashedString name);
}
