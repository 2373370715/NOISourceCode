using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001C61 RID: 7265
public class BuildToolRotateButtonUI : MonoBehaviour
{
	// Token: 0x0600970B RID: 38667 RVA: 0x003B16D8 File Offset: 0x003AF8D8
	private void Awake()
	{
		this.tooltip.refreshWhileHovering = true;
		this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
		this.button.onClick += delegate()
		{
			BuildTool.Instance.TryRotate();
		};
		this.UpdateTooltip(false);
	}

	// Token: 0x0600970C RID: 38668 RVA: 0x003B1730 File Offset: 0x003AF930
	private void Update()
	{
		bool flag = BuildTool.Instance.CanRotate();
		this.UpdateTooltip(flag);
		if (this.button.isInteractable != flag)
		{
			this.button.isInteractable = flag;
		}
	}

	// Token: 0x0600970D RID: 38669 RVA: 0x003B176C File Offset: 0x003AF96C
	private void UpdateTooltip(bool can_rotate)
	{
		PermittedRotations? permittedRotations = BuildTool.Instance.GetPermittedRotations();
		if (permittedRotations == null)
		{
			can_rotate = false;
		}
		if (can_rotate)
		{
			LocString loc_string = UI.BUILDTOOL_ROTATE;
			string feedbackString = this.GetFeedbackString(permittedRotations.Value, BuildTool.Instance.GetBuildingOrientation);
			if (feedbackString != null)
			{
				loc_string = loc_string + "\n\n " + feedbackString;
			}
			this.tooltip.SetSimpleTooltip(loc_string);
			return;
		}
		this.tooltip.SetSimpleTooltip(UI.BUILDTOOL_CANT_ROTATE);
	}

	// Token: 0x0600970E RID: 38670 RVA: 0x003B17F4 File Offset: 0x003AF9F4
	private string GetFeedbackString(PermittedRotations permitted_rotations, Orientation current_rotation)
	{
		switch (permitted_rotations)
		{
		case PermittedRotations.R90:
			if (current_rotation == Orientation.Neutral)
			{
				return UI.BUILDTOOL_ROTATE_CURRENT_UPRIGHT;
			}
			if (current_rotation == Orientation.R90)
			{
				return UI.BUILDTOOL_ROTATE_CURRENT_ON_SIDE;
			}
			break;
		case PermittedRotations.R360:
			switch (current_rotation)
			{
			case Orientation.Neutral:
				return UI.BUILDTOOL_ROTATE_CURRENT_DEGREES.ToString().Replace("{Degrees}", "0");
			case Orientation.R90:
				return UI.BUILDTOOL_ROTATE_CURRENT_DEGREES.ToString().Replace("{Degrees}", "90");
			case Orientation.R180:
				return UI.BUILDTOOL_ROTATE_CURRENT_DEGREES.ToString().Replace("{Degrees}", "180");
			case Orientation.R270:
				return UI.BUILDTOOL_ROTATE_CURRENT_DEGREES.ToString().Replace("{Degrees}", "270");
			}
			break;
		case PermittedRotations.FlipH:
			if (current_rotation == Orientation.Neutral)
			{
				return UI.BUILDTOOL_ROTATE_CURRENT_RIGHT;
			}
			if (current_rotation == Orientation.FlipH)
			{
				return UI.BUILDTOOL_ROTATE_CURRENT_LEFT;
			}
			break;
		case PermittedRotations.FlipV:
			if (current_rotation == Orientation.Neutral)
			{
				return UI.BUILDTOOL_ROTATE_CURRENT_UP;
			}
			if (current_rotation == Orientation.FlipV)
			{
				return UI.BUILDTOOL_ROTATE_CURRENT_DOWN;
			}
			break;
		}
		DebugUtil.DevLogError(string.Format("Unexpected rotation value for tooltip (permitted_rotations: {0}, current_rotation: {1})", permitted_rotations, current_rotation));
		return null;
	}

	// Token: 0x04007592 RID: 30098
	[SerializeField]
	protected KButton button;

	// Token: 0x04007593 RID: 30099
	[SerializeField]
	protected ToolTip tooltip;
}
