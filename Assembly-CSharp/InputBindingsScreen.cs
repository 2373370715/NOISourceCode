using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D5E RID: 7518
public class InputBindingsScreen : KModalScreen
{
	// Token: 0x06009CEF RID: 40175 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06009CF0 RID: 40176 RVA: 0x0010A93C File Offset: 0x00108B3C
	private bool IsKeyDown(KeyCode key_code)
	{
		return Input.GetKey(key_code) || Input.GetKeyDown(key_code);
	}

	// Token: 0x06009CF1 RID: 40177 RVA: 0x003D386C File Offset: 0x003D1A6C
	private string GetModifierString(Modifier modifiers)
	{
		string text = "";
		foreach (object obj in Enum.GetValues(typeof(Modifier)))
		{
			Modifier modifier = (Modifier)obj;
			if ((modifiers & modifier) != Modifier.None)
			{
				text = text + " + " + modifier.ToString();
			}
		}
		return text;
	}

	// Token: 0x06009CF2 RID: 40178 RVA: 0x003D38EC File Offset: 0x003D1AEC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.entryPrefab.SetActive(false);
		this.prevScreenButton.onClick += this.OnPrevScreen;
		this.nextScreenButton.onClick += this.OnNextScreen;
	}

	// Token: 0x06009CF3 RID: 40179 RVA: 0x003D393C File Offset: 0x003D1B3C
	protected override void OnActivate()
	{
		this.CollectScreens();
		string text = this.screens[this.activeScreen];
		string key = "STRINGS.INPUT_BINDINGS." + text.ToUpper() + ".NAME";
		this.screenTitle.text = Strings.Get(key);
		this.closeButton.onClick += this.OnBack;
		this.backButton.onClick += this.OnBack;
		this.resetButton.onClick += this.OnReset;
		this.BuildDisplay();
	}

	// Token: 0x06009CF4 RID: 40180 RVA: 0x003D39D8 File Offset: 0x003D1BD8
	private void CollectScreens()
	{
		this.screens.Clear();
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mGroup != null && bindingEntry.mRebindable && !this.screens.Contains(bindingEntry.mGroup) && DlcManager.IsCorrectDlcSubscribed(bindingEntry.dlcIds, null))
			{
				if (bindingEntry.mGroup == "Root")
				{
					this.activeScreen = this.screens.Count;
				}
				this.screens.Add(bindingEntry.mGroup);
			}
		}
	}

	// Token: 0x06009CF5 RID: 40181 RVA: 0x0010A94E File Offset: 0x00108B4E
	protected override void OnDeactivate()
	{
		GameInputMapping.SaveBindings();
		this.DestroyDisplay();
	}

	// Token: 0x06009CF6 RID: 40182 RVA: 0x000AA765 File Offset: 0x000A8965
	private LocString GetActionString(global::Action action)
	{
		return null;
	}

	// Token: 0x06009CF7 RID: 40183 RVA: 0x003D3A74 File Offset: 0x003D1C74
	private string GetBindingText(BindingEntry binding)
	{
		string text = GameUtil.GetKeycodeLocalized(binding.mKeyCode);
		if (binding.mKeyCode != KKeyCode.LeftAlt && binding.mKeyCode != KKeyCode.RightAlt && binding.mKeyCode != KKeyCode.LeftControl && binding.mKeyCode != KKeyCode.RightControl && binding.mKeyCode != KKeyCode.LeftShift && binding.mKeyCode != KKeyCode.RightShift)
		{
			text += this.GetModifierString(binding.mModifier);
		}
		return text;
	}

	// Token: 0x06009CF8 RID: 40184 RVA: 0x003D3AF0 File Offset: 0x003D1CF0
	private void BuildDisplay()
	{
		string text = this.screens[this.activeScreen];
		string key = "STRINGS.INPUT_BINDINGS." + text.ToUpper() + ".NAME";
		this.screenTitle.text = Strings.Get(key);
		if (this.entryPool == null)
		{
			this.entryPool = new UIPool<HorizontalLayoutGroup>(this.entryPrefab.GetComponent<HorizontalLayoutGroup>());
		}
		this.DestroyDisplay();
		int num = 0;
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry binding = GameInputMapping.KeyBindings[i];
			if (binding.mGroup == this.screens[this.activeScreen] && binding.mRebindable && DlcManager.IsCorrectDlcSubscribed(binding.dlcIds, null))
			{
				GameObject gameObject = this.entryPool.GetFreeElement(this.parent, true).gameObject;
				TMP_Text componentInChildren = gameObject.transform.GetChild(0).GetComponentInChildren<LocText>();
				string key2 = "STRINGS.INPUT_BINDINGS." + binding.mGroup.ToUpper() + "." + binding.mAction.ToString().ToUpper();
				componentInChildren.text = Strings.Get(key2);
				LocText key_label = gameObject.transform.GetChild(1).GetComponentInChildren<LocText>();
				key_label.text = this.GetBindingText(binding);
				KButton button = gameObject.GetComponentInChildren<KButton>();
				button.onClick += delegate()
				{
					this.waitingForKeyPress = true;
					this.actionToRebind = binding.mAction;
					this.ignoreRootConflicts = binding.mIgnoreRootConflics;
					this.activeButton = button;
					key_label.text = UI.FRONTEND.INPUT_BINDINGS_SCREEN.WAITING_FOR_INPUT;
				};
				gameObject.transform.SetSiblingIndex(num);
				num++;
			}
		}
	}

	// Token: 0x06009CF9 RID: 40185 RVA: 0x0010A95B File Offset: 0x00108B5B
	private void DestroyDisplay()
	{
		this.entryPool.ClearAll();
	}

	// Token: 0x06009CFA RID: 40186 RVA: 0x003D3CD0 File Offset: 0x003D1ED0
	private void Update()
	{
		if (this.waitingForKeyPress)
		{
			Modifier modifier = Modifier.None;
			modifier |= ((this.IsKeyDown(KeyCode.LeftAlt) || this.IsKeyDown(KeyCode.RightAlt)) ? Modifier.Alt : Modifier.None);
			modifier |= ((this.IsKeyDown(KeyCode.LeftControl) || this.IsKeyDown(KeyCode.RightControl)) ? Modifier.Ctrl : Modifier.None);
			modifier |= ((this.IsKeyDown(KeyCode.LeftShift) || this.IsKeyDown(KeyCode.RightShift)) ? Modifier.Shift : Modifier.None);
			modifier |= (this.IsKeyDown(KeyCode.CapsLock) ? Modifier.CapsLock : Modifier.None);
			modifier |= (this.IsKeyDown(KeyCode.BackQuote) ? Modifier.Backtick : Modifier.None);
			bool flag = false;
			for (int i = 0; i < InputBindingsScreen.validKeys.Length; i++)
			{
				KeyCode keyCode = InputBindingsScreen.validKeys[i];
				if (Input.GetKeyDown(keyCode))
				{
					KKeyCode kkey_code = (KKeyCode)keyCode;
					this.Bind(kkey_code, modifier);
					flag = true;
				}
			}
			if (!flag)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				KKeyCode kkeyCode = KKeyCode.None;
				if (axis < 0f)
				{
					kkeyCode = KKeyCode.MouseScrollDown;
				}
				else if (axis > 0f)
				{
					kkeyCode = KKeyCode.MouseScrollUp;
				}
				if (kkeyCode != KKeyCode.None)
				{
					this.Bind(kkeyCode, modifier);
				}
			}
		}
	}

	// Token: 0x06009CFB RID: 40187 RVA: 0x003D3DE8 File Offset: 0x003D1FE8
	private BindingEntry GetDuplicatedBinding(string activeScreen, BindingEntry new_binding)
	{
		BindingEntry result = default(BindingEntry);
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (new_binding.IsBindingEqual(bindingEntry) && (bindingEntry.mGroup == null || bindingEntry.mGroup == activeScreen || bindingEntry.mGroup == "Root" || activeScreen == "Root") && (!(activeScreen == "Root") || !bindingEntry.mIgnoreRootConflics) && (!(bindingEntry.mGroup == "Root") || !new_binding.mIgnoreRootConflics))
			{
				result = bindingEntry;
				break;
			}
		}
		return result;
	}

	// Token: 0x06009CFC RID: 40188 RVA: 0x0010A968 File Offset: 0x00108B68
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.waitingForKeyPress)
		{
			e.Consumed = true;
			return;
		}
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009CFD RID: 40189 RVA: 0x00103818 File Offset: 0x00101A18
	public override void OnKeyUp(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x06009CFE RID: 40190 RVA: 0x003D3E94 File Offset: 0x003D2094
	private void OnBack()
	{
		int num = this.NumUnboundActions();
		if (num == 0)
		{
			this.Deactivate();
			return;
		}
		string text;
		if (num == 1)
		{
			BindingEntry firstUnbound = this.GetFirstUnbound();
			text = string.Format(UI.FRONTEND.INPUT_BINDINGS_SCREEN.UNBOUND_ACTION, firstUnbound.mAction.ToString());
		}
		else
		{
			text = UI.FRONTEND.INPUT_BINDINGS_SCREEN.MULTIPLE_UNBOUND_ACTIONS;
		}
		this.confirmDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, base.transform.gameObject, false).GetComponent<ConfirmDialogScreen>();
		this.confirmDialog.PopupConfirmDialog(text, delegate
		{
			this.Deactivate();
		}, delegate
		{
			this.confirmDialog.Deactivate();
		}, null, null, null, null, null, null);
		this.confirmDialog.gameObject.SetActive(true);
	}

	// Token: 0x06009CFF RID: 40191 RVA: 0x003D3F50 File Offset: 0x003D2150
	private int NumUnboundActions()
	{
		int num = 0;
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mKeyCode == KKeyCode.None && bindingEntry.mRebindable && (BuildMenu.UseHotkeyBuildMenu() || !bindingEntry.mIgnoreRootConflics))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06009D00 RID: 40192 RVA: 0x003D3FA4 File Offset: 0x003D21A4
	private BindingEntry GetFirstUnbound()
	{
		BindingEntry result = default(BindingEntry);
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mKeyCode == KKeyCode.None)
			{
				result = bindingEntry;
				break;
			}
		}
		return result;
	}

	// Token: 0x06009D01 RID: 40193 RVA: 0x0010A99A File Offset: 0x00108B9A
	private void OnReset()
	{
		GameInputMapping.KeyBindings = (BindingEntry[])GameInputMapping.DefaultBindings.Clone();
		Global.GetInputManager().RebindControls();
		this.BuildDisplay();
	}

	// Token: 0x06009D02 RID: 40194 RVA: 0x0010A9C0 File Offset: 0x00108BC0
	public void OnPrevScreen()
	{
		if (this.activeScreen > 0)
		{
			this.activeScreen--;
		}
		else
		{
			this.activeScreen = this.screens.Count - 1;
		}
		this.BuildDisplay();
	}

	// Token: 0x06009D03 RID: 40195 RVA: 0x0010A9F4 File Offset: 0x00108BF4
	public void OnNextScreen()
	{
		if (this.activeScreen < this.screens.Count - 1)
		{
			this.activeScreen++;
		}
		else
		{
			this.activeScreen = 0;
		}
		this.BuildDisplay();
	}

	// Token: 0x06009D04 RID: 40196 RVA: 0x003D3FE4 File Offset: 0x003D21E4
	private void Bind(KKeyCode kkey_code, Modifier modifier)
	{
		BindingEntry bindingEntry = new BindingEntry(this.screens[this.activeScreen], GamepadButton.NumButtons, kkey_code, modifier, this.actionToRebind, true, this.ignoreRootConflicts);
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry2 = GameInputMapping.KeyBindings[i];
			if (bindingEntry2.mRebindable && bindingEntry2.mAction == this.actionToRebind)
			{
				BindingEntry duplicatedBinding = this.GetDuplicatedBinding(this.screens[this.activeScreen], bindingEntry);
				bindingEntry.mButton = GameInputMapping.KeyBindings[i].mButton;
				GameInputMapping.KeyBindings[i] = bindingEntry;
				this.activeButton.GetComponentInChildren<LocText>().text = this.GetBindingText(bindingEntry);
				if (duplicatedBinding.mAction != global::Action.Invalid && duplicatedBinding.mAction != this.actionToRebind)
				{
					this.confirmDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, base.transform.gameObject, false).GetComponent<ConfirmDialogScreen>();
					string arg = Strings.Get("STRINGS.INPUT_BINDINGS." + duplicatedBinding.mGroup.ToUpper() + "." + duplicatedBinding.mAction.ToString().ToUpper());
					string bindingText = this.GetBindingText(duplicatedBinding);
					string text = string.Format(UI.FRONTEND.INPUT_BINDINGS_SCREEN.DUPLICATE, arg, bindingText);
					this.Unbind(duplicatedBinding.mAction);
					this.confirmDialog.PopupConfirmDialog(text, null, null, null, null, null, null, null, null);
					this.confirmDialog.gameObject.SetActive(true);
				}
				Global.GetInputManager().RebindControls();
				this.waitingForKeyPress = false;
				this.actionToRebind = global::Action.NumActions;
				this.activeButton = null;
				this.BuildDisplay();
				return;
			}
		}
	}

	// Token: 0x06009D05 RID: 40197 RVA: 0x003D41A8 File Offset: 0x003D23A8
	private void Unbind(global::Action action)
	{
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mAction == action)
			{
				bindingEntry.mKeyCode = KKeyCode.None;
				bindingEntry.mModifier = Modifier.None;
				GameInputMapping.KeyBindings[i] = bindingEntry;
			}
		}
	}

	// Token: 0x06009D07 RID: 40199 RVA: 0x0010AA4D File Offset: 0x00108C4D
	// Note: this type is marked as 'beforefieldinit'.
	static InputBindingsScreen()
	{
		KeyCode[] array = new KeyCode[111];
		RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.4522A529DBF1D30936B6BCC06D2E607CD76E3B0FB1C18D9DA2635843A2840CD7).FieldHandle);
		InputBindingsScreen.validKeys = array;
	}

	// Token: 0x04007AEC RID: 31468
	private const string ROOT_KEY = "STRINGS.INPUT_BINDINGS.";

	// Token: 0x04007AED RID: 31469
	[SerializeField]
	private OptionsMenuScreen optionsScreen;

	// Token: 0x04007AEE RID: 31470
	[SerializeField]
	private ConfirmDialogScreen confirmPrefab;

	// Token: 0x04007AEF RID: 31471
	public KButton backButton;

	// Token: 0x04007AF0 RID: 31472
	public KButton resetButton;

	// Token: 0x04007AF1 RID: 31473
	public KButton closeButton;

	// Token: 0x04007AF2 RID: 31474
	public KButton prevScreenButton;

	// Token: 0x04007AF3 RID: 31475
	public KButton nextScreenButton;

	// Token: 0x04007AF4 RID: 31476
	private bool waitingForKeyPress;

	// Token: 0x04007AF5 RID: 31477
	private global::Action actionToRebind = global::Action.NumActions;

	// Token: 0x04007AF6 RID: 31478
	private bool ignoreRootConflicts;

	// Token: 0x04007AF7 RID: 31479
	private KButton activeButton;

	// Token: 0x04007AF8 RID: 31480
	[SerializeField]
	private LocText screenTitle;

	// Token: 0x04007AF9 RID: 31481
	[SerializeField]
	private GameObject parent;

	// Token: 0x04007AFA RID: 31482
	[SerializeField]
	private GameObject entryPrefab;

	// Token: 0x04007AFB RID: 31483
	private ConfirmDialogScreen confirmDialog;

	// Token: 0x04007AFC RID: 31484
	private int activeScreen = -1;

	// Token: 0x04007AFD RID: 31485
	private List<string> screens = new List<string>();

	// Token: 0x04007AFE RID: 31486
	private UIPool<HorizontalLayoutGroup> entryPool;

	// Token: 0x04007AFF RID: 31487
	private static readonly KeyCode[] validKeys;
}
