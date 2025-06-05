using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001B1E RID: 6942
[AddComponentMenu("KMonoBehaviour/scripts/HoverTextConfiguration")]
public class HoverTextConfiguration : KMonoBehaviour
{
	// Token: 0x06009171 RID: 37233 RVA: 0x00103830 File Offset: 0x00101A30
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ConfigureHoverScreen();
	}

	// Token: 0x06009172 RID: 37234 RVA: 0x0010383E File Offset: 0x00101A3E
	protected virtual void ConfigureTitle(HoverTextScreen screen)
	{
		if (string.IsNullOrEmpty(this.ToolName))
		{
			this.ToolName = Strings.Get(this.ToolNameStringKey).String.ToUpper();
		}
	}

	// Token: 0x06009173 RID: 37235 RVA: 0x00103868 File Offset: 0x00101A68
	protected void DrawTitle(HoverTextScreen screen, HoverTextDrawer drawer)
	{
		drawer.DrawText(this.ToolName, this.ToolTitleTextStyle);
	}

	// Token: 0x06009174 RID: 37236 RVA: 0x0038D480 File Offset: 0x0038B680
	protected void DrawInstructions(HoverTextScreen screen, HoverTextDrawer drawer)
	{
		TextStyleSetting standard = this.Styles_Instruction.Standard;
		drawer.NewLine(26);
		if (KInputManager.currentControllerIsGamepad)
		{
			drawer.DrawIcon(KInputManager.steamInputInterpreter.GetActionSprite(global::Action.MouseLeft, false), 20);
		}
		else
		{
			drawer.DrawIcon(screen.GetSprite("icon_mouse_left"), 20);
		}
		drawer.DrawText(this.ActionName, standard);
		drawer.AddIndent(8);
		if (KInputManager.currentControllerIsGamepad)
		{
			drawer.DrawIcon(KInputManager.steamInputInterpreter.GetActionSprite(global::Action.MouseRight, false), 20);
		}
		else
		{
			drawer.DrawIcon(screen.GetSprite("icon_mouse_right"), 20);
		}
		drawer.DrawText(this.backStr, standard);
	}

	// Token: 0x06009175 RID: 37237 RVA: 0x0038D524 File Offset: 0x0038B724
	public virtual void ConfigureHoverScreen()
	{
		if (!string.IsNullOrEmpty(this.ActionStringKey))
		{
			this.ActionName = Strings.Get(this.ActionStringKey);
		}
		HoverTextScreen instance = HoverTextScreen.Instance;
		this.ConfigureTitle(instance);
		this.backStr = UI.TOOLS.GENERIC.BACK.ToString().ToUpper();
	}

	// Token: 0x06009176 RID: 37238 RVA: 0x0038D578 File Offset: 0x0038B778
	public virtual void UpdateHoverElements(List<KSelectable> hover_objects)
	{
		HoverTextScreen instance = HoverTextScreen.Instance;
		HoverTextDrawer hoverTextDrawer = instance.BeginDrawing();
		int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		if (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			hoverTextDrawer.EndDrawing();
			return;
		}
		hoverTextDrawer.BeginShadowBar(false);
		this.DrawTitle(instance, hoverTextDrawer);
		this.DrawInstructions(HoverTextScreen.Instance, hoverTextDrawer);
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x04006E0B RID: 28171
	public TextStyleSetting[] HoverTextStyleSettings;

	// Token: 0x04006E0C RID: 28172
	public string ToolNameStringKey = "";

	// Token: 0x04006E0D RID: 28173
	public string ActionStringKey = "";

	// Token: 0x04006E0E RID: 28174
	[HideInInspector]
	public string ActionName = "";

	// Token: 0x04006E0F RID: 28175
	[HideInInspector]
	public string ToolName;

	// Token: 0x04006E10 RID: 28176
	protected string backStr;

	// Token: 0x04006E11 RID: 28177
	public TextStyleSetting ToolTitleTextStyle;

	// Token: 0x04006E12 RID: 28178
	public HoverTextConfiguration.TextStylePair Styles_Title;

	// Token: 0x04006E13 RID: 28179
	public HoverTextConfiguration.TextStylePair Styles_BodyText;

	// Token: 0x04006E14 RID: 28180
	public HoverTextConfiguration.TextStylePair Styles_Instruction;

	// Token: 0x04006E15 RID: 28181
	public HoverTextConfiguration.TextStylePair Styles_Warning;

	// Token: 0x04006E16 RID: 28182
	public HoverTextConfiguration.ValuePropertyTextStyles Styles_Values;

	// Token: 0x02001B1F RID: 6943
	[Serializable]
	public struct TextStylePair
	{
		// Token: 0x04006E17 RID: 28183
		public TextStyleSetting Standard;

		// Token: 0x04006E18 RID: 28184
		public TextStyleSetting Selected;
	}

	// Token: 0x02001B20 RID: 6944
	[Serializable]
	public struct ValuePropertyTextStyles
	{
		// Token: 0x04006E19 RID: 28185
		public HoverTextConfiguration.TextStylePair Property;

		// Token: 0x04006E1A RID: 28186
		public HoverTextConfiguration.TextStylePair Property_Decimal;

		// Token: 0x04006E1B RID: 28187
		public HoverTextConfiguration.TextStylePair Property_Unit;
	}
}
