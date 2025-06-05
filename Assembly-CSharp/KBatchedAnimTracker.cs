using System;
using UnityEngine;

// Token: 0x0200094F RID: 2383
public class KBatchedAnimTracker : MonoBehaviour
{
	// Token: 0x06002A8C RID: 10892 RVA: 0x001E6EF0 File Offset: 0x001E50F0
	private void Start()
	{
		if (this.controller == null)
		{
			Transform parent = base.transform.parent;
			while (parent != null)
			{
				this.controller = parent.GetComponent<KBatchedAnimController>();
				if (this.controller != null)
				{
					break;
				}
				parent = parent.parent;
			}
		}
		if (this.controller == null)
		{
			global::Debug.Log("Controller Null for tracker on " + base.gameObject.name, base.gameObject);
			base.enabled = false;
			return;
		}
		this.controller.onAnimEnter += this.OnAnimStart;
		this.controller.onAnimComplete += this.OnAnimStop;
		this.controller.onLayerChanged += this.OnLayerChanged;
		this.forceUpdate = true;
		if (this.myAnim != null)
		{
			return;
		}
		this.myAnim = base.GetComponent<KBatchedAnimController>();
		KBatchedAnimController kbatchedAnimController = this.myAnim;
		kbatchedAnimController.getPositionDataFunctionInUse = (Func<Vector4>)Delegate.Combine(kbatchedAnimController.getPositionDataFunctionInUse, new Func<Vector4>(this.MyAnimGetPosition));
	}

	// Token: 0x06002A8D RID: 10893 RVA: 0x001E7008 File Offset: 0x001E5208
	private Vector4 MyAnimGetPosition()
	{
		if (this.myAnim != null && this.controller != null && this.controller.transform == this.myAnim.transform.parent)
		{
			Vector3 pivotSymbolPosition = this.myAnim.GetPivotSymbolPosition();
			return new Vector4(pivotSymbolPosition.x - this.controller.Offset.x, pivotSymbolPosition.y - this.controller.Offset.y, pivotSymbolPosition.x, pivotSymbolPosition.y);
		}
		return base.transform.GetPosition();
	}

	// Token: 0x06002A8E RID: 10894 RVA: 0x001E70B0 File Offset: 0x001E52B0
	private void OnDestroy()
	{
		if (this.controller != null)
		{
			this.controller.onAnimEnter -= this.OnAnimStart;
			this.controller.onAnimComplete -= this.OnAnimStop;
			this.controller.onLayerChanged -= this.OnLayerChanged;
			this.controller = null;
		}
		if (this.myAnim != null)
		{
			KBatchedAnimController kbatchedAnimController = this.myAnim;
			kbatchedAnimController.getPositionDataFunctionInUse = (Func<Vector4>)Delegate.Remove(kbatchedAnimController.getPositionDataFunctionInUse, new Func<Vector4>(this.MyAnimGetPosition));
		}
		this.myAnim = null;
	}

	// Token: 0x06002A8F RID: 10895 RVA: 0x001E7154 File Offset: 0x001E5354
	private void LateUpdate()
	{
		if (this.controller != null && (this.controller.IsVisible() || this.forceAlwaysVisible || this.forceUpdate))
		{
			this.UpdateFrame();
		}
		if (!this.alive)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06002A90 RID: 10896 RVA: 0x000C0298 File Offset: 0x000BE498
	public void SetAnimControllers(KBatchedAnimController controller, KBatchedAnimController parentController)
	{
		this.myAnim = controller;
		this.controller = parentController;
	}

	// Token: 0x06002A91 RID: 10897 RVA: 0x001E71A4 File Offset: 0x001E53A4
	private void UpdateFrame()
	{
		this.forceUpdate = false;
		bool flag = false;
		if (this.controller.CurrentAnim != null)
		{
			Matrix2x3 symbolLocalTransform = this.controller.GetSymbolLocalTransform(this.symbol, out flag);
			Vector3 position = this.controller.transform.GetPosition();
			if (flag && (this.previousMatrix != symbolLocalTransform || position != this.previousPosition || (this.useTargetPoint && this.targetPoint != this.previousTargetPoint) || (this.matchParentOffset && this.myAnim.Offset != this.controller.Offset)))
			{
				this.previousMatrix = symbolLocalTransform;
				this.previousPosition = position;
				Matrix2x3 overrideTransformMatrix = ((this.useTargetPoint || this.myAnim == null) ? this.controller.GetTransformMatrix() : this.controller.GetTransformMatrix(new Vector2(this.myAnim.animWidth * this.myAnim.animScale, -this.myAnim.animHeight * this.myAnim.animScale))) * symbolLocalTransform;
				float z = base.transform.GetPosition().z;
				base.transform.SetPosition(overrideTransformMatrix.MultiplyPoint(this.offset));
				if (this.useTargetPoint)
				{
					this.previousTargetPoint = this.targetPoint;
					Vector3 position2 = base.transform.GetPosition();
					position2.z = 0f;
					Vector3 vector = this.targetPoint - position2;
					float num = Vector3.Angle(vector, Vector3.right);
					if (vector.y < 0f)
					{
						num = 360f - num;
					}
					base.transform.localRotation = Quaternion.identity;
					base.transform.RotateAround(position2, new Vector3(0f, 0f, 1f), num);
					float sqrMagnitude = vector.sqrMagnitude;
					this.myAnim.GetBatchInstanceData().SetClipRadius(base.transform.GetPosition().x, base.transform.GetPosition().y, sqrMagnitude, true);
				}
				else
				{
					Vector3 v = this.controller.FlipX ? Vector3.left : Vector3.right;
					Vector3 v2 = this.controller.FlipY ? Vector3.down : Vector3.up;
					base.transform.up = overrideTransformMatrix.MultiplyVector(v2);
					base.transform.right = overrideTransformMatrix.MultiplyVector(v);
					if (this.myAnim != null)
					{
						KBatchedAnimInstanceData batchInstanceData = this.myAnim.GetBatchInstanceData();
						if (batchInstanceData != null)
						{
							batchInstanceData.SetOverrideTransformMatrix(overrideTransformMatrix);
						}
					}
				}
				base.transform.SetPosition(new Vector3(base.transform.GetPosition().x, base.transform.GetPosition().y, z));
				if (this.matchParentOffset)
				{
					this.myAnim.Offset = this.controller.Offset;
				}
				this.myAnim.SetDirty();
			}
		}
		if (this.myAnim != null && flag != this.myAnim.enabled && this.synchronizeEnabledState)
		{
			this.myAnim.enabled = flag;
		}
	}

	// Token: 0x06002A92 RID: 10898 RVA: 0x000C02A8 File Offset: 0x000BE4A8
	[ContextMenu("ForceAlive")]
	private void OnAnimStart(HashedString name)
	{
		this.alive = true;
		base.enabled = true;
		this.forceUpdate = true;
	}

	// Token: 0x06002A93 RID: 10899 RVA: 0x000C02BF File Offset: 0x000BE4BF
	private void OnAnimStop(HashedString name)
	{
		if (!this.forceAlwaysAlive)
		{
			this.alive = false;
		}
	}

	// Token: 0x06002A94 RID: 10900 RVA: 0x000C02D0 File Offset: 0x000BE4D0
	private void OnLayerChanged(int layer)
	{
		this.myAnim.SetLayer(layer);
	}

	// Token: 0x06002A95 RID: 10901 RVA: 0x000C02DE File Offset: 0x000BE4DE
	public void SetTarget(Vector3 target)
	{
		this.targetPoint = target;
		this.targetPoint.z = 0f;
	}

	// Token: 0x04001CCA RID: 7370
	public KBatchedAnimController controller;

	// Token: 0x04001CCB RID: 7371
	public Vector3 offset = Vector3.zero;

	// Token: 0x04001CCC RID: 7372
	public HashedString symbol;

	// Token: 0x04001CCD RID: 7373
	public Vector3 targetPoint = Vector3.zero;

	// Token: 0x04001CCE RID: 7374
	public Vector3 previousTargetPoint;

	// Token: 0x04001CCF RID: 7375
	public bool useTargetPoint;

	// Token: 0x04001CD0 RID: 7376
	public bool fadeOut = true;

	// Token: 0x04001CD1 RID: 7377
	public bool forceAlwaysVisible;

	// Token: 0x04001CD2 RID: 7378
	public bool matchParentOffset;

	// Token: 0x04001CD3 RID: 7379
	public bool forceAlwaysAlive;

	// Token: 0x04001CD4 RID: 7380
	private bool alive = true;

	// Token: 0x04001CD5 RID: 7381
	private bool forceUpdate;

	// Token: 0x04001CD6 RID: 7382
	private Matrix2x3 previousMatrix;

	// Token: 0x04001CD7 RID: 7383
	private Vector3 previousPosition;

	// Token: 0x04001CD8 RID: 7384
	public bool synchronizeEnabledState = true;

	// Token: 0x04001CD9 RID: 7385
	[SerializeField]
	private KBatchedAnimController myAnim;
}
