﻿using System;
using System.Collections.Generic;
using System.Linq;
using KMod;
using STRINGS;
using UnityEngine;

public class ModsScreen : KModalScreen
{
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

	protected override void OnDeactivate()
	{
		Manager modManager = Global.Instance.modManager;
		modManager.on_update = (Manager.OnUpdate)Delegate.Remove(modManager.on_update, new Manager.OnUpdate(this.RebuildDisplay));
		base.OnDeactivate();
	}

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

	private void RebuildDisplay(object change_source)
	{
		if (change_source != this)
		{
			this.BuildDisplay();
		}
	}

	private bool ShouldDisplayMod(Mod mod)
	{
		return mod.status != Mod.Status.NotInstalled && mod.status != Mod.Status.UninstallPending && !mod.HasOnlyTranslationContent();
	}

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

	private void OnToggleClicked(MultiToggle toggle, Label mod)
	{
		Manager modManager = Global.Instance.modManager;
		bool flag = modManager.IsModEnabled(mod);
		flag = !flag;
		toggle.ChangeState(flag ? 1 : 0);
		modManager.EnableMod(mod, flag, this);
		this.UpdateToggleAllButton();
	}

	private bool AreAnyModsDisabled()
	{
		return Global.Instance.modManager.mods.Any((Mod mod) => !mod.IsEmpty() && !mod.IsEnabledForActiveDlc() && this.ShouldDisplayMod(mod));
	}

	private void UpdateToggleAllButton()
	{
		this.toggleAllButton.GetComponentInChildren<LocText>().text = (this.AreAnyModsDisabled() ? UI.FRONTEND.MODS.ENABLE_ALL : UI.FRONTEND.MODS.DISABLE_ALL);
	}

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

	[SerializeField]
	private KButton closeButtonTitle;

	[SerializeField]
	private KButton closeButton;

	[SerializeField]
	private KButton toggleAllButton;

	[SerializeField]
	private KButton workshopButton;

	[SerializeField]
	private GameObject entryPrefab;

	[SerializeField]
	private Transform entryParent;

	private List<ModsScreen.DisplayedMod> displayedMods = new List<ModsScreen.DisplayedMod>();

	private List<Label> mod_footprint = new List<Label>();

	private struct DisplayedMod
	{
		public RectTransform rect_transform;

		public int mod_index;
	}

	private class ModOrderingDragListener : DragMe.IDragListener
	{
		public ModOrderingDragListener(ModsScreen screen, List<ModsScreen.DisplayedMod> mods)
		{
			this.screen = screen;
			this.mods = mods;
		}

		public void OnBeginDrag(Vector2 pos)
		{
			this.startDragIdx = this.GetDragIdx(pos, false);
		}

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

		private List<ModsScreen.DisplayedMod> mods;

		private ModsScreen screen;

		private int startDragIdx = -1;
	}
}
