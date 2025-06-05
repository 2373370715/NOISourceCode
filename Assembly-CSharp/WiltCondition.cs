using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001231 RID: 4657
[AddComponentMenu("KMonoBehaviour/scripts/WiltCondition")]
public class WiltCondition : KMonoBehaviour
{
	// Token: 0x06005E6F RID: 24175 RVA: 0x000E223A File Offset: 0x000E043A
	public bool IsWilting()
	{
		return this.wilting;
	}

	// Token: 0x06005E70 RID: 24176 RVA: 0x002AFB08 File Offset: 0x002ADD08
	public List<WiltCondition.Condition> CurrentWiltSources()
	{
		List<WiltCondition.Condition> list = new List<WiltCondition.Condition>();
		foreach (KeyValuePair<int, bool> keyValuePair in this.WiltConditions)
		{
			if (!keyValuePair.Value)
			{
				list.Add((WiltCondition.Condition)keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x06005E71 RID: 24177 RVA: 0x002AFB74 File Offset: 0x002ADD74
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.WiltConditions.Add(0, true);
		this.WiltConditions.Add(1, true);
		this.WiltConditions.Add(2, true);
		this.WiltConditions.Add(3, true);
		this.WiltConditions.Add(4, true);
		this.WiltConditions.Add(5, true);
		this.WiltConditions.Add(6, true);
		this.WiltConditions.Add(7, true);
		this.WiltConditions.Add(9, true);
		this.WiltConditions.Add(10, true);
		this.WiltConditions.Add(11, true);
		this.WiltConditions.Add(12, true);
		base.Subscribe<WiltCondition>(-107174716, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(-1758196852, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(-1234705021, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(-55477301, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(115888613, WiltCondition.SetTemperatureTrueDelegate);
		base.Subscribe<WiltCondition>(-593125877, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(-1175525437, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(-907106982, WiltCondition.SetPressureTrueDelegate);
		base.Subscribe<WiltCondition>(103243573, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(646131325, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(221594799, WiltCondition.SetAtmosphereElementFalseDelegate);
		base.Subscribe<WiltCondition>(777259436, WiltCondition.SetAtmosphereElementTrueDelegate);
		base.Subscribe<WiltCondition>(1949704522, WiltCondition.SetDrowningFalseDelegate);
		base.Subscribe<WiltCondition>(99949694, WiltCondition.SetDrowningTrueDelegate);
		base.Subscribe<WiltCondition>(-2057657673, WiltCondition.SetDryingOutFalseDelegate);
		base.Subscribe<WiltCondition>(1555379996, WiltCondition.SetDryingOutTrueDelegate);
		base.Subscribe<WiltCondition>(-370379773, WiltCondition.SetIrrigationFalseDelegate);
		base.Subscribe<WiltCondition>(207387507, WiltCondition.SetIrrigationTrueDelegate);
		base.Subscribe<WiltCondition>(-1073674739, WiltCondition.SetFertilizedFalseDelegate);
		base.Subscribe<WiltCondition>(-1396791468, WiltCondition.SetFertilizedTrueDelegate);
		base.Subscribe<WiltCondition>(1113102781, WiltCondition.SetIlluminationComfortTrueDelegate);
		base.Subscribe<WiltCondition>(1387626797, WiltCondition.SetIlluminationComfortFalseDelegate);
		base.Subscribe<WiltCondition>(1628751838, WiltCondition.SetReceptacleTrueDelegate);
		base.Subscribe<WiltCondition>(960378201, WiltCondition.SetReceptacleFalseDelegate);
		base.Subscribe<WiltCondition>(-1089732772, WiltCondition.SetEntombedDelegate);
		base.Subscribe<WiltCondition>(912965142, WiltCondition.SetRootHealthDelegate);
		base.Subscribe<WiltCondition>(874353739, WiltCondition.SetRadiationComfortTrueDelegate);
		base.Subscribe<WiltCondition>(1788072223, WiltCondition.SetRadiationComfortFalseDelegate);
	}

	// Token: 0x06005E72 RID: 24178 RVA: 0x002AFE04 File Offset: 0x002AE004
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.CheckShouldWilt();
		if (this.wilting)
		{
			this.DoWilt();
			if (!this.goingToWilt)
			{
				this.goingToWilt = true;
				this.Recover();
				return;
			}
		}
		else
		{
			this.DoRecover();
			if (this.goingToWilt)
			{
				this.goingToWilt = false;
				this.Wilt();
			}
		}
	}

	// Token: 0x06005E73 RID: 24179 RVA: 0x000E2242 File Offset: 0x000E0442
	protected override void OnCleanUp()
	{
		this.wiltSchedulerHandler.ClearScheduler();
		this.recoverSchedulerHandler.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x06005E74 RID: 24180 RVA: 0x000E2260 File Offset: 0x000E0460
	private void SetCondition(WiltCondition.Condition condition, bool satisfiedState)
	{
		if (!this.WiltConditions.ContainsKey((int)condition))
		{
			return;
		}
		this.WiltConditions[(int)condition] = satisfiedState;
		this.CheckShouldWilt();
	}

	// Token: 0x06005E75 RID: 24181 RVA: 0x002AFE5C File Offset: 0x002AE05C
	private void CheckShouldWilt()
	{
		bool flag = false;
		foreach (KeyValuePair<int, bool> keyValuePair in this.WiltConditions)
		{
			if (!keyValuePair.Value)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (!this.goingToWilt)
			{
				this.Wilt();
				return;
			}
		}
		else if (this.goingToWilt)
		{
			this.Recover();
		}
	}

	// Token: 0x06005E76 RID: 24182 RVA: 0x002AFED8 File Offset: 0x002AE0D8
	private void Wilt()
	{
		if (!this.goingToWilt)
		{
			this.goingToWilt = true;
			this.recoverSchedulerHandler.ClearScheduler();
			if (!this.wiltSchedulerHandler.IsValid)
			{
				this.wiltSchedulerHandler = GameScheduler.Instance.Schedule("Wilt", this.WiltDelay, new Action<object>(WiltCondition.DoWiltCallback), this, null);
			}
		}
	}

	// Token: 0x06005E77 RID: 24183 RVA: 0x002AFF38 File Offset: 0x002AE138
	private void Recover()
	{
		if (this.goingToWilt)
		{
			this.goingToWilt = false;
			this.wiltSchedulerHandler.ClearScheduler();
			if (!this.recoverSchedulerHandler.IsValid)
			{
				this.recoverSchedulerHandler = GameScheduler.Instance.Schedule("Recover", this.RecoveryDelay, new Action<object>(WiltCondition.DoRecoverCallback), this, null);
			}
		}
	}

	// Token: 0x06005E78 RID: 24184 RVA: 0x000E2284 File Offset: 0x000E0484
	private static void DoWiltCallback(object data)
	{
		((WiltCondition)data).DoWilt();
	}

	// Token: 0x06005E79 RID: 24185 RVA: 0x002AFF98 File Offset: 0x002AE198
	private void DoWilt()
	{
		this.wiltSchedulerHandler.ClearScheduler();
		KSelectable component = base.GetComponent<KSelectable>();
		component.GetComponent<KPrefabID>().AddTag(GameTags.Wilting, false);
		if (!this.wilting)
		{
			this.wilting = true;
			base.Trigger(-724860998, null);
		}
		if (this.rm != null)
		{
			if (this.rm.Replanted)
			{
				component.AddStatusItem(Db.Get().CreatureStatusItems.WiltingDomestic, base.GetComponent<ReceptacleMonitor>());
				return;
			}
			component.AddStatusItem(Db.Get().CreatureStatusItems.Wilting, base.GetComponent<ReceptacleMonitor>());
			return;
		}
		else
		{
			ReceptacleMonitor.StatesInstance smi = component.GetSMI<ReceptacleMonitor.StatesInstance>();
			if (smi != null && !smi.IsInsideState(smi.sm.wild))
			{
				component.AddStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowingDomestic, this);
				return;
			}
			component.AddStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowing, this);
			return;
		}
	}

	// Token: 0x06005E7A RID: 24186 RVA: 0x002B0084 File Offset: 0x002AE284
	public string WiltCausesString()
	{
		string text = "";
		List<IWiltCause> allSMI = this.GetAllSMI<IWiltCause>();
		allSMI.AddRange(base.GetComponents<IWiltCause>());
		foreach (IWiltCause wiltCause in allSMI)
		{
			foreach (WiltCondition.Condition key in wiltCause.Conditions)
			{
				if (this.WiltConditions.ContainsKey((int)key) && !this.WiltConditions[(int)key])
				{
					text += "\n";
					text += wiltCause.WiltStateString;
					break;
				}
			}
		}
		return text;
	}

	// Token: 0x06005E7B RID: 24187 RVA: 0x000E2291 File Offset: 0x000E0491
	private static void DoRecoverCallback(object data)
	{
		((WiltCondition)data).DoRecover();
	}

	// Token: 0x06005E7C RID: 24188 RVA: 0x002B013C File Offset: 0x002AE33C
	private void DoRecover()
	{
		this.recoverSchedulerHandler.ClearScheduler();
		KSelectable component = base.GetComponent<KSelectable>();
		this.wilting = false;
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.WiltingDomestic, false);
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.Wilting, false);
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowing, false);
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowingDomestic, false);
		component.GetComponent<KPrefabID>().RemoveTag(GameTags.Wilting);
		base.Trigger(712767498, null);
	}

	// Token: 0x04004373 RID: 17267
	[MyCmpGet]
	private ReceptacleMonitor rm;

	// Token: 0x04004374 RID: 17268
	[Serialize]
	private bool goingToWilt;

	// Token: 0x04004375 RID: 17269
	[Serialize]
	private bool wilting;

	// Token: 0x04004376 RID: 17270
	private Dictionary<int, bool> WiltConditions = new Dictionary<int, bool>();

	// Token: 0x04004377 RID: 17271
	public float WiltDelay = 1f;

	// Token: 0x04004378 RID: 17272
	public float RecoveryDelay = 1f;

	// Token: 0x04004379 RID: 17273
	private SchedulerHandle wiltSchedulerHandler;

	// Token: 0x0400437A RID: 17274
	private SchedulerHandle recoverSchedulerHandler;

	// Token: 0x0400437B RID: 17275
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetTemperatureFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Temperature, false);
	});

	// Token: 0x0400437C RID: 17276
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetTemperatureTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Temperature, true);
	});

	// Token: 0x0400437D RID: 17277
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetPressureFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Pressure, false);
	});

	// Token: 0x0400437E RID: 17278
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetPressureTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Pressure, true);
	});

	// Token: 0x0400437F RID: 17279
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetAtmosphereElementFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.AtmosphereElement, false);
	});

	// Token: 0x04004380 RID: 17280
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetAtmosphereElementTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.AtmosphereElement, true);
	});

	// Token: 0x04004381 RID: 17281
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDrowningFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Drowning, false);
	});

	// Token: 0x04004382 RID: 17282
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDrowningTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Drowning, true);
	});

	// Token: 0x04004383 RID: 17283
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDryingOutFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.DryingOut, false);
	});

	// Token: 0x04004384 RID: 17284
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDryingOutTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.DryingOut, true);
	});

	// Token: 0x04004385 RID: 17285
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIrrigationFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Irrigation, false);
	});

	// Token: 0x04004386 RID: 17286
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIrrigationTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Irrigation, true);
	});

	// Token: 0x04004387 RID: 17287
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetFertilizedFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Fertilized, false);
	});

	// Token: 0x04004388 RID: 17288
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetFertilizedTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Fertilized, true);
	});

	// Token: 0x04004389 RID: 17289
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIlluminationComfortFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.IlluminationComfort, false);
	});

	// Token: 0x0400438A RID: 17290
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIlluminationComfortTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.IlluminationComfort, true);
	});

	// Token: 0x0400438B RID: 17291
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetReceptacleFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Receptacle, false);
	});

	// Token: 0x0400438C RID: 17292
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetReceptacleTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Receptacle, true);
	});

	// Token: 0x0400438D RID: 17293
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetEntombedDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Entombed, !(bool)data);
	});

	// Token: 0x0400438E RID: 17294
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetRootHealthDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.UnhealthyRoot, (bool)data);
	});

	// Token: 0x0400438F RID: 17295
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetRadiationComfortFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Radiation, false);
	});

	// Token: 0x04004390 RID: 17296
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetRadiationComfortTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Radiation, true);
	});

	// Token: 0x02001232 RID: 4658
	public enum Condition
	{
		// Token: 0x04004392 RID: 17298
		Temperature,
		// Token: 0x04004393 RID: 17299
		Pressure,
		// Token: 0x04004394 RID: 17300
		AtmosphereElement,
		// Token: 0x04004395 RID: 17301
		Drowning,
		// Token: 0x04004396 RID: 17302
		Fertilized,
		// Token: 0x04004397 RID: 17303
		DryingOut,
		// Token: 0x04004398 RID: 17304
		Irrigation,
		// Token: 0x04004399 RID: 17305
		IlluminationComfort,
		// Token: 0x0400439A RID: 17306
		Darkness,
		// Token: 0x0400439B RID: 17307
		Receptacle,
		// Token: 0x0400439C RID: 17308
		Entombed,
		// Token: 0x0400439D RID: 17309
		UnhealthyRoot,
		// Token: 0x0400439E RID: 17310
		Radiation,
		// Token: 0x0400439F RID: 17311
		Count
	}
}
