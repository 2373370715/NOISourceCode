using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000C08 RID: 3080
public class DevToolStateMachineDebug : DevTool
{
	// Token: 0x06003A54 RID: 14932 RVA: 0x002345E8 File Offset: 0x002327E8
	private void Update()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.selectedGameObject == null)
		{
			this.lockSelection = false;
		}
		SelectTool instance = SelectTool.Instance;
		GameObject gameObject;
		if (instance == null)
		{
			gameObject = null;
		}
		else
		{
			KSelectable selected = instance.selected;
			gameObject = ((selected != null) ? selected.gameObject : null);
		}
		GameObject gameObject2 = gameObject;
		if (!this.lockSelection && this.selectedGameObject != gameObject2 && gameObject2 != null && gameObject2.GetComponentsInChildren<StateMachineController>().Length != 0)
		{
			this.selectedGameObject = gameObject2;
			this.selectedStateMachine = 0;
		}
	}

	// Token: 0x06003A55 RID: 14933 RVA: 0x00234668 File Offset: 0x00232868
	public void ShowEditor(StateMachineDebuggerSettings.Entry entry)
	{
		ImGui.Text(entry.typeName);
		ImGui.SameLine();
		ImGui.PushID(entry.typeName);
		ImGui.PushID(1);
		ImGui.Checkbox("", ref entry.enableConsoleLogging);
		ImGui.PopID();
		ImGui.SameLine();
		ImGui.PushID(2);
		ImGui.Checkbox("", ref entry.breakOnGoTo);
		ImGui.PopID();
		ImGui.SameLine();
		ImGui.PushID(3);
		ImGui.Checkbox("", ref entry.saveHistory);
		ImGui.PopID();
		ImGui.PopID();
	}

	// Token: 0x06003A56 RID: 14934 RVA: 0x002346F4 File Offset: 0x002328F4
	protected override void RenderTo(DevPanel panel)
	{
		this.Update();
		ImGui.InputText("Filter:", ref this.stateMachineFilter, 256U);
		if (this.showSettings = ImGui.CollapsingHeader("Debug Settings:"))
		{
			if (ImGui.Button("Reset"))
			{
				StateMachineDebuggerSettings.Get().Clear();
			}
			ImGui.Text("EnableConsoleLogging / BreakOnGoTo / SaveHistory");
			int num = 0;
			foreach (StateMachineDebuggerSettings.Entry entry in StateMachineDebuggerSettings.Get())
			{
				if (string.IsNullOrEmpty(this.stateMachineFilter) || entry.typeName.ToLower().IndexOf(this.stateMachineFilter) >= 0)
				{
					this.ShowEditor(entry);
					num++;
				}
			}
		}
		if (Application.isPlaying && this.selectedGameObject != null)
		{
			StateMachineController[] componentsInChildren = this.selectedGameObject.GetComponentsInChildren<StateMachineController>();
			if (componentsInChildren.Length == 0)
			{
				return;
			}
			List<string> list = new List<string>();
			List<StateMachine.Instance> list2 = new List<StateMachine.Instance>();
			List<StateMachine.BaseDef> list3 = new List<StateMachine.BaseDef>();
			foreach (StateMachineController stateMachineController in componentsInChildren)
			{
				foreach (StateMachine.Instance instance in stateMachineController)
				{
					string text = stateMachineController.name + "." + instance.ToString();
					if (instance.isCrashed)
					{
						text = "(ERROR)" + text;
					}
					list.Add(text);
				}
			}
			List<string> list4;
			if (this.stateMachineFilter == null || this.stateMachineFilter == "")
			{
				list4 = (from name in list
				select name.ToLower()).ToList<string>();
			}
			else
			{
				list4 = (from name in list
				where name.ToLower().Contains(this.stateMachineFilter)
				select name.ToLower()).ToList<string>();
			}
			foreach (StateMachineController stateMachineController2 in componentsInChildren)
			{
				foreach (StateMachine.Instance instance2 in stateMachineController2)
				{
					string text2 = stateMachineController2.name + "." + instance2.ToString();
					if (instance2.isCrashed)
					{
						text2 = "(ERROR)" + text2;
					}
					if (list4.Contains(text2.ToLower()))
					{
						list2.Add(instance2);
					}
				}
				foreach (StateMachine.BaseDef item in stateMachineController2.GetDefs<StateMachine.BaseDef>())
				{
					list3.Add(item);
				}
			}
			if (list4.Count == 0)
			{
				string label = "Defs";
				string fmt;
				if (list3.Count != 0)
				{
					fmt = string.Join(", ", from d in list3
					select d.GetType().ToString());
				}
				else
				{
					fmt = "(none)";
				}
				ImGui.LabelText(label, fmt);
				foreach (StateMachineController controller in componentsInChildren)
				{
					this.ShowControllerLog(controller);
				}
				return;
			}
			this.selectedStateMachine = Math.Min(this.selectedStateMachine, list4.Count - 1);
			string label2 = "Defs";
			string fmt2;
			if (list3.Count != 0)
			{
				fmt2 = string.Join(", ", from d in list3
				select d.GetType().ToString());
			}
			else
			{
				fmt2 = "(none)";
			}
			ImGui.LabelText(label2, fmt2);
			ImGui.Checkbox("Lock selection", ref this.lockSelection);
			ImGui.Indent();
			ImGui.Combo("Select state machine", ref this.selectedStateMachine, list4.ToArray(), list4.Count);
			ImGui.Unindent();
			StateMachine.Instance instance3 = list2[this.selectedStateMachine];
			this.ShowStates(instance3);
			this.ShowTags(instance3);
			this.ShowDetails(instance3);
			this.ShowLog(instance3);
			this.ShowControllerLog(instance3);
			this.ShowHistory(instance3.GetMaster().GetComponent<StateMachineController>());
			this.ShowKAnimControllerLog();
		}
	}

	// Token: 0x06003A57 RID: 14935 RVA: 0x00234B6C File Offset: 0x00232D6C
	private void ShowStates(StateMachine.Instance state_machine_instance)
	{
		StateMachine stateMachine = state_machine_instance.GetStateMachine();
		ImGui.Text(stateMachine.ToString() + ": ");
		ImGui.Checkbox("Break On GoTo: ", ref state_machine_instance.breakOnGoTo);
		ImGui.Checkbox("Console Logging: ", ref state_machine_instance.enableConsoleLogging);
		string value = "None";
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		if (currentState != null)
		{
			value = currentState.name;
		}
		string[] array = stateMachine.GetStateNames().Append("None");
		array[0] = array[0];
		int num = Array.IndexOf<string>(array, value);
		int num2 = num;
		for (int i = 0; i < array.Length; i++)
		{
			ImGui.RadioButton(array[i], ref num2, i);
		}
		if (num2 != num)
		{
			if (array[num2] == "None")
			{
				state_machine_instance.StopSM("StateMachineEditor.StopSM");
				return;
			}
			state_machine_instance.GoTo(array[num2]);
		}
	}

	// Token: 0x06003A58 RID: 14936 RVA: 0x00234C3C File Offset: 0x00232E3C
	public void ShowTags(StateMachine.Instance state_machine_instance)
	{
		ImGui.Text("Tags:");
		ImGui.Indent();
		KPrefabID component = state_machine_instance.GetComponent<KPrefabID>();
		if (component != null)
		{
			foreach (Tag tag in component.Tags)
			{
				ImGui.Text(tag.Name);
			}
		}
		ImGui.Unindent();
	}

	// Token: 0x06003A59 RID: 14937 RVA: 0x00234CB8 File Offset: 0x00232EB8
	private void ShowDetails(StateMachine.Instance state_machine_instance)
	{
		state_machine_instance.GetStateMachine();
		string str = "None";
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		if (currentState != null)
		{
			str = currentState.name;
		}
		ImGui.Text(str + ": ");
		ImGui.Indent();
		this.ShowParameters(state_machine_instance);
		this.ShowEvents(state_machine_instance);
		this.ShowTransitions(state_machine_instance);
		this.ShowEnterActions(state_machine_instance);
		this.ShowExitActions(state_machine_instance);
		ImGui.Unindent();
	}

	// Token: 0x06003A5A RID: 14938 RVA: 0x00234D20 File Offset: 0x00232F20
	private void ShowParameters(StateMachine.Instance state_machine_instance)
	{
		ImGui.Text("Parameters:");
		ImGui.Indent();
		StateMachine.Parameter.Context[] parameterContexts = state_machine_instance.GetParameterContexts();
		for (int i = 0; i < parameterContexts.Length; i++)
		{
			parameterContexts[i].ShowDevTool(state_machine_instance);
		}
		ImGui.Unindent();
	}

	// Token: 0x06003A5B RID: 14939 RVA: 0x00234D60 File Offset: 0x00232F60
	private void ShowEvents(StateMachine.Instance state_machine_instance)
	{
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		ImGui.Text("Events: ");
		if (currentState == null)
		{
			return;
		}
		ImGui.Indent();
		for (int i = 0; i < currentState.GetStateCount(); i++)
		{
			StateMachine.BaseState state = currentState.GetState(i);
			if (state.events != null)
			{
				foreach (StateEvent stateEvent in state.events)
				{
					ImGui.Text(stateEvent.GetName());
				}
			}
		}
		ImGui.Unindent();
	}

	// Token: 0x06003A5C RID: 14940 RVA: 0x00234DF8 File Offset: 0x00232FF8
	private void ShowTransitions(StateMachine.Instance state_machine_instance)
	{
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		ImGui.Text("Transitions:");
		if (currentState == null)
		{
			return;
		}
		ImGui.Indent();
		for (int i = 0; i < currentState.GetStateCount(); i++)
		{
			StateMachine.BaseState state = currentState.GetState(i);
			if (state.transitions != null)
			{
				for (int j = 0; j < state.transitions.Count; j++)
				{
					ImGui.Text(state.transitions[j].ToString());
				}
			}
		}
		ImGui.Unindent();
	}

	// Token: 0x06003A5D RID: 14941 RVA: 0x00234E74 File Offset: 0x00233074
	private void ShowExitActions(StateMachine.Instance state_machine_instance)
	{
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		ImGui.Text("Exit Actions: ");
		if (currentState == null)
		{
			return;
		}
		ImGui.Indent();
		for (int i = 0; i < currentState.GetStateCount(); i++)
		{
			StateMachine.BaseState state = currentState.GetState(i);
			if (state.exitActions != null)
			{
				foreach (StateMachine.Action action in state.exitActions)
				{
					ImGui.Text(action.name);
				}
			}
		}
		ImGui.Unindent();
	}

	// Token: 0x06003A5E RID: 14942 RVA: 0x00234F0C File Offset: 0x0023310C
	private void ShowEnterActions(StateMachine.Instance state_machine_instance)
	{
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		ImGui.Text("Enter Actions: ");
		if (currentState == null)
		{
			return;
		}
		ImGui.Indent();
		for (int i = 0; i < currentState.GetStateCount(); i++)
		{
			StateMachine.BaseState state = currentState.GetState(i);
			if (state.enterActions != null)
			{
				foreach (StateMachine.Action action in state.enterActions)
				{
					ImGui.Text(action.name);
				}
			}
		}
		ImGui.Unindent();
	}

	// Token: 0x06003A5F RID: 14943 RVA: 0x000CA24F File Offset: 0x000C844F
	private void ShowLog(StateMachine.Instance state_machine_instance)
	{
		ImGui.Text("Machine Log:");
	}

	// Token: 0x06003A60 RID: 14944 RVA: 0x000CA25B File Offset: 0x000C845B
	private void ShowKAnimControllerLog()
	{
		this.selectedGameObject.GetComponentInChildren<KAnimControllerBase>() == null;
	}

	// Token: 0x06003A61 RID: 14945 RVA: 0x000CA26F File Offset: 0x000C846F
	private void ShowHistory(StateMachineController controller)
	{
		ImGui.Text("Logger disabled");
	}

	// Token: 0x06003A62 RID: 14946 RVA: 0x000CA27B File Offset: 0x000C847B
	private void ShowControllerLog(StateMachineController controller)
	{
		ImGui.Text("Object Log:");
	}

	// Token: 0x06003A63 RID: 14947 RVA: 0x000CA287 File Offset: 0x000C8487
	private void ShowControllerLog(StateMachine.Instance state_machine)
	{
		if (!state_machine.GetMaster().isNull)
		{
			this.ShowControllerLog(state_machine.GetMaster().GetComponent<StateMachineController>());
		}
	}

	// Token: 0x0400286A RID: 10346
	private int selectedStateMachine;

	// Token: 0x0400286B RID: 10347
	private int selectedLog;

	// Token: 0x0400286C RID: 10348
	private GameObject selectedGameObject;

	// Token: 0x0400286D RID: 10349
	private Vector2 scrollPos;

	// Token: 0x0400286E RID: 10350
	private bool lockSelection;

	// Token: 0x0400286F RID: 10351
	private bool showSettings;

	// Token: 0x04002870 RID: 10352
	private string stateMachineFilter = "";
}
