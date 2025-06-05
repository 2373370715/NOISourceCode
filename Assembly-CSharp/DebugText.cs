using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001246 RID: 4678
[AddComponentMenu("KMonoBehaviour/scripts/DebugText")]
public class DebugText : KMonoBehaviour
{
	// Token: 0x06005F2E RID: 24366 RVA: 0x000E29A1 File Offset: 0x000E0BA1
	public static void DestroyInstance()
	{
		DebugText.Instance = null;
	}

	// Token: 0x06005F2F RID: 24367 RVA: 0x000E29A9 File Offset: 0x000E0BA9
	protected override void OnPrefabInit()
	{
		DebugText.Instance = this;
	}

	// Token: 0x06005F30 RID: 24368 RVA: 0x002B4190 File Offset: 0x002B2390
	public void Draw(string text, Vector3 pos, Color color)
	{
		DebugText.Entry item = new DebugText.Entry
		{
			text = text,
			pos = pos,
			color = color
		};
		this.entries.Add(item);
	}

	// Token: 0x06005F31 RID: 24369 RVA: 0x002B41CC File Offset: 0x002B23CC
	private void LateUpdate()
	{
		foreach (Text text in this.texts)
		{
			UnityEngine.Object.Destroy(text.gameObject);
		}
		this.texts.Clear();
		foreach (DebugText.Entry entry in this.entries)
		{
			GameObject gameObject = new GameObject();
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.GetComponent<RectTransform>());
			gameObject.transform.SetPosition(entry.pos);
			rectTransform.localScale = new Vector3(0.02f, 0.02f, 1f);
			Text text2 = gameObject.AddComponent<Text>();
			text2.font = Assets.DebugFont;
			text2.text = entry.text;
			text2.color = entry.color;
			text2.horizontalOverflow = HorizontalWrapMode.Overflow;
			text2.verticalOverflow = VerticalWrapMode.Overflow;
			text2.alignment = TextAnchor.MiddleCenter;
			this.texts.Add(text2);
		}
		this.entries.Clear();
	}

	// Token: 0x040043F5 RID: 17397
	public static DebugText Instance;

	// Token: 0x040043F6 RID: 17398
	private List<DebugText.Entry> entries = new List<DebugText.Entry>();

	// Token: 0x040043F7 RID: 17399
	private List<Text> texts = new List<Text>();

	// Token: 0x02001247 RID: 4679
	private struct Entry
	{
		// Token: 0x040043F8 RID: 17400
		public string text;

		// Token: 0x040043F9 RID: 17401
		public Vector3 pos;

		// Token: 0x040043FA RID: 17402
		public Color color;
	}
}
