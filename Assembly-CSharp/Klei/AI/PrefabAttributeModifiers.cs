using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CE2 RID: 15586
	[AddComponentMenu("KMonoBehaviour/scripts/PrefabAttributeModifiers")]
	public class PrefabAttributeModifiers : KMonoBehaviour
	{
		// Token: 0x0600EF46 RID: 61254 RVA: 0x000B74E6 File Offset: 0x000B56E6
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
		}

		// Token: 0x0600EF47 RID: 61255 RVA: 0x001450D2 File Offset: 0x001432D2
		public void AddAttributeDescriptor(AttributeModifier modifier)
		{
			this.descriptors.Add(modifier);
		}

		// Token: 0x0600EF48 RID: 61256 RVA: 0x001450E0 File Offset: 0x001432E0
		public void RemovePrefabAttribute(AttributeModifier modifier)
		{
			this.descriptors.Remove(modifier);
		}

		// Token: 0x0400EAD9 RID: 60121
		public List<AttributeModifier> descriptors = new List<AttributeModifier>();
	}
}
