using System;
using UnityEngine;

// Token: 0x02001F13 RID: 7955
public class QuickLayout : KMonoBehaviour
{
	// Token: 0x0600A752 RID: 42834 RVA: 0x00111055 File Offset: 0x0010F255
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ForceUpdate();
	}

	// Token: 0x0600A753 RID: 42835 RVA: 0x00111063 File Offset: 0x0010F263
	private void OnEnable()
	{
		this.ForceUpdate();
	}

	// Token: 0x0600A754 RID: 42836 RVA: 0x0011106B File Offset: 0x0010F26B
	private void LateUpdate()
	{
		this.Run(false);
	}

	// Token: 0x0600A755 RID: 42837 RVA: 0x00111074 File Offset: 0x0010F274
	public void ForceUpdate()
	{
		this.Run(true);
	}

	// Token: 0x0600A756 RID: 42838 RVA: 0x0040461C File Offset: 0x0040281C
	private void Run(bool forceUpdate = false)
	{
		forceUpdate = (forceUpdate || this._elementSize != this.elementSize);
		forceUpdate = (forceUpdate || this._spacing != this.spacing);
		forceUpdate = (forceUpdate || this._layoutDirection != this.layoutDirection);
		forceUpdate = (forceUpdate || this._offset != this.offset);
		if (forceUpdate)
		{
			this._elementSize = this.elementSize;
			this._spacing = this.spacing;
			this._layoutDirection = this.layoutDirection;
			this._offset = this.offset;
		}
		int num = 0;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.activeInHierarchy)
			{
				num++;
			}
		}
		if (num != this.oldActiveChildCount || forceUpdate)
		{
			this.Layout();
			this.oldActiveChildCount = num;
		}
	}

	// Token: 0x0600A757 RID: 42839 RVA: 0x00404714 File Offset: 0x00402914
	public void Layout()
	{
		Vector3 vector = this._offset;
		bool flag = false;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.activeInHierarchy)
			{
				flag = true;
				base.transform.GetChild(i).rectTransform().anchoredPosition = vector;
				vector += (float)(this._elementSize + this._spacing) * this.GetDirectionVector();
			}
		}
		if (this.driveParentRectSize != null)
		{
			if (!flag)
			{
				if (this._layoutDirection == QuickLayout.LayoutDirection.BottomToTop || this._layoutDirection == QuickLayout.LayoutDirection.TopToBottom)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(Mathf.Abs(this.driveParentRectSize.sizeDelta.x), 0f);
					return;
				}
				if (this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight || this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(0f, Mathf.Abs(this.driveParentRectSize.sizeDelta.y));
					return;
				}
			}
			else
			{
				if (this._layoutDirection == QuickLayout.LayoutDirection.BottomToTop || this._layoutDirection == QuickLayout.LayoutDirection.TopToBottom)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(this.driveParentRectSize.sizeDelta.x, Mathf.Abs(vector.y));
					return;
				}
				if (this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight || this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(Mathf.Abs(vector.x), this.driveParentRectSize.sizeDelta.y);
				}
			}
		}
	}

	// Token: 0x0600A758 RID: 42840 RVA: 0x004048AC File Offset: 0x00402AAC
	private Vector2 GetDirectionVector()
	{
		Vector2 result = Vector3.zero;
		switch (this._layoutDirection)
		{
		case QuickLayout.LayoutDirection.TopToBottom:
			result = Vector2.down;
			break;
		case QuickLayout.LayoutDirection.BottomToTop:
			result = Vector2.up;
			break;
		case QuickLayout.LayoutDirection.LeftToRight:
			result = Vector2.right;
			break;
		case QuickLayout.LayoutDirection.RightToLeft:
			result = Vector2.left;
			break;
		}
		return result;
	}

	// Token: 0x0400833E RID: 33598
	[Header("Configuration")]
	[SerializeField]
	private int elementSize;

	// Token: 0x0400833F RID: 33599
	[SerializeField]
	private int spacing;

	// Token: 0x04008340 RID: 33600
	[SerializeField]
	private QuickLayout.LayoutDirection layoutDirection;

	// Token: 0x04008341 RID: 33601
	[SerializeField]
	private Vector2 offset;

	// Token: 0x04008342 RID: 33602
	[SerializeField]
	private RectTransform driveParentRectSize;

	// Token: 0x04008343 RID: 33603
	private int _elementSize;

	// Token: 0x04008344 RID: 33604
	private int _spacing;

	// Token: 0x04008345 RID: 33605
	private QuickLayout.LayoutDirection _layoutDirection;

	// Token: 0x04008346 RID: 33606
	private Vector2 _offset;

	// Token: 0x04008347 RID: 33607
	private int oldActiveChildCount;

	// Token: 0x02001F14 RID: 7956
	private enum LayoutDirection
	{
		// Token: 0x04008349 RID: 33609
		TopToBottom,
		// Token: 0x0400834A RID: 33610
		BottomToTop,
		// Token: 0x0400834B RID: 33611
		LeftToRight,
		// Token: 0x0400834C RID: 33612
		RightToLeft
	}
}
