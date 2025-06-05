using System;
using UnityEngine;

// Token: 0x0200208C RID: 8332
public class SwapUIAnimationController : MonoBehaviour
{
	// Token: 0x0600B189 RID: 45449 RVA: 0x0043968C File Offset: 0x0043788C
	public void SetState(bool Primary)
	{
		this.AnimationControllerObject_Primary.SetActive(Primary);
		if (!Primary)
		{
			this.AnimationControllerObject_Alternate.GetComponent<KAnimControllerBase>().TintColour = new Color(1f, 1f, 1f, 0.5f);
			this.AnimationControllerObject_Primary.GetComponent<KAnimControllerBase>().TintColour = Color.clear;
		}
		this.AnimationControllerObject_Alternate.SetActive(!Primary);
		if (Primary)
		{
			this.AnimationControllerObject_Primary.GetComponent<KAnimControllerBase>().TintColour = Color.white;
			this.AnimationControllerObject_Alternate.GetComponent<KAnimControllerBase>().TintColour = Color.clear;
		}
	}

	// Token: 0x04008C05 RID: 35845
	public GameObject AnimationControllerObject_Primary;

	// Token: 0x04008C06 RID: 35846
	public GameObject AnimationControllerObject_Alternate;
}
