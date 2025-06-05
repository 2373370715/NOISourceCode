using System;
using UnityEngine;

// Token: 0x0200175C RID: 5980
public class FogOfWarPostFX : MonoBehaviour
{
	// Token: 0x06007B00 RID: 31488 RVA: 0x000F57F0 File Offset: 0x000F39F0
	private void Awake()
	{
		if (this.shader != null)
		{
			this.material = new Material(this.shader);
		}
	}

	// Token: 0x06007B01 RID: 31489 RVA: 0x000F5811 File Offset: 0x000F3A11
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.SetupUVs();
		Graphics.Blit(source, destination, this.material, 0);
	}

	// Token: 0x06007B02 RID: 31490 RVA: 0x00327F88 File Offset: 0x00326188
	private void SetupUVs()
	{
		if (this.myCamera == null)
		{
			this.myCamera = base.GetComponent<Camera>();
			if (this.myCamera == null)
			{
				return;
			}
		}
		Ray ray = this.myCamera.ViewportPointToRay(Vector3.zero);
		float distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		Vector3 point = ray.GetPoint(distance);
		Vector4 vector;
		vector.x = point.x / Grid.WidthInMeters;
		vector.y = point.y / Grid.HeightInMeters;
		ray = this.myCamera.ViewportPointToRay(Vector3.one);
		distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		point = ray.GetPoint(distance);
		vector.z = point.x / Grid.WidthInMeters - vector.x;
		vector.w = point.y / Grid.HeightInMeters - vector.y;
		this.material.SetVector("_UVOffsetScale", vector);
	}

	// Token: 0x04005C9E RID: 23710
	[SerializeField]
	private Shader shader;

	// Token: 0x04005C9F RID: 23711
	private Material material;

	// Token: 0x04005CA0 RID: 23712
	private Camera myCamera;
}
