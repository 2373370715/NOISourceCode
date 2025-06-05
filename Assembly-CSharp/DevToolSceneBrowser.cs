using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000BFC RID: 3068
public class DevToolSceneBrowser : DevTool
{
	// Token: 0x06003A21 RID: 14881 RVA: 0x00231A18 File Offset: 0x0022FC18
	public DevToolSceneBrowser()
	{
		this.drawFlags = ImGuiWindowFlags.MenuBar;
		DevToolSceneBrowser.StackItem stackItem = new DevToolSceneBrowser.StackItem();
		stackItem.SceneRoot = true;
		stackItem.Filter = "";
		this.Stack.Add(stackItem);
	}

	// Token: 0x06003A22 RID: 14882 RVA: 0x00231A68 File Offset: 0x0022FC68
	private void PushGameObject(GameObject go)
	{
		if (this.StackIndex < this.Stack.Count && go == this.Stack[this.StackIndex].Root)
		{
			return;
		}
		if (this.Stack.Count > this.StackIndex + 1)
		{
			this.Stack.RemoveRange(this.StackIndex + 1, this.Stack.Count - (this.StackIndex + 1));
		}
		DevToolSceneBrowser.StackItem stackItem = new DevToolSceneBrowser.StackItem();
		stackItem.SceneRoot = (go == null);
		stackItem.Root = go;
		stackItem.Filter = "";
		this.Stack.Add(stackItem);
		this.StackIndex++;
	}

	// Token: 0x06003A23 RID: 14883 RVA: 0x00231B24 File Offset: 0x0022FD24
	protected override void RenderTo(DevPanel panel)
	{
		for (int i = this.Stack.Count - 1; i > 0; i--)
		{
			DevToolSceneBrowser.StackItem stackItem = this.Stack[i];
			if (!stackItem.SceneRoot && stackItem.Root.IsNullOrDestroyed())
			{
				this.Stack.RemoveAt(i);
				this.StackIndex = Math.Min(i - 1, this.StackIndex);
			}
		}
		bool flag = false;
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.BeginMenu("Utils"))
			{
				if (ImGui.MenuItem("Goto current selection"))
				{
					SelectTool instance = SelectTool.Instance;
					UnityEngine.Object x;
					if (instance == null)
					{
						x = null;
					}
					else
					{
						KSelectable selected = instance.selected;
						x = ((selected != null) ? selected.gameObject : null);
					}
					if (x != null)
					{
						SelectTool instance2 = SelectTool.Instance;
						GameObject go2;
						if (instance2 == null)
						{
							go2 = null;
						}
						else
						{
							KSelectable selected2 = instance2.selected;
							go2 = ((selected2 != null) ? selected2.gameObject : null);
						}
						this.PushGameObject(go2);
					}
				}
				if (ImGui.MenuItem("Search All"))
				{
					flag = true;
				}
				ImGui.EndMenu();
			}
			ImGui.EndMenuBar();
		}
		if (ImGui.Button(" < ") && this.StackIndex > 0)
		{
			this.StackIndex--;
		}
		ImGui.SameLine();
		if (ImGui.Button(" ^ ") && this.StackIndex > 0 && !this.Stack[this.StackIndex].SceneRoot)
		{
			Transform parent = this.Stack[this.StackIndex].Root.transform.parent;
			this.PushGameObject((parent != null) ? parent.gameObject : null);
		}
		ImGui.SameLine();
		if (ImGui.Button(" > ") && this.StackIndex + 1 < this.Stack.Count)
		{
			this.StackIndex++;
		}
		DevToolSceneBrowser.StackItem stackItem2 = this.Stack[this.StackIndex];
		if (!stackItem2.SceneRoot)
		{
			ImGui.SameLine();
			if (ImGui.Button("Inspect"))
			{
				DevToolSceneInspector.Inspect(stackItem2.Root);
			}
		}
		List<GameObject> list;
		if (stackItem2.SceneRoot)
		{
			ImGui.Text("Scene root");
			Scene activeScene = SceneManager.GetActiveScene();
			list = new List<GameObject>(activeScene.rootCount);
			activeScene.GetRootGameObjects(list);
		}
		else
		{
			ImGui.LabelText("Selected object", stackItem2.Root.name);
			list = new List<GameObject>();
			foreach (object obj in stackItem2.Root.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject != stackItem2.Root)
				{
					list.Add(transform.gameObject);
				}
			}
		}
		if (ImGui.Button("Clear"))
		{
			stackItem2.Filter = "";
		}
		ImGui.SameLine();
		ImGui.InputText("Filter", ref stackItem2.Filter, 64U);
		ImGui.BeginChild("ScrollRegion", new Vector2(0f, 0f), true, ImGuiWindowFlags.None);
		for (int j = 0; j < list.Count; j++)
		{
			GameObject gameObject = list[j];
			if (!(stackItem2.Filter != "") || gameObject.name.IndexOf(stackItem2.Filter, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
			{
				if (ImGui.Selectable(gameObject.name, false, ImGuiSelectableFlags.AllowDoubleClick) && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
				{
					this.PushGameObject(gameObject);
				}
				if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
				{
					DevToolSceneBrowser.SelectedIndex = j;
					ImGui.OpenPopup("RightClickMenu");
				}
			}
		}
		if (ImGui.BeginPopup("RightClickMenu"))
		{
			if (ImGui.MenuItem("Inspect"))
			{
				DevToolSceneInspector.Inspect(list[DevToolSceneBrowser.SelectedIndex]);
				DevToolSceneBrowser.SelectedIndex = -1;
			}
			ImGui.EndPopup();
		}
		ImGui.EndChild();
		if (flag)
		{
			ImGui.OpenPopup("SearchAll");
		}
		if (ImGui.BeginPopupModal("SearchAll"))
		{
			ImGui.Text("Search all objects in the scene:");
			ImGui.Separator();
			if (ImGui.Button("Clear"))
			{
				DevToolSceneBrowser.SearchFilter = "";
			}
			ImGui.SameLine();
			if (ImGui.InputText("Filter", ref DevToolSceneBrowser.SearchFilter, 64U))
			{
				DevToolSceneBrowser.SearchResults = (from go in UnityEngine.Object.FindObjectsOfType<GameObject>()
				where go.name.IndexOf(DevToolSceneBrowser.SearchFilter, 0, StringComparison.CurrentCultureIgnoreCase) != -1
				orderby go.name
				select go).ToList<GameObject>();
			}
			ImGui.BeginChild("ScrollRegion", new Vector2(0f, 200f), true, ImGuiWindowFlags.None);
			int num = 0;
			using (List<GameObject>.Enumerator enumerator2 = DevToolSceneBrowser.SearchResults.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (ImGui.Selectable(enumerator2.Current.name, DevToolSceneBrowser.SearchSelectedIndex == num))
					{
						DevToolSceneBrowser.SearchSelectedIndex = num;
					}
					num++;
				}
			}
			ImGui.EndChild();
			bool flag2 = false;
			if (ImGui.Button("Browse") && DevToolSceneBrowser.SearchSelectedIndex >= 0)
			{
				this.PushGameObject(DevToolSceneBrowser.SearchResults[DevToolSceneBrowser.SearchSelectedIndex]);
				flag2 = true;
			}
			ImGui.SameLine();
			if (ImGui.Button("Inspect") && DevToolSceneBrowser.SearchSelectedIndex >= 0)
			{
				DevToolSceneInspector.Inspect(DevToolSceneBrowser.SearchResults[DevToolSceneBrowser.SearchSelectedIndex]);
				flag2 = true;
			}
			ImGui.SameLine();
			if (ImGui.Button("Cancel"))
			{
				flag2 = true;
			}
			if (flag2)
			{
				DevToolSceneBrowser.SearchFilter = "";
				DevToolSceneBrowser.SearchResults.Clear();
				DevToolSceneBrowser.SearchSelectedIndex = -1;
				ImGui.CloseCurrentPopup();
			}
			ImGui.EndPopup();
		}
	}

	// Token: 0x0400282C RID: 10284
	private List<DevToolSceneBrowser.StackItem> Stack = new List<DevToolSceneBrowser.StackItem>();

	// Token: 0x0400282D RID: 10285
	private int StackIndex;

	// Token: 0x0400282E RID: 10286
	private static int SelectedIndex = -1;

	// Token: 0x0400282F RID: 10287
	private static string SearchFilter = "";

	// Token: 0x04002830 RID: 10288
	private static List<GameObject> SearchResults = new List<GameObject>();

	// Token: 0x04002831 RID: 10289
	private static int SearchSelectedIndex = -1;

	// Token: 0x02000BFD RID: 3069
	private class StackItem
	{
		// Token: 0x04002832 RID: 10290
		public bool SceneRoot;

		// Token: 0x04002833 RID: 10291
		public GameObject Root;

		// Token: 0x04002834 RID: 10292
		public string Filter;
	}
}
