using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001A00 RID: 6656
public class StatusItem : Resource
{
	// Token: 0x06008A9C RID: 35484 RVA: 0x0036AC3C File Offset: 0x00368E3C
	private StatusItem(string id, string composed_prefix) : base(id, Strings.Get(composed_prefix + ".NAME"))
	{
		this.composedPrefix = composed_prefix;
		this.tooltipText = Strings.Get(composed_prefix + ".TOOLTIP");
	}

	// Token: 0x06008A9D RID: 35485 RVA: 0x0036AC90 File Offset: 0x00368E90
	private void SetIcon(string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool show_world_icon = true, int status_overlays = 129022, Func<string, object, string> resolve_string_callback = null)
	{
		switch (icon_type)
		{
		case StatusItem.IconType.Info:
			icon = "dash";
			break;
		case StatusItem.IconType.Exclamation:
			icon = "status_item_exclamation";
			break;
		}
		this.iconName = icon;
		this.notificationType = notification_type;
		this.sprite = Assets.GetTintedSprite(icon);
		if (this.sprite == null)
		{
			this.sprite = new TintedSprite();
			this.sprite.sprite = Assets.GetSprite(icon);
			this.sprite.color = new Color(0f, 0f, 0f, 255f);
		}
		this.iconType = icon_type;
		this.allowMultiples = allow_multiples;
		this.render_overlay = render_overlay;
		this.showShowWorldIcon = show_world_icon;
		this.status_overlays = status_overlays;
		this.resolveStringCallback = resolve_string_callback;
		if (this.sprite == null)
		{
			global::Debug.LogWarning("Status item '" + this.Id + "' references a missing icon: " + icon);
		}
	}

	// Token: 0x06008A9E RID: 35486 RVA: 0x0036AD7C File Offset: 0x00368F7C
	public StatusItem(string id, string prefix, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool showWorldIcon = true, int status_overlays = 129022, Func<string, object, string> resolve_string_callback = null) : this(id, "STRINGS." + prefix + ".STATUSITEMS." + id.ToUpper())
	{
		this.SetIcon(icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, resolve_string_callback);
	}

	// Token: 0x06008A9F RID: 35487 RVA: 0x0036ADBC File Offset: 0x00368FBC
	public StatusItem(string id, string name, string tooltip, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, int status_overlays = 129022, bool showWorldIcon = true, Func<string, object, string> resolve_string_callback = null) : base(id, name)
	{
		this.tooltipText = tooltip;
		this.SetIcon(icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, resolve_string_callback);
	}

	// Token: 0x06008AA0 RID: 35488 RVA: 0x0036ADF8 File Offset: 0x00368FF8
	public void AddNotification(string sound_path = null, string notification_text = null, string notification_tooltip = null)
	{
		this.shouldNotify = true;
		if (sound_path == null)
		{
			if (this.notificationType == NotificationType.Bad)
			{
				this.soundPath = "Warning";
			}
			else
			{
				this.soundPath = "Notification";
			}
		}
		else
		{
			this.soundPath = sound_path;
		}
		if (notification_text != null)
		{
			this.notificationText = notification_text;
		}
		else
		{
			DebugUtil.Assert(this.composedPrefix != null, "When adding a notification, either set the status prefix or specify strings!");
			this.notificationText = Strings.Get(this.composedPrefix + ".NOTIFICATION_NAME");
		}
		if (notification_tooltip != null)
		{
			this.notificationTooltipText = notification_tooltip;
			return;
		}
		DebugUtil.Assert(this.composedPrefix != null, "When adding a notification, either set the status prefix or specify strings!");
		this.notificationTooltipText = Strings.Get(this.composedPrefix + ".NOTIFICATION_TOOLTIP");
	}

	// Token: 0x06008AA1 RID: 35489 RVA: 0x000FF209 File Offset: 0x000FD409
	public virtual string GetName(object data)
	{
		return this.ResolveString(this.Name, data);
	}

	// Token: 0x06008AA2 RID: 35490 RVA: 0x000FF218 File Offset: 0x000FD418
	public virtual string GetTooltip(object data)
	{
		return this.ResolveTooltip(this.tooltipText, data);
	}

	// Token: 0x06008AA3 RID: 35491 RVA: 0x000FF227 File Offset: 0x000FD427
	private string ResolveString(string str, object data)
	{
		if (this.resolveStringCallback != null && (data != null || this.resolveStringCallback_shouldStillCallIfDataIsNull))
		{
			return this.resolveStringCallback(str, data);
		}
		return str;
	}

	// Token: 0x06008AA4 RID: 35492 RVA: 0x0036AEB8 File Offset: 0x003690B8
	private string ResolveTooltip(string str, object data)
	{
		if (data != null)
		{
			if (this.resolveTooltipCallback != null)
			{
				return this.resolveTooltipCallback(str, data);
			}
			if (this.resolveStringCallback != null)
			{
				return this.resolveStringCallback(str, data);
			}
		}
		else
		{
			if (this.resolveStringCallback_shouldStillCallIfDataIsNull && this.resolveStringCallback != null)
			{
				return this.resolveStringCallback(str, data);
			}
			if (this.resolveTooltipCallback_shouldStillCallIfDataIsNull && this.resolveTooltipCallback != null)
			{
				return this.resolveTooltipCallback(str, data);
			}
		}
		return str;
	}

	// Token: 0x06008AA5 RID: 35493 RVA: 0x000FF24B File Offset: 0x000FD44B
	public bool ShouldShowIcon()
	{
		return this.iconType == StatusItem.IconType.Custom && this.showShowWorldIcon;
	}

	// Token: 0x06008AA6 RID: 35494 RVA: 0x0036AF34 File Offset: 0x00369134
	public virtual void ShowToolTip(ToolTip tooltip_widget, object data, TextStyleSetting property_style)
	{
		tooltip_widget.ClearMultiStringTooltip();
		string tooltip = this.GetTooltip(data);
		tooltip_widget.AddMultiStringTooltip(tooltip, property_style);
	}

	// Token: 0x06008AA7 RID: 35495 RVA: 0x000FF25E File Offset: 0x000FD45E
	public void SetIcon(Image image, object data)
	{
		if (this.sprite == null)
		{
			return;
		}
		image.color = this.sprite.color;
		image.sprite = this.sprite.sprite;
	}

	// Token: 0x06008AA8 RID: 35496 RVA: 0x000FF28B File Offset: 0x000FD48B
	public bool UseConditionalCallback(HashedString overlay, Transform transform)
	{
		return overlay != OverlayModes.None.ID && this.conditionalOverlayCallback != null && this.conditionalOverlayCallback(overlay, transform);
	}

	// Token: 0x06008AA9 RID: 35497 RVA: 0x000FF2B1 File Offset: 0x000FD4B1
	public StatusItem SetResolveStringCallback(Func<string, object, string> cb)
	{
		this.resolveStringCallback = cb;
		return this;
	}

	// Token: 0x06008AAA RID: 35498 RVA: 0x000FF2BB File Offset: 0x000FD4BB
	public void OnClick(object data)
	{
		if (this.statusItemClickCallback != null)
		{
			this.statusItemClickCallback(data);
		}
	}

	// Token: 0x06008AAB RID: 35499 RVA: 0x0036AF58 File Offset: 0x00369158
	public static StatusItem.StatusItemOverlays GetStatusItemOverlayBySimViewMode(HashedString mode)
	{
		StatusItem.StatusItemOverlays result;
		if (!StatusItem.overlayBitfieldMap.TryGetValue(mode, out result))
		{
			string str = "ViewMode ";
			HashedString hashedString = mode;
			global::Debug.LogWarning(str + hashedString.ToString() + " has no StatusItemOverlay value");
			result = StatusItem.StatusItemOverlays.None;
		}
		return result;
	}

	// Token: 0x04006885 RID: 26757
	public string tooltipText;

	// Token: 0x04006886 RID: 26758
	public string notificationText;

	// Token: 0x04006887 RID: 26759
	public string notificationTooltipText;

	// Token: 0x04006888 RID: 26760
	public string soundPath;

	// Token: 0x04006889 RID: 26761
	public string iconName;

	// Token: 0x0400688A RID: 26762
	public bool unique;

	// Token: 0x0400688B RID: 26763
	public TintedSprite sprite;

	// Token: 0x0400688C RID: 26764
	public bool shouldNotify;

	// Token: 0x0400688D RID: 26765
	public StatusItem.IconType iconType;

	// Token: 0x0400688E RID: 26766
	public NotificationType notificationType;

	// Token: 0x0400688F RID: 26767
	public Notification.ClickCallback notificationClickCallback;

	// Token: 0x04006890 RID: 26768
	public Func<string, object, string> resolveStringCallback;

	// Token: 0x04006891 RID: 26769
	public Func<string, object, string> resolveTooltipCallback;

	// Token: 0x04006892 RID: 26770
	public bool resolveStringCallback_shouldStillCallIfDataIsNull;

	// Token: 0x04006893 RID: 26771
	public bool resolveTooltipCallback_shouldStillCallIfDataIsNull;

	// Token: 0x04006894 RID: 26772
	public bool allowMultiples;

	// Token: 0x04006895 RID: 26773
	public Func<HashedString, object, bool> conditionalOverlayCallback;

	// Token: 0x04006896 RID: 26774
	public HashedString render_overlay;

	// Token: 0x04006897 RID: 26775
	public int status_overlays;

	// Token: 0x04006898 RID: 26776
	public Action<object> statusItemClickCallback;

	// Token: 0x04006899 RID: 26777
	private string composedPrefix;

	// Token: 0x0400689A RID: 26778
	private bool showShowWorldIcon = true;

	// Token: 0x0400689B RID: 26779
	public const int ALL_OVERLAYS = 129022;

	// Token: 0x0400689C RID: 26780
	private static Dictionary<HashedString, StatusItem.StatusItemOverlays> overlayBitfieldMap = new Dictionary<HashedString, StatusItem.StatusItemOverlays>
	{
		{
			OverlayModes.None.ID,
			StatusItem.StatusItemOverlays.None
		},
		{
			OverlayModes.Power.ID,
			StatusItem.StatusItemOverlays.PowerMap
		},
		{
			OverlayModes.Temperature.ID,
			StatusItem.StatusItemOverlays.Temperature
		},
		{
			OverlayModes.ThermalConductivity.ID,
			StatusItem.StatusItemOverlays.ThermalComfort
		},
		{
			OverlayModes.Light.ID,
			StatusItem.StatusItemOverlays.Light
		},
		{
			OverlayModes.LiquidConduits.ID,
			StatusItem.StatusItemOverlays.LiquidPlumbing
		},
		{
			OverlayModes.GasConduits.ID,
			StatusItem.StatusItemOverlays.GasPlumbing
		},
		{
			OverlayModes.SolidConveyor.ID,
			StatusItem.StatusItemOverlays.Conveyor
		},
		{
			OverlayModes.Decor.ID,
			StatusItem.StatusItemOverlays.Decor
		},
		{
			OverlayModes.Disease.ID,
			StatusItem.StatusItemOverlays.Pathogens
		},
		{
			OverlayModes.Crop.ID,
			StatusItem.StatusItemOverlays.Farming
		},
		{
			OverlayModes.Rooms.ID,
			StatusItem.StatusItemOverlays.Rooms
		},
		{
			OverlayModes.Suit.ID,
			StatusItem.StatusItemOverlays.Suits
		},
		{
			OverlayModes.Logic.ID,
			StatusItem.StatusItemOverlays.Logic
		},
		{
			OverlayModes.Oxygen.ID,
			StatusItem.StatusItemOverlays.None
		},
		{
			OverlayModes.TileMode.ID,
			StatusItem.StatusItemOverlays.None
		},
		{
			OverlayModes.Radiation.ID,
			StatusItem.StatusItemOverlays.Radiation
		}
	};

	// Token: 0x02001A01 RID: 6657
	public enum IconType
	{
		// Token: 0x0400689E RID: 26782
		Info,
		// Token: 0x0400689F RID: 26783
		Exclamation,
		// Token: 0x040068A0 RID: 26784
		Custom
	}

	// Token: 0x02001A02 RID: 6658
	[Flags]
	public enum StatusItemOverlays
	{
		// Token: 0x040068A2 RID: 26786
		None = 2,
		// Token: 0x040068A3 RID: 26787
		PowerMap = 4,
		// Token: 0x040068A4 RID: 26788
		Temperature = 8,
		// Token: 0x040068A5 RID: 26789
		ThermalComfort = 16,
		// Token: 0x040068A6 RID: 26790
		Light = 32,
		// Token: 0x040068A7 RID: 26791
		LiquidPlumbing = 64,
		// Token: 0x040068A8 RID: 26792
		GasPlumbing = 128,
		// Token: 0x040068A9 RID: 26793
		Decor = 256,
		// Token: 0x040068AA RID: 26794
		Pathogens = 512,
		// Token: 0x040068AB RID: 26795
		Farming = 1024,
		// Token: 0x040068AC RID: 26796
		Rooms = 4096,
		// Token: 0x040068AD RID: 26797
		Suits = 8192,
		// Token: 0x040068AE RID: 26798
		Logic = 16384,
		// Token: 0x040068AF RID: 26799
		Conveyor = 32768,
		// Token: 0x040068B0 RID: 26800
		Radiation = 65536
	}
}
