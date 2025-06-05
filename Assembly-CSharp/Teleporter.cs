using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001029 RID: 4137
public class Teleporter : KMonoBehaviour
{
	// Token: 0x170004C5 RID: 1221
	// (get) Token: 0x0600539F RID: 21407 RVA: 0x000DAE56 File Offset: 0x000D9056
	// (set) Token: 0x060053A0 RID: 21408 RVA: 0x000DAE5E File Offset: 0x000D905E
	[Serialize]
	public int teleporterID { get; private set; }

	// Token: 0x060053A1 RID: 21409 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060053A2 RID: 21410 RVA: 0x000DAE67 File Offset: 0x000D9067
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Teleporters.Add(this);
		this.SetTeleporterID(0);
		base.Subscribe<Teleporter>(-801688580, Teleporter.OnLogicValueChangedDelegate);
	}

	// Token: 0x060053A3 RID: 21411 RVA: 0x002871EC File Offset: 0x002853EC
	private void OnLogicValueChanged(object data)
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		List<int> list = new List<int>();
		int num = 0;
		int num2 = Mathf.Min(this.ID_LENGTH, component.inputPorts.Count);
		for (int i = 0; i < num2; i++)
		{
			int logicUICell = component.inputPorts[i].GetLogicUICell();
			LogicCircuitNetwork networkForCell = logicCircuitManager.GetNetworkForCell(logicUICell);
			int item = (networkForCell != null) ? networkForCell.OutputValue : 1;
			list.Add(item);
		}
		foreach (int num3 in list)
		{
			num = (num << 1 | num3);
		}
		this.SetTeleporterID(num);
	}

	// Token: 0x060053A4 RID: 21412 RVA: 0x000DAE92 File Offset: 0x000D9092
	protected override void OnCleanUp()
	{
		Components.Teleporters.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060053A5 RID: 21413 RVA: 0x000DAEA5 File Offset: 0x000D90A5
	public bool HasTeleporterTarget()
	{
		return this.FindTeleportTarget() != null;
	}

	// Token: 0x060053A6 RID: 21414 RVA: 0x000DAEB3 File Offset: 0x000D90B3
	public bool IsValidTeleportTarget(Teleporter from_tele)
	{
		return from_tele.teleporterID == this.teleporterID && this.operational.IsOperational;
	}

	// Token: 0x060053A7 RID: 21415 RVA: 0x002872BC File Offset: 0x002854BC
	public Teleporter FindTeleportTarget()
	{
		List<Teleporter> list = new List<Teleporter>();
		foreach (object obj in Components.Teleporters)
		{
			Teleporter teleporter = (Teleporter)obj;
			if (teleporter.IsValidTeleportTarget(this) && teleporter != this)
			{
				list.Add(teleporter);
			}
		}
		Teleporter result = null;
		if (list.Count > 0)
		{
			result = list.GetRandom<Teleporter>();
		}
		return result;
	}

	// Token: 0x060053A8 RID: 21416 RVA: 0x00287344 File Offset: 0x00285544
	public void SetTeleporterID(int ID)
	{
		this.teleporterID = ID;
		foreach (object obj in Components.Teleporters)
		{
			((Teleporter)obj).Trigger(-1266722732, null);
		}
	}

	// Token: 0x060053A9 RID: 21417 RVA: 0x000DAED0 File Offset: 0x000D90D0
	public void SetTeleportTarget(Teleporter target)
	{
		this.teleportTarget.Set(target);
	}

	// Token: 0x060053AA RID: 21418 RVA: 0x002873A8 File Offset: 0x002855A8
	public void TeleportObjects()
	{
		Teleporter teleporter = this.teleportTarget.Get();
		int widthInCells = base.GetComponent<Building>().Def.WidthInCells;
		int num = base.GetComponent<Building>().Def.HeightInCells - 1;
		Vector3 position = base.transform.GetPosition();
		if (teleporter != null)
		{
			ListPool<ScenePartitionerEntry, Teleporter>.PooledList pooledList = ListPool<ScenePartitionerEntry, Teleporter>.Allocate();
			GameScenePartitioner.Instance.GatherEntries((int)position.x - widthInCells / 2 + 1, (int)position.y - num / 2 + 1, widthInCells, num, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
			int cell = Grid.PosToCell(teleporter);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				GameObject gameObject = (scenePartitionerEntry.obj as Pickupable).gameObject;
				Vector3 vector = gameObject.transform.GetPosition() - position;
				MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
				if (component != null)
				{
					new EmoteChore(component.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_interacts_portal_kanim", Telepad.PortalBirthAnim, null);
				}
				else
				{
					vector += Vector3.up;
				}
				gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move) + vector);
			}
			pooledList.Recycle();
		}
		TeleportalPad.StatesInstance smi = this.teleportTarget.Get().GetSMI<TeleportalPad.StatesInstance>();
		smi.sm.doTeleport.Trigger(smi);
		this.teleportTarget.Set(null);
	}

	// Token: 0x04003B05 RID: 15109
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003B07 RID: 15111
	[Serialize]
	public Ref<Teleporter> teleportTarget = new Ref<Teleporter>();

	// Token: 0x04003B08 RID: 15112
	public int ID_LENGTH = 4;

	// Token: 0x04003B09 RID: 15113
	private static readonly EventSystem.IntraObjectHandler<Teleporter> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Teleporter>(delegate(Teleporter component, object data)
	{
		component.OnLogicValueChanged(data);
	});
}
