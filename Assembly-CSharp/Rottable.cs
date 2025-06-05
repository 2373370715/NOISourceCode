using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000B2A RID: 2858
public class Rottable : GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>
{
	// Token: 0x06003503 RID: 13571 RVA: 0x00219698 File Offset: 0x00217898
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Fresh;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.TagTransition(GameTags.Preserved, this.Preserved, false).TagTransition(GameTags.Entombed, this.Preserved, false);
		this.Fresh.ToggleStatusItem(Db.Get().CreatureStatusItems.Fresh, (Rottable.Instance smi) => smi).ParamTransition<float>(this.rotParameter, this.Stale_Pre, (Rottable.Instance smi, float p) => p <= smi.def.spoilTime - (smi.def.spoilTime - smi.def.staleTime)).Update(delegate(Rottable.Instance smi, float dt)
		{
			smi.sm.rotParameter.Set(smi.RotValue, smi, false);
		}, UpdateRate.SIM_1000ms, true).FastUpdate("Rot", Rottable.rotCB, UpdateRate.SIM_1000ms, true);
		this.Preserved.TagTransition(Rottable.PRESERVED_TAGS, this.Fresh, true).Enter("RefreshModifiers", delegate(Rottable.Instance smi)
		{
			smi.RefreshModifiers(0f);
		});
		this.Stale_Pre.Enter(delegate(Rottable.Instance smi)
		{
			smi.GoTo(this.Stale);
		});
		this.Stale.ToggleStatusItem(Db.Get().CreatureStatusItems.Stale, (Rottable.Instance smi) => smi).ParamTransition<float>(this.rotParameter, this.Fresh, (Rottable.Instance smi, float p) => p > smi.def.spoilTime - (smi.def.spoilTime - smi.def.staleTime)).ParamTransition<float>(this.rotParameter, this.Spoiled, GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.IsLTEZero).Update(delegate(Rottable.Instance smi, float dt)
		{
			smi.sm.rotParameter.Set(smi.RotValue, smi, false);
		}, UpdateRate.SIM_1000ms, false).FastUpdate("Rot", Rottable.rotCB, UpdateRate.SIM_1000ms, false);
		this.Spoiled.Enter(delegate(Rottable.Instance smi)
		{
			GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(smi.master.gameObject), 0, 0, "RotPile", Grid.SceneLayer.Ore);
			gameObject.gameObject.GetComponent<KSelectable>().SetName(UI.GAMEOBJECTEFFECTS.ROTTEN + " " + smi.master.gameObject.GetProperName());
			gameObject.transform.SetPosition(smi.master.transform.GetPosition());
			gameObject.GetComponent<PrimaryElement>().Mass = smi.master.GetComponent<PrimaryElement>().Mass;
			gameObject.GetComponent<PrimaryElement>().Temperature = smi.master.GetComponent<PrimaryElement>().Temperature;
			gameObject.SetActive(true);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ITEMS.FOOD.ROTPILE.NAME, gameObject.transform, 1.5f, false);
			Edible component = smi.GetComponent<Edible>();
			if (component != null)
			{
				if (component.worker != null)
				{
					ChoreDriver component2 = component.worker.GetComponent<ChoreDriver>();
					if (component2 != null && component2.GetCurrentChore() != null)
					{
						component2.GetCurrentChore().Fail("food rotted");
					}
				}
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.ROTTED, "{0}", smi.gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.ROTTED_CONTEXT);
			}
			Util.KDestroyGameObject(smi.gameObject);
		});
	}

	// Token: 0x06003504 RID: 13572 RVA: 0x002198BC File Offset: 0x00217ABC
	private static string OnStaleTooltip(List<Notification> notifications, object data)
	{
		string text = "\n";
		foreach (Notification notification in notifications)
		{
			if (notification.tooltipData != null)
			{
				GameObject gameObject = (GameObject)notification.tooltipData;
				if (gameObject != null)
				{
					text = text + "\n" + gameObject.GetProperName();
				}
			}
		}
		return string.Format(MISC.NOTIFICATIONS.FOODSTALE.TOOLTIP, text);
	}

	// Token: 0x06003505 RID: 13573 RVA: 0x00219948 File Offset: 0x00217B48
	public static void SetStatusItems(IRottable rottable)
	{
		Grid.PosToCell(rottable.gameObject);
		KSelectable component = rottable.gameObject.GetComponent<KSelectable>();
		Rottable.RotRefrigerationLevel rotRefrigerationLevel = Rottable.RefrigerationLevel(rottable);
		if (rotRefrigerationLevel != Rottable.RotRefrigerationLevel.Refrigerated)
		{
			if (rotRefrigerationLevel == Rottable.RotRefrigerationLevel.Frozen)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, Db.Get().CreatureStatusItems.RefrigeratedFrozen, rottable);
			}
			else
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, Db.Get().CreatureStatusItems.Unrefrigerated, rottable);
			}
		}
		else
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, Db.Get().CreatureStatusItems.Refrigerated, rottable);
		}
		Rottable.RotAtmosphereQuality rotAtmosphereQuality = Rottable.AtmosphereQuality(rottable);
		if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Sterilizing)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, Db.Get().CreatureStatusItems.SterilizingAtmosphere, null);
			return;
		}
		if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Contaminating)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, Db.Get().CreatureStatusItems.ContaminatedAtmosphere, null);
			return;
		}
		component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, null, null);
	}

	// Token: 0x06003506 RID: 13574 RVA: 0x00219A68 File Offset: 0x00217C68
	public static bool IsInActiveFridge(IRottable rottable)
	{
		Pickupable component = rottable.gameObject.GetComponent<Pickupable>();
		if (component != null && component.storage != null)
		{
			Refrigerator component2 = component.storage.GetComponent<Refrigerator>();
			return component2 != null && component2.IsActive();
		}
		return false;
	}

	// Token: 0x06003507 RID: 13575 RVA: 0x00219AB8 File Offset: 0x00217CB8
	public static Rottable.RotRefrigerationLevel RefrigerationLevel(IRottable rottable)
	{
		int num = Grid.PosToCell(rottable.gameObject);
		Rottable.Instance smi = rottable.gameObject.GetSMI<Rottable.Instance>();
		PrimaryElement component = rottable.gameObject.GetComponent<PrimaryElement>();
		float num2 = component.Temperature;
		bool flag = false;
		if (!Grid.IsValidCell(num))
		{
			if (!smi.IsRottableInSpace())
			{
				return Rottable.RotRefrigerationLevel.Normal;
			}
			flag = true;
		}
		if (!flag && Grid.Element[num].id != SimHashes.Vacuum)
		{
			num2 = Mathf.Min(Grid.Temperature[num], component.Temperature);
		}
		if (num2 < rottable.PreserveTemperature)
		{
			return Rottable.RotRefrigerationLevel.Frozen;
		}
		if (num2 < rottable.RotTemperature || Rottable.IsInActiveFridge(rottable))
		{
			return Rottable.RotRefrigerationLevel.Refrigerated;
		}
		return Rottable.RotRefrigerationLevel.Normal;
	}

	// Token: 0x06003508 RID: 13576 RVA: 0x00219B58 File Offset: 0x00217D58
	public static Rottable.RotAtmosphereQuality AtmosphereQuality(IRottable rottable)
	{
		int num = Grid.PosToCell(rottable.gameObject);
		int num2 = Grid.CellAbove(num);
		if (!Grid.IsValidCell(num))
		{
			if (rottable.gameObject.GetSMI<Rottable.Instance>().IsRottableInSpace())
			{
				return Rottable.RotAtmosphereQuality.Sterilizing;
			}
			return Rottable.RotAtmosphereQuality.Normal;
		}
		else
		{
			SimHashes id = Grid.Element[num].id;
			Rottable.RotAtmosphereQuality rotAtmosphereQuality = Rottable.RotAtmosphereQuality.Normal;
			Rottable.AtmosphereModifier.TryGetValue((int)id, out rotAtmosphereQuality);
			Rottable.RotAtmosphereQuality rotAtmosphereQuality2 = Rottable.RotAtmosphereQuality.Normal;
			if (Grid.IsValidCell(num2))
			{
				SimHashes id2 = Grid.Element[num2].id;
				if (!Rottable.AtmosphereModifier.TryGetValue((int)id2, out rotAtmosphereQuality2))
				{
					rotAtmosphereQuality2 = rotAtmosphereQuality;
				}
			}
			else
			{
				rotAtmosphereQuality2 = rotAtmosphereQuality;
			}
			if (rotAtmosphereQuality == rotAtmosphereQuality2)
			{
				return rotAtmosphereQuality;
			}
			if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Contaminating || rotAtmosphereQuality2 == Rottable.RotAtmosphereQuality.Contaminating)
			{
				return Rottable.RotAtmosphereQuality.Contaminating;
			}
			if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Normal || rotAtmosphereQuality2 == Rottable.RotAtmosphereQuality.Normal)
			{
				return Rottable.RotAtmosphereQuality.Normal;
			}
			return Rottable.RotAtmosphereQuality.Sterilizing;
		}
	}

	// Token: 0x04002476 RID: 9334
	public StateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.FloatParameter rotParameter;

	// Token: 0x04002477 RID: 9335
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Preserved;

	// Token: 0x04002478 RID: 9336
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Fresh;

	// Token: 0x04002479 RID: 9337
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Stale_Pre;

	// Token: 0x0400247A RID: 9338
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Stale;

	// Token: 0x0400247B RID: 9339
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Spoiled;

	// Token: 0x0400247C RID: 9340
	private static readonly Tag[] PRESERVED_TAGS = new Tag[]
	{
		GameTags.Preserved,
		GameTags.Dehydrated,
		GameTags.Entombed
	};

	// Token: 0x0400247D RID: 9341
	private static readonly Rottable.RotCB rotCB = new Rottable.RotCB();

	// Token: 0x0400247E RID: 9342
	public static Dictionary<int, Rottable.RotAtmosphereQuality> AtmosphereModifier = new Dictionary<int, Rottable.RotAtmosphereQuality>
	{
		{
			721531317,
			Rottable.RotAtmosphereQuality.Contaminating
		},
		{
			1887387588,
			Rottable.RotAtmosphereQuality.Contaminating
		},
		{
			-1528777920,
			Rottable.RotAtmosphereQuality.Normal
		},
		{
			1836671383,
			Rottable.RotAtmosphereQuality.Normal
		},
		{
			1960575215,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-899515856,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1554872654,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1858722091,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			758759285,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1046145888,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1324664829,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1406916018,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-432557516,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-805366663,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			1966552544,
			Rottable.RotAtmosphereQuality.Sterilizing
		}
	};

	// Token: 0x02000B2B RID: 2859
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400247F RID: 9343
		public float spoilTime;

		// Token: 0x04002480 RID: 9344
		public float staleTime;

		// Token: 0x04002481 RID: 9345
		public float preserveTemperature = 255.15f;

		// Token: 0x04002482 RID: 9346
		public float rotTemperature = 277.15f;
	}

	// Token: 0x02000B2C RID: 2860
	private class RotCB : UpdateBucketWithUpdater<Rottable.Instance>.IUpdater
	{
		// Token: 0x0600350D RID: 13581 RVA: 0x000C6FEF File Offset: 0x000C51EF
		public void Update(Rottable.Instance smi, float dt)
		{
			smi.Rot(smi, dt);
		}
	}

	// Token: 0x02000B2D RID: 2861
	public new class Instance : GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.GameInstance, IRottable
	{
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x0600350F RID: 13583 RVA: 0x000C6FF9 File Offset: 0x000C51F9
		// (set) Token: 0x06003510 RID: 13584 RVA: 0x000C7006 File Offset: 0x000C5206
		public float RotValue
		{
			get
			{
				return this.rotAmountInstance.value;
			}
			set
			{
				base.sm.rotParameter.Set(value, this, false);
				this.rotAmountInstance.SetValue(value);
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06003511 RID: 13585 RVA: 0x000C7029 File Offset: 0x000C5229
		public float RotConstitutionPercentage
		{
			get
			{
				return this.RotValue / base.def.spoilTime;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06003512 RID: 13586 RVA: 0x000C703D File Offset: 0x000C523D
		public float RotTemperature
		{
			get
			{
				return base.def.rotTemperature;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06003513 RID: 13587 RVA: 0x000C704A File Offset: 0x000C524A
		public float PreserveTemperature
		{
			get
			{
				return base.def.preserveTemperature;
			}
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x00219D0C File Offset: 0x00217F0C
		public Instance(IStateMachineTarget master, Rottable.Def def) : base(master, def)
		{
			this.pickupable = base.gameObject.RequireComponent<Pickupable>();
			base.master.Subscribe(-2064133523, new Action<object>(this.OnAbsorb));
			base.master.Subscribe(1335436905, new Action<object>(this.OnSplitFromChunk));
			this.primaryElement = base.gameObject.GetComponent<PrimaryElement>();
			Amounts amounts = master.gameObject.GetAmounts();
			this.rotAmountInstance = amounts.Add(new AmountInstance(Db.Get().Amounts.Rot, master.gameObject));
			this.rotAmountInstance.maxAttribute.Add(new AttributeModifier("Rot", def.spoilTime, null, false, false, true));
			this.rotAmountInstance.SetValue(def.spoilTime);
			base.sm.rotParameter.Set(this.rotAmountInstance.value, base.smi, false);
			if (Rottable.Instance.unrefrigeratedModifier == null)
			{
				Rottable.Instance.unrefrigeratedModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -0.7f, DUPLICANTS.MODIFIERS.ROTTEMPERATURE.UNREFRIGERATED, false, false, true);
				Rottable.Instance.refrigeratedModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -0.2f, DUPLICANTS.MODIFIERS.ROTTEMPERATURE.REFRIGERATED, false, false, true);
				Rottable.Instance.frozenModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, --0f, DUPLICANTS.MODIFIERS.ROTTEMPERATURE.FROZEN, false, false, true);
				Rottable.Instance.contaminatedAtmosphereModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -1f, DUPLICANTS.MODIFIERS.ROTATMOSPHERE.CONTAMINATED, false, false, true);
				Rottable.Instance.normalAtmosphereModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -0.3f, DUPLICANTS.MODIFIERS.ROTATMOSPHERE.NORMAL, false, false, true);
				Rottable.Instance.sterileAtmosphereModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, --0f, DUPLICANTS.MODIFIERS.ROTATMOSPHERE.STERILE, false, false, true);
			}
			this.RefreshModifiers(0f);
		}

		// Token: 0x06003515 RID: 13589 RVA: 0x00219F28 File Offset: 0x00218128
		[OnDeserialized]
		private void OnDeserialized()
		{
			if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 23))
			{
				this.rotAmountInstance.SetValue(this.rotAmountInstance.value * 2f);
			}
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x00219F6C File Offset: 0x0021816C
		public string StateString()
		{
			string result = "";
			if (base.smi.GetCurrentState() == base.sm.Fresh)
			{
				result = Db.Get().CreatureStatusItems.Fresh.resolveStringCallback(CREATURES.STATUSITEMS.FRESH.NAME, this);
			}
			if (base.smi.GetCurrentState() == base.sm.Stale)
			{
				result = Db.Get().CreatureStatusItems.Fresh.resolveStringCallback(CREATURES.STATUSITEMS.STALE.NAME, this);
			}
			return result;
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x000C7057 File Offset: 0x000C5257
		public void Rot(Rottable.Instance smi, float deltaTime)
		{
			this.RefreshModifiers(deltaTime);
			if (smi.pickupable.storage != null)
			{
				smi.pickupable.storage.Trigger(-1197125120, null);
			}
		}

		// Token: 0x06003518 RID: 13592 RVA: 0x00219FFC File Offset: 0x002181FC
		public bool IsRottableInSpace()
		{
			if (base.gameObject.GetMyWorld() == null)
			{
				Pickupable component = base.GetComponent<Pickupable>();
				if (component != null && component.storage && (component.storage.GetComponent<RocketModuleCluster>() || component.storage.GetComponent<ClusterTraveler>()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x0021A060 File Offset: 0x00218260
		public void RefreshModifiers(float dt)
		{
			if (this.GetMaster().isNull)
			{
				return;
			}
			if (!Grid.IsValidCell(Grid.PosToCell(base.gameObject)) && !this.IsRottableInSpace())
			{
				return;
			}
			this.rotAmountInstance.deltaAttribute.ClearModifiers();
			KPrefabID component = base.GetComponent<KPrefabID>();
			if (!component.HasAnyTags(Rottable.PRESERVED_TAGS))
			{
				Rottable.RotRefrigerationLevel rotRefrigerationLevel = Rottable.RefrigerationLevel(this);
				if (rotRefrigerationLevel != Rottable.RotRefrigerationLevel.Refrigerated)
				{
					if (rotRefrigerationLevel == Rottable.RotRefrigerationLevel.Frozen)
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.frozenModifier);
					}
					else
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.unrefrigeratedModifier);
					}
				}
				else
				{
					this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.refrigeratedModifier);
				}
				Rottable.RotAtmosphereQuality rotAtmosphereQuality = Rottable.AtmosphereQuality(this);
				if (rotAtmosphereQuality != Rottable.RotAtmosphereQuality.Sterilizing)
				{
					if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Contaminating)
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.contaminatedAtmosphereModifier);
					}
					else
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.normalAtmosphereModifier);
					}
				}
				else
				{
					this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.sterileAtmosphereModifier);
				}
			}
			if (component.HasTag(Db.Get().Spices.PreservingSpice.Id))
			{
				this.rotAmountInstance.deltaAttribute.Add(Db.Get().Spices.PreservingSpice.FoodModifier);
			}
			Rottable.SetStatusItems(this);
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x0021A1AC File Offset: 0x002183AC
		private void OnAbsorb(object data)
		{
			Pickupable pickupable = (Pickupable)data;
			if (pickupable != null)
			{
				PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
				PrimaryElement primaryElement = pickupable.PrimaryElement;
				Rottable.Instance smi = pickupable.gameObject.GetSMI<Rottable.Instance>();
				if (component != null && primaryElement != null && smi != null)
				{
					float num = component.Units * base.sm.rotParameter.Get(base.smi);
					float num2 = primaryElement.Units * base.sm.rotParameter.Get(smi);
					float value = (num + num2) / (component.Units + primaryElement.Units);
					base.sm.rotParameter.Set(value, base.smi, false);
				}
			}
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x000C7089 File Offset: 0x000C5289
		public bool IsRotLevelStackable(Rottable.Instance other)
		{
			return Mathf.Abs(this.RotConstitutionPercentage - other.RotConstitutionPercentage) < 0.1f;
		}

		// Token: 0x0600351C RID: 13596 RVA: 0x000C70A4 File Offset: 0x000C52A4
		public string GetToolTip()
		{
			return this.rotAmountInstance.GetTooltip();
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x0021A264 File Offset: 0x00218464
		private void OnSplitFromChunk(object data)
		{
			Pickupable pickupable = (Pickupable)data;
			if (pickupable != null)
			{
				Rottable.Instance smi = pickupable.GetSMI<Rottable.Instance>();
				if (smi != null)
				{
					this.RotValue = smi.RotValue;
				}
			}
		}

		// Token: 0x0600351E RID: 13598 RVA: 0x000C70B1 File Offset: 0x000C52B1
		public void OnPreserved(object data)
		{
			if ((bool)data)
			{
				base.smi.GoTo(base.sm.Preserved);
				return;
			}
			base.smi.GoTo(base.sm.Fresh);
		}

		// Token: 0x04002483 RID: 9347
		private AmountInstance rotAmountInstance;

		// Token: 0x04002484 RID: 9348
		private static AttributeModifier unrefrigeratedModifier;

		// Token: 0x04002485 RID: 9349
		private static AttributeModifier refrigeratedModifier;

		// Token: 0x04002486 RID: 9350
		private static AttributeModifier frozenModifier;

		// Token: 0x04002487 RID: 9351
		private static AttributeModifier contaminatedAtmosphereModifier;

		// Token: 0x04002488 RID: 9352
		private static AttributeModifier normalAtmosphereModifier;

		// Token: 0x04002489 RID: 9353
		private static AttributeModifier sterileAtmosphereModifier;

		// Token: 0x0400248A RID: 9354
		public PrimaryElement primaryElement;

		// Token: 0x0400248B RID: 9355
		public Pickupable pickupable;
	}

	// Token: 0x02000B2E RID: 2862
	public enum RotAtmosphereQuality
	{
		// Token: 0x0400248D RID: 9357
		Normal,
		// Token: 0x0400248E RID: 9358
		Sterilizing,
		// Token: 0x0400248F RID: 9359
		Contaminating
	}

	// Token: 0x02000B2F RID: 2863
	public enum RotRefrigerationLevel
	{
		// Token: 0x04002491 RID: 9361
		Normal,
		// Token: 0x04002492 RID: 9362
		Refrigerated,
		// Token: 0x04002493 RID: 9363
		Frozen
	}
}
