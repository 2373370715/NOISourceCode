using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001A9A RID: 6810
public class WhiteBoard : KGameObjectComponentManager<WhiteBoard.Data>, IKComponentManager
{
	// Token: 0x06008E0C RID: 36364 RVA: 0x00378344 File Offset: 0x00376544
	public HandleVector<int>.Handle Add(GameObject go)
	{
		return base.Add(go, new WhiteBoard.Data
		{
			keyValueStore = new Dictionary<HashedString, object>()
		});
	}

	// Token: 0x06008E0D RID: 36365 RVA: 0x00378370 File Offset: 0x00376570
	protected override void OnCleanUp(HandleVector<int>.Handle h)
	{
		WhiteBoard.Data data = base.GetData(h);
		data.keyValueStore.Clear();
		data.keyValueStore = null;
		base.SetData(h, data);
	}

	// Token: 0x06008E0E RID: 36366 RVA: 0x00101491 File Offset: 0x000FF691
	public bool HasValue(HandleVector<int>.Handle h, HashedString key)
	{
		return h.IsValid() && base.GetData(h).keyValueStore.ContainsKey(key);
	}

	// Token: 0x06008E0F RID: 36367 RVA: 0x001014B0 File Offset: 0x000FF6B0
	public object GetValue(HandleVector<int>.Handle h, HashedString key)
	{
		return base.GetData(h).keyValueStore[key];
	}

	// Token: 0x06008E10 RID: 36368 RVA: 0x003783A0 File Offset: 0x003765A0
	public void SetValue(HandleVector<int>.Handle h, HashedString key, object value)
	{
		if (!h.IsValid())
		{
			return;
		}
		WhiteBoard.Data data = base.GetData(h);
		data.keyValueStore[key] = value;
		base.SetData(h, data);
	}

	// Token: 0x06008E11 RID: 36369 RVA: 0x003783D4 File Offset: 0x003765D4
	public void RemoveValue(HandleVector<int>.Handle h, HashedString key)
	{
		if (!h.IsValid())
		{
			return;
		}
		WhiteBoard.Data data = base.GetData(h);
		data.keyValueStore.Remove(key);
		base.SetData(h, data);
	}

	// Token: 0x02001A9B RID: 6811
	public struct Data
	{
		// Token: 0x04006B3B RID: 27451
		public Dictionary<HashedString, object> keyValueStore;
	}
}
