using System;

// Token: 0x02000ACE RID: 2766
public static class MinionStorageDataHolder_StaticHelpers
{
	// Token: 0x0600329B RID: 12955 RVA: 0x000C54A0 File Offset: 0x000C36A0
	public static void UpdateData<T>(this MinionStorageDataHolder dataHolderComponent, MinionStorageDataHolder.DataPackData data)
	{
		dataHolderComponent.Internal_UpdateData(typeof(T).ToString(), data);
	}

	// Token: 0x0600329C RID: 12956 RVA: 0x000C54B8 File Offset: 0x000C36B8
	public static MinionStorageDataHolder.DataPack GetDataPack<T>(this MinionStorageDataHolder dataHolderComponent)
	{
		return dataHolderComponent.Internal_GetDataPack(typeof(T).ToString());
	}
}
