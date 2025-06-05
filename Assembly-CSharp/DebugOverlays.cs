using System;

// Token: 0x02001B16 RID: 6934
public class DebugOverlays : KScreen
{
	// Token: 0x170009A3 RID: 2467
	// (get) Token: 0x06009149 RID: 37193 RVA: 0x00103694 File Offset: 0x00101894
	// (set) Token: 0x0600914A RID: 37194 RVA: 0x0010369B File Offset: 0x0010189B
	public static DebugOverlays instance { get; private set; }

	// Token: 0x0600914B RID: 37195 RVA: 0x0038CAFC File Offset: 0x0038ACFC
	protected override void OnPrefabInit()
	{
		DebugOverlays.instance = this;
		KPopupMenu componentInChildren = base.GetComponentInChildren<KPopupMenu>();
		componentInChildren.SetOptions(new string[]
		{
			"None",
			"Rooms",
			"Lighting",
			"Style",
			"Flow"
		});
		KPopupMenu kpopupMenu = componentInChildren;
		kpopupMenu.OnSelect = (Action<string, int>)Delegate.Combine(kpopupMenu.OnSelect, new Action<string, int>(this.OnSelect));
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600914C RID: 37196 RVA: 0x0038CB78 File Offset: 0x0038AD78
	private void OnSelect(string str, int index)
	{
		if (str == "None")
		{
			SimDebugView.Instance.SetMode(OverlayModes.None.ID);
			return;
		}
		if (str == "Flow")
		{
			SimDebugView.Instance.SetMode(SimDebugView.OverlayModes.Flow);
			return;
		}
		if (str == "Lighting")
		{
			SimDebugView.Instance.SetMode(OverlayModes.Light.ID);
			return;
		}
		if (!(str == "Rooms"))
		{
			Debug.LogError("Unknown debug view: " + str);
			return;
		}
		SimDebugView.Instance.SetMode(OverlayModes.Rooms.ID);
	}
}
