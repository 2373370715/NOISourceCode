using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000D08 RID: 3336
[SerializationConfig(MemberSerialization.OptIn)]
public class CircuitSwitch : Switch, IPlayerControlledToggle, ISim33ms
{
	// Token: 0x06003FF5 RID: 16373 RVA: 0x000CDFAE File Offset: 0x000CC1AE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<CircuitSwitch>(-905833192, CircuitSwitch.OnCopySettingsDelegate);
	}

	// Token: 0x06003FF6 RID: 16374 RVA: 0x0024760C File Offset: 0x0024580C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.CircuitOnToggle;
		int cell = Grid.PosToCell(base.transform.GetPosition());
		GameObject gameObject = Grid.Objects[cell, (int)this.objectLayer];
		Wire wire = (gameObject != null) ? gameObject.GetComponent<Wire>() : null;
		if (wire == null)
		{
			this.wireConnectedGUID = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, null);
		}
		this.AttachWire(wire);
		this.wasOn = this.switchedOn;
		this.UpdateCircuit(true);
		base.GetComponent<KBatchedAnimController>().Play(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06003FF7 RID: 16375 RVA: 0x002476DC File Offset: 0x002458DC
	protected override void OnCleanUp()
	{
		if (this.attachedWire != null)
		{
			this.UnsubscribeFromWire(this.attachedWire);
		}
		bool switchedOn = this.switchedOn;
		this.switchedOn = true;
		this.UpdateCircuit(false);
		this.switchedOn = switchedOn;
	}

	// Token: 0x06003FF8 RID: 16376 RVA: 0x00247720 File Offset: 0x00245920
	private void OnCopySettings(object data)
	{
		CircuitSwitch component = ((GameObject)data).GetComponent<CircuitSwitch>();
		if (component != null)
		{
			this.switchedOn = component.switchedOn;
			this.UpdateCircuit(true);
		}
	}

	// Token: 0x06003FF9 RID: 16377 RVA: 0x00247758 File Offset: 0x00245958
	public bool IsConnected()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		GameObject gameObject = Grid.Objects[cell, (int)this.objectLayer];
		return gameObject != null && gameObject.GetComponent<IDisconnectable>() != null;
	}

	// Token: 0x06003FFA RID: 16378 RVA: 0x000CDFC7 File Offset: 0x000CC1C7
	private void CircuitOnToggle(bool on)
	{
		this.UpdateCircuit(true);
	}

	// Token: 0x06003FFB RID: 16379 RVA: 0x0024779C File Offset: 0x0024599C
	public void AttachWire(Wire wire)
	{
		if (wire == this.attachedWire)
		{
			return;
		}
		if (this.attachedWire != null)
		{
			this.UnsubscribeFromWire(this.attachedWire);
		}
		this.attachedWire = wire;
		if (this.attachedWire != null)
		{
			this.SubscribeToWire(this.attachedWire);
			this.UpdateCircuit(true);
			this.wireConnectedGUID = base.GetComponent<KSelectable>().RemoveStatusItem(this.wireConnectedGUID, false);
			return;
		}
		if (this.wireConnectedGUID == Guid.Empty)
		{
			this.wireConnectedGUID = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, null);
		}
	}

	// Token: 0x06003FFC RID: 16380 RVA: 0x000CDFD0 File Offset: 0x000CC1D0
	private void OnWireDestroyed(object data)
	{
		if (this.attachedWire != null)
		{
			this.attachedWire.Unsubscribe(1969584890, new Action<object>(this.OnWireDestroyed));
		}
	}

	// Token: 0x06003FFD RID: 16381 RVA: 0x000CDFC7 File Offset: 0x000CC1C7
	private void OnWireStateChanged(object data)
	{
		this.UpdateCircuit(true);
	}

	// Token: 0x06003FFE RID: 16382 RVA: 0x00247848 File Offset: 0x00245A48
	private void SubscribeToWire(Wire wire)
	{
		wire.Subscribe(1969584890, new Action<object>(this.OnWireDestroyed));
		wire.Subscribe(-1735440190, new Action<object>(this.OnWireStateChanged));
		wire.Subscribe(774203113, new Action<object>(this.OnWireStateChanged));
	}

	// Token: 0x06003FFF RID: 16383 RVA: 0x002478A0 File Offset: 0x00245AA0
	private void UnsubscribeFromWire(Wire wire)
	{
		wire.Unsubscribe(1969584890, new Action<object>(this.OnWireDestroyed));
		wire.Unsubscribe(-1735440190, new Action<object>(this.OnWireStateChanged));
		wire.Unsubscribe(774203113, new Action<object>(this.OnWireStateChanged));
	}

	// Token: 0x06004000 RID: 16384 RVA: 0x002478F4 File Offset: 0x00245AF4
	private void UpdateCircuit(bool should_update_anim = true)
	{
		if (this.attachedWire != null)
		{
			if (this.switchedOn)
			{
				this.attachedWire.Connect();
			}
			else
			{
				this.attachedWire.Disconnect();
			}
		}
		if (should_update_anim && this.wasOn != this.switchedOn)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
			Game.Instance.userMenu.Refresh(base.gameObject);
		}
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06004001 RID: 16385 RVA: 0x000CDFFC File Offset: 0x000CC1FC
	public void Sim33ms(float dt)
	{
		if (this.ToggleRequested)
		{
			this.Toggle();
			this.ToggleRequested = false;
			this.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
	}

	// Token: 0x06004002 RID: 16386 RVA: 0x000CE030 File Offset: 0x000CC230
	public void ToggledByPlayer()
	{
		this.Toggle();
	}

	// Token: 0x06004003 RID: 16387 RVA: 0x000CE038 File Offset: 0x000CC238
	public bool ToggledOn()
	{
		return this.switchedOn;
	}

	// Token: 0x06004004 RID: 16388 RVA: 0x000CE040 File Offset: 0x000CC240
	public KSelectable GetSelectable()
	{
		return base.GetComponent<KSelectable>();
	}

	// Token: 0x1700030B RID: 779
	// (get) Token: 0x06004005 RID: 16389 RVA: 0x000CE048 File Offset: 0x000CC248
	public string SideScreenTitleKey
	{
		get
		{
			return "STRINGS.BUILDINGS.PREFABS.SWITCH.SIDESCREEN_TITLE";
		}
	}

	// Token: 0x1700030C RID: 780
	// (get) Token: 0x06004006 RID: 16390 RVA: 0x000CE04F File Offset: 0x000CC24F
	// (set) Token: 0x06004007 RID: 16391 RVA: 0x000CE057 File Offset: 0x000CC257
	public bool ToggleRequested { get; set; }

	// Token: 0x04002C3C RID: 11324
	[SerializeField]
	public ObjectLayer objectLayer;

	// Token: 0x04002C3D RID: 11325
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002C3E RID: 11326
	private static readonly EventSystem.IntraObjectHandler<CircuitSwitch> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CircuitSwitch>(delegate(CircuitSwitch component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04002C3F RID: 11327
	private Wire attachedWire;

	// Token: 0x04002C40 RID: 11328
	private Guid wireConnectedGUID;

	// Token: 0x04002C41 RID: 11329
	private bool wasOn;
}
