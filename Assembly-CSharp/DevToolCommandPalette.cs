using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000BD5 RID: 3029
public class DevToolCommandPalette : DevTool
{
	// Token: 0x06003976 RID: 14710 RVA: 0x000C9A0E File Offset: 0x000C7C0E
	public DevToolCommandPalette() : this(null)
	{
	}

	// Token: 0x06003977 RID: 14711 RVA: 0x0022C040 File Offset: 0x0022A240
	public DevToolCommandPalette(List<DevToolCommandPalette.Command> commands = null)
	{
		this.drawFlags |= ImGuiWindowFlags.NoResize;
		this.drawFlags |= ImGuiWindowFlags.NoScrollbar;
		this.drawFlags |= ImGuiWindowFlags.NoScrollWithMouse;
		if (commands == null)
		{
			this.commands.allValues = DevToolCommandPaletteUtil.GenerateDefaultCommandPalette();
			return;
		}
		this.commands.allValues = commands;
	}

	// Token: 0x06003978 RID: 14712 RVA: 0x000C9A17 File Offset: 0x000C7C17
	public static void Init()
	{
		DevToolCommandPalette.InitWithCommands(DevToolCommandPaletteUtil.GenerateDefaultCommandPalette());
	}

	// Token: 0x06003979 RID: 14713 RVA: 0x000C9A23 File Offset: 0x000C7C23
	public static void InitWithCommands(List<DevToolCommandPalette.Command> commands)
	{
		DevToolManager.Instance.panels.AddPanelFor(new DevToolCommandPalette(commands));
	}

	// Token: 0x0600397A RID: 14714 RVA: 0x0022C0D0 File Offset: 0x0022A2D0
	protected override void RenderTo(DevPanel panel)
	{
		DevToolCommandPalette.Resize(panel);
		if (this.commands.allValues == null)
		{
			ImGui.Text("No commands list given");
			return;
		}
		if (this.commands.allValues.Count == 0)
		{
			ImGui.Text("Given command list is empty, no results to show.");
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			panel.Close();
			return;
		}
		if (!ImGui.IsWindowFocused(ImGuiFocusedFlags.ChildWindows))
		{
			panel.Close();
			return;
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			this.m_selected_index--;
			this.shouldScrollToSelectedCommandFlag = true;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.m_selected_index++;
			this.shouldScrollToSelectedCommandFlag = true;
		}
		if (this.commands.filteredValues.Count > 0)
		{
			while (this.m_selected_index < 0)
			{
				this.m_selected_index += this.commands.filteredValues.Count;
			}
			this.m_selected_index %= this.commands.filteredValues.Count;
		}
		else
		{
			this.m_selected_index = 0;
		}
		DevToolCommandPalette.Command command = null;
		if ((Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) && this.commands.filteredValues.Count > 0 && command == null)
		{
			command = this.commands.filteredValues[this.m_selected_index];
		}
		if (this.m_should_focus_search)
		{
			ImGui.SetKeyboardFocusHere();
		}
		if (ImGui.InputText("Filter", ref this.commands.filter, 30U) || this.m_should_focus_search)
		{
			this.commands.Refilter();
		}
		this.m_should_focus_search = false;
		ImGui.Separator();
		string text = "Up arrow & down arrow to navigate. Enter to select. ";
		if (this.commands.filteredValues.Count > 0 && this.commands.didUseFilter)
		{
			text += string.Format("Found {0} Results", this.commands.filteredValues.Count);
		}
		ImGui.Text(text);
		ImGui.Separator();
		if (ImGui.BeginChild("ID_scroll_region"))
		{
			if (this.commands.filteredValues.Count <= 0)
			{
				ImGui.Text("Couldn't find anything that matches \"" + this.commands.filter + "\", maybe it hasn't been added yet?");
			}
			else
			{
				for (int i = 0; i < this.commands.filteredValues.Count; i++)
				{
					DevToolCommandPalette.Command command2 = this.commands.filteredValues[i];
					bool flag = i == this.m_selected_index;
					ImGui.PushID(i);
					bool flag2;
					if (flag)
					{
						flag2 = ImGui.Selectable("> " + command2.display_name, flag);
					}
					else
					{
						flag2 = ImGui.Selectable("  " + command2.display_name, flag);
					}
					ImGui.PopID();
					if (this.shouldScrollToSelectedCommandFlag && flag)
					{
						this.shouldScrollToSelectedCommandFlag = false;
						ImGui.SetScrollHereY(0.5f);
					}
					if (flag2 && command == null)
					{
						command = command2;
					}
				}
			}
		}
		ImGui.EndChild();
		if (command != null)
		{
			command.Internal_Select();
			panel.Close();
		}
	}

	// Token: 0x0600397B RID: 14715 RVA: 0x0022C3C0 File Offset: 0x0022A5C0
	private static void Resize(DevPanel devToolPanel)
	{
		float num = 800f;
		float num2 = 400f;
		Rect rect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		Rect rect2 = new Rect
		{
			x = rect.x + rect.width / 2f - num / 2f,
			y = rect.y + rect.height / 2f - num2 / 2f,
			width = num,
			height = num2
		};
		devToolPanel.SetPosition(rect2.position, ImGuiCond.None);
		devToolPanel.SetSize(rect2.size, ImGuiCond.None);
	}

	// Token: 0x040027B4 RID: 10164
	private int m_selected_index;

	// Token: 0x040027B5 RID: 10165
	private StringSearchableList<DevToolCommandPalette.Command> commands = new StringSearchableList<DevToolCommandPalette.Command>(delegate(DevToolCommandPalette.Command command, in string filter)
	{
		return !StringSearchableListUtil.DoAnyTagsMatchFilter(command.tags, filter);
	});

	// Token: 0x040027B6 RID: 10166
	private bool m_should_focus_search = true;

	// Token: 0x040027B7 RID: 10167
	private bool shouldScrollToSelectedCommandFlag;

	// Token: 0x02000BD6 RID: 3030
	public class Command
	{
		// Token: 0x0600397C RID: 14716 RVA: 0x000C9A3B File Offset: 0x000C7C3B
		public Command(string primary_tag, System.Action on_select) : this(new string[]
		{
			primary_tag
		}, on_select)
		{
		}

		// Token: 0x0600397D RID: 14717 RVA: 0x000C9A4E File Offset: 0x000C7C4E
		public Command(string primary_tag, string tag_a, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a
		}, on_select)
		{
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x000C9A65 File Offset: 0x000C7C65
		public Command(string primary_tag, string tag_a, string tag_b, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b
		}, on_select)
		{
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x000C9A81 File Offset: 0x000C7C81
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c
		}, on_select)
		{
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x000C9AA2 File Offset: 0x000C7CA2
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c,
			tag_d
		}, on_select)
		{
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x000C9AC8 File Offset: 0x000C7CC8
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, string tag_e, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c,
			tag_d,
			tag_e
		}, on_select)
		{
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x000C9AF3 File Offset: 0x000C7CF3
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, string tag_e, string tag_f, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c,
			tag_d,
			tag_e,
			tag_f
		}, on_select)
		{
		}

		// Token: 0x06003983 RID: 14723 RVA: 0x000C9B23 File Offset: 0x000C7D23
		public Command(string primary_tag, string[] additional_tags, System.Action on_select) : this(new string[]
		{
			primary_tag
		}.Concat(additional_tags).ToArray<string>(), on_select)
		{
		}

		// Token: 0x06003984 RID: 14724 RVA: 0x0022C478 File Offset: 0x0022A678
		public Command(string[] tags, System.Action on_select)
		{
			this.display_name = tags[0];
			this.tags = (from t in tags
			select t.ToLowerInvariant()).ToArray<string>();
			this.m_on_select = on_select;
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x000C9B41 File Offset: 0x000C7D41
		public void Internal_Select()
		{
			this.m_on_select();
		}

		// Token: 0x040027B8 RID: 10168
		public string display_name;

		// Token: 0x040027B9 RID: 10169
		public string[] tags;

		// Token: 0x040027BA RID: 10170
		private System.Action m_on_select;
	}
}
