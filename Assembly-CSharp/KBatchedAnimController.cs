using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200094B RID: 2379
[DebuggerDisplay("{name} visible={isVisible} suspendUpdates={suspendUpdates} moving={moving}")]
public class KBatchedAnimController : KAnimControllerBase, KAnimConverter.IAnimConverter
{
	// Token: 0x06002A32 RID: 10802 RVA: 0x000BFF17 File Offset: 0x000BE117
	public int GetCurrentFrameIndex()
	{
		return this.curAnimFrameIdx;
	}

	// Token: 0x06002A33 RID: 10803 RVA: 0x000BFF1F File Offset: 0x000BE11F
	public KBatchedAnimInstanceData GetBatchInstanceData()
	{
		return this.batchInstanceData;
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06002A34 RID: 10804 RVA: 0x000BFF27 File Offset: 0x000BE127
	// (set) Token: 0x06002A35 RID: 10805 RVA: 0x000BFF2F File Offset: 0x000BE12F
	protected bool forceRebuild
	{
		get
		{
			return this._forceRebuild;
		}
		set
		{
			this._forceRebuild = value;
		}
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06002A36 RID: 10806 RVA: 0x000BFF38 File Offset: 0x000BE138
	public bool IsMoving
	{
		get
		{
			return this.moving;
		}
	}

	// Token: 0x06002A37 RID: 10807 RVA: 0x001E5460 File Offset: 0x001E3660
	public KBatchedAnimController()
	{
		this.batchInstanceData = new KBatchedAnimInstanceData(this);
	}

	// Token: 0x06002A38 RID: 10808 RVA: 0x000BFF40 File Offset: 0x000BE140
	public bool IsActive()
	{
		return base.isActiveAndEnabled && this._enabled;
	}

	// Token: 0x06002A39 RID: 10809 RVA: 0x000BFF52 File Offset: 0x000BE152
	public bool IsVisible()
	{
		return this.isVisible;
	}

	// Token: 0x06002A3A RID: 10810 RVA: 0x001E54E0 File Offset: 0x001E36E0
	public Vector4 GetPositionData()
	{
		if (this.getPositionDataFunctionInUse != null)
		{
			return this.getPositionDataFunctionInUse();
		}
		Vector3 position = base.transform.GetPosition();
		Vector3 positionIncludingOffset = base.PositionIncludingOffset;
		return new Vector4(position.x, position.y, positionIncludingOffset.x, positionIncludingOffset.y);
	}

	// Token: 0x06002A3B RID: 10811 RVA: 0x001E5534 File Offset: 0x001E3734
	public void SetSymbolScale(KAnimHashedString symbol_name, float scale)
	{
		KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(this.GetBatchGroupID(false)).GetSymbol(symbol_name);
		if (symbol == null)
		{
			return;
		}
		base.symbolInstanceGpuData.SetSymbolScale(symbol.symbolIndexInSourceBuild, scale);
		this.SuspendUpdates(false);
		this.SetDirty();
	}

	// Token: 0x06002A3C RID: 10812 RVA: 0x001E557C File Offset: 0x001E377C
	public void SetSymbolTint(KAnimHashedString symbol_name, Color color)
	{
		KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(this.GetBatchGroupID(false)).GetSymbol(symbol_name);
		if (symbol == null)
		{
			return;
		}
		base.symbolInstanceGpuData.SetSymbolTint(symbol.symbolIndexInSourceBuild, color);
		this.SuspendUpdates(false);
		this.SetDirty();
	}

	// Token: 0x06002A3D RID: 10813 RVA: 0x001E55C4 File Offset: 0x001E37C4
	public Vector2I GetCellXY()
	{
		Vector3 positionIncludingOffset = base.PositionIncludingOffset;
		if (Grid.CellSizeInMeters == 0f)
		{
			return new Vector2I((int)positionIncludingOffset.x, (int)positionIncludingOffset.y);
		}
		return Grid.PosToXY(positionIncludingOffset);
	}

	// Token: 0x06002A3E RID: 10814 RVA: 0x000BFF5A File Offset: 0x000BE15A
	public float GetZ()
	{
		return base.transform.GetPosition().z;
	}

	// Token: 0x06002A3F RID: 10815 RVA: 0x000BFF6C File Offset: 0x000BE16C
	public string GetName()
	{
		return base.name;
	}

	// Token: 0x06002A40 RID: 10816 RVA: 0x001E5600 File Offset: 0x001E3800
	public override KAnim.Anim GetAnim(int index)
	{
		if (!this.batchGroupID.IsValid || !(this.batchGroupID != KAnimBatchManager.NO_BATCH))
		{
			global::Debug.LogError(base.name + " batch not ready");
		}
		KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.batchGroupID);
		global::Debug.Assert(batchGroupData != null);
		return batchGroupData.GetAnim(index);
	}

	// Token: 0x06002A41 RID: 10817 RVA: 0x001E5664 File Offset: 0x001E3864
	private void Initialize()
	{
		if (this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH)
		{
			this.DeRegister();
			this.Register();
		}
	}

	// Token: 0x06002A42 RID: 10818 RVA: 0x000BFF74 File Offset: 0x000BE174
	private void OnMovementStateChanged(bool is_moving)
	{
		if (is_moving == this.moving)
		{
			return;
		}
		this.moving = is_moving;
		this.SetDirty();
		this.ConfigureUpdateListener();
	}

	// Token: 0x06002A43 RID: 10819 RVA: 0x000BFF93 File Offset: 0x000BE193
	private static void OnMovementStateChanged(Transform transform, bool is_moving)
	{
		transform.GetComponent<KBatchedAnimController>().OnMovementStateChanged(is_moving);
	}

	// Token: 0x06002A44 RID: 10820 RVA: 0x001E56A0 File Offset: 0x001E38A0
	private void SetBatchGroup(KAnimFileData kafd)
	{
		if (this.batchGroupID.IsValid && kafd != null && this.batchGroupID == kafd.batchTag)
		{
			return;
		}
		DebugUtil.Assert(!this.batchGroupID.IsValid, "Should only be setting the batch group once.");
		DebugUtil.Assert(kafd != null, "Null anim data!! For", base.name);
		base.curBuild = kafd.build;
		DebugUtil.Assert(base.curBuild != null, "Null build for anim!! ", base.name, kafd.name);
		KAnimGroupFile.Group group = KAnimGroupFile.GetGroup(base.curBuild.batchTag);
		HashedString batchGroupID = kafd.build.batchTag;
		if (group.renderType == KAnimBatchGroup.RendererType.DontRender || group.renderType == KAnimBatchGroup.RendererType.AnimOnly)
		{
			bool isValid = group.swapTarget.IsValid;
			string str = "Invalid swap target fro group [";
			HashedString id = group.id;
			global::Debug.Assert(isValid, str + id.ToString() + "]");
			batchGroupID = group.swapTarget;
		}
		this.batchGroupID = batchGroupID;
		base.symbolInstanceGpuData = new SymbolInstanceGpuData(KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID).maxSymbolsPerBuild);
		base.symbolOverrideInfoGpuData = new SymbolOverrideInfoGpuData(KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID).symbolFrameInstances.Count);
		if (!this.batchGroupID.IsValid || this.batchGroupID == KAnimBatchManager.NO_BATCH)
		{
			global::Debug.LogError("Batch is not ready: " + base.name);
		}
		if (this.materialType == KAnimBatchGroup.MaterialType.Default && this.batchGroupID == KAnimBatchManager.BATCH_HUMAN)
		{
			this.materialType = KAnimBatchGroup.MaterialType.Human;
		}
	}

	// Token: 0x06002A45 RID: 10821 RVA: 0x001E583C File Offset: 0x001E3A3C
	public void LoadAnims()
	{
		if (!KAnimBatchManager.Instance().isReady)
		{
			global::Debug.LogError("KAnimBatchManager is not ready when loading anim:" + base.name);
		}
		if (this.animFiles.Length == 0)
		{
			DebugUtil.Assert(false, "KBatchedAnimController has no anim files:" + base.name);
		}
		if (!this.animFiles[0].IsBuildLoaded)
		{
			DebugUtil.LogErrorArgs(base.gameObject, new object[]
			{
				string.Format("First anim file needs to be the build file but {0} doesn't have an associated build", this.animFiles[0].GetData().name)
			});
		}
		this.overrideAnims.Clear();
		this.anims.Clear();
		this.SetBatchGroup(this.animFiles[0].GetData());
		for (int i = 0; i < this.animFiles.Length; i++)
		{
			base.AddAnims(this.animFiles[i]);
		}
		this.forceRebuild = true;
		if (this.layering != null)
		{
			this.layering.HideSymbols();
		}
		if (this.usingNewSymbolOverrideSystem)
		{
			DebugUtil.Assert(base.GetComponent<SymbolOverrideController>() != null);
		}
	}

	// Token: 0x06002A46 RID: 10822 RVA: 0x001E5948 File Offset: 0x001E3B48
	public void SwapAnims(KAnimFile[] anims)
	{
		if (this.batchGroupID.IsValid)
		{
			this.DeRegister();
			this.batchGroupID = HashedString.Invalid;
		}
		base.AnimFiles = anims;
		this.LoadAnims();
		if (base.curBuild != null)
		{
			this.UpdateHiddenSymbolSet(this.hiddenSymbolsSet);
		}
		this.Register();
	}

	// Token: 0x06002A47 RID: 10823 RVA: 0x001E59A0 File Offset: 0x001E3BA0
	public void UpdateAnim(float dt)
	{
		if (this.batch != null && base.transform.hasChanged)
		{
			base.transform.hasChanged = false;
			if (this.batch != null && this.batch.group.maxGroupSize == 1 && this.lastPos.z != base.transform.GetPosition().z)
			{
				this.batch.OverrideZ(base.transform.GetPosition().z);
			}
			Vector3 positionIncludingOffset = base.PositionIncludingOffset;
			this.lastPos = positionIncludingOffset;
			if (this.visibilityType != KAnimControllerBase.VisibilityType.Always && KAnimBatchManager.ControllerToChunkXY(this) != this.lastChunkXY && this.lastChunkXY != KBatchedAnimUpdater.INVALID_CHUNK_ID)
			{
				this.DeRegister();
				this.Register();
			}
			this.SetDirty();
		}
		if (this.batchGroupID == KAnimBatchManager.NO_BATCH || !this.IsActive())
		{
			return;
		}
		if (!this.forceRebuild && (this.mode == KAnim.PlayMode.Paused || this.stopped || this.curAnim == null || (this.mode == KAnim.PlayMode.Once && this.curAnim != null && (this.elapsedTime > this.curAnim.totalTime || this.curAnim.totalTime <= 0f) && this.animQueue.Count == 0)))
		{
			this.SuspendUpdates(true);
		}
		if (!this.isVisible && !this.forceRebuild)
		{
			if (this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate && !this.stopped && this.mode != KAnim.PlayMode.Paused)
			{
				base.SetElapsedTime(this.elapsedTime + dt * this.playSpeed);
			}
			return;
		}
		this.curAnimFrameIdx = base.GetFrameIdx(this.elapsedTime, true);
		if (this.eventManagerHandle.IsValid() && this.aem != null)
		{
			float elapsedTime = this.aem.GetElapsedTime(this.eventManagerHandle);
			if ((int)((this.elapsedTime - elapsedTime) * 100f) != 0)
			{
				base.UpdateAnimEventSequenceTime();
			}
		}
		this.UpdateFrame(this.elapsedTime);
		if (!this.stopped && this.mode != KAnim.PlayMode.Paused)
		{
			base.SetElapsedTime(this.elapsedTime + dt * this.playSpeed);
		}
		this.forceRebuild = false;
	}

	// Token: 0x06002A48 RID: 10824 RVA: 0x001E5BC8 File Offset: 0x001E3DC8
	protected override void UpdateFrame(float t)
	{
		base.previousFrame = base.currentFrame;
		if (!this.stopped || this.forceRebuild)
		{
			if (this.curAnim != null && (this.mode == KAnim.PlayMode.Loop || this.elapsedTime <= base.GetDuration() || this.forceRebuild))
			{
				base.currentFrame = this.curAnim.GetFrameIdx(this.mode, this.elapsedTime);
				if (base.currentFrame != base.previousFrame || this.forceRebuild)
				{
					this.SetDirty();
				}
			}
			else
			{
				this.TriggerStop();
			}
			if (!this.stopped && this.mode == KAnim.PlayMode.Loop && base.currentFrame == 0)
			{
				base.AnimEnter(this.curAnim.hash);
			}
		}
		if (this.synchronizer != null)
		{
			this.synchronizer.SyncTime();
		}
	}

	// Token: 0x06002A49 RID: 10825 RVA: 0x001E5C98 File Offset: 0x001E3E98
	public override void TriggerStop()
	{
		if (this.animQueue.Count > 0)
		{
			base.StartQueuedAnim();
			return;
		}
		if (this.curAnim != null && this.mode == KAnim.PlayMode.Once)
		{
			base.currentFrame = this.curAnim.numFrames - 1;
			base.Stop();
			base.gameObject.Trigger(-1061186183, null);
			if (this.destroyOnAnimComplete)
			{
				base.DestroySelf();
			}
		}
	}

	// Token: 0x06002A4A RID: 10826 RVA: 0x001E5D04 File Offset: 0x001E3F04
	public override void UpdateHiddenSymbol(KAnimHashedString symbolToUpdate)
	{
		KBatchGroupData batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID);
		for (int i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
		{
			if (!(symbolToUpdate != batchGroupData.frameElementSymbols[i].hash))
			{
				KAnim.Build.Symbol symbol = batchGroupData.frameElementSymbols[i];
				bool is_visible = !this.hiddenSymbolsSet.Contains(symbol.hash);
				base.symbolInstanceGpuData.SetVisible(i, is_visible);
			}
		}
		this.SetDirty();
	}

	// Token: 0x06002A4B RID: 10827 RVA: 0x001E5D88 File Offset: 0x001E3F88
	public override void UpdateHiddenSymbolSet(HashSet<KAnimHashedString> symbolsToUpdate)
	{
		KBatchGroupData batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID);
		for (int i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
		{
			if (symbolsToUpdate.Contains(batchGroupData.frameElementSymbols[i].hash))
			{
				KAnim.Build.Symbol symbol = batchGroupData.frameElementSymbols[i];
				bool is_visible = !this.hiddenSymbolsSet.Contains(symbol.hash);
				base.symbolInstanceGpuData.SetVisible(i, is_visible);
			}
		}
		this.SetDirty();
	}

	// Token: 0x06002A4C RID: 10828 RVA: 0x001E5E0C File Offset: 0x001E400C
	public override void UpdateAllHiddenSymbols()
	{
		KBatchGroupData batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID);
		for (int i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
		{
			KAnim.Build.Symbol symbol = batchGroupData.frameElementSymbols[i];
			bool is_visible = !this.hiddenSymbolsSet.Contains(symbol.hash);
			base.symbolInstanceGpuData.SetVisible(i, is_visible);
		}
		this.SetDirty();
	}

	// Token: 0x06002A4D RID: 10829 RVA: 0x000BFFA1 File Offset: 0x000BE1A1
	public int GetMaxVisible()
	{
		return this.maxSymbols;
	}

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06002A4E RID: 10830 RVA: 0x000BFFA9 File Offset: 0x000BE1A9
	// (set) Token: 0x06002A4F RID: 10831 RVA: 0x000BFFB1 File Offset: 0x000BE1B1
	public HashedString batchGroupID { get; private set; }

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06002A50 RID: 10832 RVA: 0x000BFFBA File Offset: 0x000BE1BA
	// (set) Token: 0x06002A51 RID: 10833 RVA: 0x000BFFC2 File Offset: 0x000BE1C2
	public HashedString batchGroupIDOverride { get; private set; }

	// Token: 0x06002A52 RID: 10834 RVA: 0x001E5E78 File Offset: 0x001E4078
	public HashedString GetBatchGroupID(bool isEditorWindow = false)
	{
		global::Debug.Assert(isEditorWindow || this.animFiles == null || this.animFiles.Length == 0 || (this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH));
		return this.batchGroupID;
	}

	// Token: 0x06002A53 RID: 10835 RVA: 0x000BFFCB File Offset: 0x000BE1CB
	public HashedString GetBatchGroupIDOverride()
	{
		return this.batchGroupIDOverride;
	}

	// Token: 0x06002A54 RID: 10836 RVA: 0x000BFFD3 File Offset: 0x000BE1D3
	public void SetBatchGroupOverride(HashedString id)
	{
		this.batchGroupIDOverride = id;
		this.DeRegister();
		this.Register();
	}

	// Token: 0x06002A55 RID: 10837 RVA: 0x000BFFE8 File Offset: 0x000BE1E8
	public int GetLayer()
	{
		return base.gameObject.layer;
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x000BFFF5 File Offset: 0x000BE1F5
	public KAnimBatch GetBatch()
	{
		return this.batch;
	}

	// Token: 0x06002A57 RID: 10839 RVA: 0x001E5ED0 File Offset: 0x001E40D0
	public void SetBatch(KAnimBatch new_batch)
	{
		this.batch = new_batch;
		if (this.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			KBatchedAnimCanvasRenderer kbatchedAnimCanvasRenderer = base.GetComponent<KBatchedAnimCanvasRenderer>();
			if (kbatchedAnimCanvasRenderer == null && new_batch != null)
			{
				kbatchedAnimCanvasRenderer = base.gameObject.AddComponent<KBatchedAnimCanvasRenderer>();
			}
			if (kbatchedAnimCanvasRenderer != null)
			{
				kbatchedAnimCanvasRenderer.SetBatch(this);
			}
		}
	}

	// Token: 0x06002A58 RID: 10840 RVA: 0x000BFFFD File Offset: 0x000BE1FD
	public int GetCurrentNumFrames()
	{
		if (this.curAnim == null)
		{
			return 0;
		}
		return this.curAnim.numFrames;
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x000C0014 File Offset: 0x000BE214
	public int GetFirstFrameIndex()
	{
		if (this.curAnim == null)
		{
			return -1;
		}
		return this.curAnim.firstFrameIdx;
	}

	// Token: 0x06002A5A RID: 10842 RVA: 0x001E5F1C File Offset: 0x001E411C
	private Canvas GetRootCanvas()
	{
		if (this.rt == null)
		{
			return null;
		}
		RectTransform component = this.rt.parent.GetComponent<RectTransform>();
		while (component != null)
		{
			Canvas component2 = component.GetComponent<Canvas>();
			if (component2 != null && component2.isRootCanvas)
			{
				return component2;
			}
			component = component.parent.GetComponent<RectTransform>();
		}
		return null;
	}

	// Token: 0x06002A5B RID: 10843 RVA: 0x001E5F80 File Offset: 0x001E4180
	public override Matrix2x3 GetTransformMatrix()
	{
		Vector3 v = base.PositionIncludingOffset;
		v.z = 0f;
		Vector2 scale = new Vector2(this.animScale * this.animWidth, -this.animScale * this.animHeight);
		if (this.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			this.rt = base.GetComponent<RectTransform>();
			if (this.rootCanvas == null)
			{
				this.rootCanvas = this.GetRootCanvas();
			}
			if (this.scaler == null && this.rootCanvas != null)
			{
				this.scaler = this.rootCanvas.GetComponent<CanvasScaler>();
			}
			if (this.rootCanvas == null)
			{
				this.screenOffset.x = (float)(Screen.width / 2);
				this.screenOffset.y = (float)(Screen.height / 2);
			}
			else
			{
				this.screenOffset.x = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.width / 2f));
				this.screenOffset.y = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.height / 2f));
			}
			float num = 1f;
			if (this.scaler != null)
			{
				num = 1f / this.scaler.scaleFactor;
			}
			v = (this.rt.localToWorldMatrix.MultiplyPoint(this.rt.pivot) + this.offset) * num - this.screenOffset;
			float num2 = this.animWidth * this.animScale;
			float num3 = this.animHeight * this.animScale;
			if (this.setScaleFromAnim && this.curAnim != null)
			{
				num2 *= this.rt.rect.size.x / this.curAnim.unScaledSize.x;
				num3 *= this.rt.rect.size.y / this.curAnim.unScaledSize.y;
			}
			else
			{
				num2 *= this.rt.rect.size.x / this.animOverrideSize.x;
				num3 *= this.rt.rect.size.y / this.animOverrideSize.y;
			}
			scale = new Vector3(this.rt.lossyScale.x * num2 * num, -this.rt.lossyScale.y * num3 * num, this.rt.lossyScale.z * num);
			this.pivot = this.rt.pivot;
		}
		Matrix2x3 n = Matrix2x3.Scale(scale);
		Matrix2x3 n2 = Matrix2x3.Scale(new Vector2(this.flipX ? -1f : 1f, this.flipY ? -1f : 1f));
		Matrix2x3 result;
		if (this.rotation != 0f)
		{
			Matrix2x3 n3 = Matrix2x3.Translate(-this.pivot);
			Matrix2x3 n4 = Matrix2x3.Rotate(this.rotation * 0.017453292f);
			Matrix2x3 n5 = Matrix2x3.Translate(this.pivot) * n4 * n3;
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n5 * n * this.navMatrix * n2;
		}
		else
		{
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n * this.navMatrix * n2;
		}
		return result;
	}

	// Token: 0x06002A5C RID: 10844 RVA: 0x001E63A0 File Offset: 0x001E45A0
	public Matrix2x3 GetTransformMatrix(Vector2 customScale)
	{
		Vector3 v = base.PositionIncludingOffset;
		v.z = 0f;
		Vector2 scale = customScale;
		if (this.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			this.rt = base.GetComponent<RectTransform>();
			if (this.rootCanvas == null)
			{
				this.rootCanvas = this.GetRootCanvas();
			}
			if (this.scaler == null && this.rootCanvas != null)
			{
				this.scaler = this.rootCanvas.GetComponent<CanvasScaler>();
			}
			if (this.rootCanvas == null)
			{
				this.screenOffset.x = (float)(Screen.width / 2);
				this.screenOffset.y = (float)(Screen.height / 2);
			}
			else
			{
				this.screenOffset.x = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.width / 2f));
				this.screenOffset.y = ((this.rootCanvas.renderMode == RenderMode.WorldSpace) ? 0f : (this.rootCanvas.rectTransform().rect.height / 2f));
			}
			float num = 1f;
			if (this.scaler != null)
			{
				num = 1f / this.scaler.scaleFactor;
			}
			v = (this.rt.localToWorldMatrix.MultiplyPoint(this.rt.pivot) + this.offset) * num - this.screenOffset;
			float num2 = this.animWidth * this.animScale;
			float num3 = this.animHeight * this.animScale;
			if (this.setScaleFromAnim && this.curAnim != null)
			{
				num2 *= this.rt.rect.size.x / this.curAnim.unScaledSize.x;
				num3 *= this.rt.rect.size.y / this.curAnim.unScaledSize.y;
			}
			else
			{
				num2 *= this.rt.rect.size.x / this.animOverrideSize.x;
				num3 *= this.rt.rect.size.y / this.animOverrideSize.y;
			}
			scale = new Vector3(this.rt.lossyScale.x * num2 * num, -this.rt.lossyScale.y * num3 * num, this.rt.lossyScale.z * num);
			this.pivot = this.rt.pivot;
		}
		Matrix2x3 n = Matrix2x3.Scale(scale);
		Matrix2x3 n2 = Matrix2x3.Scale(new Vector2(this.flipX ? -1f : 1f, this.flipY ? -1f : 1f));
		Matrix2x3 result;
		if (this.rotation != 0f)
		{
			Matrix2x3 n3 = Matrix2x3.Translate(-this.pivot);
			Matrix2x3 n4 = Matrix2x3.Rotate(this.rotation * 0.017453292f);
			Matrix2x3 n5 = Matrix2x3.Translate(this.pivot) * n4 * n3;
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n5 * n * this.navMatrix * n2;
		}
		else
		{
			result = Matrix2x3.TRS(v, base.transform.rotation, base.transform.localScale) * n * this.navMatrix * n2;
		}
		return result;
	}

	// Token: 0x06002A5D RID: 10845 RVA: 0x001E67A0 File Offset: 0x001E49A0
	public override Matrix4x4 GetSymbolTransform(HashedString symbol, out bool symbolVisible)
	{
		if (this.curAnimFrameIdx != -1 && this.batch != null)
		{
			Matrix2x3 symbolLocalTransform = this.GetSymbolLocalTransform(symbol, out symbolVisible);
			if (symbolVisible)
			{
				return this.GetTransformMatrix() * symbolLocalTransform;
			}
		}
		symbolVisible = false;
		return default(Matrix4x4);
	}

	// Token: 0x06002A5E RID: 10846 RVA: 0x001E67F0 File Offset: 0x001E49F0
	public override Matrix2x3 GetSymbolLocalTransform(HashedString symbol, out bool symbolVisible)
	{
		KAnim.Anim.Frame frame;
		if (this.curAnimFrameIdx != -1 && this.batch != null && this.batch.group.data.TryGetFrame(this.curAnimFrameIdx, out frame))
		{
			for (int i = 0; i < frame.numElements; i++)
			{
				int num = frame.firstElementIdx + i;
				if (num < this.batch.group.data.frameElements.Count)
				{
					KAnim.Anim.FrameElement frameElement = this.batch.group.data.frameElements[num];
					if (frameElement.symbol == symbol)
					{
						symbolVisible = true;
						return frameElement.transform;
					}
				}
			}
		}
		symbolVisible = false;
		return Matrix2x3.identity;
	}

	// Token: 0x06002A5F RID: 10847 RVA: 0x000C002B File Offset: 0x000BE22B
	public override void SetLayer(int layer)
	{
		if (layer == base.gameObject.layer)
		{
			return;
		}
		base.SetLayer(layer);
		this.DeRegister();
		base.gameObject.layer = layer;
		this.Register();
	}

	// Token: 0x06002A60 RID: 10848 RVA: 0x000C005B File Offset: 0x000BE25B
	public override void SetDirty()
	{
		if (this.batch != null)
		{
			this.batch.SetDirty(this);
		}
	}

	// Token: 0x06002A61 RID: 10849 RVA: 0x000C0071 File Offset: 0x000BE271
	protected override void OnStartQueuedAnim()
	{
		this.SuspendUpdates(false);
	}

	// Token: 0x06002A62 RID: 10850 RVA: 0x001E68A8 File Offset: 0x001E4AA8
	protected override void OnAwake()
	{
		this.LoadAnims();
		if (this.visibilityType == KAnimControllerBase.VisibilityType.Default)
		{
			this.visibilityType = ((this.materialType == KAnimBatchGroup.MaterialType.UI) ? KAnimControllerBase.VisibilityType.Always : this.visibilityType);
		}
		if (this.materialType == KAnimBatchGroup.MaterialType.Default && this.batchGroupID == KAnimBatchManager.BATCH_HUMAN)
		{
			this.materialType = KAnimBatchGroup.MaterialType.Human;
		}
		this.symbolOverrideController = base.GetComponent<SymbolOverrideController>();
		this.UpdateAllHiddenSymbols();
		this.hasEnableRun = false;
	}

	// Token: 0x06002A63 RID: 10851 RVA: 0x001E6918 File Offset: 0x001E4B18
	protected override void OnStart()
	{
		if (this.batch == null)
		{
			this.Initialize();
		}
		if (this.visibilityType == KAnimControllerBase.VisibilityType.Always || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate)
		{
			this.ConfigureUpdateListener();
		}
		CellChangeMonitor instance = Singleton<CellChangeMonitor>.Instance;
		if (instance != null)
		{
			instance.RegisterMovementStateChanged(base.transform, new Action<Transform, bool>(KBatchedAnimController.OnMovementStateChanged));
			this.moving = instance.IsMoving(base.transform);
		}
		this.symbolOverrideController = base.GetComponent<SymbolOverrideController>();
		this.SetDirty();
	}

	// Token: 0x06002A64 RID: 10852 RVA: 0x000C007A File Offset: 0x000BE27A
	protected override void OnStop()
	{
		this.SetDirty();
	}

	// Token: 0x06002A65 RID: 10853 RVA: 0x000C0082 File Offset: 0x000BE282
	private void OnEnable()
	{
		if (this._enabled)
		{
			this.Enable();
		}
	}

	// Token: 0x06002A66 RID: 10854 RVA: 0x001E6990 File Offset: 0x001E4B90
	protected override void Enable()
	{
		if (this.hasEnableRun)
		{
			return;
		}
		this.hasEnableRun = true;
		if (this.batch == null)
		{
			this.Initialize();
		}
		this.SetDirty();
		this.SuspendUpdates(false);
		this.ConfigureVisibilityListener(true);
		if (!this.stopped && this.curAnim != null && this.mode != KAnim.PlayMode.Paused && !this.eventManagerHandle.IsValid())
		{
			base.StartAnimEventSequence();
		}
	}

	// Token: 0x06002A67 RID: 10855 RVA: 0x000C0092 File Offset: 0x000BE292
	private void OnDisable()
	{
		this.Disable();
	}

	// Token: 0x06002A68 RID: 10856 RVA: 0x001E69FC File Offset: 0x001E4BFC
	protected override void Disable()
	{
		if (App.IsExiting || KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		if (!this.hasEnableRun)
		{
			return;
		}
		this.hasEnableRun = false;
		this.SuspendUpdates(true);
		if (this.batch != null)
		{
			this.DeRegister();
		}
		this.ConfigureVisibilityListener(false);
		base.StopAnimEventSequence();
	}

	// Token: 0x06002A69 RID: 10857 RVA: 0x001E6A4C File Offset: 0x001E4C4C
	protected override void OnDestroy()
	{
		if (App.IsExiting)
		{
			return;
		}
		CellChangeMonitor instance = Singleton<CellChangeMonitor>.Instance;
		if (instance != null)
		{
			instance.UnregisterMovementStateChanged(base.transform, new Action<Transform, bool>(KBatchedAnimController.OnMovementStateChanged));
		}
		KBatchedAnimUpdater instance2 = Singleton<KBatchedAnimUpdater>.Instance;
		if (instance2 != null)
		{
			instance2.UpdateUnregister(this);
		}
		this.isVisible = false;
		this.DeRegister();
		this.stopped = true;
		base.StopAnimEventSequence();
		this.batchInstanceData = null;
		this.batch = null;
		base.OnDestroy();
	}

	// Token: 0x06002A6A RID: 10858 RVA: 0x000C009A File Offset: 0x000BE29A
	public void SetBlendValue(float value)
	{
		this.batchInstanceData.SetBlend(value);
		this.SetDirty();
	}

	// Token: 0x06002A6B RID: 10859 RVA: 0x000C00AE File Offset: 0x000BE2AE
	public SymbolOverrideController SetupSymbolOverriding()
	{
		if (!this.symbolOverrideController.IsNullOrDestroyed())
		{
			return this.symbolOverrideController;
		}
		this.usingNewSymbolOverrideSystem = true;
		this.symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(base.gameObject);
		return this.symbolOverrideController;
	}

	// Token: 0x06002A6C RID: 10860 RVA: 0x001E6AC0 File Offset: 0x001E4CC0
	public bool ApplySymbolOverrides()
	{
		this.batch.atlases.Apply(this.batch.matProperties);
		if (this.symbolOverrideController != null)
		{
			if (this.symbolOverrideControllerVersion != this.symbolOverrideController.version || this.symbolOverrideController.applySymbolOverridesEveryFrame)
			{
				this.symbolOverrideControllerVersion = this.symbolOverrideController.version;
				this.symbolOverrideController.ApplyOverrides();
			}
			this.symbolOverrideController.ApplyAtlases();
			return true;
		}
		return false;
	}

	// Token: 0x06002A6D RID: 10861 RVA: 0x000C00E2 File Offset: 0x000BE2E2
	public void SetSymbolOverrides(int symbol_start_idx, int symbol_num_frames, int atlas_idx, KBatchGroupData source_data, int source_start_idx, int source_num_frames)
	{
		base.symbolOverrideInfoGpuData.SetSymbolOverrideInfo(symbol_start_idx, symbol_num_frames, atlas_idx, source_data, source_start_idx, source_num_frames);
	}

	// Token: 0x06002A6E RID: 10862 RVA: 0x000C00F8 File Offset: 0x000BE2F8
	public void SetSymbolOverride(int symbol_idx, ref KAnim.Build.SymbolFrameInstance symbol_frame_instance)
	{
		base.symbolOverrideInfoGpuData.SetSymbolOverrideInfo(symbol_idx, ref symbol_frame_instance);
	}

	// Token: 0x06002A6F RID: 10863 RVA: 0x001E6B40 File Offset: 0x001E4D40
	protected override void Register()
	{
		if (!this.IsActive())
		{
			return;
		}
		if (this.batch != null)
		{
			return;
		}
		if (this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH)
		{
			this.lastChunkXY = KAnimBatchManager.ControllerToChunkXY(this);
			KAnimBatchManager.Instance().Register(this);
			this.forceRebuild = true;
			this.SetDirty();
		}
	}

	// Token: 0x06002A70 RID: 10864 RVA: 0x000C0107 File Offset: 0x000BE307
	protected override void DeRegister()
	{
		if (this.batch != null)
		{
			this.batch.Deregister(this);
		}
	}

	// Token: 0x06002A71 RID: 10865 RVA: 0x001E6BA8 File Offset: 0x001E4DA8
	private void ConfigureUpdateListener()
	{
		if ((this.IsActive() && !this.suspendUpdates && this.isVisible) || this.moving || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate || this.visibilityType == KAnimControllerBase.VisibilityType.Always)
		{
			Singleton<KBatchedAnimUpdater>.Instance.UpdateRegister(this);
			return;
		}
		Singleton<KBatchedAnimUpdater>.Instance.UpdateUnregister(this);
	}

	// Token: 0x06002A72 RID: 10866 RVA: 0x000C011D File Offset: 0x000BE31D
	protected override void SuspendUpdates(bool suspend)
	{
		this.suspendUpdates = suspend;
		this.ConfigureUpdateListener();
	}

	// Token: 0x06002A73 RID: 10867 RVA: 0x000C012C File Offset: 0x000BE32C
	public void SetVisiblity(bool is_visible)
	{
		if (is_visible != this.isVisible)
		{
			this.isVisible = is_visible;
			if (is_visible)
			{
				this.SuspendUpdates(false);
				this.SetDirty();
				base.UpdateAnimEventSequenceTime();
				return;
			}
			this.SuspendUpdates(true);
			this.SetDirty();
		}
	}

	// Token: 0x06002A74 RID: 10868 RVA: 0x000C0162 File Offset: 0x000BE362
	private void ConfigureVisibilityListener(bool enabled)
	{
		if (this.visibilityType == KAnimControllerBase.VisibilityType.Always || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate)
		{
			return;
		}
		if (enabled)
		{
			this.RegisterVisibilityListener();
			return;
		}
		this.UnregisterVisibilityListener();
	}

	// Token: 0x06002A75 RID: 10869 RVA: 0x000C0187 File Offset: 0x000BE387
	public virtual KAnimConverter.PostProcessingEffects GetPostProcessingEffectsCompatibility()
	{
		return this.postProcessingEffectsAllowed;
	}

	// Token: 0x06002A76 RID: 10870 RVA: 0x000C018F File Offset: 0x000BE38F
	public float GetPostProcessingParams()
	{
		return this.postProcessingParameters;
	}

	// Token: 0x06002A77 RID: 10871 RVA: 0x000C0197 File Offset: 0x000BE397
	protected override void RefreshVisibilityListener()
	{
		if (!this.visibilityListenerRegistered)
		{
			return;
		}
		this.ConfigureVisibilityListener(false);
		this.ConfigureVisibilityListener(true);
	}

	// Token: 0x06002A78 RID: 10872 RVA: 0x000C01B0 File Offset: 0x000BE3B0
	private void RegisterVisibilityListener()
	{
		DebugUtil.Assert(!this.visibilityListenerRegistered);
		Singleton<KBatchedAnimUpdater>.Instance.VisibilityRegister(this);
		this.visibilityListenerRegistered = true;
	}

	// Token: 0x06002A79 RID: 10873 RVA: 0x000C01D2 File Offset: 0x000BE3D2
	private void UnregisterVisibilityListener()
	{
		DebugUtil.Assert(this.visibilityListenerRegistered);
		Singleton<KBatchedAnimUpdater>.Instance.VisibilityUnregister(this);
		this.visibilityListenerRegistered = false;
	}

	// Token: 0x06002A7A RID: 10874 RVA: 0x001E6C04 File Offset: 0x001E4E04
	public void SetSceneLayer(Grid.SceneLayer layer)
	{
		float layerZ = Grid.GetLayerZ(layer);
		this.sceneLayer = layer;
		Vector3 position = base.transform.GetPosition();
		position.z = layerZ;
		base.transform.SetPosition(position);
		this.DeRegister();
		this.Register();
	}

	// Token: 0x04001CA2 RID: 7330
	[NonSerialized]
	protected bool _forceRebuild;

	// Token: 0x04001CA3 RID: 7331
	private Vector3 lastPos = Vector3.zero;

	// Token: 0x04001CA4 RID: 7332
	private Vector2I lastChunkXY = KBatchedAnimUpdater.INVALID_CHUNK_ID;

	// Token: 0x04001CA5 RID: 7333
	private KAnimBatch batch;

	// Token: 0x04001CA6 RID: 7334
	public float animScale = 0.005f;

	// Token: 0x04001CA7 RID: 7335
	private bool suspendUpdates;

	// Token: 0x04001CA8 RID: 7336
	private bool visibilityListenerRegistered;

	// Token: 0x04001CA9 RID: 7337
	private bool moving;

	// Token: 0x04001CAA RID: 7338
	private SymbolOverrideController symbolOverrideController;

	// Token: 0x04001CAB RID: 7339
	private int symbolOverrideControllerVersion;

	// Token: 0x04001CAC RID: 7340
	[NonSerialized]
	public KBatchedAnimUpdater.RegistrationState updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Unregistered;

	// Token: 0x04001CAD RID: 7341
	public Grid.SceneLayer sceneLayer;

	// Token: 0x04001CAE RID: 7342
	private RectTransform rt;

	// Token: 0x04001CAF RID: 7343
	private Vector3 screenOffset = new Vector3(0f, 0f, 0f);

	// Token: 0x04001CB0 RID: 7344
	public Matrix2x3 navMatrix = Matrix2x3.identity;

	// Token: 0x04001CB1 RID: 7345
	private CanvasScaler scaler;

	// Token: 0x04001CB2 RID: 7346
	public bool setScaleFromAnim = true;

	// Token: 0x04001CB3 RID: 7347
	public Vector2 animOverrideSize = Vector2.one;

	// Token: 0x04001CB4 RID: 7348
	private Canvas rootCanvas;

	// Token: 0x04001CB5 RID: 7349
	public bool isMovable;

	// Token: 0x04001CB6 RID: 7350
	public Func<Vector4> getPositionDataFunctionInUse;

	// Token: 0x04001CB7 RID: 7351
	public KAnimConverter.PostProcessingEffects postProcessingEffectsAllowed;

	// Token: 0x04001CB8 RID: 7352
	public float postProcessingParameters;
}
