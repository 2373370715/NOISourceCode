using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001CF4 RID: 7412
[AddComponentMenu("KMonoBehaviour/scripts/DescriptorPanel")]
public class DescriptorPanel : KMonoBehaviour
{
	// Token: 0x06009AAA RID: 39594 RVA: 0x00109220 File Offset: 0x00107420
	public bool HasDescriptors()
	{
		return this.labels.Count > 0;
	}

	// Token: 0x06009AAB RID: 39595 RVA: 0x003C8A2C File Offset: 0x003C6C2C
	public void SetDescriptors(IList<Descriptor> descriptors)
	{
		int i;
		for (i = 0; i < descriptors.Count; i++)
		{
			GameObject gameObject;
			if (i >= this.labels.Count)
			{
				gameObject = Util.KInstantiate((this.customLabelPrefab != null) ? this.customLabelPrefab : ScreenPrefabs.Instance.DescriptionLabel, base.gameObject, null);
				gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				this.labels.Add(gameObject);
			}
			else
			{
				gameObject = this.labels[i];
			}
			gameObject.GetComponent<LocText>().text = descriptors[i].IndentedText();
			gameObject.GetComponent<ToolTip>().toolTip = descriptors[i].tooltipText;
			gameObject.SetActive(true);
		}
		while (i < this.labels.Count)
		{
			this.labels[i].SetActive(false);
			i++;
		}
	}

	// Token: 0x040078C6 RID: 30918
	[SerializeField]
	private GameObject customLabelPrefab;

	// Token: 0x040078C7 RID: 30919
	private List<GameObject> labels = new List<GameObject>();
}
