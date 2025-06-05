using System;
using UnityEngine;

// Token: 0x0200175B RID: 5979
public class ClusterCoverPostFX : MonoBehaviour
{
	// Token: 0x06007AFC RID: 31484 RVA: 0x000F57B9 File Offset: 0x000F39B9
	private void Awake()
	{
		if (this.shader != null)
		{
			this.material = new Material(this.shader);
		}
	}

	// Token: 0x06007AFD RID: 31485 RVA: 0x000F57DA File Offset: 0x000F39DA
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.SetupUVs();
		Graphics.Blit(source, destination, this.material, 0);
	}

	// Token: 0x06007AFE RID: 31486 RVA: 0x00327DC0 File Offset: 0x00325FC0
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
		ray = this.myCamera.ViewportPointToRay(Vector3.one);
		distance = Mathf.Abs(ray.origin.z / ray.direction.z);
		Vector3 point2 = ray.GetPoint(distance);
		Vector4 value;
		value.x = point.x;
		value.y = point.y;
		value.z = point2.x - point.x;
		value.w = point2.y - point.y;
		this.material.SetVector("_CameraCoords", value);
		Vector4 value2;
		if (ClusterManager.Instance != null && !CameraController.Instance.ignoreClusterFX)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			Vector2I worldOffset = activeWorld.WorldOffset;
			Vector2I worldSize = activeWorld.WorldSize;
			value2 = new Vector4((float)worldOffset.x, (float)worldOffset.y, (float)worldSize.x, (float)worldSize.y);
			this.material.SetFloat("_HideSurface", ClusterManager.Instance.activeWorld.FullyEnclosedBorder ? 1f : 0f);
		}
		else
		{
			value2 = new Vector4(0f, 0f, (float)Grid.WidthInCells, (float)Grid.HeightInCells);
			this.material.SetFloat("_HideSurface", 0f);
		}
		this.material.SetVector("_WorldCoords", value2);
	}

	// Token: 0x04005C9B RID: 23707
	[SerializeField]
	private Shader shader;

	// Token: 0x04005C9C RID: 23708
	private Material material;

	// Token: 0x04005C9D RID: 23709
	private Camera myCamera;
}
