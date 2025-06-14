﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class MotdData
{
	public static MotdData Parse(string inputStr)
	{
		MotdData result;
		try
		{
			MotdData motdData = new MotdData();
			JObject jobject = JObject.Parse(inputStr);
			motdData.liveVersion = int.Parse(jobject["live-version"].Value<string>());
			foreach (JToken jtoken in ((IEnumerable<JToken>)jobject["boxes-live"][0]["Category"]))
			{
				JProperty jproperty = (JProperty)jtoken;
				string name = jproperty.Name;
				foreach (JToken jtoken2 in ((IEnumerable<JToken>)jproperty.Value))
				{
					JObject jobject2 = (JObject)jtoken2;
					MotdData_Box motdData_Box = new MotdData_Box
					{
						category = name,
						guid = jobject2.Value<string>("guid"),
						startTime = 0L,
						finishTime = 0L,
						title = jobject2.Value<string>("title"),
						text = jobject2.Value<string>("text"),
						image = jobject2.Value<string>("image"),
						href = jobject2.Value<string>("href")
					};
					long startTime;
					if (long.TryParse(jobject2.Value<string>("start-time"), out startTime))
					{
						motdData_Box.startTime = startTime;
					}
					long finishTime;
					if (long.TryParse(jobject2.Value<string>("finish-time"), out finishTime))
					{
						motdData_Box.finishTime = finishTime;
					}
					motdData.boxesLive.Add(motdData_Box);
				}
			}
			result = motdData;
		}
		catch (Exception arg)
		{
			Debug.LogWarning(string.Format("Motd Parse Error:\n--------------------\n{0}\n--------------------\n{1}", inputStr, arg));
			result = null;
		}
		return result;
	}

	public int liveVersion;

	public List<MotdData_Box> boxesLive = new List<MotdData_Box>();
}
