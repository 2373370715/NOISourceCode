using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Klei;
using Newtonsoft.Json;
using UnityEngine;

namespace KMod
{
	// Token: 0x02002250 RID: 8784
	[JsonObject(MemberSerialization.OptIn)]
	[DebuggerDisplay("{title}")]
	public class Mod : IHasDlcRestrictions
	{
		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x0600BA70 RID: 47728 RVA: 0x0011C963 File Offset: 0x0011AB63
		// (set) Token: 0x0600BA71 RID: 47729 RVA: 0x0011C96B File Offset: 0x0011AB6B
		public Content available_content { get; private set; }

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x0600BA72 RID: 47730 RVA: 0x0011C974 File Offset: 0x0011AB74
		// (set) Token: 0x0600BA73 RID: 47731 RVA: 0x0011C97C File Offset: 0x0011AB7C
		[JsonProperty]
		public string staticID { get; private set; }

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x0600BA74 RID: 47732 RVA: 0x0011C985 File Offset: 0x0011AB85
		// (set) Token: 0x0600BA75 RID: 47733 RVA: 0x0011C98D File Offset: 0x0011AB8D
		public LocString manage_tooltip { get; private set; }

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x0600BA76 RID: 47734 RVA: 0x0011C996 File Offset: 0x0011AB96
		// (set) Token: 0x0600BA77 RID: 47735 RVA: 0x0011C99E File Offset: 0x0011AB9E
		public System.Action on_managed { get; private set; }

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x0600BA78 RID: 47736 RVA: 0x0011C9A7 File Offset: 0x0011ABA7
		public bool is_managed
		{
			get
			{
				return this.manage_tooltip != null;
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x0600BA79 RID: 47737 RVA: 0x0011C9B2 File Offset: 0x0011ABB2
		// (set) Token: 0x0600BA7A RID: 47738 RVA: 0x0011C9BF File Offset: 0x0011ABBF
		public string title
		{
			get
			{
				return this.label.title;
			}
			set
			{
				this.label.title = value;
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x0600BA7B RID: 47739 RVA: 0x0011C9CD File Offset: 0x0011ABCD
		// (set) Token: 0x0600BA7C RID: 47740 RVA: 0x0011C9D5 File Offset: 0x0011ABD5
		public string description { get; set; }

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x0600BA7D RID: 47741 RVA: 0x0011C9DE File Offset: 0x0011ABDE
		// (set) Token: 0x0600BA7E RID: 47742 RVA: 0x0011C9E6 File Offset: 0x0011ABE6
		public Content loaded_content { get; private set; }

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x0600BA7F RID: 47743 RVA: 0x0011C9EF File Offset: 0x0011ABEF
		// (set) Token: 0x0600BA80 RID: 47744 RVA: 0x0011C9F7 File Offset: 0x0011ABF7
		public IFileSource file_source
		{
			get
			{
				return this._fileSource;
			}
			set
			{
				if (this._fileSource != null)
				{
					this._fileSource.Dispose();
				}
				this._fileSource = value;
			}
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x0600BA81 RID: 47745 RVA: 0x0011CA13 File Offset: 0x0011AC13
		// (set) Token: 0x0600BA82 RID: 47746 RVA: 0x0011CA1B File Offset: 0x0011AC1B
		public bool DevModCrashTriggered { get; private set; }

		// Token: 0x0600BA83 RID: 47747 RVA: 0x0011CA24 File Offset: 0x0011AC24
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600BA84 RID: 47748 RVA: 0x0011CA2C File Offset: 0x0011AC2C
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x0600BA85 RID: 47749 RVA: 0x0011CA34 File Offset: 0x0011AC34
		[JsonConstructor]
		public Mod()
		{
		}

		// Token: 0x0600BA86 RID: 47750 RVA: 0x0047EF68 File Offset: 0x0047D168
		public void CopyPersistentDataTo(Mod other_mod)
		{
			other_mod.status = this.status;
			other_mod.enabledForDlc = ((this.enabledForDlc != null) ? new List<string>(this.enabledForDlc) : new List<string>());
			other_mod.crash_count = this.crash_count;
			other_mod.loaded_content = this.loaded_content;
			other_mod.loaded_mod_data = this.loaded_mod_data;
			other_mod.reinstall_path = this.reinstall_path;
		}

		// Token: 0x0600BA87 RID: 47751 RVA: 0x0047EFD4 File Offset: 0x0047D1D4
		public Mod(Label label, string staticID, string description, IFileSource file_source, LocString manage_tooltip, System.Action on_managed)
		{
			this.label = label;
			this.status = Mod.Status.NotInstalled;
			this.staticID = staticID;
			this.description = description;
			this.file_source = file_source;
			this.manage_tooltip = manage_tooltip;
			this.on_managed = on_managed;
			this.loaded_content = (Content)0;
			this.available_content = (Content)0;
			this.ScanContent();
		}

		// Token: 0x0600BA88 RID: 47752 RVA: 0x0011CA47 File Offset: 0x0011AC47
		public bool IsEnabledForActiveDlc()
		{
			return this.IsEnabledForDlc(DlcManager.GetHighestActiveDlcId());
		}

		// Token: 0x0600BA89 RID: 47753 RVA: 0x0011CA54 File Offset: 0x0011AC54
		public bool IsEnabledForDlc(string dlcId)
		{
			return this.enabledForDlc != null && this.enabledForDlc.Contains(dlcId);
		}

		// Token: 0x0600BA8A RID: 47754 RVA: 0x0011CA6C File Offset: 0x0011AC6C
		public void SetEnabledForActiveDlc(bool enabled)
		{
			this.SetEnabledForDlc(DlcManager.GetHighestActiveDlcId(), enabled);
		}

		// Token: 0x0600BA8B RID: 47755 RVA: 0x0047F03C File Offset: 0x0047D23C
		public void SetEnabledForDlc(string dlcId, bool set_enabled)
		{
			if (this.enabledForDlc == null)
			{
				this.enabledForDlc = new List<string>();
			}
			bool flag = this.enabledForDlc.Contains(dlcId);
			if (set_enabled && !flag)
			{
				this.enabledForDlc.Add(dlcId);
				return;
			}
			if (!set_enabled && flag)
			{
				this.enabledForDlc.Remove(dlcId);
			}
		}

		// Token: 0x0600BA8C RID: 47756 RVA: 0x0047F094 File Offset: 0x0047D294
		public void ScanContent()
		{
			this.ModDevLog(string.Format("{0} ({1}): Setting up mod.", this.label, this.label.id));
			this.available_content = (Content)0;
			if (this.file_source == null)
			{
				if (this.label.id.EndsWith(".zip"))
				{
					DebugUtil.DevAssert(false, "Does this actually get used ever?", null);
					this.file_source = new ZipFile(this.label.install_path);
				}
				else
				{
					this.file_source = new Directory(this.label.install_path);
				}
			}
			if (!this.file_source.Exists())
			{
				global::Debug.LogWarning(string.Format("{0}: File source does not appear to be valid, skipping. ({1})", this.label, this.label.install_path));
				return;
			}
			KModHeader header = KModUtil.GetHeader(this.file_source, this.label.defaultStaticID, this.label.title, this.description, this.IsDev);
			if (this.label.title != header.title)
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"\t",
					this.label.title,
					" has a mod.yaml with the title `",
					header.title,
					"`, using that from now on."
				}));
			}
			if (this.label.defaultStaticID != header.staticID)
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"\t",
					this.label.title,
					" has a mod.yaml with a staticID `",
					header.staticID,
					"`, using that from now on."
				}));
			}
			this.label.title = header.title;
			this.staticID = header.staticID;
			this.description = header.description;
			Mod.ArchivedVersion mostSuitableArchive = this.GetMostSuitableArchive();
			if (mostSuitableArchive == null)
			{
				global::Debug.LogWarning(string.Format("{0}: No archive supports this game version, skipping content.", this.label));
				this.contentCompatability = ModContentCompatability.DoesntSupportDLCConfig;
				this.available_content = (Content)0;
				this.SetEnabledForActiveDlc(false);
				return;
			}
			this.packagedModInfo = mostSuitableArchive.info;
			Content content;
			this.ScanContentFromSource(mostSuitableArchive.relativePath, out content);
			if (content == (Content)0)
			{
				global::Debug.LogWarning(string.Format("{0}: No supported content for mod, skipping content.", this.label));
				this.contentCompatability = ModContentCompatability.NoContent;
				this.available_content = (Content)0;
				this.SetEnabledForActiveDlc(false);
				return;
			}
			bool flag = mostSuitableArchive.info.APIVersion == 2;
			if ((content & Content.DLL) != (Content)0 && !flag)
			{
				global::Debug.LogWarning(string.Format("{0}: DLLs found but not using the correct API version.", this.label));
				this.contentCompatability = ModContentCompatability.OldAPI;
				this.available_content = (Content)0;
				this.SetEnabledForActiveDlc(false);
				return;
			}
			this.contentCompatability = ModContentCompatability.OK;
			this.available_content = content;
			this.relative_root = mostSuitableArchive.relativePath;
			global::Debug.Assert(this.content_source == null);
			this.content_source = new Directory(this.ContentPath);
			string arg = string.IsNullOrEmpty(this.relative_root) ? "root" : this.relative_root;
			global::Debug.Log(string.Format("{0}: Successfully loaded from path '{1}' with content '{2}'.", this.label, arg, this.available_content.ToString()));
		}

		// Token: 0x0600BA8D RID: 47757 RVA: 0x0047F3C4 File Offset: 0x0047D5C4
		private Mod.ArchivedVersion GetMostSuitableArchive()
		{
			Mod.PackagedModInfo packagedModInfo = this.GetModInfoForFolder("");
			if (packagedModInfo == null)
			{
				if (!this.ScanContentFromSourceForTranslationsOnly(""))
				{
					global::Debug.Log(string.Format("{0}: Is missing a mod_info.yaml file and will not be loaded, which is required. See the stickied post in the Mods and Tools section on the Klei forums.", this.label));
					return null;
				}
				this.ModDevLogWarning(string.Format("{0}: No mod_info.yaml found, but since it's a translation we will load it.", this.label));
				packagedModInfo = new Mod.PackagedModInfo
				{
					minimumSupportedBuild = 0
				};
			}
			this.requiredDlcIds = packagedModInfo.requiredDlcIds;
			this.forbiddenDlcIds = packagedModInfo.forbiddenDlcIds;
			Mod.ArchivedVersion archivedVersion = new Mod.ArchivedVersion
			{
				relativePath = "",
				info = packagedModInfo
			};
			if (!this.file_source.Exists("archived_versions"))
			{
				this.ModDevLog(string.Format("\t{0}: No archived_versions for this mod, using root version directly.", this.label));
				if (!DlcManager.IsCorrectDlcSubscribed(packagedModInfo))
				{
					return null;
				}
				return archivedVersion;
			}
			else
			{
				List<FileSystemItem> list = new List<FileSystemItem>();
				this.file_source.GetTopLevelItems(list, "archived_versions");
				if (list.Count == 0)
				{
					this.ModDevLog(string.Format("\t{0}: No archived_versions for this mod, using root version directly.", this.label));
					if (!DlcManager.IsCorrectDlcSubscribed(packagedModInfo))
					{
						return null;
					}
					return archivedVersion;
				}
				else
				{
					List<Mod.ArchivedVersion> list2 = new List<Mod.ArchivedVersion>();
					list2.Add(archivedVersion);
					foreach (FileSystemItem fileSystemItem in list)
					{
						if (fileSystemItem.type != FileSystemItem.ItemType.File)
						{
							string relativePath = Path.Combine("archived_versions", fileSystemItem.name);
							Mod.PackagedModInfo modInfoForFolder = this.GetModInfoForFolder(relativePath);
							if (modInfoForFolder != null)
							{
								list2.Add(new Mod.ArchivedVersion
								{
									relativePath = relativePath,
									info = modInfoForFolder
								});
							}
						}
					}
					list2 = (from v in list2
					where DlcManager.IsCorrectDlcSubscribed(v.info)
					select v).ToList<Mod.ArchivedVersion>();
					list2 = (from v in list2
					where v.info.APIVersion == 2 || v.info.APIVersion == 0
					select v).ToList<Mod.ArchivedVersion>();
					Mod.ArchivedVersion archivedVersion2 = (from v in list2
					where (long)v.info.minimumSupportedBuild <= 663500L
					orderby v.info.minimumSupportedBuild descending
					select v).FirstOrDefault<Mod.ArchivedVersion>();
					if (archivedVersion2 != null)
					{
						this.requiredDlcIds = archivedVersion2.info.requiredDlcIds;
						this.forbiddenDlcIds = archivedVersion2.info.forbiddenDlcIds;
					}
					if (archivedVersion2 == null)
					{
						return null;
					}
					return archivedVersion2;
				}
			}
		}

		// Token: 0x0600BA8E RID: 47758 RVA: 0x0047F648 File Offset: 0x0047D848
		private Mod.PackagedModInfo GetModInfoForFolder(string relative_root)
		{
			List<FileSystemItem> list = new List<FileSystemItem>();
			this.file_source.GetTopLevelItems(list, relative_root);
			bool flag = false;
			foreach (FileSystemItem fileSystemItem in list)
			{
				if (fileSystemItem.type == FileSystemItem.ItemType.File && fileSystemItem.name.ToLower() == "mod_info.yaml")
				{
					flag = true;
					break;
				}
			}
			string text = string.IsNullOrEmpty(relative_root) ? "root" : relative_root;
			if (!flag)
			{
				this.ModDevLogWarning(string.Concat(new string[]
				{
					"\t",
					this.title,
					": has no mod_info.yaml in folder '",
					text,
					"'"
				}));
				return null;
			}
			string text2 = this.file_source.Read(Path.Combine(relative_root, "mod_info.yaml"));
			if (string.IsNullOrEmpty(text2))
			{
				this.ModDevLogError(string.Format("\t{0}: Failed to read {1} in folder '{2}', skipping", this.label, "mod_info.yaml", text));
				return null;
			}
			YamlIO.ErrorHandler handle_error = delegate(YamlIO.Error e, bool force_warning)
			{
				YamlIO.LogError(e, !this.IsDev);
			};
			Mod.PackagedModInfo packagedModInfo = YamlIO.Parse<Mod.PackagedModInfo>(text2, default(FileHandle), handle_error, null);
			if (packagedModInfo == null)
			{
				this.ModDevLogError(string.Format("\t{0}: Failed to parse {1} in folder '{2}', text is {3}", new object[]
				{
					this.label,
					"mod_info.yaml",
					text,
					text2
				}));
				return null;
			}
			if (packagedModInfo.supportedContent != null && packagedModInfo.requiredDlcIds == null && packagedModInfo.forbiddenDlcIds == null)
			{
				packagedModInfo.supportedContent = packagedModInfo.supportedContent.ToUpperInvariant();
				this.ModDevLogWarning(string.Format("\t{0}: {1} in folder '{2}' is using supportedContent which has been deprecated. See stickied post on the Klei forums.", this.label, "mod_info.yaml", text));
				bool flag2 = packagedModInfo.supportedContent.Contains("ALL");
				bool flag3 = packagedModInfo.supportedContent.Contains("VANILLA_ID");
				bool flag4 = packagedModInfo.supportedContent.Contains("EXPANSION1_ID");
				if (flag2)
				{
					packagedModInfo.requiredDlcIds = null;
					packagedModInfo.forbiddenDlcIds = null;
				}
				else
				{
					string pattern = "\\b\\w+_ID\\b";
					List<string> list2 = new List<string>();
					foreach (object obj in Regex.Matches(packagedModInfo.supportedContent, pattern))
					{
						Match match = (Match)obj;
						if (!(match.Value == "VANILLA_ID") && (!(match.Value == "EXPANSION1_ID") || !flag3))
						{
							if (match.Value != "EXPANSION1_ID")
							{
								this.ModDevLogWarning(string.Format("\t{0}: {1} in folder '{2}' found a DLC '{3}' it didn't recognize, ignoring.", new object[]
								{
									this.label,
									"mod_info.yaml",
									text,
									match.Value
								}));
							}
							else
							{
								list2.Add(match.Value);
							}
						}
					}
					if (list2.Count > 0)
					{
						packagedModInfo.requiredDlcIds = list2.ToArray();
					}
					if (!flag4)
					{
						packagedModInfo.forbiddenDlcIds = DlcManager.EXPANSION1;
					}
				}
			}
			if (packagedModInfo.requiredDlcIds != null)
			{
				for (int i = 0; i < packagedModInfo.requiredDlcIds.Length; i++)
				{
					packagedModInfo.requiredDlcIds[i] = packagedModInfo.requiredDlcIds[i].ToUpperInvariant();
					if (!DlcManager.IsDlcId(packagedModInfo.requiredDlcIds[i]))
					{
						this.ModDevLogWarning(string.Format("\t{0}: {1} in folder '{2}' is using an unrecognized DLC in requiredDlcIds '{3}'", new object[]
						{
							this.label,
							"mod_info.yaml",
							text,
							packagedModInfo.requiredDlcIds[i]
						}));
					}
				}
			}
			if (packagedModInfo.forbiddenDlcIds != null)
			{
				for (int j = 0; j < packagedModInfo.forbiddenDlcIds.Length; j++)
				{
					packagedModInfo.forbiddenDlcIds[j] = packagedModInfo.forbiddenDlcIds[j].ToUpperInvariant();
					if (!DlcManager.IsDlcId(packagedModInfo.forbiddenDlcIds[j]))
					{
						this.ModDevLogWarning(string.Format("\t{0}: {1} in folder '{2}' is using an unrecognized DLC in forbiddenDlcIds '{3}'", new object[]
						{
							this.label,
							"mod_info.yaml",
							text,
							packagedModInfo.forbiddenDlcIds[j]
						}));
					}
				}
			}
			if (packagedModInfo.lastWorkingBuild != 0)
			{
				this.ModDevLogError(string.Format("\t{0}: {1} in folder '{2}' is using `{3}`, please upgrade this to `{4}`", new object[]
				{
					this.label,
					"mod_info.yaml",
					text,
					"lastWorkingBuild",
					"minimumSupportedBuild"
				}));
				if (packagedModInfo.minimumSupportedBuild == 0)
				{
					packagedModInfo.minimumSupportedBuild = packagedModInfo.lastWorkingBuild;
				}
			}
			this.ModDevLog(string.Format("\t{0}: Found valid mod_info.yaml in folder '{1}': requiredDlcIds='{2}', forbiddenDlcIds='{3}' at {4}", new object[]
			{
				this.label,
				text,
				packagedModInfo.requiredDlcIds.DebugToCommaSeparatedList(),
				packagedModInfo.forbiddenDlcIds.DebugToCommaSeparatedList(),
				packagedModInfo.minimumSupportedBuild
			}));
			return packagedModInfo;
		}

		// Token: 0x0600BA8F RID: 47759 RVA: 0x0047FB38 File Offset: 0x0047DD38
		private bool ScanContentFromSource(string relativeRoot, out Content available)
		{
			available = (Content)0;
			List<FileSystemItem> list = new List<FileSystemItem>();
			this.file_source.GetTopLevelItems(list, relativeRoot);
			foreach (FileSystemItem fileSystemItem in list)
			{
				if (fileSystemItem.type == FileSystemItem.ItemType.Directory)
				{
					string directory = fileSystemItem.name.ToLower();
					available |= this.AddDirectory(directory);
				}
				else
				{
					string file = fileSystemItem.name.ToLower();
					available |= this.AddFile(file);
				}
			}
			return available > (Content)0;
		}

		// Token: 0x0600BA90 RID: 47760 RVA: 0x0047FBD8 File Offset: 0x0047DDD8
		private bool ScanContentFromSourceForTranslationsOnly(string relativeRoot)
		{
			this.available_content = (Content)0;
			List<FileSystemItem> list = new List<FileSystemItem>();
			this.file_source.GetTopLevelItems(list, relativeRoot);
			foreach (FileSystemItem fileSystemItem in list)
			{
				if (fileSystemItem.type == FileSystemItem.ItemType.File && fileSystemItem.name.ToLower().EndsWith(".po"))
				{
					this.available_content |= Content.Translation;
				}
			}
			return this.available_content > (Content)0;
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x0600BA91 RID: 47761 RVA: 0x0011CA7A File Offset: 0x0011AC7A
		public string ContentPath
		{
			get
			{
				return Path.Combine(this.label.install_path, this.relative_root);
			}
		}

		// Token: 0x0600BA92 RID: 47762 RVA: 0x0011CA92 File Offset: 0x0011AC92
		public bool IsEmpty()
		{
			return this.available_content == (Content)0;
		}

		// Token: 0x0600BA93 RID: 47763 RVA: 0x0047FC70 File Offset: 0x0047DE70
		private Content AddDirectory(string directory)
		{
			Content content = (Content)0;
			string text = directory.TrimEnd('/');
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 1519694028U)
			{
				if (num != 948591336U)
				{
					if (num != 1318520008U)
					{
						if (num == 1519694028U)
						{
							if (text == "elements")
							{
								content |= Content.LayerableFiles;
							}
						}
					}
					else if (text == "buildingfacades")
					{
						content |= Content.Animation;
					}
				}
				else if (text == "templates")
				{
					content |= Content.LayerableFiles;
				}
			}
			else if (num <= 3037049615U)
			{
				if (num != 2960291089U)
				{
					if (num == 3037049615U)
					{
						if (text == "worldgen")
						{
							content |= Content.LayerableFiles;
						}
					}
				}
				else if (text == "strings")
				{
					content |= Content.Strings;
				}
			}
			else if (num != 3319670096U)
			{
				if (num == 3570262116U)
				{
					if (text == "codex")
					{
						content |= Content.LayerableFiles;
					}
				}
			}
			else if (text == "anim")
			{
				content |= Content.Animation;
			}
			return content;
		}

		// Token: 0x0600BA94 RID: 47764 RVA: 0x0047FD80 File Offset: 0x0047DF80
		private Content AddFile(string file)
		{
			Content content = (Content)0;
			if (file.EndsWith(".dll"))
			{
				content |= Content.DLL;
			}
			if (file.EndsWith(".po"))
			{
				content |= Content.Translation;
			}
			return content;
		}

		// Token: 0x0600BA95 RID: 47765 RVA: 0x0011CA9D File Offset: 0x0011AC9D
		private static void AccumulateExtensions(Content content, List<string> extensions)
		{
			if ((content & Content.DLL) != (Content)0)
			{
				extensions.Add(".dll");
			}
			if ((content & (Content.Strings | Content.Translation)) != (Content)0)
			{
				extensions.Add(".po");
			}
		}

		// Token: 0x0600BA96 RID: 47766 RVA: 0x0047FDB4 File Offset: 0x0047DFB4
		[Conditional("DEBUG")]
		private void Assert(bool condition, string failure_message)
		{
			if (string.IsNullOrEmpty(this.title))
			{
				DebugUtil.Assert(condition, string.Format("{2}\n\t{0}\n\t{1}", this.title, this.label.ToString(), failure_message));
				return;
			}
			DebugUtil.Assert(condition, string.Format("{1}\n\t{0}", this.label.ToString(), failure_message));
		}

		// Token: 0x0600BA97 RID: 47767 RVA: 0x0047FE1C File Offset: 0x0047E01C
		public void Install()
		{
			if (this.IsLocal)
			{
				this.status = Mod.Status.Installed;
				return;
			}
			this.status = Mod.Status.ReinstallPending;
			if (this.file_source == null)
			{
				return;
			}
			if (!FileUtil.DeleteDirectory(this.label.install_path, 0))
			{
				return;
			}
			if (!FileUtil.CreateDirectory(this.label.install_path, 0))
			{
				return;
			}
			this.file_source.CopyTo(this.label.install_path, null);
			this.file_source = new Directory(this.label.install_path);
			this.status = Mod.Status.Installed;
		}

		// Token: 0x0600BA98 RID: 47768 RVA: 0x0047FEAC File Offset: 0x0047E0AC
		public bool Uninstall()
		{
			this.SetEnabledForActiveDlc(false);
			if (this.loaded_content != (Content)0)
			{
				global::Debug.Log(string.Format("Can't uninstall {0}: still has loaded content: {1}", this.label.ToString(), this.loaded_content.ToString()));
				this.status = Mod.Status.UninstallPending;
				return false;
			}
			if (!this.IsLocal && !FileUtil.DeleteDirectory(this.label.install_path, 0))
			{
				global::Debug.Log(string.Format("Can't uninstall {0}: directory deletion failed", this.label.ToString()));
				this.status = Mod.Status.UninstallPending;
				return false;
			}
			this.status = Mod.Status.NotInstalled;
			return true;
		}

		// Token: 0x0600BA99 RID: 47769 RVA: 0x0047FF54 File Offset: 0x0047E154
		private bool LoadStrings()
		{
			string path = FileSystem.Normalize(Path.Combine(this.ContentPath, "strings"));
			if (!Directory.Exists(path))
			{
				return false;
			}
			int num = 0;
			foreach (FileInfo fileInfo in new DirectoryInfo(path).GetFiles())
			{
				if (!(fileInfo.Extension.ToLower() != ".po"))
				{
					num++;
					Localization.OverloadStrings(Localization.LoadStringsFile(fileInfo.FullName, false));
				}
			}
			return true;
		}

		// Token: 0x0600BA9A RID: 47770 RVA: 0x000B1628 File Offset: 0x000AF828
		private bool LoadTranslations()
		{
			return false;
		}

		// Token: 0x0600BA9B RID: 47771 RVA: 0x0047FFD4 File Offset: 0x0047E1D4
		private bool LoadAnimation()
		{
			string path = FileSystem.Normalize(Path.Combine(this.ContentPath, "anim"));
			if (!Directory.Exists(path))
			{
				return false;
			}
			int num = 0;
			DirectoryInfo[] directories = new DirectoryInfo(path).GetDirectories();
			for (int i = 0; i < directories.Length; i++)
			{
				foreach (DirectoryInfo directoryInfo in directories[i].GetDirectories())
				{
					KAnimFile.Mod mod = new KAnimFile.Mod();
					foreach (FileInfo fileInfo in directoryInfo.GetFiles())
					{
						if (fileInfo.Extension == ".png")
						{
							byte[] data = File.ReadAllBytes(fileInfo.FullName);
							Texture2D texture2D = new Texture2D(2, 2);
							texture2D.LoadImage(data);
							mod.textures.Add(texture2D);
						}
						else if (fileInfo.Extension == ".bytes")
						{
							string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
							byte[] array = File.ReadAllBytes(fileInfo.FullName);
							if (fileNameWithoutExtension.EndsWith("_anim"))
							{
								mod.anim = array;
							}
							else if (fileNameWithoutExtension.EndsWith("_build"))
							{
								mod.build = array;
							}
							else
							{
								DebugUtil.LogWarningArgs(new object[]
								{
									string.Format("Unhandled TextAsset ({0})...ignoring", fileInfo.FullName)
								});
							}
						}
						else
						{
							DebugUtil.LogWarningArgs(new object[]
							{
								string.Format("Unhandled asset ({0})...ignoring", fileInfo.FullName)
							});
						}
					}
					string name = directoryInfo.Name + "_kanim";
					if (mod.IsValid() && ModUtil.AddKAnimMod(name, mod))
					{
						num++;
					}
				}
			}
			return true;
		}

		// Token: 0x0600BA9C RID: 47772 RVA: 0x00480198 File Offset: 0x0047E398
		public void Load(Content content)
		{
			content &= (this.available_content & ~this.loaded_content);
			if (content > (Content)0)
			{
				global::Debug.Log(string.Format("Loading mod content {2} [{0}:{1}] (provides {3})", new object[]
				{
					this.title,
					this.label.id,
					content.ToString(),
					this.available_content.ToString()
				}));
			}
			if ((content & Content.Strings) != (Content)0 && this.LoadStrings())
			{
				this.loaded_content |= Content.Strings;
			}
			if ((content & Content.Translation) != (Content)0 && this.LoadTranslations())
			{
				this.loaded_content |= Content.Translation;
			}
			if ((content & Content.DLL) != (Content)0)
			{
				this.loaded_mod_data = DLLLoader.LoadDLLs(this, this.staticID, this.ContentPath, this.IsDev);
				if (this.loaded_mod_data != null)
				{
					this.loaded_content |= Content.DLL;
				}
			}
			if ((content & Content.LayerableFiles) != (Content)0)
			{
				global::Debug.Assert(this.content_source != null, "Attempting to Load layerable files with content_source not initialized");
				FileSystem.file_sources.Insert(0, this.content_source.GetFileSystem());
				this.loaded_content |= Content.LayerableFiles;
			}
			if ((content & Content.Animation) != (Content)0 && this.LoadAnimation())
			{
				this.loaded_content |= Content.Animation;
			}
		}

		// Token: 0x0600BA9D RID: 47773 RVA: 0x0011CAC0 File Offset: 0x0011ACC0
		public void PostLoad(IReadOnlyList<Mod> mods)
		{
			if ((this.loaded_content & Content.DLL) != (Content)0 && this.loaded_mod_data != null)
			{
				DLLLoader.PostLoadDLLs(this.staticID, this.loaded_mod_data, mods);
			}
		}

		// Token: 0x0600BA9E RID: 47774 RVA: 0x0011CAE6 File Offset: 0x0011ACE6
		public void Unload(Content content)
		{
			content &= this.loaded_content;
			if ((content & Content.LayerableFiles) != (Content)0)
			{
				FileSystem.file_sources.Remove(this.content_source.GetFileSystem());
				this.loaded_content &= ~Content.LayerableFiles;
			}
		}

		// Token: 0x0600BA9F RID: 47775 RVA: 0x0011CB1F File Offset: 0x0011AD1F
		private void SetCrashCount(int new_crash_count)
		{
			this.crash_count = MathUtil.Clamp(0, 3, new_crash_count);
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x0600BAA0 RID: 47776 RVA: 0x0011CB2F File Offset: 0x0011AD2F
		public bool IsDev
		{
			get
			{
				return this.label.distribution_platform == Label.DistributionPlatform.Dev;
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x0600BAA1 RID: 47777 RVA: 0x0011CB3F File Offset: 0x0011AD3F
		public bool IsLocal
		{
			get
			{
				return this.label.distribution_platform == Label.DistributionPlatform.Dev || this.label.distribution_platform == Label.DistributionPlatform.Local;
			}
		}

		// Token: 0x0600BAA2 RID: 47778 RVA: 0x0011CB5F File Offset: 0x0011AD5F
		public void SetCrashed()
		{
			this.SetCrashCount(this.crash_count + 1);
			if (!this.IsDev)
			{
				this.SetEnabledForActiveDlc(false);
			}
		}

		// Token: 0x0600BAA3 RID: 47779 RVA: 0x0011CB7E File Offset: 0x0011AD7E
		public void Uncrash()
		{
			this.SetCrashCount(this.IsDev ? (this.crash_count - 1) : 0);
		}

		// Token: 0x0600BAA4 RID: 47780 RVA: 0x0011CB99 File Offset: 0x0011AD99
		public bool IsActive()
		{
			return this.loaded_content > (Content)0;
		}

		// Token: 0x0600BAA5 RID: 47781 RVA: 0x0011CBA4 File Offset: 0x0011ADA4
		public bool AllActive(Content content)
		{
			return (this.loaded_content & content) == content;
		}

		// Token: 0x0600BAA6 RID: 47782 RVA: 0x0011CBB1 File Offset: 0x0011ADB1
		public bool AllActive()
		{
			return (this.loaded_content & this.available_content) == this.available_content;
		}

		// Token: 0x0600BAA7 RID: 47783 RVA: 0x0011CBC8 File Offset: 0x0011ADC8
		public bool AnyActive(Content content)
		{
			return (this.loaded_content & content) > (Content)0;
		}

		// Token: 0x0600BAA8 RID: 47784 RVA: 0x0011CBD5 File Offset: 0x0011ADD5
		public bool HasContent()
		{
			return this.available_content > (Content)0;
		}

		// Token: 0x0600BAA9 RID: 47785 RVA: 0x0011CBE0 File Offset: 0x0011ADE0
		public bool HasAnyContent(Content content)
		{
			return (this.available_content & content) > (Content)0;
		}

		// Token: 0x0600BAAA RID: 47786 RVA: 0x0011CBED File Offset: 0x0011ADED
		public bool HasOnlyTranslationContent()
		{
			return this.available_content == Content.Translation;
		}

		// Token: 0x0600BAAB RID: 47787 RVA: 0x004802D8 File Offset: 0x0047E4D8
		public Texture2D GetPreviewImage()
		{
			string text = null;
			foreach (string text2 in Mod.PREVIEW_FILENAMES)
			{
				if (Directory.Exists(this.ContentPath) && File.Exists(Path.Combine(this.ContentPath, text2)))
				{
					text = text2;
					break;
				}
			}
			if (text == null)
			{
				return null;
			}
			Texture2D result;
			try
			{
				byte[] data = File.ReadAllBytes(Path.Combine(this.ContentPath, text));
				Texture2D texture2D = new Texture2D(2, 2);
				texture2D.LoadImage(data);
				result = texture2D;
			}
			catch
			{
				global::Debug.LogWarning(string.Format("Mod {0} seems to have a preview.png but it didn't load correctly.", this.label));
				result = null;
			}
			return result;
		}

		// Token: 0x0600BAAC RID: 47788 RVA: 0x0011CBF8 File Offset: 0x0011ADF8
		public void ModDevLog(string msg)
		{
			if (this.IsDev)
			{
				global::Debug.Log(msg);
			}
		}

		// Token: 0x0600BAAD RID: 47789 RVA: 0x0011CC08 File Offset: 0x0011AE08
		public void ModDevLogWarning(string msg)
		{
			if (this.IsDev)
			{
				global::Debug.LogWarning(msg);
			}
		}

		// Token: 0x0600BAAE RID: 47790 RVA: 0x0011CC18 File Offset: 0x0011AE18
		public void ModDevLogError(string msg)
		{
			if (this.IsDev)
			{
				this.DevModCrashTriggered = true;
				global::Debug.LogError(msg);
			}
		}

		// Token: 0x040098E7 RID: 39143
		public const int MOD_API_VERSION_NONE = 0;

		// Token: 0x040098E8 RID: 39144
		public const int MOD_API_VERSION_HARMONY1 = 1;

		// Token: 0x040098E9 RID: 39145
		public const int MOD_API_VERSION_HARMONY2 = 2;

		// Token: 0x040098EA RID: 39146
		public const int MOD_API_VERSION = 2;

		// Token: 0x040098EB RID: 39147
		[JsonProperty]
		public Label label;

		// Token: 0x040098EC RID: 39148
		[JsonProperty]
		public Mod.Status status;

		// Token: 0x040098ED RID: 39149
		[JsonProperty]
		public bool enabled;

		// Token: 0x040098EE RID: 39150
		[JsonProperty]
		public List<string> enabledForDlc;

		// Token: 0x040098F0 RID: 39152
		[JsonProperty]
		public int crash_count;

		// Token: 0x040098F1 RID: 39153
		[JsonProperty]
		public string reinstall_path;

		// Token: 0x040098F3 RID: 39155
		public bool foundInStackTrace;

		// Token: 0x040098F4 RID: 39156
		public string relative_root = "";

		// Token: 0x040098F5 RID: 39157
		public Mod.PackagedModInfo packagedModInfo;

		// Token: 0x040098FA RID: 39162
		public LoadedModData loaded_mod_data;

		// Token: 0x040098FB RID: 39163
		private IFileSource _fileSource;

		// Token: 0x040098FC RID: 39164
		public IFileSource content_source;

		// Token: 0x040098FD RID: 39165
		public bool is_subscribed;

		// Token: 0x040098FF RID: 39167
		private const string VANILLA_ID = "VANILLA_ID";

		// Token: 0x04009900 RID: 39168
		private const string ALL_ID = "ALL";

		// Token: 0x04009901 RID: 39169
		private const string ARCHIVED_VERSIONS_FOLDER = "archived_versions";

		// Token: 0x04009902 RID: 39170
		private const string MOD_INFO_FILENAME = "mod_info.yaml";

		// Token: 0x04009903 RID: 39171
		public ModContentCompatability contentCompatability;

		// Token: 0x04009904 RID: 39172
		public string[] requiredDlcIds;

		// Token: 0x04009905 RID: 39173
		public string[] forbiddenDlcIds;

		// Token: 0x04009906 RID: 39174
		public const int MAX_CRASH_COUNT = 3;

		// Token: 0x04009907 RID: 39175
		private static readonly List<string> PREVIEW_FILENAMES = new List<string>
		{
			"preview.png",
			"Preview.png",
			"PREVIEW.PNG"
		};

		// Token: 0x02002251 RID: 8785
		public enum Status
		{
			// Token: 0x04009909 RID: 39177
			NotInstalled,
			// Token: 0x0400990A RID: 39178
			Installed,
			// Token: 0x0400990B RID: 39179
			UninstallPending,
			// Token: 0x0400990C RID: 39180
			ReinstallPending
		}

		// Token: 0x02002252 RID: 8786
		public class ArchivedVersion
		{
			// Token: 0x0400990D RID: 39181
			public string relativePath;

			// Token: 0x0400990E RID: 39182
			public Mod.PackagedModInfo info;
		}

		// Token: 0x02002253 RID: 8787
		public class PackagedModInfo : IHasDlcRestrictions
		{
			// Token: 0x17000C14 RID: 3092
			// (get) Token: 0x0600BAB2 RID: 47794 RVA: 0x0011CC6D File Offset: 0x0011AE6D
			// (set) Token: 0x0600BAB3 RID: 47795 RVA: 0x0011CC75 File Offset: 0x0011AE75
			[Obsolete("Use IHasDlcRestrictions interface instead")]
			public string supportedContent { get; set; }

			// Token: 0x17000C15 RID: 3093
			// (get) Token: 0x0600BAB4 RID: 47796 RVA: 0x0011CC7E File Offset: 0x0011AE7E
			// (set) Token: 0x0600BAB5 RID: 47797 RVA: 0x0011CC86 File Offset: 0x0011AE86
			public string[] requiredDlcIds { get; set; }

			// Token: 0x17000C16 RID: 3094
			// (get) Token: 0x0600BAB6 RID: 47798 RVA: 0x0011CC8F File Offset: 0x0011AE8F
			// (set) Token: 0x0600BAB7 RID: 47799 RVA: 0x0011CC97 File Offset: 0x0011AE97
			public string[] forbiddenDlcIds { get; set; }

			// Token: 0x17000C17 RID: 3095
			// (get) Token: 0x0600BAB8 RID: 47800 RVA: 0x0011CCA0 File Offset: 0x0011AEA0
			// (set) Token: 0x0600BAB9 RID: 47801 RVA: 0x0011CCA8 File Offset: 0x0011AEA8
			[Obsolete("Use minimumSupportedBuild instead!")]
			public int lastWorkingBuild { get; set; }

			// Token: 0x17000C18 RID: 3096
			// (get) Token: 0x0600BABA RID: 47802 RVA: 0x0011CCB1 File Offset: 0x0011AEB1
			// (set) Token: 0x0600BABB RID: 47803 RVA: 0x0011CCB9 File Offset: 0x0011AEB9
			public int minimumSupportedBuild { get; set; }

			// Token: 0x17000C19 RID: 3097
			// (get) Token: 0x0600BABC RID: 47804 RVA: 0x0011CCC2 File Offset: 0x0011AEC2
			// (set) Token: 0x0600BABD RID: 47805 RVA: 0x0011CCCA File Offset: 0x0011AECA
			public int APIVersion { get; set; }

			// Token: 0x17000C1A RID: 3098
			// (get) Token: 0x0600BABE RID: 47806 RVA: 0x0011CCD3 File Offset: 0x0011AED3
			// (set) Token: 0x0600BABF RID: 47807 RVA: 0x0011CCDB File Offset: 0x0011AEDB
			public string version { get; set; }

			// Token: 0x0600BAC0 RID: 47808 RVA: 0x0011CCE4 File Offset: 0x0011AEE4
			public string[] GetRequiredDlcIds()
			{
				return this.requiredDlcIds;
			}

			// Token: 0x0600BAC1 RID: 47809 RVA: 0x0011CCEC File Offset: 0x0011AEEC
			public string[] GetForbiddenDlcIds()
			{
				return this.forbiddenDlcIds;
			}
		}
	}
}
