using System;
using KSerialization;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200141B RID: 5147
[AddComponentMenu("KMonoBehaviour/scripts/HarvestDesignatable")]
public class HarvestDesignatable : KMonoBehaviour
{
	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x06006956 RID: 26966 RVA: 0x000E96F7 File Offset: 0x000E78F7
	public bool InPlanterBox
	{
		get
		{
			return this.isInPlanterBox;
		}
	}

	// Token: 0x170006B8 RID: 1720
	// (get) Token: 0x06006957 RID: 26967 RVA: 0x000E96FF File Offset: 0x000E78FF
	// (set) Token: 0x06006958 RID: 26968 RVA: 0x000E9707 File Offset: 0x000E7907
	public bool MarkedForHarvest
	{
		get
		{
			return this.isMarkedForHarvest;
		}
		set
		{
			this.isMarkedForHarvest = value;
		}
	}

	// Token: 0x170006B9 RID: 1721
	// (get) Token: 0x06006959 RID: 26969 RVA: 0x000E9710 File Offset: 0x000E7910
	public bool HarvestWhenReady
	{
		get
		{
			return this.harvestWhenReady;
		}
	}

	// Token: 0x0600695A RID: 26970 RVA: 0x000E9718 File Offset: 0x000E7918
	protected HarvestDesignatable()
	{
		this.onEnableOverlayDelegate = new Action<object>(this.OnEnableOverlay);
	}

	// Token: 0x0600695B RID: 26971 RVA: 0x000E974B File Offset: 0x000E794B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HarvestDesignatable>(1309017699, HarvestDesignatable.SetInPlanterBoxTrueDelegate);
	}

	// Token: 0x0600695C RID: 26972 RVA: 0x002E8D84 File Offset: 0x002E6F84
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForHarvest)
		{
			this.MarkForHarvest();
		}
		Components.HarvestDesignatables.Add(this);
		base.Subscribe<HarvestDesignatable>(493375141, HarvestDesignatable.OnRefreshUserMenuDelegate);
		base.Subscribe<HarvestDesignatable>(2127324410, HarvestDesignatable.OnCancelDelegate);
		Game.Instance.Subscribe(1248612973, this.onEnableOverlayDelegate);
		Game.Instance.Subscribe(1798162660, this.onEnableOverlayDelegate);
		Game.Instance.Subscribe(2015652040, new Action<object>(this.OnDisableOverlay));
		Game.Instance.Subscribe(1983128072, new Action<object>(this.RefreshOverlayIcon));
		this.area = base.GetComponent<OccupyArea>();
	}

	// Token: 0x0600695D RID: 26973 RVA: 0x002E8E44 File Offset: 0x002E7044
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.HarvestDesignatables.Remove(this);
		this.DestroyOverlayIcon();
		Game.Instance.Unsubscribe(1248612973, this.onEnableOverlayDelegate);
		Game.Instance.Unsubscribe(2015652040, new Action<object>(this.OnDisableOverlay));
		Game.Instance.Unsubscribe(1798162660, this.onEnableOverlayDelegate);
		Game.Instance.Unsubscribe(1983128072, new Action<object>(this.RefreshOverlayIcon));
	}

	// Token: 0x0600695E RID: 26974 RVA: 0x000E9764 File Offset: 0x000E7964
	private void DestroyOverlayIcon()
	{
		if (this.HarvestWhenReadyOverlayIcon != null)
		{
			UnityEngine.Object.Destroy(this.HarvestWhenReadyOverlayIcon.gameObject);
			this.HarvestWhenReadyOverlayIcon = null;
		}
	}

	// Token: 0x0600695F RID: 26975 RVA: 0x002E8EC8 File Offset: 0x002E70C8
	private void CreateOverlayIcon()
	{
		if (this.HarvestWhenReadyOverlayIcon != null)
		{
			return;
		}
		if (base.GetComponent<AttackableBase>() == null)
		{
			this.HarvestWhenReadyOverlayIcon = Util.KInstantiate(Assets.UIPrefabs.HarvestWhenReadyOverlayIcon, GameScreenManager.Instance.worldSpaceCanvas, null).GetComponent<RectTransform>();
			Extents extents = base.GetComponent<OccupyArea>().GetExtents();
			Vector3 position;
			if (base.GetComponent<KPrefabID>().HasTag(GameTags.Hanging))
			{
				position = new Vector3((float)(extents.x + extents.width / 2) + 0.5f, (float)(extents.y + extents.height)) + this.iconOffset;
			}
			else
			{
				position = new Vector3((float)(extents.x + extents.width / 2) + 0.5f, (float)extents.y) + this.iconOffset;
			}
			this.HarvestWhenReadyOverlayIcon.transform.SetPosition(position);
			this.RefreshOverlayIcon(null);
		}
	}

	// Token: 0x06006960 RID: 26976 RVA: 0x000E978B File Offset: 0x000E798B
	private void OnDisableOverlay(object data)
	{
		this.DestroyOverlayIcon();
	}

	// Token: 0x06006961 RID: 26977 RVA: 0x000E9793 File Offset: 0x000E7993
	private void OnEnableOverlay(object data)
	{
		if ((HashedString)data == OverlayModes.Harvest.ID)
		{
			this.CreateOverlayIcon();
			return;
		}
		this.DestroyOverlayIcon();
	}

	// Token: 0x06006962 RID: 26978 RVA: 0x002E8FC0 File Offset: 0x002E71C0
	private void RefreshOverlayIcon(object data = null)
	{
		if (this.HarvestWhenReadyOverlayIcon != null)
		{
			if ((Grid.IsVisible(Grid.PosToCell(base.gameObject)) && base.gameObject.GetMyWorldId() == ClusterManager.Instance.activeWorldId) || (CameraController.Instance != null && CameraController.Instance.FreeCameraEnabled))
			{
				if (!this.HarvestWhenReadyOverlayIcon.gameObject.activeSelf)
				{
					this.HarvestWhenReadyOverlayIcon.gameObject.SetActive(true);
				}
			}
			else if (this.HarvestWhenReadyOverlayIcon.gameObject.activeSelf)
			{
				this.HarvestWhenReadyOverlayIcon.gameObject.SetActive(false);
			}
			HierarchyReferences component = this.HarvestWhenReadyOverlayIcon.GetComponent<HierarchyReferences>();
			if (this.harvestWhenReady)
			{
				Image image = (Image)component.GetReference("On");
				image.gameObject.SetActive(true);
				image.color = GlobalAssets.Instance.colorSet.harvestEnabled;
				component.GetReference("Off").gameObject.SetActive(false);
				return;
			}
			component.GetReference("On").gameObject.SetActive(false);
			Image image2 = (Image)component.GetReference("Off");
			image2.gameObject.SetActive(true);
			image2.color = GlobalAssets.Instance.colorSet.harvestDisabled;
		}
	}

	// Token: 0x06006963 RID: 26979 RVA: 0x002E9114 File Offset: 0x002E7314
	public bool CanBeHarvested()
	{
		Harvestable component = base.GetComponent<Harvestable>();
		return !(component != null) || component.CanBeHarvested;
	}

	// Token: 0x06006964 RID: 26980 RVA: 0x000E97B4 File Offset: 0x000E79B4
	public void SetInPlanterBox(bool state)
	{
		if (state)
		{
			if (!this.isInPlanterBox)
			{
				this.isInPlanterBox = true;
				this.SetHarvestWhenReady(this.defaultHarvestStateWhenPlanted);
				return;
			}
		}
		else
		{
			this.isInPlanterBox = false;
		}
	}

	// Token: 0x06006965 RID: 26981 RVA: 0x002E913C File Offset: 0x002E733C
	public void SetHarvestWhenReady(bool state)
	{
		this.harvestWhenReady = state;
		if (this.harvestWhenReady && this.CanBeHarvested() && !this.isMarkedForHarvest)
		{
			this.MarkForHarvest();
		}
		if (this.isMarkedForHarvest && !this.harvestWhenReady)
		{
			this.OnCancel(null);
			if (this.CanBeHarvested() && this.isInPlanterBox)
			{
				base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, this);
			}
		}
		base.Trigger(-266953818, null);
		this.RefreshOverlayIcon(null);
	}

	// Token: 0x06006966 RID: 26982 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnCancel(object data = null)
	{
	}

	// Token: 0x06006967 RID: 26983 RVA: 0x002E91C4 File Offset: 0x002E73C4
	public virtual void MarkForHarvest()
	{
		if (!this.CanBeHarvested())
		{
			return;
		}
		this.isMarkedForHarvest = true;
		Harvestable component = base.GetComponent<Harvestable>();
		if (component != null)
		{
			component.OnMarkedForHarvest();
		}
	}

	// Token: 0x06006968 RID: 26984 RVA: 0x000E97DC File Offset: 0x000E79DC
	protected virtual void OnClickHarvestWhenReady()
	{
		this.SetHarvestWhenReady(true);
	}

	// Token: 0x06006969 RID: 26985 RVA: 0x002E91F8 File Offset: 0x002E73F8
	protected virtual void OnClickCancelHarvestWhenReady()
	{
		Harvestable component = base.GetComponent<Harvestable>();
		if (component != null)
		{
			component.Trigger(2127324410, null);
		}
		this.SetHarvestWhenReady(false);
	}

	// Token: 0x0600696A RID: 26986 RVA: 0x002E9228 File Offset: 0x002E7428
	public virtual void OnRefreshUserMenu(object data)
	{
		if (this.showUserMenuButtons)
		{
			KIconButtonMenu.ButtonInfo button = this.harvestWhenReady ? new KIconButtonMenu.ButtonInfo("action_harvest", UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.NAME, delegate()
			{
				this.OnClickCancelHarvestWhenReady();
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.GAMEOBJECTEFFECTS.PLANT_DO_NOT_HARVEST, base.transform, 1.5f, false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCEL_HARVEST_WHEN_READY.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_harvest", UI.USERMENUACTIONS.HARVEST_WHEN_READY.NAME, delegate()
			{
				this.OnClickHarvestWhenReady();
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.GAMEOBJECTEFFECTS.PLANT_MARK_FOR_HARVEST, base.transform, 1.5f, false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.HARVEST_WHEN_READY.TOOLTIP, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
		}
	}

	// Token: 0x04004FE5 RID: 20453
	public Vector2 iconOffset = Vector2.zero;

	// Token: 0x04004FE6 RID: 20454
	public bool defaultHarvestStateWhenPlanted = true;

	// Token: 0x04004FE7 RID: 20455
	public OccupyArea area;

	// Token: 0x04004FE8 RID: 20456
	[Serialize]
	protected bool isMarkedForHarvest;

	// Token: 0x04004FE9 RID: 20457
	[Serialize]
	private bool isInPlanterBox;

	// Token: 0x04004FEA RID: 20458
	public bool showUserMenuButtons = true;

	// Token: 0x04004FEB RID: 20459
	[Serialize]
	protected bool harvestWhenReady;

	// Token: 0x04004FEC RID: 20460
	public RectTransform HarvestWhenReadyOverlayIcon;

	// Token: 0x04004FED RID: 20461
	private Action<object> onEnableOverlayDelegate;

	// Token: 0x04004FEE RID: 20462
	private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> OnCancelDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>(delegate(HarvestDesignatable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04004FEF RID: 20463
	private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>(delegate(HarvestDesignatable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04004FF0 RID: 20464
	private static readonly EventSystem.IntraObjectHandler<HarvestDesignatable> SetInPlanterBoxTrueDelegate = new EventSystem.IntraObjectHandler<HarvestDesignatable>(delegate(HarvestDesignatable component, object data)
	{
		component.SetInPlanterBox(true);
	});
}
