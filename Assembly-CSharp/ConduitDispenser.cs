using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001115 RID: 4373
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ConduitDispenser")]
public class ConduitDispenser : KMonoBehaviour, ISaveLoadable, IConduitDispenser
{
	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x0600596A RID: 22890 RVA: 0x000DEB30 File Offset: 0x000DCD30
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x1700055C RID: 1372
	// (get) Token: 0x0600596B RID: 22891 RVA: 0x000DEB38 File Offset: 0x000DCD38
	public ConduitType ConduitType
	{
		get
		{
			return this.conduitType;
		}
	}

	// Token: 0x1700055D RID: 1373
	// (get) Token: 0x0600596C RID: 22892 RVA: 0x000DEB40 File Offset: 0x000DCD40
	public ConduitFlow.ConduitContents ConduitContents
	{
		get
		{
			return this.GetConduitManager().GetContents(this.utilityCell);
		}
	}

	// Token: 0x0600596D RID: 22893 RVA: 0x000DEB53 File Offset: 0x000DCD53
	public void SetConduitData(ConduitType type)
	{
		this.conduitType = type;
	}

	// Token: 0x0600596E RID: 22894 RVA: 0x0029D7C8 File Offset: 0x0029B9C8
	public ConduitFlow GetConduitManager()
	{
		ConduitType conduitType = this.conduitType;
		if (conduitType == ConduitType.Gas)
		{
			return Game.Instance.gasConduitFlow;
		}
		if (conduitType != ConduitType.Liquid)
		{
			return null;
		}
		return Game.Instance.liquidConduitFlow;
	}

	// Token: 0x0600596F RID: 22895 RVA: 0x000DEB5C File Offset: 0x000DCD5C
	private void OnConduitConnectionChanged(object data)
	{
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x06005970 RID: 22896 RVA: 0x0029D800 File Offset: 0x0029BA00
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("PlumbingTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Plumbing, true);
		}, null, null);
		ConduitFlow conduitManager = this.GetConduitManager();
		this.utilityCell = this.GetOutputCell(conduitManager.conduitType);
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(this.conduitType == ConduitType.Gas) ? 12 : 16];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Dispense);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x06005971 RID: 22897 RVA: 0x000DEB74 File Offset: 0x000DCD74
	protected override void OnCleanUp()
	{
		this.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06005972 RID: 22898 RVA: 0x000DEBA3 File Offset: 0x000DCDA3
	public void SetOnState(bool onState)
	{
		this.isOn = onState;
	}

	// Token: 0x06005973 RID: 22899 RVA: 0x000DEBAC File Offset: 0x000DCDAC
	private void ConduitUpdate(float dt)
	{
		if (this.operational != null)
		{
			this.operational.SetFlag(ConduitDispenser.outputConduitFlag, this.IsConnected);
		}
		this.blocked = false;
		if (this.isOn)
		{
			this.Dispense(dt);
		}
	}

	// Token: 0x06005974 RID: 22900 RVA: 0x0029D8CC File Offset: 0x0029BACC
	private void Dispense(float dt)
	{
		if ((this.operational != null && this.operational.IsOperational) || this.alwaysDispense)
		{
			if (this.building != null && this.building.Def.CanMove)
			{
				this.utilityCell = this.GetOutputCell(this.GetConduitManager().conduitType);
			}
			PrimaryElement primaryElement = this.FindSuitableElement();
			if (primaryElement != null)
			{
				primaryElement.KeepZeroMassObject = true;
				this.empty = false;
				float num = this.GetConduitManager().AddElement(this.utilityCell, primaryElement.ElementID, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount);
				if (num > 0f)
				{
					int num2 = (int)(num / primaryElement.Mass * (float)primaryElement.DiseaseCount);
					primaryElement.ModifyDiseaseCount(-num2, "ConduitDispenser.ConduitUpdate");
					primaryElement.Mass -= num;
					this.storage.Trigger(-1697596308, primaryElement.gameObject);
					return;
				}
				this.blocked = true;
				return;
			}
			else
			{
				this.empty = true;
			}
		}
	}

	// Token: 0x06005975 RID: 22901 RVA: 0x0029D9E4 File Offset: 0x0029BBE4
	private PrimaryElement FindSuitableElement()
	{
		List<GameObject> items = this.storage.items;
		int count = items.Count;
		for (int i = 0; i < count; i++)
		{
			int index = (i + this.elementOutputOffset) % count;
			PrimaryElement component = items[index].GetComponent<PrimaryElement>();
			if (component != null && component.Mass > 0f && ((this.conduitType == ConduitType.Liquid) ? component.Element.IsLiquid : component.Element.IsGas) && (this.elementFilter == null || this.elementFilter.Length == 0 || (!this.invertElementFilter && this.IsFilteredElement(component.ElementID)) || (this.invertElementFilter && !this.IsFilteredElement(component.ElementID))))
			{
				this.elementOutputOffset = (this.elementOutputOffset + 1) % count;
				return component;
			}
		}
		return null;
	}

	// Token: 0x06005976 RID: 22902 RVA: 0x0029DAC4 File Offset: 0x0029BCC4
	private bool IsFilteredElement(SimHashes element)
	{
		for (int num = 0; num != this.elementFilter.Length; num++)
		{
			if (this.elementFilter[num] == element)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x1700055E RID: 1374
	// (get) Token: 0x06005977 RID: 22903 RVA: 0x0029DAF4 File Offset: 0x0029BCF4
	public bool IsConnected
	{
		get
		{
			GameObject gameObject = Grid.Objects[this.utilityCell, (this.conduitType == ConduitType.Gas) ? 12 : 16];
			return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
		}
	}

	// Token: 0x06005978 RID: 22904 RVA: 0x0029DB38 File Offset: 0x0029BD38
	private int GetOutputCell(ConduitType outputConduitType)
	{
		Building component = base.GetComponent<Building>();
		if (!(component != null))
		{
			return Grid.OffsetCell(Grid.PosToCell(this), this.noBuildingOutputCellOffset);
		}
		if (this.useSecondaryOutput)
		{
			ISecondaryOutput[] components = base.GetComponents<ISecondaryOutput>();
			foreach (ISecondaryOutput secondaryOutput in components)
			{
				if (secondaryOutput.HasSecondaryConduitType(outputConduitType))
				{
					return Grid.OffsetCell(component.NaturalBuildingCell(), secondaryOutput.GetSecondaryConduitOffset(outputConduitType));
				}
			}
			return Grid.OffsetCell(component.NaturalBuildingCell(), components[0].GetSecondaryConduitOffset(outputConduitType));
		}
		return component.GetUtilityOutputCell();
	}

	// Token: 0x04003F9B RID: 16283
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x04003F9C RID: 16284
	[SerializeField]
	public SimHashes[] elementFilter;

	// Token: 0x04003F9D RID: 16285
	[SerializeField]
	public bool invertElementFilter;

	// Token: 0x04003F9E RID: 16286
	[SerializeField]
	public bool alwaysDispense;

	// Token: 0x04003F9F RID: 16287
	[SerializeField]
	public bool isOn = true;

	// Token: 0x04003FA0 RID: 16288
	[SerializeField]
	public bool blocked;

	// Token: 0x04003FA1 RID: 16289
	[SerializeField]
	public bool empty = true;

	// Token: 0x04003FA2 RID: 16290
	[SerializeField]
	public bool useSecondaryOutput;

	// Token: 0x04003FA3 RID: 16291
	[SerializeField]
	public CellOffset noBuildingOutputCellOffset;

	// Token: 0x04003FA4 RID: 16292
	private static readonly Operational.Flag outputConduitFlag = new Operational.Flag("output_conduit", Operational.Flag.Type.Functional);

	// Token: 0x04003FA5 RID: 16293
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04003FA6 RID: 16294
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04003FA7 RID: 16295
	[MyCmpGet]
	private Building building;

	// Token: 0x04003FA8 RID: 16296
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003FA9 RID: 16297
	private int utilityCell = -1;

	// Token: 0x04003FAA RID: 16298
	private int elementOutputOffset;
}
