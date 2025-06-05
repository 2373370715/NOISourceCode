using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A76 RID: 2678
public class EntityCellVisualizer : KMonoBehaviour
{
	// Token: 0x170001EA RID: 490
	// (get) Token: 0x060030A8 RID: 12456 RVA: 0x000C415D File Offset: 0x000C235D
	public BuildingCellVisualizerResources Resources
	{
		get
		{
			return BuildingCellVisualizerResources.Instance();
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x060030A9 RID: 12457 RVA: 0x000C1501 File Offset: 0x000BF701
	protected int CenterCell
	{
		get
		{
			return Grid.PosToCell(this);
		}
	}

	// Token: 0x060030AA RID: 12458 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void DefinePorts()
	{
	}

	// Token: 0x060030AB RID: 12459 RVA: 0x000C4164 File Offset: 0x000C2364
	protected override void OnPrefabInit()
	{
		this.LoadDiseaseIcon();
		this.DefinePorts();
	}

	// Token: 0x060030AC RID: 12460 RVA: 0x000C4172 File Offset: 0x000C2372
	public void ConnectedEventWithDelay(float delay, int connectionCount, int cell, string soundName)
	{
		base.StartCoroutine(this.ConnectedDelay(delay, connectionCount, cell, soundName));
	}

	// Token: 0x060030AD RID: 12461 RVA: 0x000C4186 File Offset: 0x000C2386
	private IEnumerator ConnectedDelay(float delay, int connectionCount, int cell, string soundName)
	{
		float startTime = Time.realtimeSinceStartup;
		float currentTime = startTime;
		while (currentTime < startTime + delay)
		{
			currentTime += Time.unscaledDeltaTime;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.ConnectedEvent(cell);
		string sound = GlobalAssets.GetSound(soundName, false);
		if (sound != null)
		{
			Vector3 position = base.transform.GetPosition();
			position.z = 0f;
			EventInstance instance = SoundEvent.BeginOneShot(sound, position, 1f, false);
			instance.setParameterByName("connectedCount", (float)connectionCount, false);
			SoundEvent.EndOneShot(instance);
		}
		yield break;
	}

	// Token: 0x060030AE RID: 12462 RVA: 0x0020A290 File Offset: 0x00208490
	private int ComputeCell(CellOffset cellOffset)
	{
		CellOffset offset = cellOffset;
		if (this.rotatable != null)
		{
			offset = this.rotatable.GetRotatedCellOffset(cellOffset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.gameObject), offset);
	}

	// Token: 0x060030AF RID: 12463 RVA: 0x0020A2CC File Offset: 0x002084CC
	public void ConnectedEvent(int cell)
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (this.ComputeCell(portEntry.cellOffset) == cell && portEntry.visualizer != null)
			{
				SizePulse pulse = portEntry.visualizer.AddComponent<SizePulse>();
				pulse.speed = 20f;
				pulse.multiplier = 0.75f;
				pulse.updateWhenPaused = true;
				SizePulse pulse2 = pulse;
				pulse2.onComplete = (System.Action)Delegate.Combine(pulse2.onComplete, new System.Action(delegate()
				{
					UnityEngine.Object.Destroy(pulse);
				}));
			}
		}
	}

	// Token: 0x060030B0 RID: 12464 RVA: 0x000C41B2 File Offset: 0x000C23B2
	public virtual void AddPort(EntityCellVisualizer.Ports type, CellOffset cell)
	{
		this.AddPort(type, cell, Color.white);
	}

	// Token: 0x060030B1 RID: 12465 RVA: 0x000C41C1 File Offset: 0x000C23C1
	public virtual void AddPort(EntityCellVisualizer.Ports type, CellOffset cell, Color tint)
	{
		this.AddPort(type, cell, tint, tint, 1.5f, false);
	}

	// Token: 0x060030B2 RID: 12466 RVA: 0x000C41D3 File Offset: 0x000C23D3
	public virtual void AddPort(EntityCellVisualizer.Ports type, CellOffset cell, Color connectedTint, Color disconnectedTint, float scale = 1.5f, bool hideBG = false)
	{
		this.ports.Add(new EntityCellVisualizer.PortEntry(type, cell, connectedTint, disconnectedTint, scale, hideBG));
		this.addedPorts |= type;
	}

	// Token: 0x060030B3 RID: 12467 RVA: 0x0020A3A8 File Offset: 0x002085A8
	protected override void OnCleanUp()
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (portEntry.visualizer != null)
			{
				UnityEngine.Object.Destroy(portEntry.visualizer);
			}
		}
		GameObject[] array = new GameObject[]
		{
			this.switchVisualizer,
			this.wireVisualizerAlpha,
			this.wireVisualizerBeta
		};
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i]);
		}
		base.OnCleanUp();
	}

	// Token: 0x060030B4 RID: 12468 RVA: 0x000C41FC File Offset: 0x000C23FC
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (this.icons == null)
		{
			this.icons = new Dictionary<GameObject, Image>();
		}
		Components.EntityCellVisualizers.Add(this);
	}

	// Token: 0x060030B5 RID: 12469 RVA: 0x000C4222 File Offset: 0x000C2422
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		Components.EntityCellVisualizers.Remove(this);
	}

	// Token: 0x060030B6 RID: 12470 RVA: 0x0020A44C File Offset: 0x0020864C
	public void DrawIcons(HashedString mode)
	{
		EntityCellVisualizer.Ports ports = (EntityCellVisualizer.Ports)0;
		if (base.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
		{
			ports = (EntityCellVisualizer.Ports)0;
		}
		else if (mode == OverlayModes.Power.ID)
		{
			ports = (EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut);
		}
		else if (mode == OverlayModes.GasConduits.ID)
		{
			ports = (EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut);
		}
		else if (mode == OverlayModes.LiquidConduits.ID)
		{
			ports = (EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut);
		}
		else if (mode == OverlayModes.SolidConveyor.ID)
		{
			ports = (EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut);
		}
		else if (mode == OverlayModes.Radiation.ID)
		{
			ports = (EntityCellVisualizer.Ports.HighEnergyParticleIn | EntityCellVisualizer.Ports.HighEnergyParticleOut);
		}
		else if (mode == OverlayModes.Disease.ID)
		{
			ports = (EntityCellVisualizer.Ports.DiseaseIn | EntityCellVisualizer.Ports.DiseaseOut);
		}
		else if (mode == OverlayModes.Temperature.ID || mode == OverlayModes.HeatFlow.ID)
		{
			ports = (EntityCellVisualizer.Ports.HeatSource | EntityCellVisualizer.Ports.HeatSink);
		}
		bool flag = false;
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if ((portEntry.type & ports) == portEntry.type)
			{
				this.DrawUtilityIcon(portEntry);
				flag = true;
			}
			else if (portEntry.visualizer != null && portEntry.visualizer.activeInHierarchy)
			{
				portEntry.visualizer.SetActive(false);
			}
		}
		if (mode == OverlayModes.Power.ID)
		{
			if (!flag)
			{
				Switch component = base.GetComponent<Switch>();
				if (component != null)
				{
					int cell = Grid.PosToCell(base.transform.GetPosition());
					Color32 c = component.IsHandlerOn() ? this.Resources.switchColor : this.Resources.switchOffColor;
					this.DrawUtilityIcon(cell, this.Resources.switchIcon, ref this.switchVisualizer, c, 1f, false);
					return;
				}
				WireUtilityNetworkLink component2 = base.GetComponent<WireUtilityNetworkLink>();
				if (component2 != null)
				{
					int cell2;
					int cell3;
					component2.GetCells(out cell2, out cell3);
					this.DrawUtilityIcon(cell2, (Game.Instance.circuitManager.GetCircuitID(cell2) == ushort.MaxValue) ? this.Resources.electricityBridgeIcon : this.Resources.electricityConnectedIcon, ref this.wireVisualizerAlpha, this.Resources.electricityInputColor, 1f, false);
					this.DrawUtilityIcon(cell3, (Game.Instance.circuitManager.GetCircuitID(cell3) == ushort.MaxValue) ? this.Resources.electricityBridgeIcon : this.Resources.electricityConnectedIcon, ref this.wireVisualizerBeta, this.Resources.electricityInputColor, 1f, false);
					return;
				}
			}
		}
		else
		{
			foreach (GameObject gameObject in new GameObject[]
			{
				this.switchVisualizer,
				this.wireVisualizerAlpha,
				this.wireVisualizerBeta
			})
			{
				if (gameObject != null && gameObject.activeInHierarchy)
				{
					gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x060030B7 RID: 12471 RVA: 0x0020A730 File Offset: 0x00208930
	private Sprite GetSpriteForPortType(EntityCellVisualizer.Ports type, bool connected)
	{
		if (type <= EntityCellVisualizer.Ports.SolidOut)
		{
			if (type <= EntityCellVisualizer.Ports.LiquidIn)
			{
				switch (type)
				{
				case EntityCellVisualizer.Ports.PowerIn:
					if (!connected)
					{
						return this.Resources.electricityInputIcon;
					}
					return this.Resources.electricityBridgeConnectedIcon;
				case EntityCellVisualizer.Ports.PowerOut:
					if (!connected)
					{
						return this.Resources.electricityOutputIcon;
					}
					return this.Resources.electricityBridgeConnectedIcon;
				case EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut:
					break;
				case EntityCellVisualizer.Ports.GasIn:
					return this.Resources.gasInputIcon;
				default:
					if (type == EntityCellVisualizer.Ports.GasOut)
					{
						return this.Resources.gasOutputIcon;
					}
					if (type == EntityCellVisualizer.Ports.LiquidIn)
					{
						return this.Resources.liquidInputIcon;
					}
					break;
				}
			}
			else
			{
				if (type == EntityCellVisualizer.Ports.LiquidOut)
				{
					return this.Resources.liquidOutputIcon;
				}
				if (type == EntityCellVisualizer.Ports.SolidIn)
				{
					return this.Resources.liquidInputIcon;
				}
				if (type == EntityCellVisualizer.Ports.SolidOut)
				{
					return this.Resources.liquidOutputIcon;
				}
			}
		}
		else if (type <= EntityCellVisualizer.Ports.DiseaseIn)
		{
			if (type == EntityCellVisualizer.Ports.HighEnergyParticleIn)
			{
				return this.Resources.highEnergyParticleInputIcon;
			}
			if (type == EntityCellVisualizer.Ports.HighEnergyParticleOut)
			{
				return this.GetIconForHighEnergyOutput();
			}
			if (type == EntityCellVisualizer.Ports.DiseaseIn)
			{
				return this.diseaseSourceSprite;
			}
		}
		else
		{
			if (type == EntityCellVisualizer.Ports.DiseaseOut)
			{
				return this.diseaseSourceSprite;
			}
			if (type == EntityCellVisualizer.Ports.HeatSource)
			{
				return this.Resources.heatSourceIcon;
			}
			if (type == EntityCellVisualizer.Ports.HeatSink)
			{
				return this.Resources.heatSinkIcon;
			}
		}
		return null;
	}

	// Token: 0x060030B8 RID: 12472 RVA: 0x0020A8A4 File Offset: 0x00208AA4
	protected virtual void DrawUtilityIcon(EntityCellVisualizer.PortEntry port)
	{
		int cell = this.ComputeCell(port.cellOffset);
		bool flag = true;
		bool connected = true;
		EntityCellVisualizer.Ports type = port.type;
		if (type <= EntityCellVisualizer.Ports.GasOut)
		{
			if (type - EntityCellVisualizer.Ports.PowerIn > 1)
			{
				if (type == EntityCellVisualizer.Ports.GasIn || type == EntityCellVisualizer.Ports.GasOut)
				{
					flag = (null != Grid.Objects[cell, 12]);
				}
			}
			else
			{
				bool flag2 = base.GetComponent<Building>() as BuildingPreview != null;
				BuildingEnabledButton component = base.GetComponent<BuildingEnabledButton>();
				connected = (!flag2 && Game.Instance.circuitManager.GetCircuitID(cell) != ushort.MaxValue);
				flag = (flag2 || component == null || component.IsEnabled);
			}
		}
		else if (type <= EntityCellVisualizer.Ports.LiquidOut)
		{
			if (type == EntityCellVisualizer.Ports.LiquidIn || type == EntityCellVisualizer.Ports.LiquidOut)
			{
				flag = (null != Grid.Objects[cell, 16]);
			}
		}
		else if (type == EntityCellVisualizer.Ports.SolidIn || type == EntityCellVisualizer.Ports.SolidOut)
		{
			flag = (null != Grid.Objects[cell, 20]);
		}
		this.DrawUtilityIcon(cell, this.GetSpriteForPortType(port.type, connected), ref port.visualizer, flag ? port.connectedTint : port.disconnectedTint, port.scale, port.hideBG);
	}

	// Token: 0x060030B9 RID: 12473 RVA: 0x0020A9E0 File Offset: 0x00208BE0
	protected virtual void LoadDiseaseIcon()
	{
		DiseaseVisualization.Info info = Assets.instance.DiseaseVisualization.GetInfo(this.DiseaseCellVisName);
		if (info.name != null)
		{
			this.diseaseSourceSprite = Assets.instance.DiseaseVisualization.overlaySprite;
			this.diseaseSourceColour = GlobalAssets.Instance.colorSet.GetColorByName(info.overlayColourName);
		}
	}

	// Token: 0x060030BA RID: 12474 RVA: 0x0020AA40 File Offset: 0x00208C40
	protected virtual Sprite GetIconForHighEnergyOutput()
	{
		IHighEnergyParticleDirection component = base.GetComponent<IHighEnergyParticleDirection>();
		Sprite result = this.Resources.highEnergyParticleOutputIcons[0];
		if (component != null)
		{
			int directionIndex = EightDirectionUtil.GetDirectionIndex(component.Direction);
			result = this.Resources.highEnergyParticleOutputIcons[directionIndex];
		}
		return result;
	}

	// Token: 0x060030BB RID: 12475 RVA: 0x0020AA80 File Offset: 0x00208C80
	private void DrawUtilityIcon(int cell, Sprite icon_img, ref GameObject visualizerObj, Color tint, float scaleMultiplier = 1.5f, bool hideBG = false)
	{
		Vector3 position = Grid.CellToPosCCC(cell, Grid.SceneLayer.Building);
		if (visualizerObj == null)
		{
			visualizerObj = global::Util.KInstantiate(Assets.UIPrefabs.ResourceVisualizer, GameScreenManager.Instance.worldSpaceCanvas, null);
			visualizerObj.transform.SetAsFirstSibling();
			this.icons.Add(visualizerObj, visualizerObj.transform.GetChild(0).GetComponent<Image>());
		}
		if (!visualizerObj.gameObject.activeInHierarchy)
		{
			visualizerObj.gameObject.SetActive(true);
		}
		visualizerObj.GetComponent<Image>().enabled = !hideBG;
		this.icons[visualizerObj].raycastTarget = this.enableRaycast;
		this.icons[visualizerObj].sprite = icon_img;
		visualizerObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = tint;
		visualizerObj.transform.SetPosition(position);
		if (visualizerObj.GetComponent<SizePulse>() == null)
		{
			visualizerObj.transform.localScale = Vector3.one * scaleMultiplier;
		}
	}

	// Token: 0x060030BC RID: 12476 RVA: 0x0020AB94 File Offset: 0x00208D94
	public Image GetPowerOutputIcon()
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (portEntry.type == EntityCellVisualizer.Ports.PowerOut)
			{
				return (portEntry.visualizer != null) ? portEntry.visualizer.transform.GetChild(0).GetComponent<Image>() : null;
			}
		}
		return null;
	}

	// Token: 0x060030BD RID: 12477 RVA: 0x0020AC18 File Offset: 0x00208E18
	public Image GetPowerInputIcon()
	{
		foreach (EntityCellVisualizer.PortEntry portEntry in this.ports)
		{
			if (portEntry.type == EntityCellVisualizer.Ports.PowerIn)
			{
				return (portEntry.visualizer != null) ? portEntry.visualizer.transform.GetChild(0).GetComponent<Image>() : null;
			}
		}
		return null;
	}

	// Token: 0x04002160 RID: 8544
	protected List<EntityCellVisualizer.PortEntry> ports = new List<EntityCellVisualizer.PortEntry>();

	// Token: 0x04002161 RID: 8545
	public EntityCellVisualizer.Ports addedPorts;

	// Token: 0x04002162 RID: 8546
	private GameObject switchVisualizer;

	// Token: 0x04002163 RID: 8547
	private GameObject wireVisualizerAlpha;

	// Token: 0x04002164 RID: 8548
	private GameObject wireVisualizerBeta;

	// Token: 0x04002165 RID: 8549
	public const EntityCellVisualizer.Ports HEAT_PORTS = EntityCellVisualizer.Ports.HeatSource | EntityCellVisualizer.Ports.HeatSink;

	// Token: 0x04002166 RID: 8550
	public const EntityCellVisualizer.Ports POWER_PORTS = EntityCellVisualizer.Ports.PowerIn | EntityCellVisualizer.Ports.PowerOut;

	// Token: 0x04002167 RID: 8551
	public const EntityCellVisualizer.Ports GAS_PORTS = EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut;

	// Token: 0x04002168 RID: 8552
	public const EntityCellVisualizer.Ports LIQUID_PORTS = EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut;

	// Token: 0x04002169 RID: 8553
	public const EntityCellVisualizer.Ports SOLID_PORTS = EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut;

	// Token: 0x0400216A RID: 8554
	public const EntityCellVisualizer.Ports ENERGY_PARTICLES_PORTS = EntityCellVisualizer.Ports.HighEnergyParticleIn | EntityCellVisualizer.Ports.HighEnergyParticleOut;

	// Token: 0x0400216B RID: 8555
	public const EntityCellVisualizer.Ports DISEASE_PORTS = EntityCellVisualizer.Ports.DiseaseIn | EntityCellVisualizer.Ports.DiseaseOut;

	// Token: 0x0400216C RID: 8556
	public const EntityCellVisualizer.Ports MATTER_PORTS = EntityCellVisualizer.Ports.GasIn | EntityCellVisualizer.Ports.GasOut | EntityCellVisualizer.Ports.LiquidIn | EntityCellVisualizer.Ports.LiquidOut | EntityCellVisualizer.Ports.SolidIn | EntityCellVisualizer.Ports.SolidOut;

	// Token: 0x0400216D RID: 8557
	protected Sprite diseaseSourceSprite;

	// Token: 0x0400216E RID: 8558
	protected Color32 diseaseSourceColour;

	// Token: 0x0400216F RID: 8559
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04002170 RID: 8560
	protected bool enableRaycast = true;

	// Token: 0x04002171 RID: 8561
	protected Dictionary<GameObject, Image> icons;

	// Token: 0x04002172 RID: 8562
	public string DiseaseCellVisName = DUPLICANTSTATS.STANDARD.Secretions.PEE_DISEASE;

	// Token: 0x02000A77 RID: 2679
	[Flags]
	public enum Ports
	{
		// Token: 0x04002174 RID: 8564
		PowerIn = 1,
		// Token: 0x04002175 RID: 8565
		PowerOut = 2,
		// Token: 0x04002176 RID: 8566
		GasIn = 4,
		// Token: 0x04002177 RID: 8567
		GasOut = 8,
		// Token: 0x04002178 RID: 8568
		LiquidIn = 16,
		// Token: 0x04002179 RID: 8569
		LiquidOut = 32,
		// Token: 0x0400217A RID: 8570
		SolidIn = 64,
		// Token: 0x0400217B RID: 8571
		SolidOut = 128,
		// Token: 0x0400217C RID: 8572
		HighEnergyParticleIn = 256,
		// Token: 0x0400217D RID: 8573
		HighEnergyParticleOut = 512,
		// Token: 0x0400217E RID: 8574
		DiseaseIn = 1024,
		// Token: 0x0400217F RID: 8575
		DiseaseOut = 2048,
		// Token: 0x04002180 RID: 8576
		HeatSource = 4096,
		// Token: 0x04002181 RID: 8577
		HeatSink = 8192
	}

	// Token: 0x02000A78 RID: 2680
	protected class PortEntry
	{
		// Token: 0x060030BF RID: 12479 RVA: 0x000C4264 File Offset: 0x000C2464
		public PortEntry(EntityCellVisualizer.Ports type, CellOffset cellOffset, Color connectedTint, Color disconnectedTint, float scale, bool hideBG)
		{
			this.type = type;
			this.cellOffset = cellOffset;
			this.visualizer = null;
			this.connectedTint = connectedTint;
			this.disconnectedTint = disconnectedTint;
			this.scale = scale;
			this.hideBG = hideBG;
		}

		// Token: 0x04002182 RID: 8578
		public EntityCellVisualizer.Ports type;

		// Token: 0x04002183 RID: 8579
		public CellOffset cellOffset;

		// Token: 0x04002184 RID: 8580
		public GameObject visualizer;

		// Token: 0x04002185 RID: 8581
		public Color connectedTint;

		// Token: 0x04002186 RID: 8582
		public Color disconnectedTint;

		// Token: 0x04002187 RID: 8583
		public float scale;

		// Token: 0x04002188 RID: 8584
		public bool hideBG;
	}
}
