﻿using System;
using System.Collections.Generic;
using UnityEngine;

public struct AsteroidDescriptor
{
	public AsteroidDescriptor(string text, string tooltip, Color associatedColor, List<global::Tuple<string, Color, float>> bands = null, string associatedIcon = null)
	{
		this.text = text;
		this.tooltip = tooltip;
		this.associatedColor = associatedColor;
		this.bands = bands;
		this.associatedIcon = associatedIcon;
	}

	public string text;

	public string tooltip;

	public List<global::Tuple<string, Color, float>> bands;

	public Color associatedColor;

	public string associatedIcon;
}
