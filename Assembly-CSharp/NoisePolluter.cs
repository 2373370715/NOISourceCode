using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020016B6 RID: 5814
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/NoisePolluter")]
public class NoisePolluter : KMonoBehaviour, IPolluter
{
	// Token: 0x060077DA RID: 30682 RVA: 0x000F3566 File Offset: 0x000F1766
	public static bool IsNoiseableCell(int cell)
	{
		return Grid.IsValidCell(cell) && (Grid.IsGas(cell) || !Grid.IsSubstantialLiquid(cell, 0.35f));
	}

	// Token: 0x060077DB RID: 30683 RVA: 0x000F358A File Offset: 0x000F178A
	public void ResetCells()
	{
		if (this.radius == 0)
		{
			global::Debug.LogFormat("[{0}] has a 0 radius noise, this will disable it", new object[]
			{
				this.GetName()
			});
			return;
		}
	}

	// Token: 0x060077DC RID: 30684 RVA: 0x000F35AE File Offset: 0x000F17AE
	public void SetAttributes(Vector2 pos, int dB, GameObject go, string name)
	{
		this.sourceName = name;
		this.noise = dB;
	}

	// Token: 0x060077DD RID: 30685 RVA: 0x000F35BF File Offset: 0x000F17BF
	public int GetRadius()
	{
		return this.radius;
	}

	// Token: 0x060077DE RID: 30686 RVA: 0x000F35C7 File Offset: 0x000F17C7
	public int GetNoise()
	{
		return this.noise;
	}

	// Token: 0x060077DF RID: 30687 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x060077E0 RID: 30688 RVA: 0x000F35CF File Offset: 0x000F17CF
	public void SetSplat(NoiseSplat new_splat)
	{
		this.splat = new_splat;
	}

	// Token: 0x060077E1 RID: 30689 RVA: 0x000F35D8 File Offset: 0x000F17D8
	public void Clear()
	{
		if (this.splat != null)
		{
			this.splat.Clear();
			this.splat = null;
		}
	}

	// Token: 0x060077E2 RID: 30690 RVA: 0x000F35F4 File Offset: 0x000F17F4
	public Vector2 GetPosition()
	{
		return base.transform.GetPosition();
	}

	// Token: 0x1700078B RID: 1931
	// (get) Token: 0x060077E3 RID: 30691 RVA: 0x000F3606 File Offset: 0x000F1806
	// (set) Token: 0x060077E4 RID: 30692 RVA: 0x000F360E File Offset: 0x000F180E
	public string sourceName { get; private set; }

	// Token: 0x1700078C RID: 1932
	// (get) Token: 0x060077E5 RID: 30693 RVA: 0x000F3617 File Offset: 0x000F1817
	// (set) Token: 0x060077E6 RID: 30694 RVA: 0x000F361F File Offset: 0x000F181F
	public bool active { get; private set; }

	// Token: 0x060077E7 RID: 30695 RVA: 0x000F3628 File Offset: 0x000F1828
	public void SetActive(bool active = true)
	{
		if (!active && this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
		}
		this.active = active;
	}

	// Token: 0x060077E8 RID: 30696 RVA: 0x0031C764 File Offset: 0x0031A964
	public void Refresh()
	{
		if (this.active)
		{
			if (this.splat != null)
			{
				AudioEventManager.Get().ClearNoiseSplat(this.splat);
				this.splat.Clear();
			}
			KSelectable component = base.GetComponent<KSelectable>();
			string name = (component != null) ? component.GetName() : base.name;
			GameObject gameObject = base.GetComponent<KMonoBehaviour>().gameObject;
			this.splat = AudioEventManager.Get().CreateNoiseSplat(this.GetPosition(), this.noise, this.radius, name, gameObject);
		}
	}

	// Token: 0x060077E9 RID: 30697 RVA: 0x0031C7EC File Offset: 0x0031A9EC
	private void OnActiveChanged(object data)
	{
		bool isActive = ((Operational)data).IsActive;
		this.SetActive(isActive);
		this.Refresh();
	}

	// Token: 0x060077EA RID: 30698 RVA: 0x000F3657 File Offset: 0x000F1857
	public void SetValues(EffectorValues values)
	{
		this.noise = values.amount;
		this.radius = values.radius;
	}

	// Token: 0x060077EB RID: 30699 RVA: 0x0031C814 File Offset: 0x0031AA14
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.radius == 0 || this.noise == 0)
		{
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"Noisepollutor::OnSpawn [",
				this.GetName(),
				"] noise: [",
				this.noise.ToString(),
				"] radius: [",
				this.radius.ToString(),
				"]"
			}));
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.ResetCells();
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			base.Subscribe<NoisePolluter>(824508782, NoisePolluter.OnActiveChangedDelegate);
		}
		this.refreshCallback = new System.Action(this.Refresh);
		this.refreshPartionerCallback = delegate(object data)
		{
			this.Refresh();
		};
		this.onCollectNoisePollutersCallback = new Action<object>(this.OnCollectNoisePolluters);
		Attributes attributes = this.GetAttributes();
		Db db = Db.Get();
		this.dB = attributes.Add(db.BuildingAttributes.NoisePollution);
		this.dBRadius = attributes.Add(db.BuildingAttributes.NoisePollutionRadius);
		if (this.noise != 0 && this.radius != 0)
		{
			AttributeModifier modifier = new AttributeModifier(db.BuildingAttributes.NoisePollution.Id, (float)this.noise, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			AttributeModifier modifier2 = new AttributeModifier(db.BuildingAttributes.NoisePollutionRadius.Id, (float)this.radius, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			attributes.Add(modifier);
			attributes.Add(modifier2);
		}
		else
		{
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"Noisepollutor::OnSpawn [",
				this.GetName(),
				"] radius: [",
				this.radius.ToString(),
				"] noise: [",
				this.noise.ToString(),
				"]"
			}));
		}
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		this.isMovable = (component2 != null && component2.isMovable);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "NoisePolluter.OnSpawn");
		AttributeInstance attributeInstance = this.dB;
		attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, this.refreshCallback);
		AttributeInstance attributeInstance2 = this.dBRadius;
		attributeInstance2.OnDirty = (System.Action)Delegate.Combine(attributeInstance2.OnDirty, this.refreshCallback);
		if (component != null)
		{
			this.OnActiveChanged(component.IsActive);
		}
	}

	// Token: 0x060077EC RID: 30700 RVA: 0x000F3671 File Offset: 0x000F1871
	private void OnCellChange()
	{
		this.Refresh();
	}

	// Token: 0x060077ED RID: 30701 RVA: 0x000F3679 File Offset: 0x000F1879
	private void OnCollectNoisePolluters(object data)
	{
		((List<NoisePolluter>)data).Add(this);
	}

	// Token: 0x060077EE RID: 30702 RVA: 0x000F3687 File Offset: 0x000F1887
	public string GetName()
	{
		if (string.IsNullOrEmpty(this.sourceName))
		{
			this.sourceName = base.GetComponent<KSelectable>().GetName();
		}
		return this.sourceName;
	}

	// Token: 0x060077EF RID: 30703 RVA: 0x0031CA98 File Offset: 0x0031AC98
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (base.isSpawned)
		{
			if (this.dB != null)
			{
				AttributeInstance attributeInstance = this.dB;
				attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, this.refreshCallback);
				AttributeInstance attributeInstance2 = this.dBRadius;
				attributeInstance2.OnDirty = (System.Action)Delegate.Remove(attributeInstance2.OnDirty, this.refreshCallback);
			}
			if (this.isMovable)
			{
				Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
			}
		}
		if (this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
		}
	}

	// Token: 0x060077F0 RID: 30704 RVA: 0x000F36AD File Offset: 0x000F18AD
	public float GetNoiseForCell(int cell)
	{
		return this.splat.GetDBForCell(cell);
	}

	// Token: 0x060077F1 RID: 30705 RVA: 0x0031CB44 File Offset: 0x0031AD44
	public List<Descriptor> GetEffectDescriptions()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.dB != null && this.dBRadius != null)
		{
			float totalValue = this.dB.GetTotalValue();
			float totalValue2 = this.dBRadius.GetTotalValue();
			string text = (this.noise > 0) ? UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_INCREASE : UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_DECREASE;
			text = text + "\n\n" + this.dB.GetAttributeValueTooltip();
			string arg = GameUtil.AddPositiveSign(totalValue.ToString(), totalValue > 0f);
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.NOISE_CREATED, arg, totalValue2), string.Format(text, arg, totalValue2), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else if (this.noise != 0)
		{
			string format = (this.noise >= 0) ? UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_INCREASE : UI.BUILDINGEFFECTS.TOOLTIPS.NOISE_POLLUTION_DECREASE;
			string arg2 = GameUtil.AddPositiveSign(this.noise.ToString(), this.noise > 0);
			Descriptor item2 = new Descriptor(string.Format(UI.BUILDINGEFFECTS.NOISE_CREATED, arg2, this.radius), string.Format(format, arg2, this.radius), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x060077F2 RID: 30706 RVA: 0x000F36BB File Offset: 0x000F18BB
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.GetEffectDescriptions();
	}

	// Token: 0x04005A23 RID: 23075
	public const string ID = "NoisePolluter";

	// Token: 0x04005A24 RID: 23076
	public int radius;

	// Token: 0x04005A25 RID: 23077
	public int noise;

	// Token: 0x04005A26 RID: 23078
	public AttributeInstance dB;

	// Token: 0x04005A27 RID: 23079
	public AttributeInstance dBRadius;

	// Token: 0x04005A28 RID: 23080
	private NoiseSplat splat;

	// Token: 0x04005A2A RID: 23082
	public System.Action refreshCallback;

	// Token: 0x04005A2B RID: 23083
	public Action<object> refreshPartionerCallback;

	// Token: 0x04005A2C RID: 23084
	public Action<object> onCollectNoisePollutersCallback;

	// Token: 0x04005A2D RID: 23085
	public bool isMovable;

	// Token: 0x04005A2E RID: 23086
	[MyCmpReq]
	public OccupyArea occupyArea;

	// Token: 0x04005A30 RID: 23088
	private static readonly EventSystem.IntraObjectHandler<NoisePolluter> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<NoisePolluter>(delegate(NoisePolluter component, object data)
	{
		component.OnActiveChanged(data);
	});
}
