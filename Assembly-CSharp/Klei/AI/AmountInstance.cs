using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C5C RID: 15452
	[SerializationConfig(MemberSerialization.OptIn)]
	[DebuggerDisplay("{amount.Name} {value} ({deltaAttribute.value}/{minAttribute.value}/{maxAttribute.value})")]
	public class AmountInstance : ModifierInstance<Amount>, ISaveLoadable, ISim200ms
	{
		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x0600ECD7 RID: 60631 RVA: 0x00143786 File Offset: 0x00141986
		public Amount amount
		{
			get
			{
				return this.modifier;
			}
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x0600ECD8 RID: 60632 RVA: 0x0014378E File Offset: 0x0014198E
		// (set) Token: 0x0600ECD9 RID: 60633 RVA: 0x00143796 File Offset: 0x00141996
		public bool paused
		{
			get
			{
				return this._paused;
			}
			set
			{
				this._paused = this.paused;
				if (this._paused)
				{
					this.Deactivate();
					return;
				}
				this.Activate();
			}
		}

		// Token: 0x0600ECDA RID: 60634 RVA: 0x001437B9 File Offset: 0x001419B9
		public float GetMin()
		{
			return this.minAttribute.GetTotalValue();
		}

		// Token: 0x0600ECDB RID: 60635 RVA: 0x001437C6 File Offset: 0x001419C6
		public float GetMax()
		{
			return this.maxAttribute.GetTotalValue();
		}

		// Token: 0x0600ECDC RID: 60636 RVA: 0x001437D3 File Offset: 0x001419D3
		public float GetDelta()
		{
			return this.deltaAttribute.GetTotalValue();
		}

		// Token: 0x0600ECDD RID: 60637 RVA: 0x004DF1E4 File Offset: 0x004DD3E4
		public AmountInstance(Amount amount, GameObject game_object) : base(game_object, amount)
		{
			Attributes attributes = game_object.GetAttributes();
			this.minAttribute = attributes.Add(amount.minAttribute);
			this.maxAttribute = attributes.Add(amount.maxAttribute);
			this.deltaAttribute = attributes.Add(amount.deltaAttribute);
		}

		// Token: 0x0600ECDE RID: 60638 RVA: 0x001437E0 File Offset: 0x001419E0
		public float SetValue(float value)
		{
			this.value = Mathf.Min(Mathf.Max(value, this.GetMin()), this.GetMax());
			return this.value;
		}

		// Token: 0x0600ECDF RID: 60639 RVA: 0x00143805 File Offset: 0x00141A05
		public void Publish(float delta, float previous_value)
		{
			if (this.OnDelta != null)
			{
				this.OnDelta(delta);
			}
			if (this.OnMaxValueReached != null && previous_value < this.GetMax() && this.value >= this.GetMax())
			{
				this.OnMaxValueReached();
			}
		}

		// Token: 0x0600ECE0 RID: 60640 RVA: 0x004DF238 File Offset: 0x004DD438
		public float ApplyDelta(float delta)
		{
			float previous_value = this.value;
			this.SetValue(this.value + delta);
			this.Publish(delta, previous_value);
			return this.value;
		}

		// Token: 0x0600ECE1 RID: 60641 RVA: 0x00143845 File Offset: 0x00141A45
		public string GetValueString()
		{
			return this.amount.GetValueString(this);
		}

		// Token: 0x0600ECE2 RID: 60642 RVA: 0x00143853 File Offset: 0x00141A53
		public string GetDescription()
		{
			return this.amount.GetDescription(this);
		}

		// Token: 0x0600ECE3 RID: 60643 RVA: 0x00143861 File Offset: 0x00141A61
		public string GetTooltip()
		{
			return this.amount.GetTooltip(this);
		}

		// Token: 0x0600ECE4 RID: 60644 RVA: 0x0014386F File Offset: 0x00141A6F
		public void Activate()
		{
			SimAndRenderScheduler.instance.Add(this, false);
		}

		// Token: 0x0600ECE5 RID: 60645 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Sim200ms(float dt)
		{
		}

		// Token: 0x0600ECE6 RID: 60646 RVA: 0x004DF26C File Offset: 0x004DD46C
		public static void BatchUpdate(List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances, float time_delta)
		{
			if (time_delta == 0f)
			{
				return;
			}
			AmountInstance.BatchUpdateContext context = new AmountInstance.BatchUpdateContext(amount_instances, time_delta);
			AmountInstance.AmmountInstanceBatchUpdateDispatcher.Instance.Reset(context);
			GlobalJobManager.Run(AmountInstance.AmmountInstanceBatchUpdateDispatcher.Instance);
			AmountInstance.AmmountInstanceBatchUpdateDispatcher.Instance.Finish();
			AmountInstance.AmmountInstanceBatchUpdateDispatcher.Instance.Reset(AmountInstance.BatchUpdateContext.EmptyContext);
		}

		// Token: 0x0600ECE7 RID: 60647 RVA: 0x000C550D File Offset: 0x000C370D
		public void Deactivate()
		{
			SimAndRenderScheduler.instance.Remove(this);
		}

		// Token: 0x0400E913 RID: 59667
		[Serialize]
		public float value;

		// Token: 0x0400E914 RID: 59668
		public AttributeInstance minAttribute;

		// Token: 0x0400E915 RID: 59669
		public AttributeInstance maxAttribute;

		// Token: 0x0400E916 RID: 59670
		public AttributeInstance deltaAttribute;

		// Token: 0x0400E917 RID: 59671
		public Action<float> OnDelta;

		// Token: 0x0400E918 RID: 59672
		public System.Action OnMaxValueReached;

		// Token: 0x0400E919 RID: 59673
		public bool hide;

		// Token: 0x0400E91A RID: 59674
		private bool _paused;

		// Token: 0x02003C5D RID: 15453
		private struct BatchUpdateContext
		{
			// Token: 0x0600ECE8 RID: 60648 RVA: 0x0014387D File Offset: 0x00141A7D
			public BatchUpdateContext(List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances, float time_delta)
			{
				this.amount_instances = amount_instances;
				this.time_delta = time_delta;
			}

			// Token: 0x0400E91B RID: 59675
			public List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances;

			// Token: 0x0400E91C RID: 59676
			public float time_delta;

			// Token: 0x0400E91D RID: 59677
			public static AmountInstance.BatchUpdateContext EmptyContext = new AmountInstance.BatchUpdateContext(null, 0f);

			// Token: 0x02003C5E RID: 15454
			public struct Result
			{
				// Token: 0x0400E91E RID: 59678
				public AmountInstance amount_instance;

				// Token: 0x0400E91F RID: 59679
				public float previous;

				// Token: 0x0400E920 RID: 59680
				public float delta;
			}
		}

		// Token: 0x02003C5F RID: 15455
		private class AmmountInstanceBatchUpdateDispatcher : WorkItemCollectionWithThreadContex<AmountInstance.BatchUpdateContext, List<AmountInstance.BatchUpdateContext.Result>>
		{
			// Token: 0x17000C48 RID: 3144
			// (get) Token: 0x0600ECEA RID: 60650 RVA: 0x0014389F File Offset: 0x00141A9F
			public static AmountInstance.AmmountInstanceBatchUpdateDispatcher Instance
			{
				get
				{
					if (AmountInstance.AmmountInstanceBatchUpdateDispatcher.instance == null || AmountInstance.AmmountInstanceBatchUpdateDispatcher.instance.threadContexts.Count != GlobalJobManager.ThreadCount)
					{
						AmountInstance.AmmountInstanceBatchUpdateDispatcher.instance = new AmountInstance.AmmountInstanceBatchUpdateDispatcher();
					}
					return AmountInstance.AmmountInstanceBatchUpdateDispatcher.instance;
				}
			}

			// Token: 0x0600ECEB RID: 60651 RVA: 0x004DF2BC File Offset: 0x004DD4BC
			public AmmountInstanceBatchUpdateDispatcher()
			{
				this.threadContexts = new List<List<AmountInstance.BatchUpdateContext.Result>>(GlobalJobManager.ThreadCount);
				for (int i = 0; i < GlobalJobManager.ThreadCount; i++)
				{
					this.threadContexts.Add(new List<AmountInstance.BatchUpdateContext.Result>());
				}
			}

			// Token: 0x0600ECEC RID: 60652 RVA: 0x001438CD File Offset: 0x00141ACD
			public void Reset(AmountInstance.BatchUpdateContext context)
			{
				this.sharedData = context;
				if (context.amount_instances == null)
				{
					this.count = 0;
					return;
				}
				this.count = (context.amount_instances.Count + 512 - 1) / 512;
			}

			// Token: 0x0600ECED RID: 60653 RVA: 0x004DF300 File Offset: 0x004DD500
			public override void RunItem(int item, ref AmountInstance.BatchUpdateContext shared_data, List<AmountInstance.BatchUpdateContext.Result> thread_context, int threadIndex)
			{
				int num = item * 512;
				int num2 = Mathf.Min(num + 512, shared_data.amount_instances.Count);
				for (int i = num; i < num2; i++)
				{
					AmountInstance amountInstance = (AmountInstance)shared_data.amount_instances[i].data;
					float num3 = amountInstance.GetDelta() * shared_data.time_delta;
					if (num3 != 0f)
					{
						thread_context.Add(new AmountInstance.BatchUpdateContext.Result
						{
							amount_instance = amountInstance,
							previous = amountInstance.value,
							delta = num3
						});
						amountInstance.SetValue(amountInstance.value + num3);
					}
				}
			}

			// Token: 0x0600ECEE RID: 60654 RVA: 0x004DF3A0 File Offset: 0x004DD5A0
			public void Finish()
			{
				foreach (List<AmountInstance.BatchUpdateContext.Result> list in this.threadContexts)
				{
					foreach (AmountInstance.BatchUpdateContext.Result result in list)
					{
						result.amount_instance.Publish(result.delta, result.previous);
					}
					list.Clear();
				}
			}

			// Token: 0x0400E921 RID: 59681
			private const int kBatchSize = 512;

			// Token: 0x0400E922 RID: 59682
			private static AmountInstance.AmmountInstanceBatchUpdateDispatcher instance;
		}
	}
}
