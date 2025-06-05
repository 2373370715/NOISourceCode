using System;
using UnityEngine;

// Token: 0x020020D5 RID: 8405
public class ScreenResolutionMonitor : MonoBehaviour
{
	// Token: 0x0600B324 RID: 45860 RVA: 0x001190B5 File Offset: 0x001172B5
	private void Awake()
	{
		this.previousSize = new Vector2((float)Screen.width, (float)Screen.height);
	}

	// Token: 0x0600B325 RID: 45861 RVA: 0x00440658 File Offset: 0x0043E858
	private void Update()
	{
		if ((this.previousSize.x != (float)Screen.width || this.previousSize.y != (float)Screen.height) && Game.Instance != null)
		{
			Game.Instance.Trigger(445618876, null);
			this.previousSize.x = (float)Screen.width;
			this.previousSize.y = (float)Screen.height;
		}
		this.UpdateShouldUseGamepadUIMode();
	}

	// Token: 0x0600B326 RID: 45862 RVA: 0x001190CE File Offset: 0x001172CE
	public static bool UsingGamepadUIMode()
	{
		return ScreenResolutionMonitor.previousGamepadUIMode;
	}

	// Token: 0x0600B327 RID: 45863 RVA: 0x004406D0 File Offset: 0x0043E8D0
	private void UpdateShouldUseGamepadUIMode()
	{
		bool flag = (Screen.dpi > 130f && Screen.height < 900) || KInputManager.currentControllerIsGamepad;
		if (flag != ScreenResolutionMonitor.previousGamepadUIMode)
		{
			ScreenResolutionMonitor.previousGamepadUIMode = flag;
			if (Game.Instance == null)
			{
				return;
			}
			Game.Instance.Trigger(-442024484, null);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound(flag ? "ControllerType_ToggleOn" : "ControllerType_ToggleOff", false));
		}
	}

	// Token: 0x04008DCC RID: 36300
	[SerializeField]
	private Vector2 previousSize;

	// Token: 0x04008DCD RID: 36301
	private static bool previousGamepadUIMode;

	// Token: 0x04008DCE RID: 36302
	private const float HIGH_DPI = 130f;
}
