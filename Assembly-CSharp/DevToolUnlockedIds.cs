using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000C14 RID: 3092
public class DevToolUnlockedIds : DevTool
{
	// Token: 0x06003A9D RID: 15005 RVA: 0x000CA51F File Offset: 0x000C871F
	public DevToolUnlockedIds()
	{
		this.RequiresGameRunning = true;
	}

	// Token: 0x06003A9E RID: 15006 RVA: 0x00235B44 File Offset: 0x00233D44
	protected override void RenderTo(DevPanel panel)
	{
		bool flag;
		DevToolUnlockedIds.UnlocksWrapper unlocksWrapper;
		this.GetUnlocks().Deconstruct(out flag, out unlocksWrapper);
		bool flag2 = flag;
		DevToolUnlockedIds.UnlocksWrapper unlocksWrapper2 = unlocksWrapper;
		if (!flag2)
		{
			ImGui.Text("Couldn't access global unlocks");
			return;
		}
		if (ImGui.TreeNode("Help"))
		{
			ImGui.TextWrapped("This is a list of global unlocks that are persistant across saves. Changes made here will be saved to disk immediately.");
			ImGui.Spacing();
			ImGui.TextWrapped("NOTE: It may be necessary to relaunch the game after modifying unlocks in order for systems to respond.");
			ImGui.TreePop();
		}
		ImGui.Spacing();
		ImGuiEx.InputFilter("Filter", ref this.filterForUnlockIds, 50U);
		ImGuiTableFlags flags = ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersInnerH | ImGuiTableFlags.BordersOuterH | ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.BordersOuterV | ImGuiTableFlags.ScrollY;
		if (ImGui.BeginTable("ID_unlockIds", 2, flags))
		{
			ImGui.TableSetupScrollFreeze(2, 2);
			ImGui.TableSetupColumn("Unlock ID");
			ImGui.TableSetupColumn("Actions", ImGuiTableColumnFlags.WidthFixed);
			ImGui.TableHeadersRow();
			ImGui.PushID("ID_row_add_new");
			ImGui.TableNextRow();
			ImGui.TableSetColumnIndex(0);
			ImGui.InputText("", ref this.unlockIdToAdd, 50U);
			ImGui.TableSetColumnIndex(1);
			if (ImGui.Button("Add"))
			{
				unlocksWrapper2.AddId(this.unlockIdToAdd);
				global::Debug.Log("[Added unlock id] " + this.unlockIdToAdd);
				this.unlockIdToAdd = "";
			}
			ImGui.PopID();
			int num = 0;
			foreach (string text in unlocksWrapper2.GetAllIds())
			{
				string text2 = (text == null) ? "<<null>>" : ("\"" + text + "\"");
				if (text2.ToLower().Contains(this.filterForUnlockIds.ToLower()))
				{
					ImGui.TableNextRow();
					ImGui.PushID(string.Format("ID_row_{0}", num++));
					ImGui.TableSetColumnIndex(0);
					ImGui.Text(text2);
					ImGui.TableSetColumnIndex(1);
					if (ImGui.Button("Copy"))
					{
						GUIUtility.systemCopyBuffer = text;
						global::Debug.Log("[Copied to clipboard] " + text);
					}
					ImGui.SameLine();
					if (ImGui.Button("Remove"))
					{
						unlocksWrapper2.RemoveId(text);
						global::Debug.Log("[Removed unlock id] " + text);
					}
					ImGui.PopID();
				}
			}
			ImGui.EndTable();
		}
	}

	// Token: 0x06003A9F RID: 15007 RVA: 0x00235D70 File Offset: 0x00233F70
	private Option<DevToolUnlockedIds.UnlocksWrapper> GetUnlocks()
	{
		if (App.IsExiting)
		{
			return Option.None;
		}
		if (Game.Instance == null || !Game.Instance)
		{
			return Option.None;
		}
		if (Game.Instance.unlocks == null)
		{
			return Option.None;
		}
		return Option.Some<DevToolUnlockedIds.UnlocksWrapper>(new DevToolUnlockedIds.UnlocksWrapper(Game.Instance.unlocks));
	}

	// Token: 0x04002892 RID: 10386
	private string filterForUnlockIds = "";

	// Token: 0x04002893 RID: 10387
	private string unlockIdToAdd = "";

	// Token: 0x02000C15 RID: 3093
	public readonly struct UnlocksWrapper
	{
		// Token: 0x06003AA0 RID: 15008 RVA: 0x000CA544 File Offset: 0x000C8744
		public UnlocksWrapper(Unlocks unlocks)
		{
			this.unlocks = unlocks;
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x000CA54D File Offset: 0x000C874D
		public void AddId(string unlockId)
		{
			this.unlocks.Unlock(unlockId, true);
		}

		// Token: 0x06003AA2 RID: 15010 RVA: 0x000CA55C File Offset: 0x000C875C
		public void RemoveId(string unlockId)
		{
			this.unlocks.Lock(unlockId);
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x000CA56A File Offset: 0x000C876A
		public IEnumerable<string> GetAllIds()
		{
			return from s in this.unlocks.GetAllUnlockedIds()
			orderby s
			select s;
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06003AA4 RID: 15012 RVA: 0x000CA59B File Offset: 0x000C879B
		public int Count
		{
			get
			{
				return this.unlocks.GetAllUnlockedIds().Count;
			}
		}

		// Token: 0x04002894 RID: 10388
		public readonly Unlocks unlocks;
	}
}
