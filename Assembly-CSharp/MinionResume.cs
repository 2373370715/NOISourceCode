using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001841 RID: 6209
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MinionResume")]
public class MinionResume : IExperienceRecipient, ISaveLoadable, ISim200ms
{
	// Token: 0x17000814 RID: 2068
	// (get) Token: 0x06007F8C RID: 32652 RVA: 0x000F87CE File Offset: 0x000F69CE
	public MinionIdentity GetIdentity
	{
		get
		{
			return this.identity;
		}
	}

	// Token: 0x17000815 RID: 2069
	// (get) Token: 0x06007F8D RID: 32653 RVA: 0x000F87D6 File Offset: 0x000F69D6
	public float TotalExperienceGained
	{
		get
		{
			return this.totalExperienceGained;
		}
	}

	// Token: 0x17000816 RID: 2070
	// (get) Token: 0x06007F8E RID: 32654 RVA: 0x000F87DE File Offset: 0x000F69DE
	public int TotalSkillPointsGained
	{
		get
		{
			return MinionResume.CalculateTotalSkillPointsGained(this.TotalExperienceGained);
		}
	}

	// Token: 0x06007F8F RID: 32655 RVA: 0x000F87EB File Offset: 0x000F69EB
	public static int CalculateTotalSkillPointsGained(float experience)
	{
		return Mathf.FloorToInt(Mathf.Pow(experience / (float)SKILLS.TARGET_SKILLS_CYCLE / 600f, 1f / SKILLS.EXPERIENCE_LEVEL_POWER) * (float)SKILLS.TARGET_SKILLS_EARNED);
	}

	// Token: 0x17000817 RID: 2071
	// (get) Token: 0x06007F90 RID: 32656 RVA: 0x0033C948 File Offset: 0x0033AB48
	public int SkillsMastered
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
			{
				if (keyValuePair.Value)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x17000818 RID: 2072
	// (get) Token: 0x06007F91 RID: 32657 RVA: 0x000F8817 File Offset: 0x000F6A17
	public int AvailableSkillpoints
	{
		get
		{
			return this.TotalSkillPointsGained - this.SkillsMastered + ((this.GrantedSkillIDs == null) ? 0 : this.GrantedSkillIDs.Count);
		}
	}

	// Token: 0x06007F92 RID: 32658 RVA: 0x0033C9A4 File Offset: 0x0033ABA4
	[OnDeserialized]
	private void OnDeserializedMethod()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 7))
		{
			foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryByRoleID)
			{
				if (keyValuePair.Value && keyValuePair.Key != "NoRole")
				{
					this.ForceAddSkillPoint();
				}
			}
			foreach (KeyValuePair<HashedString, float> keyValuePair2 in this.AptitudeByRoleGroup)
			{
				this.AptitudeBySkillGroup[keyValuePair2.Key] = keyValuePair2.Value;
			}
		}
		if (this.TotalSkillPointsGained > 1000 || this.TotalSkillPointsGained < 0)
		{
			this.ForceSetSkillPoints(100);
		}
	}

	// Token: 0x06007F93 RID: 32659 RVA: 0x000F883D File Offset: 0x000F6A3D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.MinionResumes.Add(this);
	}

	// Token: 0x06007F94 RID: 32660 RVA: 0x0033CAA0 File Offset: 0x0033ACA0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.GrantedSkillIDs.RemoveAll((string x) => Db.Get().Skills.TryGet(x) == null);
		List<string> list = new List<string>();
		foreach (string text in this.MasteryBySkillID.Keys)
		{
			if (Db.Get().Skills.TryGet(text) == null)
			{
				list.Add(text);
			}
		}
		foreach (string key in list)
		{
			this.MasteryBySkillID.Remove(key);
		}
		if (this.GrantedSkillIDs == null)
		{
			this.GrantedSkillIDs = new List<string>();
		}
		List<string> list2 = new List<string>();
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).deprecated)
			{
				list2.Add(keyValuePair.Key);
			}
		}
		foreach (string skillId in list2)
		{
			this.UnmasterSkill(skillId);
		}
		foreach (KeyValuePair<string, bool> keyValuePair2 in this.MasteryBySkillID)
		{
			if (keyValuePair2.Value)
			{
				Skill skill = Db.Get().Skills.Get(keyValuePair2.Key);
				foreach (SkillPerk skillPerk in skill.perks)
				{
					if (Game.IsCorrectDlcActiveForCurrentSave(skillPerk))
					{
						if (skillPerk.OnRemove != null)
						{
							skillPerk.OnRemove(this);
						}
						if (skillPerk.OnApply != null)
						{
							skillPerk.OnApply(this);
						}
					}
				}
				if (!this.ownedHats.ContainsKey(skill.hat))
				{
					this.ownedHats.Add(skill.hat, true);
				}
			}
		}
		this.UpdateExpectations();
		this.UpdateMorale();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		MinionResume.ApplyHat(this.currentHat, component);
		this.ShowNewSkillPointNotification();
	}

	// Token: 0x06007F95 RID: 32661 RVA: 0x000F8850 File Offset: 0x000F6A50
	public void RestoreResume(Dictionary<string, bool> MasteryBySkillID, Dictionary<HashedString, float> AptitudeBySkillGroup, List<string> GrantedSkillIDs, float totalExperienceGained)
	{
		this.MasteryBySkillID = MasteryBySkillID;
		this.GrantedSkillIDs = ((GrantedSkillIDs != null) ? GrantedSkillIDs : new List<string>());
		this.AptitudeBySkillGroup = AptitudeBySkillGroup;
		this.totalExperienceGained = totalExperienceGained;
	}

	// Token: 0x06007F96 RID: 32662 RVA: 0x000F8879 File Offset: 0x000F6A79
	protected override void OnCleanUp()
	{
		Components.MinionResumes.Remove(this);
		if (this.lastSkillNotification != null)
		{
			Game.Instance.GetComponent<Notifier>().Remove(this.lastSkillNotification);
			this.lastSkillNotification = null;
		}
		base.OnCleanUp();
	}

	// Token: 0x06007F97 RID: 32663 RVA: 0x000F88B0 File Offset: 0x000F6AB0
	public bool HasMasteredSkill(string skillId)
	{
		return this.MasteryBySkillID.ContainsKey(skillId) && this.MasteryBySkillID[skillId];
	}

	// Token: 0x06007F98 RID: 32664 RVA: 0x0033CD7C File Offset: 0x0033AF7C
	public void UpdateUrge()
	{
		if (this.targetHat != this.currentHat)
		{
			if (!base.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
			{
				base.gameObject.GetComponent<ChoreConsumer>().AddUrge(Db.Get().Urges.LearnSkill);
				return;
			}
		}
		else
		{
			base.gameObject.GetComponent<ChoreConsumer>().RemoveUrge(Db.Get().Urges.LearnSkill);
		}
	}

	// Token: 0x17000819 RID: 2073
	// (get) Token: 0x06007F99 RID: 32665 RVA: 0x000F88CE File Offset: 0x000F6ACE
	public string CurrentRole
	{
		get
		{
			return this.currentRole;
		}
	}

	// Token: 0x1700081A RID: 2074
	// (get) Token: 0x06007F9A RID: 32666 RVA: 0x000F88D6 File Offset: 0x000F6AD6
	public string CurrentHat
	{
		get
		{
			return this.currentHat;
		}
	}

	// Token: 0x1700081B RID: 2075
	// (get) Token: 0x06007F9B RID: 32667 RVA: 0x000F88DE File Offset: 0x000F6ADE
	public string TargetHat
	{
		get
		{
			return this.targetHat;
		}
	}

	// Token: 0x06007F9C RID: 32668 RVA: 0x000F88E6 File Offset: 0x000F6AE6
	public void SetHats(string current, string target)
	{
		this.currentHat = current;
		this.targetHat = target;
	}

	// Token: 0x06007F9D RID: 32669 RVA: 0x000F88F6 File Offset: 0x000F6AF6
	public void ClearAdditionalHats()
	{
		this.AdditionalHats.Clear();
	}

	// Token: 0x06007F9E RID: 32670 RVA: 0x0033CDFC File Offset: 0x0033AFFC
	public void AddAdditionalHat(string context, string hat)
	{
		MinionResume.HatInfo hatInfo = null;
		foreach (MinionResume.HatInfo hatInfo2 in this.AdditionalHats)
		{
			if (hatInfo2.Source == context && hatInfo2.Hat == hat)
			{
				hatInfo = hatInfo2;
				break;
			}
		}
		if (hatInfo != null)
		{
			hatInfo.count++;
			return;
		}
		this.AdditionalHats.Add(new MinionResume.HatInfo(context, hat));
	}

	// Token: 0x06007F9F RID: 32671 RVA: 0x0033CE90 File Offset: 0x0033B090
	public void RemoveAdditionalHat(string context, string hat)
	{
		MinionResume.HatInfo hatInfo = null;
		foreach (MinionResume.HatInfo hatInfo2 in this.AdditionalHats)
		{
			if (hatInfo2.Source == context && hatInfo2.Hat == hat)
			{
				hatInfo2.count--;
				hatInfo = hatInfo2;
				break;
			}
		}
		if (hatInfo != null && hatInfo.count <= 0)
		{
			this.AdditionalHats.Remove(hatInfo);
			if (this.currentHat == hat)
			{
				this.RemoveHat();
			}
		}
	}

	// Token: 0x06007FA0 RID: 32672 RVA: 0x000F8903 File Offset: 0x000F6B03
	public void SetCurrentRole(string role_id)
	{
		this.currentRole = role_id;
	}

	// Token: 0x1700081C RID: 2076
	// (get) Token: 0x06007FA1 RID: 32673 RVA: 0x000F890C File Offset: 0x000F6B0C
	public string TargetRole
	{
		get
		{
			return this.targetRole;
		}
	}

	// Token: 0x06007FA2 RID: 32674 RVA: 0x0033CF3C File Offset: 0x0033B13C
	public void ApplyAdditionalSkillPerks(SkillPerk[] perks)
	{
		foreach (SkillPerk skillPerk in perks)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(skillPerk))
			{
				this.AdditionalGrantedSkillPerkIDs.Add(skillPerk.IdHash);
				if (skillPerk.OnApply != null)
				{
					skillPerk.OnApply(this);
				}
			}
		}
		Game.Instance.Trigger(-1523247426, null);
	}

	// Token: 0x06007FA3 RID: 32675 RVA: 0x0033CF9C File Offset: 0x0033B19C
	public void RemoveAdditionalSkillPerks(SkillPerk[] perks)
	{
		foreach (SkillPerk skillPerk in perks)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(skillPerk))
			{
				this.AdditionalGrantedSkillPerkIDs.Remove(skillPerk.IdHash);
				if (skillPerk.OnRemove != null)
				{
					skillPerk.OnRemove(this);
				}
			}
		}
	}

	// Token: 0x06007FA4 RID: 32676 RVA: 0x0033CFEC File Offset: 0x0033B1EC
	private void ApplySkillPerksForSkill(string skillId)
	{
		foreach (SkillPerk skillPerk in Db.Get().Skills.Get(skillId).perks)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(skillPerk) && skillPerk.OnApply != null)
			{
				skillPerk.OnApply(this);
			}
		}
	}

	// Token: 0x06007FA5 RID: 32677 RVA: 0x0033D064 File Offset: 0x0033B264
	private void RemoveSkillPerksForSkill(string skillId)
	{
		foreach (SkillPerk skillPerk in Db.Get().Skills.Get(skillId).perks)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(skillPerk) && skillPerk.OnRemove != null)
			{
				skillPerk.OnRemove(this);
			}
		}
	}

	// Token: 0x06007FA6 RID: 32678 RVA: 0x0033D0DC File Offset: 0x0033B2DC
	public void Sim200ms(float dt)
	{
		this.DEBUG_SecondsAlive += dt;
		if (!base.GetComponent<KPrefabID>().HasTag(GameTags.Dead))
		{
			this.DEBUG_PassiveExperienceGained += dt * SKILLS.PASSIVE_EXPERIENCE_PORTION;
			this.AddExperience(dt * SKILLS.PASSIVE_EXPERIENCE_PORTION);
		}
	}

	// Token: 0x06007FA7 RID: 32679 RVA: 0x0033D12C File Offset: 0x0033B32C
	public bool IsAbleToLearnSkill(string skillId)
	{
		Skill skill = Db.Get().Skills.Get(skillId);
		string choreGroupID = Db.Get().SkillGroups.Get(skill.skillGroup).choreGroupID;
		if (!string.IsNullOrEmpty(choreGroupID))
		{
			Traits component = base.GetComponent<Traits>();
			if (component != null && component.IsChoreGroupDisabled(choreGroupID))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06007FA8 RID: 32680 RVA: 0x0033D190 File Offset: 0x0033B390
	public bool BelowMoraleExpectation(Skill skill)
	{
		float num = Db.Get().Attributes.QualityOfLife.Lookup(this).GetTotalValue();
		float totalValue = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(this).GetTotalValue();
		int moraleExpectation = skill.GetMoraleExpectation();
		if (this.AptitudeBySkillGroup.ContainsKey(skill.skillGroup) && this.AptitudeBySkillGroup[skill.skillGroup] > 0f)
		{
			num += 1f;
		}
		return totalValue + (float)moraleExpectation <= num;
	}

	// Token: 0x06007FA9 RID: 32681 RVA: 0x0033D220 File Offset: 0x0033B420
	public bool HasMasteredDirectlyRequiredSkillsForSkill(Skill skill)
	{
		for (int i = 0; i < skill.priorSkills.Count; i++)
		{
			if (!this.HasMasteredSkill(skill.priorSkills[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06007FAA RID: 32682 RVA: 0x000F8914 File Offset: 0x000F6B14
	public bool HasSkillPointsRequiredForSkill(Skill skill)
	{
		return this.AvailableSkillpoints >= 1;
	}

	// Token: 0x06007FAB RID: 32683 RVA: 0x000F8922 File Offset: 0x000F6B22
	public bool HasSkillAptitude(Skill skill)
	{
		return this.AptitudeBySkillGroup.ContainsKey(skill.skillGroup) && this.AptitudeBySkillGroup[skill.skillGroup] > 0f;
	}

	// Token: 0x06007FAC RID: 32684 RVA: 0x000F895C File Offset: 0x000F6B5C
	public bool HasBeenGrantedSkill(Skill skill)
	{
		return this.GrantedSkillIDs != null && this.GrantedSkillIDs.Contains(skill.Id);
	}

	// Token: 0x06007FAD RID: 32685 RVA: 0x000F897E File Offset: 0x000F6B7E
	public bool HasBeenGrantedSkill(string id)
	{
		return this.GrantedSkillIDs != null && this.GrantedSkillIDs.Contains(id);
	}

	// Token: 0x06007FAE RID: 32686 RVA: 0x0033D25C File Offset: 0x0033B45C
	public MinionResume.SkillMasteryConditions[] GetSkillMasteryConditions(string skillId)
	{
		List<MinionResume.SkillMasteryConditions> list = new List<MinionResume.SkillMasteryConditions>();
		Skill skill = Db.Get().Skills.Get(skillId);
		if (this.HasSkillAptitude(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.SkillAptitude);
		}
		if (!this.BelowMoraleExpectation(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.StressWarning);
		}
		if (!this.IsAbleToLearnSkill(skillId))
		{
			list.Add(MinionResume.SkillMasteryConditions.UnableToLearn);
		}
		if (!this.HasSkillPointsRequiredForSkill(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.NeedsSkillPoints);
		}
		if (!this.HasMasteredDirectlyRequiredSkillsForSkill(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.MissingPreviousSkill);
		}
		return list.ToArray();
	}

	// Token: 0x06007FAF RID: 32687 RVA: 0x000F899B File Offset: 0x000F6B9B
	public bool CanMasterSkill(MinionResume.SkillMasteryConditions[] masteryConditions)
	{
		return !Array.Exists<MinionResume.SkillMasteryConditions>(masteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.UnableToLearn || element == MinionResume.SkillMasteryConditions.NeedsSkillPoints || element == MinionResume.SkillMasteryConditions.MissingPreviousSkill);
	}

	// Token: 0x06007FB0 RID: 32688 RVA: 0x0033D2D8 File Offset: 0x0033B4D8
	public bool OwnsHat(string hatId)
	{
		using (List<MinionResume.HatInfo>.Enumerator enumerator = this.AdditionalHats.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Hat == hatId)
				{
					return true;
				}
			}
		}
		return this.ownedHats.ContainsKey(hatId) && this.ownedHats[hatId];
	}

	// Token: 0x06007FB1 RID: 32689 RVA: 0x0033D354 File Offset: 0x0033B554
	public List<MinionResume.HatInfo> GetAllHats()
	{
		List<MinionResume.HatInfo> list = new List<MinionResume.HatInfo>();
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value)
			{
				Skill skill = Db.Get().Skills.TryGet(keyValuePair.Key);
				if (!skill.hat.IsNullOrWhiteSpace())
				{
					list.Add(new MinionResume.HatInfo(skill.Name, skill.hat));
				}
			}
		}
		list.AddRange(this.AdditionalHats);
		return list;
	}

	// Token: 0x06007FB2 RID: 32690 RVA: 0x0033D3F8 File Offset: 0x0033B5F8
	public void SkillLearned()
	{
		if (base.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
		{
			base.gameObject.GetComponent<ChoreConsumer>().RemoveUrge(Db.Get().Urges.LearnSkill);
		}
		foreach (string key in this.ownedHats.Keys.ToList<string>())
		{
			this.ownedHats[key] = true;
		}
		if (this.targetHat != null && this.currentHat != this.targetHat)
		{
			this.CreateHatChangeChore();
		}
	}

	// Token: 0x06007FB3 RID: 32691 RVA: 0x000F89C7 File Offset: 0x000F6BC7
	public void CreateHatChangeChore()
	{
		if (this.lastHatChore != null)
		{
			this.lastHatChore.Cancel("New Hat");
		}
		this.lastHatChore = new PutOnHatChore(this, Db.Get().ChoreTypes.SwitchHat);
	}

	// Token: 0x06007FB4 RID: 32692 RVA: 0x0033D4BC File Offset: 0x0033B6BC
	public void MasterSkill(string skillId)
	{
		if (!base.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
		{
			base.gameObject.GetComponent<ChoreConsumer>().AddUrge(Db.Get().Urges.LearnSkill);
		}
		this.MasteryBySkillID[skillId] = true;
		this.ApplySkillPerksForSkill(skillId);
		this.UpdateExpectations();
		this.UpdateMorale();
		this.TriggerMasterSkillEvents();
		GameScheduler.Instance.Schedule("Morale Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Morale, true);
		}, null, null);
		if (!this.ownedHats.ContainsKey(Db.Get().Skills.Get(skillId).hat))
		{
			this.ownedHats.Add(Db.Get().Skills.Get(skillId).hat, false);
		}
		if (this.AvailableSkillpoints == 0 && this.lastSkillNotification != null)
		{
			Game.Instance.GetComponent<Notifier>().Remove(this.lastSkillNotification);
			this.lastSkillNotification = null;
		}
	}

	// Token: 0x06007FB5 RID: 32693 RVA: 0x000F89FC File Offset: 0x000F6BFC
	public void UnmasterSkill(string skillId)
	{
		if (this.MasteryBySkillID.ContainsKey(skillId))
		{
			this.MasteryBySkillID.Remove(skillId);
			this.RemoveSkillPerksForSkill(skillId);
			this.UpdateExpectations();
			this.UpdateMorale();
			this.TriggerMasterSkillEvents();
		}
	}

	// Token: 0x06007FB6 RID: 32694 RVA: 0x0033D5D4 File Offset: 0x0033B7D4
	public void GrantSkill(string skillId)
	{
		if (this.GrantedSkillIDs == null)
		{
			this.GrantedSkillIDs = new List<string>();
		}
		if (!this.HasBeenGrantedSkill(skillId))
		{
			this.MasteryBySkillID[skillId] = true;
			this.ApplySkillPerksForSkill(skillId);
			this.GrantedSkillIDs.Add(skillId);
			this.UpdateExpectations();
			this.UpdateMorale();
			this.TriggerMasterSkillEvents();
			if (!this.ownedHats.ContainsKey(Db.Get().Skills.Get(skillId).hat))
			{
				this.ownedHats.Add(Db.Get().Skills.Get(skillId).hat, false);
			}
		}
	}

	// Token: 0x06007FB7 RID: 32695 RVA: 0x0033D674 File Offset: 0x0033B874
	public void UngrantSkill(string skillId)
	{
		if (this.GrantedSkillIDs != null)
		{
			this.GrantedSkillIDs.RemoveAll((string match) => match == skillId);
		}
		this.UnmasterSkill(skillId);
	}

	// Token: 0x06007FB8 RID: 32696 RVA: 0x0033D6BC File Offset: 0x0033B8BC
	public Sprite GetSkillGrantSourceIcon(string skillID)
	{
		if (!this.GrantedSkillIDs.Contains(skillID))
		{
			return null;
		}
		BionicUpgradesMonitor.Instance smi = base.gameObject.GetSMI<BionicUpgradesMonitor.Instance>();
		if (smi != null)
		{
			foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in smi.upgradeComponentSlots)
			{
				if (upgradeComponentSlot.HasUpgradeInstalled)
				{
					return Def.GetUISprite(upgradeComponentSlot.installedUpgradeComponent.gameObject, "ui", false).first;
				}
			}
		}
		return Assets.GetSprite("skill_granted_trait");
	}

	// Token: 0x06007FB9 RID: 32697 RVA: 0x000F8A32 File Offset: 0x000F6C32
	private void TriggerMasterSkillEvents()
	{
		base.Trigger(540773776, null);
		Game.Instance.Trigger(-1523247426, this);
	}

	// Token: 0x06007FBA RID: 32698 RVA: 0x000F8A50 File Offset: 0x000F6C50
	public void ForceSetSkillPoints(int points)
	{
		this.totalExperienceGained = MinionResume.CalculatePreviousExperienceBar(points);
	}

	// Token: 0x06007FBB RID: 32699 RVA: 0x000F8A5E File Offset: 0x000F6C5E
	public void ForceAddSkillPoint()
	{
		this.AddExperience(MinionResume.CalculateNextExperienceBar(this.TotalSkillPointsGained) - this.totalExperienceGained);
	}

	// Token: 0x06007FBC RID: 32700 RVA: 0x000F8A78 File Offset: 0x000F6C78
	public static float CalculateNextExperienceBar(int current_skill_points)
	{
		return Mathf.Pow((float)(current_skill_points + 1) / (float)SKILLS.TARGET_SKILLS_EARNED, SKILLS.EXPERIENCE_LEVEL_POWER) * (float)SKILLS.TARGET_SKILLS_CYCLE * 600f;
	}

	// Token: 0x06007FBD RID: 32701 RVA: 0x000F8A9C File Offset: 0x000F6C9C
	public static float CalculatePreviousExperienceBar(int current_skill_points)
	{
		return Mathf.Pow((float)current_skill_points / (float)SKILLS.TARGET_SKILLS_EARNED, SKILLS.EXPERIENCE_LEVEL_POWER) * (float)SKILLS.TARGET_SKILLS_CYCLE * 600f;
	}

	// Token: 0x06007FBE RID: 32702 RVA: 0x0033D734 File Offset: 0x0033B934
	private void UpdateExpectations()
	{
		int num = 0;
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && !this.HasBeenGrantedSkill(keyValuePair.Key))
			{
				Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
				num += skill.tier + 1;
			}
		}
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(this);
		if (this.skillsMoraleExpectationModifier != null)
		{
			attributeInstance.Remove(this.skillsMoraleExpectationModifier);
			this.skillsMoraleExpectationModifier = null;
		}
		if (num > 0)
		{
			this.skillsMoraleExpectationModifier = new AttributeModifier(attributeInstance.Id, (float)num, DUPLICANTS.NEEDS.QUALITYOFLIFE.EXPECTATION_MOD_NAME, false, false, true);
			attributeInstance.Add(this.skillsMoraleExpectationModifier);
		}
	}

	// Token: 0x06007FBF RID: 32703 RVA: 0x0033D820 File Offset: 0x0033BA20
	private void UpdateMorale()
	{
		int num = 0;
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && !this.HasBeenGrantedSkill(keyValuePair.Key))
			{
				Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
				float num2 = 0f;
				if (this.AptitudeBySkillGroup.TryGetValue(new HashedString(skill.skillGroup), out num2))
				{
					num += (int)num2;
				}
			}
		}
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(this);
		if (this.skillsMoraleModifier != null)
		{
			attributeInstance.Remove(this.skillsMoraleModifier);
			this.skillsMoraleModifier = null;
		}
		if (num > 0)
		{
			this.skillsMoraleModifier = new AttributeModifier(attributeInstance.Id, (float)num, DUPLICANTS.NEEDS.QUALITYOFLIFE.APTITUDE_SKILLS_MOD_NAME, false, false, true);
			attributeInstance.Add(this.skillsMoraleModifier);
		}
	}

	// Token: 0x06007FC0 RID: 32704 RVA: 0x0033D928 File Offset: 0x0033BB28
	private void OnSkillPointGained()
	{
		Game.Instance.Trigger(1505456302, this);
		this.ShowNewSkillPointNotification();
		if (PopFXManager.Instance != null)
		{
			string text = MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.identity.GetProperName());
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, text, base.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
		}
		new UpgradeFX.Instance(base.gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, -0.1f)).StartSM();
	}

	// Token: 0x06007FC1 RID: 32705 RVA: 0x0033D9D4 File Offset: 0x0033BBD4
	private void ShowNewSkillPointNotification()
	{
		if (this.AvailableSkillpoints == 1)
		{
			this.lastSkillNotification = new ManagementMenuNotification(global::Action.ManageSkills, NotificationValence.Good, this.identity.GetSoleOwner().gameObject.GetInstanceID().ToString(), MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.identity.GetProperName()), NotificationType.Good, new Func<List<Notification>, object, string>(this.GetSkillPointGainedTooltip), this.identity, true, 0f, delegate(object d)
			{
				ManagementMenu.Instance.OpenSkills(this.identity);
			}, null, null, true);
			base.GetComponent<Notifier>().Add(this.lastSkillNotification, "");
		}
	}

	// Token: 0x06007FC2 RID: 32706 RVA: 0x000F8ABE File Offset: 0x000F6CBE
	private string GetSkillPointGainedTooltip(List<Notification> notifications, object data)
	{
		return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.TOOLTIP.Replace("{Duplicant}", ((MinionIdentity)data).GetProperName());
	}

	// Token: 0x06007FC3 RID: 32707 RVA: 0x000F8ADA File Offset: 0x000F6CDA
	public void SetAptitude(HashedString skillGroupID, float amount)
	{
		this.AptitudeBySkillGroup[skillGroupID] = amount;
	}

	// Token: 0x06007FC4 RID: 32708 RVA: 0x0033DA70 File Offset: 0x0033BC70
	public float GetAptitudeExperienceMultiplier(HashedString skillGroupId, float buildingFrequencyMultiplier)
	{
		float num = 0f;
		this.AptitudeBySkillGroup.TryGetValue(skillGroupId, out num);
		return 1f + num * SKILLS.APTITUDE_EXPERIENCE_MULTIPLIER * buildingFrequencyMultiplier;
	}

	// Token: 0x06007FC5 RID: 32709 RVA: 0x0033DAA4 File Offset: 0x0033BCA4
	public void AddExperience(float amount)
	{
		float num = this.totalExperienceGained;
		float num2 = MinionResume.CalculateNextExperienceBar(this.TotalSkillPointsGained);
		this.totalExperienceGained += amount;
		if (base.isSpawned && this.totalExperienceGained >= num2 && num < num2)
		{
			this.OnSkillPointGained();
		}
	}

	// Token: 0x06007FC6 RID: 32710 RVA: 0x0033DAF0 File Offset: 0x0033BCF0
	public override void AddExperienceWithAptitude(string skillGroupId, float amount, float buildingMultiplier)
	{
		float num = amount * this.GetAptitudeExperienceMultiplier(skillGroupId, buildingMultiplier) * SKILLS.ACTIVE_EXPERIENCE_PORTION;
		this.DEBUG_ActiveExperienceGained += num;
		this.AddExperience(num);
	}

	// Token: 0x06007FC7 RID: 32711 RVA: 0x0033DB28 File Offset: 0x0033BD28
	public bool HasPerk(HashedString perkId)
	{
		using (List<HashedString>.Enumerator enumerator = this.AdditionalGrantedSkillPerkIDs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == perkId)
				{
					return true;
				}
			}
		}
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).GivesPerk(perkId))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06007FC8 RID: 32712 RVA: 0x0033DBE8 File Offset: 0x0033BDE8
	public bool HasPerk(SkillPerk perk)
	{
		using (List<HashedString>.Enumerator enumerator = this.AdditionalGrantedSkillPerkIDs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == perk.IdHash)
				{
					return true;
				}
			}
		}
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).GivesPerk(perk))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06007FC9 RID: 32713 RVA: 0x000F8AE9 File Offset: 0x000F6CE9
	public void RemoveHat()
	{
		MinionResume.RemoveHat(base.GetComponent<KBatchedAnimController>());
		this.currentHat = null;
		this.targetHat = null;
	}

	// Token: 0x06007FCA RID: 32714 RVA: 0x0033DCB0 File Offset: 0x0033BEB0
	public static void RemoveHat(KBatchedAnimController controller)
	{
		AccessorySlot hat = Db.Get().AccessorySlots.Hat;
		Accessorizer component = controller.GetComponent<Accessorizer>();
		if (component != null)
		{
			Accessory accessory = component.GetAccessory(hat);
			if (accessory != null)
			{
				component.RemoveAccessory(accessory);
			}
		}
		else
		{
			controller.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride(hat.targetSymbolId, 4);
		}
		controller.SetSymbolVisiblity(hat.targetSymbolId, false);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
	}

	// Token: 0x06007FCB RID: 32715 RVA: 0x0033DD4C File Offset: 0x0033BF4C
	public static void AddHat(string hat_id, KBatchedAnimController controller)
	{
		AccessorySlot hat = Db.Get().AccessorySlots.Hat;
		Accessory accessory = hat.Lookup(hat_id);
		if (accessory == null)
		{
			global::Debug.LogWarning("Missing hat: " + hat_id);
		}
		Accessorizer component = controller.GetComponent<Accessorizer>();
		if (component != null)
		{
			Accessory accessory2 = component.GetAccessory(Db.Get().AccessorySlots.Hat);
			if (accessory2 != null)
			{
				component.RemoveAccessory(accessory2);
			}
			if (accessory != null)
			{
				component.AddAccessory(accessory);
			}
		}
		else
		{
			SymbolOverrideController component2 = controller.GetComponent<SymbolOverrideController>();
			component2.TryRemoveSymbolOverride(hat.targetSymbolId, 4);
			component2.AddSymbolOverride(hat.targetSymbolId, accessory.symbol, 4);
		}
		controller.SetSymbolVisiblity(hat.targetSymbolId, true);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
	}

	// Token: 0x06007FCC RID: 32716 RVA: 0x0033DE34 File Offset: 0x0033C034
	public void ApplyTargetHat()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		MinionResume.ApplyHat(this.targetHat, component);
		this.currentHat = this.targetHat;
		this.targetHat = null;
	}

	// Token: 0x06007FCD RID: 32717 RVA: 0x000F8B04 File Offset: 0x000F6D04
	public static void ApplyHat(string hat_id, KBatchedAnimController controller)
	{
		if (hat_id.IsNullOrWhiteSpace())
		{
			MinionResume.RemoveHat(controller);
			return;
		}
		MinionResume.AddHat(hat_id, controller);
	}

	// Token: 0x06007FCE RID: 32718 RVA: 0x000F8B1C File Offset: 0x000F6D1C
	public string GetSkillsSubtitle()
	{
		return string.Format(DUPLICANTS.NEEDS.QUALITYOFLIFE.TOTAL_SKILL_POINTS, this.TotalSkillPointsGained);
	}

	// Token: 0x06007FCF RID: 32719 RVA: 0x0033DE68 File Offset: 0x0033C068
	public static bool AnyMinionHasPerk(string perk, int worldId = -1)
	{
		using (List<MinionResume>.Enumerator enumerator = (from minion in (worldId >= 0) ? Components.MinionResumes.GetWorldItems(worldId, true) : Components.MinionResumes.Items
		where !minion.HasTag(GameTags.Dead)
		select minion).ToList<MinionResume>().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasPerk(perk))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06007FD0 RID: 32720 RVA: 0x0033DF08 File Offset: 0x0033C108
	public static bool AnyOtherMinionHasPerk(string perk, MinionResume me)
	{
		foreach (MinionResume minionResume in Components.MinionResumes.Items)
		{
			if (!(minionResume == me) && minionResume.HasPerk(perk))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06007FD1 RID: 32721 RVA: 0x0033DF78 File Offset: 0x0033C178
	public void ResetSkillLevels(bool returnSkillPoints = true)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (string skillId in list)
		{
			this.UnmasterSkill(skillId);
		}
	}

	// Token: 0x040060FF RID: 24831
	[MyCmpReq]
	private MinionIdentity identity;

	// Token: 0x04006100 RID: 24832
	[Serialize]
	public Dictionary<string, bool> MasteryByRoleID = new Dictionary<string, bool>();

	// Token: 0x04006101 RID: 24833
	[Serialize]
	public Dictionary<string, bool> MasteryBySkillID = new Dictionary<string, bool>();

	// Token: 0x04006102 RID: 24834
	[Serialize]
	public List<string> GrantedSkillIDs = new List<string>();

	// Token: 0x04006103 RID: 24835
	private List<HashedString> AdditionalGrantedSkillPerkIDs = new List<HashedString>();

	// Token: 0x04006104 RID: 24836
	private List<MinionResume.HatInfo> AdditionalHats = new List<MinionResume.HatInfo>();

	// Token: 0x04006105 RID: 24837
	[Serialize]
	public Dictionary<HashedString, float> AptitudeByRoleGroup = new Dictionary<HashedString, float>();

	// Token: 0x04006106 RID: 24838
	[Serialize]
	public Dictionary<HashedString, float> AptitudeBySkillGroup = new Dictionary<HashedString, float>();

	// Token: 0x04006107 RID: 24839
	[Serialize]
	private string currentRole = "NoRole";

	// Token: 0x04006108 RID: 24840
	[Serialize]
	private string targetRole = "NoRole";

	// Token: 0x04006109 RID: 24841
	[Serialize]
	private string currentHat;

	// Token: 0x0400610A RID: 24842
	[Serialize]
	private string targetHat;

	// Token: 0x0400610B RID: 24843
	private Dictionary<string, bool> ownedHats = new Dictionary<string, bool>();

	// Token: 0x0400610C RID: 24844
	[Serialize]
	private float totalExperienceGained;

	// Token: 0x0400610D RID: 24845
	private Notification lastSkillNotification;

	// Token: 0x0400610E RID: 24846
	private PutOnHatChore lastHatChore;

	// Token: 0x0400610F RID: 24847
	private AttributeModifier skillsMoraleExpectationModifier;

	// Token: 0x04006110 RID: 24848
	private AttributeModifier skillsMoraleModifier;

	// Token: 0x04006111 RID: 24849
	public float DEBUG_PassiveExperienceGained;

	// Token: 0x04006112 RID: 24850
	public float DEBUG_ActiveExperienceGained;

	// Token: 0x04006113 RID: 24851
	public float DEBUG_SecondsAlive;

	// Token: 0x02001842 RID: 6210
	public class HatInfo
	{
		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06007FD4 RID: 32724 RVA: 0x000F8B4A File Offset: 0x000F6D4A
		public string Source { get; }

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06007FD5 RID: 32725 RVA: 0x000F8B52 File Offset: 0x000F6D52
		public string Hat { get; }

		// Token: 0x06007FD6 RID: 32726 RVA: 0x000F8B5A File Offset: 0x000F6D5A
		public HatInfo(string source, string hat)
		{
			this.Source = source;
			this.Hat = hat;
			this.count = 1;
		}

		// Token: 0x04006116 RID: 24854
		public int count;
	}

	// Token: 0x02001843 RID: 6211
	public enum SkillMasteryConditions
	{
		// Token: 0x04006118 RID: 24856
		SkillAptitude,
		// Token: 0x04006119 RID: 24857
		StressWarning,
		// Token: 0x0400611A RID: 24858
		UnableToLearn,
		// Token: 0x0400611B RID: 24859
		NeedsSkillPoints,
		// Token: 0x0400611C RID: 24860
		MissingPreviousSkill
	}
}
