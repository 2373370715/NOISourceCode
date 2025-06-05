using System;
using UnityEngine;

// Token: 0x02000AD3 RID: 2771
[AddComponentMenu("KMonoBehaviour/scripts/NavPathDrawer")]
public class NavPathDrawer : KMonoBehaviour
{
	// Token: 0x17000212 RID: 530
	// (get) Token: 0x060032D5 RID: 13013 RVA: 0x000C5802 File Offset: 0x000C3A02
	// (set) Token: 0x060032D6 RID: 13014 RVA: 0x000C5809 File Offset: 0x000C3A09
	public static NavPathDrawer Instance { get; private set; }

	// Token: 0x060032D7 RID: 13015 RVA: 0x000C5811 File Offset: 0x000C3A11
	public static void DestroyInstance()
	{
		NavPathDrawer.Instance = null;
	}

	// Token: 0x060032D8 RID: 13016 RVA: 0x002123F8 File Offset: 0x002105F8
	protected override void OnPrefabInit()
	{
		Shader shader = Shader.Find("Lines/Colored Blended");
		this.material = new Material(shader);
		NavPathDrawer.Instance = this;
	}

	// Token: 0x060032D9 RID: 13017 RVA: 0x000C5811 File Offset: 0x000C3A11
	protected override void OnCleanUp()
	{
		NavPathDrawer.Instance = null;
	}

	// Token: 0x060032DA RID: 13018 RVA: 0x000C5819 File Offset: 0x000C3A19
	public void DrawPath(Vector3 navigator_pos, PathFinder.Path path)
	{
		this.navigatorPos = navigator_pos;
		this.navigatorPos.y = this.navigatorPos.y + 0.5f;
		this.path = path;
	}

	// Token: 0x060032DB RID: 13019 RVA: 0x000C5845 File Offset: 0x000C3A45
	public Navigator GetNavigator()
	{
		return this.navigator;
	}

	// Token: 0x060032DC RID: 13020 RVA: 0x000C584D File Offset: 0x000C3A4D
	public void SetNavigator(Navigator navigator)
	{
		this.navigator = navigator;
	}

	// Token: 0x060032DD RID: 13021 RVA: 0x000C5856 File Offset: 0x000C3A56
	public void ClearNavigator()
	{
		this.navigator = null;
	}

	// Token: 0x060032DE RID: 13022 RVA: 0x00212424 File Offset: 0x00210624
	private void DrawPath(PathFinder.Path path, Vector3 navigator_pos, Color color)
	{
		if (path.nodes != null && path.nodes.Count > 1)
		{
			GL.PushMatrix();
			this.material.SetPass(0);
			GL.Begin(1);
			GL.Color(color);
			GL.Vertex(navigator_pos);
			GL.Vertex(NavTypeHelper.GetNavPos(path.nodes[1].cell, path.nodes[1].navType));
			for (int i = 1; i < path.nodes.Count - 1; i++)
			{
				if ((int)Grid.WorldIdx[path.nodes[i].cell] == ClusterManager.Instance.activeWorldId && (int)Grid.WorldIdx[path.nodes[i + 1].cell] == ClusterManager.Instance.activeWorldId)
				{
					Vector3 navPos = NavTypeHelper.GetNavPos(path.nodes[i].cell, path.nodes[i].navType);
					Vector3 navPos2 = NavTypeHelper.GetNavPos(path.nodes[i + 1].cell, path.nodes[i + 1].navType);
					GL.Vertex(navPos);
					GL.Vertex(navPos2);
				}
			}
			GL.End();
			GL.PopMatrix();
		}
	}

	// Token: 0x060032DF RID: 13023 RVA: 0x00212570 File Offset: 0x00210770
	private void OnPostRender()
	{
		this.DrawPath(this.path, this.navigatorPos, Color.white);
		this.path = default(PathFinder.Path);
		this.DebugDrawSelectedNavigator();
		if (this.navigator != null)
		{
			GL.PushMatrix();
			this.material.SetPass(0);
			GL.Begin(1);
			PathFinderQuery query = PathFinderQueries.drawNavGridQuery.Reset(null);
			this.navigator.RunQuery(query);
			GL.End();
			GL.PopMatrix();
		}
	}

	// Token: 0x060032E0 RID: 13024 RVA: 0x002125F0 File Offset: 0x002107F0
	private void DebugDrawSelectedNavigator()
	{
		if (!DebugHandler.DebugPathFinding)
		{
			return;
		}
		if (SelectTool.Instance == null)
		{
			return;
		}
		if (SelectTool.Instance.selected == null)
		{
			return;
		}
		Navigator component = SelectTool.Instance.selected.GetComponent<Navigator>();
		if (component == null)
		{
			return;
		}
		int mouseCell = DebugHandler.GetMouseCell();
		if (Grid.IsValidCell(mouseCell))
		{
			PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(Grid.PosToCell(component), component.CurrentNavType, component.flags);
			PathFinder.Path path = default(PathFinder.Path);
			PathFinder.UpdatePath(component.NavGrid, component.GetCurrentAbilities(), potential_path, PathFinderQueries.cellQuery.Reset(mouseCell), ref path);
			string text = "";
			text = text + "Source: " + Grid.PosToCell(component).ToString() + "\n";
			text = text + "Dest: " + mouseCell.ToString() + "\n";
			text = text + "Cost: " + path.cost.ToString();
			this.DrawPath(path, component.GetComponent<KAnimControllerBase>().GetPivotSymbolPosition(), Color.green);
			DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
		}
	}

	// Token: 0x040022BD RID: 8893
	private PathFinder.Path path;

	// Token: 0x040022BE RID: 8894
	public Material material;

	// Token: 0x040022BF RID: 8895
	private Vector3 navigatorPos;

	// Token: 0x040022C0 RID: 8896
	private Navigator navigator;
}
