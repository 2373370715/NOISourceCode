using System;
using System.Linq;
using UnityEngine;

// Token: 0x02001DE4 RID: 7652
[Serializable]
public class MainMenu_Motd
{
	// Token: 0x06009FFD RID: 40957 RVA: 0x003E1988 File Offset: 0x003DFB88
	public void Setup()
	{
		this.CleanUp();
		this.boxA.gameObject.SetActive(false);
		this.boxB.gameObject.SetActive(false);
		this.boxC.gameObject.SetActive(false);
		this.motdDataFetchRequest = new MotdDataFetchRequest();
		this.motdDataFetchRequest.Fetch(MotdDataFetchRequest.BuildUrl());
		this.motdDataFetchRequest.OnComplete(delegate(MotdData motdData)
		{
			this.RecieveMotdData(motdData);
		});
	}

	// Token: 0x06009FFE RID: 40958 RVA: 0x0010CA4F File Offset: 0x0010AC4F
	public void CleanUp()
	{
		if (this.motdDataFetchRequest != null)
		{
			this.motdDataFetchRequest.Dispose();
			this.motdDataFetchRequest = null;
		}
	}

	// Token: 0x06009FFF RID: 40959 RVA: 0x003E1A00 File Offset: 0x003DFC00
	private void RecieveMotdData(MotdData motdData)
	{
		MainMenu_Motd.<>c__DisplayClass6_0 CS$<>8__locals1 = new MainMenu_Motd.<>c__DisplayClass6_0();
		CS$<>8__locals1.<>4__this = this;
		if (motdData == null || motdData.boxesLive == null || motdData.boxesLive.Count == 0)
		{
			global::Debug.LogWarning("MOTD Error: failed to get valid motd data, hiding ui.");
			this.boxA.gameObject.SetActive(false);
			this.boxB.gameObject.SetActive(false);
			this.boxC.gameObject.SetActive(false);
			return;
		}
		CS$<>8__locals1.boxes = motdData.boxesLive.StableSort((MotdData_Box a, MotdData_Box b) => CS$<>8__locals1.<>4__this.CalcScore(a).CompareTo(CS$<>8__locals1.<>4__this.CalcScore(b))).ToList<MotdData_Box>();
		MotdData_Box motdData_Box = CS$<>8__locals1.<RecieveMotdData>g__ConsumeBox|1("PatchNotes");
		MotdData_Box motdData_Box2 = CS$<>8__locals1.<RecieveMotdData>g__ConsumeBox|1("News");
		MotdData_Box motdData_Box3 = CS$<>8__locals1.<RecieveMotdData>g__ConsumeBox|1("Skins");
		if (motdData_Box != null)
		{
			this.boxA.Config(new MotdBox.PageData[]
			{
				this.ConvertToPageData(motdData_Box)
			});
			this.boxA.gameObject.SetActive(true);
		}
		if (motdData_Box2 != null)
		{
			this.boxB.Config(new MotdBox.PageData[]
			{
				this.ConvertToPageData(motdData_Box2)
			});
			this.boxB.gameObject.SetActive(true);
		}
		if (motdData_Box3 != null)
		{
			this.boxC.Config(new MotdBox.PageData[]
			{
				this.ConvertToPageData(motdData_Box3)
			});
			this.boxC.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600A000 RID: 40960 RVA: 0x000B1628 File Offset: 0x000AF828
	private int CalcScore(MotdData_Box box)
	{
		return 0;
	}

	// Token: 0x0600A001 RID: 40961 RVA: 0x0010CA6B File Offset: 0x0010AC6B
	private MotdBox.PageData ConvertToPageData(MotdData_Box box)
	{
		return new MotdBox.PageData
		{
			Texture = box.resolvedImage,
			HeaderText = box.title,
			ImageText = box.text,
			URL = box.href
		};
	}

	// Token: 0x04007DA8 RID: 32168
	[SerializeField]
	private MotdBox boxA;

	// Token: 0x04007DA9 RID: 32169
	[SerializeField]
	private MotdBox boxB;

	// Token: 0x04007DAA RID: 32170
	[SerializeField]
	private MotdBox boxC;

	// Token: 0x04007DAB RID: 32171
	private MotdDataFetchRequest motdDataFetchRequest;
}
