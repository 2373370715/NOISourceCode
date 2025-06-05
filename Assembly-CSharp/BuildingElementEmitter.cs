using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000CAC RID: 3244
[AddComponentMenu("KMonoBehaviour/scripts/BuildingElementEmitter")]
public class BuildingElementEmitter : KMonoBehaviour, IGameObjectEffectDescriptor, IElementEmitter, ISim200ms
{
	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x06003DB2 RID: 15794 RVA: 0x000CC61E File Offset: 0x000CA81E
	public float AverageEmitRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x06003DB3 RID: 15795 RVA: 0x000CC635 File Offset: 0x000CA835
	public float EmitRate
	{
		get
		{
			return this.emitRate;
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x06003DB4 RID: 15796 RVA: 0x000CC63D File Offset: 0x000CA83D
	public SimHashes Element
	{
		get
		{
			return this.element;
		}
	}

	// Token: 0x06003DB5 RID: 15797 RVA: 0x000CC645 File Offset: 0x000CA845
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		base.Subscribe<BuildingElementEmitter>(824508782, BuildingElementEmitter.OnActiveChangedDelegate);
		this.SimRegister();
	}

	// Token: 0x06003DB6 RID: 15798 RVA: 0x000CC67F File Offset: 0x000CA87F
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.accumulator);
		this.SimUnregister();
		base.OnCleanUp();
	}

	// Token: 0x06003DB7 RID: 15799 RVA: 0x000CC6A3 File Offset: 0x000CA8A3
	private void OnActiveChanged(object data)
	{
		this.simActive = ((Operational)data).IsActive;
		this.dirty = true;
	}

	// Token: 0x06003DB8 RID: 15800 RVA: 0x000CC6BD File Offset: 0x000CA8BD
	public void Sim200ms(float dt)
	{
		this.UnsafeUpdate(dt);
	}

	// Token: 0x06003DB9 RID: 15801 RVA: 0x00240738 File Offset: 0x0023E938
	private unsafe void UnsafeUpdate(float dt)
	{
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		this.UpdateSimState();
		int handleIndex = Sim.GetHandleIndex(this.simHandle);
		Sim.EmittedMassInfo emittedMassInfo = Game.Instance.simData.emittedMassEntries[handleIndex];
		if (emittedMassInfo.mass > 0f)
		{
			Game.Instance.accumulators.Accumulate(this.accumulator, emittedMassInfo.mass);
			if (this.element == SimHashes.Oxygen)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, emittedMassInfo.mass, base.gameObject.GetProperName(), null);
			}
		}
	}

	// Token: 0x06003DBA RID: 15802 RVA: 0x002407D8 File Offset: 0x0023E9D8
	private void UpdateSimState()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		if (this.simActive)
		{
			if (this.element != (SimHashes)0 && this.emitRate > 0f)
			{
				int game_cell = Grid.PosToCell(new Vector3(base.transform.GetPosition().x + this.modifierOffset.x, base.transform.GetPosition().y + this.modifierOffset.y, 0f));
				SimMessages.ModifyElementEmitter(this.simHandle, game_cell, (int)this.emitRange, this.element, 0.2f, this.emitRate * 0.2f, this.temperature, float.MaxValue, this.emitDiseaseIdx, this.emitDiseaseCount);
			}
			this.statusHandle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.EmittingElement, this);
			return;
		}
		SimMessages.ModifyElementEmitter(this.simHandle, 0, 0, SimHashes.Vacuum, 0f, 0f, 0f, 0f, byte.MaxValue, 0);
		this.statusHandle = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, this);
	}

	// Token: 0x06003DBB RID: 15803 RVA: 0x00240910 File Offset: 0x0023EB10
	private void SimRegister()
	{
		if (base.isSpawned && this.simHandle == -1)
		{
			this.simHandle = -2;
			SimMessages.AddElementEmitter(float.MaxValue, Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(BuildingElementEmitter.OnSimRegisteredCallback), this, "BuildingElementEmitter").index, -1, -1);
		}
	}

	// Token: 0x06003DBC RID: 15804 RVA: 0x000CC6C6 File Offset: 0x000CA8C6
	private void SimUnregister()
	{
		if (this.simHandle != -1)
		{
			if (Sim.IsValidHandle(this.simHandle))
			{
				SimMessages.RemoveElementEmitter(-1, this.simHandle);
			}
			this.simHandle = -1;
		}
	}

	// Token: 0x06003DBD RID: 15805 RVA: 0x000CC6F1 File Offset: 0x000CA8F1
	private static void OnSimRegisteredCallback(int handle, object data)
	{
		((BuildingElementEmitter)data).OnSimRegistered(handle);
	}

	// Token: 0x06003DBE RID: 15806 RVA: 0x000CC6FF File Offset: 0x000CA8FF
	private void OnSimRegistered(int handle)
	{
		if (this != null)
		{
			this.simHandle = handle;
			return;
		}
		SimMessages.RemoveElementEmitter(-1, handle);
	}

	// Token: 0x06003DBF RID: 15807 RVA: 0x0024096C File Offset: 0x0023EB6C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(this.element).tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_FIXEDTEMP, arg, GameUtil.GetFormattedMass(this.EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_FIXEDTEMP, arg, GameUtil.GetFormattedMass(this.EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x04002A91 RID: 10897
	[SerializeField]
	public float emitRate = 0.3f;

	// Token: 0x04002A92 RID: 10898
	[SerializeField]
	[Serialize]
	public float temperature = 293f;

	// Token: 0x04002A93 RID: 10899
	[SerializeField]
	[HashedEnum]
	public SimHashes element = SimHashes.Oxygen;

	// Token: 0x04002A94 RID: 10900
	[SerializeField]
	public Vector2 modifierOffset;

	// Token: 0x04002A95 RID: 10901
	[SerializeField]
	public byte emitRange = 1;

	// Token: 0x04002A96 RID: 10902
	[SerializeField]
	public byte emitDiseaseIdx = byte.MaxValue;

	// Token: 0x04002A97 RID: 10903
	[SerializeField]
	public int emitDiseaseCount;

	// Token: 0x04002A98 RID: 10904
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04002A99 RID: 10905
	private int simHandle = -1;

	// Token: 0x04002A9A RID: 10906
	private bool simActive;

	// Token: 0x04002A9B RID: 10907
	private bool dirty = true;

	// Token: 0x04002A9C RID: 10908
	private Guid statusHandle;

	// Token: 0x04002A9D RID: 10909
	private static readonly EventSystem.IntraObjectHandler<BuildingElementEmitter> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<BuildingElementEmitter>(delegate(BuildingElementEmitter component, object data)
	{
		component.OnActiveChanged(data);
	});
}
