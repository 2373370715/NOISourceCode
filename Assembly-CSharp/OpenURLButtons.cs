using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

// Token: 0x02001EBC RID: 7868
[AddComponentMenu("KMonoBehaviour/scripts/OpenURLButtons")]
public class OpenURLButtons : KMonoBehaviour
{
	// Token: 0x0600A518 RID: 42264 RVA: 0x003F8304 File Offset: 0x003F6504
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		for (int i = 0; i < this.buttonData.Count; i++)
		{
			OpenURLButtons.URLButtonData data = this.buttonData[i];
			GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, base.gameObject, true);
			string text = Strings.Get(data.stringKey);
			gameObject.GetComponentInChildren<LocText>().SetText(text);
			switch (data.urlType)
			{
			case OpenURLButtons.URLButtonType.url:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenURL(data.url);
				};
				break;
			case OpenURLButtons.URLButtonType.platformUrl:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenPlatformURL(data.url);
				};
				break;
			case OpenURLButtons.URLButtonType.patchNotes:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenPatchNotes();
				};
				break;
			case OpenURLButtons.URLButtonType.feedbackScreen:
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenFeedbackScreen();
				};
				break;
			}
		}
	}

	// Token: 0x0600A519 RID: 42265 RVA: 0x0010F8E4 File Offset: 0x0010DAE4
	public void OpenPatchNotes()
	{
		Util.KInstantiateUI(this.patchNotesScreenPrefab, FrontEndManager.Instance.gameObject, true);
	}

	// Token: 0x0600A51A RID: 42266 RVA: 0x0010F8FD File Offset: 0x0010DAFD
	public void OpenFeedbackScreen()
	{
		Util.KInstantiateUI(this.feedbackScreenPrefab.gameObject, FrontEndManager.Instance.gameObject, true);
	}

	// Token: 0x0600A51B RID: 42267 RVA: 0x0010F91B File Offset: 0x0010DB1B
	public void OpenURL(string URL)
	{
		App.OpenWebURL(URL);
	}

	// Token: 0x0600A51C RID: 42268 RVA: 0x003F8410 File Offset: 0x003F6610
	public void OpenPlatformURL(string URL)
	{
		if (DistributionPlatform.Inst.Platform == "Steam" && DistributionPlatform.Inst.Initialized)
		{
			DistributionPlatform.Inst.GetAuthTicket(delegate(byte[] ticket)
			{
				string newValue = string.Concat(Array.ConvertAll<byte, string>(ticket, (byte x) => x.ToString("X2")));
				App.OpenWebURL(URL.Replace("{SteamID}", DistributionPlatform.Inst.LocalUser.Id.ToInt64().ToString()).Replace("{SteamTicket}", newValue));
			});
			return;
		}
		string value = URL.Replace("{SteamID}", "").Replace("{SteamTicket}", "");
		App.OpenWebURL("https://accounts.klei.com/login?goto={gotoUrl}".Replace("{gotoUrl}", WebUtility.HtmlEncode(value)));
	}

	// Token: 0x04008118 RID: 33048
	public GameObject buttonPrefab;

	// Token: 0x04008119 RID: 33049
	public List<OpenURLButtons.URLButtonData> buttonData;

	// Token: 0x0400811A RID: 33050
	[SerializeField]
	private GameObject patchNotesScreenPrefab;

	// Token: 0x0400811B RID: 33051
	[SerializeField]
	private FeedbackScreen feedbackScreenPrefab;

	// Token: 0x02001EBD RID: 7869
	public enum URLButtonType
	{
		// Token: 0x0400811D RID: 33053
		url,
		// Token: 0x0400811E RID: 33054
		platformUrl,
		// Token: 0x0400811F RID: 33055
		patchNotes,
		// Token: 0x04008120 RID: 33056
		feedbackScreen
	}

	// Token: 0x02001EBE RID: 7870
	[Serializable]
	public class URLButtonData
	{
		// Token: 0x04008121 RID: 33057
		public string stringKey;

		// Token: 0x04008122 RID: 33058
		public OpenURLButtons.URLButtonType urlType;

		// Token: 0x04008123 RID: 33059
		public string url;
	}
}
