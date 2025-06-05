using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001E55 RID: 7765
[SerializationConfig(MemberSerialization.OptIn)]
public class MessageTarget : ISaveLoadable
{
	// Token: 0x0600A294 RID: 41620 RVA: 0x003EDDD0 File Offset: 0x003EBFD0
	public MessageTarget(KPrefabID prefab_id)
	{
		this.prefabId.Set(prefab_id);
		this.position = prefab_id.transform.GetPosition();
		this.name = "Unknown";
		KSelectable component = prefab_id.GetComponent<KSelectable>();
		if (component != null)
		{
			this.name = component.GetName();
		}
		prefab_id.Subscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
	}

	// Token: 0x0600A295 RID: 41621 RVA: 0x0010E16F File Offset: 0x0010C36F
	public Vector3 GetPosition()
	{
		if (this.prefabId.Get() != null)
		{
			return this.prefabId.Get().transform.GetPosition();
		}
		return this.position;
	}

	// Token: 0x0600A296 RID: 41622 RVA: 0x0010E1A0 File Offset: 0x0010C3A0
	public KSelectable GetSelectable()
	{
		if (this.prefabId.Get() != null)
		{
			return this.prefabId.Get().transform.GetComponent<KSelectable>();
		}
		return null;
	}

	// Token: 0x0600A297 RID: 41623 RVA: 0x0010E1CC File Offset: 0x0010C3CC
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x0600A298 RID: 41624 RVA: 0x003EDE4C File Offset: 0x003EC04C
	private void OnAbsorbedBy(object data)
	{
		if (this.prefabId.Get() != null)
		{
			this.prefabId.Get().Unsubscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
		}
		KPrefabID component = ((GameObject)data).GetComponent<KPrefabID>();
		component.Subscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
		this.prefabId.Set(component);
	}

	// Token: 0x0600A299 RID: 41625 RVA: 0x003EDEC0 File Offset: 0x003EC0C0
	public void OnCleanUp()
	{
		if (this.prefabId.Get() != null)
		{
			this.prefabId.Get().Unsubscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
			this.prefabId.Set(null);
		}
	}

	// Token: 0x04007F4B RID: 32587
	[Serialize]
	private Ref<KPrefabID> prefabId = new Ref<KPrefabID>();

	// Token: 0x04007F4C RID: 32588
	[Serialize]
	private Vector3 position;

	// Token: 0x04007F4D RID: 32589
	[Serialize]
	private string name;
}
