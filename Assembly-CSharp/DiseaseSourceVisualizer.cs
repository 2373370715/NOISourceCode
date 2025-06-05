using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200127F RID: 4735
[AddComponentMenu("KMonoBehaviour/scripts/DiseaseSourceVisualizer")]
public class DiseaseSourceVisualizer : KMonoBehaviour
{
	// Token: 0x060060AC RID: 24748 RVA: 0x000E35E3 File Offset: 0x000E17E3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateVisibility();
		Components.DiseaseSourceVisualizers.Add(this);
	}

	// Token: 0x060060AD RID: 24749 RVA: 0x002BD240 File Offset: 0x002BB440
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnViewModeChanged));
		base.OnCleanUp();
		Components.DiseaseSourceVisualizers.Remove(this);
		if (this.visualizer != null)
		{
			UnityEngine.Object.Destroy(this.visualizer);
			this.visualizer = null;
		}
	}

	// Token: 0x060060AE RID: 24750 RVA: 0x002BD2A4 File Offset: 0x002BB4A4
	private void CreateVisualizer()
	{
		if (this.visualizer != null)
		{
			return;
		}
		if (GameScreenManager.Instance.worldSpaceCanvas == null)
		{
			return;
		}
		this.visualizer = Util.KInstantiate(Assets.UIPrefabs.ResourceVisualizer, GameScreenManager.Instance.worldSpaceCanvas, null);
	}

	// Token: 0x060060AF RID: 24751 RVA: 0x002BD2F4 File Offset: 0x002BB4F4
	public void UpdateVisibility()
	{
		this.CreateVisualizer();
		if (string.IsNullOrEmpty(this.alwaysShowDisease))
		{
			this.visible = false;
		}
		else
		{
			Disease disease = Db.Get().Diseases.Get(this.alwaysShowDisease);
			if (disease != null)
			{
				this.SetVisibleDisease(disease);
			}
		}
		if (OverlayScreen.Instance != null)
		{
			this.Show(OverlayScreen.Instance.GetMode());
		}
	}

	// Token: 0x060060B0 RID: 24752 RVA: 0x002BD35C File Offset: 0x002BB55C
	private void SetVisibleDisease(Disease disease)
	{
		Sprite overlaySprite = Assets.instance.DiseaseVisualization.overlaySprite;
		Color32 colorByName = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
		Image component = this.visualizer.transform.GetChild(0).GetComponent<Image>();
		component.sprite = overlaySprite;
		component.color = colorByName;
		this.visible = true;
	}

	// Token: 0x060060B1 RID: 24753 RVA: 0x000E35FC File Offset: 0x000E17FC
	private void Update()
	{
		if (this.visualizer == null)
		{
			return;
		}
		this.visualizer.transform.SetPosition(base.transform.GetPosition() + this.offset);
	}

	// Token: 0x060060B2 RID: 24754 RVA: 0x000E3634 File Offset: 0x000E1834
	private void OnViewModeChanged(HashedString mode)
	{
		this.Show(mode);
	}

	// Token: 0x060060B3 RID: 24755 RVA: 0x000E363D File Offset: 0x000E183D
	public void Show(HashedString mode)
	{
		base.enabled = (this.visible && mode == OverlayModes.Disease.ID);
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(base.enabled);
		}
	}

	// Token: 0x04004515 RID: 17685
	[SerializeField]
	private Vector3 offset;

	// Token: 0x04004516 RID: 17686
	private GameObject visualizer;

	// Token: 0x04004517 RID: 17687
	private bool visible;

	// Token: 0x04004518 RID: 17688
	public string alwaysShowDisease;
}
