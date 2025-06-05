using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001DA5 RID: 7589
public static class KleiItemsStatusRefresher
{
	// Token: 0x06009E89 RID: 40585 RVA: 0x0010B9F8 File Offset: 0x00109BF8
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void Initialize()
	{
		KleiItems.AddInventoryRefreshCallback(new KleiItems.InventoryRefreshCallback(KleiItemsStatusRefresher.OnRefreshResponseFromServer));
	}

	// Token: 0x06009E8A RID: 40586 RVA: 0x003DCABC File Offset: 0x003DACBC
	private static void OnRefreshResponseFromServer()
	{
		foreach (KleiItemsStatusRefresher.UIListener uilistener in KleiItemsStatusRefresher.listeners)
		{
			uilistener.Internal_RefreshUI();
		}
	}

	// Token: 0x06009E8B RID: 40587 RVA: 0x003DCABC File Offset: 0x003DACBC
	public static void Refresh()
	{
		foreach (KleiItemsStatusRefresher.UIListener uilistener in KleiItemsStatusRefresher.listeners)
		{
			uilistener.Internal_RefreshUI();
		}
	}

	// Token: 0x06009E8C RID: 40588 RVA: 0x0010BA0B File Offset: 0x00109C0B
	public static KleiItemsStatusRefresher.UIListener AddOrGetListener(Component component)
	{
		return KleiItemsStatusRefresher.AddOrGetListener(component.gameObject);
	}

	// Token: 0x06009E8D RID: 40589 RVA: 0x0010BA18 File Offset: 0x00109C18
	public static KleiItemsStatusRefresher.UIListener AddOrGetListener(GameObject onGameObject)
	{
		return onGameObject.AddOrGet<KleiItemsStatusRefresher.UIListener>();
	}

	// Token: 0x04007C93 RID: 31891
	public static HashSet<KleiItemsStatusRefresher.UIListener> listeners = new HashSet<KleiItemsStatusRefresher.UIListener>();

	// Token: 0x02001DA6 RID: 7590
	public class UIListener : MonoBehaviour
	{
		// Token: 0x06009E8F RID: 40591 RVA: 0x0010BA2C File Offset: 0x00109C2C
		public void Internal_RefreshUI()
		{
			if (this.refreshUIFn != null)
			{
				this.refreshUIFn();
			}
		}

		// Token: 0x06009E90 RID: 40592 RVA: 0x0010BA41 File Offset: 0x00109C41
		public void OnRefreshUI(System.Action fn)
		{
			this.refreshUIFn = fn;
		}

		// Token: 0x06009E91 RID: 40593 RVA: 0x0010BA4A File Offset: 0x00109C4A
		private void OnEnable()
		{
			KleiItemsStatusRefresher.listeners.Add(this);
		}

		// Token: 0x06009E92 RID: 40594 RVA: 0x0010BA58 File Offset: 0x00109C58
		private void OnDisable()
		{
			KleiItemsStatusRefresher.listeners.Remove(this);
		}

		// Token: 0x04007C94 RID: 31892
		private System.Action refreshUIFn;
	}
}
