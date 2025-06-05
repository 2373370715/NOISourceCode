using System;
using System.IO;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x0200135D RID: 4957
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/GameClock")]
public class GameClock : KMonoBehaviour, ISaveLoadable, ISim33ms, IRender1000ms
{
	// Token: 0x0600658F RID: 25999 RVA: 0x000E6BA7 File Offset: 0x000E4DA7
	public static void DestroyInstance()
	{
		GameClock.Instance = null;
	}

	// Token: 0x06006590 RID: 26000 RVA: 0x000E6BAF File Offset: 0x000E4DAF
	protected override void OnPrefabInit()
	{
		GameClock.Instance = this;
		this.timeSinceStartOfCycle = 50f;
	}

	// Token: 0x06006591 RID: 26001 RVA: 0x002D2C28 File Offset: 0x002D0E28
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.time != 0f)
		{
			this.cycle = (int)(this.time / 600f);
			this.timeSinceStartOfCycle = Mathf.Max(this.time - (float)this.cycle * 600f, 0f);
			this.time = 0f;
		}
	}

	// Token: 0x06006592 RID: 26002 RVA: 0x000E6BC2 File Offset: 0x000E4DC2
	public void Sim33ms(float dt)
	{
		this.AddTime(dt);
	}

	// Token: 0x06006593 RID: 26003 RVA: 0x000E6BCB File Offset: 0x000E4DCB
	public void Render1000ms(float dt)
	{
		this.timePlayed += dt;
	}

	// Token: 0x06006594 RID: 26004 RVA: 0x000E6BDB File Offset: 0x000E4DDB
	private void LateUpdate()
	{
		this.frame++;
	}

	// Token: 0x06006595 RID: 26005 RVA: 0x002D2C84 File Offset: 0x002D0E84
	private void AddTime(float dt)
	{
		this.timeSinceStartOfCycle += dt;
		bool flag = false;
		while (this.timeSinceStartOfCycle >= 600f)
		{
			this.cycle++;
			this.timeSinceStartOfCycle -= 600f;
			base.Trigger(631075836, null);
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				worldContainer.Trigger(631075836, null);
			}
			flag = true;
		}
		if (!this.isNight && this.IsNighttime())
		{
			this.isNight = true;
			base.Trigger(-722330267, null);
		}
		if (this.isNight && !this.IsNighttime())
		{
			this.isNight = false;
		}
		if (flag && SaveGame.Instance.AutoSaveCycleInterval > 0 && this.cycle % SaveGame.Instance.AutoSaveCycleInterval == 0)
		{
			this.DoAutoSave(this.cycle);
		}
		int num = Mathf.FloorToInt(this.timeSinceStartOfCycle - dt / 25f);
		int num2 = Mathf.FloorToInt(this.timeSinceStartOfCycle / 25f);
		if (num != num2)
		{
			base.Trigger(-1215042067, num2);
		}
	}

	// Token: 0x06006596 RID: 26006 RVA: 0x000E6BEB File Offset: 0x000E4DEB
	public float GetTimeSinceStartOfReport()
	{
		if (this.IsNighttime())
		{
			return 525f - this.GetTimeSinceStartOfCycle();
		}
		return this.GetTimeSinceStartOfCycle() + 75f;
	}

	// Token: 0x06006597 RID: 26007 RVA: 0x000E6C0E File Offset: 0x000E4E0E
	public float GetTimeSinceStartOfCycle()
	{
		return this.timeSinceStartOfCycle;
	}

	// Token: 0x06006598 RID: 26008 RVA: 0x000E6C16 File Offset: 0x000E4E16
	public float GetCurrentCycleAsPercentage()
	{
		return this.timeSinceStartOfCycle / 600f;
	}

	// Token: 0x06006599 RID: 26009 RVA: 0x000E6C24 File Offset: 0x000E4E24
	public float GetTime()
	{
		return this.timeSinceStartOfCycle + (float)this.cycle * 600f;
	}

	// Token: 0x0600659A RID: 26010 RVA: 0x000E6C3A File Offset: 0x000E4E3A
	public float GetTimeInCycles()
	{
		return (float)this.cycle + this.GetCurrentCycleAsPercentage();
	}

	// Token: 0x0600659B RID: 26011 RVA: 0x000E6C4A File Offset: 0x000E4E4A
	public int GetFrame()
	{
		return this.frame;
	}

	// Token: 0x0600659C RID: 26012 RVA: 0x000E6C52 File Offset: 0x000E4E52
	public int GetCycle()
	{
		return this.cycle;
	}

	// Token: 0x0600659D RID: 26013 RVA: 0x000E6C5A File Offset: 0x000E4E5A
	public bool IsNighttime()
	{
		return GameClock.Instance.GetCurrentCycleAsPercentage() >= 0.875f;
	}

	// Token: 0x0600659E RID: 26014 RVA: 0x000E6C70 File Offset: 0x000E4E70
	public float GetDaytimeDurationInPercentage()
	{
		return 0.875f;
	}

	// Token: 0x0600659F RID: 26015 RVA: 0x002D2DD0 File Offset: 0x002D0FD0
	public void SetTime(float new_time)
	{
		float dt = Mathf.Max(new_time - this.GetTime(), 0f);
		this.AddTime(dt);
	}

	// Token: 0x060065A0 RID: 26016 RVA: 0x000E6C77 File Offset: 0x000E4E77
	public float GetTimePlayedInSeconds()
	{
		return this.timePlayed;
	}

	// Token: 0x060065A1 RID: 26017 RVA: 0x002D2DF8 File Offset: 0x002D0FF8
	private void DoAutoSave(int day)
	{
		if (GenericGameSettings.instance.disableAutosave)
		{
			return;
		}
		day++;
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, GameClock.NewCycleKey, day);
		OniMetrics.SendEvent(OniMetrics.Event.EndOfCycle, "DoAutoSave");
		string text = SaveLoader.GetActiveSaveFilePath();
		if (text == null)
		{
			text = SaveLoader.GetAutosaveFilePath();
		}
		int num = text.LastIndexOf("\\");
		if (num > 0)
		{
			int num2 = text.IndexOf(" Cycle ", num);
			if (num2 > 0)
			{
				text = text.Substring(0, num2);
			}
		}
		text = Path.ChangeExtension(text, null);
		text = text + " Cycle " + day.ToString();
		text = SaveScreen.GetValidSaveFilename(text);
		text = Path.Combine(SaveLoader.GetActiveAutoSavePath(), Path.GetFileName(text));
		string text2 = text;
		int num3 = 1;
		while (File.Exists(text))
		{
			text = text2.Replace(".sav", "");
			text = SaveScreen.GetValidSaveFilename(text2 + " (" + num3.ToString() + ")");
			num3++;
		}
		Game.Instance.StartDelayedSave(text, true, false);
	}

	// Token: 0x04004984 RID: 18820
	public static GameClock Instance;

	// Token: 0x04004985 RID: 18821
	[Serialize]
	private int frame;

	// Token: 0x04004986 RID: 18822
	[Serialize]
	private float time;

	// Token: 0x04004987 RID: 18823
	[Serialize]
	private float timeSinceStartOfCycle;

	// Token: 0x04004988 RID: 18824
	[Serialize]
	private int cycle;

	// Token: 0x04004989 RID: 18825
	[Serialize]
	private float timePlayed;

	// Token: 0x0400498A RID: 18826
	[Serialize]
	private bool isNight;

	// Token: 0x0400498B RID: 18827
	public static readonly string NewCycleKey = "NewCycle";
}
