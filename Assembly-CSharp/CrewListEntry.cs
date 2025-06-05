using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001DF1 RID: 7665
[AddComponentMenu("KMonoBehaviour/scripts/CrewListEntry")]
public class CrewListEntry : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x17000A6E RID: 2670
	// (get) Token: 0x0600A036 RID: 41014 RVA: 0x0010CC0F File Offset: 0x0010AE0F
	public MinionIdentity Identity
	{
		get
		{
			return this.identity;
		}
	}

	// Token: 0x0600A037 RID: 41015 RVA: 0x0010CC17 File Offset: 0x0010AE17
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.mouseOver = true;
		this.BGImage.enabled = true;
		this.BorderHighlight.color = new Color(0.65882355f, 0.2901961f, 0.4745098f);
	}

	// Token: 0x0600A038 RID: 41016 RVA: 0x0010CC4B File Offset: 0x0010AE4B
	public void OnPointerExit(PointerEventData eventData)
	{
		this.mouseOver = false;
		this.BGImage.enabled = false;
		this.BorderHighlight.color = new Color(0.8f, 0.8f, 0.8f);
	}

	// Token: 0x0600A039 RID: 41017 RVA: 0x003E2F8C File Offset: 0x003E118C
	public void OnPointerClick(PointerEventData eventData)
	{
		bool focus = Time.unscaledTime - this.lastClickTime < 0.3f;
		this.SelectCrewMember(focus);
		this.lastClickTime = Time.unscaledTime;
	}

	// Token: 0x0600A03A RID: 41018 RVA: 0x003E2FC0 File Offset: 0x003E11C0
	public virtual void Populate(MinionIdentity _identity)
	{
		this.identity = _identity;
		if (this.portrait == null)
		{
			GameObject parent = (this.crewPortraitParent != null) ? this.crewPortraitParent : base.gameObject;
			this.portrait = Util.KInstantiateUI<CrewPortrait>(this.PortraitPrefab.gameObject, parent, false);
			if (this.crewPortraitParent == null)
			{
				this.portrait.transform.SetSiblingIndex(2);
			}
		}
		this.portrait.SetIdentityObject(_identity, true);
	}

	// Token: 0x0600A03B RID: 41019 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void Refresh()
	{
	}

	// Token: 0x0600A03C RID: 41020 RVA: 0x0010CC7F File Offset: 0x0010AE7F
	public void RefreshCrewPortraitContent()
	{
		if (this.portrait != null)
		{
			this.portrait.ForceRefresh();
		}
	}

	// Token: 0x0600A03D RID: 41021 RVA: 0x0010CC9A File Offset: 0x0010AE9A
	private string seniorityString()
	{
		return this.identity.GetAttributes().GetProfessionString(true);
	}

	// Token: 0x0600A03E RID: 41022 RVA: 0x003E3044 File Offset: 0x003E1244
	public void SelectCrewMember(bool focus)
	{
		if (focus)
		{
			SelectTool.Instance.SelectAndFocus(this.identity.transform.GetPosition(), this.identity.GetComponent<KSelectable>(), new Vector3(8f, 0f, 0f));
			return;
		}
		SelectTool.Instance.Select(this.identity.GetComponent<KSelectable>(), false);
	}

	// Token: 0x04007DE0 RID: 32224
	protected MinionIdentity identity;

	// Token: 0x04007DE1 RID: 32225
	protected CrewPortrait portrait;

	// Token: 0x04007DE2 RID: 32226
	public CrewPortrait PortraitPrefab;

	// Token: 0x04007DE3 RID: 32227
	public GameObject crewPortraitParent;

	// Token: 0x04007DE4 RID: 32228
	protected bool mouseOver;

	// Token: 0x04007DE5 RID: 32229
	public Image BorderHighlight;

	// Token: 0x04007DE6 RID: 32230
	public Image BGImage;

	// Token: 0x04007DE7 RID: 32231
	public float lastClickTime;
}
