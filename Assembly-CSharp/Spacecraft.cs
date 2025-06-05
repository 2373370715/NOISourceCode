using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001998 RID: 6552
[SerializationConfig(MemberSerialization.OptIn)]
public class Spacecraft
{
	// Token: 0x06008880 RID: 34944 RVA: 0x000FDD1D File Offset: 0x000FBF1D
	public Spacecraft(LaunchConditionManager launchConditions)
	{
		this.launchConditions = launchConditions;
	}

	// Token: 0x06008881 RID: 34945 RVA: 0x000FDD4E File Offset: 0x000FBF4E
	public Spacecraft()
	{
	}

	// Token: 0x170008FB RID: 2299
	// (get) Token: 0x06008882 RID: 34946 RVA: 0x000FDD78 File Offset: 0x000FBF78
	// (set) Token: 0x06008883 RID: 34947 RVA: 0x000FDD85 File Offset: 0x000FBF85
	public LaunchConditionManager launchConditions
	{
		get
		{
			return this.refLaunchConditions.Get();
		}
		set
		{
			this.refLaunchConditions.Set(value);
		}
	}

	// Token: 0x06008884 RID: 34948 RVA: 0x000FDD93 File Offset: 0x000FBF93
	public void SetRocketName(string newName)
	{
		this.rocketName = newName;
		this.UpdateNameOnRocketModules();
	}

	// Token: 0x06008885 RID: 34949 RVA: 0x000FDDA2 File Offset: 0x000FBFA2
	public string GetRocketName()
	{
		return this.rocketName;
	}

	// Token: 0x06008886 RID: 34950 RVA: 0x003634B8 File Offset: 0x003616B8
	public void UpdateNameOnRocketModules()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchConditions.GetComponent<AttachableBuilding>()))
		{
			RocketModule component = gameObject.GetComponent<RocketModule>();
			if (component != null)
			{
				component.SetParentRocketName(this.rocketName);
			}
		}
	}

	// Token: 0x06008887 RID: 34951 RVA: 0x000FDDAA File Offset: 0x000FBFAA
	public bool HasInvalidID()
	{
		return this.id == -1;
	}

	// Token: 0x06008888 RID: 34952 RVA: 0x000FDDB5 File Offset: 0x000FBFB5
	public void SetID(int id)
	{
		this.id = id;
	}

	// Token: 0x06008889 RID: 34953 RVA: 0x000FDDBE File Offset: 0x000FBFBE
	public void SetState(Spacecraft.MissionState state)
	{
		this.state = state;
	}

	// Token: 0x0600888A RID: 34954 RVA: 0x000FDDC7 File Offset: 0x000FBFC7
	public void BeginMission(SpaceDestination destination)
	{
		this.missionElapsed = 0f;
		this.missionDuration = (float)destination.OneBasedDistance * ROCKETRY.MISSION_DURATION_SCALE / this.GetPilotNavigationEfficiency();
		this.SetState(Spacecraft.MissionState.Launching);
	}

	// Token: 0x0600888B RID: 34955 RVA: 0x00363528 File Offset: 0x00361728
	private float GetPilotNavigationEfficiency()
	{
		float num = 1f;
		if (!this.launchConditions.GetComponent<CommandModule>().robotPilotControlled)
		{
			List<MinionStorage.Info> storedMinionInfo = this.launchConditions.GetComponent<MinionStorage>().GetStoredMinionInfo();
			if (storedMinionInfo.Count < 1)
			{
				return 1f;
			}
			StoredMinionIdentity component = storedMinionInfo[0].serializedMinion.Get().GetComponent<StoredMinionIdentity>();
			string b = Db.Get().Attributes.SpaceNavigation.Id;
			foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
			{
				foreach (SkillPerk skillPerk in Db.Get().Skills.Get(keyValuePair.Key).perks)
				{
					if (Game.IsCorrectDlcActiveForCurrentSave(skillPerk))
					{
						SkillAttributePerk skillAttributePerk = skillPerk as SkillAttributePerk;
						if (skillAttributePerk != null && skillAttributePerk.modifier.AttributeId == b)
						{
							num += skillAttributePerk.modifier.Value;
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x0600888C RID: 34956 RVA: 0x000FDDF5 File Offset: 0x000FBFF5
	public void ForceComplete()
	{
		this.missionElapsed = this.missionDuration;
	}

	// Token: 0x0600888D RID: 34957 RVA: 0x0036366C File Offset: 0x0036186C
	public void ProgressMission(float deltaTime)
	{
		if (this.state == Spacecraft.MissionState.Underway)
		{
			this.missionElapsed += deltaTime;
			if (this.controlStationBuffTimeRemaining > 0f)
			{
				this.missionElapsed += deltaTime * 0.20000005f;
				this.controlStationBuffTimeRemaining -= deltaTime;
			}
			else
			{
				this.controlStationBuffTimeRemaining = 0f;
			}
			if (this.missionElapsed > this.missionDuration)
			{
				this.CompleteMission();
			}
		}
	}

	// Token: 0x0600888E RID: 34958 RVA: 0x000FDE03 File Offset: 0x000FC003
	public float GetTimeLeft()
	{
		return this.missionDuration - this.missionElapsed;
	}

	// Token: 0x0600888F RID: 34959 RVA: 0x000FDE12 File Offset: 0x000FC012
	public float GetDuration()
	{
		return this.missionDuration;
	}

	// Token: 0x06008890 RID: 34960 RVA: 0x000FDE1A File Offset: 0x000FC01A
	public void CompleteMission()
	{
		SpacecraftManager.instance.PushReadyToLandNotification(this);
		this.SetState(Spacecraft.MissionState.WaitingToLand);
		this.Land();
	}

	// Token: 0x06008891 RID: 34961 RVA: 0x003636E0 File Offset: 0x003618E0
	private void Land()
	{
		this.launchConditions.Trigger(-1165815793, SpacecraftManager.instance.GetSpacecraftDestination(this.id));
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchConditions.GetComponent<AttachableBuilding>()))
		{
			if (gameObject != this.launchConditions.gameObject)
			{
				gameObject.Trigger(-1165815793, SpacecraftManager.instance.GetSpacecraftDestination(this.id));
			}
		}
	}

	// Token: 0x06008892 RID: 34962 RVA: 0x00363784 File Offset: 0x00361984
	public void TemporallyTear()
	{
		SpacecraftManager.instance.hasVisitedWormHole = true;
		LaunchConditionManager launchConditions = this.launchConditions;
		for (int i = launchConditions.rocketModules.Count - 1; i >= 0; i--)
		{
			Storage component = launchConditions.rocketModules[i].GetComponent<Storage>();
			if (component != null)
			{
				component.ConsumeAllIgnoringDisease();
			}
			MinionStorage component2 = launchConditions.rocketModules[i].GetComponent<MinionStorage>();
			if (component2 != null)
			{
				List<MinionStorage.Info> storedMinionInfo = component2.GetStoredMinionInfo();
				for (int j = storedMinionInfo.Count - 1; j >= 0; j--)
				{
					component2.DeleteStoredMinion(storedMinionInfo[j].id);
				}
			}
			Util.KDestroyGameObject(launchConditions.rocketModules[i].gameObject);
		}
	}

	// Token: 0x06008893 RID: 34963 RVA: 0x000FDE34 File Offset: 0x000FC034
	public void GenerateName()
	{
		this.SetRocketName(GameUtil.GenerateRandomRocketName());
	}

	// Token: 0x04006768 RID: 26472
	[Serialize]
	public int id = -1;

	// Token: 0x04006769 RID: 26473
	[Serialize]
	public string rocketName = UI.STARMAP.DEFAULT_NAME;

	// Token: 0x0400676A RID: 26474
	[Serialize]
	public float controlStationBuffTimeRemaining;

	// Token: 0x0400676B RID: 26475
	[Serialize]
	public Ref<LaunchConditionManager> refLaunchConditions = new Ref<LaunchConditionManager>();

	// Token: 0x0400676C RID: 26476
	[Serialize]
	public Spacecraft.MissionState state;

	// Token: 0x0400676D RID: 26477
	[Serialize]
	private float missionElapsed;

	// Token: 0x0400676E RID: 26478
	[Serialize]
	private float missionDuration;

	// Token: 0x02001999 RID: 6553
	public enum MissionState
	{
		// Token: 0x04006770 RID: 26480
		Grounded,
		// Token: 0x04006771 RID: 26481
		Launching,
		// Token: 0x04006772 RID: 26482
		Underway,
		// Token: 0x04006773 RID: 26483
		WaitingToLand,
		// Token: 0x04006774 RID: 26484
		Landing,
		// Token: 0x04006775 RID: 26485
		Destroyed
	}
}
