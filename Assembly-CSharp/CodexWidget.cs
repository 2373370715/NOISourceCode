﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CodexWidget<SubClass> : ICodexWidget
{
	public int preferredWidth { get; set; }

	public int preferredHeight { get; set; }

	protected CodexWidget()
	{
		this.preferredWidth = -1;
		this.preferredHeight = -1;
	}

	protected CodexWidget(int preferredWidth, int preferredHeight)
	{
		this.preferredWidth = preferredWidth;
		this.preferredHeight = preferredHeight;
	}

	public abstract void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles);

	protected void ConfigurePreferredLayout(GameObject contentGameObject)
	{
		LayoutElement componentInChildren = contentGameObject.GetComponentInChildren<LayoutElement>();
		componentInChildren.minWidth = (float)this.preferredWidth;
		componentInChildren.minHeight = (float)this.preferredHeight;
		componentInChildren.preferredHeight = (float)this.preferredHeight;
		componentInChildren.preferredWidth = (float)this.preferredWidth;
	}
}
