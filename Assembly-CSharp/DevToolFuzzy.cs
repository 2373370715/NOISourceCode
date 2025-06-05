using System;
using System.Collections.Generic;
using System.Text;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000BEB RID: 3051
public class DevToolFuzzy : DevTool
{
	// Token: 0x060039D0 RID: 14800 RVA: 0x000C9E08 File Offset: 0x000C8008
	public DevToolFuzzy()
	{
		this.mostRecentEditTime = Time.unscaledTime;
	}

	// Token: 0x060039D1 RID: 14801 RVA: 0x0022E140 File Offset: 0x0022C340
	private void RecipesUi(StringBuilder sb, string id, List<SearchUtil.NameDescCache> recipes)
	{
		int num = 0;
		foreach (SearchUtil.NameDescCache nameDescCache in recipes)
		{
			if (nameDescCache.Score > num)
			{
				num = nameDescCache.Score;
			}
		}
		if (!this.IsPassingScore(num))
		{
			return;
		}
		sb.Clear();
		sb.AppendFormat("[{0}] Recipes##{1}", num, id);
		if (ImGui.CollapsingHeader(sb.ToString()))
		{
			ImGui.Indent();
			foreach (SearchUtil.NameDescCache nameDescCache2 in recipes)
			{
				if (this.IsPassingScore(nameDescCache2.Score))
				{
					sb.Clear();
					sb.AppendFormat("{0}##{1}", DevToolFuzzy.FormatScoreDisplay(nameDescCache2.Score, nameDescCache2.name.text), id);
					if (ImGui.CollapsingHeader(sb.ToString()))
					{
						this.DisplayIfScorePasses(nameDescCache2);
					}
				}
			}
			ImGui.Unindent();
		}
	}

	// Token: 0x060039D2 RID: 14802 RVA: 0x0022E25C File Offset: 0x0022C45C
	private void TechItemUi(StringBuilder sb, string id, SearchUtil.TechItemCache techItem, SearchUtil.TechCache parentTech = null)
	{
		if (!this.IsPassingScore(techItem.Score))
		{
			return;
		}
		sb.Clear();
		sb.AppendFormat("{0}##TechItem{1}", DevToolFuzzy.FormatScoreDisplay(techItem.Score, techItem.nameDescSearchTerms.nameDesc.name.text), id);
		string text = sb.ToString();
		if (ImGui.CollapsingHeader(text))
		{
			ImGui.Indent();
			if (parentTech != null)
			{
				ImGui.LabelText("Parent Tech", parentTech.tech.nameDesc.name.text);
			}
			this.DisplayIfScorePasses(techItem.nameDescSearchTerms);
			this.RecipesUi(sb, text, techItem.recipes);
			ImGui.Unindent();
		}
	}

	// Token: 0x060039D3 RID: 14803 RVA: 0x0022E304 File Offset: 0x0022C504
	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.InputText("Search Text", ref this.searchText, 30U))
		{
			this.refresh = true;
			this.mostRecentEditTime = Time.unscaledTime;
		}
		if (this.refresh && Time.unscaledTime - this.mostRecentEditTime > 0.4f)
		{
			this.Refresh();
			this.refresh = false;
		}
		ImGui.DragInt("Score Threshold", ref this.scoreThreshold, 0.5f, 0, 100);
		StringBuilder stringBuilder = new StringBuilder();
		if (ImGui.CollapsingHeader("Techs"))
		{
			ImGui.Indent();
			foreach (SearchUtil.TechCache techCache in this.techs)
			{
				if (this.IsPassingScore(techCache.Score))
				{
					stringBuilder.Clear();
					stringBuilder.AppendFormat("{0}##Tech", DevToolFuzzy.FormatScoreDisplay(techCache.Score, techCache.tech.nameDesc.name.text));
					string text = stringBuilder.ToString();
					if (ImGui.CollapsingHeader(text))
					{
						ImGui.Indent();
						this.DisplayIfScorePasses(techCache.tech);
						foreach (KeyValuePair<string, SearchUtil.TechItemCache> keyValuePair in techCache.techItems)
						{
							this.TechItemUi(stringBuilder, text, keyValuePair.Value, null);
						}
						ImGui.Unindent();
					}
				}
			}
			ImGui.Unindent();
		}
		if (ImGui.CollapsingHeader("TechItems"))
		{
			ImGui.Indent();
			foreach (SearchUtil.TechCache techCache2 in this.techs)
			{
				foreach (KeyValuePair<string, SearchUtil.TechItemCache> keyValuePair2 in techCache2.techItems)
				{
					this.TechItemUi(stringBuilder, "TechItem", keyValuePair2.Value, techCache2);
				}
			}
			ImGui.Unindent();
		}
		if (ImGui.CollapsingHeader("BuildingDefs"))
		{
			ImGui.Indent();
			foreach (SearchUtil.BuildingDefCache buildingDefCache in this.buildingDefs)
			{
				if (this.IsPassingScore(buildingDefCache.Score))
				{
					stringBuilder.Clear();
					stringBuilder.AppendFormat("{0}##BuildingDef", DevToolFuzzy.FormatScoreDisplay(buildingDefCache.Score, buildingDefCache.nameDescSearchTerms.nameDesc.name.text));
					string text2 = stringBuilder.ToString();
					if (ImGui.CollapsingHeader(text2))
					{
						ImGui.Indent();
						this.DisplayIfScorePasses(buildingDefCache.nameDescSearchTerms);
						this.DisplayIfScorePasses("Effect", buildingDefCache.effect);
						this.RecipesUi(stringBuilder, text2, buildingDefCache.recipes);
						ImGui.Unindent();
					}
				}
			}
			ImGui.Unindent();
		}
	}

	// Token: 0x060039D4 RID: 14804 RVA: 0x0022E624 File Offset: 0x0022C824
	private void Refresh()
	{
		string text = this.searchText.ToUpper().Trim();
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (this.techs.Count == 0)
		{
			foreach (KeyValuePair<string, SearchUtil.TechCache> keyValuePair in SearchUtil.CacheTechs())
			{
				this.techs.Add(keyValuePair.Value);
			}
		}
		foreach (SearchUtil.TechCache techCache in this.techs)
		{
			techCache.Bind(text);
		}
		this.techs.Sort();
		if (this.buildingDefs.Count == 0)
		{
			foreach (BuildingDef def in Assets.BuildingDefs)
			{
				this.buildingDefs.Add(SearchUtil.MakeBuildingDefCache(def));
			}
		}
		foreach (SearchUtil.BuildingDefCache buildingDefCache in this.buildingDefs)
		{
			buildingDefCache.Bind(text);
		}
		this.buildingDefs.Sort();
	}

	// Token: 0x060039D5 RID: 14805 RVA: 0x000C9E44 File Offset: 0x000C8044
	private bool IsPassingScore(int score)
	{
		return score >= this.scoreThreshold;
	}

	// Token: 0x060039D6 RID: 14806 RVA: 0x000C9E52 File Offset: 0x000C8052
	private static string FormatScoreDisplay(int score, string text)
	{
		return string.Format("[{0}] {1}", score, FuzzySearch.Canonicalize(text));
	}

	// Token: 0x060039D7 RID: 14807 RVA: 0x0022E79C File Offset: 0x0022C99C
	private static void DisplayScore(int score, string label, string token, string text)
	{
		ImGui.Text(string.Format("[{0}]", score));
		ImGui.SameLine();
		ImGui.Text(label);
		ImGui.SameLine();
		ImGui.Text(string.Format("({0})", token));
		ImGui.SameLine();
		ImGui.TextWrapped(text);
	}

	// Token: 0x060039D8 RID: 14808 RVA: 0x000C9E6A File Offset: 0x000C806A
	private static void DisplayScore(string label, SearchUtil.MatchCache match)
	{
		DevToolFuzzy.DisplayScore(match.Score, label, match.FuzzyMatch.token, match.text);
	}

	// Token: 0x060039D9 RID: 14809 RVA: 0x000C9E89 File Offset: 0x000C8089
	private void DisplayIfScorePasses(string label, SearchUtil.MatchCache match)
	{
		if (this.IsPassingScore(match.Score))
		{
			DevToolFuzzy.DisplayScore(label, match);
		}
	}

	// Token: 0x060039DA RID: 14810 RVA: 0x000C9EA0 File Offset: 0x000C80A0
	private void DisplayIfScorePasses(SearchUtil.NameDescCache nameDesc)
	{
		this.DisplayIfScorePasses("Name", nameDesc.name);
		this.DisplayIfScorePasses("Desc", nameDesc.desc);
	}

	// Token: 0x060039DB RID: 14811 RVA: 0x0022E7EC File Offset: 0x0022C9EC
	private void DisplayIfScorePasses(SearchUtil.NameDescSearchTermsCache nameDescSearchTerms)
	{
		this.DisplayIfScorePasses(nameDescSearchTerms.nameDesc);
		if (this.IsPassingScore(nameDescSearchTerms.SearchTermsScore.score))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendJoin<string>(", ", nameDescSearchTerms.searchTerms);
			DevToolFuzzy.DisplayScore(nameDescSearchTerms.SearchTermsScore.score, "SearchTerms", nameDescSearchTerms.SearchTermsScore.token, stringBuilder.ToString());
		}
	}

	// Token: 0x040027DF RID: 10207
	private string searchText = "";

	// Token: 0x040027E0 RID: 10208
	private float mostRecentEditTime;

	// Token: 0x040027E1 RID: 10209
	private bool refresh;

	// Token: 0x040027E2 RID: 10210
	private const float REFRESH_DELAY = 0.4f;

	// Token: 0x040027E3 RID: 10211
	private int scoreThreshold = 79;

	// Token: 0x040027E4 RID: 10212
	private readonly List<SearchUtil.TechCache> techs = new List<SearchUtil.TechCache>();

	// Token: 0x040027E5 RID: 10213
	private readonly List<SearchUtil.BuildingDefCache> buildingDefs = new List<SearchUtil.BuildingDefCache>();
}
