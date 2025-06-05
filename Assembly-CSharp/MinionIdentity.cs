using System;
using System.Collections.Generic;
using Klei.AI;
using Klei.CustomSettings;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000AC6 RID: 2758
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MinionIdentity")]
public class MinionIdentity : KMonoBehaviour, ISaveLoadable, IAssignableIdentity, IListableOption, ISim1000ms
{
	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06003261 RID: 12897 RVA: 0x000C525D File Offset: 0x000C345D
	// (set) Token: 0x06003262 RID: 12898 RVA: 0x000C5265 File Offset: 0x000C3465
	[Serialize]
	public string genderStringKey { get; set; }

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06003263 RID: 12899 RVA: 0x000C526E File Offset: 0x000C346E
	// (set) Token: 0x06003264 RID: 12900 RVA: 0x000C5276 File Offset: 0x000C3476
	[Serialize]
	public string nameStringKey { get; set; }

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06003265 RID: 12901 RVA: 0x000C527F File Offset: 0x000C347F
	// (set) Token: 0x06003266 RID: 12902 RVA: 0x000C5287 File Offset: 0x000C3487
	[Serialize]
	public HashedString personalityResourceId { get; set; }

	// Token: 0x06003267 RID: 12903 RVA: 0x000C5290 File Offset: 0x000C3490
	public static void DestroyStatics()
	{
		MinionIdentity.maleNameList = null;
		MinionIdentity.femaleNameList = null;
	}

	// Token: 0x06003268 RID: 12904 RVA: 0x002106B4 File Offset: 0x0020E8B4
	protected override void OnPrefabInit()
	{
		if (this.name == null)
		{
			this.name = MinionIdentity.ChooseRandomName();
		}
		if (GameClock.Instance != null)
		{
			this.arrivalTime = (float)GameClock.Instance.GetCycle();
		}
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		if (component != null)
		{
			KAnimControllerBase kanimControllerBase = component;
			kanimControllerBase.OnUpdateBounds = (Action<Bounds>)Delegate.Combine(kanimControllerBase.OnUpdateBounds, new Action<Bounds>(this.OnUpdateBounds));
		}
		GameUtil.SubscribeToTags<MinionIdentity>(this, MinionIdentity.OnDeadTagAddedDelegate, true);
		base.Subscribe<MinionIdentity>(1502190696, MinionIdentity.OnQueueDestroyObjectDelegate);
	}

	// Token: 0x06003269 RID: 12905 RVA: 0x00210744 File Offset: 0x0020E944
	protected override void OnSpawn()
	{
		if (this.addToIdentityList)
		{
			this.ValidateProxy();
			this.CleanupLimboMinions();
		}
		PathProber component = base.GetComponent<PathProber>();
		if (component != null)
		{
			component.SetGroupProber(MinionGroupProber.Get());
		}
		this.SetName(this.name);
		if (this.nameStringKey == null)
		{
			this.nameStringKey = this.name;
		}
		this.SetGender(this.gender);
		if (this.genderStringKey == null)
		{
			this.genderStringKey = "NB";
		}
		if (this.personalityResourceId == HashedString.Invalid)
		{
			Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(this.nameStringKey);
			if (personalityFromNameStringKey != null)
			{
				this.personalityResourceId = personalityFromNameStringKey.Id;
			}
		}
		if (!this.model.IsValid)
		{
			Personality personalityFromNameStringKey2 = Db.Get().Personalities.GetPersonalityFromNameStringKey(this.nameStringKey);
			if (personalityFromNameStringKey2 != null)
			{
				this.model = personalityFromNameStringKey2.model;
			}
		}
		if (this.addToIdentityList)
		{
			Components.MinionIdentities.Add(this);
			if (!Components.MinionIdentitiesByModel.ContainsKey(this.model))
			{
				Components.MinionIdentitiesByModel[this.model] = new Components.Cmps<MinionIdentity>();
			}
			Components.MinionIdentitiesByModel[this.model].Add(this);
			if (!base.gameObject.HasTag(GameTags.Dead))
			{
				Components.LiveMinionIdentities.Add(this);
				if (!Components.LiveMinionIdentitiesByModel.ContainsKey(this.model))
				{
					Components.LiveMinionIdentitiesByModel[this.model] = new Components.Cmps<MinionIdentity>();
				}
				Components.LiveMinionIdentitiesByModel[this.model].Add(this);
				Game.Instance.Trigger(2144209314, this);
			}
		}
		SymbolOverrideController component2 = base.GetComponent<SymbolOverrideController>();
		if (component2 != null)
		{
			Accessorizer component3 = base.gameObject.GetComponent<Accessorizer>();
			if (component3 != null)
			{
				string str = HashCache.Get().Get(component3.GetAccessory(Db.Get().AccessorySlots.Mouth).symbol.hash).Replace("mouth", "cheek");
				component2.AddSymbolOverride("snapto_cheek", Assets.GetAnim("head_swap_kanim").GetData().build.GetSymbol(str), 1);
				component2.AddSymbolOverride("snapto_hair_always", component3.GetAccessory(Db.Get().AccessorySlots.Hair).symbol, 1);
				component2.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(component3.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
			}
		}
		this.voiceId = (this.voiceIdx + 1).ToString("D2");
		Prioritizable component4 = base.GetComponent<Prioritizable>();
		if (component4 != null)
		{
			component4.showIcon = false;
		}
		Pickupable component5 = base.GetComponent<Pickupable>();
		if (component5 != null)
		{
			component5.carryAnimOverride = Assets.GetAnim("anim_incapacitated_carrier_kanim");
		}
		this.ApplyCustomGameSettings();
	}

	// Token: 0x0600326A RID: 12906 RVA: 0x000C529E File Offset: 0x000C349E
	public void ValidateProxy()
	{
		this.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(this.assignableProxy, this);
	}

	// Token: 0x0600326B RID: 12907 RVA: 0x00210A88 File Offset: 0x0020EC88
	private void CleanupLimboMinions()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (component.InstanceID == -1)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Minion with an invalid kpid! Attempting to recover...",
				this.name
			});
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		if (component.conflicted)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Minion with a conflicted kpid! Attempting to recover... ",
				component.InstanceID,
				this.name
			});
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		this.assignableProxy.Get().SetTarget(this, base.gameObject);
	}

	// Token: 0x0600326C RID: 12908 RVA: 0x000C52B2 File Offset: 0x000C34B2
	public string GetProperName()
	{
		return base.gameObject.GetProperName();
	}

	// Token: 0x0600326D RID: 12909 RVA: 0x000C52BF File Offset: 0x000C34BF
	public string GetVoiceId()
	{
		return this.voiceId;
	}

	// Token: 0x0600326E RID: 12910 RVA: 0x000C52C7 File Offset: 0x000C34C7
	public void SetName(string name)
	{
		this.name = name;
		if (this.selectable != null)
		{
			this.selectable.SetName(name);
		}
		base.gameObject.name = name;
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
	}

	// Token: 0x0600326F RID: 12911 RVA: 0x000C5306 File Offset: 0x000C3506
	public void SetStickerType(string stickerType)
	{
		this.stickerType = stickerType;
	}

	// Token: 0x06003270 RID: 12912 RVA: 0x000C530F File Offset: 0x000C350F
	public bool IsNull()
	{
		return this == null;
	}

	// Token: 0x06003271 RID: 12913 RVA: 0x000C5318 File Offset: 0x000C3518
	public void SetGender(string gender)
	{
		this.gender = gender;
		this.selectable.SetGender(gender);
	}

	// Token: 0x06003272 RID: 12914 RVA: 0x00210BC4 File Offset: 0x0020EDC4
	public static string ChooseRandomName()
	{
		if (MinionIdentity.femaleNameList == null)
		{
			MinionIdentity.maleNameList = new MinionIdentity.NameList(Game.Instance.maleNamesFile);
			MinionIdentity.femaleNameList = new MinionIdentity.NameList(Game.Instance.femaleNamesFile);
		}
		if (UnityEngine.Random.value > 0.5f)
		{
			return MinionIdentity.maleNameList.Next();
		}
		return MinionIdentity.femaleNameList.Next();
	}

	// Token: 0x06003273 RID: 12915 RVA: 0x000C532D File Offset: 0x000C352D
	private void OnQueueDestroyObject()
	{
		this.RemoveFromComponentsLists();
	}

	// Token: 0x06003274 RID: 12916 RVA: 0x00210C24 File Offset: 0x0020EE24
	private void RemoveFromComponentsLists()
	{
		Components.MinionIdentities.Remove(this);
		if (Components.MinionIdentitiesByModel.ContainsKey(this.model))
		{
			Components.MinionIdentitiesByModel[this.model].Remove(this);
		}
		Components.LiveMinionIdentities.Remove(this);
		if (Components.LiveMinionIdentitiesByModel.ContainsKey(this.model))
		{
			Components.LiveMinionIdentitiesByModel[this.model].Remove(this);
		}
	}

	// Token: 0x06003275 RID: 12917 RVA: 0x00210C98 File Offset: 0x0020EE98
	protected override void OnCleanUp()
	{
		if (this.assignableProxy != null)
		{
			MinionAssignablesProxy minionAssignablesProxy = this.assignableProxy.Get();
			if (minionAssignablesProxy && minionAssignablesProxy.target == this)
			{
				Util.KDestroyGameObject(minionAssignablesProxy.gameObject);
			}
		}
		this.RemoveFromComponentsLists();
		Game.Instance.Trigger(2144209314, this);
	}

	// Token: 0x06003276 RID: 12918 RVA: 0x000C5335 File Offset: 0x000C3535
	private void OnUpdateBounds(Bounds bounds)
	{
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		component.offset = bounds.center;
		component.size = bounds.extents;
	}

	// Token: 0x06003277 RID: 12919 RVA: 0x00210CEC File Offset: 0x0020EEEC
	private void OnDied(object data)
	{
		this.GetSoleOwner().UnassignAll();
		this.GetEquipment().UnequipAll();
		Components.LiveMinionIdentities.Remove(this);
		if (Components.LiveMinionIdentitiesByModel.ContainsKey(this.model))
		{
			Components.LiveMinionIdentitiesByModel[this.model].Remove(this);
		}
		Game.Instance.Trigger(-1523247426, this);
		Game.Instance.Trigger(2144209314, this);
	}

	// Token: 0x06003278 RID: 12920 RVA: 0x000C5360 File Offset: 0x000C3560
	public List<Ownables> GetOwners()
	{
		return this.assignableProxy.Get().ownables;
	}

	// Token: 0x06003279 RID: 12921 RVA: 0x000C5372 File Offset: 0x000C3572
	public Ownables GetSoleOwner()
	{
		return this.assignableProxy.Get().GetComponent<Ownables>();
	}

	// Token: 0x0600327A RID: 12922 RVA: 0x000C5384 File Offset: 0x000C3584
	public bool HasOwner(Assignables owner)
	{
		return this.GetOwners().Contains(owner as Ownables);
	}

	// Token: 0x0600327B RID: 12923 RVA: 0x000C5397 File Offset: 0x000C3597
	public int NumOwners()
	{
		return this.GetOwners().Count;
	}

	// Token: 0x0600327C RID: 12924 RVA: 0x000C53A4 File Offset: 0x000C35A4
	public Equipment GetEquipment()
	{
		return this.assignableProxy.Get().GetComponent<Equipment>();
	}

	// Token: 0x0600327D RID: 12925 RVA: 0x00210D64 File Offset: 0x0020EF64
	public void Sim1000ms(float dt)
	{
		if (this == null)
		{
			return;
		}
		if (this.navigator == null)
		{
			this.navigator = base.GetComponent<Navigator>();
		}
		if (this.navigator != null && !this.navigator.IsMoving())
		{
			return;
		}
		if (this.choreDriver == null)
		{
			this.choreDriver = base.GetComponent<ChoreDriver>();
		}
		if (this.choreDriver != null)
		{
			Chore currentChore = this.choreDriver.GetCurrentChore();
			if (currentChore != null && currentChore is FetchAreaChore)
			{
				MinionResume component = base.GetComponent<MinionResume>();
				if (component != null)
				{
					component.AddExperienceWithAptitude(Db.Get().SkillGroups.Hauling.Id, dt, SKILLS.ALL_DAY_EXPERIENCE);
				}
			}
		}
	}

	// Token: 0x0600327E RID: 12926 RVA: 0x00210E20 File Offset: 0x0020F020
	private void ApplyCustomGameSettings()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ImmuneSystem);
		if (currentQualitySetting.id == "Compromised")
		{
			Db.Get().Attributes.DiseaseCureSpeed.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, -0.3333f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, -2f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting.id == "Weak")
		{
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, -1f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.WEAK.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting.id == "Strong")
		{
			Db.Get().Attributes.DiseaseCureSpeed.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, 2f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, 2f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting.id == "Invincible")
		{
			Db.Get().Attributes.DiseaseCureSpeed.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, 100000000f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, 200f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		SettingLevel currentQualitySetting2 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Stress);
		if (currentQualitySetting2.id == "Doomed")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.033333335f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.DOOMED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting2.id == "Pessimistic")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.016666668f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.PESSIMISTIC.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting2.id == "Optimistic")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.016666668f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.OPTIMISTIC.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting2.id == "Indomitable")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, float.NegativeInfinity, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.INDOMITABLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		SettingLevel currentQualitySetting3 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CalorieBurn);
		if (currentQualitySetting3.id == "VeryHard")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * 1f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.VERYHARD.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			return;
		}
		if (currentQualitySetting3.id == "Hard")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * 0.5f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.HARD.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			return;
		}
		if (currentQualitySetting3.id == "Easy")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * -0.5f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.EASY.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			return;
		}
		if (currentQualitySetting3.id == "Disabled")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, float.PositiveInfinity, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.DISABLED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
	}

	// Token: 0x0600327F RID: 12927 RVA: 0x002113E8 File Offset: 0x0020F5E8
	public static float GetCalorieBurnMultiplier()
	{
		float result = 1f;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CalorieBurn);
		if (currentQualitySetting.id == "VeryHard")
		{
			result = 2f;
		}
		else if (currentQualitySetting.id == "Hard")
		{
			result = 1.5f;
		}
		else if (currentQualitySetting.id == "Easy")
		{
			result = 0.5f;
		}
		else if (currentQualitySetting.id == "Disabled")
		{
			result = 0f;
		}
		return result;
	}

	// Token: 0x0400227C RID: 8828
	public const string HairAlwaysSymbol = "snapto_hair_always";

	// Token: 0x0400227D RID: 8829
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x0400227E RID: 8830
	[MyCmpReq]
	public Modifiers modifiers;

	// Token: 0x0400227F RID: 8831
	public int femaleVoiceCount;

	// Token: 0x04002280 RID: 8832
	public int maleVoiceCount;

	// Token: 0x04002281 RID: 8833
	[Serialize]
	public Tag model;

	// Token: 0x04002282 RID: 8834
	[Serialize]
	private new string name;

	// Token: 0x04002283 RID: 8835
	[Serialize]
	public string gender;

	// Token: 0x04002287 RID: 8839
	[Serialize]
	public string stickerType;

	// Token: 0x04002288 RID: 8840
	[Serialize]
	[ReadOnly]
	public float arrivalTime;

	// Token: 0x04002289 RID: 8841
	[Serialize]
	public int voiceIdx;

	// Token: 0x0400228A RID: 8842
	[Serialize]
	public Ref<MinionAssignablesProxy> assignableProxy;

	// Token: 0x0400228B RID: 8843
	private Navigator navigator;

	// Token: 0x0400228C RID: 8844
	private ChoreDriver choreDriver;

	// Token: 0x0400228D RID: 8845
	public float timeLastSpoke;

	// Token: 0x0400228E RID: 8846
	private string voiceId;

	// Token: 0x0400228F RID: 8847
	private KAnimHashedString overrideExpression;

	// Token: 0x04002290 RID: 8848
	private KAnimHashedString expression;

	// Token: 0x04002291 RID: 8849
	public bool addToIdentityList = true;

	// Token: 0x04002292 RID: 8850
	private static MinionIdentity.NameList maleNameList;

	// Token: 0x04002293 RID: 8851
	private static MinionIdentity.NameList femaleNameList;

	// Token: 0x04002294 RID: 8852
	private static readonly EventSystem.IntraObjectHandler<MinionIdentity> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<MinionIdentity>(GameTags.Dead, delegate(MinionIdentity component, object data)
	{
		component.OnDied(data);
	});

	// Token: 0x04002295 RID: 8853
	private static readonly EventSystem.IntraObjectHandler<MinionIdentity> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<MinionIdentity>(delegate(MinionIdentity component, object data)
	{
		component.OnQueueDestroyObject();
	});

	// Token: 0x02000AC7 RID: 2759
	private class NameList
	{
		// Token: 0x06003282 RID: 12930 RVA: 0x00211474 File Offset: 0x0020F674
		public NameList(TextAsset file)
		{
			string[] array = file.text.Replace("  ", " ").Replace("\r\n", "\n").Split('\n', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(' ', StringSplitOptions.None);
				if (array2[array2.Length - 1] != "" && array2[array2.Length - 1] != null)
				{
					this.names.Add(array2[array2.Length - 1]);
				}
			}
			this.names.Shuffle<string>();
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x00211514 File Offset: 0x0020F714
		public string Next()
		{
			List<string> list = this.names;
			int num = this.idx;
			this.idx = num + 1;
			return list[num % this.names.Count];
		}

		// Token: 0x04002296 RID: 8854
		private List<string> names = new List<string>();

		// Token: 0x04002297 RID: 8855
		private int idx;
	}
}
