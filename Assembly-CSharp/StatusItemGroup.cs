using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B4C RID: 2892
public class StatusItemGroup
{
	// Token: 0x060035C1 RID: 13761 RVA: 0x000C77EA File Offset: 0x000C59EA
	public IEnumerator<StatusItemGroup.Entry> GetEnumerator()
	{
		return this.items.GetEnumerator();
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x060035C2 RID: 13762 RVA: 0x000C77FC File Offset: 0x000C59FC
	// (set) Token: 0x060035C3 RID: 13763 RVA: 0x000C7804 File Offset: 0x000C5A04
	public GameObject gameObject { get; private set; }

	// Token: 0x060035C4 RID: 13764 RVA: 0x000C780D File Offset: 0x000C5A0D
	public StatusItemGroup(GameObject go)
	{
		this.gameObject = go;
	}

	// Token: 0x060035C5 RID: 13765 RVA: 0x000C7841 File Offset: 0x000C5A41
	public void SetOffset(Vector3 offset)
	{
		this.offset = offset;
		Game.Instance.SetStatusItemOffset(this.gameObject.transform, offset);
	}

	// Token: 0x060035C6 RID: 13766 RVA: 0x0021D1F4 File Offset: 0x0021B3F4
	public StatusItemGroup.Entry GetStatusItem(StatusItemCategory category)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].category == category)
			{
				return this.items[i];
			}
		}
		return StatusItemGroup.Entry.EmptyEntry;
	}

	// Token: 0x060035C7 RID: 13767 RVA: 0x0021D240 File Offset: 0x0021B440
	public Guid SetStatusItem(StatusItemCategory category, StatusItem item, object data = null)
	{
		if (item != null && item.allowMultiples)
		{
			throw new ArgumentException(item.Name + " allows multiple instances of itself to be active so you must access it via its handle");
		}
		if (category == null)
		{
			throw new ArgumentException("SetStatusItem requires a category.");
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].category == category)
			{
				if (this.items[i].item == item)
				{
					this.Log("Set (exists in category)", item, this.items[i].id, category);
					return this.items[i].id;
				}
				this.Log("Set->Remove existing in category", item, this.items[i].id, category);
				this.RemoveStatusItem(this.items[i].id, false);
			}
		}
		if (item != null)
		{
			Guid guid = this.AddStatusItem(item, data, category);
			this.Log("Set (new)", item, guid, category);
			return guid;
		}
		this.Log("Set (failed)", item, Guid.Empty, category);
		return Guid.Empty;
	}

	// Token: 0x060035C8 RID: 13768 RVA: 0x000C7860 File Offset: 0x000C5A60
	public void SetStatusItem(Guid guid, StatusItemCategory category, StatusItem new_item, object data = null)
	{
		this.RemoveStatusItem(guid, false);
		if (new_item != null)
		{
			this.AddStatusItem(new_item, data, category);
		}
	}

	// Token: 0x060035C9 RID: 13769 RVA: 0x0021D35C File Offset: 0x0021B55C
	public bool HasStatusItem(StatusItem status_item)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].item.Id == status_item.Id)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060035CA RID: 13770 RVA: 0x0021D3A8 File Offset: 0x0021B5A8
	public bool HasStatusItemID(string status_item_id)
	{
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].item.Id == status_item_id)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060035CB RID: 13771 RVA: 0x0021D3EC File Offset: 0x0021B5EC
	public Guid AddStatusItem(StatusItem item, object data = null, StatusItemCategory category = null)
	{
		if (this.gameObject == null || (!item.allowMultiples && this.HasStatusItem(item)))
		{
			return Guid.Empty;
		}
		if (!item.allowMultiples)
		{
			using (List<StatusItemGroup.Entry>.Enumerator enumerator = this.items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.item.Id == item.Id)
					{
						throw new ArgumentException("Tried to add " + item.Id + " multiples times which is not permitted.");
					}
				}
			}
		}
		StatusItemGroup.Entry entry = new StatusItemGroup.Entry(item, category, data);
		if (item.shouldNotify)
		{
			entry.notification = new Notification(item.notificationText, item.notificationType, new Func<List<Notification>, object, string>(StatusItemGroup.OnToolTip), item, false, 0f, item.notificationClickCallback, data, null, true, false, false);
			this.gameObject.AddOrGet<Notifier>().Add(entry.notification, "");
		}
		if (item.ShouldShowIcon())
		{
			Game.Instance.AddStatusItem(this.gameObject.transform, item);
			Game.Instance.SetStatusItemOffset(this.gameObject.transform, this.offset);
		}
		this.items.Add(entry);
		if (this.OnAddStatusItem != null)
		{
			this.OnAddStatusItem(entry, category);
		}
		return entry.id;
	}

	// Token: 0x060035CC RID: 13772 RVA: 0x0021D55C File Offset: 0x0021B75C
	public Guid RemoveStatusItem(StatusItem status_item, bool immediate = false)
	{
		if (status_item.allowMultiples)
		{
			throw new ArgumentException(status_item.Name + " allows multiple instances of itself to be active so it must be released via an instance handle");
		}
		int i = 0;
		while (i < this.items.Count)
		{
			if (this.items[i].item.Id == status_item.Id)
			{
				Guid id = this.items[i].id;
				if (id == Guid.Empty)
				{
					return id;
				}
				this.RemoveStatusItemInternal(id, i, immediate);
				return id;
			}
			else
			{
				i++;
			}
		}
		return Guid.Empty;
	}

	// Token: 0x060035CD RID: 13773 RVA: 0x0021D5F4 File Offset: 0x0021B7F4
	public Guid RemoveStatusItem(Guid guid, bool immediate = false)
	{
		if (guid == Guid.Empty)
		{
			return guid;
		}
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].id == guid)
			{
				this.RemoveStatusItemInternal(guid, i, immediate);
				return guid;
			}
		}
		return Guid.Empty;
	}

	// Token: 0x060035CE RID: 13774 RVA: 0x0021D650 File Offset: 0x0021B850
	private void RemoveStatusItemInternal(Guid guid, int itemIdx, bool immediate)
	{
		StatusItemGroup.Entry entry = this.items[itemIdx];
		this.items.RemoveAt(itemIdx);
		if (entry.notification != null)
		{
			this.gameObject.GetComponent<Notifier>().Remove(entry.notification);
		}
		if (entry.item.ShouldShowIcon() && Game.Instance != null)
		{
			Game.Instance.RemoveStatusItem(this.gameObject.transform, entry.item);
		}
		if (this.OnRemoveStatusItem != null)
		{
			this.OnRemoveStatusItem(entry, immediate);
		}
	}

	// Token: 0x060035CF RID: 13775 RVA: 0x000C7879 File Offset: 0x000C5A79
	private static string OnToolTip(List<Notification> notifications, object data)
	{
		return ((StatusItem)data).notificationTooltipText + notifications.ReduceMessages(true);
	}

	// Token: 0x060035D0 RID: 13776 RVA: 0x000C7892 File Offset: 0x000C5A92
	public void Destroy()
	{
		if (Game.IsQuitting())
		{
			return;
		}
		while (this.items.Count > 0)
		{
			this.RemoveStatusItem(this.items[0].id, false);
		}
	}

	// Token: 0x060035D1 RID: 13777 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_LOGGER")]
	private void Log(string action, StatusItem item, Guid guid)
	{
	}

	// Token: 0x060035D2 RID: 13778 RVA: 0x000AA038 File Offset: 0x000A8238
	private void Log(string action, StatusItem item, Guid guid, StatusItemCategory category)
	{
	}

	// Token: 0x0400252E RID: 9518
	private List<StatusItemGroup.Entry> items = new List<StatusItemGroup.Entry>();

	// Token: 0x0400252F RID: 9519
	public Action<StatusItemGroup.Entry, StatusItemCategory> OnAddStatusItem;

	// Token: 0x04002530 RID: 9520
	public Action<StatusItemGroup.Entry, bool> OnRemoveStatusItem;

	// Token: 0x04002532 RID: 9522
	private Vector3 offset = new Vector3(0f, 0f, 0f);

	// Token: 0x02000B4D RID: 2893
	public struct Entry : IComparable<StatusItemGroup.Entry>, IEquatable<StatusItemGroup.Entry>
	{
		// Token: 0x060035D3 RID: 13779 RVA: 0x000C78C3 File Offset: 0x000C5AC3
		public Entry(StatusItem item, StatusItemCategory category, object data)
		{
			this.id = Guid.NewGuid();
			this.item = item;
			this.data = data;
			this.category = category;
			this.notification = null;
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x000C78EC File Offset: 0x000C5AEC
		public string GetName()
		{
			return this.item.GetName(this.data);
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x000C78FF File Offset: 0x000C5AFF
		public void ShowToolTip(ToolTip tooltip_widget, TextStyleSetting property_style)
		{
			this.item.ShowToolTip(tooltip_widget, this.data, property_style);
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x000C7914 File Offset: 0x000C5B14
		public void SetIcon(Image image)
		{
			this.item.SetIcon(image, this.data);
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x000C7928 File Offset: 0x000C5B28
		public int CompareTo(StatusItemGroup.Entry other)
		{
			return this.id.CompareTo(other.id);
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x000C793B File Offset: 0x000C5B3B
		public bool Equals(StatusItemGroup.Entry other)
		{
			return this.id == other.id;
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x000C794E File Offset: 0x000C5B4E
		public void OnClick()
		{
			this.item.OnClick(this.data);
		}

		// Token: 0x04002533 RID: 9523
		public static StatusItemGroup.Entry EmptyEntry = new StatusItemGroup.Entry
		{
			id = Guid.Empty
		};

		// Token: 0x04002534 RID: 9524
		public Guid id;

		// Token: 0x04002535 RID: 9525
		public StatusItem item;

		// Token: 0x04002536 RID: 9526
		public object data;

		// Token: 0x04002537 RID: 9527
		public Notification notification;

		// Token: 0x04002538 RID: 9528
		public StatusItemCategory category;
	}
}
