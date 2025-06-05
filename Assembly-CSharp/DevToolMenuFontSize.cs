using System;
using ImGuiNET;

// Token: 0x02000BE9 RID: 3049
public class DevToolMenuFontSize
{
	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x060039CA RID: 14794 RVA: 0x000C9DE9 File Offset: 0x000C7FE9
	// (set) Token: 0x060039C9 RID: 14793 RVA: 0x000C9DE0 File Offset: 0x000C7FE0
	public bool initialized { get; private set; }

	// Token: 0x060039CB RID: 14795 RVA: 0x0022DFDC File Offset: 0x0022C1DC
	public void RefreshFontSize()
	{
		DevToolMenuFontSize.FontSizeCategory @int = (DevToolMenuFontSize.FontSizeCategory)KPlayerPrefs.GetInt("Imgui_font_size_category", 2);
		this.SetFontSizeCategory(@int);
	}

	// Token: 0x060039CC RID: 14796 RVA: 0x000C9DF1 File Offset: 0x000C7FF1
	public void InitializeIfNeeded()
	{
		if (!this.initialized)
		{
			this.initialized = true;
			this.RefreshFontSize();
		}
	}

	// Token: 0x060039CD RID: 14797 RVA: 0x0022DFFC File Offset: 0x0022C1FC
	public void DrawMenu()
	{
		if (ImGui.BeginMenu("Settings"))
		{
			bool flag = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Fabric;
			bool flag2 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Small;
			bool flag3 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Regular;
			bool flag4 = this.fontSizeCategory == DevToolMenuFontSize.FontSizeCategory.Large;
			if (ImGui.BeginMenu("Size"))
			{
				if (ImGui.Checkbox("Original Font", ref flag) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Fabric)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Fabric);
				}
				if (ImGui.Checkbox("Small Text", ref flag2) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Small)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Small);
				}
				if (ImGui.Checkbox("Regular Text", ref flag3) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Regular)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Regular);
				}
				if (ImGui.Checkbox("Large Text", ref flag4) && this.fontSizeCategory != DevToolMenuFontSize.FontSizeCategory.Large)
				{
					this.SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory.Large);
				}
				ImGui.EndMenu();
			}
			ImGui.EndMenu();
		}
	}

	// Token: 0x060039CE RID: 14798 RVA: 0x0022E0D0 File Offset: 0x0022C2D0
	public unsafe void SetFontSizeCategory(DevToolMenuFontSize.FontSizeCategory size)
	{
		this.fontSizeCategory = size;
		KPlayerPrefs.SetInt("Imgui_font_size_category", (int)size);
		ImGuiIOPtr io = ImGui.GetIO();
		if (size < (DevToolMenuFontSize.FontSizeCategory)io.Fonts.Fonts.Size)
		{
			ImFontPtr wrappedPtr = *io.Fonts.Fonts[(int)size];
			io.NativePtr->FontDefault = wrappedPtr;
		}
	}

	// Token: 0x040027D7 RID: 10199
	public const string SETTINGS_KEY_FONT_SIZE_CATEGORY = "Imgui_font_size_category";

	// Token: 0x040027D8 RID: 10200
	private DevToolMenuFontSize.FontSizeCategory fontSizeCategory;

	// Token: 0x02000BEA RID: 3050
	public enum FontSizeCategory
	{
		// Token: 0x040027DB RID: 10203
		Fabric,
		// Token: 0x040027DC RID: 10204
		Small,
		// Token: 0x040027DD RID: 10205
		Regular,
		// Token: 0x040027DE RID: 10206
		Large
	}
}
