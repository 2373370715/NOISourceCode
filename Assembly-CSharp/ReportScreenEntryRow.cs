using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F1E RID: 7966
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenEntryRow")]
public class ReportScreenEntryRow : KMonoBehaviour
{
	// Token: 0x0600A791 RID: 42897 RVA: 0x004055A0 File Offset: 0x004037A0
	private List<ReportManager.ReportEntry.Note> Sort(List<ReportManager.ReportEntry.Note> notes, ReportManager.ReportEntry.Order order)
	{
		if (order == ReportManager.ReportEntry.Order.Ascending)
		{
			notes.Sort((ReportManager.ReportEntry.Note x, ReportManager.ReportEntry.Note y) => x.value.CompareTo(y.value));
		}
		else if (order == ReportManager.ReportEntry.Order.Descending)
		{
			notes.Sort((ReportManager.ReportEntry.Note x, ReportManager.ReportEntry.Note y) => y.value.CompareTo(x.value));
		}
		return notes;
	}

	// Token: 0x0600A792 RID: 42898 RVA: 0x0011134B File Offset: 0x0010F54B
	public static void DestroyStatics()
	{
		ReportScreenEntryRow.notes = null;
	}

	// Token: 0x0600A793 RID: 42899 RVA: 0x00405604 File Offset: 0x00403804
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.added.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnPositiveNoteTooltip);
		this.removed.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnNegativeNoteTooltip);
		this.net.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnNetNoteTooltip);
		this.name.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnNetNoteTooltip);
	}

	// Token: 0x0600A794 RID: 42900 RVA: 0x00405688 File Offset: 0x00403888
	private string OnNoteTooltip(float total_accumulation, string tooltip_text, ReportManager.ReportEntry.Order order, ReportManager.FormattingFn format_fn, Func<ReportManager.ReportEntry.Note, bool> is_note_applicable_cb, ReportManager.GroupFormattingFn group_format_fn = null)
	{
		ReportScreenEntryRow.notes.Clear();
		this.entry.IterateNotes(delegate(ReportManager.ReportEntry.Note note)
		{
			if (is_note_applicable_cb(note))
			{
				ReportScreenEntryRow.notes.Add(note);
			}
		});
		string text = "";
		float num = 0f;
		if (this.entry.contextEntries.Count > 0)
		{
			num = (float)this.entry.contextEntries.Count;
		}
		else
		{
			num = (float)ReportScreenEntryRow.notes.Count;
		}
		num = Mathf.Max(num, 1f);
		foreach (ReportManager.ReportEntry.Note note2 in this.Sort(ReportScreenEntryRow.notes, this.reportGroup.posNoteOrder))
		{
			string arg = format_fn(note2.value);
			if (this.toggle.gameObject.activeInHierarchy && group_format_fn != null)
			{
				arg = group_format_fn(note2.value, num);
			}
			text = string.Format(UI.ENDOFDAYREPORT.NOTES.NOTE_ENTRY_LINE_ITEM, text, note2.note, arg);
		}
		string arg2 = format_fn(total_accumulation);
		if (this.entry.context != null)
		{
			return string.Format(tooltip_text + "\n" + text, arg2, this.entry.context);
		}
		if (group_format_fn != null)
		{
			arg2 = group_format_fn(total_accumulation, num);
			return string.Format(tooltip_text + "\n" + text, arg2, UI.ENDOFDAYREPORT.MY_COLONY);
		}
		return string.Format(tooltip_text + "\n" + text, arg2, UI.ENDOFDAYREPORT.MY_COLONY);
	}

	// Token: 0x0600A795 RID: 42901 RVA: 0x00405824 File Offset: 0x00403A24
	private string OnNegativeNoteTooltip()
	{
		return this.OnNoteTooltip(-this.entry.Negative, this.reportGroup.negativeTooltip, this.reportGroup.negNoteOrder, this.reportGroup.formatfn, (ReportManager.ReportEntry.Note note) => this.IsNegativeNote(note), this.reportGroup.groupFormatfn);
	}

	// Token: 0x0600A796 RID: 42902 RVA: 0x0040587C File Offset: 0x00403A7C
	private string OnPositiveNoteTooltip()
	{
		return this.OnNoteTooltip(this.entry.Positive, this.reportGroup.positiveTooltip, this.reportGroup.posNoteOrder, this.reportGroup.formatfn, (ReportManager.ReportEntry.Note note) => this.IsPositiveNote(note), this.reportGroup.groupFormatfn);
	}

	// Token: 0x0600A797 RID: 42903 RVA: 0x00111353 File Offset: 0x0010F553
	private string OnNetNoteTooltip()
	{
		if (this.entry.Net > 0f)
		{
			return this.OnPositiveNoteTooltip();
		}
		return this.OnNegativeNoteTooltip();
	}

	// Token: 0x0600A798 RID: 42904 RVA: 0x00111374 File Offset: 0x0010F574
	private bool IsPositiveNote(ReportManager.ReportEntry.Note note)
	{
		return note.value > 0f;
	}

	// Token: 0x0600A799 RID: 42905 RVA: 0x00111386 File Offset: 0x0010F586
	private bool IsNegativeNote(ReportManager.ReportEntry.Note note)
	{
		return note.value < 0f;
	}

	// Token: 0x0600A79A RID: 42906 RVA: 0x004058D4 File Offset: 0x00403AD4
	public void SetLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup)
	{
		this.entry = entry;
		this.reportGroup = reportGroup;
		ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.PooledList pos_notes = ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.Allocate();
		entry.IterateNotes(delegate(ReportManager.ReportEntry.Note note)
		{
			if (this.IsPositiveNote(note))
			{
				pos_notes.Add(note);
			}
		});
		ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.PooledList neg_notes = ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.Allocate();
		entry.IterateNotes(delegate(ReportManager.ReportEntry.Note note)
		{
			if (this.IsNegativeNote(note))
			{
				neg_notes.Add(note);
			}
		});
		LayoutElement component = this.name.GetComponent<LayoutElement>();
		if (entry.context == null)
		{
			component.minWidth = (component.preferredWidth = this.nameWidth);
			if (entry.HasContextEntries())
			{
				this.toggle.gameObject.SetActive(true);
				this.spacer.minWidth = this.groupSpacerWidth;
			}
			else
			{
				this.toggle.gameObject.SetActive(false);
				this.spacer.minWidth = this.groupSpacerWidth + this.toggle.GetComponent<LayoutElement>().minWidth;
			}
			this.name.text = reportGroup.stringKey;
		}
		else
		{
			this.toggle.gameObject.SetActive(false);
			this.spacer.minWidth = this.contextSpacerWidth;
			this.name.text = entry.context;
			component.minWidth = (component.preferredWidth = this.nameWidth - this.indentWidth);
			if (base.transform.GetSiblingIndex() % 2 != 0)
			{
				this.bgImage.color = this.oddRowColor;
			}
		}
		if (this.addedValue != entry.Positive)
		{
			string text = reportGroup.formatfn(entry.Positive);
			if (reportGroup.groupFormatfn != null && entry.context == null)
			{
				float num;
				if (entry.contextEntries.Count > 0)
				{
					num = (float)entry.contextEntries.Count;
				}
				else
				{
					num = (float)pos_notes.Count;
				}
				num = Mathf.Max(num, 1f);
				text = reportGroup.groupFormatfn(entry.Positive, num);
			}
			this.added.text = text;
			this.addedValue = entry.Positive;
		}
		if (this.removedValue != entry.Negative)
		{
			string text2 = reportGroup.formatfn(-entry.Negative);
			if (reportGroup.groupFormatfn != null && entry.context == null)
			{
				float num2;
				if (entry.contextEntries.Count > 0)
				{
					num2 = (float)entry.contextEntries.Count;
				}
				else
				{
					num2 = (float)neg_notes.Count;
				}
				num2 = Mathf.Max(num2, 1f);
				text2 = reportGroup.groupFormatfn(-entry.Negative, num2);
			}
			this.removed.text = text2;
			this.removedValue = entry.Negative;
		}
		if (this.netValue != entry.Net)
		{
			string text3 = (reportGroup.formatfn == null) ? entry.Net.ToString() : reportGroup.formatfn(entry.Net);
			if (reportGroup.groupFormatfn != null && entry.context == null)
			{
				float num3;
				if (entry.contextEntries.Count > 0)
				{
					num3 = (float)entry.contextEntries.Count;
				}
				else
				{
					num3 = (float)(pos_notes.Count + neg_notes.Count);
				}
				num3 = Mathf.Max(num3, 1f);
				text3 = reportGroup.groupFormatfn(entry.Net, num3);
			}
			this.net.text = text3;
			this.netValue = entry.Net;
		}
		pos_notes.Recycle();
		neg_notes.Recycle();
	}

	// Token: 0x04008381 RID: 33665
	[SerializeField]
	public new LocText name;

	// Token: 0x04008382 RID: 33666
	[SerializeField]
	public LocText added;

	// Token: 0x04008383 RID: 33667
	[SerializeField]
	public LocText removed;

	// Token: 0x04008384 RID: 33668
	[SerializeField]
	public LocText net;

	// Token: 0x04008385 RID: 33669
	private float addedValue = float.NegativeInfinity;

	// Token: 0x04008386 RID: 33670
	private float removedValue = float.NegativeInfinity;

	// Token: 0x04008387 RID: 33671
	private float netValue = float.NegativeInfinity;

	// Token: 0x04008388 RID: 33672
	[SerializeField]
	public MultiToggle toggle;

	// Token: 0x04008389 RID: 33673
	[SerializeField]
	private LayoutElement spacer;

	// Token: 0x0400838A RID: 33674
	[SerializeField]
	private Image bgImage;

	// Token: 0x0400838B RID: 33675
	public float groupSpacerWidth;

	// Token: 0x0400838C RID: 33676
	public float contextSpacerWidth;

	// Token: 0x0400838D RID: 33677
	private float nameWidth = 164f;

	// Token: 0x0400838E RID: 33678
	private float indentWidth = 6f;

	// Token: 0x0400838F RID: 33679
	[SerializeField]
	private Color oddRowColor;

	// Token: 0x04008390 RID: 33680
	private static List<ReportManager.ReportEntry.Note> notes = new List<ReportManager.ReportEntry.Note>();

	// Token: 0x04008391 RID: 33681
	private ReportManager.ReportEntry entry;

	// Token: 0x04008392 RID: 33682
	private ReportManager.ReportGroup reportGroup;
}
