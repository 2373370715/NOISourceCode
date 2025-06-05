using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;

// Token: 0x020013A1 RID: 5025
public class GameplaySeasonManager : GameStateMachine<GameplaySeasonManager, GameplaySeasonManager.Instance, IStateMachineTarget, GameplaySeasonManager.Def>
{
	// Token: 0x06006700 RID: 26368 RVA: 0x002DFE08 File Offset: 0x002DE008
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.root;
		this.root.Enter(delegate(GameplaySeasonManager.Instance smi)
		{
			smi.Initialize();
		}).Update(delegate(GameplaySeasonManager.Instance smi, float dt)
		{
			smi.Update(dt);
		}, UpdateRate.SIM_4000ms, false);
	}

	// Token: 0x020013A2 RID: 5026
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020013A3 RID: 5027
	public new class Instance : GameStateMachine<GameplaySeasonManager, GameplaySeasonManager.Instance, IStateMachineTarget, GameplaySeasonManager.Def>.GameInstance
	{
		// Token: 0x06006703 RID: 26371 RVA: 0x000E7A66 File Offset: 0x000E5C66
		public Instance(IStateMachineTarget master, GameplaySeasonManager.Def def) : base(master, def)
		{
			this.activeSeasons = new List<GameplaySeasonInstance>();
		}

		// Token: 0x06006704 RID: 26372 RVA: 0x002DFE70 File Offset: 0x002DE070
		public void Initialize()
		{
			this.activeSeasons.RemoveAll((GameplaySeasonInstance item) => item.Season == null);
			List<GameplaySeason> list = new List<GameplaySeason>();
			if (this.m_worldContainer != null)
			{
				ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
				using (List<string>.Enumerator enumerator = this.m_worldContainer.GetSeasonIds().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						GameplaySeason gameplaySeason = Db.Get().GameplaySeasons.TryGet(text);
						if (gameplaySeason == null)
						{
							Debug.LogWarning("world " + component.name + " has invalid season " + text);
						}
						else
						{
							if (gameplaySeason.type != GameplaySeason.Type.World)
							{
								Debug.LogWarning(string.Concat(new string[]
								{
									"world ",
									component.name,
									" has specified season ",
									text,
									", which is not a world type season"
								}));
							}
							list.Add(gameplaySeason);
						}
					}
					goto IL_146;
				}
			}
			Debug.Assert(base.GetComponent<SaveGame>() != null);
			list = (from season in Db.Get().GameplaySeasons.resources
			where season.type == GameplaySeason.Type.Cluster
			select season).ToList<GameplaySeason>();
			IL_146:
			foreach (GameplaySeason gameplaySeason2 in list)
			{
				if (Game.IsDlcActiveForCurrentSave(gameplaySeason2.dlcId) && gameplaySeason2.startActive && !this.SeasonExists(gameplaySeason2) && gameplaySeason2.events.Count > 0)
				{
					this.activeSeasons.Add(gameplaySeason2.Instantiate(this.GetWorldId()));
				}
			}
			foreach (GameplaySeasonInstance gameplaySeasonInstance in new List<GameplaySeasonInstance>(this.activeSeasons))
			{
				if (!list.Contains(gameplaySeasonInstance.Season) || !Game.IsDlcActiveForCurrentSave(gameplaySeasonInstance.Season.dlcId))
				{
					this.activeSeasons.Remove(gameplaySeasonInstance);
				}
			}
		}

		// Token: 0x06006705 RID: 26373 RVA: 0x000E7A7B File Offset: 0x000E5C7B
		private int GetWorldId()
		{
			if (this.m_worldContainer != null)
			{
				return this.m_worldContainer.id;
			}
			return -1;
		}

		// Token: 0x06006706 RID: 26374 RVA: 0x002E00C8 File Offset: 0x002DE2C8
		public void Update(float dt)
		{
			foreach (GameplaySeasonInstance gameplaySeasonInstance in this.activeSeasons)
			{
				if (gameplaySeasonInstance.ShouldGenerateEvents() && GameUtil.GetCurrentTimeInCycles() > gameplaySeasonInstance.NextEventTime)
				{
					int num = 0;
					while (num < gameplaySeasonInstance.Season.numEventsToStartEachPeriod && gameplaySeasonInstance.StartEvent(false))
					{
						num++;
					}
				}
			}
		}

		// Token: 0x06006707 RID: 26375 RVA: 0x000E7A98 File Offset: 0x000E5C98
		public void StartNewSeason(GameplaySeason seasonType)
		{
			if (Game.IsDlcActiveForCurrentSave(seasonType.dlcId))
			{
				this.activeSeasons.Add(seasonType.Instantiate(this.GetWorldId()));
			}
		}

		// Token: 0x06006708 RID: 26376 RVA: 0x002E0148 File Offset: 0x002DE348
		public bool SeasonExists(GameplaySeason seasonType)
		{
			return this.activeSeasons.Find((GameplaySeasonInstance e) => e.Season.IdHash == seasonType.IdHash) != null;
		}

		// Token: 0x04004DC9 RID: 19913
		[Serialize]
		public List<GameplaySeasonInstance> activeSeasons;

		// Token: 0x04004DCA RID: 19914
		[MyCmpGet]
		private WorldContainer m_worldContainer;
	}
}
