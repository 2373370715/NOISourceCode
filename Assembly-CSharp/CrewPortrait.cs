using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DF5 RID: 7669
[AddComponentMenu("KMonoBehaviour/scripts/CrewPortrait")]
[Serializable]
public class CrewPortrait : KMonoBehaviour
{
	// Token: 0x17000A6F RID: 2671
	// (get) Token: 0x0600A054 RID: 41044 RVA: 0x0010CDA1 File Offset: 0x0010AFA1
	// (set) Token: 0x0600A055 RID: 41045 RVA: 0x0010CDA9 File Offset: 0x0010AFA9
	public IAssignableIdentity identityObject { get; private set; }

	// Token: 0x0600A056 RID: 41046 RVA: 0x0010CDB2 File Offset: 0x0010AFB2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.startTransparent)
		{
			base.StartCoroutine(this.AlphaIn());
		}
		this.requiresRefresh = true;
	}

	// Token: 0x0600A057 RID: 41047 RVA: 0x0010CDD6 File Offset: 0x0010AFD6
	private IEnumerator AlphaIn()
	{
		this.SetAlpha(0f);
		for (float i = 0f; i < 1f; i += Time.unscaledDeltaTime * 4f)
		{
			this.SetAlpha(i);
			yield return 0;
		}
		this.SetAlpha(1f);
		yield break;
	}

	// Token: 0x0600A058 RID: 41048 RVA: 0x0010CDE5 File Offset: 0x0010AFE5
	private void OnRoleChanged(object data)
	{
		if (this.controller == null)
		{
			return;
		}
		CrewPortrait.RefreshHat(this.identityObject, this.controller);
	}

	// Token: 0x0600A059 RID: 41049 RVA: 0x003E3688 File Offset: 0x003E1888
	private void RegisterEvents()
	{
		if (this.areEventsRegistered)
		{
			return;
		}
		KMonoBehaviour kmonoBehaviour = this.identityObject as KMonoBehaviour;
		if (kmonoBehaviour == null)
		{
			return;
		}
		kmonoBehaviour.Subscribe(540773776, new Action<object>(this.OnRoleChanged));
		this.areEventsRegistered = true;
	}

	// Token: 0x0600A05A RID: 41050 RVA: 0x003E36D4 File Offset: 0x003E18D4
	private void UnregisterEvents()
	{
		if (!this.areEventsRegistered)
		{
			return;
		}
		this.areEventsRegistered = false;
		KMonoBehaviour kmonoBehaviour = this.identityObject as KMonoBehaviour;
		if (kmonoBehaviour == null)
		{
			return;
		}
		kmonoBehaviour.Unsubscribe(540773776, new Action<object>(this.OnRoleChanged));
	}

	// Token: 0x0600A05B RID: 41051 RVA: 0x0010CE07 File Offset: 0x0010B007
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RegisterEvents();
		this.ForceRefresh();
	}

	// Token: 0x0600A05C RID: 41052 RVA: 0x0010CE1B File Offset: 0x0010B01B
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.UnregisterEvents();
	}

	// Token: 0x0600A05D RID: 41053 RVA: 0x0010CE29 File Offset: 0x0010B029
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.UnregisterEvents();
	}

	// Token: 0x0600A05E RID: 41054 RVA: 0x003E3720 File Offset: 0x003E1920
	public void SetIdentityObject(IAssignableIdentity identity, bool jobEnabled = true)
	{
		this.UnregisterEvents();
		this.identityObject = identity;
		this.RegisterEvents();
		this.targetImage.enabled = true;
		if (this.identityObject != null)
		{
			this.targetImage.enabled = false;
		}
		if (this.useLabels && (identity is MinionIdentity || identity is MinionAssignablesProxy))
		{
			this.SetDuplicantJobTitleActive(jobEnabled);
		}
		this.requiresRefresh = true;
	}

	// Token: 0x0600A05F RID: 41055 RVA: 0x003E3788 File Offset: 0x003E1988
	public void SetSubTitle(string newTitle)
	{
		if (this.subTitle != null)
		{
			if (string.IsNullOrEmpty(newTitle))
			{
				this.subTitle.gameObject.SetActive(false);
				return;
			}
			this.subTitle.gameObject.SetActive(true);
			this.subTitle.SetText(newTitle);
		}
	}

	// Token: 0x0600A060 RID: 41056 RVA: 0x0010CE37 File Offset: 0x0010B037
	public void SetDuplicantJobTitleActive(bool state)
	{
		if (this.duplicantJob != null && this.duplicantJob.gameObject.activeInHierarchy != state)
		{
			this.duplicantJob.gameObject.SetActive(state);
		}
	}

	// Token: 0x0600A061 RID: 41057 RVA: 0x0010CE6B File Offset: 0x0010B06B
	public void ForceRefresh()
	{
		this.requiresRefresh = true;
	}

	// Token: 0x0600A062 RID: 41058 RVA: 0x0010CE74 File Offset: 0x0010B074
	public void Update()
	{
		if (this.requiresRefresh && (this.controller == null || this.controller.enabled))
		{
			this.requiresRefresh = false;
			this.Rebuild();
		}
	}

	// Token: 0x0600A063 RID: 41059 RVA: 0x003E37DC File Offset: 0x003E19DC
	private void Rebuild()
	{
		if (this.controller == null)
		{
			this.controller = base.GetComponentInChildren<KBatchedAnimController>();
			if (this.controller == null)
			{
				if (this.targetImage != null)
				{
					this.targetImage.enabled = true;
				}
				global::Debug.LogWarning("Controller for [" + base.name + "] null");
				return;
			}
		}
		CrewPortrait.SetPortraitData(this.identityObject, this.controller, this.useDefaultExpression);
		if (this.useLabels && this.duplicantName != null)
		{
			this.duplicantName.SetText((!this.identityObject.IsNullOrDestroyed()) ? this.identityObject.GetProperName() : "");
			if (this.identityObject is MinionIdentity && this.duplicantJob != null)
			{
				this.duplicantJob.SetText((this.identityObject != null) ? (this.identityObject as MinionIdentity).GetComponent<MinionResume>().GetSkillsSubtitle() : "");
				this.duplicantJob.GetComponent<ToolTip>().toolTip = (this.identityObject as MinionIdentity).GetComponent<MinionResume>().GetSkillsSubtitle();
			}
		}
	}

	// Token: 0x0600A064 RID: 41060 RVA: 0x003E3914 File Offset: 0x003E1B14
	private static void RefreshHat(IAssignableIdentity identityObject, KBatchedAnimController controller)
	{
		string hat_id = "";
		MinionIdentity minionIdentity = identityObject as MinionIdentity;
		if (minionIdentity != null)
		{
			hat_id = minionIdentity.GetComponent<MinionResume>().CurrentHat;
		}
		else if (identityObject as StoredMinionIdentity != null)
		{
			hat_id = (identityObject as StoredMinionIdentity).currentHat;
		}
		MinionResume.ApplyHat(hat_id, controller);
	}

	// Token: 0x0600A065 RID: 41061 RVA: 0x003E3968 File Offset: 0x003E1B68
	public static void SetPortraitData(IAssignableIdentity identityObject, KBatchedAnimController controller, bool useDefaultExpression = true)
	{
		if (identityObject == null)
		{
			controller.gameObject.SetActive(false);
			return;
		}
		MinionIdentity minionIdentity = identityObject as MinionIdentity;
		if (minionIdentity == null)
		{
			MinionAssignablesProxy minionAssignablesProxy = identityObject as MinionAssignablesProxy;
			if (minionAssignablesProxy != null && minionAssignablesProxy.target != null)
			{
				minionIdentity = (minionAssignablesProxy.target as MinionIdentity);
			}
		}
		controller.gameObject.SetActive(true);
		controller.Play("ui_idle", KAnim.PlayMode.Once, 1f, 0f);
		SymbolOverrideController component = controller.GetComponent<SymbolOverrideController>();
		component.RemoveAllSymbolOverrides(0);
		if (minionIdentity != null)
		{
			HashSet<KAnimHashedString> hashSet = new HashSet<KAnimHashedString>();
			HashSet<KAnimHashedString> hashSet2 = new HashSet<KAnimHashedString>();
			Accessorizer component2 = minionIdentity.GetComponent<Accessorizer>();
			foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
			{
				Accessory accessory = component2.GetAccessory(accessorySlot);
				if (accessory != null)
				{
					component.AddSymbolOverride(accessorySlot.targetSymbolId, accessory.symbol, 0);
					hashSet.Add(accessorySlot.targetSymbolId);
				}
				else
				{
					hashSet2.Add(accessorySlot.targetSymbolId);
				}
			}
			controller.BatchSetSymbolsVisiblity(hashSet, true);
			controller.BatchSetSymbolsVisiblity(hashSet2, false);
			component.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(component2.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
			CrewPortrait.RefreshHat(minionIdentity, controller);
		}
		else
		{
			HashSet<KAnimHashedString> hashSet3 = new HashSet<KAnimHashedString>();
			HashSet<KAnimHashedString> hashSet4 = new HashSet<KAnimHashedString>();
			StoredMinionIdentity storedMinionIdentity = identityObject as StoredMinionIdentity;
			if (storedMinionIdentity == null)
			{
				MinionAssignablesProxy minionAssignablesProxy2 = identityObject as MinionAssignablesProxy;
				if (minionAssignablesProxy2 != null && minionAssignablesProxy2.target != null)
				{
					storedMinionIdentity = (minionAssignablesProxy2.target as StoredMinionIdentity);
				}
			}
			if (!(storedMinionIdentity != null))
			{
				controller.gameObject.SetActive(false);
				return;
			}
			foreach (AccessorySlot accessorySlot2 in Db.Get().AccessorySlots.resources)
			{
				Accessory accessory2 = storedMinionIdentity.GetAccessory(accessorySlot2);
				if (accessory2 != null)
				{
					component.AddSymbolOverride(accessorySlot2.targetSymbolId, accessory2.symbol, 0);
					hashSet3.Add(accessorySlot2.targetSymbolId);
				}
				else
				{
					hashSet4.Add(accessorySlot2.targetSymbolId);
				}
			}
			controller.BatchSetSymbolsVisiblity(hashSet3, true);
			controller.BatchSetSymbolsVisiblity(hashSet4, false);
			component.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
			CrewPortrait.RefreshHat(storedMinionIdentity, controller);
		}
		float animScale = 0.25f;
		controller.animScale = animScale;
		string s = "ui_idle";
		controller.Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		controller.SetSymbolVisiblity("snapTo_neck", false);
		controller.SetSymbolVisiblity("snapTo_goggles", false);
	}

	// Token: 0x0600A066 RID: 41062 RVA: 0x003E3D00 File Offset: 0x003E1F00
	public void SetAlpha(float value)
	{
		if (this.controller == null)
		{
			return;
		}
		if ((float)this.controller.TintColour.a != value)
		{
			this.controller.TintColour = new Color(1f, 1f, 1f, value);
		}
	}

	// Token: 0x04007DFB RID: 32251
	public Image targetImage;

	// Token: 0x04007DFC RID: 32252
	public bool startTransparent;

	// Token: 0x04007DFD RID: 32253
	public bool useLabels = true;

	// Token: 0x04007DFE RID: 32254
	[SerializeField]
	public KBatchedAnimController controller;

	// Token: 0x04007DFF RID: 32255
	public float animScaleBase = 0.2f;

	// Token: 0x04007E00 RID: 32256
	public LocText duplicantName;

	// Token: 0x04007E01 RID: 32257
	public LocText duplicantJob;

	// Token: 0x04007E02 RID: 32258
	public LocText subTitle;

	// Token: 0x04007E03 RID: 32259
	public bool useDefaultExpression = true;

	// Token: 0x04007E04 RID: 32260
	private bool requiresRefresh;

	// Token: 0x04007E05 RID: 32261
	private bool areEventsRegistered;
}
