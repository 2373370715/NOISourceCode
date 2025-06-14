﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class AlertVignette : KMonoBehaviour
{
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	private void Update()
	{
		Color color = this.image.color;
		if (ClusterManager.Instance.GetWorld(this.worldID) == null)
		{
			color = Color.clear;
			this.image.color = color;
			return;
		}
		if (ClusterManager.Instance.GetWorld(this.worldID).IsRedAlert())
		{
			if (color.r != Vignette.Instance.redAlertColor.r || color.g != Vignette.Instance.redAlertColor.g || color.b != Vignette.Instance.redAlertColor.b)
			{
				color = Vignette.Instance.redAlertColor;
			}
		}
		else if (ClusterManager.Instance.GetWorld(this.worldID).IsYellowAlert())
		{
			if (color.r != Vignette.Instance.yellowAlertColor.r || color.g != Vignette.Instance.yellowAlertColor.g || color.b != Vignette.Instance.yellowAlertColor.b)
			{
				color = Vignette.Instance.yellowAlertColor;
			}
		}
		else
		{
			color = Color.clear;
		}
		if (color != Color.clear)
		{
			color.a = 0.2f + (0.5f + Mathf.Sin(Time.unscaledTime * 4f - 1f) / 2f) * 0.5f;
		}
		if (this.image.color != color)
		{
			this.image.color = color;
		}
	}

	public Image image;

	public int worldID;
}
