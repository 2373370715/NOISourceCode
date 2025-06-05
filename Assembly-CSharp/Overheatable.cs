using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000AEF RID: 2799
[SkipSaveFileSerialization]
public class Overheatable : StateMachineComponent<Overheatable.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06003389 RID: 13193 RVA: 0x000C61E4 File Offset: 0x000C43E4
	public void ResetTemperature()
	{
		base.GetComponent<PrimaryElement>().Temperature = 293.15f;
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x00213DD4 File Offset: 0x00211FD4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overheatTemp = this.GetAttributes().Add(Db.Get().BuildingAttributes.OverheatTemperature);
		this.fatalTemp = this.GetAttributes().Add(Db.Get().BuildingAttributes.FatalTemperature);
	}

	// Token: 0x0600338B RID: 13195 RVA: 0x00213E28 File Offset: 0x00212028
	private void InitializeModifiers()
	{
		if (this.modifiersInitialized)
		{
			return;
		}
		this.modifiersInitialized = true;
		AttributeModifier modifier = new AttributeModifier(this.overheatTemp.Id, this.baseOverheatTemp, UI.TOOLTIPS.BASE_VALUE, false, false, true)
		{
			OverrideTimeSlice = new GameUtil.TimeSlice?(GameUtil.TimeSlice.None)
		};
		AttributeModifier modifier2 = new AttributeModifier(this.fatalTemp.Id, this.baseFatalTemp, UI.TOOLTIPS.BASE_VALUE, false, false, true);
		this.GetAttributes().Add(modifier);
		this.GetAttributes().Add(modifier2);
	}

	// Token: 0x0600338C RID: 13196 RVA: 0x00213EB4 File Offset: 0x002120B4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.InitializeModifiers();
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		if (handle.IsValid() && GameComps.StructureTemperatures.IsEnabled(handle))
		{
			GameComps.StructureTemperatures.Disable(handle);
			GameComps.StructureTemperatures.Enable(handle);
		}
		base.smi.StartSM();
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x0600338D RID: 13197 RVA: 0x000C61F6 File Offset: 0x000C43F6
	public float OverheatTemperature
	{
		get
		{
			this.InitializeModifiers();
			if (this.overheatTemp == null)
			{
				return 10000f;
			}
			return this.overheatTemp.GetTotalValue();
		}
	}

	// Token: 0x0600338E RID: 13198 RVA: 0x00213F18 File Offset: 0x00212118
	public Notification CreateOverheatedNotification()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		return new Notification(MISC.NOTIFICATIONS.BUILDINGOVERHEATED.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.BUILDINGOVERHEATED.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + component.GetProperName(), false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x0600338F RID: 13199 RVA: 0x00213F78 File Offset: 0x00212178
	private static string ToolTipResolver(List<Notification> notificationList, object data)
	{
		string text = "";
		for (int i = 0; i < notificationList.Count; i++)
		{
			Notification notification = notificationList[i];
			text += (string)notification.tooltipData;
			if (i < notificationList.Count - 1)
			{
				text += "\n";
			}
		}
		return string.Format(MISC.NOTIFICATIONS.BUILDINGOVERHEATED.TOOLTIP, text);
	}

	// Token: 0x06003390 RID: 13200 RVA: 0x00213FE0 File Offset: 0x002121E0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.overheatTemp != null && this.fatalTemp != null)
		{
			string formattedValue = this.overheatTemp.GetFormattedValue();
			string formattedValue2 = this.fatalTemp.GetFormattedValue();
			string text = UI.BUILDINGEFFECTS.TOOLTIPS.OVERHEAT_TEMP;
			text = text + "\n\n" + this.overheatTemp.GetAttributeValueTooltip();
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.OVERHEAT_TEMP, formattedValue, formattedValue2), string.Format(text, formattedValue, formattedValue2), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else if (this.baseOverheatTemp != 0f)
		{
			string formattedTemperature = GameUtil.GetFormattedTemperature(this.baseOverheatTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
			string formattedTemperature2 = GameUtil.GetFormattedTemperature(this.baseFatalTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
			string format = UI.BUILDINGEFFECTS.TOOLTIPS.OVERHEAT_TEMP;
			Descriptor item2 = new Descriptor(string.Format(UI.BUILDINGEFFECTS.OVERHEAT_TEMP, formattedTemperature, formattedTemperature2), string.Format(format, formattedTemperature, formattedTemperature2), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x04002353 RID: 9043
	private bool modifiersInitialized;

	// Token: 0x04002354 RID: 9044
	private AttributeInstance overheatTemp;

	// Token: 0x04002355 RID: 9045
	private AttributeInstance fatalTemp;

	// Token: 0x04002356 RID: 9046
	public float baseOverheatTemp;

	// Token: 0x04002357 RID: 9047
	public float baseFatalTemp;

	// Token: 0x02000AF0 RID: 2800
	public class StatesInstance : GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.GameInstance
	{
		// Token: 0x06003392 RID: 13202 RVA: 0x000C621F File Offset: 0x000C441F
		public StatesInstance(Overheatable smi) : base(smi)
		{
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x002140D4 File Offset: 0x002122D4
		public void TryDoOverheatDamage()
		{
			if (Time.time - this.lastOverheatDamageTime < 7.5f)
			{
				return;
			}
			this.lastOverheatDamageTime += 7.5f;
			base.master.Trigger(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = 1,
				source = BUILDINGS.DAMAGESOURCES.BUILDING_OVERHEATED,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.OVERHEAT,
				fullDamageEffectName = "smoke_damage_kanim"
			});
		}

		// Token: 0x04002358 RID: 9048
		public float lastOverheatDamageTime;
	}

	// Token: 0x02000AF1 RID: 2801
	public class States : GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable>
	{
		// Token: 0x06003394 RID: 13204 RVA: 0x0021415C File Offset: 0x0021235C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.safeTemperature;
			this.root.EventTransition(GameHashes.BuildingBroken, this.invulnerable, null);
			this.invulnerable.EventHandler(GameHashes.BuildingPartiallyRepaired, delegate(Overheatable.StatesInstance smi)
			{
				smi.master.ResetTemperature();
			}).EventTransition(GameHashes.BuildingPartiallyRepaired, this.safeTemperature, null);
			this.safeTemperature.TriggerOnEnter(GameHashes.OptimalTemperatureAchieved, null).EventTransition(GameHashes.BuildingOverheated, this.overheated, null);
			this.overheated.Enter(delegate(Overheatable.StatesInstance smi)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_OverheatingBuildings, true);
			}).EventTransition(GameHashes.BuildingNoLongerOverheated, this.safeTemperature, null).ToggleStatusItem(Db.Get().BuildingStatusItems.Overheated, null).ToggleNotification((Overheatable.StatesInstance smi) => smi.master.CreateOverheatedNotification()).TriggerOnEnter(GameHashes.TooHotWarning, null).Enter("InitOverheatTime", delegate(Overheatable.StatesInstance smi)
			{
				smi.lastOverheatDamageTime = Time.time;
			}).Update("OverheatDamage", delegate(Overheatable.StatesInstance smi, float dt)
			{
				smi.TryDoOverheatDamage();
			}, UpdateRate.SIM_4000ms, false);
		}

		// Token: 0x04002359 RID: 9049
		public GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State invulnerable;

		// Token: 0x0400235A RID: 9050
		public GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State safeTemperature;

		// Token: 0x0400235B RID: 9051
		public GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State overheated;
	}
}
