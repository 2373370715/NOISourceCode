using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020019C9 RID: 6601
public class ClusterMapVisualizer : KMonoBehaviour
{
	// Token: 0x06008999 RID: 35225 RVA: 0x00367104 File Offset: 0x00365304
	public void Init(ClusterGridEntity entity, ClusterMapPathDrawer pathDrawer)
	{
		this.entity = entity;
		this.pathDrawer = pathDrawer;
		this.animControllers = new List<KBatchedAnimController>();
		if (this.animContainer == null)
		{
			GameObject gameObject = new GameObject("AnimContainer", new Type[]
			{
				typeof(RectTransform)
			});
			RectTransform component = base.GetComponent<RectTransform>();
			RectTransform component2 = gameObject.GetComponent<RectTransform>();
			component2.SetParent(component, false);
			component2.SetLocalPosition(new Vector3(0f, 0f, 0f));
			component2.sizeDelta = component.sizeDelta;
			component2.localScale = Vector3.one;
			this.animContainer = component2;
		}
		Vector3 position = ClusterGrid.Instance.GetPosition(entity);
		this.rectTransform().SetLocalPosition(position);
		this.RefreshPathDrawing();
		entity.Subscribe(543433792, new Action<object>(this.OnClusterDestinationChanged));
	}

	// Token: 0x0600899A RID: 35226 RVA: 0x000FE735 File Offset: 0x000FC935
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.doesTransitionAnimation)
		{
			new ClusterMapTravelAnimator.StatesInstance(this, this.entity).StartSM();
		}
	}

	// Token: 0x0600899B RID: 35227 RVA: 0x003671DC File Offset: 0x003653DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.entity != null)
		{
			if (this.doesTransitionAnimation)
			{
				base.gameObject.GetSMI<ClusterMapTravelAnimator.StatesInstance>().keepRotationOnIdle = this.entity.KeepRotationWhenSpacingOutInHex();
			}
			if (this.entity is Clustercraft)
			{
				new ClusterMapRocketAnimator.StatesInstance(this, this.entity).StartSM();
				return;
			}
			if (this.entity is BallisticClusterGridEntity)
			{
				new ClusterMapBallisticAnimator.StatesInstance(this, this.entity).StartSM();
				return;
			}
			if (this.entity.Layer == EntityLayer.FX)
			{
				new ClusterMapFXAnimator.StatesInstance(this, this.entity).StartSM();
			}
		}
	}

	// Token: 0x0600899C RID: 35228 RVA: 0x000FE756 File Offset: 0x000FC956
	protected override void OnCleanUp()
	{
		if (this.entity != null)
		{
			this.entity.Unsubscribe(543433792, new Action<object>(this.OnClusterDestinationChanged));
		}
		base.OnCleanUp();
	}

	// Token: 0x0600899D RID: 35229 RVA: 0x000FE788 File Offset: 0x000FC988
	private void OnClusterDestinationChanged(object data)
	{
		this.RefreshPathDrawing();
	}

	// Token: 0x0600899E RID: 35230 RVA: 0x00367280 File Offset: 0x00365480
	public void Select(bool selected)
	{
		if (this.animControllers == null || this.animControllers.Count == 0)
		{
			return;
		}
		if (!selected == this.isSelected)
		{
			this.isSelected = selected;
			this.RefreshPathDrawing();
		}
		this.GetFirstAnimController().SetSymbolVisiblity("selected", selected);
	}

	// Token: 0x0600899F RID: 35231 RVA: 0x000FE790 File Offset: 0x000FC990
	public void PlayAnim(string animName, KAnim.PlayMode playMode)
	{
		if (this.animControllers.Count > 0)
		{
			this.GetFirstAnimController().Play(animName, playMode, 1f, 0f);
		}
	}

	// Token: 0x060089A0 RID: 35232 RVA: 0x000FE7BC File Offset: 0x000FC9BC
	public KBatchedAnimController GetFirstAnimController()
	{
		return this.GetAnimController(0);
	}

	// Token: 0x060089A1 RID: 35233 RVA: 0x000FE7C5 File Offset: 0x000FC9C5
	public KBatchedAnimController GetAnimController(int index)
	{
		if (index < this.animControllers.Count)
		{
			return this.animControllers[index];
		}
		return null;
	}

	// Token: 0x060089A2 RID: 35234 RVA: 0x000FE7E3 File Offset: 0x000FC9E3
	public void ManualAddAnimController(KBatchedAnimController externalAnimController)
	{
		this.animControllers.Add(externalAnimController);
	}

	// Token: 0x060089A3 RID: 35235 RVA: 0x003672D4 File Offset: 0x003654D4
	public void Show(ClusterRevealLevel level)
	{
		if (!this.entity.IsVisible)
		{
			level = ClusterRevealLevel.Hidden;
		}
		if (level == this.lastRevealLevel)
		{
			return;
		}
		this.lastRevealLevel = level;
		switch (level)
		{
		case ClusterRevealLevel.Hidden:
			base.gameObject.SetActive(false);
			break;
		case ClusterRevealLevel.Peeked:
		{
			this.ClearAnimControllers();
			KBatchedAnimController kbatchedAnimController = UnityEngine.Object.Instantiate<KBatchedAnimController>(this.peekControllerPrefab, this.animContainer);
			kbatchedAnimController.gameObject.SetActive(true);
			this.animControllers.Add(kbatchedAnimController);
			base.gameObject.SetActive(true);
			break;
		}
		case ClusterRevealLevel.Visible:
			this.ClearAnimControllers();
			if (this.animControllerPrefab != null && this.entity.AnimConfigs != null)
			{
				foreach (ClusterGridEntity.AnimConfig animConfig in this.entity.AnimConfigs)
				{
					KBatchedAnimController kbatchedAnimController2 = UnityEngine.Object.Instantiate<KBatchedAnimController>(this.animControllerPrefab, this.animContainer);
					kbatchedAnimController2.AnimFiles = new KAnimFile[]
					{
						animConfig.animFile
					};
					kbatchedAnimController2.initialMode = animConfig.playMode;
					kbatchedAnimController2.initialAnim = animConfig.initialAnim;
					kbatchedAnimController2.Offset = animConfig.animOffset;
					kbatchedAnimController2.gameObject.AddComponent<LoopingSounds>();
					if (animConfig.animPlaySpeedModifier != 0f)
					{
						kbatchedAnimController2.PlaySpeedMultiplier = animConfig.animPlaySpeedModifier;
					}
					if (!string.IsNullOrEmpty(animConfig.symbolSwapTarget) && !string.IsNullOrEmpty(animConfig.symbolSwapSymbol))
					{
						SymbolOverrideController component = kbatchedAnimController2.GetComponent<SymbolOverrideController>();
						KAnim.Build.Symbol symbol = kbatchedAnimController2.AnimFiles[0].GetData().build.GetSymbol(animConfig.symbolSwapSymbol);
						component.AddSymbolOverride(animConfig.symbolSwapTarget, symbol, 0);
					}
					kbatchedAnimController2.gameObject.SetActive(true);
					this.animControllers.Add(kbatchedAnimController2);
				}
			}
			base.gameObject.SetActive(true);
			break;
		}
		this.entity.OnClusterMapIconShown(level);
	}

	// Token: 0x060089A4 RID: 35236 RVA: 0x003674D4 File Offset: 0x003656D4
	public void RefreshPathDrawing()
	{
		if (this.entity == null)
		{
			return;
		}
		ClusterTraveler component = this.entity.GetComponent<ClusterTraveler>();
		if (component == null)
		{
			return;
		}
		List<AxialI> list = (this.entity.IsVisible && component.IsTraveling()) ? component.CurrentPath : null;
		if (list != null && list.Count > 0)
		{
			if (this.mapPath == null)
			{
				this.mapPath = this.pathDrawer.AddPath();
			}
			this.mapPath.SetPoints(ClusterMapPathDrawer.GetDrawPathList(base.transform.GetLocalPosition(), list));
			Color color;
			if (this.isSelected)
			{
				color = ClusterMapScreen.Instance.rocketSelectedPathColor;
			}
			else if (this.entity.ShowPath())
			{
				color = ClusterMapScreen.Instance.rocketPathColor;
			}
			else
			{
				color = new Color(0f, 0f, 0f, 0f);
			}
			this.mapPath.SetColor(color);
			return;
		}
		if (this.mapPath != null)
		{
			global::Util.KDestroyGameObject(this.mapPath);
			this.mapPath = null;
		}
	}

	// Token: 0x060089A5 RID: 35237 RVA: 0x000FE7F1 File Offset: 0x000FC9F1
	public void SetAnimRotation(float rotation)
	{
		this.animContainer.localRotation = Quaternion.Euler(0f, 0f, rotation);
	}

	// Token: 0x060089A6 RID: 35238 RVA: 0x000FE80E File Offset: 0x000FCA0E
	public float GetPathAngle()
	{
		if (this.mapPath == null)
		{
			return 0f;
		}
		return this.mapPath.GetRotationForNextSegment();
	}

	// Token: 0x060089A7 RID: 35239 RVA: 0x003675F0 File Offset: 0x003657F0
	private void ClearAnimControllers()
	{
		if (this.animControllers == null)
		{
			return;
		}
		foreach (KBatchedAnimController kbatchedAnimController in this.animControllers)
		{
			global::Util.KDestroyGameObject(kbatchedAnimController.gameObject);
		}
		this.animControllers.Clear();
	}

	// Token: 0x04006817 RID: 26647
	public KBatchedAnimController animControllerPrefab;

	// Token: 0x04006818 RID: 26648
	public KBatchedAnimController peekControllerPrefab;

	// Token: 0x04006819 RID: 26649
	public Transform nameTarget;

	// Token: 0x0400681A RID: 26650
	public AlertVignette alertVignette;

	// Token: 0x0400681B RID: 26651
	public bool doesTransitionAnimation;

	// Token: 0x0400681C RID: 26652
	[HideInInspector]
	public Transform animContainer;

	// Token: 0x0400681D RID: 26653
	private ClusterGridEntity entity;

	// Token: 0x0400681E RID: 26654
	private ClusterMapPathDrawer pathDrawer;

	// Token: 0x0400681F RID: 26655
	private ClusterMapPath mapPath;

	// Token: 0x04006820 RID: 26656
	private List<KBatchedAnimController> animControllers;

	// Token: 0x04006821 RID: 26657
	private bool isSelected;

	// Token: 0x04006822 RID: 26658
	private ClusterRevealLevel lastRevealLevel;

	// Token: 0x020019CA RID: 6602
	private class UpdateXPositionParameter : LoopingSoundParameterUpdater
	{
		// Token: 0x060089A9 RID: 35241 RVA: 0x000FE82F File Offset: 0x000FCA2F
		public UpdateXPositionParameter() : base("Starmap_Position_X")
		{
		}

		// Token: 0x060089AA RID: 35242 RVA: 0x0036765C File Offset: 0x0036585C
		public override void Add(LoopingSoundParameterUpdater.Sound sound)
		{
			ClusterMapVisualizer.UpdateXPositionParameter.Entry item = new ClusterMapVisualizer.UpdateXPositionParameter.Entry
			{
				transform = sound.transform,
				ev = sound.ev,
				parameterId = sound.description.GetParameterId(base.parameter)
			};
			this.entries.Add(item);
		}

		// Token: 0x060089AB RID: 35243 RVA: 0x003676B4 File Offset: 0x003658B4
		public override void Update(float dt)
		{
			foreach (ClusterMapVisualizer.UpdateXPositionParameter.Entry entry in this.entries)
			{
				if (!(entry.transform == null))
				{
					EventInstance ev = entry.ev;
					ev.setParameterByID(entry.parameterId, entry.transform.GetPosition().x / (float)Screen.width, false);
				}
			}
		}

		// Token: 0x060089AC RID: 35244 RVA: 0x0036773C File Offset: 0x0036593C
		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ev.handle == sound.ev.handle)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x04006823 RID: 26659
		private List<ClusterMapVisualizer.UpdateXPositionParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateXPositionParameter.Entry>();

		// Token: 0x020019CB RID: 6603
		private struct Entry
		{
			// Token: 0x04006824 RID: 26660
			public Transform transform;

			// Token: 0x04006825 RID: 26661
			public EventInstance ev;

			// Token: 0x04006826 RID: 26662
			public PARAMETER_ID parameterId;
		}
	}

	// Token: 0x020019CC RID: 6604
	private class UpdateYPositionParameter : LoopingSoundParameterUpdater
	{
		// Token: 0x060089AD RID: 35245 RVA: 0x000FE84C File Offset: 0x000FCA4C
		public UpdateYPositionParameter() : base("Starmap_Position_Y")
		{
		}

		// Token: 0x060089AE RID: 35246 RVA: 0x00367794 File Offset: 0x00365994
		public override void Add(LoopingSoundParameterUpdater.Sound sound)
		{
			ClusterMapVisualizer.UpdateYPositionParameter.Entry item = new ClusterMapVisualizer.UpdateYPositionParameter.Entry
			{
				transform = sound.transform,
				ev = sound.ev,
				parameterId = sound.description.GetParameterId(base.parameter)
			};
			this.entries.Add(item);
		}

		// Token: 0x060089AF RID: 35247 RVA: 0x003677EC File Offset: 0x003659EC
		public override void Update(float dt)
		{
			foreach (ClusterMapVisualizer.UpdateYPositionParameter.Entry entry in this.entries)
			{
				if (!(entry.transform == null))
				{
					EventInstance ev = entry.ev;
					ev.setParameterByID(entry.parameterId, entry.transform.GetPosition().y / (float)Screen.height, false);
				}
			}
		}

		// Token: 0x060089B0 RID: 35248 RVA: 0x00367874 File Offset: 0x00365A74
		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ev.handle == sound.ev.handle)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x04006827 RID: 26663
		private List<ClusterMapVisualizer.UpdateYPositionParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateYPositionParameter.Entry>();

		// Token: 0x020019CD RID: 6605
		private struct Entry
		{
			// Token: 0x04006828 RID: 26664
			public Transform transform;

			// Token: 0x04006829 RID: 26665
			public EventInstance ev;

			// Token: 0x0400682A RID: 26666
			public PARAMETER_ID parameterId;
		}
	}

	// Token: 0x020019CE RID: 6606
	private class UpdateZoomPercentageParameter : LoopingSoundParameterUpdater
	{
		// Token: 0x060089B1 RID: 35249 RVA: 0x000FE869 File Offset: 0x000FCA69
		public UpdateZoomPercentageParameter() : base("Starmap_Zoom_Percentage")
		{
		}

		// Token: 0x060089B2 RID: 35250 RVA: 0x003678CC File Offset: 0x00365ACC
		public override void Add(LoopingSoundParameterUpdater.Sound sound)
		{
			ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry item = new ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry
			{
				ev = sound.ev,
				parameterId = sound.description.GetParameterId(base.parameter)
			};
			this.entries.Add(item);
		}

		// Token: 0x060089B3 RID: 35251 RVA: 0x00367918 File Offset: 0x00365B18
		public override void Update(float dt)
		{
			foreach (ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry entry in this.entries)
			{
				EventInstance ev = entry.ev;
				ev.setParameterByID(entry.parameterId, ClusterMapScreen.Instance.CurrentZoomPercentage(), false);
			}
		}

		// Token: 0x060089B4 RID: 35252 RVA: 0x00367984 File Offset: 0x00365B84
		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ev.handle == sound.ev.handle)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0400682B RID: 26667
		private List<ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry>();

		// Token: 0x020019CF RID: 6607
		private struct Entry
		{
			// Token: 0x0400682C RID: 26668
			public Transform transform;

			// Token: 0x0400682D RID: 26669
			public EventInstance ev;

			// Token: 0x0400682E RID: 26670
			public PARAMETER_ID parameterId;
		}
	}
}
