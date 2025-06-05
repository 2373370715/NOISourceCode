using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x0200175E RID: 5982
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/PrimaryElement")]
public class PrimaryElement : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06007B07 RID: 31495 RVA: 0x000F5827 File Offset: 0x000F3A27
	public void SetUseSimDiseaseInfo(bool use)
	{
		this.useSimDiseaseInfo = use;
	}

	// Token: 0x170007AA RID: 1962
	// (get) Token: 0x06007B08 RID: 31496 RVA: 0x000F5830 File Offset: 0x000F3A30
	// (set) Token: 0x06007B09 RID: 31497 RVA: 0x00328138 File Offset: 0x00326338
	[Serialize]
	public float Units
	{
		get
		{
			return this._units;
		}
		set
		{
			if (float.IsInfinity(value) || float.IsNaN(value))
			{
				DebugUtil.DevLogError("Invalid units value for element, setting Units to 0");
				this._units = 0f;
			}
			else
			{
				this._units = value;
			}
			if (this.onDataChanged != null)
			{
				this.onDataChanged(this);
			}
		}
	}

	// Token: 0x170007AB RID: 1963
	// (get) Token: 0x06007B0A RID: 31498 RVA: 0x000F5838 File Offset: 0x000F3A38
	// (set) Token: 0x06007B0B RID: 31499 RVA: 0x000F5846 File Offset: 0x000F3A46
	public float Temperature
	{
		get
		{
			return this.getTemperatureCallback(this);
		}
		set
		{
			this.SetTemperature(value);
		}
	}

	// Token: 0x170007AC RID: 1964
	// (get) Token: 0x06007B0C RID: 31500 RVA: 0x000F584F File Offset: 0x000F3A4F
	// (set) Token: 0x06007B0D RID: 31501 RVA: 0x000F5857 File Offset: 0x000F3A57
	public float InternalTemperature
	{
		get
		{
			return this._Temperature;
		}
		set
		{
			this._Temperature = value;
		}
	}

	// Token: 0x06007B0E RID: 31502 RVA: 0x00328188 File Offset: 0x00326388
	[OnSerializing]
	private void OnSerializing()
	{
		this._Temperature = this.Temperature;
		this.SanitizeMassAndTemperature();
		this.diseaseID.HashValue = 0;
		this.diseaseCount = 0;
		if (this.useSimDiseaseInfo)
		{
			int i = Grid.PosToCell(base.transform.GetPosition());
			if (Grid.DiseaseIdx[i] != 255)
			{
				this.diseaseID = Db.Get().Diseases[(int)Grid.DiseaseIdx[i]].id;
				this.diseaseCount = Grid.DiseaseCount[i];
				return;
			}
		}
		else if (this.diseaseHandle.IsValid())
		{
			DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle);
			if (header.diseaseIdx != 255)
			{
				this.diseaseID = Db.Get().Diseases[(int)header.diseaseIdx].id;
				this.diseaseCount = header.diseaseCount;
			}
		}
	}

	// Token: 0x06007B0F RID: 31503 RVA: 0x00328278 File Offset: 0x00326478
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.ElementID == (SimHashes)351109216)
		{
			this.ElementID = SimHashes.Creature;
		}
		this.SanitizeMassAndTemperature();
		float temperature = this._Temperature;
		if (float.IsNaN(temperature) || float.IsInfinity(temperature) || temperature < 0f || 10000f < temperature)
		{
			DeserializeWarnings.Instance.PrimaryElementTemperatureIsNan.Warn(string.Format("{0} has invalid temperature of {1}. Resetting temperature.", base.name, this.Temperature), null);
			temperature = this.Element.defaultValues.temperature;
		}
		this._Temperature = temperature;
		this.Temperature = temperature;
		if (this.Element == null)
		{
			DeserializeWarnings.Instance.PrimaryElementHasNoElement.Warn(base.name + "Primary element has no element.", null);
		}
		if (this.Mass < 0f)
		{
			DebugUtil.DevLogError(base.gameObject, "deserialized ore with less than 0 mass. Error! Destroying");
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		if (this.Mass == 0f && !this.KeepZeroMassObject)
		{
			DebugUtil.DevLogError(base.gameObject, "deserialized element with 0 mass. Destroying");
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		if (this.onDataChanged != null)
		{
			this.onDataChanged(this);
		}
		byte index = Db.Get().Diseases.GetIndex(this.diseaseID);
		if (index == 255 || this.diseaseCount <= 0)
		{
			if (this.diseaseHandle.IsValid())
			{
				GameComps.DiseaseContainers.Remove(base.gameObject);
				this.diseaseHandle.Clear();
				return;
			}
		}
		else
		{
			if (this.diseaseHandle.IsValid())
			{
				DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle);
				header.diseaseIdx = index;
				header.diseaseCount = this.diseaseCount;
				GameComps.DiseaseContainers.SetHeader(this.diseaseHandle, header);
				return;
			}
			this.diseaseHandle = GameComps.DiseaseContainers.Add(base.gameObject, index, this.diseaseCount);
		}
	}

	// Token: 0x06007B10 RID: 31504 RVA: 0x000F5860 File Offset: 0x000F3A60
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
	}

	// Token: 0x06007B11 RID: 31505 RVA: 0x0032845C File Offset: 0x0032665C
	private void SanitizeMassAndTemperature()
	{
		if (this._Temperature <= 0f)
		{
			DebugUtil.DevLogError(base.gameObject.name + " is attempting to serialize a temperature of <= 0K. Resetting to default. world=" + base.gameObject.DebugGetMyWorldName());
			this._Temperature = this.Element.defaultValues.temperature;
		}
		if (this.Mass > PrimaryElement.MAX_MASS)
		{
			DebugUtil.DevLogError(string.Format("{0} is attempting to serialize very large mass {1}. Resetting to default. world={2}", base.gameObject.name, this.Mass, base.gameObject.DebugGetMyWorldName()));
			this.Mass = this.Element.defaultValues.mass;
		}
	}

	// Token: 0x170007AD RID: 1965
	// (get) Token: 0x06007B12 RID: 31506 RVA: 0x000F5868 File Offset: 0x000F3A68
	// (set) Token: 0x06007B13 RID: 31507 RVA: 0x000F5877 File Offset: 0x000F3A77
	public float Mass
	{
		get
		{
			return this.Units * this.MassPerUnit;
		}
		set
		{
			this.SetMass(value);
			if (this.onDataChanged != null)
			{
				this.onDataChanged(this);
			}
		}
	}

	// Token: 0x06007B14 RID: 31508 RVA: 0x00328504 File Offset: 0x00326704
	private void SetMass(float mass)
	{
		if ((mass > PrimaryElement.MAX_MASS || mass < 0f) && this.ElementID != SimHashes.Regolith)
		{
			DebugUtil.DevLogErrorFormat(base.gameObject, "{0} is getting an abnormal mass set {1}.", new object[]
			{
				base.gameObject.name,
				mass
			});
		}
		mass = Mathf.Clamp(mass, 0f, PrimaryElement.MAX_MASS);
		this.Units = mass / this.MassPerUnit;
		if (this.Units <= 0f && !this.KeepZeroMassObject)
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x06007B15 RID: 31509 RVA: 0x0032859C File Offset: 0x0032679C
	private void SetTemperature(float temperature)
	{
		if (float.IsNaN(temperature) || float.IsInfinity(temperature))
		{
			DebugUtil.LogErrorArgs(base.gameObject, new object[]
			{
				"Invalid temperature [" + temperature.ToString() + "]"
			});
			return;
		}
		if (temperature <= 0f)
		{
			KCrashReporter.Assert(false, "Tried to set PrimaryElement.Temperature to a value <= 0", null);
		}
		this.setTemperatureCallback(this, temperature);
	}

	// Token: 0x06007B16 RID: 31510 RVA: 0x000F5894 File Offset: 0x000F3A94
	public void SetMassTemperature(float mass, float temperature)
	{
		this.SetMass(mass);
		this.SetTemperature(temperature);
	}

	// Token: 0x170007AE RID: 1966
	// (get) Token: 0x06007B17 RID: 31511 RVA: 0x000F58A4 File Offset: 0x000F3AA4
	public Element Element
	{
		get
		{
			if (this._Element == null)
			{
				this._Element = ElementLoader.FindElementByHash(this.ElementID);
			}
			return this._Element;
		}
	}

	// Token: 0x170007AF RID: 1967
	// (get) Token: 0x06007B18 RID: 31512 RVA: 0x00328608 File Offset: 0x00326808
	public byte DiseaseIdx
	{
		get
		{
			if (this.diseaseRedirectTarget)
			{
				return this.diseaseRedirectTarget.DiseaseIdx;
			}
			byte result = byte.MaxValue;
			if (this.useSimDiseaseInfo)
			{
				int i = Grid.PosToCell(base.transform.GetPosition());
				result = Grid.DiseaseIdx[i];
			}
			else if (this.diseaseHandle.IsValid())
			{
				result = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle).diseaseIdx;
			}
			return result;
		}
	}

	// Token: 0x170007B0 RID: 1968
	// (get) Token: 0x06007B19 RID: 31513 RVA: 0x00328680 File Offset: 0x00326880
	public int DiseaseCount
	{
		get
		{
			if (this.diseaseRedirectTarget)
			{
				return this.diseaseRedirectTarget.DiseaseCount;
			}
			int result = 0;
			if (this.useSimDiseaseInfo)
			{
				int i = Grid.PosToCell(base.transform.GetPosition());
				result = Grid.DiseaseCount[i];
			}
			else if (this.diseaseHandle.IsValid())
			{
				result = GameComps.DiseaseContainers.GetHeader(this.diseaseHandle).diseaseCount;
			}
			return result;
		}
	}

	// Token: 0x06007B1A RID: 31514 RVA: 0x000F58C5 File Offset: 0x000F3AC5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameComps.InfraredVisualizers.Add(base.gameObject);
		base.Subscribe<PrimaryElement>(1335436905, PrimaryElement.OnSplitFromChunkDelegate);
		base.Subscribe<PrimaryElement>(-2064133523, PrimaryElement.OnAbsorbDelegate);
	}

	// Token: 0x06007B1B RID: 31515 RVA: 0x003286F4 File Offset: 0x003268F4
	protected override void OnSpawn()
	{
		Attributes attributes = this.GetAttributes();
		if (attributes != null)
		{
			foreach (AttributeModifier modifier in this.Element.attributeModifiers)
			{
				attributes.Add(modifier);
			}
		}
	}

	// Token: 0x06007B1C RID: 31516 RVA: 0x00328758 File Offset: 0x00326958
	public void ForcePermanentDiseaseContainer(bool force_on)
	{
		if (force_on)
		{
			if (!this.diseaseHandle.IsValid())
			{
				this.diseaseHandle = GameComps.DiseaseContainers.Add(base.gameObject, byte.MaxValue, 0);
			}
		}
		else if (this.diseaseHandle.IsValid() && this.DiseaseIdx == 255)
		{
			GameComps.DiseaseContainers.Remove(base.gameObject);
			this.diseaseHandle.Clear();
		}
		this.forcePermanentDiseaseContainer = force_on;
	}

	// Token: 0x06007B1D RID: 31517 RVA: 0x000F5900 File Offset: 0x000F3B00
	protected override void OnCleanUp()
	{
		GameComps.InfraredVisualizers.Remove(base.gameObject);
		if (this.diseaseHandle.IsValid())
		{
			GameComps.DiseaseContainers.Remove(base.gameObject);
			this.diseaseHandle.Clear();
		}
		base.OnCleanUp();
	}

	// Token: 0x06007B1E RID: 31518 RVA: 0x000F5940 File Offset: 0x000F3B40
	public void SetElement(SimHashes element_id, bool addTags = true)
	{
		this.ElementID = element_id;
		if (addTags)
		{
			this.UpdateTags();
		}
	}

	// Token: 0x06007B1F RID: 31519 RVA: 0x003287D0 File Offset: 0x003269D0
	public void UpdateTags()
	{
		if (this.ElementID == (SimHashes)0)
		{
			global::Debug.Log("UpdateTags() Primary element 0", base.gameObject);
			return;
		}
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (component != null)
		{
			List<Tag> list = new List<Tag>();
			foreach (Tag item in this.Element.oreTags)
			{
				list.Add(item);
			}
			if (component.HasAnyTags(PrimaryElement.metalTags))
			{
				list.Add(GameTags.StoredMetal);
			}
			foreach (Tag tag in list)
			{
				component.AddTag(tag, false);
			}
		}
	}

	// Token: 0x06007B20 RID: 31520 RVA: 0x00328894 File Offset: 0x00326A94
	public void ModifyDiseaseCount(int delta, string reason)
	{
		if (this.diseaseRedirectTarget)
		{
			this.diseaseRedirectTarget.ModifyDiseaseCount(delta, reason);
			return;
		}
		if (this.useSimDiseaseInfo)
		{
			SimMessages.ModifyDiseaseOnCell(Grid.PosToCell(this), byte.MaxValue, delta);
			return;
		}
		if (delta != 0 && this.diseaseHandle.IsValid() && GameComps.DiseaseContainers.ModifyDiseaseCount(this.diseaseHandle, delta) <= 0 && !this.forcePermanentDiseaseContainer)
		{
			base.Trigger(-1689370368, false);
			GameComps.DiseaseContainers.Remove(base.gameObject);
			this.diseaseHandle.Clear();
		}
	}

	// Token: 0x06007B21 RID: 31521 RVA: 0x00328930 File Offset: 0x00326B30
	public void AddDisease(byte disease_idx, int delta, string reason)
	{
		if (delta == 0)
		{
			return;
		}
		if (this.diseaseRedirectTarget)
		{
			this.diseaseRedirectTarget.AddDisease(disease_idx, delta, reason);
			return;
		}
		if (this.useSimDiseaseInfo)
		{
			SimMessages.ModifyDiseaseOnCell(Grid.PosToCell(this), disease_idx, delta);
			return;
		}
		if (this.diseaseHandle.IsValid())
		{
			if (GameComps.DiseaseContainers.AddDisease(this.diseaseHandle, disease_idx, delta) <= 0)
			{
				GameComps.DiseaseContainers.Remove(base.gameObject);
				this.diseaseHandle.Clear();
				return;
			}
		}
		else if (delta > 0)
		{
			this.diseaseHandle = GameComps.DiseaseContainers.Add(base.gameObject, disease_idx, delta);
			base.Trigger(-1689370368, true);
			base.Trigger(-283306403, null);
		}
	}

	// Token: 0x06007B22 RID: 31522 RVA: 0x000F584F File Offset: 0x000F3A4F
	private static float OnGetTemperature(PrimaryElement primary_element)
	{
		return primary_element._Temperature;
	}

	// Token: 0x06007B23 RID: 31523 RVA: 0x003289EC File Offset: 0x00326BEC
	private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		global::Debug.Assert(!float.IsNaN(temperature));
		if (temperature <= 0f)
		{
			DebugUtil.LogErrorArgs(primary_element.gameObject, new object[]
			{
				primary_element.gameObject.name + " has a temperature of zero which has always been an error in my experience."
			});
		}
		primary_element._Temperature = temperature;
	}

	// Token: 0x06007B24 RID: 31524 RVA: 0x00328A40 File Offset: 0x00326C40
	private void OnSplitFromChunk(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable == null)
		{
			return;
		}
		float percent = this.Units / (this.Units + pickupable.PrimaryElement.Units);
		SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(pickupable.PrimaryElement, percent);
		this.AddDisease(percentOfDisease.idx, percentOfDisease.count, "PrimaryElement.SplitFromChunk");
		pickupable.PrimaryElement.ModifyDiseaseCount(-percentOfDisease.count, "PrimaryElement.SplitFromChunk");
	}

	// Token: 0x06007B25 RID: 31525 RVA: 0x00328AB4 File Offset: 0x00326CB4
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable == null)
		{
			return;
		}
		this.AddDisease(pickupable.PrimaryElement.DiseaseIdx, pickupable.PrimaryElement.DiseaseCount, "PrimaryElement.OnAbsorb");
	}

	// Token: 0x06007B26 RID: 31526 RVA: 0x00328AF4 File Offset: 0x00326CF4
	private void SetDiseaseVisualProvider(GameObject visualizer)
	{
		HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(base.gameObject);
		if (handle != HandleVector<int>.InvalidHandle)
		{
			DiseaseContainer payload = GameComps.DiseaseContainers.GetPayload(handle);
			payload.visualDiseaseProvider = visualizer;
			GameComps.DiseaseContainers.SetPayload(handle, ref payload);
		}
	}

	// Token: 0x06007B27 RID: 31527 RVA: 0x000F5952 File Offset: 0x000F3B52
	public void RedirectDisease(GameObject target)
	{
		this.SetDiseaseVisualProvider(target);
		this.diseaseRedirectTarget = (target ? target.GetComponent<PrimaryElement>() : null);
		global::Debug.Assert(this.diseaseRedirectTarget != this, "Disease redirect target set to myself");
	}

	// Token: 0x04005CA2 RID: 23714
	public static float MAX_MASS = 100000f;

	// Token: 0x04005CA3 RID: 23715
	public SimTemperatureTransfer sttOptimizationHook;

	// Token: 0x04005CA4 RID: 23716
	public PrimaryElement.GetTemperatureCallback getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(PrimaryElement.OnGetTemperature);

	// Token: 0x04005CA5 RID: 23717
	public PrimaryElement.SetTemperatureCallback setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(PrimaryElement.OnSetTemperature);

	// Token: 0x04005CA6 RID: 23718
	private PrimaryElement diseaseRedirectTarget;

	// Token: 0x04005CA7 RID: 23719
	private bool useSimDiseaseInfo;

	// Token: 0x04005CA8 RID: 23720
	public const float DefaultChunkMass = 400f;

	// Token: 0x04005CA9 RID: 23721
	private static readonly Tag[] metalTags = new Tag[]
	{
		GameTags.Metal,
		GameTags.RefinedMetal
	};

	// Token: 0x04005CAA RID: 23722
	[Serialize]
	[HashedEnum]
	public SimHashes ElementID;

	// Token: 0x04005CAB RID: 23723
	private float _units = 1f;

	// Token: 0x04005CAC RID: 23724
	[Serialize]
	[SerializeField]
	private float _Temperature;

	// Token: 0x04005CAD RID: 23725
	[Serialize]
	[NonSerialized]
	public bool KeepZeroMassObject;

	// Token: 0x04005CAE RID: 23726
	[Serialize]
	private HashedString diseaseID;

	// Token: 0x04005CAF RID: 23727
	[Serialize]
	private int diseaseCount;

	// Token: 0x04005CB0 RID: 23728
	private HandleVector<int>.Handle diseaseHandle = HandleVector<int>.InvalidHandle;

	// Token: 0x04005CB1 RID: 23729
	public float MassPerUnit = 1f;

	// Token: 0x04005CB2 RID: 23730
	[NonSerialized]
	private Element _Element;

	// Token: 0x04005CB3 RID: 23731
	[NonSerialized]
	public Action<PrimaryElement> onDataChanged;

	// Token: 0x04005CB4 RID: 23732
	[NonSerialized]
	private bool forcePermanentDiseaseContainer;

	// Token: 0x04005CB5 RID: 23733
	private static readonly EventSystem.IntraObjectHandler<PrimaryElement> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<PrimaryElement>(delegate(PrimaryElement component, object data)
	{
		component.OnSplitFromChunk(data);
	});

	// Token: 0x04005CB6 RID: 23734
	private static readonly EventSystem.IntraObjectHandler<PrimaryElement> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<PrimaryElement>(delegate(PrimaryElement component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x0200175F RID: 5983
	// (Invoke) Token: 0x06007B2B RID: 31531
	public delegate float GetTemperatureCallback(PrimaryElement primary_element);

	// Token: 0x02001760 RID: 5984
	// (Invoke) Token: 0x06007B2F RID: 31535
	public delegate void SetTemperatureCallback(PrimaryElement primary_element, float temperature);
}
