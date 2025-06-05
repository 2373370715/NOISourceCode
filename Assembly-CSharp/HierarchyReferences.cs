using System;
using UnityEngine;

// Token: 0x02001D50 RID: 7504
[AddComponentMenu("KMonoBehaviour/scripts/HierarchyReferences")]
public class HierarchyReferences : KMonoBehaviour
{
	// Token: 0x06009CB3 RID: 40115 RVA: 0x003D3340 File Offset: 0x003D1540
	public bool HasReference(string name)
	{
		ElementReference[] array = this.references;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].Name == name)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06009CB4 RID: 40116 RVA: 0x003D337C File Offset: 0x003D157C
	public SpecifiedType GetReference<SpecifiedType>(string name) where SpecifiedType : Component
	{
		foreach (ElementReference elementReference in this.references)
		{
			if (elementReference.Name == name)
			{
				if (elementReference.behaviour is SpecifiedType)
				{
					return (SpecifiedType)((object)elementReference.behaviour);
				}
				global::Debug.LogError(string.Format("Behavior is not specified type", Array.Empty<object>()));
			}
		}
		global::Debug.LogError(string.Format("Could not find UI reference '{0}' or convert to specified type)", name));
		return default(SpecifiedType);
	}

	// Token: 0x06009CB5 RID: 40117 RVA: 0x003D33FC File Offset: 0x003D15FC
	public Component GetReference(string name)
	{
		foreach (ElementReference elementReference in this.references)
		{
			if (elementReference.Name == name)
			{
				return elementReference.behaviour;
			}
		}
		global::Debug.LogWarning("Couldn't find reference to object named " + name + " Make sure the name matches the field in the inspector.");
		return null;
	}

	// Token: 0x04007AC6 RID: 31430
	public ElementReference[] references;
}
