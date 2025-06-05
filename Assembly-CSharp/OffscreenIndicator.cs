using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020016C7 RID: 5831
[AddComponentMenu("KMonoBehaviour/scripts/OffscreenIndicator")]
public class OffscreenIndicator : KMonoBehaviour
{
	// Token: 0x06007856 RID: 30806 RVA: 0x000F3A5A File Offset: 0x000F1C5A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		OffscreenIndicator.Instance = this;
	}

	// Token: 0x06007857 RID: 30807 RVA: 0x000F3A68 File Offset: 0x000F1C68
	protected override void OnForcedCleanUp()
	{
		OffscreenIndicator.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06007858 RID: 30808 RVA: 0x0031DF24 File Offset: 0x0031C124
	private void Update()
	{
		foreach (KeyValuePair<GameObject, GameObject> keyValuePair in this.targets)
		{
			this.UpdateArrow(keyValuePair.Value, keyValuePair.Key);
		}
	}

	// Token: 0x06007859 RID: 30809 RVA: 0x0031DF84 File Offset: 0x0031C184
	public void ActivateIndicator(GameObject target)
	{
		if (!this.targets.ContainsKey(target))
		{
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(target, "ui", false);
			if (uisprite != null)
			{
				this.ActivateIndicator(target, uisprite);
			}
		}
	}

	// Token: 0x0600785A RID: 30810 RVA: 0x0031DFB8 File Offset: 0x0031C1B8
	public void ActivateIndicator(GameObject target, GameObject iconSource)
	{
		if (!this.targets.ContainsKey(target))
		{
			MinionIdentity component = iconSource.GetComponent<MinionIdentity>();
			if (component != null)
			{
				GameObject gameObject = Util.KInstantiateUI(this.IndicatorPrefab, this.IndicatorContainer, true);
				gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("icon").gameObject.SetActive(false);
				CrewPortrait reference = gameObject.GetComponent<HierarchyReferences>().GetReference<CrewPortrait>("Portrait");
				reference.gameObject.SetActive(true);
				reference.SetIdentityObject(component, true);
				this.targets.Add(target, gameObject);
			}
		}
	}

	// Token: 0x0600785B RID: 30811 RVA: 0x0031E044 File Offset: 0x0031C244
	public void ActivateIndicator(GameObject target, global::Tuple<Sprite, Color> icon)
	{
		if (!this.targets.ContainsKey(target))
		{
			GameObject gameObject = Util.KInstantiateUI(this.IndicatorPrefab, this.IndicatorContainer, true);
			Image reference = gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("icon");
			if (icon != null)
			{
				reference.sprite = icon.first;
				reference.color = icon.second;
				this.targets.Add(target, gameObject);
			}
		}
	}

	// Token: 0x0600785C RID: 30812 RVA: 0x000F3A76 File Offset: 0x000F1C76
	public void DeactivateIndicator(GameObject target)
	{
		if (this.targets.ContainsKey(target))
		{
			UnityEngine.Object.Destroy(this.targets[target]);
			this.targets.Remove(target);
		}
	}

	// Token: 0x0600785D RID: 30813 RVA: 0x0031E0AC File Offset: 0x0031C2AC
	private void UpdateArrow(GameObject arrow, GameObject target)
	{
		if (target == null)
		{
			UnityEngine.Object.Destroy(arrow);
			this.targets.Remove(target);
			return;
		}
		Vector3 vector = Camera.main.WorldToViewportPoint(target.transform.position);
		if ((double)vector.x > 0.3 && (double)vector.x < 0.7 && (double)vector.y > 0.3 && (double)vector.y < 0.7)
		{
			arrow.GetComponent<HierarchyReferences>().GetReference<CrewPortrait>("Portrait").SetIdentityObject(null, true);
			arrow.SetActive(false);
			return;
		}
		arrow.SetActive(true);
		arrow.rectTransform().SetLocalPosition(Vector3.zero);
		Vector3 b = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
		b.z = target.transform.position.z;
		Vector3 normalized = (target.transform.position - b).normalized;
		arrow.transform.up = normalized;
		this.UpdateTargetIconPosition(target, arrow);
	}

	// Token: 0x0600785E RID: 30814 RVA: 0x0031E1D0 File Offset: 0x0031C3D0
	private void UpdateTargetIconPosition(GameObject goTarget, GameObject indicator)
	{
		Vector3 vector = goTarget.transform.position;
		vector = Camera.main.WorldToViewportPoint(vector);
		if (vector.z < 0f)
		{
			vector.x = 1f - vector.x;
			vector.y = 1f - vector.y;
			vector.z = 0f;
			vector = this.Vector3Maxamize(vector);
		}
		vector = Camera.main.ViewportToScreenPoint(vector);
		vector.x = Mathf.Clamp(vector.x, this.edgeInset, (float)Screen.width - this.edgeInset);
		vector.y = Mathf.Clamp(vector.y, this.edgeInset, (float)Screen.height - this.edgeInset);
		indicator.transform.position = vector;
		indicator.GetComponent<HierarchyReferences>().GetReference<Image>("icon").rectTransform.up = Vector3.up;
		indicator.GetComponent<HierarchyReferences>().GetReference<CrewPortrait>("Portrait").transform.up = Vector3.up;
	}

	// Token: 0x0600785F RID: 30815 RVA: 0x0031E2DC File Offset: 0x0031C4DC
	public Vector3 Vector3Maxamize(Vector3 vector)
	{
		float num = 0f;
		num = ((vector.x > num) ? vector.x : num);
		num = ((vector.y > num) ? vector.y : num);
		num = ((vector.z > num) ? vector.z : num);
		return vector / num;
	}

	// Token: 0x04005A69 RID: 23145
	public GameObject IndicatorPrefab;

	// Token: 0x04005A6A RID: 23146
	public GameObject IndicatorContainer;

	// Token: 0x04005A6B RID: 23147
	private Dictionary<GameObject, GameObject> targets = new Dictionary<GameObject, GameObject>();

	// Token: 0x04005A6C RID: 23148
	public static OffscreenIndicator Instance;

	// Token: 0x04005A6D RID: 23149
	[SerializeField]
	private float edgeInset = 25f;
}
