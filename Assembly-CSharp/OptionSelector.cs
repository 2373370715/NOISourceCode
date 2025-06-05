using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020020D3 RID: 8403
public class OptionSelector : MonoBehaviour
{
	// Token: 0x0600B31E RID: 45854 RVA: 0x00119056 File Offset: 0x00117256
	private void Start()
	{
		this.selectedItem.GetComponent<KButton>().onBtnClick += this.OnClick;
	}

	// Token: 0x0600B31F RID: 45855 RVA: 0x00119074 File Offset: 0x00117274
	public void Initialize(object id)
	{
		this.id = id;
	}

	// Token: 0x0600B320 RID: 45856 RVA: 0x0011907D File Offset: 0x0011727D
	private void OnClick(KKeyCode button)
	{
		if (button == KKeyCode.Mouse0)
		{
			this.OnChangePriority(this.id, 1);
			return;
		}
		if (button != KKeyCode.Mouse1)
		{
			return;
		}
		this.OnChangePriority(this.id, -1);
	}

	// Token: 0x0600B321 RID: 45857 RVA: 0x0044056C File Offset: 0x0043E76C
	public void ConfigureItem(bool disabled, OptionSelector.DisplayOptionInfo display_info)
	{
		HierarchyReferences component = this.selectedItem.GetComponent<HierarchyReferences>();
		KImage kimage = component.GetReference("BG") as KImage;
		if (display_info.bgOptions == null)
		{
			kimage.gameObject.SetActive(false);
		}
		else
		{
			kimage.sprite = display_info.bgOptions[display_info.bgIndex];
		}
		KImage kimage2 = component.GetReference("FG") as KImage;
		if (display_info.fgOptions == null)
		{
			kimage2.gameObject.SetActive(false);
		}
		else
		{
			kimage2.sprite = display_info.fgOptions[display_info.fgIndex];
		}
		KImage kimage3 = component.GetReference("Fill") as KImage;
		if (kimage3 != null)
		{
			kimage3.enabled = !disabled;
			kimage3.color = display_info.fillColour;
		}
		KImage kimage4 = component.GetReference("Outline") as KImage;
		if (kimage4 != null)
		{
			kimage4.enabled = !disabled;
		}
	}

	// Token: 0x04008DC3 RID: 36291
	private object id;

	// Token: 0x04008DC4 RID: 36292
	public Action<object, int> OnChangePriority;

	// Token: 0x04008DC5 RID: 36293
	[SerializeField]
	private KImage selectedItem;

	// Token: 0x04008DC6 RID: 36294
	[SerializeField]
	private KImage itemTemplate;

	// Token: 0x020020D4 RID: 8404
	public class DisplayOptionInfo
	{
		// Token: 0x04008DC7 RID: 36295
		public IList<Sprite> bgOptions;

		// Token: 0x04008DC8 RID: 36296
		public IList<Sprite> fgOptions;

		// Token: 0x04008DC9 RID: 36297
		public int bgIndex;

		// Token: 0x04008DCA RID: 36298
		public int fgIndex;

		// Token: 0x04008DCB RID: 36299
		public Color32 fillColour;
	}
}
