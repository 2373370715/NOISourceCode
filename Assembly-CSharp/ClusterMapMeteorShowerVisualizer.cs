using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001923 RID: 6435
public class ClusterMapMeteorShowerVisualizer : ClusterGridEntity
{
	// Token: 0x1700088F RID: 2191
	// (get) Token: 0x0600854E RID: 34126 RVA: 0x000FC12D File Offset: 0x000FA32D
	public override string Name
	{
		get
		{
			return this.p_name;
		}
	}

	// Token: 0x17000890 RID: 2192
	// (get) Token: 0x0600854F RID: 34127 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Craft;
		}
	}

	// Token: 0x17000891 RID: 2193
	// (get) Token: 0x06008550 RID: 34128 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000892 RID: 2194
	// (get) Token: 0x06008551 RID: 34129 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x17000893 RID: 2195
	// (get) Token: 0x06008552 RID: 34130 RVA: 0x00355128 File Offset: 0x00353328
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim(this.clusterAnimName),
					initialAnim = this.AnimName,
					animPlaySpeedModifier = 0.5f
				},
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("shower_identify_kanim"),
					initialAnim = "identify_off",
					playMode = KAnim.PlayMode.Once
				},
				this.questionMarkAnimConfig
			};
		}
	}

	// Token: 0x17000894 RID: 2196
	// (get) Token: 0x06008553 RID: 34131 RVA: 0x000FC135 File Offset: 0x000FA335
	public ClusterRevealLevel clusterCellRevealLevel
	{
		get
		{
			return ClusterGrid.Instance.GetCellRevealLevel(base.Location);
		}
	}

	// Token: 0x17000895 RID: 2197
	// (get) Token: 0x06008554 RID: 34132 RVA: 0x000FC147 File Offset: 0x000FA347
	public string AnimName
	{
		get
		{
			if (!this.revealed || this.clusterCellRevealLevel != ClusterRevealLevel.Visible)
			{
				return "unknown";
			}
			return "idle_loop";
		}
	}

	// Token: 0x17000896 RID: 2198
	// (get) Token: 0x06008555 RID: 34133 RVA: 0x000FC165 File Offset: 0x000FA365
	public string QuestionMarkAnimName
	{
		get
		{
			if (!this.revealed || this.clusterCellRevealLevel != ClusterRevealLevel.Visible)
			{
				return this.questionMarkAnimConfig.initialAnim;
			}
			return "off";
		}
	}

	// Token: 0x06008556 RID: 34134 RVA: 0x003551C0 File Offset: 0x003533C0
	public KBatchedAnimController CreateQuestionMarkInstance(KBatchedAnimController origin, Transform parent)
	{
		KBatchedAnimController kbatchedAnimController = UnityEngine.Object.Instantiate<KBatchedAnimController>(origin, parent);
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController.SwapAnims(new KAnimFile[]
		{
			this.questionMarkAnimConfig.animFile
		});
		kbatchedAnimController.Play(this.QuestionMarkAnimName, KAnim.PlayMode.Once, 1f, 0f);
		kbatchedAnimController.gameObject.AddOrGet<ClusterMapIconFixRotation>();
		return kbatchedAnimController;
	}

	// Token: 0x06008557 RID: 34135 RVA: 0x00355224 File Offset: 0x00353424
	protected override void OnCleanUp()
	{
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
			if (entityVisAnim != null)
			{
				entityVisAnim.gameObject.SetActive(false);
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x06008558 RID: 34136 RVA: 0x000FC189 File Offset: 0x000FA389
	public void SetInitialLocation(AxialI startLocation)
	{
		this.m_location = startLocation;
		this.RefreshVisuals();
	}

	// Token: 0x06008559 RID: 34137 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool SpaceOutInSameHex()
	{
		return true;
	}

	// Token: 0x0600855A RID: 34138 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool KeepRotationWhenSpacingOutInHex()
	{
		return true;
	}

	// Token: 0x0600855B RID: 34139 RVA: 0x000B2651 File Offset: 0x000B0851
	public override bool ShowPath()
	{
		return this.m_selectable.IsSelected;
	}

	// Token: 0x0600855C RID: 34140 RVA: 0x00355268 File Offset: 0x00353468
	public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		switch (levelUsed)
		{
		case ClusterRevealLevel.Hidden:
			this.Deselect();
			break;
		case ClusterRevealLevel.Peeked:
		{
			KBatchedAnimController firstAnimController = entityVisAnim.GetFirstAnimController();
			if (firstAnimController != null)
			{
				firstAnimController.SwapAnims(new KAnimFile[]
				{
					this.AnimConfigs[0].animFile
				});
				KBatchedAnimController externalAnimController = this.CreateQuestionMarkInstance(entityVisAnim.peekControllerPrefab, firstAnimController.transform.parent);
				entityVisAnim.ManualAddAnimController(externalAnimController);
			}
			this.RefreshVisuals();
			this.Deselect();
			break;
		}
		case ClusterRevealLevel.Visible:
			this.RefreshVisuals();
			break;
		}
		KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
		if (animController != null && !this.revealed)
		{
			animController.gameObject.AddOrGet<ClusterMapIconFixRotation>();
		}
	}

	// Token: 0x0600855D RID: 34141 RVA: 0x000FC198 File Offset: 0x000FA398
	public void Deselect()
	{
		if (this.m_selectable.IsSelected)
		{
			this.m_selectable.Unselect();
		}
	}

	// Token: 0x0600855E RID: 34142 RVA: 0x00355328 File Offset: 0x00353528
	public void RefreshVisuals()
	{
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		if (entityVisAnim != null)
		{
			KBatchedAnimController firstAnimController = entityVisAnim.GetFirstAnimController();
			if (firstAnimController != null)
			{
				firstAnimController.Play(this.AnimName, KAnim.PlayMode.Loop, 1f, 0f);
			}
			KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
			if (animController != null)
			{
				animController.Play(this.QuestionMarkAnimName, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x0600855F RID: 34143 RVA: 0x003553A4 File Offset: 0x003535A4
	public void PlayRevealAnimation(bool playIdentifyAnimationIfVisible)
	{
		this.revealed = true;
		this.RefreshVisuals();
		if (playIdentifyAnimationIfVisible)
		{
			ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
			KBatchedAnimController animController = entityVisAnim.GetAnimController(1);
			entityVisAnim.GetAnimController(2);
			if (animController != null)
			{
				animController.Play("identify", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x06008560 RID: 34144 RVA: 0x000FC1B2 File Offset: 0x000FA3B2
	public void PlayHideAnimation()
	{
		this.revealed = false;
		if (ClusterMapScreen.Instance.GetEntityVisAnim(this) != null)
		{
			this.RefreshVisuals();
		}
	}

	// Token: 0x04006578 RID: 25976
	private ClusterGridEntity.AnimConfig questionMarkAnimConfig = new ClusterGridEntity.AnimConfig
	{
		animFile = Assets.GetAnim("shower_question_mark_kanim"),
		initialAnim = "idle",
		playMode = KAnim.PlayMode.Once
	};

	// Token: 0x04006579 RID: 25977
	public string p_name;

	// Token: 0x0400657A RID: 25978
	public string clusterAnimName;

	// Token: 0x0400657B RID: 25979
	public bool revealed;
}
