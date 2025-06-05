using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020014ED RID: 5357
internal class LogicEventHandler : ILogicEventReceiver, ILogicNetworkConnection, ILogicUIElement, IUniformGridObject
{
	// Token: 0x06006F53 RID: 28499 RVA: 0x000ED689 File Offset: 0x000EB889
	public LogicEventHandler(int cell, Action<int, int> on_value_changed, Action<int, bool> on_connection_changed, LogicPortSpriteType sprite_type)
	{
		this.cell = cell;
		this.onValueChanged = on_value_changed;
		this.onConnectionChanged = on_connection_changed;
		this.spriteType = sprite_type;
	}

	// Token: 0x06006F54 RID: 28500 RVA: 0x0030079C File Offset: 0x002FE99C
	public void ReceiveLogicEvent(int value)
	{
		this.TriggerAudio(value);
		int arg = this.value;
		this.value = value;
		this.onValueChanged(value, arg);
	}

	// Token: 0x1700071C RID: 1820
	// (get) Token: 0x06006F55 RID: 28501 RVA: 0x000ED6AE File Offset: 0x000EB8AE
	public int Value
	{
		get
		{
			return this.value;
		}
	}

	// Token: 0x06006F56 RID: 28502 RVA: 0x000ED6B6 File Offset: 0x000EB8B6
	public int GetLogicUICell()
	{
		return this.cell;
	}

	// Token: 0x06006F57 RID: 28503 RVA: 0x000ED6BE File Offset: 0x000EB8BE
	public LogicPortSpriteType GetLogicPortSpriteType()
	{
		return this.spriteType;
	}

	// Token: 0x06006F58 RID: 28504 RVA: 0x000ED6C6 File Offset: 0x000EB8C6
	public Vector2 PosMin()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06006F59 RID: 28505 RVA: 0x000ED6C6 File Offset: 0x000EB8C6
	public Vector2 PosMax()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x06006F5A RID: 28506 RVA: 0x000ED6B6 File Offset: 0x000EB8B6
	public int GetLogicCell()
	{
		return this.cell;
	}

	// Token: 0x06006F5B RID: 28507 RVA: 0x003007CC File Offset: 0x002FE9CC
	private void TriggerAudio(int new_value)
	{
		LogicCircuitNetwork networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(this.cell);
		SpeedControlScreen instance = SpeedControlScreen.Instance;
		if (networkForCell != null && new_value != this.value && instance != null && !instance.IsPaused)
		{
			if (KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayAutomation) && KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) != 1 && OverlayScreen.Instance.GetMode() != OverlayModes.Logic.ID)
			{
				return;
			}
			string name = "Logic_Building_Toggle";
			if (!CameraController.Instance.IsAudibleSound(Grid.CellToPosCCC(this.cell, Grid.SceneLayer.BuildingFront)))
			{
				return;
			}
			LogicCircuitNetwork.LogicSoundPair logicSoundPair = new LogicCircuitNetwork.LogicSoundPair();
			Dictionary<int, LogicCircuitNetwork.LogicSoundPair> logicSoundRegister = LogicCircuitNetwork.logicSoundRegister;
			int id = networkForCell.id;
			if (!logicSoundRegister.ContainsKey(id))
			{
				logicSoundRegister.Add(id, logicSoundPair);
			}
			else
			{
				logicSoundPair.playedIndex = logicSoundRegister[id].playedIndex;
				logicSoundPair.lastPlayed = logicSoundRegister[id].lastPlayed;
			}
			if (logicSoundPair.playedIndex < 2)
			{
				logicSoundRegister[id].playedIndex = logicSoundPair.playedIndex + 1;
			}
			else
			{
				logicSoundRegister[id].playedIndex = 0;
				logicSoundRegister[id].lastPlayed = Time.time;
			}
			float num = (Time.time - logicSoundPair.lastPlayed) / 3f;
			EventInstance instance2 = KFMOD.BeginOneShot(GlobalAssets.GetSound(name, false), Grid.CellToPos(this.cell), 1f);
			instance2.setParameterByName("logic_volumeModifer", num, false);
			instance2.setParameterByName("wireCount", (float)(networkForCell.WireCount % 24), false);
			instance2.setParameterByName("enabled", (float)new_value, false);
			KFMOD.EndOneShot(instance2);
		}
	}

	// Token: 0x06006F5C RID: 28508 RVA: 0x000ED6D8 File Offset: 0x000EB8D8
	public void OnLogicNetworkConnectionChanged(bool connected)
	{
		if (this.onConnectionChanged != null)
		{
			this.onConnectionChanged(this.cell, connected);
		}
	}

	// Token: 0x040053B8 RID: 21432
	private int cell;

	// Token: 0x040053B9 RID: 21433
	private int value;

	// Token: 0x040053BA RID: 21434
	private Action<int, int> onValueChanged;

	// Token: 0x040053BB RID: 21435
	private Action<int, bool> onConnectionChanged;

	// Token: 0x040053BC RID: 21436
	private LogicPortSpriteType spriteType;
}
