using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x0200175D RID: 5981
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/PreventFOWRevealTracker")]
public class PreventFOWRevealTracker : KMonoBehaviour
{
	// Token: 0x06007B04 RID: 31492 RVA: 0x0032809C File Offset: 0x0032629C
	[OnSerializing]
	private void OnSerialize()
	{
		this.preventFOWRevealCells.Clear();
		for (int i = 0; i < Grid.VisMasks.Length; i++)
		{
			if (Grid.PreventFogOfWarReveal[i])
			{
				this.preventFOWRevealCells.Add(i);
			}
		}
	}

	// Token: 0x06007B05 RID: 31493 RVA: 0x003280E0 File Offset: 0x003262E0
	[OnDeserialized]
	private void OnDeserialized()
	{
		foreach (int i in this.preventFOWRevealCells)
		{
			Grid.PreventFogOfWarReveal[i] = true;
		}
	}

	// Token: 0x04005CA1 RID: 23713
	[Serialize]
	public List<int> preventFOWRevealCells;
}
