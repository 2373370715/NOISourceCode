using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001CD9 RID: 7385
[SerializationConfig(MemberSerialization.OptIn)]
public class CreatureLure : StateMachineComponent<CreatureLure.StatesInstance>
{
	// Token: 0x060099FC RID: 39420 RVA: 0x00108939 File Offset: 0x00106B39
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.operational = base.GetComponent<Operational>();
		base.Subscribe<CreatureLure>(-905833192, CreatureLure.OnCopySettingsDelegate);
	}

	// Token: 0x060099FD RID: 39421 RVA: 0x003C5CB8 File Offset: 0x003C3EB8
	private void OnCopySettings(object data)
	{
		CreatureLure component = ((GameObject)data).GetComponent<CreatureLure>();
		if (component != null)
		{
			this.ChangeBaitSetting(component.activeBaitSetting);
		}
	}

	// Token: 0x060099FE RID: 39422 RVA: 0x003C5CE8 File Offset: 0x003C3EE8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.activeBaitSetting == Tag.Invalid)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoLureElementSelected, null);
		}
		else
		{
			this.ChangeBaitSetting(this.activeBaitSetting);
			this.OnStorageChange(null);
		}
		base.Subscribe<CreatureLure>(-1697596308, CreatureLure.OnStorageChangeDelegate);
	}

	// Token: 0x060099FF RID: 39423 RVA: 0x003C5D5C File Offset: 0x003C3F5C
	private void OnStorageChange(object data = null)
	{
		bool value = this.baitStorage.GetAmountAvailable(this.activeBaitSetting) > 0f;
		this.operational.SetFlag(CreatureLure.baited, value);
	}

	// Token: 0x06009A00 RID: 39424 RVA: 0x003C5D94 File Offset: 0x003C3F94
	public void ChangeBaitSetting(Tag baitSetting)
	{
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("SwitchedResource");
		}
		if (baitSetting != this.activeBaitSetting)
		{
			this.activeBaitSetting = baitSetting;
			this.baitStorage.DropAll(false, false, default(Vector3), true, null);
		}
		base.smi.GoTo(base.smi.sm.idle);
		this.baitStorage.storageFilters = new List<Tag>
		{
			this.activeBaitSetting
		};
		if (baitSetting != Tag.Invalid)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoLureElementSelected, false);
			if (base.smi.master.baitStorage.IsEmpty())
			{
				this.CreateFetchChore();
				return;
			}
		}
		else
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoLureElementSelected, null);
		}
	}

	// Token: 0x06009A01 RID: 39425 RVA: 0x003C5E80 File Offset: 0x003C4080
	protected void CreateFetchChore()
	{
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("Overwrite");
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.AwaitingBaitDelivery, false);
		if (this.activeBaitSetting == Tag.Invalid)
		{
			return;
		}
		this.fetchChore = new FetchChore(Db.Get().ChoreTypes.RanchingFetch, this.baitStorage, 100f, new HashSet<Tag>
		{
			this.activeBaitSetting
		}, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, null, null, null, Operational.State.None, 0);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.AwaitingBaitDelivery, null);
	}

	// Token: 0x04007824 RID: 30756
	public static float CONSUMPTION_RATE = 1f;

	// Token: 0x04007825 RID: 30757
	[Serialize]
	public Tag activeBaitSetting;

	// Token: 0x04007826 RID: 30758
	public List<Tag> baitTypes;

	// Token: 0x04007827 RID: 30759
	public Storage baitStorage;

	// Token: 0x04007828 RID: 30760
	protected FetchChore fetchChore;

	// Token: 0x04007829 RID: 30761
	private Operational operational;

	// Token: 0x0400782A RID: 30762
	private static readonly Operational.Flag baited = new Operational.Flag("Baited", Operational.Flag.Type.Requirement);

	// Token: 0x0400782B RID: 30763
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400782C RID: 30764
	private static readonly EventSystem.IntraObjectHandler<CreatureLure> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CreatureLure>(delegate(CreatureLure component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400782D RID: 30765
	private static readonly EventSystem.IntraObjectHandler<CreatureLure> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<CreatureLure>(delegate(CreatureLure component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001CDA RID: 7386
	public class StatesInstance : GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.GameInstance
	{
		// Token: 0x06009A04 RID: 39428 RVA: 0x00108966 File Offset: 0x00106B66
		public StatesInstance(CreatureLure master) : base(master)
		{
		}
	}

	// Token: 0x02001CDB RID: 7387
	public class States : GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure>
	{
		// Token: 0x06009A05 RID: 39429 RVA: 0x003C5F94 File Offset: 0x003C4194
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("off", KAnim.PlayMode.Loop).Enter(delegate(CreatureLure.StatesInstance smi)
			{
				if (smi.master.activeBaitSetting != Tag.Invalid)
				{
					if (smi.master.baitStorage.IsEmpty())
					{
						smi.master.CreateFetchChore();
						return;
					}
					if (smi.master.operational.IsOperational)
					{
						smi.GoTo(this.working);
					}
				}
			}).EventTransition(GameHashes.OnStorageChange, this.working, (CreatureLure.StatesInstance smi) => !smi.master.baitStorage.IsEmpty() && smi.master.activeBaitSetting != Tag.Invalid && smi.master.operational.IsOperational).EventTransition(GameHashes.OperationalChanged, this.working, (CreatureLure.StatesInstance smi) => !smi.master.baitStorage.IsEmpty() && smi.master.activeBaitSetting != Tag.Invalid && smi.master.operational.IsOperational);
			this.working.Enter(delegate(CreatureLure.StatesInstance smi)
			{
				smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.AwaitingBaitDelivery, false);
				HashedString batchTag = ElementLoader.FindElementByName(smi.master.activeBaitSetting.ToString()).substance.anim.batchTag;
				KAnim.Build build = ElementLoader.FindElementByName(smi.master.activeBaitSetting.ToString()).substance.anim.GetData().build;
				KAnim.Build.Symbol symbol = build.GetSymbol(new KAnimHashedString(build.name));
				HashedString target_symbol = "slime_mold";
				SymbolOverrideController component = smi.GetComponent<SymbolOverrideController>();
				component.TryRemoveSymbolOverride(target_symbol, 0);
				component.AddSymbolOverride(target_symbol, symbol, 0);
				smi.GetSMI<Lure.Instance>().SetActiveLures(new Tag[]
				{
					smi.master.activeBaitSetting
				});
			}).Exit(new StateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State.Callback(CreatureLure.States.ClearBait)).QueueAnim("working_pre", false, null).QueueAnim("working_loop", true, null).EventTransition(GameHashes.OnStorageChange, this.empty, (CreatureLure.StatesInstance smi) => smi.master.baitStorage.IsEmpty() && smi.master.activeBaitSetting != Tag.Invalid).EventTransition(GameHashes.OperationalChanged, this.idle, (CreatureLure.StatesInstance smi) => !smi.master.operational.IsOperational && !smi.master.baitStorage.IsEmpty());
			this.empty.QueueAnim("working_pst", false, null).QueueAnim("off", false, null).Enter(delegate(CreatureLure.StatesInstance smi)
			{
				smi.master.CreateFetchChore();
			}).EventTransition(GameHashes.OnStorageChange, this.working, (CreatureLure.StatesInstance smi) => !smi.master.baitStorage.IsEmpty() && smi.master.operational.IsOperational).EventTransition(GameHashes.OperationalChanged, this.working, (CreatureLure.StatesInstance smi) => !smi.master.baitStorage.IsEmpty() && smi.master.operational.IsOperational);
		}

		// Token: 0x06009A06 RID: 39430 RVA: 0x0010896F File Offset: 0x00106B6F
		private static void ClearBait(StateMachine.Instance smi)
		{
			if (smi.GetSMI<Lure.Instance>() != null)
			{
				smi.GetSMI<Lure.Instance>().SetActiveLures(null);
			}
		}

		// Token: 0x0400782E RID: 30766
		public GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State idle;

		// Token: 0x0400782F RID: 30767
		public GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State working;

		// Token: 0x04007830 RID: 30768
		public GameStateMachine<CreatureLure.States, CreatureLure.StatesInstance, CreatureLure, object>.State empty;
	}
}
