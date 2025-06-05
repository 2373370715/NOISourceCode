using System;
using UnityEngine;

// Token: 0x02000599 RID: 1433
public class SidescreenViewObjectButton : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x060018BE RID: 6334 RVA: 0x001AC334 File Offset: 0x001AA534
	public bool IsValid()
	{
		SidescreenViewObjectButton.Mode trackMode = this.TrackMode;
		if (trackMode != SidescreenViewObjectButton.Mode.Target)
		{
			return trackMode == SidescreenViewObjectButton.Mode.Cell && Grid.IsValidCell(this.TargetCell);
		}
		return this.Target != null;
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x060018BF RID: 6335 RVA: 0x000B4C7E File Offset: 0x000B2E7E
	public string SidescreenButtonText
	{
		get
		{
			return this.Text;
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060018C0 RID: 6336 RVA: 0x000B4C86 File Offset: 0x000B2E86
	public string SidescreenButtonTooltip
	{
		get
		{
			return this.Tooltip;
		}
	}

	// Token: 0x060018C1 RID: 6337 RVA: 0x000AFECA File Offset: 0x000AE0CA
	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060018C2 RID: 6338 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x060018C3 RID: 6339 RVA: 0x000B4C8E File Offset: 0x000B2E8E
	public bool SidescreenButtonInteractable()
	{
		return this.IsValid();
	}

	// Token: 0x060018C4 RID: 6340 RVA: 0x000B4C96 File Offset: 0x000B2E96
	public int HorizontalGroupID()
	{
		return this.horizontalGroupID;
	}

	// Token: 0x060018C5 RID: 6341 RVA: 0x001AC36C File Offset: 0x001AA56C
	public void OnSidescreenButtonPressed()
	{
		if (this.IsValid())
		{
			SidescreenViewObjectButton.Mode trackMode = this.TrackMode;
			if (trackMode == SidescreenViewObjectButton.Mode.Target)
			{
				GameUtil.FocusCamera(this.Target.transform.GetPosition(), 2f, true, true);
				return;
			}
			if (trackMode == SidescreenViewObjectButton.Mode.Cell)
			{
				GameUtil.FocusCamera(Grid.CellToPos(this.TargetCell), 2f, true, true);
				return;
			}
		}
		else
		{
			base.gameObject.Trigger(1980521255, null);
		}
	}

	// Token: 0x060018C6 RID: 6342 RVA: 0x000AFED1 File Offset: 0x000AE0D1
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x0400102A RID: 4138
	public string Text;

	// Token: 0x0400102B RID: 4139
	public string Tooltip;

	// Token: 0x0400102C RID: 4140
	public SidescreenViewObjectButton.Mode TrackMode;

	// Token: 0x0400102D RID: 4141
	public GameObject Target;

	// Token: 0x0400102E RID: 4142
	public int TargetCell;

	// Token: 0x0400102F RID: 4143
	public int horizontalGroupID = -1;

	// Token: 0x0200059A RID: 1434
	public enum Mode
	{
		// Token: 0x04001031 RID: 4145
		Target,
		// Token: 0x04001032 RID: 4146
		Cell
	}
}
