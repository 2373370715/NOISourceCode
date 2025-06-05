using System;
using UnityEngine;

// Token: 0x0200148F RID: 5263
[AddComponentMenu("KMonoBehaviour/scripts/SelectMarker")]
public class SelectMarker : KMonoBehaviour
{
	// Token: 0x06006D0B RID: 27915 RVA: 0x000EC17C File Offset: 0x000EA37C
	public void SetTargetTransform(Transform target_transform)
	{
		this.targetTransform = target_transform;
		this.LateUpdate();
	}

	// Token: 0x06006D0C RID: 27916 RVA: 0x002F6F58 File Offset: 0x002F5158
	private void LateUpdate()
	{
		if (this.targetTransform == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		Vector3 position = this.targetTransform.GetPosition();
		KCollider2D component = this.targetTransform.GetComponent<KCollider2D>();
		if (component != null)
		{
			position.x = component.bounds.center.x;
			position.y = component.bounds.center.y + component.bounds.size.y / 2f + 0.1f;
		}
		else
		{
			position.y += 2f;
		}
		Vector3 b = new Vector3(0f, (Mathf.Sin(Time.unscaledTime * 4f) + 1f) * this.animationOffset, 0f);
		base.transform.SetPosition(position + b);
	}

	// Token: 0x0400523F RID: 21055
	public float animationOffset = 0.1f;

	// Token: 0x04005240 RID: 21056
	private Transform targetTransform;
}
