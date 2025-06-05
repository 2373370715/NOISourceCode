using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001854 RID: 6228
public class RotPile : StateMachineComponent<RotPile.StatesInstance>
{
	// Token: 0x06008069 RID: 32873 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600806A RID: 32874 RVA: 0x000F91A8 File Offset: 0x000F73A8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0600806B RID: 32875 RVA: 0x00340D08 File Offset: 0x0033EF08
	protected void ConvertToElement()
	{
		PrimaryElement component = base.smi.master.GetComponent<PrimaryElement>();
		float mass = component.Mass;
		float temperature = component.Temperature;
		if (mass <= 0f)
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		SimHashes hash = SimHashes.ToxicSand;
		GameObject gameObject = ElementLoader.FindElementByHash(hash).substance.SpawnResource(base.smi.master.transform.GetPosition(), mass, temperature, byte.MaxValue, 0, false, false, false);
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ElementLoader.FindElementByHash(hash).name, gameObject.transform, 1.5f, false);
		Util.KDestroyGameObject(base.smi.gameObject);
	}

	// Token: 0x0600806C RID: 32876 RVA: 0x00340DBC File Offset: 0x0033EFBC
	private static string OnRottenTooltip(List<Notification> notifications, object data)
	{
		string text = "";
		foreach (Notification notification in notifications)
		{
			if (notification.tooltipData != null)
			{
				text = text + "\n• " + (string)notification.tooltipData + " ";
			}
		}
		return string.Format(MISC.NOTIFICATIONS.FOODROT.TOOLTIP, text);
	}

	// Token: 0x0600806D RID: 32877 RVA: 0x000F91BB File Offset: 0x000F73BB
	public void TryClearNotification()
	{
		if (this.notification != null)
		{
			base.gameObject.AddOrGet<Notifier>().Remove(this.notification);
		}
	}

	// Token: 0x0600806E RID: 32878 RVA: 0x00340E40 File Offset: 0x0033F040
	public void TryCreateNotification()
	{
		WorldContainer myWorld = base.smi.master.GetMyWorld();
		if (myWorld != null && myWorld.worldInventory.IsReachable(base.smi.master.gameObject.GetComponent<Pickupable>()))
		{
			this.notification = new Notification(MISC.NOTIFICATIONS.FOODROT.NAME, NotificationType.BadMinor, new Func<List<Notification>, object, string>(RotPile.OnRottenTooltip), null, true, 0f, null, null, null, true, false, false);
			this.notification.tooltipData = base.smi.master.gameObject.GetProperName();
			base.gameObject.AddOrGet<Notifier>().Add(this.notification, "");
		}
	}

	// Token: 0x040061AF RID: 25007
	private Notification notification;

	// Token: 0x02001855 RID: 6229
	public class StatesInstance : GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.GameInstance
	{
		// Token: 0x06008070 RID: 32880 RVA: 0x000F91E3 File Offset: 0x000F73E3
		public StatesInstance(RotPile master) : base(master)
		{
		}

		// Token: 0x040061B0 RID: 25008
		public AttributeModifier baseDecomposeRate;
	}

	// Token: 0x02001856 RID: 6230
	public class States : GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile>
	{
		// Token: 0x06008071 RID: 32881 RVA: 0x00340EF8 File Offset: 0x0033F0F8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.decomposing;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.decomposing.Enter(delegate(RotPile.StatesInstance smi)
			{
				smi.master.TryCreateNotification();
			}).Exit(delegate(RotPile.StatesInstance smi)
			{
				smi.master.TryClearNotification();
			}).ParamTransition<float>(this.decompositionAmount, this.convertDestroy, (RotPile.StatesInstance smi, float p) => p >= 600f).Update("Decomposing", delegate(RotPile.StatesInstance smi, float dt)
			{
				this.decompositionAmount.Delta(dt, smi);
			}, UpdateRate.SIM_200ms, false);
			this.convertDestroy.Enter(delegate(RotPile.StatesInstance smi)
			{
				smi.master.ConvertToElement();
			});
		}

		// Token: 0x040061B1 RID: 25009
		public GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.State decomposing;

		// Token: 0x040061B2 RID: 25010
		public GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.State convertDestroy;

		// Token: 0x040061B3 RID: 25011
		public StateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.FloatParameter decompositionAmount;
	}
}
