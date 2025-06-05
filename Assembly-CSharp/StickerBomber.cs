using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020007E8 RID: 2024
public class StickerBomber : GameStateMachine<StickerBomber, StickerBomber.Instance>
{
	// Token: 0x060023C1 RID: 9153 RVA: 0x001D2E7C File Offset: 0x001D107C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false).Exit(delegate(StickerBomber.Instance smi)
		{
			smi.nextStickerBomb = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.TIME_PER_STICKER_BOMB;
		});
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ToggleStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_StickerBombing, null);
		this.overjoyed.idle.Transition(this.overjoyed.place_stickers, (StickerBomber.Instance smi) => GameClock.Instance.GetTime() >= smi.nextStickerBomb, UpdateRate.SIM_200ms);
		this.overjoyed.place_stickers.Exit(delegate(StickerBomber.Instance smi)
		{
			smi.nextStickerBomb = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.TIME_PER_STICKER_BOMB;
		}).ToggleReactable((StickerBomber.Instance smi) => smi.CreateReactable()).OnSignal(this.doneStickerBomb, this.overjoyed.idle);
	}

	// Token: 0x0400180A RID: 6154
	public StateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.Signal doneStickerBomb;

	// Token: 0x0400180B RID: 6155
	public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x0400180C RID: 6156
	public StickerBomber.OverjoyedStates overjoyed;

	// Token: 0x020007E9 RID: 2025
	public class OverjoyedStates : GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400180D RID: 6157
		public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x0400180E RID: 6158
		public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State place_stickers;
	}

	// Token: 0x020007EA RID: 2026
	public new class Instance : GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060023C4 RID: 9156 RVA: 0x000BBB95 File Offset: 0x000B9D95
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x000BBB9E File Offset: 0x000B9D9E
		public Reactable CreateReactable()
		{
			return new StickerBomber.Instance.StickerBombReactable(base.master.gameObject, base.smi);
		}

		// Token: 0x0400180F RID: 6159
		[Serialize]
		public float nextStickerBomb;

		// Token: 0x020007EB RID: 2027
		private class StickerBombReactable : Reactable
		{
			// Token: 0x060023C6 RID: 9158 RVA: 0x001D2FC4 File Offset: 0x001D11C4
			public StickerBombReactable(GameObject gameObject, StickerBomber.Instance stickerBomber) : base(gameObject, "StickerBombReactable", Db.Get().ChoreTypes.Build, 2, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
			{
				this.preventChoreInterruption = true;
				this.stickerBomber = stickerBomber;
			}

			// Token: 0x060023C7 RID: 9159 RVA: 0x001D30A4 File Offset: 0x001D12A4
			public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
			{
				if (this.reactor != null)
				{
					return false;
				}
				if (new_reactor == null)
				{
					return false;
				}
				if (this.gameObject != new_reactor)
				{
					return false;
				}
				Navigator component = new_reactor.GetComponent<Navigator>();
				return !(component == null) && component.CurrentNavType != NavType.Tube && component.CurrentNavType != NavType.Ladder && component.CurrentNavType != NavType.Pole;
			}

			// Token: 0x060023C8 RID: 9160 RVA: 0x001D310C File Offset: 0x001D130C
			protected override void InternalBegin()
			{
				this.stickersToPlace = UnityEngine.Random.Range(4, 6);
				this.STICKER_PLACE_TIMER = this.TIME_PER_STICKER_PLACED;
				this.placementCell = this.FindPlacementCell();
				if (this.placementCell == 0)
				{
					base.End();
					return;
				}
				this.kbac = this.reactor.GetComponent<KBatchedAnimController>();
				this.kbac.AddAnimOverrides(this.animset, 0f);
				this.kbac.Play(this.pre_anim, KAnim.PlayMode.Once, 1f, 0f);
				this.kbac.Queue(this.loop_anim, KAnim.PlayMode.Loop, 1f, 0f);
			}

			// Token: 0x060023C9 RID: 9161 RVA: 0x001D31AC File Offset: 0x001D13AC
			public override void Update(float dt)
			{
				this.STICKER_PLACE_TIMER -= dt;
				if (this.STICKER_PLACE_TIMER <= 0f)
				{
					this.PlaceSticker();
					this.STICKER_PLACE_TIMER = this.TIME_PER_STICKER_PLACED;
				}
				if (this.stickersPlaced >= this.stickersToPlace)
				{
					this.kbac.Play(this.pst_anim, KAnim.PlayMode.Once, 1f, 0f);
					base.End();
				}
			}

			// Token: 0x060023CA RID: 9162 RVA: 0x001D3218 File Offset: 0x001D1418
			protected override void InternalEnd()
			{
				if (this.kbac != null)
				{
					this.kbac.RemoveAnimOverrides(this.animset);
					this.kbac = null;
				}
				this.stickerBomber.sm.doneStickerBomb.Trigger(this.stickerBomber);
				this.stickersPlaced = 0;
			}

			// Token: 0x060023CB RID: 9163 RVA: 0x001D3270 File Offset: 0x001D1470
			private int FindPlacementCell()
			{
				int cell = Grid.PosToCell(this.reactor.transform.GetPosition() + Vector3.up);
				ListPool<int, PathFinder>.PooledList pooledList = ListPool<int, PathFinder>.Allocate();
				ListPool<int, PathFinder>.PooledList pooledList2 = ListPool<int, PathFinder>.Allocate();
				QueuePool<GameUtil.FloodFillInfo, Comet>.PooledQueue pooledQueue = QueuePool<GameUtil.FloodFillInfo, Comet>.Allocate();
				pooledQueue.Enqueue(new GameUtil.FloodFillInfo
				{
					cell = cell,
					depth = 0
				});
				GameUtil.FloodFillConditional(pooledQueue, this.canPlaceStickerCb, pooledList, pooledList2, 2);
				if (pooledList2.Count > 0)
				{
					int random = pooledList2.GetRandom<int>();
					pooledList.Recycle();
					pooledList2.Recycle();
					pooledQueue.Recycle();
					return random;
				}
				pooledList.Recycle();
				pooledList2.Recycle();
				pooledQueue.Recycle();
				return 0;
			}

			// Token: 0x060023CC RID: 9164 RVA: 0x001D3314 File Offset: 0x001D1514
			private void PlaceSticker()
			{
				this.stickersPlaced++;
				Vector3 a = Grid.CellToPos(this.placementCell);
				int i = 10;
				while (i > 0)
				{
					i--;
					Vector3 position = a + new Vector3(UnityEngine.Random.Range(-this.tile_random_range, this.tile_random_range), UnityEngine.Random.Range(-this.tile_random_range, this.tile_random_range), -2.5f);
					if (StickerBomb.CanPlaceSticker(StickerBomb.BuildCellOffsets(position)))
					{
						GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("StickerBomb".ToTag()), position, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-this.tile_random_rotation, this.tile_random_rotation)), null, null, true, 0);
						StickerBomb component = gameObject.GetComponent<StickerBomb>();
						string stickerType = this.reactor.GetComponent<MinionIdentity>().stickerType;
						component.SetStickerType(stickerType);
						gameObject.SetActive(true);
						i = 0;
					}
				}
			}

			// Token: 0x060023CD RID: 9165 RVA: 0x000AA038 File Offset: 0x000A8238
			protected override void InternalCleanup()
			{
			}

			// Token: 0x04001810 RID: 6160
			private int stickersToPlace;

			// Token: 0x04001811 RID: 6161
			private int stickersPlaced;

			// Token: 0x04001812 RID: 6162
			private int placementCell;

			// Token: 0x04001813 RID: 6163
			private float tile_random_range = 1f;

			// Token: 0x04001814 RID: 6164
			private float tile_random_rotation = 90f;

			// Token: 0x04001815 RID: 6165
			private float TIME_PER_STICKER_PLACED = 0.66f;

			// Token: 0x04001816 RID: 6166
			private float STICKER_PLACE_TIMER;

			// Token: 0x04001817 RID: 6167
			private KBatchedAnimController kbac;

			// Token: 0x04001818 RID: 6168
			private KAnimFile animset = Assets.GetAnim("anim_stickers_kanim");

			// Token: 0x04001819 RID: 6169
			private HashedString pre_anim = "working_pre";

			// Token: 0x0400181A RID: 6170
			private HashedString loop_anim = "working_loop";

			// Token: 0x0400181B RID: 6171
			private HashedString pst_anim = "working_pst";

			// Token: 0x0400181C RID: 6172
			private StickerBomber.Instance stickerBomber;

			// Token: 0x0400181D RID: 6173
			private Func<int, bool> canPlaceStickerCb = (int cell) => !Grid.Solid[cell] && (!Grid.IsValidCell(Grid.CellLeft(cell)) || !Grid.Solid[Grid.CellLeft(cell)]) && (!Grid.IsValidCell(Grid.CellRight(cell)) || !Grid.Solid[Grid.CellRight(cell)]) && (!Grid.IsValidCell(Grid.OffsetCell(cell, 0, 1)) || !Grid.Solid[Grid.OffsetCell(cell, 0, 1)]) && (!Grid.IsValidCell(Grid.OffsetCell(cell, 0, -1)) || !Grid.Solid[Grid.OffsetCell(cell, 0, -1)]) && !Grid.IsCellOpenToSpace(cell);
		}
	}
}
