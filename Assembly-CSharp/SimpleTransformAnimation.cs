using System;
using UnityEngine;

// Token: 0x02002063 RID: 8291
public class SimpleTransformAnimation : MonoBehaviour
{
	// Token: 0x0600B049 RID: 45129 RVA: 0x000AA038 File Offset: 0x000A8238
	private void Start()
	{
	}

	// Token: 0x0600B04A RID: 45130 RVA: 0x0011738A File Offset: 0x0011558A
	private void Update()
	{
		base.transform.Rotate(this.rotationSpeed * Time.unscaledDeltaTime);
		base.transform.Translate(this.translateSpeed * Time.unscaledDeltaTime);
	}

	// Token: 0x04008AA5 RID: 35493
	[SerializeField]
	private Vector3 rotationSpeed;

	// Token: 0x04008AA6 RID: 35494
	[SerializeField]
	private Vector3 translateSpeed;
}
