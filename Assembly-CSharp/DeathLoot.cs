using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200123F RID: 4671
public class DeathLoot : GameStateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>
{
	// Token: 0x06005F08 RID: 24328 RVA: 0x000E2841 File Offset: 0x000E0A41
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x040043D8 RID: 17368
	private StateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>.BoolParameter WasLoopDropped;

	// Token: 0x02001240 RID: 4672
	public class Loot
	{
		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06005F0B RID: 24331 RVA: 0x000E2863 File Offset: 0x000E0A63
		// (set) Token: 0x06005F0A RID: 24330 RVA: 0x000E285A File Offset: 0x000E0A5A
		public Tag Id { get; private set; } = Tag.Invalid;

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06005F0D RID: 24333 RVA: 0x000E2874 File Offset: 0x000E0A74
		// (set) Token: 0x06005F0C RID: 24332 RVA: 0x000E286B File Offset: 0x000E0A6B
		public bool IsElement { get; private set; }

		// Token: 0x06005F0E RID: 24334 RVA: 0x000E287C File Offset: 0x000E0A7C
		public Loot(Tag tag)
		{
			this.Id = tag;
			this.IsElement = false;
			this.Quantity = 1f;
		}

		// Token: 0x06005F0F RID: 24335 RVA: 0x000E28A8 File Offset: 0x000E0AA8
		public Loot(SimHashes element, float quantity)
		{
			this.Id = element.CreateTag();
			this.IsElement = true;
			this.Quantity = quantity;
		}

		// Token: 0x040043DB RID: 17371
		public float Quantity;
	}

	// Token: 0x02001241 RID: 4673
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040043DC RID: 17372
		public DeathLoot.Loot[] loot;

		// Token: 0x040043DD RID: 17373
		public CellOffset lootSpawnOffset;
	}

	// Token: 0x02001242 RID: 4674
	public new class Instance : GameStateMachine<DeathLoot, DeathLoot.Instance, IStateMachineTarget, DeathLoot.Def>.GameInstance
	{
		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06005F11 RID: 24337 RVA: 0x000E28D5 File Offset: 0x000E0AD5
		public bool WasLoopDropped
		{
			get
			{
				return base.sm.WasLoopDropped.Get(base.smi);
			}
		}

		// Token: 0x06005F12 RID: 24338 RVA: 0x000E28ED File Offset: 0x000E0AED
		public Instance(IStateMachineTarget master, DeathLoot.Def def) : base(master, def)
		{
			base.Subscribe(1623392196, new Action<object>(this.OnDeath));
		}

		// Token: 0x06005F13 RID: 24339 RVA: 0x000E290E File Offset: 0x000E0B0E
		private void OnDeath(object obj)
		{
			if (!this.WasLoopDropped)
			{
				base.sm.WasLoopDropped.Set(true, this, false);
				this.CreateLoot();
			}
		}

		// Token: 0x06005F14 RID: 24340 RVA: 0x002B2E20 File Offset: 0x002B1020
		public GameObject[] CreateLoot()
		{
			if (base.def.loot == null)
			{
				return null;
			}
			GameObject[] array = new GameObject[base.def.loot.Length];
			for (int i = 0; i < base.def.loot.Length; i++)
			{
				DeathLoot.Loot loot = base.def.loot[i];
				if (!(loot.Id == Tag.Invalid))
				{
					GameObject gameObject = Scenario.SpawnPrefab(this.GetLootSpawnCell(), 0, 0, loot.Id.ToString(), Grid.SceneLayer.Ore);
					gameObject.SetActive(true);
					Edible component = gameObject.GetComponent<Edible>();
					if (component)
					{
						ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
					}
					if (loot.IsElement)
					{
						PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
						if (component2 != null)
						{
							component2.Mass = loot.Quantity;
						}
					}
					array[i] = gameObject;
				}
			}
			return array;
		}

		// Token: 0x06005F15 RID: 24341 RVA: 0x002B2F30 File Offset: 0x002B1130
		public int GetLootSpawnCell()
		{
			int num = Grid.PosToCell(base.gameObject);
			int num2 = Grid.OffsetCell(num, base.def.lootSpawnOffset);
			if (Grid.IsWorldValidCell(num2) && Grid.IsValidCellInWorld(num2, base.gameObject.GetMyWorldId()))
			{
				return num2;
			}
			return num;
		}

		// Token: 0x06005F16 RID: 24342 RVA: 0x000E2933 File Offset: 0x000E0B33
		protected override void OnCleanUp()
		{
			base.Unsubscribe(1623392196, new Action<object>(this.OnDeath));
		}
	}
}
