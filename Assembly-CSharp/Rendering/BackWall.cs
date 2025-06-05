using System;
using UnityEngine;

namespace rendering
{
	// Token: 0x02002145 RID: 8517
	public class BackWall : MonoBehaviour
	{
		// Token: 0x0600B571 RID: 46449 RVA: 0x0011A572 File Offset: 0x00118772
		private void Awake()
		{
			this.backwallMaterial.SetTexture("images", this.array);
		}

		// Token: 0x04008FB2 RID: 36786
		[SerializeField]
		public Material backwallMaterial;

		// Token: 0x04008FB3 RID: 36787
		[SerializeField]
		public Texture2DArray array;
	}
}
