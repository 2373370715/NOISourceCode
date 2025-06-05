using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200117D RID: 4477
public class CreatureLightToggleController : GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>
{
	// Token: 0x06005B35 RID: 23349 RVA: 0x002A5594 File Offset: 0x002A3794
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.light_off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.light_off.Enter(delegate(CreatureLightToggleController.Instance smi)
		{
			smi.SwitchLight(false);
		}).EventHandlerTransition(GameHashes.TagsChanged, this.turning_on, new Func<CreatureLightToggleController.Instance, object, bool>(CreatureLightToggleController.ShouldProduceLight));
		this.turning_off.BatchUpdate(delegate(List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances, float time_delta)
		{
			CreatureLightToggleController.Instance.ModifyBrightness(instances, CreatureLightToggleController.Instance.dim, time_delta);
		}, UpdateRate.SIM_200ms).Transition(this.light_off, (CreatureLightToggleController.Instance smi) => smi.IsOff(), UpdateRate.SIM_200ms);
		this.light_on.Enter(delegate(CreatureLightToggleController.Instance smi)
		{
			smi.SwitchLight(true);
		}).EventHandlerTransition(GameHashes.TagsChanged, this.turning_off, (CreatureLightToggleController.Instance smi, object obj) => !CreatureLightToggleController.ShouldProduceLight(smi, obj));
		this.turning_on.Enter(delegate(CreatureLightToggleController.Instance smi)
		{
			smi.SwitchLight(true);
		}).BatchUpdate(delegate(List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances, float time_delta)
		{
			CreatureLightToggleController.Instance.ModifyBrightness(instances, CreatureLightToggleController.Instance.brighten, time_delta);
		}, UpdateRate.SIM_200ms).Transition(this.light_on, (CreatureLightToggleController.Instance smi) => smi.IsOn(), UpdateRate.SIM_200ms);
	}

	// Token: 0x06005B36 RID: 23350 RVA: 0x000DFD74 File Offset: 0x000DDF74
	public static bool ShouldProduceLight(CreatureLightToggleController.Instance smi, object obj)
	{
		return !smi.prefabID.HasTag(GameTags.Creatures.Overcrowded) && !smi.prefabID.HasTag(GameTags.Creatures.TrappedInCargoBay);
	}

	// Token: 0x040040E3 RID: 16611
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State light_off;

	// Token: 0x040040E4 RID: 16612
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State turning_off;

	// Token: 0x040040E5 RID: 16613
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State light_on;

	// Token: 0x040040E6 RID: 16614
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State turning_on;

	// Token: 0x0200117E RID: 4478
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200117F RID: 4479
	public new class Instance : GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.GameInstance
	{
		// Token: 0x06005B39 RID: 23353 RVA: 0x002A5724 File Offset: 0x002A3924
		public Instance(IStateMachineTarget master, CreatureLightToggleController.Def def) : base(master, def)
		{
			this.prefabID = base.gameObject.GetComponent<KPrefabID>();
			this.light = master.GetComponent<Light2D>();
			this.originalLux = this.light.Lux;
			this.originalRange = this.light.Range;
		}

		// Token: 0x06005B3A RID: 23354 RVA: 0x000DFDA7 File Offset: 0x000DDFA7
		public void SwitchLight(bool on)
		{
			this.light.enabled = on;
		}

		// Token: 0x06005B3B RID: 23355 RVA: 0x002A5778 File Offset: 0x002A3978
		public static void ModifyBrightness(List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances, CreatureLightToggleController.Instance.ModifyLuxDelegate modify_lux, float time_delta)
		{
			CreatureLightToggleController.Instance.modify_brightness_job.Reset(null);
			for (int num = 0; num != instances.Count; num++)
			{
				UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry entry = instances[num];
				entry.lastUpdateTime = 0f;
				instances[num] = entry;
				CreatureLightToggleController.Instance data = entry.data;
				modify_lux(data, time_delta);
				data.light.Range = data.originalRange * (float)data.light.Lux / (float)data.originalLux;
				data.light.RefreshShapeAndPosition();
				if (data.light.RefreshShapeAndPosition() != Light2D.RefreshResult.None)
				{
					CreatureLightToggleController.Instance.modify_brightness_job.Add(new CreatureLightToggleController.Instance.ModifyBrightnessTask(data.light.emitter));
				}
			}
			GlobalJobManager.Run(CreatureLightToggleController.Instance.modify_brightness_job);
			for (int num2 = 0; num2 != CreatureLightToggleController.Instance.modify_brightness_job.Count; num2++)
			{
				CreatureLightToggleController.Instance.modify_brightness_job.GetWorkItem(num2).Finish();
			}
			CreatureLightToggleController.Instance.modify_brightness_job.Reset(null);
		}

		// Token: 0x06005B3C RID: 23356 RVA: 0x000DFDB5 File Offset: 0x000DDFB5
		public bool IsOff()
		{
			return this.light.Lux == 0;
		}

		// Token: 0x06005B3D RID: 23357 RVA: 0x000DFDC5 File Offset: 0x000DDFC5
		public bool IsOn()
		{
			return this.light.Lux >= this.originalLux;
		}

		// Token: 0x040040E7 RID: 16615
		private const float DIM_TIME = 25f;

		// Token: 0x040040E8 RID: 16616
		private const float GLOW_TIME = 15f;

		// Token: 0x040040E9 RID: 16617
		private int originalLux;

		// Token: 0x040040EA RID: 16618
		private float originalRange;

		// Token: 0x040040EB RID: 16619
		private Light2D light;

		// Token: 0x040040EC RID: 16620
		public KPrefabID prefabID;

		// Token: 0x040040ED RID: 16621
		private static WorkItemCollection<CreatureLightToggleController.Instance.ModifyBrightnessTask, object> modify_brightness_job = new WorkItemCollection<CreatureLightToggleController.Instance.ModifyBrightnessTask, object>();

		// Token: 0x040040EE RID: 16622
		public static CreatureLightToggleController.Instance.ModifyLuxDelegate dim = delegate(CreatureLightToggleController.Instance instance, float time_delta)
		{
			float num = (float)instance.originalLux / 25f;
			instance.light.Lux = Mathf.FloorToInt(Mathf.Max(0f, (float)instance.light.Lux - num * time_delta));
		};

		// Token: 0x040040EF RID: 16623
		public static CreatureLightToggleController.Instance.ModifyLuxDelegate brighten = delegate(CreatureLightToggleController.Instance instance, float time_delta)
		{
			float num = (float)instance.originalLux / 15f;
			instance.light.Lux = Mathf.CeilToInt(Mathf.Min((float)instance.originalLux, (float)instance.light.Lux + num * time_delta));
		};

		// Token: 0x02001180 RID: 4480
		private struct ModifyBrightnessTask : IWorkItem<object>
		{
			// Token: 0x06005B3F RID: 23359 RVA: 0x000DFE13 File Offset: 0x000DE013
			public ModifyBrightnessTask(LightGridManager.LightGridEmitter emitter)
			{
				this.emitter = emitter;
				emitter.RemoveFromGrid();
			}

			// Token: 0x06005B40 RID: 23360 RVA: 0x000DFE22 File Offset: 0x000DE022
			public void Run(object context, int threadIndex)
			{
				this.emitter.UpdateLitCells();
			}

			// Token: 0x06005B41 RID: 23361 RVA: 0x000DFE2F File Offset: 0x000DE02F
			public void Finish()
			{
				this.emitter.AddToGrid(false);
			}

			// Token: 0x040040F0 RID: 16624
			private LightGridManager.LightGridEmitter emitter;
		}

		// Token: 0x02001181 RID: 4481
		// (Invoke) Token: 0x06005B43 RID: 23363
		public delegate void ModifyLuxDelegate(CreatureLightToggleController.Instance instance, float time_delta);
	}
}
