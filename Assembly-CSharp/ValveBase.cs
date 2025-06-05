using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001066 RID: 4198
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ValveBase")]
public class ValveBase : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x06005551 RID: 21841 RVA: 0x000DBFD0 File Offset: 0x000DA1D0
	// (set) Token: 0x06005550 RID: 21840 RVA: 0x000DBFC7 File Offset: 0x000DA1C7
	public float CurrentFlow
	{
		get
		{
			return this.currentFlow;
		}
		set
		{
			this.currentFlow = value;
		}
	}

	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x06005552 RID: 21842 RVA: 0x000DBFD8 File Offset: 0x000DA1D8
	public HandleVector<int>.Handle AccumulatorHandle
	{
		get
		{
			return this.flowAccumulator;
		}
	}

	// Token: 0x170004EC RID: 1260
	// (get) Token: 0x06005553 RID: 21843 RVA: 0x000DBFE0 File Offset: 0x000DA1E0
	public float MaxFlow
	{
		get
		{
			return this.maxFlow;
		}
	}

	// Token: 0x06005554 RID: 21844 RVA: 0x000DBFE8 File Offset: 0x000DA1E8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.flowAccumulator = Game.Instance.accumulators.Add("Flow", this);
	}

	// Token: 0x06005555 RID: 21845 RVA: 0x0028C398 File Offset: 0x0028A598
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		Conduit.GetFlowManager(this.conduitType).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.UpdateAnim();
		this.OnCmpEnable();
	}

	// Token: 0x06005556 RID: 21846 RVA: 0x000DC00B File Offset: 0x000DA20B
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.flowAccumulator);
		Conduit.GetFlowManager(this.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x06005557 RID: 21847 RVA: 0x0028C3F4 File Offset: 0x0028A5F4
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.conduitType);
		ConduitFlow.Conduit conduit = flowManager.GetConduit(this.inputCell);
		if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
		{
			this.OnMassTransfer(0f);
			this.UpdateAnim();
			return;
		}
		ConduitFlow.ConduitContents contents = conduit.GetContents(flowManager);
		float num = Mathf.Min(contents.mass, this.currentFlow * dt);
		float num2 = 0f;
		if (num > 0f)
		{
			int disease_count = (int)(num / contents.mass * (float)contents.diseaseCount);
			num2 = flowManager.AddElement(this.outputCell, contents.element, num, contents.temperature, contents.diseaseIdx, disease_count);
			Game.Instance.accumulators.Accumulate(this.flowAccumulator, num2);
			if (num2 > 0f)
			{
				flowManager.RemoveElement(this.inputCell, num2);
			}
		}
		this.OnMassTransfer(num2);
		this.UpdateAnim();
	}

	// Token: 0x06005558 RID: 21848 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnMassTransfer(float amount)
	{
	}

	// Token: 0x06005559 RID: 21849 RVA: 0x0028C4EC File Offset: 0x0028A6EC
	public virtual void UpdateAnim()
	{
		float averageRate = Game.Instance.accumulators.GetAverageRate(this.flowAccumulator);
		if (averageRate > 0f)
		{
			int i = 0;
			while (i < this.animFlowRanges.Length)
			{
				if (averageRate <= this.animFlowRanges[i].minFlow)
				{
					if (this.curFlowIdx != i)
					{
						this.curFlowIdx = i;
						this.controller.Play(this.animFlowRanges[i].animName, (averageRate <= 0f) ? KAnim.PlayMode.Once : KAnim.PlayMode.Loop, 1f, 0f);
						return;
					}
					return;
				}
				else
				{
					i++;
				}
			}
			return;
		}
		this.controller.Play("off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04003C48 RID: 15432
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x04003C49 RID: 15433
	[SerializeField]
	public float maxFlow = 0.5f;

	// Token: 0x04003C4A RID: 15434
	[Serialize]
	private float currentFlow;

	// Token: 0x04003C4B RID: 15435
	[MyCmpGet]
	protected KBatchedAnimController controller;

	// Token: 0x04003C4C RID: 15436
	protected HandleVector<int>.Handle flowAccumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04003C4D RID: 15437
	private int curFlowIdx = -1;

	// Token: 0x04003C4E RID: 15438
	private int inputCell;

	// Token: 0x04003C4F RID: 15439
	private int outputCell;

	// Token: 0x04003C50 RID: 15440
	[SerializeField]
	public ValveBase.AnimRangeInfo[] animFlowRanges;

	// Token: 0x02001067 RID: 4199
	[Serializable]
	public struct AnimRangeInfo
	{
		// Token: 0x0600555B RID: 21851 RVA: 0x000DC06A File Offset: 0x000DA26A
		public AnimRangeInfo(float min_flow, string anim_name)
		{
			this.minFlow = min_flow;
			this.animName = anim_name;
		}

		// Token: 0x04003C51 RID: 15441
		public float minFlow;

		// Token: 0x04003C52 RID: 15442
		public string animName;
	}
}
