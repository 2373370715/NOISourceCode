using System;
using UnityEngine;

// Token: 0x020020D1 RID: 8401
[Serializable]
public struct ToggleState
{
	// Token: 0x04008DB0 RID: 36272
	public string Name;

	// Token: 0x04008DB1 RID: 36273
	public string on_click_override_sound_path;

	// Token: 0x04008DB2 RID: 36274
	public string on_release_override_sound_path;

	// Token: 0x04008DB3 RID: 36275
	public string sound_parameter_name;

	// Token: 0x04008DB4 RID: 36276
	public float sound_parameter_value;

	// Token: 0x04008DB5 RID: 36277
	public bool has_sound_parameter;

	// Token: 0x04008DB6 RID: 36278
	public Sprite sprite;

	// Token: 0x04008DB7 RID: 36279
	public Color color;

	// Token: 0x04008DB8 RID: 36280
	public Color color_on_hover;

	// Token: 0x04008DB9 RID: 36281
	public bool use_color_on_hover;

	// Token: 0x04008DBA RID: 36282
	public bool use_rect_margins;

	// Token: 0x04008DBB RID: 36283
	public Vector2 rect_margins;

	// Token: 0x04008DBC RID: 36284
	public StatePresentationSetting[] additional_display_settings;
}
