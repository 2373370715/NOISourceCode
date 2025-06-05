using System;

// Token: 0x020019B9 RID: 6585
public class ClusterMapIconFixRotation : KMonoBehaviour
{
	// Token: 0x06008955 RID: 35157 RVA: 0x00366480 File Offset: 0x00364680
	private void Update()
	{
		if (base.transform.parent != null)
		{
			float z = base.transform.parent.rotation.eulerAngles.z;
			this.rotation = -z;
			this.animController.Rotation = this.rotation;
		}
	}

	// Token: 0x040067E0 RID: 26592
	[MyCmpGet]
	private KBatchedAnimController animController;

	// Token: 0x040067E1 RID: 26593
	private float rotation;
}
