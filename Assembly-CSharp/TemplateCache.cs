using System;
using System.Collections.Generic;
using Klei;
using ProcGen;

// Token: 0x0200124B RID: 4683
public static class TemplateCache
{
	// Token: 0x170005B1 RID: 1457
	// (get) Token: 0x06005F4D RID: 24397 RVA: 0x000E2AC5 File Offset: 0x000E0CC5
	// (set) Token: 0x06005F4E RID: 24398 RVA: 0x000E2ACC File Offset: 0x000E0CCC
	public static bool Initted { get; private set; }

	// Token: 0x06005F4F RID: 24399 RVA: 0x000E2AD4 File Offset: 0x000E0CD4
	public static void Init()
	{
		if (TemplateCache.Initted)
		{
			return;
		}
		TemplateCache.templates = new Dictionary<string, TemplateContainer>();
		TemplateCache.Initted = true;
	}

	// Token: 0x06005F50 RID: 24400 RVA: 0x000E2AEE File Offset: 0x000E0CEE
	public static void Clear()
	{
		TemplateCache.templates = null;
		TemplateCache.Initted = false;
	}

	// Token: 0x06005F51 RID: 24401 RVA: 0x002B4C50 File Offset: 0x002B2E50
	public static string RewriteTemplatePath(string scopePath)
	{
		string dlcId;
		string str;
		SettingsCache.GetDlcIdAndPath(scopePath, out dlcId, out str);
		return SettingsCache.GetAbsoluteContentPath(dlcId, "templates/" + str);
	}

	// Token: 0x06005F52 RID: 24402 RVA: 0x000E2AFC File Offset: 0x000E0CFC
	public static string RewriteTemplateYaml(string scopePath)
	{
		return TemplateCache.RewriteTemplatePath(scopePath) + ".yaml";
	}

	// Token: 0x06005F53 RID: 24403 RVA: 0x002B4C78 File Offset: 0x002B2E78
	public static TemplateContainer GetTemplate(string templatePath)
	{
		if (!TemplateCache.templates.ContainsKey(templatePath))
		{
			TemplateCache.templates.Add(templatePath, null);
		}
		if (TemplateCache.templates[templatePath] == null)
		{
			string text = TemplateCache.RewriteTemplateYaml(templatePath);
			TemplateContainer templateContainer = YamlIO.LoadFile<TemplateContainer>(text, null, null);
			if (templateContainer == null)
			{
				Debug.LogWarning("Missing template [" + text + "]");
			}
			templateContainer.name = templatePath;
			TemplateCache.templates[templatePath] = templateContainer;
		}
		return TemplateCache.templates[templatePath];
	}

	// Token: 0x06005F54 RID: 24404 RVA: 0x000E2B0E File Offset: 0x000E0D0E
	public static bool TemplateExists(string templatePath)
	{
		return FileSystem.FileExists(TemplateCache.RewriteTemplateYaml(templatePath));
	}

	// Token: 0x04004411 RID: 17425
	private const string defaultAssetFolder = "bases";

	// Token: 0x04004412 RID: 17426
	private static Dictionary<string, TemplateContainer> templates;
}
