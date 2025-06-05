using System;
using UnityEngine.UI;

// Token: 0x02001CCD RID: 7373
public class ControlsScreen : KScreen
{
	// Token: 0x060099D5 RID: 39381 RVA: 0x003C5340 File Offset: 0x003C3540
	protected override void OnPrefabInit()
	{
		BindingEntry[] bindingEntries = GameInputMapping.GetBindingEntries();
		string text = "";
		foreach (BindingEntry bindingEntry in bindingEntries)
		{
			text += bindingEntry.mAction.ToString();
			text += ": ";
			text += bindingEntry.mKeyCode.ToString();
			text += "\n";
		}
		this.controlLabel.text = text;
	}

	// Token: 0x060099D6 RID: 39382 RVA: 0x001087B1 File Offset: 0x001069B1
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Help) || e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
		}
	}

	// Token: 0x04007805 RID: 30725
	public Text controlLabel;
}
