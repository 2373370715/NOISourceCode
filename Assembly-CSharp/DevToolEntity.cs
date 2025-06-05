using System;
using System.Runtime.CompilerServices;
using ImGuiNET;
using STRINGS;
using UnityEngine;

// Token: 0x02000BDC RID: 3036
public class DevToolEntity : DevTool
{
	// Token: 0x06003991 RID: 14737 RVA: 0x0022C658 File Offset: 0x0022A858
	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.MenuItem("New Window"))
			{
				DevToolUtil.Open(new DevToolEntity());
			}
			ImGui.EndMenuBar();
		}
		ImGui.Text(this.currentTargetOpt.IsNone() ? "Pick target:" : "Change target:");
		ImGui.SameLine();
		if (ImGui.Button("Eyedrop"))
		{
			panel.PushDevTool(new DevToolEntity_EyeDrop(delegate(DevToolEntityTarget result)
			{
				this.currentTargetOpt = result;
			}, null));
		}
		ImGui.SameLine();
		if (ImGui.Button("Search GameObjects (NOT implemented)"))
		{
			panel.PushDevTool(new DevToolEntity_SearchGameObjects(delegate(DevToolEntityTarget result)
			{
				this.currentTargetOpt = result;
			}));
		}
		if (this.GetInGameSelectedEntity().IsSome())
		{
			ImGui.SameLine();
			if (ImGui.Button("\"" + this.GetInGameSelectedEntity().Unwrap().name + "\""))
			{
				this.currentTargetOpt = new DevToolEntityTarget.ForWorldGameObject(this.GetInGameSelectedEntity().Unwrap());
			}
		}
		ImGui.Separator();
		ImGui.Spacing();
		if (this.currentTargetOpt.IsNone())
		{
			this.Name = "Entity";
			ImGui.Text("<nothing selected>");
		}
		else
		{
			this.Name = "Entity: " + this.currentTargetOpt.Unwrap().ToString();
			this.Name = "EntityType: " + this.currentTargetOpt.Unwrap().GetType().FullName.Substring("For".Length);
			ImGuiEx.SimpleField("Entity Name", this.currentTargetOpt.Unwrap().ToString());
		}
		ImGui.Spacing();
		ImGui.Separator();
		ImGui.Spacing();
		if (this.currentTargetOpt.IsNone())
		{
			return;
		}
		DevToolEntityTarget devToolEntityTarget = this.currentTargetOpt.Unwrap();
		DevToolEntityTarget.ForUIGameObject forUIGameObject = devToolEntityTarget as DevToolEntityTarget.ForUIGameObject;
		Option<GameObject> option;
		if (forUIGameObject != null)
		{
			option = forUIGameObject.gameObject;
		}
		else
		{
			DevToolEntityTarget.ForWorldGameObject forWorldGameObject = devToolEntityTarget as DevToolEntityTarget.ForWorldGameObject;
			if (forWorldGameObject != null)
			{
				option = forWorldGameObject.gameObject;
			}
			else
			{
				option = Option.None;
			}
		}
		if (ImGui.CollapsingHeader("Actions", ImGuiTreeNodeFlags.DefaultOpen))
		{
			ImGui.Indent();
			ImGui.Checkbox("Draw Bounding Box", ref this.shouldDrawBoundingBox);
			if (option.IsSome())
			{
				GameObject gameObject = option.Unwrap();
				if (ImGui.Button(string.Format("Inspect GameObject in DevTools###ID_InspectInGame_{0}", gameObject.GetInstanceID())))
				{
					DevToolSceneInspector.Inspect(gameObject);
				}
				JoyBehaviourMonitor.Instance smi = gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
				if (smi.IsNullOrDestroyed())
				{
					ImGuiEx.Button("Duplicant: Make Overjoyed", "No JoyBehaviourMonitor.Instance found on the selected GameObject");
				}
				else if (ImGui.Button("Duplicant: Make Overjoyed"))
				{
					smi.GoToOverjoyed();
				}
				WildnessMonitor.Instance smi2 = gameObject.GetSMI<WildnessMonitor.Instance>();
				if (smi2.IsNullOrDestroyed())
				{
					ImGuiEx.Button("Taming: Covert to Tamed", "No WildnessMonitor.Instance found on the selected GameObject");
				}
				else
				{
					WildnessMonitor wildnessMonitor = (WildnessMonitor)smi2.GetStateMachine();
					if (smi2.GetCurrentState() != wildnessMonitor.tame)
					{
						if (ImGui.Button("Taming: Convert to Tamed"))
						{
							smi2.wildness.SetValue(0f);
							smi2.GoTo(wildnessMonitor.tame);
						}
					}
					else if (ImGui.Button("Taming: Convert to Untamed"))
					{
						smi2.wildness.value = smi2.wildness.GetMax();
						smi2.GoTo(wildnessMonitor.wild);
					}
				}
			}
			ImGui.Unindent();
		}
		ImGui.Spacing();
		if (ImGui.CollapsingHeader("Related DevTools", ImGuiTreeNodeFlags.DefaultOpen))
		{
			ImGui.Indent();
			if (ImGuiEx.Button("Debug Status Items", DevToolStatusItems.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolStatusItems((DevToolEntityTarget.ForWorldGameObject)devToolEntityTarget));
			}
			if (ImGuiEx.Button("Debug Cavity", DevToolCavity.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolCavity((DevToolEntityTarget.ForSimCell)devToolEntityTarget));
			}
			if (ImGuiEx.Button("Debug GoTo", DevToolEntity_DebugGoTo.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolEntity_DebugGoTo((DevToolEntityTarget.ForWorldGameObject)devToolEntityTarget));
			}
			if (ImGuiEx.Button("Debug RanchStation", DevToolEntity_RanchStation.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolEntity_RanchStation((DevToolEntityTarget.ForWorldGameObject)devToolEntityTarget));
			}
			ImGui.Unindent();
		}
		if (this.shouldDrawBoundingBox)
		{
			Option<ValueTuple<Vector2, Vector2>> screenRect = devToolEntityTarget.GetScreenRect();
			if (screenRect.IsSome())
			{
				DevToolEntity.DrawBoundingBox(screenRect.Unwrap(), devToolEntityTarget.GetDebugName(), ImGui.IsWindowFocused());
			}
		}
	}

	// Token: 0x06003992 RID: 14738 RVA: 0x0022CAA8 File Offset: 0x0022ACA8
	public Option<GameObject> GetInGameSelectedEntity()
	{
		if (SelectTool.Instance == null)
		{
			return Option.None;
		}
		KSelectable selected = SelectTool.Instance.selected;
		if (selected.IsNullOrDestroyed())
		{
			return Option.None;
		}
		return selected.gameObject;
	}

	// Token: 0x06003993 RID: 14739 RVA: 0x0022CAF8 File Offset: 0x0022ACF8
	public static string GetNameFor(GameObject gameObject)
	{
		if (gameObject.IsNullOrDestroyed())
		{
			return "<null or destroyed GameObject>";
		}
		return string.Concat(new string[]
		{
			"\"",
			UI.StripLinkFormatting(gameObject.name),
			"\" [0x",
			gameObject.GetInstanceID().ToString("X"),
			"]"
		});
	}

	// Token: 0x06003994 RID: 14740 RVA: 0x0022CB5C File Offset: 0x0022AD5C
	public static Vector2 GetPositionFor(GameObject gameObject)
	{
		if (Camera.main != null)
		{
			Camera main = Camera.main;
			Vector2 vector = main.WorldToScreenPoint(gameObject.transform.position);
			vector.y = (float)main.pixelHeight - vector.y;
			return vector;
		}
		return Vector2.zero;
	}

	// Token: 0x06003995 RID: 14741 RVA: 0x0022CBB0 File Offset: 0x0022ADB0
	public static Vector2 GetScreenPosition(Vector3 pos)
	{
		if (Camera.main != null)
		{
			Camera main = Camera.main;
			Vector2 vector = main.WorldToScreenPoint(pos);
			vector.y = (float)main.pixelHeight - vector.y;
			return vector;
		}
		return Vector2.zero;
	}

	// Token: 0x06003996 RID: 14742 RVA: 0x0022CBFC File Offset: 0x0022ADFC
	public static void DrawBoundingBox([TupleElementNames(new string[]
	{
		"cornerA",
		"cornerB"
	})] ValueTuple<Vector2, Vector2> screenRect, string name, bool isFocused)
	{
		if (isFocused)
		{
			DevToolEntity.DrawScreenRect(screenRect, name, new Color(1f, 0f, 0f, 1f), new Color(1f, 0f, 0f, 0.3f), default(Option<DevToolUtil.TextAlignment>));
			return;
		}
		DevToolEntity.DrawScreenRect(screenRect, Option.None, new Color(0.9f, 0f, 0f, 0.6f), default(Option<Color>), default(Option<DevToolUtil.TextAlignment>));
	}

	// Token: 0x06003997 RID: 14743 RVA: 0x0022CCA0 File Offset: 0x0022AEA0
	public unsafe static void DrawScreenRect([TupleElementNames(new string[]
	{
		"cornerA",
		"cornerB"
	})] ValueTuple<Vector2, Vector2> screenRect, Option<string> text = default(Option<string>), Option<Color> outlineColor = default(Option<Color>), Option<Color> fillColor = default(Option<Color>), Option<DevToolUtil.TextAlignment> alignment = default(Option<DevToolUtil.TextAlignment>))
	{
		Vector2 vector = Vector2.Min(screenRect.Item1, screenRect.Item2);
		Vector2 vector2 = Vector2.Max(screenRect.Item1, screenRect.Item2);
		ImGui.GetBackgroundDrawList().AddRect(vector, vector2, ImGui.GetColorU32(outlineColor.UnwrapOr(Color.red, null)), 0f, ImDrawFlags.None, 4f);
		ImGui.GetBackgroundDrawList().AddRectFilled(vector, vector2, ImGui.GetColorU32(fillColor.UnwrapOr(Color.clear, null)));
		float font_size = 30f;
		if (text.IsSome())
		{
			Vector2 pos = new Vector2(vector2.x, vector.y) + new Vector2(15f, 0f);
			if (alignment.HasValue)
			{
				font_size = *ImGui.GetFont().FontSize;
				Vector2 vector3 = ImGui.CalcTextSize(text.Unwrap());
				if (alignment == DevToolUtil.TextAlignment.Center)
				{
					Vector2 vector4 = vector2 - vector;
					pos.x = vector.x + (vector4.x - vector3.x) * 0.5f;
					pos.y = vector.y + (vector4.y - vector3.y) * 0.5f;
				}
			}
			ImGui.GetBackgroundDrawList().AddText(ImGui.GetFont(), font_size, pos, ImGui.GetColorU32(Color.white), text.Unwrap());
		}
	}

	// Token: 0x040027C0 RID: 10176
	private Option<DevToolEntityTarget> currentTargetOpt;

	// Token: 0x040027C1 RID: 10177
	private bool shouldDrawBoundingBox = true;
}
