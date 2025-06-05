using System;
using UnityEngine;

// Token: 0x02001A38 RID: 6712
public class Thought : Resource
{
	// Token: 0x06008BD6 RID: 35798 RVA: 0x0036F9FC File Offset: 0x0036DBFC
	public Thought(string id, ResourceSet parent, Sprite icon, string mode_icon, string sound_name, string bubble, string speech_prefix, LocString hover_text, bool show_immediately = false, float show_time = 4f) : base(id, parent, null)
	{
		this.sprite = icon;
		if (mode_icon != null)
		{
			this.modeSprite = Assets.GetSprite(mode_icon);
		}
		this.bubbleSprite = Assets.GetSprite(bubble);
		this.sound = sound_name;
		this.speechPrefix = speech_prefix;
		this.hoverText = hover_text;
		this.showImmediately = show_immediately;
		this.showTime = show_time;
	}

	// Token: 0x06008BD7 RID: 35799 RVA: 0x0036FA6C File Offset: 0x0036DC6C
	public Thought(string id, ResourceSet parent, string icon, string mode_icon, string sound_name, string bubble, string speech_prefix, LocString hover_text, bool show_immediately = false, float show_time = 4f) : base(id, parent, null)
	{
		this.sprite = Assets.GetSprite(icon);
		if (mode_icon != null)
		{
			this.modeSprite = Assets.GetSprite(mode_icon);
		}
		this.bubbleSprite = Assets.GetSprite(bubble);
		this.sound = sound_name;
		this.speechPrefix = speech_prefix;
		this.hoverText = hover_text;
		this.showImmediately = show_immediately;
		this.showTime = show_time;
	}

	// Token: 0x0400698D RID: 27021
	public int priority;

	// Token: 0x0400698E RID: 27022
	public Sprite sprite;

	// Token: 0x0400698F RID: 27023
	public Sprite modeSprite;

	// Token: 0x04006990 RID: 27024
	public string sound;

	// Token: 0x04006991 RID: 27025
	public Sprite bubbleSprite;

	// Token: 0x04006992 RID: 27026
	public string speechPrefix;

	// Token: 0x04006993 RID: 27027
	public LocString hoverText;

	// Token: 0x04006994 RID: 27028
	public bool showImmediately;

	// Token: 0x04006995 RID: 27029
	public float showTime;
}
