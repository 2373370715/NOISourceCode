using System;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x02001807 RID: 6151
[SerializationConfig(MemberSerialization.OptIn)]
public class ResourceRef<ResourceType> : ISaveLoadable where ResourceType : Resource
{
	// Token: 0x06007E97 RID: 32407 RVA: 0x000F7D12 File Offset: 0x000F5F12
	public ResourceRef(ResourceType resource)
	{
		this.Set(resource);
	}

	// Token: 0x06007E98 RID: 32408 RVA: 0x000AA024 File Offset: 0x000A8224
	public ResourceRef()
	{
	}

	// Token: 0x17000806 RID: 2054
	// (get) Token: 0x06007E99 RID: 32409 RVA: 0x000F7D21 File Offset: 0x000F5F21
	public ResourceGuid Guid
	{
		get
		{
			return this.guid;
		}
	}

	// Token: 0x06007E9A RID: 32410 RVA: 0x000F7D29 File Offset: 0x000F5F29
	public ResourceType Get()
	{
		return this.resource;
	}

	// Token: 0x06007E9B RID: 32411 RVA: 0x000F7D31 File Offset: 0x000F5F31
	public void Set(ResourceType resource)
	{
		this.guid = null;
		this.resource = resource;
	}

	// Token: 0x06007E9C RID: 32412 RVA: 0x000F7D41 File Offset: 0x000F5F41
	[OnSerializing]
	private void OnSerializing()
	{
		if (this.resource == null)
		{
			this.guid = null;
			return;
		}
		this.guid = this.resource.Guid;
	}

	// Token: 0x06007E9D RID: 32413 RVA: 0x000F7D6E File Offset: 0x000F5F6E
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.guid != null)
		{
			this.resource = Db.Get().GetResource<ResourceType>(this.guid);
			if (this.resource != null)
			{
				this.guid = null;
			}
		}
	}

	// Token: 0x04006029 RID: 24617
	[Serialize]
	private ResourceGuid guid;

	// Token: 0x0400602A RID: 24618
	private ResourceType resource;
}
