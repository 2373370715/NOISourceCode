using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000C6A RID: 3178
[AddComponentMenu("KMonoBehaviour/scripts/MinionAssignablesProxy")]
public class MinionAssignablesProxy : KMonoBehaviour, IAssignableIdentity
{
	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06003C4B RID: 15435 RVA: 0x000CB629 File Offset: 0x000C9829
	// (set) Token: 0x06003C4C RID: 15436 RVA: 0x000CB631 File Offset: 0x000C9831
	public IAssignableIdentity target { get; private set; }

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06003C4D RID: 15437 RVA: 0x000CB63A File Offset: 0x000C983A
	public bool IsConfigured
	{
		get
		{
			return this.slotsConfigured;
		}
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06003C4E RID: 15438 RVA: 0x000CB642 File Offset: 0x000C9842
	public int TargetInstanceID
	{
		get
		{
			return this.target_instance_id;
		}
	}

	// Token: 0x06003C4F RID: 15439 RVA: 0x0023B850 File Offset: 0x00239A50
	public GameObject GetTargetGameObject()
	{
		if (this.target == null && this.target_instance_id != -1)
		{
			this.RestoreTargetFromInstanceID();
		}
		KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)this.target;
		if (kmonoBehaviour != null)
		{
			return kmonoBehaviour.gameObject;
		}
		return null;
	}

	// Token: 0x06003C50 RID: 15440 RVA: 0x0023B894 File Offset: 0x00239A94
	public float GetArrivalTime()
	{
		if (this.GetTargetGameObject().GetComponent<MinionIdentity>() != null)
		{
			return this.GetTargetGameObject().GetComponent<MinionIdentity>().arrivalTime;
		}
		if (this.GetTargetGameObject().GetComponent<StoredMinionIdentity>() != null)
		{
			return this.GetTargetGameObject().GetComponent<StoredMinionIdentity>().arrivalTime;
		}
		global::Debug.LogError("Could not get minion arrival time");
		return -1f;
	}

	// Token: 0x06003C51 RID: 15441 RVA: 0x0023B8F8 File Offset: 0x00239AF8
	public int GetTotalSkillpoints()
	{
		if (this.GetTargetGameObject().GetComponent<MinionIdentity>() != null)
		{
			return this.GetTargetGameObject().GetComponent<MinionResume>().TotalSkillPointsGained;
		}
		if (this.GetTargetGameObject().GetComponent<StoredMinionIdentity>() != null)
		{
			return MinionResume.CalculateTotalSkillPointsGained(this.GetTargetGameObject().GetComponent<StoredMinionIdentity>().TotalExperienceGained);
		}
		global::Debug.LogError("Could not get minion skill points time");
		return -1;
	}

	// Token: 0x06003C52 RID: 15442 RVA: 0x0023B960 File Offset: 0x00239B60
	public Tag GetMinionModel()
	{
		MinionIdentity component = this.GetTargetGameObject().GetComponent<MinionIdentity>();
		if (component != null)
		{
			return component.model;
		}
		StoredMinionIdentity component2 = this.GetTargetGameObject().GetComponent<StoredMinionIdentity>();
		if (component2 != null)
		{
			return component2.model;
		}
		global::Debug.LogError("Could not get minion model");
		return Tag.Invalid;
	}

	// Token: 0x06003C53 RID: 15443 RVA: 0x0023B9B4 File Offset: 0x00239BB4
	public void SetTarget(IAssignableIdentity target, GameObject targetGO)
	{
		global::Debug.Assert(target != null, "target was null");
		if (targetGO == null)
		{
			global::Debug.LogWarningFormat("{0} MinionAssignablesProxy.SetTarget {1}, {2}, {3}. DESTROYING", new object[]
			{
				base.GetInstanceID(),
				this.target_instance_id,
				target,
				targetGO
			});
			Util.KDestroyGameObject(base.gameObject);
		}
		this.target = target;
		this.target_instance_id = targetGO.GetComponent<KPrefabID>().InstanceID;
		base.gameObject.name = "Minion Assignables Proxy : " + targetGO.name;
	}

	// Token: 0x06003C54 RID: 15444 RVA: 0x0023BA4C File Offset: 0x00239C4C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ownables = new List<Ownables>
		{
			base.gameObject.AddOrGet<Ownables>()
		};
		Components.MinionAssignablesProxy.Add(this);
		base.Subscribe<MinionAssignablesProxy>(1502190696, MinionAssignablesProxy.OnQueueDestroyObjectDelegate);
		this.ConfigureAssignableSlots();
	}

	// Token: 0x06003C55 RID: 15445 RVA: 0x000AA038 File Offset: 0x000A8238
	[OnDeserialized]
	private void OnDeserialized()
	{
	}

	// Token: 0x06003C56 RID: 15446 RVA: 0x0023BAA0 File Offset: 0x00239CA0
	public void ConfigureAssignableSlots()
	{
		if (this.slotsConfigured)
		{
			return;
		}
		Ownables component = base.GetComponent<Ownables>();
		Equipment component2 = base.GetComponent<Equipment>();
		if (component2 != null)
		{
			foreach (AssignableSlot assignableSlot in Db.Get().AssignableSlots.resources)
			{
				if (assignableSlot is OwnableSlot)
				{
					OwnableSlotInstance slot_instance = new OwnableSlotInstance(component, (OwnableSlot)assignableSlot);
					component.Add(slot_instance);
				}
				else if (assignableSlot is EquipmentSlot)
				{
					EquipmentSlotInstance slot_instance2 = new EquipmentSlotInstance(component2, (EquipmentSlot)assignableSlot);
					component2.Add(slot_instance2);
				}
			}
			BionicUpgradesMonitor.CreateAssignableSlots(this);
		}
		this.slotsConfigured = true;
	}

	// Token: 0x06003C57 RID: 15447 RVA: 0x0023BB60 File Offset: 0x00239D60
	public void RestoreTargetFromInstanceID()
	{
		if (this.target_instance_id != -1 && this.target == null)
		{
			KPrefabID instance = KPrefabIDTracker.Get().GetInstance(this.target_instance_id);
			if (instance)
			{
				IAssignableIdentity component = instance.GetComponent<IAssignableIdentity>();
				if (component != null)
				{
					this.SetTarget(component, instance.gameObject);
					return;
				}
				global::Debug.LogWarningFormat("RestoreTargetFromInstanceID target ID {0} was found but it wasn't an IAssignableIdentity, destroying proxy object.", new object[]
				{
					this.target_instance_id
				});
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			else
			{
				global::Debug.LogWarningFormat("RestoreTargetFromInstanceID target ID {0} was not found, destroying proxy object.", new object[]
				{
					this.target_instance_id
				});
				Util.KDestroyGameObject(base.gameObject);
			}
		}
	}

	// Token: 0x06003C58 RID: 15448 RVA: 0x000CB64A File Offset: 0x000C984A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreTargetFromInstanceID();
		if (this.target != null)
		{
			base.Subscribe<MinionAssignablesProxy>(-1585839766, MinionAssignablesProxy.OnAssignablesChangedDelegate);
			Game.Instance.assignmentManager.AddToAssignmentGroup("public", this);
		}
	}

	// Token: 0x06003C59 RID: 15449 RVA: 0x000CB686 File Offset: 0x000C9886
	private void OnQueueDestroyObject(object data)
	{
		Components.MinionAssignablesProxy.Remove(this);
	}

	// Token: 0x06003C5A RID: 15450 RVA: 0x000CB693 File Offset: 0x000C9893
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.assignmentManager.RemoveFromAllGroups(this);
		base.GetComponent<Ownables>().UnassignAll();
		base.GetComponent<Equipment>().UnequipAll();
	}

	// Token: 0x06003C5B RID: 15451 RVA: 0x000CB6C1 File Offset: 0x000C98C1
	private void OnAssignablesChanged(object data)
	{
		if (!this.target.IsNull())
		{
			((KMonoBehaviour)this.target).Trigger(-1585839766, data);
		}
	}

	// Token: 0x06003C5C RID: 15452 RVA: 0x0023BC08 File Offset: 0x00239E08
	private void CheckTarget()
	{
		if (this.target == null)
		{
			KPrefabID instance = KPrefabIDTracker.Get().GetInstance(this.target_instance_id);
			if (instance != null)
			{
				this.target = instance.GetComponent<IAssignableIdentity>();
				if (this.target != null)
				{
					MinionIdentity minionIdentity = this.target as MinionIdentity;
					if (minionIdentity)
					{
						minionIdentity.ValidateProxy();
						return;
					}
					StoredMinionIdentity storedMinionIdentity = this.target as StoredMinionIdentity;
					if (storedMinionIdentity)
					{
						storedMinionIdentity.ValidateProxy();
					}
				}
			}
		}
	}

	// Token: 0x06003C5D RID: 15453 RVA: 0x000CB6E6 File Offset: 0x000C98E6
	public List<Ownables> GetOwners()
	{
		this.CheckTarget();
		return this.target.GetOwners();
	}

	// Token: 0x06003C5E RID: 15454 RVA: 0x000CB6F9 File Offset: 0x000C98F9
	public string GetProperName()
	{
		this.CheckTarget();
		return this.target.GetProperName();
	}

	// Token: 0x06003C5F RID: 15455 RVA: 0x000CB70C File Offset: 0x000C990C
	public Ownables GetSoleOwner()
	{
		this.CheckTarget();
		return this.target.GetSoleOwner();
	}

	// Token: 0x06003C60 RID: 15456 RVA: 0x000CB71F File Offset: 0x000C991F
	public bool HasOwner(Assignables owner)
	{
		this.CheckTarget();
		return this.target.HasOwner(owner);
	}

	// Token: 0x06003C61 RID: 15457 RVA: 0x000CB733 File Offset: 0x000C9933
	public int NumOwners()
	{
		this.CheckTarget();
		return this.target.NumOwners();
	}

	// Token: 0x06003C62 RID: 15458 RVA: 0x000CB746 File Offset: 0x000C9946
	public bool IsNull()
	{
		this.CheckTarget();
		return this.target.IsNull();
	}

	// Token: 0x06003C63 RID: 15459 RVA: 0x0023BC80 File Offset: 0x00239E80
	public static Ref<MinionAssignablesProxy> InitAssignableProxy(Ref<MinionAssignablesProxy> assignableProxyRef, IAssignableIdentity source)
	{
		if (assignableProxyRef == null)
		{
			assignableProxyRef = new Ref<MinionAssignablesProxy>();
		}
		GameObject gameObject = ((KMonoBehaviour)source).gameObject;
		MinionAssignablesProxy minionAssignablesProxy = assignableProxyRef.Get();
		if (minionAssignablesProxy == null)
		{
			GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab(MinionAssignablesProxyConfig.ID), Grid.SceneLayer.NoLayer, null, 0);
			minionAssignablesProxy = gameObject2.GetComponent<MinionAssignablesProxy>();
			minionAssignablesProxy.SetTarget(source, gameObject);
			gameObject2.SetActive(true);
			assignableProxyRef.Set(minionAssignablesProxy);
		}
		else
		{
			minionAssignablesProxy.SetTarget(source, gameObject);
		}
		return assignableProxyRef;
	}

	// Token: 0x040029E8 RID: 10728
	public List<Ownables> ownables;

	// Token: 0x040029EA RID: 10730
	[Serialize]
	private int target_instance_id = -1;

	// Token: 0x040029EB RID: 10731
	private bool slotsConfigured;

	// Token: 0x040029EC RID: 10732
	private static readonly EventSystem.IntraObjectHandler<MinionAssignablesProxy> OnAssignablesChangedDelegate = new EventSystem.IntraObjectHandler<MinionAssignablesProxy>(delegate(MinionAssignablesProxy component, object data)
	{
		component.OnAssignablesChanged(data);
	});

	// Token: 0x040029ED RID: 10733
	private static readonly EventSystem.IntraObjectHandler<MinionAssignablesProxy> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<MinionAssignablesProxy>(delegate(MinionAssignablesProxy component, object data)
	{
		component.OnQueueDestroyObject(data);
	});
}
