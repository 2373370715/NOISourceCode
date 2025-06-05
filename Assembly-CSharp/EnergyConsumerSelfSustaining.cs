using System;
using System.Diagnostics;
using KSerialization;

// Token: 0x020012D0 RID: 4816
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name} {WattsUsed}W")]
public class EnergyConsumerSelfSustaining : EnergyConsumer
{
	// Token: 0x1400001F RID: 31
	// (add) Token: 0x060062C4 RID: 25284 RVA: 0x002C5E08 File Offset: 0x002C4008
	// (remove) Token: 0x060062C5 RID: 25285 RVA: 0x002C5E40 File Offset: 0x002C4040
	public event System.Action OnConnectionChanged;

	// Token: 0x1700061C RID: 1564
	// (get) Token: 0x060062C6 RID: 25286 RVA: 0x000E4E71 File Offset: 0x000E3071
	public override bool IsPowered
	{
		get
		{
			return this.isSustained || this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	// Token: 0x1700061D RID: 1565
	// (get) Token: 0x060062C7 RID: 25287 RVA: 0x000E4E86 File Offset: 0x000E3086
	public bool IsExternallyPowered
	{
		get
		{
			return this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	// Token: 0x060062C8 RID: 25288 RVA: 0x000E4E91 File Offset: 0x000E3091
	public void SetSustained(bool isSustained)
	{
		this.isSustained = isSustained;
	}

	// Token: 0x060062C9 RID: 25289 RVA: 0x002C5E78 File Offset: 0x002C4078
	public override void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
	{
		CircuitManager.ConnectionStatus connectionStatus = this.connectionStatus;
		switch (connection_status)
		{
		case CircuitManager.ConnectionStatus.NotConnected:
			this.connectionStatus = CircuitManager.ConnectionStatus.NotConnected;
			break;
		case CircuitManager.ConnectionStatus.Unpowered:
			if (this.connectionStatus == CircuitManager.ConnectionStatus.Powered && base.GetComponent<Battery>() == null)
			{
				this.connectionStatus = CircuitManager.ConnectionStatus.Unpowered;
			}
			break;
		case CircuitManager.ConnectionStatus.Powered:
			if (this.connectionStatus != CircuitManager.ConnectionStatus.Powered)
			{
				this.connectionStatus = CircuitManager.ConnectionStatus.Powered;
			}
			break;
		}
		this.UpdatePoweredStatus();
		if (connectionStatus != this.connectionStatus && this.OnConnectionChanged != null)
		{
			this.OnConnectionChanged();
		}
	}

	// Token: 0x060062CA RID: 25290 RVA: 0x000E4E9A File Offset: 0x000E309A
	public void UpdatePoweredStatus()
	{
		this.operational.SetFlag(EnergyConsumer.PoweredFlag, this.IsPowered);
	}

	// Token: 0x040046CE RID: 18126
	private bool isSustained;

	// Token: 0x040046CF RID: 18127
	private CircuitManager.ConnectionStatus connectionStatus;
}
