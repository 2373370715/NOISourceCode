using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

// Token: 0x02001B15 RID: 6933
public class CreditsScreen : KModalScreen
{
	// Token: 0x06009145 RID: 37189 RVA: 0x0038C9A0 File Offset: 0x0038ABA0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (TextAsset csv in this.creditsFiles)
		{
			this.AddCredits(csv);
		}
		this.CloseButton.onClick += this.Close;
	}

	// Token: 0x06009146 RID: 37190 RVA: 0x00103679 File Offset: 0x00101879
	public void Close()
	{
		this.Deactivate();
	}

	// Token: 0x06009147 RID: 37191 RVA: 0x0038C9EC File Offset: 0x0038ABEC
	private void AddCredits(TextAsset csv)
	{
		string[,] array = CSVReader.SplitCsvGrid(csv.text, csv.name);
		List<string> list = new List<string>();
		for (int i = 1; i < array.GetLength(1); i++)
		{
			string text = string.Format("{0} {1}", array[0, i], array[1, i]);
			if (!(text == " "))
			{
				list.Add(text);
			}
		}
		list.Shuffle<string>();
		string text2 = array[0, 0];
		GameObject gameObject = Util.KInstantiateUI(this.teamHeaderPrefab, this.entryContainer.gameObject, true);
		gameObject.GetComponent<LocText>().text = text2;
		this.teamContainers.Add(text2, gameObject);
		foreach (string text3 in list)
		{
			Util.KInstantiateUI(this.entryPrefab, this.teamContainers[text2], true).GetComponent<LocText>().text = text3;
		}
	}

	// Token: 0x04006DEA RID: 28138
	public GameObject entryPrefab;

	// Token: 0x04006DEB RID: 28139
	public GameObject teamHeaderPrefab;

	// Token: 0x04006DEC RID: 28140
	private Dictionary<string, GameObject> teamContainers = new Dictionary<string, GameObject>();

	// Token: 0x04006DED RID: 28141
	public Transform entryContainer;

	// Token: 0x04006DEE RID: 28142
	public KButton CloseButton;

	// Token: 0x04006DEF RID: 28143
	public TextAsset[] creditsFiles;
}
