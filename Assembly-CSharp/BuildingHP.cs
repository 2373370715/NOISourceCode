using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CB0 RID: 3248
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/BuildingHP")]
public class BuildingHP : Workable
{
	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x06003DD4 RID: 15828 RVA: 0x000CC862 File Offset: 0x000CAA62
	public int HitPoints
	{
		get
		{
			return this.hitpoints;
		}
	}

	// Token: 0x06003DD5 RID: 15829 RVA: 0x000CC86A File Offset: 0x000CAA6A
	public void SetHitPoints(int hp)
	{
		this.hitpoints = hp;
	}

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x06003DD6 RID: 15830 RVA: 0x000CC873 File Offset: 0x000CAA73
	public int MaxHitPoints
	{
		get
		{
			return this.building.Def.HitPoints;
		}
	}

	// Token: 0x06003DD7 RID: 15831 RVA: 0x000CC885 File Offset: 0x000CAA85
	public BuildingHP.DamageSourceInfo GetDamageSourceInfo()
	{
		return this.damageSourceInfo;
	}

	// Token: 0x06003DD8 RID: 15832 RVA: 0x000CC88D File Offset: 0x000CAA8D
	protected override void OnLoadLevel()
	{
		this.smi = null;
		base.OnLoadLevel();
	}

	// Token: 0x06003DD9 RID: 15833 RVA: 0x000CC89C File Offset: 0x000CAA9C
	public void DoDamage(int damage)
	{
		if (!this.invincible)
		{
			damage = Math.Max(0, damage);
			this.hitpoints = Math.Max(0, this.hitpoints - damage);
			base.Trigger(-1964935036, this);
		}
	}

	// Token: 0x06003DDA RID: 15834 RVA: 0x00240C28 File Offset: 0x0023EE28
	public void Repair(int repair_amount)
	{
		if (this.hitpoints + repair_amount < this.hitpoints)
		{
			this.hitpoints = this.building.Def.HitPoints;
		}
		else
		{
			this.hitpoints = Math.Min(this.hitpoints + repair_amount, this.building.Def.HitPoints);
		}
		base.Trigger(-1699355994, this);
		if (this.hitpoints >= this.building.Def.HitPoints)
		{
			base.Trigger(-1735440190, this);
		}
	}

	// Token: 0x06003DDB RID: 15835 RVA: 0x000CC8CF File Offset: 0x000CAACF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(10f);
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
	}

	// Token: 0x06003DDC RID: 15836 RVA: 0x00240CB0 File Offset: 0x0023EEB0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new BuildingHP.SMInstance(this);
		this.smi.StartSM();
		base.Subscribe<BuildingHP>(-794517298, BuildingHP.OnDoBuildingDamageDelegate);
		if (this.destroyOnDamaged)
		{
			base.Subscribe<BuildingHP>(774203113, BuildingHP.DestroyOnDamagedDelegate);
		}
		if (this.hitpoints <= 0)
		{
			base.Trigger(774203113, this);
		}
	}

	// Token: 0x06003DDD RID: 15837 RVA: 0x000CC902 File Offset: 0x000CAB02
	private void DestroyOnDamaged(object data)
	{
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06003DDE RID: 15838 RVA: 0x00240D1C File Offset: 0x0023EF1C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		int num = (int)Db.Get().Attributes.Machinery.Lookup(worker).GetTotalValue();
		int repair_amount = 10 + Math.Max(0, num * 10);
		this.Repair(repair_amount);
	}

	// Token: 0x06003DDF RID: 15839 RVA: 0x000CC90F File Offset: 0x000CAB0F
	private void OnDoBuildingDamage(object data)
	{
		if (this.invincible)
		{
			return;
		}
		this.damageSourceInfo = (BuildingHP.DamageSourceInfo)data;
		this.DoDamage(this.damageSourceInfo.damage);
		this.DoDamagePopFX(this.damageSourceInfo);
		this.DoTakeDamageFX(this.damageSourceInfo);
	}

	// Token: 0x06003DE0 RID: 15840 RVA: 0x00240D5C File Offset: 0x0023EF5C
	private void DoTakeDamageFX(BuildingHP.DamageSourceInfo info)
	{
		if (info.takeDamageEffect != SpawnFXHashes.None)
		{
			BuildingDef def = base.GetComponent<BuildingComplete>().Def;
			int cell = Grid.OffsetCell(Grid.PosToCell(this), 0, def.HeightInCells - 1);
			Game.Instance.SpawnFX(info.takeDamageEffect, cell, 0f);
		}
	}

	// Token: 0x06003DE1 RID: 15841 RVA: 0x00240DA8 File Offset: 0x0023EFA8
	private void DoDamagePopFX(BuildingHP.DamageSourceInfo info)
	{
		if (info.popString != null && Time.time > this.lastPopTime + this.minDamagePopInterval)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, info.popString, base.gameObject.transform, 1.5f, false);
			this.lastPopTime = Time.time;
		}
	}

	// Token: 0x170002DA RID: 730
	// (get) Token: 0x06003DE2 RID: 15842 RVA: 0x000CC94F File Offset: 0x000CAB4F
	public bool IsBroken
	{
		get
		{
			return this.hitpoints == 0;
		}
	}

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x06003DE3 RID: 15843 RVA: 0x000CC95A File Offset: 0x000CAB5A
	public bool NeedsRepairs
	{
		get
		{
			return this.HitPoints < this.building.Def.HitPoints;
		}
	}

	// Token: 0x04002AA7 RID: 10919
	[Serialize]
	[SerializeField]
	private int hitpoints;

	// Token: 0x04002AA8 RID: 10920
	[Serialize]
	private BuildingHP.DamageSourceInfo damageSourceInfo;

	// Token: 0x04002AA9 RID: 10921
	private static readonly EventSystem.IntraObjectHandler<BuildingHP> OnDoBuildingDamageDelegate = new EventSystem.IntraObjectHandler<BuildingHP>(delegate(BuildingHP component, object data)
	{
		component.OnDoBuildingDamage(data);
	});

	// Token: 0x04002AAA RID: 10922
	private static readonly EventSystem.IntraObjectHandler<BuildingHP> DestroyOnDamagedDelegate = new EventSystem.IntraObjectHandler<BuildingHP>(delegate(BuildingHP component, object data)
	{
		component.DestroyOnDamaged(data);
	});

	// Token: 0x04002AAB RID: 10923
	public static List<Meter> kbacQueryList = new List<Meter>();

	// Token: 0x04002AAC RID: 10924
	public bool destroyOnDamaged;

	// Token: 0x04002AAD RID: 10925
	public bool invincible;

	// Token: 0x04002AAE RID: 10926
	[MyCmpGet]
	private Building building;

	// Token: 0x04002AAF RID: 10927
	private BuildingHP.SMInstance smi;

	// Token: 0x04002AB0 RID: 10928
	private float minDamagePopInterval = 4f;

	// Token: 0x04002AB1 RID: 10929
	private float lastPopTime;

	// Token: 0x02000CB1 RID: 3249
	public struct DamageSourceInfo
	{
		// Token: 0x06003DE6 RID: 15846 RVA: 0x000CC9C7 File Offset: 0x000CABC7
		public override string ToString()
		{
			return this.source;
		}

		// Token: 0x04002AB2 RID: 10930
		public int damage;

		// Token: 0x04002AB3 RID: 10931
		public string source;

		// Token: 0x04002AB4 RID: 10932
		public string popString;

		// Token: 0x04002AB5 RID: 10933
		public SpawnFXHashes takeDamageEffect;

		// Token: 0x04002AB6 RID: 10934
		public string fullDamageEffectName;

		// Token: 0x04002AB7 RID: 10935
		public string statusItemID;
	}

	// Token: 0x02000CB2 RID: 3250
	public class SMInstance : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.GameInstance
	{
		// Token: 0x06003DE7 RID: 15847 RVA: 0x000CC9CF File Offset: 0x000CABCF
		public SMInstance(BuildingHP master) : base(master)
		{
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x00240E08 File Offset: 0x0023F008
		public Notification CreateBrokenMachineNotification()
		{
			return new Notification(MISC.NOTIFICATIONS.BROKENMACHINE.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.BROKENMACHINE.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + base.master.damageSourceInfo.source, false, 0f, null, null, null, true, false, false);
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x00240E6C File Offset: 0x0023F06C
		public void ShowProgressBar(bool show)
		{
			if (show && Grid.IsValidCell(Grid.PosToCell(base.gameObject)) && Grid.IsVisible(Grid.PosToCell(base.gameObject)))
			{
				this.CreateProgressBar();
				return;
			}
			if (this.progressBar != null)
			{
				this.progressBar.gameObject.DeleteObject();
				this.progressBar = null;
			}
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x000CC9D8 File Offset: 0x000CABD8
		public void UpdateMeter()
		{
			if (this.progressBar == null)
			{
				this.ShowProgressBar(true);
			}
			if (this.progressBar)
			{
				this.progressBar.Update();
			}
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x000CCA07 File Offset: 0x000CAC07
		private float HealthPercent()
		{
			return (float)base.smi.master.HitPoints / (float)base.smi.master.building.Def.HitPoints;
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x00240ECC File Offset: 0x0023F0CC
		private void CreateProgressBar()
		{
			if (this.progressBar != null)
			{
				return;
			}
			this.progressBar = Util.KInstantiateUI<ProgressBar>(ProgressBarsConfig.Instance.progressBarPrefab, null, false);
			this.progressBar.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform);
			this.progressBar.name = base.smi.master.name + "." + base.smi.master.GetType().Name + " ProgressBar";
			this.progressBar.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("ProgressBar");
			this.progressBar.SetUpdateFunc(new Func<float>(this.HealthPercent));
			this.progressBar.barColor = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
			CanvasGroup component = this.progressBar.GetComponent<CanvasGroup>();
			component.interactable = false;
			component.blocksRaycasts = false;
			this.progressBar.Update();
			float d = 0.15f;
			Vector3 vector = base.gameObject.transform.GetPosition() + Vector3.down * d;
			vector.z += 0.05f;
			Rotatable component2 = base.GetComponent<Rotatable>();
			if (component2 == null || component2.GetOrientation() == Orientation.Neutral || base.smi.master.building.Def.WidthInCells < 2 || base.smi.master.building.Def.HeightInCells < 2)
			{
				vector -= Vector3.right * 0.5f * (float)(base.smi.master.building.Def.WidthInCells % 2);
			}
			else
			{
				vector += Vector3.left * (1f + 0.5f * (float)(base.smi.master.building.Def.WidthInCells % 2));
			}
			this.progressBar.transform.SetPosition(vector);
			this.progressBar.SetVisibility(true);
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x002410FC File Offset: 0x0023F2FC
		private static string ToolTipResolver(List<Notification> notificationList, object data)
		{
			string text = "";
			for (int i = 0; i < notificationList.Count; i++)
			{
				Notification notification = notificationList[i];
				text += string.Format(BUILDINGS.DAMAGESOURCES.NOTIFICATION_TOOLTIP, notification.NotifierName, (string)notification.tooltipData);
				if (i < notificationList.Count - 1)
				{
					text += "\n";
				}
			}
			return text;
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x00241168 File Offset: 0x0023F368
		public void ShowDamagedEffect()
		{
			if (base.master.damageSourceInfo.takeDamageEffect != SpawnFXHashes.None)
			{
				BuildingDef def = base.master.GetComponent<BuildingComplete>().Def;
				int cell = Grid.OffsetCell(Grid.PosToCell(base.master), 0, def.HeightInCells - 1);
				Game.Instance.SpawnFX(base.master.damageSourceInfo.takeDamageEffect, cell, 0f);
			}
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x002411D4 File Offset: 0x0023F3D4
		public FXAnim.Instance InstantiateDamageFX()
		{
			if (base.master.damageSourceInfo.fullDamageEffectName == null)
			{
				return null;
			}
			BuildingDef def = base.master.GetComponent<BuildingComplete>().Def;
			Vector3 zero = Vector3.zero;
			if (def.HeightInCells > 1)
			{
				zero = new Vector3(0f, (float)(def.HeightInCells - 1), 0f);
			}
			else
			{
				zero = new Vector3(0f, 0.5f, 0f);
			}
			return new FXAnim.Instance(base.smi.master, base.master.damageSourceInfo.fullDamageEffectName, "idle", KAnim.PlayMode.Loop, zero, Color.white);
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x00241278 File Offset: 0x0023F478
		public void SetCrackOverlayValue(float value)
		{
			KBatchedAnimController component = base.master.GetComponent<KBatchedAnimController>();
			if (component == null)
			{
				return;
			}
			component.SetBlendValue(value);
			BuildingHP.kbacQueryList.Clear();
			base.master.GetComponentsInChildren<Meter>(BuildingHP.kbacQueryList);
			for (int i = 0; i < BuildingHP.kbacQueryList.Count; i++)
			{
				BuildingHP.kbacQueryList[i].GetComponent<KBatchedAnimController>().SetBlendValue(value);
			}
		}

		// Token: 0x04002AB8 RID: 10936
		private ProgressBar progressBar;
	}

	// Token: 0x02000CB4 RID: 3252
	public class States : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP>
	{
		// Token: 0x06003DF4 RID: 15860 RVA: 0x002412E8 File Offset: 0x0023F4E8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.healthy;
			this.healthy.DefaultState(this.healthy.imperfect).EventTransition(GameHashes.BuildingReceivedDamage, this.damaged, (BuildingHP.SMInstance smi) => smi.master.HitPoints <= 0);
			this.healthy.imperfect.Enter(delegate(BuildingHP.SMInstance smi)
			{
				smi.ShowProgressBar(true);
			}).DefaultState(this.healthy.imperfect.playEffect).EventTransition(GameHashes.BuildingPartiallyRepaired, this.healthy.perfect, (BuildingHP.SMInstance smi) => smi.master.HitPoints == smi.master.building.Def.HitPoints).EventHandler(GameHashes.BuildingPartiallyRepaired, delegate(BuildingHP.SMInstance smi)
			{
				smi.UpdateMeter();
			}).ToggleStatusItem(delegate(BuildingHP.SMInstance smi)
			{
				if (smi.master.damageSourceInfo.statusItemID == null)
				{
					return null;
				}
				return Db.Get().BuildingStatusItems.Get(smi.master.damageSourceInfo.statusItemID);
			}, null).Exit(delegate(BuildingHP.SMInstance smi)
			{
				smi.ShowProgressBar(false);
			});
			this.healthy.imperfect.playEffect.Transition(this.healthy.imperfect.waiting, (BuildingHP.SMInstance smi) => true, UpdateRate.SIM_200ms);
			this.healthy.imperfect.waiting.ScheduleGoTo((BuildingHP.SMInstance smi) => UnityEngine.Random.Range(15f, 30f), this.healthy.imperfect.playEffect);
			this.healthy.perfect.EventTransition(GameHashes.BuildingReceivedDamage, this.healthy.imperfect, (BuildingHP.SMInstance smi) => smi.master.HitPoints < smi.master.building.Def.HitPoints);
			this.damaged.Enter(delegate(BuildingHP.SMInstance smi)
			{
				Operational component = smi.GetComponent<Operational>();
				if (component != null)
				{
					component.SetFlag(BuildingHP.States.healthyFlag, false);
				}
				smi.ShowProgressBar(true);
				smi.master.Trigger(774203113, smi.master);
				smi.SetCrackOverlayValue(1f);
			}).ToggleNotification((BuildingHP.SMInstance smi) => smi.CreateBrokenMachineNotification()).ToggleStatusItem(Db.Get().BuildingStatusItems.Broken, null).ToggleFX((BuildingHP.SMInstance smi) => smi.InstantiateDamageFX()).EventTransition(GameHashes.BuildingPartiallyRepaired, this.healthy.perfect, (BuildingHP.SMInstance smi) => smi.master.HitPoints == smi.master.building.Def.HitPoints).EventHandler(GameHashes.BuildingPartiallyRepaired, delegate(BuildingHP.SMInstance smi)
			{
				smi.UpdateMeter();
			}).Exit(delegate(BuildingHP.SMInstance smi)
			{
				Operational component = smi.GetComponent<Operational>();
				if (component != null)
				{
					component.SetFlag(BuildingHP.States.healthyFlag, true);
				}
				smi.ShowProgressBar(false);
				smi.SetCrackOverlayValue(0f);
			});
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x0024160C File Offset: 0x0023F80C
		private Chore CreateRepairChore(BuildingHP.SMInstance smi)
		{
			return new WorkChore<BuildingHP>(Db.Get().ChoreTypes.Repair, smi.master, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x04002ABB RID: 10939
		private static readonly Operational.Flag healthyFlag = new Operational.Flag("healthy", Operational.Flag.Type.Functional);

		// Token: 0x04002ABC RID: 10940
		public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State damaged;

		// Token: 0x04002ABD RID: 10941
		public BuildingHP.States.Healthy healthy;

		// Token: 0x02000CB5 RID: 3253
		public class Healthy : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State
		{
			// Token: 0x04002ABE RID: 10942
			public BuildingHP.States.ImperfectStates imperfect;

			// Token: 0x04002ABF RID: 10943
			public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State perfect;
		}

		// Token: 0x02000CB6 RID: 3254
		public class ImperfectStates : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State
		{
			// Token: 0x04002AC0 RID: 10944
			public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State playEffect;

			// Token: 0x04002AC1 RID: 10945
			public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State waiting;
		}
	}
}
