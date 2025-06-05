using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000AEA RID: 2794
[AddComponentMenu("KMonoBehaviour/scripts/Operational")]
public class Operational : KMonoBehaviour
{
	// Token: 0x17000223 RID: 547
	// (get) Token: 0x0600336C RID: 13164 RVA: 0x000C5FF6 File Offset: 0x000C41F6
	// (set) Token: 0x0600336D RID: 13165 RVA: 0x000C5FFE File Offset: 0x000C41FE
	public bool IsFunctional { get; private set; }

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x0600336E RID: 13166 RVA: 0x000C6007 File Offset: 0x000C4207
	// (set) Token: 0x0600336F RID: 13167 RVA: 0x000C600F File Offset: 0x000C420F
	public bool IsOperational { get; private set; }

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06003370 RID: 13168 RVA: 0x000C6018 File Offset: 0x000C4218
	// (set) Token: 0x06003371 RID: 13169 RVA: 0x000C6020 File Offset: 0x000C4220
	public bool IsActive { get; private set; }

	// Token: 0x06003372 RID: 13170 RVA: 0x000C6029 File Offset: 0x000C4229
	[OnSerializing]
	private void OnSerializing()
	{
		this.AddTimeData(this.IsActive);
		this.activeStartTime = GameClock.Instance.GetTime();
		this.inactiveStartTime = GameClock.Instance.GetTime();
	}

	// Token: 0x06003373 RID: 13171 RVA: 0x000C6057 File Offset: 0x000C4257
	protected override void OnPrefabInit()
	{
		this.UpdateFunctional();
		this.UpdateOperational();
		base.Subscribe<Operational>(-1661515756, Operational.OnNewBuildingDelegate);
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDay));
	}

	// Token: 0x06003374 RID: 13172 RVA: 0x00213A20 File Offset: 0x00211C20
	public void OnNewBuilding(object data)
	{
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.creationTime > 0f)
		{
			this.inactiveStartTime = component.creationTime;
			this.activeStartTime = component.creationTime;
		}
	}

	// Token: 0x06003375 RID: 13173 RVA: 0x000C6092 File Offset: 0x000C4292
	public bool IsOperationalType(Operational.Flag.Type type)
	{
		if (type == Operational.Flag.Type.Functional)
		{
			return this.IsFunctional;
		}
		return this.IsOperational;
	}

	// Token: 0x06003376 RID: 13174 RVA: 0x00213A5C File Offset: 0x00211C5C
	public void SetFlag(Operational.Flag flag, bool value)
	{
		bool flag2 = false;
		if (this.Flags.TryGetValue(flag, out flag2))
		{
			if (flag2 != value)
			{
				this.Flags[flag] = value;
				base.Trigger(187661686, flag);
			}
		}
		else
		{
			this.Flags[flag] = value;
			base.Trigger(187661686, flag);
		}
		if (flag.FlagType == Operational.Flag.Type.Functional && value != this.IsFunctional)
		{
			this.UpdateFunctional();
		}
		if (value != this.IsOperational)
		{
			this.UpdateOperational();
		}
	}

	// Token: 0x06003377 RID: 13175 RVA: 0x00213ADC File Offset: 0x00211CDC
	public bool GetFlag(Operational.Flag flag)
	{
		bool result = false;
		this.Flags.TryGetValue(flag, out result);
		return result;
	}

	// Token: 0x06003378 RID: 13176 RVA: 0x00213AFC File Offset: 0x00211CFC
	private void UpdateFunctional()
	{
		bool isFunctional = true;
		foreach (KeyValuePair<Operational.Flag, bool> keyValuePair in this.Flags)
		{
			if (keyValuePair.Key.FlagType == Operational.Flag.Type.Functional && !keyValuePair.Value)
			{
				isFunctional = false;
				break;
			}
		}
		this.IsFunctional = isFunctional;
		base.Trigger(-1852328367, this.IsFunctional);
	}

	// Token: 0x06003379 RID: 13177 RVA: 0x00213B84 File Offset: 0x00211D84
	private void UpdateOperational()
	{
		Dictionary<Operational.Flag, bool>.Enumerator enumerator = this.Flags.GetEnumerator();
		bool flag = true;
		while (enumerator.MoveNext())
		{
			KeyValuePair<Operational.Flag, bool> keyValuePair = enumerator.Current;
			if (!keyValuePair.Value)
			{
				flag = false;
				break;
			}
		}
		if (flag != this.IsOperational)
		{
			this.IsOperational = flag;
			if (!this.IsOperational)
			{
				this.SetActive(false, false);
			}
			if (this.IsOperational)
			{
				base.GetComponent<KPrefabID>().AddTag(GameTags.Operational, false);
			}
			else
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Operational);
			}
			base.Trigger(-592767678, this.IsOperational);
			Game.Instance.Trigger(-809948329, base.gameObject);
		}
	}

	// Token: 0x0600337A RID: 13178 RVA: 0x000C60A5 File Offset: 0x000C42A5
	public void SetActive(bool value, bool force_ignore = false)
	{
		if (this.IsActive != value)
		{
			this.AddTimeData(value);
			base.Trigger(824508782, this);
			Game.Instance.Trigger(-809948329, base.gameObject);
		}
	}

	// Token: 0x0600337B RID: 13179 RVA: 0x00213C38 File Offset: 0x00211E38
	private void AddTimeData(bool value)
	{
		float num = this.IsActive ? this.activeStartTime : this.inactiveStartTime;
		float time = GameClock.Instance.GetTime();
		float num2 = time - num;
		if (this.IsActive)
		{
			this.activeTime += num2;
		}
		else
		{
			this.inactiveTime += num2;
		}
		this.IsActive = value;
		if (this.IsActive)
		{
			this.activeStartTime = time;
			return;
		}
		this.inactiveStartTime = time;
	}

	// Token: 0x0600337C RID: 13180 RVA: 0x00213CB0 File Offset: 0x00211EB0
	public void OnNewDay(object data)
	{
		this.AddTimeData(this.IsActive);
		this.uptimeData.Add(this.activeTime / 600f);
		while (this.uptimeData.Count > this.MAX_DATA_POINTS)
		{
			this.uptimeData.RemoveAt(0);
		}
		this.activeTime = 0f;
		this.inactiveTime = 0f;
	}

	// Token: 0x0600337D RID: 13181 RVA: 0x00213D18 File Offset: 0x00211F18
	public float GetCurrentCycleUptime()
	{
		if (this.IsActive)
		{
			float num = this.IsActive ? this.activeStartTime : this.inactiveStartTime;
			float num2 = GameClock.Instance.GetTime() - num;
			return (this.activeTime + num2) / GameClock.Instance.GetTimeSinceStartOfCycle();
		}
		return this.activeTime / GameClock.Instance.GetTimeSinceStartOfCycle();
	}

	// Token: 0x0600337E RID: 13182 RVA: 0x000C60D8 File Offset: 0x000C42D8
	public float GetLastCycleUptime()
	{
		if (this.uptimeData.Count > 0)
		{
			return this.uptimeData[this.uptimeData.Count - 1];
		}
		return 0f;
	}

	// Token: 0x0600337F RID: 13183 RVA: 0x00213D78 File Offset: 0x00211F78
	public float GetUptimeOverCycles(int num_cycles)
	{
		if (this.uptimeData.Count > 0)
		{
			int num = Mathf.Min(this.uptimeData.Count, num_cycles);
			float num2 = 0f;
			for (int i = num - 1; i >= 0; i--)
			{
				num2 += this.uptimeData[i];
			}
			return num2 / (float)num;
		}
		return 0f;
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x000C6106 File Offset: 0x000C4306
	public bool MeetsRequirements(Operational.State stateRequirement)
	{
		switch (stateRequirement)
		{
		case Operational.State.Operational:
			return this.IsOperational;
		case Operational.State.Functional:
			return this.IsFunctional;
		case Operational.State.Active:
			return this.IsActive;
		}
		return true;
	}

	// Token: 0x06003381 RID: 13185 RVA: 0x000C6136 File Offset: 0x000C4336
	public static GameHashes GetEventForState(Operational.State state)
	{
		if (state == Operational.State.Operational)
		{
			return GameHashes.OperationalChanged;
		}
		if (state == Operational.State.Functional)
		{
			return GameHashes.FunctionalChanged;
		}
		return GameHashes.ActiveChanged;
	}

	// Token: 0x04002340 RID: 9024
	[Serialize]
	public float inactiveStartTime;

	// Token: 0x04002341 RID: 9025
	[Serialize]
	public float activeStartTime;

	// Token: 0x04002342 RID: 9026
	[Serialize]
	private List<float> uptimeData = new List<float>();

	// Token: 0x04002343 RID: 9027
	[Serialize]
	private float activeTime;

	// Token: 0x04002344 RID: 9028
	[Serialize]
	private float inactiveTime;

	// Token: 0x04002345 RID: 9029
	private int MAX_DATA_POINTS = 5;

	// Token: 0x04002346 RID: 9030
	public Dictionary<Operational.Flag, bool> Flags = new Dictionary<Operational.Flag, bool>();

	// Token: 0x04002347 RID: 9031
	private static readonly EventSystem.IntraObjectHandler<Operational> OnNewBuildingDelegate = new EventSystem.IntraObjectHandler<Operational>(delegate(Operational component, object data)
	{
		component.OnNewBuilding(data);
	});

	// Token: 0x02000AEB RID: 2795
	public enum State
	{
		// Token: 0x04002349 RID: 9033
		Operational,
		// Token: 0x0400234A RID: 9034
		Functional,
		// Token: 0x0400234B RID: 9035
		Active,
		// Token: 0x0400234C RID: 9036
		None
	}

	// Token: 0x02000AEC RID: 2796
	public class Flag
	{
		// Token: 0x06003384 RID: 13188 RVA: 0x000C6191 File Offset: 0x000C4391
		public Flag(string name, Operational.Flag.Type type)
		{
			this.Name = name;
			this.FlagType = type;
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x000C61A7 File Offset: 0x000C43A7
		public static Operational.Flag.Type GetFlagType(Operational.State operationalState)
		{
			switch (operationalState)
			{
			case Operational.State.Operational:
			case Operational.State.Active:
				return Operational.Flag.Type.Requirement;
			case Operational.State.Functional:
				return Operational.Flag.Type.Functional;
			}
			throw new InvalidOperationException("Can not convert NONE state to an Operational Flag Type");
		}

		// Token: 0x0400234D RID: 9037
		public string Name;

		// Token: 0x0400234E RID: 9038
		public Operational.Flag.Type FlagType;

		// Token: 0x02000AED RID: 2797
		public enum Type
		{
			// Token: 0x04002350 RID: 9040
			Requirement,
			// Token: 0x04002351 RID: 9041
			Functional
		}
	}
}
