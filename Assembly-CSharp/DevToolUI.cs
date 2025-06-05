using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000C13 RID: 3091
public class DevToolUI : DevTool
{
	// Token: 0x06003A95 RID: 14997 RVA: 0x000CA4E2 File Offset: 0x000C86E2
	protected override void RenderTo(DevPanel panel)
	{
		this.RepopulateRaycastHits();
		this.DrawPingObject();
		this.DrawRaycastHits();
	}

	// Token: 0x06003A96 RID: 14998 RVA: 0x002358FC File Offset: 0x00233AFC
	private void DrawPingObject()
	{
		if (this.m_last_pinged_hit != null)
		{
			GameObject gameObject = this.m_last_pinged_hit.Value.gameObject;
			if (gameObject != null && gameObject)
			{
				ImGui.Text("Last Pinged: \"" + DevToolUI.GetQualifiedName(gameObject) + "\"");
				ImGui.SameLine();
				if (ImGui.Button("Inspect"))
				{
					DevToolSceneInspector.Inspect(gameObject);
				}
				ImGui.Spacing();
				ImGui.Spacing();
			}
			else
			{
				this.m_last_pinged_hit = null;
			}
		}
		ImGui.Text("Press \",\" to ping the top hovered ui object");
		ImGui.Spacing();
		ImGui.Spacing();
	}

	// Token: 0x06003A97 RID: 14999 RVA: 0x000CA4F6 File Offset: 0x000C86F6
	private void Internal_Ping(RaycastResult raycastResult)
	{
		GameObject gameObject = raycastResult.gameObject;
		this.m_last_pinged_hit = new RaycastResult?(raycastResult);
	}

	// Token: 0x06003A98 RID: 15000 RVA: 0x0023599C File Offset: 0x00233B9C
	public static void PingHoveredObject()
	{
		using (ListPool<RaycastResult, DevToolUI>.PooledList pooledList = PoolsFor<DevToolUI>.AllocateList<RaycastResult>())
		{
			UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
			if (!(current == null) && current)
			{
				current.RaycastAll(new PointerEventData(current)
				{
					position = Input.mousePosition
				}, pooledList);
				DevToolUI devToolUI = DevToolManager.Instance.panels.AddOrGetDevTool<DevToolUI>();
				if (pooledList.Count > 0)
				{
					devToolUI.Internal_Ping(pooledList[0]);
				}
			}
		}
	}

	// Token: 0x06003A99 RID: 15001 RVA: 0x00235A28 File Offset: 0x00233C28
	private void DrawRaycastHits()
	{
		if (this.m_raycast_hits.Count <= 0)
		{
			ImGui.Text("Didn't hit any ui");
			return;
		}
		ImGui.Text("Raycast Hits:");
		ImGui.Indent();
		for (int i = 0; i < this.m_raycast_hits.Count; i++)
		{
			RaycastResult raycastResult = this.m_raycast_hits[i];
			ImGui.BulletText(string.Format("[{0}] {1}", i, DevToolUI.GetQualifiedName(raycastResult.gameObject)));
		}
		ImGui.Unindent();
	}

	// Token: 0x06003A9A RID: 15002 RVA: 0x00235AA8 File Offset: 0x00233CA8
	private void RepopulateRaycastHits()
	{
		this.m_raycast_hits.Clear();
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current == null || !current)
		{
			return;
		}
		current.RaycastAll(new PointerEventData(current)
		{
			position = Input.mousePosition
		}, this.m_raycast_hits);
	}

	// Token: 0x06003A9B RID: 15003 RVA: 0x00235AFC File Offset: 0x00233CFC
	private static string GetQualifiedName(GameObject game_object)
	{
		KScreen componentInParent = game_object.GetComponentInParent<KScreen>();
		if (componentInParent != null)
		{
			return componentInParent.gameObject.name + " :: " + game_object.name;
		}
		return game_object.name ?? "";
	}

	// Token: 0x04002890 RID: 10384
	private List<RaycastResult> m_raycast_hits = new List<RaycastResult>();

	// Token: 0x04002891 RID: 10385
	private RaycastResult? m_last_pinged_hit;
}
