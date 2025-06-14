﻿using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct StatePresentationSetting
{
	public string name;

	public Image image_target;

	public Sprite sprite;

	public Color color;

	public Color color_on_hover;

	public bool use_color_on_hover;
}
