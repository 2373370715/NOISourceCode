using System;
using System.Collections;
using UnityEngine;

// Token: 0x020017DB RID: 6107
public class MultipleRenderTarget : MonoBehaviour
{
	// Token: 0x14000022 RID: 34
	// (add) Token: 0x06007D80 RID: 32128 RVA: 0x003327D0 File Offset: 0x003309D0
	// (remove) Token: 0x06007D81 RID: 32129 RVA: 0x00332808 File Offset: 0x00330A08
	public event Action<Camera> onSetupComplete;

	// Token: 0x06007D82 RID: 32130 RVA: 0x000F7290 File Offset: 0x000F5490
	private void Start()
	{
		base.StartCoroutine(this.SetupProxy());
	}

	// Token: 0x06007D83 RID: 32131 RVA: 0x000F729F File Offset: 0x000F549F
	private IEnumerator SetupProxy()
	{
		yield return null;
		Camera component = base.GetComponent<Camera>();
		Camera camera = new GameObject().AddComponent<Camera>();
		camera.CopyFrom(component);
		this.renderProxy = camera.gameObject.AddComponent<MultipleRenderTargetProxy>();
		camera.name = component.name + " MRT";
		camera.transform.parent = component.transform;
		camera.transform.SetLocalPosition(Vector3.zero);
		camera.depth = component.depth - 1f;
		component.cullingMask = 0;
		component.clearFlags = CameraClearFlags.Color;
		this.quad = new FullScreenQuad("MultipleRenderTarget", component, true);
		if (this.onSetupComplete != null)
		{
			this.onSetupComplete(camera);
		}
		yield break;
	}

	// Token: 0x06007D84 RID: 32132 RVA: 0x000F72AE File Offset: 0x000F54AE
	private void OnPreCull()
	{
		if (this.renderProxy != null)
		{
			this.quad.Draw(this.renderProxy.Textures[0]);
		}
	}

	// Token: 0x06007D85 RID: 32133 RVA: 0x000F72D6 File Offset: 0x000F54D6
	public void ToggleColouredOverlayView(bool enabled)
	{
		if (this.renderProxy != null)
		{
			this.renderProxy.ToggleColouredOverlayView(enabled);
		}
	}

	// Token: 0x04005F48 RID: 24392
	private MultipleRenderTargetProxy renderProxy;

	// Token: 0x04005F49 RID: 24393
	private FullScreenQuad quad;

	// Token: 0x04005F4B RID: 24395
	public bool isFrontEnd;
}
