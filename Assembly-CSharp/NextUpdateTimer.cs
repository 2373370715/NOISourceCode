using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001EA9 RID: 7849
[AddComponentMenu("KMonoBehaviour/scripts/NextUpdateTimer")]
public class NextUpdateTimer : KMonoBehaviour
{
	// Token: 0x0600A497 RID: 42135 RVA: 0x0010F378 File Offset: 0x0010D578
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.initialAnimScale = this.UpdateAnimController.animScale;
	}

	// Token: 0x0600A498 RID: 42136 RVA: 0x000C4795 File Offset: 0x000C2995
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x0600A499 RID: 42137 RVA: 0x0010F391 File Offset: 0x0010D591
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshReleaseTimes();
	}

	// Token: 0x0600A49A RID: 42138 RVA: 0x003F6690 File Offset: 0x003F4890
	public void UpdateReleaseTimes(string lastUpdateTime, string nextUpdateTime, string textOverride)
	{
		if (!System.DateTime.TryParse(lastUpdateTime, out this.currentReleaseDate))
		{
			global::Debug.LogWarning("Failed to parse last_update_time: " + lastUpdateTime);
		}
		if (!System.DateTime.TryParse(nextUpdateTime, out this.nextReleaseDate))
		{
			global::Debug.LogWarning("Failed to parse next_update_time: " + nextUpdateTime);
		}
		this.m_releaseTextOverride = textOverride;
		this.RefreshReleaseTimes();
	}

	// Token: 0x0600A49B RID: 42139 RVA: 0x003F66E8 File Offset: 0x003F48E8
	private void RefreshReleaseTimes()
	{
		TimeSpan timeSpan = this.nextReleaseDate - this.currentReleaseDate;
		TimeSpan timeSpan2 = this.nextReleaseDate - System.DateTime.UtcNow;
		TimeSpan timeSpan3 = System.DateTime.UtcNow - this.currentReleaseDate;
		string s = "4";
		string text;
		if (!string.IsNullOrEmpty(this.m_releaseTextOverride))
		{
			text = this.m_releaseTextOverride;
		}
		else if (timeSpan2.TotalHours < 8.0)
		{
			text = UI.DEVELOPMENTBUILDS.UPDATES.TWENTY_FOUR_HOURS;
			s = "4";
		}
		else if (timeSpan2.TotalDays < 1.0)
		{
			text = string.Format(UI.DEVELOPMENTBUILDS.UPDATES.FINAL_WEEK, 1);
			s = "3";
		}
		else
		{
			int num = timeSpan2.Days % 7;
			int num2 = (timeSpan2.Days - num) / 7;
			if (num2 <= 0)
			{
				text = string.Format(UI.DEVELOPMENTBUILDS.UPDATES.FINAL_WEEK, num);
				s = "2";
			}
			else
			{
				text = string.Format(UI.DEVELOPMENTBUILDS.UPDATES.BIGGER_TIMES, num, num2);
				s = "1";
			}
		}
		this.TimerText.text = text;
		this.UpdateAnimController.Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		float positionPercent = Mathf.Clamp01((float)(timeSpan3.TotalSeconds / timeSpan.TotalSeconds));
		this.UpdateAnimMeterController.SetPositionPercent(positionPercent);
	}

	// Token: 0x040080B3 RID: 32947
	public LocText TimerText;

	// Token: 0x040080B4 RID: 32948
	public KBatchedAnimController UpdateAnimController;

	// Token: 0x040080B5 RID: 32949
	public KBatchedAnimController UpdateAnimMeterController;

	// Token: 0x040080B6 RID: 32950
	public float initialAnimScale;

	// Token: 0x040080B7 RID: 32951
	public System.DateTime nextReleaseDate;

	// Token: 0x040080B8 RID: 32952
	public System.DateTime currentReleaseDate;

	// Token: 0x040080B9 RID: 32953
	private string m_releaseTextOverride;
}
