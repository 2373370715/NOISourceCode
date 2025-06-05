using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001EB7 RID: 7863
public class NotificationHighlightController : KMonoBehaviour
{
	// Token: 0x0600A4F4 RID: 42228 RVA: 0x0010F72C File Offset: 0x0010D92C
	protected override void OnSpawn()
	{
		this.highlightBox = Util.KInstantiateUI<RectTransform>(this.highlightBoxPrefab.gameObject, base.gameObject, false);
		this.HideBox();
	}

	// Token: 0x0600A4F5 RID: 42229 RVA: 0x003F7D8C File Offset: 0x003F5F8C
	[ContextMenu("Force Update")]
	protected void LateUpdate()
	{
		bool flag = false;
		if (this.activeTargetNotification != null)
		{
			foreach (NotificationHighlightTarget notificationHighlightTarget in this.targets)
			{
				if (notificationHighlightTarget.targetKey == this.activeTargetNotification.highlightTarget)
				{
					this.SnapBoxToTarget(notificationHighlightTarget);
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			this.HideBox();
		}
	}

	// Token: 0x0600A4F6 RID: 42230 RVA: 0x0010F751 File Offset: 0x0010D951
	public void AddTarget(NotificationHighlightTarget target)
	{
		this.targets.Add(target);
	}

	// Token: 0x0600A4F7 RID: 42231 RVA: 0x0010F75F File Offset: 0x0010D95F
	public void RemoveTarget(NotificationHighlightTarget target)
	{
		this.targets.Remove(target);
	}

	// Token: 0x0600A4F8 RID: 42232 RVA: 0x0010F76E File Offset: 0x0010D96E
	public void SetActiveTarget(ManagementMenuNotification notification)
	{
		this.activeTargetNotification = notification;
	}

	// Token: 0x0600A4F9 RID: 42233 RVA: 0x0010F777 File Offset: 0x0010D977
	public void ClearActiveTarget(ManagementMenuNotification checkNotification)
	{
		if (checkNotification == this.activeTargetNotification)
		{
			this.activeTargetNotification = null;
		}
	}

	// Token: 0x0600A4FA RID: 42234 RVA: 0x0010F789 File Offset: 0x0010D989
	public void ClearActiveTarget()
	{
		this.activeTargetNotification = null;
	}

	// Token: 0x0600A4FB RID: 42235 RVA: 0x0010F792 File Offset: 0x0010D992
	public void TargetViewed(NotificationHighlightTarget target)
	{
		if (this.activeTargetNotification != null && this.activeTargetNotification.highlightTarget == target.targetKey)
		{
			this.activeTargetNotification.View();
		}
	}

	// Token: 0x0600A4FC RID: 42236 RVA: 0x003F7E10 File Offset: 0x003F6010
	private void SnapBoxToTarget(NotificationHighlightTarget target)
	{
		RectTransform rectTransform = target.rectTransform();
		Vector3 position = rectTransform.GetPosition();
		this.highlightBox.sizeDelta = rectTransform.rect.size;
		this.highlightBox.SetPosition(position + new Vector3(rectTransform.rect.position.x, rectTransform.rect.position.y, 0f));
		RectMask2D componentInParent = rectTransform.GetComponentInParent<RectMask2D>();
		if (componentInParent != null)
		{
			RectTransform rectTransform2 = componentInParent.rectTransform();
			Vector3 a = rectTransform2.TransformPoint(rectTransform2.rect.min);
			Vector3 a2 = rectTransform2.TransformPoint(rectTransform2.rect.max);
			Vector3 b = this.highlightBox.TransformPoint(this.highlightBox.rect.min);
			Vector3 b2 = this.highlightBox.TransformPoint(this.highlightBox.rect.max);
			Vector3 vector = a - b;
			Vector3 vector2 = a2 - b2;
			if (vector.x > 0f)
			{
				this.highlightBox.anchoredPosition = this.highlightBox.anchoredPosition + new Vector2(vector.x, 0f);
				this.highlightBox.sizeDelta -= new Vector2(vector.x, 0f);
			}
			else if (vector.y > 0f)
			{
				this.highlightBox.anchoredPosition = this.highlightBox.anchoredPosition + new Vector2(0f, vector.y);
				this.highlightBox.sizeDelta -= new Vector2(0f, vector.y);
			}
			if (vector2.x < 0f)
			{
				this.highlightBox.sizeDelta += new Vector2(vector2.x, 0f);
			}
			if (vector2.y < 0f)
			{
				this.highlightBox.sizeDelta += new Vector2(0f, vector2.y);
			}
		}
		this.highlightBox.gameObject.SetActive(this.highlightBox.sizeDelta.x > 0f && this.highlightBox.sizeDelta.y > 0f);
	}

	// Token: 0x0600A4FD RID: 42237 RVA: 0x0010F7BF File Offset: 0x0010D9BF
	private void HideBox()
	{
		this.highlightBox.gameObject.SetActive(false);
	}

	// Token: 0x04008104 RID: 33028
	public RectTransform highlightBoxPrefab;

	// Token: 0x04008105 RID: 33029
	private RectTransform highlightBox;

	// Token: 0x04008106 RID: 33030
	private List<NotificationHighlightTarget> targets = new List<NotificationHighlightTarget>();

	// Token: 0x04008107 RID: 33031
	private ManagementMenuNotification activeTargetNotification;
}
