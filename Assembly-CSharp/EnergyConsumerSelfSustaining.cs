using System;
using System.Diagnostics;
using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name} {WattsUsed}W")]
public class EnergyConsumerSelfSustaining : EnergyConsumer
{
add) Token: 0x060062C4 RID: 25284 RVA: 0x002C5E08 File Offset: 0x002C4008
remove) Token: 0x060062C5 RID: 25285 RVA: 0x002C5E40 File Offset: 0x002C4040
	public event System.Action OnConnectionChanged;

	public override bool IsPowered
	{
		get
		{
			return this.isSustained || this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	public bool IsExternallyPowered
	{
		get
		{
			return this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	public void SetSustained(bool isSustained)
	{
		this.isSustained = isSustained;
	}

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

	public void UpdatePoweredStatus()
	{
		this.operational.SetFlag(EnergyConsumer.PoweredFlag, this.IsPowered);
	}

	private bool isSustained;

	private CircuitManager.ConnectionStatus connectionStatus;
}
