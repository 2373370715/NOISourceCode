using System;
using System.Collections.Generic;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using KSerialization;
using UnityEngine;

// Token: 0x020012CF RID: 4815
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name} {WattsUsed}W")]
[AddComponentMenu("KMonoBehaviour/scripts/EnergyConsumer")]
public class EnergyConsumer : KMonoBehaviour, ISaveLoadable, IEnergyConsumer, ICircuitConnected, IGameObjectEffectDescriptor
{
	// Token: 0x17000610 RID: 1552
	// (get) Token: 0x060062A9 RID: 25257 RVA: 0x000E4CBC File Offset: 0x000E2EBC
	public int PowerSortOrder
	{
		get
		{
			return this.powerSortOrder;
		}
	}

	// Token: 0x17000611 RID: 1553
	// (get) Token: 0x060062AA RID: 25258 RVA: 0x000E4CC4 File Offset: 0x000E2EC4
	// (set) Token: 0x060062AB RID: 25259 RVA: 0x000E4CCC File Offset: 0x000E2ECC
	public int PowerCell { get; private set; }

	// Token: 0x17000612 RID: 1554
	// (get) Token: 0x060062AC RID: 25260 RVA: 0x000E4CD5 File Offset: 0x000E2ED5
	public bool HasWire
	{
		get
		{
			return Grid.Objects[this.PowerCell, 26] != null;
		}
	}

	// Token: 0x17000613 RID: 1555
	// (get) Token: 0x060062AD RID: 25261 RVA: 0x000E4CEF File Offset: 0x000E2EEF
	// (set) Token: 0x060062AE RID: 25262 RVA: 0x000E4D01 File Offset: 0x000E2F01
	public virtual bool IsPowered
	{
		get
		{
			return this.operational.GetFlag(EnergyConsumer.PoweredFlag);
		}
		protected set
		{
			this.operational.SetFlag(EnergyConsumer.PoweredFlag, value);
		}
	}

	// Token: 0x17000614 RID: 1556
	// (get) Token: 0x060062AF RID: 25263 RVA: 0x000E4D14 File Offset: 0x000E2F14
	public bool IsConnected
	{
		get
		{
			return this.CircuitID != ushort.MaxValue;
		}
	}

	// Token: 0x17000615 RID: 1557
	// (get) Token: 0x060062B0 RID: 25264 RVA: 0x000E4D26 File Offset: 0x000E2F26
	public string Name
	{
		get
		{
			return this.selectable.GetName();
		}
	}

	// Token: 0x17000616 RID: 1558
	// (get) Token: 0x060062B1 RID: 25265 RVA: 0x000E4D33 File Offset: 0x000E2F33
	// (set) Token: 0x060062B2 RID: 25266 RVA: 0x000E4D3B File Offset: 0x000E2F3B
	public bool IsVirtual { get; private set; }

	// Token: 0x17000617 RID: 1559
	// (get) Token: 0x060062B3 RID: 25267 RVA: 0x000E4D44 File Offset: 0x000E2F44
	// (set) Token: 0x060062B4 RID: 25268 RVA: 0x000E4D4C File Offset: 0x000E2F4C
	public object VirtualCircuitKey { get; private set; }

	// Token: 0x17000618 RID: 1560
	// (get) Token: 0x060062B5 RID: 25269 RVA: 0x000E4D55 File Offset: 0x000E2F55
	// (set) Token: 0x060062B6 RID: 25270 RVA: 0x000E4D5D File Offset: 0x000E2F5D
	public ushort CircuitID { get; private set; }

	// Token: 0x17000619 RID: 1561
	// (get) Token: 0x060062B7 RID: 25271 RVA: 0x000E4D66 File Offset: 0x000E2F66
	// (set) Token: 0x060062B8 RID: 25272 RVA: 0x000E4D6E File Offset: 0x000E2F6E
	public float BaseWattageRating
	{
		get
		{
			return this._BaseWattageRating;
		}
		set
		{
			this._BaseWattageRating = value;
		}
	}

	// Token: 0x1700061A RID: 1562
	// (get) Token: 0x060062B9 RID: 25273 RVA: 0x000E4D77 File Offset: 0x000E2F77
	public float WattsUsed
	{
		get
		{
			if (this.operational.IsActive)
			{
				return this.BaseWattageRating;
			}
			return 0f;
		}
	}

	// Token: 0x1700061B RID: 1563
	// (get) Token: 0x060062BA RID: 25274 RVA: 0x000E4D92 File Offset: 0x000E2F92
	public float WattsNeededWhenActive
	{
		get
		{
			return this.building.Def.EnergyConsumptionWhenActive;
		}
	}

	// Token: 0x060062BB RID: 25275 RVA: 0x000E4DA4 File Offset: 0x000E2FA4
	protected override void OnPrefabInit()
	{
		this.CircuitID = ushort.MaxValue;
		this.IsPowered = false;
		this.BaseWattageRating = this.building.Def.EnergyConsumptionWhenActive;
	}

	// Token: 0x060062BC RID: 25276 RVA: 0x002C5C40 File Offset: 0x002C3E40
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.EnergyConsumers.Add(this);
		Building component = base.GetComponent<Building>();
		this.PowerCell = component.GetPowerInputCell();
		Game.Instance.circuitManager.Connect(this);
		Game.Instance.energySim.AddEnergyConsumer(this);
	}

	// Token: 0x060062BD RID: 25277 RVA: 0x000E4DCE File Offset: 0x000E2FCE
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveEnergyConsumer(this);
		Game.Instance.circuitManager.Disconnect(this, true);
		Components.EnergyConsumers.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060062BE RID: 25278 RVA: 0x000E4E02 File Offset: 0x000E3002
	public virtual void EnergySim200ms(float dt)
	{
		this.CircuitID = Game.Instance.circuitManager.GetCircuitID(this);
		if (!this.IsConnected)
		{
			this.IsPowered = false;
		}
		this.circuitOverloadTime = Mathf.Max(0f, this.circuitOverloadTime - dt);
	}

	// Token: 0x060062BF RID: 25279 RVA: 0x002C5C94 File Offset: 0x002C3E94
	public virtual void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
	{
		switch (connection_status)
		{
		case CircuitManager.ConnectionStatus.NotConnected:
			this.IsPowered = false;
			return;
		case CircuitManager.ConnectionStatus.Unpowered:
			if (this.IsPowered && base.GetComponent<Battery>() == null)
			{
				this.IsPowered = false;
				this.circuitOverloadTime = 6f;
				this.PlayCircuitSound("overdraw");
				return;
			}
			break;
		case CircuitManager.ConnectionStatus.Powered:
			if (!this.IsPowered && this.circuitOverloadTime <= 0f)
			{
				this.IsPowered = true;
				this.PlayCircuitSound("powered");
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060062C0 RID: 25280 RVA: 0x002C5D18 File Offset: 0x002C3F18
	protected void PlayCircuitSound(string state)
	{
		EventReference event_ref;
		if (state == "powered")
		{
			event_ref = Sounds.Instance.BuildingPowerOnMigrated;
		}
		else if (state == "overdraw")
		{
			event_ref = Sounds.Instance.ElectricGridOverloadMigrated;
		}
		else
		{
			event_ref = default(EventReference);
			global::Debug.Log("Invalid state for sound in EnergyConsumer.");
		}
		if (!CameraController.Instance.IsAudibleSound(base.transform.GetPosition()))
		{
			return;
		}
		float num;
		if (!this.lastTimeSoundPlayed.TryGetValue(state, out num))
		{
			num = 0f;
		}
		float value = (Time.time - num) / this.soundDecayTime;
		Vector3 position = base.transform.GetPosition();
		position.z = 0f;
		FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(event_ref, CameraController.Instance.GetVerticallyScaledPosition(position, false), 1f);
		instance.setParameterByName("timeSinceLast", value, false);
		KFMOD.EndOneShot(instance);
		this.lastTimeSoundPlayed[state] = Time.time;
	}

	// Token: 0x060062C1 RID: 25281 RVA: 0x000AA765 File Offset: 0x000A8965
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x040046C1 RID: 18113
	[MyCmpReq]
	private Building building;

	// Token: 0x040046C2 RID: 18114
	[MyCmpGet]
	protected Operational operational;

	// Token: 0x040046C3 RID: 18115
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x040046C4 RID: 18116
	[SerializeField]
	public int powerSortOrder;

	// Token: 0x040046C6 RID: 18118
	[Serialize]
	protected float circuitOverloadTime;

	// Token: 0x040046C7 RID: 18119
	public static readonly Operational.Flag PoweredFlag = new Operational.Flag("powered", Operational.Flag.Type.Requirement);

	// Token: 0x040046C8 RID: 18120
	private Dictionary<string, float> lastTimeSoundPlayed = new Dictionary<string, float>();

	// Token: 0x040046C9 RID: 18121
	private float soundDecayTime = 10f;

	// Token: 0x040046CD RID: 18125
	private float _BaseWattageRating;
}
