using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200026B RID: 619
public class ScoutRoverConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060008D9 RID: 2265 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x0016D36C File Offset: 0x0016B56C
	public GameObject CreatePrefab()
	{
		return BaseRoverConfig.BaseRover("ScoutRover", STRINGS.ROBOTS.MODELS.SCOUT.NAME, GameTags.Robots.Models.ScoutRover, STRINGS.ROBOTS.MODELS.SCOUT.DESC, "scout_bot_kanim", 100f, 1f, 2f, TUNING.ROBOTS.SCOUTBOT.CARRY_CAPACITY, TUNING.ROBOTS.SCOUTBOT.DIGGING, TUNING.ROBOTS.SCOUTBOT.CONSTRUCTION, TUNING.ROBOTS.SCOUTBOT.ATHLETICS, TUNING.ROBOTS.SCOUTBOT.HIT_POINTS, TUNING.ROBOTS.SCOUTBOT.BATTERY_CAPACITY, TUNING.ROBOTS.SCOUTBOT.BATTERY_DEPLETION_RATE, Db.Get().Amounts.InternalChemicalBattery, false);
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x000AE78A File Offset: 0x000AC98A
	public void OnPrefabInit(GameObject inst)
	{
		BaseRoverConfig.OnPrefabInit(inst, Db.Get().Amounts.InternalChemicalBattery);
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x0016D3E4 File Offset: 0x0016B5E4
	public void OnSpawn(GameObject inst)
	{
		BaseRoverConfig.OnSpawn(inst);
		Effects effects = inst.GetComponent<Effects>();
		if (inst.transform.parent == null)
		{
			if (effects.HasEffect("ScoutBotCharging"))
			{
				effects.Remove("ScoutBotCharging");
			}
		}
		else if (!effects.HasEffect("ScoutBotCharging"))
		{
			effects.Add("ScoutBotCharging", false);
		}
		inst.Subscribe(856640610, delegate(object data)
		{
			if (inst.transform.parent == null)
			{
				if (effects.HasEffect("ScoutBotCharging"))
				{
					effects.Remove("ScoutBotCharging");
					return;
				}
			}
			else if (!effects.HasEffect("ScoutBotCharging"))
			{
				effects.Add("ScoutBotCharging", false);
			}
		});
	}

	// Token: 0x040006CB RID: 1739
	public const string ID = "ScoutRover";

	// Token: 0x040006CC RID: 1740
	public const float MASS = 100f;

	// Token: 0x040006CD RID: 1741
	private const float WIDTH = 1f;

	// Token: 0x040006CE RID: 1742
	private const float HEIGHT = 2f;

	// Token: 0x040006CF RID: 1743
	public const int MAXIMUM_TECH_CONSTRUCTION_TIER = 2;
}
