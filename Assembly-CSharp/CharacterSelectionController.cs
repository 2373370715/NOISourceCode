using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02001B09 RID: 6921
public class CharacterSelectionController : KModalScreen
{
	// Token: 0x170009A0 RID: 2464
	// (get) Token: 0x06009103 RID: 37123 RVA: 0x00103337 File Offset: 0x00101537
	// (set) Token: 0x06009104 RID: 37124 RVA: 0x0010333F File Offset: 0x0010153F
	public bool IsStarterMinion { get; set; }

	// Token: 0x170009A1 RID: 2465
	// (get) Token: 0x06009105 RID: 37125 RVA: 0x00103348 File Offset: 0x00101548
	public bool AllowsReplacing
	{
		get
		{
			return this.allowsReplacing;
		}
	}

	// Token: 0x06009106 RID: 37126 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnProceed()
	{
	}

	// Token: 0x06009107 RID: 37127 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnDeliverableAdded()
	{
	}

	// Token: 0x06009108 RID: 37128 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnDeliverableRemoved()
	{
	}

	// Token: 0x06009109 RID: 37129 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnLimitReached()
	{
	}

	// Token: 0x0600910A RID: 37130 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnLimitUnreached()
	{
	}

	// Token: 0x0600910B RID: 37131 RVA: 0x0038B1D4 File Offset: 0x003893D4
	protected virtual void InitializeContainers()
	{
		this.DisableProceedButton();
		if (this.containers != null && this.containers.Count > 0)
		{
			return;
		}
		this.OnReplacedEvent = null;
		this.containers = new List<ITelepadDeliverableContainer>();
		if (this.IsStarterMinion || CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CarePackages).id != "Enabled")
		{
			this.numberOfDuplicantOptions = 3;
			this.numberOfCarePackageOptions = 0;
		}
		else
		{
			this.numberOfCarePackageOptions = ((UnityEngine.Random.Range(0, 101) > 70) ? 2 : 1);
			this.numberOfDuplicantOptions = 4 - this.numberOfCarePackageOptions;
		}
		for (int i = 0; i < this.numberOfDuplicantOptions; i++)
		{
			CharacterContainer characterContainer = Util.KInstantiateUI<CharacterContainer>(this.containerPrefab.gameObject, this.containerParent, false);
			characterContainer.SetController(this);
			characterContainer.SetReshufflingState(true);
			this.containers.Add(characterContainer);
		}
		for (int j = 0; j < this.numberOfCarePackageOptions; j++)
		{
			CarePackageContainer carePackageContainer = Util.KInstantiateUI<CarePackageContainer>(this.carePackageContainerPrefab.gameObject, this.containerParent, false);
			carePackageContainer.SetController(this);
			this.containers.Add(carePackageContainer);
			carePackageContainer.gameObject.transform.SetSiblingIndex(UnityEngine.Random.Range(0, carePackageContainer.transform.parent.childCount));
		}
		this.selectedDeliverables = new List<ITelepadDeliverable>();
	}

	// Token: 0x0600910C RID: 37132 RVA: 0x0038B31C File Offset: 0x0038951C
	public virtual void OnPressBack()
	{
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = telepadDeliverableContainer as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.ForceStopEditingTitle();
			}
		}
		this.Show(false);
	}

	// Token: 0x0600910D RID: 37133 RVA: 0x0038B384 File Offset: 0x00389584
	public void RemoveLast()
	{
		if (this.selectedDeliverables == null || this.selectedDeliverables.Count == 0)
		{
			return;
		}
		ITelepadDeliverable obj = this.selectedDeliverables[this.selectedDeliverables.Count - 1];
		if (this.OnReplacedEvent != null)
		{
			this.OnReplacedEvent(obj);
		}
	}

	// Token: 0x0600910E RID: 37134 RVA: 0x0038B3D4 File Offset: 0x003895D4
	public void AddDeliverable(ITelepadDeliverable deliverable)
	{
		if (this.selectedDeliverables.Contains(deliverable))
		{
			global::Debug.Log("Tried to add the same minion twice.");
			return;
		}
		if (this.selectedDeliverables.Count >= this.selectableCount)
		{
			global::Debug.LogError("Tried to add minions beyond the allowed limit");
			return;
		}
		this.selectedDeliverables.Add(deliverable);
		this.OnDeliverableAdded();
		if (this.selectedDeliverables.Count == this.selectableCount)
		{
			this.EnableProceedButton();
			if (this.OnLimitReachedEvent != null)
			{
				this.OnLimitReachedEvent();
			}
			this.OnLimitReached();
		}
	}

	// Token: 0x0600910F RID: 37135 RVA: 0x0038B45C File Offset: 0x0038965C
	public void RemoveDeliverable(ITelepadDeliverable deliverable)
	{
		bool flag = this.selectedDeliverables.Count >= this.selectableCount;
		this.selectedDeliverables.Remove(deliverable);
		this.OnDeliverableRemoved();
		if (flag && this.selectedDeliverables.Count < this.selectableCount)
		{
			this.DisableProceedButton();
			if (this.OnLimitUnreachedEvent != null)
			{
				this.OnLimitUnreachedEvent();
			}
			this.OnLimitUnreached();
		}
	}

	// Token: 0x06009110 RID: 37136 RVA: 0x00103350 File Offset: 0x00101550
	public bool IsSelected(ITelepadDeliverable deliverable)
	{
		return this.selectedDeliverables.Contains(deliverable);
	}

	// Token: 0x06009111 RID: 37137 RVA: 0x0010335E File Offset: 0x0010155E
	protected void EnableProceedButton()
	{
		this.proceedButton.isInteractable = true;
		this.proceedButton.ClearOnClick();
		this.proceedButton.onClick += delegate()
		{
			this.OnProceed();
		};
	}

	// Token: 0x06009112 RID: 37138 RVA: 0x0038B4C8 File Offset: 0x003896C8
	protected void DisableProceedButton()
	{
		this.proceedButton.ClearOnClick();
		this.proceedButton.isInteractable = false;
		this.proceedButton.onClick += delegate()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		};
	}

	// Token: 0x04006DA1 RID: 28065
	[SerializeField]
	private CharacterContainer containerPrefab;

	// Token: 0x04006DA2 RID: 28066
	[SerializeField]
	private CarePackageContainer carePackageContainerPrefab;

	// Token: 0x04006DA3 RID: 28067
	[SerializeField]
	private GameObject containerParent;

	// Token: 0x04006DA4 RID: 28068
	[SerializeField]
	protected KButton proceedButton;

	// Token: 0x04006DA5 RID: 28069
	protected int numberOfDuplicantOptions = 3;

	// Token: 0x04006DA6 RID: 28070
	protected int numberOfCarePackageOptions;

	// Token: 0x04006DA7 RID: 28071
	[SerializeField]
	protected int selectableCount;

	// Token: 0x04006DA8 RID: 28072
	[SerializeField]
	private bool allowsReplacing;

	// Token: 0x04006DAA RID: 28074
	protected List<ITelepadDeliverable> selectedDeliverables;

	// Token: 0x04006DAB RID: 28075
	protected List<ITelepadDeliverableContainer> containers;

	// Token: 0x04006DAC RID: 28076
	public System.Action OnLimitReachedEvent;

	// Token: 0x04006DAD RID: 28077
	public System.Action OnLimitUnreachedEvent;

	// Token: 0x04006DAE RID: 28078
	public Action<bool> OnReshuffleEvent;

	// Token: 0x04006DAF RID: 28079
	public Action<ITelepadDeliverable> OnReplacedEvent;

	// Token: 0x04006DB0 RID: 28080
	public System.Action OnProceedEvent;
}
