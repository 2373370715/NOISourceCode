using System;
using UnityEngine;

// Token: 0x02000959 RID: 2393
public class NativeAnimBatchLoader : MonoBehaviour
{
	// Token: 0x06002AB6 RID: 10934 RVA: 0x001E8524 File Offset: 0x001E6724
	private void Start()
	{
		if (this.generateObjects)
		{
			for (int i = 0; i < this.enableObjects.Length; i++)
			{
				if (this.enableObjects[i] != null)
				{
					this.enableObjects[i].GetComponent<KBatchedAnimController>().visibilityType = KAnimControllerBase.VisibilityType.Always;
					this.enableObjects[i].SetActive(true);
				}
			}
		}
		if (this.setTimeScale)
		{
			Time.timeScale = 1f;
		}
		if (this.destroySelf)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06002AB7 RID: 10935 RVA: 0x001E85A0 File Offset: 0x001E67A0
	private void LateUpdate()
	{
		if (this.destroySelf)
		{
			return;
		}
		if (this.performUpdate)
		{
			KAnimBatchManager.Instance().UpdateActiveArea(new Vector2I(0, 0), new Vector2I(9999, 9999));
			KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
		}
		if (this.performRender)
		{
			KAnimBatchManager.Instance().Render();
		}
	}

	// Token: 0x04001CF9 RID: 7417
	public bool performTimeUpdate;

	// Token: 0x04001CFA RID: 7418
	public bool performUpdate;

	// Token: 0x04001CFB RID: 7419
	public bool performRender;

	// Token: 0x04001CFC RID: 7420
	public bool setTimeScale;

	// Token: 0x04001CFD RID: 7421
	public bool destroySelf;

	// Token: 0x04001CFE RID: 7422
	public bool generateObjects;

	// Token: 0x04001CFF RID: 7423
	public GameObject[] enableObjects;
}
