using System;
using System.Collections.Generic;
using System.Linq;
using KMod;
using STRINGS;
using UnityEngine;

// Token: 0x02001B45 RID: 6981
public class ModsScreen : KModalScreen
{
	// Token: 0x06009278 RID: 37496 RVA: 0x00392DE4 File Offset: 0x00390FE4
	protected override void OnActivate()
	{
		base.OnActivate();
		this.closeButtonTitle.onClick += this.Exit;
		this.closeButton.onClick += this.Exit;
		System.Action value = delegate()
		{
			App.OpenWebURL("http://steamcommunity.com/workshop/browse/?appid=457140");
		};
		this.workshopButton.onClick += value;
		this.UpdateToggleAllButton();
		this.toggleAllButton.onClick += this.OnToggleAllClicked;
		Global.Instance.modManager.Sanitize(base.gameObject);
		this.mod_footprint.Clear();
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.IsEnabledForActiveDlc())
			{
				this.mod_footprint.Add(mod.label);
				if ((mod.loaded_content & (Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation)) == (mod.available_content & (Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation)))
				{
					mod.Uncrash();
				}
			}
		}
		this.BuildDisplay();
		Manager modManager = Global.Instance.modManager;
		modManager.on_update = (Manager.OnUpdate)Delegate.Combine(modManager.on_update, new Manager.OnUpdate(this.RebuildDisplay));
	}

	// Token: 0x06009279 RID: 37497 RVA: 0x001044BB File Offset: 0x001026BB
	protected override void OnDeactivate()
	{
		Manager modManager = Global.Instance.modManager;
		modManager.on_update = (Manager.OnUpdate)Delegate.Remove(modManager.on_update, new Manager.OnUpdate(this.RebuildDisplay));
		base.OnDeactivate();
	}

	// Token: 0x0600927A RID: 37498 RVA: 0x00392F3C File Offset: 0x0039113C
	private void Exit()
	{
		Global.Instance.modManager.Save();
		if (!Global.Instance.modManager.MatchFootprint(this.mod_footprint, Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation))
		{
			Global.Instance.modManager.RestartDialog(UI.FRONTEND.MOD_DIALOGS.MODS_SCREEN_CHANGES.TITLE, UI.FRONTEND.MOD_DIALOGS.MODS_SCREEN_CHANGES.MESSAGE, new System.Action(this.Deactivate), true, base.gameObject, null);
		}
		else
		{
			this.Deactivate();
		}
		Global.Instance.modManager.events.Clear();
	}

	// Token: 0x0600927B RID: 37499 RVA: 0x001044EE File Offset: 0x001026EE
	private void RebuildDisplay(object change_source)
	{
		if (change_source != this)
		{
			this.BuildDisplay();
		}
	}

	// Token: 0x0600927C RID: 37500 RVA: 0x001044FA File Offset: 0x001026FA
	private bool ShouldDisplayMod(Mod mod)
	{
		return mod.status != Mod.Status.NotInstalled && mod.status != Mod.Status.UninstallPending && !mod.HasOnlyTranslationContent();
	}

	// Token: 0x0600927D RID: 37501 RVA: 0x00392FC8 File Offset: 0x003911C8
	private void BuildDisplay()
	{
		foreach (ModsScreen.DisplayedMod displayedMod in this.displayedMods)
		{
			if (displayedMod.rect_transform != null)
			{
				UnityEngine.Object.Destroy(displayedMod.rect_transform.gameObject);
			}
		}
		this.displayedMods.Clear();
		ModsScreen.ModOrderingDragListener listener = new ModsScreen.ModOrderingDragListener(this, this.displayedMods);
		for (int num = 0; num != Global.Instance.modManager.mods.Count; num++)
		{
			Mod mod = Global.Instance.modManager.mods[num];
			if (this.ShouldDisplayMod(mod))
			{
				HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.entryPrefab, this.entryParent.gameObject, false);
				this.displayedMods.Add(new ModsScreen.DisplayedMod
				{
					rect_transform = hierarchyReferences.gameObject.GetComponent<RectTransform>(),
					mod_index = num
				});
				hierarchyReferences.GetComponent<DragMe>().listener = listener;
				LocText reference = hierarchyReferences.GetReference<LocText>("Title");
				string text = mod.title;
				StringEntry entry;
				if (Strings.TryGet(mod.title, out entry))
				{
					text = entry;
				}
				hierarchyReferences.name = mod.title;
				ToolTip reference2 = hierarchyReferences.GetReference<ToolTip>("Description");
				if (mod.available_content == (Content)0)
				{
					switch (mod.contentCompatability)
					{
					case ModContentCompatability.NoContent:
						text += UI.FRONTEND.MODS.CONTENT_FAILURE.NO_CONTENT;
						reference2.toolTip = UI.FRONTEND.MODS.CONTENT_FAILURE.NO_CONTENT_TOOLTIP;
						goto IL_39F;
					case ModContentCompatability.OldAPI:
						text += UI.FRONTEND.MODS.CONTENT_FAILURE.OLD_API;
						reference2.toolTip = UI.FRONTEND.MODS.CONTENT_FAILURE.OLD_API_TOOLTIP;
						goto IL_39F;
					}
					string text2 = GlobalAssets.Instance.colorSet.GetColorByName("statusItemBad").ToHexString();
					string text3 = UI.FRONTEND.MODS.CONTENT_FAILURE.DISABLED_CONTENT_TOOLTIP + "\n\n";
					if (mod.GetRequiredDlcIds() != null)
					{
						text3 += UI.FRONTEND.MODS.CONTENT_FAILURE.DISABLED_CONTENT_TOOLTIP_REQUIRED;
						foreach (string dlcId in mod.GetRequiredDlcIds())
						{
							if (DlcManager.IsContentSubscribed(dlcId))
							{
								text3 = text3 + "\n     •  <i>" + DlcManager.GetDlcTitleNoFormatting(dlcId) + "</i>";
							}
							else
							{
								text3 = string.Concat(new string[]
								{
									text3,
									"\n     •  <i><color=#",
									text2,
									">",
									DlcManager.GetDlcTitleNoFormatting(dlcId),
									"</color></i>"
								});
							}
						}
						if (mod.GetForbiddenDlcIds() != null)
						{
							text3 += "\n\n";
						}
					}
					if (mod.GetForbiddenDlcIds() != null)
					{
						text3 += UI.FRONTEND.MODS.CONTENT_FAILURE.DISABLED_CONTENT_TOOLTIP_FORBIDDEN_DLC;
						foreach (string dlcId2 in mod.GetForbiddenDlcIds())
						{
							if (!DlcManager.IsContentSubscribed(dlcId2))
							{
								text3 = text3 + "\n     •  <i>" + DlcManager.GetDlcTitleNoFormatting(dlcId2) + "</i>";
							}
							else
							{
								text3 = string.Concat(new string[]
								{
									text3,
									"\n     •  <i><color=#",
									text2,
									">",
									DlcManager.GetDlcTitleNoFormatting(dlcId2),
									"</color></i>"
								});
							}
						}
					}
					reference2.toolTip = text3;
					text += UI.FRONTEND.MODS.CONTENT_FAILURE.DISABLED_CONTENT;
				}
				IL_39F:
				reference.text = text;
				LocText reference3 = hierarchyReferences.GetReference<LocText>("Version");
				if (mod.packagedModInfo != null && mod.packagedModInfo.version != null && mod.packagedModInfo.version.Length > 0)
				{
					string text4 = mod.packagedModInfo.version;
					if (text4.StartsWith("V"))
					{
						text4 = "v" + text4.Substring(1, text4.Length - 1);
					}
					else if (!text4.StartsWith("v"))
					{
						text4 = "v" + text4;
					}
					reference3.text = text4;
					reference3.gameObject.SetActive(true);
				}
				else
				{
					reference3.gameObject.SetActive(false);
				}
				if (mod.available_content > (Content)0)
				{
					StringEntry entry2;
					if (Strings.TryGet(mod.description, out entry2))
					{
						reference2.toolTip = entry2;
					}
					else
					{
						reference2.toolTip = mod.description;
					}
				}
				if (mod.crash_count != 0)
				{
					reference.color = Color.Lerp(Color.white, Color.red, (float)mod.crash_count / 3f);
				}
				KButton reference4 = hierarchyReferences.GetReference<KButton>("ManageButton");
				reference4.GetComponentInChildren<LocText>().text = (mod.IsLocal ? UI.FRONTEND.MODS.MANAGE_LOCAL : UI.FRONTEND.MODS.MANAGE);
				reference4.isInteractable = mod.is_managed;
				if (reference4.isInteractable)
				{
					reference4.GetComponent<ToolTip>().toolTip = mod.manage_tooltip;
					reference4.onClick += mod.on_managed;
				}
				KImage reference5 = hierarchyReferences.GetReference<KImage>("BG");
				MultiToggle toggle = hierarchyReferences.GetReference<MultiToggle>("EnabledToggle");
				toggle.ChangeState(mod.IsEnabledForActiveDlc() ? 1 : 0);
				if (mod.available_content != (Content)0)
				{
					reference5.defaultState = KImage.ColorSelector.Inactive;
					reference5.ColorState = KImage.ColorSelector.Inactive;
					MultiToggle toggle2 = toggle;
					toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
					{
						this.OnToggleClicked(toggle, mod.label);
					}));
					toggle.GetComponent<ToolTip>().OnToolTip = (() => mod.IsEnabledForActiveDlc() ? UI.FRONTEND.MODS.TOOLTIPS.ENABLED : UI.FRONTEND.MODS.TOOLTIPS.DISABLED);
				}
				else
				{
					reference5.defaultState = KImage.ColorSelector.Disabled;
					reference5.ColorState = KImage.ColorSelector.Disabled;
				}
				hierarchyReferences.gameObject.SetActive(true);
			}
		}
		foreach (ModsScreen.DisplayedMod displayedMod2 in this.displayedMods)
		{
			displayedMod2.rect_transform.gameObject.SetActive(true);
		}
		int count = this.displayedMods.Count;
	}

	// Token: 0x0600927E RID: 37502 RVA: 0x00393698 File Offset: 0x00391898
	private void OnToggleClicked(MultiToggle toggle, Label mod)
	{
		Manager modManager = Global.Instance.modManager;
		bool flag = modManager.IsModEnabled(mod);
		flag = !flag;
		toggle.ChangeState(flag ? 1 : 0);
		modManager.EnableMod(mod, flag, this);
		this.UpdateToggleAllButton();
	}

	// Token: 0x0600927F RID: 37503 RVA: 0x00104518 File Offset: 0x00102718
	private bool AreAnyModsDisabled()
	{
		return Global.Instance.modManager.mods.Any((Mod mod) => !mod.IsEmpty() && !mod.IsEnabledForActiveDlc() && this.ShouldDisplayMod(mod));
	}

	// Token: 0x06009280 RID: 37504 RVA: 0x0010453A File Offset: 0x0010273A
	private void UpdateToggleAllButton()
	{
		this.toggleAllButton.GetComponentInChildren<LocText>().text = (this.AreAnyModsDisabled() ? UI.FRONTEND.MODS.ENABLE_ALL : UI.FRONTEND.MODS.DISABLE_ALL);
	}

	// Token: 0x06009281 RID: 37505 RVA: 0x003936D8 File Offset: 0x003918D8
	private void OnToggleAllClicked()
	{
		bool enabled = this.AreAnyModsDisabled();
		Manager modManager = Global.Instance.modManager;
		foreach (Mod mod in modManager.mods)
		{
			if (this.ShouldDisplayMod(mod))
			{
				modManager.EnableMod(mod.label, enabled, this);
			}
		}
		this.BuildDisplay();
		this.UpdateToggleAllButton();
	}

	// Token: 0x04006EFE RID: 28414
	[SerializeField]
	private KButton closeButtonTitle;

	// Token: 0x04006EFF RID: 28415
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04006F00 RID: 28416
	[SerializeField]
	private KButton toggleAllButton;

	// Token: 0x04006F01 RID: 28417
	[SerializeField]
	private KButton workshopButton;

	// Token: 0x04006F02 RID: 28418
	[SerializeField]
	private GameObject entryPrefab;

	// Token: 0x04006F03 RID: 28419
	[SerializeField]
	private Transform entryParent;

	// Token: 0x04006F04 RID: 28420
	private List<ModsScreen.DisplayedMod> displayedMods = new List<ModsScreen.DisplayedMod>();

	// Token: 0x04006F05 RID: 28421
	private List<Label> mod_footprint = new List<Label>();

	// Token: 0x02001B46 RID: 6982
	private struct DisplayedMod
	{
		// Token: 0x04006F06 RID: 28422
		public RectTransform rect_transform;

		// Token: 0x04006F07 RID: 28423
		public int mod_index;
	}

	// Token: 0x02001B47 RID: 6983
	private class ModOrderingDragListener : DragMe.IDragListener
	{
		// Token: 0x06009284 RID: 37508 RVA: 0x0010459E File Offset: 0x0010279E
		public ModOrderingDragListener(ModsScreen screen, List<ModsScreen.DisplayedMod> mods)
		{
			this.screen = screen;
			this.mods = mods;
		}

		// Token: 0x06009285 RID: 37509 RVA: 0x001045BB File Offset: 0x001027BB
		public void OnBeginDrag(Vector2 pos)
		{
			this.startDragIdx = this.GetDragIdx(pos, false);
		}

		// Token: 0x06009286 RID: 37510 RVA: 0x0039375C File Offset: 0x0039195C
		public void OnEndDrag(Vector2 pos)
		{
			if (this.startDragIdx < 0)
			{
				return;
			}
			int dragIdx = this.GetDragIdx(pos, true);
			if (dragIdx != this.startDragIdx)
			{
				int mod_index = this.mods[this.startDragIdx].mod_index;
				int target_index = (0 <= dragIdx && dragIdx < this.mods.Count) ? this.mods[dragIdx].mod_index : -1;
				Global.Instance.modManager.Reinsert(mod_index, target_index, dragIdx >= this.mods.Count, this);
				this.screen.BuildDisplay();
			}
		}

		// Token: 0x06009287 RID: 37511 RVA: 0x003937F4 File Offset: 0x003919F4
		private int GetDragIdx(Vector2 pos, bool halfPosition)
		{
			int result = -1;
			for (int i = 0; i < this.mods.Count; i++)
			{
				Vector2 vector;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(this.mods[i].rect_transform, pos, null, out vector);
				if (!halfPosition)
				{
					vector += this.mods[i].rect_transform.rect.min;
				}
				if (vector.y >= 0f)
				{
					break;
				}
				result = i;
			}
			return result;
		}

		// Token: 0x04006F08 RID: 28424
		private List<ModsScreen.DisplayedMod> mods;

		// Token: 0x04006F09 RID: 28425
		private ModsScreen screen;

		// Token: 0x04006F0A RID: 28426
		private int startDragIdx = -1;
	}
}
