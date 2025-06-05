using System;
using System.Runtime.CompilerServices;
using FMODUnity;
using ProcGenGame;
using UnityEngine;

// Token: 0x02001B4D RID: 6989
public class NewBaseScreen : KScreen
{
	// Token: 0x06009292 RID: 37522 RVA: 0x000B95A1 File Offset: 0x000B77A1
	public override float GetSortKey()
	{
		return 1f;
	}

	// Token: 0x06009293 RID: 37523 RVA: 0x00104621 File Offset: 0x00102821
	protected override void OnPrefabInit()
	{
		NewBaseScreen.Instance = this;
		base.OnPrefabInit();
		TimeOfDay.Instance.SetScale(0f);
	}

	// Token: 0x06009294 RID: 37524 RVA: 0x0010463E File Offset: 0x0010283E
	protected override void OnForcedCleanUp()
	{
		NewBaseScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06009295 RID: 37525 RVA: 0x00393AC4 File Offset: 0x00391CC4
	public static Vector2I SetInitialCamera()
	{
		Vector2I vector2I = SaveLoader.Instance.cachedGSD.baseStartPos;
		vector2I += ClusterManager.Instance.GetStartWorld().WorldOffset;
		Vector3 pos = Grid.CellToPosCCC(Grid.OffsetCell(Grid.OffsetCell(0, vector2I.x, vector2I.y), 0, -2), Grid.SceneLayer.Background);
		CameraController.Instance.SetMaxOrthographicSize(40f);
		CameraController.Instance.SnapTo(pos);
		CameraController.Instance.SetTargetPos(pos, 20f, false);
		CameraController.Instance.OrthographicSize = 40f;
		CameraSaveData.valid = false;
		return vector2I;
	}

	// Token: 0x06009296 RID: 37526 RVA: 0x00393B5C File Offset: 0x00391D5C
	protected override void OnActivate()
	{
		if (this.disabledUIElements != null)
		{
			foreach (CanvasGroup canvasGroup in this.disabledUIElements)
			{
				if (canvasGroup != null)
				{
					canvasGroup.interactable = false;
				}
			}
		}
		NewBaseScreen.SetInitialCamera();
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		this.Final();
	}

	// Token: 0x06009297 RID: 37527 RVA: 0x0010464C File Offset: 0x0010284C
	public void Init(Cluster clusterLayout, ITelepadDeliverable[] startingMinionStats)
	{
		this.m_clusterLayout = clusterLayout;
		this.m_minionStartingStats = startingMinionStats;
	}

	// Token: 0x06009298 RID: 37528 RVA: 0x00393BC0 File Offset: 0x00391DC0
	protected override void OnDeactivate()
	{
		Game.Instance.Trigger(-122303817, null);
		if (this.disabledUIElements != null)
		{
			foreach (CanvasGroup canvasGroup in this.disabledUIElements)
			{
				if (canvasGroup != null)
				{
					canvasGroup.interactable = true;
				}
			}
		}
	}

	// Token: 0x06009299 RID: 37529 RVA: 0x00393C10 File Offset: 0x00391E10
	public override void OnKeyDown(KButtonEvent e)
	{
		global::Action[] array = new global::Action[4];
		RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.46E7A7E6CE942EAE1E13925BEDED6E6321F99918099A108FDB32BB9510B8E88D).FieldHandle);
		global::Action[] array2 = array;
		if (!e.Consumed)
		{
			int num = 0;
			while (num < array2.Length && !e.TryConsume(array2[num]))
			{
				num++;
			}
		}
	}

	// Token: 0x0600929A RID: 37530 RVA: 0x00393C50 File Offset: 0x00391E50
	private void Final()
	{
		SpeedControlScreen.Instance.Unpause(false);
		GameObject telepad = GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id);
		if (telepad)
		{
			this.SpawnMinions(telepad);
		}
		Game.Instance.baseAlreadyCreated = true;
		this.Deactivate();
	}

	// Token: 0x0600929B RID: 37531 RVA: 0x00393CA0 File Offset: 0x00391EA0
	private void SpawnMinions(GameObject start_pad)
	{
		int num = Grid.PosToCell(start_pad);
		if (num == -1)
		{
			global::Debug.LogWarning("No headquarters in saved base template. Cannot place minions. Confirm there is a headquarters saved to the base template, or consider creating a new one.");
			return;
		}
		int num2;
		int num3;
		Grid.CellToXY(num, out num2, out num3);
		if (Grid.WidthInCells < 64)
		{
			return;
		}
		int baseLeft = this.m_clusterLayout.currentWorld.BaseLeft;
		int baseRight = this.m_clusterLayout.currentWorld.BaseRight;
		Db.Get().effects.Get("AnewHope");
		Telepad component = start_pad.GetComponent<Telepad>();
		for (int i = 0; i < this.m_minionStartingStats.Length; i++)
		{
			MinionStartingStats minionStartingStats = (MinionStartingStats)this.m_minionStartingStats[i];
			int x = num2 + i % (baseRight - baseLeft) + 1;
			int y = num3;
			int cell = Grid.XYToCell(x, y);
			GameObject prefab = Assets.GetPrefab(BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
			GameObject gameObject = Util.KInstantiate(prefab, null, null);
			gameObject.name = prefab.name;
			Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
			gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
			gameObject.SetActive(true);
			minionStartingStats.Apply(gameObject);
			if (component != null)
			{
				component.AddNewBaseMinion(gameObject, minionStartingStats.personality.model == GameTags.Minions.Models.Bionic);
			}
		}
		component.ScheduleNewBaseEvents();
		ClusterManager.Instance.activeWorld.SetDupeVisited();
	}

	// Token: 0x04006F18 RID: 28440
	public static NewBaseScreen Instance;

	// Token: 0x04006F19 RID: 28441
	[SerializeField]
	private CanvasGroup[] disabledUIElements;

	// Token: 0x04006F1A RID: 28442
	public EventReference ScanSoundMigrated;

	// Token: 0x04006F1B RID: 28443
	public EventReference BuildBaseSoundMigrated;

	// Token: 0x04006F1C RID: 28444
	private ITelepadDeliverable[] m_minionStartingStats;

	// Token: 0x04006F1D RID: 28445
	private Cluster m_clusterLayout;
}
