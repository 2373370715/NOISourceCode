using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x020014EE RID: 5358
public class LogicCircuitNetwork : UtilityNetwork
{
	// Token: 0x06006F5D RID: 28509 RVA: 0x0030097C File Offset: 0x002FEB7C
	public override void AddItem(object item)
	{
		if (item is LogicWire)
		{
			LogicWire logicWire = (LogicWire)item;
			LogicWire.BitDepth maxBitDepth = logicWire.MaxBitDepth;
			List<LogicWire> list = this.wireGroups[(int)maxBitDepth];
			if (list == null)
			{
				list = new List<LogicWire>();
				this.wireGroups[(int)maxBitDepth] = list;
			}
			list.Add(logicWire);
			return;
		}
		if (item is ILogicEventReceiver)
		{
			ILogicEventReceiver item2 = (ILogicEventReceiver)item;
			this.receivers.Add(item2);
			return;
		}
		if (item is ILogicEventSender)
		{
			ILogicEventSender item3 = (ILogicEventSender)item;
			this.senders.Add(item3);
		}
	}

	// Token: 0x06006F5E RID: 28510 RVA: 0x003009FC File Offset: 0x002FEBFC
	public override void RemoveItem(object item)
	{
		if (item is LogicWire)
		{
			LogicWire logicWire = (LogicWire)item;
			this.wireGroups[(int)logicWire.MaxBitDepth].Remove(logicWire);
			return;
		}
		if (item is ILogicEventReceiver)
		{
			ILogicEventReceiver item2 = item as ILogicEventReceiver;
			this.receivers.Remove(item2);
			return;
		}
		if (item is ILogicEventSender)
		{
			ILogicEventSender item3 = (ILogicEventSender)item;
			this.senders.Remove(item3);
		}
	}

	// Token: 0x06006F5F RID: 28511 RVA: 0x000ED6F4 File Offset: 0x000EB8F4
	public override void ConnectItem(object item)
	{
		if (item is ILogicEventReceiver)
		{
			((ILogicEventReceiver)item).OnLogicNetworkConnectionChanged(true);
			return;
		}
		if (item is ILogicEventSender)
		{
			((ILogicEventSender)item).OnLogicNetworkConnectionChanged(true);
		}
	}

	// Token: 0x06006F60 RID: 28512 RVA: 0x000ED71F File Offset: 0x000EB91F
	public override void DisconnectItem(object item)
	{
		if (item is ILogicEventReceiver)
		{
			ILogicEventReceiver logicEventReceiver = item as ILogicEventReceiver;
			logicEventReceiver.ReceiveLogicEvent(0);
			logicEventReceiver.OnLogicNetworkConnectionChanged(false);
			return;
		}
		if (item is ILogicEventSender)
		{
			(item as ILogicEventSender).OnLogicNetworkConnectionChanged(false);
		}
	}

	// Token: 0x06006F61 RID: 28513 RVA: 0x00300A68 File Offset: 0x002FEC68
	public override void Reset(UtilityNetworkGridNode[] grid)
	{
		this.resetting = true;
		this.previousValue = -1;
		this.outputValue = 0;
		for (int i = 0; i < 2; i++)
		{
			List<LogicWire> list = this.wireGroups[i];
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					LogicWire logicWire = list[j];
					if (logicWire != null)
					{
						int num = Grid.PosToCell(logicWire.transform.GetPosition());
						UtilityNetworkGridNode utilityNetworkGridNode = grid[num];
						utilityNetworkGridNode.networkIdx = -1;
						grid[num] = utilityNetworkGridNode;
					}
				}
				list.Clear();
			}
		}
		this.senders.Clear();
		this.receivers.Clear();
		this.resetting = false;
		this.RemoveOverloadedNotification();
	}

	// Token: 0x06006F62 RID: 28514 RVA: 0x00300B1C File Offset: 0x002FED1C
	public void UpdateLogicValue()
	{
		if (this.resetting)
		{
			return;
		}
		this.previousValue = this.outputValue;
		this.outputValue = 0;
		foreach (ILogicEventSender logicEventSender in this.senders)
		{
			logicEventSender.LogicTick();
		}
		foreach (ILogicEventSender logicEventSender2 in this.senders)
		{
			int logicValue = logicEventSender2.GetLogicValue();
			this.outputValue |= logicValue;
		}
	}

	// Token: 0x06006F63 RID: 28515 RVA: 0x00300BD8 File Offset: 0x002FEDD8
	public int GetBitsUsed()
	{
		int result;
		if (this.outputValue > 1)
		{
			result = 4;
		}
		else
		{
			result = 1;
		}
		return result;
	}

	// Token: 0x06006F64 RID: 28516 RVA: 0x000ED751 File Offset: 0x000EB951
	public bool IsBitActive(int bit)
	{
		return (this.OutputValue & 1 << bit) > 0;
	}

	// Token: 0x06006F65 RID: 28517 RVA: 0x000ED763 File Offset: 0x000EB963
	public static bool IsBitActive(int bit, int value)
	{
		return (value & 1 << bit) > 0;
	}

	// Token: 0x06006F66 RID: 28518 RVA: 0x000ED770 File Offset: 0x000EB970
	public static int GetBitValue(int bit, int value)
	{
		return value & 1 << bit;
	}

	// Token: 0x06006F67 RID: 28519 RVA: 0x00300BF8 File Offset: 0x002FEDF8
	public void SendLogicEvents(bool force_send, int id)
	{
		if (this.resetting)
		{
			return;
		}
		if (this.outputValue != this.previousValue || force_send)
		{
			foreach (ILogicEventReceiver logicEventReceiver in this.receivers)
			{
				logicEventReceiver.ReceiveLogicEvent(this.outputValue);
			}
			if (!force_send)
			{
				this.TriggerAudio((this.previousValue >= 0) ? this.previousValue : 0, id);
			}
		}
	}

	// Token: 0x06006F68 RID: 28520 RVA: 0x00300C88 File Offset: 0x002FEE88
	private void TriggerAudio(int old_value, int id)
	{
		SpeedControlScreen instance = SpeedControlScreen.Instance;
		if (old_value != this.outputValue && instance != null && !instance.IsPaused)
		{
			int num = 0;
			GridArea visibleArea = GridVisibleArea.GetVisibleArea();
			List<LogicWire> list = new List<LogicWire>();
			for (int i = 0; i < 2; i++)
			{
				List<LogicWire> list2 = this.wireGroups[i];
				if (list2 != null)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						num++;
						if (visibleArea.Min <= list2[j].transform.GetPosition() && list2[j].transform.GetPosition() <= visibleArea.Max)
						{
							list.Add(list2[j]);
						}
					}
				}
			}
			if (list.Count > 0)
			{
				int index = Mathf.CeilToInt((float)(list.Count / 2));
				if (list[index] != null)
				{
					Vector3 position = list[index].transform.GetPosition();
					position.z = 0f;
					string name = "Logic_Circuit_Toggle";
					LogicCircuitNetwork.LogicSoundPair logicSoundPair = new LogicCircuitNetwork.LogicSoundPair();
					if (!LogicCircuitNetwork.logicSoundRegister.ContainsKey(id))
					{
						LogicCircuitNetwork.logicSoundRegister.Add(id, logicSoundPair);
					}
					else
					{
						logicSoundPair.playedIndex = LogicCircuitNetwork.logicSoundRegister[id].playedIndex;
						logicSoundPair.lastPlayed = LogicCircuitNetwork.logicSoundRegister[id].lastPlayed;
					}
					if (logicSoundPair.playedIndex < 2)
					{
						LogicCircuitNetwork.logicSoundRegister[id].playedIndex = logicSoundPair.playedIndex + 1;
					}
					else
					{
						LogicCircuitNetwork.logicSoundRegister[id].playedIndex = 0;
						LogicCircuitNetwork.logicSoundRegister[id].lastPlayed = Time.time;
					}
					float value = (Time.time - logicSoundPair.lastPlayed) / 3f;
					EventInstance instance2 = KFMOD.BeginOneShot(GlobalAssets.GetSound(name, false), position, 1f);
					instance2.setParameterByName("logic_volumeModifer", value, false);
					instance2.setParameterByName("wireCount", (float)(num % 24), false);
					instance2.setParameterByName("enabled", (float)this.outputValue, false);
					KFMOD.EndOneShot(instance2);
				}
			}
		}
	}

	// Token: 0x06006F69 RID: 28521 RVA: 0x00300EC0 File Offset: 0x002FF0C0
	public void UpdateOverloadTime(float dt, int bits_used)
	{
		bool flag = false;
		List<LogicWire> list = null;
		List<LogicUtilityNetworkLink> list2 = null;
		for (int i = 0; i < 2; i++)
		{
			List<LogicWire> list3 = this.wireGroups[i];
			List<LogicUtilityNetworkLink> list4 = this.relevantBridges[i];
			float num = (float)LogicWire.GetBitDepthAsInt((LogicWire.BitDepth)i);
			if ((float)bits_used > num && ((list4 != null && list4.Count > 0) || (list3 != null && list3.Count > 0)))
			{
				flag = true;
				list = list3;
				list2 = list4;
				break;
			}
		}
		if (list != null)
		{
			list.RemoveAll((LogicWire x) => x == null);
		}
		if (list2 != null)
		{
			list2.RemoveAll((LogicUtilityNetworkLink x) => x == null);
		}
		if (flag)
		{
			this.timeOverloaded += dt;
			if (this.timeOverloaded > 6f)
			{
				this.timeOverloaded = 0f;
				if (this.targetOverloadedWire == null)
				{
					if (list2 != null && list2.Count > 0)
					{
						int index = UnityEngine.Random.Range(0, list2.Count);
						this.targetOverloadedWire = list2[index].gameObject;
					}
					else if (list != null && list.Count > 0)
					{
						int index2 = UnityEngine.Random.Range(0, list.Count);
						this.targetOverloadedWire = list[index2].gameObject;
					}
				}
				if (this.targetOverloadedWire != null)
				{
					this.targetOverloadedWire.Trigger(-794517298, new BuildingHP.DamageSourceInfo
					{
						damage = 1,
						source = BUILDINGS.DAMAGESOURCES.LOGIC_CIRCUIT_OVERLOADED,
						popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.LOGIC_CIRCUIT_OVERLOADED,
						takeDamageEffect = SpawnFXHashes.BuildingLogicOverload,
						fullDamageEffectName = "logic_ribbon_damage_kanim",
						statusItemID = Db.Get().BuildingStatusItems.LogicOverloaded.Id
					});
				}
				if (this.overloadedNotification == null)
				{
					this.timeOverloadNotificationDisplayed = 0f;
					this.overloadedNotification = new Notification(MISC.NOTIFICATIONS.LOGIC_CIRCUIT_OVERLOADED.NAME, NotificationType.BadMinor, null, null, true, 0f, null, null, this.targetOverloadedWire.transform, true, false, false);
					Game.Instance.FindOrAdd<Notifier>().Add(this.overloadedNotification, "");
					return;
				}
			}
		}
		else
		{
			this.timeOverloaded = Mathf.Max(0f, this.timeOverloaded - dt * 0.95f);
			this.timeOverloadNotificationDisplayed += dt;
			if (this.timeOverloadNotificationDisplayed > 5f)
			{
				this.RemoveOverloadedNotification();
			}
		}
	}

	// Token: 0x06006F6A RID: 28522 RVA: 0x000ED77A File Offset: 0x000EB97A
	private void RemoveOverloadedNotification()
	{
		if (this.overloadedNotification != null)
		{
			Game.Instance.FindOrAdd<Notifier>().Remove(this.overloadedNotification);
			this.overloadedNotification = null;
		}
	}

	// Token: 0x06006F6B RID: 28523 RVA: 0x0030113C File Offset: 0x002FF33C
	public void UpdateRelevantBridges(List<LogicUtilityNetworkLink>[] bridgeGroups)
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		for (int i = 0; i < bridgeGroups.Length; i++)
		{
			if (this.relevantBridges[i] != null)
			{
				this.relevantBridges[i].Clear();
			}
			for (int j = 0; j < bridgeGroups[i].Count; j++)
			{
				if (logicCircuitManager.GetNetworkForCell(bridgeGroups[i][j].cell_one) == this || logicCircuitManager.GetNetworkForCell(bridgeGroups[i][j].cell_two) == this)
				{
					if (this.relevantBridges[i] == null)
					{
						this.relevantBridges[i] = new List<LogicUtilityNetworkLink>();
					}
					this.relevantBridges[i].Add(bridgeGroups[i][j]);
				}
			}
		}
	}

	// Token: 0x1700071D RID: 1821
	// (get) Token: 0x06006F6C RID: 28524 RVA: 0x000ED7A0 File Offset: 0x000EB9A0
	public int OutputValue
	{
		get
		{
			return this.outputValue;
		}
	}

	// Token: 0x1700071E RID: 1822
	// (get) Token: 0x06006F6D RID: 28525 RVA: 0x003011F0 File Offset: 0x002FF3F0
	public int WireCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < 2; i++)
			{
				if (this.wireGroups[i] != null)
				{
					num += this.wireGroups[i].Count;
				}
			}
			return num;
		}
	}

	// Token: 0x1700071F RID: 1823
	// (get) Token: 0x06006F6E RID: 28526 RVA: 0x000ED7A8 File Offset: 0x000EB9A8
	public List<ILogicEventSender> Senders
	{
		get
		{
			return this.senders;
		}
	}

	// Token: 0x17000720 RID: 1824
	// (get) Token: 0x06006F6F RID: 28527 RVA: 0x000ED7B0 File Offset: 0x000EB9B0
	public List<ILogicEventReceiver> Receivers
	{
		get
		{
			return this.receivers;
		}
	}

	// Token: 0x040053BD RID: 21437
	private List<LogicWire>[] wireGroups = new List<LogicWire>[2];

	// Token: 0x040053BE RID: 21438
	private List<LogicUtilityNetworkLink>[] relevantBridges = new List<LogicUtilityNetworkLink>[2];

	// Token: 0x040053BF RID: 21439
	private List<ILogicEventReceiver> receivers = new List<ILogicEventReceiver>();

	// Token: 0x040053C0 RID: 21440
	private List<ILogicEventSender> senders = new List<ILogicEventSender>();

	// Token: 0x040053C1 RID: 21441
	private int previousValue = -1;

	// Token: 0x040053C2 RID: 21442
	private int outputValue;

	// Token: 0x040053C3 RID: 21443
	private bool resetting;

	// Token: 0x040053C4 RID: 21444
	public static float logicSoundLastPlayedTime = 0f;

	// Token: 0x040053C5 RID: 21445
	private const float MIN_OVERLOAD_TIME_FOR_DAMAGE = 6f;

	// Token: 0x040053C6 RID: 21446
	private const float MIN_OVERLOAD_NOTIFICATION_DISPLAY_TIME = 5f;

	// Token: 0x040053C7 RID: 21447
	public const int VALID_LOGIC_SIGNAL_MASK = 15;

	// Token: 0x040053C8 RID: 21448
	public const int UNINITIALIZED_LOGIC_STATE = -16;

	// Token: 0x040053C9 RID: 21449
	private GameObject targetOverloadedWire;

	// Token: 0x040053CA RID: 21450
	private float timeOverloaded;

	// Token: 0x040053CB RID: 21451
	private float timeOverloadNotificationDisplayed;

	// Token: 0x040053CC RID: 21452
	private Notification overloadedNotification;

	// Token: 0x040053CD RID: 21453
	public static Dictionary<int, LogicCircuitNetwork.LogicSoundPair> logicSoundRegister = new Dictionary<int, LogicCircuitNetwork.LogicSoundPair>();

	// Token: 0x020014EF RID: 5359
	public class LogicSoundPair
	{
		// Token: 0x040053CE RID: 21454
		public int playedIndex;

		// Token: 0x040053CF RID: 21455
		public float lastPlayed;
	}
}
