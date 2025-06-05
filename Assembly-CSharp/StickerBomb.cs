using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000FF3 RID: 4083
public class StickerBomb : StateMachineComponent<StickerBomb.StatesInstance>
{
	// Token: 0x06005238 RID: 21048 RVA: 0x00282AB0 File Offset: 0x00280CB0
	protected override void OnSpawn()
	{
		if (this.stickerName.IsNullOrWhiteSpace())
		{
			global::Debug.LogError("Missing sticker db entry for " + this.stickerType);
		}
		else
		{
			DbStickerBomb dbStickerBomb = Db.GetStickerBombs().Get(this.stickerName);
			base.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[]
			{
				dbStickerBomb.animFile
			});
		}
		this.cellOffsets = StickerBomb.BuildCellOffsets(base.transform.GetPosition());
		base.smi.destroyTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.STICKER_DURATION;
		base.smi.StartSM();
		Extents extents = base.GetComponent<OccupyArea>().GetExtents();
		Extents extents2 = new Extents(extents.x - 1, extents.y - 1, extents.width + 2, extents.height + 2);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("StickerBomb.OnSpawn", base.gameObject, extents2, GameScenePartitioner.Instance.objectLayers[2], new Action<object>(this.OnFoundationCellChanged));
		base.OnSpawn();
	}

	// Token: 0x06005239 RID: 21049 RVA: 0x00282BB8 File Offset: 0x00280DB8
	[OnDeserialized]
	public void OnDeserialized()
	{
		if (this.stickerName.IsNullOrWhiteSpace() && !this.stickerType.IsNullOrWhiteSpace())
		{
			string[] array = this.stickerType.Split('_', StringSplitOptions.None);
			if (array.Length == 2)
			{
				this.stickerName = array[1];
			}
		}
	}

	// Token: 0x0600523A RID: 21050 RVA: 0x000D9FAE File Offset: 0x000D81AE
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x0600523B RID: 21051 RVA: 0x000D9FC6 File Offset: 0x000D81C6
	private void OnFoundationCellChanged(object data)
	{
		if (!StickerBomb.CanPlaceSticker(this.cellOffsets))
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x0600523C RID: 21052 RVA: 0x00282C00 File Offset: 0x00280E00
	public static List<int> BuildCellOffsets(Vector3 position)
	{
		List<int> list = new List<int>();
		bool flag = position.x % 1f < 0.5f;
		bool flag2 = position.y % 1f > 0.5f;
		int num = Grid.PosToCell(position);
		list.Add(num);
		if (flag)
		{
			list.Add(Grid.CellLeft(num));
			if (flag2)
			{
				list.Add(Grid.CellAbove(num));
				list.Add(Grid.CellUpLeft(num));
			}
			else
			{
				list.Add(Grid.CellBelow(num));
				list.Add(Grid.CellDownLeft(num));
			}
		}
		else
		{
			list.Add(Grid.CellRight(num));
			if (flag2)
			{
				list.Add(Grid.CellAbove(num));
				list.Add(Grid.CellUpRight(num));
			}
			else
			{
				list.Add(Grid.CellBelow(num));
				list.Add(Grid.CellDownRight(num));
			}
		}
		return list;
	}

	// Token: 0x0600523D RID: 21053 RVA: 0x00282CD0 File Offset: 0x00280ED0
	public static bool CanPlaceSticker(List<int> offsets)
	{
		using (List<int>.Enumerator enumerator = offsets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (Grid.IsCellOpenToSpace(enumerator.Current))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600523E RID: 21054 RVA: 0x00282D24 File Offset: 0x00280F24
	public void SetStickerType(string newStickerType)
	{
		if (newStickerType == null)
		{
			newStickerType = "sticker";
		}
		DbStickerBomb randomSticker = Db.GetStickerBombs().GetRandomSticker();
		this.stickerName = randomSticker.Id;
		this.stickerType = string.Format("{0}_{1}", newStickerType, randomSticker.Id);
		base.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[]
		{
			randomSticker.animFile
		});
	}

	// Token: 0x04003A15 RID: 14869
	[Serialize]
	public string stickerType;

	// Token: 0x04003A16 RID: 14870
	[Serialize]
	public string stickerName;

	// Token: 0x04003A17 RID: 14871
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003A18 RID: 14872
	private List<int> cellOffsets;

	// Token: 0x02000FF4 RID: 4084
	public class StatesInstance : GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.GameInstance
	{
		// Token: 0x06005240 RID: 21056 RVA: 0x000D9FE8 File Offset: 0x000D81E8
		public StatesInstance(StickerBomb master) : base(master)
		{
		}

		// Token: 0x06005241 RID: 21057 RVA: 0x000D9FF1 File Offset: 0x000D81F1
		public string GetStickerAnim(string type)
		{
			return string.Format("{0}_{1}", type, base.master.stickerType);
		}

		// Token: 0x04003A19 RID: 14873
		[Serialize]
		public float destroyTime;
	}

	// Token: 0x02000FF5 RID: 4085
	public class States : GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb>
	{
		// Token: 0x06005242 RID: 21058 RVA: 0x00282D84 File Offset: 0x00280F84
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Transition(this.destroy, (StickerBomb.StatesInstance smi) => GameClock.Instance.GetTime() >= smi.destroyTime, UpdateRate.SIM_200ms).DefaultState(this.idle);
			this.idle.PlayAnim((StickerBomb.StatesInstance smi) => smi.GetStickerAnim("idle"), KAnim.PlayMode.Once).ScheduleGoTo((StickerBomb.StatesInstance smi) => (float)UnityEngine.Random.Range(20, 30), this.sparkle);
			this.sparkle.PlayAnim((StickerBomb.StatesInstance smi) => smi.GetStickerAnim("sparkle"), KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
			this.destroy.Enter(delegate(StickerBomb.StatesInstance smi)
			{
				Util.KDestroyGameObject(smi.master);
			});
		}

		// Token: 0x04003A1A RID: 14874
		public GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State destroy;

		// Token: 0x04003A1B RID: 14875
		public GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State sparkle;

		// Token: 0x04003A1C RID: 14876
		public GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State idle;
	}
}
