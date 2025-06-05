using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020012DF RID: 4831
public class EntityLuminescence : GameStateMachine<EntityLuminescence, EntityLuminescence.Instance, IStateMachineTarget, EntityLuminescence.Def>
{
	// Token: 0x06006321 RID: 25377 RVA: 0x000E518B File Offset: 0x000E338B
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x020012E0 RID: 4832
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400470D RID: 18189
		public Color lightColor;

		// Token: 0x0400470E RID: 18190
		public float lightRange;

		// Token: 0x0400470F RID: 18191
		public float lightAngle;

		// Token: 0x04004710 RID: 18192
		public Vector2 lightOffset;

		// Token: 0x04004711 RID: 18193
		public Vector2 lightDirection;

		// Token: 0x04004712 RID: 18194
		public global::LightShape lightShape;
	}

	// Token: 0x020012E1 RID: 4833
	public new class Instance : GameStateMachine<EntityLuminescence, EntityLuminescence.Instance, IStateMachineTarget, EntityLuminescence.Def>.GameInstance
	{
		// Token: 0x06006324 RID: 25380 RVA: 0x002C7334 File Offset: 0x002C5534
		public Instance(IStateMachineTarget master, EntityLuminescence.Def def) : base(master, def)
		{
			this.light.Color = def.lightColor;
			this.light.Range = def.lightRange;
			this.light.Angle = def.lightAngle;
			this.light.Direction = def.lightDirection;
			this.light.Offset = def.lightOffset;
			this.light.shape = def.lightShape;
		}

		// Token: 0x06006325 RID: 25381 RVA: 0x002C73B0 File Offset: 0x002C55B0
		public override void StartSM()
		{
			base.StartSM();
			this.luminescence = Db.Get().Attributes.Luminescence.Lookup(base.gameObject);
			AttributeInstance attributeInstance = this.luminescence;
			attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, new System.Action(this.OnLuminescenceChanged));
			this.RefreshLight();
		}

		// Token: 0x06006326 RID: 25382 RVA: 0x000E51A4 File Offset: 0x000E33A4
		private void OnLuminescenceChanged()
		{
			this.RefreshLight();
		}

		// Token: 0x06006327 RID: 25383 RVA: 0x002C7410 File Offset: 0x002C5610
		public void RefreshLight()
		{
			if (this.luminescence != null)
			{
				int num = (int)this.luminescence.GetTotalValue();
				this.light.Lux = num;
				bool flag = num > 0;
				if (this.light.enabled != flag)
				{
					this.light.enabled = flag;
				}
			}
		}

		// Token: 0x06006328 RID: 25384 RVA: 0x000E51AC File Offset: 0x000E33AC
		protected override void OnCleanUp()
		{
			if (this.luminescence != null)
			{
				AttributeInstance attributeInstance = this.luminescence;
				attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, new System.Action(this.OnLuminescenceChanged));
			}
			base.OnCleanUp();
		}

		// Token: 0x04004713 RID: 18195
		[MyCmpAdd]
		private Light2D light;

		// Token: 0x04004714 RID: 18196
		private AttributeInstance luminescence;
	}
}
