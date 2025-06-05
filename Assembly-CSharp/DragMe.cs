using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200000D RID: 13
public class DragMe : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
{
	// Token: 0x0600002F RID: 47 RVA: 0x0014628C File Offset: 0x0014448C
	public void OnBeginDrag(PointerEventData eventData)
	{
		Canvas canvas = DragMe.FindInParents<Canvas>(base.gameObject);
		if (canvas == null)
		{
			return;
		}
		this.m_DraggingIcon = UnityEngine.Object.Instantiate<GameObject>(base.gameObject, canvas.transform, false);
		GraphicRaycaster component = this.m_DraggingIcon.GetComponent<GraphicRaycaster>();
		if (component != null)
		{
			component.enabled = false;
		}
		this.m_DraggingIcon.name = "dragObj";
		this.m_DraggingIcon.transform.SetAsLastSibling();
		RectTransform component2 = this.m_DraggingIcon.GetComponent<RectTransform>();
		component2.pivot = Vector2.zero;
		component2.sizeDelta = base.GetComponent<RectTransform>().rect.size;
		this.x = this.m_DraggingIcon.transform.position.x;
		Canvas component3 = this.m_DraggingIcon.GetComponent<Canvas>();
		component3.overrideSorting = true;
		component3.sortingOrder = 99;
		if (this.dragOnSurfaces)
		{
			this.m_DraggingPlane = (base.transform as RectTransform);
		}
		else
		{
			this.m_DraggingPlane = (canvas.transform as RectTransform);
		}
		this.SetDraggedPosition(eventData);
		this.listener.OnBeginDrag(eventData.position);
	}

	// Token: 0x06000030 RID: 48 RVA: 0x000AA03A File Offset: 0x000A823A
	public void OnDrag(PointerEventData data)
	{
		if (this.m_DraggingIcon != null)
		{
			this.SetDraggedPosition(data);
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x001463AC File Offset: 0x001445AC
	private void SetDraggedPosition(PointerEventData data)
	{
		if (this.dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
		{
			this.m_DraggingPlane = (data.pointerEnter.transform as RectTransform);
		}
		RectTransform component = this.m_DraggingIcon.GetComponent<RectTransform>();
		Vector3 position;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.m_DraggingPlane, data.position, data.pressEventCamera, out position))
		{
			position.x = this.x + 5f;
			position.y -= component.sizeDelta.y / 2f;
			component.position = position;
			component.rotation = this.m_DraggingPlane.rotation;
		}
	}

	// Token: 0x06000032 RID: 50 RVA: 0x000AA051 File Offset: 0x000A8251
	public void OnEndDrag(PointerEventData eventData)
	{
		this.listener.OnEndDrag(eventData.position);
		if (this.m_DraggingIcon != null)
		{
			UnityEngine.Object.Destroy(this.m_DraggingIcon);
		}
	}

	// Token: 0x06000033 RID: 51 RVA: 0x0014646C File Offset: 0x0014466C
	public static T FindInParents<T>(GameObject go) where T : Component
	{
		if (go == null)
		{
			return default(T);
		}
		T t = default(T);
		Transform parent = go.transform.parent;
		while (parent != null && t == null)
		{
			t = parent.gameObject.GetComponent<T>();
			parent = parent.parent;
		}
		return t;
	}

	// Token: 0x04000034 RID: 52
	public bool dragOnSurfaces = true;

	// Token: 0x04000035 RID: 53
	private GameObject m_DraggingIcon;

	// Token: 0x04000036 RID: 54
	private RectTransform m_DraggingPlane;

	// Token: 0x04000037 RID: 55
	private float x;

	// Token: 0x04000038 RID: 56
	public DragMe.IDragListener listener;

	// Token: 0x0200000E RID: 14
	public interface IDragListener
	{
		// Token: 0x06000035 RID: 53
		void OnBeginDrag(Vector2 position);

		// Token: 0x06000036 RID: 54
		void OnEndDrag(Vector2 position);
	}
}
