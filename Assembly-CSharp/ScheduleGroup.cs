﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

[DebuggerDisplay("{Id}")]
public class ScheduleGroup : Resource
{
	public int defaultSegments { get; private set; }

	public string description { get; private set; }

	public string notificationTooltip { get; private set; }

	public List<ScheduleBlockType> allowedTypes { get; private set; }

	public bool alarm { get; private set; }

	public Color uiColor { get; private set; }

	public ScheduleGroup(string id, ResourceSet parent, int defaultSegments, string name, string description, Color uiColor, string notificationTooltip, List<ScheduleBlockType> allowedTypes, bool alarm = false) : base(id, parent, name)
	{
		this.defaultSegments = defaultSegments;
		this.description = description;
		this.notificationTooltip = notificationTooltip;
		this.allowedTypes = allowedTypes;
		this.alarm = alarm;
		this.uiColor = uiColor;
	}

	public bool Allowed(ScheduleBlockType type)
	{
		return this.allowedTypes.Contains(type);
	}

	public string GetTooltip()
	{
		return string.Format(UI.SCHEDULEGROUPS.TOOLTIP_FORMAT, this.Name, this.description);
	}
}
