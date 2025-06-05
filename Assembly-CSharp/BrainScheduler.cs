using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200064F RID: 1615
[AddComponentMenu("KMonoBehaviour/scripts/BrainScheduler")]
public class BrainScheduler : KMonoBehaviour, IRenderEveryTick, ICPULoad
{
	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06001CAC RID: 7340 RVA: 0x000B75C5 File Offset: 0x000B57C5
	private bool isAsyncPathProbeEnabled
	{
		get
		{
			return !TuningData<BrainScheduler.Tuning>.Get().disableAsyncPathProbes;
		}
	}

	// Token: 0x06001CAD RID: 7341 RVA: 0x000B75D4 File Offset: 0x000B57D4
	public List<BrainScheduler.BrainGroup> debugGetBrainGroups()
	{
		return this.brainGroups;
	}

	// Token: 0x06001CAE RID: 7342 RVA: 0x001B8D2C File Offset: 0x001B6F2C
	protected override void OnPrefabInit()
	{
		this.brainGroups.Add(new BrainScheduler.DupeBrainGroup());
		this.brainGroups.Add(new BrainScheduler.CreatureBrainGroup());
		Components.Brains.Register(new Action<Brain>(this.OnAddBrain), new Action<Brain>(this.OnRemoveBrain));
		CPUBudget.AddRoot(this);
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			CPUBudget.AddChild(this, brainGroup, brainGroup.LoadBalanceThreshold());
		}
		CPUBudget.FinalizeChildren(this);
	}

	// Token: 0x06001CAF RID: 7343 RVA: 0x001B8DD4 File Offset: 0x001B6FD4
	private void OnAddBrain(Brain brain)
	{
		bool test = false;
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			if (brain.HasTag(brainGroup.tag))
			{
				brainGroup.AddBrain(brain);
				test = true;
			}
			Navigator component = brain.GetComponent<Navigator>();
			if (component != null)
			{
				component.executePathProbeTaskAsync = this.isAsyncPathProbeEnabled;
			}
		}
		DebugUtil.Assert(test);
	}

	// Token: 0x06001CB0 RID: 7344 RVA: 0x001B8E5C File Offset: 0x001B705C
	private void OnRemoveBrain(Brain brain)
	{
		bool test = false;
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			if (brain.HasTag(brainGroup.tag))
			{
				test = true;
				brainGroup.RemoveBrain(brain);
			}
			Navigator component = brain.GetComponent<Navigator>();
			if (component != null)
			{
				component.executePathProbeTaskAsync = false;
			}
		}
		DebugUtil.Assert(test);
	}

	// Token: 0x06001CB1 RID: 7345 RVA: 0x001B8EE0 File Offset: 0x001B70E0
	public void PrioritizeBrain(Brain brain)
	{
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			if (brain.HasTag(brainGroup.tag))
			{
				brainGroup.PrioritizeBrain(brain);
			}
		}
	}

	// Token: 0x06001CB2 RID: 7346 RVA: 0x000B75DC File Offset: 0x000B57DC
	public float GetEstimatedFrameTime()
	{
		return TuningData<BrainScheduler.Tuning>.Get().frameTime;
	}

	// Token: 0x06001CB3 RID: 7347 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool AdjustLoad(float currentFrameTime, float frameTimeDelta)
	{
		return false;
	}

	// Token: 0x06001CB4 RID: 7348 RVA: 0x001B8F44 File Offset: 0x001B7144
	public void RenderEveryTick(float dt)
	{
		if (Game.IsQuitting() || KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			brainGroup.RenderEveryTick(dt);
		}
	}

	// Token: 0x06001CB5 RID: 7349 RVA: 0x000B75E8 File Offset: 0x000B57E8
	protected override void OnForcedCleanUp()
	{
		CPUBudget.Remove(this);
		base.OnForcedCleanUp();
	}

	// Token: 0x0400122C RID: 4652
	public const float millisecondsPerFrame = 33.33333f;

	// Token: 0x0400122D RID: 4653
	public const float secondsPerFrame = 0.033333328f;

	// Token: 0x0400122E RID: 4654
	public const float framesPerSecond = 30.000006f;

	// Token: 0x0400122F RID: 4655
	private List<BrainScheduler.BrainGroup> brainGroups = new List<BrainScheduler.BrainGroup>();

	// Token: 0x02000650 RID: 1616
	private class Tuning : TuningData<BrainScheduler.Tuning>
	{
		// Token: 0x04001230 RID: 4656
		public bool disableAsyncPathProbes;

		// Token: 0x04001231 RID: 4657
		public float frameTime = 5f;
	}

	// Token: 0x02000651 RID: 1617
	public abstract class BrainGroup : ICPULoad
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06001CB8 RID: 7352 RVA: 0x000B761C File Offset: 0x000B581C
		// (set) Token: 0x06001CB9 RID: 7353 RVA: 0x000B7624 File Offset: 0x000B5824
		public Tag tag { get; private set; }

		// Token: 0x06001CBA RID: 7354 RVA: 0x001B8FA4 File Offset: 0x001B71A4
		protected BrainGroup(Tag tag)
		{
			this.tag = tag;
			this.probeSize = this.InitialProbeSize();
			this.probeCount = this.InitialProbeCount();
			string str = tag.ToString();
			this.increaseLoadLabel = "IncLoad" + str;
			this.decreaseLoadLabel = "DecLoad" + str;
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x000B762D File Offset: 0x000B582D
		public void AddBrain(Brain brain)
		{
			this.brains.Add(brain);
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x001B9028 File Offset: 0x001B7228
		public void RemoveBrain(Brain brain)
		{
			int num = this.brains.IndexOf(brain);
			if (num != -1)
			{
				this.brains.RemoveAt(num);
				this.OnRemoveBrain(num, ref this.nextUpdateBrain);
				this.OnRemoveBrain(num, ref this.nextPathProbeBrain);
			}
			if (this.priorityBrains.Contains(brain))
			{
				List<Brain> list = new List<Brain>(this.priorityBrains);
				list.Remove(brain);
				this.priorityBrains = new Queue<Brain>(list);
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06001CBD RID: 7357 RVA: 0x000B763B File Offset: 0x000B583B
		public int BrainCount
		{
			get
			{
				return this.brains.Count;
			}
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x000B7648 File Offset: 0x000B5848
		public void PrioritizeBrain(Brain brain)
		{
			if (!this.priorityBrains.Contains(brain))
			{
				this.priorityBrains.Enqueue(brain);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06001CBF RID: 7359 RVA: 0x000B7664 File Offset: 0x000B5864
		// (set) Token: 0x06001CC0 RID: 7360 RVA: 0x000B766C File Offset: 0x000B586C
		public int probeSize { get; private set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06001CC1 RID: 7361 RVA: 0x000B7675 File Offset: 0x000B5875
		// (set) Token: 0x06001CC2 RID: 7362 RVA: 0x000B767D File Offset: 0x000B587D
		public int probeCount { get; private set; }

		// Token: 0x06001CC3 RID: 7363 RVA: 0x001B909C File Offset: 0x001B729C
		public bool AdjustLoad(float currentFrameTime, float frameTimeDelta)
		{
			if (this.debugFreezeLoadAdustment)
			{
				return false;
			}
			bool flag = frameTimeDelta > 0f;
			int num = 0;
			int num2 = Math.Max(this.probeCount, Math.Min(this.brains.Count, CPUBudget.coreCount));
			num += num2 - this.probeCount;
			this.probeCount = num2;
			float num3 = Math.Min(1f, (float)this.probeCount / (float)CPUBudget.coreCount);
			float num4 = num3 * (float)this.probeSize;
			float num5 = num3 * (float)this.probeSize;
			float num6 = currentFrameTime / num5;
			float num7 = frameTimeDelta / num6;
			if (num == 0)
			{
				float num8 = num4 + num7 / (float)CPUBudget.coreCount;
				int num9 = MathUtil.Clamp(this.MinProbeSize(), this.IdealProbeSize(), (int)(num8 / num3));
				num += num9 - this.probeSize;
				this.probeSize = num9;
			}
			if (num == 0)
			{
				int num10 = Math.Max(1, (int)num3 + (flag ? 1 : -1));
				int probeSize = MathUtil.Clamp(this.MinProbeSize(), this.IdealProbeSize(), (int)((num5 + num7) / (float)num10));
				int num11 = Math.Min(this.brains.Count, num10 * CPUBudget.coreCount);
				num += num11 - this.probeCount;
				this.probeCount = num11;
				this.probeSize = probeSize;
			}
			if (num == 0 && flag)
			{
				int num12 = this.probeSize + this.ProbeSizeStep();
				num += num12 - this.probeSize;
				this.probeSize = num12;
			}
			if (num >= 0 && num <= 0 && this.brains.Count > 0)
			{
				global::Debug.LogWarning("AdjustLoad() failed");
			}
			return num != 0;
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x000B7686 File Offset: 0x000B5886
		public void ResetLoad()
		{
			this.probeSize = this.InitialProbeSize();
			this.probeCount = this.InitialProbeCount();
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000B76A0 File Offset: 0x000B58A0
		private void IncrementBrainIndex(ref int brainIndex)
		{
			brainIndex++;
			if (brainIndex == this.brains.Count)
			{
				brainIndex = 0;
			}
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x000B76BA File Offset: 0x000B58BA
		private void ClampBrainIndex(ref int brainIndex)
		{
			brainIndex = MathUtil.Clamp(0, this.brains.Count - 1, brainIndex);
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x000B76D3 File Offset: 0x000B58D3
		private void OnRemoveBrain(int removedIndex, ref int brainIndex)
		{
			if (removedIndex < brainIndex)
			{
				brainIndex--;
				return;
			}
			if (brainIndex == this.brains.Count)
			{
				brainIndex = 0;
			}
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x001B9220 File Offset: 0x001B7420
		private void AsyncPathProbe()
		{
			this.pathProbeJob.Reset(null);
			for (int num = 0; num != this.brains.Count; num++)
			{
				this.ClampBrainIndex(ref this.nextPathProbeBrain);
				Brain brain = this.brains[this.nextPathProbeBrain];
				if (brain.IsRunning())
				{
					Navigator component = brain.GetComponent<Navigator>();
					if (component != null)
					{
						component.executePathProbeTaskAsync = true;
						component.PathProber.potentialCellsPerUpdate = this.probeSize;
						component.pathProbeTask.Update();
						this.pathProbeJob.Add(component.pathProbeTask);
						if (this.pathProbeJob.Count == this.probeCount)
						{
							break;
						}
					}
				}
				this.IncrementBrainIndex(ref this.nextPathProbeBrain);
			}
			CPUBudget.Start(this);
			GlobalJobManager.Run(this.pathProbeJob);
			CPUBudget.End(this);
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x001B92F8 File Offset: 0x001B74F8
		public void RenderEveryTick(float dt)
		{
			this.BeginBrainGroupUpdate();
			int num = this.InitialProbeCount();
			int num2 = 0;
			while (num2 != this.brains.Count && num != 0)
			{
				this.ClampBrainIndex(ref this.nextUpdateBrain);
				this.debugMaxPriorityBrainCountSeen = Mathf.Max(this.debugMaxPriorityBrainCountSeen, this.priorityBrains.Count);
				Brain brain;
				if (this.AllowPriorityBrains() && this.priorityBrains.Count > 0)
				{
					brain = this.priorityBrains.Dequeue();
				}
				else
				{
					brain = this.brains[this.nextUpdateBrain];
					this.IncrementBrainIndex(ref this.nextUpdateBrain);
				}
				if (brain.IsRunning())
				{
					brain.UpdateBrain();
					num--;
				}
				num2++;
			}
			this.EndBrainGroupUpdate();
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x001B93B8 File Offset: 0x001B75B8
		public void AccumulatePathProbeIterations(Dictionary<string, int> pathProbeIterations)
		{
			foreach (Brain brain in this.brains)
			{
				Navigator component = brain.GetComponent<Navigator>();
				if (!(component == null) && !pathProbeIterations.ContainsKey(brain.name))
				{
					pathProbeIterations.Add(brain.name, component.PathProber.updateCount);
				}
			}
		}

		// Token: 0x06001CCB RID: 7371
		protected abstract int InitialProbeCount();

		// Token: 0x06001CCC RID: 7372
		protected abstract int InitialProbeSize();

		// Token: 0x06001CCD RID: 7373
		protected abstract int MinProbeSize();

		// Token: 0x06001CCE RID: 7374
		protected abstract int IdealProbeSize();

		// Token: 0x06001CCF RID: 7375
		protected abstract int ProbeSizeStep();

		// Token: 0x06001CD0 RID: 7376
		public abstract float GetEstimatedFrameTime();

		// Token: 0x06001CD1 RID: 7377
		public abstract float LoadBalanceThreshold();

		// Token: 0x06001CD2 RID: 7378
		public abstract bool AllowPriorityBrains();

		// Token: 0x06001CD3 RID: 7379 RVA: 0x000B76F3 File Offset: 0x000B58F3
		public virtual void BeginBrainGroupUpdate()
		{
			if (Game.BrainScheduler.isAsyncPathProbeEnabled)
			{
				this.AsyncPathProbe();
			}
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void EndBrainGroupUpdate()
		{
		}

		// Token: 0x04001233 RID: 4659
		protected List<Brain> brains = new List<Brain>();

		// Token: 0x04001234 RID: 4660
		protected Queue<Brain> priorityBrains = new Queue<Brain>();

		// Token: 0x04001235 RID: 4661
		private string increaseLoadLabel;

		// Token: 0x04001236 RID: 4662
		private string decreaseLoadLabel;

		// Token: 0x04001237 RID: 4663
		public bool debugFreezeLoadAdustment;

		// Token: 0x04001238 RID: 4664
		public int debugMaxPriorityBrainCountSeen;

		// Token: 0x04001239 RID: 4665
		private WorkItemCollection<Navigator.PathProbeTask, object> pathProbeJob = new WorkItemCollection<Navigator.PathProbeTask, object>();

		// Token: 0x0400123A RID: 4666
		private int nextUpdateBrain;

		// Token: 0x0400123B RID: 4667
		private int nextPathProbeBrain;
	}

	// Token: 0x02000652 RID: 1618
	private class DupeBrainGroup : BrainScheduler.BrainGroup
	{
		// Token: 0x06001CD5 RID: 7381 RVA: 0x000B7707 File Offset: 0x000B5907
		public DupeBrainGroup() : base(GameTags.DupeBrain)
		{
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x000B771B File Offset: 0x000B591B
		protected override int InitialProbeCount()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().initialProbeCount;
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x000B7727 File Offset: 0x000B5927
		protected override int InitialProbeSize()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().initialProbeSize;
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x000B7733 File Offset: 0x000B5933
		protected override int MinProbeSize()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().minProbeSize;
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x000B773F File Offset: 0x000B593F
		protected override int IdealProbeSize()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().idealProbeSize;
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x000B774B File Offset: 0x000B594B
		protected override int ProbeSizeStep()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().probeSizeStep;
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x000B7757 File Offset: 0x000B5957
		public override float GetEstimatedFrameTime()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().estimatedFrameTime;
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x000B7763 File Offset: 0x000B5963
		public override float LoadBalanceThreshold()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().loadBalanceThreshold;
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000B776F File Offset: 0x000B596F
		public override bool AllowPriorityBrains()
		{
			return this.usePriorityBrain;
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000B7777 File Offset: 0x000B5977
		public override void BeginBrainGroupUpdate()
		{
			base.BeginBrainGroupUpdate();
			this.usePriorityBrain = !this.usePriorityBrain;
		}

		// Token: 0x0400123E RID: 4670
		private bool usePriorityBrain = true;

		// Token: 0x02000653 RID: 1619
		public class Tuning : TuningData<BrainScheduler.DupeBrainGroup.Tuning>
		{
			// Token: 0x0400123F RID: 4671
			public int initialProbeCount = 1;

			// Token: 0x04001240 RID: 4672
			public int initialProbeSize = 1000;

			// Token: 0x04001241 RID: 4673
			public int minProbeSize = 100;

			// Token: 0x04001242 RID: 4674
			public int idealProbeSize = 1000;

			// Token: 0x04001243 RID: 4675
			public int probeSizeStep = 100;

			// Token: 0x04001244 RID: 4676
			public float estimatedFrameTime = 2f;

			// Token: 0x04001245 RID: 4677
			public float loadBalanceThreshold = 0.1f;
		}
	}

	// Token: 0x02000654 RID: 1620
	private class CreatureBrainGroup : BrainScheduler.BrainGroup
	{
		// Token: 0x06001CE0 RID: 7392 RVA: 0x000B778E File Offset: 0x000B598E
		public CreatureBrainGroup() : base(GameTags.CreatureBrain)
		{
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x000B779B File Offset: 0x000B599B
		protected override int InitialProbeCount()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().initialProbeCount;
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x000B77A7 File Offset: 0x000B59A7
		protected override int InitialProbeSize()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().initialProbeSize;
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x000B77B3 File Offset: 0x000B59B3
		protected override int MinProbeSize()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().minProbeSize;
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x000B77BF File Offset: 0x000B59BF
		protected override int IdealProbeSize()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().idealProbeSize;
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x000B77CB File Offset: 0x000B59CB
		protected override int ProbeSizeStep()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().probeSizeStep;
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x000B77D7 File Offset: 0x000B59D7
		public override float GetEstimatedFrameTime()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().estimatedFrameTime;
		}

		// Token: 0x06001CE7 RID: 7399 RVA: 0x000B77E3 File Offset: 0x000B59E3
		public override float LoadBalanceThreshold()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().loadBalanceThreshold;
		}

		// Token: 0x06001CE8 RID: 7400 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public override bool AllowPriorityBrains()
		{
			return true;
		}

		// Token: 0x02000655 RID: 1621
		public class Tuning : TuningData<BrainScheduler.CreatureBrainGroup.Tuning>
		{
			// Token: 0x04001246 RID: 4678
			public int initialProbeCount = 5;

			// Token: 0x04001247 RID: 4679
			public int initialProbeSize = 1000;

			// Token: 0x04001248 RID: 4680
			public int minProbeSize = 100;

			// Token: 0x04001249 RID: 4681
			public int idealProbeSize = 300;

			// Token: 0x0400124A RID: 4682
			public int probeSizeStep = 100;

			// Token: 0x0400124B RID: 4683
			public float estimatedFrameTime = 1f;

			// Token: 0x0400124C RID: 4684
			public float loadBalanceThreshold = 0.1f;
		}
	}
}
