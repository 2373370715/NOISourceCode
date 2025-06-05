using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000AC9 RID: 2761
[SerializationConfig(MemberSerialization.OptIn)]
public class MinionStorageDataHolder : KMonoBehaviour, StoredMinionIdentity.IStoredMinionExtension
{
	// Token: 0x06003288 RID: 12936 RVA: 0x000C474E File Offset: 0x000C294E
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06003289 RID: 12937 RVA: 0x0021154C File Offset: 0x0020F74C
	public MinionStorageDataHolder.DataPack Internal_GetDataPack(string ID)
	{
		if (this.storedDataPacks != null)
		{
			MinionStorageDataHolder.DataPack dataPack = this.storedDataPacks.Find((MinionStorageDataHolder.DataPack d) => d.ID == ID);
			if (dataPack != null)
			{
				return dataPack;
			}
		}
		return null;
	}

	// Token: 0x0600328A RID: 12938 RVA: 0x000C541D File Offset: 0x000C361D
	public void Internal_UpdateData(string ID, MinionStorageDataHolder.DataPackData data)
	{
		this.SetData(ID, data, false);
	}

	// Token: 0x0600328B RID: 12939 RVA: 0x0021158C File Offset: 0x0020F78C
	private void SetData(string ID, MinionStorageDataHolder.DataPackData data, bool markAsNewDataToRead)
	{
		if (this.storedDataPacks == null)
		{
			this.storedDataPacks = new List<MinionStorageDataHolder.DataPack>();
		}
		MinionStorageDataHolder.DataPack dataPack = this.storedDataPacks.Find((MinionStorageDataHolder.DataPack d) => d.ID == ID);
		if (dataPack == null)
		{
			dataPack = new MinionStorageDataHolder.DataPack(ID);
			this.storedDataPacks.Add(dataPack);
		}
		dataPack.SetData(data, markAsNewDataToRead);
	}

	// Token: 0x0600328C RID: 12940 RVA: 0x002115F4 File Offset: 0x0020F7F4
	public void PullFrom(StoredMinionIdentity source)
	{
		MinionStorageDataHolder component = source.GetComponent<MinionStorageDataHolder>();
		if (component != null && component.storedDataPacks != null)
		{
			for (int i = 0; i < component.storedDataPacks.Count; i++)
			{
				MinionStorageDataHolder.DataPack dataPack = component.storedDataPacks[i];
				if (dataPack != null)
				{
					this.SetData(dataPack.ID, dataPack.ReadData(), true);
				}
			}
		}
	}

	// Token: 0x0600328D RID: 12941 RVA: 0x00211654 File Offset: 0x0020F854
	public void PushTo(StoredMinionIdentity destination)
	{
		Action<StoredMinionIdentity> onCopyBegins = this.OnCopyBegins;
		if (onCopyBegins != null)
		{
			onCopyBegins(destination);
		}
		this.AddStoredMinionGameObjectRequirements(destination.gameObject);
		MinionStorageDataHolder component = destination.gameObject.GetComponent<MinionStorageDataHolder>();
		if (this.storedDataPacks != null)
		{
			for (int i = 0; i < this.storedDataPacks.Count; i++)
			{
				MinionStorageDataHolder.DataPack dataPack = this.storedDataPacks[i];
				if (dataPack != null)
				{
					component.SetData(dataPack.ID, dataPack.ReadData(), true);
				}
			}
		}
	}

	// Token: 0x0600328E RID: 12942 RVA: 0x000C5428 File Offset: 0x000C3628
	public void AddStoredMinionGameObjectRequirements(GameObject storedMinionGameObject)
	{
		storedMinionGameObject.AddOrGet<MinionStorageDataHolder>();
	}

	// Token: 0x04002299 RID: 8857
	public Action<StoredMinionIdentity> OnCopyBegins;

	// Token: 0x0400229A RID: 8858
	[Serialize]
	private List<MinionStorageDataHolder.DataPack> storedDataPacks;

	// Token: 0x02000ACA RID: 2762
	[SerializationConfig(MemberSerialization.OptIn)]
	public class DataPackData
	{
		// Token: 0x0400229B RID: 8859
		[Serialize]
		public bool[] Bools;

		// Token: 0x0400229C RID: 8860
		[Serialize]
		public Tag[] Tags;
	}

	// Token: 0x02000ACB RID: 2763
	[SerializationConfig(MemberSerialization.OptIn)]
	public class DataPack
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06003291 RID: 12945 RVA: 0x000C5431 File Offset: 0x000C3631
		public bool IsStoringNewData
		{
			get
			{
				return this.isStoringNewData;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06003292 RID: 12946 RVA: 0x000C5439 File Offset: 0x000C3639
		public string ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x000C5441 File Offset: 0x000C3641
		public DataPack(string id)
		{
			this.id = id;
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x000C5450 File Offset: 0x000C3650
		public void SetData(MinionStorageDataHolder.DataPackData data, bool markAsNewDataToRead)
		{
			this.data = data;
			if (markAsNewDataToRead)
			{
				this.isStoringNewData = markAsNewDataToRead;
			}
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x000C5463 File Offset: 0x000C3663
		public MinionStorageDataHolder.DataPackData ReadData()
		{
			this.isStoringNewData = false;
			return this.data;
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x000C5472 File Offset: 0x000C3672
		public MinionStorageDataHolder.DataPackData PeekData()
		{
			return this.data;
		}

		// Token: 0x0400229D RID: 8861
		[Serialize]
		private string id;

		// Token: 0x0400229E RID: 8862
		[Serialize]
		private bool isStoringNewData;

		// Token: 0x0400229F RID: 8863
		[Serialize]
		private MinionStorageDataHolder.DataPackData data;
	}
}
