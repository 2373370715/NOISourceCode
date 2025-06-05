using System;
using UnityEngine;

// Token: 0x020016D7 RID: 5847
public class OreSizeVisualizerComponents : KGameObjectComponentManager<OreSizeVisualizerData>
{
	// Token: 0x060078A3 RID: 30883 RVA: 0x00320768 File Offset: 0x0031E968
	public HandleVector<int>.Handle Add(GameObject go)
	{
		HandleVector<int>.Handle handle = base.Add(go, new OreSizeVisualizerData(go));
		this.OnPrefabInit(handle);
		return handle;
	}

	// Token: 0x060078A4 RID: 30884 RVA: 0x0032078C File Offset: 0x0031E98C
	public static HashedString GetAnimForMass(float mass)
	{
		for (int i = 0; i < OreSizeVisualizerComponents.MassTiers.Length; i++)
		{
			if (mass <= OreSizeVisualizerComponents.MassTiers[i].massRequired)
			{
				return OreSizeVisualizerComponents.MassTiers[i].animName;
			}
		}
		return HashedString.Invalid;
	}

	// Token: 0x060078A5 RID: 30885 RVA: 0x003207D4 File Offset: 0x0031E9D4
	protected override void OnPrefabInit(HandleVector<int>.Handle handle)
	{
		Action<object> action = delegate(object ev_data)
		{
			OreSizeVisualizerComponents.OnMassChanged(handle, ev_data);
		};
		OreSizeVisualizerData data = base.GetData(handle);
		data.onMassChangedCB = action;
		data.primaryElement.Subscribe(-2064133523, action);
		data.primaryElement.Subscribe(1335436905, action);
		base.SetData(handle, data);
	}

	// Token: 0x060078A6 RID: 30886 RVA: 0x00320844 File Offset: 0x0031EA44
	protected override void OnSpawn(HandleVector<int>.Handle handle)
	{
		OreSizeVisualizerData data = base.GetData(handle);
		OreSizeVisualizerComponents.OnMassChanged(handle, data.primaryElement.GetComponent<Pickupable>());
	}

	// Token: 0x060078A7 RID: 30887 RVA: 0x0032086C File Offset: 0x0031EA6C
	protected override void OnCleanUp(HandleVector<int>.Handle handle)
	{
		OreSizeVisualizerData data = base.GetData(handle);
		if (data.primaryElement != null)
		{
			Action<object> onMassChangedCB = data.onMassChangedCB;
			data.primaryElement.Unsubscribe(-2064133523, onMassChangedCB);
			data.primaryElement.Unsubscribe(1335436905, onMassChangedCB);
		}
	}

	// Token: 0x060078A8 RID: 30888 RVA: 0x003208B8 File Offset: 0x0031EAB8
	private static void OnMassChanged(HandleVector<int>.Handle handle, object other_data)
	{
		PrimaryElement primaryElement = GameComps.OreSizeVisualizers.GetData(handle).primaryElement;
		float num = primaryElement.Mass;
		if (other_data != null)
		{
			PrimaryElement component = ((Pickupable)other_data).GetComponent<PrimaryElement>();
			num += component.Mass;
		}
		OreSizeVisualizerComponents.MassTier massTier = default(OreSizeVisualizerComponents.MassTier);
		for (int i = 0; i < OreSizeVisualizerComponents.MassTiers.Length; i++)
		{
			if (num <= OreSizeVisualizerComponents.MassTiers[i].massRequired)
			{
				massTier = OreSizeVisualizerComponents.MassTiers[i];
				break;
			}
		}
		primaryElement.GetComponent<KBatchedAnimController>().Play(massTier.animName, KAnim.PlayMode.Once, 1f, 0f);
		KCircleCollider2D component2 = primaryElement.GetComponent<KCircleCollider2D>();
		if (component2 != null)
		{
			component2.radius = massTier.colliderRadius;
		}
		primaryElement.Trigger(1807976145, null);
	}

	// Token: 0x04005AA0 RID: 23200
	private static readonly OreSizeVisualizerComponents.MassTier[] MassTiers = new OreSizeVisualizerComponents.MassTier[]
	{
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle1",
			massRequired = 50f,
			colliderRadius = 0.15f
		},
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle2",
			massRequired = 600f,
			colliderRadius = 0.2f
		},
		new OreSizeVisualizerComponents.MassTier
		{
			animName = "idle3",
			massRequired = float.MaxValue,
			colliderRadius = 0.25f
		}
	};

	// Token: 0x020016D8 RID: 5848
	private struct MassTier
	{
		// Token: 0x04005AA1 RID: 23201
		public HashedString animName;

		// Token: 0x04005AA2 RID: 23202
		public float massRequired;

		// Token: 0x04005AA3 RID: 23203
		public float colliderRadius;
	}
}
