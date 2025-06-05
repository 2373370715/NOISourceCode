using System;
using UnityEngine;

// Token: 0x02000936 RID: 2358
public class BatchAnimCamera : MonoBehaviour
{
	// Token: 0x06002968 RID: 10600 RVA: 0x000BF624 File Offset: 0x000BD824
	private void Awake()
	{
		this.cam = base.GetComponent<Camera>();
	}

	// Token: 0x06002969 RID: 10601 RVA: 0x001E2694 File Offset: 0x001E0894
	private void Update()
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.right * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.left * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.up * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			base.transform.SetPosition(base.transform.GetPosition() + Vector3.down * BatchAnimCamera.pan_speed * Time.deltaTime);
		}
		this.ClampToBounds();
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.do_pan = true;
				this.last_pan = KInputManager.GetMousePos();
			}
			else if (Input.GetMouseButton(0) && this.do_pan)
			{
				Vector3 vector = this.cam.ScreenToViewportPoint(this.last_pan - KInputManager.GetMousePos());
				Vector3 translation = new Vector3(vector.x * BatchAnimCamera.pan_speed, vector.y * BatchAnimCamera.pan_speed, 0f);
				base.transform.Translate(translation, Space.World);
				this.ClampToBounds();
				this.last_pan = KInputManager.GetMousePos();
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.do_pan = false;
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis != 0f)
		{
			this.cam.fieldOfView = Mathf.Clamp(this.cam.fieldOfView - axis * BatchAnimCamera.zoom_speed, this.zoom_min, this.zoom_max);
		}
	}

	// Token: 0x0600296A RID: 10602 RVA: 0x001E2898 File Offset: 0x001E0A98
	private void ClampToBounds()
	{
		Vector3 position = base.transform.GetPosition();
		position.x = Mathf.Clamp(base.transform.GetPosition().x, BatchAnimCamera.bounds.min.x, BatchAnimCamera.bounds.max.x);
		position.y = Mathf.Clamp(base.transform.GetPosition().y, BatchAnimCamera.bounds.min.y, BatchAnimCamera.bounds.max.y);
		position.z = Mathf.Clamp(base.transform.GetPosition().z, BatchAnimCamera.bounds.min.z, BatchAnimCamera.bounds.max.z);
		base.transform.SetPosition(position);
	}

	// Token: 0x0600296B RID: 10603 RVA: 0x000BF632 File Offset: 0x000BD832
	private void OnDrawGizmosSelected()
	{
		DebugExtension.DebugBounds(BatchAnimCamera.bounds, Color.red, 0f, true);
	}

	// Token: 0x04001C2E RID: 7214
	private static readonly float pan_speed = 5f;

	// Token: 0x04001C2F RID: 7215
	private static readonly float zoom_speed = 5f;

	// Token: 0x04001C30 RID: 7216
	public static Bounds bounds = new Bounds(new Vector3(0f, 0f, -50f), new Vector3(0f, 0f, 50f));

	// Token: 0x04001C31 RID: 7217
	private float zoom_min = 1f;

	// Token: 0x04001C32 RID: 7218
	private float zoom_max = 100f;

	// Token: 0x04001C33 RID: 7219
	private Camera cam;

	// Token: 0x04001C34 RID: 7220
	private bool do_pan;

	// Token: 0x04001C35 RID: 7221
	private Vector3 last_pan;
}
