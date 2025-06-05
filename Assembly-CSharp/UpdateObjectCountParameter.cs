using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000960 RID: 2400
internal class UpdateObjectCountParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06002AEC RID: 10988 RVA: 0x001E9278 File Offset: 0x001E7478
	public static UpdateObjectCountParameter.Settings GetSettings(HashedString path_hash, SoundDescription description)
	{
		UpdateObjectCountParameter.Settings settings = default(UpdateObjectCountParameter.Settings);
		if (!UpdateObjectCountParameter.settings.TryGetValue(path_hash, out settings))
		{
			settings = default(UpdateObjectCountParameter.Settings);
			EventDescription eventDescription = RuntimeManager.GetEventDescription(description.path);
			USER_PROPERTY user_PROPERTY;
			if (eventDescription.getUserProperty("minObj", out user_PROPERTY) == RESULT.OK)
			{
				settings.minObjects = (float)((short)user_PROPERTY.floatValue());
			}
			else
			{
				settings.minObjects = 1f;
			}
			USER_PROPERTY user_PROPERTY2;
			if (eventDescription.getUserProperty("maxObj", out user_PROPERTY2) == RESULT.OK)
			{
				settings.maxObjects = user_PROPERTY2.floatValue();
			}
			else
			{
				settings.maxObjects = 0f;
			}
			USER_PROPERTY user_PROPERTY3;
			if (eventDescription.getUserProperty("curveType", out user_PROPERTY3) == RESULT.OK && user_PROPERTY3.stringValue() == "exp")
			{
				settings.useExponentialCurve = true;
			}
			settings.parameterId = description.GetParameterId(UpdateObjectCountParameter.parameterHash);
			settings.path = path_hash;
			UpdateObjectCountParameter.settings[path_hash] = settings;
		}
		return settings;
	}

	// Token: 0x06002AED RID: 10989 RVA: 0x001E9360 File Offset: 0x001E7560
	public static void ApplySettings(EventInstance ev, int count, UpdateObjectCountParameter.Settings settings)
	{
		float num = 0f;
		if (settings.maxObjects != settings.minObjects)
		{
			num = ((float)count - settings.minObjects) / (settings.maxObjects - settings.minObjects);
			num = Mathf.Clamp01(num);
		}
		if (settings.useExponentialCurve)
		{
			num *= num;
		}
		ev.setParameterByID(settings.parameterId, num, false);
	}

	// Token: 0x06002AEE RID: 10990 RVA: 0x000C059B File Offset: 0x000BE79B
	public UpdateObjectCountParameter() : base("objectCount")
	{
	}

	// Token: 0x06002AEF RID: 10991 RVA: 0x001E93BC File Offset: 0x001E75BC
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateObjectCountParameter.Settings settings = UpdateObjectCountParameter.GetSettings(sound.path, sound.description);
		UpdateObjectCountParameter.Entry item = new UpdateObjectCountParameter.Entry
		{
			ev = sound.ev,
			settings = settings
		};
		this.entries.Add(item);
	}

	// Token: 0x06002AF0 RID: 10992 RVA: 0x001E9408 File Offset: 0x001E7608
	public override void Update(float dt)
	{
		DictionaryPool<HashedString, int, LoopingSoundManager>.PooledDictionary pooledDictionary = DictionaryPool<HashedString, int, LoopingSoundManager>.Allocate();
		foreach (UpdateObjectCountParameter.Entry entry in this.entries)
		{
			int num = 0;
			pooledDictionary.TryGetValue(entry.settings.path, out num);
			num = (pooledDictionary[entry.settings.path] = num + 1);
		}
		foreach (UpdateObjectCountParameter.Entry entry2 in this.entries)
		{
			int count = pooledDictionary[entry2.settings.path];
			UpdateObjectCountParameter.ApplySettings(entry2.ev, count, entry2.settings);
		}
		pooledDictionary.Recycle();
	}

	// Token: 0x06002AF1 RID: 10993 RVA: 0x001E94F4 File Offset: 0x001E76F4
	public override void Remove(LoopingSoundParameterUpdater.Sound sound)
	{
		for (int i = 0; i < this.entries.Count; i++)
		{
			if (this.entries[i].ev.handle == sound.ev.handle)
			{
				this.entries.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06002AF2 RID: 10994 RVA: 0x000C05B8 File Offset: 0x000BE7B8
	public static void Clear()
	{
		UpdateObjectCountParameter.settings.Clear();
	}

	// Token: 0x04001D13 RID: 7443
	private List<UpdateObjectCountParameter.Entry> entries = new List<UpdateObjectCountParameter.Entry>();

	// Token: 0x04001D14 RID: 7444
	private static Dictionary<HashedString, UpdateObjectCountParameter.Settings> settings = new Dictionary<HashedString, UpdateObjectCountParameter.Settings>();

	// Token: 0x04001D15 RID: 7445
	private static readonly HashedString parameterHash = "objectCount";

	// Token: 0x02000961 RID: 2401
	private struct Entry
	{
		// Token: 0x04001D16 RID: 7446
		public EventInstance ev;

		// Token: 0x04001D17 RID: 7447
		public UpdateObjectCountParameter.Settings settings;
	}

	// Token: 0x02000962 RID: 2402
	public struct Settings
	{
		// Token: 0x04001D18 RID: 7448
		public HashedString path;

		// Token: 0x04001D19 RID: 7449
		public PARAMETER_ID parameterId;

		// Token: 0x04001D1A RID: 7450
		public float minObjects;

		// Token: 0x04001D1B RID: 7451
		public float maxObjects;

		// Token: 0x04001D1C RID: 7452
		public bool useExponentialCurve;
	}
}
