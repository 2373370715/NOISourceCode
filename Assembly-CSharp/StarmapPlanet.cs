using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200207E RID: 8318
[AddComponentMenu("KMonoBehaviour/scripts/StarmapPlanet")]
public class StarmapPlanet : KMonoBehaviour
{
	// Token: 0x0600B112 RID: 45330 RVA: 0x00434E24 File Offset: 0x00433024
	public void SetSprite(Sprite sprite, Color color)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.image.sprite = sprite;
			starmapPlanetVisualizer.image.color = color;
		}
	}

	// Token: 0x0600B113 RID: 45331 RVA: 0x00434E88 File Offset: 0x00433088
	public void SetFillAmount(float amount)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.image.fillAmount = amount;
		}
	}

	// Token: 0x0600B114 RID: 45332 RVA: 0x00434EE0 File Offset: 0x004330E0
	public void SetUnknownBGActive(bool active, Color color)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.unknownBG.gameObject.SetActive(active);
			starmapPlanetVisualizer.unknownBG.color = color;
		}
	}

	// Token: 0x0600B115 RID: 45333 RVA: 0x00434F48 File Offset: 0x00433148
	public void SetSelectionActive(bool active)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.selection.gameObject.SetActive(active);
		}
	}

	// Token: 0x0600B116 RID: 45334 RVA: 0x00434FA4 File Offset: 0x004331A4
	public void SetAnalysisActive(bool active)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.analysisSelection.SetActive(active);
		}
	}

	// Token: 0x0600B117 RID: 45335 RVA: 0x00434FFC File Offset: 0x004331FC
	public void SetLabel(string text)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.label.text = text;
			this.ShowLabel(false);
		}
	}

	// Token: 0x0600B118 RID: 45336 RVA: 0x0043505C File Offset: 0x0043325C
	public void ShowLabel(bool show)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.label.gameObject.SetActive(show);
		}
	}

	// Token: 0x0600B119 RID: 45337 RVA: 0x004350B8 File Offset: 0x004332B8
	public void SetOnClick(System.Action del)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.button.onClick = del;
		}
	}

	// Token: 0x0600B11A RID: 45338 RVA: 0x00435110 File Offset: 0x00433310
	public void SetOnEnter(System.Action del)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.button.onEnter = del;
		}
	}

	// Token: 0x0600B11B RID: 45339 RVA: 0x00435168 File Offset: 0x00433368
	public void SetOnExit(System.Action del)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.button.onExit = del;
		}
	}

	// Token: 0x0600B11C RID: 45340 RVA: 0x004351C0 File Offset: 0x004333C0
	public void AnimateSelector(float time)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			starmapPlanetVisualizer.selection.anchoredPosition = new Vector2(0f, 25f + Mathf.Sin(time * 4f) * 5f);
		}
	}

	// Token: 0x0600B11D RID: 45341 RVA: 0x00435238 File Offset: 0x00433438
	public void ShowAsCurrentRocketDestination(bool show)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			RectTransform rectTransform = starmapPlanetVisualizer.rocketIconContainer.rectTransform();
			if (rectTransform.childCount > 0)
			{
				rectTransform.GetChild(rectTransform.childCount - 1).GetComponent<HierarchyReferences>().GetReference<Image>("fg").color = (show ? new Color(0.11764706f, 0.8627451f, 0.3137255f) : Color.white);
			}
		}
	}

	// Token: 0x0600B11E RID: 45342 RVA: 0x004352D8 File Offset: 0x004334D8
	public void SetRocketIcons(int numRockets, GameObject iconPrefab)
	{
		foreach (StarmapPlanetVisualizer starmapPlanetVisualizer in this.visualizers)
		{
			RectTransform rectTransform = starmapPlanetVisualizer.rocketIconContainer.rectTransform();
			for (int i = rectTransform.childCount; i < numRockets; i++)
			{
				Util.KInstantiateUI(iconPrefab, starmapPlanetVisualizer.rocketIconContainer, true);
			}
			for (int j = rectTransform.childCount; j > numRockets; j--)
			{
				UnityEngine.Object.Destroy(rectTransform.GetChild(j - 1).gameObject);
			}
			int num = 0;
			foreach (object obj in rectTransform)
			{
				((RectTransform)obj).anchoredPosition = new Vector2((float)num * -10f, 0f);
				num++;
			}
		}
	}

	// Token: 0x04008B77 RID: 35703
	public List<StarmapPlanetVisualizer> visualizers;
}
