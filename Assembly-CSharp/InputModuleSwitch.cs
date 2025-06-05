using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001ABB RID: 6843
public class InputModuleSwitch : MonoBehaviour
{
	// Token: 0x06008F26 RID: 36646 RVA: 0x00380CB0 File Offset: 0x0037EEB0
	private void Update()
	{
		if (this.lastMousePosition != Input.mousePosition && KInputManager.currentControllerIsGamepad)
		{
			KInputManager.currentControllerIsGamepad = false;
			KInputManager.InputChange.Invoke();
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			this.virtualInput.enabled = KInputManager.currentControllerIsGamepad;
			if (this.standaloneInput.enabled)
			{
				this.standaloneInput.enabled = false;
				this.ChangeInputHandler();
				return;
			}
		}
		else
		{
			this.lastMousePosition = Input.mousePosition;
			this.standaloneInput.enabled = true;
			if (this.virtualInput.enabled)
			{
				this.virtualInput.enabled = false;
				this.ChangeInputHandler();
			}
		}
	}

	// Token: 0x06008F27 RID: 36647 RVA: 0x00380D54 File Offset: 0x0037EF54
	private void ChangeInputHandler()
	{
		GameInputManager inputManager = Global.GetInputManager();
		for (int i = 0; i < inputManager.usedMenus.Count; i++)
		{
			if (inputManager.usedMenus[i].Equals(null))
			{
				inputManager.usedMenus.RemoveAt(i);
			}
		}
		if (inputManager.GetControllerCount() > 1)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				Cursor.visible = false;
				inputManager.GetController(1).inputHandler.TransferHandles(inputManager.GetController(0).inputHandler);
				return;
			}
			Cursor.visible = true;
			inputManager.GetController(0).inputHandler.TransferHandles(inputManager.GetController(1).inputHandler);
		}
	}

	// Token: 0x04006BDB RID: 27611
	public VirtualInputModule virtualInput;

	// Token: 0x04006BDC RID: 27612
	public StandaloneInputModule standaloneInput;

	// Token: 0x04006BDD RID: 27613
	private Vector3 lastMousePosition;
}
