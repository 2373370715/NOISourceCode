using System;
using UnityEngine;

// Token: 0x02000CC9 RID: 3273
public class AlgaeHabitat : StateMachineComponent<AlgaeHabitat.SMInstance>
{
	// Token: 0x06003E6F RID: 15983 RVA: 0x00242A94 File Offset: 0x00240C94
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("WaterFetchingTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_FetchingWater, true);
		}, null, null);
		this.ConfigurePollutedWaterOutput();
		Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
	}

	// Token: 0x06003E70 RID: 15984 RVA: 0x000CCF71 File Offset: 0x000CB171
	protected override void OnCleanUp()
	{
		Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06003E71 RID: 15985 RVA: 0x00242B04 File Offset: 0x00240D04
	private void ConfigurePollutedWaterOutput()
	{
		Storage storage = null;
		Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
		foreach (Storage storage2 in base.GetComponents<Storage>())
		{
			if (storage2.storageFilters.Contains(tag))
			{
				storage = storage2;
				break;
			}
		}
		foreach (ElementConverter elementConverter in base.GetComponents<ElementConverter>())
		{
			ElementConverter.OutputElement[] outputElements = elementConverter.outputElements;
			for (int j = 0; j < outputElements.Length; j++)
			{
				if (outputElements[j].elementHash == SimHashes.DirtyWater)
				{
					elementConverter.SetStorage(storage);
					break;
				}
			}
		}
		this.pollutedWaterStorage = storage;
	}

	// Token: 0x04002B23 RID: 11043
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002B24 RID: 11044
	private Storage pollutedWaterStorage;

	// Token: 0x04002B25 RID: 11045
	[SerializeField]
	public float lightBonusMultiplier = 1.1f;

	// Token: 0x04002B26 RID: 11046
	public CellOffset pressureSampleOffset = CellOffset.none;

	// Token: 0x02000CCA RID: 3274
	public class SMInstance : GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.GameInstance
	{
		// Token: 0x06003E73 RID: 15987 RVA: 0x000CCFAD File Offset: 0x000CB1AD
		public SMInstance(AlgaeHabitat master) : base(master)
		{
			this.converter = master.GetComponent<ElementConverter>();
		}

		// Token: 0x06003E74 RID: 15988 RVA: 0x000CCFC2 File Offset: 0x000CB1C2
		public bool HasEnoughMass(Tag tag)
		{
			return this.converter.HasEnoughMass(tag, false);
		}

		// Token: 0x06003E75 RID: 15989 RVA: 0x000CCFD1 File Offset: 0x000CB1D1
		public bool NeedsEmptying()
		{
			return base.smi.master.pollutedWaterStorage.RemainingCapacity() <= 0f;
		}

		// Token: 0x06003E76 RID: 15990 RVA: 0x00242BB0 File Offset: 0x00240DB0
		public void CreateEmptyChore()
		{
			if (this.emptyChore != null)
			{
				this.emptyChore.Cancel("dupe");
			}
			AlgaeHabitatEmpty component = base.master.GetComponent<AlgaeHabitatEmpty>();
			this.emptyChore = new WorkChore<AlgaeHabitatEmpty>(Db.Get().ChoreTypes.EmptyStorage, component, null, true, new Action<Chore>(this.OnEmptyComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			this.emptyChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x000CCFF2 File Offset: 0x000CB1F2
		public void CancelEmptyChore()
		{
			if (this.emptyChore != null)
			{
				this.emptyChore.Cancel("Cancelled");
				this.emptyChore = null;
			}
		}

		// Token: 0x06003E78 RID: 15992 RVA: 0x00242C30 File Offset: 0x00240E30
		private void OnEmptyComplete(Chore chore)
		{
			this.emptyChore = null;
			base.master.pollutedWaterStorage.DropAll(true, false, default(Vector3), true, null);
		}

		// Token: 0x04002B27 RID: 11047
		public ElementConverter converter;

		// Token: 0x04002B28 RID: 11048
		public Chore emptyChore;
	}

	// Token: 0x02000CCB RID: 3275
	public class States : GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat>
	{
		// Token: 0x06003E79 RID: 15993 RVA: 0x00242C64 File Offset: 0x00240E64
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.noAlgae;
			this.root.EventTransition(GameHashes.OperationalChanged, this.notoperational, (AlgaeHabitat.SMInstance smi) => !smi.master.operational.IsOperational).EventTransition(GameHashes.OperationalChanged, this.noAlgae, (AlgaeHabitat.SMInstance smi) => smi.master.operational.IsOperational);
			this.notoperational.QueueAnim("off", false, null);
			this.gotAlgae.PlayAnim("on_pre").OnAnimQueueComplete(this.noWater);
			this.gotEmptied.PlayAnim("on_pre").OnAnimQueueComplete(this.generatingOxygen);
			this.lostAlgae.PlayAnim("on_pst").OnAnimQueueComplete(this.noAlgae);
			this.noAlgae.QueueAnim("off", false, null).EventTransition(GameHashes.OnStorageChange, this.gotAlgae, (AlgaeHabitat.SMInstance smi) => smi.HasEnoughMass(GameTags.Algae)).Enter(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.noWater.QueueAnim("on", false, null).Enter(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.master.GetComponent<PassiveElementConsumer>().EnableConsumption(true);
			}).EventTransition(GameHashes.OnStorageChange, this.lostAlgae, (AlgaeHabitat.SMInstance smi) => !smi.HasEnoughMass(GameTags.Algae)).EventTransition(GameHashes.OnStorageChange, this.gotWater, (AlgaeHabitat.SMInstance smi) => smi.HasEnoughMass(GameTags.Algae) && smi.HasEnoughMass(GameTags.Water));
			this.needsEmptying.QueueAnim("off", false, null).Enter(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.CreateEmptyChore();
			}).Exit(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.CancelEmptyChore();
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.HabitatNeedsEmptying, null).EventTransition(GameHashes.OnStorageChange, this.noAlgae, (AlgaeHabitat.SMInstance smi) => !smi.HasEnoughMass(GameTags.Algae) || !smi.HasEnoughMass(GameTags.Water)).EventTransition(GameHashes.OnStorageChange, this.gotEmptied, (AlgaeHabitat.SMInstance smi) => smi.HasEnoughMass(GameTags.Algae) && smi.HasEnoughMass(GameTags.Water) && !smi.NeedsEmptying());
			this.gotWater.PlayAnim("working_pre").OnAnimQueueComplete(this.needsEmptying);
			this.generatingOxygen.Enter(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Update("GeneratingOxygen", delegate(AlgaeHabitat.SMInstance smi, float dt)
			{
				int num = Grid.PosToCell(smi.master.transform.GetPosition());
				smi.converter.OutputMultiplier = ((Grid.LightCount[num] > 0) ? smi.master.lightBonusMultiplier : 1f);
			}, UpdateRate.SIM_200ms, false).QueueAnim("working_loop", true, null).EventTransition(GameHashes.OnStorageChange, this.stoppedGeneratingOxygen, (AlgaeHabitat.SMInstance smi) => !smi.HasEnoughMass(GameTags.Water) || !smi.HasEnoughMass(GameTags.Algae) || smi.NeedsEmptying());
			this.stoppedGeneratingOxygen.PlayAnim("working_pst").OnAnimQueueComplete(this.stoppedGeneratingOxygenTransition);
			this.stoppedGeneratingOxygenTransition.EventTransition(GameHashes.OnStorageChange, this.needsEmptying, (AlgaeHabitat.SMInstance smi) => smi.NeedsEmptying()).EventTransition(GameHashes.OnStorageChange, this.noWater, (AlgaeHabitat.SMInstance smi) => !smi.HasEnoughMass(GameTags.Water)).EventTransition(GameHashes.OnStorageChange, this.lostAlgae, (AlgaeHabitat.SMInstance smi) => !smi.HasEnoughMass(GameTags.Algae)).EventTransition(GameHashes.OnStorageChange, this.gotWater, (AlgaeHabitat.SMInstance smi) => smi.HasEnoughMass(GameTags.Water) && smi.HasEnoughMass(GameTags.Algae));
		}

		// Token: 0x04002B29 RID: 11049
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State generatingOxygen;

		// Token: 0x04002B2A RID: 11050
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State stoppedGeneratingOxygen;

		// Token: 0x04002B2B RID: 11051
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State stoppedGeneratingOxygenTransition;

		// Token: 0x04002B2C RID: 11052
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State noWater;

		// Token: 0x04002B2D RID: 11053
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State noAlgae;

		// Token: 0x04002B2E RID: 11054
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State needsEmptying;

		// Token: 0x04002B2F RID: 11055
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State gotAlgae;

		// Token: 0x04002B30 RID: 11056
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State gotWater;

		// Token: 0x04002B31 RID: 11057
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State gotEmptied;

		// Token: 0x04002B32 RID: 11058
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State lostAlgae;

		// Token: 0x04002B33 RID: 11059
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State notoperational;
	}
}
